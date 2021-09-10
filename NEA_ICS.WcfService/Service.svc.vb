Imports NEA_ICS.Business
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF

''' <summary>
''' Service Layer - for ICS;
''' 17 Dec 08 - Kenny GOH
''' </summary>
''' <remarks>
''' CHANGE LOG:
''' 18Dec08  KG       RefID  Create partial class;
''' 30Dec08  Jianfa   RefID  To include view row as well
''' </remarks>
Public Class Service
    Implements IService

#Region " Constructor "
    Public Sub New()
    End Sub
#End Region

    ''' <summary>
    ''' to fill row with DBNull with blank;
    ''' 21 Dec 08, KG
    ''' </summary>
    ''' <param name="RowWithNull"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Shared Function FillRowWithNull(ByVal RowWithNull As DataRow, ByVal columnCol As DataColumnCollection) As DataRow
        Dim Count As Int16 = 0
        For Each dc As DataColumn In columnCol
            If Not dc.ReadOnly Then
                If RowWithNull(Count) Is DBNull.Value Or RowWithNull(Count) Is Nothing Then
                    Select Case dc.DataType().ToString
                        Case "System.String"
                            RowWithNull(Count) = ""
                        Case "System.DateTime"
                            RowWithNull(Count) = Date.MinValue
                        Case "System.Date"
                            RowWithNull(Count) = Date.MinValue
                        Case "System.Double"
                            RowWithNull(Count) = 0.0
                        Case "System.Float"
                            RowWithNull(Count) = 0.0
                        Case "System.Decimal"
                            RowWithNull(Count) = 0D
                        Case "System.Int16"
                            RowWithNull(Count) = 0
                        Case "System.Int32"
                            RowWithNull(Count) = 0
                        Case "System.Int64"
                            RowWithNull(Count) = 0
                        Case "Integer"
                            RowWithNull(Count) = 0
                        Case "System.Boolean"
                            RowWithNull(Count) = False
                    End Select
                End If
            End If
            Count += 1
        Next

        Return RowWithNull
    End Function

    ''' <summary>
    ''' To fill view row with DBNull with blanks;
    ''' 30 Dec 2008 - Jianfa;
    ''' </summary>
    ''' <param name="ViewRowWithNull"></param>
    ''' <param name="columnCol"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Shared Function FillViewRowWithNull(ByVal ViewRowWithNull As DataRowView, ByVal columnCol As DataColumnCollection) As DataRowView
        Dim Count As Int16 = 0
        For Each dc As DataColumn In columnCol
            If ViewRowWithNull(Count) Is DBNull.Value Then
                If dc.DataType().ToString = "System.String" Then
                    ViewRowWithNull(Count) = ""
                End If
            End If
            Count += 1
        Next

        Return ViewRowWithNull
    End Function

    Protected Function GetEmailContent(ByVal storeID As String, ByVal emailFormat As String) As List(Of String) Implements IService.GetEmailContent
        Dim dt As DataTable
        dt = MasterListBL.GetEmailContent(storeID, emailFormat)

        Dim emailOutput As New List(Of String)
        If (dt.Rows.Count > 0) Then
            emailOutput.Add(dt.Rows(0)(0).ToString())
            emailOutput.Add(dt.Rows(0)(1).ToString())
            emailOutput.Add(dt.Rows(0)(2).ToString())
            emailOutput.Add(dt.Rows(0)(3).ToString())
            emailOutput.Add(dt.Rows(0)(4).ToString())
            emailOutput.Add(dt.Rows(0)(5).ToString())
        End If
        Return emailOutput

    End Function
End Class
