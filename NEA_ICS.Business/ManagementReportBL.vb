Imports NEA_ICS.DataAccess
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

''' <summary>
''' Business Layer - for Management Report;
''' 29 Dec 08 - Liu Guo Feng;
''' </summary>
''' <remarks>
''' CHANGE LOG:
''' ddMMMyy AuthorName RefID Description;
''' 29Dec08  Guo Feng               Create GetRackItemBalance;
''' 29Dec08  Guo Fng                 Create GetRackLocation;
''' </remarks>
Public Class ManagementReportBL

    ''' <summary>
    ''' Get StockCode;
    ''' 29Dec08, Guo Feng
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <returns>StockCode DataSet Collection</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetStockCode(ByVal storeId As String) As DataSet

        Dim StockCodes As New DataSet
        Try

            StockCodes = ManagementReportDAL.GetStockCode(storeId)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return StockCodes
    End Function

    ''' <summary>
    ''' Get RackLocation;
    ''' 29Dec08, Guo Feng
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <returns>RackLocation DataSet Collection</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetRackLocation(ByVal storeId As String) As DataSet

        Dim RackLocations As New DataSet
        Try

            RackLocations = ManagementReportDAL.GetRackLocation(storeId)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return RackLocations
    End Function

    ''' <summary>
    ''' Get RackItemBalance based on Parameters;
    ''' 29Dec08, Guo Feng
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="rackLocationFrom"></param>
    ''' <param name="rackLocationTo"></param>
    ''' <returns>RackItemBalance DataSet Collection</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetMR001GetRackItemBalance(ByVal storeId As String, ByVal rackLocationFrom As String, ByVal rackLocationTo As String) As DataSet

        Dim returnDS As New DataSet
        Try

            returnDS = ManagementReportDAL.GetMR001GetRackItemBalance(storeId, rackLocationFrom, rackLocationTo)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return returnDS
    End Function

    ''' <summary>
    ''' Get TransactionList based on Parameters;
    ''' 29Dec08, Guo Feng
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="stockCodeFrom"></param>
    ''' <param name="stockCodeTo"></param>
    ''' <param name="transDateFrom"></param>
    ''' <param name="transDateTo"></param>
    ''' <returns>TransactionList DataSet Collection</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetMR002GetTransactionList(ByVal storeId As String, ByVal stockCodeFrom As String, ByVal stockCodeTo As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime, ByVal equipmentID As String) As DataSet

        Dim returnDS As New DataSet
        Try

            returnDS = ManagementReportDAL.GetMR002GetTransactionList(storeId, stockCodeFrom, stockCodeTo, transDateFrom, transDateTo, equipmentID)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return returnDS
    End Function
    Public Shared Function GetMR002GetDirectIssueList(ByVal storeId As String, ByVal stockCodeFrom As String, ByVal stockCodeTo As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime) As DataSet

        Dim returnDS As New DataSet
        Try

            returnDS = ManagementReportDAL.GetMR002GetDirectIssueList(storeId, stockCodeFrom, stockCodeTo, transDateFrom, transDateTo)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return returnDS
    End Function
    Public Shared Function GetMR003DirectIssue(ByVal storeId As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime) As DataSet

        Dim returnDS As New DataSet
        Try

            returnDS = ManagementReportDAL.GetMR003DirectIssue(storeId, transDateFrom, transDateTo)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return returnDS
    End Function
    Public Shared Function GetMR003IssueDocumentDetails(ByVal storeId As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime) As DataSet

        Dim returnDS As New DataSet
        Try

            returnDS = ManagementReportDAL.GetMR003IssueDocumentDetails(storeId, transDateFrom, transDateTo)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return returnDS
    End Function
    Public Shared Function GetMR004StockReviewList(ByVal storeId As String, ByVal stockCodeFrom As String, ByVal stockCodeTo As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime) As DataSet

        Dim returnDS As New DataSet
        Try

            returnDS = ManagementReportDAL.GetMR004StockReviewList(storeId, stockCodeFrom, stockCodeTo, transDateFrom, transDateTo)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return returnDS
    End Function
    Public Shared Function GetMR005StockReturn(ByVal storeId As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime) As DataSet

        Dim returnDS As New DataSet
        Try

            returnDS = ManagementReportDAL.GetMR005StockReturn(storeId, transDateFrom, transDateTo)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return returnDS
    End Function
    Public Shared Function GetMR006StockReturnCheckListAdjust(ByVal storeId As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime) As DataSet

        Dim returnDS As New DataSet
        Try

            returnDS = ManagementReportDAL.GetMR006StockReturnCheckListAdjust(storeId, transDateFrom, transDateTo)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return returnDS
    End Function
    Public Shared Function GetMR006StockReturnCheckListIssue(ByVal storeId As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime) As DataSet

        Dim returnDS As New DataSet
        Try

            returnDS = ManagementReportDAL.GetMR006StockReturnCheckListIssue(storeId, transDateFrom, transDateTo)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return returnDS
    End Function
    Public Shared Function GetMR006StockReturnCheckListReceive(ByVal storeId As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime) As DataSet

        Dim returnDS As New DataSet
        Try

            returnDS = ManagementReportDAL.GetMR006StockReturnCheckListReceive(storeId, transDateFrom, transDateTo)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return returnDS
    End Function
    Public Shared Function GetMR006StockReturnCheckListStockItem(ByVal storeId As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime, ByVal excludeZero As Boolean) As DataSet

        Dim returnDS As New DataSet
        Try

            returnDS = ManagementReportDAL.GetMR006StockReturnCheckListStockItem(storeId, transDateFrom, transDateTo, excludeZero)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return returnDS
    End Function
    Public Shared Function GetMR007PeriodIssues(ByVal storeId As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime, ByVal consumerID As String) As DataSet

        Dim returnDS As New DataSet
        Try

            returnDS = ManagementReportDAL.GetMR007PeriodIssues(storeId, transDateFrom, transDateTo, consumerID)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return returnDS
    End Function
    Public Shared Function GetMR007PeriodDirectIssues(ByVal storeId As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime, ByVal consumerID As String) As DataSet

        Dim returnDS As New DataSet
        Try

            returnDS = ManagementReportDAL.GetMR007PeriodDirectIssues(storeId, transDateFrom, transDateTo, consumerID)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return returnDS
    End Function
    Public Shared Function GetMR008QuantityIssueSummary(ByVal storeId As String, ByVal stockCodeFrom As String, ByVal stockCodeTo As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime) As DataSet

        Dim returnDS As New DataSet
        Try

            returnDS = ManagementReportDAL.GetMR008QuantityIssueSummary(storeId, stockCodeFrom, stockCodeTo, transDateFrom, transDateTo)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return returnDS
    End Function
    Public Shared Function GetMR009ReorderStockItemList(ByVal storeId As String, ByVal stockCodeFrom As String, ByVal stockCodeTo As String) As DataSet

        Dim returnDS As New DataSet
        Try

            returnDS = ManagementReportDAL.GetMR009ReorderStockItemList(storeId, stockCodeFrom, stockCodeTo)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return returnDS
    End Function
    Public Shared Function GetMR010StockAdjustmentEntries(ByVal storeId As String, ByVal stockCodeFrom As String, ByVal stockCodeTo As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime) As DataSet

        Dim returnDS As New DataSet
        Try

            returnDS = ManagementReportDAL.GetMR010StockAdjustmentEntries(storeId, stockCodeFrom, stockCodeTo, transDateFrom, transDateTo)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return returnDS
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

        Dim returnDS As New DataSet

        Try

            returnDS = ManagementReportDAL.GetAdHocReports(SQL, returnMessage)

        Catch ex As Exception

            returnMessage = "Unable to evaluate an expression. Please re-enter criteria again."

        End Try

        Return returnDS

    End Function

#End Region

End Class
