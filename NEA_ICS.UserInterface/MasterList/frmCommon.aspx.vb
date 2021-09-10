Imports Microsoft.Reporting.WebForms
Imports System.Web.Services.Description
Imports NEA_ICS.UserInterface.NEA_ICS.WcfService
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ServiceModel
Imports System.Reflection

Partial Public Class frmCommon
    Inherits clsCommonFunction

    'Private Shared CommonList As List(Of CommonDetails)

#Region " Page Load "

    ''' <summary>
    ''' Page Load;
    ''' 03 Jan 2009 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' </remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim AccessRights As New List(Of RoleDetails)

        CheckValidSession()

        If Not Page.IsPostBack Then

            AccessRights = clsCommonFunction.AssignAccessRights(Cache(Session(ESession.UserID.ToString) & Session(ESession.StoreID.ToString) & "AccessRights"), _
                                                                clsCommonFunction.moduleID.Common)

            If AccessRights(0).SelectRight = False Then
                Server.Transfer("..\frmUnauthorisedPage.aspx")
                Exit Sub
            End If

            If AccessRights(0).InsertRight = False Then
                btnAddNewCode.Visible = False
            End If

            If AccessRights(0).UpdateRight = False Then
                gdvLocateCode.Columns(0).Visible = False
            End If

            BindCommonGroup()

        End If
    End Sub
#End Region

#Region " Sub Procedures and Functions "

    ''' <summary>
    ''' Sub Proc - BindCommonGroup;
    ''' 03 Jan 09 - Jianfa;
    ''' </summary>
    ''' <remarks>
    ''' 1) To bind data items into common code group dropdown list.
    ''' </remarks>
    Protected Sub BindCommonGroup()

        Try

            Dim Client As New ServiceClient
            Dim CommonDetails As New CommonDetails
            Dim CommonList As New List(Of CommonDetails)

            CommonDetails.StoreID = Session("StoreID")
            CommonDetails.CodeGroup = String.Empty
            CommonDetails.Status = DEFAULTED
            CommonList = Client.GetDistinctCommon(CommonDetails)
            Client.Close()

            ddlCommonItems.DataSource = CommonList
            ddlCommonItems.DataTextField = "CodeGroup"
            ddlCommonItems.DataValueField = "CodeGroup"

            ddlCommonItems.DataBind()
            ddlCommonItems.Items.Insert(0, New ListItem(" - Please Select - ", ""))


        Catch ex As FaultException
            Dim fault As ServiceFault = ex.Data

            lblErrSaveCode.Text = fault.MessageText
            lblErrSaveCode.Visible = True

        Catch ex As Exception
            lblErrSaveCode.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            lblErrSaveCode.Visible = True
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If

        End Try

    End Sub

#End Region

