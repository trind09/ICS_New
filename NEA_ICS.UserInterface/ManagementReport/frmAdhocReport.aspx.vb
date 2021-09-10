Imports Microsoft.Reporting.WebForms
Imports System.Web.Services.Description
Imports NEA_ICS.UserInterface.NEA_ICS.WcfService
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ServiceModel
Imports System.Reflection

Partial Public Class frmAdhocReport
    Inherits clsCommonFunction

    Private Message As String = EMPTY

#Region " PROPERTIES "

    Private _dataValue As String
    Public Property DataValue() As String
        Get
            Return _dataValue
        End Get
        Set(ByVal value As String)
            _dataValue = value
        End Set
    End Property

    Private _dataText As String
    Public Property DataText() As String
        Get
            Return _dataText
        End Get
        Set(ByVal value As String)
            _dataText = value
        End Set
    End Property

#End Region

#Region " PAGE Functions "

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
            ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", "alert('" & Message & "');", True)
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Dim AccessRights As New List(Of RoleDetails)

            CheckValidSession()

            If Not Page.IsPostBack Then
                AccessRights = clsCommonFunction.AssignAccessRights(Cache(Session(ESession.UserID.ToString) & Session(ESession.StoreID.ToString) & "AccessRights"), _
                                        clsCommonFunction.moduleID.AdhocReport)

                If AccessRights(0).SelectRight = False Then
                    Server.Transfer("..\frmUnauthorisedPage.aspx")
                    Exit Sub
                End If
            End If

        Catch ex As Exception

            Server.Transfer("..\frmUnauthorisedPage.aspx")
            Exit Sub

        End Try

    End Sub
#End Region

