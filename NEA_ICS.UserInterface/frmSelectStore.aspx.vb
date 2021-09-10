Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports NEA_ICS.UserInterface.NEA_ICS.WcfService

''' <summary>
''' Class - Select Store
''' 16 Feb 09 - Jianfa
''' </summary>
''' <remarks></remarks>
Partial Public Class frmSelectStore
    Inherits clsCommonFunction

    Private Message As String = EMPTY

#Region " Access Rights "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Not Page.IsPostBack Then

            Response.Cache.SetCacheability(HttpCacheability.ServerAndNoCache)

            CheckValidSessionFromLogin()

            Dim strSessionId As String = Guid.NewGuid.ToString.Replace("-", "") ' use Guid instead bec. Session.SessionId is not unique sometimes
            Session("UserSessionID") = strSessionId.Substring(0, 23) ' truncate to 23 chars. only
            clsCommonFunction.LogError("test")

            CheckMultipleLogin(Session("UserID"), Session("UserSessionID"), False)
            'If CheckMultipleLogin(Session("UserID"), Session("UserSessionID"), False) > 0 Then
            '    Session("LoginMessage") = GetMessage(messageID.MultipleLogin)
            'End If

            Try

                Dim Client As ServiceClient
                Dim RoleDetails As New RoleDetails
                Dim StoreList As New List(Of StoreDetails)
                Dim RoleList As New List(Of RoleDetails)
                Dim TestAccount As String = EMPTY

                'If Session("UserID") Is Nothing Then
                '    '' ONLY for test account S1234567D
                '    TestAccount = ConfigurationManager.AppSettings("TestAccount").ToString()
                '    Session("UserID") = IIf(TestAccount = "S1234567D", TestAccount, EMPTY)
                'End If

                RoleDetails.UserID = Session("UserID") '****!!!


                If RoleDetails.UserID <> EMPTY Then
                    Client = New ServiceClient
                    StoreList = Client.GetStoreAccess(RoleDetails)
                    Client.Close()
                End If

                Select Case StoreList.Count
                    Case Is <= 0
                        ' if all stores of user are closed
                        Client = New ServiceClient
                        Client.AddUserAudit("", Session("UserID").ToString(), Request.UserHostAddress, "", False, True, True)
                        Client.Close()
                        Response.Redirect(ConfigurationManager.AppSettings("LoginURL").ToString() & "?err=1")
                    Case 1
                        Session("StoreID") = StoreList(0).StoreId
                        Session("StoreName") = StoreList(0).StoreName

                        'ManageInactiveUser(StoreList(0).StoreId, Session("UserID").ToString())
                        ManageInactiveUser(StoreList(0).StoreId)
                        If IsUserInactiveDeleted(StoreList(0).StoreId, Session("UserID").ToString()) Then ' check if user is inactive or deleted
                            Message = "Access is denied. Please verify your login information."
                            Response.Redirect(ConfigurationManager.AppSettings("LoginURL").ToString() & "?err=1")
                        Else
                            Client = New ServiceClient
                            Client.AddUserAudit(Session("StoreID"), Session("UserID"), Request.UserHostAddress, Session("UserSessionID"), False, False, False)
                            Client.Close()
                        End If

                        PopulateUserProfile()

                        Server.Execute("frmICS.aspx")

                    Case Is >= 2
                        ddlSelectStore.DataSource = StoreList
                        ddlSelectStore.DataValueField = "StoreID"
                        ddlSelectStore.DataTextField = "StoreName"
                        ddlSelectStore.DataBind()

                        ShowModal.Visible = True
                        ShowModal_Click(sender, e)

                End Select

            Catch ex As Exception
                clsCommonFunction.LogError(ex.InnerException.Message)
                Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
                If (rethrow) Then Throw

                '-- TO DO: To re-direct to common login page
                Response.Redirect(ConfigurationManager.AppSettings("LoginURL").ToString() & "?err=1")

            End Try

        End If

    End Sub

    Private Sub ShowModal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ShowModal.Click

        mpuSelectStore.Show()

    End Sub

    Private Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGo.Click
        Session("StoreID") = ddlSelectStore.SelectedValue
        Session("StoreName") = ddlSelectStore.SelectedItem.Text

        Try
            ManageInactiveUser(ddlSelectStore.SelectedValue)

            If IsUserInactiveDeleted(Session("StoreID"), Session("UserId")) Then ' user is inactive or deleted ics user
                Message = "Access is denied. Please verify your login information."
                Response.Redirect(ConfigurationManager.AppSettings("LoginURL").ToString() & "?err=1")
            Else
                Dim Client As ServiceClient = New ServiceClient
                Client.AddUserAudit(ddlSelectStore.SelectedValue, Session("UserID"), Request.UserHostAddress, Session("UserSessionID"), False, False, False)
                Client.Close()
            End If

            Try
                PopulateUserProfile()
            Catch ex As Exception
                clsCommonFunction.LogError(ex.Message)
            End Try

            Server.Transfer("frmICS.aspx")
        Catch ex As Exception
            clsCommonFunction.LogError(ex.InnerException.Message)
            Server.Transfer(ConfigurationManager.AppSettings("LoginURL").ToString() & "?err=1")
        End Try

    End Sub

#End Region

#Region " Sub Procedures and Functions "

    ''' <summary>
    ''' Sub Proc - PopulateUserProfile;
    ''' 16 Feb 09 - Jianfa;
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PopulateUserProfile()
        'clsCommonFunction.LogError("populate user profile start")
        Try

            Dim Client As ServiceClient
            Dim RoleDetails As New RoleDetails
            Dim UserProfileList As New List(Of RoleDetails)

            RoleDetails.StoreID = Session("StoreID")
            RoleDetails.UserID = Session("UserID")
            'RoleDetails.UserIP = Request.UserHostAddress

            Client = New ServiceClient
            UserProfileList = Client.GetUserProfile(RoleDetails)
            Client.Close()

            Select Case UserProfileList.Count

                Case Is <= 0

                    Server.Execute("frmUnauthorisedPageFromLogin.aspx")

                Case Else
                    'If UserProfileList(0).UserRole.Trim = String.Empty Or _
                    '    (UserProfileList(0).LastLogout = DateTime.MinValue And _
                    '    UserProfileList(0).LastLogin >= Now()) Then
                    If UserProfileList(0).UserRole.Trim = String.Empty Then
                        Server.Execute("frmUnauthorisedPageFromLogin.aspx")
                    Else
                        Session("UserName") = StrConv(UserProfileList(0).Name, VbStrConv.ProperCase)
                        Session("UserDesignation") = IIf(UserProfileList(0).Designation Is Nothing, "", UserProfileList(0).Designation)
                        Session("UserRoleType") = Left(UserProfileList(0).UserRole, UserProfileList(0).UserRole.Length - 1)
                        Session("UserLastLogin") = UserProfileList(0).LastLogin

                        'Client = New ServiceClient
                        ''Client.AddUserAudit(RoleDetails, Session.SessionID)
                        'Client.AddUserAudit(Session("StoreID"), Session("UserID"), Request.UserHostAddress, Session("UserSessionID"), False, False, False)
                        'Client.Close()

                    End If

            End Select

        Catch ex As Exception
            '-- TO DO: To re-direct to common login page
            'Response.Redirect(ConfigurationManager.AppSettings("LoginURL").ToString())
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try

    End Sub

    Private Function IsUserInactiveDeleted(ByVal storeId As String, ByVal userId As String) As Boolean
        Try
            Dim Client As ServiceClient
            Client = New ServiceClient
            Dim userIP As String = Request.UserHostAddress

            'Dim RoleDetails As New RoleDetails
            'RoleDetails.StoreID = storeId
            'RoleDetails.UserID = userId

            Dim UserRoleStatusList As New List(Of RoleDetails)
            UserRoleStatusList = Client.GetUserRoleStatus(storeId, userId)
            Client.Close()

            If UserRoleStatusList(0).IsUserDeleted Then 'if user is system deleted, user id used will be tag as non-ics user in the report
                Client = New ServiceClient
                Client.AddUserAudit(storeId, userId, userIP, Session("UserSessionID"), True, False, True)
                Client.Close()
                Return True
            ElseIf UserRoleStatusList(0).Status = "Inactive" Then ' inactive 
                Client = New ServiceClient
                Client.AddUserAudit(storeId, userId, userIP, Session("UserSessionID"), False, True, True)
                Client.Close()
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            clsCommonFunction.LogError("Go button is clicked")
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try

    End Function

    'Private Sub ManageInactiveUser(ByVal storeId As String, ByVal userId As String)
    Private Sub ManageInactiveUser(ByVal storeId As String)
        Try
            Dim Client As New ServiceClient
            Client.ManageInactiveUser(storeId)
            Client.Close()
        Catch ex As Exception
            'Response.Redirect(ConfigurationManager.AppSettings("LoginURL").ToString() & "?err=1")
            Response.Redirect(ConfigurationManager.AppSettings("LoginURL").ToString())
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Failed to update user status to Inactive")
            If (rethrow) Then Throw
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ' alert user with message (if any)
        If Message <> EMPTY Then
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "AlertRegister", "alert('" & Message & "');", True)
        End If
    End Sub
#End Region
End Class