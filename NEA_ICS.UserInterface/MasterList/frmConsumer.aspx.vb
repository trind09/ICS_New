Imports Microsoft.Reporting.WebForms
Imports System.Web.Services.Description
Imports NEA_ICS.UserInterface.NEA_ICS.WcfService
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ServiceModel
Imports System.Reflection

Partial Public Class frmConsumer
    Inherits clsCommonFunction

    'Private Shared ConsumerList As New List(Of ConsumerDetails)
    Private Message As String = EMPTY
    Public Property NoRecordFond() As String
        Get
            If ViewState("NoRecordFond") Is Nothing Then
                Return "N"
            Else
                Return ViewState("NoRecordFond").ToString()
            End If
        End Get
        Set(ByVal value As String)
            ViewState("NoRecordFond") = value
        End Set
    End Property

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ' alert user with message (if any)
        If Message <> EMPTY Then
            ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", "ShowAlertMessage('" & Message & "');", True)
        End If
    End Sub

#Region " PAGE LOAD "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim AccessRights As New List(Of RoleDetails)

        CheckValidSession()

        If Not Page.IsPostBack Then

            AccessRights = clsCommonFunction.AssignAccessRights(Cache(Session(ESession.UserID.ToString) & Session(ESession.StoreID.ToString) & "AccessRights"), _
                                                               clsCommonFunction.moduleID.Consumer)

            If AccessRights(0).SelectRight = False Then
                Server.Transfer("..\frmUnauthorisedPage.aspx")
                Exit Sub
            End If

            If AccessRights(0).InsertRight = False Then
                tbpNewConsumer.Visible = False
            End If

            If AccessRights(0).UpdateRight = False Then
                lbtnLocateEdit.Visible = False
                ViewState("_EDIT") = False
            Else
                ViewState("_EDIT") = True
            End If

            If AccessRights(0).DeleteRight = False Then
                lbtnLocateDel.Visible = False
            End If

            tbcConsumer.Visible = True

            BindAddUsers()

        End If
    End Sub
#End Region

#Region " New Tab "
    ''' <summary>
    ''' btnAddConsumer - Click;
    ''' 29 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAddConsumer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddConsumer.Click

        If txtAddConsumer.Text.Trim = String.Empty Or txtAddDescription.Text.Trim = String.Empty Then

            lblErrAddConsumer.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.MandatoryField)

            Dim Script As String = "ShowAlertMessage('" & lblErrAddConsumer.Text & "');"
            ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

            lblErrAddConsumer.Visible = True

        Else

            Try

                Dim Client As New ServiceClient
                Dim ConsumerDetails As New ConsumerDetails
                Dim UserList As New List(Of String)

                For Each item As ListItem In lstAddUsers.Items
                    UserList.Add(item.Value)
                Next

                ConsumerDetails.StoreID = Session("StoreID")
                ConsumerDetails.ConsumerID = txtAddConsumer.Text.Trim.ToUpper
                ConsumerDetails.ConsumerDescription = txtAddDescription.Text.Trim
                ConsumerDetails.LoginUser = Session("UserID")
                ConsumerDetails.ConsumerStatus = "O"

                lblErrAddConsumer.Text = Client.AddConsumer(ConsumerDetails, UserList)
                Client.Close()

                If lblErrAddConsumer.Text.Trim = String.Empty Then

                    lblErrAddConsumer.Visible = False

                    lblMsgConsumerCode.Text = txtAddConsumer.Text.Trim
                    lblMsgDescription.Text = txtAddDescription.Text.Trim

                    For Each item As ListItem In lstAddUsers.Items
                        lblMsgUserList.Text &= item.Text & " <br>"
                    Next


                    '-- UPDATE CACHE
                    If Not Cache(Session(ESession.StoreID.ToString) & ECache.ConsumerList) Is Nothing Then
                        clsCommonFunction.EditCache(Cache(Session(ESession.StoreID.ToString) & ECache.ConsumerList), GetType(ConsumerDetails), ConsumerDetails)
                    End If

                    divMsgBox.Visible = True
                    pnlAddConsumer.Visible = False

                Else

                    Dim Script As String = "ShowAlertMessage('" & lblErrAddConsumer.Text & "');"
                    ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

                    lblErrAddConsumer.Visible = True

                End If

            Catch ex As FaultException

                lblErrAddConsumer.Text = ex.Message
                lblErrAddConsumer.Visible = True

            Catch ex As Exception
                Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
                If (rethrow) Then
                    Throw
                End If
            End Try

        End If

    End Sub

    ''' <summary>
    ''' btnClearConsumer - Click;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnClearConsumer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearConsumer.Click

        txtAddConsumer.Text = String.Empty
        txtAddDescription.Text = String.Empty
        lblErrAddConsumer.Visible = False

        lstAddUsers.Items.Clear()
        BindAddUsers()

    End Sub

    ''' <summary>
    ''' btnAddUser - Click;
    ''' 29 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAddUser_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddUser.Click

        On Error Resume Next

        lstAddUsers.Items.Add(New ListItem(lstUserLists.SelectedItem.Text, lstUserLists.SelectedValue))
        lstUserLists.Items.Remove(lstUserLists.SelectedItem)

    End Sub

    ''' <summary>
    ''' btnRemoveUser - Click;
    ''' 29 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnRemoveUser_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemoveUser.Click

        On Error Resume Next

        lstUserLists.Items.Add(New ListItem(lstAddUsers.SelectedItem.Text, lstAddUsers.SelectedValue))
        lstAddUsers.Items.Remove(lstAddUsers.SelectedItem)

    End Sub

    ''' <summary>
    ''' btnAddConsumerOK - Click;
    ''' 29 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAddConsumerOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddConsumerOK.Click

        pnlAddConsumer.Visible = True
        divMsgBox.Visible = False
        lblErrAddConsumer.Visible = False

    End Sub
