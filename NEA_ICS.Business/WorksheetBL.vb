Imports NEA_ICS.DataAccess
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

''' <summary>
''' Business Layer for WorkSheet
''' 30 Jan 09 - Jianfa;
''' </summary>
''' <remarks></remarks>
Public Class WorksheetBL

#Region " VERIFICATION WORKSHEET "

    ''' <summary>
    ''' Function - GetWorkSheetItems;
    ''' 30 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="stockCodeFrom"></param>
    ''' <param name="stockCodeTo"></param>
    ''' <param name="stockType"></param>
    ''' <param name="status"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetWorkSheetItems(ByVal storeID As String, ByVal stockCodeFrom As String, _
                                            ByVal stockCodeTo As String, ByVal stockType As String, _
                                            ByVal subType As String, _
                                            ByVal totalValue As Decimal, _
                                            ByVal status As String) As DataSet

        Dim WorkSheetItems As New DataSet

        Try

            WorkSheetItems = WorksheetDAL.GetWorksheetItem(storeID, stockCodeFrom, stockCodeTo, _
                                                           stockType, subType, totalValue, status)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return WorkSheetItems

    End Function

    ''' <summary>
    ''' Function - GetMarkedWorksheetItem
    ''' 12 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="worksheetID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetMarkedWorksheetItem(ByVal storeID As String, ByVal worksheetID As Integer) As DataSet

        Dim WorkSheetItems As New DataSet

        Try

            WorkSheetItems = WorksheetDAL.GetMarkedWorksheetItem(storeID, worksheetID)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return WorkSheetItems

    End Function

    ''' <summary>
    ''' Function - AddWorkSheetItem;
    ''' 3 Feb 2009 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="workSheetItemList"></param>
    ''' <param name="loginUser"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function AddWorkSheetItem(ByVal storeID As String, ByVal workSheetItemList As DataTable, _
                                            ByVal loginUser As String, ByRef workSheetID As Integer) As String

        Dim errorMessage As String = String.Empty

        Try

            workSheetID = WorksheetDAL.GenerateWorkSheetID(storeID)
            WorksheetDAL.InsertWorksheetItem(storeID, workSheetItemList, workSheetID, loginUser)

        Catch ex As ApplicationException

            errorMessage = "<br>Error: Worksheet Item details was not inserted"
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
    ''' Function - GetWorkSheetID;
    ''' 03 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetWorkSheetID(ByVal storeID As String)

        Dim WorkSheetID As Integer

        Try

            WorkSheetID = WorksheetDAL.GenerateWorkSheetID(storeID)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return WorkSheetID

    End Function

    ''' <summary>
    ''' Function - GetWorksheetGeneratedDate;
    ''' 07 Feb 2009 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="workSheetID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetWorksheetGeneratedDate(ByVal storeID As String, ByVal workSheetID As Integer) As String

        Dim GeneratedDate As String = String.Empty

        Try

            GeneratedDate = WorksheetDAL.GetWorksheetGeneratedDate(storeID, workSheetID)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
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
    ''' <param name="storeID"></param>
    ''' <param name="workSheetID"></param>
    ''' <param name="verifierName"></param>
    ''' <param name="loginUser"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateWorksheet(ByVal storeID As String, _
                                           ByVal workSheetID As Integer, ByVal verifierName As String, _
                                           ByVal checkerName As String, ByVal approverName As String, _
                                           ByVal loginUser As String) As String

        Dim errorMessage As String = String.Empty
        Dim found As Boolean

        Try

            found = WorksheetDAL.CheckWorkSheetID(storeID, workSheetID)

            If found Then

                WorksheetDAL.UpdateWorksheet(storeID, workSheetID, verifierName, checkerName, approverName, loginUser)

            Else

                errorMessage = "Worksheet Verification Reference No does not exists."

            End If

        Catch ex As ApplicationException

            errorMessage = "<br>Error: Worksheet was not updated"
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
    ''' Function - DeleteWorksheet;
    ''' 06 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="workSheetID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteWorksheet(ByVal storeID As String, ByVal workSheetID As Integer) As String

        Dim errorMessage As String = String.Empty
        Dim found As Boolean

        Try

            found = WorksheetDAL.CheckWorkSheetID(storeID, workSheetID)

            If found Then

                WorksheetDAL.DeleteWorksheet(storeID, workSheetID)

            Else

                errorMessage = "Worksheet Verification Reference No does not exists."

            End If

        Catch ex As ApplicationException

            errorMessage = "<br>Error: Worksheet was not deleted"
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
