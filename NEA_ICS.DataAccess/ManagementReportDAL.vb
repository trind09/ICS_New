Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

''' <summary>
''' Data Access Layer - for Management Report
''' 29Dec08, Liu Guo Feng
''' </summary>
''' <remarks>
''' CHANGE LOG:
''' 29Dec08  Guo Feng RefID  Include the Exception Handling to all methods;
''' 29Dec08  Guo Feng RefID  use a separate SP for Get RackItemBalance by its code Group;
''' </remarks>
Public Class ManagementReportDAL

    ''' <summary>
    ''' Get RackLocation;
    ''' 29Dec08, KG
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <returns>RackLocation DataSet</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetRackLocation(ByVal storeId As String) As DataSet

        Dim RackLocations As New DataSet
        Try
            Dim sqlStoredProc As String = "spGetRackLocation"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)

                    RackLocations.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "RackLocation")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return RackLocations

    End Function

    ''' <summary>
    ''' Get StockCode;
    ''' 29Dec08, Guo Feng
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <returns>StockCode DataSet</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetStockCode(ByVal storeId As String) As DataSet

        Dim StockCodes As New DataSet
        Try
            Dim sqlStoredProc As String = "spGetStockCode"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)

                    StockCodes.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "StockCode")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return StockCodes

    End Function

    ''' <summary>
    ''' Get Suppliers based on Parameters;
    ''' 12Dec08, KG
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="rackLocationFrom"></param>
    ''' <param name="rackLocationTo"></param>
    ''' <returns>Suppliers DataSet</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetMR001GetRackItemBalance(ByVal storeId As String, ByVal rackLocationFrom As String, ByVal rackLocationTo As String) As DataSet

        Dim ReturnLists As New DataSet
        Try
            Dim sqlStoredProc As String = "spMR001GetRackItemBalance"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
                    db.AddInParameter(dbCommand, "@RackLocationFrom", DbType.String, rackLocationFrom)
                    db.AddInParameter(dbCommand, "@RackLocationTo", DbType.String, rackLocationTo)

                    ReturnLists.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "ReturnLists")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ReturnLists

    End Function
    ''' <summary>
    ''' Get TransactionList based on Parameters;
    ''' 30Dec08, Guo Feng
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="stockCodeFrom"></param>
    ''' <param name="stockCodeTo"></param>
    ''' <param name="transDateFrom"></param>
    ''' <param name="transDateTo"></param>
    ''' <returns>TransactionList DataSet</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetMR002GetTransactionList(ByVal storeId As String, ByVal stockCodeFrom As String, ByVal stockCodeTo As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime, ByVal equipmentID As String, Optional isECSelected As Boolean = False) As DataSet

        Dim ReturnLists As New DataSet
        Try
            Dim sqlStoredProc As String = "spMR002GetTransactionList"
            If isECSelected = True Then
                sqlStoredProc = "spMR002GetTransactionListEC"
            End If
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
                    db.AddInParameter(dbCommand, "@strStockCodeFrom", DbType.String, stockCodeFrom)
                    db.AddInParameter(dbCommand, "@strStockCodeTo", DbType.String, stockCodeTo)
                    'db.AddInParameter(dbCommand, "@dateTransFrom", DbType.Date, transDateFrom.Date)
                    'db.AddInParameter(dbCommand, "@dateTransTo", DbType.Date, transDateTo.Date)
                    db.AddInParameter(dbCommand, "@dateTransFrom", DbType.DateTime, transDateFrom)
                    db.AddInParameter(dbCommand, "@dateTransTo", DbType.DateTime, transDateTo)
                    db.AddInParameter(dbCommand, "@equipmentID", DbType.String, equipmentID)

                    ReturnLists.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "ReturnLists")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ReturnLists

    End Function
    Public Shared Function GetMR002GetDirectIssueList(ByVal storeId As String, ByVal stockCodeFrom As String, ByVal stockCodeTo As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime) As DataSet

        Dim ReturnLists As New DataSet
        Try
            Dim sqlStoredProc As String = "spMR002GetDirectIssueList"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
                    '-- CHANGE AMENDMENT: Direct ISSUE does not have any Stock Code
                    'db.AddInParameter(dbCommand, "@strStockCodeFrom", DbType.String, String.Empty)
                    'db.AddInParameter(dbCommand, "@strStockCodeTo", DbType.String, String.Empty)
                    db.AddInParameter(dbCommand, "@dateTransFrom", DbType.DateTime, transDateFrom)
                    db.AddInParameter(dbCommand, "@dateTransTo", DbType.DateTime, transDateTo)

                    ReturnLists.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "ReturnLists")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ReturnLists

    End Function
    Public Shared Function GetMR003DirectIssue(ByVal storeId As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime) As DataSet

        Dim ReturnLists As New DataSet
        Try
            Dim sqlStoredProc As String = "spMR003DirectIssue"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
                    db.AddInParameter(dbCommand, "@dateTransFrom", DbType.DateTime, transDateFrom)
                    db.AddInParameter(dbCommand, "@dateTransTo", DbType.DateTime, transDateTo)

                    ReturnLists.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "ReturnLists")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ReturnLists

    End Function
    Public Shared Function GetMR003IssueDocumentDetails(ByVal storeId As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime) As DataSet

        Dim ReturnLists As New DataSet
        Try
            Dim sqlStoredProc As String = "spMR003IssueDocumentDetails"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
                    db.AddInParameter(dbCommand, "@dateTransFrom", DbType.DateTime, transDateFrom)
                    db.AddInParameter(dbCommand, "@dateTransTo", DbType.DateTime, transDateTo)

                    ReturnLists.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "ReturnLists")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ReturnLists

    End Function
    Public Shared Function GetMR004StockReviewList(ByVal storeId As String, ByVal stockCodeFrom As String, ByVal stockCodeTo As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime, Optional isECSelected As Boolean = False) As DataSet

        Dim ReturnLists As New DataSet
        Try
            Dim sqlStoredProc As String = "spMR004StockReviewList"
            If isECSelected = True Then
                sqlStoredProc = "spMR004StockReviewListEC"
            End If
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
                    db.AddInParameter(dbCommand, "@strStockCodeFrom", DbType.String, stockCodeFrom)
                    db.AddInParameter(dbCommand, "@strStockCodeTo", DbType.String, stockCodeTo)
                    db.AddInParameter(dbCommand, "@dateTransFrom", DbType.DateTime, transDateFrom)
                    db.AddInParameter(dbCommand, "@dateTransTo", DbType.DateTime, transDateTo)

                    ReturnLists.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "ReturnLists")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ReturnLists

    End Function
    Public Shared Function GetMR005StockReturn(ByVal storeId As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime, Optional isECSelected As Boolean = False) As DataSet

        Dim ReturnLists As New DataSet
        Try
            Dim sqlStoredProc As String = "spMR005StockReturn"

            If isECSelected = True Then
                sqlStoredProc = "spMR005StockReturnEC"
            End If
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
                    db.AddInParameter(dbCommand, "@dateTransFrom", DbType.DateTime, transDateFrom)
                    db.AddInParameter(dbCommand, "@dateTransTo", DbType.DateTime, transDateTo)

                    ReturnLists.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "ReturnLists")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ReturnLists

    End Function
    Public Shared Function GetMR006StockReturnCheckListAdjust(ByVal storeId As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime, Optional isECSelected As Boolean = False) As DataSet

        Dim ReturnLists As New DataSet
        Try
            Dim sqlStoredProc As String = "spMR006StockReturnCheckListAdjust"

            If isECSelected = True Then
                sqlStoredProc = "spMR006StockReturnCheckListAdjustEC"
            End If

            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
                    db.AddInParameter(dbCommand, "@dateTransFrom", DbType.DateTime, transDateFrom)
                    db.AddInParameter(dbCommand, "@dateTransTo", DbType.DateTime, transDateTo)

                    ReturnLists.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "ReturnLists")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ReturnLists

    End Function
    Public Shared Function GetMR006StockReturnCheckListIssue(ByVal storeId As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime, Optional isECSelected As Boolean = False) As DataSet

        Dim ReturnLists As New DataSet
        Try
            Dim sqlStoredProc As String = "spMR006StockReturnCheckListIssue"
            If isECSelected = True Then
                sqlStoredProc = "spMR006StockReturnCheckListIssueEC"
            End If
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
                    db.AddInParameter(dbCommand, "@dateTransFrom", DbType.DateTime, transDateFrom)
                    db.AddInParameter(dbCommand, "@dateTransTo", DbType.DateTime, transDateTo)

                    ReturnLists.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "ReturnLists")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ReturnLists

    End Function
    Public Shared Function GetMR006StockReturnCheckListReceive(ByVal storeId As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime, Optional isECSelected As Boolean = False) As DataSet

        Dim ReturnLists As New DataSet
        Try
            Dim sqlStoredProc As String = "spMR006StockReturnCheckListReceive"
            If isECSelected = True Then
                sqlStoredProc = "spMR006StockReturnCheckListReceiveEC"
            End If
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
                    db.AddInParameter(dbCommand, "@dateTransFrom", DbType.DateTime, transDateFrom)
                    db.AddInParameter(dbCommand, "@dateTransTo", DbType.DateTime, transDateTo)

                    ReturnLists.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "ReturnLists")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ReturnLists

    End Function
    Public Shared Function GetMR006StockReturnCheckListStockItem(ByVal storeId As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime, ByVal excludeZero As Boolean, Optional isECSelected As Boolean = False) As DataSet

        Dim ReturnLists As New DataSet
        Try
            Dim sqlStoredProc As String = "spMR006StockReturnCheckListStockItem"
            If isECSelected = True Then
                sqlStoredProc = "spMR006StockReturnCheckListStockItemEC"
            End If
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
                    db.AddInParameter(dbCommand, "@dateTransFrom", DbType.DateTime, transDateFrom)
                    db.AddInParameter(dbCommand, "@dateTransTo", DbType.DateTime, transDateTo)
                    db.AddInParameter(dbCommand, "@excludeZero", DbType.Boolean, excludeZero)

                    ReturnLists.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "ReturnLists")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ReturnLists

    End Function
    Public Shared Function GetMR007PeriodIssues(ByVal storeId As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime, ByVal consumerID As String) As DataSet

        Dim ReturnLists As New DataSet
        Try
            Dim sqlStoredProc As String = "spMR007PeriodIssues"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
                    db.AddInParameter(dbCommand, "@dateTransFrom", DbType.DateTime, transDateFrom)
                    db.AddInParameter(dbCommand, "@dateTransTo", DbType.DateTime, transDateTo)
                    db.AddInParameter(dbCommand, "@consumerID", DbType.String, consumerID)

                    ReturnLists.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "ReturnLists")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ReturnLists

    End Function
    Public Shared Function GetMR007PeriodDirectIssues(ByVal storeId As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime, ByVal consumerID As String) As DataSet

        Dim ReturnLists As New DataSet
        Try
            Dim sqlStoredProc As String = "spMR007PeriodDirectIssues"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
                    db.AddInParameter(dbCommand, "@dateTransFrom", DbType.DateTime, transDateFrom)
                    db.AddInParameter(dbCommand, "@dateTransTo", DbType.DateTime, transDateTo)
                    db.AddInParameter(dbCommand, "@consumerID", DbType.String, consumerID)

                    ReturnLists.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "ReturnLists")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ReturnLists

    End Function
    Public Shared Function GetMR008QuantityIssueSummary(ByVal storeId As String, ByVal stockCodeFrom As String, ByVal stockCodeTo As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime, Optional isECSelected As Boolean = False) As DataSet

        Dim ReturnLists As New DataSet
        Try
            Dim sqlStoredProc As String = "spMR008QuantityIssueSummary"
            If isECSelected = True Then
                sqlStoredProc = "spMR008QuantityIssueSummaryEC"
            End If
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
                    db.AddInParameter(dbCommand, "@strStockCodeFrom", DbType.String, stockCodeFrom)
                    db.AddInParameter(dbCommand, "@strStockCodeTo", DbType.String, stockCodeTo)
                    db.AddInParameter(dbCommand, "@dateTransFrom", DbType.DateTime, transDateFrom)
                    db.AddInParameter(dbCommand, "@dateTransTo", DbType.DateTime, transDateTo)

                    ReturnLists.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "ReturnLists")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ReturnLists

    End Function
    Public Shared Function GetMR009ReorderStockItemList(ByVal storeId As String, ByVal stockCodeFrom As String, ByVal stockCodeTo As String, Optional isECSelected As Boolean = False) As DataSet

        Dim ReturnLists As New DataSet
        Try
            Dim sqlStoredProc As String = "spMR009ReorderStockItemList"

            If isECSelected = True Then
                sqlStoredProc = "spMR009ReorderStockItemListEC"
            End If

            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
                    db.AddInParameter(dbCommand, "@strStockCodeFrom", DbType.String, stockCodeFrom)
                    db.AddInParameter(dbCommand, "@strStockCodeTo", DbType.String, stockCodeTo)

                    ReturnLists.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "ReturnLists")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ReturnLists

    End Function
    Public Shared Function GetMR010StockAdjustmentEntries(ByVal storeId As String, ByVal stockCodeFrom As String, ByVal stockCodeTo As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime, Optional isECSelected As Boolean = False) As DataSet

        Dim ReturnLists As New DataSet
        Try
            Dim sqlStoredProc As String = "spMR010StockAdjustmentEntries"

            If isECSelected = True Then
                sqlStoredProc = "spMR010StockAdjustmentEntriesEC"
            End If

            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
                    db.AddInParameter(dbCommand, "@strStockCodeFrom", DbType.String, stockCodeFrom)
                    db.AddInParameter(dbCommand, "@strStockCodeTo", DbType.String, stockCodeTo)
                    db.AddInParameter(dbCommand, "@dateTransFrom", DbType.DateTime, transDateFrom)
                    db.AddInParameter(dbCommand, "@dateTransTo", DbType.DateTime, transDateTo)

                    ReturnLists.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "ReturnLists")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ReturnLists

    End Function

#Region " Ad Hoc Reports "

    ''' <summary>
    ''' Function - GetAdHocReports;
    ''' 30 Mar 09 - Jianfa;
    ''' </summary>
    ''' <param name="SQL"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetAdHocReports(ByVal SQL As String, ByRef returnMessage As String) As DataSet

        Dim ReturnLists As New DataSet
        Try
            Dim sqlStoredProc As String = "spAdHocReports"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@SQL", DbType.String, SQL)

                    ReturnLists.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "ReturnLists")

                End Using
            End Using

        Catch ex As Exception

            returnmessage = "Unable to evaluate an expression. Please re-enter criteria again."

        End Try

        Return ReturnLists

    End Function

#End Region

End Class