#End Region

#Region " Locate Tab "
    ''' <summary>
    ''' btnLocateGo - Click;
    ''' 21 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnLocateGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLocateGo.Click

        Try

            Dim Client As New ServiceClient
            Dim ConsumerDetails As New ConsumerDetails

            ConsumerDetails.StoreID = Session("StoreID")
            ConsumerDetails.ConsumerID = txtLocateConsumer.Text.Trim
            ConsumerDetails.ConsumerDescription = txtLocateDescription.Text.Trim
            ConsumerDetails.ConsumerStatus = ddlLocateStatus.SelectedValue

            Cache(Session(ESession.StoreID.ToString) & "ConsumerList") = Client.GetConsumers(ConsumerDetails, String.Empty, String.Empty)
            Client.Close()

            gdvLocateConsumer.DataSource = Cache(Session(ESession.StoreID.ToString) & "ConsumerList")
            gdvLocateConsumer.DataBind()


            pnlLocateConsumer.Visible = True
            pnlEditConsumer.Visible = False

        Catch ex As FaultException

            lblErrLocateConsumer.Text = ex.Message
            lblErrLocateConsumer.Visible = True

        Catch ex As Exception
            lblErrLocateConsumer.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            lblErrLocateConsumer.Visible = True
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If

        End Try

    End Sub


    ''' <summary>
    ''' btnLocateClear - Click;
    ''' 29 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnLocateClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLocateClear.Click

        txtLocateConsumer.Text = String.Empty
        txtLocateDescription.Text = String.Empty

        ddlLocateStatus.SelectedValue = OPEN

        pnlEditConsumer.Visible = False
        pnlLocateConsumer.Visible = False

    End Sub

    ''' <summary>
    ''' gdvLocateConsumer - PageIndexChanging;
    ''' 29 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gdvLocateConsumer_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gdvLocateConsumer.PageIndexChanging

        gdvLocateConsumer.PageIndex = e.NewPageIndex
        gdvLocateConsumer.DataSource = Cache(Session(ESession.StoreID.ToString) & "ConsumerList")
        gdvLocateConsumer.DataBind()

    End Sub

    ''' <summary>
    ''' gdvLocateConsumer - RowDataBound;
    ''' 29 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gdvLocateConsumer_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvLocateConsumer.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            Select Case CType(e.Row.FindControl("hidStatus"), HiddenField).Value.ToUpper
                Case "O"
                    CType(e.Row.FindControl("lblStatus"), Label).Text = "Open"
                Case "C"
                    CType(e.Row.FindControl("lblStatus"), Label).Text = "Closed"
            End Select

        End If

    End Sub

    ''' <summary>
    ''' gdvLocateConsumer - SelectedIndexChanged;
    ''' 29 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gdvLocateConsumer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvLocateConsumer.SelectedIndexChanged

        lblEditConsumerCode.Text = Replace(gdvLocateConsumer.SelectedRow.Cells(1).Text, "&amp;", "&")
        txtEditDescription.Text = gdvLocateConsumer.SelectedRow.Cells(2).Text
        lblEditStatus.Text = CType(gdvLocateConsumer.SelectedRow.FindControl("lblStatus"), Label).Text
        hidEditStatus.Value = CType(gdvLocateConsumer.SelectedRow.FindControl("hidStatus"), HiddenField).Value.ToUpper
        lblSelectedUsers.Text = " (" & CType(gdvLocateConsumer.SelectedRow.FindControl("lblStatus"), Label).Text & ") "

        If ViewState("_EDIT") Then
            Select Case CType(gdvLocateConsumer.SelectedRow.FindControl("hidStatus"), HiddenField).Value.ToUpper

                Case "O"
                    lbtnLocateReopen.Visible = False
                    lbtnLocateClose.Visible = True

                Case "C"
                    lbtnLocateReopen.Visible = True
                    lbtnLocateClose.Visible = False

                Case Else
                    lbtnLocateReopen.Visible = False
                    lbtnLocateClose.Visible = False

            End Select
        Else
            lbtnLocateClose.Visible = False
            lbtnLocateReopen.Visible = False
        End If

        txtEditDescription.Enabled = False
        lstEditUsers.Enabled = False
        lstEditUsersLists.Enabled = False
        btnAddEditUsers.Enabled = False
        btnRemoveEditUsers.Enabled = False

        btnEditConsumerSave.Enabled = False

        BindEditUserRef(lblEditConsumerCode.Text.Trim, hidEditStatus.Value)
        BindEditUsers()

        pnlEditConsumer.Visible = True

    End Sub

    ''' <summary>
    ''' gdvLocateConsumer - Sorting;
    ''' 29 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gdvLocateConsumer_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gdvLocateConsumer.Sorting

        If ViewState("_SortDirection") Is Nothing Then
            ViewState("_SortDirection") = "DESC"
        Else

            If ViewState("_SortDirection") = "DESC" Then
                ViewState("_SortDirection") = "ASC"
            Else
                ViewState("_SortDirection") = "DESC"
            End If

        End If

        Try

            Dim Client As New ServiceClient
            Dim ConsumerDetails As New ConsumerDetails

            ConsumerDetails.StoreID = Session("StoreID")
            ConsumerDetails.ConsumerID = txtLocateConsumer.Text.Trim
            ConsumerDetails.ConsumerDescription = txtLocateDescription.Text.Trim
            ConsumerDetails.ConsumerStatus = ddlLocateStatus.SelectedValue

            Cache(Session(ESession.StoreID.ToString) & "ConsumerList") = Client.GetConsumers(ConsumerDetails, e.SortExpression, ViewState("_SortDirection"))
            Client.Close()

            gdvLocateConsumer.DataSource = Cache(Session(ESession.StoreID.ToString) & "ConsumerList")
            gdvLocateConsumer.DataBind()


        Catch ex As FaultException

            lblErrLocateConsumer.Text = ex.Message
            lblErrLocateConsumer.Visible = True

        Catch ex As Exception
            lblErrLocateConsumer.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            lblErrLocateConsumer.Visible = True
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If

        End Try

    End Sub

    ''' <summary>
    ''' lbtnLocateEdit - Click;
    ''' 29 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub lbtnLocateEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnLocateEdit.Click

        txtEditDescription.Enabled = True
        lstEditUsers.Enabled = True
        lstEditUsersLists.Enabled = True
        btnAddEditUsers.Enabled = True
        btnRemoveEditUsers.Enabled = True

        btnEditConsumerSave.Enabled = True

    End Sub

    ''' <summary>
    ''' btnEditConsumerSave - Click;
    ''' 29 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnEditConsumerSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditConsumerSave.Click

        If txtEditDescription.Text.Trim = String.Empty Then

            lblErrSaveConsumer.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.MandatoryField)
            lblErrSaveConsumer.Visible = True

            Dim Script As String = "ShowAlertMessage('" & lblErrSaveConsumer.Text & "');"
            ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

        Else

            Try

                Dim Client As New ServiceClient
                Dim ConsumerDetails As New ConsumerDetails
                Dim UserList As New List(Of String)

                ConsumerDetails.StoreID = Session("StoreID")
                ConsumerDetails.ConsumerID = lblEditConsumerCode.Text.Trim
                ConsumerDetails.ConsumerDescription = txtEditDescription.Text.Trim
                ConsumerDetails.LoginUser = Session("UserID")
                ConsumerDetails.ConsumerStatus = hidEditStatus.Value

                For Each item As ListItem In lstEditUsers.Items
                    UserList.Add(item.Value)
                Next

                lblErrSaveConsumer.Text = Client.UpdateConsumer(ConsumerDetails, UserList)
                Client.Close()

                If lblErrSaveConsumer.Text = String.Empty Then

                    lblErrSaveConsumer.Visible = False
                    Dim Script As String = "ShowSuccessMessage('" & clsCommonFunction.GetMessage( _
                                           clsCommonFunction.messageID.Success, "saved", "Consumer") & "');"

                    ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)

                    btnLocateGo_Click(sender, e)
                    gdvLocateConsumer_SelectedIndexChanged(sender, e)

                Else

                    lblErrSaveConsumer.Visible = True

                    Dim Script As String = "ShowAlertMessage('" & lblErrSaveConsumer.Text & "');"
                    ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

                End If

            Catch ex As FaultException

                lblErrSaveConsumer.Text = ex.Message
                lblErrSaveConsumer.Visible = True

            Catch ex As Exception
                Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
                If (rethrow) Then
                    Throw
                End If
            End Try

        End If

    End Sub

    ''' <summary>
    ''' btnAddEditUsers - Click;
    ''' 29 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAddEditUsers_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddEditUsers.Click

        On Error Resume Next

        lstEditUsers.Items.Add(New ListItem(lstEditUsersLists.SelectedItem.Text, lstEditUsersLists.SelectedValue))
        lstEditUsersLists.Items.Remove(lstEditUsersLists.SelectedItem)

    End Sub

    ''' <summary>
    ''' btnRemoveEditUsers - Click;
    ''' 29 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnRemoveEditUsers_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemoveEditUsers.Click

        On Error Resume Next

        lstEditUsersLists.Items.Add(New ListItem(lstEditUsers.SelectedItem.Text, lstEditUsers.SelectedValue))
        lstEditUsers.Items.Remove(lstEditUsers.SelectedItem)

    End Sub

    ''' <summary>
    ''' btnEditConsumerCancel - Click;
    ''' 29 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnEditConsumerCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditConsumerCancel.Click

        pnlEditConsumer.Visible = False
        pnlLocateConsumer.Visible = True

    End Sub

    ''' <summary>
    ''' lbtnLocateClose - Click;
    ''' 29 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub lbtnLocateClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnLocateClose.Click

        Try

            '----------------------------------------------------------------------------------
            '-- TO FIND IF THERE ARE ANY PENDING ITEM REQUESTS
            '----------------------------------------------------------------------------------
            If CacheListIsEmpty(Cache(Session(ESession.StoreID.ToString) & ECache.RequestList), GetType(RequestDetails)) Then
                GetRequestList(Cache(Session(ESession.StoreID.ToString) & ECache.RequestList), Session("StoreID").ToString, ALL)
            End If

            If Not CacheListIsEmpty(Cache(Session(ESession.StoreID.ToString) & ECache.RequestList), GetType(RequestDetails)) Then

                If DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.RequestList), List(Of RequestDetails)).Exists _
                   (Function(i As RequestDetails) i.Status <> CLOSED And i.ConsumerID = lblEditConsumerCode.Text) Then

                    lblErrSaveConsumer.Text = "Unable to close Consumer Code " & lblEditConsumerCode.Text & ". \nThere are pending item requests found in this Consumer. "

                    Dim Script As String = "ShowAlertMessage('" & lblErrSaveConsumer.Text & "');"
                    ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

                    Exit Sub

                End If

            End If
            '----------------------------------------------------------------------------------

            Dim Client As New ServiceClient
            Dim ConsumerDetails As New ConsumerDetails
            Dim UserList As New List(Of String)

            ConsumerDetails.StoreID = Session("StoreID")
            ConsumerDetails.ConsumerID = lblEditConsumerCode.Text.Trim
            ConsumerDetails.ConsumerDescription = txtEditDescription.Text.Trim
            ConsumerDetails.LoginUser = Session("UserID")
            ConsumerDetails.ConsumerStatus = CLOSED

            For Each item As ListItem In lstEditUsers.Items
                UserList.Add(item.Value)
            Next

            lblErrSaveConsumer.Text = Client.UpdateConsumer(ConsumerDetails, UserList)
            Client.Close()

            If lblErrSaveConsumer.Text = String.Empty Then

                lblErrSaveConsumer.Visible = False
                Dim Script As String = "ShowSuccessMessage('" & clsCommonFunction.GetMessage( _
                                       clsCommonFunction.messageID.Success, "closed", "Consumer") & "');"

                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)

                btnLocateGo_Click(sender, e)
                gdvLocateConsumer_SelectedIndexChanged(sender, e)

            Else

                lblErrSaveConsumer.Visible = True

            End If

        Catch ex As FaultException

            lblErrSaveConsumer.Text = ex.Message
            lblErrSaveConsumer.Visible = True

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

    ''' <summary>
    ''' lbtnLocateReopen - Click;
    ''' 29 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub lbtnLocateReopen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnLocateReopen.Click

        Try

            Dim Client As New ServiceClient
            Dim ConsumerDetails As New ConsumerDetails
            Dim UserList As New List(Of String)

            ConsumerDetails.StoreID = Session("StoreID")
            ConsumerDetails.ConsumerID = lblEditConsumerCode.Text.Trim
            ConsumerDetails.ConsumerDescription = txtEditDescription.Text.Trim
            ConsumerDetails.LoginUser = Session("UserID")
            ConsumerDetails.ConsumerStatus = "O"

            For Each item As ListItem In lstEditUsers.Items
                UserList.Add(item.Value)
            Next

            lblErrSaveConsumer.Text = Client.UpdateConsumer(ConsumerDetails, UserList)
            Client.Close()

            If lblErrSaveConsumer.Text = String.Empty Then

                lblErrSaveConsumer.Visible = False
                Dim Script As String = "ShowSuccessMessage('" & clsCommonFunction.GetMessage( _
                                       clsCommonFunction.messageID.Success, "opened", "Consumer") & "');"

                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)

                btnLocateGo_Click(sender, e)
                gdvLocateConsumer_SelectedIndexChanged(sender, e)

            Else

                lblErrSaveConsumer.Visible = True

            End If

        Catch ex As FaultException

            lblErrSaveConsumer.Text = ex.Message
            lblErrSaveConsumer.Visible = True

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

    ''' <summary>
    ''' lbtnLocateDel - Click;
    ''' 29 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub lbtnLocateDel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnLocateDel.Click

        Try

            Dim Client As New ServiceClient
            Dim ConsumerDetails As New ConsumerDetails
            Dim UserList As New List(Of String)

            ConsumerDetails.StoreID = Session("StoreID")
            ConsumerDetails.ConsumerID = lblEditConsumerCode.Text.Trim
            ConsumerDetails.ConsumerDescription = txtEditDescription.Text.Trim
            ConsumerDetails.LoginUser = Session("UserID")
            ConsumerDetails.ConsumerStatus = "D"

            For Each item As ListItem In lstEditUsers.Items
                UserList.Add(item.Value)
            Next

            lblErrSaveConsumer.Text = Client.UpdateConsumer(ConsumerDetails, UserList)
            Client.Close()

            If lblErrSaveConsumer.Text = String.Empty Then

                lblErrSaveConsumer.Visible = False
                Dim Script As String = "ShowSuccessMessage('" & clsCommonFunction.GetMessage( _
                                       clsCommonFunction.messageID.Success, "deleted", "Consumer") & "');"

                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)

                btnLocateGo_Click(sender, e)
                gdvLocateConsumer_SelectedIndexChanged(sender, e)

            Else

                lblErrSaveConsumer.Visible = True

            End If

        Catch ex As FaultException

            lblErrSaveConsumer.Text = ex.Message
            lblErrSaveConsumer.Visible = True

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

