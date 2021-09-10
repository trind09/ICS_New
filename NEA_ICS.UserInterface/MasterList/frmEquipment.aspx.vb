Imports Microsoft.Reporting.WebForms
Imports System.Web.Services.Description
Imports NEA_ICS.UserInterface.NEA_ICS.WcfService
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ServiceModel
Imports System.Reflection

Partial Public Class frmEquipment
    Inherits clsCommonFunction

    'Private Shared EquipmentList As List(Of EquipmentDetails)
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

#Region " Page Load "
    ''' <summary>
    ''' Page Load 
    ''' 24 Dec 2008 - Jianfa 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim AccessRights As New List(Of RoleDetails)

        CheckValidSession()

        If Not Page.IsPostBack Then

            AccessRights = clsCommonFunction.AssignAccessRights(Cache(Session(ESession.UserID.ToString) & Session(ESession.StoreID.ToString) & "AccessRights"), _
                                                               clsCommonFunction.moduleID.Equipment)

            If AccessRights(0).SelectRight = False Then
                Server.Transfer("..\frmUnauthorisedPage.aspx")
                Exit Sub
            End If

            If AccessRights(0).InsertRight = False Then
                tbpNewEquipment.Visible = False
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

            tbcEquipment.Visible = True

            BindEquipmentType()

        End If
    End Sub
#End Region

#Region " New Tab "
    ''' <summary>
    ''' btnAddEquipment - Click;
    ''' 24 December 2008 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 1) Validate Mandatory Fields;
    ''' 2) Validate unique Equipment Code;
    ''' 3) Insert Equipment Details;
    ''' 4) Display Inserted Equipment Details;
    ''' </remarks>
    Private Sub btnAddEquipment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddEquipment.Click

        Try

            If txtEquipmentCode.Text = String.Empty Or txtDescription.Text = String.Empty Or _
            ddlEquipmentType.SelectedIndex <= 0 Then

                lblErrAddEquipment.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.MandatoryField)
                lblErrAddEquipment.Visible = True

                Dim Script As String = "ShowAlertMessage('" & lblErrAddEquipment.Text & "');"
                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

            Else

                Dim Client As New ServiceClient
                Dim EquipmentDetails As New EquipmentDetails

                EquipmentDetails.StoreID = Session("StoreID").ToString
                EquipmentDetails.EquipmentID = txtEquipmentCode.Text.ToUpper
                EquipmentDetails.EquipmentType = ddlEquipmentType.SelectedValue
                EquipmentDetails.EquipmentDescription = txtDescription.Text
                EquipmentDetails.Status = "O"
                EquipmentDetails.LoginUser = Session("UserID").ToString

                lblErrAddEquipment.Text = Client.AddEquipment(EquipmentDetails)
                Client.Close()

                If lblErrAddEquipment.Text.Trim = String.Empty Then

                    lblMsgEquipmentCode.Text = txtEquipmentCode.Text.ToUpper
                    lblMsgEquipmentType.Text = IIf(ddlEquipmentType.SelectedIndex <= 0, String.Empty, ddlEquipmentType.SelectedItem.Text)
                    lblMsgEquipmentDescription.Text = txtDescription.Text

                    lblErrAddEquipment.Visible = False
                    pnlAddEquipment.Visible = False
                    divMsgBox.Visible = True

                Else
                    lblErrAddEquipment.Visible = True

                    Dim Script As String = "ShowAlertMessage('" & lblErrAddEquipment.Text & "');"
                    ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

                End If

            End If

        Catch ex As FaultException
            Dim fault As ServiceFault = ex.Data

            lblErrAddEquipment.Text = fault.MessageText
            lblErrAddEquipment.Visible = True

        Catch ex As Exception
            lblErrAddEquipment.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            lblErrAddEquipment.Visible = True
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

    ''' <summary>
    ''' btnAddEquipmentOK - Click;
    ''' 29 Dec 2008 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' To unhide the pnlAddEquipment
    ''' </remarks>
    Private Sub btnAddEquipmentOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddEquipmentOK.Click

        pnlAddEquipment.Visible = True
        divMsgBox.Visible = False

    End Sub

    ''' <summary>
    ''' btnClear - Click;
    ''' 24 Dec 2008 - Jianfa
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' To clear away UI controls for Add Equipment
    ''' </remarks>
    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click

        ddlEquipmentType.SelectedIndex = -1
        txtEquipmentCode.Text = String.Empty
        txtDescription.Text = String.Empty
        lblErrAddEquipment.Visible = False

    End Sub

#End Region

