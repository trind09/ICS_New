Imports NEA_ICS.Business
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Partial Public Class Service

#Region " Audit Trail Report "
    ''' <summary>
    ''' Get the list of Stock Item based on the search criteria;
    ''' 4 Jan 2009 - Liu Guo Feng;
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="dateFrom"></param>
    ''' <param name="dateTo"></param>
    ''' <param name="auditType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAuditTrailStockItem(ByVal storeId As String, ByVal dateFrom As DateTime, ByVal dateTo As DateTime, ByVal auditType As String, ByVal itemStatus As Boolean, ByVal orderBy As String) As List(Of AStockItemDetails) Implements IService.GetAuditTrailStockItem

        Dim StockItemDetailsList As New List(Of AStockItemDetails)
        Try

            Dim StockItemRetrieved As New DataSet
            StockItemDetailsList.Clear()

            StockItemRetrieved = AuditTrailReportBL.GetAuditTrailStockItem(storeId, dateFrom, dateTo, auditType, itemStatus, orderBy)

            For Each col As DataColumn In StockItemRetrieved.Tables(0).Columns
                col.ReadOnly = False
            Next

            If StockItemRetrieved.Tables(0).Rows.Count > 0 Then
                Dim dtItems As New DataTable

                ' if filtered by 'open' to 'closed'
                If itemStatus Then
                    Dim idCol As DataColumn = New DataColumn("itemId", System.Type.GetType("System.Int32"))
                    StockItemRetrieved.Tables(0).Columns.Add(idCol)

                    Dim dtFilteredItems As New DataTable
                    dtFilteredItems = StockItemRetrieved.Tables(0).Clone()
                    Dim ctr As Int32 = 0
                    For Each rowFiltered As DataRow In StockItemRetrieved.Tables(0).Rows
                        ctr += 1
                        rowFiltered("itemId") = ctr ' add item id column
                        If rowFiltered("AStockItemStatus") = "C" And rowFiltered("AStockItemImage") = "AFTER" Then
                            Dim strExp As String = "AStockItemImage = 'BEFORE' AND AStockItemStatus = 'O' AND AStockItemId = '" & rowFiltered("AStockItemId") & "' AND itemId = " & ctr - 1
                            Dim foundRow As DataRow() = StockItemRetrieved.Tables(0).Select(strExp)
                            If foundRow.Length > 0 Then
                                'dtFilteredItems.Rows.Add(foundRow(0))
                                dtFilteredItems.ImportRow(foundRow(0))
                                dtFilteredItems.ImportRow(rowFiltered)
                                dtFilteredItems.AcceptChanges()
                            End If
                        End If
                    Next

                    dtItems = dtFilteredItems
                Else
                    dtItems = StockItemRetrieved.Tables(0)
                End If

                'For Each row As DataRow In StockItemRetrieved.Tables(0).Rows
                For Each row As DataRow In dtItems.Rows
                    row = FillRowWithNull(row, StockItemRetrieved.Tables(0).Columns)

                    Dim StockItemDetailsItem As New AStockItemDetails

                    StockItemDetailsItem.AStockItemImage = row("AStockItemImage")
                    StockItemDetailsItem.AStockItemAuditID = row("AStockItemAuditID")
                    StockItemDetailsItem.AStockItemAuditType = row("AStockItemAuditType")
                    StockItemDetailsItem.AStockItemCreateDte = row("AStockItemCreateDte")
                    StockItemDetailsItem.AStockItemDescription = row("AStockItemDescription")
                    StockItemDetailsItem.AStockItemID = row("AStockItemID")
                    StockItemDetailsItem.AStockItemMaxLevel = row("AStockItemMaxLevel")
                    StockItemDetailsItem.AStockItemMinLevel = row("AStockItemMinLevel")
                    StockItemDetailsItem.AStockItemPartNo = row("AStockItemPartNo")
                    StockItemDetailsItem.AStockItemLocation = row("AStockItemLocation")
                    StockItemDetailsItem.AStockItemReorderLevel = row("AStockItemReorderLevel")
                    StockItemDetailsItem.AStockItemStatus = row("AStockItemStatus")
                    StockItemDetailsItem.AStockItemStockType = row("AStockItemStockType")
                    StockItemDetailsItem.AStockItemStoreID = row("AStockItemStoreID")
                    StockItemDetailsItem.AStockItemUOM = row("AStockItemUOM")
                    StockItemDetailsItem.AStockItemUserID = row("AStockItemUserID")

                    StockItemDetailsList.Add(StockItemDetailsItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return StockItemDetailsList

    End Function

    ''' <summary>
    ''' Get the list of Order based on the search criteria;
    ''' 4 Jan 2009 - Liu Guo Feng;
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="dateFrom"></param>
    ''' <param name="dateTo"></param>
    ''' <param name="auditType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAuditTrailOrder(ByVal storeId As String, ByVal dateFrom As DateTime, ByVal dateTo As DateTime, ByVal auditType As String, _
                                       ByVal orderBy As String) As List(Of AOrderDetails) Implements IService.GetAuditTrailOrder

        Dim OrderDetailsList As New List(Of AOrderDetails)
        Try

            Dim OrderRetrieved As New DataSet
            OrderDetailsList.Clear()

            OrderRetrieved = AuditTrailReportBL.GetAuditTrailOrder(storeId, dateFrom, dateTo, auditType, orderBy)
            For Each col As DataColumn In OrderRetrieved.Tables(0).Columns
                col.ReadOnly = False
            Next

            If OrderRetrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In OrderRetrieved.Tables(0).Rows
                    row = FillRowWithNull(row, OrderRetrieved.Tables(0).Columns)

                    Dim OrderDetailsItem As New AOrderDetails

                    OrderDetailsItem.AOrderImage = String.Empty 'row("AOrderImage")
                    OrderDetailsItem.AOrderAuditType = row("AOrderAuditType")
                    OrderDetailsItem.AOrderCreateDte = row("AOrderCreateDte")
                    OrderDetailsItem.AOrderDte = row("AOrderDte")
                    OrderDetailsItem.AOrderID = row("AOrderID")
                    OrderDetailsItem.AOrderItemQty = row("AOrderItemQty")
                    OrderDetailsItem.AOrderItemStockItemID = row("AOrderItemStockItemID")
                    OrderDetailsItem.AOrderItemTotalCost = row("AOrderItemTotalCost")
                    OrderDetailsItem.AOrderItemUnitCost = row("AOrderItemUnitCost")
                    OrderDetailsItem.AOrderStoreID = row("AOrderStoreID")
                    OrderDetailsItem.AOrderSupplierID = row("AOrderSupplierID")
                    OrderDetailsItem.AOrderUserID = row("AOrderUserID")
                    OrderDetailsItem.AStockItemDescription = row("AStockItemDescription")
                    OrderDetailsItem.AStockItemUOM = row("AStockItemUOM")

                    OrderDetailsList.Add(OrderDetailsItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return OrderDetailsList
    End Function

    ''' <summary>
    ''' Get the list of Common based on the search criteria;
    ''' 10 Oct 2016 - Pritpalkaur hunjan;
    ''' </summary>
    ''' <param name="dateFrom"></param>
    ''' <param name="dateTo"></param>
    ''' <param name="Alldate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAuditTrailACommon(ByVal storeID As String, ByVal dateFrom As DateTime, ByVal dateTo As DateTime) As List(Of ACommon) Implements IService.GetAuditTrailACommon

        Dim ACommonList As New List(Of ACommon)
        Try

            Dim commonRetrieved As New DataSet
            ACommonList.Clear()

            commonRetrieved = AuditTrailReportBL.GetAuditTrailACommon(storeID, dateFrom, dateTo)
            For Each col As DataColumn In commonRetrieved.Tables(0).Columns
                col.ReadOnly = False
            Next

            If commonRetrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In commonRetrieved.Tables(0).Rows
                    row = FillRowWithNull(row, commonRetrieved.Tables(0).Columns)

                    Dim ACommonItem As New ACommon

                    ' ACommonItem.AOrderImage = String.Empty 'row("AOrderImage")
                    ACommonItem.ACommonStoreID = row("ACommonStoreID")
                    ACommonItem.ACommonID = row("ACommonID")
                    ACommonItem.ACommonCodeGroup = row("ACommonCodeGroup")
                    ACommonItem.ACommonCodeID = row("ACommonCodeID")
                    ACommonItem.ACommonCodeDescription = row("ACommonCodeDescription")
                    ACommonItem.ACommonStatus = row("ACommonStatus")
                    ACommonItem.ACommonCreateDte = row("ACommonCreateDte")
                    ACommonItem.ACommonCreateUserID = row("ACommonCreateUserID")
                    ACommonItem.ACommonUpdateDte = row("ACommonUpdateDte")
                    ACommonItem.ACommonUpdateUserID = row("ACommonUpdateUserID")
                    ACommonItem.ACommonAuditType = row("ACommonAuditType")
                    ACommonList.Add(ACommonItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ACommonList
    End Function

    ''' <summary>
    ''' Get the list of Consumer based on the search criteria;
    ''' 11 Oct 2016 - Pritpalkaur hunjan;
    ''' </summary>
    ''' <param name="dateFrom"></param>
    ''' <param name="dateTo"></param>
    ''' <param name="Alldate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAuditTrailAConsumer(ByVal storeID As String, ByVal dateFrom As DateTime, ByVal dateTo As DateTime) As List(Of AConsumer) Implements IService.GetAuditTrailAConsumer

        Dim AConsumerList As New List(Of AConsumer)
        Try

            Dim AConsumerds As New DataSet
            AConsumerList.Clear()

            AConsumerds = AuditTrailReportBL.GetAuditTrailAConsumer(storeID, dateFrom, dateTo)
            For Each col As DataColumn In AConsumerds.Tables(0).Columns
                col.ReadOnly = False
            Next

            If AConsumerds.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In AConsumerds.Tables(0).Rows
                    row = FillRowWithNull(row, AConsumerds.Tables(0).Columns)

                    Dim AConsumerItem As New AConsumer

                    ' ACommonItem.AOrderImage = String.Empty 'row("AOrderImage")
                    AConsumerItem.AConsumerStoreID = row("AConsumerStoreID")
                    AConsumerItem.AConsumerID = row("AConsumerID")
                    AConsumerItem.AConsumerDescription = row("AConsumerDescription")
                    AConsumerItem.ConsumerStoreRefUserID = row("ConsumerStoreRefUserID")
                    AConsumerItem.AConsumerStatus = row("AConsumerStatus")
                    AConsumerItem.AConsumerCreateDte = row("AConsumerCreateDte")
                    AConsumerItem.AConsumerCreateUserID = row("AConsumerCreateUserID")
                    AConsumerItem.AConsumerUpdateDte = row("AConsumerUpdateDte")
                    AConsumerItem.AConsumerUpdateUserID = row("AConsumerUpdateUserID")
                    AConsumerItem.AConsumerAuditType = row("AConsumerAuditType")

                    AConsumerList.Add(AConsumerItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return AConsumerList
    End Function

    ''' <summary>
    ''' Get the list of AEquipment based on the search criteria;
    ''' 11 Oct 2016 - Pritpalkaur hunjan;
    ''' </summary>
    ''' <param name="dateFrom"></param>
    ''' <param name="dateTo"></param>
    ''' <param name="Alldate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAuditTrailAEquipment(ByVal storeID As String, ByVal dateFrom As DateTime, ByVal dateTo As DateTime) As List(Of AEquipment) Implements IService.GetAuditTrailAEquipment

        Dim AEquipmentList As New List(Of AEquipment)
        Try

            Dim AEquipmentds As New DataSet
            AEquipmentList.Clear()

            AEquipmentds = AuditTrailReportBL.GetAuditTrailAEquipment(storeID, dateFrom, dateTo)
            For Each col As DataColumn In AEquipmentds.Tables(0).Columns
                col.ReadOnly = False
            Next

            If AEquipmentds.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In AEquipmentds.Tables(0).Rows
                    row = FillRowWithNull(row, AEquipmentds.Tables(0).Columns)

                    Dim AEquipmentdsItem As New AEquipment

                    AEquipmentdsItem.AEquipmentStoreID = row("AEquipmentStoreID")
                    AEquipmentdsItem.AEquipmentID = row("AEquipmentID")
                    AEquipmentdsItem.AEquipmentType = row("AEquipmentType")
                    AEquipmentdsItem.AEquipmentDescription = row("AEquipmentDescription")
                    AEquipmentdsItem.AEquipmentStatus = row("AEquipmentStatus")
                    AEquipmentdsItem.AEquipmentCreateDte = row("AEquipmentCreateDte")
                    AEquipmentdsItem.AEquipmentCreateUserID = row("AEquipmentCreateUserID")
                    AEquipmentdsItem.AEquipmentUpdateUserID = row("AEquipmentUpdateUserID")
                    AEquipmentdsItem.AEquipmentUpdateDte = row("AEquipmentUpdateDte")

                    AEquipmentdsItem.AEquipmentAuditType = row("AEquipmentAuditType")

                    AEquipmentList.Add(AEquipmentdsItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return AEquipmentList
    End Function

    ''' <summary>
    ''' Get the list of AStore based on the search criteria;
    ''' 11 Oct 2016 - Pritpalkaur hunjan;
    ''' </summary>
    ''' <param name="dateFrom"></param>
    ''' <param name="dateTo"></param>
    ''' <param name="Alldate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAuditTrailAStore(ByVal storeID As String, ByVal dateFrom As DateTime, ByVal dateTo As DateTime) As List(Of AStore) Implements IService.GetAuditTrailAStore

        Dim AStoreList As New List(Of AStore)
        Try

            Dim AStoreds As New DataSet
            AStoreList.Clear()

            AStoreds = AuditTrailReportBL.GetAuditTrailAStore(storeID, dateFrom, dateTo)
            For Each col As DataColumn In AStoreds.Tables(0).Columns
                col.ReadOnly = False
            Next

            If AStoreds.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In AStoreds.Tables(0).Rows
                    row = FillRowWithNull(row, AStoreds.Tables(0).Columns)

                    Dim AStoreItem As New AStore
                    AStoreItem.AStoreID = row("AStoreID")
                    AStoreItem.AStoreName = row("AStoreName")
                    AStoreItem.AStoreAddressType = row("AStoreAddressType")
                    AStoreItem.AStoreBlockHouseNo = row("AStoreBlockHouseNo")
                    AStoreItem.AStoreStreetName = row("AStoreStreetName")
                    AStoreItem.AStoreFloorNo = row("AStoreFloorNo")
                    AStoreItem.AStoreUnitNo = row("AStoreUnitNo")
                    AStoreItem.AStoreBuildingName = row("AStoreBuildingName")
                    AStoreItem.AStorePostalCode = row("AStorePostalCode")
                    AStoreItem.AStoreContactPersonName = row("AStoreContactPersonName")
                    AStoreItem.AStoreTelephoneNo = row("AStoreTelephoneNo")
                    AStoreItem.AStoreFaxNo = row("AStoreFaxNo")
                    AStoreItem.AStoreOtherInfo = row("AStoreOtherInfo")
                    AStoreItem.AStoreStatus = row("AStoreStatus")
                    AStoreItem.AStoreCreateDte = row("AStoreCreateDte")
                    AStoreItem.AStoreCreateUserID = row("AStoreCreateUserID")
                    AStoreItem.AStoreUpdateDte = row("AStoreUpdateDte")
                    AStoreItem.AStoreUpdateUserID = row("AStoreUpdateUserID")
                    AStoreItem.AStoreAuditType = row("AStoreAuditType")
                    AStoreList.Add(AStoreItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return AStoreList
    End Function

    ''' <summary>
    ''' Get the list of Common based on the search criteria;
    ''' 11 Oct 2016 - Pritpalkaur hunjan;
    ''' </summary>
    ''' <param name="dateFrom"></param>
    ''' <param name="dateTo"></param>
    ''' <param name="Alldate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAuditTrailASupplier(ByVal storeID As String, ByVal dateFrom As DateTime, ByVal dateTo As DateTime) As List(Of ASupplier) Implements IService.GetAuditTrailASupplier

        Dim ASupplierList As New List(Of ASupplier)
        Try

            Dim ASupplierds As New DataSet
            ASupplierList.Clear()

            ASupplierds = AuditTrailReportBL.GetAuditTrailASupplier(storeID, dateFrom, dateTo)
            For Each col As DataColumn In ASupplierds.Tables(0).Columns
                col.ReadOnly = False
            Next

            If ASupplierds.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In ASupplierds.Tables(0).Rows
                    row = FillRowWithNull(row, ASupplierds.Tables(0).Columns)

                    Dim ASupplierItem As New ASupplier

                    ASupplierItem.ASupplierStoreID = row("ASupplierStoreID")
                    ASupplierItem.ASupplierID = row("ASupplierID")
                    ASupplierItem.ASupplierUEN = row("ASupplierUEN")
                    ASupplierItem.ASupplierCompanyName = row("ASupplierCompanyName")
                    ASupplierItem.ASupplierAddressType = row("ASupplierAddressType")
                    ASupplierItem.ASupplierBlockHouseNo = row("ASupplierBlockHouseNo")
                    ASupplierItem.ASupplierStreetName = row("ASupplierStreetName")
                    ASupplierItem.ASupplierFloorNo = row("ASupplierFloorNo")
                    ASupplierItem.ASupplierUnitNo = row("ASupplierUnitNo")
                    ASupplierItem.ASupplierBuildingName = row("ASupplierBuildingName")
                    ASupplierItem.ASupplierPostalCode = row("ASupplierPostalCode")
                    ASupplierItem.ASupplierContactPersonName = row("ASupplierContactPersonName")
                    ASupplierItem.ASupplierTelephoneno = row("ASupplierTelephoneno")
                    ASupplierItem.ASupplierFaxNo = row("ASupplierFaxNo")
                    ASupplierItem.ASupplierOtherInfo = row("ASupplierOtherInfo")
                    ASupplierItem.ASupplierStatus = row("ASupplierStatus")
                    ASupplierItem.ASupplierCreateDte = row("ASupplierCreateDte")
                    ASupplierItem.ASupplierCreateUserID = row("ASupplierCreateUserID")
                    ASupplierItem.ASupplierUpdateDte = row("ASupplierUpdateDte")
                    ASupplierItem.ASupplierUpdateUserID = row("ASupplierUpdateUserID")
                    ASupplierItem.ASupplierAuditType = row("ASupplierAuditType")
                    ASupplierList.Add(ASupplierItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ASupplierList
    End Function

    ''' <summary>
    ''' Get the list of Common based on the search criteria;
    ''' 11 Oct 2016 - Pritpalkaur hunjan;
    ''' </summary>
    ''' <param name="dateFrom"></param>
    ''' <param name="dateTo"></param>
    ''' <param name="Alldate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAuditTrailAUserRole(ByVal storeID As String, ByVal dateFrom As DateTime, ByVal dateTo As DateTime) As List(Of AUserRole) Implements IService.GetAuditTrailAUserRole

        Dim AUserRoleList As New List(Of AUserRole)
        Try

            Dim AUserRoleds As New DataSet
            AUserRoleList.Clear()

            AUserRoleds = AuditTrailReportBL.GetAuditTrailAUserRole(storeID, dateFrom, dateTo)
            For Each col As DataColumn In AUserRoleds.Tables(0).Columns
                col.ReadOnly = False
            Next

            If AUserRoleds.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In AUserRoleds.Tables(0).Rows
                    row = FillRowWithNull(row, AUserRoleds.Tables(0).Columns)

                    Dim AUserRoleItem As New AUserRole

                    AUserRoleItem.AUserRoleStoreID = row("AUserRoleStoreID")
                    AUserRoleItem.AUserRoleCode = row("AUserRoleCode")
                    AUserRoleItem.AUserRoleUserID = row("AUserRoleUserID")
                    AUserRoleItem.ConsumerDescription = row("ConsumerDescription")
                    AUserRoleItem.AUserRoleDescription = row("AUserRoleDescription")
                    AUserRoleItem.AUserRoleDesignation = row("AUserRoleDesignation")
                    AUserRoleItem.AUserRoleDivisionCode = row("AUserRoleDivisionCode")
                    AUserRoleItem.AUserRoleInstallCode = row("AUserRoleInstallCode")
                    AUserRoleItem.AUserRoleDepartCode = row("AUserRoleDepartCode")
                    AUserRoleItem.AUserRoleSectionCode = row("AUserRoleSectionCode")
                    AUserRoleItem.AUserRoleStatus = row("AUserRoleStatus")
                    AUserRoleItem.AUserRoleCreateDte = row("AUserRoleCreateDte")
                    AUserRoleItem.AUserRoleCreateUserID = row("AUserRoleCreateUserID")
                    AUserRoleItem.AUserRoleUpdateDte = row("AUserRoleUpdateDte")
                    AUserRoleItem.AUserRoleUpdateUserID = row("AUserRoleUpdateUserID")
                    AUserRoleItem.AIsUserDeleted = row("AIsUserDeleted")
                    AUserRoleItem.AChangeStatusReason = row("AChangeStatusReason")
                    AUserRoleItem.AUserRoleAuditType = row("AUserRoleAuditType")
                    AUserRoleList.Add(AUserRoleItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return AUserRoleList
    End Function

    ''' <summary>
    ''' Get the list of Stock Transaction based on the search criteria;
    ''' 4 Jan 2009 - Liu Guo Feng;
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="dateFrom"></param>
    ''' <param name="dateTo"></param>
    ''' <param name="auditType"></param>
    ''' <param name="transType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAuditTrailStockTransaction(ByVal storeId As String, ByVal dateFrom As DateTime, ByVal dateTo As DateTime, ByVal auditType As String, ByVal transType As String, ByVal orderBy As String) As List(Of AStockTransactionDetails) Implements IService.GetAuditTrailStockTransaction

        Dim StockTransactionDetailsList As New List(Of AStockTransactionDetails)
        Try

            Dim StockTransactionRetrieved As New DataSet
            StockTransactionDetailsList.Clear()

            StockTransactionRetrieved = AuditTrailReportBL.GetAuditTrailStockTransaction(storeId, dateFrom, dateTo, auditType, transType, orderBy)
            For Each col As DataColumn In StockTransactionRetrieved.Tables(0).Columns
                col.ReadOnly = False
            Next

            If StockTransactionRetrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In StockTransactionRetrieved.Tables(0).Rows
                    row = FillRowWithNull(row, StockTransactionRetrieved.Tables(0).Columns)

                    Dim StockTransactionDetailsItem As New AStockTransactionDetails

                    StockTransactionDetailsItem.AStockItemDescription = row("AStockItemDescription")
                    StockTransactionDetailsItem.AStockItemUOM = row("AStockItemUOM")
                    StockTransactionDetailsItem.AStockTransactionAuditType = row("AStockTransactionAuditType")
                    StockTransactionDetailsItem.AStockTransactionCreateDte = row("AStockTransactionCreateDte")
                    StockTransactionDetailsItem.AStockTransactionDocNo = row("AStockTransactionDocNo")
                    StockTransactionDetailsItem.AStockTransactionDte = row("AStockTransactionDte")
                    StockTransactionDetailsItem.AStockTransactionInvolveID = row("AStockTransactionInvolveID")
                    StockTransactionDetailsItem.AStockTransactionQty = row("AStockTransactionQty")
                    StockTransactionDetailsItem.AStockTransactionRemarks = row("AStockTransactionRemarks")
                    StockTransactionDetailsItem.AStockTransactionSerialNo = row("AStockTransactionSerialNo")
                    StockTransactionDetailsItem.AStockTransactionStockItemID = row("AStockTransactionStockItemID")
                    StockTransactionDetailsItem.AStockTransactionStoreID = row("AStockTransactionStoreID")
                    StockTransactionDetailsItem.AStockTransactionTotalCost = row("AStockTransactionTotalCost")
                    StockTransactionDetailsItem.AStockTransactionType = row("AStockTransactionType")
                    StockTransactionDetailsItem.AStockTransactionUnitCost = row("AStockTransactionUnitCost")
                    StockTransactionDetailsItem.AStockTransactionUserID = row("AStockTransactionUserID")

                    StockTransactionDetailsList.Add(StockTransactionDetailsItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return StockTransactionDetailsList
    End Function

    ''' <summary>
    ''' Get the list of Residue Costs based on the search criteria;
    ''' 18 Oct 2010 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="month"></param>
    ''' <param name="year"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAuditTrailResidue(ByVal storeID As String, ByVal month As Integer, ByVal year As Integer) As List(Of AResidueDetails) Implements IService.GetAuditTrailResidue

        Dim ResidueDetailsList As New List(Of AResidueDetails)
        Try

            Dim ResidueRetrieved As New DataSet
            ResidueDetailsList.Clear()

            ResidueRetrieved = AuditTrailReportBL.GetAuditTrailResidue(storeID, month, year)

            For Each col As DataColumn In ResidueRetrieved.Tables(0).Columns
                col.ReadOnly = False
            Next

            If ResidueRetrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In ResidueRetrieved.Tables(0).Rows
                    row = FillRowWithNull(row, ResidueRetrieved.Tables(0).Columns)

                    Dim ResidueDetailsItem As New AResidueDetails

                    ResidueDetailsItem.StockItemImage = row("FStockItemImage")
                    ResidueDetailsItem.StockTransactionType = row("FStockTransactionType")
                    ResidueDetailsItem.StockItemID = row("FStockItemID")
                    ResidueDetailsItem.StockItemDescription = row("FStockItemDesc")
                    ResidueDetailsItem.StockItemUOM = row("FStockItemUOM")
                    ResidueDetailsItem.StockItemQty = row("FStockItemQty")
                    ResidueDetailsItem.StockItemTotalValue = row("FStockItemTotalValue")
                    ResidueDetailsItem.StockTransactionDte = row("FStockTransactionDte")
                    ResidueDetailsItem.StockItemRemarks = row("FStockItemRemarks")

                    ResidueDetailsList.Add(ResidueDetailsItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ResidueDetailsList

    End Function

#End Region

End Class