#End Region

#Region " Print Tab "

#End Region

#Region " Sub Procedures and Functions "

    ''' <summary>
    ''' Sub Proc - BindUsers()
    ''' 29 Jan 09 - Jianfa;
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindAddUsers()

        Try

            Dim Client As New ServiceClient
            Dim ConsumerDetails As New ConsumerDetails
            Dim UserList As New List(Of ConsumerDetails)

            UserList.Clear()

            ConsumerDetails.StoreID = Session("StoreID")

            UserList = Client.GetUsers(ConsumerDetails)
            Client.Close()

            lstUserLists.Items.Clear()
            lstUserLists.DataSource = UserList
            lstUserLists.DataValueField = "UserID"
            lstUserLists.DataTextField = "UserName"
            lstUserLists.DataBind()


            For Each itemX As ListItem In lstAddUsers.Items

                For Each itemY As ListItem In lstUserLists.Items

                    If itemX.Value = itemY.Value Then
                        lstUserLists.Items.Remove(itemY)
                        Exit For
                    End If

                Next

            Next

        Catch ex As FaultException

            lblErrAddConsumer.Text = ex.Message
            lblErrAddConsumer.Visible = True

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

    ''' <summary>
    ''' Sub Proc - BindEditUsers;
    ''' 29 Jan 09 - Jianfa;
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindEditUsers()

        Try

            Dim Client As New ServiceClient
            Dim ConsumerDetails As New ConsumerDetails
            Dim UserList As New List(Of ConsumerDetails)

            UserList.Clear()

            ConsumerDetails.StoreID = Session("StoreID")

            UserList = Client.GetUsers(ConsumerDetails)
            Client.Close()

            lstEditUsersLists.Items.Clear()
            lstEditUsersLists.DataSource = UserList
            lstEditUsersLists.DataValueField = "UserID"
            lstEditUsersLists.DataTextField = "UserName"
            lstEditUsersLists.DataBind()


            For Each itemX As ListItem In lstEditUsers.Items

                For Each itemY As ListItem In lstEditUsersLists.Items

                    If itemX.Value = itemY.Value Then
                        lstEditUsersLists.Items.Remove(itemY)
                        Exit For
                    End If

                Next

            Next

        Catch ex As FaultException

            lblErrLocateConsumer.Text = ex.Message
            lblErrLocateConsumer.Visible = True

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

    ''' <summary>
    ''' Sub Proc - BindEditUserRef;
    ''' 29 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="consumerID"></param>
    ''' <remarks></remarks>
    Private Sub BindEditUserRef(ByVal consumerID As String, ByVal status As String)

        Try

            Dim Client As New ServiceClient
            Dim ConsumerDetails As New ConsumerDetails
            Dim UserList As New List(Of ConsumerDetails)

            UserList.Clear()

            ConsumerDetails.StoreID = Session("StoreID")
            ConsumerDetails.ConsumerID = consumerID
            ConsumerDetails.ConsumerRefStatus = status

            UserList = Client.GetUserRef(ConsumerDetails)
            Client.Close()

            lstEditUsers.Items.Clear()
            lstEditUsers.DataSource = UserList
            lstEditUsers.DataValueField = "UserID"
            lstEditUsers.DataTextField = "UserName"
            lstEditUsers.DataBind()


        Catch ex As FaultException

            lblErrLocateConsumer.Text = ex.Message
            lblErrLocateConsumer.Visible = True

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try


    End Sub

