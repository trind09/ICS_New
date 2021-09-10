Imports NEA_ICS.Business
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Partial Public Class Service

#Region " Enum "
    ''' <summary>
    ''' Enum for the column name to be used;
    ''' 07 Feb 09 - KG;
    ''' </summary>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    <DataContract()> _
    Public Enum ColumnName As Integer
        <EnumMember()> _
        OrderId = 2012
        <EnumMember()> _
        OrderGebizPONo = 2013
        <EnumMember()> _
        IssueSerialNo = 2035
        <EnumMember()> _
        DirectIssueSerialNo = 2046
        <EnumMember()> _
        AdjustSerialNo = 3004
        <EnumMember()> _
        AdjustInID = 3012
        <EnumMember()> _
        AdjustOutID = 3022
    End Enum

    ''' <summary>
    ''' Enum for the Module to be used;
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    <DataContract()> _
    Public Enum ModuleName As Integer
        <EnumMember()> _
            Receive = 202
        <EnumMember()> _
            Request = 203
        <EnumMember()> _
            DirectIssue = 204
        <EnumMember()> _
            AdjustIn = 301
        <EnumMember()> _
            AdjustOut = 302
    End Enum
#End Region

#Region " Common Function "
    ''' <summary>
    ''' Check the field if it is a Unique value;
    ''' 05Feb09 - KG;
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="columnName"></param>
    ''' <param name="columnValue"></param>
    ''' <param name="pkColumnValue"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Function FieldIsUnique(ByVal storeId As String _
                           , ByVal columnName As ColumnName _
                           , ByVal columnValue As String _
                           , ByVal pkColumnValue As String _
                           ) As Boolean Implements IService.FieldIsUnique

        Try
            Dim BL As StockControlBL.ColumnName

            ' convert IService Enum to BL Enum
            Select Case columnName
                Case columnName.OrderId
                    BL = StockControlBL.ColumnName.OrderID
                Case columnName.OrderGebizPONo
                    BL = StockControlBL.ColumnName.GebizPONo
                Case columnName.DirectIssueSerialNo
                    BL = StockControlBL.ColumnName.DirectIssueSerialNo
                Case columnName.IssueSerialNo
                    BL = StockControlBL.ColumnName.IssueSerialNo
                Case columnName.AdjustSerialNo
                    BL = StockControlBL.ColumnName.AdjustSerialNo
                Case columnName.AdjustInID
                    BL = StockControlBL.ColumnName.AdjustIn
                Case Service.ColumnName.AdjustOutID
                    BL = StockControlBL.ColumnName.AdjustOut
            End Select
            Return StockControlBL.FieldIsUnique(storeId _
                                                , BL _
                                                , columnValue _
                                                , pkColumnValue _
                                                )

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw
        End Try
    End Function

    ''' <summary>
    ''' Get More Item Info
    ''' 08Feb09 - KG;
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="stockItem"></param>
    ''' <param name="asOfDte"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Function GetMoreItemInfo(ByVal storeId As String _
                             , ByVal stockItem As String _
                             , ByVal asOfDte As Date _
                             ) As MoreItemInfoDetails Implements IService.GetMoreItemInfo

        Dim DetailsItem As New MoreItemInfoDetails

        Try
            Dim RecordRetrieved As New DataSet

            RecordRetrieved = StockControlBL.GetMoreItemInfo(storeId _
                                                             , stockItem _
                                                             , asOfDte _
                                                             )

            If RecordRetrieved.Tables(0).Rows.Count = 1 Then
                Dim row As DataRow = RecordRetrieved.Tables(0).Rows.Item(0)
                row = FillRowWithNull(row, RecordRetrieved.Tables(0).Columns)

                DetailsItem = New MoreItemInfoDetails(row("ItemID") _
                                                      , row("MaxLevel") _
                                                      , row("Balance") _
                                                      , row("AvgUnitCost") _
                                                      , row("TotalValue") _
                                                      , row("OnOrderQty") _
                                                      , row("OnOrderCount") _
                                                      )
            Else
                Throw New Exception("Error: Expecting only a record return")
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw
        End Try
        Return DetailsItem
    End Function

    ''' <summary>
    ''' Get the Last Serial No for the given table name;
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="moduleName"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Function GetLastSerialNo(ByVal storeID As String _
                             , ByVal moduleName As Service.ModuleName _
                             ) As String Implements IService.GetLastSerialNo
        Try
            Dim TableName As StockControlBL.TableName
            Dim DocType As String = String.Empty

            ' Convert service Enum to BL Enum
            Select Case moduleName
                Case moduleName.AdjustIn
                    TableName = StockControlBL.TableName.Adjust
                    DocType = "AI"
                Case moduleName.AdjustOut
                    TableName = StockControlBL.TableName.Adjust
                    DocType = "AO"
                Case moduleName.DirectIssue
                    TableName = StockControlBL.TableName.DirectIssue
                Case moduleName.Request
                    TableName = StockControlBL.TableName.Issue
            End Select

            Return StockControlBL.GetLastSerialNo(storeID, TableName, DocType)
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw

            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Check Financial Cutoff date, if within return TRUE;
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="tranDte"></param>
    ''' <returns>True = within; False = Not within</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Function WithinFinancialCutoff(ByVal storeID As String _
                                   , ByVal tranDte As Date _
                                   ) As Boolean Implements IService.WithinFinancialCutoff
        Try
            Return StockControlBL.WithinFinancialCutoff(storeID _
                                                        , tranDte _
                                                        )
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw
        End Try
    End Function

    ''' <summary>
    ''' Delete all stock transaction for a single document Reference (receive, issue, adjustment) on a specific date;
    ''' 11Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="docNo"></param>
    ''' <param name="stockControlType"></param>
    ''' <param name="originalDte"></param>
    ''' <param name="loginUser"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Sub DeleteAllStockTransaction(ByVal storeID As String _
                                  , ByVal docNo As String _
                                  , ByVal stockControlType As Service.ModuleName _
                                  , ByVal originalDte As Date _
                                  , ByVal loginUser As String _
                                  ) Implements IService.DeleteAllStockTransaction
        Try
            Dim SCType As StockControlBL.StockControlType

            ' Convert service Enum to BL Enum
            Select Case stockControlType
                Case ModuleName.AdjustIn
                    SCType = StockControlBL.StockControlType.AdjustInward
                Case ModuleName.AdjustOut
                    SCType = StockControlBL.StockControlType.AdjustOutward
                Case ModuleName.Receive
                    SCType = StockControlBL.StockControlType.ReceiveOrder
                Case ModuleName.Request
                    SCType = StockControlBL.StockControlType.IssueRequest
            End Select

            StockControlBL.DeleteAllStockTransaction(storeID _
                                                     , docNo _
                                                     , SCType _
                                                     , originalDte _
                                                     , loginUser _
                                                     )
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

#End Region

