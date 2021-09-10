Imports Microsoft.Reporting.WebForms
Imports System.Web.Services.Description
Imports System.Web.Services
Imports NEA_ICS.UserInterface.NEA_ICS.WcfService
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ServiceModel
Imports System.Reflection
Imports System.IO

''' <summary>
''' AUTHOR: Jianfa Chen
''' </summary>
''' <remarks>
''' ---------------------------------------------------------------------------------------------
''' CHANGE HISTORY: 
''' ---------------------------------------------------------------------------------------------
''' ERSS 921133947: 
''' 1) To provide option for Open/Close or both in item master list;
''' 2) To be able to close stock item on any date when both value and quantity is of zero value;
''' 3) To populate next incremental stock code after keying in first 4 characters; 
''' ---------------------------------------------------------------------------------------------
''' </remarks>
Partial Public Class frmItem
    Inherits clsCommonFunction

    'Private Shared ItemList As List(Of ItemDetails)
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

            aceStockCodeFrom.ContextKey = Session("StoreID").ToString
            aceStockCodeTo.ContextKey = Session("StoreID").ToString

            AccessRights = clsCommonFunction.AssignAccessRights(Cache(Session(ESession.UserID.ToString) & Session(ESession.StoreID.ToString) & "AccessRights"), _
                                                                clsCommonFunction.moduleID.Item)

            If AccessRights(0).SelectRight = False Then
                Server.Transfer("..\frmUnauthorisedPage.aspx")
                Exit Sub
            End If

            If AccessRights(0).InsertRight = False Then
                tbpNewItem.Visible = False
                ViewState("_NEW") = False
            Else
                tbpNewItem.Visible = True
                ViewState("_NEW") = True
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

            tbcItem.Visible = True

            BindUOM()
            BindStockType()
            BindSubType()
            BindEquipmentNo()
            'BindStockCode()

            If CacheListIsEmpty(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), GetType(ItemDetails)) Then GetItemList(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), Session(ESession.StoreID.ToString))

        End If

        '-- Client Side Java Scripting
        txtAddOpeningBal.Attributes.Add("onchange", "computeItemUnit('" & txtAddOpeningBal.ClientID & "','" & txtAddOpeningTotalValue.ClientID & "','" & lblAUC.ClientID & "');")
        txtAddOpeningBal.Attributes.Add("onkeyup", "computeItemUnit('" & txtAddOpeningBal.ClientID & "','" & txtAddOpeningTotalValue.ClientID & "','" & lblAUC.ClientID & "');")
        txtAddOpeningTotalValue.Attributes.Add("onchange", "computeItemUnit('" & txtAddOpeningBal.ClientID & "','" & txtAddOpeningTotalValue.ClientID & "','" & lblAUC.ClientID & "');")
        txtAddOpeningTotalValue.Attributes.Add("onkeyup", "computeItemUnit('" & txtAddOpeningBal.ClientID & "','" & txtAddOpeningTotalValue.ClientID & "','" & lblAUC.ClientID & "');")
        txtEditOpeningBal.Attributes.Add("onchange", "computeItemUnit('" & txtEditOpeningBal.ClientID & "','" & txtEditOpeningTotalValue.ClientID & "','" & lblDisplayAUC.ClientID & "');")
        txtEditOpeningBal.Attributes.Add("onkeyup", "computeItemUnit('" & txtEditOpeningBal.ClientID & "','" & txtEditOpeningTotalValue.ClientID & "','" & lblDisplayAUC.ClientID & "');")
        txtEditOpeningTotalValue.Attributes.Add("onchange", "computeItemUnit('" & txtEditOpeningBal.ClientID & "','" & txtEditOpeningTotalValue.ClientID & "','" & lblDisplayAUC.ClientID & "');")
        txtEditOpeningTotalValue.Attributes.Add("onkeyup", "computeItemUnit('" & txtEditOpeningBal.ClientID & "','" & txtEditOpeningTotalValue.ClientID & "','" & lblDisplayAUC.ClientID & "');")

        'tbcItem.ActiveTabIndex = tbcItem.Tabs.Count

    End Sub
#End Region

#Region " New Tab "
    ''' <summary> 
    ''' btnAddItem - Click;
    ''' 07 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CODE LOGIC:
    ''' 1) Validate mandatory fields
    ''' 2) Check Min Level is less than Max Level
    ''' 3) Check Min Level less than or equal to Reorder Level 
    ''' 4) Check ReOrder Level less than or equal to Max Level
    ''' 5) Check Opening Balance More Than or Equal to Min Level 
    ''' 6) Check Opening Balance Less Than or Equal to Max Level
    ''' </remarks>
    Private Sub btnAddItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddItem.Click

        '-------------------------------------------------------------------------
        '--- VALIDATION CHECK
        '-------------------------------------------------------------------------
        If txtAddStockCode.Text.Trim = String.Empty Or txtAddDescription.Text.Trim = String.Empty Or _
            ddlAddStockType.SelectedIndex <= 0 Or ddlAddSubType.SelectedIndex <= 0 Or _
            ddlAddUOM.SelectedIndex <= 0 Or txtAddMinLevel.Text.Trim = String.Empty Or _
            txtAddMaxLevel.Text.Trim = String.Empty Or txtAddReorderLevel.Text.Trim = String.Empty Or _
            txtAddOpeningBal.Text.Trim = String.Empty Or txtAddOpeningTotalValue.Text.Trim = String.Empty Or _
            txtAddLocation1.Text = String.Empty Then

            lblErrAddItem.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.MandatoryField)
            lblErrAddItem.Visible = True

            Dim Script As String = "ShowAlertMessage('" & lblErrAddItem.Text & "');"
            ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

            Exit Sub

        End If

        If Not revAddMinLevel.IsValid Or Not revAddMaxLevel.IsValid Or Not revAddReorderLevel.IsValid Or _
        Not revAddOpeningBal.IsValid Or Not revAddOpeningTotalValue.IsValid Then

            Exit Sub

        End If

        If CDbl(txtAddMinLevel.Text) > CDbl(txtAddMaxLevel.Text) Then

            lblErrAddItem.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.MoreLessThan, , , "Min Level", "Max Level", "< (Less Than)")
            lblErrAddItem.Visible = True

            Dim Script As String = "ShowAlertMessage('" & lblErrAddItem.Text & "');"
            ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

            Exit Sub

        End If
        If CDbl(txtAddMinLevel.Text) > CDbl(txtAddReorderLevel.Text) Then

            lblErrAddItem.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.MoreLessThan, , , "Min Level", "Reorder Level", "<= (Less Than or Equal)")
            lblErrAddItem.Visible = True

            Dim Script As String = "ShowAlertMessage('" & lblErrAddItem.Text & "');"
            ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

            Exit Sub

        End If
        If CDbl(txtAddReorderLevel.Text) > CDbl(txtAddMaxLevel.Text) Then

            lblErrAddItem.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.MoreLessThan, , , "Reorder Level", "Max Level", "<= (Less Than or Equal)")
            lblErrAddItem.Visible = True

            Dim Script As String = "ShowAlertMessage('" & lblErrAddItem.Text & "');"
            ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

            Exit Sub

        End If

        Try

            Dim Client As New ServiceClient
            Dim ItemDetails As New ItemDetails

            ItemDetails.StoreID = Session("StoreID")
            ItemDetails.ItemID = txtAddStockCode.Text.Trim.ToUpper
            ItemDetails.EquipmentID = ddlAddEquipmentNo.SelectedValue
            ItemDetails.ItemDescription = txtAddDescription.Text.Trim
            ItemDetails.ItemID_Description = txtAddStockCode.Text.Trim & " - " & txtAddDescription.Text.Trim
            ItemDetails.PartNo = txtAddPartNo.Text.Trim.ToUpper
            ItemDetails.StockType = ddlAddStockType.SelectedValue
            ItemDetails.SubType = ddlAddSubType.SelectedValue
            ItemDetails.UOM = ddlAddUOM.SelectedValue
            ItemDetails.Location = txtAddLocation1.Text.Trim
            ItemDetails.Location2 = txtAddLocation2.Text.Trim
            ItemDetails.MinLevel = txtAddMinLevel.Text.Trim
            ItemDetails.ReorderLevel = txtAddReorderLevel.Text.Trim
            ItemDetails.MaxLevel = txtAddMaxLevel.Text.Trim
            ItemDetails.OpeningBalance = txtAddOpeningBal.Text.Trim
            ItemDetails.OpeningTotalValue = txtAddOpeningTotalValue.Text.Trim
            ItemDetails.Status = OPEN
            ItemDetails.LoginUser = Session("UserID")

            lblErrAddItem.Text = Client.AddItem(ItemDetails)
            Client.Close()

            If lblErrAddItem.Text = String.Empty Then

                lblMsgAddStockCode.Text = txtAddStockCode.Text.Trim.ToUpper
                lblMsgAddEquipmentNo.Text = IIf(ddlAddEquipmentNo.SelectedValue = String.Empty, String.Empty, ddlAddEquipmentNo.SelectedItem.Text)
                lblMsgAddDescription.Text = txtAddDescription.Text.Trim
                lblMsgAddPartNo.Text = txtAddPartNo.Text.Trim
                lblMsgAddStockType.Text = ddlAddStockType.SelectedItem.Text
                lblMsgAddSubType.Text = ddlAddSubType.SelectedItem.Text
                lblMsgAddUOM.Text = ddlAddUOM.SelectedItem.Text
                lblMsgAddLocation1.Text = txtAddLocation1.Text.Trim
                lblMsgAddLocation2.Text = txtAddLocation2.Text.Trim
                lblMsgAddMinLevel.Text = txtAddMinLevel.Text.Trim
                lblMsgAddReorderLevel.Text = txtAddReorderLevel.Text.Trim
                lblMsgAddMaxLevel.Text = txtAddMaxLevel.Text.Trim
                lblMsgAddOpeningBalance.Text = txtAddOpeningBal.Text.Trim
                lblMsgAddOpeningTotalValue.Text = txtAddOpeningTotalValue.Text.Trim

                lblErrAddItem.Visible = False
                pnlAddItem.Visible = False
                pnlAddItem2.Visible = False
                divMsgBox.Visible = True


                clsCommonFunction.EditCache(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), GetType(ItemDetails), ItemDetails)
                'BindStockCode()

            Else

                Dim Script As String = "ShowAlertMessage('" & lblErrAddItem.Text & "');"
                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

                lblErrAddItem.Visible = True
            End If

        Catch ex As FaultException

            lblErrAddItem.Text = ex.Message
            lblErrAddItem.Visible = True

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

    ''' <summary>
    ''' txtAddStockCode - TextChanged;
    ''' 07 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtAddStockCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAddStockCode.TextChanged

        'Dim Letter As Char
        'Letter = Mid(txtAddStockCode.Text.ToUpper, 4, 1)

        'If IsNumeric(Left(txtAddStockCode.Text.Trim, 3)) And Letter >= "A" And Letter <= "Z" Then

        '    If IsValidEquipmentNo(Left(txtAddStockCode.Text.Trim, 3)) And _
        '        IsValidSubType(CStr(Letter)) Then
        '        lbtnGenerateStockCode.Visible = True
        '    Else
        '        lbtnGenerateStockCode.Visible = False
        '        ddlAddEquipmentNo.Enabled = True
        '        ddlAddSubType.Enabled = True
        '    End If
        'Else
        '    lbtnGenerateStockCode.Visible = False
        '    ddlAddEquipmentNo.SelectedIndex = -1
        '    ddlAddEquipmentNo.Enabled = True
        '    ddlAddSubType.SelectedIndex = -1
        '    ddlAddSubType.Enabled = True
        'End If

        If txtAddStockCode.Text.Length >= 1 Then
            lbtnGenerateStockCode.Visible = True
        Else
            lbtnGenerateStockCode.Visible = False
        End If

    End Sub

    ''' <summary>
    ''' btnAddItemOk - Click;
    ''' 08 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAddItemOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddItemOK.Click

        lblErrAddItem.Visible = False
        pnlAddItem.Visible = True
        pnlAddItem2.Visible = True
        divMsgBox.Visible = False

    End Sub

    ''' <summary>
    ''' lbtnGenerateStockCode - Click;
    ''' 07 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub lbtnGenerateStockCode_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnGenerateStockCode.Click

        Dim GeneratedItemID As String = String.Empty

        Try

            Dim Client As New ServiceClient
            Dim ItemDetails As New ItemDetails

            ItemDetails.ItemID = txtAddStockCode.Text
            ItemDetails.StoreID = Session("StoreID")

            lblErrAddItem.Text = Client.GenerateItemID(ItemDetails, GeneratedItemID)
            Client.Close()

            If lblErrAddItem.Text.Trim = String.Empty Then
                txtAddStockCode.Text = GeneratedItemID.ToUpper
            Else
                lblErrAddItem.Visible = True
            End If

        Catch ex As FaultException

            lblErrAddItem.Text = ex.Message
            lblErrAddItem.Visible = True

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

    ''' <summary>
    ''' btnAddClear - Click;
    ''' 08 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAddClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddClear.Click

        lblErrAddItem.Visible = False
        txtAddStockCode.Text = String.Empty
        ddlAddEquipmentNo.SelectedIndex = -1
        ddlAddEquipmentNo.Enabled = True
        txtAddDescription.Text = String.Empty
        txtAddPartNo.Text = String.Empty
        ddlAddStockType.SelectedIndex = -1
        ddlAddSubType.SelectedIndex = -1
        ddlAddSubType.Enabled = True
        ddlAddUOM.SelectedIndex = -1
        txtAddLocation1.Text = String.Empty
        txtAddLocation2.Text = String.Empty
        txtAddMinLevel.Text = "0.00"
        txtAddReorderLevel.Text = "0.00"
        txtAddMaxLevel.Text = "1.00"
        txtAddOpeningBal.Text = "0.00"
        txtAddOpeningTotalValue.Text = "0.00"
        lblAUC.Text = "0.0000"

        lbtnGenerateStockCode.Visible = False

    End Sub

#End Region

#Region " Locate Tab "
    ''' <summary>
    ''' btnLocateClear - Click;
    ''' 08 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnLocateClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLocateClear.Click

        pnlItemInfo.Visible = False
        pnlSearchResults.Visible = False
        txtLocateStockCode.Text = String.Empty
        txtLocateLocation.Text = String.Empty
        ddlLocateStatus.SelectedValue = "O"

    End Sub

    ''' <summary>
    ''' btnLocateGo - Click;
    ''' 08 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnLocateGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLocateGo.Click

        Try

            Dim Client As New ServiceClient
            Dim ItemSearch As New ItemDetails

            ItemSearch.StoreID = Session("StoreID")
            ItemSearch.ItemID = txtLocateStockCode.Text.Trim.ToUpper
            '-- UAT.02.19 -- To include Stock Description as wild card search 
            ItemSearch.ItemDescription = txtLocateDescription.Text.Trim
            ItemSearch.Location = txtLocateLocation.Text.Trim
            ItemSearch.Status = ddlLocateStatus.SelectedValue

            'ItemList = Client.GetItems(ItemSearch, String.Empty, String.Empty)
            Cache(Session(ESession.StoreID.ToString) & "ItemList") = Client.GetItems(ItemSearch, String.Empty, String.Empty)
            Client.Close()

            gdvLocateItem.DataSource = Cache(Session(ESession.StoreID.ToString) & "ItemList")
            gdvLocateItem.DataBind()

            pnlSearchResults.Visible = True
            pnlItemInfo.Visible = False

        Catch ex As FaultException

            lblErrLocateItem.Text = ex.Message
            lblErrLocateItem.Visible = True

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

    ''' <summary>
    ''' gdvLocateItem - PageIndexChaning;
    ''' 09 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gdvLocateItem_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gdvLocateItem.PageIndexChanging

        gdvLocateItem.PageIndex = e.NewPageIndex
        gdvLocateItem.DataSource = Cache(Session(ESession.StoreID.ToString) & "ItemList")
        gdvLocateItem.DataBind()


    End Sub

    ''' <summary>
    ''' gdvLocateItem - RowDataBound;
    ''' 09 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gdvLocateItem_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvLocateItem.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            Select Case CType(e.Row.FindControl("hidStatus"), HiddenField).Value.ToUpper
                Case "O"
                    CType(e.Row.FindControl("lblStatus"), Label).Text = "Open"
                Case "C"
                    CType(e.Row.FindControl("lblStatus"), Label).Text = "Closed"
                Case "D"
                    CType(e.Row.FindControl("lblStatus"), Label).Text = "Deleted"
                Case "S"
                    CType(e.Row.FindControl("lblStatus"), Label).Text = "Suspended"
            End Select

        End If

    End Sub

    ''' <summary>
    ''' gdvLocateItem - SelectedIndexChanged;
    ''' 09 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gdvLocateItem_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvLocateItem.SelectedIndexChanged

        lblEditStockCode.Text = gdvLocateItem.SelectedRow.Cells(1).Text
        txtEditDescription.Text = Replace(System.Web.HttpUtility.HtmlDecode(gdvLocateItem.SelectedRow.Cells(2).Text), "&nbsp;", String.Empty)

        ''txtEditDescription.Text = gdvLocateItem.SelectedRow.Cells(2).Text
        txtEditLocation.Text = Replace(gdvLocateItem.SelectedRow.Cells(3).Text, "&nbsp;", String.Empty)
        txtEditLocation2.Text = Replace(gdvLocateItem.SelectedRow.Cells(4).Text, "&nbsp;", String.Empty)
        ddlEditEquipmentNo.SelectedValue = clsCommonFunction.BindActiveCodes(ddlEditEquipmentNo, CType(gdvLocateItem.SelectedRow.FindControl("hidEquipmentID"), HiddenField).Value)
        txtEditPartNo.Text = CType(gdvLocateItem.SelectedRow.FindControl("hidPartNo"), HiddenField).Value
        ddlEditStockType.SelectedValue = clsCommonFunction.BindActiveCodes(ddlEditStockType, CType(gdvLocateItem.SelectedRow.FindControl("hidStockType"), HiddenField).Value)
        ddlEditSubType.SelectedValue = clsCommonFunction.BindActiveCodes(ddlEditSubType, CType(gdvLocateItem.SelectedRow.FindControl("hidSubType"), HiddenField).Value)
        ddlEditUOM.SelectedValue = clsCommonFunction.BindActiveCodes(ddlEditUOM, CType(gdvLocateItem.SelectedRow.FindControl("hidUOM"), HiddenField).Value)
        txtEditMinLevel.Text = String.Format("{0:0.00}", CDec(CType(gdvLocateItem.SelectedRow.FindControl("hidMinLevel"), HiddenField).Value))
        txtEditMaxLevel.Text = String.Format("{0:0.00}", CDec(CType(gdvLocateItem.SelectedRow.FindControl("hidMaxLevel"), HiddenField).Value))
        txtEditReOrderLevel.Text = String.Format("{0:0.00}", CDec(CType(gdvLocateItem.SelectedRow.FindControl("hidReorderLevel"), HiddenField).Value))
        lblDisplayStatus.Text = CType(gdvLocateItem.SelectedRow.FindControl("lblStatus"), Label).Text

        If ViewState("_EDIT") Then
            Select Case CType(gdvLocateItem.SelectedRow.FindControl("hidStatus"), HiddenField).Value.Trim
                Case "O"
                    lbtnLocateClose.Visible = True
                    lbtnLocateReopen.Visible = False
                Case "C"
                    lbtnLocateClose.Visible = False
                    lbtnLocateReopen.Visible = True
                Case Else
                    lbtnLocateClose.Visible = False
                    lbtnLocateReopen.Visible = False
            End Select
        Else
            lbtnLocateClose.Visible = False
            lbtnLocateReopen.Visible = False
        End If

        Dim Client As New ServiceClient
        Dim StockTransList As New List(Of ItemDetails)
        Dim ItemDetails As New ItemDetails

        ItemDetails.StoreID = Session("StoreID")
        ItemDetails.ItemID = lblEditStockCode.Text

        StockTransList = Client.GetStockTransaction(ItemDetails, "BALANCE")
        Client.Close()

        If StockTransList.Count > 0 Then
            txtEditOpeningBal.Text = String.Format("{0:0.00}", CDec(StockTransList.Item(0).OpeningBalance))
            txtEditOpeningTotalValue.Text = String.Format("{0:0.00}", CDec(StockTransList.Item(0).OpeningTotalValue))
            lblDisplayAUC.Text = "0.0000"
            hidTransactiondate.Value = StockTransList.Item(0).TransactionDate
        End If


        EditMode(False)
        pnlItemInfo.Visible = True
        lblErrSaveItem.Visible = False
        lblErrLocateItem.Visible = False

    End Sub

    ''' <summary>
    ''' gdvLocateItem - Sorting
    ''' 09 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gdvLocateItem_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gdvLocateItem.Sorting

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
            Dim ItemSearch As New ItemDetails

            ItemSearch.StoreID = Session("StoreID")
            ItemSearch.ItemID = txtLocateStockCode.Text.Trim.ToUpper
            ItemSearch.Location = txtLocateLocation.Text.Trim
            ItemSearch.Status = ddlLocateStatus.SelectedValue

            'ItemList = Client.GetItems(ItemSearch, e.SortExpression, ViewState("_SortDirection").ToString)
            Cache(Session(ESession.StoreID.ToString) & "ItemList") = Client.GetItems(ItemSearch, e.SortExpression, ViewState("_SortDirection").ToString)
            Client.Close()

            gdvLocateItem.DataSource = Cache(Session(ESession.StoreID.ToString) & "ItemList")
            gdvLocateItem.DataBind()

            pnlSearchResults.Visible = True

        Catch ex As FaultException

            lblErrLocateItem.Text = ex.Message
            lblErrLocateItem.Visible = True

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

    ''' <summary>
    ''' lbtnLocateReopen - Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub lbtnLocateReopen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnLocateReopen.Click

        Try

            Dim Client As New ServiceClient
            Dim ItemDetails As New ItemDetails
            Dim returnMessage As String = String.Empty

            ItemDetails.StoreID = Session("StoreID")
            ItemDetails.ItemID = lblEditStockCode.Text
            ItemDetails.Status = "O"
            ItemDetails.LoginUser = Session("UserID").ToString

            lblErrSaveItem.Text = Client.UpdateItemStatus(ItemDetails, returnMessage)
            Client.Close()

            If lblErrSaveItem.Text = String.Empty Then

                If returnMessage <> String.Empty Then

                    Dim Script As String = "ShowAlertMessage('" & returnMessage & "');"

                    ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)


                Else

                    Dim Script As String = "ShowSuccessMessage('" & clsCommonFunction.GetMessage( _
                                            clsCommonFunction.messageID.Success, "opened", "Item") & "');"

                    ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)


                    '-- UPDATE CACHE
                    clsCommonFunction.EditCache(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), GetType(ItemDetails), ItemDetails, True)

                    btnLocateGo_Click(sender, e)
                    gdvLocateItem_SelectedIndexChanged(sender, e)

                End If

            Else
                lblErrLocateItem.Visible = True
            End If

        Catch ex As FaultException

            lblErrSaveItem.Text = ex.Message
            lblErrSaveItem.Visible = True

        Catch ex As Exception
            lblErrSaveItem.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            lblErrSaveItem.Visible = True
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

    ''' <summary>
    ''' lbtnLocateEdit - Click;
    ''' 09 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub lbtnLocateEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnLocateEdit.Click

        Dim Letter As Char
        Letter = Mid(lblEditStockCode.Text.ToUpper, 4, 1)

        lblErrSaveItem.Visible = False

        If Session(ESession.UserRoleType.ToString).ToString.Contains(STOREOFFICER) Then  ' if Store officer has granted an Edit right, only allow him to edit Location 1 & 2 - ERSS:12143543
            EditMode(False)
            txtEditLocation.Enabled = True
            txtEditLocation2.Enabled = True
            btnLocateSave.Enabled = True
        Else
            EditMode(True)
        End If

        If Not clsCommonFunction.IsWithinFinanceCutoffDate(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), Session("StoreID"), CDate(hidTransactiondate.Value)) Then
            txtEditOpeningBal.Enabled = False
            txtEditOpeningTotalValue.Enabled = False
        Else
            txtEditOpeningBal.Enabled = True
            txtEditOpeningTotalValue.Enabled = True
        End If


        'If Not IsValidEquipmentNo(Left(lblEditStockCode.Text.Trim, 3)) And IsValidSubType(CStr(Letter)) Then
        '    ddlEditEquipmentNo.Enabled = False
        '    ddlEditSubType.Enabled = False
        'End If

    End Sub

    ''' <summary>
    ''' btnLocateSave - Click;
    ''' 10 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnLocateSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLocateSave.Click

        '-------------------------------------------------------------------------
        '--- VALIDATION CHECK
        '-------------------------------------------------------------------------
        If txtEditDescription.Text.Trim = String.Empty Or _
            ddlEditStockType.SelectedIndex <= 0 Or ddlEditSubType.SelectedIndex <= 0 Or _
            ddlEditUOM.SelectedIndex <= 0 Or txtEditMinLevel.Text.Trim = String.Empty Or _
            txtEditMaxLevel.Text.Trim = String.Empty Or txtEditReOrderLevel.Text.Trim = String.Empty Or _
            txtEditOpeningBal.Text.Trim = String.Empty Or txtEditOpeningTotalValue.Text.Trim = String.Empty Or _
            txtEditLocation.Text = String.Empty Then

            lblErrSaveItem.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.MandatoryField)
            lblErrSaveItem.Visible = True

            Dim Script As String = "ShowAlertMessage('" & lblErrSaveItem.Text & "');"
            ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

            Exit Sub

        End If

        If Not revEditMinLevel.IsValid Or Not revEditMaxLevel.IsValid Or Not revEditReorderLevel.IsValid Or _
        Not revEditOpeningBal.IsValid Or Not revEditOpeningTotalValue.IsValid Then

            Exit Sub

        End If


        If CDbl(txtEditMinLevel.Text) > CDbl(txtEditMaxLevel.Text) Then

            lblErrSaveItem.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.MoreLessThan, , , "Min Level", "Max Level", "< (Less Than)")
            lblErrSaveItem.Visible = True

            Dim Script As String = "ShowAlertMessage('" & lblErrSaveItem.Text & "');"
            ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

            Exit Sub

        End If
        If CDbl(txtEditMinLevel.Text) > CDbl(txtEditReOrderLevel.Text) Then

            lblErrSaveItem.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.MoreLessThan, , , "Min Level", "Reorder Level", "<= (Less Than or Equal)")
            lblErrSaveItem.Visible = True

            Dim Script As String = "ShowAlertMessage('" & lblErrSaveItem.Text & "');"
            ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

            Exit Sub

        End If
        If CDbl(txtEditReOrderLevel.Text) > CDbl(txtEditMaxLevel.Text) Then

            lblErrSaveItem.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.MoreLessThan, , , "Reorder Level", "Max Level", "<= (Less Than or Equal)")
            lblErrSaveItem.Visible = True

            Dim Script As String = "ShowAlertMessage('" & lblErrSaveItem.Text & "');"
            ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

            Exit Sub

        End If

        Try

            Dim Client As New ServiceClient
            Dim ItemDetails As New ItemDetails
            Dim ReturnMessage As String = String.Empty

            ItemDetails.StoreID = Session("StoreID")
            ItemDetails.ItemID = lblEditStockCode.Text.Trim.ToUpper
            ItemDetails.EquipmentID = ddlEditEquipmentNo.SelectedValue
            ItemDetails.ItemDescription = txtEditDescription.Text.Trim
            ItemDetails.ItemID_Description = lblEditStockCode.Text.Trim & " - " & txtEditDescription.Text.Trim
            ItemDetails.PartNo = txtEditPartNo.Text.Trim.ToUpper
            ItemDetails.StockType = ddlEditStockType.SelectedValue
            ItemDetails.SubType = ddlEditSubType.SelectedValue
            ItemDetails.UOM = ddlEditUOM.SelectedValue
            ItemDetails.Location = txtEditLocation.Text.Trim
            ItemDetails.Location2 = txtEditLocation2.Text.Trim
            ItemDetails.MinLevel = txtEditMinLevel.Text.Trim
            ItemDetails.ReorderLevel = txtEditReOrderLevel.Text.Trim
            ItemDetails.MaxLevel = txtEditMaxLevel.Text.Trim
            ItemDetails.OpeningBalance = IIf(txtEditOpeningBal.Enabled, txtEditOpeningBal.Text.Trim, -1.0)
            ItemDetails.OpeningTotalValue = IIf(txtEditOpeningTotalValue.Enabled, txtEditOpeningTotalValue.Text.Trim, -1.0)
            ItemDetails.Status = "O"
            ItemDetails.LoginUser = Session("UserID")

            lblErrSaveItem.Text = Client.UpdateItem(ItemDetails, ReturnMessage)
            Client.Close()

            If lblErrSaveItem.Text = String.Empty Then

                If ReturnMessage = String.Empty Then

                    lblErrSaveItem.Visible = False

                    Dim Script As String = "ShowSuccessMessage('" & clsCommonFunction.GetMessage( _
                                           clsCommonFunction.messageID.Success, "saved", "Item") & "');"

                    ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)


                    '-- UPDATE CACHE
                    clsCommonFunction.EditCache(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), GetType(ItemDetails), ItemDetails, True)

                    btnLocateGo_Click(sender, e)
                    gdvLocateItem_SelectedIndexChanged(sender, e)

                Else

                    Dim Script As String = "ShowAlertMessage('" & ReturnMessage & "');"

                    ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

                End If

            Else
                lblErrSaveItem.Visible = True

                Dim Script As String = "ShowAlertMessage('" & lblErrSaveItem.Text & "');"
                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

            End If

        Catch ex As FaultException

            lblErrSaveItem.Text = ex.Message
            lblErrSaveItem.Visible = True

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

    ''' <summary>
    ''' btnLocateCancel - Click;
    ''' 12 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnLocateCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLocateCancel.Click

        lblErrSaveItem.Visible = False
        pnlItemInfo.Visible = False

    End Sub

    ''' <summary>
    ''' lbtnLocateClose - Click;
    ''' 11 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub lbtnLocateClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnLocateClose.Click


        Try

            Dim Client As New ServiceClient
            Dim ItemDetails As New ItemDetails
            Dim returnMessage As String = String.Empty

            ItemDetails.StoreID = Session("StoreID")
            ItemDetails.ItemID = lblEditStockCode.Text
            ItemDetails.Status = "C"
            ItemDetails.LoginUser = Session("UserID").ToString

            lblErrSaveItem.Text = Client.UpdateItemStatus(ItemDetails, returnMessage)
            Client.Close()

            If lblErrSaveItem.Text = String.Empty Then

                If returnMessage <> String.Empty Then

                    Dim Script As String = "ShowAlertMessage('" & returnMessage & "');"

                    ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)


                Else

                    Dim Script As String = "ShowSuccessMessage('" & clsCommonFunction.GetMessage( _
                                            clsCommonFunction.messageID.Success, "closed", "Item") & "');"

                    ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)


                    '-- UPDATE CACHE
                    clsCommonFunction.EditCache(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), GetType(ItemDetails), ItemDetails, True)

                    lblErrSaveItem.Text = String.Empty
                    lblErrSaveItem.Visible = False

                    btnLocateGo_Click(sender, e)
                    gdvLocateItem_SelectedIndexChanged(sender, e)

                End If

            Else

                Dim Script As String = "ShowAlertMessage('" & lblErrSaveItem.Text & "');"
                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)
                lblErrSaveItem.Visible = False

            End If

        Catch ex As FaultException
            Dim fault As ServiceFault = ex.Data

            lblErrSaveItem.Text = fault.MessageText
            lblErrSaveItem.Visible = True

        Catch ex As Exception
            lblErrSaveItem.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            lblErrSaveItem.Visible = True
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

    ''' <summary>
    ''' lbtnLocateDel - Click;
    ''' 12 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub lbtnLocateDel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnLocateDel.Click


        Try

            Dim Client As New ServiceClient
            Dim ItemDetails As New ItemDetails
            Dim returnMessage As String = String.Empty

            ItemDetails.StoreID = Session("StoreID")
            ItemDetails.ItemID = lblEditStockCode.Text
            ItemDetails.Status = DELETE
            ItemDetails.LoginUser = Session("UserID").ToString

            lblErrSaveItem.Text = Client.UpdateItemStatus(ItemDetails, returnMessage)
            Client.Close()

            If lblErrSaveItem.Text = String.Empty Then

                If returnMessage <> String.Empty Then

                    Dim Script As String = "ShowAlertMessage('" & returnMessage & "');"

                    ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)


                Else

                    Dim Script As String = "ShowSuccessMessage('" & clsCommonFunction.GetMessage( _
                                            clsCommonFunction.messageID.Success, "deleted", "Item") & "');"

                    ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)


                    '-- DELETE CACHE
                    Dim itemToRemove As ItemDetails = _
                    DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), List(Of ItemDetails)).Find(Function(i As ItemDetails) i.ItemID = lblEditStockCode.Text)
                    DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), List(Of ItemDetails)).Remove(itemToRemove)

                    btnLocateGo_Click(sender, e)
                    gdvLocateItem_SelectedIndexChanged(sender, e)

                End If

            Else
                lblErrLocateItem.Visible = True
            End If

        Catch ex As FaultException
            Dim fault As ServiceFault = ex.Data

            lblErrSaveItem.Text = fault.MessageText
            lblErrSaveItem.Visible = True

        Catch ex As Exception
            lblErrSaveItem.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            lblErrSaveItem.Visible = True
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub
#End Region

