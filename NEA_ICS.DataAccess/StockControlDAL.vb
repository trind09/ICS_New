Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

''' <summary>
''' Data Access Layer - for Stock Control;
''' 11Feb09 - KG;
''' </summary>
''' <remarks>
''' CHANGE LOG:
''' ddMMMyy  AuthorName  RefID  Description;
''' </remarks>
Public Class StockControlDAL

#Region " Common Function "
    Public Enum TableName As Integer
        Order = 201
        Request = 203
        DirectIssue = 204
        Adjust = 300
    End Enum

    Public Enum ColumnName As Integer
        OrderId = 2012
        OrderGebizPONo = 2013
        RequestID = 2033
        RequestSerialNo = 2035
        DirectIssueDocNo = 2044
        DirectIssueSerialNo = 2046
        AdjustID = 3002
        AdjustSerialNo = 3004
    End Enum

    ''' <summary>
    ''' Check field is Unique within the table, PK is ONLY required when the check is on existing record and column is not the PK;
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="tableName"></param>
    ''' <param name="columnName"></param>
    ''' <param name="columnValue"></param>
    ''' <param name="pkcolumnName">table's unqiue key for each store</param>
    ''' <param name="pkColumnValue">required ONLY if checking on existing record and column is not the primary key</param>
    ''' <returns>True = Unique; False = Not Unique</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function FieldIsUnique(ByVal storeID As String _
                                          , ByVal tableName As TableName _
                                          , ByVal columnName As ColumnName _
                                          , ByVal columnValue As String _
                                          , Optional ByVal pkColumnName As ColumnName = 0 _
                                          , Optional ByVal pkColumnValue As String = "" _
                                          ) As Boolean

        Try
            Dim sqlStoredProc As String = "spCheckRecordByColumnTable"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strTableName", DbType.String, tableName.ToString)
                    db.AddInParameter(dbCommand, "@strColumnName", DbType.String, columnName.ToString)

                    '-- UAT 03. Jianfa [17/04/2009] - [If string is empty for Direct Issue convert to NULL]
                    If tableName.ToString = "DirectIssue" And columnValue.Trim = String.Empty Then
                        db.AddInParameter(dbCommand, "@strValue", DbType.String, DBNull.Value)
                    Else
                        db.AddInParameter(dbCommand, "@strValue", DbType.String, columnValue)
                    End If

                    If pkColumnName = 0 Then
                        db.AddInParameter(dbCommand, "@strPKColumnName", DbType.String, DBNull.Value)
                    Else
                        db.AddInParameter(dbCommand, "@strPKColumnName", DbType.String, pkColumnName.ToString)
                    End If
                    db.AddInParameter(dbCommand, "@strPKValue", DbType.String, pkColumnValue)

                    Return (Not CInt(db.ExecuteScalar(dbCommand)) > 0)
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try
    End Function

    ''' <summary>
    ''' Get More Item Info for a single Stock Item based on Parameters;
    ''' 8 Feb 2009 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="stockItem"></param>
    ''' <param name="asOfDte"></param>
    ''' <returns>More Stock Item Info DataSet Collection</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetMoreItemInfo(ByVal storeID As String _
                                          , ByVal stockItem As String _
                                          , ByVal asOfDte As Date _
                                          ) As DataSet

        Dim MoreItemInfo As New DataSet
        Try
            Dim sqlStoredProc As String = "spSelectMoreItemInfo"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strStockItem", DbType.String, stockItem)
                    If asOfDte = Date.MinValue Then
                        db.AddInParameter(dbCommand, "@dteAsOfDte", DbType.Date, DBNull.Value)
                    Else
                        db.AddInParameter(dbCommand, "@dteAsOfDte", DbType.Date, asOfDte)
                    End If

                    MoreItemInfo.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "GetMoreItemInfo")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try

        Return MoreItemInfo

    End Function

    ''' <summary>
    ''' Get the Last Serial No for the provided Table;
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="tableName"></param>
    ''' <param name="docType"></param>
    ''' <returns>last serial number</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetLastSerialNo(ByVal storeID As String _
                                           , ByVal tableName As TableName _
                                           , Optional ByVal docType As String = "" _
                                           ) As String
        Dim lastSerialNo As String = String.Empty

        Try
            Dim sqlStoredProc As String = "spSelectLastSerialNo"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strTableName", DbType.String, tableName.ToString)
                    db.AddInParameter(dbCommand, "@strDocType", DbType.String, docType)

                    lastSerialNo = CStr(db.ExecuteScalar(dbCommand))
                    If lastSerialNo Is Nothing Then lastSerialNo = String.Empty
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try

        Return lastSerialNo
    End Function

    ''' <summary>
    ''' Check Financial Cutoff date by specific date, if within return TRUE;
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="tranDte"></param>
    ''' <returns>True = within; False = Not within</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function WithinFinancialCutoff(ByVal storeID As String _
                                                 , ByVal tranDte As Date _
                                                 ) As Boolean
        Try
            Dim sqlStoredProc As String = "spCheckWithinFinancialCutoffByDte"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    If tranDte = Date.MinValue Then
                        db.AddInParameter(dbCommand, "@dteTranDte", DbType.Date, DBNull.Value)
                    Else
                        db.AddInParameter(dbCommand, "@dteTranDte", DbType.Date, tranDte)
                    End If

                    Return CBool(db.ExecuteScalar(dbCommand))
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try
    End Function

    ''' <summary>
    ''' Check Financial Cutoff date by Transaction ID, if within return TRUE;
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="tranID"></param>
    ''' <returns>True = within; False = Not within</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function WithinFinancialCutoff(ByVal storeID As String _
                                                 , ByVal tranID As Integer _
                                                 ) As Boolean
        Try
            Dim sqlStoredProc As String = "spCheckWithinFinancialCutoffByTranID"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@intTranID", DbType.Int32, tranID)

                    Return CBool(db.ExecuteScalar(dbCommand))
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
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
        Dim DocNo As String = String.Empty

        Try
            Dim sqlStoredProc As String = "spGenerateDocNo"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strTableName", DbType.String, tableName.ToString)

                    DocNo = CStr(db.ExecuteScalar(dbCommand))
                    If DocNo Is Nothing Then Throw New Exception(String.Format("Error:[{0}][{1}] Document Number cannot be generated.", storeID, tableName.ToString))
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try

        Return DocNo
    End Function

#End Region