#Region " Order "
    ''' <summary>
    ''' Add Order n Order Items
    ''' 05Feb09 - KG;
    ''' </summary>
    ''' <param name="orderDetails"></param>
    ''' <param name="orderItemDetails"></param>
    ''' <returns>Error Message (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Function AddOrder(ByVal orderDetails As OrderDetails _
                      , ByVal orderItemDetails As List(Of OrderItemDetails) _
                      ) As String Implements IService.AddOrder
        Try
            Dim OrderItemList As New List(Of OrderItem)
            Dim OrderItem As OrderItem

            'convert wcf list to BL list
            For Each item In orderItemDetails
                OrderItem = New OrderItem(item.OrderItemID _
                                          , item.StockItemID _
                                          , item.Qty _
                                          , item.TotalCost _
                                          , item.ExpectedDeliveryDte _
                                          , item.WarrantyDte _
                                          , item.Remarks _
                                          , item.Status _
                                          , item.Mode _
                                          )
                OrderItemList.Add(OrderItem)
            Next

            Return StockControlBL.AddOrder(orderDetails.StoreID _
                                           , orderDetails.OrderID _
                                           , orderDetails.GebizPONo _
                                           , orderDetails.Type _
                                           , orderDetails.Dte _
                                           , orderDetails.SupplierID _
                                           , orderDetails.LoginUser _
                                           , OrderItemList _
                                           )
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw

            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Get Order
    ''' 05Feb09 - KG;
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="status"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Function GetOrder(ByVal storeID As String _
                      , Optional ByVal status As String = "O" _
                      ) As List(Of OrderDetails) Implements IService.GetOrder
        Try
            Dim OrderList As New List(Of OrderDetails)
            Dim Retrieved As New DataSet
            Dim OrderDetailsItem As New OrderDetails

            OrderList.Clear()

            Retrieved = StockControlBL.GetOrder(storeID _
                                                      , status _
                                                      )

            If Retrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In Retrieved.Tables(0).Rows
                    row = FillRowWithNull(row, Retrieved.Tables(0).Columns)
                    OrderDetailsItem = New OrderDetails

                    OrderDetailsItem.StoreID = row("OrderStoreID")
                    OrderDetailsItem.OrderID = row("OrderID")
                    OrderDetailsItem.GebizPONo = row("OrderGebizPONo")
                    OrderDetailsItem.Type = row("OrderType")
                    OrderDetailsItem.Dte = row("OrderDte")
                    OrderDetailsItem.SupplierID = row("OrderSupplierID")
                    OrderDetailsItem.Status = row("OrderStatus")

                    OrderList.Add(OrderDetailsItem)
                Next
            End If
            Return OrderList

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw

            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Get Order Item
    ''' 05Feb09 - KG;
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="orderId"></param>
    ''' <returns>Order Items DataSet Collection</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Function GetOrderItem(ByVal storeId As String _
                          , ByVal orderId As String _
                          , Optional ByVal unfullfillOnly As Boolean = False _
                          ) As List(Of OrderItemDetails) Implements IService.GetOrderItem
        Try
            Dim OrderItemList As New List(Of OrderItemDetails)
            Dim Retrieved As New DataSet
            Dim OrderItemDetailsItem As New OrderItemDetails

            OrderItemList.Clear()

            Retrieved = StockControlBL.GetOrderItem(storeId _
                                                    , orderId _
                                                    , unfullfillOnly _
                                                    )

            If Retrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In Retrieved.Tables(0).Rows
                    row = FillRowWithNull(row, Retrieved.Tables(0).Columns)
                    OrderItemDetailsItem = New OrderItemDetails

                    OrderItemDetailsItem.OrderItemID = row("OrderItemID")
                    OrderItemDetailsItem.StockItemID = row("OrderItemStockItemID")
                    OrderItemDetailsItem.Qty = row("OrderItemQty")
                    OrderItemDetailsItem.TotalCost = row("OrderItemTotalCost")
                    OrderItemDetailsItem.ExpectedDeliveryDte = row("OrderItemExpectedDeliveryDte")
                    OrderItemDetailsItem.WarrantyDte = row("OrderItemWarrantyDte")
                    OrderItemDetailsItem.Remarks = row("OrderItemRemarks")
                    OrderItemDetailsItem.Status = row("OrderItemStatus")
                    OrderItemDetailsItem.ReceiveItemQtyReceived = row("ReceiveItemQtyReceived")

                    OrderItemList.Add(OrderItemDetailsItem)
                Next
            End If
            Return OrderItemList

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw

            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Update Order or OrderItem or Both Details, based on the orderitem's mode to either Delete, Insert or Update;
    ''' 05Feb09 - KG;
    ''' </summary>
    ''' <param name="orderDetails"></param>
    ''' <param name="orderItemDetails"></param>
    ''' <returns>Error Message (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Function UpdateOrder(ByVal orderDetails As OrderDetails _
                         , ByVal orderItemDetails As List(Of OrderItemDetails) _
                         ) As String Implements IService.UpdateOrder

        Try
            Dim OrderItemList As New List(Of OrderItem)
            Dim OrderItem As OrderItem

            'convert wcf list to BL list
            For Each item In orderItemDetails
                OrderItem = New OrderItem(item.OrderItemID _
                                          , item.StockItemID _
                                          , item.Qty _
                                          , item.TotalCost _
                                          , item.ExpectedDeliveryDte _
                                          , item.WarrantyDte _
                                          , item.Remarks _
                                          , item.Status _
                                          , item.Mode _
                                          )
                OrderItemList.Add(OrderItem)
            Next

            Return StockControlBL.UpdateOrder(orderDetails.StoreID _
                                              , orderDetails.OrderID _
                                              , orderDetails.GebizPONo _
                                              , orderDetails.Type _
                                              , orderDetails.Dte _
                                              , orderDetails.SupplierID _
                                              , orderDetails.LoginUser _
                                              , orderDetails.Mode _
                                              , OrderItemList _
                                              )
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw

            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Delete Order and Order Items details;
    ''' 05Feb09 - KG;
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="orderId"></param>
    ''' <param name="status"></param>
    ''' <param name="loginUser"></param>
    ''' <returns>error message (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Function DeleteOrder(ByVal storeId As String _
                         , ByVal orderId As String _
                         , ByVal status As String _
                         , ByVal loginUser As String _
                         ) As String Implements IService.DeleteOrder

        Dim ErrorMessage As String = String.Empty

        Try
            ErrorMessage = StockControlBL.DeleteOrder(storeId _
                                                      , orderId _
                                                      , status _
                                                      , loginUser _
                                                      )

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw
        End Try

        Return ErrorMessage
    End Function

    ''' <summary>
    ''' Get all Order list by parameters;
    ''' when orderID is with value, ignore other parameters and get only order items relates to the single Order Reference
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="status"></param>
    ''' <param name="fromDte"></param>
    ''' <param name="toDte"></param>
    ''' <param name="fromStockItemID"></param>
    ''' <param name="toStockItemID"></param>
    ''' <param name="orderID"></param>
    ''' <returns>Orders DataSet</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Function GetOrderList(ByVal storeID As String _
                          , Optional ByVal status As String = "" _
                          , Optional ByVal fromDte As Date = #12:00:00 AM# _
                          , Optional ByVal toDte As Date = #12:00:00 AM# _
                          , Optional ByVal fromStockItemID As String = "" _
                          , Optional ByVal toStockItemID As String = "" _
                          , Optional ByVal orderId As String = "" _
                          ) As List(Of OrderList) Implements IService.GetOrderList
        Try
            Dim List As New List(Of OrderList)
            Dim Retrieved As New DataSet
            Dim Item As New OrderList
            Dim theStatus As String

            Select Case status.ToUpper
                Case "A"
                    theStatus = ""
                Case "U"
                    theStatus = "O"
                Case "F"
                    theStatus = "C"
                Case Else
                    theStatus = ""
            End Select

            List.Clear()

            Retrieved = StockControlBL.GetOrderList(storeID _
                                                    , theStatus _
                                                    , fromDte _
                                                    , toDte _
                                                    , fromStockItemID _
                                                    , toStockItemID _
                                                    , orderId _
                                                    )

            If Retrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In Retrieved.Tables(0).Rows
                    row = FillRowWithNull(row, Retrieved.Tables(0).Columns)
                    Item = New OrderList

                    Item.StockType = row("StockItemStockType")
                    Item.StockItemID = row("OrderItemStockItemID")
                    Item.Description = row("StockItemDescription")
                    Item.UOM = row("StockItemUOM")
                    Item.OrderID = row("OrderItemOrderID")
                    Item.SupplierID = row("OrderSupplierID")
                    Item.OrderDte = row("OrderDte")
                    Item.OrderQty = row("OrderItemQty")
                    Item.TotalCost = row("OrderItemTotalCost")
                    If Not row.IsNull("OrderItemRemarks") Then
                        Item.Remarks = row("OrderItemRemarks")
                    End If
                    Item.Status = row("OrderStatus")
                    If Not row.IsNull("RecievedQty") Then
                        Item.TotalReceiveQty = row("RecievedQty")
                    End If
                    If Not row.IsNull("LastRecievedDte") Then
                        Item.LastReceiveDte = row("LastRecievedDte")
                    End If
                    If Not row.IsNull("LastRecievedQty") Then
                        Item.LastReceiveQty = row("LastRecievedQty")
                    End If

                    Select Case status
                        Case "A"
                            List.Add(Item)
                        Case "F"
                            If Item.OrderQty = Item.TotalReceiveQty Then
                                List.Add(Item)
                            End If
                        Case "U"
                            If Item.OrderQty > Item.TotalReceiveQty Then
                                List.Add(Item)
                            End If
                        Case Else
                            List.Add(Item)
                    End Select

                Next
            End If
            Return List

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw

            Return Nothing
        End Try
    End Function

