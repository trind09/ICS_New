Imports NEA_ICS.Business
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Partial Public Class Service

#Region " VERIFICATION WORKSHEET "

    ''' <summary>
    ''' Function - GetWorkSheetItems;
    ''' 30 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="workSheetDetails"></param>
    ''' <param name="sortExpression"></param>
    ''' <param name="sortDirection"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetWorkSheetItems(ByVal workSheetDetails As WorksheetDetails, _
                                      Optional ByVal sortExpression As String = "", _
                                      Optional ByVal sortDirection As String = "") _
                                      As List(Of WorksheetDetails) Implements IService.GetWorkSheetItems

        Dim WorkSheetItemsList As New List(Of WorksheetDetails)
        Try

            Dim WorkSheetItemsRetrieved As New DataSet
            WorkSheetItemsList.Clear()

            WorkSheetItemsRetrieved = WorksheetBL.GetWorkSheetItems(workSheetDetails.StoreID, _
                                                               workSheetDetails.StockCodeFrom, _
                                                               workSheetDetails.StockCodeTo, _
                                                               workSheetDetails.StockType, _
                                                               workSheetDetails.SubType, _
                                                               workSheetDetails.TotalValue, _
                                                               workSheetDetails.WorkSheetStatus)

            If sortExpression = String.Empty And sortDirection = String.Empty Then

                If WorkSheetItemsRetrieved.Tables(0).Rows.Count > 0 Then
                    For Each row As DataRow In WorkSheetItemsRetrieved.Tables(0).Rows
                        row = FillRowWithNull(row, WorkSheetItemsRetrieved.Tables(0).Columns)

                        Dim WorksheetItemDetailsItem As New WorksheetDetails

                        WorksheetItemDetailsItem.ItemID = row("FStockItemByStockRangeID")
                        WorksheetItemDetailsItem.ItemDescription = row("FStockItemByStockRangeDescription")
                        WorksheetItemDetailsItem.StockTypeDescription = row("FStockItemByStockRangeStockType")
                        WorksheetItemDetailsItem.EquipmentID = row("FStockItemByStockRangeEquipmentID")
                        WorksheetItemDetailsItem.StockQty = row("FStockItemByStockRangeStockBal")
                        WorksheetItemDetailsItem.TotalValue = row("FStockItemByStockRangeTotalCost")
                        WorksheetItemDetailsItem.Location = row("FStockItemByStockRangeLocation")
                        WorksheetItemDetailsItem.Location2 = row("FStockItemByStockRangeLocation2")

                        WorkSheetItemsList.Add(WorksheetItemDetailsItem)

                    Next
                End If

            Else

                Dim WorkSheetItemsRetrievedView As New DataView(WorkSheetItemsRetrieved.Tables(0))
                WorkSheetItemsRetrievedView.Sort = sortExpression & " " & sortDirection

                If WorkSheetItemsRetrieved.Tables(0).Rows.Count > 0 Then
                    For Each viewRow As DataRowView In WorkSheetItemsRetrievedView

                        viewRow = FillViewRowWithNull(viewRow, WorkSheetItemsRetrievedView.Table.Columns)

                        Dim WorksheetItemDetailsItem As New WorksheetDetails

                        WorksheetItemDetailsItem.ItemID = viewRow("FStockItemByStoreStockItemID")
                        WorksheetItemDetailsItem.ItemDescription = viewRow("FStockItemByStoreStockItemDescription")
                        WorksheetItemDetailsItem.StockTypeDescription = viewRow("FStockItemByStockRangeStockType")
                        WorksheetItemDetailsItem.EquipmentID = viewRow("FStockItemByStoreStockItemEquipmentID")
                        WorksheetItemDetailsItem.StockQty = viewRow("FStockItemByStoreStockItemStockBal")
                        WorksheetItemDetailsItem.TotalValue = viewRow("FStockItemByStoreStockItemTotalCost")
                        WorksheetItemDetailsItem.Location = viewRow("FStockItemByStoreStockItemLocation")
                        WorksheetItemDetailsItem.Location2 = viewRow("FStockItemByStoreStockItemLocation2")

                        WorkSheetItemsList.Add(WorksheetItemDetailsItem)

                    Next
                End If
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return WorkSheetItemsList

    End Function

    ''' <summary>
    ''' Function - GetMarkedWorksheetItems
    ''' 12 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="workSheetDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetMarkedWorksheetItems(ByVal workSheetDetails As WorksheetDetails, _
                                            Optional ByVal sortExpression As String = "", _
                                            Optional ByVal sortDirection As String = "") _
                                            As List(Of WorksheetDetails) Implements IService.GetMarkedWorksheetItems

        Dim WorkSheetItemsList As New List(Of WorksheetDetails)
        Try

            Dim WorkSheetItemsRetrieved As New DataSet
            WorkSheetItemsList.Clear()

            WorkSheetItemsRetrieved = WorksheetBL.GetMarkedWorksheetItem(workSheetDetails.StoreID, _
                                                             workSheetDetails.WorkSheetID)

            If sortExpression = String.Empty And sortDirection = String.Empty Then

                If WorkSheetItemsRetrieved.Tables(0).Rows.Count > 0 Then
                    For Each row As DataRow In WorkSheetItemsRetrieved.Tables(0).Rows
                        row = FillRowWithNull(row, WorkSheetItemsRetrieved.Tables(0).Columns)

                        Dim WorksheetItemDetailsItem As New WorksheetDetails

                        WorksheetItemDetailsItem.CheckerName = row("WorksheetCheckerName")
                        WorksheetItemDetailsItem.CheckDate = row("WorksheetCheckDte")
                        WorksheetItemDetailsItem.VerifierName = row("WorksheetVerifierName")
                        WorksheetItemDetailsItem.VerifyDate = row("WorksheetVerifyDte")
                        WorksheetItemDetailsItem.ApproverName = row("WorksheetApproverName")
                        WorksheetItemDetailsItem.ApproveDate = row("WorksheetApproveDte")
                        WorksheetItemDetailsItem.ItemID = row("WorksheetItemStockItemID")
                        WorksheetItemDetailsItem.StockQty = row("WorksheetItemQty")
                        WorksheetItemDetailsItem.UOM = row("StockItemUOM")
                        WorksheetItemDetailsItem.Location = row("StockItemLocation")
                        WorksheetItemDetailsItem.Location2 = row("StockItemLocation2")
                        WorksheetItemDetailsItem.ItemDescription = row("StockItemDescription")

                        WorkSheetItemsList.Add(WorksheetItemDetailsItem)

                    Next
                End If

            Else

                Dim WorkSheetItemsRetrievedView As New DataView(WorkSheetItemsRetrieved.Tables(0))
                WorkSheetItemsRetrievedView.Sort = sortExpression & " " & sortDirection

                If WorkSheetItemsRetrieved.Tables(0).Rows.Count > 0 Then
                    For Each viewRow As DataRowView In WorkSheetItemsRetrievedView

                        viewRow = FillViewRowWithNull(viewRow, WorkSheetItemsRetrievedView.Table.Columns)

                        Dim WorksheetItemDetailsItem As New WorksheetDetails

                        WorksheetItemDetailsItem.CheckerName = viewRow("WorksheetCheckerName")
                        WorksheetItemDetailsItem.CheckDate = viewRow("WorksheetCheckDte")
                        WorksheetItemDetailsItem.VerifierName = viewRow("WorksheetVerifierName")
                        WorksheetItemDetailsItem.VerifyDate = viewRow("WorksheetVerifyDte")
                        WorksheetItemDetailsItem.ApproverName = viewRow("WorksheetApproverName")
                        WorksheetItemDetailsItem.ApproveDate = viewRow("WorksheetApproveDte")
                        WorksheetItemDetailsItem.ItemID = viewRow("WorksheetItemStockItemID")
                        WorksheetItemDetailsItem.StockQty = viewRow("WorksheetItemQty")
                        WorksheetItemDetailsItem.UOM = viewRow("StockItemUOM")
                        WorksheetItemDetailsItem.Location = viewRow("StockItemLocation")
                        WorksheetItemDetailsItem.Location2 = viewRow("StockItemLocation2")
                        WorksheetItemDetailsItem.ItemDescription = viewRow("StockItemDescription")

                        WorkSheetItemsList.Add(WorksheetItemDetailsItem)

                    Next
                End If

            End If



        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return WorkSheetItemsList

    End Function

    ''' <summary>
    ''' Function - AddWorkSheetItem
    ''' 03 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="workSheetDetails"></param>
    ''' <param name="workSheetItemList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddWorkSheetItem(ByVal workSheetDetails As WorksheetDetails, _
                                     ByVal workSheetItemList As List(Of WorksheetDetails), _
                                     ByRef workSheetID As Integer) As String Implements IService.AddWorkSheetItem

        Dim errorMessage As String = String.Empty
        Dim dtWorkSheetItemList As New DataTable

        Try

            dtWorkSheetItemList.Columns.Add("WorksheetItemStockItemID", GetType(String))
            dtWorkSheetItemList.Columns.Add("WorksheetItemQty", GetType(Decimal))
            dtWorkSheetItemList.Columns.Add("WorksheetItemTotalCost", GetType(Decimal))

            For Each WorkSheetItem As WorksheetDetails In workSheetItemList

                Dim row As DataRow = dtWorkSheetItemList.NewRow()
                row.Item("WorksheetItemStockItemID") = WorkSheetItem.ItemID
                row.Item("WorksheetItemQty") = WorkSheetItem.StockQty
                row.Item("WorksheetItemTotalCost") = WorkSheetItem.TotalValue

                dtWorkSheetItemList.Rows.Add(row)

            Next

            errorMessage = WorksheetBL.AddWorkSheetItem(workSheetDetails.StoreID, dtWorkSheetItemList, _
                                                        workSheetDetails.LoginUser, workSheetID)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return errorMessage

    End Function

    ''' <summary>
    ''' Function - GetWorksheetGeneratedDate;
    ''' 06 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="workSheetDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetWorksheetGeneratedDate(ByVal workSheetDetails As WorksheetDetails) As String Implements IService.GetWorksheetGeneratedDate

        Dim GeneratedDate As String = String.Empty

        Try

            GeneratedDate = WorksheetBL.GetWorksheetGeneratedDate(workSheetDetails.StoreID, _
                                                                  workSheetDetails.WorkSheetID)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return GeneratedDate

    End Function

    ''' <summary>
    ''' Function - UpdateWorksheet;
    ''' 06 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="workSheetDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateWorksheet(ByVal workSheetDetails As WorksheetDetails) As String Implements IService.UpdateWorksheet

        Dim errorMessage As String = String.Empty

        Try

            errorMessage = WorksheetBL.UpdateWorksheet(workSheetDetails.StoreID, workSheetDetails.WorkSheetID, _
                                                       workSheetDetails.VerifierName, workSheetDetails.CheckerName, _
                                                       workSheetDetails.ApproverName, _
                                                       workSheetDetails.LoginUser)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return errorMessage

    End Function

    ''' <summary>
    ''' Function - DeleteWorksheet;
    ''' 06 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="worksheetDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DeleteWorksheet(ByVal worksheetDetails As WorksheetDetails) As String Implements IService.DeleteWorksheet

        Dim errorMessage As String = String.Empty

        Try

            errorMessage = WorksheetBL.DeleteWorksheet(worksheetDetails.StoreID, worksheetDetails.WorkSheetID)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return errorMessage

    End Function

#End Region

End Class