#Region " Order "
    ''' <summary>
    ''' Get all Order by status;
    ''' 5 Feb 2009 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="status"></param>
    ''' <returns>Orders DataSet</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetOrder(ByVal storeID As String _
                                    , ByVal status As String _
                                    ) As DataSet
        Try
            Dim Orders As New DataSet
            Dim sqlStoredProc As String = "spSelectOrder"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)

                    Orders.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "GetOrder")
                End Using
            End Using
            Return Orders

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Get all Order Item for a single Order;
    ''' 5 Feb 2009 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="orderID"></param>
    ''' <param name="unfullfillOnly"></param>
    ''' <returns>Orders Item DataSet</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetOrderItem(ByVal storeID As String _
                                         , ByVal orderID As String _
                                         , ByVal unfullfillOnly As Boolean _
                                         ) As DataSet
        Try
            Dim OrderItems As New DataSet
            Dim sqlStoredProc As String = "spSelectOrderItem"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strOrderID", DbType.String, orderID)
                    db.AddInParameter(dbCommand, "@blnUnFullfillOnly", DbType.Boolean, unfullfillOnly)
                    OrderItems.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "GetOrderItem")
                End Using
            End Using
            Return OrderItems

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Check Order ID, if found return TRUE;
    ''' 5 Feb 2009 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="orderID"></param>
    ''' <returns>True = Found; False = Not Found</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function CheckOrderID(ByVal storeID As String _
                                        , ByVal orderID As String _
                                        ) As Boolean

        Try
            Dim sqlStoredProc As String = "spCheckOrderID"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strOrderID", DbType.String, orderID)

                    Return CBool(db.ExecuteNonQuery(dbCommand) > 0)
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try
    End Function

    ''' <summary>
    ''' To add new Order;
    ''' 5 Feb 2009 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="orderID"></param>
    ''' <param name="gebizPONo"></param>
    ''' <param name="type"></param>
    ''' <param name="dte"></param>
    ''' <param name="supplierID"></param>
    ''' <param name="loginUser"></param>
    ''' <param name="status">create as Open</param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Sub InsertOrder(ByVal storeID As String _
                                  , ByVal orderID As String _
                                  , ByVal gebizPONo As String _
                                  , ByVal type As String _
                                  , ByVal dte As Date _
                                  , ByVal supplierID As String _
                                  , ByVal loginUser As String _
                                  , Optional ByVal status As String = "O" _
                                  )
        Try
            Dim sqlStoredProc As String = "spInsertOrder"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strOrderID", DbType.String, orderID)
                    db.AddInParameter(dbCommand, "@strGebizPONo", DbType.String, gebizPONo)
                    db.AddInParameter(dbCommand, "@strtype", DbType.String, type)
                    If dte = Date.MinValue Then
                        db.AddInParameter(dbCommand, "@dteOrderDte", DbType.Date, DBNull.Value)
                    Else
                        db.AddInParameter(dbCommand, "@dteOrderDte", DbType.Date, dte)
                    End If
                    db.AddInParameter(dbCommand, "@strSupplierID", DbType.String, supplierID)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                    db.ExecuteNonQuery(dbCommand)
                    'If db.ExecuteNonQuery(dbCommand) < 1 Then Throw New Exception(String.Format("Error: [{0}][{1}][{2}] Record not inserted", storeID, loginUser, "orderID:" + orderID))
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    ''' <summary>
    ''' To add new Order Item;
    ''' 05Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="orderID"></param>
    ''' <param name="stockItemID"></param>
    ''' <param name="qty"></param>
    ''' <param name="totalCost"></param>
    ''' <param name="expectedDeliveryDte"></param>
    ''' <param name="warrantyDte"></param>
    ''' <param name="remarks"></param>
    ''' <param name="loginUser"></param>
    ''' <param name="status">create as Open</param>
    ''' <returns>Identity for the new record</returns> 
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function InsertOrderItem(ByVal storeID As String _
                                           , ByVal orderID As String _
                                           , ByVal stockItemID As String _
                                           , ByVal qty As Decimal _
                                           , ByVal totalCost As Double _
                                           , ByVal expectedDeliveryDte As Date _
                                           , ByVal warrantyDte As Date _
                                           , ByVal remarks As String _
                                           , ByVal loginUser As String _
                                           , Optional ByVal status As String = "O" _
                                           ) As Integer
        Try
            Dim sqlStoredProc As String = "spInsertOrderItem"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strOrderID", DbType.String, orderID)
                    db.AddOutParameter(dbCommand, "@intOrderItemID", DbType.Int32, 4)
                    db.AddInParameter(dbCommand, "@strStockItemID", DbType.String, stockItemID)
                    db.AddInParameter(dbCommand, "@decQty", DbType.Decimal, qty)
                    db.AddInParameter(dbCommand, "@sngTotalCost", DbType.Double, totalCost)
                    If expectedDeliveryDte = Date.MinValue Then
                        db.AddInParameter(dbCommand, "@dteExpectedDeliveryDte", DbType.Date, DBNull.Value)
                    Else
                        db.AddInParameter(dbCommand, "@dteExpectedDeliveryDte", DbType.Date, expectedDeliveryDte)
                    End If
                    If warrantyDte = Date.MinValue Then
                        db.AddInParameter(dbCommand, "@dteWarrantyDte", DbType.Date, DBNull.Value)
                    Else
                        db.AddInParameter(dbCommand, "@dteWarrantyDte", DbType.Date, warrantyDte)
                    End If
                    db.AddInParameter(dbCommand, "@strRemarks", DbType.String, remarks)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                    db.ExecuteNonQuery(dbCommand)
                    'If db.ExecuteNonQuery(dbCommand) < 1 Then Throw New Exception(String.Format("Error: [{0}][{1}][{2}][{3}] Record not inserted", storeID, loginUser, "orderID:" + orderID, stockItemID))

                    Return CInt(dbCommand.Parameters("@intOrderItemID").Value)
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try
    End Function

    ''' <summary>
    ''' Update Order;
    ''' 7 Feb 2009 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="orderID"></param>
    ''' <param name="gebizPONo"></param>
    ''' <param name="type"></param>
    ''' <param name="dte"></param>
    ''' <param name="supplierID"></param>
    ''' <param name="loginUser"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Sub UpdateOrder(ByVal storeID As String _
                                  , ByVal orderID As String _
                                  , ByVal gebizPONo As String _
                                  , ByVal type As String _
                                  , ByVal dte As Date _
                                  , ByVal supplierID As String _
                                  , ByVal loginUser As String _
                                  )
        Try
            Dim sqlStoredProc As String = "spUpdateOrder"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strOrderID", DbType.String, orderID)
                    db.AddInParameter(dbCommand, "@strGebizPONo", DbType.String, gebizPONo)
                    db.AddInParameter(dbCommand, "@strType", DbType.String, type)
                    If dte = Date.MinValue Then
                        db.AddInParameter(dbCommand, "@dteOrderDte", DbType.Date, DBNull.Value)
                    Else
                        db.AddInParameter(dbCommand, "@dteOrderDte", DbType.Date, dte)
                    End If
                    db.AddInParameter(dbCommand, "@strSupplierID", DbType.String, supplierID)
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                    db.ExecuteNonQuery(dbCommand)
                    'If db.ExecuteNonQuery(dbCommand) < 1 Then Throw New Exception(String.Format("Error: [{0}][{1}][{2}] Record not updated", storeID, loginUser, "orderID:" + orderID))
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    ''' <summary>
    ''' Update Order Item;
    ''' 7 Feb 2009 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="orderID"></param>
    ''' <param name="orderItemID"></param>
    ''' <param name="stockItemID"></param>
    ''' <param name="qty"></param>
    ''' <param name="totalCost"></param>
    ''' <param name="expectedDeliveryDte"></param>
    ''' <param name="warrantyDte"></param>
    ''' <param name="remarks"></param>
    ''' <param name="loginUser"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Sub UpdateOrderItem(ByVal storeID As String _
                                      , ByVal orderID As String _
                                      , ByVal orderItemID As Integer _
                                      , ByVal stockItemID As String _
                                      , ByVal qty As Decimal _
                                      , ByVal totalCost As Double _
                                      , ByVal expectedDeliveryDte As Date _
                                      , ByVal warrantyDte As Date _
                                      , ByVal remarks As String _
                                      , ByVal loginUser As String _
                                      )
        Try
            Dim sqlStoredProc As String = "spUpdateOrderItem"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strOrderID", DbType.String, orderID)
                    db.AddInParameter(dbCommand, "@intOrderItemID", DbType.Int32, orderItemID)
                    db.AddInParameter(dbCommand, "@strStockItemID", DbType.String, stockItemID)
                    db.AddInParameter(dbCommand, "@decQty", DbType.Decimal, qty)
                    db.AddInParameter(dbCommand, "@sngTotalCost", DbType.Double, totalCost)
                    If expectedDeliveryDte = Date.MinValue Then
                        db.AddInParameter(dbCommand, "@dteExpectedDeliveryDte", DbType.Date, DBNull.Value)
                    Else
                        db.AddInParameter(dbCommand, "@dteExpectedDeliveryDte", DbType.Date, expectedDeliveryDte)
                    End If
                    If warrantyDte = Date.MinValue Then
                        db.AddInParameter(dbCommand, "@dteWarrantyDte", DbType.Date, DBNull.Value)
                    Else
                        db.AddInParameter(dbCommand, "@dteWarrantyDte", DbType.Date, warrantyDte)
                    End If
                    db.AddInParameter(dbCommand, "@strRemarks", DbType.String, remarks)
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                    db.ExecuteNonQuery(dbCommand)
                    'If db.ExecuteNonQuery(dbCommand) < 1 Then Throw New Exception(String.Format("Error: [{0}][{1}][{2}][{3}][{4}] Record not updated", storeID, loginUser, orderID, "orderItemID:" + CStr(orderItemID), stockItemID))
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    ''' <summary>
    ''' Update Order Item Status (open or closed) and Warranty date and its Order Status (partial or closed);
    ''' 14Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="orderItemID"></param>
    ''' <param name="warrantyDte"></param>
    ''' <param name="loginUser"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Sub UpdateOrderItemStatus(ByVal storeID As String _
                                           , ByVal orderItemID As Integer _
                                           , ByVal warrantyDte As Date _
                                           , Optional ByRef loginUser As String = "SYSTEM" _
                                           )
        Try
            Dim sqlStoredProc As String = "spUpdateOrderItemStatus"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@intOrderItemID", DbType.Int32, orderItemID)
                    If warrantyDte = Date.MinValue Then
                        db.AddInParameter(dbCommand, "@dteWarrantyDte", DbType.Date, DBNull.Value)
                    Else
                        db.AddInParameter(dbCommand, "@dteWarrantyDte", DbType.Date, warrantyDte)
                    End If
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                    db.ExecuteNonQuery(dbCommand)
                    'If db.ExecuteNonQuery(dbCommand) < 1 Then Throw New Exception(String.Format("Error: [{0}][{1}][{2}][{3}] Record not updated", storeID, loginUser, "orderItemID:" + CStr(orderItemID)))
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    ''' <summary>
    ''' Delete Order and all its Order Item;
    ''' 07Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="orderID"></param>
    ''' <param name="loginUser"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Sub DeleteOrder(ByVal storeID As String _
                                  , ByVal orderID As String _
                                  , ByVal loginUser As String _
                                  )
        Try
            Dim sqlStoredProc As String = "spDeleteOrder"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strOrderID", DbType.String, orderID)
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                    'If db.ExecuteNonQuery(dbCommand) < 1 Then Throw New Exception(String.Format("Error: [{0}][{1}][{2}] Record not deleted", storeID, loginUser, "orderID:" + orderID))
                    db.ExecuteNonQuery(dbCommand)
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    ''' <summary>
    ''' Delete Order Item;
    ''' 07 Feb 09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="orderID"></param>
    ''' <param name="orderItemID"></param>
    ''' <param name="StockItemID"></param>
    ''' <param name="loginUser"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' 16Mar2009  KG          UAT01  include StockItemID;
    ''' </remarks>
    Public Shared Sub DeleteOrderItem(ByVal storeID As String _
                                      , ByVal orderID As String _
                                      , ByVal orderItemID As Integer _
                                      , ByVal StockItemID As String _
                                      , ByVal loginUser As String _
                                      )
        Try
            Dim sqlStoredProc As String = "spDeleteOrderItem"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strOrderID", DbType.String, orderID)
                    db.AddInParameter(dbCommand, "@intOrderItemID", DbType.Int32, orderItemID)
                    db.AddInParameter(dbCommand, "@strStockItemID", DbType.String, StockItemID)
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                    db.ExecuteNonQuery(dbCommand)
                    'If db.ExecuteNonQuery(dbCommand) < 1 Then Throw New Exception(String.Format("Error: [{0}][{1}][{2}][{3}] Record not deleted", storeID, loginUser, orderID, "orderItemID:" + orderItemID))
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    ''' <summary>
    ''' Get all Order list by parameters;
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

        Dim Orders As New DataSet
        Try
            Dim sqlStoredProc As String = "spSelectOrderList"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strStockCodeFrom", DbType.String, fromStockItemID)
                    db.AddInParameter(dbCommand, "@strStockCodeTo", DbType.String, toStockItemID)
                    If fromDte = Date.MinValue Then
                        db.AddInParameter(dbCommand, "@dteFromDte", DbType.Date, DBNull.Value)
                    Else
                        db.AddInParameter(dbCommand, "@dteFromDte", DbType.Date, fromDte.Date)
                    End If
                    If toDte = Date.MinValue Then
                        db.AddInParameter(dbCommand, "@dteToDte", DbType.Date, DBNull.Value)
                    Else
                        db.AddInParameter(dbCommand, "@dteToDte", DbType.Date, toDte.Date)
                    End If
                    db.AddInParameter(dbCommand, "@strOrderID", DbType.String, orderID)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)

                    Orders.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "GetOrderList")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try

        Return Orders

    End Function

#End Region

#Region " Direct Issue "

    ''' <summary>
    ''' Sub Proc - InsertDirectIssue;
    ''' 13 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="consumerID"></param>
    ''' <param name="directIssueType"></param>
    ''' <param name="directIssueSerialNo"></param>
    ''' <param name="directIssueDte"></param>
    ''' <param name="directIssueStatus"></param>
    ''' <param name="loginUser"></param>
    ''' <remarks></remarks>
    Public Shared Function InsertDirectIssue(ByVal storeID As String, _
                                     ByVal consumerID As String, _
                                     ByVal directIssueDocNo As String, _
                                     ByVal directIssueType As String, _
                                     ByVal directIssueSerialNo As String, _
                                     ByVal directIssueDte As Date, _
                                     ByVal directIssueStatus As String, _
                                     ByVal loginUser As String) As Integer
        Dim IssueID As Integer = 0

        Try
            Dim sqlStoredProc As String = "spInsertDirectIssue"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strConsumerID", DbType.String, consumerID)
                    db.AddInParameter(dbCommand, "@strDirectIssueDocNo", DbType.String, directIssueDocNo)
                    db.AddInParameter(dbCommand, "@strDirectIssueType", DbType.String, directIssueType)
                    db.AddInParameter(dbCommand, "@strDirectIssueSerialNo", DbType.String, IIf(directIssueSerialNo = String.Empty, DBNull.Value, directIssueSerialNo))

                    If directIssueDte = DateTime.MinValue Then
                        db.AddInParameter(dbCommand, "@dteDirectIssueDte", DbType.Date, System.DBNull.Value)
                    Else
                        db.AddInParameter(dbCommand, "@dteDirectIssueDte", DbType.Date, directIssueDte)
                    End If

                    db.AddInParameter(dbCommand, "@strDirectIssueStatus", DbType.String, directIssueStatus)
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)
                    db.AddOutParameter(dbCommand, "@intDirectIssueID", DbType.Int64, Integer.MaxValue)

                    db.ExecuteNonQuery(dbCommand)

                    If dbCommand.Parameters("@intDirectIssueID").Value IsNot Nothing Then
                        IssueID = Convert.ToInt64(db.GetParameterValue(dbCommand, "@intDirectIssueID"))
                    End If

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try

        Return IssueID

    End Function

    ''' <summary>
    ''' Sub Proc - InsertDirectIssueItem;
    ''' 13 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="directIssueID"></param>
    ''' <param name="itemID"></param>
    ''' <param name="itemDescription"></param>
    ''' <param name="stockType"></param>
    ''' <param name="itemQty"></param>
    ''' <param name="totalCost"></param>
    ''' <param name="remarks"></param>
    ''' <param name="itemStatus"></param>
    ''' <param name="loginUser"></param>
    ''' <remarks></remarks>
    Public Shared Sub InsertDirectIssueItem(ByVal directIssueID As Integer, _
                                                 ByVal itemID As String, _
                                                 ByVal itemDescription As String, _
                                                 ByVal stockType As String, _
                                                 ByVal itemQty As Decimal, _
                                                 ByVal UOM As String, _
                                                 ByVal totalCost As Decimal, _
                                                 ByVal remarks As String, _
                                                 ByVal itemStatus As String, _
                                                 ByVal loginUser As String)

        Try

            Dim sqlStoredProc As String = "spInsertDirectIssueItem"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@intDirectIssueID", DbType.Int64, directIssueID)
                    db.AddInParameter(dbCommand, "@strItemID", DbType.String, itemID)
                    db.AddInParameter(dbCommand, "@strItemDescription", DbType.String, itemDescription)
                    db.AddInParameter(dbCommand, "@strStockType", DbType.String, stockType)
                    db.AddInParameter(dbCommand, "@decItemQty", DbType.Decimal, itemQty)
                    db.AddInParameter(dbCommand, "@strUOM", DbType.String, UOM)
                    db.AddInParameter(dbCommand, "@decTotalCost", DbType.Decimal, totalCost)
                    db.AddInParameter(dbCommand, "@strRemarks", DbType.String, remarks)
                    db.AddInParameter(dbCommand, "@strItemStatus", DbType.String, itemStatus)
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                    db.ExecuteNonQuery(dbCommand)

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try

    End Sub

    ''' <summary>
    ''' Function - CheckSerialNo
    ''' 13 Feb 09 - Jianfa
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CheckSerialNo(ByVal storeID As String, ByVal serialNo As String) As Boolean

        Try

            Dim sqlStoredProc As String = "spCheckSerialNo"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strSerialNo", DbType.String, serialNo)

                    If db.ExecuteScalar(dbCommand) > 0 Then
                        Return True
                    Else
                        Return False
                    End If

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try

        Return True

    End Function

    ''' <summary>
    ''' Sub Proc - DeleteDirectIssue;
    ''' 14 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="directIssueID"></param>
    ''' <remarks></remarks>
    Public Shared Sub DeleteDirectIssue(ByVal storeID As String, ByVal directIssueID As Integer)

        Try
            Dim sqlStoredProc As String = "spDeleteDirectIssue"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@intDirectIssueID", DbType.Int64, directIssueID)

                    db.ExecuteNonQuery(dbCommand)

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try

    End Sub

    ''' <summary>
    ''' Sub Proc - DeleteDirectIssueItem;
    ''' 14 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="directIssueID"></param>
    ''' <remarks></remarks>
    Public Shared Sub DeleteDirectIssueItem(ByVal directIssueID As Integer, Optional ByVal itemID As String = "")

        Try
            Dim sqlStoredProc As String = "spDeleteDirectIssueItem"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@intDirectIssueID", DbType.Int64, directIssueID)
                    db.AddInParameter(dbCommand, "@strItemID", DbType.String, itemID)

                    db.ExecuteNonQuery(dbCommand)

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try

    End Sub

    ''' <summary>
    ''' Sub Proc - UpdateDirectIssue;
    ''' 14 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="directIssueID"></param>
    ''' <param name="storeID"></param>
    ''' <param name="consumerID"></param>
    ''' <param name="directIssueDocNo"></param>
    ''' <param name="directIssueType"></param>
    ''' <param name="directIssueSerialNo"></param>
    ''' <param name="directIssueDte"></param>
    ''' <param name="directIssueStatus"></param>
    ''' <param name="loginUser"></param>
    ''' <remarks></remarks>
    Public Shared Sub UpdateDirectIssue(ByVal directIssueID As Integer, _
                                     ByVal storeID As String, _
                                     ByVal consumerID As String, _
                                     ByVal directIssueDocNo As String, _
                                     ByVal directIssueType As String, _
                                     ByVal directIssueSerialNo As String, _
                                     ByVal directIssueDte As Date, _
                                     ByVal directIssueStatus As String, _
                                     ByVal loginUser As String)

        Try
            Dim sqlStoredProc As String = "spUpdateDirectIssue"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strConsumerID", DbType.String, consumerID)
                    db.AddInParameter(dbCommand, "@strDirectIssueDocNo", DbType.String, directIssueDocNo)
                    db.AddInParameter(dbCommand, "@strDirectIssueType", DbType.String, directIssueType)
                    db.AddInParameter(dbCommand, "@strDirectIssueSerialNo", DbType.String, directIssueSerialNo)

                    If directIssueDte = DateTime.MinValue Then
                        db.AddInParameter(dbCommand, "@dteDirectIssueDte", DbType.Date, System.DBNull.Value)
                    Else
                        db.AddInParameter(dbCommand, "@dteDirectIssueDte", DbType.Date, directIssueDte)
                    End If

                    db.AddInParameter(dbCommand, "@strDirectIssueStatus", DbType.String, directIssueStatus)
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)
                    db.AddInParameter(dbCommand, "@intDirectIssueID", DbType.Int64, directIssueID)

                    db.ExecuteNonQuery(dbCommand)

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try

    End Sub

    ''' <summary>
    ''' Sub Proc - UpdateDirectIssueItem;
    ''' 14 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="directIssueID"></param>
    ''' <param name="itemID"></param>
    ''' <param name="itemDescription"></param>
    ''' <param name="stockType"></param>
    ''' <param name="itemQty"></param>
    ''' <param name="UOM"></param>
    ''' <param name="totalCost"></param>
    ''' <param name="remarks"></param>
    ''' <param name="itemStatus"></param>
    ''' <param name="loginUser"></param>
    ''' <remarks></remarks>
    Public Shared Sub UpdateDirectIssueItem(ByVal directIssueID As Integer, _
                                                 ByVal itemID As String, _
                                                 ByVal itemDescription As String, _
                                                 ByVal stockType As String, _
                                                 ByVal itemQty As Decimal, _
                                                 ByVal UOM As String, _
                                                 ByVal totalCost As Decimal, _
                                                 ByVal remarks As String, _
                                                 ByVal itemStatus As String, _
                                                 ByVal loginUser As String)

        Try

            Dim sqlStoredProc As String = "spUpdateDirectIssueItem"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@intDirectIssueID", DbType.Int64, directIssueID)
                    db.AddInParameter(dbCommand, "@strItemID", DbType.String, itemID)
                    db.AddInParameter(dbCommand, "@strItemDescription", DbType.String, itemDescription)
                    db.AddInParameter(dbCommand, "@strStockType", DbType.String, stockType)
                    db.AddInParameter(dbCommand, "@decItemQty", DbType.Decimal, itemQty)
                    db.AddInParameter(dbCommand, "@strUOM", DbType.String, UOM)
                    db.AddInParameter(dbCommand, "@decTotalCost", DbType.Decimal, totalCost)
                    db.AddInParameter(dbCommand, "@strRemarks", DbType.String, remarks)
                    db.AddInParameter(dbCommand, "@strItemStatus", DbType.String, itemStatus)
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                    db.ExecuteNonQuery(dbCommand)

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try

    End Sub

    ''' <summary>
    ''' Function - GetDistinctDirectIssueID;
    ''' 14 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetDistinctDirectIssueID(ByVal storeID As String) As DataSet

        Dim DirectIssues As New DataSet

        Try
            Dim sqlStoredProc As String = "spSelectDistinctDirectIssueID"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeID)

                    DirectIssues.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "DirectIssue")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try

        Return DirectIssues

    End Function

    ''' <summary>
    ''' Function - GetDirectIssues
    ''' 14 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="directIssueID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetDirectIssues(ByVal storeID As String, ByVal directIssueID As Integer, _
                                           ByVal serialNo As String) As DataSet

        Dim DirectIssues As New DataSet

        Try
            Dim sqlStoredProc As String = "spSelectDirectIssues"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@intDirectIssueID", DbType.Int64, directIssueID)
                    db.AddInParameter(dbCommand, "@strSerialNo", DbType.String, serialNo)

                    DirectIssues.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "DirectIssue")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try

        Return DirectIssues

    End Function

    ''' <summary>
    ''' Function - GeDirectIssueItems
    ''' 14 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="directIssueID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetDirectIssueItems(ByVal directIssueID As Integer) As DataSet

        Dim DirectIssueItems As New DataSet

        Try
            Dim sqlStoredProc As String = "spSelectDirectIssueItems"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@intDirectIssueID", DbType.Int64, directIssueID)

                    DirectIssueItems.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "DirectIssueItems")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try

        Return DirectIssueItems

    End Function

    ''' <summary>
    ''' Function - GetDirectIssueInformation;
    ''' 18 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetDirectIssueInformation(ByVal storeID As String)

        Dim DirectIssues As New DataSet

        Try
            Dim sqlStoredProc As String = "spSelectDirectIssueInfo"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeID)

                    DirectIssues.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "DirectIssueInfo")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try

        Return DirectIssues

    End Function

