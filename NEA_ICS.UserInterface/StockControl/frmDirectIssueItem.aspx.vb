Imports Microsoft.Reporting.WebForms
Imports System.Web.Services.Description
Imports NEA_ICS.UserInterface.NEA_ICS.WcfService
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ServiceModel
Imports DBauer.Web.UI.WebControls
Imports System.Web.Services
Imports System.Reflection

''' <summary>
''' Code Behind for frmDirectIssueItem;
''' 12 Feb 09 - Jianfa;
''' </summary>
''' <remarks></remarks>
Partial Public Class frmDirectIssueItem
    Inherits clsCommonFunction

#Region " PAGE LOAD "
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

    ''' <summary>
    ''' Page Load;
    ''' 12 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim AccessRights As New List(Of RoleDetails)

        CheckValidSession()

        If Not Page.IsPostBack Then

            AccessRights = clsCommonFunction.AssignAccessRights(Cache(Session(ESession.UserID.ToString) & Session(ESession.StoreID.ToString) & "AccessRights"), _
                                                   clsCommonFunction.moduleID.DirectIssue)

            If AccessRights(0).SelectRight = False Then
                Server.Transfer("..\frmUnauthorisedPage.aspx")
                Exit Sub
            End If

            If AccessRights(0).InsertRight = False Then
                tbpNew.Visible = False
            End If

            If AccessRights(0).UpdateRight = False Then
                lbtnLocateEdit.Visible = False
            End If

            If AccessRights(0).DeleteRight = False Then
                lbtnLocateDel.Visible = False
            End If

            BindDocumentType()
            BindConsumerCode()
            BindDirectIssueID()
            BindDirectIssueInfo()

            txtPODateFrom.Text = Today.AddMonths(-1).ToString("01/MM/yyyy")
            txtPODateTo.Text = ConvertToDate(Today.ToString("01/MM/yyyy")).AddDays(-1).ToString("dd/MM/yyyy")

        End If

    End Sub
#End Region

