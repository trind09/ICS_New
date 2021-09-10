Imports NEA_ICS.UserInterface.NEA_ICS.WcfService
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

''' <summary>
''' BASE CLASS for Preparing Report Data Source - clsReportUtility
''' 26 Dec 2008 - Liu Guo Feng
''' </summary>
Public Class clsReportUtility
    Public Function GetSuppliers(ByVal storeId As String, ByVal supplierId As String, ByVal companyName As String, ByVal status As String) As System.Collections.Generic.List(Of NEA_ICS.WcfService.SupplierDetails)
        Try

            Dim Client As New ServiceClient
            Dim SupplierSearch As New SupplierDetails
            Dim SupplierList As New List(Of SupplierDetails)

            If String.IsNullOrEmpty(storeId) Then
                SupplierSearch.StoreId = ""
            Else
                SupplierSearch.StoreId = storeId
            End If
            If String.IsNullOrEmpty(supplierId) Then
                SupplierSearch.SupplierId = ""
            Else
                SupplierSearch.SupplierId = supplierId
            End If
            If String.IsNullOrEmpty(companyName) Then
                SupplierSearch.CompanyName = ""
            Else
                SupplierSearch.CompanyName = companyName
            End If
            If String.IsNullOrEmpty(status) Then
                SupplierSearch.Status = ""
            Else
                SupplierSearch.Status = status
            End If

            SupplierList = Client.GetSuppliers(SupplierSearch, String.Empty, String.Empty)
            Client.Close()

            Return SupplierList

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If

            Return New List(Of NEA_ICS.WcfService.SupplierDetails)
        End Try
    End Function

    Public Function GetItems(ByVal storeId As String, ByVal reportOption As Integer, ByVal sortBy As String, ByVal stockCodeFrom As String, ByVal stockCodeTo As String, ByVal excludeType As String) As System.Collections.Generic.List(Of NEA_ICS.WcfService.ItemDetails)
        Try

            Dim Client As New ServiceClient
            Dim ItemSearch As New ItemDetails
            ItemSearch.StoreID = storeId
            ItemSearch.ItemID = ""
            ItemSearch.Location = ""
            ItemSearch.Status = ""
            ItemSearch.EquipmentID = ""

            Dim ItemList As New List(Of ItemDetails)
            ItemList = Client.GetItems(ItemSearch, sortBy, String.Empty)
            Client.Close()

            Dim returnList As New List(Of ItemDetails)
            For Each item As ItemDetails In ItemList
                'Full Master List
                '2 Minimum Level i.e. stock bal <= min level 
                '3 Reorder Level i.e. stock bal <= reorder level
                '4 Slow Moving Items i.e. no transaction for 3 yrs (365*3 days) 
                '5 No Transaction period (From & To Date)
                Dim returnFlag As Boolean = True
                Select Case reportOption
                    Case 2
                        If item.OpeningBalance <= item.MinLevel Then
                            returnFlag = True
                        Else
                            returnFlag = False
                        End If
                    Case 3
                        If item.OpeningBalance <= item.ReorderLevel Then
                            returnFlag = True
                        Else
                            returnFlag = False
                        End If
                    Case 4
                        If item.TransactionDate.AddYears(3) <= DateTime.Now Then
                            returnFlag = True
                        Else
                            returnFlag = False
                        End If
                    Case Else
                        returnFlag = True
                End Select

                If returnFlag = True And item.ItemID >= stockCodeFrom And item.ItemID <= stockCodeTo And item.StockType <> excludeType Then
                    returnList.Add(item)
                End If
            Next

            Return returnList

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If

            Return New List(Of NEA_ICS.WcfService.ItemDetails)
        End Try
    End Function
End Class


