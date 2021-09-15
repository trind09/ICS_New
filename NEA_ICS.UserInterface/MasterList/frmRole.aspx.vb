Imports Microsoft.Reporting.WebForms
Imports System.Web.Services.Description
Imports NEA_ICS.UserInterface.NEA_ICS.WcfService
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ServiceModel
Imports System.Reflection

Partial Public Class frmRole
    Inherits clsCommonFunction

    'Private Shared ModuleRoleList As New List(Of RoleDetails)
    'Private Shared UserRoleList As New List(Of RoleDetails)

#Region " PAGE LOAD "
    ''' <summary>
    ''' Page Load;
    ''' 14 Jan 2009 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim AccessRights As New List(Of RoleDetails)

        CheckValidSession()

        If Not Page.IsPostBack Then
            Dim UserID = ESession.UserID.ToString
            Dim UserIDSession = Session(UserID)
            Dim StoreID = ESession.StoreID.ToString
            Dim StoreIDSession = Session(ESession.StoreID.ToString)
            Dim cacheStr = UserIDSession & StoreIDSession & "AccessRights"
            Dim catAccessRights0 = Cache(cacheStr)
            Dim catAccessRights = Cache(Session(ESession.UserID.ToString) & Session(ESession.StoreID.ToString) & "AccessRights")
            Dim Role = clsCommonFunction.moduleID.Role
            AccessRights = clsCommonFunction.AssignAccessRights(Cache(Session(ESession.UserID.ToString) & Session(ESession.StoreID.ToString) & "AccessRights"),
                                                               clsCommonFunction.moduleID.Role)

            If AccessRights(0).SelectRight = False Then
                Server.Transfer("..\frmUnauthorisedPage.aspx")
                Exit Sub
            End If

            If AccessRights(0).InsertRight = False Then
                btnAddUser.Visible = False
            End If

            If AccessRights(0).UpdateRight = False Then
                gdvLocateModule.Columns(0).Visible = False
                gdvLocateUser.Columns(0).Visible = False
            End If

            If AccessRights(0).DeleteRight = False Then
                gdvLocateUser.Columns(1).Visible = False
            End If

            tbcRole.Visible = True

            BindStores()
            BindUserRoles()

        End If

    End Sub
#End Region

#Region " MODULE TAB "

    ''' <summary>
    ''' btnModuleClear - Click;
    ''' 13 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnModuleClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnModuleClear.Click

        lblErrLocateModule.Visible = False
        ddlModuleStore.SelectedIndex = -1
        ddlModuleRole.SelectedIndex = -1

        ddlModuleStore.Enabled = True
        ddlModuleRole.Enabled = True

        pnlModuleSearchResults.Visible = False

    End Sub

    ''' <summary>
    ''' btnModuleGo - Click;
    ''' 14 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnModuleGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnModuleGo.Click

        Try

            Dim Client As New ServiceClient
            Dim RoleDetails As New RoleDetails
            RoleDetails.StoreID = ddlModuleStore.SelectedValue
            RoleDetails.UserRole = ddlModuleRole.SelectedValue

            Cache(Session(ESession.StoreID.ToString) & "ModuleRoleList") = Client.GetModuleRoles(RoleDetails)
            Client.Close()

            gdvLocateModule.DataSource = Cache(Session(ESession.StoreID.ToString) & "ModuleRoleList")
            gdvLocateModule.DataBind()

            ddlModuleStore.Enabled = False
            ddlModuleRole.Enabled = False

            pnlModuleSearchResults.Visible = True


        Catch ex As FaultException

            lblErrLocateModule.Text = ex.Message
            lblErrLocateModule.Visible = True

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub

    ''' <summary>
    ''' gdvLocateModule - RowCancelingEdit;
    ''' 16 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gdvLocateModule_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gdvLocateModule.RowCancelingEdit

        lblErrSaveModule.Visible = False
        gdvLocateModule.EditIndex = -1
        gdvLocateModule.DataSource = Cache(Session(ESession.StoreID.ToString) & "ModuleRoleList")
        gdvLocateModule.DataBind()

    End Sub

    ''' <summary>
    ''' gdvLocateModule - RowEditing;
    ''' 16 Jan 2009 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gdvLocateModule_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gdvLocateModule.RowEditing

        gdvLocateModule.EditIndex = e.NewEditIndex
        gdvLocateModule.SelectedIndex = e.NewEditIndex
        gdvLocateModule.DataSource = Cache(Session(ESession.StoreID.ToString) & "ModuleRoleList")
        gdvLocateModule.DataBind()

    End Sub

    ''' <summary>
    ''' chkEditViewAccess - CheckChanged;
    ''' 16 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub chkEditViewAccess_CheckChanged(ByVal sender As Object, ByVal e As EventArgs)

        Select Case sender.checked

            Case False
                CType(gdvLocateModule.Rows(gdvLocateModule.SelectedIndex).FindControl("chkEditAddAccess"), CheckBox).Checked = False
                CType(gdvLocateModule.Rows(gdvLocateModule.SelectedIndex).FindControl("chkEditUpdateAccess"), CheckBox).Checked = False
                CType(gdvLocateModule.Rows(gdvLocateModule.SelectedIndex).FindControl("chkEditDeleteAccess"), CheckBox).Checked = False

        End Select

    End Sub

    ''' <summary>
    ''' chkEditAddAccess - CheckChanged
    ''' 16 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub chkEditAddAccess_CheckChanged(ByVal sender As Object, ByVal e As EventArgs)

        Select Case sender.checked

            Case True

                CType(gdvLocateModule.Rows(gdvLocateModule.SelectedIndex).FindControl("chkEditViewAccess"), CheckBox).Checked = True

        End Select

    End Sub

    ''' <summary>
    ''' chkEditUpdateAccess - CheckChanged
    ''' 16 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub chkEditUpdateAccess_CheckChanged(ByVal sender As Object, ByVal e As EventArgs)

        Select Case sender.checked

            Case True

                CType(gdvLocateModule.Rows(gdvLocateModule.SelectedIndex).FindControl("chkEditViewAccess"), CheckBox).Checked = True

        End Select

    End Sub

    ''' <summary>
    ''' chkEditDeleteAccess - CheckChanged
    ''' 16 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub chkEditDeleteAccess_CheckChanged(ByVal sender As Object, ByVal e As EventArgs)

        Select Case sender.checked

            Case True

                CType(gdvLocateModule.Rows(gdvLocateModule.SelectedIndex).FindControl("chkEditViewAccess"), CheckBox).Checked = True

        End Select

    End Sub

    ''' <summary>
    ''' gdvLocateModule - RowUpdating;
    ''' 16 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gdvLocateModule_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gdvLocateModule.RowUpdating

        Try

            Dim ModuleRoleStatus As String
            Dim Client As New ServiceClient
            Dim RoleDetails As New RoleDetails

            If Not CType(gdvLocateModule.Rows(e.RowIndex).FindControl("chkEditViewAccess"), CheckBox).Checked And
               Not CType(gdvLocateModule.Rows(e.RowIndex).FindControl("chkEditAddAccess"), CheckBox).Checked And
               Not CType(gdvLocateModule.Rows(e.RowIndex).FindControl("chkEditUpdateAccess"), CheckBox).Checked And
               Not CType(gdvLocateModule.Rows(e.RowIndex).FindControl("chkEditDeleteAccess"), CheckBox).Checked Then

                ModuleRoleStatus = "C"

            Else

                ModuleRoleStatus = "O"

            End If

            RoleDetails.StoreID = ddlModuleStore.SelectedValue
            RoleDetails.UserRole = ddlModuleRole.SelectedValue
            RoleDetails.ModuleID = CType(gdvLocateModule.Rows(e.RowIndex).FindControl("hidDisplayModuleID"), HiddenField).Value
            RoleDetails.SelectRight = CType(gdvLocateModule.Rows(e.RowIndex).FindControl("chkEditViewAccess"), CheckBox).Checked
            RoleDetails.InsertRight = CType(gdvLocateModule.Rows(e.RowIndex).FindControl("chkEditAddAccess"), CheckBox).Checked
            RoleDetails.UpdateRight = CType(gdvLocateModule.Rows(e.RowIndex).FindControl("chkEditUpdateAccess"), CheckBox).Checked
            RoleDetails.DeleteRight = CType(gdvLocateModule.Rows(e.RowIndex).FindControl("chkEditDeleteAccess"), CheckBox).Checked
            RoleDetails.Status = ModuleRoleStatus
            RoleDetails.LoginUser = Session("UserID")

            lblErrSaveModule.Text = Client.UpdateModuleRole(RoleDetails)
            Client.Close()

            If lblErrSaveModule.Text = String.Empty Then

                Dim Script As String = "ShowSuccessMessage('" & clsCommonFunction.GetMessage(
                                            clsCommonFunction.messageID.Success, "saved", "Module Role") & "');"

                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)

                gdvLocateModule.EditIndex = -1
                btnModuleGo_Click(sender, e)

            Else
                lblErrSaveModule.Visible = True
            End If

        Catch ex As FaultException

            lblErrSaveModule.Text = ex.Message
            lblErrSaveModule.Visible = True

        Catch ex As Exception
            lblErrSaveModule.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            lblErrSaveModule.Visible = True
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If

        End Try

    End Sub

