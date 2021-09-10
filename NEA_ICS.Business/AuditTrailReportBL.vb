Imports NEA_ICS.DataAccess
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

''' <summary>
''' Business Layer - for Audit Trail Report;
''' 4Jan2009 - Liu Guo Feng;
''' </summary>
Public Class AuditTrailReportBL

    ''' <summary>
    ''' Get Stock Item based on Parameters;
    ''' 4Jan2009, Guo Feng
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="dateFrom"></param>
    ''' <param name="dateTo"></param>
    ''' <param name="auditType"></param>
    ''' <returns>StockItem DataSet Collection</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetAuditTrailStockItem(ByVal storeId As String, ByVal dateFrom As DateTime, ByVal dateTo As DateTime, ByVal auditType As String, ByVal itemStatus As Boolean, ByVal orderBy As String) As DataSet

        Dim StockItems As New DataSet
        Try

            StockItems = AuditTrailReportDAL.GetAuditTrailStockItem(storeId, dateFrom, dateTo, auditType, itemStatus, orderBy)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return StockItems
    End Function

    ''' <summary>
    ''' Get Order based on Parameters;
    ''' 4Jan2009, Guo Feng
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="dateFrom"></param>
    ''' <param name="dateTo"></param>
    ''' <param name="auditType"></param>
    ''' <returns>Order DataSet Collection</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetAuditTrailOrder(ByVal storeId As String, ByVal dateFrom As DateTime, ByVal dateTo As DateTime, ByVal auditType As String, _
                                              ByVal orderBy As String) As DataSet

        Dim Orders As New DataSet
        Try

            Orders = AuditTrailReportDAL.GetAuditTrailOrder(storeId, dateFrom, dateTo, auditType, orderBy)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Orders
    End Function

    ''' <summary>
    ''' Get common val pritpalksaur
    ''' </summary>
    ''' <param name="dateFrom"></param>
    ''' <param name="dateTo"></param>
    ''' <param name="Alldate"></param>
    ''' <returns>Order DataSet Collection</returns>
    Public Shared Function GetAuditTrailACommon(ByVal storeId As String, ByVal datefrom As DateTime, ByVal dateto As DateTime)
        Dim common As New DataSet
        Try

            common = AuditTrailReportDAL.GetAuditTrailACommon(storeId, datefrom, dateto)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return common
    End Function

    ''' <summary>
    ''' Get common val pritpalksaur
    ''' </summary>
    ''' <param name="dateFrom"></param>
    ''' <param name="dateTo"></param>
    ''' <param name="Alldate"></param>
    ''' <returns>Order DataSet Collection</returns>
    Public Shared Function GetAuditTrailAConsumer(ByVal storeId As String, ByVal datefrom As DateTime, ByVal dateto As DateTime)
        Dim Consumer As New DataSet
        Try

            Consumer = AuditTrailReportDAL.GetAuditTrailAConsumer(storeId, datefrom, dateto)


        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Consumer
    End Function

    ''' <summary>
    ''' Get common val pritpalksaur
    ''' </summary>
    ''' <param name="dateFrom"></param>
    ''' <param name="dateTo"></param>
    ''' <param name="Alldate"></param>
    ''' <returns>Order DataSet Collection</returns>
    Public Shared Function GetAuditTrailAEquipment(ByVal storeId As String, ByVal datefrom As DateTime, ByVal dateto As DateTime)
        Dim AEquipmentds As New DataSet
        Try

            AEquipmentds = AuditTrailReportDAL.GetAuditTrailAEquipment(storeId, datefrom, dateto)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return AEquipmentds
    End Function

    ''' <summary>
    ''' Get common val pritpalksaur
    ''' </summary>
    ''' <param name="dateFrom"></param>
    ''' <param name="dateTo"></param>
    ''' <param name="Alldate"></param>
    ''' <returns>Order DataSet Collection</returns>
    Public Shared Function GetAuditTrailAStore(ByVal storeId As String, ByVal datefrom As DateTime, ByVal dateto As DateTime)
        Dim AStoreds As New DataSet
        Try

            AStoreds = AuditTrailReportDAL.GetAuditTrailAStore(storeId, datefrom, dateto)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return AStoreds
    End Function

    ''' <summary>
    ''' Get common val pritpalksaur
    ''' </summary>
    ''' <param name="dateFrom"></param>
    ''' <param name="dateTo"></param>
    ''' <param name="Alldate"></param>
    ''' <returns>Order DataSet Collection</returns>
    Public Shared Function GetAuditTrailASupplier(ByVal storeId As String, ByVal datefrom As DateTime, ByVal dateto As DateTime)
        Dim ASupplierds As New DataSet
        Try

            ASupplierds = AuditTrailReportDAL.GetAuditTrailASupplier(storeId, datefrom, dateto)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ASupplierds
    End Function

    ''' <summary>
    ''' Get common val pritpalksaur
    ''' </summary>
    ''' <param name="dateFrom"></param>
    ''' <param name="dateTo"></param>
    ''' <param name="Alldate"></param>
    ''' <returns>Order DataSet Collection</returns>
    Public Shared Function GetAuditTrailAUserRole(ByVal storeId As String, ByVal datefrom As DateTime, ByVal dateto As DateTime)
        Dim AUserRoleds As New DataSet
        Try

            AUserRoleds = AuditTrailReportDAL.GetAuditTrailAUserRole(storeId, datefrom, dateto)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return AUserRoleds
    End Function

    ''' <summary>
    ''' Get Stock Transaction based on Parameters;
    ''' 4Jan2009, Guo Feng
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="dateFrom"></param>
    ''' <param name="dateTo"></param>
    ''' <param name="auditType"></param>
    ''' <param name="transType"></param>
    ''' <returns>StockTransaction DataSet Collection</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetAuditTrailStockTransaction(ByVal storeId As String, ByVal dateFrom As DateTime, ByVal dateTo As DateTime, ByVal auditType As String, ByVal transType As String, ByVal orderBy As String) As DataSet

        Dim StockTransactions As New DataSet
        Try

            StockTransactions = AuditTrailReportDAL.GetAuditTrailStockTransaction(storeId, dateFrom, dateTo, auditType, transType, orderBy)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return StockTransactions
    End Function


    ''' <summary>
    ''' Get Residue Cost Adjustments based on parameters
    ''' 18 Oct 2010 
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="month"></param>
    ''' <param name="year"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetAuditTrailResidue(ByVal storeId As String, ByVal month As Integer, ByVal year As Integer) As DataSet

        Dim ResidueCostAdjustments As New DataSet

        Try

            ResidueCostAdjustments = AuditTrailReportDAL.GetAuditTrailResidue(storeId, month, year)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ResidueCostAdjustments

    End Function


End Class
