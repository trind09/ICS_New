Imports NEA_ICS.DataAccess
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

''' <summary>
''' Business Layer - for Master List;
''' 17 Dec 2008 - Kenny GOH, Jianfa CHEN;
''' </summary>
''' <remarks>
''' CHANGE LOG:
''' ddMMMyyyy AuthorName RefID Description;
''' 12Dec2008  KG      RefID  Create GetSuppliers;
''' 18Dec2008  Jianfa  RefID  Create AddSupplier;
''' 18Dec2008  KG      RefID  Modify AddSupplier;
''' 23Dec2008  Jianfa  RefID  Create GetCodeDescription;
''' </remarks>
Public Class MasterListBL

#Region " SUPPLIERS "
    ''' <summary>
    ''' Get Suppliers based on Parameters;
    ''' 12Dec2008, KG
    ''' </summary>
    ''' <param name="supplierId"></param>
    ''' <param name="companyName"></param>
    ''' <param name="status"></param>
    ''' <returns>Suppliers DataSet Collection</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function GetSuppliers( _
        ByVal storeId As String _
        , ByVal supplierId As String _
        , ByVal companyName As String _
        , ByVal status As String _
    ) As DataSet

        Dim Suppliers As New DataSet
        Try

            Suppliers = MasterListDAL.GetSuppliers(storeId, supplierId, companyName, status)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Suppliers
    End Function

    ''' <summary>
    ''' Add New Supplier Details;
    ''' 18Dec2008, Jianfa CHEN
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="supplierId"></param>
    ''' <param name="companyName"></param>
    ''' <param name="addressType"></param>
    ''' <param name="addressBlockHouseNo"></param>
    ''' <param name="addressStreetName"></param>
    ''' <param name="addressFloorNo"></param>
    ''' <param name="addressUnitNo"></param>
    ''' <param name="addressBuildingName"></param>
    ''' <param name="addressPostalCode"></param>
    ''' <param name="contactPerson"></param>
    ''' <param name="telephoneNo"></param>
    ''' <param name="faxNo"></param>
    ''' <param name="otherInformation"></param>
    ''' <param name="status"></param>
    ''' <param name="loginUser"></param>
    ''' <returns>Error Message (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' 18Dec2008  KG  RefID  Include check for unique Supplier ID and Company Name;
    ''' </remarks>
    Public Shared Function AddSupplier( _
            ByVal storeId As String _
            , ByVal supplierId As String _
            , ByVal companyName As String _
            , ByVal addressType As String _
            , ByVal addressBlockHouseNo As String _
            , ByVal addressStreetName As String _
            , ByVal addressFloorNo As String _
            , ByVal addressUnitNo As String _
            , ByVal addressBuildingName As String _
            , ByVal addressPostalCode As String _
            , ByVal contactPerson As String _
            , ByVal telephoneNo As String _
            , ByVal faxNo As String _
            , ByVal otherInformation As String _
            , ByVal status As String _
            , ByVal loginUser As String _
        ) As String

        Dim ErrorMessage As String = ""
        Try
            Dim Found As Boolean

            ' check for supplier details mandatory fields
            If storeId = "" _
                Or supplierId = "" _
                Or companyName = "" _
                Or addressBlockHouseNo = "" _
                Or addressStreetName = "" _
                Or addressPostalCode = "" _
                Or contactPerson = "" _
                Or telephoneNo = "" _
                Or status = "" _
                Or loginUser = "" _
            Then
                ErrorMessage = "Missing Mandatory Fields"

                ' Abort the process when Supplier Id already exists in the store
            Else : Found = MasterListDAL.CheckSupplierID(storeId, supplierId)
                If Found Then
                    ErrorMessage = "Supplier Code already exists."

                    ' Abort the process when Company Name already exists in the store
                Else : Found = MasterListDAL.CheckCompanyName(storeId, companyName)
                    If Found Then
                        ErrorMessage = "Company Name already exists. "

                        ' Insert supplier details
                    Else
                        MasterListDAL.InsertSupplier( _
                            storeId _
                            , supplierId _
                            , companyName _
                            , addressType _
                            , addressBlockHouseNo _
                            , addressStreetName _
                            , addressFloorNo _
                            , addressUnitNo _
                            , addressBuildingName _
                            , addressPostalCode _
                            , contactPerson _
                            , telephoneNo _
                            , faxNo _
                            , otherInformation _
                            , status _
                            , loginUser _
                        )
                    End If
                End If
            End If

        Catch ex As ApplicationException
            ErrorMessage = "Error: Supplier details was not inserted"
            Return ErrorMessage

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ErrorMessage
    End Function

    ''' <summary>
    ''' Update Supplier Details;
    ''' Jianfa CHEN, 19 Dec 2008;
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="supplierId"></param>
    ''' <param name="companyName"></param>
    ''' <param name="addressType"></param>
    ''' <param name="addressBlockHouseNo"></param>
    ''' <param name="addressStreetName"></param>
    ''' <param name="addressFloorNo"></param>
    ''' <param name="addressUnitNo"></param>
    ''' <param name="addressBuildingName"></param>
    ''' <param name="addressPostalCode"></param>
    ''' <param name="contactPerson"></param>
    ''' <param name="telephoneNo"></param>
    ''' <param name="faxNo"></param>
    ''' <param name="otherInformation"></param>
    ''' <param name="status"></param>
    ''' <param name="loginUser"></param>
    ''' <param name="originalCompanyName"></param>
    ''' <returns>Error Message (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' 21Dec2008  KG  RefID  Check the new Company Name (if change) is not already in use by others
    ''' </remarks>
    Public Shared Function UpdateSupplier( _
        ByVal storeId As String _
        , ByVal supplierId As String _
        , ByVal companyName As String _
        , ByVal addressType As String _
        , ByVal addressBlockHouseNo As String _
        , ByVal addressStreetName As String _
        , ByVal addressFloorNo As String _
        , ByVal addressUnitNo As String _
        , ByVal addressBuildingName As String _
        , ByVal addressPostalCode As String _
        , ByVal contactPerson As String _
        , ByVal telephoneNo As String _
        , ByVal faxNo As String _
        , ByVal otherInformation As String _
        , ByVal status As String _
        , ByVal loginUser As String _
        , ByVal originalCompanyName As String _
        , Optional ByRef UEN As String = "" _
    ) As String

        Dim ErrorMessage As String = ""
        Try
            Dim Found As Boolean

            If originalCompanyName <> companyName Then
                Found = MasterListDAL.CheckCompanyName(storeId, companyName, supplierId)
            Else
                Found = False
            End If

            If Found Then
                ErrorMessage = "Company Name already exists."
            Else
                MasterListDAL.UpdateSupplier( _
                     storeId _
                     , supplierId _
                     , companyName _
                     , addressType _
                     , addressBlockHouseNo _
                     , addressStreetName _
                     , addressFloorNo _
                     , addressUnitNo _
                     , addressBuildingName _
                     , addressPostalCode _
                     , contactPerson _
                     , telephoneNo _
                     , faxNo _
                     , otherInformation _
                     , loginUser _
                     , UEN _
                 )
            End If

        Catch ex As ApplicationException
            ErrorMessage = "Error: Supplier details was not updated"
            Return ErrorMessage

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ErrorMessage
    End Function

    ''' <summary>
    ''' Update Supplier Status. No Outstanding Orders to either Close or Delete Supplier;
    ''' KG, 19 Dec 2008;
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="supplierId"></param>
    ''' <param name="operationType"></param>
    ''' <param name="loginUser"></param>
    ''' <returns>Error Message (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function UpdateSupplierStatus( _
        ByVal storeId As String _
        , ByVal supplierId As String _
        , ByVal operationType As String _
        , ByVal loginUser As String _
    ) As String

        Dim ErrorMessage As String = ""
        Try
            Dim Outstanding As Boolean = False

            ' For Closing & Deleting Only, check there is no outstanding order
            If operationType = "C" Or operationType = "D" Then
                Outstanding = False

                'TODO: include method to do the check the outstanding order

                If Outstanding Then
                    ErrorMessage = "Error: There is still Outstanding Order(s) pending receiving"
                End If
            End If

            ' do update supplier status
            If Outstanding = False Then
                MasterListDAL.UpdateSupplierStatus( _
                    storeId _
                    , supplierId _
                    , operationType _
                    , loginUser _
                )
            End If

        Catch ex As ApplicationException
            ErrorMessage = "Error: Supplier details was not updated"
            Return ErrorMessage

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ErrorMessage
    End Function


#End Region

#Region " COMMON "

    ''' <summary>
    ''' Get Common Items Based On Parameters;
    ''' 23 Dec 2008 - Jianfa;
    ''' </summary>
    ''' <param name="status"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetCommon( _
        ByVal storeID As String _
        , ByVal codeGroup As String _
        , ByVal status As String _
        , Optional ByVal distinct As Boolean = False _
        ) As DataSet

        Dim Common As New DataSet

        Try

            If distinct Then
                Common = MasterListDAL.GetDistinctCommon(status)
            Else
                Common = IIf(codeGroup.Trim = String.Empty, _
                             MasterListDAL.GetCommon(storeID, status), _
                             MasterListDAL.GetCommonByGroup(storeID, codeGroup, status))
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Common

    End Function

    ''' <summary>
    ''' Function -  AddCommon;
    ''' 05 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="codeGroup"></param>
    ''' <param name="codeID"></param>
    ''' <param name="codeDescription"></param>
    ''' <param name="status"></param>
    ''' <param name="loginUser"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function AddCommon(ByVal storeID As String, _
                                  ByVal codeGroup As String, _
                                   ByVal codeID As String, _
                                   ByVal codeDescription As String, _
                                   ByVal status As String, _
                                   ByVal loginUser As String) As String

        Dim errorMessage As String = String.Empty

        Try

            MasterListDAL.InsertCommon(storeID, codeGroup, codeID, codeDescription, status, loginUser)

        Catch ex As ApplicationException
            errorMessage = "Error: Code details was not inserted"
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
    ''' Function - UpdateCommon;
    ''' 06 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="commonID"></param>
    ''' <param name="codeGroup"></param>
    ''' <param name="codeID"></param>
    ''' <param name="codeDescription"></param>
    ''' <param name="status"></param>
    ''' <param name="loginUser"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateCommon(ByVal storeID As String, ByVal commonID As Integer, _
                                        ByVal codeGroup As String, ByVal codeID As String, _
                                        ByVal codeDescription As String, ByVal status As String, _
                                        ByVal loginUser As String) As String

        Dim errorMessage As String = String.Empty

        Try

            MasterListDAL.UpdateCommon(storeID, commonID, codeGroup, codeID, codeDescription, status, loginUser)

        Catch ex As ApplicationException
            errorMessage = "Error: Code details was not updated"
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

#Region " EQUIPMENTS "

    ''' <summary>
    ''' To insert new equipment;
    ''' 29 Dec 2008 - Jianfa;
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="equipmentId"></param>
    ''' <param name="equipmentDescription"></param>
    ''' <param name="operationType"></param>
    ''' <param name="loginUser"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function AddEquipment(ByVal storeId As String, ByVal equipmentId As String, _
                                        ByVal equipmentType As String, _
                                        ByVal equipmentDescription As String, _
                                        ByVal operationType As String, ByVal loginUser As String) As String

        Dim ErrorMessage As String = ""

        Try

            Dim found As Boolean

            found = MasterListDAL.CheckEquipmentID(storeId, equipmentId)

            If found Then
                ErrorMessage = "Equipment Code already exists."
            Else

                MasterListDAL.InsertEquipment(storeId, equipmentId, equipmentType, _
                                              equipmentDescription, operationType, loginUser)

            End If

        Catch ex As ApplicationException
            ErrorMessage = "Error: Equipment details was not inserted"
            Return ErrorMessage

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ErrorMessage

    End Function

    ''' <summary>
    ''' To get equipments;
    ''' 30 Dec 2008 - Jianfa
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="equipmentId"></param>
    ''' <param name="equipmentDescription"></param>
    ''' <param name="status"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetEquipments(ByVal storeId As String, ByVal equipmentId As String, _
                                         ByVal equipmentType As String, _
                                         ByVal equipmentDescription As String, _
                                         ByVal status As String) As DataSet

        Dim Equipments As New DataSet
        Try

            Equipments = MasterListDAL.GetEquipments(storeId, equipmentId, equipmentType, _
                                                     equipmentDescription, status)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Equipments

    End Function

    ''' <summary>
    ''' To update equipment
    ''' 30 Dec 2008 - Jianfa
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="equipmentId"></param>
    ''' <param name="equipmentDescription"></param>
    ''' <param name="loginUser"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateEquipment( _
       ByVal storeId As String _
       , ByVal equipmentId As String _
       , ByVal equipmentType As String _
       , ByVal equipmentDescription As String _
       , ByVal loginUser As String _
   ) As String

        Dim ErrorMessage As String = ""
        Try

            MasterListDAL.UpdateEquipment(storeId, equipmentId, equipmentType, equipmentDescription, loginUser)

        Catch ex As ApplicationException
            ErrorMessage = "Error: Supplier details was not updated"
            Return ErrorMessage

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ErrorMessage
    End Function

    ''' <summary>
    ''' Function - Update Equipment Status;
    ''' 31 Dec 2008 - Jianfa;
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="equipmentId"></param>
    ''' <param name="operationType"></param>
    ''' <param name="loginUser"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateEquipmentStatus( _
        ByVal storeId As String _
        , ByVal equipmentId As String _
        , ByVal operationType As String _
        , ByVal loginUser As String _
    ) As String

        Dim ErrorMessage As String = ""
        Try
            Dim Outstanding As Boolean = False

            ' For Closing & Deleting Only, check there is no outstanding order
            If operationType = "C" Or operationType = "D" Then
                Outstanding = False

                'TODO: include method to do the check the outstanding order
                '      (NOT VERY SURE WHETHER CAN CLOSE/DELETE EQUIPMENT)
                ' Kenny - FYNA

                If Outstanding Then
                    ErrorMessage = "Error: There is still Outstanding Order(s) pending receiving"
                End If
            End If

            ' do update equipment status
            If Outstanding = False Then
                MasterListDAL.UpdateEquipmentStatus( _
                    storeId _
                    , equipmentId _
                    , operationType _
                    , loginUser _
                )
            End If

        Catch ex As ApplicationException
            ErrorMessage = "Error: Equipment details was not updated"
            Return ErrorMessage

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ErrorMessage
    End Function


#End Region

#Region " STORES "

    ''' <summary>
    ''' Function - Add Store
    ''' 01 Jan 2008 - Jianfa;
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="storeName"></param>
    ''' <param name="addressType"></param>
    ''' <param name="blockHouseNo"></param>
    ''' <param name="streetName"></param>
    ''' <param name="floorNo"></param>
    ''' <param name="unitNo"></param>
    ''' <param name="buildingName"></param>
    ''' <param name="postalCode"></param>
    ''' <param name="contactPerson"></param>
    ''' <param name="telephoneNo"></param>
    ''' <param name="faxNo"></param>
    ''' <param name="otherInfo"></param>
    ''' <param name="status"></param>
    ''' <param name="loginUser"></param>
    ''' <param name="userRoles"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' 1) Validate unique store ID
    ''' 2) Begin inserting Store and Module Ref based on rol groups
    ''' </remarks>
    Public Shared Function AddStore(ByVal storeId As String, ByVal storeName As String, _
                                    ByVal addressType As String, ByVal blockHouseNo As String, _
                                    ByVal streetName As String, ByVal floorNo As String, _
                                    ByVal unitNo As String, ByVal buildingName As String, _
                                    ByVal postalCode As String, ByVal contactPerson As String, _
                                    ByVal telephoneNo As String, ByVal faxNo As String, _
                                    ByVal otherInfo As String, ByVal status As String, _
                                    ByVal loginUser As String, ByVal userRoles As String) As String

        Dim errorMessage As String = ""

        Try

            Dim found As Boolean = MasterListDAL.CheckStoreID(storeId)

            If found Then
                errorMessage = "Store Code already exists."
            Else : found = MasterListDAL.CheckStoreName(storeName)

                If found Then
                    errorMessage = "Store Name already exists."
                Else
                    MasterListDAL.InsertStore(storeId, storeName, addressType, blockHouseNo, streetName, floorNo, _
                                          unitNo, buildingName, postalCode, contactPerson, telephoneNo, faxNo, _
                                          otherInfo, status, loginUser, userRoles)
                End If


            End If

        Catch ex As ApplicationException
            errorMessage = "Error: Store details was not inserted"
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
    ''' Function - Get Stores;
    ''' 01 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="storeName"></param>
    ''' <param name="status"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetStores(ByVal storeId As String, ByVal storeName As String, _
                                    ByVal status As String) As DataSet

        Dim Stores As New DataSet
        Try

            Stores = MasterListDAL.GetStores(storeId, storeName, status)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Stores

    End Function

    ''' <summary>
    ''' Function - Update Store;
    ''' 01 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="storeName"></param>
    ''' <param name="addressType"></param>
    ''' <param name="blockHouseNo"></param>
    ''' <param name="streetName"></param>
    ''' <param name="floorNo"></param>
    ''' <param name="unitNo"></param>
    ''' <param name="buildingName"></param>
    ''' <param name="postalCode"></param>
    ''' <param name="contactPerson"></param>
    ''' <param name="telephoneNo"></param>
    ''' <param name="faxNo"></param>
    ''' <param name="otherInfo"></param>
    ''' <param name="originalStoreName"></param>
    ''' <param name="loginUser"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateStore(ByVal storeId As String, ByVal storeName As String, _
                                    ByVal addressType As String, ByVal blockHouseNo As String, _
                                    ByVal streetName As String, ByVal floorNo As String, _
                                    ByVal unitNo As String, ByVal buildingName As String, _
                                    ByVal postalCode As String, ByVal contactPerson As String, _
                                    ByVal telephoneNo As String, ByVal faxNo As String, _
                                    ByVal otherInfo As String, ByVal originalStoreName As String, _
                                    ByVal loginUser As String) As String

        Dim ErrorMessage As String = ""
        Dim found As Boolean

        Try

            If originalStoreName <> storeName Then
                found = MasterListDAL.CheckStoreName(storeName)
            Else
                found = False
            End If

            If found Then
                ErrorMessage = "Store Name already exists."
            Else

                MasterListDAL.UpdateStore(storeId, storeName, addressType, blockHouseNo, streetName, _
                                          floorNo, unitNo, buildingName, postalCode, contactPerson, _
                                          telephoneNo, faxNo, otherInfo, loginUser)
            End If

        Catch ex As ApplicationException
            ErrorMessage = "Error: Store details was not updated"
            Return ErrorMessage

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
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
    ''' <param name="storeId"></param>
    ''' <param name="operationType"></param>
    ''' <param name="loginUser"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateStoreStatus( _
        ByVal storeId As String _
        , ByVal operationType As String _
        , ByVal loginUser As String _
    ) As String

        Dim ErrorMessage As String = ""
        Try

            Dim Valid As Boolean = False

            Select Case operationType
                Case "C"

                    Valid = MasterListDAL.CheckStoreValue(storeId)

                    '-- If Stock Value is Not Zero
                    If Not Valid Then
                        ErrorMessage = "Unable to close Store Code " & storeId & ". \nStore Value is not Zero (0)."

                        Return ErrorMessage
                        Exit Function
                    End If

                    Valid = MasterListDAL.CheckZeroStock(storeId)

                    '-- If Stock Balance is Zero
                    If Not Valid Then
                        ErrorMessage = "Unable to close Store Code " & storeId & ". \nStock Balance is not Zero (0)."

                        Return ErrorMessage
                        Exit Function
                    End If

                    MasterListDAL.UpdateStoreStatus(storeId, operationType, "C", loginUser)

                Case "O"
                    MasterListDAL.UpdateStoreStatus(storeId, operationType, "O", loginUser)
                Case "D"
                    MasterListDAL.UpdateStoreStatus(storeId, operationType, "D", loginUser)
            End Select


        Catch ex As ApplicationException
            ErrorMessage = "Error: Store status was not updated"
            Return ErrorMessage

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ErrorMessage
    End Function

#End Region

#Region " ITEM "

    ''' <summary>
    ''' Function -  GeneratedItemID
    ''' 07 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="itemID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GenerateItemID(ByVal itemID As String, ByVal storeID As String, ByRef generatedItemID As String) As String

        Dim errorMessage As String = String.Empty

        Try

            generatedItemID = MasterListDAL.GenerateItemID(itemID, storeID)

        Catch ex As ApplicationException
            errorMessage = "Error: Stock Item Code could not be generated."
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
    ''' Function - AddItem;
    ''' 09 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="itemID"></param>
    ''' <param name="equipmentID"></param>
    ''' <param name="itemDescription"></param>
    ''' <param name="partNo"></param>
    ''' <param name="stockType"></param>
    ''' <param name="subType"></param>
    ''' <param name="UOM"></param>
    ''' <param name="location"></param>
    ''' <param name="location2"></param>
    ''' <param name="minLevel"></param>
    ''' <param name="reOrderLevel"></param>
    ''' <param name="maxLevel"></param>
    ''' <param name="openingBalance"></param>
    ''' <param name="openingTotalValue"></param>
    ''' <param name="status"></param>
    ''' <param name="loginUser"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function AddItem(ByVal storeID As String, _
                              ByVal itemID As String, _
                              ByVal equipmentID As String, _
                              ByVal itemDescription As String, _
                              ByVal partNo As String, _
                              ByVal stockType As String, _
                              ByVal subType As String, _
                              ByVal UOM As String, _
                              ByVal location As String, _
                              ByVal location2 As String, _
                              ByVal minLevel As Decimal, _
                              ByVal reOrderLevel As Decimal, _
                              ByVal maxLevel As Decimal, _
                              ByVal openingBalance As Decimal, _
                              ByVal openingTotalValue As Decimal, _
                              ByVal status As String, _
                              ByVal loginUser As String) As String

        Dim errorMessage As String = String.Empty

        Try

            Dim found As Boolean = MasterListDAL.CheckItemID(storeID, itemID)

            If found Then
                errorMessage = "Stock Code already exists."
            Else
                MasterListDAL.InsertItem(storeID, itemID, equipmentID, itemDescription, partNo, _
                                     stockType, subType, UOM, location, location2, minLevel, _
                                     reOrderLevel, maxLevel, openingBalance, openingTotalValue, _
                                     status, loginUser)
            End If

        Catch ex As ApplicationException
            errorMessage = "Error: Stock Item was not inserted."
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
    ''' Function - GetItems
    ''' 08 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="itemID"></param>
    ''' <param name="location"></param>
    ''' <param name="status"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetItems(ByVal storeID As String, _
                                    ByVal itemID As String, _
                                    ByVal location As String, _
                                    ByVal status As String, _
                                    ByVal equipmentID As String, _
                                    ByVal itemDescription As String) As DataSet
        Dim Items As New DataSet

        Try

            Items = MasterListDAL.GetItems(storeID, itemID, location, status, equipmentID, itemDescription)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Items

    End Function

    ''' <summary>
    ''' Function - GetItemsMasterList
    ''' 3 Mar 2009 - Guo Feng
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="stockCodeFrom"></param>
    ''' <param name="stockCodeTo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetItemsMasterList(ByVal storeID As String, _
                      ByVal stockCodeFrom As String, _
                      ByVal stockCodeTo As String, _
                      ByVal itemStatus As String) As DataSet
        Dim Items As New DataSet

        Try

            Items = MasterListDAL.GetItemsMasterList(storeID, stockCodeFrom, stockCodeTo, itemStatus)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Items
    End Function

    ''' <summary>
    ''' Function - GetStockTransaction;
    ''' 10 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="itemID"></param>
    ''' <param name="transactionType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetStockTransaction(ByVal storeID As String, ByVal itemID As String, ByVal transactionType As String) As DataSet

        Dim Items As New DataSet

        Try

            Items = MasterListDAL.GetStockTransaction(storeID, itemID, transactionType)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Items

    End Function

    ''' <summary>
    ''' Function - UpdateItem
    ''' 10 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="itemID"></param>
    ''' <param name="equipmentID"></param>
    ''' <param name="itemDescription"></param>
    ''' <param name="partNo"></param>
    ''' <param name="stockType"></param>
    ''' <param name="subType"></param>
    ''' <param name="UOM"></param>
    ''' <param name="location"></param>
    ''' <param name="location2"></param>
    ''' <param name="minLevel"></param>
    ''' <param name="reOrderLevel"></param>
    ''' <param name="maxLevel"></param>
    ''' <param name="openingBalance"></param>
    ''' <param name="openingTotalValue"></param>
    ''' <param name="status"></param>
    ''' <param name="loginUser"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateItem(ByVal storeID As String, _
                              ByVal itemID As String, _
                              ByVal equipmentID As String, _
                              ByVal itemDescription As String, _
                              ByVal partNo As String, _
                              ByVal stockType As String, _
                              ByVal subType As String, _
                              ByVal UOM As String, _
                              ByVal location As String, _
                              ByVal location2 As String, _
                              ByVal minLevel As Decimal, _
                              ByVal reOrderLevel As Decimal, _
                              ByVal maxLevel As Decimal, _
                              ByVal openingBalance As Decimal, _
                              ByVal openingTotalValue As Decimal, _
                              ByVal status As String, _
                              ByVal loginUser As String, _
                              ByRef returnMessage As String) As String

        Dim errorMessage As String = String.Empty
        Dim TransactionList As DataTable
        Dim StockQtyBalance As Decimal = CDec(openingBalance)
        Dim FinalStockQtyBalance As Decimal = 0.0
        Dim CurrentStockQtyBalance As Decimal = 0.0
        Dim StockQtyBalanceDiff As Decimal = 0.0

        Try

            '-- If there are no opening balance and opening total value to be updated.
            If openingBalance = -1.0 And openingTotalValue = -1.0 Then

                MasterListDAL.UpdateItem(storeID, itemID, equipmentID, itemDescription, partNo, _
                                         stockType, subType, UOM, location, location2, minLevel, _
                                         reOrderLevel, maxLevel, openingBalance, openingTotalValue, _
                                         status, loginUser)

                Return errorMessage
                Exit Function

            End If

            TransactionList = MasterListDAL.CheckStockBalanceQty(storeID, itemID)

            '-- If the top row does not indicate an "Open" opening balance. Return an alert message
            If TransactionList.Rows(0).Item("FCalcTransByStockType").ToString.ToUpper <> "BALANCE" Then

                returnMessage = "Unable to update Stock Code " & itemID & ". \nOpening Balance has been locked for Financial Closing."

                Return errorMessage
                Exit Function

            Else

                For Each row As DataRow In TransactionList.Rows

                    If row.Item("FCalcTransByStockType").ToString.ToUpper <> "BALANCE" Then

                        '-- Increment/Decrement qty balance based on receive/issue/order
                        StockQtyBalance += row.Item("FCalcTransByStockQty")
                        FinalStockQtyBalance = row.Item("FCalcTransByStockBalanceQty")

                    Else

                        '-- Assign Current Stock Qty Opening Balance
                        CurrentStockQtyBalance = row.Item("FCalcTransByStockBalanceQty")

                    End If

                    '-----------------------------------------------------------------------------
                    '-- EXCEPTION (1):
                    '-- If Calculation occurs a negative value. Return an alert message.
                    '-----------------------------------------------------------------------------
                    If StockQtyBalance < 0 Then

                        returnMessage = "Unable to update Stock Code " & itemID & ". \nCalculation for Stock Qty has hit a (-) Negative Value."

                        Return errorMessage
                        Exit Function

                    End If
                    '------------------------------------------------------------------------------

                Next

                StockQtyBalanceDiff = openingBalance - CurrentStockQtyBalance

                '----------------------------------------------------------------------------------
                '-- EXCEPTION (2):
                '-- If Calculation has burst its Maximum Level. Return an alert message. 
                '----------------------------------------------------------------------------------
                If (StockQtyBalanceDiff + FinalStockQtyBalance) > maxLevel Then

                    returnMessage = "Unable to update Stock Code " & itemID & ". \nCalculation for Stock Qty has exceeded its Maximum Level."

                    Return errorMessage
                    Exit Function

                Else

                    MasterListDAL.UpdateItem(storeID, itemID, equipmentID, itemDescription, partNo, _
                                            stockType, subType, UOM, location, location2, minLevel, _
                                            reOrderLevel, maxLevel, openingBalance, openingTotalValue, _
                                            status, loginUser)

                End If

            End If

        Catch ex As ApplicationException
            errorMessage = "Error: Stock Item was not updated."
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
    ''' Function - UpdateItemStatus;
    ''' 12 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="itemID"></param>
    ''' <param name="status"></param>
    ''' <param name="loginUser"></param>
    ''' <param name="returnMessage"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateItemStatus(ByVal storeID As String, ByVal itemID As String, _
                                            ByVal status As String, ByVal loginUser As String, _
                                            ByRef returnMessage As String) As String

        Dim found As Boolean = False
        Dim valid As Boolean = False

        Try

            Select Case status
                Case "C" 'Close

                    found = MasterListDAL.CheckTransactionMonth(storeID, itemID)

                    '-- If there are transaction movement, return an alert message
                    If found Then

                        returnMessage = "Unable to close Stock Code " & itemID & ". \nThere are ongoing stock transactions found in this Item. "
                        Return returnMessage
                        Exit Function

                    End If

                    valid = MasterListDAL.CheckZeroQtyCost(storeID, itemID)

                    '-- If total qty and value <> zero, return an alert, message
                    If valid Then
                        MasterListDAL.UpdateItemStatus(storeID, itemID, status, loginUser)
                    Else
                        returnMessage = "Unable to close Stock Code " & itemID & ". \nStock Qty Balance or Stock Total Value is not Zero (0). "
                        Return returnMessage
                        Exit Function
                    End If               

                Case "O" 'Open

                    MasterListDAL.UpdateItemStatus(storeID, itemID, status, loginUser)

                Case "D" 'Delete

                    found = MasterListDAL.CheckTransactionMovement(storeID, itemID)

                    '-- If there are transaction movement, return an alert message
                    If found Then

                        returnMessage = "Unable to delete Stock Code " & itemID & ". \nThere are Stock Transactions found in this item. "
                        Return returnMessage
                        Exit Function

                    End If

                    MasterListDAL.DeleteItem(storeID, itemID, status, loginUser)

            End Select

        Catch ex As ApplicationException

            Select Case status
                Case "D"
                    returnMessage = "Error: Stock Item Status was not deleted."
                Case Else
                    returnMessage = "Error: Stock Item Status was not updated."
            End Select

            Return returnMessage

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
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
    ''' <param name="storeID"></param>
    ''' <param name="itemID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetItemSearch(ByVal storeID As String, ByVal itemID As String) As DataSet

        Dim Items As New DataSet

        Try

            Items = MasterListDAL.GetItemSearch(storeID, itemID)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Items

    End Function

    ''' <summary>
    ''' 01 May 09 - Jianfa
    ''' Function - CheckItemID
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="itemID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CheckItemID(ByVal storeID As String, ByVal itemID As String) As Boolean

        Try

            Dim found As Boolean = MasterListDAL.CheckItemID(storeID, itemID)

            If found Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return False

    End Function

    ''' <summary>
    ''' 28 Feb 2011 - Jianfa
    ''' Function - Check Item Status 
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="itemID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CheckItemStatus(ByVal storeID As String, ByVal itemID As String) As Boolean

        Try

            Dim valid As Boolean = MasterListDAL.CheckItemStatus(storeID, itemID)

            If valid Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return False

    End Function


#End Region

#Region " ROLES "

    ''' <summary>
    ''' Function - GetModuleRoles;
    ''' 16 Jan 2009 - Jianfa; 
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="role"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetModuleRoles(ByVal storeID As String, ByVal role As String) As DataSet

        Dim Roles As New DataSet

        Try

            Roles = MasterListDAL.GetModuleRoles(storeID, role)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Roles

    End Function

    ''' <summary>
    ''' Function - GetUserRoles;
    ''' 18 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="role"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetUserRoles(ByVal storeID As String, ByVal role As String) As DataSet

        Dim Roles As New DataSet

        Try

            Roles = MasterListDAL.GetUserRoles(storeID, role)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Roles

    End Function


    ''' <summary>
    ''' Function - UpdateModuleRoles;
    ''' 16 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="role"></param>
    ''' <param name="moduleID"></param>
    ''' <param name="selectRight"></param>
    ''' <param name="insertRight"></param>
    ''' <param name="updateRight"></param>
    ''' <param name="deleteRight"></param>
    ''' <param name="loginUser"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateModuleRoles(ByVal storeID As String, ByVal role As String, _
                                             ByVal moduleID As Integer, ByVal selectRight As Boolean, _
                                             ByVal insertRight As Boolean, ByVal updateRight As Boolean, _
                                             ByVal deleteRight As Boolean, ByVal status As String, _
                                             ByVal loginUser As String) As String

        Dim errorMessage As String = String.Empty
        Dim found As Boolean

        Try

            found = MasterListDAL.CheckModuleRoles(storeID, role, moduleID)

            If found Then
                MasterListDAL.UpdateModuleRoles(storeID, role, moduleID, selectRight, insertRight, updateRight, _
                                                deleteRight, status, loginUser)
            Else
                MasterListDAL.InsertModuleRoles(storeID, role, moduleID, selectRight, insertRight, updateRight, _
                                                deleteRight, status, loginUser)
            End If

        Catch ex As ApplicationException
            errorMessage = "Error: Module Role details was not updated"
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
    ''' Function - UpdateUserRole;
    ''' 22 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="userID"></param>
    ''' <param name="role"></param>
    ''' <param name="status"></param>
    ''' <param name="loginUser"></param>
    ''' <param name="consumerList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateUserRole(ByVal storeID As String, ByVal userID As String, ByVal soeID As String,
                                          ByVal isUserDeleted As Boolean, ByVal changeReason As String,
                                          ByVal role As String, ByVal status As String,
                                          ByVal loginUser As String,
                                          ByVal consumerList As List(Of String)) As String

        Dim errorMessage As String = String.Empty

        Try

            MasterListDAL.UpdateUserRole(storeID, userID, soeID, isUserDeleted, changeReason, role, status, loginUser, consumerList)

        Catch ex As ApplicationException

            errorMessage = "Error: User Role details was not updated"
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
    ''' Function - CheckNRIC;
    ''' 23 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="userID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CheckNRIC(ByVal userID As String) As DataSet

        Dim Roles As New DataSet

        Try

            Roles = MasterListDAL.CheckUserID(userID)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Roles

    End Function

    ''' <summary>
    ''' Function - AddUserRole;
    ''' 24 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="userID"></param>
    ''' <param name="role"></param>
    ''' <param name="status"></param>
    ''' <param name="loginUser"></param>
    ''' <param name="consumerList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function AddUserRole(ByVal storeID As String, ByVal userID As String, ByVal soeID As String,
                                          ByVal role As String, ByVal status As String,
                                          ByVal loginUser As String,
                                          ByVal consumerList As List(Of String)) As String

        Dim errorMessage As String = String.Empty
        Dim found As Boolean

        Try

            found = MasterListDAL.ValidNRIC(userID)

            If Not found Then
                errorMessage = "Invalid NEA User ID."
                Return errorMessage

            Else : found = MasterListDAL.ValidUserID(storeID, userID, role)

                If found Then

                    errorMessage = "User ID already exists for the role."
                    Return errorMessage

                Else

                    MasterListDAL.InsertUserRole(storeID, userID, soeID, role, status, loginUser, consumerList)

                End If
            End If

        Catch ex As ApplicationException

            errorMessage = "Error: User Role details was not inserted"
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
    ''' Function - DeleteUserRole;
    ''' 26 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="userID"></param>
    ''' <param name="role"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteUserRole(ByVal storeID As String, ByVal userID As String, ByVal role As String) As String

        Dim errorMessage As String = String.Empty

        Try

            MasterListDAL.DeleteUserRole(storeID, userID, role)

        Catch ex As ApplicationException

            errorMessage = "Error: User Role details was not deleted"
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
    ''' Function - GetUserRoleIDBySoeID;
    ''' 10 Sep 21 - Nguyen Dung Tri;
    ''' </summary>
    ''' <param name="soeID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetUserRoleIDBySoeID(ByVal soeID As String) As DataSet

        Try

            Dim RolesRetrieved As New DataSet

            RolesRetrieved = MasterListDAL.GetUserRoleIDBySoeID(soeID)

            If RolesRetrieved.Tables(0).Rows.Count > 0 Then
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
    ''' <param name="storeID"></param>
    ''' <param name="userID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetModuleAccess(ByVal storeID As String, ByVal userID As String) As DataSet

        Dim ModuleAccess As New DataSet

        Try

            ModuleAccess = MasterListDAL.GetModuleAccess(storeID, userID)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ModuleAccess

    End Function

    ''' <summary>
    ''' Function - GetModuleAccessRights
    ''' 11 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="userID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetModuleAccessRights(ByVal storeID As String, ByVal userID As String)

        Dim ModuleAccessRights As New DataSet

        Try

            ModuleAccessRights = MasterListDAL.GetModuleAccessRights(storeID, userID)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ModuleAccessRights

    End Function

#End Region

#Region " CONSUMERS "

    ''' <summary>
    ''' Function - GetConsumers;
    ''' 20 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="status"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetConsumers(ByVal storeID As String, ByVal consumerID As String, _
                                        ByVal consumerDescription As String, ByVal status As String) As DataSet

        Dim Consumers As New DataSet

        Try

            Consumers = MasterListDAL.GetConsumers(storeID, consumerID, consumerDescription, status)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Consumers

    End Function

    ''' <summary>
    ''' Function - GetConsumerRef;
    ''' 20 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="status"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetConsumerRef(ByVal storeID As String, ByVal status As String) As DataSet

        Dim ConsumerRef As New DataSet

        Try

            ConsumerRef = MasterListDAL.GetConsumerRef(storeID, status)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ConsumerRef

    End Function

    ''' <summary>
    ''' Function - GetConsumerRefByUserID;
    ''' 20 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="status"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetConsumerRefByUserID(ByVal storeID As String, ByVal userID As String, _
                                        ByVal role As String, ByVal status As String) As DataSet

        Dim ConsumerRef As New DataSet

        Try

            ConsumerRef = MasterListDAL.GetConsumerRefByUserID(storeID, userID, role, status)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ConsumerRef

    End Function

    ''' <summary>
    ''' Function - AddConsumer;
    ''' 27 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="consumerId"></param>
    ''' <param name="consumerDescription"></param>
    ''' <param name="loginUser"></param>
    ''' <param name="status"></param>
    ''' <param name="userList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function AddConsumer(ByVal storeId As String, ByVal consumerId As String, _
                                     ByVal consumerDescription As String, ByVal loginUser As String, _
                                     ByVal status As String, _
                                     ByVal userList As List(Of String))

        Dim errorMessage As String = String.Empty
        Dim found As Boolean

        Try

            found = MasterListDAL.CheckConsumerID(storeId, consumerId)

            If found Then

                errorMessage = "Consumer Code already exists."

            Else

                MasterListDAL.InsertConsumer(storeId, consumerId, consumerDescription, loginUser, status, _
                                             userList)

            End If


        Catch ex As ApplicationException

            errorMessage = "Error: Consumer details was not inserted"
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
    ''' Function - GetUsers;
    ''' 29 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetUsers(ByVal storeID As String) As DataSet

        Dim Users As New DataSet

        Try

            Users = MasterListDAL.GetUsers(storeID)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Users

    End Function

    ''' <summary>
    ''' Function - GetUserRef;
    ''' 29 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="consumerID"></param>
    ''' <param name="status"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetUserRef(ByVal storeID As String, ByVal consumerID As String, _
                                      ByVal status As String) As DataSet

        Dim UsersRef As New DataSet

        Try

            UsersRef = MasterListDAL.GetUsersRef(storeID, consumerID, status)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return UsersRef

    End Function

    ''' <summary>
    ''' Function - UpdateConsumer;
    ''' 29 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="consumerId"></param>
    ''' <param name="consumerDescription"></param>
    ''' <param name="loginUser"></param>
    ''' <param name="status"></param>
    ''' <param name="userList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateConsumer(ByVal storeId As String, ByVal consumerId As String, _
                                     ByVal consumerDescription As String, ByVal loginUser As String, _
                                     ByVal status As String, _
                                     ByVal userList As List(Of String)) As String

        Dim errorMessage As String = String.Empty

        Try

            If status.ToUpper = "D" Then

                MasterListDAL.DeleteConsumer(storeId, consumerId)

            Else

                MasterListDAL.UpdateConsumer(storeId, consumerId, consumerDescription, loginUser, status, _
                                             userList)
            End If


        Catch ex As ApplicationException

            errorMessage = "Error: Consumer details was not updated."
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

#Region "EMAIl CONTENT"



    Public Shared Function AddEmailContent(ByVal storeID As String, _
                                         ByVal emailFormat As String, _
                              ByVal toAddr As String, _
                              ByVal ccAddr As String, _
                              ByVal subject As String, _
                              ByVal msgFormat As String, _
                              ByVal firstRemainder As String, _
                              ByVal secondRemainder As String, _
                              ByVal loginUser As String) As String


        Dim errorMessage As String = ""

        Try


            MasterListDAL.AddEmailContent(storeID, emailFormat, toAddr, ccAddr, subject, msgFormat, firstRemainder, secondRemainder, loginUser)





        Catch ex As ApplicationException
            errorMessage = "Error: Email Content was not inserted"
            Return errorMessage

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Business Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return errorMessage

    End Function

    Public Shared Function GetEmailContent(ByVal storeID As String, ByVal emailFormat As String) As DataTable
        Return MasterListDAL.GetEmailContent(storeID, emailFormat)

    End Function
#End Region

End Class