#Region " NEW Tab "
    ''' <summary>
    ''' btnAddDirectIssue - Click;
    ''' 12 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' </remarks>
    Private Sub btnAddDirectIssue_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDirectIssue.Click

        Try

            If ddlDocType.SelectedIndex <= 0 Or _
            txtDateIssue.Text.Trim = String.Empty Or ddlConsumerCode.SelectedIndex <= 0 Then
                lblErrAddDirectIssue.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.MandatoryField)
                lblErrAddDirectIssue.Visible = True

                '-- UAT.02.16 - Include popup screen
                Message = lblErrAddDirectIssue.Text

                Exit Sub
            End If

            lblErrAddDirectIssue.Visible = False
            btnAddDirectIssueItem_Click(sender, e)
            btnAddDirectIssue.Visible = False
            pnlNewIssue.Visible = True

            ddlDocType.Enabled = False
            txtSerialNo.Enabled = False
            txtDateIssue.Enabled = False
            ddlConsumerCode.Enabled = False

        Catch ex As Exception
            lblErrAddDirectIssue.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

    ''' <summary>
    ''' btnAddDirectIssueItem - Click;
    ''' 12 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAddDirectIssueItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDirectIssueItem.Click

        Try

            BindUOM()

            Dim DirectIssueItem = New DirectIssueItem

            If ViewState("_UserControl") Is Nothing Then
                ViewState("_UserControl") = 1
            Else
                ViewState("_UserControl") += 1
            End If

            DirectIssueItem = LoadControl("DirectIssueItem.ascx")
            DirectIssueItem.ID = ViewState("_UserControl")

            DCP.Controls.Add(DirectIssueItem)

            DirectCast(DirectIssueItem.FindControl("ddlUOM"), DropDownList).DataSource = Cache(Session(ESession.StoreID.ToString) & "UOM")
            DirectCast(DirectIssueItem.FindControl("ddlUOM"), DropDownList).DataValueField = "CommonCodeID"
            DirectCast(DirectIssueItem.FindControl("ddlUOM"), DropDownList).DataTextField = "CommonCodeDescription"
            DirectCast(DirectIssueItem.FindControl("ddlUOM"), DropDownList).DataBind()
            DirectCast(DirectIssueItem.FindControl("ddlUOM"), DropDownList).Items.Insert(0, New ListItem(" - Please Select - ", String.Empty))

        Catch ex As FaultException
            lblErrAddDirectIssue.Text = ex.Message

        Catch ex As Exception
            lblErrAddDirectIssue.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try

    End Sub

    ''' <summary>
    ''' Friend Sub - CancelDirectIssue;
    ''' 12 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Friend Sub CancelDirectIssue(ByVal sender As Object, ByVal e As EventArgs)

        Try

            Dim DirectIssueItem As New DirectIssueItem
            DirectIssueItem = DirectCast(FindControl(sender), DirectIssueItem)

            If tbcIssueItem.ActiveTab.ID = NEWTAB Then
                DCP.Controls.Remove(FindControl(sender))
            ElseIf tbcIssueItem.ActiveTab.ID = LOCATETAB Then
                If DirectCast(DirectIssueItem.FindControl("hidMode"), HiddenField).Value = INSERT Then
                    DCPEdit.Controls.Remove(FindControl(sender))
                Else
                    DirectCast(DirectIssueItem.FindControl("hidMode"), HiddenField).Value = DELETE
                    DirectIssueItem.Visible = False
                End If
            End If

        Catch ex As Exception
            lblErrAddDirectIssue.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try

    End Sub

    ''' <summary>
    ''' Sub Proc - UpdateGTotal;
    ''' 12 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="Value"></param>
    ''' <remarks></remarks>
    Protected Friend Sub UpdateGTotal(Optional ByVal value As Decimal = 0.0, _
                                      Optional ByVal originalValue As Decimal = 0.0)

        Try
            Dim Total As Double = 0.0
            If tbcIssueItem.ActiveTab.ID = NEWTAB Then
                If IsNumeric(lblGTotal.Text) Then Total = Convert.ToDecimal(lblGTotal.Text)
                lblGTotal.Text = (Total + value).ToString("0.0000")
            ElseIf tbcIssueItem.ActiveTab.ID = LOCATETAB Then

                If IsNumeric(lblEditGTotal.Text) Then Total = Convert.ToDecimal(lblEditGTotal.Text)
                lblEditGTotal.Text = (Total + (value - originalValue)).ToString("0.0000")

            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    ''' <summary>
    ''' btnAddDirectIssueOK - Click;
    ''' 13 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAddDirectIssueOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDirectIssueOK.Click

        ddlDocType.Enabled = True
        txtSerialNo.Enabled = True
        txtDateIssue.Enabled = True
        ddlConsumerCode.Enabled = True

        divMsgBox.Visible = False
        pnlNewIssue.Visible = False
        pnlMain.Visible = True

        btnAddDirectIssue.Visible = True
        BindDirectIssueID()

    End Sub

    ''' <summary>
    ''' btnAddDirectIssueCancel - Click;
    ''' 13 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAddDirectIssueCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDirectIssueCancel.Click

        btnAddDirectIssue.Visible = True
        pnlMain.Enabled = True
        pnlNewIssue.Visible = False
        divMsgBox.Visible = False

        ddlDocType.Enabled = True
        txtSerialNo.Enabled = True
        txtDateIssue.Enabled = True
        ddlConsumerCode.Enabled = True

        DCP.Controls.Clear()
        DCPEdit.Controls.Clear()

    End Sub

    ''' <summary>
    ''' btnAddDirectIssueSave - Click;
    ''' 13 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE REQ:
    ''' 1) To handle Document No via Business Layer
    ''' </remarks>
    Private Sub btnAddDirectIssueSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDirectIssueSave.Click

        Dim ItemToInsert As Boolean = False
        Dim DirectIssueDetails As DirectIssueDetails = Nothing
        Dim DirectIssueDocNo As String = String.Empty

        Try

            Dim Client As New ServiceClient
            Dim DirectIssueItemList As New List(Of DirectIssueDetails)

            '-- VALIDATION CHECKS 
            If ddlDocType.SelectedIndex <= 0 Or _
                txtDateIssue.Text.Trim = String.Empty Or ddlConsumerCode.SelectedIndex <= 0 Then

                lblErrSaveDirectIssue.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.MandatoryField)
                lblErrSaveDirectIssue.Visible = True

                '-- UAT.02.16 - Include popup screen
                Message = lblErrSaveDirectIssue.Text

                Exit Sub

            End If

            If clsCommonFunction.ConvertToDate(txtDateIssue.Text) = DateTime.MinValue Then
                lblErrSaveDirectIssue.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.NotIsDate)
                lblErrSaveDirectIssue.Visible = True

                '-- UAT.02.16 - Include popup screen
                Message = lblErrSaveDirectIssue.Text

                Exit Sub
            End If

            For idx As Integer = 1 To ViewState("_UserControl")

                DirectIssueDetails = New DirectIssueDetails
                Dim DirectIssueItem As New DirectIssueItem
                DirectIssueItem = TryCast(DCP.FindControl(idx.ToString), DirectIssueItem)

                If Not DirectIssueItem Is Nothing Then

                    DirectIssueDetails.StoreID = Session("StoreID").ToString
                    DirectIssueDetails.ConsumerID = ddlConsumerCode.SelectedValue
                    DirectIssueDetails.IssueType = ddlDocType.SelectedValue
                    DirectIssueDetails.SerialNo = txtSerialNo.Text.Trim
                    DirectIssueDetails.DirectIssueDate = clsCommonFunction.ConvertToDate(txtDateIssue.Text)
                    DirectIssueDetails.Status = CLOSED
                    DirectIssueDetails.LoginUser = Session("UserID").ToString

                    Dim Value As String

                    Value = CType(DirectIssueItem.FindControl("lblStockCode"), Label).Text.Trim
                    DirectIssueDetails.ItemID = Value

                    Value = CType(DirectIssueItem.FindControl("lblStockType"), Label).Text.Trim
                    DirectIssueDetails.StockType = Value

                    Value = CType(DirectIssueItem.FindControl("txtDescription"), TextBox).Text.Trim
                    If Value = String.Empty Then
                        lblErrSaveDirectIssue.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.MandatoryField)
                        lblErrSaveDirectIssue.Visible = True

                        '-- UAT.02.16 - Include popup screen
                        Message = lblErrSaveDirectIssue.Text

                        Exit Sub
                    End If
                    DirectIssueDetails.ItemDescription = Value

                    Value = CType(DirectIssueItem.FindControl("ddlUOM"), DropDownList).SelectedValue
                    If Value = String.Empty Then
                        lblErrSaveDirectIssue.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.MandatoryField)
                        lblErrSaveDirectIssue.Visible = True

                        '-- UAT.02.16 - Include popup screen
                        Message = lblErrSaveDirectIssue.Text

                        Exit Sub
                    End If
                    DirectIssueDetails.UOM = Value

                    Value = CType(DirectIssueItem.FindControl("txtQtyIssued"), TextBox).Text.Trim
                    DirectIssueDetails.ItemQty = IIf(Value = String.Empty, 0.0, CDec(Value))

                    Value = CType(DirectIssueItem.FindControl("txtTotalCost"), TextBox).Text.Trim
                    DirectIssueDetails.TotalCost = IIf(Value = String.Empty, 0.0, CDec(Value))

                    Value = CType(DirectIssueItem.FindControl("txtRemarks"), TextBox).Text.Trim
                    DirectIssueDetails.Remarks = Value

                    DirectIssueItemList.Add(DirectIssueDetails)

                    ItemToInsert = True

                End If

            Next

            If Not ItemToInsert Then

                lblErrSaveDirectIssue.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.AtLeastOneItemSelected, "stock item", "Direct Issue")
                lblErrSaveDirectIssue.Visible = True

                Exit Sub

            Else

                lblErrSaveDirectIssue.Text = Client.AddDirectIssue(DirectIssueDetails, DirectIssueItemList, DirectIssueDocNo)
                Client.Close()

                If lblErrSaveDirectIssue.Text = String.Empty Then

                    lblErrSaveDirectIssue.Visible = False

                    Dim Script As String = "ShowSuccessMessage('" & clsCommonFunction.GetMessage( _
                                            clsCommonFunction.messageID.Success, "added", "Direct Issue") & "\nYour Direct Issue Document No is " & _
                                            DirectIssueDocNo & ".')"

                    ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)


                    DCP.Controls.Clear()

                    BindDirectIssueID()
                    BindDirectIssueInfo()

                    lblGTotal.Text = "0.0000"

                    btnAddDirectIssueCancel_Click(sender, e)

                    Exit Sub

                Else

                    Dim Script As String = "ShowAlertMessage('" & lblErrSaveDirectIssue.Text & "');"
                    ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)

                    lblErrSaveDirectIssue.Visible = False

                End If

            End If

        Catch ex As FaultException

            lblErrSaveDirectIssue.Text = ex.Message
            lblErrSaveDirectIssue.Visible = True

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

