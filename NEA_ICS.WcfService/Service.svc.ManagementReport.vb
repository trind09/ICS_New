Imports NEA_ICS.Business
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Partial Public Class Service

#Region " Management Report "
    ''' <summary>
    ''' Get the list of Suppliers based on the search criteria;
    ''' 29 Dec 08 - Guo Feng;
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRackLocation(ByVal storeId As String) As List(Of String) Implements IService.GetRackLocation

        Dim RackLocations As New List(Of String)
        Try

            Dim RackLocationRetrieved As New DataSet
            RackLocations.Clear()

            RackLocationRetrieved = ManagementReportBL.GetRackLocation(storeId)
            For Each col As DataColumn In RackLocationRetrieved.Tables(0).Columns
                col.ReadOnly = False
            Next

            If RackLocationRetrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In RackLocationRetrieved.Tables(0).Rows
                    row = FillRowWithNull(row, RackLocationRetrieved.Tables(0).Columns)

                    Dim RackLocationItem As String = row("StockItemLocation")
                    RackLocations.Add(RackLocationItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return RackLocations
    End Function

    ''' <summary>
    ''' Get the list of StockCode based on the search criteria;
    ''' 29 Dec 08 - Guo Feng;
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetStockCode(ByVal storeId As String) As List(Of String) Implements IService.GetStockCode

        Dim StockCodes As New List(Of String)
        Try

            Dim StockCodeRetrieved As New DataSet
            StockCodes.Clear()

            StockCodeRetrieved = ManagementReportBL.GetStockCode(storeId)
            For Each col As DataColumn In StockCodeRetrieved.Tables(0).Columns
                col.ReadOnly = False
            Next

            If StockCodeRetrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In StockCodeRetrieved.Tables(0).Rows
                    row = FillRowWithNull(row, StockCodeRetrieved.Tables(0).Columns)

                    Dim StockCodeItem As String = row("StockItemID")
                    StockCodes.Add(StockCodeItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return StockCodes
    End Function

    ''' <summary>
    ''' Get the list of Suppliers based on the search criteria;
    ''' 29 Dec 08 - Guo Feng;
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="rackLocationFrom"></param>
    ''' <param name="rackLocationTo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRackItemBalance(ByVal storeId As String, ByVal rackLocationFrom As String, ByVal rackLocationTo As String) As List(Of RackItemBalanceDetails) Implements IService.GetRackItemBalance

        Dim RackItemBalanceDetailsList As New List(Of RackItemBalanceDetails)
        Try

            Dim RackItemBalanceRetrieved As New DataSet
            RackItemBalanceDetailsList.Clear()

            RackItemBalanceRetrieved = ManagementReportBL.GetMR001GetRackItemBalance(storeId, rackLocationFrom, rackLocationTo)
            For Each col As DataColumn In RackItemBalanceRetrieved.Tables(0).Columns
                col.ReadOnly = False
            Next

            If RackItemBalanceRetrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In RackItemBalanceRetrieved.Tables(0).Rows
                    row = FillRowWithNull(row, RackItemBalanceRetrieved.Tables(0).Columns)

                    Dim RackItemBalanceDetailsItem As New RackItemBalanceDetails

                    RackItemBalanceDetailsItem.StoreID = row("StockItemStoreID")
                    RackItemBalanceDetailsItem.StockID = row("StockItemID")
                    RackItemBalanceDetailsItem.Description = row("StockItemDescription")
                    RackItemBalanceDetailsItem.Location = row("StockItemLocation")
                    RackItemBalanceDetailsItem.Location2 = row("StockItemLocation2")
                    RackItemBalanceDetailsItem.UOM = row("StockItemUOM")
                    If Not row.IsNull("StockBalance") Then
                        RackItemBalanceDetailsItem.StockBalance = row("StockBalance")
                    End If

                    RackItemBalanceDetailsList.Add(RackItemBalanceDetailsItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return RackItemBalanceDetailsList
    End Function

    ''' <summary>
    ''' Get the list of TransactionList based on the search criteria;
    ''' 29 Dec 08 - Guo Feng;
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="stockCodeFrom"></param>
    ''' <param name="stockCodeTo"></param>
    ''' <param name="transDateFrom"></param>
    ''' <param name="transDateTo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetTransactionList(ByVal storeId As String, ByVal stockCodeFrom As String, ByVal stockCodeTo As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime) As List(Of TransactionListDetails) Implements IService.GetTransactionList

        Dim TransactionList As New List(Of TransactionListDetails)
        Try

            Dim TransactionListRetrieved As New DataSet
            TransactionList.Clear()

            TransactionListRetrieved = ManagementReportBL.GetMR002GetTransactionList(storeId, stockCodeFrom, stockCodeTo, transDateFrom, transDateTo, "")
            For Each col As DataColumn In TransactionListRetrieved.Tables(0).Columns
                col.ReadOnly = False
            Next
            If TransactionListRetrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In TransactionListRetrieved.Tables(0).Rows
                    row = FillRowWithNull(row, TransactionListRetrieved.Tables(0).Columns)

                    Dim TransactionListItem As New TransactionListDetails

                    TransactionListItem.StoreID = row("StockItemStoreID")
                    TransactionListItem.StockID = row("StockItemID")
                    TransactionListItem.Description = row("StockItemDescription")
                    TransactionListItem.UOM = row("StockItemUOM")
                    TransactionListItem.TransType = row("StockTransactionType")
                    TransactionListItem.TransDate = row("StockTransactionDte")
                    ''UAT02.54
                    ''TransactionListItem.TransID = row("StockTransactionID")
                    ''If Not row.IsNull("StockTransactionItemRef") Then
                    ''    TransactionListItem.ItemRef = row("StockTransactionItemRef")
                    ''End If
                    TransactionListItem.DocNo = IIf(Not row.IsNull("DocNo"), row("DocNo"), "")
                    TransactionListItem.DocReturn = IIf(Not row.IsNull("DocReturn"), row("DocReturn"), "")
                    TransactionListItem.SerialNo = row("SerialNo")
                    TransactionListItem.ToOrFrom = row("ToOrFrom")
                    TransactionListItem.ReceiptsQty = row("ReceiptsQty")
                    TransactionListItem.IssuesQty = row("IssuesQty")
                    TransactionListItem.TotalCost = row("TotalCost")
                    TransactionListItem.BalanceQty = row("BalanceQty")
                    TransactionListItem.AUCost = row("AUCost")
                    TransactionListItem.TotalValue = row("TotalValue")
                    TransactionListItem.TotalQty = row("TotalQty")
                    TransactionListItem.UnitCost = row("UnitCost")

                    TransactionList.Add(TransactionListItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return TransactionList
    End Function
    Public Function GetMR001GetRackItemBalance(ByVal storeId As String, ByVal rackLocationFrom As String, ByVal rackLocationTo As String) As List(Of MR001GetRackItemBalanceDetails) Implements IService.GetMR001GetRackItemBalance

        Dim detailsList As New List(Of MR001GetRackItemBalanceDetails)
        Try

            Dim retrievedDS As New DataSet

            retrievedDS = ManagementReportBL.GetMR001GetRackItemBalance(storeId, rackLocationFrom, rackLocationTo)
            For Each col As DataColumn In retrievedDS.Tables(0).Columns
                col.ReadOnly = False
            Next

            If retrievedDS.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In retrievedDS.Tables(0).Rows
                    row = FillRowWithNull(row, retrievedDS.Tables(0).Columns)

                    Dim detailsItem As New MR001GetRackItemBalanceDetails

                    detailsItem.StockItemStoreID = row("StockItemStoreID")
                    detailsItem.StockItemID = row("StockItemID")
                    detailsItem.StockItemDescription = row("StockItemDescription")
                    detailsItem.StockItemLocation = row("StockItemLocation")
                    detailsItem.StockItemLocation2 = row("StockItemLocation2")
                    detailsItem.StockItemUOM = row("StockItemUOM")
                    If Not row.IsNull("StockBalance") Then
                        detailsItem.StockBalance = row("StockBalance")
                    End If

                    detailsList.Add(detailsItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return detailsList
    End Function
    Public Function GetMR002GetTransactionList(ByVal storeId As String, ByVal stockCodeFrom As String, ByVal stockCodeTo As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime, ByVal directIssue As String, ByVal equipmentID As String) As List(Of MR002GetTransactionListDetails) Implements IService.GetMR002GetTransactionList

        Dim detailsList As New List(Of MR002GetTransactionListDetails)
        Try

            Dim retrievedDS As New DataSet

            retrievedDS = ManagementReportBL.GetMR002GetTransactionList(storeId, stockCodeFrom, stockCodeTo, transDateFrom, transDateTo, equipmentID)
            For Each col As DataColumn In retrievedDS.Tables(0).Columns
                col.ReadOnly = False
            Next

            Dim issueDS As New DataSet
            If directIssue = "Y" Then
                issueDS = ManagementReportBL.GetMR002GetDirectIssueList(storeId, stockCodeFrom, stockCodeTo, transDateFrom, transDateTo)
                For Each col As DataColumn In issueDS.Tables(0).Columns
                    col.ReadOnly = False
                Next

                '-- UAT 02.48 When checking Direct Issue Error Found
                ''retrievedDS.Merge(issueDS, True)
            End If

            If retrievedDS.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In retrievedDS.Tables(0).Rows
                    row = FillRowWithNull(row, retrievedDS.Tables(0).Columns)

                    Dim detailsItem As New MR002GetTransactionListDetails

                    detailsItem.StockItemStoreID = row("StockItemStoreID")
                    detailsItem.StockItemID = row("StockItemID")
                    detailsItem.StockItemDescription = row("StockItemDescription")
                    detailsItem.StockItemUOM = row("StockItemUOM")
                    detailsItem.StockTransactionType = row("StockTransactionType")
                    detailsItem.StockTransactionDte = row("StockTransactionDte")
                    '' UAT02.54
                    'detailsItem.StockTransactionID = row("StockTransactionID")
                    'If Not row.IsNull("StockTransactionItemRef") Then
                    '    detailsItem.StockTransactionItemRef = row("StockTransactionItemRef")
                    'End If
                    detailsItem.DocNo = IIf(Not row.IsNull("DocNo"), row("DocNo"), "")
                    detailsItem.DocReturn = IIf(Not row.IsNull("DocReturn"), row("DocReturn"), "")
                    detailsItem.SerialNo = IIf(Not row.IsNull("SerialNo"), row("SerialNo"), "")
                    detailsItem.ToOrFrom = IIf(Not row.IsNull("ToOrFrom"), row("ToOrFrom"), "")
                    detailsItem.ReceiptsQty = row("ReceiptsQty")
                    detailsItem.IssuesQty = row("IssuesQty")
                    detailsItem.TotalCost = row("TotalCost")
                    detailsItem.BalanceQty = row("BalanceQty")
                    detailsItem.AUCost = row("AUCost")
                    detailsItem.TotalValue = row("TotalValue")
                    detailsItem.TotalQty = row("TotalQty")
                    detailsItem.UnitCost = row("UnitCost")

                    detailsList.Add(detailsItem)

                Next
            End If

            '-- DIRECT ISSUES
            If directIssue = "Y" Then
                If issueDS.Tables(0).Rows.Count > 0 Then
                    For Each row As DataRow In issueDS.Tables(0).Rows
                        row = FillRowWithNull(row, issueDS.Tables(0).Columns)

                        Dim detailsItem As New MR002GetTransactionListDetails

                        detailsItem.StockItemStoreID = row("StockItemStoreID")
                        detailsItem.StockItemID = row("StockItemID")
                        detailsItem.StockItemDescription = row("StockItemDescription")
                        detailsItem.StockItemUOM = row("StockItemUOM")
                        detailsItem.StockTransactionType = row("StockTransactionType")
                        detailsItem.StockTransactionDte = row("StockTransactionDte")
                        '' UAT02.54
                        'detailsItem.StockTransactionID = row("StockTransactionID")
                        'If Not row.IsNull("StockTransactionItemRef") Then
                        '    detailsItem.StockTransactionItemRef = row("StockTransactionItemRef")
                        'End If
                        detailsItem.DocNo = IIf(Not row.IsNull("DocNo"), row("DocNo"), "")
                        detailsItem.DocReturn = IIf(Not row.IsNull("DocReturn"), row("DocReturn"), "")
                        detailsItem.SerialNo = IIf(Not row.IsNull("SerialNo"), row("SerialNo"), "")
                        detailsItem.ToOrFrom = IIf(Not row.IsNull("ToOrFrom"), row("ToOrFrom"), "")
                        detailsItem.ReceiptsQty = row("ReceiptsQty")
                        detailsItem.IssuesQty = row("IssuesQty")
                        detailsItem.TotalCost = row("TotalCost")
                        detailsItem.BalanceQty = row("BalanceQty")
                        detailsItem.AUCost = row("AUCost")
                        detailsItem.TotalValue = row("TotalValue")
                        detailsItem.TotalQty = row("TotalQty")
                        detailsItem.UnitCost = row("UnitCost")

                        detailsList.Add(detailsItem)

                    Next
                End If
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return detailsList
    End Function
    Public Function GetMR003IssueDocumentDetails(ByVal storeId As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime, ByVal directIssue As String) As List(Of MR003IssueDocumentDetails) Implements IService.GetMR003IssueDocumentDetails

        Dim detailsList As New List(Of MR003IssueDocumentDetails)
        Try

            Dim retrievedDS As New DataSet

            retrievedDS = ManagementReportBL.GetMR003IssueDocumentDetails(storeId, transDateFrom, transDateTo)
            For Each col As DataColumn In retrievedDS.Tables(0).Columns
                col.ReadOnly = False
            Next

            Dim issueDS As New DataSet
            If directIssue = "Y" Then
                issueDS = ManagementReportBL.GetMR003DirectIssue(storeId, transDateFrom, transDateTo)
                For Each col As DataColumn In issueDS.Tables(0).Columns
                    col.ReadOnly = False
                Next

                '-- UAT.02.48
                ''retrievedDS.Merge(issueDS, True)
            End If

            If retrievedDS.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In retrievedDS.Tables(0).Rows
                    row = FillRowWithNull(row, retrievedDS.Tables(0).Columns)

                    Dim detailsItem As New MR003IssueDocumentDetails

                    detailsItem.IssueConsumerID = row("IssueConsumerID")
                    detailsItem.IssueDte = row("IssueDte")
                    detailsItem.IssueSerialNo = row("IssueSerialNo")
                    detailsItem.IssueID = row("IssueID")
                    detailsItem.IssueStockItemDesc = row("IssueStockItemDesc")
                    detailsItem.IssueStockItemID = row("IssueStockItemID")
                    If Not row.IsNull("IssueTotalCost") Then
                        detailsItem.IssueTotalCost = row("IssueTotalCost")
                    End If
                    If Not row.IsNull("IssueQty") Then
                        detailsItem.IssueQty = row("IssueQty")
                    End If
                    detailsItem.IssueType = row("IssueType")
                    If Not row.IsNull("IssueUnitCost") Then
                        detailsItem.IssueUnitCost = row("IssueUnitCost")
                    End If
                    detailsItem.IssueUOM = row("IssueUOM")

                    detailsList.Add(detailsItem)

                Next
            End If

            '-- DIRECT ISSUE
            If directIssue = "Y" Then

                If issueDS.Tables(0).Rows.Count > 0 Then
                    For Each row As DataRow In issueDS.Tables(0).Rows
                        row = FillRowWithNull(row, issueDS.Tables(0).Columns)

                        Dim detailsItem As New MR003IssueDocumentDetails

                        detailsItem.IssueConsumerID = row("IssueConsumerID")
                        detailsItem.IssueDte = row("IssueDte")
                        detailsItem.IssueSerialNo = row("IssueSerialNo")
                        detailsItem.IssueID = row("IssueID")
                        detailsItem.IssueStockItemDesc = row("IssueStockItemDesc")
                        detailsItem.IssueStockItemID = row("IssueStockItemID")
                        If Not row.IsNull("IssueTotalCost") Then
                            detailsItem.IssueTotalCost = row("IssueTotalCost")
                        End If
                        If Not row.IsNull("IssueQty") Then
                            detailsItem.IssueQty = row("IssueQty")
                        End If
                        detailsItem.IssueType = row("IssueType")
                        If Not row.IsNull("IssueUnitCost") Then
                            detailsItem.IssueUnitCost = row("IssueUnitCost")
                        End If
                        detailsItem.IssueUOM = row("IssueUOM")

                        detailsList.Add(detailsItem)

                    Next
                End If

            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return detailsList
    End Function
    Public Function GetMR004StockReviewList(ByVal storeId As String, ByVal stockCodeFrom As String, ByVal stockCodeTo As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime) As List(Of MR004StockReviewListDetails) Implements IService.GetMR004StockReviewList

        Dim detailsList As New List(Of MR004StockReviewListDetails)
        Try

            Dim retrievedDS As New DataSet

            retrievedDS = ManagementReportBL.GetMR004StockReviewList(storeId, stockCodeFrom, stockCodeTo, transDateFrom, transDateTo)
            For Each col As DataColumn In retrievedDS.Tables(0).Columns
                col.ReadOnly = False
            Next
            If retrievedDS.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In retrievedDS.Tables(0).Rows
                    row = FillRowWithNull(row, retrievedDS.Tables(0).Columns)

                    Dim detailsItem As New MR004StockReviewListDetails

                    detailsItem.StockItemDescription = row("StockItemDescription")
                    detailsItem.StockItemID = row("StockItemID")
                    If Not row.IsNull("AvgIssue") Then
                        detailsItem.AvgIssue = row("AvgIssue")
                    End If
                    If Not row.IsNull("AvgIssuePerMth") Then
                        detailsItem.AvgIssuePerMth = row("AvgIssuePerMth")
                    End If
                    If Not row.IsNull("DesireMaxlevel") Then
                        detailsItem.DesireMaxlevel = row("DesireMaxlevel")
                    End If
                    If Not row.IsNull("DesireOrderlevel") Then
                        detailsItem.DesireOrderlevel = row("DesireOrderlevel")
                    End If
                    If Not row.IsNull("HighestIssue") Then
                        detailsItem.HighestIssue = row("HighestIssue")
                    End If
                    If Not row.IsNull("NoOFIssueByStockRange") Then
                        detailsItem.NoOfIssue = row("NoOFIssueByStockRange")
                    End If
                    If Not row.IsNull("NoOfOrder") Then
                        detailsItem.NoOfOrder = row("NoOfOrder")
                    End If
                    If Not row.IsNull("StockItemBalanceQty") Then
                        detailsItem.StockItemBalanceQty = row("StockItemBalanceQty")
                    End If
                    If Not row.IsNull("StockItemMaxLevel") Then
                        detailsItem.StockItemMaxLevel = row("StockItemMaxLevel")
                    End If
                    If Not row.IsNull("StockItemMinLevel") Then
                        detailsItem.StockItemMinLevel = row("StockItemMinLevel")
                    End If
                    If Not row.IsNull("StockItemReorderLevel") Then
                        detailsItem.StockItemReorderLevel = row("StockItemReorderLevel")
                    End If
                    If Not row.IsNull("StockItemUnitCost") Then
                        detailsItem.StockItemUnitCost = row("StockItemUnitCost")
                    End If
                    If Not row.IsNull("TotalIssueQty") Then
                        detailsItem.TotalIssueQty = row("TotalIssueQty")
                    End If

                    detailsList.Add(detailsItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return detailsList
    End Function
    Public Function GetMR005StockReturn(ByVal storeId As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime) As List(Of MR005StockReturnDetails) Implements IService.GetMR005StockReturn

        Dim detailsList As New List(Of MR005StockReturnDetails)
        Try

            Dim retrievedDS As New DataSet

            retrievedDS = ManagementReportBL.GetMR005StockReturn(storeId, transDateFrom, transDateTo)
            For Each col As DataColumn In retrievedDS.Tables(0).Columns
                col.ReadOnly = False
            Next

            If retrievedDS.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In retrievedDS.Tables(0).Rows
                    row = FillRowWithNull(row, retrievedDS.Tables(0).Columns)

                    Dim detailsItem As New MR005StockReturnDetails

                    detailsItem.StockType = row("StockType")
                    detailsItem.StockTypeDesc = row("StockTypeDesc")
                    If Not row.IsNull("AdjustIssueTotalCost") Then
                        detailsItem.AdjustIssueTotalCost = row("AdjustIssueTotalCost")
                    End If
                    If Not row.IsNull("AdjustTotalCost") Then
                        detailsItem.AdjustTotalCost = row("AdjustTotalCost")
                    End If
                    If Not row.IsNull("EndBalanceTotalCost") Then
                        detailsItem.EndBalanceTotalCost = row("EndBalanceTotalCost")
                    End If
                    If Not row.IsNull("IssueTotalCost") Then
                        detailsItem.IssueTotalCost = row("IssueTotalCost")
                    End If
                    If Not row.IsNull("ReceiveTotalCost") Then
                        detailsItem.ReceiveTotalCost = row("ReceiveTotalCost")
                    End If
                    If Not row.IsNull("StartBalanceTotalCost") Then
                        detailsItem.StartBalanceTotalCost = row("StartBalanceTotalCost")
                    End If

                    detailsList.Add(detailsItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return detailsList
    End Function
    Public Function GetMR006StockReturnCheckListAdjust(ByVal storeId As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime) As List(Of MR006StockReturnCheckListAdjustDetails) Implements IService.GetMR006StockReturnCheckListAdjust

        Dim detailsList As New List(Of MR006StockReturnCheckListAdjustDetails)
        Try

            Dim retrievedDS As New DataSet

            retrievedDS = ManagementReportBL.GetMR006StockReturnCheckListAdjust(storeId, transDateFrom, transDateTo)
            For Each col As DataColumn In retrievedDS.Tables(0).Columns
                col.ReadOnly = False
            Next

            If retrievedDS.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In retrievedDS.Tables(0).Rows
                    row = FillRowWithNull(row, retrievedDS.Tables(0).Columns)

                    Dim detailsItem As New MR006StockReturnCheckListAdjustDetails

                    detailsItem.StockType = row("StockTypeDesc")
                    detailsItem.StockItemID = row("StockItemID")
                    detailsItem.StockItemDescription = row("StockItemDescription")
                    detailsItem.AdjustID = row("AdjustID")
                    'detailsItem.AdjustDte = row("AdjustDte")
                    detailsItem.AdjustDte = row("StockTransactionDte")
                    detailsItem.AdjustSerialNo = row("AdjustSerialNo")
                    detailsItem.AdjustRemarks = row("AdjustRemarks")
                    detailsItem.AdjustType = row("AdjustType")
                    detailsItem.AdjustUOM = row("AdjustUOM")
                    If Not row.IsNull("AdjustQty") Then
                        detailsItem.AdjustQty = row("AdjustQty")
                    End If
                    If Not row.IsNull("AdjustTotalCost") Then
                        detailsItem.AdjustTotalCost = row("AdjustTotalCost")
                    End If
                    If Not row.IsNull("AdjustUnitCost") Then
                        detailsItem.AdjustUnitCost = row("AdjustUnitCost")
                    End If

                    detailsList.Add(detailsItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return detailsList
    End Function
    Public Function GetMR006StockReturnCheckListIssue(ByVal storeId As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime) As List(Of MR006StockReturnCheckListIssueDetails) Implements IService.GetMR006StockReturnCheckListIssue

        Dim detailsList As New List(Of MR006StockReturnCheckListIssueDetails)
        Try

            Dim retrievedDS As New DataSet

            retrievedDS = ManagementReportBL.GetMR006StockReturnCheckListIssue(storeId, transDateFrom, transDateTo)
            For Each col As DataColumn In retrievedDS.Tables(0).Columns
                col.ReadOnly = False
            Next

            If retrievedDS.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In retrievedDS.Tables(0).Rows
                    row = FillRowWithNull(row, retrievedDS.Tables(0).Columns)

                    Dim detailsItem As New MR006StockReturnCheckListIssueDetails

                    detailsItem.StockType = row("StockTypeDesc")
                    detailsItem.StockItemID = row("StockItemID")
                    detailsItem.StockItemDescription = row("StockItemDescription")
                    detailsItem.ConsumerDescription = row("ConsumerDescription")
                    detailsItem.IssueDte = row("IssueDte")
                    detailsItem.ConsumerID = row("ConsumerID")
                    detailsItem.RequestID = row("RequestID")
                    detailsItem.IssueType = row("IssueType")
                    detailsItem.IssueUOM = row("IssueUOM")
                    If Not row.IsNull("IssueQty") Then
                        detailsItem.IssueQty = row("IssueQty")
                    End If
                    If Not row.IsNull("IssueTotalCost") Then
                        detailsItem.IssueTotalCost = row("IssueTotalCost")
                    End If
                    If Not row.IsNull("IssueUnitCost") Then
                        detailsItem.IssueUnitCost = row("IssueUnitCost")
                    End If

                    detailsList.Add(detailsItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return detailsList
    End Function
    Public Function GetMR006StockReturnCheckListReceive(ByVal storeId As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime) As List(Of MR006StockReturnCheckListReceiveDetails) Implements IService.GetMR006StockReturnCheckListReceive

        Dim detailsList As New List(Of MR006StockReturnCheckListReceiveDetails)
        Try

            Dim retrievedDS As New DataSet

            retrievedDS = ManagementReportBL.GetMR006StockReturnCheckListReceive(storeId, transDateFrom, transDateTo)
            For Each col As DataColumn In retrievedDS.Tables(0).Columns
                col.ReadOnly = False
            Next

            If retrievedDS.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In retrievedDS.Tables(0).Rows
                    row = FillRowWithNull(row, retrievedDS.Tables(0).Columns)

                    Dim detailsItem As New MR006StockReturnCheckListReceiveDetails

                    detailsItem.StockType = row("StockTypeDesc")
                    detailsItem.StockItemID = row("StockItemID")
                    detailsItem.StockItemDescription = row("StockItemDescription")
                    detailsItem.ReceiveDte = row("ReceiveDte")
                    detailsItem.ReceiveDocNo = row("ReceiveDocNo")
                    detailsItem.ReceiveRemarks = row("ReceiveRemarks")
                    detailsItem.ReceiveType = row("ReceiveType")
                    detailsItem.ReceiveUOM = row("ReceiveUOM")
                    If Not row.IsNull("ReceiveQty") Then
                        detailsItem.ReceiveQty = row("ReceiveQty")
                    End If
                    If Not row.IsNull("ReceiveTotalCost") Then
                        detailsItem.ReceiveTotalCost = row("ReceiveTotalCost")
                    End If
                    If Not row.IsNull("ReceiveUnitCost") Then
                        detailsItem.ReceiveUnitCost = row("ReceiveUnitCost")
                    End If

                    detailsList.Add(detailsItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return detailsList
    End Function
    Public Function GetMR006StockReturnCheckListStockItem(ByVal storeId As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime, ByVal excludeZero As Boolean) As List(Of MR006StockReturnCheckListStockItemDetails) Implements IService.GetMR006StockReturnCheckListStockItem

        Dim detailsList As New List(Of MR006StockReturnCheckListStockItemDetails)
        Try

            Dim retrievedDS As New DataSet

            retrievedDS = ManagementReportBL.GetMR006StockReturnCheckListStockItem(storeId, transDateFrom, transDateTo, excludeZero)
            For Each col As DataColumn In retrievedDS.Tables(0).Columns
                col.ReadOnly = False
            Next

            If retrievedDS.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In retrievedDS.Tables(0).Rows
                    row = FillRowWithNull(row, retrievedDS.Tables(0).Columns)

                    Dim detailsItem As New MR006StockReturnCheckListStockItemDetails

                    detailsItem.StockType = row("StockTypeDesc")
                    detailsItem.StockItemID = row("StockItemID")
                    detailsItem.StockItemDescription = row("StockItemDescription")
                    detailsItem.StockItemPartNo = row("StockItemPartNo")
                    detailsItem.StockItemLocation = row("StockItemLocation")
                    detailsItem.StockItemLocation2 = row("StockItemLocation2")
                    detailsItem.StockItemUOM = row("StockItemUOM")
                    If Not row.IsNull("StockItemStockBal") Then
                        detailsItem.StockItemStockBal = row("StockItemStockBal")
                    End If
                    If Not row.IsNull("StockItemTotalCost") Then
                        detailsItem.StockItemTotalCost = row("StockItemTotalCost")
                    End If
                    If Not row.IsNull("StockItemUnitCost") Then
                        detailsItem.StockItemUnitCost = row("StockItemUnitCost")
                    End If

                    detailsList.Add(detailsItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return detailsList
    End Function
    Public Function GetMR007PeriodIssues(ByVal storeId As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime, ByVal consumerID As String, ByVal directIssue As String) As List(Of MR007PeriodIssuesDetails) Implements IService.GetMR007PeriodIssues

        Dim detailsList As New List(Of MR007PeriodIssuesDetails)
        Try

            Dim retrievedDS As New DataSet

            retrievedDS = ManagementReportBL.GetMR007PeriodIssues(storeId, transDateFrom, transDateTo, consumerID)
            For Each col As DataColumn In retrievedDS.Tables(0).Columns
                col.ReadOnly = False
            Next

            Dim issueDS As New DataSet
            If directIssue = "Y" Then
                issueDS = ManagementReportBL.GetMR007PeriodDirectIssues(storeId, transDateFrom, transDateTo, consumerID)
                For Each col As DataColumn In issueDS.Tables(0).Columns
                    col.ReadOnly = False
                Next

                '-- UAT.02.48 
                ''retrievedDS.Merge(issueDS, True)

            End If

            If retrievedDS.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In retrievedDS.Tables(0).Rows
                    row = FillRowWithNull(row, retrievedDS.Tables(0).Columns)

                    Dim detailsItem As New MR007PeriodIssuesDetails

                    detailsItem.IssueSerialNo = row("IssueSerialNo")
                    detailsItem.StockItemID = row("StockItemID")
                    detailsItem.StockItemDescription = row("StockItemDescription")
                    detailsItem.ConsumerDescription = row("ConsumerDescription")
                    detailsItem.IssueDte = row("IssueDte")
                    detailsItem.ConsumerID = row("ConsumerID")
                    detailsItem.RequestID = row("RequestID")
                    detailsItem.IssueType = row("IssueType")
                    detailsItem.IssueUOM = row("IssueUOM")
                    If Not row.IsNull("IssueQty") Then
                        detailsItem.IssueQty = row("IssueQty")
                    End If
                    If Not row.IsNull("IssueTotalCost") Then
                        detailsItem.IssueTotalCost = row("IssueTotalCost")
                    End If
                    If Not row.IsNull("IssueUnitCost") Then
                        detailsItem.IssueUnitCost = row("IssueUnitCost")
                    End If

                    detailsList.Add(detailsItem)

                Next
            End If

            '-- DIRECT ISSUE
            If directIssue = "Y" Then

                If issueDS.Tables(0).Rows.Count > 0 Then
                    For Each row As DataRow In issueDS.Tables(0).Rows
                        row = FillRowWithNull(row, issueDS.Tables(0).Columns)

                        Dim detailsItem As New MR007PeriodIssuesDetails

                        detailsItem.IssueSerialNo = row("IssueSerialNo")
                        detailsItem.StockItemID = row("StockItemID")
                        detailsItem.StockItemDescription = row("StockItemDescription")
                        detailsItem.ConsumerDescription = row("ConsumerDescription")
                        detailsItem.IssueDte = row("IssueDte")
                        detailsItem.ConsumerID = row("ConsumerID")
                        detailsItem.RequestID = row("RequestID")
                        detailsItem.IssueType = row("IssueType")
                        detailsItem.IssueUOM = row("IssueUOM")
                        If Not row.IsNull("IssueQty") Then
                            detailsItem.IssueQty = row("IssueQty")
                        End If
                        If Not row.IsNull("IssueTotalCost") Then
                            detailsItem.IssueTotalCost = row("IssueTotalCost")
                        End If
                        If Not row.IsNull("IssueUnitCost") Then
                            detailsItem.IssueUnitCost = row("IssueUnitCost")
                        End If

                        detailsList.Add(detailsItem)

                    Next
                End If

            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return detailsList

    End Function
    Public Function GetMR008QuantityIssueSummary(ByVal storeId As String, ByVal stockCodeFrom As String, ByVal stockCodeTo As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime) As List(Of MR008QuantityIssueSummaryDetails) Implements IService.GetMR008QuantityIssueSummary

        Dim detailsList As New List(Of MR008QuantityIssueSummaryDetails)
        Try

            Dim retrievedDS As New DataSet

            retrievedDS = ManagementReportBL.GetMR008QuantityIssueSummary(storeId, stockCodeFrom, stockCodeTo, transDateFrom, transDateTo)
            For Each col As DataColumn In retrievedDS.Tables(0).Columns
                col.ReadOnly = False
            Next
            If retrievedDS.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In retrievedDS.Tables(0).Rows
                    row = FillRowWithNull(row, retrievedDS.Tables(0).Columns)

                    Dim detailsItem As New MR008QuantityIssueSummaryDetails

                    detailsItem.StockItemDescription = row("StockItemDescription")
                    detailsItem.StockItemUOM = row("StockItemUOM")
                    detailsItem.StockItemID = row("StockItemID")
                    detailsItem.FIssueConsumerID = row("FIssueByStockRangeConsumerID")
                    If Not row.IsNull("StockItemStockBal") Then
                        detailsItem.StockItemStockBal = row("StockItemStockBal")
                    End If
                    If Not row.IsNull("StockItemTotalCost") Then
                        detailsItem.StockItemTotalCost = row("StockItemTotalCost")
                    End If
                    If Not row.IsNull("SumIssueQty") Then
                        detailsItem.SumIssueQty = row("SumIssueQty")
                    End If
                    If Not row.IsNull("SumIssueTotalCost") Then
                        detailsItem.SumIssueTotalCost = row("SumIssueTotalCost")
                    End If

                    detailsList.Add(detailsItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return detailsList
    End Function
    Public Function GetMR009ReorderStockItemList(ByVal storeId As String, ByVal stockCodeFrom As String, ByVal stockCodeTo As String) As List(Of MR009ReorderStockItemListDetails) Implements IService.GetMR009ReorderStockItemList

        Dim detailsList As New List(Of MR009ReorderStockItemListDetails)
        Try

            Dim retrievedDS As New DataSet

            retrievedDS = ManagementReportBL.GetMR009ReorderStockItemList(storeId, stockCodeFrom, stockCodeTo)
            For Each col As DataColumn In retrievedDS.Tables(0).Columns
                col.ReadOnly = False
            Next

            If retrievedDS.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In retrievedDS.Tables(0).Rows
                    row = FillRowWithNull(row, retrievedDS.Tables(0).Columns)

                    Dim detailsItem As New MR009ReorderStockItemListDetails

                    detailsItem.OrderItemOrderID = row("OrderItemOrderID")
                    detailsItem.StockItemID = row("StockItemID")
                    detailsItem.StockItemDescription = row("StockItemDescription")
                    detailsItem.OrderItemRemarks = row("OrderItemRemarks")
                    detailsItem.StockItemUOM = row("StockItemUOM")
                    If Not row.IsNull("OrderItemQty") Then
                        detailsItem.OrderItemQty = row("OrderItemQty")
                    End If
                    If Not row.IsNull("OrderItemTotalCost") Then
                        detailsItem.OrderItemTotalCost = row("OrderItemTotalCost")
                    End If
                    If Not row.IsNull("StockItemMaxLevel") Then
                        detailsItem.StockItemMaxLevel = row("StockItemMaxLevel")
                    End If
                    If Not row.IsNull("StockItemMinLevel") Then
                        detailsItem.StockItemMinLevel = row("StockItemMinLevel")
                    End If
                    If Not row.IsNull("StockItemReorderLevel") Then
                        detailsItem.StockItemReorderLevel = row("StockItemReorderLevel")
                    End If
                    If Not row.IsNull("StockItemStockBal") Then
                        detailsItem.StockItemStockBal = row("StockItemStockBal")
                    End If
                    If Not row.IsNull("StockItemTotalCost") Then
                        detailsItem.StockItemTotalCost = row("StockItemTotalCost")
                    End If

                    detailsList.Add(detailsItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return detailsList
    End Function
    Public Function GetMR010StockAdjustmentEntries(ByVal storeId As String, ByVal stockCodeFrom As String, ByVal stockCodeTo As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime) As List(Of MR010StockAdjustmentEntriesDetails) Implements IService.GetMR010StockAdjustmentEntries

        Dim detailsList As New List(Of MR010StockAdjustmentEntriesDetails)
        Try

            'Dim retrievedDS As New DataSet

            'retrievedDS = ManagementReportBL.GetMR010StockAdjustmentEntries(storeId, stockCodeFrom, stockCodeTo, transDateFrom, transDateTo)

            'For Each col As DataColumn In retrievedDS.Tables(0).Columns
            '    col.ReadOnly = False
            'Next

            'If retrievedDS.Tables(0).Rows.Count > 0 Then
            '    For Each row As DataRow In retrievedDS.Tables(0).Rows
            '        row = FillRowWithNull(row, retrievedDS.Tables(0).Columns)

            '        Dim detailsItem As New MR010StockAdjustmentEntriesDetails

            '        detailsItem.StockItemDescription = row("StockItemDescription")
            '        detailsItem.StockItemID = row("StockItemID")
            '        detailsItem.AdjustAdjustID = row("AdjustAdjustID")
            '        detailsItem.AdjustDocReturn = row("AdjustDocReturn")
            '        detailsItem.AdjustDte = row("AdjustDte")
            '        detailsItem.AdjustRemarks = row("AdjustRemarks")
            '        detailsItem.AdjustType = row("AdjustType")
            '        detailsItem.StockItemUOM = row("StockItemUOM")
            '        If Not row.IsNull("AdjustQty") Then
            '            detailsItem.AdjustQty = row("AdjustQty")
            '        End If
            '        If Not row.IsNull("StockItemTotalCost") Then
            '            detailsItem.StockItemTotalCost = row("StockItemTotalCost")
            '        End If
            '        If Not row.IsNull("StockItemStockBal") Then
            '            detailsItem.StockItemStockBal = row("StockItemStockBal")
            '        End If
            '        If Not row.IsNull("AdjustTotalCost") Then
            '            detailsItem.AdjustTotalCost = row("AdjustTotalCost")
            '        End If

            '        detailsList.Add(detailsItem)

            '    Next
            'End If

        Catch ex As Exception

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return detailsList

    End Function

#End Region

#Region " Ad Hoc Report "

    ''' <summary>
    ''' Function - GetAdHocReport;
    ''' 30 Mar 09 - Jianfa;
    ''' </summary>
    ''' <param name="adHocReport"></param>
    ''' <param name="returnMessage"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAdHocReport(ByVal AdHocReport As AdHocReportDetails, ByRef returnMessage As String) _
                                    As List(Of AdHocReportDetails) Implements IService.GetAdHocReport

        Dim AdHocReportList As New List(Of AdHocReportDetails)

        Try

            Dim retrievedDS As New DataSet

            retrievedDS = ManagementReportBL.GetAdHocReports(AdHocReport.SQLStatement, returnMessage)

            For Each col As DataColumn In retrievedDS.Tables(0).Columns
                col.ReadOnly = False
            Next
            If retrievedDS.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In retrievedDS.Tables(0).Rows
                    row = FillRowWithNull(row, retrievedDS.Tables(0).Columns)

                    Dim AdHocReportItem As New AdHocReportDetails
                    Dim rowCount As Integer = retrievedDS.Tables(0).Columns.Count

                    AdHocReportItem.AdHoc_1 = GetRowText(rowCount, 0, row)
                    AdHocReportItem.AdHoc_2 = GetRowText(rowCount, 1, row)
                    AdHocReportItem.AdHoc_3 = GetRowText(rowCount, 2, row)
                    AdHocReportItem.AdHoc_4 = GetRowText(rowCount, 3, row)
                    AdHocReportItem.AdHoc_5 = GetRowText(rowCount, 4, row)
                    AdHocReportItem.AdHoc_6 = GetRowText(rowCount, 5, row)
                    AdHocReportItem.AdHoc_7 = GetRowText(rowCount, 6, row)
                    AdHocReportItem.AdHoc_8 = GetRowText(rowCount, 7, row)
                    AdHocReportItem.AdHoc_9 = GetRowText(rowCount, 8, row)
                    AdHocReportItem.AdHoc_10 = GetRowText(rowCount, 9, row)
                    AdHocReportItem.AdHoc_11 = GetRowText(rowCount, 10, row)
                    AdHocReportItem.AdHoc_12 = GetRowText(rowCount, 11, row)
                    AdHocReportItem.AdHoc_13 = GetRowText(rowCount, 12, row)
                    AdHocReportItem.AdHoc_14 = GetRowText(rowCount, 13, row)
                    AdHocReportItem.AdHoc_15 = GetRowText(rowCount, 14, row)
                    AdHocReportItem.AdHoc_16 = GetRowText(rowCount, 15, row)
                    AdHocReportItem.AdHoc_17 = GetRowText(rowCount, 16, row)
                    AdHocReportItem.AdHoc_18 = GetRowText(rowCount, 17, row)

                    AdHocReportList.Add(AdHocReportItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return AdHocReportList

    End Function

    ''' <summary>
    ''' Function - GetRowText 
    ''' 31 Mar 09 - Jianfa
    ''' </summary>
    ''' <param name="idx"></param>
    ''' <param name="row"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRowText(ByVal rowCount As Integer, ByVal idx As Integer, ByVal row As DataRow) As String

        If rowCount <= idx Then
            Return String.Empty
        Else
            Return row.Item(idx).ToString
        End If

    End Function

#End Region

End Class
