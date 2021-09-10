Imports NEA_ICS.DataAccess
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

''' <summary>
''' Business Layer - for Access Rights to ICS
''' 16 Feb 09 - Jianfa;
''' </summary>
''' <remarks></remarks>
Public Class AccessRightsBL

#Region " Access Rights "

    ''' <summary>
    ''' Function - GetStoreAccess;
    ''' 16 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="userID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetStoreAccess(ByVal userID As String) As DataSet

        Dim StoreAccess As New DataSet

        Try

            StoreAccess = AccessRightsDAL.GetStoreAccess(userID)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return StoreAccess

    End Function

    ''' <summary>
    ''' Function - GetUserProfile;
    ''' 16 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="userID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetUserProfile(ByVal storeID As String, ByVal userID As String) As DataSet

        Dim UserProfile As New DataSet

        Try

            UserProfile = AccessRightsDAL.GetUserProfile(storeID, userID)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return UserProfile

    End Function


    ''' <summary>
    ''' Function - GetUserStoreCodes;
    ''' 20 Sep 09 - Christina;
    ''' </summary>
    ''' <param name="userId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetUserStoreCodes(ByVal userId As String) As String
        Dim storeCodes As String = ""
        Try
            storeCodes = AccessRightsDAL.GetUserStoreCodes(userId)
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return storeCodes
    End Function

    ''' <summary>
    ''' Function - GetUserLogins;
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

            NumLogins = AccessRightsDAL.GetUserLogins(userID, sessionId, checkIfLogout)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
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
        Dim exist As Boolean = False

        Try
            exist = AccessRightsDAL.CheckUserIdExist(userID)
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return exist
    End Function

    ''' <summary>
    ''' Check User Roles status - inactive or deleted
    ''' Christina - 02 Sep 2013
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="userId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetUserRoleStatus(ByVal storeID As String, ByVal userId As String) As DataSet

        Dim UserRoleStatus As New DataSet
        Try

            UserRoleStatus = AccessRightsDAL.GetUserRoleStatus(storeID, userId)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return UserRoleStatus

    End Function

    ''' <summary>
    ''' Function - GetUserActivityList;
    ''' 11 Sept 13 - Christina;
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

            UserActivity = AccessRightsDAL.GetUserActivityList(storeID, userId, fromDte, toDte, byTimeStamp)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
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

            UnsuccessfulLogins = AccessRightsDAL.GetUserUnsuccessfulLoginList(storeID, userId, fromDte, toDte)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
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

            NewUserAccounts = AccessRightsDAL.GetNewUserAccountList(storeID, fromDte, toDte)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
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

            NonIcsAccounts = AccessRightsDAL.GetNonIcsUserUnsuccessfulLogin(storeID, fromDte, toDte)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return NonIcsAccounts
    End Function

    ''' <summary>
    ''' Function - GetInactiveUsers;
    ''' 11 Sept 13 - Christina;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetInactiveUsers(ByVal storeId As String, ByVal datefrom As DateTime, ByVal dateto As DateTime) As DataSet

        Dim InactiveUSers As New DataSet
        Try

            InactiveUSers = AccessRightsDAL.GetInactiveUsers(storeId, datefrom, dateto)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
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

        Dim LastLoginDate As New DateTime
        Try

            LastLoginDate = AccessRightsDAL.GetLastLoginDate(storeID, userId)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return LastLoginDate
    End Function

    ''' <summary>
    ''' Function - AddUserAudit;
    ''' 02 Sept 13 - Christina;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="userID"></param>
    ''' <param name="userIP"></param>
    ''' <param name="isNonIcsUser"></param>
    ''' <param name="isInactiveUser"></param>
    ''' <param name="isUnsuccessfulLogin"></param>
    ''' <remarks></remarks>
    Public Shared Function AddUserAudit(ByVal storeID As String, ByVal userID As String _
                                         , ByVal userIP As String, ByVal sessionID As String _
                                         , Optional ByVal isNonIcsUser As Boolean = False _
                                         , Optional ByVal isInactiveUser As Boolean = False _
                                         , Optional ByVal isUnsuccessfulLogin As Boolean = False) As String

        Dim errorMessage As String = String.Empty

        Try

            AccessRightsDAL.InsertUserAudit(storeID, userID, userIP, sessionID, isNonIcsUser, isInactiveUser, isUnsuccessfulLogin)

        Catch ex As ApplicationException

            errorMessage = "Error: User Audit details was not inserted"
            Return errorMessage

        Catch ex As Exception
            Dim strFile As String = "C:\\inetpub\\wwwroot\\ICS\\IcsLog.txt"
            Dim bAns As Boolean = False
            Dim objReader As IO.StreamWriter

            objReader = New IO.StreamWriter(strFile)
            objReader.Write(errorMessage)
            objReader.Close()
            bAns = True
        End Try

        Return errorMessage

    End Function

    ''' <summary>
    ''' Function - UpdateUserAudit
    ''' 16 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="userID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateUserAudit(ByVal storeID As String, ByVal userID As String, ByVal sessionID As String) As String

        Dim errorMessage As String = String.Empty

        Try

            AccessRightsDAL.UpdateUserAudit(storeID, userID, sessionID)

        Catch ex As ApplicationException

            errorMessage = "Error: User Audit details was not updated"
            Return errorMessage

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return errorMessage

    End Function

    ''' <summary>
    ''' Manage Financial Closing, update past finanical cutoff date record details;
    ''' 28Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="loginUser"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Sub ManageFinancialClosing(ByVal storeID As String, ByVal loginUser As String)
        Dim ClosingDte As Date = Date.MinValue

        ' When today is not within financial cutoff date, process to close last month and prior transactions
        If Not StockControlDAL.WithinFinancialCutoff(storeID _
                                                     , Today _
                                                     ) Then
            ' set the last day of last month as the closing date
            ClosingDte = CDate(Today.ToString("yyyy/MM/01")).AddDays(-1)
            AccessRightsDAL.UpdateBalanceTransaction(storeID _
                                                     , ClosingDte _
                                                     , loginUser _
                                                     )
        End If
    End Sub

    ''' <summary>
    ''' Update user status to inactive if user last activity is greater than or equal to 90 days, if yes deactivate user;
    ''' 14 Sept 2013 - Christina;
    ''' </summary>
    ''' <param name="storeID"></param>
    Public Shared Function ManageInactiveUser(ByVal storeID As String) As String
        Dim errorMessage As String = String.Empty

        Try
            AccessRightsDAL.ManageInactiveUser(storeID)
        Catch ex As ApplicationException
            errorMessage = "Error: User Status to Inactive was not updated"
            Return errorMessage

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try
        Return errorMessage
    End Function

#End Region

End Class