#Region " Print Tab "

    Protected Sub ObjectDataSource1_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs) Handles ObjectDataSource1.Selecting
        Dim sortBy As String
        Select Case ddlSortBy.SelectedIndex
            Case 0
                sortBy = "FStockItemByStockRangeID"
            Case 1
                sortBy = "FStockItemByStockRangeID"
            Case 2
                sortBy = "FStockItemByStockRangeStockType"
            Case 3
                sortBy = "FStockItemByStockRangeSubType"
            Case 4
                sortBy = "FStockItemByStockRangeEquipmentID"
            Case Else
                sortBy = "FStockItemByStockRangeID"
        End Select

        e.InputParameters("storeID") = Session("StoreID")
        e.InputParameters("sortBy") = sortBy
        e.InputParameters("printOption") = ddlPrintOption.SelectedValue
        If rblStockCode.SelectedValue = "No" Then
            e.InputParameters("stockCodeFrom") = txtStockCodeFrom.Text 'ddlStockCodeFrom.Items(1).Value
            e.InputParameters("stockCodeTo") = txtStockCodeTo.Text  'ddlStockCodeFrom.Items(ddlStockCodeFrom.Items.Count - 1).Value
        Else
            e.InputParameters("stockCodeFrom") = String.Empty 'ddlStockCodeFrom.SelectedValue.ToString()
            e.InputParameters("stockCodeTo") = String.Empty  'ddlStockCodeTo.SelectedValue.ToString()
        End If

        Dim stockTypes As String = ""
        For Each item As ListItem In chkStockType.Items
            If item.Selected Then
                stockTypes += ("|" + item.Value)
            End If
        Next

        e.InputParameters("excludeStockCodeTypes") = stockTypes
        e.InputParameters("itemStatus") = ddlItemStatus.SelectedValue

    End Sub
    Protected Sub ObjectDataSource1_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles ObjectDataSource1.Selected
        Dim returnList As List(Of ItemDetails) = e.ReturnValue
        If returnList.Count <= 0 Then
            NoRecordFond = "Y"
        Else
            NoRecordFond = "N"
        End If
    End Sub

    ''' <summary>
    ''' btnPDF - Click;
    ''' Guo Feng
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' ADDITIONAL TEST CASE:
    ''' 1) Include validation checks;
    ''' </remarks>
    Protected Sub btnPDF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPDF.Click

        txtStockCodeFrom.Text = Split(txtStockCodeFrom.Text, " | ")(0).Trim.ToUpper
        txtStockCodeTo.Text = Split(txtStockCodeTo.Text, " | ")(0).Trim.ToUpper

        '-- ADDITIONAL CODE TO BE ADDED
        If Not ViewState("_NEW") Is Nothing And ViewState("_NEW") = False Then
            tbpNewItem.Visible = False
        End If

        If ddlPrintOption.SelectedIndex <= 0 Or _
        (rblStockCode.Items(0).Selected = False And rblStockCode.Items(1).Selected = False) Then

            lblErrPrintItem.Text = GetMessage(messageID.MandatoryField)
            lblErrPrintItem.Visible = True

            Dim Script As String = "ShowAlertMessage('" & lblErrPrintItem.Text & "');"
            ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

            Exit Sub

        End If


        rvr.LocalReport.DataSources.Clear()
        rvr.LocalReport.DataSources.Add(New ReportDataSource("ItemDetails", ObjectDataSource1))

        Dim paraStockFrom As String
        Dim paraStockTo As String
        If rblStockCode.SelectedValue = "Yes" Then
            paraStockFrom = "All"
            paraStockTo = "All"
        Else
            paraStockFrom = txtStockCodeFrom.Text 'ddlStockCodeFrom.SelectedValue
            paraStockTo = txtStockCodeTo.Text 'ddlStockCodeTo.SelectedValue
        End If

        Dim parameterlist As New List(Of ReportParameter)
        Dim p1 As New ReportParameter("ReportType", ddlPrintOption.SelectedItem.Text)
        Dim p2 As New ReportParameter("SortBy", ddlSortBy.SelectedItem.Text)
        Dim p3 As New ReportParameter("StockCodeFrom", paraStockFrom)
        Dim p4 As New ReportParameter("StockCodeTo", paraStockTo)
        Dim p5 As New ReportParameter("Store", Session("StoreName").ToString)
        Dim p6 As New ReportParameter("ItemStatus", ddlItemStatus.SelectedItem.Text)
        parameterlist.Add(p1)
        parameterlist.Add(p2)
        parameterlist.Add(p3)
        parameterlist.Add(p4)
        parameterlist.Add(p5)
        parameterlist.Add(p6)

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
        Response.AddHeader("content-disposition", "attachment;filename=StockMasterList.pdf")
        Response.BinaryWrite(bytValue)
        Response.Flush()
        Response.End()

    End Sub

    ''' <summary>
    ''' btnExcel - Click;
    ''' Guo Feng
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' ADDITIONAL TEST CASE:
    ''' 1) Include Validation Checks;
    ''' </remarks>
    Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExcel.Click

        txtStockCodeFrom.Text = Split(txtStockCodeFrom.Text, " | ")(0).Trim.ToUpper
        txtStockCodeTo.Text = Split(txtStockCodeTo.Text, " | ")(0).Trim.ToUpper

        If Session("StoreID").ToString.ToUpper <> "TIP" And Session("StoreID").ToString.ToUpper <> "TSIP" Then

            If ddlPrintOption.SelectedIndex <= 0 Or _
            (rblStockCode.Items(0).Selected = False And rblStockCode.Items(1).Selected = False) Then

                lblErrPrintItem.Text = GetMessage(messageID.MandatoryField)
                lblErrPrintItem.Visible = True

                Dim Script As String = "ShowAlertMessage('" & lblErrPrintItem.Text & "');"
                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

                Exit Sub

            End If

            rvr.LocalReport.DataSources.Clear()
            rvr.LocalReport.DataSources.Add(New ReportDataSource("ItemDetails", ObjectDataSource1))

            Dim paraStockFrom As String
            Dim paraStockTo As String
            If rblStockCode.SelectedValue = "Yes" Then
                paraStockFrom = "All"
                paraStockTo = "All"
            Else
                paraStockFrom = txtStockCodeFrom.Text  'ddlStockCodeFrom.SelectedValue
                paraStockTo = txtStockCodeTo.Text 'ddlStockCodeTo.SelectedValue
            End If

            Dim parameterlist As New List(Of ReportParameter)
            Dim p1 As New ReportParameter("ReportType", ddlPrintOption.SelectedItem.Text)
            Dim p2 As New ReportParameter("SortBy", ddlSortBy.SelectedItem.Text)
            Dim p3 As New ReportParameter("StockCodeFrom", paraStockFrom)
            Dim p4 As New ReportParameter("StockCodeTo", paraStockTo)
            Dim p5 As New ReportParameter("Store", Session("StoreName").ToString)
            Dim p6 As New ReportParameter("ItemStatus", ddlItemStatus.SelectedItem.Text)
            parameterlist.Add(p1)
            parameterlist.Add(p2)
            parameterlist.Add(p3)
            parameterlist.Add(p4)
            parameterlist.Add(p5)
            parameterlist.Add(p6)

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
            Response.AddHeader("content-disposition", "attachment;filename=StockMasterList.xls")
            Response.BinaryWrite(bytValue)
            Response.Flush()
            Response.End()

        Else


            Dim sortBy, stockCodeFrom, stockCodeTo As String
            Select Case ddlSortBy.SelectedIndex
                Case 0
                    sortBy = "FStockItemByStockRangeID"
                Case 1
                    sortBy = "FStockItemByStockRangeID"
                Case 2
                    sortBy = "FStockItemByStockRangeStockType"
                Case 3
                    sortBy = "FStockItemByStockRangeSubType"
                Case 4
                    sortBy = "FStockItemByStockRangeEquipmentID"
                Case Else
                    sortBy = "FStockItemByStockRangeID"
            End Select

            If rblStockCode.SelectedValue = "Yes" Then
                stockCodeFrom = txtStockCodeFrom.Text 'ddlStockCodeFrom.Items(1).Value
                stockCodeTo = txtStockCodeTo.Text 'ddlStockCodeFrom.Items(ddlStockCodeFrom.Items.Count - 1).Value
            Else
                stockCodeFrom = String.Empty  'ddlStockCodeFrom.SelectedValue.ToString()
                stockCodeTo = String.Empty 'ddlStockCodeTo.SelectedValue.ToString()
            End If

            Dim stockTypes As String = ""
            For Each item As ListItem In chkStockType.Items
                If item.Selected Then
                    stockTypes += ("|" + item.Value)
                End If
            Next

            Dim Client As New ServiceClient
            Dim ItemDetails As New ItemDetails
            Dim ItemList As New List(Of ItemDetails)

            ItemList = Client.GetItemsMasterList(Session("StoreID"), ddlPrintOption.SelectedValue, sortBy, stockCodeFrom, _
                                        stockCodeTo, stockTypes, ddlItemStatus.SelectedValue)

            gdvItemReport.DataSource = ItemList
            gdvItemReport.DataBind()

            For Each row As GridViewRow In gdvItemReport.Rows

                If row.RowType = DataControlRowType.DataRow Then
                    row.Cells(1).Attributes.Add("class", "text")
                    row.Cells(8).Attributes.Add("class", "2dec")
                    row.Cells(9).Attributes.Add("class", "2dec")
                    row.Cells(10).Attributes.Add("class", "2dec")
                    row.Cells(11).Attributes.Add("class", "4dec")
                    row.Cells(12).Attributes.Add("class", "4dec")
                    row.Cells(13).Attributes.Add("class", "4dec")
                End If

            Next

            Dim attachment As String = "attachment; filename=ItemMasterList.xls"
            Response.ClearContent()
            Response.AddHeader("content-disposition", attachment)
            Response.ContentType = "application/ms-excel"

            Dim sw As New StringWriter
            Dim htw As New HtmlTextWriter(sw)
            Dim sw2 As New StringWriter
            Dim htw2 As New HtmlTextWriter(sw2)
            Dim style As String = "<style> .text { mso-number-format: \@; } .2dec { mso-number-format: 0\.00; } .4dec { mso-number-format: 0\.0000; } </style>"
            Dim header, footer As String
            Dim TotalStockValue As Double = 0.0

            For Each row As GridViewRow In gdvItemReport.Rows
                TotalStockValue += CDbl(row.Cells(12).Text)
            Next

            header = "<div width='100%'>"
            header &= "<table width='100%' border='0'>"
            header &= "<tr>"
            header &= "<td width='20%' rowspan='4'>"
            header &= "<img id='imgLogo' src='" & Server.MapPath("~\Images") & "\NEA_Color_Logo.gif' height=90>"
            header &= "</td>"
            header &= "<td width='5%'>&nbsp;</td>"
            header &= "<td width='5%'>&nbsp;</td>"
            header &= "<td width='40%' align='center' valign='top' style='font-family: Verdana; font-weight: bold; font-size: 9pt;'>NATIONAL ENVIRONMENT AGENCY</td>"
            header &= "<td width='5%'>&nbsp;</td>"
            header &= "<td width='20%' align='right' valign='top' rowspan='4' tyle='font-family: Verdana; font-size: small'>Printed On: " & DateTime.Now.ToString("dd/MM/yyyy hh:mm tt").ToString & "</td> "
            header &= "</tr><tr>"
            header &= "<td width='5%'>&nbsp;</td>"
            header &= "<td width='5%'>&nbsp;</td>"
            header &= "<td width='40%' align='center' valign='top' style='font-family: Verdana; font-weight: bold; font-size: 9pt;'>INVENTORY CONTROL SYSTEM</td>"
            header &= "<td width='5%'>&nbsp;</td>"
            header &= "</tr><tr>"
            header &= "<td width='5%'>&nbsp;</td>"
            header &= "<td width='5%'>&nbsp;</td>"
            header &= "<td align='center' valign='top' style='font-family: Verdana; font-weight: bold; font-size: 8pt;'>" & Session("StoreName") & "</td>"
            header &= "<td width='5%'>&nbsp;</td>"
            header &= "</tr><tr>"
            header &= "<td width='5%'>&nbsp;</td>"
            header &= "<td width='5%'>&nbsp;</td>"
            header &= "<td align='center' valign='top' style='font-family: Verdana; font-weight: bold; font-size: 8pt;'>Item Master List Report</td>"
            header &= "<td width='5%'>&nbsp;</td>"
            header &= "</tr></table><br><br>"

            gdvItemReport.RenderControl(htw)

            footer = "<br><br><table border='0'>"
            footer &= "<td>&nbsp;</td>"
            footer &= "<td>&nbsp;</td>"
            footer &= "<td>&nbsp;</td>"
            footer &= "<td>&nbsp;</td>"
            footer &= "<td>&nbsp;</td>"
            footer &= "<td>&nbsp;</td>"
            footer &= "<td>&nbsp;</td>"
            footer &= "<td>&nbsp;</td>"
            footer &= "<td>&nbsp;</td>"
            footer &= "<td>&nbsp;</td>"
            footer &= "<td>&nbsp;</td>"
            footer &= "<td style='background-color: #666699; color: White; font-weight: bold;'>Total Stock Value</td>"
            footer &= "<td class='4dec'>" & TotalStockValue & "</td>"
            footer &= "</table>"

            Response.Write(style)
            Response.Write(header)
            Response.Write(sw.ToString)
            Response.Write(footer)
            Response.Write("</div>")
            Response.End()

        End If


    End Sub

    Protected Sub rblStockCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rblStockCode.SelectedIndexChanged

        If rblStockCode.SelectedValue = "Yes" Then

            txtStockCodeFrom.Enabled = False
            txtStockCodeTo.Enabled = False

            '-- ADDITIONAL CODE TO BE ADDED
            If Not ViewState("_NEW") Is Nothing And ViewState("_NEW") = False Then
                tbpNewItem.Visible = False
            Else
                tbcItem.ActiveTab = tbpPrintItem
            End If

        Else

            txtStockCodeFrom.Enabled = True
            txtStockCodeTo.Enabled = True

            '-- ADDITIONAL CODE TO BE ADDED
            If Not ViewState("_NEW") Is Nothing And ViewState("_NEW") = False Then
                tbpNewItem.Visible = False
            Else
                tbcItem.ActiveTab = tbpPrintItem
            End If

        End If
    End Sub

    ''' <summary>
    ''' btnReportClear - Click;
    ''' 25 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnReportClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReportClear.Click

        ddlPrintOption.SelectedIndex = -1
        ddlSortBy.SelectedIndex = -1
        rblStockCode.Items.Item(0).Selected = False
        rblStockCode.Items.Item(1).Selected = False

        'ddlStockCodeFrom.SelectedIndex = -1
        'ddlStockCodeTo.SelectedIndex = -1

        txtStockCodeFrom.Text = String.Empty
        txtStockCodeTo.Text = String.Empty

        For Each item As ListItem In chkStockType.Items
            item.Selected = False
        Next

        lblErrPrintItem.Visible = False

    End Sub

#End Region

#Region " Sub Procedures and Functions "

    ''' <summary>
    ''' Sub Proc - BindStockCode;
    ''' 12 Jan 09 - Jianfa;
    ''' </summary>
    ''' <remarks>
    ''' CHANGE REQ:
    ''' 24 Feb 09 - To use cache instead of re-loading items
    ''' </remarks>
    Private Sub BindStockCode()

        Try

            Dim Client As New ServiceClient
            Dim ItemSearch As New ItemDetails

            ItemSearch.StoreID = Session("StoreID")
            ItemSearch.ItemID = txtLocateStockCode.Text.Trim.ToUpper
            ItemSearch.Location = txtLocateLocation.Text.Trim
            ItemSearch.Status = ddlLocateStatus.SelectedValue
            ItemSearch.EquipmentID = String.Empty

            'ItemList = Client.GetItems(ItemSearch, String.Empty, String.Empty)
            If Cache(Session(ESession.StoreID.ToString) & ECache.ItemList) Is Nothing Then
                Cache(Session(ESession.StoreID.ToString) & ECache.ItemList) = Client.GetItems(ItemSearch, String.Empty, String.Empty)
            End If
            Client.Close()

            'ddlStockCodeFrom.DataSource = Cache(Session(ESession.StoreID.ToString) & ECache.ItemList)
            'ddlStockCodeFrom.DataValueField = "ItemID"
            'ddlStockCodeFrom.DataTextField = "ItemID"
            'ddlStockCodeFrom.DataBind()

            'ddlStockCodeFrom.Items.Insert(0, New ListItem(" - Please Select - ", ""))

            '---------------------------------------------------------------------------------------
            '-- NOT NEEDED SINCE DATA WILL BE POPULATED FROM ddlStockCode SelectedIndexChanged
            '---------------------------------------------------------------------------------------
            'ddlStockCodeTo.DataSource = Cache(Session(ESession.StoreID.ToString) & ECache.ItemList)
            'ddlStockCodeTo.DataValueField = "ItemID"
            'ddlStockCodeTo.DataTextField = "ItemID"

            'ddlStockCodeTo.Items.Insert(0, New ListItem(" - Please Select - ", ""))
            'ddlStockCodeTo.Enabled = False
            ''ddlStockCodeTo.DataBind()


        Catch ex As FaultException

            lblErrAddItem.Text = ex.Message
            lblErrAddItem.Visible = True

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

    ''' <summary>
    ''' Sub Proc - BindUOM
    ''' 07 Jan 09 - Jianfa;
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindUOM()

        Dim View As DataView
        View = clsCommonFunction.GetCommonDataByCodeGroup(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), clsCommonFunction.codeGroup.UOM, "O")

        ddlAddUOM.DataSource = View
        ddlAddUOM.DataValueField = "CommonCodeID"
        ddlAddUOM.DataTextField = "CommonCodeDescription"
        ddlAddUOM.DataBind()

        ddlAddUOM.Items.Insert(0, New ListItem(" - Please Select - ", ""))

        ddlEditUOM.DataSource = View
        ddlEditUOM.DataValueField = "CommonCodeID"
        ddlEditUOM.DataTextField = "CommonCodeDescription"
        ddlEditUOM.DataBind()

        ddlEditUOM.Items.Insert(0, New ListItem(" - Please Select - ", ""))

    End Sub

    ''' <summary>
    ''' Sub Proc - Bind Stock Type
    ''' 07 Jan 09 - Jianfa;
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindStockType()

        Dim View As DataView
        View = clsCommonFunction.GetCommonDataByCodeGroup(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), clsCommonFunction.codeGroup.StockType, "O", "D")

        ddlAddStockType.DataSource = View
        ddlAddStockType.DataValueField = "CommonCodeID"
        ddlAddStockType.DataTextField = "CommonCodeDescription"
        ddlAddStockType.DataBind()

        ddlAddStockType.Items.Insert(0, New ListItem(" - Please Select - ", ""))

        ddlEditStockType.DataSource = View
        ddlEditStockType.DataValueField = "CommonCodeID"
        ddlEditStockType.DataTextField = "CommonCodeDescription"
        ddlEditStockType.DataBind()

        ddlEditStockType.Items.Insert(0, New ListItem(" - Please Select - ", ""))

        chkStockType.DataSource = View
        chkStockType.DataValueField = "CommonCodeID"
        chkStockType.DataTextField = "CommonCodeDescription"
        chkStockType.DataBind()

    End Sub

    ''' <summary>
    ''' Sub Proc - Bind Sub Type
    ''' 07 Jan 09 - Jianfa;
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindSubType()

        Dim View As DataView
        View = clsCommonFunction.GetCommonDataByCodeGroup(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), clsCommonFunction.codeGroup.SubType, "O")

        ddlAddSubType.DataSource = View
        ddlAddSubType.DataValueField = "CommonCodeID"
        ddlAddSubType.DataTextField = "CommonCodeDescription"
        ddlAddSubType.DataBind()

        ddlAddSubType.Items.Insert(0, New ListItem(" - Please Select - ", ""))

        ddlEditSubType.DataSource = View
        ddlEditSubType.DataValueField = "CommonCodeID"
        ddlEditSubType.DataTextField = "CommonCodeDescription"
        ddlEditSubType.DataBind()

        ddlEditSubType.Items.Insert(0, New ListItem(" - Please Select - ", ""))

    End Sub

    ''' <summary>
    ''' Sub Proc - BindEquipmentNo;
    ''' 07 Jan 09 - Jianfa;
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindEquipmentNo()

        Try

            Dim Client As New ServiceClient
            Dim EquipmentDetails As New EquipmentDetails
            Dim EquipmentList As New List(Of EquipmentDetails)

            EquipmentDetails.StoreID = Session("StoreID")
            EquipmentDetails.EquipmentID = String.Empty
            EquipmentDetails.EquipmentType = String.Empty
            EquipmentDetails.EquipmentDescription = String.Empty
            EquipmentDetails.Status = "O"

            EquipmentList = Client.GetEquipments(EquipmentDetails, String.Empty, String.Empty)
            Client.Close()

            ddlAddEquipmentNo.DataSource = EquipmentList
            ddlAddEquipmentNo.DataValueField = "EquipmentID"
            ddlAddEquipmentNo.DataTextField = "EquipmentID_Description"
            ddlAddEquipmentNo.DataBind()

            ddlAddEquipmentNo.Items.Insert(0, New ListItem(" - Please Select - ", ""))

            ddlEditEquipmentNo.DataSource = EquipmentList
            ddlEditEquipmentNo.DataValueField = "EquipmentID"
            ddlEditEquipmentNo.DataTextField = "EquipmentID_Description"
            ddlEditEquipmentNo.DataBind()

            ddlEditEquipmentNo.Items.Insert(0, New ListItem(" - Please Select - ", ""))


        Catch ex As FaultException

            lblErrLocateItem.Text = ex.Message
            lblErrLocateItem.Visible = True

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

    ''' <summary>
    ''' Function - IsValidEquipmentNo;
    ''' 07 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="equipmentNo"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' CODE LOGIC:
    ''' 1) Determine if Equipment No is Valid 
    ''' </remarks>
    Private Function IsValidEquipmentNo(ByVal equipmentNo As String) As Boolean

        If ddlAddEquipmentNo.Items.Count = 1 Then
            Return False
        Else

            For idx As Integer = 1 To ddlAddEquipmentNo.Items.Count - 1

                If ddlAddEquipmentNo.Items(idx).Value = equipmentNo Then

                    ddlAddEquipmentNo.SelectedValue = equipmentNo
                    ddlAddEquipmentNo.Enabled = False

                    ddlEditEquipmentNo.SelectedValue = equipmentNo
                    ddlEditEquipmentNo.Enabled = False

                    Return True

                End If
            Next

        End If

        Return False

    End Function

    ''' <summary>
    ''' Function - IsValidSubType;
    ''' 07 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="subType"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' CODE LOGIC:
    ''' 1) Determine if the Sub Type is Valid
    ''' </remarks>
    Private Function IsValidSubType(ByVal subType As String) As Boolean

        If ddlAddSubType.Items.Count = 1 Then
            Return False
        Else

            For idx As Integer = 1 To ddlAddSubType.Items.Count - 1

                If ddlAddSubType.Items(idx).Value = subType Then

                    ddlAddSubType.SelectedValue = subType
                    ddlAddSubType.Enabled = False

                    ddlEditSubType.SelectedValue = subType
                    ddlEditSubType.Enabled = False

                    Return True

                End If
            Next

        End If

        Return False

    End Function

    ''' <summary>
    ''' Sub Procedure - DisableEditMode;
    ''' 01 Jan 09 - To enable/disable UI controls for Edit
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    Private Sub EditMode(ByVal Edit As Boolean)

        Select Case Edit

            Case False
                txtEditPartNo.Enabled = False
                txtEditDescription.Enabled = False
                txtEditLocation.Enabled = False
                txtEditLocation2.Enabled = False
                txtEditMaxLevel.Enabled = False
                txtEditMinLevel.Enabled = False
                txtEditReOrderLevel.Enabled = False
                ddlEditEquipmentNo.Enabled = False
                ddlEditStockType.Enabled = False
                ddlEditSubType.Enabled = False
                ddlEditUOM.Enabled = False
                txtEditOpeningBal.Enabled = False
                txtEditOpeningTotalValue.Enabled = False
                btnLocateSave.Enabled = False
            Case True
                txtEditPartNo.Enabled = True
                txtEditDescription.Enabled = True
                txtEditLocation.Enabled = True
                txtEditLocation2.Enabled = True
                txtEditMaxLevel.Enabled = True
                txtEditMinLevel.Enabled = True
                txtEditReOrderLevel.Enabled = True
                ddlEditEquipmentNo.Enabled = True
                ddlEditStockType.Enabled = True
                ddlEditSubType.Enabled = True
                ddlEditUOM.Enabled = True
                txtEditOpeningBal.Enabled = True
                txtEditOpeningTotalValue.Enabled = True
                btnLocateSave.Enabled = True
        End Select

    End Sub
#End Region

    <WebMethod()> _
    <Script.Services.ScriptMethod()> _
    Public Shared Function GetStockItems(ByVal prefixText As String, _
                                         ByVal count As Integer, _
                                         ByVal contextKey As String) _
                                            As List(Of String)

        Dim Client As New ServiceClient
        Dim ItemSearch As New ItemDetails
        Dim ItemList As New List(Of String)

        Try

            ItemSearch.StoreID = contextKey
            ItemSearch.ItemID = prefixText

            ItemList = Client.GetItemSearch(ItemSearch)

            Client.Close()

        Catch ex As Exception
            Throw
        End Try

        Return ItemList

    End Function

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)

    End Sub

End Class