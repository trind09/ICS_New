Imports NEA_ICS.DataAccess
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Transactions

''' <summary>
''' Business Layer - for Stock Control;
''' 5 Feb 2009 - KG;
''' </summary>
''' <remarks>
''' CHANGE LOG:
''' ddMMMyy  AuthorName  RefID  Description;
''' </remarks>
Public Class StockControlBL

#Region " Constant Value "
    ' Stock Control Type
    Private Shared ReadOnly ORDER As String = "O"
    Private Shared ReadOnly RECEIVE As String = "O"
    Private Shared ReadOnly ISSUE As String = "I"
    Private Shared ReadOnly DIRECTISSUE As String = "D"
    Private Shared ReadOnly ADJUSTMENT As String = "A"

#End Region

#Region " Common Function "
    Public Enum TableName As Integer
        Issue = 203
        DirectIssue = 204
        Adjust = 300
    End Enum

    Public Enum ColumnName As Integer
        OrderID = 2012
        GebizPONo = 2013
        IssueSerialNo = 2035
        DirectIssueSerialNo = 2046
        AdjustSerialNo = 3004
        AdjustIn = 3012
        AdjustOut = 3022
    End Enum

    Public Enum StockControlType As Integer
        ReceiveOrder = 201
        IssueRequest = 203
        DirectIssue = 204
        AdjustInward = 301
        AdjustOutward = 302
    End Enum

    ''' <summary>
    ''' Check field is Unique, PK is ONLY required when the check is on existing record and column is not the PK;
    ''' 05Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="columnName">column to check</param>
    ''' <param name="columnValue">value to check against the column</param>
    ''' <param name="pkColumnValue">optional</param>
    ''' <returns>True = Unique</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function FieldIsUnique(ByVal storeID As String _
                                         , ByVal columnName As ColumnName _
                                         , ByVal columnValue As String _
                                         , Optional ByVal pkColumnValue As String = "" _
                                         ) As Boolean
        Dim Unique As Boolean = False

        Try
            Select Case columnName
                Case columnName.OrderID
                    Unique = StockControlDAL.FieldIsUnique(storeID, StockControlDAL.TableName.Order, StockControlDAL.ColumnName.OrderId, columnValue)
                Case columnName.GebizPONo
                    Unique = StockControlDAL.FieldIsUnique(storeID, StockControlDAL.TableName.Order, StockControlDAL.ColumnName.OrderGebizPONo, columnValue, StockControlDAL.ColumnName.OrderId, pkColumnValue)
                Case columnName.DirectIssueSerialNo
                    Unique = StockControlDAL.FieldIsUnique(storeID, StockControlDAL.TableName.DirectIssue, StockControlDAL.ColumnName.DirectIssueSerialNo, columnValue, StockControlDAL.ColumnName.DirectIssueDocNo, pkColumnValue)
                Case columnName.IssueSerialNo
                    Unique = StockControlDAL.FieldIsUnique(storeID, StockControlDAL.TableName.Request, StockControlDAL.ColumnName.RequestSerialNo, columnValue, StockControlDAL.ColumnName.RequestID, pkColumnValue)
                Case columnName.AdjustSerialNo
                    Unique = StockControlDAL.FieldIsUnique(storeID, StockControlDAL.TableName.Adjust, StockControlDAL.ColumnName.AdjustSerialNo, columnValue, StockControlDAL.ColumnName.AdjustID, pkColumnValue)
                Case StockControlBL.ColumnName.AdjustIn
                    Unique = StockControlDAL.FieldIsUnique(storeID, StockControlDAL.TableName.Adjust, StockControlDAL.ColumnName.AdjustID, columnValue)
                Case StockControlBL.ColumnName.AdjustOut
                    Unique = StockControlDAL.FieldIsUnique(storeID, StockControlDAL.TableName.Adjust, StockControlDAL.ColumnName.AdjustID, columnValue)
            End Select

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then Throw
        End Try

        Return Unique
    End Function

    ''' <summary>
    ''' Get More Stock Item Info for a single Stock item based on specific date;
    ''' 08Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="stockItem"></param>
    ''' <param name="asOfDte"></param>
    ''' <returns>More Stock Items Info DataSet Collection</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetMoreItemInfo(ByVal storeID As String _
                                           , ByVal stockItem As String _
                                           , ByVal asOfDte As Date _
                                           ) As DataSet
        Try
            Return StockControlDAL.GetMoreItemInfo(storeID _
                                                   , stockItem _
                                                   , asOfDte _
                                                   )

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then Throw

            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Get the Last Serial No base on the provide info;
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="tableName"></param>
    ''' <param name="docType"></param>
    ''' <returns>last serial no</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetLastSerialNo(ByVal storeID As String _
                                           , ByVal tableName As TableName _
                                           , ByVal docType As String _
                                           ) As String
        Try
            Select Case tableName
                Case tableName.Issue
                    Return StockControlDAL.GetLastSerialNo(storeID, StockControlDAL.TableName.Request)
                Case tableName.DirectIssue
                    Return StockControlDAL.GetLastSerialNo(storeID, StockControlDAL.TableName.DirectIssue)
                Case tableName.Adjust
                    Return StockControlDAL.GetLastSerialNo(storeID, StockControlDAL.TableName.Adjust, docType)
                Case Else
                    Return String.Empty
            End Select
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
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
    Public Shared Function WithinFinancialCutoff(ByVal storeID As String _
                                                 , ByVal tranDte As Date _
                                                 ) As Boolean
        Try
            Return StockControlDAL.WithinFinancialCutoff(storeID, tranDte)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then Throw
        End Try
    End Function

    ''' <summary>
    ''' Generate Unique Doc No based on {4char Storeid}{2digit year}{1char type}{5digit incremental number};
    ''' 20Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="tableName"></param>
    ''' <returns>generated Doc No</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GenerateDocNo(ByVal storeID As String _
                                         , ByVal tableName As TableName _
                                         ) As String
        Try
            Select Case tableName
                Case tableName.Issue
                    Return StockControlDAL.GenerateDocNo(storeID, StockControlDAL.TableName.Request)
                Case tableName.DirectIssue
                    Return StockControlDAL.GenerateDocNo(storeID, StockControlDAL.TableName.DirectIssue)
                Case tableName.Adjust
                    Return StockControlDAL.GenerateDocNo(storeID, StockControlDAL.TableName.Adjust)
                Case Else
                    Return String.Empty
            End Select

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then Throw

            Return String.Empty
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
    Public Shared Sub DeleteAllStockTransaction(ByVal storeID As String _
                                                , ByVal docNo As String _
                                                , ByVal stockControlType As StockControlType _
                                                , ByVal originalDte As Date _
                                                , ByVal loginUser As String _
                                                )
        Try
            ' check for order details mandatory fields
            If storeID = String.Empty _
                Or docNo = String.Empty _
                Or originalDte = Date.MinValue _
                Or loginUser = String.Empty _
            Then
                Throw New Exception(String.Format("Error:[{0}][{1}][{2}][{3}] Record not Deleted, missing receive info", storeID, loginUser, docNo, stockControlType.ToString, CStr(originalDte)))
            End If


            ' check date is allow
            If Not StockControlDAL.WithinFinancialCutoff(storeID _
                                                         , originalDte _
                                                         ) Then
                Throw New ApplicationException(String.Format("[{0}] is not within financial cut off date. Record not deleted", docNo), New Exception(String.Format("Error:[{0}][{1}][{2}][{3}][{4}] Record not deleted", storeID, loginUser, docNo, stockControlType.ToString, CStr(originalDte))))
            End If

            Using scope As New TransactionScope(TransactionScopeOption.RequiresNew)
                Select Case stockControlType
                    Case stockControlType.ReceiveOrder
                        Dim Retrieved As New DataSet

                        ' retrieve the orderitemID first before its deleted
                        Retrieved = StockControlBL.GetOrderItem(storeID _
                                                                , docNo _
                                                                , False _
                                                                )

                        StockControlDAL.DeleteReceive(storeID _
                                                      , docNo _
                                                      , originalDte _
                                                      , loginUser _
                                                      )

                        ' update the Order Status as accordingly
                        If Retrieved.Tables(0).Rows.Count > 0 Then
                            For Each row As DataRow In Retrieved.Tables(0).Rows
                                StockControlDAL.UpdateOrderItemStatus(storeID _
                                                                      , row("OrderItemID") _
                                                                      , IIf(Not row.IsNull("OrderItemWarrantyDte"), row("OrderItemWarrantyDte"), Date.MinValue) _
                                                                      )
                            Next
                        End If

                    Case stockControlType.IssueRequest
                        StockControlDAL.DeleteIssue(storeID _
                                                    , docNo _
                                                    , loginUser _
                                                    )

                        ' revert the Request Status back to Approve
                        StockControlDAL.UpdateRequestStatus(storeID _
                                                            , docNo _
                                                            , "A" _
                                                            , loginUser _
                                                            )

                    Case stockControlType.AdjustInward
                        StockControlDAL.DeleteAdjust(storeID _
                                                     , "AI" _
                                                     , docNo _
                                                     , loginUser _
                                                     )

                    Case stockControlType.AdjustOutward
                        StockControlDAL.DeleteAdjust(storeID _
                                                     , "AO" _
                                                     , docNo _
                                                     , loginUser _
                                                     )

                End Select

                scope.Complete()
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then Throw
        End Try
    End Sub
#End Region

#Region " Order "
    ''' <summary>
    ''' Add New Order and its Item Details;
    ''' 1) Check mandatory fields;
    ''' 2) Check OrderID and GeBizPONo(if any) is unique;
    ''' 3) Insert records using Transaction Scope;
    ''' 05Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="orderID"></param>
    ''' <param name="gebizPONo"></param>
    ''' <param name="type"></param>
    ''' <param name="dte"></param>
    ''' <param name="supplierID"></param>
    ''' <param name="loginUser"></param>
    ''' <param name="orderItemList"></param>
    ''' <returns>Error Message (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function AddOrder(ByVal storeID As String _
                                    , ByVal orderID As String _
                                    , ByVal gebizPONo As String _
                                    , ByVal type As String _
                                    , ByVal dte As Date _
                                    , ByVal supplierID As String _
                                    , ByVal loginUser As String _
                                    , ByVal orderItemList As List(Of OrderItem) _
                                    ) As String
        Try
            Dim OrderItemID As Integer = 0

            ' check for order details required fields
            If (storeID = String.Empty _
                Or loginUser = String.Empty _
                ) Then
                Throw New Exception(String.Format("Error:[{0}][{1}] Record not Inserted", storeID, loginUser))
            End If


            ' check for order details mandatory fields
            If (orderID = String.Empty _
                Or type = String.Empty _
                Or dte = DateTime.MinValue _
                Or supplierID = String.Empty _
                ) Then
                Throw New ApplicationException("Missing Order Mandatory Fields", New Exception(String.Format("Error:[{0}][{1}][{2}][{3}][{4}] Record not Inserted", storeID, loginUser, orderID, dte, supplierID)))
            End If


            ' check for order item details mandatory fields
            If orderItemList.Count > 0 Then
                ' search orderItemList for mandatory fields in OrderItem, retrieve blank list
                Dim EmptyStockItemID As List(Of OrderItem) = orderItemList.FindAll(Function(item As OrderItem) item.StockItemID = Nothing Or item.StockItemID = String.Empty)
                Dim EmptyQty As List(Of OrderItem) = orderItemList.FindAll(Function(item As OrderItem) item.Qty = Nothing Or item.Qty = 0D)
                Dim EmptyTotalCost As List(Of OrderItem) = orderItemList.FindAll(Function(item As OrderItem) item.TotalCost = Nothing Or item.TotalCost = 0.0)
                Dim EmptyExpectedDeliveryDte As List(Of OrderItem) = orderItemList.FindAll(Function(item As OrderItem) item.ExpectedDeliveryDte = Nothing Or item.ExpectedDeliveryDte = DateTime.MinValue)
                Dim EmptyRemarks As List(Of OrderItem) = orderItemList.FindAll(Function(item As OrderItem) item.Remarks = Nothing Or item.Remarks = String.Empty)

                If (EmptyStockItemID.Count _
                    + EmptyQty.Count _
                    + EmptyTotalCost.Count _
                    + EmptyExpectedDeliveryDte.Count _
                    + EmptyRemarks.Count _
                    ) > 0 Then
                    Throw New ApplicationException("Missing Order Item Mandatory Fields", New Exception(String.Format("Error:[{0}][{1}][{2}] Record not Inserted", storeID, loginUser, orderID)))
                End If
            Else
                Throw New ApplicationException("Missing Order Item Mandatory Fields", New Exception(String.Format("Error:[{0}][{1}][{2}] Record not Inserted", storeID, loginUser, orderID)))
            End If


            ' check order id
            If Not StockControlBL.FieldIsUnique(storeID, ColumnName.OrderID, orderID) Then
                Throw New ApplicationException(String.Format("Order Reference[{0}] already exists.", orderID), New Exception(String.Format("Error:[{0}][{1}][{2}] Record not Inserted", storeID, loginUser, orderID)))
            End If


            ' Additional check if Gebiz PO No is with value
            If gebizPONo.Length > 0 Then
                If Not StockControlBL.FieldIsUnique(storeID, ColumnName.GebizPONo, gebizPONo, orderID) Then
                    Throw New ApplicationException(String.Format("Gebiz PO No[{0}] for [{1}] already exists.", gebizPONo, orderID), New Exception(String.Format("Error:[{0}][{1}][{2}] Record not Inserted", storeID, loginUser, orderID)))
                End If
            End If


            ' Insert order details
            Using scope As New TransactionScope(TransactionScopeOption.RequiresNew)
                StockControlDAL.InsertOrder(storeID _
                                            , orderID _
                                            , gebizPONo _
                                            , type _
                                            , dte _
                                            , supplierID _
                                            , loginUser _
                                            )

                ' Insert order item details
                For Each item In orderItemList
                    OrderItemID = 0
                    OrderItemID = StockControlDAL.InsertOrderItem(storeID _
                                                                  , orderID _
                                                                  , item.StockItemID _
                                                                  , item.Qty _
                                                                  , item.TotalCost _
                                                                  , item.ExpectedDeliveryDte _
                                                                  , item.WarrantyDte _
                                                                  , item.Remarks _
                                                                  , loginUser _
                                                                  )

                    If OrderItemID < 1 Then Throw New Exception(String.Format("Error:[{0}][{1}][{2}][{3}] Record not Inserted, orderitem identity not created", storeID, loginUser, orderID, item.StockItemID))
                Next

                scope.Complete()
            End Using

            Return String.Empty
        Catch ex As ApplicationException
            ExceptionPolicy.HandleException(ex, "Business Policy")
            Return ex.Message

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then Throw

            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Get Order based on status, default all Open Order only;
    ''' 05Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="status"></param>
    ''' <returns>Orders DataSet Collection</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetOrder(ByVal storeID As String _
                                    , Optional ByVal status As String = "O" _
                                    ) As DataSet
        Try
            Return StockControlDAL.GetOrder(storeID _
                                            , status _
                                            )
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then Throw

            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Get all Order Items for a single Order Reference;
    ''' 05Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="orderID"></param>
    ''' <param name="unfullfillOnly">status NOT equal to Closed</param>
    ''' <returns>Order Items DataSet Collection</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetOrderItem(ByVal storeID As String _
                                         , ByVal orderID As String _
                                         , Optional ByVal unfullfillOnly As Boolean = True _
                                         ) As DataSet
        Try
            Return StockControlDAL.GetOrderItem(storeID _
                                                , orderID _
                                                , unfullfillOnly _
                                                )
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then Throw
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Update Order or OrderItem or Both Details, based on the orderitem's mode to either Delete, Insert or Update;
    ''' 1) Check Order's info
    ''' 2) Check OrderItem's info
    ''' 3) Update records using Transaction Scope
    ''' 05Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="orderID"></param>
    ''' <param name="gebizPONo"></param>
    ''' <param name="type"></param>
    ''' <param name="dte"></param>
    ''' <param name="supplierID"></param>
    ''' <param name="loginUser"></param>
    ''' <param name="orderItemList"></param>
    ''' <returns>Error Message (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' 16Mar09  KG          UAT01  include StockItemID for DeleteItem;
    ''' </remarks>
    Public Shared Function UpdateOrder(ByVal storeID As String _
                                       , ByVal orderID As String _
                                       , ByVal gebizPONo As String _
                                       , ByVal type As String _
                                       , ByVal dte As Date _
                                       , ByVal supplierID As String _
                                       , ByVal loginUser As String _
                                       , ByVal mode As String _
                                       , ByVal orderItemList As List(Of OrderItem) _
                                       ) As String
        Try
            Dim OrderItemID As Integer = 0

            ' check for order details required fields
            If (storeID = String.Empty _
                Or orderID = String.Empty _
                Or loginUser = String.Empty _
                ) Then
                Throw New Exception(String.Format("Error:[{0}][{1}][{2}] Record not Updated", storeID, orderID, loginUser))
            End If


            ' Process Order's details Update when mode is "U" 
            If mode = "U" Then
                ' check for order details mandatory fields
                If (type = String.Empty _
                    Or dte = DateTime.MinValue _
                    Or supplierID = String.Empty _
                    ) Then
                    Throw New ApplicationException("Missing Order Mandatory Fields", New Exception(String.Format("Error:[{0}][{1}][{2}] Record not Updated", storeID, loginUser, orderID)))
                End If

                ' Additional check if Gebiz PO No is with value
                If gebizPONo.Length > 0 Then
                    If Not StockControlBL.FieldIsUnique(storeID, ColumnName.GebizPONo, gebizPONo, orderID) Then
                        Throw New ApplicationException(String.Format("Gebiz PO No[{0}] for [{1}] already exists.", gebizPONo, orderID), New Exception(String.Format("Error:[{0}][{1}][{2}] Record not Updated", storeID, loginUser, orderID)))
                    End If
                End If
            End If


            ' Skip Order item's details Update if list is empty
            If orderItemList.Count > 0 Then
                ' search orderItemList for mandatory fields in OrderItem, retrieve blank list
                Dim EmptyMode As List(Of OrderItem) = orderItemList.FindAll(Function(orderItem As OrderItem) orderItem.Mode = Nothing Or orderItem.Mode = String.Empty)

                If (EmptyMode.Count) > 0 Then
                    Throw New Exception(String.Format("Error:[{0}][{1}][{2}] Record not Updated, check 1", storeID, loginUser, orderID))
                End If
            End If


            ' Update order details
            Using scope As New TransactionScope(TransactionScopeOption.RequiresNew)
                If mode = "U" Then
                    StockControlDAL.UpdateOrder(storeID _
                                                , orderID _
                                                , gebizPONo _
                                                , type _
                                                , dte _
                                                , supplierID _
                                                , loginUser _
                                                )
                End If

                For Each item In orderItemList
                    Select Case item.Mode
                        Case "D"
                            If (item.OrderItemID = 0) Then
                                Throw New Exception(String.Format("Error:[{0}][{1}][{2}][{3}] Record not Updated, orderitem delete", storeID, loginUser, orderID, OrderItemID))
                            End If
                            StockControlDAL.DeleteOrderItem(storeID _
                                                            , orderID _
                                                            , item.OrderItemID _
                                                            , item.StockItemID _
                                                            , loginUser _
                                                            )

                        Case "I"
                            If (item.StockItemID = String.Empty _
                                Or item.Qty = 0D _
                                Or item.TotalCost = 0.0 _
                                Or item.ExpectedDeliveryDte = DateTime.MinValue _
                                Or item.Remarks = String.Empty _
                                ) Then
                                Throw New ApplicationException("Missing Order Item Mandatory Fields", New Exception(String.Format("Error:[{0}][{1}][{2}][{3}] Record not Updated, orderitem insert", storeID, loginUser, orderID, OrderItemID)))
                            End If

                            OrderItemID = StockControlDAL.InsertOrderItem(storeID _
                                                                          , orderID _
                                                                          , item.StockItemID _
                                                                          , item.Qty _
                                                                          , item.TotalCost _
                                                                          , item.ExpectedDeliveryDte _
                                                                          , item.WarrantyDte _
                                                                          , item.Remarks _
                                                                          , loginUser _
                                                                          )
                            If OrderItemID < 1 Then Throw New Exception(String.Format("Error:[{0}][{1}][{2}][{3}] Record not Updated, orderitem insert", storeID, loginUser, orderID, item.StockItemID))

                        Case "U"
                            If (item.StockItemID = String.Empty _
                                Or item.Qty = 0D _
                                Or item.TotalCost = 0.0 _
                                Or item.ExpectedDeliveryDte = DateTime.MinValue _
                                Or item.Remarks = String.Empty _
                                ) Then
                                Throw New ApplicationException("Missing Order Item Mandatory Fields", New Exception(String.Format("Error:[{0}][{1}][{2}][{3}] Record not Updated, orderitem insert", storeID, loginUser, orderID, OrderItemID)))
                            End If

                            If (item.OrderItemID = 0) Then
                                Throw New Exception(String.Format("Error:[{0}][{1}][{2}][{3}] Record not Updated, orderitem update", storeID, loginUser, orderID, OrderItemID))
                            End If
                            StockControlDAL.UpdateOrderItem(storeID _
                                                            , orderID _
                                                            , item.OrderItemID _
                                                            , item.StockItemID _
                                                            , item.Qty _
                                                            , item.TotalCost _
                                                            , item.ExpectedDeliveryDte _
                                                            , item.WarrantyDte _
                                                            , item.Remarks _
                                                            , loginUser _
                                                            )

                    End Select
                Next

                scope.Complete()
            End Using
            Return String.Empty

        Catch ex As ApplicationException
            ExceptionPolicy.HandleException(ex, "Business Policy")
            Return ex.Message

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then Throw

            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Delete Order and its Order Items details;
    ''' check order is not receive before allowing deletion;
    ''' 08Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="orderID"></param>
    ''' <param name="loginUser"></param>
    ''' <returns>error message (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function DeleteOrder(ByVal storeID As String _
                                       , ByVal orderID As String _
                                       , ByVal status As String _
                                       , ByVal loginUser As String _
                                       ) As String

        Try
            ' check for order details mandatory fields
            If storeID = String.Empty _
                Or orderID = String.Empty _
                Or loginUser = String.Empty _
            Then
                Throw New Exception(String.Format("Error:[{0}][{1}][{2}] Record not Deleted", storeID, loginUser, "order:" + orderID))
            End If


            ' check status is Open to allow for deletion
            If status = "O" Then
                StockControlDAL.DeleteOrder(storeID _
                                            , orderID _
                                            , loginUser _
                                            )
            Else
                Throw New ApplicationException(String.Format("Order[{0}] is already processed, Deletion not allow.", orderID), New Exception(String.Format("Error:[{0}][{1}][{2}] Record not Deleted", storeID, loginUser, orderID)))
            End If
            Return String.Empty

        Catch ex As ApplicationException
            ExceptionPolicy.HandleException(ex, "Business Policy")
            Return ex.Message

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
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
    Public Shared Function GetOrderList(ByVal storeID As String _
                                        , Optional ByVal status As String = "" _
                                        , Optional ByVal fromDte As Date = #12:00:00 AM# _
                                        , Optional ByVal toDte As Date = #12:00:00 AM# _
                                        , Optional ByVal fromStockItemID As String = "" _
                                        , Optional ByVal toStockItemID As String = "" _
                                        , Optional ByVal orderID As String = "" _
                                        ) As DataSet
        Try
            Return StockControlDAL.GetOrderList(storeID _
                                                , status _
                                                , fromDte _
                                                , toDte _
                                                , fromStockItemID _
                                                , toStockItemID _
                                                , orderID _
                                                )
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
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
    ''' <param name="storeID"></param>
    ''' <param name="consumerID"></param>
    ''' <param name="directIssueType"></param>
    ''' <param name="directIssueSerialNo"></param>
    ''' <param name="directIssueDte"></param>
    ''' <param name="directIssueStatus"></param>
    ''' <param name="directIssueItemList"></param>
    ''' <param name="loginUser"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' CHANGE REQ:
    ''' 1) To use 'FieldIsUnique' instead
    ''' </remarks>
    Public Shared Function AddDirectIssue(ByVal storeID As String, _
                                          ByVal consumerID As String, _
                                          ByVal directIssueType As String, _
                                          ByVal directIssueSerialNo As String, _
                                          ByVal directIssueDte As Date, _
                                          ByVal directIssueStatus As String, _
                                          ByVal directIssueItemList As List(Of DirectIssueItem), _
                                          ByVal loginUser As String, _
                                          ByRef directIssueDocNo As String) As String

        Dim errorMessage As String = String.Empty
        Dim unique As Boolean = False
        Dim DirectIssueID As Integer

        Try

            Using scope As New TransactionScope(TransactionScopeOption.RequiresNew)

                'found = StockControlDAL.CheckSerialNo(storeID, directIssueSerialNo)
                unique = StockControlDAL.FieldIsUnique(storeID, StockControlDAL.TableName.DirectIssue, _
                                                      StockControlDAL.ColumnName.DirectIssueSerialNo, _
                                                      directIssueSerialNo)

                If Not unique Then

                    errorMessage = "Serial No [" & directIssueSerialNo & "] already exists."

                Else
                    directIssueDocNo = StockControlDAL.GenerateDocNo(storeID, StockControlDAL.TableName.DirectIssue)

                    DirectIssueID = StockControlDAL.InsertDirectIssue(storeID, consumerID, directIssueDocNo, _
                                                                      directIssueType, directIssueSerialNo, _
                                                                      directIssueDte, directIssueStatus, _
                                                                      loginUser)

                    For Each item As DirectIssueItem In directIssueItemList

                        StockControlDAL.InsertDirectIssueItem(DirectIssueID, item.ItemID, item.ItemDescription, _
                                                              item.StockType, item.ItemQty, item.UOM, _
                                                              item.TotalCost, item.Remarks, item.ItemStatus, _
                                                              loginUser)
                    Next
                End If

                scope.Complete()
            End Using

        Catch ex As ApplicationException
            errorMessage = "Error: Direct Issue details was not inserted"
            Return errorMessage

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then Throw
        End Try

        Return errorMessage

    End Function

    ''' <summary>
    ''' Function - UpdateDirectIssue;
    ''' 17 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="directIssueID"></param>
    ''' <param name="storeID"></param>
    ''' <param name="consumerID"></param>
    ''' <param name="directIssueDocumentNo"></param>
    ''' <param name="directIssueType"></param>
    ''' <param name="directIssueSerialNo"></param>
    ''' <param name="directIssueDte"></param>
    ''' <param name="directIssueStatus"></param>
    ''' <param name="directIssueItemList"></param>
    ''' <param name="loginUser"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateDirectIssue(ByVal directIssueID As Integer, _
                                          ByVal storeID As String, _
                                          ByVal consumerID As String, _
                                          ByVal directIssueDocumentNo As String, _
                                          ByVal directIssueType As String, _
                                          ByVal directIssueSerialNo As String, _
                                          ByVal directIssueDte As Date, _
                                          ByVal directIssueStatus As String, _
                                          ByVal mode As String, _
                                          ByVal directIssueItemList As List(Of DirectIssueItem), _
                                          ByVal loginUser As String) As String

        Dim ErrorMessage As String = String.Empty
        Dim unique As Boolean = False

        Try

            Using scope As New TransactionScope(TransactionScopeOption.RequiresNew)

                'found = StockControlDAL.CheckSerialNo(storeID, directIssueSerialNo)
                unique = StockControlDAL.FieldIsUnique(storeID, StockControlDAL.TableName.DirectIssue, _
                                                      StockControlDAL.ColumnName.DirectIssueSerialNo, _
                                                      directIssueSerialNo, StockControlDAL.ColumnName.DirectIssueDocNo, directIssueDocumentNo)

                If Not unique Then

                    ErrorMessage = "Serial No [" & directIssueSerialNo & "] already exists."

                Else

                    StockControlDAL.UpdateDirectIssue(directIssueID, storeID, consumerID, _
                                                  directIssueDocumentNo, directIssueType, _
                                                  directIssueSerialNo, directIssueDte, _
                                                  directIssueStatus, loginUser)


                    For Each item As DirectIssueItem In directIssueItemList

                        Select Case item.Mode
                            Case "I"

                                item.ItemID = IIf(item.ItemID = String.Empty, "DIRECT", item.ItemID)

                                StockControlDAL.InsertDirectIssueItem(directIssueID, item.ItemID, item.ItemDescription, _
                                                              item.StockType, item.ItemQty, item.UOM, _
                                                              item.TotalCost, item.Remarks, item.ItemStatus, _
                                                              loginUser)
                            Case "D"

                                StockControlDAL.DeleteDirectIssueItem(directIssueID, item.ItemID)

                            Case "U"

                                StockControlDAL.UpdateDirectIssueItem(directIssueID, item.ItemID, item.ItemDescription, _
                                                                      item.StockType, item.ItemQty, item.UOM, _
                                                                      item.TotalCost, item.Remarks, item.ItemStatus, _
                                                                      loginUser)

                        End Select

                    Next

                End If

                scope.Complete()

            End Using

        Catch ex As ApplicationException
            ErrorMessage = "Error: Direct Issue details was not updated"
            Return ErrorMessage

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then Throw
        End Try

        Return ErrorMessage

    End Function

    ''' <summary>
    ''' Function - DeleteDirectIssue;
    ''' 14 Feb 09 - Jianfa;
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteDirectIssue(ByVal storeID As String, ByVal directIssueID As Integer) As String

        Dim ErrorMessage As String = String.Empty

        Try

            Using scope As New TransactionScope(TransactionScopeOption.RequiresNew)

                StockControlDAL.DeleteDirectIssueItem(directIssueID)

                scope.Complete()

            End Using

        Catch ex As ApplicationException

            ErrorMessage = "Error: Direct Issue details was not deleted"
            Return ErrorMessage

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then Throw
        End Try

        Return ErrorMessage

    End Function

    ''' <summary>
    ''' Function - GetDirectIssueID
    ''' 14 Feb 09 - Jianfa
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetDirectIssueID(ByVal storeID As String) As DataSet

        Dim RecordRetrieved As New DataSet
        Try

            RecordRetrieved = StockControlDAL.GetDistinctDirectIssueID(storeID)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then Throw
        End Try

        Return RecordRetrieved

    End Function

    ''' <summary>
    ''' Function - GetDirectIssueInfo;
    ''' 18 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Change Req:
    ''' 1) To use 'GetLastSerialNo' instead
    ''' </remarks>
    Public Shared Function GetDirectIssueInfo(ByVal storeID As String) As String

        Dim RecordRetrieved As String = String.Empty
        Try

            'RecordRetrieved = StockControlDAL.GetDirectIssueInformation(storeID)
            RecordRetrieved = StockControlDAL.GetLastSerialNo(storeID, StockControlDAL.TableName.DirectIssue)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then Throw
        End Try

        Return RecordRetrieved

    End Function

    ''' <summary>
    ''' Function - GetDirectIssues;
    ''' 18 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="directIssueID"></param>
    ''' <param name="serialNo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetDirectIssues(ByVal storeID As String, ByVal directIssueID As Integer, _
                                           ByVal serialNo As String) As DataSet

        Dim RecordRetrieved As New DataSet
        Try

            RecordRetrieved = StockControlDAL.GetDirectIssues(storeID, directIssueID, serialNo)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then Throw
        End Try

        Return RecordRetrieved

    End Function

    ''' <summary>
    ''' Function - GetDirectIssueItems
    ''' 14 Feb 09 - Jianfa
    ''' </summary>
    ''' <param name="directIssueID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetDirectIssueItems(ByVal directIssueID As Integer) As DataSet

        Dim RecordRetrieved As New DataSet
        Try

            RecordRetrieved = StockControlDAL.GetDirectIssueItems(directIssueID)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then Throw
        End Try

        Return RecordRetrieved

    End Function

#End Region

#Region " Receive "
    ''' <summary>
    ''' Get the Receive Items for a single Order Reference on a specific date
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
    Public Shared Function GetReceiveItem(ByVal storeID As String _
                                          , ByVal orderID As String _
                                          , ByVal dte As Date _
                                          ) As DataSet
        Try
            Return StockControlDAL.GetReceiveItem(storeID _
                                                  , orderID _
                                                  , dte _
                                                  )
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then Throw

            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Get all Receive Date under the same Order
    ''' 14Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="orderID"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetReceiveDte(ByVal storeID As String _
                                         , ByVal orderID As String _
                                         ) As DataSet
        Try
            Return StockControlDAL.GetReceiveDte(storeID _
                                                 , orderID _
                                                 )
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then Throw

            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Update the Receive Items base on the Mode either Insert, Update or Delete transaction;
    ''' 1)check mandatory fields;
    ''' 2)check receive date is within financial cut off date;
    ''' 3)Process receive item. check received date is also with financial cut off date and status is still Open;
    ''' 4)the warranty date (if changed) and order status will also be updated to reflect its order fullfillment;
    ''' 14Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="type"></param>
    ''' <param name="dte"></param>
    ''' <param name="loginUser"></param>
    ''' <param name="receiveItemList"></param>
    ''' <returns>error message (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function UpdateReceiveItem(ByVal storeID As String _
                                             , ByVal type As String _
                                             , ByVal dte As Date _
                                             , ByVal loginUser As String _
                                             , ByVal receiveItemList As List(Of ReceiveItem) _
                                             ) As String
        Try
            Dim TransactionID As Integer = 0
            Dim Count As Integer = 0

            ' check for receive details required fields
            If (storeID = String.Empty _
                Or type = String.Empty _
                Or dte = Date.MinValue _
                Or loginUser = String.Empty _
                ) Then
                Throw New Exception(String.Format("Error:[{0}][{1}][{2}][{3}] Record not Updated, missing required fields", storeID, loginUser, type, CStr(dte)))
            End If


            ' check for receive item record
            If receiveItemList.Count = 0 Then
                Throw New ApplicationException(String.Format("Error:[{0}][{1}][{2}] Record not Updated, empty receive list", storeID, loginUser, CStr(dte)))
            End If


            ' check entered date is allow
            If Not StockControlDAL.WithinFinancialCutoff(storeID _
                                                         , dte _
                                                         ) Then
                Throw New ApplicationException(String.Format("[{0}] is not within financial cut off date. Record not updated", CStr(dte)), New Exception(String.Format("Error:[{0}][{1}][{2}] Record not Updated", storeID, loginUser, CStr(dte))))
            End If


            ' Process Receive Stock Transaction
            Using scope As New TransactionScope(TransactionScopeOption.RequiresNew)
                For Each item In receiveItemList

                    Select Case item.Mode
                        Case "D"
                            ' check ID fields
                            If (item.TranID < 1) Then
                                Throw New Exception(String.Format("Error:[{0}][{1}] Record not Updated, receiveitem delete", storeID, loginUser))
                            End If

                            ' check if the existing record is still allow for deletion
                            If Not StockControlDAL.WithinFinancialCutoff(storeID _
                                                                         , item.TranID _
                                                                         ) Then
                                Throw New ApplicationException("Receive Transaction already over financial cut off date. Record not updated", New Exception(String.Format("Error:[{0}][{1}][{2}] Record not Updated, receiveitem delete", storeID, loginUser, CStr(item.TranID))))
                            End If

                            ' delete transaction
                            StockControlDAL.DeleteStockTransaction(storeID _
                                                                   , item.TranID _
                                                                   , loginUser _
                                                                   )

                        Case "I"
                            ' check mandatory fields
                            If (item.Qty <= 0D _
                                Or item.TotalCost <= 0.0 _
                                ) Then
                                Throw New ApplicationException("Missing Receive Item Mandatory Fields", New Exception(String.Format("Error:[{0}][{1}] Record not updated", storeID, loginUser)))
                            End If

                            ' check mandatory fields
                            If (item.StockItemID = String.Empty _
                                Or item.ItemRef < 1 _
                                ) Then
                                Throw New Exception(String.Format("Error:[{0}] Record not Updated, receiveitem insert", storeID))
                            End If

                            ' insert transaction
                            TransactionID = StockControlDAL.InsertStockTransaction(storeID _
                                                                                   , type _
                                                                                   , item.StockItemID _
                                                                                   , dte _
                                                                                   , item.Qty _
                                                                                   , item.Remarks _
                                                                                   , item.ItemRef _
                                                                                   , loginUser _
                                                                                   , item.TotalCost _
                                                                                   )
                            ' check if record created had created successfully
                            If TransactionID < 1 Then Throw New Exception(String.Format("Error:[{0}][{1}][{2}][{3}] Record not Updated, receiveitem insert", storeID, loginUser, CStr(item.ItemRef), item.StockItemID))

                        Case "U"
                            ' check mandatory fields
                            If (item.Qty <= 0D _
                                Or item.TotalCost <= 0.0 _
                                ) Then
                                Throw New ApplicationException("Missing Receive Item Mandatory Fields", New Exception(String.Format("Error:[{0}][{1}] Record not updated", storeID, loginUser)))
                            End If

                            ' check ID field
                            If (item.TranID < 1) Then
                                Throw New Exception(String.Format("Error:[{0}][{1}] Record not Updated, missing tranID receive item update", storeID, loginUser))
                            End If

                            ' check if the existing record is still allow for update
                            If Not StockControlDAL.WithinFinancialCutoff(storeID _
                                                                         , item.TranID _
                                                                         ) Then
                                Throw New ApplicationException("Receive Transaction already over financial cut off date. Record not updated", New Exception(String.Format("Error:[{0}][{1}][{2}] Record not Updated, receiveitem delete", storeID, loginUser, CStr(item.TranID))))
                            End If

                            ' update transaction
                            StockControlDAL.UpdateStockTransaction(storeID _
                                                                   , item.TranID _
                                                                   , dte _
                                                                   , item.Qty _
                                                                   , item.Remarks _
                                                                   , loginUser _
                                                                   , item.TotalCost _
                                                                   )
                    End Select

                    ' Update Order Item Status and warranty date (if any changed)
                    StockControlDAL.UpdateOrderItemStatus(storeID _
                                                          , item.ItemRef _
                                                          , item.OrderItemWarrantyDte _
                                                          )

                Next

                scope.Complete()
            End Using

            Return String.Empty
        Catch ex As ApplicationException
            ExceptionPolicy.HandleException(ex, "Business Policy")
            Return ex.Message
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then Throw

            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Get the Receive list base on parameters;
    ''' when orderID is with value, ignore other parameters and get only receive items relates to the single Order Reference
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="fromDte"></param>
    ''' <param name="toDte"></param>
    ''' <param name="fromStockItemID"></param>
    ''' <param name="toStockItemID"></param>
    ''' <param name="orderID">overwrite other parameter and return value for this order only</param>
    ''' <returns>Receive DataSet</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetReceiveList(ByVal storeID As String _
                                          , Optional ByVal fromDte As Date = #12:00:00 AM# _
                                          , Optional ByVal toDte As Date = #12:00:00 AM# _
                                          , Optional ByVal fromStockItemID As String = "" _
                                          , Optional ByVal toStockItemID As String = "" _
                                          , Optional ByVal orderID As String = "" _
                                          ) As DataSet
        Try
            Return StockControlDAL.GetReceiveList(storeID _
                                                  , fromDte _
                                                  , toDte _
                                                  , fromStockItemID _
                                                  , toStockItemID _
                                                  , orderID _
                                                  )
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then Throw

            Return Nothing
        End Try
    End Function

#End Region

#Region " Issue from Store (Request, Approve, Issue) "
    ''' <summary>
    ''' Add New Request and its Item Details;
    ''' 1) Check mandatory fields;
    ''' 2) Insert records using Transaction Scope;
    ''' 20Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="consumerID"></param>
    ''' <param name="requestID">Unique ID generated for the new record</param>
    ''' <param name="type"></param>
    ''' <param name="sought"></param>
    ''' <param name="loginUser"></param>
    ''' <param name="requestItemList"></param>
    ''' <returns>Error Message (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function AddRequest(ByVal storeID As String _
                                      , ByVal consumerID As String _
                                      , ByVal requestID As String _
                                      , ByVal type As String _
                                      , ByVal sought As Boolean _
                                      , ByVal loginUser As String _
                                      , ByVal requestItemList As List(Of RequestItem) _
                                      ) As String
        Try
            Dim RequestItemID As Integer = 0

            ' check for request details required fields
            If (storeID = String.Empty _
                Or loginUser = String.Empty _
                ) Then
                Throw New Exception(String.Format("Error:[{0}][{1}] Record not Inserted", storeID, loginUser))
            End If


            ' check for request details mandatory fields
            If (consumerID = String.Empty _
                Or type = String.Empty _
                Or sought = False _
                ) Then
                Throw New ApplicationException("Missing Request Mandatory Fields", New Exception(String.Format("Error:[{0}][{1}][{2}] Record not Inserted", storeID, loginUser, consumerID)))
            End If


            ' check for request item details mandatory fields
            If requestItemList.Count > 0 Then
                ' search requestItemList for mandatory fields in RequestItem, retrieve blank list
                Dim EmptyStockItemID As List(Of RequestItem) = requestItemList.FindAll(Function(item As RequestItem) item.StockItemID = Nothing Or item.StockItemID = String.Empty)
                Dim EmptyQty As List(Of RequestItem) = requestItemList.FindAll(Function(item As RequestItem) item.Qty = Nothing Or item.Qty = 0D)

                If (EmptyStockItemID.Count _
                    + EmptyQty.Count _
                    ) > 0 Then
                    Throw New ApplicationException("Missing Request Item Mandatory Fields", New Exception(String.Format("Error:[{0}][{1}] Record not Inserted", storeID, loginUser)))
                End If
            Else
                Throw New ApplicationException("Missing Request Item Mandatory Fields", New Exception(String.Format("Error:[{0}][{1}] Record not Inserted", storeID, loginUser)))
            End If


            ' Insert request details
            Using scope As New TransactionScope(TransactionScopeOption.RequiresNew)
                requestID = GenerateDocNo(storeID _
                                          , TableName.Issue _
                                          )

                If requestID = String.Empty Then Throw New Exception(String.Format("Error:[{0}][{1}] Record not Inserted, request unique ID not created", storeID, loginUser))
                StockControlDAL.InsertRequest(storeID _
                                              , consumerID _
                                              , requestID _
                                              , type _
                                              , sought _
                                              , loginUser _
                                              )

                ' Insert request item details
                For Each item In requestItemList
                    RequestItemID = 0
                    RequestItemID = StockControlDAL.InsertRequestItem(storeID _
                                                                      , requestID _
                                                                      , item.StockItemID _
                                                                      , item.Qty _
                                                                      , loginUser _
                                                                      )
                    If RequestItemID < 1 Then Throw New Exception(String.Format("Error:[{0}][{1}][{2}][{3}] Record not Inserted, requestitem identity not created", storeID, loginUser, requestID, item.StockItemID))
                Next

                scope.Complete()
            End Using

            Return requestID
        Catch ex As ApplicationException
            ExceptionPolicy.HandleException(ex, "Business Policy")
            Return ex.Message

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then Throw

            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Get Request based on status, default all Open Request only;
    ''' 20Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="status"></param>
    ''' <returns>Requests DataSet Collection</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetRequest(ByVal storeID As String _
                                      , Optional ByVal status As String = "O" _
                                      ) As DataSet
        Try
            Return StockControlDAL.GetRequest(storeID _
                                              , status _
                                              )
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then Throw

            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Get Request by search criteria list;
    ''' 26Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="consumerID"></param>
    ''' <param name="requestID"></param>
    ''' <param name="status"></param>
    ''' <returns>Requests DataSet Collection</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetRequest(ByVal storeID As String _
                                      , ByVal consumerID As String _
                                      , ByVal requestID As String _
                                      , Optional ByVal status As String = "O" _
                                      ) As DataSet
        Try
            Return StockControlDAL.GetRequest(storeID _
                                              , consumerID _
                                              , requestID _
                                              , status _
                                              )
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then Throw

            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Get all Request items with or without Issue items for a single Request Reference;
    ''' 20Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="requestID"></param>
    ''' <returns>Request Items DataSet Collection</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetRequestItem(ByVal storeID As String _
                                          , ByVal requestID As String _
                                          ) As DataSet
        Try
            Return StockControlDAL.GetRequestItem(storeID _
                                                  , requestID _
                                                  )
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then Throw
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Update only to RequestItem Details, based on the requestitem's mode to either Delete, Insert or Update;
    ''' 1) Check RequestItem's info
    ''' 2) Update records using Transaction Scope
    ''' 20Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="requestID"></param>
    ''' <param name="loginUser"></param>
    ''' <param name="requestItemList"></param>
    ''' <returns>Error Message (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function UpdateRequest(ByVal storeID As String _
                                         , ByVal requestID As String _
                                         , ByVal loginUser As String _
                                         , ByVal requestItemList As List(Of RequestItem) _
                                         ) As String
        Try
            Dim RequestItemID As Integer = 0

            ' check for request details required fields
            If (storeID = String.Empty _
                Or requestID = String.Empty _
                Or loginUser = String.Empty _
                ) Then
                Throw New Exception(String.Format("Error:[{0}][{1}][{2}] Record not Updated", storeID, loginUser, requestID))
            End If

            If requestItemList.Count > 0 Then
                ' search requestItemList for mandatory fields in RequestItem, retrieve blank list
                Dim EmptyMode As List(Of RequestItem) = requestItemList.FindAll(Function(item As RequestItem) item.Mode = Nothing Or item.Mode = String.Empty)

                If (EmptyMode.Count) > 0 Then
                    Throw New Exception(String.Format("Error:[{0}][{1}][{2}] Record not Updated, missing receiveitem mode", storeID, loginUser, requestID))
                End If
            Else
                Throw New ApplicationException("Missing Request Item Mandatory Fields", New Exception(String.Format("Error:[{0}][{1}][{2}] Record not Updated", storeID, loginUser, requestID)))
            End If


            ' Update request details
            Using scope As New TransactionScope(TransactionScopeOption.RequiresNew)
                For Each item In requestItemList
                    Select Case item.Mode
                        Case "D"
                            If (item.RequestItemID = 0) Then
                                Throw New Exception(String.Format("Error:[{0}][{1}][{2}] Record not Updated, requestitem delete", storeID, loginUser, requestID))
                            End If
                            StockControlDAL.DeleteRequestItem(storeID _
                                                              , requestID _
                                                              , item.RequestItemID _
                                                              , loginUser _
                                                              )

                        Case "I"
                            RequestItemID = StockControlDAL.InsertRequestItem(storeID _
                                                                              , requestID _
                                                                              , item.StockItemID _
                                                                              , item.Qty _
                                                                              , loginUser _
                                                                              )
                            If RequestItemID < 1 Then Throw New Exception(String.Format("Error:[{0}][{1}][{2}][{3}] Record not Updated, requestitem insert", storeID, loginUser, requestID, item.StockItemID))

                        Case "U"
                            If (item.StockItemID = String.Empty _
                                Or item.Qty = 0D _
                                ) Then
                                Throw New ApplicationException("Missing Request Item Mandatory Fields", New Exception(String.Format("Error:[{0}][{1}][{2}][{3}] Record not Updated, requestitem insert", storeID, loginUser, requestID, RequestItemID)))
                            End If

                            If (item.RequestItemID <= 0) Then
                                Throw New Exception(String.Format("Error:[{0}][{1}][{2}][{3}] Record not Updated, requestitem update", storeID, loginUser, requestID, RequestItemID))
                            End If
                            StockControlDAL.UpdateRequestItem(storeID _
                                                              , requestID _
                                                              , item.RequestItemID _
                                                              , item.StockItemID _
                                                              , item.Qty _
                                                              , loginUser _
                                                              )
                    End Select
                Next

                scope.Complete()
            End Using
            Return String.Empty

        Catch ex As ApplicationException
            ExceptionPolicy.HandleException(ex, "Business Policy")
            Return ex.Message

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then Throw

            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Delete Request and its Request Items details;
    ''' check request is not processed before allowing deletion;
    ''' 20Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="requestID"></param>
    ''' <param name="loginUser"></param>
    ''' <returns>error message (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function DeleteRequest(ByVal storeID As String _
                                         , ByVal requestID As String _
                                         , ByVal status As String _
                                         , ByVal loginUser As String _
                                         ) As String
        Try
            ' check for request details mandatory fields
            If storeID = String.Empty _
                Or requestID = String.Empty _
                Or loginUser = String.Empty _
                Or status = String.Empty _
            Then
                Throw New Exception(String.Format("Error:[{0}][{1}][{2}][{3}] Record not Deleted", storeID, loginUser, "request:" + requestID, status))
            End If

            ' check status is Open to allow for deletion
            If status = "O" Then
                StockControlDAL.DeleteRequest(storeID _
                                              , requestID _
                                              , loginUser _
                                              )
            Else
                Throw New ApplicationException(String.Format("Request[{0}] is already processed, Deletion not allow.", requestID), New Exception(String.Format("Error:[{0}][{1}][{2}] Record not Deleted", storeID, loginUser, requestID)))
            End If
            Return String.Empty

        Catch ex As ApplicationException
            ExceptionPolicy.HandleException(ex, "Business Policy")
            Return ex.Message

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
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
    Public Shared Function UpdateRequestStatus(ByVal storeID As String _
                                          , ByVal requestID As String _
                                          , ByVal status As String _
                                          , ByVal loginUser As String _
                                          ) As String
        Try
            ' Update Request Status
            StockControlDAL.UpdateRequestStatus(storeID _
                                                , requestID _
                                                , status _
                                                , loginUser _
                                                )
            Return String.Empty
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            'If (rethrow) Then Throw

            Return ex.Message
        End Try
    End Function

    ''' <summary>
    ''' Update Adjust Status to either Approved or Rejected;
    ''' 01Mar12 - Christina;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="requestID"></param>
    ''' <param name="status">Approved, Rejected</param>
    ''' <param name="loginUser"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function UpdateAdjustStatus(ByVal storeID As String _
                                         , ByVal adjustID As String _
                                         , ByVal adjustType As String _
                                         , ByVal status As String _
                                         , ByVal loginUser As String _
                                         , Optional ByVal returnBy As String = "", Optional ByVal returnDte As Date = #12:00:00 AM# _
                                         , Optional ByVal approveBy As String = "", Optional ByVal approveDte As Date = #12:00:00 AM# _
                                         , Optional ByVal receiveBy As String = "", Optional ByVal receiveDte As Date = #12:00:00 AM# _
                                         ) As String
        Try
            ' Update Request Status
            StockControlDAL.UpdateAdjustStatus(storeID _
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
            Return String.Empty
        Catch ex As ApplicationException
            ExceptionPolicy.HandleException(ex, "Business Policy")
            Return ex.Message

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then Throw

            Return String.Empty
        End Try
    End Function


    ''' <summary>
    ''' Update the Issue Items base on the Mode either Insert, Update or Delete transaction;
    ''' 1)check mandatory fields;
    ''' 2)check status is approved or closed to allow for issue update.
    ''' 3)check issue date is within financial cut off date;
    ''' 4)Process issue item. check issued date is also with financial cut off date and status is still Open;
    ''' 5)the serial number will be updated and request status set to Closed;
    ''' 14Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="requestType"></param>
    ''' <param name="requestID"></param>
    ''' <param name="issueDte"></param>
    ''' <param name="requestSerialNo"></param>
    ''' <param name="requestStatus"></param>
    ''' <param name="loginUser"></param>
    ''' <param name="issueItemList"></param>
    ''' <returns>error message (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function UpdateIssueItem(ByVal storeID As String _
                                           , ByVal requestType As String _
                                           , ByVal requestID As String _
                                           , ByVal issueDte As Date _
                                           , ByVal requestSerialNo As String _
                                           , ByVal requestStatus As String _
                                           , ByVal loginUser As String _
                                           , ByVal issueItemList As List(Of IssueItem) _
                                           ) As String
        Try
            Dim TransactionID As Integer = 0
            Dim Count As Integer = 0

            ' check for issue details required fields
            If (storeID = String.Empty _
                Or requestType = String.Empty _
                Or requestID = String.Empty _
                Or issueDte = Date.MinValue _
                Or requestStatus = String.Empty _
                Or loginUser = String.Empty _
                ) Then
                Throw New Exception(String.Format("Error:[{0}][{1}][{2}][{3}][{4}][{5}] Record not Updated, missing required fields", storeID, loginUser, requestType, requestID, CStr(issueDte), requestStatus))
            End If

            '' UAT02 - don't check to allow closing of Request without Issue
            ' '' check for issue item record
            ''If issueItemList.Count = 0 Then
            ''    Throw New Exception(String.Format("Error:[{0}][{1}][{2}][{3}] Record not Updated, empty issue list", storeID, loginUser, requestID, CStr(issueDte)))
            ''End If


            ' check status is either approved or closed (issued) to allow for issue update
            If (requestStatus = "R" Or requestStatus = "O") Then
                Throw New ApplicationException(String.Format("Request[{0}] is either not approve or already rejected, Issue not allow.", requestID), New Exception(String.Format("Error:[{0}][{1}][{2}][{3}] Record not updated", storeID, loginUser, requestID, requestStatus)))
            End If


            ' check entered date is allow
            If Not StockControlDAL.WithinFinancialCutoff(storeID _
                                                         , issueDte _
                                                         ) Then
                Throw New ApplicationException(String.Format("[{0}][{1}] is not within financial cut off date. Record not updated", requestID, CStr(issueDte)), New Exception(String.Format("Error:[{0}][{1}][{2}][{3}] Record not Updated", storeID, loginUser, requestID, CStr(issueDte))))
            End If


            ' Process Issue Stock Transaction
            Using scope As New TransactionScope(TransactionScopeOption.RequiresNew)
                For Each item In issueItemList

                    Select Case item.Mode
                        Case "D"
                            ' check ID fields
                            If (item.TranID < 1) Then
                                Throw New Exception(String.Format("Error:[{0}][{1}][{2}] Record not Updated, issueitem delete", storeID, loginUser, requestID))
                            End If

                            ' check if the existing record is still allow for deletion
                            If Not StockControlDAL.WithinFinancialCutoff(storeID _
                                                                         , item.TranID _
                                                                         ) Then
                                Throw New ApplicationException(String.Format("[{0}] Issue Transaction already over financial cut off date. Record not updated", requestID), New Exception(String.Format("Error:[{0}][{1}][{2}][{3}] Record not Updated, issueitem delete", storeID, loginUser, requestID, CStr(item.TranID))))
                            End If

                            ' delete transaction
                            StockControlDAL.DeleteStockTransaction(storeID _
                                                                   , item.TranID _
                                                                   , loginUser _
                                                                   )

                        Case "I"
                            ' check mandatory fields
                            If (item.Qty = 0D) Then
                                Throw New ApplicationException("Issue Qty cannot be Zero", New Exception(String.Format("Error:[{0}][{1}][{2}] Record not updated", storeID, loginUser, requestID)))
                            End If

                            ' check mandatory fields
                            If (item.StockItemID = String.Empty _
                                Or item.ItemRef < 1 _
                                ) Then
                                Throw New Exception(String.Format("Error:[{0}][{1}] Record not Updated, issueitem insert", storeID, requestID))
                            End If

                            ' insert transaction
                            TransactionID = StockControlDAL.InsertStockTransaction(storeID _
                                                                                   , requestType _
                                                                                   , item.StockItemID _
                                                                                   , issueDte _
                                                                                   , item.Qty _
                                                                                   , item.Remarks _
                                                                                   , item.ItemRef _
                                                                                   , loginUser _
                                                                                   )
                            ' check if record created had created successfully
                            If TransactionID < 1 Then Throw New Exception(String.Format("Error:[{0}][{1}][{2}][{3}][{4}] Record not Updated, issueitem insert", storeID, loginUser, requestID, CStr(item.ItemRef), item.StockItemID))

                        Case "U"
                            ' check mandatory fields
                            If (item.Qty = 0D) Then
                                Throw New ApplicationException("Issue Qty cannot be Zero", New Exception(String.Format("Error:[{0}][{1}][{2}] Record not updated", storeID, loginUser, requestID)))
                            End If

                            ' check ID field
                            If (item.TranID < 1) Then
                                Throw New Exception(String.Format("Error:[{0}][{1}] Record not Updated, missing tranID issueitem update", storeID, loginUser))
                            End If

                            ' check if the existing record is still allow for update
                            If Not StockControlDAL.WithinFinancialCutoff(storeID _
                                                                         , item.TranID _
                                                                         ) Then
                                Throw New ApplicationException(String.Format("[{0}] Issue Transaction already over financial cut off date. Record not updated", requestID), New Exception(String.Format("Error:[{0}][{1}][{2}][{3}] Record not Updated, issueitem delete", storeID, loginUser, requestID, CStr(item.TranID))))
                            End If

                            ' update transaction
                            StockControlDAL.UpdateStockTransaction(storeID _
                                                                   , item.TranID _
                                                                   , issueDte _
                                                                   , item.Qty _
                                                                   , item.Remarks _
                                                                   , loginUser _
                                                                   )
                    End Select

                Next

                ' Update Request Status to Closed and Serial No (if any changed)
                StockControlDAL.UpdateRequestStatus(storeID _
                                                    , requestID _
                                                    , "C" _
                                                    , loginUser _
                                                    , requestSerialNo _
                                                    )

                scope.Complete()
            End Using

            Return String.Empty
        Catch ex As ApplicationException
            ExceptionPolicy.HandleException(ex, "Business Policy")
            Return ex.Message
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then Throw

            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Get the Issue list base on parameters;
    ''' when requestID is with value, ignore other parameters and get only issue items relates to the single Request Reference
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="fromDte"></param>
    ''' <param name="toDte"></param>
    ''' <param name="fromStockItemID"></param>
    ''' <param name="toStockItemID"></param>
    ''' <param name="requestID">overwrite other parameter and return value for this request only</param>
    ''' <returns>Issue DataSet</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetIssueList(ByVal storeID As String _
                                          , Optional ByVal fromDte As Date = #12:00:00 AM# _
                                          , Optional ByVal toDte As Date = #12:00:00 AM# _
                                          , Optional ByVal fromStockItemID As String = "" _
                                          , Optional ByVal toStockItemID As String = "" _
                                          , Optional ByVal requestID As String = "" _
                                          , Optional ByVal consumerID As String = "" _
                                          ) As DataSet
        Try
            Return StockControlDAL.GetIssueList(storeID _
                                                , fromDte _
                                                , toDte _
                                                , fromStockItemID _
                                                , toStockItemID _
                                                , requestID _
                                                , consumerID _
                                                )
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then Throw

            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Get the direct Issue list base on parameters;
    ''' 25Feb09 - Guo Feng;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="fromDte"></param>
    ''' <param name="toDte"></param>
    ''' <param name="docNo"></param>
    ''' <returns>Direct Issue DataSet</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetDirectIssueList(ByVal storeID As String _
                                          , Optional ByVal fromDte As Date = #12:00:00 AM# _
                                          , Optional ByVal toDte As Date = #12:00:00 AM# _
                                          , Optional ByVal docNo As String = "" _
                                          ) As DataSet
        Try
            Return StockControlDAL.GetDirectIssueList(storeID _
                                                , fromDte _
                                                , toDte _
                                                , docNo _
                                                )
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then Throw

            Return Nothing
        End Try
    End Function

#End Region

#Region " Adjustment "
    ''' <summary>
    ''' Get Adjust based on type(AI or AO) n status, default all Open Adjust only;
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
    Public Shared Function GetAdjust(ByVal storeID As String _
                                     , ByVal type As String _
                                     , Optional ByVal status As String = "O" _
                                     ) As DataSet
        Try
            Return StockControlDAL.GetAdjust(storeID _
                                             , type _
                                             , status _
                                             )
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
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
    Public Shared Function GetAdjustItem(ByVal storeID As String _
                                         , ByVal adjustID As String _
                                         , ByVal type As String _
                                         ) As DataSet
        Try
            Return StockControlDAL.GetAdjustItem(storeID _
                                                 , adjustID _
                                                 , type _
                                                 )
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then Throw
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Get Adjust by search criteria list;
    ''' 09 Mar 12 - Christina;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="consumerID"></param>
    ''' <param name="adjustID"></param>
    ''' <param name="status"></param>
    ''' <returns>Adjust DataSet Collection</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetAdjustBySearch(ByVal storeID As String _
                                      , ByVal consumerID As String _
                                      , ByVal adjustID As String _
                                      , ByVal adjustType As String _
                                      , Optional ByVal status As String = "O" _
                                      ) As DataSet
        Try
            Return StockControlDAL.GetAdjustBySearch(storeID _
                                              , consumerID _
                                              , adjustID _
                                              , adjustType _
                                              , status _
                                              )
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then Throw

            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Add New Adjust and its Item Details also to stock transaction;
    ''' 1) Check mandatory fields;
    ''' 2) Insert records using Transaction Scope;
    ''' 20Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="adjustID">Unique ID generated for the new record</param>
    ''' <param name="type"></param>
    ''' <param name="dte"></param>
    ''' <param name="serialNo"></param>
    ''' <param name="involveID"></param>
    ''' <param name="loginUser"></param>
    ''' <param name="adjustItemList"></param>
    ''' <param name="docReturn">only applies to adjustment inwards for returning previous Issued</param>
    ''' <returns>Error Message (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function AddAdjust(ByVal storeID As String _
                                     , ByVal adjustID As String _
                                     , ByVal type As String _
                                     , ByRef dte As Date _
                                     , ByVal serialNo As String _
                                     , ByVal involveID As String _
                                     , ByVal docReturn As String _
                                     , ByVal loginUser As String _
                                     , ByVal returnUser As String _
                                     , ByVal returnDte As Date _
                                     , ByVal adjustItemList As List(Of AdjustItem) _
                                     ) As String
        Try
            Dim AdjustItemID As Integer = 0
            Dim TransactionID As Integer = 0
            Dim Count As Integer = 0
            Dim adjustStatus As String = "C"
            Dim adjustTransactionStatus = "O"

            ' check for adjust details required fields
            If (storeID = String.Empty _
                Or loginUser = String.Empty _
                ) Then
                Throw New Exception(String.Format("Error:[{0}][{1}] Record not Inserted", storeID, loginUser))
            End If


            ' check for adjust details mandatory fields
            If (type = String.Empty) Then
                Throw New ApplicationException("Missing Adjust Mandatory Fields", New Exception(String.Format("Error:[{0}][{1}][{2}][{3}] Record not Inserted", storeID, loginUser, adjustID, type)))
            End If


            ' extra check for adjust inwards's type = returns from consumer
            If (type = "AIRETURN") Then
                If (involveID = String.Empty _
                    Or docReturn = String.Empty) Then
                    Throw New ApplicationException("Missing Adjust Mandatory Fields", New Exception(String.Format("Error:[{0}][{1}][{2}][{3}] Record not Inserted", storeID, loginUser, adjustID, type)))
                End If
            End If


            ' check for adjust item details mandatory fields
            If adjustItemList.Count > 0 Then
                ' search adjustItemList for mandatory fields in AdjustItem, retrieve blank list

                'Dim subLen As Integer = 5
                'If type.Contains("AOLOSS") Then
                '    subLen = 4
                'End If

                'Select Case type.Substring(2, subLen)
                Select Case type.Substring(2, 5)
                    ' stock adjustment need qty value only
                    Case "STOCK"
                        Dim EmptyQty As List(Of AdjustItem) = adjustItemList.FindAll(Function(item As AdjustItem) item.Qty = Nothing Or item.Qty = 0D)
                        Dim FillTotalCost As List(Of AdjustItem) = adjustItemList.FindAll(Function(item As AdjustItem) item.TotalCost <> 0.0)
                        Count = EmptyQty.Count + FillTotalCost.Count

                        ' price adjustment need totalcost only
                    Case "PRICE"
                        Dim EmptyTotalCost As List(Of AdjustItem) = adjustItemList.FindAll(Function(item As AdjustItem) item.TotalCost = Nothing Or item.TotalCost = 0.0)
                        Dim FillQty As List(Of AdjustItem) = adjustItemList.FindAll(Function(item As AdjustItem) item.Qty <> 0D)
                        Count = EmptyTotalCost.Count + FillQty.Count

                        ' return adjustment need qty only
                    Case "RETUR"
                        Dim EmptyQty As List(Of AdjustItem) = adjustItemList.FindAll(Function(item As AdjustItem) item.Qty = Nothing Or item.Qty = 0D)
                        Count = EmptyQty.Count
                        adjustStatus = "O"
                        adjustTransactionStatus = "P"
                    Case Else
                        Select Case type.Substring(0, 2)
                            ' other adjustment inwards need both qty & total cost value
                            Case "AI"
                                Dim EmptyQty As List(Of AdjustItem) = adjustItemList.FindAll(Function(item As AdjustItem) item.Qty = Nothing Or item.Qty = 0D)
                                Dim EmptyTotalCost As List(Of AdjustItem) = adjustItemList.FindAll(Function(item As AdjustItem) item.TotalCost = Nothing Or item.TotalCost = 0.0)
                                Count = EmptyQty.Count + EmptyTotalCost.Count

                                ' other adjustment outwards only need qty and NOT total cost
                            Case "AO"
                                Dim EmptyQty As List(Of AdjustItem) = adjustItemList.FindAll(Function(item As AdjustItem) item.Qty = Nothing Or item.Qty = 0D)
                                If type = "AOOBSOLETE" Or type = "AODAMAGE" Then
                                    Count = EmptyQty.Count
                                    adjustStatus = "O"
                                    adjustTransactionStatus = "P"
                                Else
                                    Dim FillTotalCost As List(Of AdjustItem) = adjustItemList.FindAll(Function(item As AdjustItem) item.TotalCost <> 0.0)
                                    Count = EmptyQty.Count + FillTotalCost.Count
                                End If
                        End Select
                End Select

                Dim EmptyStockItemID As List(Of AdjustItem) = adjustItemList.FindAll(Function(item As AdjustItem) item.StockItemID = Nothing Or item.StockItemID = String.Empty)

                If (EmptyStockItemID.Count) + Count > 0 Then
                    Throw New ApplicationException("Missing Adjust Item Mandatory Fields, Please enter qty & total cost accordingly to the adjustment type.", New Exception(String.Format("Error:[{0}][{1}][{2}] Record not Inserted", storeID, loginUser, adjustID)))
                End If
            Else
                Throw New ApplicationException("Missing Adjust Item Mandatory Fields", New Exception(String.Format("Error:[{0}][{1}][{2}] Record not Inserted", storeID, loginUser, adjustID, type)))
            End If


            ' check date is within financial cut off
            If Not StockControlDAL.WithinFinancialCutoff(storeID _
                                                         , dte _
                                                         ) Then
                Throw New ApplicationException(String.Format("[{0}] Adjust date already over financial cut off date. Record not inserted", CStr(dte)), New Exception(String.Format("Error:[{0}][{1}][{2}][{3}][{4}] Record not Inserted", storeID, loginUser, adjustID, type, CStr(dte))))
            End If


            ' Insert adjust details
            Using scope As New TransactionScope(TransactionScopeOption.RequiresNew)
                ' Generates a unique Document number if user don't provide one
                If adjustID = String.Empty Then
                    adjustID = StockControlDAL.GenerateDocNo(storeID _
                                                             , StockControlDAL.TableName.Adjust _
                                                             )
                    If adjustID = String.Empty Then Throw New Exception(String.Format("Error:[{0}][{1}][{2}] Record not Inserted, adjust unique ID not created", storeID, loginUser, type))
                End If

                StockControlDAL.InsertAdjust(storeID _
                                             , adjustID _
                                             , type _
                                             , serialNo _
                                             , involveID _
                                             , docReturn _
                                             , dte _
                                             , loginUser _
                                             , returnUser _
                                             , returnDte _
                                             , adjustStatus _
                                             )

                For Each item In adjustItemList
                    ' Insert adjust item details
                    AdjustItemID = StockControlDAL.InsertAdjustItem(storeID _
                                                                    , adjustID _
                                                                    , item.StockItemID _
                                                                    , loginUser _
                                                                    , item.Status _
                                                                    , item.Qty _
                                                                    , item.Remarks _
                                                                    )
                    ' the identity for AjustItem will be used for transaction creation
                    If AdjustItemID < 1 Then Throw New Exception(String.Format("Error:[{0}][{1}][{2}][{3}][{4}] Record not Inserted, adjustitem identity not created", storeID, loginUser, adjustID, type, item.StockItemID))


                    If adjustTransactionStatus <> "P" Then
                        ' insert Transaction
                        TransactionID = StockControlDAL.InsertStockTransaction(storeID _
                                                                              , type _
                                                                              , item.StockItemID _
                                                                              , dte _
                                                                              , item.Qty _
                                                                              , item.Remarks _
                                                                              , AdjustItemID _
                                                                              , loginUser _
                                                                              , item.TotalCost _
                                                                              , item.ItemReturn _
                                                                              )
                        'check if record created had created successfully
                        If TransactionID < 1 Then Throw New Exception(String.Format("Error:[{0}][{1}][{2}][{3}][{4}][{5}] Record not Inserted, transaction identity not created", storeID, loginUser, adjustID, type, CStr(AdjustItemID), item.StockItemID))
                    End If

                Next

                scope.Complete()
            End Using

            Return adjustID
        Catch ex As ApplicationException
            ExceptionPolicy.HandleException(ex, "Business Policy")
            Return ex.Message

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then Throw

            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Update to Adjust n AdjustItem Details, based on the adjustitem's mode to either Delete, Insert or Update;
    ''' 1) Check AdjustItem's info
    ''' 2) Update records using Transaction Scope
    ''' 20Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="adjustID"></param>
    ''' <param name="adjustType"></param>
    ''' <param name="adjustDte"></param>
    ''' <param name="serialNo"></param>
    ''' <param name="involveID"></param>
    ''' <param name="loginUser"></param>
    ''' <param name="adjustItemList"></param>
    ''' <returns>Error Message (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' 23Mar09  KG          UAT02  fix DeleteAdjustItem;
    ''' </remarks>
    Public Shared Function UpdateAdjust(ByVal storeID As String _
                                        , ByVal adjustID As String _
                                        , ByVal adjustType As String _
                                        , ByVal adjustDte As Date _
                                        , ByVal serialNo As String _
                                        , ByVal involveID As String _
                                        , ByVal loginUser As String _
                                        , ByVal returnUser As String _
                                        , ByVal returnDte As Date _
                                        , ByVal approveUser As String _
                                        , ByVal approveDte As Date _
                                        , ByVal receiveUser As String _
                                        , ByVal receiveDte As Date _
                                        , ByVal adjustItemList As List(Of AdjustItem) _
                                        ) As String
        Try
            Dim TransactionID As Integer = 0
            Dim Count As Integer = 0

            'Dim transactionStatus As String = "C"

            ' check for adjust details required fields
            If (storeID = String.Empty _
                Or adjustID = String.Empty _
                Or adjustType = String.Empty _
                Or adjustDte = Date.MinValue _
                Or loginUser = String.Empty _
                ) Then
                Throw New Exception(String.Format("Error:[{0}][{1}][{2}][{3}] Record not Updated", storeID, loginUser, adjustID, adjustType))
            End If


            ' extra check for adjust inwards's type = returns from consumer
            If (adjustType = "AIRETURN") Then
                If (involveID = String.Empty) Then
                    Throw New ApplicationException("Missing Adjust Mandatory Fields", New Exception(String.Format("Error:[{0}][{1}][{2}][{3}] Record not Updated", storeID, loginUser, adjustID, adjustType)))
                End If
            End If


            ' check the adjustitem list
            'If adjustItemList.Count > 0 Then
            ' search adjustItemList for mandatory fields in AdjustItem, retrieve blank list
            Dim EmptyMode As List(Of AdjustItem) = adjustItemList.FindAll(Function(item As AdjustItem) item.Mode = Nothing Or item.Mode = String.Empty)

            If (EmptyMode.Count) > 0 Then
                Throw New Exception(String.Format("Error:[{0}][{1}][{2}][{3}] Record not Updated, missing receiveitem mode", storeID, loginUser, adjustID, adjustType))
            End If

            'Dim subLen As Integer = 5
            'If adjustType.Contains("AOLOSS") Then
            '    subLen = 4
            'End If
            'Select Case adjustType.Substring(2, subLen)

            ' the follow check exclude when Mode = "D"
            Select Case adjustType.Substring(2, 5)
                ' stock adjustment need qty value only
                Case "STOCK"
                    Dim EmptyQty As List(Of AdjustItem) = adjustItemList.FindAll(Function(item As AdjustItem) (item.Qty = Nothing Or item.Qty = 0D) And item.Mode <> "D")
                    Dim FillTotalCost As List(Of AdjustItem) = adjustItemList.FindAll(Function(item As AdjustItem) (item.TotalCost <> 0.0) And item.Mode <> "D")
                    Count = EmptyQty.Count + FillTotalCost.Count

                    ' price adjustment need totalcost only
                Case "PRICE"
                    Dim EmptyTotalCost As List(Of AdjustItem) = adjustItemList.FindAll(Function(item As AdjustItem) (item.TotalCost = Nothing Or item.TotalCost = 0.0) And item.Mode <> "D")
                    Dim FillQty As List(Of AdjustItem) = adjustItemList.FindAll(Function(item As AdjustItem) (item.Qty <> 0D) And item.Mode <> "D")
                    Count = EmptyTotalCost.Count + FillQty.Count

                    ' Return adjustment need qty only
                Case "RETUR"
                    Dim EmptyQty As List(Of AdjustItem) = adjustItemList.FindAll(Function(item As AdjustItem) (item.Qty = Nothing Or item.Qty = 0D) And item.Mode <> "D")
                    Count = EmptyQty.Count
                    'If adjustStatus <> "C" Then ' to preserve status of Adjustment to 'P' which is pending for approval
                    '    adjustStatus = "P"
                    'End If
                Case Else
                    Select Case adjustType.Substring(0, 2)
                        ' other adjustment inwards need both qty & total cost value
                        Case "AI"
                            Dim EmptyQty As List(Of AdjustItem) = adjustItemList.FindAll(Function(item As AdjustItem) (item.Qty = Nothing Or item.Qty = 0D) And item.Mode <> "D")
                            Dim EmptyTotalCost As List(Of AdjustItem) = adjustItemList.FindAll(Function(item As AdjustItem) (item.TotalCost = Nothing Or item.TotalCost = 0.0) And item.Mode <> "D")
                            Count = EmptyQty.Count + EmptyTotalCost.Count

                            ' other adjustment outwards only need qty and NOT total cost
                        Case "AO"
                            Dim EmptyQty As List(Of AdjustItem) = adjustItemList.FindAll(Function(item As AdjustItem) (item.Qty = Nothing Or item.Qty = 0D) And item.Mode <> "D")
                            If adjustType = "AOOBSOLETE" Or adjustType = "AODAMAGE" Then
                                Count = EmptyQty.Count
                            Else
                                Dim FillTotalCost As List(Of AdjustItem) = adjustItemList.FindAll(Function(item As AdjustItem) (item.TotalCost <> 0.0) And item.Mode <> "D")
                                Count = EmptyQty.Count + FillTotalCost.Count
                            End If
                    End Select
            End Select
            Dim EmptyStockItemID As List(Of AdjustItem) = adjustItemList.FindAll(Function(item As AdjustItem) (item.StockItemID = Nothing Or item.StockItemID = String.Empty) And item.Mode <> "D")

            If (Count + EmptyStockItemID.Count) > 0 Then
                Throw New ApplicationException("Missing Adjust Item Mandatory Fields, Please enter qty & total cost accordingly to the adjustment type.", New Exception(String.Format("Error:[{0}][{1}][{2}][{3}] Record not Inserted", storeID, loginUser, adjustID, adjustType)))
            End If

            'Else
            'Throw New ApplicationException("Missing Adjust Item Mandatory Fields", New Exception(String.Format("Error:[{0}][{1}][{2}][{3}] Record not Updated", storeID, loginUser, adjustID, adjustType)))
            'End If


            ' check if the existing record is still allow amendment
            If Not StockControlDAL.WithinFinancialCutoff(storeID _
                                                         , adjustDte _
                                                         ) Then
                Throw New ApplicationException(String.Format("[{0}] Adjust Transaction already over financial cut off date. Record not updated", CStr(adjustDte)), New Exception(String.Format("Error:[{0}][{1}][{2}][{3}][{4}] Record not Updated", storeID, loginUser, adjustID, adjustType, CStr(adjustDte))))
            End If


            ' Update adjust details
            Using scope As New TransactionScope(TransactionScopeOption.RequiresNew)
                ' update adjust (only to serialNo & involveID)
                StockControlDAL.UpdateAdjust(storeID _
                                             , adjustID _
                                             , adjustType _
                                             , serialNo _
                                             , involveID _
                                             , adjustDte _
                                             , loginUser _
                                             , returnUser _
                                             , returnDte _
                                             , approveUser _
                                             , approveDte _
                                             , receiveUser _
                                             , receiveDte _
                                             )

                ' process Adjust item
                If adjustItemList.Count > 0 Then
                    For Each item In adjustItemList
                        Select Case item.Mode
                            Case "D"
                                If (item.TranID = 0) Then
                                    Throw New Exception(String.Format("Error:[{0}][{1}][{2}][{3}] Record not Updated, adjustitem delete", storeID, loginUser, adjustID, adjustType))
                                End If

                                ' delete adjust item  ' UAT02 - use ItemRef instead of TranID
                                StockControlDAL.DeleteAdjustItem(storeID _
                                                                 , adjustID _
                                                                 , item.ItemRef _
                                                                 , loginUser _
                                                                 )

                                ' delete transaction
                                StockControlDAL.DeleteStockTransaction(storeID _
                                                                       , item.TranID _
                                                                       , loginUser _
                                                                       )

                            Case "I"
                                item.TranID = StockControlDAL.InsertAdjustItem(storeID _
                                                                                , adjustID _
                                                                                , item.StockItemID _
                                                                                , loginUser _
                                                                                , item.Status _
                                                                                , item.Qty _
                                                                                , item.Remarks _
                                                                                )

                                ' the identity for AjustItem will be used for transaction creation
                                If item.TranID < 1 Then Throw New Exception(String.Format("Error:[{0}][{1}][{2}][{3}][{4}] Record not Updated, adjustitem identity not created", storeID, loginUser, adjustID, adjustType, item.StockItemID))

                                ' insert transaction
                                TransactionID = StockControlDAL.InsertStockTransaction(storeID _
                                                                                       , adjustType _
                                                                                       , item.StockItemID _
                                                                                       , adjustDte _
                                                                                       , item.Qty _
                                                                                       , item.Remarks _
                                                                                       , item.TranID _
                                                                                       , loginUser _
                                                                                       , item.TotalCost _
                                                                                       , item.ItemReturn _
                                                                                       )

                                ' check if record created had created successfully
                                If TransactionID < 1 Then Throw New Exception(String.Format("Error:[{0}][{1}][{2}][{3}][{4}][{5}] Record not Updated, transaction identity not created", storeID, loginUser, adjustID, adjustType, CStr(item.TranID), item.StockItemID))

                            Case "U"
                                StockControlDAL.UpdateAdjustStatus(storeID _
                                                               , adjustID _
                                                               , adjustType _
                                                               , item.Status _
                                                               , loginUser _
                                                               , returnUser _
                                                               , returnDte _
                                                               , approveUser _
                                                               , approveDte _
                                                               , receiveUser _
                                                               , receiveDte _
                                                               , item.Qty _
                                                               , item.Remarks _
                                                               , item.ItemRef _
                                                               )



                                If item.TranID <= 0 And item.Status = "C" Then ' If adjustment is received and closed
                                    ' insert transaction for ajustment to receive and close
                                    TransactionID = StockControlDAL.InsertStockTransaction(storeID _
                                                                                      , adjustType _
                                                                                      , item.StockItemID _
                                                                                      , adjustDte _
                                                                                      , item.Qty _
                                                                                      , item.Remarks _
                                                                                      , item.ItemRef _
                                                                                      , loginUser _
                                                                                      , item.TotalCost _
                                                                                      , item.ItemReturn _
                                                                                      )
                                    If (TransactionID <= 0) Then
                                        Throw New Exception(String.Format("Error:[{0}][{1}][{2}][{3}] Record not Updated, transaction update", storeID, loginUser, adjustID, adjustType))
                                    End If

                                Else
                                    If (item.TranID > 0) Then
                                        ' update transaction
                                        StockControlDAL.UpdateStockTransaction(storeID _
                                                                           , item.TranID _
                                                                           , adjustDte _
                                                                           , item.Qty _
                                                                           , item.Remarks _
                                                                           , loginUser _
                                                                           , item.TotalCost _
                                                                           )
                                    End If
                                End If
                        End Select
                    Next

                End If
                scope.Complete()
            End Using

            Return String.Empty
        Catch ex As ApplicationException
            ExceptionPolicy.HandleException(ex, "Business Policy")
            Return ex.Message

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then Throw

            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Delete Adjust and all its Adjust Items n Transaction details;
    ''' check adjust is still within financial cutoff before allowing deletion;
    ''' 20Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="adjustType"></param>
    ''' <param name="adjustID"></param>
    ''' <param name="loginUser"></param>
    ''' <returns>error message (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function DeleteAdjust(ByVal storeID As String _
                                        , ByVal adjustID As String _
                                        , ByVal adjustType As String _
                                        , ByVal originalDte As Date _
                                        , ByVal loginUser As String _
                                        ) As String
        Try
            ' check for adjust details mandatory fields
            If (storeID = String.Empty _
                Or adjustID = String.Empty _
                Or originalDte = Date.MinValue _
                Or loginUser = String.Empty _
                ) Then
                Throw New Exception(String.Format("Error:[{0}][{1}][{2}][{3}] Record not Deleted", storeID, loginUser, "adjust:" + adjustID, CStr(originalDte)))
            End If

            ' check if the existing record is still allow deletion
            If Not StockControlDAL.WithinFinancialCutoff(storeID _
                                                         , originalDte _
                                                         ) Then
                Throw New ApplicationException(String.Format("[{0}][{1}] is not within financial cut off date. Record not updated", adjustID, CStr(originalDte)), New Exception(String.Format("Error:[{0}][{1}][{2}][{3}] Record not Updated", storeID, loginUser, adjustID, CStr(originalDte))))

            Else
                ' delete Adjust and all its AdjustItem & Transaction records
                StockControlDAL.DeleteAdjust(storeID _
                                             , adjustID _
                                             , adjustType _
                                             , loginUser _
                                             )
            End If
            Return String.Empty

        Catch ex As ApplicationException
            ExceptionPolicy.HandleException(ex, "Business Policy")
            Return ex.Message

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
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
    ''' <param name="type"></param>
    ''' <param name="fromDte"></param>
    ''' <param name="toDte"></param>
    ''' <param name="adjustID">overwrite other parameter and return value for this adjust only</param>
    ''' <returns>Adjust DataSet</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetAdjustList(ByVal storeID As String _
                                         , ByVal type As String _
                                         , Optional ByVal fromDte As Date = #12:00:00 AM# _
                                         , Optional ByVal toDte As Date = #12:00:00 AM# _
                                         , Optional ByVal adjustID As String = "" _
                                         ) As DataSet
        Try
            Return StockControlDAL.GetAdjustList(storeID _
                                                 , type _
                                                 , fromDte _
                                                 , toDte _
                                                 , adjustID _
                                                 )
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then Throw

            Return Nothing
        End Try
    End Function

#End Region

End Class
