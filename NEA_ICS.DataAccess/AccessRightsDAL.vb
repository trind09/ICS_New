Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

''' <summary>
''' Data Access Layer - for Access Rights to ICS
''' 16 Feb 09 - Jianfa
''' </summary>
''' <remarks></remarks>
Public Class AccessRightsDAL


#Region " ACCESS RIGHTS "

    ''' <summary>
    ''' Function - GetStoreAccess;
    ''' 16 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="UserID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetStoreAccess(ByVal UserID As String) As DataSet

        Dim StoreAccessRights As New DataSet

        Try

            Dim sqlStoredProc As String = "spSelectStoreAccess"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strUserID", DbType.String, UserID)

                    StoreAccessRights.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "StoreAccessRights")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return StoreAccessRights

    End Function

    ''' <summary>
    ''' Function - GetUserProfile
    ''' 16 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="userID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetUserProfile(ByVal storeID As String, ByVal userID As String) As DataSet
        Dim UserProfileStatus As New DataSet
        Try

            Dim sqlStoredProc As String = "spSelectUserProfile"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strUserID", DbType.String, userID)

                    UserProfileStatus.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "UserProfile")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return UserProfileStatus
    End Function

    ''' <summary>
    ''' Function - GetUserStoreCodes;
    ''' 20 Sep 09 - Christina;
    ''' </summary>
    ''' <param name="userId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetUserStoreCodes(ByVal userID As String) As String
        Dim storeCodes As String = ""
        Try

            Dim sqlStoredProc As String = "spSelectUserStores"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strUserId", DbType.String, userID)
                    storeCodes = Convert.ToString(db.ExecuteScalar(dbCommand))
                    Return storeCodes
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try
        Return storeCodes
    End Function

    ''' <summary>
    ''' Function - CheckConcurrentLogin;
    ''' 25 Aug 11 - Christina;
    ''' </summary>
    ''' <param name="userId"></param>
    ''' <param name="checkIfLogout"></param>
    ''' <param name="sessionId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetUserLogins(ByVal userID As String, ByVal sessionId As String, ByVal checkIfLogout As Boolean) As Integer

        Dim NumLogins As Integer

        Try

            Dim sqlStoredProc As String = "spCheckMultipleLogin"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strUserId", DbType.String, userID)
                    db.AddInParameter(dbCommand, "@strSessionId", DbType.String, sessionId)
                    db.AddInParameter(dbCommand, "@checkIfLogout", DbType.Boolean, checkIfLogout)
                    NumLogins = db.ExecuteScalar(dbCommand)
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return NumLogins

    End Function

    ''' <summary>
    ''' Check if such User ID exist
    ''' Christina - 02 Sep 2013
    ''' </summary>
    ''' <param name="userID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CheckUserIdExist(ByVal userID As String) As Boolean
        Try

            Dim sqlStoredProc As String = "spCheckValidUserID"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strUserId", DbType.String, userID)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, DBNull.Value)
                    db.AddInParameter(dbCommand, "@strRole", DbType.String, DBNull.Value)

                    If db.ExecuteScalar(dbCommand) > 0 Then
                        Return True
                    Else
                        Return False
                    End If
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    ''' <summary>
    ''' Check User Roles status - inactive or deleted
    ''' Christina - 02 Sep 2013
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="userID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetUserRoleStatus(ByVal storeID As String, ByVal userID As String) As DataSet

        Dim UserRoleStatus As New DataSet
        Try

            Dim sqlStoredProc As String = "spGetUserRoleStatus"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strUserID", DbType.String, userID)

                    UserRoleStatus.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "UserProfile")

                End Using
            End Using
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try
        Return UserRoleStatus
    End Function

    ''' <summary>
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="userId"></param>
    ''' <param name="fromDte"></param>
    ''' <param name="toDte"></param>
    ''' <param name="byTimeStamp"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetUserActivityList(ByVal storeID As String, ByVal userId As String, ByVal fromDte As DateTime, ByVal toDte As DateTime, ByVal byTimeStamp As Boolean) As DataSet

        Dim UserActivity As New DataSet

        Try

            Dim sqlStoredProc As String = "spAR001GetUserActivities"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strUserId", DbType.String, userId)
                    db.AddInParameter(dbCommand, "@dateTransFrom", DbType.Date, fromDte)
                    db.AddInParameter(dbCommand, "@dateTransTo", DbType.Date, toDte)
                    db.AddInParameter(dbCommand, "@byTimeStamp", DbType.Boolean, byTimeStamp)

                    UserActivity.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "UserActivity")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return UserActivity
    End Function

    ''' <summary>
    ''' Function - GetUserUnsuccessfulLoginList;
    ''' 11 Sept 13 - Christina;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="fromDte"></param>
    ''' <param name="toDte"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetUserUnsuccessfulLoginList(ByVal storeID As String, ByVal userId As String, ByVal fromDte As DateTime, ByVal toDte As DateTime) As DataSet

        Dim UnsuccessfulLogins As New DataSet
        Try

            Dim sqlStoredProc As String = "spAR002GetUserUnsuccessfulLogin"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strUserID", DbType.String, userId)
                    db.AddInParameter(dbCommand, "@dateTransFrom", DbType.Date, fromDte)
                    db.AddInParameter(dbCommand, "@dateTransTo", DbType.Date, toDte)

                    UnsuccessfulLogins.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "UnsuccessfulLogins")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return UnsuccessfulLogins
    End Function

    ''' <summary>
    ''' Function - GetNewUserAccountList;
    ''' 11 Sept 13 - Christina;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="fromDte"></param>
    ''' <param name="toDte"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetNewUserAccountList(ByVal storeID As String, ByVal fromDte As DateTime, ByVal toDte As DateTime) As DataSet

        Dim NewUserAccounts As New DataSet
        Try

            Dim sqlStoredProc As String = "spAR004GetNewUserCreated"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@dateTransFrom", DbType.Date, fromDte)
                    db.AddInParameter(dbCommand, "@dateTransTo", DbType.Date, toDte)

                    NewUserAccounts.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "UnsuccessfulLogins")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return NewUserAccounts
    End Function

    ''' <summary>
    ''' Function - GetNonIcsUserUnsuccessfulLogin;
    ''' 11 Sept 13 - Christina;
    ''' </summary>
    ''' <param name="fromDte"></param>
    ''' <param name="toDte"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetNonIcsUserUnsuccessfulLogin(ByVal storeID As String, ByVal fromDte As DateTime, ByVal toDte As DateTime) As DataSet

        Dim NonIcsAccounts As New DataSet
        Try

            Dim sqlStoredProc As String = "spAR003GetNonIcsUserUnsuccessfulLogin"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@dateTransFrom", DbType.Date, fromDte)
                    db.AddInParameter(dbCommand, "@dateTransTo", DbType.Date, toDte)

                    NonIcsAccounts.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "UnsuccessfulLogins")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return NonIcsAccounts
    End Function

    ''' <summary>
    ''' Function - Get Inactive ICS Users;
    ''' 11 Sept 13 - Christina;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="userId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetInactiveUsers(ByVal storeID As String, ByVal dateFrom As DateTime, ByVal dateTo As DateTime) As DataSet

        Dim InactiveUSers As New DataSet
        Try

            Dim sqlStoredProc As String = "spAR005GetInactiveUser"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@pdateFrom", DbType.DateTime, dateFrom)
                    db.AddInParameter(dbCommand, "@pdateTo", DbType.DateTime, dateTo)
                    'db.AddInParameter(dbCommand, "@strUserID", DbType.String, userId)
                    'db.AddInParameter(dbCommand, "@dateTransFrom", DbType.DateTime, fromDte)
                    'db.AddInParameter(dbCommand, "@dateTransTo", DbType.DateTime, toDte)

                    InactiveUSers.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "UnsuccessfulLogins")

                End Using
            End Using

        Catch ex As Exception
            Dim mm As String
            mm = ex.Message
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return InactiveUSers
    End Function

    ''' <summary>
    ''' Function - Get last login date of user;
    ''' 11 Sept 13 - Christina;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="userId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetLastLoginDate(ByVal storeID As String, ByVal userId As String) As DateTime
        Dim lastLoginDate As DateTime = DateTime.MinValue
        Try
            Dim sqlStoredProc As String = "spGetUserLastLogin"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strUserID", DbType.String, userId)

                    If Not db.ExecuteScalar(dbCommand) Is Nothing Then
                        Return Convert.ToDateTime(db.ExecuteScalar(dbCommand))
                    Else
                        Return lastLoginDate
                    End If
                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then Throw
        End Try
    End Function

    ''' <summary>
    ''' Sub Proc - InsertUserAudit
    ''' 02 Sept 13 - Christina;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="userID"></param>
    ''' <param name="userIP"></param>
    ''' <param name="isNonIcsUser"></param>
    ''' <param name="isInactiveUser"></param>
    ''' <param name="isUnsuccessfulLogin"></param>
    ''' <remarks></remarks>
    Public Shared Sub InsertUserAudit(ByVal storeID As String, ByVal userID As String _
                                      , ByVal userIP As String, ByVal sessionID As String _
                                      , Optional ByVal isNonIcsUser As Boolean = False _
                                      , Optional ByVal isInactiveUser As Boolean = False _
                                      , Optional ByVal isUnsuccessfulLogin As Boolean = False)

        Try

            Dim sqlStoredProc As String = "spInsertAUser"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strUserID", DbType.String, userID)
                    db.AddInParameter(dbCommand, "@strUserIP", DbType.String, userIP)
                    db.AddInParameter(dbCommand, "@strSessionID", DbType.String, sessionID)
                    db.AddInParameter(dbCommand, "@blnNonIcsUser", DbType.Boolean, isNonIcsUser)
                    db.AddInParameter(dbCommand, "@blnInactiveUser", DbType.Boolean, isInactiveUser)
                    db.AddInParameter(dbCommand, "@blnUnsuccessfulLogin", DbType.Boolean, isUnsuccessfulLogin)

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
    ''' Sub Proc - UpdateUserAudit;
    ''' 16 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="userID"></param>
    ''' <remarks></remarks>
    Public Shared Sub UpdateUserAudit(ByVal storeID As String, ByVal userID As String, ByVal sessionID As String)

        Try

            Dim sqlStoredProc As String = "spUpdateAUser"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strUserID", DbType.String, userID)
                    db.AddInParameter(dbCommand, "@strSessionId", DbType.String, sessionID)

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

    '''' <summary>
    '''' Check if user last activity is greater than or equal to 90 days, if yes deactivate user;
    '''' 14 Sept 2013 - Christina;
    '''' </summary>
    '''' <param name="storeID"></param>
    '''' <param name="userId"></param>
    '''' <returns>True = user is active; False = activity is >= 90 days </returns>
    'Public Shared Function IsUserHasInactivity(ByVal storeID As String, ByVal userId As String) As Boolean
    '    Try
    '        Dim sqlStoredProc As String = "spCheckUserLastActivity"
    '        Dim db As Database = DatabaseFactory.CreateDatabase()
    '        Using conn As DbConnection = db.CreateConnection()
    '            Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
    '                db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeID)
    '                db.AddInParameter(dbCommand, "@strUserId", DbType.Date, userId)

    '                Return CBool(db.ExecuteScalar(dbCommand))
    '            End Using
    '        End Using

    '    Catch ex As Exception
    '        Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
    '        If (rethrow) Then Throw
    '    End Try
    'End Function

    ''' <summary>
    ''' Update user to inactive whose last login > 90 days
    ''' 14 Sept 2013 - Christina;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="changeReason"></param>
    Public Shared Sub ManageInactiveUser(ByVal storeID As String)
        Dim trans As DbTransaction
        Dim sqlStoredProc As String = "spManageInactiveUser"
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim conn As DbConnection = db.CreateConnection()

        conn.Open()
        trans = conn.BeginTransaction
        Try

            Dim dbCommand As DbCommand
            dbCommand = db.GetStoredProcCommand(sqlStoredProc)
            dbCommand.Transaction = trans

            db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeID)
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

    ''' <summary>
    ''' Update Transactions' balance which has past its finanical cutoff date;
    ''' 28Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="loginUser"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Sub UpdateBalanceTransaction(ByVal storeID As String, ByVal tranDte As Date, ByVal loginUser As String)
        Try
            Dim sqlStoredProc As String = "spUpdateBalanceTransaction"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@storeID", DbType.String, storeID)
                    If tranDte = Date.MinValue Then
                        db.AddInParameter(dbCommand, "@balTransDate", DbType.Date, DBNull.Value)
                    Else
                        db.AddInParameter(dbCommand, "@balTransDate", DbType.Date, tranDte)
                    End If
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

#End Region


End Class