#End Region

    Protected Sub ObjectDataSource1_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs) Handles ObjectDataSource1.Selecting
        Dim searchConsumer As New ConsumerDetails()
        searchConsumer.StoreID = Session("StoreID")
        searchConsumer.ConsumerID = ""
        searchConsumer.ConsumerStatus = ""
        searchConsumer.ConsumerDescription = ""

        e.InputParameters("consumerDetails") = searchConsumer
        e.InputParameters("sortExpression") = "ConsumerID"
        e.InputParameters("sortDirection") = ""
    End Sub
    Protected Sub ObjectDataSource1_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles ObjectDataSource1.Selected
        Dim returnList As List(Of ConsumerDetails) = e.ReturnValue
        If returnList.Count <= 0 Then
            NoRecordFond = "Y"
        Else
            NoRecordFond = "N"
        End If
    End Sub

    Protected Sub btnPDF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPDF.Click
        rvr.LocalReport.DataSources.Clear()
        rvr.LocalReport.DataSources.Add(New ReportDataSource("ConsumerDetails", ObjectDataSource1))

        Dim parameterlist As New List(Of ReportParameter)
        Dim p1 As New ReportParameter("ReportTitle", "Consumer Master List Report")
        Dim p2 As New ReportParameter("PlantName", Session("StoreName").ToString())
        parameterlist.Add(p1)
        parameterlist.Add(p2)

        rvr.LocalReport.SetParameters(parameterlist)
        rvr.LocalReport.Refresh()

        Dim bytValue As Byte()
        bytValue = rvr.LocalReport.Render("PDF", Nothing, "application/pdf", "", "pdf", Nothing, Nothing)
        If NoRecordFond = "Y" Then
            Message = GetMessage(messageID.NoRecordFound)
            Exit Sub
        End If

        Response.Buffer = True
        Response.Clear()
        Response.ContentType = "application/pdf"
        Response.AddHeader("content-disposition", "attachment;filename=ConsumerMasterList.pdf")
        Response.BinaryWrite(bytValue)
        Response.Flush()
        Response.End()
    End Sub

    Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExcel.Click
        rvr.LocalReport.DataSources.Clear()
        rvr.LocalReport.DataSources.Add(New ReportDataSource("ConsumerDetails", ObjectDataSource1))

        Dim parameterlist As New List(Of ReportParameter)
        Dim p1 As New ReportParameter("ReportTitle", "Consumer Master List Report")
        Dim p2 As New ReportParameter("PlantName", Session("StoreName").ToString())
        parameterlist.Add(p1)
        parameterlist.Add(p2)

        rvr.LocalReport.SetParameters(parameterlist)
        rvr.LocalReport.Refresh()

        Dim bytValue As Byte()
        bytValue = rvr.LocalReport.Render("Excel", Nothing, "application/ms-excel", "", "xls", Nothing, Nothing)
        If NoRecordFond = "Y" Then
            Message = GetMessage(messageID.NoRecordFound)
            Exit Sub
        End If

        Response.Buffer = True
        Response.Clear()
        Response.ContentType = "application/ms-excel"
        Response.AddHeader("content-disposition", "attachment;filename=ConsumerMasterList.xls")
        Response.BinaryWrite(bytValue)
        Response.Flush()
        Response.End()
    End Sub

End Class