#Region " AD HOC REPORTS "
    ''' <summary>
    ''' btnGo - Click;
    ''' 29 Mar 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGo.Click

        If ddlView.SelectedIndex <= 0 Then

            Message = GetMessage(messageID.MandatoryField)
            ddlView.Enabled = True

        Else

            ddlView.Enabled = False
            pnlDetails.Visible = True
            BindDataItems()

            Message = "* Note:\n\nData Item of [Date] Data Type should be entered as [dd/mm/yyyy] format. \n\nPlease enter Code for if Status is selected: \n(E.g) O = Open, C = Closed, A = Approved, R = Rejected"

        End If

    End Sub

    ''' <summary>
    ''' btnClear - Click;
    ''' 29 Mar 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click

        ddlView.SelectedIndex = 0
        pnlDetails.Visible = False
        ddlView.Enabled = True

    End Sub

    ''' <summary>
    ''' btnSelectAll - Click;
    ''' 30 Mar 09 - Jianfa
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSelectAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectAll.Click

        For Each item As ListItem In chkAdHocReport.Items
            item.Selected = True
        Next

    End Sub

    ''' <summary>
    ''' btnUnSelectAll - Click;
    ''' 30 Mar 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnUnSelectAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUnSelectAll.Click

        For Each item As ListItem In chkAdHocReport.Items
            item.Selected = False
        Next

    End Sub

    ''' <summary>
    ''' gdvCondition - RowDataBound;
    ''' 30 Mar 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gdvCondition_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvCondition.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            '-- Bind to Dropdown List 
            For Each item As ListItem In chkAdHocReport.Items
                CType(e.Row.FindControl("ddlDataItem"), DropDownList).Items.Add(item)
            Next

        End If

    End Sub

#End Region

#Region " Sub Procedures and Functions "

    ''' <summary>
    ''' Sub Proc - BindDataItems;
    ''' 29 Mar 09 - Jianfa;
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindDataItems()

        Dim GList As New List(Of Integer)
        chkAdHocReport.Items.Clear()
        ddlSortBy.Items.Clear()

        Select Case ddlView.SelectedIndex

            Case 1 ' Stock Transactions

                chkAdHocReport.Items.Add(New ListItem("Transaction Type", "t.StockTransactionType"))
                chkAdHocReport.Items.Add(New ListItem("Stock Code", "t.StockTransactionStockItemID"))
                chkAdHocReport.Items.Add(New ListItem("Stock Description", "i.StockItemDescription"))
                chkAdHocReport.Items.Add(New ListItem("Transaction Date", "convert(varchar, t.StockTransactionDte, 103)"))
                chkAdHocReport.Items.Add(New ListItem("Qty", "t.StockTransactionQty"))
                chkAdHocReport.Items.Add(New ListItem("Total Cost", "t.StockTransactionTotalCost"))
                chkAdHocReport.Items.Add(New ListItem("Status", "t.StockTransactionStatus"))
                chkAdHocReport.Items.Add(New ListItem("Created Date", "convert(varchar, t.StockTransactionCreateDte, 103)"))
                chkAdHocReport.Items.Add(New ListItem("Created User ID", "t.StockTransactionCreateUserID"))
                chkAdHocReport.Items.Add(New ListItem("Updated Date", "convert(varchar, t.StockTransactionUpdateDte, 103)"))
                chkAdHocReport.Items.Add(New ListItem("Updated User ID", "t.StockTransactionUpdateUserID"))

            Case 2 ' Order

                chkAdHocReport.Items.Add(New ListItem("Order Reference", "o.OrderID"))
                chkAdHocReport.Items.Add(New ListItem("GeBiz PO No", "o.OrderGebizPONo"))
                chkAdHocReport.Items.Add(New ListItem("Order Date", "convert(varchar, o.OrderDte, 103)"))
                chkAdHocReport.Items.Add(New ListItem("Supplier Code", "o.OrderSupplierID"))
                chkAdHocReport.Items.Add(New ListItem("Supplier Company Name", "s.SupplierCompanyName"))
                chkAdHocReport.Items.Add(New ListItem("Stock Code", "oi.OrderItemStockItemID"))
                chkAdHocReport.Items.Add(New ListItem("Stock Description", "i.StockItemDescription"))
                chkAdHocReport.Items.Add(New ListItem("Order Item Qty", "oi.OrderItemQty"))
                chkAdHocReport.Items.Add(New ListItem("Total Cost", "oi.OrderItemTotalCost"))
                chkAdHocReport.Items.Add(New ListItem("Expected Delivery Date", "convert(varchar, oi.OrderItemExpectedDeliveryDte, 103)"))
                chkAdHocReport.Items.Add(New ListItem("Warranty Date", "convert(varchar, oi.OrderItemWarrantyDte, 103)"))
                chkAdHocReport.Items.Add(New ListItem("Remarks", "oi.OrderItemRemarks"))
                chkAdHocReport.Items.Add(New ListItem("Status", "oi.OrderItemStatus"))
                chkAdHocReport.Items.Add(New ListItem("Created Date", "convert(varchar, oi.OrderItemCreateDte, 103)"))
                chkAdHocReport.Items.Add(New ListItem("Created User ID", "oi.OrderItemCreateUserID"))
                chkAdHocReport.Items.Add(New ListItem("Updated Date", "convert(varchar, oi.OrderItemUpdateDte, 103)"))
                chkAdHocReport.Items.Add(New ListItem("Updated User ID", "oi.OrderItemUpdateUserID"))

            Case 3 ' Request/Issue

                chkAdHocReport.Items.Add(New ListItem("Consumer Code", "r.RequestConsumerID"))
                chkAdHocReport.Items.Add(New ListItem("Consumer Description", "c.ConsumerDescription"))
                chkAdHocReport.Items.Add(New ListItem("Issue Reference", "r.RequestID"))
                chkAdHocReport.Items.Add(New ListItem("Approver User ID", "r.RequestApproverUserID"))
                chkAdHocReport.Items.Add(New ListItem("Approval Date", "convert(varchar, r.RequestApproveDte, 103)"))
                chkAdHocReport.Items.Add(New ListItem("Issuer User ID", "r.RequestIssuerUserID"))
                chkAdHocReport.Items.Add(New ListItem("Issued Date", "convert(varchar, r.RequestIssueDte, 103)"))
                chkAdHocReport.Items.Add(New ListItem("Stock Code", "ri.RequestItemStockItemID"))
                chkAdHocReport.Items.Add(New ListItem("Stock Description", "i.StockItemDescription"))
                chkAdHocReport.Items.Add(New ListItem("Request/Issue Item Qty", "ri.RequestItemQty"))
                chkAdHocReport.Items.Add(New ListItem("Status", "ri.RequestItemStatus"))
                chkAdHocReport.Items.Add(New ListItem("Created Date", "convert(varchar, ri.RequestItemCreateDte, 103)"))
                chkAdHocReport.Items.Add(New ListItem("Created User ID", "ri.RequestItemCreateUserID"))
                chkAdHocReport.Items.Add(New ListItem("Updated Date", "convert(varchar, ri.RequestItemUpdateDte, 103)"))
                chkAdHocReport.Items.Add(New ListItem("Updated User ID", "ri.RequestItemUpdateUserID"))

            Case 4 ' Adjustment

                chkAdHocReport.Items.Add(New ListItem("Document No", "a.AdjustID"))
                chkAdHocReport.Items.Add(New ListItem("Serial No", "a.AdjustSerialNo"))
                chkAdHocReport.Items.Add(New ListItem("Adjustment Date", "convert(varchar, a.AdjustDte, 103)"))
                chkAdHocReport.Items.Add(New ListItem("Stock Code", "ai.AdjustItemStockItemID"))
                chkAdHocReport.Items.Add(New ListItem("Stock Description", "i.StockItemDescription"))
                chkAdHocReport.Items.Add(New ListItem("Status", "ai.AdjustItemStatus"))
                chkAdHocReport.Items.Add(New ListItem("Created Date", "convert(varchar, ai.AdjustItemCreateDte, 103)"))
                chkAdHocReport.Items.Add(New ListItem("Created User ID", "ai.AdjustItemCreateUserID"))
                chkAdHocReport.Items.Add(New ListItem("Updated Date", "convert(varchar, ai.AdjustItemUpdateDte, 103)"))
                chkAdHocReport.Items.Add(New ListItem("Updated User ID", "ai.AdjustItemUpdateUserID"))

            Case 5 ' Direct Issue

                chkAdHocReport.Items.Add(New ListItem("Consumer Code", "d.DirectIssueConsumerID"))
                chkAdHocReport.Items.Add(New ListItem("Consumer Description", "c.ConsumerDescription"))
                chkAdHocReport.Items.Add(New ListItem("Document No", "d.DirectIssueDocNo"))
                chkAdHocReport.Items.Add(New ListItem("Serial No", "d.DirectIssueSerialNo"))
                chkAdHocReport.Items.Add(New ListItem("Direct Issued Date", "convert(varchar, d.DirectIssueDte, 103)"))
                chkAdHocReport.Items.Add(New ListItem("Stock Code", "di.DirectIssueItemStockItemID"))
                chkAdHocReport.Items.Add(New ListItem("Stock Description", "di.DirectIssueItemStockDescription"))
                chkAdHocReport.Items.Add(New ListItem("Direct Issue Item Qty", "di.DirectIssueItemQty"))
                chkAdHocReport.Items.Add(New ListItem("Total Cost", "di.DirectIssueItemTotalCost"))
                chkAdHocReport.Items.Add(New ListItem("Status", "di.DirectIssueItemStatus"))
                chkAdHocReport.Items.Add(New ListItem("Created Date", "di.DirectIssueItemCreateUserID"))
                chkAdHocReport.Items.Add(New ListItem("Created User ID", "di.DirectIssueItemCreateUserID"))
                chkAdHocReport.Items.Add(New ListItem("Updated Date", "convert(varchar, di.DirectIssueItemUpdateDte, 103)"))
                chkAdHocReport.Items.Add(New ListItem("Updated User ID", "di.DirectIssueItemUpdateUserID"))

        End Select

        For idx As Integer = 1 To 5
            GList.Add(idx)
        Next

        gdvCondition.DataSource = GList
        gdvCondition.DataBind()

        '-- Bind to Dropdown List 
        For Each item As ListItem In chkAdHocReport.Items
            ddlSortBy.Items.Add(item)
        Next

    End Sub

    ''' <summary>
    ''' Sub Proc - Selected Row Text
    ''' 30 Mar 09 - Jianfa
    ''' </summary>
    ''' <param name="idx"></param>
    ''' <remarks></remarks>
    Private Function SelectedRowText(ByVal idx As Integer) As String

        Try

            If chkAdHocReport.Items.Item(idx).Selected And idx <= chkAdHocReport.Items.Count - 1 Then
                Return chkAdHocReport.Items.Item(idx).Text.ToString
            Else
                Return String.Empty
            End If

        Catch ex As Exception

            Return String.Empty

        End Try

    End Function