#End Region

#Region " Receive "
    ''' <summary>
    ''' Get all Order item with or without Receive items for a single Order Reference on a specific date;
    ''' 11Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="orderID"></param>
    ''' <param name="dte"></param>
    ''' <returns>Receive items DataSet</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetReceiveItem(ByVal storeID As String _
                                          , ByVal orderID As String _
                                          , ByVal dte As Date _
                                          ) As DataSet
        Dim List As New DataSet

        Try
            Dim sqlStoredProc As String = "spSelectReceiveItem"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strOrderID", DbType.String, orderID)
                    db.AddInParameter(dbCommand, "@dteReceiveDte", DbType.Date, dte)

                    List.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "GetReceiveItem")
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try

        Return List
    End Function

    ''' <summary>
    ''' Get all the Receive Date for a Order Reference;
    ''' 11Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="orderID"></param>
    ''' <returns>Date dataset</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetReceiveDte(ByVal storeID As String _
                                         , ByVal orderID As String _
                                         ) As DataSet
        Try
            Dim List As New DataSet
            Dim sqlStoredProc As String = "spSelectReceiveDte"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strOrderID", DbType.String, orderID)

                    List.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "GetReceiveDte")
                End Using
            End Using
            Return List

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw

            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Delete all receive Items for a single Order Reference on a specific date;
    ''' 11Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="orderID"></param>
    ''' <param name="receiveDte"></param>
    ''' <param name="loginUser"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Sub DeleteReceive(ByVal storeID As String _
                                    , ByVal orderID As String _
                                    , ByVal receiveDte As Date _
                                    , ByVal loginUser As String _
                                    )
        Try
            Dim sqlStoredProc As String = "spDeleteReceive"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strOrderID", DbType.String, orderID)
                    db.AddInParameter(dbCommand, "@dteReceiveDte", DbType.Date, receiveDte)
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                    db.ExecuteNonQuery(dbCommand)
                    'If db.ExecuteNonQuery(dbCommand) < 1 Then Throw New Exception(String.Format("Error: [{0}][{1}][{2}][{3}] Record not deleted", storeID, loginUser, orderID, "receive:" + CStr(receiveDte)))
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    ''' <summary>
    ''' Get Receive list based on Parameters;
    ''' when docNo is with value, ignore other parameters and get only receive items relates to the single Order Reference
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="fromDte"></param>
    ''' <param name="toDte"></param>
    ''' <param name="fromStockItemID"></param>
    ''' <param name="toStockItemID"></param>
    ''' <param name="docNo">overwrite other parameter and return value for this order only</param>
    ''' <returns>Receive DataSet</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetReceiveList(ByVal storeID As String _
                                          , Optional ByVal fromDte As Date = #12:00:00 AM# _
                                          , Optional ByVal toDte As Date = #12:00:00 AM# _
                                          , Optional ByVal fromStockItemID As String = "" _
                                          , Optional ByVal toStockItemID As String = "" _
                                          , Optional ByVal docNo As String = "" _
                                          ) As DataSet
        Try
            Dim List As New DataSet
            Dim sqlStoredProc As String = "spSelectRecieveList"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    If fromDte = Date.MinValue Then
                        db.AddInParameter(dbCommand, "@dteFromDte", DbType.Date, DBNull.Value)
                    Else
                        db.AddInParameter(dbCommand, "@dteFromDte", DbType.Date, fromDte)
                    End If
                    If toDte = Date.MinValue Then
                        db.AddInParameter(dbCommand, "@dteToDte", DbType.Date, DBNull.Value)
                    Else
                        db.AddInParameter(dbCommand, "@dteToDte", DbType.Date, toDte)
                    End If
                    db.AddInParameter(dbCommand, "@strStockCodeFrom", DbType.String, fromStockItemID)
                    db.AddInParameter(dbCommand, "@strStockCodeTo", DbType.String, toStockItemID)
                    db.AddInParameter(dbCommand, "@strDocNo", DbType.String, docNo)

                    List.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "GetReceiveList")
                End Using
            End Using

            Return List
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw

            Return Nothing
        End Try
    End Function