#End Region

#Region " USER ROLE TAB "

    ''' <summary>
    ''' btnUserGo - Click;
    ''' 18 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnUserGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUserGo.Click

        Try

            Dim Client As New ServiceClient
            Dim RoleDetails As New RoleDetails

            RoleDetails.StoreID = ddlUserStore.SelectedValue
            RoleDetails.UserRole = ddlUserRole.SelectedValue

            Cache(Session(ESession.StoreID.ToString) & "UserRoleList") = Client.GetUserRoles(RoleDetails, String.Empty, String.Empty)
            Client.Close()

            gdvLocateUser.DataSource = Cache(Session(ESession.StoreID.ToString) & "UserRoleList")
            gdvLocateUser.DataBind()


            pnlUserRole.Visible = True
            pnlAddEditUser.Visible = False
            ddlUserStore.Enabled = False
            ddlUserRole.Enabled = False
            lblErrSaveUser.Visible = False

        Catch ex As FaultException

            lblErrLocateUser.Text = ex.Message
            lblErrLocateUser.Visible = True

        Catch ex As Exception
            lblErrLocateUser.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            lblErrLocateUser.Visible = True
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If

        End Try

    End Sub

    ''' <summary>
    ''' gdvLocateUser - RowDataBound;
    ''' 18 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gdvLocateUser_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvLocateUser.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            Select Case CType(e.Row.FindControl("hidStatus"), HiddenField).Value.ToUpper
                Case "O"
                    CType(e.Row.FindControl("lblStatus"), Label).Text = "Open"
                Case "C"
                    CType(e.Row.FindControl("lblStatus"), Label).Text = "Closed"
                Case "D"
                    CType(e.Row.FindControl("lblStatus"), Label).Text = "Deleted"
            End Select

            'Mask NRIC Start
            hdnNRIC.Value = e.Row.Cells(2).Text
            Dim NRICVal As TableCell = e.Row.Cells(2)
            NRICVal.CssClass = NRICVal.Text
            NRICVal.Text = String.Format("{0}****{1}", NRICVal.Text.Substring(0, 1), NRICVal.Text.Substring(5, 4))
            'Mask NRIC End

        End If

    End Sub

    ''' <summary>
    ''' gdvLocateUser - RowDeleting;
    ''' 24 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gdvLocateUser_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gdvLocateUser.RowDeleting

        Try

            Dim Client As New ServiceClient
            Dim RoleDetails As New RoleDetails

            RoleDetails.StoreID = ddlUserStore.SelectedValue
            RoleDetails.UserRole = ddlUserRole.SelectedValue
            RoleDetails.UserID = gdvLocateUser.Rows(e.RowIndex).Cells(2).CssClass

            lblErrLocateUser.Text = Client.DeleteUserRole(RoleDetails)
            Client.Close()

            If lblErrLocateUser.Text = String.Empty Then

                Dim Script As String = "ShowSuccessMessage('" & clsCommonFunction.GetMessage(
                                           clsCommonFunction.messageID.Success, "deleted", "User") & "');"

                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)

                btnUserGo_Click(sender, e)

            Else
                lblErrLocateUser.Visible = True
            End If

        Catch ex As FaultException

            lblErrLocateUser.Text = ex.Message
            lblErrLocateUser.Visible = True

        Catch ex As Exception
            lblErrLocateUser.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            lblErrLocateUser.Visible = True
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If

        End Try

    End Sub

    ''' <summary>
    ''' gdvLocateUser - SelectedIndexChanged;
    ''' 18 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gdvLocateUser_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvLocateUser.SelectedIndexChanged

        lblUserNRIC.Visible = True
        txtUserNRIC.Visible = False
        btnUserNRICCheck.Visible = False
        btnAddUserSave.Visible = False
        btnSaveUser.Visible = True

        lblStoreCode.Text = ddlUserStore.SelectedItem.Text.Trim
        lblRole.Text = ddlUserRole.SelectedItem.Text.Trim
        ' Mask NRIC Start
        'Dim getNRIC As String = gdvLocateUser.SelectedRow.Cells(2).Text
        'getNRIC = String.Format("{0}****{1}", getNRIC.Substring(0, 1), getNRIC.Substring(5, 4))
        'lblUserNRIC.Text = getNRIC
        lblUserNRIC.Text = gdvLocateUser.SelectedRow.Cells(2).Text
        ' Mask NRIC End
        lblUserName.Text = gdvLocateUser.SelectedRow.Cells(3).Text
        lblDesignation.Text = gdvLocateUser.SelectedRow.Cells(4).Text
        lblDivision.Text = gdvLocateUser.SelectedRow.Cells(5).Text
        lblDepartment.Text = gdvLocateUser.SelectedRow.Cells(6).Text
        lblInstallationCode.Text = gdvLocateUser.SelectedRow.Cells(7).Text
        lblSection.Text = gdvLocateUser.SelectedRow.Cells(8).Text

        hdnNRIC.Value = gdvLocateUser.SelectedRow.Cells(2).CssClass
        If (gdvLocateUser.SelectedRow.Cells(10).Text <> "&nbsp;") Then
            txtSOEID.Text = gdvLocateUser.SelectedRow.Cells(10).Text
        Else
            txtSOEID.Text = String.Empty
        End If

        Select Case CType(gdvLocateUser.SelectedRow.FindControl("hidStatus"), HiddenField).Value.ToUpper

            Case "O"
                rdoActivateY.Checked = True
                rdoActivateN.Checked = False

            Case "C"
                rdoActivateY.Checked = False
                rdoActivateN.Checked = True

        End Select

        pnlAddEditUser.Visible = True
        pnlUserRole.Visible = False
        lblErrLocateUser.Visible = False
        lblErrSaveModule.Visible = False

        BindConsumerRef()
        BindConsumers()

    End Sub

    ''' <summary>
    ''' gdvLocateUser - Sorting;
    ''' 18 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gdvLocateUser_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gdvLocateUser.Sorting

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
            Dim RoleDetails As New RoleDetails

            RoleDetails.StoreID = ddlUserStore.SelectedValue
            RoleDetails.UserRole = ddlUserRole.SelectedValue

            Cache(Session(ESession.StoreID.ToString) & "UserRoleList") = Client.GetUserRoles(RoleDetails, e.SortExpression, ViewState("_SortDirection"))
            Client.Close()

            gdvLocateUser.DataSource = Cache(Session(ESession.StoreID.ToString) & "UserRoleList")
            gdvLocateUser.DataBind()


            pnlUserRole.Visible = True

        Catch ex As FaultException

            lblErrLocateUser.Text = ex.Message
            lblErrLocateUser.Visible = True

        Catch ex As Exception
            lblErrLocateUser.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            lblErrLocateUser.Visible = True
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If

        End Try


    End Sub

    ''' <summary>
    ''' btnCancelUser - Click;
    ''' 18 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnCancelUser_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelUser.Click

        pnlAddEditUser.Visible = False
        pnlUserRole.Visible = True

    End Sub

    ''' <summary>
    ''' btnAddUser - Click;
    ''' 23 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAddUser_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddUser.Click

        pnlAddEditUser.Visible = True
        pnlUserRole.Visible = False

        lblUserNRIC.Visible = False
        txtUserNRIC.Visible = True
        btnUserNRICCheck.Visible = True
        btnAddUserSave.Visible = True
        btnSaveUser.Visible = False

        lstConsumerLists.Items.Clear()
        lstSelectedConsumers.Items.Clear()

        lblStoreCode.Text = ddlUserStore.SelectedValue
        lblRole.Text = ddlUserRole.SelectedItem.Text.ToString
        lblUserNRIC.Text = String.Empty
        lblUserName.Text = String.Empty
        lblSection.Text = String.Empty
        lblInstallationCode.Text = String.Empty
        lblDivision.Text = String.Empty
        lblDepartment.Text = String.Empty
        lblDesignation.Text = String.Empty
        lblErrSaveUser.Visible = False
        lblErrLocateUser.Visible = False

        txtSOEID.Text = String.Empty

        BindConsumers()

    End Sub

    ''' <summary>
    ''' btnUserClear - Click;
    ''' 20 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnUserClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUserClear.Click

        pnlUserRole.Visible = False
        pnlAddEditUser.Visible = False
        ddlUserStore.Enabled = True
        ddlUserRole.Enabled = True

    End Sub

    ''' <summary>
    ''' btnAddConsumer - Click;
    ''' 20 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAddConsumer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddConsumer.Click

        On Error Resume Next

        lstSelectedConsumers.Items.Add(New ListItem(lstConsumerLists.SelectedItem.Text, lstConsumerLists.SelectedValue))
        lstConsumerLists.Items.Remove(lstConsumerLists.SelectedItem)

    End Sub

    ''' <summary>
    ''' btnRemoveConsumer - Click;
    ''' 20 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnRemoveConsumer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemoveConsumer.Click

        On Error Resume Next

        lstConsumerLists.Items.Add(New ListItem(lstSelectedConsumers.SelectedItem.Text, lstSelectedConsumers.SelectedValue))
        lstSelectedConsumers.Items.Remove(lstSelectedConsumers.SelectedItem)

    End Sub

    ''' <summary>
    ''' btnUserNRICCheck - Click;
    ''' 23 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnUserNRICCheck_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUserNRICCheck.Click

        Try

            If txtUserNRIC.Text.Trim = String.Empty Then

                lblErrSaveUser.Text = "Please enter fields for User ID."
                lblErrSaveUser.Visible = True
                Exit Sub

            End If

            Dim Client As New ServiceClient
            Dim NRICSearch As New RoleDetails

            NRICSearch.UserID = txtUserNRIC.Text.Trim.ToUpper

            Cache(Session(ESession.StoreID.ToString) & "UserRoleList") = Client.CheckNRIC(NRICSearch)
            Client.Close()

            If Cache(Session(ESession.StoreID.ToString) & "UserRoleList").Count > 0 Then

                For Each item As RoleDetails In Cache(Session(ESession.StoreID.ToString) & "UserRoleList")

                    txtUserNRIC.Text = item.UserID
                    txtSOEID.Text = item.SoeID
                    lblUserName.Text = item.Name
                    lblDesignation.Text = item.Designation
                    lblDivision.Text = item.Division
                    lblDepartment.Text = item.Department
                    lblInstallationCode.Text = item.Installation
                    lblSection.Text = item.Section

                Next

            Else

                txtUserNRIC.Text = "S"
                lblUserName.Text = String.Empty
                lblDesignation.Text = String.Empty
                lblDivision.Text = String.Empty
                lblDepartment.Text = String.Empty
                lblInstallationCode.Text = String.Empty
                lblSection.Text = String.Empty

            End If


        Catch ex As FaultException

            lblErrSaveUser.Text = ex.Message
            lblErrSaveUser.Visible = True

        Catch ex As Exception
            lblErrSaveUser.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            lblErrSaveUser.Visible = True
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If

        End Try

    End Sub

    ''' <summary>
    ''' btnSaveUser - Click;
    ''' 22 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSaveUser_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveUser.Click

        Try
            Dim NRIC = lblUserNRIC.Text
            Dim Client As New ServiceClient
            Dim RoleDetails As New RoleDetails
            Dim ConsumerList As New List(Of String)

            For Each item As ListItem In lstSelectedConsumers.Items
                ConsumerList.Add(item.Value)
            Next

            RoleDetails.StoreID = ddlUserStore.SelectedValue
            ' NRIC Mask -- Start
            'RoleDetails.UserID = lblUserNRIC.Text.Trim
            RoleDetails.UserID = hdnNRIC.Value
            ' NRIC Mask -- End
            RoleDetails.UserRole = ddlUserRole.SelectedValue
            RoleDetails.UserStatus = IIf(rdoActivateY.Checked, "O", "C")
            RoleDetails.LoginUser = Session("UserID")
            RoleDetails.ChangeStatusReason = "Deactivated by ICS User"
            RoleDetails.SoeID = txtSOEID.Text.Trim.Replace(" ", "")

            lblErrSaveUser.Text = Client.UpdateUserRole(RoleDetails, ConsumerList)
            Client.Close()

            If lblErrSaveUser.Text = String.Empty Then

                Dim Script As String = "ShowSuccessMessage('" & clsCommonFunction.GetMessage(
                                           clsCommonFunction.messageID.Success, "saved", "User") & "');"

                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)

                btnUserGo_Click(sender, e)
                gdvLocateUser_SelectedIndexChanged(sender, e)

            Else
                lblErrSaveUser.Visible = True
            End If

        Catch ex As FaultException

            lblErrSaveUser.Text = ex.Message
            lblErrSaveUser.Visible = True

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

    ''' <summary>
    ''' btnAddUserSave - Click;
    ''' 24 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAddUserSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddUserSave.Click

        Try

            Dim Client As New ServiceClient
            Dim RoleDetails As New RoleDetails
            Dim ConsumerList As New List(Of String)

            For Each item As ListItem In lstSelectedConsumers.Items
                ConsumerList.Add(item.Value)
            Next

            RoleDetails.StoreID = ddlUserStore.SelectedValue
            RoleDetails.UserID = txtUserNRIC.Text.Trim.ToUpper
            RoleDetails.SoeID = txtSOEID.Text.Trim.Replace(" ", "")
            RoleDetails.UserRole = ddlUserRole.SelectedValue
            RoleDetails.UserStatus = IIf(rdoActivateY.Checked, "O", "C")
            RoleDetails.LoginUser = Session("UserID")

            lblErrSaveUser.Text = Client.AddUserRole(RoleDetails, ConsumerList)
            Client.Close()

            If lblErrSaveUser.Text = String.Empty Then

                lblErrSaveUser.Visible = False

                Dim Script As String = "ShowSuccessMessage('" & clsCommonFunction.GetMessage(
                                           clsCommonFunction.messageID.Success, "added", "User") & "');"

                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)

                btnUserGo_Click(sender, e)

            Else
                lblErrSaveUser.Visible = True

                lblUserName.Text = String.Empty
                lblDesignation.Text = String.Empty
                lblDivision.Text = String.Empty
                lblDepartment.Text = String.Empty
                lblInstallationCode.Text = String.Empty
                lblSection.Text = String.Empty

                Dim Script As String = "ShowAlertMessage('" & lblErrSaveUser.Text & "');"
                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

            End If

        Catch ex As FaultException

            lblErrSaveUser.Text = ex.Message
            lblErrSaveUser.Visible = True

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

#End Region

#Region " Sub Procedures and Functions "

    ''' <summary>
    ''' Sub Proc - BindStores;
    ''' 14 Jan 09 - Jianfa;
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindStores()

        Dim Roles As String() = Split(Session("UserRoleType").ToString, ",")

        For idx As Integer = 0 To UBound(Roles)

            Select Case Roles(idx).ToUpper
                Case "SA"

                    Try

                        Dim Client As New ServiceClient
                        Dim StoreSearch As New StoreDetails
                        Dim StoreList As New List(Of StoreDetails)

                        StoreSearch.StoreId = ""
                        StoreSearch.StoreName = ""
                        StoreSearch.Status = ""

                        StoreList = Client.GetStores(StoreSearch, String.Empty, String.Empty).ToList()
                        Client.Close()

                        ddlModuleStore.Items.Clear()
                        ddlModuleStore.DataSource = StoreList
                        ddlModuleStore.DataValueField = "StoreId"
                        ddlModuleStore.DataTextField = "StoreId"
                        ddlModuleStore.DataBind()

                        ddlUserStore.Items.Clear()
                        ddlUserStore.DataSource = StoreList
                        ddlUserStore.DataValueField = "StoreId"
                        ddlUserStore.DataTextField = "StoreId"
                        ddlUserStore.DataBind()


                    Catch ex As FaultException

                        lblErrLocateModule.Text = ex.Message
                        lblErrLocateModule.Visible = True

                    Catch ex As Exception
                        Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
                        If (rethrow) Then
                            Throw
                        End If
                    End Try

                    Exit Sub

                Case Else

                    ddlModuleStore.Items.Clear()
                    ddlModuleStore.Items.Insert(0, New ListItem(Session("StoreID").ToString, Session("StoreID").ToString))
                    ddlModuleStore.DataBind()

                    ddlUserStore.Items.Clear()
                    ddlUserStore.Items.Insert(0, New ListItem(Session("StoreID").ToString, Session("StoreID").ToString))
                    ddlUserStore.DataBind()

            End Select

        Next

    End Sub

    ''' <summary>
    ''' Sub Proc - BindUserRoles();
    ''' 14 Jan 09 - Jianfa;
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindUserRoles()

        Dim View As DataView
        View = clsCommonFunction.GetCommonDataByCodeGroup(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), clsCommonFunction.codeGroup.UserRole, "O")

        Dim Roles As String() = Split(Session("UserRoleType").ToString, ",")

        For idx As Integer = 0 To UBound(Roles)

            Select Case Roles(idx).ToUpper
                Case "SA"

                    ddlModuleRole.DataSource = View
                    ddlModuleRole.DataValueField = "CommonCodeID"
                    ddlModuleRole.DataTextField = "CommonCodeDescription"
                    ddlModuleRole.DataBind()

                    ddlUserRole.DataSource = View
                    ddlUserRole.DataValueField = "CommonCodeID"
                    ddlUserRole.DataTextField = "CommonCodeDescription"
                    ddlUserRole.DataBind()

                    Exit Sub

            End Select
        Next

        View.RowFilter = " CommonCodeID <> 'SA' and CommonCodeGroup = 'User Role'"

        ddlModuleRole.DataSource = View
        ddlModuleRole.DataValueField = "CommonCodeID"
        ddlModuleRole.DataTextField = "CommonCodeDescription"
        ddlModuleRole.DataBind()

        ddlUserRole.DataSource = View
        ddlUserRole.DataValueField = "CommonCodeID"
        ddlUserRole.DataTextField = "CommonCodeDescription"
        ddlUserRole.DataBind()

    End Sub

    ''' <summary>
    ''' Sub Proc - BindConsumers;
    ''' 21 Jan 09 - Jianfa;
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindConsumers()

        Try

            Dim Client As New ServiceClient
            Dim ConsumerDetails As New ConsumerDetails
            Dim ConsumerList As New List(Of ConsumerDetails)

            ConsumerList.Clear()

            ConsumerDetails.StoreID = ddlUserStore.SelectedValue
            ConsumerDetails.ConsumerID = String.Empty
            ConsumerDetails.ConsumerDescription = String.Empty
            ConsumerDetails.ConsumerStatus = "O"

            ConsumerList = Client.GetConsumers(ConsumerDetails, String.Empty, String.Empty).ToList()
            Client.Close()

            lstConsumerLists.Items.Clear()
            lstConsumerLists.DataSource = ConsumerList
            lstConsumerLists.DataValueField = "ConsumerID"
            lstConsumerLists.DataTextField = "ConsumerDescription"
            lstConsumerLists.DataBind()


            For Each itemX As ListItem In lstSelectedConsumers.Items

                For Each itemY As ListItem In lstConsumerLists.Items

                    If itemX.Value = itemY.Value Then
                        lstConsumerLists.Items.Remove(itemY)
                        Exit For
                    End If

                Next

            Next

        Catch ex As FaultException

            lblErrSaveUser.Text = ex.Message
            lblErrSaveUser.Visible = True

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

    ''' <summary>
    ''' Sub Proc - BindConsumerRef;
    ''' 20 Jan 09 - Jianfa;
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindConsumerRef()

        Try

            Dim Client As New ServiceClient
            Dim ConsumerDetails As New ConsumerDetails
            Dim ConsumerList As New List(Of ConsumerDetails)

            ConsumerList.Clear()

            ConsumerDetails.StoreID = ddlUserStore.SelectedValue
            ConsumerDetails.UserID = gdvLocateUser.SelectedRow.Cells(2).Text.Trim
            ConsumerDetails.UserRole = ddlUserRole.SelectedValue
            ConsumerDetails.ConsumerRefStatus = "O"

            ConsumerList = Client.GetConsumerRefByUserID(ConsumerDetails).ToList()
            Client.Close()

            lstSelectedConsumers.Items.Clear()
            lstSelectedConsumers.DataSource = ConsumerList
            lstSelectedConsumers.DataValueField = "ConsumerID"
            lstSelectedConsumers.DataTextField = "ConsumerDescription"
            lstSelectedConsumers.DataBind()


        Catch ex As FaultException

            lblErrSaveUser.Text = ex.Message
            lblErrSaveUser.Visible = True

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try


    End Sub

#End Region

End Class