#End Region

    ''' <summary>
    ''' btnExcel - Click;
    ''' 30 Mar 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcel.Click

        rvr.LocalReport.DataSources.Clear()
        rvr.LocalReport.DataSources.Add(New ReportDataSource("AdHocReport", ObjectDataSource1))

        Dim ListToSelect As New List(Of String)
        Dim idx As Integer

        For Each item As ListItem In chkAdHocReport.Items

            If item.Selected Then
                ListToSelect.Add(item.Text)
            End If

        Next

        idx = ListToSelect.Count

        Do Until idx > 18
            ListToSelect.Add(String.Empty)
            idx += 1
        Loop

        Dim parameterlist As New List(Of ReportParameter)
        Dim p0 As New ReportParameter("StoreName", Session("StoreName").ToString())
        Dim p1 As New ReportParameter("p1", ListToSelect.Item(0).ToString)
        Dim p2 As New ReportParameter("p2", ListToSelect.Item(1).ToString)
        Dim p3 As New ReportParameter("p3", ListToSelect.Item(2).ToString)
        Dim p4 As New ReportParameter("p4", ListToSelect.Item(3).ToString)
        Dim p5 As New ReportParameter("p5", ListToSelect.Item(4).ToString)
        Dim p6 As New ReportParameter("p6", ListToSelect.Item(5).ToString)
        Dim p7 As New ReportParameter("p7", ListToSelect.Item(6).ToString)
        Dim p8 As New ReportParameter("p8", ListToSelect.Item(7).ToString)
        Dim p9 As New ReportParameter("p9", ListToSelect.Item(8).ToString)
        Dim p10 As New ReportParameter("p10", ListToSelect.Item(9).ToString)
        Dim p11 As New ReportParameter("p11", ListToSelect.Item(10).ToString)
        Dim p12 As New ReportParameter("p12", ListToSelect.Item(11).ToString)
        Dim p13 As New ReportParameter("p13", ListToSelect.Item(12).ToString)
        Dim p14 As New ReportParameter("p14", ListToSelect.Item(13).ToString)
        Dim p15 As New ReportParameter("p15", ListToSelect.Item(14).ToString)
        Dim p16 As New ReportParameter("p16", ListToSelect.Item(15).ToString)
        Dim p17 As New ReportParameter("p17", ListToSelect.Item(16).ToString)
        Dim p18 As New ReportParameter("p18", ListToSelect.Item(17).ToString)

        parameterlist.Add(p0)
        parameterlist.Add(p1)
        parameterlist.Add(p2)
        parameterlist.Add(p3)
        parameterlist.Add(p4)
        parameterlist.Add(p5)
        parameterlist.Add(p6)
        parameterlist.Add(p7)
        parameterlist.Add(p8)
        parameterlist.Add(p9)
        parameterlist.Add(p10)
        parameterlist.Add(p11)
        parameterlist.Add(p12)
        parameterlist.Add(p13)
        parameterlist.Add(p14)
        parameterlist.Add(p15)
        parameterlist.Add(p16)
        parameterlist.Add(p17)
        parameterlist.Add(p18)

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
        Response.AddHeader("content-disposition", "attachment;filename=AdHocReports.xls")
        Response.BinaryWrite(bytValue)
        Response.Flush()
        Response.End()

    End Sub

    ''' <summary>
    ''' ObjectDataSource1 - Selected;
    ''' 30 Mar 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ObjectDataSource1_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles ObjectDataSource1.Selected

        Try
            Dim returnList As List(Of AdHocReportDetails) = e.ReturnValue
            If returnList.Count <= 0 Then
                NoRecordFond = "Y"
            Else
                NoRecordFond = "N"
            End If
        Catch ex As Exception

        End Try

    End Sub

    ''' <summary>
    ''' ObjectDataSource1 - Selecting;
    ''' 30 Mar 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ObjectDataSource1_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs) Handles ObjectDataSource1.Selecting

        Dim strSelect As String = " SELECT "
        Dim strFrom As String = " FROM "
        Dim strWhere As String = " WHERE "
        Dim strOrderBy As String = " ORDER BY "
        Dim strJoin As String = String.Empty
        Dim strDataItems As String = String.Empty
        Dim strConditions As String = String.Empty
        Dim strStore As String = Session("StoreID").ToString
        Dim strSQL As String = String.Empty
        Dim bnlFirstRecord As Boolean = False

        For Each item As ListItem In chkAdHocReport.Items

            If item.Selected Then
                strDataItems &= item.Value & ","
            End If

        Next

        strDataItems = Left(strDataItems, strDataItems.Length - 1)

        Select Case ddlView.SelectedIndex

            Case 1 ' Stock Transactions
                strJoin = " INNER JOIN StockItem i ON i.StockItemID = t.StockTransactionStockItemID "
                strStore = String.Format(" t.StockTransactionStoreID = '{0}' ", strStore)

            Case 2 ' Order
                strJoin = " INNER JOIN StockItem i ON i.StockItemID = oi.OrderItemStockItemID "
                strJoin &= " INNER JOIN Supplier s ON s.SupplierID = o.OrderSupplierID "
                strStore = String.Format(" o.OrderStoreID = '{0}' ", strStore)

            Case 3 ' Request / Issue
                strJoin = " INNER JOIN StockItem i ON i.StockItemID = ri.RequestItemStockItemID "
                strJoin &= " INNER JOIN Consumer c ON c.ConsumerID = r.RequestConsumerID "
                strStore = String.Format(" r.RequestStoreID = '{0}' ", strStore)

            Case 4 ' Adjustment
                strJoin = " INNER JOIN StockItem i ON i.StockItemID = ai.AdjustItemStockItemID "
                strStore = String.Format(" a.AdjustStoreID = '{0}' ", strStore)

            Case 5 ' Direct Issue
                strJoin = " INNER JOIN Consumer c ON c.ConsumerID = d.DirectIssueConsumerID "
                strStore = String.Format(" d.DirectIssueStoreID = '{0}' ", strStore)

        End Select


        For Each gRow As GridViewRow In gdvCondition.Rows

            If CType(gRow.FindControl("chkInclude"), CheckBox).Checked Then

                Dim strOperator As String = String.Empty

                Select Case CType(gRow.FindControl("ddlOperator"), DropDownList).SelectedValue.ToString
                    Case "0" ' =
                        strOperator = " = "
                    Case "1" ' <>
                        strOperator = " <> "
                    Case "2" ' >
                        strOperator = " > "
                    Case "3" ' >=
                        strOperator = " >= "
                    Case "4" ' <
                        strOperator = " < "
                    Case "5" ' <=
                        strOperator = " <= "
                    Case "6" ' %
                        strOperator = " LIKE "
                End Select

                If Not bnlFirstRecord Then

                    bnlFirstRecord = True
                    strConditions = " AND " & CType(gRow.FindControl("ddlDataItem"), DropDownList).SelectedValue.Replace("convert(varchar", "convert(datetime") & _
                                    strOperator

                    If CType(gRow.FindControl("ddlOperator"), DropDownList).SelectedValue.ToString = "6" Then
                        strConditions &= "'%" & CType(gRow.FindControl("txtCriteria"), TextBox).Text & "%' "
                    Else

                        If CType(gRow.FindControl("ddlDataItem"), DropDownList).SelectedValue.ToUpper.Contains("DTE") Then
                            strConditions &= "convert(datetime, '" & CType(gRow.FindControl("txtCriteria"), TextBox).Text & "', 103) "
                        Else
                            strConditions &= "'" & CType(gRow.FindControl("txtCriteria"), TextBox).Text & "' "
                        End If

                    End If

                Else

                    strConditions &= CType(gRow.FindControl("ddlOperand"), DropDownList).SelectedValue & _
                                    CType(gRow.FindControl("ddlDataItem"), DropDownList).SelectedValue.Replace("convert(varchar", "convert(datetime") & _
                                    strOperator

                    If CType(gRow.FindControl("ddlOperator"), DropDownList).SelectedValue.ToString = "6" Then
                        strConditions &= "'%" & CType(gRow.FindControl("txtCriteria"), TextBox).Text & "%' "
                    Else

                        If CType(gRow.FindControl("ddlDataItem"), DropDownList).SelectedValue.ToUpper.Contains("DTE") Then
                            strConditions &= "convert(datetime, '" & CType(gRow.FindControl("txtCriteria"), TextBox).Text & "', 103) "
                        Else
                            strConditions &= "'" & CType(gRow.FindControl("txtCriteria"), TextBox).Text & "' "
                        End If

                    End If

                End If

            End If

        Next

        strSQL = strSelect & strDataItems & strFrom & ddlView.SelectedValue & strJoin & strWhere & strStore
        strSQL &= strConditions
        strSQL &= strOrderBy & ddlSortBy.SelectedValue & " " & ddlSortDirection.SelectedValue

        Dim adHocReportDetails As New AdHocReportDetails
        adHocReportDetails.SQLStatement = strSQL

        e.InputParameters("AdHocReport") = adHocReportDetails
        e.InputParameters("returnMessage") = Message

    End Sub

End Class