#End Region

#Region " Direct Issue "

    ''' <summary>
    ''' Function - AddDirectIssue;
    ''' 13 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="directIssueDetails"></param>
    ''' <param name="directIssueItemList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddDirectIssue(ByVal directIssueDetails As DirectIssueDetails, _
                                   ByVal directIssueItemList As List(Of DirectIssueDetails), _
                                   ByRef directIssueDocNo As String) As String Implements IService.AddDirectIssue

        Dim errorMessage As String = String.Empty
        Dim IssueItemList As New List(Of DirectIssueItem)

        Try

            For Each item As DirectIssueDetails In directIssueItemList
                Dim IssueItem As New DirectIssueItem(item.ItemID, item.ItemDescription, item.StockType, _
                                                     item.ItemQty, item.UOM, item.TotalCost, item.Remarks, _
                                                     item.Status, item.Mode)
                IssueItemList.Add(IssueItem)

            Next

            errorMessage = StockControlBL.AddDirectIssue(directIssueDetails.StoreID, directIssueDetails.ConsumerID, _
                                                         directIssueDetails.IssueType, directIssueDetails.SerialNo, _
                                                         directIssueDetails.DirectIssueDate, directIssueDetails.Status, _
                                                         IssueItemList, directIssueDetails.LoginUser, directIssueDocNo)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw
        End Try

        Return errorMessage
    End Function

    ''' <summary>
    ''' Function - DeleteDirectIssue;
    ''' 14 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="directIssueDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DeleteDirectIssue(ByVal directIssueDetails As DirectIssueDetails) As String Implements IService.DeleteDirectIssue

        Dim ErrorMessge As String = String.Empty

        Try

            ErrorMessge = StockControlBL.DeleteDirectIssue(directIssueDetails.StoreID, _
                                                           directIssueDetails.DirectIssueID)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw
        End Try

        Return ErrorMessge
    End Function

    ''' <summary>
    ''' Function - UpdateDirectIssue;
    ''' 18 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="directIssueDetails"></param>
    ''' <param name="directIssueItemList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateDirectIssue(ByVal directIssueDetails As DirectIssueDetails, _
                                      ByVal directIssueItemList As List(Of DirectIssueDetails)) As String _
                                       Implements IService.UpdateDirectIssue


        Dim errorMessage As String = String.Empty
        Dim IssueItemList As New List(Of DirectIssueItem)

        Try

            For Each item As DirectIssueDetails In directIssueItemList
                Dim IssueItem As New DirectIssueItem(item.ItemID, item.ItemDescription, item.StockType, _
                                                     item.ItemQty, item.UOM, item.TotalCost, item.Remarks, _
                                                     item.Status, item.Mode)
                IssueItemList.Add(IssueItem)

            Next

            errorMessage = StockControlBL.UpdateDirectIssue(directIssueDetails.DirectIssueID, _
                                                            directIssueDetails.StoreID, _
                                                            directIssueDetails.ConsumerID, _
                                                            directIssueDetails.DocumentNo, _
                                                            directIssueDetails.IssueType, _
                                                            directIssueDetails.SerialNo, _
                                                            directIssueDetails.DirectIssueDate, _
                                                            directIssueDetails.Status, _
                                                            directIssueDetails.Mode, _
                                                            IssueItemList, directIssueDetails.LoginUser)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw
        End Try

        Return errorMessage
    End Function

    ''' <summary>
    ''' Function - GetDirectIssueID;
    ''' 14 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="directIssueDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDirectIssueID(ByVal directIssueDetails As DirectIssueDetails) _
                                     As List(Of DirectIssueDetails) Implements IService.GetDirectIssueID

        Dim DirectIssueList As New List(Of DirectIssueDetails)

        Try
            Dim DirectIssueRetrieved As New DataSet

            DirectIssueList.Clear()

            DirectIssueRetrieved = StockControlBL.GetDirectIssueID(directIssueDetails.StoreID)

            If DirectIssueRetrieved.Tables(0).Rows.Count > 0 Then

                For Each row As DataRow In DirectIssueRetrieved.Tables(0).Rows
                    row = FillRowWithNull(row, DirectIssueRetrieved.Tables(0).Columns)

                    Dim DirectIssueDetailsItem As New DirectIssueDetails

                    DirectIssueDetailsItem.DirectIssueID = row("DirectIssueID")
                    DirectIssueDetailsItem.DocumentNo = row("DirectIssueDocNo")

                    DirectIssueList.Add(DirectIssueDetailsItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw
        End Try

        Return DirectIssueList
    End Function

    ''' <summary>
    ''' Function  - GetDirectIssueInfo;
    ''' 18 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="directIssueDetails"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Change Requirement:
    ''' 1) To use 'GetLastSerialNo' instead
    ''' </remarks>
    Public Function GetDirectIssueInfo(ByVal directIssueDetails As DirectIssueDetails) As String _
                                       Implements IService.GetDirectIssueInfo

        Dim DirectIssueRetrieved As String = String.Empty  'New DataSet

        Try

            DirectIssueRetrieved = StockControlBL.GetDirectIssueInfo(directIssueDetails.StoreID)

            'If DirectIssueRetrieved.Tables(0).Rows.Count > 0 Then

            '    For Each row As DataRow In DirectIssueRetrieved.Tables(0).Rows
            '        row = FillRowWithNull(row, DirectIssueRetrieved.Tables(0).Columns)

            '        Dim DirectIssueDetailsItem As New DirectIssueDetails

            '        DirectIssueDetailsItem.DocumentNo = row("DirectIssueDocNo")
            '        DirectIssueDetailsItem.SerialNo = row("DirectIssueSerialNo")

            '        DirectIssueList.Add(DirectIssueDetailsItem)

            '    Next
            'End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw
        End Try

        Return DirectIssueRetrieved
    End Function

    ''' <summary>
    ''' Function - GetDirectIssueList
    ''' 25 Feb 09 - Guo Feng;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="dteIssueFrom"></param>
    ''' <param name="dteIssueTo"></param>
    ''' <param name="docNo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDirectIssueList(ByVal storeID As String, _
                                          ByVal dteIssueFrom As Date, ByVal dteIssueTo As Date, ByVal docNo As String) _
                                          As List(Of DirectIssueDetails) Implements IService.GetDirectIssueList

        Try
            Dim directIssueList As New List(Of DirectIssueDetails)
            Dim ItemDetails As DirectIssueDetails
            Dim itemRetrieved As New DataSet

            directIssueList.Clear()

            itemRetrieved = StockControlBL.GetDirectIssueList(storeID _
                                                    , dteIssueFrom _
                                                    , dteIssueTo _
                                                    , docNo _
                                                    )

            If itemRetrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In itemRetrieved.Tables(0).Rows
                    row = FillRowWithNull(row, itemRetrieved.Tables(0).Columns)
                    ItemDetails = New DirectIssueDetails()

                    ItemDetails.ConsumerID = row("DirectIssueConsumerID")
                    ItemDetails.DirectIssueDate = row("DirectIssueDte")
                    ItemDetails.DirectIssueID = row("DirectIssueItemDirectIssueID")
                    ItemDetails.DocumentNo = row("DirectIssueDocNo")
                    ItemDetails.IssueType = row("DirectIssueItemStockType")
                    ItemDetails.ItemDescription = row("DirectIssueItemStockDescription")
                    ItemDetails.ItemID = row("DirectIssueItemStockItemID")
                    ItemDetails.ItemQty = row("DirectIssueItemQty")
                    'ItemDetails.LoginUser = row("")
                    'ItemDetails.Mode = row("")
                    ItemDetails.SerialNo = row("DirectIssueSerialNo")
                    If Not row.IsNull("DirectIssueItemRemarks") Then
                        ItemDetails.Remarks = row("DirectIssueItemRemarks")
                    End If
                    ItemDetails.Status = row("DirectIssueItemStatus")
                    ItemDetails.StockType = row("DirectIssueItemStockType")
                    'ItemDetails.StoreID = row("")
                    ItemDetails.TotalCost = row("DirectIssueItemTotalCost")
                    ItemDetails.UOM = row("DirectIssueItemUOM")

                    directIssueList.Add(ItemDetails)
                Next
            End If
            Return directIssueList

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw

            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' Function - GetDirectIssues;
    ''' 14 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="directIssueDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDirectIssues(ByVal directIssueDetails As DirectIssueDetails) _
                                    As List(Of DirectIssueDetails) Implements IService.GetDirectIssues

        Dim DirectIssueList As New List(Of DirectIssueDetails)

        Try
            Dim DirectIssueRetrieved As New DataSet

            DirectIssueList.Clear()

            DirectIssueRetrieved = StockControlBL.GetDirectIssues(directIssueDetails.StoreID, _
                                                                  directIssueDetails.DirectIssueID, _
                                                                  directIssueDetails.SerialNo)

            If DirectIssueRetrieved.Tables(0).Rows.Count > 0 Then

                For Each row As DataRow In DirectIssueRetrieved.Tables(0).Rows
                    row = FillRowWithNull(row, DirectIssueRetrieved.Tables(0).Columns)

                    Dim DirectIssueDetailsItem As New DirectIssueDetails

                    DirectIssueDetailsItem.ConsumerID = row("DirectIssueConsumerID")
                    DirectIssueDetailsItem.IssueType = row("DirectIssueType")
                    DirectIssueDetailsItem.DirectIssueDate = row("DirectIssueDte")
                    DirectIssueDetailsItem.SerialNo = row("DirectIssueSerialNo")

                    DirectIssueList.Add(DirectIssueDetailsItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw
        End Try

        Return DirectIssueList
    End Function

    ''' <summary>
    ''' Function - GetDirectIssueItems;
    ''' 14 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="directIssueDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDirectIssueItems(ByVal directIssueDetails As DirectIssueDetails) As  _
                                             List(Of DirectIssueDetails) Implements IService.GetDirectIssueItems

        Dim DirectIssueList As New List(Of DirectIssueDetails)

        Try
            Dim DirectIssueRetrieved As New DataSet

            DirectIssueList.Clear()

            DirectIssueRetrieved = StockControlBL.GetDirectIssueItems(directIssueDetails.DirectIssueID)

            If DirectIssueRetrieved.Tables(0).Rows.Count > 0 Then

                For Each row As DataRow In DirectIssueRetrieved.Tables(0).Rows
                    row = FillRowWithNull(row, DirectIssueRetrieved.Tables(0).Columns)

                    Dim DirectIssueDetailsItem As New DirectIssueDetails

                    DirectIssueDetailsItem.ItemID = row("DirectIssueItemID")
                    DirectIssueDetailsItem.ItemDescription = row("DirectIssueItemStockDescription")
                    DirectIssueDetailsItem.ItemQty = row("DirectIssueItemQty")
                    DirectIssueDetailsItem.UOM = row("DirectIssueItemUOM")
                    DirectIssueDetailsItem.TotalCost = row("DirectIssueItemTotalCost")
                    DirectIssueDetailsItem.Remarks = row("DirectIssueItemRemarks")

                    DirectIssueList.Add(DirectIssueDetailsItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw
        End Try

        Return DirectIssueList
    End Function
#End Region

#Region " Receive "
    ''' <summary>
    ''' Get the Receive Items for a single Order Reference on a specific date;
    ''' including the outstanding quantity for this order item remaining as current;
    ''' 14Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="orderID"></param>
    ''' <param name="dte"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Function GetReceiveItem(ByVal storeID As String _
                            , ByVal orderID As String _
                            , ByVal dte As Date _
                            ) As List(Of ReceiveItemDetails) Implements IService.GetReceiveItem
        Try
            Dim ReceiveList As New List(Of ReceiveItemDetails)
            Dim Retrieved As New DataSet
            Dim ReceiveDetailsItem As New ReceiveItemDetails

            ReceiveList.Clear()

            Retrieved = StockControlBL.GetReceiveItem(storeID _
                                                      , orderID _
                                                      , dte _
                                                      )

            If Retrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In Retrieved.Tables(0).Rows
                    row = FillRowWithNull(row, Retrieved.Tables(0).Columns)
                    ReceiveDetailsItem = New ReceiveItemDetails
                    ReceiveDetailsItem.TranID = IIf(Not row.IsNull("StockTransactionID"), row("StockTransactionID"), 0)
                    ReceiveDetailsItem.StockItemID = row("OrderItemStockItemID")
                    ReceiveDetailsItem.Qty = row("StockTransactionQty")
                    ReceiveDetailsItem.TotalCost = row("StockTransactionTotalCost")
                    ReceiveDetailsItem.Remarks = row("StockTransactionRemarks")
                    ReceiveDetailsItem.OrderItemID = row("OrderItemID")
                    ReceiveDetailsItem.Status = row("StockTransactionStatus")
                    ReceiveDetailsItem.OrderItemWarrantyDte = row("OrderItemWarrantyDte")
                    ReceiveDetailsItem.OrderItemQtyOutstanding = row("OrderItemQtyOutstanding")
                    ReceiveDetailsItem.OrderItemUnitCost = row("OrderItemUnitCost")

                    ReceiveList.Add(ReceiveDetailsItem)
                Next
            End If
            Return ReceiveList

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw

            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Get Receive Date under the same Order Item
    ''' 14Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="orderID"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Function GetReceiveDte(ByVal storeID As String _
                           , ByVal orderID As String _
                           ) As List(Of Date) Implements IService.GetReceiveDte
        Try
            Dim DteList As New List(Of Date)
            Dim Retrieved As New DataSet

            DteList.Clear()

            Retrieved = StockControlBL.GetReceiveDte(storeID _
                                                     , orderID _
                                                     )

            If Retrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In Retrieved.Tables(0).Rows
                    row = FillRowWithNull(row, Retrieved.Tables(0).Columns)
                    DteList.Add(CDate(row(0)))
                Next
            End If
            Return DteList

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw

            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Update the Receive Items 
    ''' 12Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="type"></param>
    ''' <param name="dte"></param>
    ''' <param name="receiveDetails"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Function UpdateReceiveItem(ByVal storeID As String _
                               , ByVal type As String _
                               , ByVal dte As Date _
                               , ByVal loginUser As String _
                               , ByVal receiveDetails As List(Of ReceiveItemDetails) _
                               ) As String Implements IService.UpdateReceiveItem


        Try
            Dim ReceiveItem As ReceiveItem
            Dim ReceiveItemList As New List(Of ReceiveItem)

            'convert wcf list to BL list
            For Each item In receiveDetails
                ReceiveItem = New ReceiveItem(item.TranID _
                                              , item.StockItemID _
                                              , item.Qty _
                                              , item.TotalCost _
                                              , item.Remarks _
                                              , item.OrderItemID _
                                              , item.Status _
                                              , item.OrderItemWarrantyDte _
                                              , item.Mode _
                                              )

                ReceiveItemList.Add(ReceiveItem)
            Next

            Return StockControlBL.UpdateReceiveItem(storeID _
                                                    , type _
                                                    , dte _
                                                    , loginUser _
                                                    , ReceiveItemList _
                                                    )
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw

            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Get all Order list by parameters;
    ''' when orderID is with value, ignore other parameters and get only order items relates to the single Order Reference
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="fromDte"></param>
    ''' <param name="toDte"></param>
    ''' <param name="fromStockItemID"></param>
    ''' <param name="toStockItemID"></param>
    ''' <param name="orderID"></param>
    ''' <returns>Orders DataSet</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Function GetReceiveList(ByVal storeID As String _
                            , Optional ByVal fromDte As Date = #12:00:00 AM# _
                            , Optional ByVal toDte As Date = #12:00:00 AM# _
                            , Optional ByVal fromStockItemID As String = "" _
                            , Optional ByVal toStockItemID As String = "" _
                            , Optional ByVal orderId As String = "" _
                            ) As List(Of ReceiveList) Implements IService.GetReceiveList
        Try
            Dim List As New List(Of ReceiveList)
            Dim Retrieved As New DataSet
            Dim ReceiveDetailsItem As ReceiveList

            List.Clear()

            Retrieved = StockControlBL.GetReceiveList(storeID _
                                                      , fromDte _
                                                      , toDte _
                                                      , fromStockItemID _
                                                      , toStockItemID _
                                                      , orderId _
                                                      )

            If Retrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In Retrieved.Tables(0).Rows
                    row = FillRowWithNull(row, Retrieved.Tables(0).Columns)
                    ReceiveDetailsItem = New ReceiveList

                    ReceiveDetailsItem.StockType = row("FReceiveByStockRangeStockType")
                    ReceiveDetailsItem.StockItemID = row("FReceiveByStockRangeStockItemID")
                    If Not row.IsNull("FReceiveByStockRangeStockItemDesc") Then
                        ReceiveDetailsItem.Description = row("FReceiveByStockRangeStockItemDesc")
                    End If
                    ReceiveDetailsItem.UOM = row("FReceiveByStockRangeUOM")
                    ReceiveDetailsItem.OrderID = row("FReceiveByStockRangeDocNo")
                    ReceiveDetailsItem.ReceiveDte = row("FReceiveByStockRangeDte")
                    ReceiveDetailsItem.ReceiveQty = row("FReceiveByStockRangeQty")
                    ReceiveDetailsItem.TotalCost = row("FReceiveByStockRangeTotalCost")
                    If Not row.IsNull("FReceiveByStockRangeRemarks") Then
                        ReceiveDetailsItem.Remarks = row("FReceiveByStockRangeRemarks")
                    End If
                    ReceiveDetailsItem.Status = row("FReceiveByStockRangeStatus")
                    If Not row.IsNull("FReceiveByStockRangeWarrantyDte") Then
                        ReceiveDetailsItem.OrderDte = row("FReceiveByStockRangeWarrantyDte")
                    End If

                    List.Add(ReceiveDetailsItem)
                Next
            End If
            Return List

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw

            Return Nothing
        End Try
    End Function
#End Region

#Region " Issue from Store (Request, Approve, Issue) "
    ''' <summary>
    ''' Add New Request and its Item Details;
    ''' 20Feb09 - KG;
    ''' </summary>
    ''' <param name="requestDetails"></param>
    ''' <param name="ItemDetails"></param>
    ''' <returns>Error Message (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Function AddRequest(ByVal requestDetails As RequestDetails _
                      , ByVal ItemDetails As List(Of IssueItemDetails) _
                      ) As String Implements IService.AddRequest
        Try
            Dim List As New List(Of RequestItem)
            Dim RequestItem As RequestItem

            'convert wcf list to BL list
            For Each Item In ItemDetails
                RequestItem = New RequestItem(requestDetails.StoreID _
                                              , requestDetails.RequestID _
                                              , Item.RequestItemID _
                                              , Item.StockItemID _
                                              , Item.RequestItemQty _
                                              , Item.Status _
                                              , requestDetails.LoginUser _
                                              , Item.Mode _
                                              )
                List.Add(RequestItem)
            Next

            Return StockControlBL.AddRequest(requestDetails.StoreID _
                                             , requestDetails.ConsumerID _
                                             , requestDetails.RequestID _
                                             , requestDetails.Type _
                                             , requestDetails.Sought _
                                             , requestDetails.LoginUser _
                                             , List _
                                             )
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw

            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Get Request based on status, default all Open Request only;
    ''' 20Feb09 - KG;
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="status"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Function GetRequest(ByVal storeID As String _
                        , Optional ByVal status As String = "O" _
                        ) As List(Of RequestDetails) Implements IService.GetRequest
        Try
            Dim List As New List(Of RequestDetails)
            Dim Retrieved As New DataSet
            Dim RequestDetailsItem As New RequestDetails

            List.Clear()

            Retrieved = StockControlBL.GetRequest(storeID _
                                                  , status _
                                                  )

            If Retrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In Retrieved.Tables(0).Rows
                    row = FillRowWithNull(row, Retrieved.Tables(0).Columns)
                    RequestDetailsItem = New RequestDetails

                    RequestDetailsItem.StoreID = row("RequestStoreID")
                    RequestDetailsItem.ConsumerID = row("RequestConsumerID")
                    RequestDetailsItem.RequestID = row("RequestID")
                    RequestDetailsItem.Type = row("RequestType")
                    RequestDetailsItem.SerialNo = row("RequestSerialNo")
                    RequestDetailsItem.Sought = row("RequestSought")
                    RequestDetailsItem.RequestDte = row("RequestCreateDte")
                    RequestDetailsItem.RequestBy = row("RequestCreateUserID")
                    RequestDetailsItem.ApproveDte = row("RequestApproveDte")
                    RequestDetailsItem.ApproveBy = row("RequestApproverUserID")
                    RequestDetailsItem.IssueDte = row("RequestIssueDte")
                    RequestDetailsItem.IssueBy = row("RequestIssuerUserID")
                    RequestDetailsItem.Status = row("RequestStatus")

                    List.Add(RequestDetailsItem)
                Next
            End If
            Return List

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw

            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Get Request by search criteria list;
    ''' 26Feb09 - KG;
    ''' </summary>
    ''' <param name="requestDetails"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Function GetRequestBySearch(ByVal requestDetails As RequestDetails _
                                ) As List(Of RequestDetails) Implements IService.GetRequestBySearch
        Try
            Dim List As New List(Of RequestDetails)
            Dim Retrieved As New DataSet
            Dim RequestDetailsItem As New RequestDetails

            List.Clear()

            Retrieved = StockControlBL.GetRequest(requestDetails.StoreID _
                                                  , requestDetails.ConsumerID _
                                                  , requestDetails.RequestID _
                                                  , requestDetails.Status _
                                                  )

            If Retrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In Retrieved.Tables(0).Rows
                    row = FillRowWithNull(row, Retrieved.Tables(0).Columns)
                    RequestDetailsItem = New RequestDetails

                    RequestDetailsItem.StoreID = row("RequestStoreID")
                    RequestDetailsItem.ConsumerID = row("RequestConsumerID")
                    RequestDetailsItem.RequestID = row("RequestID")
                    RequestDetailsItem.Type = row("RequestType")
                    RequestDetailsItem.SerialNo = row("RequestSerialNo")
                    RequestDetailsItem.Sought = row("RequestSought")
                    RequestDetailsItem.RequestDte = row("RequestCreateDte")
                    RequestDetailsItem.RequestBy = row("RequestCreateUserID")
                    RequestDetailsItem.ApproveDte = row("RequestApproveDte")
                    RequestDetailsItem.ApproveBy = row("RequestApproverUserID")
                    RequestDetailsItem.IssueDte = row("RequestIssueDte")
                    RequestDetailsItem.IssueBy = row("RequestIssuerUserID")
                    RequestDetailsItem.Status = row("RequestStatus")

                    List.Add(RequestDetailsItem)
                Next
            End If
            Return List

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw

            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Get Request Item
    ''' 05Feb09 - KG;
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="requestId"></param>
    ''' <returns>Request Items DataSet Collection</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Function GetRequestItem(ByVal storeId As String _
                          , ByVal requestId As String _
                          ) As List(Of IssueItemDetails) Implements IService.GetRequestItem
        Try
            Dim Retrieved As New DataSet
            Dim List As New List(Of IssueItemDetails)
            Dim ItemDetails As IssueItemDetails

            List.Clear()

            Retrieved = StockControlBL.GetRequestItem(storeId _
                                                      , requestId _
                                                      )

            If Retrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In Retrieved.Tables(0).Rows
                    row = FillRowWithNull(row, Retrieved.Tables(0).Columns)
                    ItemDetails = New IssueItemDetails

                    ItemDetails.TranID = IIf(Not row.IsNull("StockTransactionID"), row("StockTransactionID"), 0)
                    ItemDetails.StockItemID = row("RequestItemStockItemID")
                    ItemDetails.Qty = IIf(Not row.IsNull("StockTransactionQty"), row("StockTransactionQty"), 0)
                    ItemDetails.TotalCost = IIf(Not row.IsNull("StockTransactionTotalCost"), row("StockTransactionTotalCost"), 0)
                    ItemDetails.Remarks = row("StockTransactionRemarks")
                    ItemDetails.RequestItemID = row("RequestItemID")
                    ItemDetails.Status = row("StockTransactionStatus")
                    ItemDetails.RequestItemQty = row("RequestItemQty")
                    ItemDetails.RequestItemStatus = row("RequestItemStatus")
                    ItemDetails.BalanceQty = IIf(Not row.IsNull("BalanceQty"), row("BalanceQty"), 0)

                    List.Add(ItemDetails)
                Next
            End If
            Return List

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw

            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Update only to RequestItem Details, based on the requestitem's mode to either Delete, Insert or Update;
    ''' 20Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="requestID"></param>
    ''' <param name="loginUser"></param>
    ''' <param name="requestItemDetails"></param>
    ''' <returns>Error Message (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Function UpdateRequest(ByVal storeID As String _
                           , ByVal requestID As String _
                           , ByVal loginUser As String _
                           , ByVal requestItemDetails As List(Of IssueItemDetails) _
                           ) As String Implements IService.UpdateRequest
        Try
            Dim List As New List(Of RequestItem)
            Dim ItemDetails As RequestItem

            'convert wcf list to BL list
            For Each item In requestItemDetails
                ItemDetails = New RequestItem(storeID _
                                              , requestID _
                                              , item.RequestItemID _
                                              , item.StockItemID _
                                              , item.RequestItemQty _
                                              , item.RequestItemStatus _
                                              , loginUser _
                                              , item.Mode _
                                              )

                List.Add(ItemDetails)
            Next

            Return StockControlBL.UpdateRequest(storeID _
                                                , requestID _
                                                , loginUser _
                                                , List _
                                                )
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw

            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Delete Request and Request Items details;
    ''' 05Feb09 - KG;
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="requestId"></param>
    ''' <param name="status"></param>
    ''' <param name="loginUser"></param>
    ''' <returns>error message (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Function DeleteRequest(ByVal storeId As String _
                           , ByVal requestId As String _
                           , ByVal status As String _
                           , ByVal loginUser As String _
                           ) As String Implements IService.DeleteRequest
        Try
            Return StockControlBL.DeleteRequest(storeId _
                                                , requestId _
                                                , status _
                                                , loginUser _
                                                )
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw

            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Update Request Status to either Approved or Rejected;
    ''' 23Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="requestID"></param>
    ''' <param name="status">Approved, Rejected</param>
    ''' <param name="loginUser"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Function UpdateRequestStatus(ByVal storeID As String _
                                          , ByVal requestID As String _
                                          , ByVal status As String _
                                          , ByVal loginUser As String _
                                          ) As String Implements IService.UpdateRequestStatus
        Try
            ' Update Request Status
            Return StockControlBL.UpdateRequestStatus(storeID _
                                                      , requestID _
                                                      , status _
                                                      , loginUser _
                                                      )

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw

            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Update Adjust Status to either Approved or Rejected;
    ''' 01Mar12 - Christina;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="adjustID"></param>
    ''' <param name="status">Approved, Rejected</param>
    ''' <param name="loginUser"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Function UpdateAdjustStatus(ByVal storeID As String _
                                          , ByVal adjustID As String _
                                          , ByVal adjustType As String _
                                          , ByVal status As String _
                                          , ByVal loginUser As String _
                                          , Optional ByVal returnBy As String = "", Optional ByVal returnDte As Date = #12:00:00 AM# _
                                          , Optional ByVal approveBy As String = "", Optional ByVal approveDte As Date = #12:00:00 AM# _
                                          , Optional ByVal receiveBy As String = "", Optional ByVal receiveDte As Date = #12:00:00 AM# _
                                          ) As String Implements IService.UpdateAdjustStatus

        Try
            ' Update Request Status
            Return StockControlBL.UpdateAdjustStatus(storeID _
                                                      , adjustID _
                                                      , adjustType _
                                                      , status _
                                                      , loginUser _
                                                      , returnBy _
                                                       , returnDte _
                                                       , approveBy _
                                                       , approveDte _
                                                       , receiveBy _
                                                       , receiveDte _
                                                      )

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw

            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Update the Issue Items base on the Mode either Insert, Update or Delete transaction;
    ''' 14Feb09 - KG;
    ''' </summary>
    ''' <param name="requestDetails"></param>
    ''' <param name="issueItemList"></param>
    ''' <returns>error message (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Function UpdateIssueItem(ByVal requestDetails As RequestDetails _
                             , ByVal issueItemList As List(Of IssueItemDetails) _
                             ) As String Implements IService.UpdateIssueItem
        Try
            Dim List As New List(Of IssueItem)
            Dim ItemDetails As IssueItem

            'convert wcf list to BL list
            For Each item In issueItemList
                ItemDetails = New IssueItem(item.TranID _
                                            , item.StockItemID _
                                            , item.Qty _
                                            , item.Remarks _
                                            , item.RequestItemID _
                                            , item.Status _
                                            , item.Mode _
                                            )

                List.Add(ItemDetails)
            Next

            Return StockControlBL.UpdateIssueItem(requestDetails.StoreID _
                                                  , requestDetails.Type _
                                                  , requestDetails.RequestID _
                                                  , requestDetails.IssueDte _
                                                  , requestDetails.SerialNo _
                                                  , requestDetails.Status _
                                                  , requestDetails.LoginUser _
                                                  , List _
                                                  )
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw

            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Get all Request list by parameters;
    ''' when requestID is with value, ignore other parameters and get only request items relates to the single Request Reference
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="fromDte"></param>
    ''' <param name="toDte"></param>
    ''' <param name="fromStockItemID"></param>
    ''' <param name="toStockItemID"></param>
    ''' <param name="requestID"></param>
    ''' <returns>Requests DataSet</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Function GetIssueList(ByVal storeID As String _
                          , Optional ByVal fromDte As Date = #12:00:00 AM# _
                          , Optional ByVal toDte As Date = #12:00:00 AM# _
                          , Optional ByVal fromStockItemID As String = "" _
                          , Optional ByVal toStockItemID As String = "" _
                          , Optional ByVal requestId As String = "" _
                          , Optional ByVal consumerID As String = "" _
                          ) As List(Of IssueList) Implements IService.GetIssueList
        Try
            Dim List As New List(Of IssueList)
            Dim ItemDetails As IssueList
            Dim Retrieved As New DataSet

            List.Clear()

            Retrieved = StockControlBL.GetIssueList(storeID _
                                                    , fromDte _
                                                    , toDte _
                                                    , fromStockItemID _
                                                    , toStockItemID _
                                                    , requestId _
                                                    , consumerID _
                                                    )

            If Retrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In Retrieved.Tables(0).Rows
                    row = FillRowWithNull(row, Retrieved.Tables(0).Columns)
                    ItemDetails = New IssueList

                    ItemDetails.DocNo = row("FIssueByStockRangeRequestID")
                    ItemDetails.SerialNo = row("FIssueByStockRangeSerialNo")
                    ItemDetails.ConsumerID = row("FIssueByStockRangeConsumerID")
                    ItemDetails.RequestBy = row("FIssueByStockRangeUserID")
                    ItemDetails.IssueDte = row("FIssueByStockRangeDte")
                    ItemDetails.Status = row("FIssueByStockRangeRequestStatus")
                    ItemDetails.IssueItemID = row("FIssueByStockRangeID")
                    ItemDetails.StockItemID = row("FIssueByStockRangeStockItemID")
                    ItemDetails.Description = row("FIssueByStockRangeStockItemDesc")
                    ItemDetails.UOM = row("FIssueByStockRangeUOM")
                    ItemDetails.IssueQty = row("FIssueByStockRangeQty")
                    ItemDetails.TotalCost = row("FIssueByStockRangeTotalCost")
                    If Not row.IsNull("FIssueByStockRangeRemarks") Then
                        ItemDetails.Remarks = row("FIssueByStockRangeRemarks")
                    End If
                    ItemDetails.ItemStatus = row("FIssueByStockRangeStatus")

                    List.Add(ItemDetails)
                Next
            End If
            Return List

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw

            Return Nothing
        End Try
    End Function

#End Region

#Region " Adjustment "
    ''' <summary>
    ''' Add New Adjust and its Item Details also to stock transaction;
    ''' 20Feb09 - KG;
    ''' </summary>
    ''' <param name="adjustDetails"></param>
    ''' <param name="adjustItemDetails"></param>
    ''' <returns>Error Message (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Function AddAdjust(ByVal adjustDetails As AdjustDetails _
                       , ByVal adjustItemDetails As List(Of AdjustItemDetails)
                       ) As String Implements IService.AddAdjust
        Try
            Dim List As New List(Of AdjustItem)
            Dim AdjustItem As AdjustItem

            'convert wcf list to BL list
            For Each Item In adjustItemDetails
                AdjustItem = New AdjustItem(Item.TranID _
                                            , Item.StockItemID _
                                            , Item.Qty _
                                            , Item.TotalCost _
                                            , Item.Remarks _
                                            , Item.AdjustItemID _
                                            , Item.ItemReturn _
                                            , Item.Status _
                                            , Item.Mode
                                            )
                List.Add(AdjustItem)
            Next

            Return StockControlBL.AddAdjust(adjustDetails.StoreID _
                                            , adjustDetails.AdjustID _
                                            , adjustDetails.Type _
                                            , adjustDetails.Dte _
                                            , adjustDetails.SerialNo _
                                            , adjustDetails.InvolveID _
                                            , adjustDetails.DocReturn _
                                            , adjustDetails.LoginUser _
                                            , adjustDetails.ReturnUserID _
                                            , adjustDetails.ReturnDte _
                                            , List
                                            )
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw

            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Get Adjust based on type(AI or AO) n status, all adjust is created as "C" only;
    ''' 20Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="type">AI = Inwards, AO = Outwards</param>
    ''' <param name="status"></param>
    ''' <returns>Adjusts DataSet Collection</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Function GetAdjust(ByVal storeID As String _
                       , ByVal type As String _
                       , Optional ByVal status As String = "C" _
                       ) As List(Of AdjustDetails) Implements IService.GetAdjust
        Try
            Dim List As New List(Of AdjustDetails)
            Dim Retrieved As New DataSet
            Dim AdjustDetailsItem As New AdjustDetails

            List.Clear()

            Retrieved = StockControlBL.GetAdjust(storeID _
                                                 , type _
                                                 , status _
                                                 )

            If Retrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In Retrieved.Tables(0).Rows
                    row = FillRowWithNull(row, Retrieved.Tables(0).Columns)
                    AdjustDetailsItem = New AdjustDetails

                    AdjustDetailsItem.StoreID = row("AdjustStoreID")
                    AdjustDetailsItem.AdjustID = row("AdjustID")
                    AdjustDetailsItem.Type = row("AdjustType")
                    AdjustDetailsItem.Dte = row("AdjustDte")
                    AdjustDetailsItem.SerialNo = row("AdjustSerialNo")
                    AdjustDetailsItem.InvolveID = row("AdjustInvolveID")
                    AdjustDetailsItem.DocReturn = row("AdjustDocReturn")
                    AdjustDetailsItem.Status = row("AdjustStatus")
                    AdjustDetailsItem.LoginUser = row("AdjustCreateUserID")
                    AdjustDetailsItem.ReturnUserID = row("AdjustReturnUserID")
                    AdjustDetailsItem.ReturnDte = row("AdjustReturnDte")
                    AdjustDetailsItem.ApproveUserID = row("AdjustApproveUserID")
                    AdjustDetailsItem.ApproveDte = row("AdjustApproveDte")
                    AdjustDetailsItem.ReceiveUserID = row("AdjustReceiveUserID")
                    AdjustDetailsItem.ReceiveDte = row("AdjustReceiveDte")
                    AdjustDetailsItem.SerialNo = row("AdjustSerialNo")

                    List.Add(AdjustDetailsItem)
                Next
            End If
            Return List

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw

            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Get all Adjust items for a single Adjust Reference;
    ''' 20Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="adjustID"></param>
    ''' <param name="type">AI = Inwards, AO = Outwards</param>
    ''' <returns>Adjust Items DataSet Collection</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Function GetAdjustItem(ByVal storeID As String _
                           , ByVal adjustID As String _
                           , ByVal type As String _
                           ) As List(Of AdjustItemDetails) Implements IService.GetAdjustItem

        Try
            Dim Retrieved As New DataSet
            Dim List As New List(Of AdjustItemDetails)
            Dim ItemDetails As AdjustItemDetails

            List.Clear()

            Retrieved = StockControlBL.GetAdjustItem(storeID _
                                                     , adjustID _
                                                     , type _
                                                     )

            If Retrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In Retrieved.Tables(0).Rows
                    row = FillRowWithNull(row, Retrieved.Tables(0).Columns)
                    ItemDetails = New AdjustItemDetails

                    ItemDetails.TranID = IIf(Not row.IsNull("StockTransactionID"), row("StockTransactionID"), 0)
                    ItemDetails.StockItemID = row("AdjustItemStockItemID")

                    If row("StockTransactionQty") <> Nothing Then
                        ItemDetails.Qty = row("StockTransactionQty")
                    Else
                        ItemDetails.Qty = row("AdjustItemQty")
                    End If

                    ItemDetails.TotalCost = row("StockTransactionTotalCost")

                    If row("StockTransactionRemarks") <> Nothing Then
                        ItemDetails.Remarks = row("StockTransactionRemarks")
                    Else
                        ItemDetails.Remarks = row("AdjustItemRemarks")
                    End If

                    ItemDetails.AdjustItemID = IIf(Not row.IsNull("AdjustItemID"), row("AdjustItemID"), 0)
                    ItemDetails.ItemReturn = row("StockTransactionItemReturn")
                    ItemDetails.Status = row("StockTransactionStatus")
                    ItemDetails.BalanceQty = IIf(Not row.IsNull("BalanceQty"), row("BalanceQty"), 0)
                    ItemDetails.MaxLevel = IIf(Not row.IsNull("MaxLevel"), row("MaxLevel"), 0)

                    List.Add(ItemDetails)
                Next
            End If
            Return List

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw

            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Get Adjust by search criteria list;
    ''' 09 Mar 12 - Christina;
    ''' </summary>
    ''' <param name="adjustDetails"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Function GetAdjustBySearch(ByVal adjustDetails As AdjustDetails _
                                ) As List(Of AdjustDetails) Implements IService.GetAdjustBySearch
        Try
            Dim List As New List(Of AdjustDetails)
            Dim Retrieved As New DataSet
            Dim AdjustDetailsItem As New AdjustDetails

            List.Clear()

            Retrieved = StockControlBL.GetAdjustBySearch(adjustDetails.StoreID _
                                                  , adjustDetails.InvolveID _
                                                  , adjustDetails.AdjustID _
                                                  , adjustDetails.Type _
                                                  , adjustDetails.Status _
                                                  )

            If Retrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In Retrieved.Tables(0).Rows
                    row = FillRowWithNull(row, Retrieved.Tables(0).Columns)
                    AdjustDetailsItem = New AdjustDetails

                    AdjustDetailsItem.StoreID = row("AdjustStoreID")
                    AdjustDetailsItem.AdjustID = row("AdjustID")
                    AdjustDetailsItem.Type = row("AdjustType")
                    AdjustDetailsItem.Dte = row("AdjustDte")
                    AdjustDetailsItem.SerialNo = row("AdjustSerialNo")
                    AdjustDetailsItem.InvolveID = row("AdjustInvolveID")
                    AdjustDetailsItem.DocReturn = row("AdjustDocReturn")
                    AdjustDetailsItem.Status = row("AdjustStatus")
                    AdjustDetailsItem.LoginUser = row("AdjustCreateUserID")
                    AdjustDetailsItem.ReturnUserID = row("AdjustReturnUserID")
                    AdjustDetailsItem.ReturnDte = row("AdjustReturnDte")
                    AdjustDetailsItem.ApproveUserID = row("AdjustApproveUserID")
                    AdjustDetailsItem.ApproveDte = row("AdjustApproveDte")
                    AdjustDetailsItem.ReceiveUserID = row("AdjustReceiveUserID")
                    AdjustDetailsItem.ReceiveDte = row("AdjustReceiveDte")
                    AdjustDetailsItem.SerialNo = row("AdjustSerialNo")

                    List.Add(AdjustDetailsItem)
                Next
            End If
            Return List

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw

            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Update to Adjust n AdjustItem Details, based on the adjustitem's mode to either Delete, Insert or Update;
    ''' 20Feb09 - KG;
    ''' </summary>
    ''' <param name="adjustDetails"></param>
    ''' <param name="adjustItemDetails"></param>
    ''' <returns>Error Message (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Function UpdateAdjust(ByVal adjustDetails As AdjustDetails _
                          , ByVal adjustItemDetails As List(Of AdjustItemDetails) _
                          ) As String Implements IService.UpdateAdjust
        Try
            Dim List As New List(Of AdjustItem)
            Dim ItemDetails As AdjustItem

            'convert wcf list to BL list
            For Each item In adjustItemDetails
                ItemDetails = New AdjustItem(item.TranID _
                                             , item.StockItemID _
                                             , item.Qty _
                                             , item.TotalCost _
                                             , item.Remarks _
                                             , item.AdjustItemID _
                                             , item.ItemReturn _
                                             , item.Status _
                                             , item.Mode _
                                             )
                List.Add(ItemDetails)
            Next

            Return StockControlBL.UpdateAdjust(adjustDetails.StoreID _
                                               , adjustDetails.AdjustID _
                                               , adjustDetails.Type _
                                               , adjustDetails.Dte _
                                               , adjustDetails.SerialNo _
                                               , adjustDetails.InvolveID _
                                               , adjustDetails.LoginUser _
                                               , adjustDetails.ReturnUserID _
                                               , adjustDetails.ReturnDte _
                                               , adjustDetails.ApproveUserID _
                                               , adjustDetails.ApproveDte _
                                               , adjustDetails.ReceiveUserID _
                                               , adjustDetails.ReceiveDte _
                                               , List _
                                               )
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw

            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Delete Adjust and all its Adjust Items n Transaction details;
    ''' 20Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="adjustType">Inward or Outward</param>
    ''' <param name="adjustID"></param>
    ''' <param name="loginUser"></param>
    ''' <returns>error message (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Function DeleteAdjust(ByVal storeID As String _
                          , ByVal adjustID As String _
                          , ByVal adjustType As String _
                          , ByVal originalDte As Date _
                          , ByVal loginUser As String _
                          ) As String Implements IService.DeleteAdjust
        Try
            Return StockControlBL.DeleteAdjust(storeID _
                                               , adjustID _
                                               , adjustType _
                                               , originalDte _
                                               , loginUser _
                                               )
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw

            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Get the Adjust list base on parameters;
    ''' when adjustID is with value, ignore other parameters and get only adjust items relates to the single Adjust Reference
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="fromDte"></param>
    ''' <param name="toDte"></param>
    ''' <param name="adjustID">overwrite other parameter and return value for this adjust only</param>
    ''' <returns>Adjust DataSet</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Function GetAdjustList(ByVal storeID As String _
                           , ByVal type As String _
                           , Optional ByVal fromDte As Date = #12:00:00 AM# _
                           , Optional ByVal toDte As Date = #12:00:00 AM# _
                           , Optional ByVal adjustID As String = "" _
                           ) As List(Of AdjustList) Implements IService.GetAdjustList
        Try
            Dim List As New List(Of AdjustList)
            Dim ItemDetails As AdjustList
            Dim Retrieved As New DataSet

            List.Clear()

            Retrieved = StockControlBL.GetAdjustList(storeID _
                                                     , type _
                                                     , fromDte _
                                                     , toDte _
                                                     , adjustID _
                                                     )

            If Retrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In Retrieved.Tables(0).Rows
                    row = FillRowWithNull(row, Retrieved.Tables(0).Columns)
                    ItemDetails = New AdjustList

                    ItemDetails.StockType = row("FAdjustByStockRangeStockType")
                    ItemDetails.StockItemID = row("FAdjustByStockRangeStockItemID")
                    ItemDetails.Description = row("FAdjustByStockRangeStockItemDesc")
                    ItemDetails.UOM = row("FAdjustByStockRangeUOM")
                    ItemDetails.DocNo = row("FAdjustByStockRangeAdjustID")
                    ItemDetails.Type = row("FAdjustByStockRangeType")
                    ItemDetails.AdjustDte = row("FAdjustByStockRangeDte")
                    ItemDetails.Qty = row("FAdjustByStockRangeQty")
                    ItemDetails.TotalCost = row("FAdjustByStockRangeTotalCost")
                    ItemDetails.Remarks = row("FAdjustByStockRangeRemarks")
                    ItemDetails.DocReturn = row("FAdjustByStockRangeDocReturn")
                    ItemDetails.UserName = row("FAdjustByStockRangeUserID")

                    List.Add(ItemDetails)
                Next
            End If
            Return List

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw

            Return Nothing
        End Try
    End Function

#End Region

End Class
