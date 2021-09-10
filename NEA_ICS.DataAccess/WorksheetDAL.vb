Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

''' <summary>
''' Data Access Layer - for Worksheet
''' 30 Jan 09 - Jianfa;
''' </summary>
''' <remarks></remarks>
Public Class WorksheetDAL

    ''' <summary>
    ''' Function - GetWorksheetItem;
    ''' 30 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="stockCodeFrom"></param>
    ''' <param name="stockCodeTo"></param>
    ''' <param name="stockType"></param>
    ''' <param name="status"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetWorksheetItem(ByVal storeID As String, ByVal stockCodeFrom As String, _
                                            ByVal stockCodeTo As String, ByVal stockType As String, _
                                            ByVal subType As String, _
                                            ByVal totalValue As Decimal, _
                                            ByVal status As String) As DataSet

        Dim WorkSheetItem As New DataSet

        Try

            Dim sqlStoredProc As String = "spSelectWorksheetItem"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strStockCodeFrom", DbType.String, stockCodeFrom)
                    db.AddInParameter(dbCommand, "@strStockCodeTo", DbType.String, stockCodeTo)
                    db.AddInParameter(dbCommand, "@strStockType", DbType.String, stockType)
                    db.AddInParameter(dbCommand, "@strSubType", DbType.String, subType)
                    db.AddInParameter(dbCommand, "@dblTotalValue", DbType.Decimal, totalValue)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)

                    'WorkSheetItem.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "WorkSheetItem")
                    WorkSheetItem = db.ExecuteDataSet(dbCommand)

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return WorkSheetItem

    End Function

    ''' <summary>
    ''' Function - GetMarkedWorksheetItem;
    ''' 12 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="worksheetID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetMarkedWorksheetItem(ByVal storeID As String, ByVal worksheetID As Integer)

        Dim WorkSheetItem As New DataSet

        Try

            Dim sqlStoredProc As String = "spSelectMarkedWorksheetItem"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@intWorksheetID", DbType.String, worksheetID)

                    WorkSheetItem.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "WorkSheetItem")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return WorkSheetItem

    End Function

    ''' <summary>
    ''' Function - GenerateWorkSheetID;
    ''' 02 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GenerateWorkSheetID(ByVal storeID As String) As Integer

        Dim WorkSheetID As Integer

        Try

            Dim sqlStoredProc As String = "spGenerateWorksheetID"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeID)
                    WorkSheetID = db.ExecuteScalar(dbCommand)

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return WorkSheetID

    End Function

    ''' <summary>
    ''' Sub Proc - InsertWorkSheetItem;
    ''' 02 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="workSheetItemList"></param>
    ''' <remarks></remarks>
    Public Shared Sub InsertWorksheetItem(ByVal storeID As String, ByVal workSheetItemList As DataTable, _
                                          ByVal workSheetID As Integer, ByVal loginUser As String)

        Dim trans As DbTransaction
        Dim sqlStoredProc As String = "spInsertWorksheet"
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim conn As DbConnection = db.CreateConnection()

        conn.Open()
        trans = conn.BeginTransaction

        Try

            Dim dbCommand As DbCommand

            dbCommand = db.GetStoredProcCommand(sqlStoredProc)
            dbCommand.Transaction = trans

            db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
            db.AddInParameter(dbCommand, "@intWorksheetID", DbType.Int64, workSheetID)
            db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

            db.ExecuteNonQuery(dbCommand, trans)

            sqlStoredProc = "spInsertWorksheetItem"
            dbCommand.Parameters.Clear()

            dbCommand = db.GetStoredProcCommand(sqlStoredProc)

            For Each row As DataRow In workSheetItemList.Rows

                dbCommand.Parameters.Clear()
                db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                db.AddInParameter(dbCommand, "@intWorksheetID", DbType.Int64, workSheetID)
                db.AddInParameter(dbCommand, "@strItemID", DbType.String, row("WorksheetItemStockItemID"))
                db.AddInParameter(dbCommand, "@dblStockQty", DbType.Decimal, row("WorksheetItemQty"))
                db.AddInParameter(dbCommand, "@dblTotalCost", DbType.Decimal, row("WorksheetItemTotalCost"))
                db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                db.ExecuteNonQuery(dbCommand, trans)

            Next

            trans.Commit()
            conn.Close()

        Catch ex As Exception
            trans.Rollback()
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

    ''' <summary>
    ''' Function - GetWorksheetGeneratedDate;
    ''' 06 Feb 2009 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="workSheetID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetWorksheetGeneratedDate(ByVal storeID As String, ByVal workSheetID As Integer) As String

        Dim GeneratedDate As String = String.Empty

        Try

            Dim sqlStoredProc As String = "spSelectWorksheetGeneratedDate"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@intWorkSheetID", DbType.Int64, workSheetID)

                    GeneratedDate = IIf(IsDBNull(db.ExecuteScalar(dbCommand)), String.Empty, db.ExecuteScalar(dbCommand))

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return GeneratedDate

    End Function

    ''' <summary>
    ''' Function - CheckWorksheetID;
    ''' 05 Feb 2009 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="workSheetID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CheckWorkSheetID(ByVal storeID As String, ByVal workSheetID As Integer) As Boolean

        Try

            Dim sqlStoredProc As String = "spCheckWorksheetID"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@intWorkSheetID", DbType.Int64, workSheetID)

                    If db.ExecuteScalar(dbCommand) > 0 Then
                        Return True
                    Else
                        Return False
                    End If

                End Using
            End Using

            Return False

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

    ''' <summary>
    ''' Sub Proc - UpdateWorksheet;
    ''' 05 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="workSheetID"></param>
    ''' <param name="verifierName"></param>
    ''' <param name="loginUser"></param>
    ''' <remarks></remarks>
    Public Shared Sub UpdateWorksheet(ByVal storeID As String, _
                                      ByVal workSheetID As Integer, ByVal verifierName As String, _
                                      ByVal checkerName As String, ByVal approverName As String, _
                                      ByVal loginUser As String)

        Try

            Dim sqlStoredProc As String = "spUpdateWorksheet"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@intWorksheetID", DbType.Int64, workSheetID)
                    db.AddInParameter(dbCommand, "@strVerifierName", DbType.String, verifierName)
                    db.AddInParameter(dbCommand, "@strCheckerName", DbType.String, checkerName)
                    db.AddInParameter(dbCommand, "@strApproverName", DbType.String, approverName)
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                    db.ExecuteNonQuery(dbCommand)
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

    ''' <summary>
    ''' Sub Proc - DeleteWorksheet
    ''' 06 Feb 2009 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="worksheetID"></param>
    ''' <remarks></remarks>
    Public Shared Sub DeleteWorksheet(ByVal storeID As String, ByVal worksheetID As Integer)

        Dim trans As DbTransaction
        Dim sqlStoredProc As String = "spDeleteWorksheetItem"
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim conn As DbConnection = db.CreateConnection()

        conn.Open()
        trans = conn.BeginTransaction

        Try

            Dim dbCommand As DbCommand

            dbCommand = db.GetStoredProcCommand(sqlStoredProc)
            dbCommand.Transaction = trans

            db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
            db.AddInParameter(dbCommand, "@intWorksheetID", DbType.Int64, worksheetID)

            db.ExecuteNonQuery(dbCommand, trans)

            sqlStoredProc = "spDeleteWorksheet"
            dbCommand.Parameters.Clear()

            dbCommand = db.GetStoredProcCommand(sqlStoredProc)

            db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
            db.AddInParameter(dbCommand, "@intWorksheetID", DbType.Int64, worksheetID)

            db.ExecuteNonQuery(dbCommand, trans)

            trans.Commit()
            conn.Close()

        Catch ex As Exception
            trans.Rollback()
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

End Class