#Region " Search Tab "
    ''' <summary>
    ''' btnLocateGo - Click;
    ''' 29 Dec 2008 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnLocateGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLocateGo.Click

        Try

            Dim Client As New ServiceClient
            Dim EquipmentSearch As New EquipmentDetails

            EquipmentSearch.StoreID = Session("StoreID").ToString
            EquipmentSearch.EquipmentID = txtLocateEquipmentCode.Text.Trim
            EquipmentSearch.EquipmentType = ddlLocateEquipmentType.SelectedValue
            EquipmentSearch.EquipmentDescription = txtLocateDescription.Text.Trim
            EquipmentSearch.Status = ddlLocateStatus.SelectedValue

            Cache(Session(ESession.StoreID.ToString) & "EquipmentList") = Client.GetEquipments(EquipmentSearch, String.Empty, String.Empty)

            Client.Close()

            gdvLocateEquipment.DataSource = Cache(Session(ESession.StoreID.ToString) & "EquipmentList")
            gdvLocateEquipment.DataBind()

            pnlSearchResults.Visible = True
            gdvLocateEquipment.Visible = True

        Catch ex As FaultException
            Dim fault As ServiceFault = ex.Data

            lblErrLocateEquipment.Text = fault.MessageText
            lblErrLocateEquipment.Visible = True

        Catch ex As Exception
            lblErrLocateEquipment.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            lblErrLocateEquipment.Visible = True
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If

        End Try

    End Sub

    ''' <summary>
    ''' btnLocateSave - Click;
    ''' 30 Dec 2008 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 1) To check for mandatory fields
    ''' 2) to update equipment description
    ''' </remarks>
    Private Sub btnLocateSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLocateSave.Click

        Try
            BindEquipmentType()

            'If txtEditDescription.Text = String.Empty Or ddlEditEquipmentType.SelectedIndex <= 0 Then

            If txtEditDescription.Text = String.Empty Then

                lblErrSaveEquipment.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.MandatoryField)

                Dim Script As String = "ShowAlertMessage('" & lblErrSaveEquipment.Text & "');"
                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

            Else

                Dim Client As New ServiceClient
                Dim EquipmentDetails As New EquipmentDetails

                EquipmentDetails.StoreID = Session("StoreID").ToString
                EquipmentDetails.EquipmentID = lblEditEquipmentCode.Text
                EquipmentDetails.EquipmentType = ddlEditEquipmentType.SelectedValue
                EquipmentDetails.EquipmentDescription = txtEditDescription.Text
                EquipmentDetails.LoginUser = Session("UserID").ToString

                lblErrSaveEquipment.Text = Client.UpdateEquipment(EquipmentDetails)
                Client.Close()

                If lblErrSaveEquipment.Text.Trim = String.Empty Then

                    Dim Script As String = "ShowSuccessMessage('" & clsCommonFunction.GetMessage( _
                                                                clsCommonFunction.messageID.Success, "saved", "Equipment") & "');"

                    ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)

                    btnLocateGo_Click(sender, e)

                Else
                    lblErrSaveEquipment.Visible = True

                    Dim Script As String = "ShowAlertMessage('" & lblErrSaveEquipment.Text & "');"
                    ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)
                End If

            End If

        Catch ex As FaultException
            Dim fault As ServiceFault = ex.Data

            lblErrSaveEquipment.Text = fault.MessageText
            lblErrSaveEquipment.Visible = True

        Catch ex As Exception
            lblErrSaveEquipment.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            lblErrSaveEquipment.Visible = True
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try


    End Sub

    ''' <summary>
    ''' btnLocateCancel - Click;
    ''' 30 Dec 2008 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 1) To toggle visibility of the equipment info
    ''' </remarks>
    Private Sub btnLocateCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLocateCancel.Click

        pnlEquipmentInfo.Visible = False
        EditMode(False)

    End Sub

    ''' <summary>
    ''' btnLocateClear - Click;
    ''' 29 Dec 2008 - Jianfa; 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 1) To clear away UI for search results
    ''' </remarks>
    Private Sub btnLocateClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLocateClear.Click

        lblErrLocateEquipment.Visible = False
        txtLocateEquipmentCode.Text = String.Empty
        txtLocateDescription.Text = String.Empty
        ddlLocateEquipmentType.SelectedIndex = -1
        ddlLocateStatus.SelectedValue = "O"
        pnlSearchResults.Visible = False
        pnlEquipmentInfo.Visible = False

    End Sub

    ''' <summary>
    ''' gdvLocateEquipment - PageIndexChanging;
    ''' 29Dec08 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CODE LOGIC:
    ''' 1) To populate data according to selected page no
    ''' </remarks>
    Private Sub gdvLocateEquipment_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gdvLocateEquipment.PageIndexChanging

        gdvLocateEquipment.PageIndex = e.NewPageIndex
        gdvLocateEquipment.DataSource = Cache(Session(ESession.StoreID.ToString) & "EquipmentList")
        gdvLocateEquipment.DataBind()

    End Sub

    ''' <summary>
    ''' gdvLocateEquipment - Sorting;
    ''' 31 Dec 2008 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gdvLocateEquipment_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gdvLocateEquipment.Sorting

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
            Dim EquipmentSearch As New EquipmentDetails

            EquipmentSearch.StoreID = Session("StoreID").ToString
            EquipmentSearch.EquipmentID = txtLocateEquipmentCode.Text.Trim
            EquipmentSearch.EquipmentType = ddlLocateEquipmentType.SelectedValue
            EquipmentSearch.EquipmentDescription = txtLocateDescription.Text.Trim
            EquipmentSearch.Status = ddlLocateStatus.SelectedValue

            Cache(Session(ESession.StoreID.ToString) & "EquipmentList") = Client.GetEquipments(EquipmentSearch, e.SortExpression, ViewState("_SortDirection").ToString)
            Client.Close()

            gdvLocateEquipment.DataSource = Cache(Session(ESession.StoreID.ToString) & "EquipmentList")
            gdvLocateEquipment.DataBind()

        Catch ex As FaultException
            Dim fault As ServiceFault = ex.Data

            lblErrLocateEquipment.Text = fault.MessageText
            lblErrLocateEquipment.Visible = True

        Catch ex As Exception
            lblErrLocateEquipment.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            lblErrLocateEquipment.Visible = True
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If

        End Try

    End Sub

    ''' <summary>
    ''' gdvLocateEquipment - RowDataBound;
    ''' 29 Dec 2008 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CODE LOGIC:
    ''' 1) To manipulate code description based on code ID from gridview
    ''' </remarks>
    Private Sub gdvLocateEquipment_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvLocateEquipment.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            Select Case CType(e.Row.FindControl("hidStatus"), HiddenField).Value.ToUpper
                Case "O"
                    CType(e.Row.FindControl("lblStatus"), Label).Text = "Open"
                Case "C"
                    CType(e.Row.FindControl("lblStatus"), Label).Text = "Closed"
                Case "D"
                    CType(e.Row.FindControl("lblStatus"), Label).Text = "Deleted"

            End Select
        End If

    End Sub

    ''' <summary>
    ''' gdvLocateEquipment - SelectedIndexChanged;
    ''' 29 Dec 2008 - Jianfa
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CODE LOGIC:
    ''' 1) To disable UI controls in Non-Editable Mode
    ''' 2) To display results in Equipment Panel
    ''' 3) To hide/unhide Close/Reopen Equipment
    ''' </remarks>
    Private Sub gdvLocateEquipment_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvLocateEquipment.SelectedIndexChanged

        lblEditEquipmentCode.Text = gdvLocateEquipment.SelectedRow.Cells(1).Text
        txtEditDescription.Text = Replace(gdvLocateEquipment.SelectedRow.Cells(2).Text, "&amp;", "&")
        lblDisplayStatus.Text = CType(gdvLocateEquipment.SelectedRow.FindControl("lblStatus"), Label).Text
        IIf(ddlEditEquipmentType.SelectedValue = CType(gdvLocateEquipment.SelectedRow.FindControl("hidEquipmentType"), HiddenField).Value, CType(gdvLocateEquipment.SelectedRow.FindControl("hidEquipmentType"), HiddenField).Value, String.Empty)
        BindEquipmentType()

        If ViewState("_EDIT") Then
            Select Case CType(gdvLocateEquipment.SelectedRow.FindControl("hidStatus"), HiddenField).Value.Trim
                Case "O"
                    lbtnLocateClose.Visible = True
                    lbtnLocateReopen.Visible = False
                Case "C"
                    lbtnLocateClose.Visible = False
                    lbtnLocateReopen.Visible = True
                Case "D"
                    lbtnLocateClose.Visible = True
                    lbtnLocateReopen.Visible = True
                Case Else
                    lbtnLocateClose.Visible = False
                    lbtnLocateReopen.Visible = False
            End Select
        Else
            lbtnLocateClose.Visible = False
            lbtnLocateReopen.Visible = False
        End If

        EditMode(False)
        pnlEquipmentInfo.Visible = True
        lblErrSaveEquipment.Visible = False
        lblErrLocateEquipment.Visible = False

    End Sub

    ''' <summary>
    ''' lbtnLocateEdit - Click;
    ''' 30 Dec 2008 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CODE LOGIC:
    ''' 1) To enable UI controls for editing
    ''' 2) To hide error message
    ''' </remarks>
    Private Sub lbtnLocateEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnLocateEdit.Click

        EditMode(True)
        lblErrSaveEquipment.Visible = False

    End Sub

    ''' <summary>
    ''' lbtnLocateDel - Click;
    ''' 23 Dec 08 - Jianfa
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CODE LOGIC:
    ''' 1) To do a logical update to the equipment status (Delete)
    ''' </remarks>
    Private Sub lbtnLocateDel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnLocateDel.Click

        Try

            Dim Client As New ServiceClient
            Dim EquipmentDetails As New EquipmentDetails

            EquipmentDetails.StoreID = Session("StoreID").ToString
            EquipmentDetails.EquipmentID = lblEditEquipmentCode.Text.ToUpper
            EquipmentDetails.Status = "D"
            EquipmentDetails.LoginUser = Session("UserID").ToString

            lblErrSaveEquipment.Text = Client.UpdateEquipmentStatus(EquipmentDetails)
            Client.Close()

            If lblErrSaveEquipment.Text = String.Empty Then

                Dim Script As String = "ShowSuccessMessage('" & clsCommonFunction.GetMessage( _
                                        clsCommonFunction.messageID.Success, "deleted", "Equipment") & "');"

                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)

                btnLocateGo_Click(sender, e)
                gdvLocateEquipment_SelectedIndexChanged(sender, e)

            Else
                lblErrSaveEquipment.Visible = True
            End If

        Catch ex As FaultException
            Dim fault As ServiceFault = ex.Data

            lblErrSaveEquipment.Text = fault.MessageText
            lblErrSaveEquipment.Visible = True

        Catch ex As Exception
            lblErrSaveEquipment.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            lblErrSaveEquipment.Visible = True
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If

        End Try

    End Sub

    ''' <summary>
    ''' lbtnLocateClose - Click;
    ''' 30 Dec 2008 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub lbtnLocateClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnLocateClose.Click

        Try

            Dim Client As New ServiceClient
            Dim EquipmentDetails As New EquipmentDetails

            EquipmentDetails.StoreID = Session("StoreID").ToString
            EquipmentDetails.EquipmentID = lblEditEquipmentCode.Text.ToUpper
            EquipmentDetails.Status = "C"
            EquipmentDetails.LoginUser = Session("UserID").ToString

            lblErrSaveEquipment.Text = Client.UpdateEquipmentStatus(EquipmentDetails)
            Client.Close()

            If lblErrSaveEquipment.Text = String.Empty Then

                Dim Script As String = "ShowSuccessMessage('" & clsCommonFunction.GetMessage( _
                                        clsCommonFunction.messageID.Success, "closed", "Equipment") & "');"

                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)

                btnLocateGo_Click(sender, e)
                gdvLocateEquipment_SelectedIndexChanged(sender, e)

            Else
                lblErrSaveEquipment.Visible = True
            End If

        Catch ex As FaultException
            Dim fault As ServiceFault = ex.Data

            lblErrSaveEquipment.Text = fault.MessageText
            lblErrSaveEquipment.Visible = True

        Catch ex As Exception
            lblErrSaveEquipment.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            lblErrSaveEquipment.Visible = True
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If

        End Try

    End Sub

    ''' <summary>
    ''' lbtnLocateReopen - Click;
    ''' 30 Dec 2008 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub lbtnLocateReopen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnLocateReopen.Click

        Try

            Dim Client As New ServiceClient
            Dim EquipmentDetails As New EquipmentDetails

            EquipmentDetails.StoreID = Session("StoreID").ToString
            EquipmentDetails.EquipmentID = lblEditEquipmentCode.Text.ToUpper
            EquipmentDetails.Status = "O"
            EquipmentDetails.LoginUser = Session("UserID").ToString

            lblErrSaveEquipment.Text = Client.UpdateEquipmentStatus(EquipmentDetails)
            Client.Close()

            If lblErrSaveEquipment.Text = String.Empty Then

                Dim Script As String = "ShowSuccessMessage('" & clsCommonFunction.GetMessage( _
                                        clsCommonFunction.messageID.Success, "opened", "Equipment") & "');"

                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)

                btnLocateGo_Click(sender, e)
                gdvLocateEquipment_SelectedIndexChanged(sender, e)

            Else
                lblErrSaveEquipment.Visible = True
            End If

        Catch ex As FaultException
            Dim fault As ServiceFault = ex.Data

            lblErrSaveEquipment.Text = fault.MessageText
            lblErrSaveEquipment.Visible = True

        Catch ex As Exception
            lblErrSaveEquipment.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            lblErrSaveEquipment.Visible = True
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If

        End Try

    End Sub

#End Region

#Region " Sub Procedures and Functions "
    ''' <summary>
    ''' Sub Procedure - DisableEditMode;
    ''' 23 Dec 08 - To enable/disable UI controls for Edit
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    Protected Sub EditMode(ByVal Edit As Boolean)

        Select Case Edit

            Case False
                txtEditDescription.Enabled = False
                btnLocateSave.Enabled = False
                ddlEditEquipmentType.Enabled = False
            Case True
                txtEditDescription.Enabled = True
                btnLocateSave.Enabled = True
                ddlEditEquipmentType.Enabled = True
                BindEquipmentType()
        End Select

    End Sub

    ''' <summary>
    ''' Sub Proc - BindEquipmentType;
    ''' 15 Jan 09 - Jianfa
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindEquipmentType()

        Dim View As DataView
        View = clsCommonFunction.GetCommonDataByCodeGroup(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), clsCommonFunction.codeGroup.EquipmentType, "O")

        ddlEquipmentType.DataSource = View

        ddlEquipmentType.DataTextField = "CommonCodeDescription"
        ddlEquipmentType.DataValueField = "CommonCodeID"
        ddlEquipmentType.DataBind()

        ddlEquipmentType.Items.Insert(0, New ListItem(" - Please Select - ", ""))

        ddlLocateEquipmentType.DataSource = View

        ddlLocateEquipmentType.DataTextField = "CommonCodeDescription"
        ddlLocateEquipmentType.DataValueField = "CommonCodeID"
        ddlLocateEquipmentType.DataBind()

        ddlLocateEquipmentType.DataBind()

        ddlLocateEquipmentType.Items.Insert(0, New ListItem(" - Please Select - ", ""))

        ddlEditEquipmentType.DataSource = View

        ddlEditEquipmentType.DataTextField = "CommonCodeDescription"
        ddlEditEquipmentType.DataValueField = "CommonCodeID"
        'ddlEditEquipmentType.DataValueField = "CommonCodeDescription"
        ddlEditEquipmentType.DataBind()

        ddlEditEquipmentType.DataBind()

        'ddlEditEquipmentType.Items.Insert(0, New ListItem(" - Please Select - ", ""))

    End Sub

#End Region

    Protected Sub ObjectDataSource1_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs) Handles ObjectDataSource1.Selecting
        Dim searchEquipment As New EquipmentDetails()
        searchEquipment.StoreID = Session("StoreID")
        searchEquipment.EquipmentID = ""
        searchEquipment.EquipmentType = ""
        searchEquipment.EquipmentDescription = ""
        searchEquipment.Status = ""

        e.InputParameters("equipmentDetails") = searchEquipment
        e.InputParameters("sortExpression") = "EquipmentID"
        e.InputParameters("sortDirection") = ""
    End Sub
    Protected Sub ObjectDataSource1_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles ObjectDataSource1.Selected
        Dim returnList As List(Of EquipmentDetails) = e.ReturnValue
        If returnList.Count <= 0 Then
            NoRecordFond = "Y"
        Else
            NoRecordFond = "N"
        End If
    End Sub

    Protected Sub btnPDF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPDF.Click
        rvr.LocalReport.DataSources.Clear()
        rvr.LocalReport.DataSources.Add(New ReportDataSource("EquipmentDetails", ObjectDataSource1))

        Dim parameterlist As New List(Of ReportParameter)
        Dim p1 As New ReportParameter("ReportTitle", "Equipment Master List Report")
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
        Response.AddHeader("content-disposition", "attachment;filename=EquipmentMasterList.pdf")
        Response.BinaryWrite(bytValue)
        Response.Flush()
        Response.End()
    End Sub

    Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExcel.Click
        rvr.LocalReport.DataSources.Clear()
        rvr.LocalReport.DataSources.Add(New ReportDataSource("EquipmentDetails", ObjectDataSource1))

        Dim parameterlist As New List(Of ReportParameter)
        Dim p1 As New ReportParameter("ReportTitle", "Equipment Master List Report")
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
        Response.AddHeader("content-disposition", "attachment;filename=EquipmentMasterList.xls")
        Response.BinaryWrite(bytValue)
        Response.Flush()
        Response.End()
    End Sub
End Class