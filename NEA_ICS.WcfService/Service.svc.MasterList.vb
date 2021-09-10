Imports NEA_ICS.Business
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Partial Public Class Service

#Region " SUPPLIER "

    ''' <summary>
    ''' Get the list of Suppliers based on the search criteria;
    ''' 18 Dec 08 - Jianfa CHEN;
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' 31 Dec 2008  Jianfa  Ref ID  To include sort expression and direction
    ''' </remarks>
    Public Function GetSuppliers(ByVal supplierDetails As SupplierDetails, _
                           Optional ByVal sortExpression As String = "", _
                           Optional ByVal sortDirection As String = "") As List(Of SupplierDetails) Implements IService.GetSuppliers

        Dim SupplierList As New List(Of SupplierDetails)

        Try

            Dim SuppliersRetrieved As New DataSet
            SupplierList.Clear()

            SuppliersRetrieved = MasterListBL.GetSuppliers( _
                supplierDetails.StoreId _
                , supplierDetails.SupplierId _
                , supplierDetails.CompanyName _
                , supplierDetails.Status _
            )

            If sortExpression = String.Empty And sortDirection = String.Empty Then

                If SuppliersRetrieved.Tables(0).Rows.Count > 0 Then
                    For Each row As DataRow In SuppliersRetrieved.Tables(0).Rows
                        row = FillRowWithNull(row, SuppliersRetrieved.Tables(0).Columns)

                        Dim SupplierDetailsItem As New SupplierDetails

                        SupplierDetailsItem.StoreId = row("SupplierStoreID")
                        SupplierDetailsItem.SupplierId = row("SupplierID")
                        SupplierDetailsItem.UEN = row("SupplierUEN")
                        SupplierDetailsItem.CompanyName = row("SupplierCompanyName")
                        SupplierDetailsItem.AddressType = row("SupplierAddressType")
                        SupplierDetailsItem.AddressBlockHouseNo = row("SupplierBlockHouseNo")
                        SupplierDetailsItem.AddressStreetName = row("SupplierStreetName")
                        SupplierDetailsItem.AddressFloorNo = row("SupplierFloorNo")
                        SupplierDetailsItem.AddressUnitNo = row("SupplierUnitNo")
                        SupplierDetailsItem.AddressBuildingName = row("SupplierBuildingName")
                        SupplierDetailsItem.AddressPostalCode = row("SupplierPostalCode")
                        SupplierDetailsItem.ContactPerson = row("SupplierContactPersonName")
                        SupplierDetailsItem.TelephoneNo = row("SupplierTelephoneNo")
                        SupplierDetailsItem.FaxNo = row("SupplierFaxNo")
                        SupplierDetailsItem.OtherInfo = row("SupplierOtherInfo")
                        SupplierDetailsItem.Status = row("SupplierStatus")

                        SupplierList.Add(SupplierDetailsItem)

                    Next
                End If

            Else

                Dim SuppliersRetrievedView As New DataView(SuppliersRetrieved.Tables(0))
                SuppliersRetrievedView.Sort = sortExpression & " " & sortDirection

                If SuppliersRetrieved.Tables(0).Rows.Count > 0 Then

                    For Each viewRow As DataRowView In SuppliersRetrievedView

                        viewRow = FillViewRowWithNull(viewRow, SuppliersRetrievedView.Table.Columns)

                        Dim SupplierDetailsItem As New SupplierDetails

                        SupplierDetailsItem.StoreId = viewRow("SupplierStoreID")
                        SupplierDetailsItem.SupplierId = viewRow("SupplierID")
                        SupplierDetailsItem.UEN = viewRow("SupplierUEN")
                        SupplierDetailsItem.CompanyName = viewRow("SupplierCompanyName")
                        SupplierDetailsItem.AddressType = viewRow("SupplierAddressType")
                        SupplierDetailsItem.AddressBlockHouseNo = viewRow("SupplierBlockHouseNo")
                        SupplierDetailsItem.AddressStreetName = viewRow("SupplierStreetName")
                        SupplierDetailsItem.AddressFloorNo = viewRow("SupplierFloorNo")
                        SupplierDetailsItem.AddressUnitNo = viewRow("SupplierUnitNo")
                        SupplierDetailsItem.AddressBuildingName = viewRow("SupplierBuildingName")
                        SupplierDetailsItem.AddressPostalCode = viewRow("SupplierPostalCode")
                        SupplierDetailsItem.ContactPerson = viewRow("SupplierContactPersonName")
                        SupplierDetailsItem.TelephoneNo = viewRow("SupplierTelephoneNo")
                        SupplierDetailsItem.FaxNo = viewRow("SupplierFaxNo")
                        SupplierDetailsItem.OtherInfo = viewRow("SupplierOtherInfo")
                        SupplierDetailsItem.Status = viewRow("SupplierStatus")

                        SupplierList.Add(SupplierDetailsItem)

                    Next

                End If

            End If


        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return SupplierList

    End Function

    ''' <summary>
    ''' To insert supplier details;
    ''' 18 Dec 08 - Jianfa CHEN;
    ''' </summary>
    ''' <param name="supplierDetails"></param>
    ''' <returns>ErrorMessage (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    ''' 
    Public Function AddSupplier(ByVal supplierDetails As SupplierDetails) As String Implements IService.AddSupplier

        Dim ErrorMessage As String = ""
        Try

            ErrorMessage = MasterListBL.AddSupplier( _
                supplierDetails.StoreId _
                , supplierDetails.SupplierId _
                , supplierDetails.CompanyName _
                , supplierDetails.AddressType _
                , supplierDetails.AddressBlockHouseNo _
                , supplierDetails.AddressStreetName _
                , supplierDetails.AddressFloorNo _
                , supplierDetails.AddressUnitNo _
                , supplierDetails.AddressBuildingName _
                , supplierDetails.AddressPostalCode _
                , supplierDetails.ContactPerson _
                , supplierDetails.TelephoneNo _
                , supplierDetails.FaxNo _
                , supplierDetails.OtherInfo _
                , supplierDetails.Status _
                , supplierDetails.LoginUser _
            )

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ErrorMessage
    End Function

    ''' <summary>
    ''' To update supplier details;
    ''' 20 Dec 08 - Jianfa CHEN;
    ''' </summary>
    ''' <param name="supplierDetails"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Function UpdateSupplier(ByVal supplierDetails As SupplierDetails) As String Implements IService.UpdateSupplier

        Dim ErrorMessage As String = ""
        Try
            ErrorMessage = MasterListBL.UpdateSupplier( _
                supplierDetails.StoreId _
                , supplierDetails.SupplierId _
                , supplierDetails.CompanyName _
                , supplierDetails.AddressType _
                , supplierDetails.AddressBlockHouseNo _
                , supplierDetails.AddressStreetName _
                , supplierDetails.AddressFloorNo _
                , supplierDetails.AddressUnitNo _
                , supplierDetails.AddressBuildingName _
                , supplierDetails.AddressPostalCode _
                , supplierDetails.ContactPerson _
                , supplierDetails.TelephoneNo _
                , supplierDetails.FaxNo _
                , supplierDetails.OtherInfo _
                , supplierDetails.Status _
                , supplierDetails.LoginUser _
                , supplierDetails.OriginalCompanyName _
                , supplierDetails.UEN _
            )

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ErrorMessage
    End Function

    ''' <summary>
    ''' Function - Update Supplier Status
    ''' </summary>
    ''' <param name="supplierDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateSupplierStatus(ByVal supplierDetails As SupplierDetails) As String Implements IService.UpdateSupplierStatus

        Dim ErrorMessage As String = ""
        Try
            ErrorMessage = MasterListBL.UpdateSupplierStatus( _
                supplierDetails.StoreId _
                , supplierDetails.SupplierId _
                , supplierDetails.Status _
                , supplierDetails.LoginUser _
            )

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ErrorMessage
    End Function
#End Region

#Region " COMMON "

    ''' <summary>
    ''' Function - Get Common;
    ''' </summary>
    ''' <param name="commonDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCommon(ByVal commonDetails As CommonDetails) As List(Of CommonDetails) Implements IService.GetCommon

        Dim CommonList As New List(Of CommonDetails)
        Dim CommonRetrieved As New DataSet

        Try

            CommonRetrieved = MasterListBL.GetCommon(commonDetails.StoreID, commonDetails.CodeGroup, commonDetails.Status)

            If CommonRetrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In CommonRetrieved.Tables(0).Rows
                    row = FillRowWithNull(row, CommonRetrieved.Tables(0).Columns)

                    Dim CommonDetailsItem As New CommonDetails

                    CommonDetailsItem.StoreID = row("CommonStoreID")
                    CommonDetailsItem.CommonID = row("CommonID")
                    CommonDetailsItem.CodeGroup = row("CommonCodeGroup")
                    CommonDetailsItem.CodeID = row("CommonCodeID")
                    CommonDetailsItem.CodeDescription = row("CommonCodeDescription")
                    CommonDetailsItem.Status = row("CommonStatus")

                    CommonList.Add(CommonDetailsItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return CommonList

    End Function

    ''' <summary>
    ''' Function - Get Distinct Common;
    ''' 03 Jan 09  Jianfa;
    ''' </summary>
    ''' <param name="commonDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDistinctCommon(ByVal commonDetails As CommonDetails) As List(Of CommonDetails) Implements IService.GetDistinctCommon

        Dim CommonList As New List(Of CommonDetails)
        Dim CommonRetrieved As New DataSet

        Try

            CommonRetrieved = MasterListBL.GetCommon(commonDetails.StoreID, commonDetails.CodeGroup, commonDetails.Status, True)

            If CommonRetrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In CommonRetrieved.Tables(0).Rows
                    row = FillRowWithNull(row, CommonRetrieved.Tables(0).Columns)

                    Dim CommonDetailsItem As New CommonDetails

                    CommonDetailsItem.CodeGroup = row("CommonCodeGroup")

                    CommonList.Add(CommonDetailsItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return CommonList

    End Function

    ''' <summary>
    ''' Function - Add Common;
    ''' 06 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="commonDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddCommon(ByVal commonDetails As CommonDetails) As String Implements IService.AddCommon

        Dim errorMessage As String = String.Empty

        Try

            errorMessage = MasterListBL.AddCommon(commonDetails.StoreID, commonDetails.CodeGroup, _
                                                  commonDetails.CodeID, commonDetails.CodeDescription, _
                                                  commonDetails.Status, commonDetails.LoginUser)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return errorMessage
    End Function

    ''' <summary>
    ''' Function - UpdateCommon;
    ''' 06 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="commonDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateCommon(ByVal commonDetails As CommonDetails) As String Implements IService.UpdateCommon

        Dim errorMessage As String = String.Empty

        Try

            errorMessage = MasterListBL.UpdateCommon(commonDetails.StoreID, commonDetails.CommonID, _
                                                     commonDetails.CodeGroup, commonDetails.CodeID, _
                                                     commonDetails.CodeDescription, _
                                                     commonDetails.Status, commonDetails.LoginUser)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return errorMessage
    End Function

#End Region

#Region " EQUIPMENTS "

    ''' <summary>
    ''' To add new equipment 
    ''' 24 Dec 2008 - Jianfa
    ''' </summary>
    ''' <param name="equipmentDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddEquipment(ByVal equipmentDetails As EquipmentDetails) As String Implements IService.AddEquipment

        Dim ErrorMessage As String = ""
        Try
            ErrorMessage = MasterListBL.AddEquipment( _
                equipmentDetails.StoreID _
                , equipmentDetails.EquipmentID _
                , equipmentDetails.EquipmentType _
                , equipmentDetails.EquipmentDescription _
                , equipmentDetails.Status _
                , equipmentDetails.LoginUser _
            )

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ErrorMessage

    End Function

    ''' <summary>
    ''' To get equipments
    ''' 29 Dec 2008 - Jianfa
    ''' </summary>
    ''' <param name="equipmentDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetEquipments(ByVal equipmentDetails As EquipmentDetails, Optional ByVal sortExpression As String = "", _
                                  Optional ByVal sortDirection As String = "") As List(Of EquipmentDetails) Implements IService.GetEquipments

        Dim EquipmentList As New List(Of EquipmentDetails)
        Try

            Dim EquipmentsRetrieved As New DataSet
            EquipmentList.Clear()

            EquipmentsRetrieved = MasterListBL.GetEquipments( _
                equipmentDetails.StoreID _
                , equipmentDetails.EquipmentID _
                , equipmentDetails.EquipmentType _
                , equipmentDetails.EquipmentDescription _
                , equipmentDetails.Status _
            )

            If sortExpression = String.Empty And sortDirection = String.Empty Then

                If EquipmentsRetrieved.Tables(0).Rows.Count > 0 Then
                    For Each row As DataRow In EquipmentsRetrieved.Tables(0).Rows
                        row = FillRowWithNull(row, EquipmentsRetrieved.Tables(0).Columns)

                        Dim EquipmentDetailsItem As New EquipmentDetails

                        EquipmentDetailsItem.StoreID = row("EquipmentStoreID")
                        EquipmentDetailsItem.EquipmentID = row("EquipmentID")
                        EquipmentDetailsItem.EquipmentType = row("EquipmentType")
                        EquipmentDetailsItem.EquipmentDescription = row("EquipmentDescription")
                        EquipmentDetailsItem.EquipmentID_Description = row("EquipmentID") & " - " & row("EquipmentDescription")
                        EquipmentDetailsItem.Status = row("EquipmentStatus")

                        EquipmentList.Add(EquipmentDetailsItem)

                    Next
                End If

            Else

                Dim EquipmentsRetrievedView As New DataView(EquipmentsRetrieved.Tables(0))
                EquipmentsRetrievedView.Sort = sortExpression & " " & sortDirection

                If EquipmentsRetrieved.Tables(0).Rows.Count > 0 Then
                    For Each viewRow As DataRowView In EquipmentsRetrievedView

                        viewRow = FillViewRowWithNull(viewRow, EquipmentsRetrievedView.Table.Columns)

                        Dim EquipmentDetailsItem As New EquipmentDetails

                        EquipmentDetailsItem.StoreID = viewRow("EquipmentStoreID")
                        EquipmentDetailsItem.EquipmentID = viewRow("EquipmentID")
                        EquipmentDetailsItem.EquipmentType = viewRow("EquipmentType")
                        EquipmentDetailsItem.EquipmentDescription = viewRow("EquipmentDescription")
                        EquipmentDetailsItem.EquipmentID_Description = viewRow("EquipmentID") & " - " & viewRow("EquipmentDescription")
                        EquipmentDetailsItem.Status = viewRow("EquipmentStatus")

                        EquipmentList.Add(EquipmentDetailsItem)

                    Next
                End If
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return EquipmentList

    End Function

    ''' <summary>
    ''' To update equipment status
    ''' 29 Dec 2008 - Jianfa
    ''' </summary>
    ''' <param name="equipmentDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateEquipmentStatus(ByVal equipmentDetails As EquipmentDetails) As String Implements IService.UpdateEquipmentStatus

        Dim ErrorMessage As String = ""
        Try
            ErrorMessage = MasterListBL.UpdateEquipmentStatus( _
                equipmentDetails.StoreID _
                , equipmentDetails.EquipmentID _
                , equipmentDetails.Status _
                , equipmentDetails.LoginUser _
            )

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ErrorMessage
    End Function

    ''' <summary>
    ''' To update equipment
    ''' 29 Dec 2008 - Jianfa;
    ''' </summary>
    ''' <param name="equipmentDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateEquipment(ByVal equipmentDetails As EquipmentDetails) As String Implements IService.UpdateEquipment

        Dim ErrorMessage As String = ""
        Try
            ErrorMessage = MasterListBL.UpdateEquipment( _
                            equipmentDetails.StoreID, equipmentDetails.EquipmentID, _
                            equipmentDetails.EquipmentType, _
                            equipmentDetails.EquipmentDescription, equipmentDetails.LoginUser)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ErrorMessage

    End Function
#End Region

#Region " STORES "

    ''' <summary>
    ''' Function -  Add Store;
    ''' 31 Dec 2008 - Jianfa;
    ''' </summary>
    ''' <param name="storeDetails"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' To insert new store based on selective roles
    ''' </remarks>
    Public Function AddStore(ByVal storeDetails As StoreDetails) As String Implements IService.AddStore

        Dim ErrorMessage As String = ""
        Try

            ErrorMessage = MasterListBL.AddStore( _
                                                storeDetails.StoreId _
                                                , storeDetails.StoreName _
                                                , storeDetails.AddressType _
                                                , storeDetails.AddressBlockHouseNo _
                                                , storeDetails.AddressStreetName _
                                                , storeDetails.AddressFloorNo _
                                                , storeDetails.AddressUnitNo _
                                                , storeDetails.AddressBuildingName _
                                                , storeDetails.AddressPostalCode _
                                                , storeDetails.ContactPerson _
                                                , storeDetails.TelephoneNo _
                                                , storeDetails.FaxNo _
                                                , storeDetails.OtherInfo _
                                                , storeDetails.Status _
                                                , storeDetails.LoginUser _
                                                , storeDetails.UserRoles _
                                                )

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ErrorMessage

    End Function

    ''' <summary>
    ''' Function - Get Stores
    ''' 01 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeDetails"></param>
    ''' <param name="sortExpression"></param>
    ''' <param name="sortDirection"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetStores(ByVal storeDetails As StoreDetails, Optional ByVal sortExpression As String = "", _
                                  Optional ByVal sortDirection As String = "") As List(Of StoreDetails) Implements IService.GetStores

        Dim StoreList As New List(Of StoreDetails)

        Try

            Dim StoreRetrieved As New DataSet
            StoreList.Clear()

            StoreRetrieved = MasterListBL.GetStores( _
                storeDetails.StoreId, storeDetails.StoreName, storeDetails.Status)


            If sortExpression = String.Empty And sortDirection = String.Empty Then

                If StoreRetrieved.Tables(0).Rows.Count > 0 Then
                    For Each row As DataRow In StoreRetrieved.Tables(0).Rows
                        row = FillRowWithNull(row, StoreRetrieved.Tables(0).Columns)

                        Dim StoreDetailsItem As New StoreDetails

                        StoreDetailsItem.StoreId = row("StoreID")
                        StoreDetailsItem.StoreName = row("StoreName")
                        StoreDetailsItem.StoreDescription = row("StoreID") & " - " & row("StoreName")
                        StoreDetailsItem.AddressType = row("StoreAddressType")
                        StoreDetailsItem.AddressBlockHouseNo = row("StoreBlockHouseNo")
                        StoreDetailsItem.AddressStreetName = row("StoreStreetName")
                        StoreDetailsItem.AddressFloorNo = row("StoreFloorNo")
                        StoreDetailsItem.AddressUnitNo = row("StoreUnitNo")
                        StoreDetailsItem.AddressBuildingName = row("StoreBuildingName")
                        StoreDetailsItem.AddressPostalCode = row("StorePostalCode")
                        StoreDetailsItem.ContactPerson = row("StoreContactPersonName")
                        StoreDetailsItem.TelephoneNo = row("StoreTelephoneNo")
                        StoreDetailsItem.FaxNo = row("StoreFaxNo")
                        StoreDetailsItem.OtherInfo = row("StoreOtherInfo")
                        StoreDetailsItem.Status = row("StoreStatus")

                        StoreList.Add(StoreDetailsItem)

                    Next
                End If

            Else

                Dim StoreRetrievedView As New DataView(StoreRetrieved.Tables(0))
                StoreRetrievedView.Sort = sortExpression & " " & sortDirection

                If StoreRetrieved.Tables(0).Rows.Count > 0 Then
                    For Each viewRow As DataRowView In StoreRetrievedView

                        viewRow = FillViewRowWithNull(viewRow, StoreRetrievedView.Table.Columns)

                        Dim StoreDetailsItem As New StoreDetails

                        StoreDetailsItem.StoreId = viewRow("StoreID")
                        StoreDetailsItem.StoreName = viewRow("StoreName")
                        StoreDetailsItem.StoreDescription = viewRow("StoreID") & " - " & viewRow("StoreName")
                        StoreDetailsItem.AddressType = viewRow("StoreAddressType")
                        StoreDetailsItem.AddressBlockHouseNo = viewRow("StoreBlockHouseNo")
                        StoreDetailsItem.AddressStreetName = viewRow("StoreStreetName")
                        StoreDetailsItem.AddressFloorNo = viewRow("StoreFloorNo")
                        StoreDetailsItem.AddressUnitNo = viewRow("StoreUnitNo")
                        StoreDetailsItem.AddressBuildingName = viewRow("StoreBuildingName")
                        StoreDetailsItem.AddressPostalCode = viewRow("StorePostalCode")
                        StoreDetailsItem.ContactPerson = viewRow("StoreContactPersonName")
                        StoreDetailsItem.TelephoneNo = viewRow("StoreTelephoneNo")
                        StoreDetailsItem.FaxNo = viewRow("StoreFaxNo")
                        StoreDetailsItem.OtherInfo = viewRow("StoreOtherInfo")
                        StoreDetailsItem.Status = viewRow("StoreStatus")

                        StoreList.Add(StoreDetailsItem)

                    Next
                End If
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return StoreList

    End Function

    ''' <summary>
    ''' Function - Update Store
    ''' 01 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateStore(ByVal storeDetails As StoreDetails) As String Implements IService.UpdateStore

        Dim ErrorMessage As String = ""
        Try
            ErrorMessage = MasterListBL.UpdateStore(storeDetails.StoreId, storeDetails.StoreName, _
                                                    storeDetails.AddressType, storeDetails.AddressBlockHouseNo, _
                                                    storeDetails.AddressStreetName, storeDetails.AddressFloorNo, _
                                                    storeDetails.AddressUnitNo, storeDetails.AddressBuildingName, _
                                                    storeDetails.AddressPostalCode, storeDetails.ContactPerson, _
                                                    storeDetails.TelephoneNo, storeDetails.FaxNo, storeDetails.OtherInfo, _
                                                    storeDetails.OriginalStoreName, storeDetails.LoginUser)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ErrorMessage

    End Function

    ''' <summary>
    ''' Function - Update Store Status
    ''' 02 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateStoreStatus(ByVal storeDetails As StoreDetails) As String Implements IService.UpdateStoreStatus

        Dim ErrorMessage As String = ""
        Try

            ErrorMessage = MasterListBL.UpdateStoreStatus( _
                            storeDetails.StoreId, storeDetails.Status, storeDetails.LoginUser)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ErrorMessage

    End Function

#End Region

#Region " ITEMS "

    ''' <summary>
    ''' Function - GenerateItemID;
    ''' 07 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="itemDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GenerateItemID(ByVal itemDetails As ItemDetails, _
                                   ByRef generatedItemID As String) As String Implements IService.GenerateItemID

        Dim errorMessage As String = String.Empty

        Try

            errorMessage = MasterListBL.GenerateItemID(itemDetails.ItemID, itemDetails.StoreID, generatedItemID)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return errorMessage

    End Function

    ''' <summary>
    ''' Function - AddItem;
    ''' 08 Jan - Jianfa;
    ''' </summary>
    ''' <param name="itemDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddItem(ByVal itemDetails As ItemDetails) As String Implements IService.AddItem

        Dim errorMessage As String = String.Empty

        Try

            errorMessage = MasterListBL.AddItem(itemDetails.StoreID, itemDetails.ItemID, itemDetails.EquipmentID, _
                                                itemDetails.ItemDescription, itemDetails.PartNo, _
                                                itemDetails.StockType, itemDetails.SubType, itemDetails.UOM, _
                                                itemDetails.Location, itemDetails.Location2, itemDetails.MinLevel, _
                                                itemDetails.ReorderLevel, itemDetails.MaxLevel, _
                                                itemDetails.OpeningBalance, itemDetails.OpeningTotalValue, _
                                                itemDetails.Status, itemDetails.LoginUser)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return errorMessage

    End Function

    ''' <summary>
    ''' Function - GetItems
    ''' 08 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="itemDetails"></param>
    ''' <param name="sortExpression"></param>
    ''' <param name="sortDirection"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetItems(ByVal itemDetails As ItemDetails, _
                             Optional ByVal sortExpression As String = "", _
                             Optional ByVal sortDirection As String = "") As List(Of ItemDetails) _
                             Implements IService.GetItems

        Dim ItemList As New List(Of ItemDetails)

        Try

            Dim ItemRetrieved As New DataSet
            ItemList.Clear()

            ItemRetrieved = MasterListBL.GetItems( _
                            itemDetails.StoreID, itemDetails.ItemID, itemDetails.Location, _
                            itemDetails.Status, itemDetails.EquipmentID, itemDetails.ItemDescription)

            If sortExpression = String.Empty And sortDirection = String.Empty Then

                If ItemRetrieved.Tables(0).Rows.Count > 0 Then
                    For Each row As DataRow In ItemRetrieved.Tables(0).Rows
                        row = FillRowWithNull(row, ItemRetrieved.Tables(0).Columns)

                        Dim ItemDetailsItem As New ItemDetails

                        ItemDetailsItem.StoreID = row("StockItemStoreID")
                        ItemDetailsItem.ItemID = row("StockItemID")
                        ItemDetailsItem.EquipmentID = row("StockItemEquipmentID")
                        ItemDetailsItem.ItemDescription = row("StockItemDescription")
                        ItemDetailsItem.ItemID_Description = row("StockItemID") & " - " & row("StockItemDescription")
                        ItemDetailsItem.PartNo = row("StockItemPartNo")
                        ItemDetailsItem.StockType = row("StockItemStockType")
                        ItemDetailsItem.SubType = row("StockItemSubType")
                        ItemDetailsItem.UOM = row("StockItemUOM")
                        ItemDetailsItem.Location = row("StockItemLocation")
                        ItemDetailsItem.Location2 = row("StockItemLocation2")
                        ItemDetailsItem.MinLevel = row("StockItemMinLevel")
                        ItemDetailsItem.ReorderLevel = row("StockItemReorderLevel")
                        ItemDetailsItem.MaxLevel = row("StockItemMaxLevel")
                        ItemDetailsItem.Status = row("StockItemStatus")
                        ItemDetailsItem.LoginUser = row("StockItemCreateUserID")

                        ItemList.Add(ItemDetailsItem)

                    Next
                End If

            Else

                Dim ItemRetrievedView As New DataView(ItemRetrieved.Tables(0))
                ItemRetrievedView.Sort = sortExpression & " " & sortDirection

                If ItemRetrieved.Tables(0).Rows.Count > 0 Then
                    For Each viewRow As DataRowView In ItemRetrievedView

                        viewRow = FillViewRowWithNull(viewRow, ItemRetrievedView.Table.Columns)

                        Dim ItemDetailsItem As New ItemDetails

                        ItemDetailsItem.StoreID = viewRow("StockItemStoreID")
                        ItemDetailsItem.ItemID = viewRow("StockItemID")
                        ItemDetailsItem.EquipmentID = viewRow("StockItemEquipmentID")
                        ItemDetailsItem.ItemDescription = viewRow("StockItemDescription")
                        ItemDetailsItem.ItemID_Description = viewRow("StockItemID") & " - " & viewRow("StockItemDescription")
                        ItemDetailsItem.PartNo = viewRow("StockItemPartNo")
                        ItemDetailsItem.StockType = viewRow("StockItemStockType")
                        ItemDetailsItem.SubType = viewRow("StockItemSubType")
                        ItemDetailsItem.UOM = viewRow("StockItemUOM")
                        ItemDetailsItem.Location = viewRow("StockItemLocation")
                        ItemDetailsItem.Location2 = viewRow("StockItemLocation2")
                        ItemDetailsItem.MinLevel = viewRow("StockItemMinLevel")
                        ItemDetailsItem.ReorderLevel = viewRow("StockItemReorderLevel")
                        ItemDetailsItem.MaxLevel = viewRow("StockItemMaxLevel")
                        ItemDetailsItem.Status = viewRow("StockItemStatus")
                        ItemDetailsItem.LoginUser = viewRow("StockItemCreateUserID")

                        ItemList.Add(ItemDetailsItem)

                    Next
                End If
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ItemList

    End Function

    ''' <summary>
    ''' Function - GetItemsMasterList
    ''' 03 Mar 2009 - Guo Feng
    ''' </summary>
    ''' <param name="printOption"></param>
    ''' <param name="sortBy"></param>
    ''' <param name="stockCodeFrom"></param>
    ''' <param name="stockCodeTo"></param>
    ''' <param name="excludeStockCodeTypes"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetItemsMasterList(ByVal storeID As String, _
                      ByVal printOption As String, _
                      ByVal sortBy As String, _
                      ByVal stockCodeFrom As String, _
                      ByVal stockCodeTo As String, _
                      ByVal excludeStockCodeTypes As String, _
                      ByVal itemStatus As String) As List(Of ItemDetails) Implements IService.GetItemsMasterList

        Dim ItemList As New List(Of ItemDetails)

        Try

            Dim ItemRetrieved As New DataSet
            ItemList.Clear()

            ItemRetrieved = MasterListBL.GetItemsMasterList(storeID, stockCodeFrom, stockCodeTo, itemStatus)

            If ItemRetrieved.Tables(0).Rows.Count > 0 Then
                Dim ItemRetrievedView As New DataView(ItemRetrieved.Tables(0))
                ItemRetrievedView.Sort = sortBy

                If ItemRetrieved.Tables(0).Rows.Count > 0 Then
                    For Each viewRow As DataRowView In ItemRetrievedView

                        viewRow = FillViewRowWithNull(viewRow, ItemRetrievedView.Table.Columns)

                        Dim ItemDetailsItem As New ItemDetails

                        ItemDetailsItem.StoreID = storeID
                        ItemDetailsItem.ItemID = viewRow("FStockItemByStockRangeID")
                        ItemDetailsItem.EquipmentID = viewRow("FStockItemByStockRangeEquipmentID")
                        ItemDetailsItem.ItemDescription = viewRow("FStockItemByStockRangeDescription")
                        ItemDetailsItem.ItemID_Description = viewRow("FStockItemByStockRangeID") & " - " & viewRow("FStockItemByStockRangeDescription")
                        ItemDetailsItem.PartNo = viewRow("FStockItemByStockRangePartNo")
                        ItemDetailsItem.StockType = viewRow("StockTypeDesc")
                        ItemDetailsItem.SubType = viewRow("SubTypeDesc")
                        ItemDetailsItem.UOM = viewRow("FStockItemByStockRangeUOM")
                        ItemDetailsItem.Location = viewRow("FStockItemByStockRangeLocation")
                        ItemDetailsItem.Location2 = viewRow("FStockItemByStockRangeLocation2")
                        ItemDetailsItem.MinLevel = viewRow("FStockItemByStockRangeMinLevel")
                        ItemDetailsItem.ReorderLevel = viewRow("FStockItemByStockRangeReorderLevel")
                        ItemDetailsItem.MaxLevel = viewRow("FStockItemByStockRangeMaxLevel")
                        ItemDetailsItem.Status = viewRow("FStockItemByStockRangeStatus")
                        If Not IsDBNull(viewRow("FStockItemByStockRangeStockBal")) Then
                            ItemDetailsItem.OpeningBalance = viewRow("FStockItemByStockRangeStockBal")
                        End If
                        If Not IsDBNull(viewRow("FStockItemByStockRangeTotalCost")) Then
                            ItemDetailsItem.OpeningTotalValue = viewRow("FStockItemByStockRangeTotalCost")
                        End If
                        If Not IsDBNull(viewRow("AUCost")) Then
                            ItemDetailsItem.AUCost = viewRow("AUCost")
                        End If

                        Dim returnFlag As Boolean = True
                        Select Case printOption
                            Case "A"
                                returnFlag = True
                            Case "M"
                                If ItemDetailsItem.OpeningBalance <= ItemDetailsItem.MinLevel Then
                                    returnFlag = True
                                Else
                                    returnFlag = False
                                End If
                            Case "R"
                                If ItemDetailsItem.OpeningBalance <= ItemDetailsItem.ReorderLevel Then
                                    returnFlag = True
                                Else
                                    returnFlag = False
                                End If
                            Case "S"
                                If ItemDetailsItem.TransactionDate.AddYears(3) <= DateTime.Now Then
                                    returnFlag = True
                                Else
                                    returnFlag = False
                                End If
                            Case Else
                                returnFlag = True
                        End Select

                        If returnFlag = True And Not excludeStockCodeTypes.Split("|").Contains(viewRow("FStockItemByStockRangeStockType").ToString()) Then
                            ItemList.Add(ItemDetailsItem)
                        End If

                    Next
                End If
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ItemList
    End Function

    ''' <summary>
    ''' Function - GetStockTransaction;
    ''' 09 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="itemDetails"></param>
    ''' <param name="transactionType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetStockTransaction(ByVal itemDetails As ItemDetails, ByVal transactionType As String) As List(Of ItemDetails) Implements IService.GetStockTransaction

        Dim ItemList As New List(Of ItemDetails)

        Try

            Dim ItemRetrieved As New DataSet
            ItemList.Clear()

            ItemRetrieved = MasterListBL.GetStockTransaction( _
                            itemDetails.StoreID, itemDetails.ItemID, transactionType)


            If ItemRetrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In ItemRetrieved.Tables(0).Rows
                    row = FillRowWithNull(row, ItemRetrieved.Tables(0).Columns)

                    Dim ItemDetailsItem As New ItemDetails

                    ItemDetailsItem.OpeningBalance = row("StockTransactionQty")
                    ItemDetailsItem.OpeningTotalValue = row("StockTransactionTotalCost")
                    ItemDetailsItem.TransactionDate = row("StockTransactionDte")

                    ItemList.Add(ItemDetailsItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ItemList

    End Function

    ''' <summary>
    ''' Function - UpdateItem;
    ''' 10 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="itemDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateItem(ByVal itemDetails As ItemDetails, _
                               ByRef returnMessage As String) As String Implements IService.UpdateItem

        Dim errorMessage As String = String.Empty

        Try

            errorMessage = MasterListBL.UpdateItem(itemDetails.StoreID, itemDetails.ItemID, itemDetails.EquipmentID, _
                                                itemDetails.ItemDescription, itemDetails.PartNo, _
                                                itemDetails.StockType, itemDetails.SubType, itemDetails.UOM, _
                                                itemDetails.Location, itemDetails.Location2, itemDetails.MinLevel, _
                                                itemDetails.ReorderLevel, itemDetails.MaxLevel, _
                                                itemDetails.OpeningBalance, itemDetails.OpeningTotalValue, _
                                                itemDetails.Status, itemDetails.LoginUser, returnMessage)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return errorMessage

    End Function

    ''' <summary>
    ''' 02 May 09 - Jianfa
    ''' </summary>
    ''' <param name="itemDetails"></param>
    ''' <param name="returnMessage"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateItemStatus(ByVal itemDetails As ItemDetails, ByRef returnMessage As String) As String Implements IService.UpdateItemStatus


        Try

            MasterListBL.UpdateItemStatus(itemDetails.StoreID, itemDetails.ItemID, _
                                          itemDetails.Status, itemDetails.LoginUser, _
                                          returnMessage)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return returnMessage

    End Function

    ''' <summary>
    ''' 01 May 09 - Jianfa
    ''' Function - GetItemSearch
    ''' </summary>
    ''' <param name="itemDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetItemSearch(ByVal itemDetails As ItemDetails) As List(Of String) _
                                    Implements IService.GetItemSearch

        Dim ItemList As New List(Of String)
        ItemList.Clear()

        Try

            Dim ItemRetrieved As New DataSet

            ItemRetrieved = MasterListBL.GetItemSearch(itemDetails.StoreID, itemDetails.ItemID)

            If ItemRetrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In ItemRetrieved.Tables(0).Rows

                    row = FillRowWithNull(row, ItemRetrieved.Tables(0).Columns)

                    ItemList.Add(row("StockItemID") & " | " & row("StockItemDescription"))


                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ItemList

    End Function

    ''' <summary>
    ''' 01 May 09 - Jianfa
    ''' Function - IsValidStockCode
    ''' </summary>
    ''' <param name="itemDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsValidStockCode(ByVal itemDetails As ItemDetails) As Boolean _
                                        Implements IService.IsValidStockCode

        Dim found As Boolean

        Try

            found = MasterListBL.CheckItemID(itemDetails.StoreID, itemDetails.ItemID)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return found

    End Function

    ''' <summary>
    ''' 28 Feb 2011 - Jianfa
    ''' Function - IsValid Status 
    ''' </summary>
    ''' <param name="itemDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsValidStatus(ByVal itemDetails As ItemDetails) As Boolean _
                                    Implements IService.IsValidStatus

        Dim valid As Boolean

        Try

            valid = MasterListBL.CheckItemStatus(itemDetails.StoreID, itemDetails.ItemID)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return valid

    End Function
#End Region

#Region " ROLES "

    ''' <summary>
    ''' Function - GetModuleRoles;
    ''' 16 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="roleDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetModuleRoles(ByVal roleDetails As RoleDetails) As List(Of RoleDetails) Implements IService.GetModuleRoles

        Dim RoleList As New List(Of RoleDetails)

        Try

            Dim RolesRetrieved As New DataSet
            RoleList.Clear()

            RolesRetrieved = MasterListBL.GetModuleRoles(roleDetails.StoreID, roleDetails.UserRole)

            If RolesRetrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In RolesRetrieved.Tables(0).Rows
                    row = FillRowWithNull(row, RolesRetrieved.Tables(0).Columns)

                    Dim RoleDetailsItem As New RoleDetails

                    RoleDetailsItem.ModuleID = row("ModuleRefModuleID")
                    RoleDetailsItem.MasterID = row("ModuleMasterID")
                    RoleDetailsItem.ModuleTitle = row("ModuleTitle")
                    RoleDetailsItem.ModuleSource = row("ModuleSource")
                    RoleDetailsItem.InsertRight = row("ModuleRefInsertRight")
                    RoleDetailsItem.DeleteRight = row("ModuleRefDeleteRight")
                    RoleDetailsItem.UpdateRight = row("ModuleRefUpdateRight")
                    RoleDetailsItem.SelectRight = row("ModuleRefSelectRight")

                    RoleList.Add(RoleDetailsItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return RoleList

    End Function

    ''' <summary>
    ''' Function - GetUserRoles;
    ''' 18 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="roleDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetUserRoles(ByVal roleDetails As RoleDetails, Optional ByVal sortExpression As String = "",
                                  Optional ByVal sortDirection As String = "") As List(Of RoleDetails) Implements IService.GetUserRoles

        Dim RoleList As New List(Of RoleDetails)

        Try

            Dim RolesRetrieved As New DataSet
            RoleList.Clear()

            RolesRetrieved = MasterListBL.GetUserRoles(roleDetails.StoreID, roleDetails.UserRole)

            If sortExpression = String.Empty And sortDirection = String.Empty Then

                If RolesRetrieved.Tables(0).Rows.Count > 0 Then
                    For Each row As DataRow In RolesRetrieved.Tables(0).Rows
                        row = FillRowWithNull(row, RolesRetrieved.Tables(0).Columns)

                        Dim RoleDetailsItem As New RoleDetails

                        RoleDetailsItem.UserID = row("UserRoleUserID")
                        RoleDetailsItem.SoeID = row("UserRoleSoeID")
                        RoleDetailsItem.Name = row("VUserProfileName")
                        RoleDetailsItem.Designation = row("VUserProfileDesignation")
                        RoleDetailsItem.Division = row("VUserProfileDivisionCode")
                        RoleDetailsItem.Department = row("VUserProfileDepartCode")
                        RoleDetailsItem.Section = row("VUserProfileSectionCode")
                        RoleDetailsItem.Installation = row("VUserProfileInstallCode")
                        RoleDetailsItem.UserStatus = row("UserRoleStatus")

                        RoleList.Add(RoleDetailsItem)

                    Next
                End If

            Else

                Dim RolesRetrievedView As New DataView(RolesRetrieved.Tables(0))
                RolesRetrievedView.Sort = sortExpression & " " & sortDirection

                If RolesRetrieved.Tables(0).Rows.Count > 0 Then

                    For Each viewRow As DataRowView In RolesRetrievedView

                        viewRow = FillViewRowWithNull(viewRow, RolesRetrievedView.Table.Columns)

                        Dim RoleDetailsItem As New RoleDetails

                        RoleDetailsItem.UserID = viewRow("UserRoleUserID")
                        RoleDetailsItem.SoeID = viewRow("UserRoleSoeID")
                        RoleDetailsItem.Name = viewRow("VUserProfileName")
                        RoleDetailsItem.Designation = viewRow("VUserProfileDesignation")
                        RoleDetailsItem.Division = viewRow("VUserProfileDivisionCode")
                        RoleDetailsItem.Department = viewRow("VUserProfileDepartCode")
                        RoleDetailsItem.Section = viewRow("VUserProfileSectionCode")
                        RoleDetailsItem.Installation = viewRow("VUserProfileInstallCode")
                        RoleDetailsItem.UserStatus = viewRow("UserRoleStatus")

                        RoleList.Add(RoleDetailsItem)

                    Next

                End If

            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return RoleList

    End Function

    ''' <summary>
    ''' Function - UpdateModuleRole;
    ''' 16 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="roleDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateModuleRole(ByVal roleDetails As RoleDetails) As String Implements IService.UpdateModuleRole

        Dim errorMessage As String = String.Empty

        Try

            errorMessage = MasterListBL.UpdateModuleRoles(roleDetails.StoreID, roleDetails.UserRole, _
                                                          roleDetails.ModuleID, roleDetails.SelectRight, _
                                                          roleDetails.InsertRight, roleDetails.UpdateRight, _
                                                          roleDetails.DeleteRight, roleDetails.Status, _
                                                          roleDetails.LoginUser)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return errorMessage

    End Function

    ''' <summary>
    ''' Function - UpdateUserRole;
    ''' 20 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="roleDetails"></param>
    ''' <param name="consumerList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateUserRole(ByVal roleDetails As RoleDetails,
                                   ByVal consumerList As List(Of String)) As String Implements IService.UpdateUserRole

        Dim errorMessage As String = String.Empty

        Try

            errorMessage = MasterListBL.UpdateUserRole(roleDetails.StoreID, roleDetails.UserID, roleDetails.SoeID,
                                                       roleDetails.IsUserDeleted, roleDetails.ChangeStatusReason,
                                                       roleDetails.UserRole, roleDetails.UserStatus,
                                                       roleDetails.LoginUser, consumerList)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return errorMessage

    End Function

    ''' <summary>
    ''' Function - CheckNRIC;
    ''' 23 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="roleDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CheckNRIC(ByVal roleDetails As RoleDetails) _
                                As List(Of RoleDetails) Implements IService.CheckNRIC

        Dim RoleList As New List(Of RoleDetails)

        Try

            Dim RolesRetrieved As New DataSet
            RoleList.Clear()

            RolesRetrieved = MasterListBL.CheckNRIC(roleDetails.UserID)

            If RolesRetrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In RolesRetrieved.Tables(0).Rows
                    row = FillRowWithNull(row, RolesRetrieved.Tables(0).Columns)

                    Dim RoleDetailsItem As New RoleDetails

                    RoleDetailsItem.UserID = row("VUserProfileUserID")
                    RoleDetailsItem.Name = row("VUserProfileName")
                    RoleDetailsItem.Designation = row("VUserProfileDesignation")
                    RoleDetailsItem.Division = row("VUserProfileDivisionCode")
                    RoleDetailsItem.Department = row("VUserProfileDepartCode")
                    RoleDetailsItem.Section = row("VUserProfileSectionCode")
                    RoleDetailsItem.Installation = row("VUserProfileInstallCode")

                    RoleList.Add(RoleDetailsItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return RoleList

    End Function

    ''' <summary>
    ''' Function - AddUserRole;
    ''' 24 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="roleDetails"></param>
    ''' <param name="consumerList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddUserRole(ByVal roleDetails As RoleDetails, ByVal consumerList As List(Of String)) As String Implements IService.AddUserRole

        Dim errorMessage As String = String.Empty

        Try

            errorMessage = MasterListBL.AddUserRole(roleDetails.StoreID, roleDetails.UserID, roleDetails.SoeID,
                                                    roleDetails.UserRole, roleDetails.UserStatus,
                                                    roleDetails.LoginUser, consumerList)
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return errorMessage

    End Function

    ''' <summary>
    ''' Function - DeleteUserRole;
    ''' 26 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="roleDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DeleteUserRole(ByVal roleDetails As RoleDetails) As String Implements IService.DeleteUserRole

        Dim errorMessage As String = String.Empty

        Try

            errorMessage = MasterListBL.DeleteUserRole(roleDetails.StoreID, roleDetails.UserID,
                                                       roleDetails.UserRole)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return errorMessage

    End Function

    Public Function GetUserRoleIDBySoeID(ByVal soeID As String) As DataSet Implements IService.GetUserRoleIDBySoeID

        Try

            Dim RolesRetrieved As New DataSet

            RolesRetrieved = MasterListBL.GetUserRoleIDBySoeID(soeID)

            If RolesRetrieved IsNot Nothing Then
                Return RolesRetrieved
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Nothing
    End Function

    ''' <summary>
    ''' Function - GetModuleAccess;
    ''' 09 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="roleDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetModuleAccess(ByVal roleDetails As RoleDetails) As List(Of RoleDetails) Implements IService.GetModuleAccess

        Dim ModuleList As New List(Of RoleDetails)

        Try

            Dim ModulesRetrieved As New DataSet
            ModuleList.Clear()

            ModulesRetrieved = MasterListBL.GetModuleAccess(roleDetails.StoreID, roleDetails.UserID)

            If ModulesRetrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In ModulesRetrieved.Tables(0).Rows
                    row = FillRowWithNull(row, ModulesRetrieved.Tables(0).Columns)

                    Dim RoleDetailsItem As New RoleDetails

                    RoleDetailsItem.ModuleID = row("ModuleID")
                    RoleDetailsItem.ParentID = row("ModuleParentID")
                    RoleDetailsItem.MasterID = row("ModuleMasterID")
                    RoleDetailsItem.ModuleTitle = row("ModuleTitle")
                    RoleDetailsItem.ModuleSource = row("ModuleSource")
                    RoleDetailsItem.ModuleType = row("ModuleType")

                    ModuleList.Add(RoleDetailsItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ModuleList

    End Function

    ''' <summary>
    ''' Function - GetModuleAccessRights;
    ''' 11 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="roleDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetModuleAccessRights(ByVal roleDetails As RoleDetails) As List(Of RoleDetails) Implements IService.GetModuleAccessRights

        Dim AccessRightsList As New List(Of RoleDetails)

        Try

            Dim AccessRightsRetrieved As New DataSet
            AccessRightsList.Clear()

            AccessRightsRetrieved = MasterListBL.GetModuleAccessRights(roleDetails.StoreID, roleDetails.UserID)

            If AccessRightsRetrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In AccessRightsRetrieved.Tables(0).Rows
                    row = FillRowWithNull(row, AccessRightsRetrieved.Tables(0).Columns)

                    Dim RoleDetailsItem As New RoleDetails

                    RoleDetailsItem.ModuleID = row("ModuleRefModuleID")
                    RoleDetailsItem.UserRole = row("ModuleRefStoreRole")
                    RoleDetailsItem.InsertRight = row("ModuleRefInsertRight")
                    RoleDetailsItem.DeleteRight = row("ModuleRefDeleteRight")
                    RoleDetailsItem.UpdateRight = row("ModuleRefUpdateRight")
                    RoleDetailsItem.SelectRight = row("ModuleRefSelectRight")

                    AccessRightsList.Add(RoleDetailsItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return AccessRightsList

    End Function

#End Region

#Region " CONSUMER "

    ''' <summary>
    ''' Function - GetConsumers;
    ''' 24 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="consumerDetails"></param>
    ''' <param name="sortExpression"></param>
    ''' <param name="sortDirection"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetConsumers(ByVal consumerDetails As ConsumerDetails, _
                                 Optional ByVal sortExpression As String = "", _
                                 Optional ByVal sortDirection As String = "") _
                                 As List(Of ConsumerDetails) Implements IService.GetConsumers

        Dim ConsumerList As New List(Of ConsumerDetails)
        Try

            Dim ConsumersRetrieved As New DataSet
            ConsumerList.Clear()

            ConsumersRetrieved = MasterListBL.GetConsumers(consumerDetails.StoreID, consumerDetails.ConsumerID, _
                                                           consumerDetails.ConsumerDescription, _
                                                           consumerDetails.ConsumerStatus)

            If sortExpression = String.Empty And sortDirection = String.Empty Then

                If ConsumersRetrieved.Tables(0).Rows.Count > 0 Then
                    For Each row As DataRow In ConsumersRetrieved.Tables(0).Rows
                        row = FillRowWithNull(row, ConsumersRetrieved.Tables(0).Columns)

                        Dim ConsumerDetailsItem As New ConsumerDetails

                        ConsumerDetailsItem.StoreID = row("ConsumerStoreID")
                        ConsumerDetailsItem.ConsumerID = row("ConsumerID")
                        ConsumerDetailsItem.ConsumerID_Description = row("ConsumerID") & " - " & row("ConsumerDescription")
                        ConsumerDetailsItem.ConsumerDescription = row("ConsumerDescription")
                        ConsumerDetailsItem.ConsumerStatus = row("ConsumerStatus")

                        ConsumerList.Add(ConsumerDetailsItem)

                    Next
                End If

            Else

                Dim ConsumersRetrievedView As New DataView(ConsumersRetrieved.Tables(0))
                ConsumersRetrievedView.Sort = sortExpression & " " & sortDirection

                If ConsumersRetrieved.Tables(0).Rows.Count > 0 Then
                    For Each viewRow As DataRowView In ConsumersRetrievedView

                        viewRow = FillViewRowWithNull(viewRow, ConsumersRetrievedView.Table.Columns)

                        Dim ConsumerDetailsItem As New ConsumerDetails

                        ConsumerDetailsItem.StoreID = viewRow("ConsumerStoreID")
                        ConsumerDetailsItem.ConsumerID = viewRow("ConsumerID")
                        ConsumerDetailsItem.ConsumerID_Description = viewRow("ConsumerID") & " - " & viewRow("ConsumerDescription")
                        ConsumerDetailsItem.ConsumerDescription = viewRow("ConsumerDescription")
                        ConsumerDetailsItem.ConsumerStatus = viewRow("ConsumerStatus")

                        ConsumerList.Add(ConsumerDetailsItem)

                    Next
                End If
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ConsumerList

    End Function

    ''' <summary>
    ''' Function - GetConsumerRef;
    ''' 20 Jan 2009 - Jianfa;
    ''' </summary>
    ''' <param name="consumerDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetConsumerRef(ByVal consumerDetails As ConsumerDetails) As List(Of ConsumerDetails) Implements IService.GetConsumerRef

        Dim ConsumerRefList As New List(Of ConsumerDetails)
        Try

            Dim ConsumerRefRetrieved As New DataSet
            ConsumerRefList.Clear()

            ConsumerRefRetrieved = MasterListBL.GetConsumerRef(consumerDetails.StoreID, consumerDetails.ConsumerRefStatus)

            If ConsumerRefRetrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In ConsumerRefRetrieved.Tables(0).Rows
                    row = FillRowWithNull(row, ConsumerRefRetrieved.Tables(0).Columns)

                    Dim ConsumerRefDetailsItem As New ConsumerDetails

                    ConsumerRefDetailsItem.StoreID = row("ConsumerStoreRefStoreID")
                    ConsumerRefDetailsItem.UserID = row("ConsumerStoreRefUserID")
                    ConsumerRefDetailsItem.ConsumerID = row("ConsumerStoreRefConsumerID")
                    ConsumerRefDetailsItem.ConsumerRefStatus = row("ConsumerStoreRefStatus")

                    ConsumerRefList.Add(ConsumerRefDetailsItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ConsumerRefList

    End Function

    ''' <summary>
    ''' Function - GetConsumerRefByUserID;
    ''' 20 Jan 2009 - Jianfa;
    ''' </summary>
    ''' <param name="consumerDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetConsumerRefByUserID(ByVal consumerDetails As ConsumerDetails) As List(Of ConsumerDetails) Implements IService.GetConsumerRefByUserID

        Dim ConsumerRefList As New List(Of ConsumerDetails)
        Try

            Dim ConsumerRefRetrieved As New DataSet
            ConsumerRefList.Clear()

            ConsumerRefRetrieved = MasterListBL.GetConsumerRefByUserID(consumerDetails.StoreID, consumerDetails.UserID, consumerDetails.UserRole, consumerDetails.ConsumerRefStatus)

            If ConsumerRefRetrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In ConsumerRefRetrieved.Tables(0).Rows
                    row = FillRowWithNull(row, ConsumerRefRetrieved.Tables(0).Columns)

                    Dim ConsumerRefDetailsItem As New ConsumerDetails

                    ConsumerRefDetailsItem.StoreID = row("ConsumerStoreRefStoreID")
                    ConsumerRefDetailsItem.UserID = row("ConsumerStoreRefUserID")
                    ConsumerRefDetailsItem.ConsumerID = row("ConsumerStoreRefConsumerID")
                    ConsumerRefDetailsItem.ConsumerDescription = row("ConsumerDescription")
                    ConsumerRefDetailsItem.ConsumerRefStatus = row("ConsumerStoreRefStatus")

                    ConsumerRefList.Add(ConsumerRefDetailsItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ConsumerRefList

    End Function

    ''' <summary>
    ''' Function - AddConsumer;
    ''' 24 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="consumerDetails"></param>
    ''' <param name="userList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddConsumer(ByVal consumerDetails As ConsumerDetails, _
                                ByVal userList As List(Of String)) _
                                As String Implements IService.AddConsumer

        Dim errorMessage As String = String.Empty

        Try

            errorMessage = MasterListBL.AddConsumer(consumerDetails.StoreID, consumerDetails.ConsumerID, _
                                     consumerDetails.ConsumerDescription, consumerDetails.LoginUser, _
                                     consumerDetails.ConsumerStatus, userList)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return errorMessage

    End Function

    ''' <summary>
    ''' Function - GetUsers;
    ''' 29 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="consumerDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetUsers(ByVal consumerDetails As ConsumerDetails) As List(Of ConsumerDetails) Implements IService.GetUsers

        Dim UserList As New List(Of ConsumerDetails)
        Try

            Dim UsersRetrieved As New DataSet
            UserList.Clear()

            UsersRetrieved = MasterListBL.GetUsers(consumerDetails.StoreID)

            If UsersRetrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In UsersRetrieved.Tables(0).Rows
                    row = FillRowWithNull(row, UsersRetrieved.Tables(0).Columns)

                    Dim UserItem As New ConsumerDetails

                    UserItem.UserID = row("UserRoleUserID")
                    UserItem.UserName = row("VUserProfileName")

                    UserList.Add(UserItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return UserList

    End Function

    ''' <summary>
    ''' Function - GetUserRef;
    ''' 29 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="consumerDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetUserRef(ByVal consumerDetails As ConsumerDetails) As List(Of ConsumerDetails) Implements IService.GetUserRef

        Dim UserRefList As New List(Of ConsumerDetails)
        Try

            Dim UsersRefRetrieved As New DataSet
            UserRefList.Clear()

            UsersRefRetrieved = MasterListBL.GetUserRef(consumerDetails.StoreID, consumerDetails.ConsumerID, _
                                                        consumerDetails.ConsumerRefStatus)

            If UsersRefRetrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In UsersRefRetrieved.Tables(0).Rows
                    row = FillRowWithNull(row, UsersRefRetrieved.Tables(0).Columns)

                    Dim UserItem As New ConsumerDetails

                    UserItem.UserID = row("ConsumerStoreRefUserID")
                    UserItem.UserName = row("VUserProfileName")

                    UserRefList.Add(UserItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return UserRefList

    End Function

    ''' <summary>
    ''' Function - UpdateConsumer;
    ''' 29 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="consumerDetails"></param>
    ''' <param name="userList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateConsumer(ByVal consumerDetails As ConsumerDetails, _
                                   ByVal userList As List(Of String)) As String Implements IService.UpdateConsumer

        Dim errorMessage As String = String.Empty

        Try

            errorMessage = MasterListBL.UpdateConsumer(consumerDetails.StoreID, consumerDetails.ConsumerID, _
                                     consumerDetails.ConsumerDescription, consumerDetails.LoginUser, _
                                     consumerDetails.ConsumerStatus, userList)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return errorMessage

    End Function

#End Region

    Public Function AddEmailContent(ByVal emailContent As EmailContent) As String Implements IService.AddEmailContent

        Dim ErrorMessage As String = ""
        Try

            ErrorMessage = MasterListBL.AddEmailContent(emailContent.StoreId, emailContent.EmailFormat, emailContent.ToAddress, emailContent.CCAddress, emailContent.Subject, emailContent.msgFormat, emailContent.FirstRemainder, emailContent.SecondRemainder, emailContent.LoginUser)


        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ErrorMessage
    End Function
End Class