#End Region

#Region " StockTransaction "
    ''' <summary>
    ''' To add new StockTransaction;
    ''' 11Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="docType"></param>
    ''' <param name="stockItemID"></param>
    ''' <param name="dte"></param>
    ''' <param name="qty"></param>
    ''' <param name="remarks"></param>
    ''' <param name="itemRef"></param>
    ''' <param name="loginUser"></param>
    ''' <param name="totalCost">only required for Receive, part of Adjustment type</param>
    ''' <param name="itemReturn">only applies for referencing to previous Issued's StockTransactionID</param>
    ''' <param name="status">create as Open</param>
    ''' <returns>Identity of the new record</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function InsertStockTransaction(ByVal storeID As String _
                                                  , ByVal docType As String _
                                                  , ByVal stockItemID As String _
                                                  , ByVal dte As Date _
                                                  , ByVal qty As Decimal _
                                                  , ByVal remarks As String _
                                                  , ByVal itemRef As Integer _
                                                  , ByVal loginUser As String _
                                                  , Optional ByVal totalCost As Double = 0.0 _
                                                  , Optional ByVal itemReturn As Integer = 0 _
                                                  , Optional ByVal status As String = "O" _
                                                  ) As Integer
        Try
            Dim sqlStoredProc As String = "spInsertStockTransaction"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            If dte = Date.MinValue Then Throw New Exception("Error: Transaction Date must have a date")

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strType", DbType.String, docType)
                    db.AddOutParameter(dbCommand, "@intTranID", DbType.Int32, 4)
                    db.AddInParameter(dbCommand, "@strStockItemID", DbType.String, stockItemID)
                    db.AddInParameter(dbCommand, "@dteTranDte", DbType.Date, dte)
                    db.AddInParameter(dbCommand, "@decQty", DbType.Decimal, qty)
                    db.AddInParameter(dbCommand, "@sngTotalCost", DbType.Double, totalCost)
                    db.AddInParameter(dbCommand, "@strRemarks", DbType.String, remarks)
                    db.AddInParameter(dbCommand, "@intItemRef", DbType.Int32, itemRef)
                    db.AddInParameter(dbCommand, "@intItemReturn", DbType.Int32, itemReturn)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                    'If db.ExecuteNonQuery(dbCommand) < 1 Then Throw New Exception(String.Format("Error: [{0}][{1}][{2}][{3}][{4}] Record not inserted", storeID, loginUser, "docType:" + docType, stockItemID, "itemRef:" + CStr(itemRef)))
                    db.ExecuteNonQuery(dbCommand)

                    Return CInt(dbCommand.Parameters("@intTranID").Value)
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try
    End Function

    ''' <summary>
    ''' Update StockTransaction;
    ''' 11Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="tranID"></param>
    ''' <param name="dte"></param>
    ''' <param name="qty"></param>
    ''' <param name="remarks"></param>
    ''' <param name="loginUser"></param>
    ''' <param name="totalCost">only required for Receive, part of Adjustment type</param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Sub UpdateStockTransaction(ByVal storeID As String _
                                             , ByVal tranID As Integer _
                                             , ByVal dte As Date _
                                             , ByVal qty As Decimal _
                                             , ByVal remarks As String _
                                             , ByVal loginUser As String _
                                             , Optional ByVal totalCost As Double = 0.0 _
                                             )
        Try
            Dim sqlStoredProc As String = "spUpdateStockTransactionByTranID"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@intTranID", DbType.Int32, tranID)
                    db.AddInParameter(dbCommand, "@dteTranDte", DbType.Date, dte)
                    db.AddInParameter(dbCommand, "@decQty", DbType.Decimal, qty)
                    db.AddInParameter(dbCommand, "@strRemarks", DbType.String, remarks)
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)
                    db.AddInParameter(dbCommand, "@sngTotalCost", DbType.Double, totalCost)
                    'db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)

                    db.ExecuteNonQuery(dbCommand)
                    'If db.ExecuteNonQuery(dbCommand) < 1 Then Throw New Exception(String.Format("Error: [{0}][{1}][{2}] Record not updated", storeID, loginUser, "tranID:" + CStr(tranID)))
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    ''' <summary>
    ''' Delete Stock Transaction;
    ''' 11Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="tranID"></param>
    ''' <param name="loginUser"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Sub DeleteStockTransaction(ByVal storeID As String _
                                                  , ByVal tranID As Integer _
                                                  , ByVal loginUser As String _
                                                  )
        Try
            Dim sqlStoredProc As String = "spDeleteStockTransactionByTranID"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@intTranID", DbType.Int32, tranID)
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                    db.ExecuteNonQuery(dbCommand)
                    'If db.ExecuteNonQuery(dbCommand) < 1 Then Throw New Exception(String.Format("Error: [{0}][{1}][{2}] Record not deleted", storeID, loginUser, "tranID:" + CStr(tranID)))
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

#End Region

#Region " Issue from Store (Request, Approve, Issue) "
    ''' <summary>
    ''' Get all Request based on its status;
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="status"></param>
    ''' <returns>Request DataSet</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetRequest(ByVal storeID As String _
                                     , ByVal status As String _
                                     ) As DataSet

        Dim Request As New DataSet
        Try
            Dim sqlStoredProc As String = "spSelectRequest"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)

                    Request.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "GetRequest")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try

        Return Request
    End Function

    ''' <summary>
    ''' Get Request by search criteria list;
    ''' 26Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="consumerID"></param>
    ''' <param name="requestID"></param>
    ''' <param name="status"></param>
    ''' <returns>Request DataSet</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetRequest(ByVal storeID As String _
                                     , ByVal consumerID As String _
                                     , ByVal requestID As String _
                                     , ByVal status As String _
                                     ) As DataSet

        Dim Request As New DataSet
        Try
            Dim sqlStoredProc As String = "spSelectRequestBySearch"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strConsumerID", DbType.String, consumerID)
                    db.AddInParameter(dbCommand, "@strRequestID", DbType.String, requestID)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)

                    Request.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "GetRequestBySearch")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try

        Return Request
    End Function

    ''' <summary>
    ''' Get all Request Item for a single Request;
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="RequestID"></param>
    ''' <returns>Requests Item DataSet</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetRequestItem(ByVal storeID As String _
                                         , ByVal RequestID As String _
                                         ) As DataSet

        Dim RequestItem As New DataSet
        Try
            Dim sqlStoredProc As String = "spSelectRequestItem"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strRequestID", DbType.String, RequestID)

                    RequestItem.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "GetRequestItem")
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try

        Return RequestItem
    End Function

    ''' <summary>
    ''' To add new Request;
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="consumerID"></param>
    ''' <param name="type"></param>
    ''' <param name="sought"></param>
    ''' <param name="loginUser"></param>
    ''' <param name="serialNo">create as empty</param>
    ''' <param name="status">create as Open</param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Sub InsertRequest(ByVal storeID As String _
                                    , ByVal consumerID As String _
                                    , ByVal requestID As String _
                                    , ByVal type As String _
                                    , ByVal sought As Boolean _
                                    , ByVal loginUser As String _
                                    , Optional ByVal serialNo As String = "" _
                                    , Optional ByVal status As String = "O" _
                                    )
        Try
            Dim sqlStoredProc As String = "spInsertRequest"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strConsumerID", DbType.String, consumerID)
                    db.AddInParameter(dbCommand, "@strRequestID", DbType.String, requestID)
                    db.AddInParameter(dbCommand, "@strtype", DbType.String, type)
                    db.AddInParameter(dbCommand, "@strSerialNo", DbType.String, serialNo)
                    db.AddInParameter(dbCommand, "@blnSought", DbType.Boolean, sought)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                    'If db.ExecuteNonQuery(dbCommand) < 1 Then Throw New Exception(String.Format("Error: [{0}][{1}][{2}] Record not inserted", storeID, loginUser, "type:" + type))
                    db.ExecuteNonQuery(dbCommand)
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    ''' <summary>
    ''' To add new Request Item;
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="requestID"></param>
    ''' <param name="stockItemID"></param>
    ''' <param name="qty"></param>
    ''' <param name="loginUser"></param>
    ''' <param name="status">create as Open</param>
    ''' <returns>Identity for the new record</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function InsertRequestItem(ByVal storeID As String _
                                             , ByVal requestID As String _
                                             , ByVal stockItemID As String _
                                             , ByVal qty As Decimal _
                                             , ByVal loginUser As String _
                                             , Optional ByVal status As String = "O" _
                                             ) As Integer
        Try
            Dim sqlStoredProc As String = "spInsertRequestItem"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strRequestID", DbType.String, requestID)
                    db.AddOutParameter(dbCommand, "@intRequestItemID", DbType.Int32, 4)
                    db.AddInParameter(dbCommand, "@strStockItemID", DbType.String, stockItemID)
                    db.AddInParameter(dbCommand, "@decQty", DbType.Decimal, qty)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                    'If db.ExecuteNonQuery(dbCommand) < 1 Then Throw New Exception(String.Format("Error: [{0}][{1}][{2}][{3}] Record not inserted", storeID, loginUser, "requestID:" + requestID, stockItemID))
                    db.ExecuteNonQuery(dbCommand)

                    Return CInt(dbCommand.Parameters("@intRequestItemID").Value)
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try
    End Function

    ''' <summary>
    ''' Update Request Item;
    ''' Only original requester can update;
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="requestID"></param>
    ''' <param name="requestItemID"></param>
    ''' <param name="stockItemID"></param>
    ''' <param name="qty"></param>
    ''' <param name="loginUser"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Sub UpdateRequestItem(ByVal storeID As String _
                                        , ByVal requestID As String _
                                        , ByVal requestItemID As Integer _
                                        , ByVal stockItemID As String _
                                        , ByVal qty As Decimal _
                                        , ByVal loginUser As String _
                                        )
        Try
            Dim sqlStoredProc As String = "spUpdateRequestItem"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strRequestID", DbType.String, requestID)
                    db.AddInParameter(dbCommand, "@intRequestItemID", DbType.Int32, requestItemID)
                    db.AddInParameter(dbCommand, "@strStockItemID", DbType.String, stockItemID)
                    db.AddInParameter(dbCommand, "@decQty", DbType.Decimal, qty)
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                    db.ExecuteNonQuery(dbCommand)
                    'If db.ExecuteNonQuery(dbCommand) < 1 Then Throw New Exception(String.Format("Error: [{0}][{1}][{2}] Record not updated", storeID, loginUser, "requestID:" + requestID))
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    ''' <summary>
    ''' Delete Request and all its Request Item;
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="requestID"></param>
    ''' <param name="loginUser"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Sub DeleteRequest(ByVal storeID As String _
                                   , ByVal requestID As String _
                                   , ByVal loginUser As String _
                                   )
        Try
            Dim sqlStoredProc As String = "spDeleteRequest"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strRequestID", DbType.String, requestID)
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                    db.ExecuteNonQuery(dbCommand)
                    'If db.ExecuteNonQuery(dbCommand) < 1 Then Throw New Exception(String.Format("Error: [{0}][{1}][{2}] Record not deleted", storeID, loginUser, "requestID:" + requestID))
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    ''' <summary>
    ''' Delete Request Item;
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="requestID"></param>
    ''' <param name="requestItemID"></param>
    ''' <param name="loginUser"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Sub DeleteRequestItem(ByVal storeID As String _
                                       , ByVal requestID As String _
                                       , ByVal requestItemID As Integer _
                                       , ByVal loginUser As String _
                                       )
        Try
            Dim sqlStoredProc As String = "spDeleteRequestItem"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strRequestID", DbType.String, requestID)
                    db.AddInParameter(dbCommand, "@strRequestItemID", DbType.Int32, requestItemID)
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                    db.ExecuteNonQuery(dbCommand)
                    'If db.ExecuteNonQuery(dbCommand) < 1 Then Throw New Exception(String.Format("Error: [{0}][{1}][{2}][{3}] Record not deleted", storeID, loginUser, requestID, "requestItemID:" + requestItemID))
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    ''' <summary>
    ''' Update Request Status and also update the Serial No(if any);
    ''' Request Issued (status = Closed), Issued request deleted (revert request status back = Approved and empty issuer info)
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="requestID"></param>
    ''' <param name="status">Approved, Rejected, Closed or revert back to Approve</param>
    ''' <param name="loginUser"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Sub UpdateRequestStatus(ByVal storeID As String _
                                          , ByVal requestID As String _
                                          , ByVal status As String _
                                          , ByVal loginUser As String _
                                          , Optional ByVal requestSerialNo As String = "" _
                                          )
        Try
            Dim sqlStoredProc As String = "spUpdateRequestStatus"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strRequestID", DbType.String, requestID)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)
                    db.AddInParameter(dbCommand, "@strRequestSerialNo", DbType.String, requestSerialNo)

                    db.ExecuteNonQuery(dbCommand)
                    'If db.ExecuteNonQuery(dbCommand) < 1 Then Throw New Exception(String.Format("Error: [{0}][{1}][{2}][{3}] Record not updated", storeID, loginUser, requestID, "request status:" + status))
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    ''' <summary>
    ''' Update Adjust Status;
    ''' Request Issued (status = Closed), Issued request deleted (revert request status back = Approved and empty issuer info)
    ''' 01Mar12 - Christina;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="requestID"></param>
    ''' <param name="status">Approved, Rejected, Closed or revert back to Approve</param>
    ''' <param name="loginUser"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Sub UpdateAdjustStatus(ByVal storeID As String _
                                          , ByVal adjustID As String _
                                          , ByVal adjustType As String _
                                          , ByVal status As String _
                                          , ByVal loginUser As String _
                                          , Optional ByVal returnBy As String = "", Optional ByVal returnDte As Date = #12:00:00 AM# _
                                          , Optional ByVal approveBy As String = "", Optional ByVal approveDte As Date = #12:00:00 AM# _
                                          , Optional ByVal receiveBy As String = "", Optional ByVal receiveDte As Date = #12:00:00 AM# _
                                          , Optional ByVal adjustQty As Integer = 0 _
                                          , Optional ByVal remarks As String = "" _
                                          , Optional ByVal adjustItemID As Integer = 0 _
                                          )

        Try
            Dim sqlStoredProc As String = "spUpdateAdjustStatus"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strAdjustID", DbType.String, adjustID)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)
                    db.AddInParameter(dbCommand, "@strReturnBy", DbType.String, returnBy)

                    If returnDte = Date.MinValue Then
                        db.AddInParameter(dbCommand, "@strReturnDte", DbType.Date, DBNull.Value)
                    Else
                        db.AddInParameter(dbCommand, "@strReturnDte", DbType.Date, returnDte)
                    End If

                    db.AddInParameter(dbCommand, "@strApproveBy", DbType.String, approveBy)

                    If approveDte = Date.MinValue Then
                        db.AddInParameter(dbCommand, "@strApproveDte", DbType.Date, DBNull.Value)
                    Else
                        db.AddInParameter(dbCommand, "@strApproveDte", DbType.Date, approveDte)
                    End If

                    db.AddInParameter(dbCommand, "@strReceiveBy", DbType.String, receiveBy)

                    If receiveDte = Date.MinValue Then
                        db.AddInParameter(dbCommand, "@strReceiveDte", DbType.Date, DBNull.Value)
                    Else
                        db.AddInParameter(dbCommand, "@strReceiveDte", DbType.Date, receiveDte)
                    End If
                    'db.AddInParameter(dbCommand, "@strRequestSerialNo", DbType.String, requestSerialNo)
                    db.AddInParameter(dbCommand, "@strAdjustType", DbType.String, adjustType)

                    db.AddInParameter(dbCommand, "@intQty", DbType.Int32, adjustQty)
                    db.AddInParameter(dbCommand, "@strRemarks", DbType.String, remarks)
                    db.AddInParameter(dbCommand, "@strAdjustItemID", DbType.Int32, adjustItemID)



                    db.ExecuteNonQuery(dbCommand)
                    'If db.ExecuteNonQuery(dbCommand) < 1 Then Throw New Exception(String.Format("Error: [{0}][{1}][{2}][{3}] Record not updated", storeID, loginUser, requestID, "request status:" + status))
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    ''' <summary>
    ''' Delete all Issue Items for a single request Reference;
    ''' 23Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="RequestID"></param>
    ''' <param name="loginUser"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Sub DeleteIssue(ByVal storeID As String _
                                  , ByVal RequestID As String _
                                  , ByVal loginUser As String _
                                  )
        Try
            Dim sqlStoredProc As String = "spDeleteIssue"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strRequestID", DbType.String, RequestID)
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                    db.ExecuteNonQuery(dbCommand)
                    'If db.ExecuteNonQuery(dbCommand) < 1 Then Throw New Exception(String.Format("Error: [{0}][{1}][{2}][{3}] Record not deleted", storeID, loginUser, orderID, "receive:" + CStr(receiveDte)))
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    ''' <summary>
    ''' Get Issue based on Parameters;
    ''' when requestID is with value, ignore other parameters and get only issue items relates to the single Document No
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="fromDte"></param>
    ''' <param name="toDte"></param>
    ''' <param name="requestID">overwrite other parameter and return value for this request ID only</param>
    ''' <returns>Receive DataSet</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetIssueList(ByVal storeID As String _
                                        , Optional ByVal fromDte As Date = #12:00:00 AM# _
                                        , Optional ByVal toDte As Date = #12:00:00 AM# _
                                        , Optional ByVal fromStockItemID As String = "" _
                                        , Optional ByVal toStockItemID As String = "" _
                                        , Optional ByVal requestID As String = "" _
                                        , Optional ByVal consumerID As String = "" _
                                        ) As DataSet
        Dim List As New DataSet

        Try
            Dim sqlStoredProc As String = "spSelectIssueList"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    If fromDte = Date.MinValue Then
                        db.AddInParameter(dbCommand, "@dteFromDte", DbType.Date, DBNull.Value)
                    Else
                        db.AddInParameter(dbCommand, "@dteFromDte", DbType.Date, fromDte)
                    End If
                    If toDte = Date.MinValue Then
                        db.AddInParameter(dbCommand, "@dteToDte", DbType.Date, DBNull.Value)
                    Else
                        db.AddInParameter(dbCommand, "@dteToDte", DbType.Date, toDte)
                    End If
                    db.AddInParameter(dbCommand, "@strStockCodeFrom", DbType.String, fromStockItemID)
                    db.AddInParameter(dbCommand, "@strStockCodeTo", DbType.String, toStockItemID)
                    db.AddInParameter(dbCommand, "@strRequestID", DbType.String, requestID)
                    db.AddInParameter(dbCommand, "@strConsumerID", DbType.String, consumerID)

                    List.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "GetIssueList")
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try

        Return List
    End Function
    ''' <summary>
    ''' Get Direct Issue based on Parameters;
    ''' 25Feb09 - Guo Feng;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="fromDte"></param>
    ''' <param name="toDte"></param>
    ''' <param name="docNo"></param>
    ''' <returns>Direct Issue DataSet</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetDirectIssueList(ByVal storeID As String _
                                        , Optional ByVal fromDte As Date = #12:00:00 AM# _
                                        , Optional ByVal toDte As Date = #12:00:00 AM# _
                                        , Optional ByVal docNo As String = "" _
                                        ) As DataSet
        Dim List As New DataSet

        Try
            Dim sqlStoredProc As String = "spSelectDirectIssueList"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    If fromDte = Date.MinValue Then
                        db.AddInParameter(dbCommand, "@dteFromDte", DbType.Date, DBNull.Value)
                    Else
                        db.AddInParameter(dbCommand, "@dteFromDte", DbType.Date, fromDte)
                    End If
                    If toDte = Date.MinValue Then
                        db.AddInParameter(dbCommand, "@dteToDte", DbType.Date, DBNull.Value)
                    Else
                        db.AddInParameter(dbCommand, "@dteToDte", DbType.Date, toDte)
                    End If
                    db.AddInParameter(dbCommand, "@strDocNo", DbType.String, docNo)

                    List.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "GetDirectIssueList")
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try

        Return List
    End Function