#End Region

#Region " LOCATE Tab "

    ''' <summary>
    ''' btnLocateGo - Click;
    ''' 14 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnLocateGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLocateGo.Click

        Dim idx As Integer = 0
        Dim GTotal As Decimal = 0.0

        If ViewState("_UserControlEdit") Is Nothing Then
            ViewState("_UserControlEdit") = 1
        Else
            '-- Reset to 1
            ViewState("_UserControlEdit") = 1
        End If

        If ddlLocateIssueRefNo.SelectedIndex <= 0 And txtLocateSerialNo.Text = String.Empty Then

            lblErrLocateDirectIssue.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.AtLeastOneItemSelected, "input field", "Direct Issue")
            lblErrLocateDirectIssue.Visible = True

            Message = lblErrLocateDirectIssue.Text

            Exit Sub

        End If

        Try

            Dim Client As New ServiceClient
            Dim DirectIssueDetails As New DirectIssueDetails
            Dim DirectIssueList As New List(Of DirectIssueDetails)
            Dim DirectIssueItemsList As New List(Of DirectIssueDetails)

            DirectIssueDetails.StoreID = Session("StoreID")
            DirectIssueDetails.DirectIssueID = ddlLocateIssueRefNo.SelectedValue
            DirectIssueDetails.SerialNo = txtLocateSerialNo.Text

            DirectIssueList = Client.GetDirectIssues(DirectIssueDetails)
            DirectIssueItemsList = Client.GetDirectIssueItems(DirectIssueDetails)

            Client.Close()

            If DirectIssueList.Count = 0 Then

                lblErrLocateDirectIssue.Text = "No record found."
                lblErrLocateDirectIssue.Visible = True

                Message = lblErrLocateDirectIssue.Text

                Exit Sub

            Else

                DCPEdit.Controls.Clear()
                BindUOM()

                lblEditIssueRefNo.Text = ddlLocateIssueRefNo.SelectedItem.Text
                hidEditIssueRefNo.Value = ddlLocateIssueRefNo.SelectedValue
                ddlEditDocType.SelectedValue = DirectIssueList(0).IssueType
                lblEditSerialNo.Text = DirectIssueList(0).SerialNo
                txtEditDateIssue.Text = DirectIssueList(0).DirectIssueDate.ToString("dd/MM/yyyy")
                ddlEditConsumerCode.SelectedValue = DirectIssueList(0).ConsumerID

                For Each item As DirectIssueDetails In DirectIssueItemsList

                    Dim DirectIssueItem = New DirectIssueItem

                    idx += 1

                    DirectIssueItem = LoadControl("DirectIssueItem.ascx")
                    DirectIssueItem.ID = idx.ToString

                    DCPEdit.Controls.Add(DirectIssueItem)

                    DirectCast(DirectIssueItem.FindControl("ddlUOM"), DropDownList).DataSource = Cache(Session(ESession.StoreID.ToString) & "UOM")
                    DirectCast(DirectIssueItem.FindControl("ddlUOM"), DropDownList).DataValueField = "CommonCodeID"
                    DirectCast(DirectIssueItem.FindControl("ddlUOM"), DropDownList).DataTextField = "CommonCodeDescription"
                    DirectCast(DirectIssueItem.FindControl("ddlUOM"), DropDownList).DataBind()
                    DirectCast(DirectIssueItem.FindControl("ddlUOM"), DropDownList).Items.Insert(0, New ListItem(" - Please Select - ", String.Empty))

                    DirectCast(DirectIssueItem.FindControl("hidStockCode"), HiddenField).Value = item.ItemID
                    DirectCast(DirectIssueItem.FindControl("txtDescription"), TextBox).Text = item.ItemDescription
                    DirectCast(DirectIssueItem.FindControl("hidDescription"), HiddenField).Value = item.ItemDescription
                    DirectCast(DirectIssueItem.FindControl("txtRemarks"), TextBox).Text = item.Remarks
                    DirectCast(DirectIssueItem.FindControl("hidRemarks"), HiddenField).Value = item.Remarks
                    DirectCast(DirectIssueItem.FindControl("txtQtyIssued"), TextBox).Text = item.ItemQty
                    DirectCast(DirectIssueItem.FindControl("hidQtyIssued"), HiddenField).Value = item.ItemQty
                    DirectCast(DirectIssueItem.FindControl("ddlUOM"), DropDownList).SelectedValue = item.UOM
                    DirectCast(DirectIssueItem.FindControl("hidUOM"), HiddenField).Value = item.UOM
                    DirectCast(DirectIssueItem.FindControl("txtTotalCost"), TextBox).Text = item.TotalCost
                    DirectCast(DirectIssueItem.FindControl("hidTotalCost"), HiddenField).Value = item.TotalCost
                    DirectCast(DirectIssueItem.FindControl("hidMode"), HiddenField).Value = UPDATE

                    GTotal += item.TotalCost

                    '-- EDIT MODE (FALSE)
                    DirectCast(DirectIssueItem.FindControl("txtDescription"), TextBox).Enabled = False
                    DirectCast(DirectIssueItem.FindControl("txtRemarks"), TextBox).Enabled = False
                    DirectCast(DirectIssueItem.FindControl("txtQtyIssued"), TextBox).Enabled = False
                    DirectCast(DirectIssueItem.FindControl("ddlUOM"), DropDownList).Enabled = False
                    DirectCast(DirectIssueItem.FindControl("txtTotalCost"), TextBox).Enabled = False
                    DirectCast(DirectIssueItem.FindControl("lbtnCancel"), LinkButton).Enabled = False

                Next

                ViewState("_UserControlEdit") = idx
                lblEditGTotal.Text = GTotal.ToString("0.0000")

                EditMode(False)
                lblErrLocateDirectIssue.Visible = False
                pnlEditIssue.Visible = True

            End If

        Catch ex As FaultException

            lblErrLocateDirectIssue.Text = ex.Message
            lblErrLocateDirectIssue.Visible = True

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

    ''' <summary>
    ''' btnLocateClear - Click;
    ''' 15 Feb 09 - Click;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnLocateClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLocateClear.Click

        EditMode(True)
        pnlEditIssue.Visible = False
        ddlLocateIssueRefNo.SelectedIndex = -1
        txtLocateSerialNo.Text = String.Empty
        lblErrLocateDirectIssue.Visible = False

    End Sub

    ''' <summary>
    ''' lbtnLocateDel - Click;
    ''' 15 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub lbtnLocateDel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnLocateDel.Click

        Try

            Dim Client As New ServiceClient
            Dim DirectIssueDetails As New DirectIssueDetails

            DirectIssueDetails.StoreID = Session("StoreID")
            DirectIssueDetails.DirectIssueID = hidEditIssueRefNo.Value

            lblErrEditSaveDirectIssue.Text = Client.DeleteDirectIssue(DirectIssueDetails)
            Client.Close()

            If lblErrEditSaveDirectIssue.Text = String.Empty Then

                lblErrEditSaveDirectIssue.Visible = False

                Dim Script As String = "ShowSuccessMessage('" & clsCommonFunction.GetMessage( _
                                        clsCommonFunction.messageID.Success, "deleted", "Direct Issue") & "');"

                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)

                pnlEditIssue.Visible = False
                BindDirectIssueID()

            Else
                lblErrEditSaveDirectIssue.Visible = True
            End If

        Catch ex As FaultException
            Dim fault As ServiceFault = ex.Data

            lblErrEditSaveDirectIssue.Text = fault.MessageText
            lblErrEditSaveDirectIssue.Visible = True

        Catch ex As Exception
            lblErrEditSaveDirectIssue.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            lblErrEditSaveDirectIssue.Visible = True
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If

        End Try
    End Sub

    ''' <summary>
    ''' btnLocateCancel - Click;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnLocateCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLocateCancel.Click

        DCPEdit.Controls.Clear()
        pnlEditIssue.Visible = False
        EditMode(False)

    End Sub

    ''' <summary>
    ''' lbtnLocateEdit - Click;
    ''' 18 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub lbtnLocateEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnLocateEdit.Click

        EditMode(True)

        For idx As Integer = 1 To ViewState("_UserControlEdit")

            Dim DirectIssueItem As New DirectIssueItem
            DirectIssueItem = TryCast(DCPEdit.FindControl(idx.ToString), DirectIssueItem)

            If Not DirectIssueItem Is Nothing Then

                DirectCast(DirectIssueItem.FindControl("txtDescription"), TextBox).Enabled = True
                DirectCast(DirectIssueItem.FindControl("txtRemarks"), TextBox).Enabled = True
                DirectCast(DirectIssueItem.FindControl("txtQtyIssued"), TextBox).Enabled = True
                DirectCast(DirectIssueItem.FindControl("ddlUOM"), DropDownList).Enabled = True
                DirectCast(DirectIssueItem.FindControl("txtTotalCost"), TextBox).Enabled = True
                DirectCast(DirectIssueItem.FindControl("lbtnCancel"), LinkButton).Enabled = True

            End If

        Next

    End Sub

    ''' <summary>
    ''' btnLocateAddDirectIssueItem - Click;
    ''' 18 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnLocateAddDirectIssueItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLocateAddDirectIssueItem.Click

        Try

            BindUOM()

            Dim DirectIssueItem = New DirectIssueItem

            If ViewState("_UserControlEdit") Is Nothing Then
                ViewState("_UserControlEdit") = 1
            Else
                ViewState("_UserControlEdit") += 1
            End If

            DirectIssueItem = LoadControl("DirectIssueItem.ascx")
            DirectIssueItem.ID = ViewState("_UserControlEdit")

            DCPEdit.Controls.Add(DirectIssueItem)

            DirectCast(DirectIssueItem.FindControl("ddlUOM"), DropDownList).DataSource = Cache(Session(ESession.StoreID.ToString) & "UOM")
            DirectCast(DirectIssueItem.FindControl("ddlUOM"), DropDownList).DataValueField = "CommonCodeID"
            DirectCast(DirectIssueItem.FindControl("ddlUOM"), DropDownList).DataTextField = "CommonCodeDescription"
            DirectCast(DirectIssueItem.FindControl("ddlUOM"), DropDownList).DataBind()
            DirectCast(DirectIssueItem.FindControl("ddlUOM"), DropDownList).Items.Insert(0, New ListItem(" - Please Select - ", String.Empty))

            DirectCast(DirectIssueItem.FindControl("hidMode"), HiddenField).Value = INSERT

        Catch ex As FaultException
            lblErrAddDirectIssue.Text = ex.Message

        Catch ex As Exception
            lblErrAddDirectIssue.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try


    End Sub

    ''' <summary>
    ''' btnLocateSave - Click;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnLocateSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLocateSave.Click

        Dim DirectIssueDetails As DirectIssueDetails = Nothing

        Try

            Dim Client As New ServiceClient
            Dim DirectIssueItemList As New List(Of DirectIssueDetails)

            '-- VALIDATION CHECKS 
            If ddlEditDocType.SelectedIndex <= 0 Or _
                txtEditDateIssue.Text.Trim = String.Empty Or ddlEditConsumerCode.SelectedIndex <= 0 Then

                lblErrLocateSaveDirectIssue.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.MandatoryField)
                lblErrLocateSaveDirectIssue.Visible = True

                '-- UAT.02.16 - Include popup screen
                Message = lblErrLocateSaveDirectIssue.Text

                Exit Sub

            End If

            If clsCommonFunction.ConvertToDate(txtEditDateIssue.Text) = DateTime.MinValue Then
                lblErrLocateSaveDirectIssue.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.NotIsDate)
                lblErrLocateSaveDirectIssue.Visible = True
                Exit Sub
            End If

            For idx As Integer = 1 To ViewState("_UserControlEdit")

                DirectIssueDetails = New DirectIssueDetails
                Dim DirectIssueItem As New DirectIssueItem
                DirectIssueItem = TryCast(DCPEdit.FindControl(idx.ToString), DirectIssueItem)

                If Not DirectIssueItem Is Nothing Then

                    DirectIssueDetails.StoreID = Session("StoreID").ToString
                    DirectIssueDetails.DirectIssueID = hidEditIssueRefNo.Value
                    DirectIssueDetails.ConsumerID = ddlEditConsumerCode.SelectedValue
                    DirectIssueDetails.IssueType = ddlEditDocType.SelectedValue
                    DirectIssueDetails.DocumentNo = lblEditIssueRefNo.Text
                    DirectIssueDetails.SerialNo = lblEditSerialNo.Text
                    DirectIssueDetails.DirectIssueDate = clsCommonFunction.ConvertToDate(txtEditDateIssue.Text)
                    DirectIssueDetails.Status = CLOSED
                    DirectIssueDetails.LoginUser = Session("UserID").ToString

                    Dim Value As String

                    Value = CType(DirectIssueItem.FindControl("hidStockCode"), HiddenField).Value.Trim
                    DirectIssueDetails.ItemID = Value

                    Value = CType(DirectIssueItem.FindControl("lblStockType"), Label).Text.Trim
                    DirectIssueDetails.StockType = Value

                    Value = CType(DirectIssueItem.FindControl("txtDescription"), TextBox).Text.Trim
                    If Value = String.Empty Then
                        lblErrLocateSaveDirectIssue.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.MandatoryField)
                        lblErrLocateSaveDirectIssue.Visible = True

                        '-- UAT.02.16 - Include popup screen
                        Message = lblErrLocateSaveDirectIssue.Text

                        Exit Sub
                    End If
                    DirectIssueDetails.ItemDescription = Value

                    Value = CType(DirectIssueItem.FindControl("ddlUOM"), DropDownList).SelectedValue
                    If Value = String.Empty Then
                        lblErrLocateSaveDirectIssue.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.MandatoryField)
                        lblErrLocateSaveDirectIssue.Visible = True

                        '-- UAT.02.16 - Include popup screen
                        Message = lblErrLocateSaveDirectIssue.Text

                        Exit Sub
                    End If
                    DirectIssueDetails.UOM = Value

                    Value = CType(DirectIssueItem.FindControl("txtQtyIssued"), TextBox).Text.Trim
                    DirectIssueDetails.ItemQty = IIf(Value = String.Empty, 0.0, CDec(Value))

                    Value = CType(DirectIssueItem.FindControl("txtTotalCost"), TextBox).Text.Trim
                    DirectIssueDetails.TotalCost = IIf(Value = String.Empty, 0.0, CDec(Value))

                    Value = CType(DirectIssueItem.FindControl("txtRemarks"), TextBox).Text.Trim
                    DirectIssueDetails.Remarks = Value

                    Value = CType(DirectIssueItem.FindControl("hidMode"), HiddenField).Value
                    DirectIssueDetails.Mode = Value

                    DirectIssueItemList.Add(DirectIssueDetails)

                End If

            Next

            lblErrLocateSaveDirectIssue.Text = Client.UpdateDirectIssue(DirectIssueDetails, DirectIssueItemList)
            Client.Close()

            If lblErrLocateSaveDirectIssue.Text = String.Empty Then

                lblErrLocateSaveDirectIssue.Visible = False

                Dim Script As String = "ShowSuccessMessage('" & clsCommonFunction.GetMessage( _
                                            clsCommonFunction.messageID.Success, "saved", "Direct Issue") & "');"

                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)


                lblEditGTotal.Text = "0.0000"

                DCPEdit.Controls.Clear()
                pnlEditIssue.Visible = False
                Page_Load(sender, e)
                tbcIssueItem.ActiveTabIndex = 1
                EditMode(False)

                Exit Sub

            Else

                Dim Script As String = "ShowAlertMessage('" & lblErrLocateSaveDirectIssue.Text & "');"
                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)

                lblErrLocateSaveDirectIssue.Visible = False

            End If


        Catch ex As FaultException

            lblErrLocateSaveDirectIssue.Text = ex.Message
            lblErrLocateSaveDirectIssue.Visible = True

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub
#End Region

#Region " REPORT Tab "
    Protected Sub btnPDF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPDF.Click
        rvr.LocalReport.DataSources.Clear()
        rvr.LocalReport.DataSources.Add(New ReportDataSource("DirectIssueDetails", ObjectDataSource1))

        Dim parameterlist As New List(Of ReportParameter)
        Dim p2 As New ReportParameter("PODateFrom", txtPODateFrom.Text)
        Dim p3 As New ReportParameter("PODateTo", txtPODateTo.Text)
        Dim p6 As New ReportParameter("DocNo", ddlIssueReference.SelectedValue)
        Dim p7 As New ReportParameter("Store", Session("StoreName").ToString)
        parameterlist.Add(p2)
        parameterlist.Add(p3)
        parameterlist.Add(p6)
        parameterlist.Add(p7)

        rvr.LocalReport.SetParameters(parameterlist)
        rvr.LocalReport.Refresh()

        Dim bytValue As Byte()
        bytValue = rvr.LocalReport.Render("PDF", Nothing, "application/pdf", "", "pdf", Nothing, Nothing)

        'If NoRecordFond = "Y" Then
        '    Message = GetMessage(messageID.NoRecordFound)
        '    Exit Sub
        'End If

        Response.Buffer = True
        Response.Clear()
        Response.ContentType = "application/pdf"
        Response.AddHeader("content-disposition", "attachment;filename=DirectIssueList.pdf")
        Response.BinaryWrite(bytValue)
        Response.Flush()
        Response.End()

    End Sub

    Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExcel.Click
        rvr.LocalReport.DataSources.Clear()
        rvr.LocalReport.DataSources.Add(New ReportDataSource("DirectIssueDetails", ObjectDataSource1))

        Dim parameterlist As New List(Of ReportParameter)
        Dim p2 As New ReportParameter("PODateFrom", txtPODateFrom.Text)
        Dim p3 As New ReportParameter("PODateTo", txtPODateTo.Text)
        Dim p6 As New ReportParameter("DocNo", ddlIssueReference.SelectedValue)
        Dim p7 As New ReportParameter("Store", Session("StoreName").ToString)
        parameterlist.Add(p2)
        parameterlist.Add(p3)
        parameterlist.Add(p6)
        parameterlist.Add(p7)

        rvr.LocalReport.SetParameters(parameterlist)
        rvr.LocalReport.Refresh()

        Dim bytValue As Byte()
        bytValue = rvr.LocalReport.Render("Excel", Nothing, "application/ms-excel", "", "xls", Nothing, Nothing)

        'If NoRecordFond = "Y" Then
        '    Message = GetMessage(messageID.NoRecordFound)
        '    Exit Sub
        'End If

        Response.Buffer = True
        Response.Clear()
        Response.ContentType = "application/ms-excel"
        Response.AddHeader("content-disposition", "attachment;filename=DirectIssueList.xls")
        Response.BinaryWrite(bytValue)
        Response.Flush()
        Response.End()
    End Sub

    Protected Sub ObjectDataSource1_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs) Handles ObjectDataSource1.Selecting

        If ddlIssueReference.SelectedIndex > 0 Then

            e.InputParameters("storeID") = Session("StoreID")
            e.InputParameters("dteIssueFrom") = Date.MinValue
            e.InputParameters("dteIssueTo") = Date.MinValue
            e.InputParameters("docNo") = ddlIssueReference.SelectedItem.Text

        Else

            e.InputParameters("storeID") = Session("StoreID")
            e.InputParameters("dteIssueFrom") = DateTime.ParseExact(Me.txtPODateFrom.Text, "dd/MM/yyyy", Nothing)
            e.InputParameters("dteIssueTo") = DateTime.ParseExact(Me.txtPODateTo.Text, "dd/MM/yyyy", Nothing)
            e.InputParameters("docNo") = String.Empty

        End If


    End Sub
    Protected Sub ObjectDataSource1_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles ObjectDataSource1.Selected
        Dim returnList As List(Of DirectIssueDetails) = e.ReturnValue
        If returnList.Count <= 0 Then
            NoRecordFond = "Y"
        Else
            NoRecordFond = "N"
        End If
    End Sub
#End Region

#Region " Sub Procedures and Functions "

    ''' <summary>
    ''' Sub Proc - BindDocumentType;
    ''' 12 Feb 09 - Jianfa;
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindDocumentType()

        Dim View As DataView
        View = clsCommonFunction.GetCommonDataByCodeGroup(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), clsCommonFunction.codeGroup.DirectDocType, "O")

        ddlDocType.DataSource = View
        ddlDocType.DataValueField = "CommonCodeID"
        ddlDocType.DataTextField = "CommonCodeDescription"
        ddlDocType.DataBind()

        ddlDocType.Items.Insert(0, New ListItem(" - Please Select - ", ""))
        ddlDocType.SelectedIndex = 1

        ddlEditDocType.DataSource = View
        ddlEditDocType.DataValueField = "CommonCodeID"
        ddlEditDocType.DataTextField = "CommonCodeDescription"
        ddlEditDocType.DataBind()

        ddlEditDocType.Items.Insert(0, New ListItem(" - Please Select - ", ""))
        ddlEditDocType.SelectedIndex = 1

    End Sub

    ''' <summary>
    ''' Sub Proc - BindConsumerCode
    ''' 12 Feb 09 - Jianfa
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindConsumerCode()

        Try

            Dim Client As New ServiceClient
            Dim ConsumerDetails As New ConsumerDetails
            Dim ConsumerList As New List(Of ConsumerDetails)

            ConsumerDetails.StoreID = Session("StoreID")
            ConsumerDetails.ConsumerID = String.Empty
            ConsumerDetails.ConsumerDescription = String.Empty
            ConsumerDetails.ConsumerStatus = OPEN

            ConsumerList = Client.GetConsumers(ConsumerDetails, String.Empty, String.Empty)

            Client.Close()

            ddlConsumerCode.DataSource = ConsumerList
            ddlConsumerCode.DataValueField = "ConsumerID"
            ddlConsumerCode.DataTextField = "ConsumerID_Description"
            ddlConsumerCode.DataBind()

            ddlConsumerCode.Items.Insert(0, New ListItem(" - Please Select - ", String.Empty))

            ddlEditConsumerCode.DataSource = ConsumerList
            ddlEditConsumerCode.DataValueField = "ConsumerID"
            ddlEditConsumerCode.DataTextField = "ConsumerID_Description"
            ddlEditConsumerCode.DataBind()

            ddlEditConsumerCode.Items.Insert(0, New ListItem(" - Please Select - ", String.Empty))

        Catch ex As FaultException

            lblErrAddDirectIssue.Text = ex.Message
            lblErrAddDirectIssue.Visible = True

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

    ''' <summary>
    ''' Sub Proc - BindUOM()
    ''' 12 Feb 09 - Jianfa;
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindUOM()

        Dim View As DataView
        View = clsCommonFunction.GetCommonDataByCodeGroup(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), clsCommonFunction.codeGroup.UOM, OPEN)

        Cache(Session(ESession.StoreID.ToString) & "UOM") = View

    End Sub

    ''' <summary>
    ''' Sub Proc - BindDirectIssueID
    ''' 14 Feb 09 - Jianfa;
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindDirectIssueID()

        Try

            Dim Client As New ServiceClient()
            Dim DirectIssueDetails As New DirectIssueDetails
            Dim DirectIssueList As New List(Of DirectIssueDetails)

            DirectIssueDetails.StoreID = Session("StoreID")

            DirectIssueList = Client.GetDirectIssueID(DirectIssueDetails)
            Client.Close()

            ddlLocateIssueRefNo.DataSource = DirectIssueList
            ddlLocateIssueRefNo.DataValueField = "DirectIssueID"
            ddlLocateIssueRefNo.DataTextField = "DocumentNo"
            ddlLocateIssueRefNo.DataBind()

            ddlLocateIssueRefNo.Items.Insert(0, New ListItem(" - Please Select - ", String.Empty))

            ddlIssueReference.DataSource = DirectIssueList
            ddlIssueReference.DataValueField = "DirectIssueID"
            ddlIssueReference.DataTextField = "DocumentNo"
            ddlIssueReference.DataBind()

            ddlIssueReference.Items.Insert(0, New ListItem(" - Please Select - ", String.Empty))

        Catch ex As FaultException

            lblErrLocateDirectIssue.Text = ex.Message
            lblErrLocateDirectIssue.Visible = True

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

    ''' <summary>
    ''' Sub Proc - EditMode;
    ''' 15 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="Mode"></param>
    ''' <remarks></remarks>
    Private Sub EditMode(ByVal Mode As Boolean)

        ddlEditDocType.Enabled = Mode
        txtEditDateIssue.Enabled = Mode
        ddlEditConsumerCode.Enabled = Mode
        btnLocateAddDirectIssueItem.Enabled = Mode
        btnLocateSave.Enabled = Mode

    End Sub

    ''' <summary>
    ''' Sub Proc - BindDirectIssueInfo;
    ''' 18 Feb 09 - Jianfa;
    ''' </summary>
    ''' <remarks>
    ''' CHANGE REQ:
    ''' 1) TO use 'GetLastSerialNo' from WCFService instead
    ''' 2) TO eliminate Document No
    ''' </remarks>
    Private Sub BindDirectIssueInfo()

        Try

            Dim Client As New ServiceClient
            Dim DirectIssueDetails As New DirectIssueDetails
            Dim DirectIssueList As New List(Of DirectIssueDetails)

            'DirectIssueDetails.StoreID = Session("StoreID")
            'DirectIssueList = Client.GetDirectIssueInfo(DirectIssueDetails)
            lblLastSerialNo.Text = Client.GetLastSerialNo(Session("StoreID"), ServiceModuleName.DirectIssue)

            Client.Close()

            '----------------------------------------------------------------------------------------------------------------------------------------
            '-- !!! TO NOTE Varchar(12) : DocNo: {4 CHARS = StoreID} & {2 DIGITS = Year} & {1 CHAR = "D" for Doc Type} & {5 PADDING CHARS = Incremental Seed}
            '--     SEQUENCE MUST NOT BREAK OR ELSE CODE WILL FAIL !!!          
            '---------------------------------------------------------------------------------------------------------------------------------------

            'If DirectIssueList.Count > 0 Then

            '    If Left(DirectIssueList(0).DocumentNo, 2) <> Right(Year(Now).ToString, 2) Then

            '        lblDocumentNo.Text = Session("StoreID").ToString & Right(Year(Now).ToString, 2) & "D" & "00001"

            '        lblLastSerialNo.Text = DirectIssueList(0).SerialNo

            '    Else

            '        Dim intCount As Integer = CInt(Right(DirectIssueList(0).DocumentNo, 5)) + 1
            '        lblDocumentNo.Text = Session("StoreID").ToString & Right(Year(Now).ToString, 2) & "D" & intCount.ToString.PadLeft(5, "0")
            '        lblLastSerialNo.Text = DirectIssueList(0).SerialNo

            '    End If

            'Else

            '    lblDocumentNo.Text = Session("StoreID").ToString & Right(Year(Now).ToString, 2) & "D" & "00001"

            'End If

        Catch ex As FaultException

            lblErrAddDirectIssue.Text = ex.Message
            lblErrAddDirectIssue.Visible = True

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

#End Region

End Class