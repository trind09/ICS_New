Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

''' <summary>
''' Data Access Layer - for Audit Trail Report
''' 4Jan08, Liu Guo Feng
''' </summary>
Public Class AuditTrailReportDAL

    ''' <summary>
    ''' Get Stock Item for Audit Trail based on Parameters;
    ''' 4Jan08, Liu Guo Feng
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="dateFrom"></param>
    ''' <param name="dateTo"></param>
    ''' <param name="auditType"></param>
    ''' <returns>DataSet</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetAuditTrailStockItem(ByVal storeId As String, ByVal dateFrom As DateTime, ByVal dateTo As DateTime, ByVal auditType As String, ByVal itemStatus As Boolean, ByVal orderBy As String) As DataSet

        Dim StockItems As New DataSet
        Try
            Dim sqlStoredProc As String = "spAR001StockItem"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@storeID", DbType.String, storeId)
                    db.AddInParameter(dbCommand, "@dateFrom", DbType.DateTime, dateFrom)
                    db.AddInParameter(dbCommand, "@dateTo", DbType.DateTime, dateTo)
                    db.AddInParameter(dbCommand, "@auditType", DbType.String, auditType)
                    db.AddInParameter(dbCommand, "@orderBy", DbType.String, orderBy)
                    db.AddInParameter(dbCommand, "@itemStatus", DbType.Boolean, itemStatus)

                    StockItems.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "StockItem")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return StockItems

    End Function

    ''' <summary>
    ''' Get Order for Audit Trail based on Parameters;
    ''' 4Jan08, Liu Guo Feng
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="dateFrom"></param>
    ''' <param name="dateTo"></param>
    ''' <param name="auditType"></param>
    ''' <returns>DataSet</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetAuditTrailOrder(ByVal storeId As String, ByVal dateFrom As DateTime, ByVal dateTo As DateTime, ByVal auditType As String, _
                                              ByVal orderBy As String) As DataSet

        Dim Orders As New DataSet
        Try
            Dim sqlStoredProc As String = "spAR002Order"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@storeID", DbType.String, storeId)
                    db.AddInParameter(dbCommand, "@dateFrom", DbType.DateTime, dateFrom)
                    db.AddInParameter(dbCommand, "@dateTo", DbType.DateTime, dateTo)
                    db.AddInParameter(dbCommand, "@auditType", DbType.String, auditType)
                    db.AddInParameter(dbCommand, "@orderBy", DbType.String, orderBy)

                    Orders.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "Order")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Orders

    End Function

    ''' <summary>
    ''' Get Item for Audit Trail Common table based on Parameters;
    ''' 11-sep-2016
    ''' </summary>
    ''' <param name="dateFrom"></param>
    ''' <param name="dateTo"></param>
    ''' <param name="Alldate"></param>
    ''' <returns>DataSet</returns>
    Public Shared Function GetAuditTrailACommon(ByVal storeID As String, ByVal dateFrom As DateTime, ByVal dateTo As DateTime) As DataSet

        Dim Commonds As New DataSet
        Try
            Dim sqlStoredProc As String = "spSelectACommonATReport"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@storeID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@pdateFrom", DbType.DateTime, dateFrom)
                    db.AddInParameter(dbCommand, "@pdateTo", DbType.DateTime, dateTo)

                    Commonds.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "ACommon")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Commonds

    End Function

    ''' <summary>
    ''' Get Item for Audit Trail AConsumer table based on Parameters;
    ''' 11-sep-2016
    ''' </summary>
    ''' <param name="dateFrom"></param>
    ''' <param name="dateTo"></param>
    ''' <param name="Alldate"></param>
    ''' <returns>DataSet</returns>
    Public Shared Function GetAuditTrailAConsumer(ByVal storeID As String, ByVal dateFrom As DateTime, ByVal dateTo As DateTime) As DataSet

        Dim Common As New DataSet
        Try
            Dim sqlStoredProc As String = "spSelectAConsumerATReport"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@storeID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@pdateFrom", DbType.DateTime, dateFrom)
                    db.AddInParameter(dbCommand, "@pdateTo", DbType.DateTime, dateTo)
                    'db.AddInParameter(dbCommand, "@palldate", DbType.Boolean, Alldate)

                    Common.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "AConsumer")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Common

    End Function

    ''' <summary>
    ''' Get Item for Audit Trail AEquipment table based on Parameters;
    ''' 11-sep-2016
    ''' </summary>
    ''' <param name="dateFrom"></param>
    ''' <param name="dateTo"></param>
    ''' <param name="Alldate"></param>
    ''' <returns>DataSet</returns>
    Public Shared Function GetAuditTrailAEquipment(ByVal storeID As String, ByVal dateFrom As DateTime, ByVal dateTo As DateTime) As DataSet

        Dim AEquipmentds As New DataSet
        Try
            Dim sqlStoredProc As String = "spSelectAEquipmentATReport"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@storeID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@pdateFrom", DbType.DateTime, dateFrom)
                    db.AddInParameter(dbCommand, "@pdateTo", DbType.DateTime, dateTo)


                    AEquipmentds.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "AEquipment")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return AEquipmentds

    End Function

    ''' <summary>
    ''' Get Item for Audit Trail Store table based on Parameters;
    ''' 11-sep-2016
    ''' </summary>
    ''' <param name="dateFrom"></param>
    ''' <param name="dateTo"></param>
    ''' <param name="Alldate"></param>
    ''' <returns>DataSet</returns>
    Public Shared Function GetAuditTrailAStore(ByVal storeID As String, ByVal dateFrom As DateTime, ByVal dateTo As DateTime) As DataSet

        Dim AStoreds As New DataSet
        Try
            Dim sqlStoredProc As String = "spSelectAStoreATReport"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@storeID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@pdateFrom", DbType.DateTime, dateFrom)
                    db.AddInParameter(dbCommand, "@pdateTo", DbType.DateTime, dateTo)


                    AStoreds.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "AStore")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return AStoreds

    End Function

    ''' <summary>
    ''' Get Item for Audit Trail Store table based on Parameters;
    ''' 11-sep-2016
    ''' </summary>
    ''' <param name="dateFrom"></param>
    ''' <param name="dateTo"></param>
    ''' <param name="Alldate"></param>
    ''' <returns>DataSet</returns>
    Public Shared Function GetAuditTrailASupplier(ByVal storeID As String, ByVal dateFrom As DateTime, ByVal dateTo As DateTime) As DataSet

        Dim ASupplierds As New DataSet
        Try
            Dim sqlStoredProc As String = "spSelectASupplierATReport"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@storeID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@pdateFrom", DbType.DateTime, dateFrom)
                    db.AddInParameter(dbCommand, "@pdateTo", DbType.DateTime, dateTo)


                    ASupplierds.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "ASupplier")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ASupplierds

    End Function

    ''' <summary>
    ''' Get Item for Audit Trail Store table based on Parameters;
    ''' 11-sep-2016
    ''' </summary>
    ''' <param name="dateFrom"></param>
    ''' <param name="dateTo"></param>
    ''' <param name="Alldate"></param>
    ''' <returns>DataSet</returns>
    Public Shared Function GetAuditTrailAUserRole(ByVal storeID As String, ByVal dateFrom As DateTime, ByVal dateTo As DateTime) As DataSet

        Dim AUserRoleds As New DataSet
        Try
            Dim sqlStoredProc As String = "spSelectAUserRoleATReport"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@storeID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@pdateFrom", DbType.DateTime, dateFrom)
                    db.AddInParameter(dbCommand, "@pdateTo", DbType.DateTime, dateTo)


                    AUserRoleds.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "AUserRole")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return AUserRoleds

    End Function
    ''' <summary>
    ''' Get Stock Transaction for Audit Trail based on Parameters;
    ''' 4Jan08, Liu Guo Feng
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="dateFrom"></param>
    ''' <param name="dateTo"></param>
    ''' <param name="auditType"></param>
    ''' <param name="transType"></param>
    ''' <returns>DataSet</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetAuditTrailStockTransaction(ByVal storeId As String, ByVal dateFrom As DateTime, ByVal dateTo As DateTime, ByVal auditType As String, ByVal transType As String, ByVal orderBy As String) As DataSet

        Dim StockTransactions As New DataSet
        Try
            Dim sqlStoredProc As String = "spAR003StockTransaction"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@storeID", DbType.String, storeId)
                    db.AddInParameter(dbCommand, "@dateFrom", DbType.DateTime, dateFrom)
                    db.AddInParameter(dbCommand, "@dateTo", DbType.DateTime, dateTo)
                    db.AddInParameter(dbCommand, "@auditType", DbType.String, auditType)
                    db.AddInParameter(dbCommand, "@transType", DbType.String, transType)
                    db.AddInParameter(dbCommand, "@orderBy", DbType.String, orderBy)

                    StockTransactions.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "StockTransaction")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return StockTransactions

    End Function

    ''' <summary>
    ''' Get Audit Trail Report for Residue Cost Adjustment
    ''' 18 Oct 2010 - Jianfa
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="month"></param>
    ''' <param name="year"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetAuditTrailResidue(ByVal storeID As String, ByVal month As Integer, ByVal year As Integer) As DataSet

        Dim ResidueCostAdjustments As New DataSet
        Try
            Dim sqlStoredProc As String = "spAR004ResidueCostAdj"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@storeID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@month", DbType.Int32, month)
                    db.AddInParameter(dbCommand, "@year", DbType.Int32, year)

                    ResidueCostAdjustments.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "ResidueCostsAdjustments")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ResidueCostAdjustments

    End Function

End Class