#End Region

#Region " Adjustment "
    ''' <summary>
    ''' Get all Adjust based on its type and status;
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="adjustType "></param>
    ''' <param name="status"></param>
    ''' <returns>Adjust DataSet</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetAdjust(ByVal storeID As String _
                                     , ByVal adjustType As String _
                                     , ByVal status As String _
                                     , Optional ByVal consumerSearch As String = "" _
                                     ) As DataSet

        Dim Adjust As New DataSet
        Try
            Dim sqlStoredProc As String = "spSelectAdjust"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strType", DbType.String, adjustType)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
                    'db.AddInParameter(dbCommand, "@strConsumerSearch", DbType.String, consumerSearch)

                    Adjust.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "GetAdjust")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try

        Return Adjust
    End Function

    ''' <summary>
    ''' Get all Adjust Item for a single Adjust;
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="adjustID"></param>
    ''' <param name="adjustType"></param>
    ''' <returns>Adjusts Item DataSet</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetAdjustItem(ByVal storeID As String _
                                         , ByVal adjustID As String _
                                         , ByVal adjustType As String _
                                         ) As DataSet

        Dim AdjustItem As New DataSet
        Try
            Dim sqlStoredProc As String = "spSelectAdjustItem"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strAdjustID", DbType.String, adjustID)
                    db.AddInParameter(dbCommand, "@strType", DbType.String, adjustType)

                    AdjustItem.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "GetAdjustItem")
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try

        Return AdjustItem
    End Function

    ''' <summary>
    ''' Get Adjust by search criteria list;
    ''' 09 Mar 12 - Christina;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="consumerID"></param>
    ''' <param name="adjustID"></param>
    ''' <param name="status"></param>
    ''' <returns>Adjust DataSet</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetAdjustBySearch(ByVal storeID As String _
                                     , ByVal consumerID As String _
                                     , ByVal adjustID As String _
                                     , ByVal adjustType As String _
                                     , ByVal status As String _
                                     ) As DataSet

        Dim Adjust As New DataSet
        Try
            Dim sqlStoredProc As String = "spSelectAdjustBySearch"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strAdjustType", DbType.String, adjustType)
                    db.AddInParameter(dbCommand, "@strConsumerID", DbType.String, consumerID)
                    db.AddInParameter(dbCommand, "@strAdjustID", DbType.String, adjustID)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)

                    Adjust.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "GetAdjustBySearch")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try

        Return Adjust
    End Function

    ''' <summary>
    ''' To add new Adjust;
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="adjustType "></param>
    ''' <param name="adjustID "></param>
    ''' <param name="serialNo"></param>
    ''' <param name="involveID"></param>
    ''' <param name="loginUser"></param>
    ''' <param name="docReturn">only applies for referencing to previous Issued requestID(type=AIRETURN)</param>
    ''' <param name="status">create as Closed</param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Sub InsertAdjust(ByVal storeID As String _
                                   , ByVal adjustID As String _
                                   , ByVal adjustType As String _
                                   , ByVal serialNo As String _
                                   , ByVal involveID As String _
                                   , ByVal docReturn As String _
                                   , ByVal dte As Date _
                                   , ByVal loginUser As String _
                                   , ByVal returnUser As String _
                                   , ByVal returnDte As Date _
                                   , Optional ByVal status As String = "C" _
                                   )
        Try
            Dim sqlStoredProc As String = "spInsertAdjust"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strType", DbType.String, adjustType)
                    db.AddInParameter(dbCommand, "@strAdjustID", DbType.String, adjustID)
                    db.AddInParameter(dbCommand, "@strSerialNo", DbType.String, serialNo)
                    db.AddInParameter(dbCommand, "@strInvolveID", DbType.String, involveID)
                    db.AddInParameter(dbCommand, "@strDocReturn", DbType.String, docReturn)
                    If dte = Date.MinValue Then
                        db.AddInParameter(dbCommand, "@dteAdjustDte", DbType.Date, DBNull.Value)
                    Else
                        db.AddInParameter(dbCommand, "@dteAdjustDte", DbType.Date, dte)
                    End If
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                    db.AddInParameter(dbCommand, "@strReturnUser", DbType.String, returnUser)
                    If returnDte = Date.MinValue Then
                        db.AddInParameter(dbCommand, "@dteReturnDte ", DbType.Date, DBNull.Value)
                    Else
                        db.AddInParameter(dbCommand, "@dteReturnDte", DbType.Date, returnDte)
                    End If

                    'If db.ExecuteNonQuery(dbCommand) < 1 Then Throw New Exception(String.Format("Error: [{0}][{1}][{2}] Record not inserted", storeID, loginUser, "type:" + type))
                    db.ExecuteNonQuery(dbCommand)
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    ''' <summary>
    ''' To add new Adjust Item;
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="adjustID"></param>
    ''' <param name="stockItemID"></param>
    ''' <param name="loginUser"></param>
    ''' <param name="status">create as Closed</param>
    ''' <returns>Identity for the new record</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function InsertAdjustItem(ByVal storeID As String _
                                            , ByVal adjustID As String _
                                            , ByVal stockItemID As String _
                                            , ByVal loginUser As String _
                                            , Optional ByVal status As String = "C" _
                                            , Optional ByVal adjustQty As Integer = 0 _
                                            , Optional ByVal remarks As String = "" _
                                            ) As Integer
        Try
            Dim sqlStoredProc As String = "spInsertAdjustItem"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strAdjustID", DbType.String, adjustID)
                    db.AddOutParameter(dbCommand, "@intAdjustItemID", DbType.Int32, 4)
                    db.AddInParameter(dbCommand, "@strStockItemID", DbType.String, stockItemID)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                    db.AddInParameter(dbCommand, "@intAdjustQty", DbType.Int32, adjustQty)
                    db.AddInParameter(dbCommand, "@strRemarks", DbType.String, remarks)

                    'If db.ExecuteNonQuery(dbCommand) < 1 Then Throw New Exception(String.Format("Error: [{0}][{1}][{2}][{3}] Record not inserted", storeID, loginUser, "adjustID:" + adjustID, stockItemID))
                    db.ExecuteNonQuery(dbCommand)

                    Return CInt(dbCommand.Parameters("@intAdjustItemID").Value)
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try
    End Function

    ''' <summary>
    ''' Update Adjust;
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="adjustType">Inward or Outward</param>
    ''' <param name="adjustID"></param>
    ''' <param name="serialNo"></param>
    ''' <param name="involveID"></param>
    ''' <param name="loginUser"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Sub UpdateAdjust(ByVal storeID As String _
                                   , ByVal adjustID As String _
                                   , ByVal adjustType As String _
                                   , ByVal serialNo As String _
                                   , ByVal involveID As String _
                                   , ByVal adjustDte As String _
                                   , ByVal loginUser As String _
                                   , ByVal returnUser As String _
                                   , ByVal returnDte As Date _
                                   , ByVal approveUser As String _
                                   , ByVal approveDte As Date _
                                   , ByVal receiveUser As String _
                                   , ByVal receiveDte As Date _
                                   )
        Try
            Dim sqlStoredProc As String = "spUpdateAdjust"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strAdjustID", DbType.String, adjustID)
                    db.AddInParameter(dbCommand, "@strType", DbType.String, adjustType)
                    db.AddInParameter(dbCommand, "@strSerialNo", DbType.String, serialNo)
                    db.AddInParameter(dbCommand, "@strInvolveID", DbType.String, involveID)
                    If adjustDte = Date.MinValue Then
                        db.AddInParameter(dbCommand, "@dteAdjustDte", DbType.Date, DBNull.Value)
                    Else
                        db.AddInParameter(dbCommand, "@dteAdjustDte", DbType.Date, adjustDte)
                    End If
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                    db.AddInParameter(dbCommand, "@strReturnUser", DbType.String, returnUser)
                    If returnDte = Date.MinValue Then
                        db.AddInParameter(dbCommand, "@dteReturnDte ", DbType.Date, DBNull.Value)
                    Else
                        db.AddInParameter(dbCommand, "@dteReturnDte", DbType.Date, returnDte)
                    End If

                    db.AddInParameter(dbCommand, "@strApproveUser", DbType.String, approveUser)
                    If approveDte = Date.MinValue Then
                        db.AddInParameter(dbCommand, "@dteApproveDte", DbType.Date, DBNull.Value)
                    Else
                        db.AddInParameter(dbCommand, "@dteApproveDte", DbType.Date, approveDte)
                    End If

                    db.AddInParameter(dbCommand, "@strReceiveUser", DbType.String, receiveUser)
                    If receiveDte = Date.MinValue Then
                        db.AddInParameter(dbCommand, "@dteReceiveDte", DbType.Date, DBNull.Value)
                    Else
                        db.AddInParameter(dbCommand, "@dteReceiveDte", DbType.Date, receiveDte)
                    End If

                    db.ExecuteNonQuery(dbCommand)
                    'If db.ExecuteNonQuery(dbCommand) < 1 Then Throw New Exception(String.Format("Error: [{0}][{1}][{2}] Record not updated", storeID, loginUser, "adjustID:" + adjustID))
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    ''' <summary>
    ''' Delete Adjust and all its Adjust Item and Transaction record;
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="adjustType">Inward or Outward</param>
    ''' <param name="adjustID"></param>
    ''' <param name="loginUser"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Sub DeleteAdjust(ByVal storeID As String _
                                   , ByVal adjustID As String _
                                   , ByVal adjustType As String _
                                   , ByVal loginUser As String _
                                   )
        Try
            Dim sqlStoredProc As String = "spDeleteAdjust"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strAdjustID", DbType.String, adjustID)
                    db.AddInParameter(dbCommand, "@strType", DbType.String, adjustType)
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                    db.ExecuteNonQuery(dbCommand)
                    'If db.ExecuteNonQuery(dbCommand) < 1 Then Throw New Exception(String.Format("Error: [{0}][{1}][{2}] Record not deleted", storeID, loginUser, "adjustID:" + adjustID))
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    ''' <summary>
    ''' Delete Adjust Item;
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="adjustID"></param>
    ''' <param name="adjustItemID"></param>
    ''' <param name="loginUser"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Sub DeleteAdjustItem(ByVal storeID As String _
                                       , ByVal adjustID As String _
                                       , ByVal adjustItemID As Integer _
                                       , ByVal loginUser As String _
                                       )
        Try
            Dim sqlStoredProc As String = "spDeleteAdjustItem"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strAdjustID", DbType.String, adjustID)
                    db.AddInParameter(dbCommand, "@strAdjustItemID", DbType.Int32, adjustItemID)
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                    db.ExecuteNonQuery(dbCommand)
                    'If db.ExecuteNonQuery(dbCommand) < 1 Then Throw New Exception(String.Format("Error: [{0}][{1}][{2}][{3}] Record not deleted", storeID, loginUser, adjustID, "adjustItemID:" + adjustItemID))
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    ''' <summary>
    ''' Get Adjust based on Parameters;
    ''' when adjustID is with value, ignore other parameters and get only adjust items relates to the single Document No
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="adjustType">Inward or Outward</param>
    ''' <param name="fromDte"></param>
    ''' <param name="toDte"></param>
    ''' <param name="adjustID">overwrite other parameter and return value for this adjust ID only</param>
    ''' <returns>Receive DataSet</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetAdjustList(ByVal storeID As String _
                                        , ByVal adjustType As String _
                                        , Optional ByVal fromDte As Date = #12:00:00 AM# _
                                        , Optional ByVal toDte As Date = #12:00:00 AM# _
                                        , Optional ByVal adjustID As String = "" _
                                        ) As DataSet
        Dim List As New DataSet

        Try
            Dim sqlStoredProc As String = "spSelectAdjustList"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strType", DbType.String, adjustType)
                    If fromDte = Date.MinValue Then
                        db.AddInParameter(dbCommand, "@dteFromDte", DbType.Date, DBNull.Value)
                    Else
                        db.AddInParameter(dbCommand, "@dteFromDte", DbType.Date, fromDte)
                    End If
                    If toDte = Date.MinValue Then
                        db.AddInParameter(dbCommand, "@dteToDte", DbType.Date, DBNull.Value)
                    Else
                        db.AddInParameter(dbCommand, "@dteToDte", DbType.Date, toDte)
                    End If
                    db.AddInParameter(dbCommand, "@strAdjustID", DbType.String, adjustID)

                    List.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "GetAdjustList")
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try

        Return List
    End Function

#End Region

#Region " Not in Used "

    ''' <summary>
    ''' OBSOLETE! replace by [FieldIsUnique]
    ''' Check Gebiz PO No, either for new or existing Order, if found return TRUE;
    ''' 05Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="gebizPONo"></param>
    ''' <returns>True = Found; False = Not Found</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function CheckGebizPONo(ByVal storeID As String _
                                          , ByVal gebizPONo As String _
                                          , Optional ByVal orderID As String = "" _
                                          ) As Boolean

        Try
            Dim sqlStoredProc As String = "spCheckGebizPONo"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strOrderID", DbType.String, orderID)
                    db.AddInParameter(dbCommand, "@strGebizPONo", DbType.String, gebizPONo)

                    Return CBool(db.ExecuteNonQuery(dbCommand) > 0)
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try
    End Function

    ''' <summary>
    ''' OBSOLETE! can't remember why i wrote this
    ''' Get Balance Qty of a StockItemID at a specific date;
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="stockItemID"></param>
    ''' <param name="asOfDate"></param>
    ''' <returns>Balance Qty</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function getBalanceQty(ByVal storeID As String _
                                          , ByVal stockItemID As String _
                                          , Optional ByVal asOfDate As Date = #12:00:00 AM# _
                                          ) As Decimal
        Try
            Dim sqlStoredProc As String = "spGetBalanceQty"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strSerialNo", DbType.String, stockItemID)
                    db.AddInParameter(dbCommand, "@strTableName", DbType.String, asOfDate)

                    Return CDec(db.ExecuteScalar(dbCommand))
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try
    End Function

    ''' <summary>
    ''' OBSOLETE! REPLACED BY [FieldIsUnique]
    ''' Check Serial No, either for new or existing based on the given table, if found return TRUE;
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="serialNo"></param>
    ''' <param name="tableName"></param>
    ''' <param name="recordID">to exclude check for existing record</param>
    ''' <returns>True = Found; False = Not Found</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function CheckSerialNo(ByVal storeID As String _
                                          , ByVal serialNo As String _
                                          , ByVal tableName As String _
                                          , Optional ByVal recordID As String = "" _
                                          ) As Boolean

        Try
            Dim sqlStoredProc As String = "spCheckSerialNoByTable"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strSerialNo", DbType.String, serialNo)
                    db.AddInParameter(dbCommand, "@strTableName", DbType.String, tableName)
                    db.AddInParameter(dbCommand, "@strRecordID", DbType.String, recordID)

                    Return CBool(db.ExecuteNonQuery(dbCommand) > 0)
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try
    End Function

    ''' <summary>
    ''' OBSOLETE!
    ''' Delete all stock transaction for a single document Reference (receive, issue, adjustment) on a specific date;
    ''' 11Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="docNo"></param>
    ''' <param name="type">the 1st character will tell the related table</param>
    ''' <param name="dte"></param>
    ''' <param name="loginUser"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Sub DeleteAllStockTransaction(ByVal storeID As String _
                                                , ByVal docNo As String _
                                                , ByVal type As String _
                                                , ByVal dte As Date _
                                                , ByVal loginUser As String _
                                                )
        Try
            Dim sqlStoredProc As String = "spDeleteAllStockTransaction"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strDocNo", DbType.String, docNo)
                    db.AddInParameter(dbCommand, "@strType", DbType.String, type)
                    db.AddInParameter(dbCommand, "@dteTranDte", DbType.Date, dte)
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                    db.ExecuteNonQuery(dbCommand)
                    'If db.ExecuteNonQuery(dbCommand) < 1 Then Throw New Exception(String.Format("Error: [{0}][{1}][{2}][{3}][{4}] Record not deleted", storeID, loginUser, docNo, type, CStr(dte)))
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    ''' <summary>
    ''' OBSOLETE!
    ''' Update Request Status to either Approved or Rejected;
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Sub UpdateRequestStatus(ByVal storeID As String _
                                          )
        '    Try
        '        Dim sqlStoredProc As String = "spUpdateRequestStatus"
        '        Dim db As Database = DatabaseFactory.CreateDatabase()
        '        Using conn As DbConnection = db.CreateConnection()
        '            Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
        '                db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
        '                db.AddInParameter(dbCommand, "@strRequestID", DbType.String, requestID)
        '                db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
        '                db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

        '                If db.ExecuteNonQuery(dbCommand) < 1 Then Throw New Exception(String.Format("Error: [{0}][{1}][{2}][{3}] Record not updated", storeID, loginUser, requestID, "request status:" + status))
        '            End Using
        '        End Using

        '    Catch ex As Exception
        '        Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
        '        If (rethrow) Then Throw
        '    End Try
    End Sub

#End Region
End Class