#Region " Common Items "
    ''' <summary>
    ''' btnGo - Click;
    ''' 03 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGo.Click

        If ddlCommonItems.SelectedIndex <= 0 Then
            lblErrLocate.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.MandatoryField)
            lblErrLocate.Visible = True

            Dim Script As String = "ShowAlertMessage('" & lblErrLocate.Text & "');"
            ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

            Exit Sub
        End If

        Try

            lblErrLocate.Text = "<br>"
            lblErrLocate.Visible = True

            Dim Client As New ServiceClient
            Dim CommonDetails As New CommonDetails

            CommonDetails.StoreID = Session("StoreID")
            CommonDetails.Status = String.Empty
            CommonDetails.CodeGroup = ddlCommonItems.SelectedValue
            Cache(Session(ESession.StoreID.ToString) & "CommonList") = Client.GetCommon(CommonDetails)
            Client.Close()

            gdvLocateCode.EditIndex = -1
            gdvLocateCode.DataSource = Cache(Session(ESession.StoreID.ToString) & "CommonList")
            gdvLocateCode.DataBind()

            Select Case ddlCommonItems.SelectedValue
                Case "Finance Cutoff Day"
                    btnAddNewCode.Enabled = False
                Case Else
                    btnAddNewCode.Enabled = True
            End Select

            pnlCommonInfo.Visible = True
            pnlAddCode.Visible = False
            divMsgBoxAddCode.Visible = False


        Catch ex As FaultException

            lblErrSaveCode.Text = ex.Message
            lblErrSaveCode.Visible = True

        Catch ex As Exception
            lblErrSaveCode.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            lblErrSaveCode.Visible = True
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If

        End Try

    End Sub

    ''' <summary>
    ''' btnClear - Click;
    ''' 06 Jan 2009 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click

        lblErrLocate.Text = "<br>"
        lblErrLocate.Visible = True
        ddlCommonItems.SelectedIndex = -1
        pnlAddCode.Visible = False
        pnlCommonInfo.Visible = False

    End Sub

    ''' <summary>
    ''' gdvLocateCode - RowCancelingEdit;
    ''' 05 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gdvLocateCode_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gdvLocateCode.RowCancelingEdit

        lblErrSaveCode.Visible = False
        gdvLocateCode.EditIndex = -1
        gdvLocateCode.DataSource = Cache(Session(ESession.StoreID.ToString) & "CommonList")
        gdvLocateCode.DataBind()

    End Sub

    ''' <summary>
    ''' gdvLocateCode - RowDataBound;
    ''' 05 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gdvLocateCode_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvLocateCode.RowDataBound

        Dim Status As String = String.Empty

        If e.Row.RowType = DataControlRowType.DataRow Then

            If Not e.Row.FindControl("hidStatus") Is Nothing Then
                Select Case CType(e.Row.FindControl("hidStatus"), HiddenField).Value.ToUpper
                    Case "O"
                        CType(e.Row.FindControl("lblStatus"), Label).Text = "Open"
                    Case "C"
                        CType(e.Row.FindControl("lblStatus"), Label).Text = "Closed"
                    Case "D"
                        CType(e.Row.FindControl("lblStatus"), Label).Text = "Deleted"
                End Select
            End If

            If e.Row.RowIndex = gdvLocateCode.EditIndex Then

                Select Case ddlCommonItems.SelectedValue.Trim

                    Case "Finance Cutoff Day", "User Role"
                        CType(e.Row.FindControl("lblDisplayCodeID"), Label).Visible = True
                        CType(e.Row.FindControl("txtCodeID"), TextBox).Visible = False

                    Case Else
                        CType(e.Row.FindControl("lblDisplayCodeID"), Label).Visible = False
                        CType(e.Row.FindControl("txtCodeID"), TextBox).Visible = True

                End Select

                CType(e.Row.FindControl("ddlStatus"), DropDownList).SelectedValue = CType(e.Row.FindControl("hidEditStatus"), HiddenField).Value.Trim

            End If

        End If

    End Sub

    ''' <summary>
    ''' gdvLocateCode - RowEditing;
    ''' 05 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gdvLocateCode_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gdvLocateCode.RowEditing

        gdvLocateCode.EditIndex = e.NewEditIndex
        gdvLocateCode.SelectedIndex = e.NewEditIndex
        gdvLocateCode.DataSource = Cache(Session(ESession.StoreID.ToString) & "CommonList")
        gdvLocateCode.DataBind()

    End Sub

    ''' <summary>
    ''' gdvLocateCode - RowUpdating;
    ''' 05 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gdvLocateCode_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gdvLocateCode.RowUpdating

        Try

            Select Case ddlCommonItems.SelectedValue
                Case "Finance Cutoff Day"

                    If CType(gdvLocateCode.Rows(e.RowIndex).FindControl("txtCodeDescription"), TextBox).Text.Trim = String.Empty Then

                        lblErrSaveCode.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.MandatoryField)
                        lblErrSaveCode.Visible = True

                        Dim Script As String = "ShowAlertMessage('" & lblErrSaveCode.Text & "');"
                        ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

                    Else

                        If Not IsNumeric(CType(gdvLocateCode.Rows(e.RowIndex).FindControl("txtCodeDescription"), TextBox).Text.Trim) Then
                            lblErrSaveCode.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.NumericFields, "Finance Cutoff Day")
                            lblErrSaveCode.Visible = True
                        Else

                            Dim Client As New ServiceClient
                            Dim CommonDetails As New CommonDetails

                            CommonDetails.StoreID = Session("StoreID")
                            CommonDetails.CommonID = CInt(CType(gdvLocateCode.Rows(e.RowIndex).FindControl("hidCommonID"), HiddenField).Value.Trim)
                            CommonDetails.CodeGroup = CType(gdvLocateCode.Rows(e.RowIndex).FindControl("hidCodeGroup"), HiddenField).Value.Trim
                            CommonDetails.CodeID = CType(gdvLocateCode.Rows(e.RowIndex).FindControl("lblDisplayCodeID"), Label).Text.Trim.ToUpper
                            CommonDetails.CodeDescription = CType(gdvLocateCode.Rows(e.RowIndex).FindControl("txtCodeDescription"), TextBox).Text.Trim
                            CommonDetails.Status = CType(gdvLocateCode.Rows(e.RowIndex).FindControl("ddlStatus"), DropDownList).SelectedValue.Trim
                            CommonDetails.LoginUser = Session("UserID")

                            lblErrSaveCode.Text = Client.UpdateCommon(CommonDetails)
                            Client.Close()

                            If lblErrSaveCode.Text = String.Empty Then

                                Dim Script As String = "ShowSuccessMessage('" & clsCommonFunction.GetMessage( _
                                            clsCommonFunction.messageID.Success, "saved", "Code") & "');"

                                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)

                                gdvLocateCode.EditIndex = -1
                                btnGo_Click(sender, e)

                            Else
                                lblErrSaveCode.Visible = True
                            End If

                        End If

                    End If

                Case "User Role"

                    If CType(gdvLocateCode.Rows(e.RowIndex).FindControl("txtCodeDescription"), TextBox).Text.Trim = String.Empty Then

                        lblErrSaveCode.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.MandatoryField)
                        lblErrSaveCode.Visible = True

                        Dim Script As String = "ShowAlertMessage('" & lblErrSaveCode.Text & "');"
                        ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

                    Else

                        Dim Client As New ServiceClient
                        Dim CommonDetails As New CommonDetails

                        CommonDetails.StoreID = Session("StoreID")
                        CommonDetails.CommonID = CInt(CType(gdvLocateCode.Rows(e.RowIndex).FindControl("hidCommonID"), HiddenField).Value.Trim)
                        CommonDetails.CodeGroup = CType(gdvLocateCode.Rows(e.RowIndex).FindControl("hidCodeGroup"), HiddenField).Value.Trim
                        CommonDetails.CodeID = CType(gdvLocateCode.Rows(e.RowIndex).FindControl("lblDisplayCodeID"), Label).Text.Trim.ToUpper
                        CommonDetails.CodeDescription = CType(gdvLocateCode.Rows(e.RowIndex).FindControl("txtCodeDescription"), TextBox).Text.Trim
                        CommonDetails.Status = CType(gdvLocateCode.Rows(e.RowIndex).FindControl("ddlStatus"), DropDownList).SelectedValue.Trim
                        CommonDetails.LoginUser = Session("UserID")

                        lblErrSaveCode.Text = Client.UpdateCommon(CommonDetails)
                        Client.Close()

                        If lblErrSaveCode.Text = String.Empty Then

                            Dim Script As String = "ShowSuccessMessage('" & clsCommonFunction.GetMessage( _
                                        clsCommonFunction.messageID.Success, "saved", "Code") & "');"

                            ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)

                            gdvLocateCode.EditIndex = -1
                            btnGo_Click(sender, e)

                        Else
                            lblErrSaveCode.Visible = True
                        End If

                    End If

                Case Else

                    If CType(gdvLocateCode.Rows(e.RowIndex).FindControl("txtCodeID"), TextBox).Text.Trim = String.Empty _
                        Or CType(gdvLocateCode.Rows(e.RowIndex).FindControl("txtCodeDescription"), TextBox).Text = String.Empty Then

                        lblErrSaveCode.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.MandatoryField)
                        lblErrSaveCode.Visible = True

                        Dim Script As String = "ShowAlertMessage('" & lblErrSaveCode.Text & "');"
                        ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

                    Else

                        Dim Client As New ServiceClient
                        Dim CommonDetails As New CommonDetails

                        CommonDetails.StoreID = Session("StoreID")
                        CommonDetails.CommonID = CInt(CType(gdvLocateCode.Rows(e.RowIndex).FindControl("hidCommonID"), HiddenField).Value.Trim)
                        CommonDetails.CodeGroup = CType(gdvLocateCode.Rows(e.RowIndex).FindControl("hidCodeGroup"), HiddenField).Value.Trim
                        CommonDetails.CodeID = CType(gdvLocateCode.Rows(e.RowIndex).FindControl("txtCodeID"), TextBox).Text.Trim.ToUpper
                        CommonDetails.CodeDescription = CType(gdvLocateCode.Rows(e.RowIndex).FindControl("txtCodeDescription"), TextBox).Text.Trim
                        CommonDetails.Status = CType(gdvLocateCode.Rows(e.RowIndex).FindControl("ddlStatus"), DropDownList).SelectedValue.Trim
                        CommonDetails.LoginUser = Session("UserID")

                        lblErrSaveCode.Text = Client.UpdateCommon(CommonDetails)
                        Client.Close()

                        If lblErrSaveCode.Text = String.Empty Then
                            Dim Script As String = "ShowSuccessMessage('" & clsCommonFunction.GetMessage( _
                                        clsCommonFunction.messageID.Success, "saved", "Code") & "');"

                            ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)


                            '-- UPDATE CACHE
                            EditCache(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), GetType(CommonDetails), CommonDetails, True)

                            gdvLocateCode.EditIndex = -1
                            btnGo_Click(sender, e)

                        Else

                            lblErrSaveCode.Visible = True

                            Dim Script As String = "ShowAlertMessage('" & lblErrSaveCode.Text & "');"
                            ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

                        End If

                    End If
            End Select


        Catch ex As FaultException

            lblErrSaveCode.Text = ex.Message
            lblErrSaveCode.Visible = True

        Catch ex As Exception
            lblErrSaveCode.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            lblErrSaveCode.Visible = True
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If

        End Try

    End Sub

    ''' <summary>
    ''' btnAddNewCode - Click;
    ''' 06 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAddNewCode_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNewCode.Click

        lblErrAddCode.Visible = False
        pnlAddCode.Visible = True
        pnlCommonInfo.Visible = False
        divMsgBoxAddCode.Visible = False
        lblDisplayCodeGroup.Text = ddlCommonItems.SelectedItem.ToString
        txtAddCodeID.Text = String.Empty
        txtAddCodeDescription.Text = String.Empty

    End Sub

    ''' <summary>
    ''' btnAddCodeOK - Click;
    ''' 06 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAddCodeOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddCodeOK.Click

        divMsgBoxAddCode.Visible = False
        pnlCode.Visible = True
        pnlCommonInfo.Visible = True
        pnlAddCode.Visible = False

        btnGo_Click(sender, e)

    End Sub

    ''' <summary>
    ''' btnAddCodeSave - Click;
    ''' 06 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAddCodeSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddCodeSave.Click

        If txtAddCodeID.Text.Trim = String.Empty Or txtAddCodeDescription.Text = String.Empty Then
            lblErrAddCode.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.MandatoryField)
            lblErrAddCode.Visible = True

            Dim Script As String = "ShowAlertMessage('" & lblErrAddCode.Text & "');"
            ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

        Else

            Try

                Dim Client As New ServiceClient
                Dim CommonDetails As New CommonDetails

                CommonDetails.StoreID = Session("StoreID")
                CommonDetails.CodeGroup = lblDisplayCodeGroup.Text.Trim
                CommonDetails.CodeID = txtAddCodeID.Text.Trim.ToUpper
                CommonDetails.Status = OPEN
                CommonDetails.LoginUser = Session("UserID").ToString
                CommonDetails.CodeDescription = txtAddCodeDescription.Text

                lblErrAddCode.Text = Client.AddCommon(CommonDetails)
                Client.Close()

                If lblErrAddCode.Text = String.Empty Then

                    lblMsgCodeGroup.Text = lblDisplayCodeGroup.Text
                    lblMsgCodeID.Text = txtAddCodeID.Text.ToUpper
                    lblMsgCodeDescription.Text = txtAddCodeDescription.Text

                    divMsgBoxAddCode.Visible = True
                    pnlCode.Visible = False
                    pnlCommonInfo.Visible = False
                    pnlAddCode.Visible = False

                    '-- ADD TO CACHE
                    EditCache(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), GetType(CommonDetails), CommonDetails)

                Else
                    lblErrAddCode.Visible = True

                    Dim Script As String = "ShowAlertMessage('" & lblErrAddCode.Text & "');"
                    ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

                End If


            Catch ex As FaultException
                Dim fault As ServiceFault = ex.Data

                lblErrAddCode.Text = fault.MessageText
                lblErrAddCode.Visible = True

            Catch ex As Exception
                lblErrAddCode.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
                lblErrAddCode.Visible = True
                Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
                If (rethrow) Then
                    Throw
                End If

            End Try
        End If

    End Sub

    ''' <summary>
    ''' btnCancel - Click;
    ''' 06 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        lblErrAddCode.Visible = False
        pnlCommonInfo.Visible = False
        pnlAddCode.Visible = False
        divMsgBoxAddCode.Visible = False

    End Sub

    ''' <summary>
    ''' btnAddCodeCancel - Click;
    ''' 06 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAddCodeCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddCodeCancel.Click

        lblErrAddCode.Visible = False
        lblErrSaveCode.Visible = False
        pnlAddCode.Visible = False
        divMsgBoxAddCode.Visible = False
        pnlCommonInfo.Visible = True

    End Sub

    ''' <summary>
    ''' btnAddCodeClear - Click;
    ''' 06 Jan 2009 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAddCodeClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddCodeClear.Click

        lblErrAddCode.Visible = False
        lblErrSaveCode.Visible = False
        txtAddCodeID.Text = String.Empty
        txtAddCodeDescription.Text = String.Empty

    End Sub

#End Region

End Class