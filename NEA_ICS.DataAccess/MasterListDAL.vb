Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

''' <summary>
''' Data Access Layer - for Master List
''' 12Dec2008, KG, Jianfa Chen
''' </summary>
''' <remarks>
''' CHANGE LOG:
''' 21Dec2008  KG RefID  Include the Exception Handling to all methods;
''' </remarks>
Public Class MasterListDAL

#Region " SUPPLIERS "
    ''' <summary>
    ''' Get Suppliers based on Parameters;
    ''' 12Dec2008, KG
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="supplierId"></param>
    ''' <param name="companyName"></param>
    ''' <param name="status"></param>
    ''' <returns>Suppliers DataSet</returns>
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
            Dim sqlStoredProc As String = "spSelectSuppliers"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
                    db.AddInParameter(dbCommand, "@strSupplierId", DbType.String, supplierId)
                    db.AddInParameter(dbCommand, "@strCompanyName", DbType.String, companyName)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)

                    Suppliers.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "Supplier")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Suppliers

    End Function

    ''' <summary>
    ''' Check Supplier id, if found return TRUE;
    ''' 18Dec08, Jianfa CHEN
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="supplierId"></param>
    ''' <returns>True = Found; False = Not Found</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function CheckSupplierID( _
        ByVal storeId As String _
        , ByVal supplierId As String _
    ) As Boolean

        Try
            Dim sqlStoredProc As String = "spCheckSupplierID"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
                    db.AddInParameter(dbCommand, "@strSupplierId", DbType.String, supplierId)

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
    ''' Check Company Name, if found return TRUE;
    ''' If Supplier Id is provided, its Company Name will be excluded from the check;
    ''' 18Dec08, KG
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="companyName"></param>
    ''' <returns>True = Found; False = Not Found</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' 21Dec2008  KG  RefID  Include SupplierId as optional, when a value is provided the Company Name of this ID will not be checked;
    ''' </remarks>
    Public Shared Function CheckCompanyName( _
        ByVal storeId As String _
        , ByVal companyName As String _
        , Optional ByVal supplierId As String = "" _
    ) As Boolean

        Try
            Dim sqlStoredProc As String = "spCheckCompanyName"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
                    db.AddInParameter(dbCommand, "@strCompanyName", DbType.String, companyName)
                    db.AddInParameter(dbCommand, "@strSupplierId", DbType.String, supplierId)
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
    ''' To add new supplier;
    ''' 18Dec08, Jianfa CHEN
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
    ''' <param name="UEN"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Sub InsertSupplier( _
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
        , Optional ByVal UEN As String = "" _
    )

        Try

            Dim sqlStoredProc As String = "spInsertSupplier"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
                    db.AddInParameter(dbCommand, "@strSupplierId", DbType.String, supplierId)
                    db.AddInParameter(dbCommand, "@strUEN", DbType.String, UEN)
                    db.AddInParameter(dbCommand, "@strCompanyName", DbType.String, companyName)
                    db.AddInParameter(dbCommand, "@strAddressType", DbType.String, addressType)
                    db.AddInParameter(dbCommand, "@strBlockHouseNo", DbType.String, addressBlockHouseNo)
                    db.AddInParameter(dbCommand, "@strStreetName", DbType.String, addressStreetName)
                    db.AddInParameter(dbCommand, "@strFloorNo", DbType.String, addressFloorNo)
                    db.AddInParameter(dbCommand, "@strUnitNo", DbType.String, addressUnitNo)
                    db.AddInParameter(dbCommand, "@strBuildingName", DbType.String, addressBuildingName)
                    db.AddInParameter(dbCommand, "@strPostalCode", DbType.String, addressPostalCode)
                    db.AddInParameter(dbCommand, "@strContactPerson", DbType.String, contactPerson)
                    db.AddInParameter(dbCommand, "@strTelephoneNo", DbType.String, telephoneNo)
                    db.AddInParameter(dbCommand, "@strFaxNo", DbType.String, faxNo)
                    db.AddInParameter(dbCommand, "@strOtherInfo", DbType.String, otherInformation)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
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
    ''' Update Supplier;
    ''' 19 Dec 2008, Jianfa; 
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
    ''' <param name="loginUser"></param>
    ''' <param name="UEN"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Sub UpdateSupplier( _
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
        , ByVal loginUser As String _
        , Optional ByVal UEN As String = "" _
    )

        Try
            Dim sqlStoredProc As String = "spUpdateSupplier"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
                    db.AddInParameter(dbCommand, "@strSupplierId", DbType.String, supplierId)
                    db.AddInParameter(dbCommand, "@strUEN", DbType.String, UEN)
                    db.AddInParameter(dbCommand, "@strCompanyName", DbType.String, companyName)
                    db.AddInParameter(dbCommand, "@strAddressType", DbType.String, addressType)
                    db.AddInParameter(dbCommand, "@strBlockHouseNo", DbType.String, addressBlockHouseNo)
                    db.AddInParameter(dbCommand, "@strStreetName", DbType.String, addressStreetName)
                    db.AddInParameter(dbCommand, "@strFloorNo", DbType.String, addressFloorNo)
                    db.AddInParameter(dbCommand, "@strUnitNo", DbType.String, addressUnitNo)
                    db.AddInParameter(dbCommand, "@strBuildingName", DbType.String, addressBuildingName)
                    db.AddInParameter(dbCommand, "@strPostalCode", DbType.String, addressPostalCode)
                    db.AddInParameter(dbCommand, "@strContactPerson", DbType.String, contactPerson)
                    db.AddInParameter(dbCommand, "@strTelephoneNo", DbType.String, telephoneNo)
                    db.AddInParameter(dbCommand, "@strFaxNo", DbType.String, faxNo)
                    db.AddInParameter(dbCommand, "@strOtherInfo", DbType.String, otherInformation)
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
    ''' Update Supplier Status;
    ''' 19Dec08, KG
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="supplierId"></param>
    ''' <param name="status"></param>
    ''' <param name="loginUser"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Shared Function UpdateSupplierStatus( _
        ByVal storeId As String _
        , ByVal supplierId As String _
        , ByVal status As Char _
        , ByVal loginUser As String _
    ) As Boolean
        Try
            Dim sqlStoredProc As String = "spUpdateSupplierStatus"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
                    db.AddInParameter(dbCommand, "@strSupplierId", DbType.String, supplierId)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                    db.ExecuteNonQuery(dbCommand)

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If

            Return False
        End Try

        Return True

    End Function
#End Region

#Region " COMMON "

    ''' <summary>
    ''' Get All Common Items;
    ''' 24 Dec 2008 - Jianfa;
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetCommon(ByVal storeID As String, ByVal status As String) As DataSet

        Dim Common As New DataSet

        Try

            Dim sqlStoredProc As String = "spSelectCommon"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)

                    Common.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "Common")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Common

    End Function

    ''' <summary>
    ''' Get Distinct Common Items
    ''' 03 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="status"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetDistinctCommon(ByVal status As String) As DataSet

        Dim Common As New DataSet

        Try

            Dim sqlStoredProc As String = "spSelectDistinctCommonCodeGroup"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)

                    Common.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "DistinctCommon")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Common
    End Function

    ''' <summary>
    ''' Function - Get Common by Group;
    ''' 03 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="codeGroup"></param>
    ''' <param name="status"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetCommonByGroup(ByVal storeID As String, ByVal codeGroup As String, ByVal status As String) As DataSet

        Dim Common As New DataSet

        Try

            Dim sqlStoredProc As String = "spSelectCommonByGroup"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strCodeGroup", DbType.String, codeGroup)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)

                    Common.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "CommonByGroup")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Common

    End Function

    ''' <summary>
    ''' Sub Proc - Insert Common
    ''' 05 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="codeGroup"></param>
    ''' <param name="codeID"></param>
    ''' <param name="codeDescription"></param>
    ''' <remarks></remarks>
    Public Shared Sub InsertCommon( _
        ByVal storeID As String _
        , ByVal codeGroup As String _
        , ByVal codeID As String _
        , ByVal codeDescription As String _
        , ByVal status As String _
        , ByVal loginUser As String _
    )

        Try
            Dim sqlStoredProc As String = "spInsertCommon"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strCodeGroup", DbType.String, codeGroup)
                    db.AddInParameter(dbCommand, "@strCodeID", DbType.String, codeID)
                    db.AddInParameter(dbCommand, "@strCodeDescription", DbType.String, codeDescription)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
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
    ''' Sub Proc - Update Common;
    ''' 06 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="commonID"></param>
    ''' <param name="codeGroup"></param>
    ''' <param name="codeID"></param>
    ''' <param name="codeDescription"></param>
    ''' <param name="status"></param>
    ''' <param name="loginUser"></param>
    ''' <remarks></remarks>
    Public Shared Sub UpdateCommon( _
            ByVal storeID As String _
            , ByVal commonID As Integer _
            , ByVal codeGroup As String _
            , ByVal codeID As String _
            , ByVal codeDescription As String _
            , ByVal status As String _
            , ByVal loginUser As String)

        Try
            Dim sqlStoredProc As String = "spUpdateCommon"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@intCommonID", DbType.Int32, commonID)
                    db.AddInParameter(dbCommand, "@strCodeGroup", DbType.String, codeGroup)
                    db.AddInParameter(dbCommand, "@strCodeID", DbType.String, codeID)
                    db.AddInParameter(dbCommand, "@strCodeDescription", DbType.String, codeDescription)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
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

#Region " EQUIPMENTS "

    ''' <summary>
    ''' Check for unique equipment id
    ''' 24 Dec 2008 - Jianfa;
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="equipmentId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CheckEquipmentID( _
        ByVal storeId As String _
        , ByVal equipmentId As String _
    ) As Boolean

        Try
            Dim sqlStoredProc As String = "spCheckEquipmentID"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
                    db.AddInParameter(dbCommand, "@strEquipmentId", DbType.String, equipmentId)

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
            Return False
        End Try
    End Function

    ''' <summary>
    ''' To insert equipment;
    ''' 30 Dec 2008 - Jianfa;
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="equipmentId"></param>
    ''' <param name="equipmentDescription"></param>
    ''' <param name="status"></param>
    ''' <param name="loginUser"></param>
    ''' <remarks></remarks>
    Public Shared Sub InsertEquipment( _
        ByVal storeId As String _
        , ByVal equipmentId As String _
        , ByVal equipmentType As String _
        , ByVal equipmentDescription As String _
        , ByVal status As String _
        , ByVal loginUser As String _
    )

        Try

            Dim sqlStoredProc As String = "spInsertEquipment"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
                    db.AddInParameter(dbCommand, "@strEquipmentId", DbType.String, equipmentId)
                    db.AddInParameter(dbCommand, "@strEquipmentType", DbType.String, equipmentType)
                    db.AddInParameter(dbCommand, "@strEquipmentDescription", DbType.String, equipmentDescription)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
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
    ''' To Get List of Equipments;
    ''' 29 Dec 2008 - Jianfa;
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="equipmentId"></param>
    ''' <param name="equipmentDescription"></param>
    ''' <param name="status"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetEquipments( _
         ByVal storeId As String _
         , ByVal equipmentId As String _
         , ByVal equipmentType As String _
         , ByVal equipmentDescription As String _
         , ByVal status As String _
     ) As DataSet

        Dim Equipments As New DataSet
        Try
            Dim sqlStoredProc As String = "spSelectEquipments"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
                    db.AddInParameter(dbCommand, "@strEquipmentId", DbType.String, equipmentId)
                    db.AddInParameter(dbCommand, "@strEquipmentType", DbType.String, equipmentType)
                    db.AddInParameter(dbCommand, "@strEquipmentDescription", DbType.String, equipmentDescription)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)

                    Equipments.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "Equipment")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Equipments

    End Function

    ''' <summary>
    ''' To update equipment status;
    ''' 29 Dec 2008 - Jianfa;
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="equipmentId"></param>
    ''' <param name="status"></param>
    ''' <param name="loginUser"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateEquipmentStatus(ByVal storeId As String, ByVal equipmentId As String, _
                                                 ByVal status As String, ByVal loginUser As String) As Boolean


        Try
            Dim sqlStoredProc As String = "spUpdateEquipmentStatus"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()

                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
                    db.AddInParameter(dbCommand, "@strEquipmentId", DbType.String, equipmentId)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                    db.ExecuteNonQuery(dbCommand)

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If

            Return False
        End Try

        Return True

    End Function

    ''' <summary>
    ''' To update Equipment
    ''' 30 Dec 2008 - Jianfa;
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="equipmentId"></param>
    ''' <param name="equipmentDescription"></param>
    ''' <param name="loginUser"></param>
    ''' <remarks></remarks>
    Public Shared Sub UpdateEquipment( _
        ByVal storeId As String _
        , ByVal equipmentId As String _
        , ByVal equipmentType As String _
        , ByVal equipmentDescription As String _
        , ByVal loginUser As String _
    )

        Try
            Dim sqlStoredProc As String = "spUpdateEquipment"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
                    db.AddInParameter(dbCommand, "@strEquipmentId", DbType.String, equipmentId)
                    db.AddInParameter(dbCommand, "@strEquipmentType", DbType.String, equipmentType)
                    db.AddInParameter(dbCommand, "@strEquipmentDescription", DbType.String, equipmentDescription)
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

#Region " STORES "

    ''' <summary>
    ''' Function - Check Store ID
    ''' 01 Jan 2009 - Jianfa
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' To check for unique store ID
    ''' </remarks>
    Public Shared Function CheckStoreID( _
        ByVal storeId As String _
    ) As Boolean

        Try
            Dim sqlStoredProc As String = "spCheckStoreID"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)

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
    ''' Function - Check Store Name
    ''' </summary>
    ''' <param name="storeName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CheckStoreName(ByVal storeName As String) As Boolean

        Try
            Dim sqlStoredProc As String = "spCheckStoreName"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreName", DbType.String, storeName)

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
    ''' Function - Insert Store
    ''' 01 Jan 2009 - Jianfa;
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="storeName"></param>
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
    ''' <param name="userRoles"></param>
    ''' <remarks>
    ''' 1) To insert store 
    ''' 2) To insert module ref for store for each individual role groups
    ''' 3) To include a database transaction to hold multiple inserts of table 
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' 28Feb2009  KG          SIT    duplicate Common table;
    ''' </remarks>
    Public Shared Sub InsertStore( _
        ByVal storeId As String _
        , ByVal storeName As String _
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
        , ByVal userRoles As String _
    )

        Dim trans As DbTransaction
        Dim sqlStoredProc As String = "spInsertStore"
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim conn As DbConnection = db.CreateConnection()

        conn.Open()
        trans = conn.BeginTransaction

        Dim arrRole As String() = Split(userRoles, ",")

        Try

            Dim dbCommand As DbCommand

            dbCommand = db.GetStoredProcCommand(sqlStoredProc)
            dbCommand.Transaction = trans

            db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
            db.AddInParameter(dbCommand, "@strStoreName", DbType.String, storeName)
            db.AddInParameter(dbCommand, "@strAddressType", DbType.String, addressType)
            db.AddInParameter(dbCommand, "@strBlockHouseNo", DbType.String, addressBlockHouseNo)
            db.AddInParameter(dbCommand, "@strStreetName", DbType.String, addressStreetName)
            db.AddInParameter(dbCommand, "@strFloorNo", DbType.String, addressFloorNo)
            db.AddInParameter(dbCommand, "@strUnitNo", DbType.String, addressUnitNo)
            db.AddInParameter(dbCommand, "@strBuildingName", DbType.String, addressBuildingName)
            db.AddInParameter(dbCommand, "@strPostalCode", DbType.String, addressPostalCode)
            db.AddInParameter(dbCommand, "@strContactPerson", DbType.String, contactPerson)
            db.AddInParameter(dbCommand, "@strTelephoneNo", DbType.String, telephoneNo)
            db.AddInParameter(dbCommand, "@strFaxNo", DbType.String, faxNo)
            db.AddInParameter(dbCommand, "@strOtherInfo", DbType.String, otherInformation)
            db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
            db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

            db.ExecuteNonQuery(dbCommand, trans)

            sqlStoredProc = "spInsertModuleRef"
            dbCommand.Parameters.Clear()

            dbCommand = db.GetStoredProcCommand(sqlStoredProc)

            For idx As Integer = 0 To UBound(arrRole)

                dbCommand.Parameters.Clear()
                db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
                db.AddInParameter(dbCommand, "@strStoreRole", DbType.String, arrRole(idx))
                db.AddInParameter(dbCommand, "@strStatus", DbType.String, "O")
                db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                db.ExecuteNonQuery(dbCommand, trans)

            Next

            sqlStoredProc = "spInsertCommonByStore"
            dbCommand.Parameters.Clear()
            dbCommand = db.GetStoredProcCommand(sqlStoredProc)
            db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
            db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

            db.ExecuteNonQuery(dbCommand, trans)

            trans.Commit()
            conn.Close()

        Catch ex As Exception
            trans.Rollback()
            conn.Close()
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Function - Get Stores;
    ''' 01 Jan 2009  - Jianfa;
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
            Dim sqlStoredProc As String = "spSelectStores"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
                    db.AddInParameter(dbCommand, "@strStoreName", DbType.String, storeName)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)

                    Stores.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "Store")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Stores

    End Function

    ''' <summary>
    ''' Sub Proc - Update Store;
    ''' 01 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="storeName"></param>
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
    ''' <param name="loginUser"></param>
    ''' <remarks></remarks>
    Public Shared Sub UpdateStore(ByVal storeId As String _
        , ByVal storeName As String _
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
        , ByVal loginUser As String)

        Try
            Dim sqlStoredProc As String = "spUpdateStore"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
                    db.AddInParameter(dbCommand, "@strStoreName", DbType.String, storeName)
                    db.AddInParameter(dbCommand, "@strAddressType", DbType.String, addressType)
                    db.AddInParameter(dbCommand, "@strBlockHouseNo", DbType.String, addressBlockHouseNo)
                    db.AddInParameter(dbCommand, "@strStreetName", DbType.String, addressStreetName)
                    db.AddInParameter(dbCommand, "@strFloorNo", DbType.String, addressFloorNo)
                    db.AddInParameter(dbCommand, "@strUnitNo", DbType.String, addressUnitNo)
                    db.AddInParameter(dbCommand, "@strBuildingName", DbType.String, addressBuildingName)
                    db.AddInParameter(dbCommand, "@strPostalCode", DbType.String, addressPostalCode)
                    db.AddInParameter(dbCommand, "@strContactPerson", DbType.String, contactPerson)
                    db.AddInParameter(dbCommand, "@strTelephoneNo", DbType.String, telephoneNo)
                    db.AddInParameter(dbCommand, "@strFaxNo", DbType.String, faxNo)
                    db.AddInParameter(dbCommand, "@strOtherInfo", DbType.String, otherInformation)
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
    ''' Function - Update Store Status;
    ''' 02 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="operationType"></param>
    ''' <param name="moduleStatus"></param>
    ''' <param name="loginUser"></param>
    ''' <remarks></remarks>
    Public Shared Function UpdateStoreStatus(ByVal storeId As String, ByVal operationType As String, _
                                        ByVal moduleStatus As String, ByVal loginUser As String) As Boolean

        Dim trans As DbTransaction
        Dim sqlStoredProc As String = "spUpdateStoreStatus"
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim conn As DbConnection = db.CreateConnection()

        conn.Open()
        trans = conn.BeginTransaction

        Try
            Dim dbCommand As DbCommand

            dbCommand = db.GetStoredProcCommand(sqlStoredProc)
            dbCommand.Transaction = trans

            db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
            db.AddInParameter(dbCommand, "@strStatus", DbType.String, operationType)
            db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

            db.ExecuteNonQuery(dbCommand, trans)

            sqlStoredProc = "spUpdateModuleRefStatus"
            dbCommand.Parameters.Clear()

            dbCommand = db.GetStoredProcCommand(sqlStoredProc)

            db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
            db.AddInParameter(dbCommand, "@strStatus", DbType.String, moduleStatus)
            db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

            db.ExecuteNonQuery(dbCommand, trans)

            trans.Commit()

        Catch ex As Exception
            trans.Rollback()
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If

            Return False
        End Try

        Return True

    End Function

    ''' <summary>
    ''' Function - CheckStoreValue;
    ''' 2 March 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CheckStoreValue(ByVal storeID As String) As Boolean

        Try
            Dim sqlStoredProc As String = "spCheckStoreValue"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)

                    If db.ExecuteScalar(dbCommand) > 0 Then
                        Return False
                    Else
                        Return True
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
    ''' Function - CheckZeroStock
    ''' 2 March 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CheckZeroStock(ByVal storeID As String) As Boolean

        Try
            Dim sqlStoredProc As String = "spCheckZeroStock"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)

                    If db.ExecuteScalar(dbCommand) > 0 Then
                        Return False
                    Else
                        Return True
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

#End Region

#Region " ITEM "

    ''' <summary>
    ''' Function - GenerateItemID;
    ''' 07 Jan 09 - Jianfa
    ''' </summary>
    ''' <param name="itemID"></param>
    ''' <remarks></remarks>
    Public Shared Function GenerateItemID(ByVal itemID As String, ByVal storeID As String) As String

        Dim generatedItemID As String = String.Empty

        Try

            Dim sqlStoredProc As String = "spGenerateItemID"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strItemId", DbType.String, itemID)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeID)
                    generatedItemID = db.ExecuteScalar(dbCommand)

                End Using
            End Using

        Catch ex As Exception
            'Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            'If (rethrow) Then
            '    Throw
            'End If
        End Try

        Return generatedItemID

    End Function

    ''' <summary>
    ''' Function -  CheckItemID;
    ''' 08 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="itemID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CheckItemID(ByVal storeID As String, _
                                       ByVal itemID As String) As Boolean

        Try
            Dim sqlStoredProc As String = "spCheckItemID"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strItemId", DbType.String, itemID)

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
    ''' Function - CheckItemStatus;
    ''' 28 Feb 11 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="itemID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CheckItemStatus(ByVal storeID As String, _
                                       ByVal itemID As String) As Boolean


        Try
            Dim sqlStoredProc As String = "spCheckItemStatus"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strItemId", DbType.String, itemID)

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
    ''' Sub Proc - AddItem;
    ''' 08 Jan 09 - Jianfa;
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
    ''' <param name="status"></param>
    ''' <param name="loginUser"></param>
    ''' <remarks></remarks>
    Public Shared Sub InsertItem(ByVal storeID As String, _
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
                              ByVal loginUser As String)

        Dim trans As DbTransaction
        Dim sqlStoredProc As String = "spInsertItem"
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim conn As DbConnection = db.CreateConnection()

        conn.Open()
        trans = conn.BeginTransaction

        Try

            Dim dbCommand As DbCommand

            dbCommand = db.GetStoredProcCommand(sqlStoredProc)
            dbCommand.Transaction = trans

            db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
            db.AddInParameter(dbCommand, "@strItemID", DbType.String, itemID)
            db.AddInParameter(dbCommand, "@strEquipmentID", DbType.String, IIf(equipmentID.Trim = String.Empty, DBNull.Value, equipmentID))
            db.AddInParameter(dbCommand, "@strItemDescription", DbType.String, itemDescription)
            db.AddInParameter(dbCommand, "@strPartNo", DbType.String, partNo)
            db.AddInParameter(dbCommand, "@strStockType", DbType.String, stockType)
            db.AddInParameter(dbCommand, "@strSubType", DbType.String, subType)
            db.AddInParameter(dbCommand, "@strUOM", DbType.String, UOM)
            db.AddInParameter(dbCommand, "@strLocation", DbType.String, location)
            db.AddInParameter(dbCommand, "@strLocation2", DbType.String, location2)
            db.AddInParameter(dbCommand, "@dblMinLevel", DbType.Decimal, minLevel)
            db.AddInParameter(dbCommand, "@dblReorderLevel", DbType.Decimal, reOrderLevel)
            db.AddInParameter(dbCommand, "@strMaxLevel", DbType.Decimal, maxLevel)
            db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
            db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

            db.ExecuteNonQuery(dbCommand, trans)

            sqlStoredProc = "spInsertStockTransaction"
            dbCommand.Parameters.Clear()

            dbCommand = db.GetStoredProcCommand(sqlStoredProc)
            db.AddInParameter(dbCommand, "@strType", DbType.String, "BALANCE")
            db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
            db.AddInParameter(dbCommand, "@strStockItemID", DbType.String, itemID)
            db.AddOutParameter(dbCommand, "@intTranID", DbType.Int32, 4)
            db.AddInParameter(dbCommand, "@decQty", DbType.Decimal, openingBalance)
            db.AddInParameter(dbCommand, "@dteTranDte", DbType.DateTime, Today)
            db.AddInParameter(dbCommand, "@sngTotalCost", DbType.Decimal, openingTotalValue)
            db.AddInParameter(dbCommand, "@strRemarks", DbType.String, "Opening Balance")
            db.AddInParameter(dbCommand, "@strStatus", DbType.String, "O")
            db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

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
    ''' Function - GetItems;
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
            Dim sqlStoredProc As String = "spSelectItems"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strItemId", DbType.String, itemID)
                    db.AddInParameter(dbCommand, "@strLocation", DbType.String, location)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
                    db.AddInParameter(dbCommand, "@strEquipmentID", DbType.String, equipmentID)
                    db.AddInParameter(dbCommand, "@strItemDescription", DbType.String, itemDescription)

                    Items.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "Item")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Items

    End Function

    ''' <summary>
    ''' Function - GetItemsMasterList
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
            Dim sqlStoredProc As String = "spSelectItemsMasterList"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strStockCodeFrom", DbType.String, stockCodeFrom)
                    db.AddInParameter(dbCommand, "@strStockCodeTo", DbType.String, stockCodeTo)
                    db.AddInParameter(dbCommand, "@strItemStatus", DbType.String, itemStatus)
                    dbCommand.CommandTimeout = 1200

                    Items.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "ItemMasterList")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Items
    End Function

    ''' <summary>
    ''' Function - GetStockTransaction;
    ''' 09 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="itemID"></param>
    ''' <param name="transactionType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetStockTransaction(ByVal storeID As String, ByVal itemID As String, _
                                               ByVal transactionType As String) _
                                               As DataSet

        Dim Items As New DataSet
        Try
            Dim sqlStoredProc As String = "spSelectStockTransaction"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strItemID", DbType.String, itemID)
                    db.AddInParameter(dbCommand, "@strTransactionType", DbType.String, transactionType)

                    Items.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "StockTransaction")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Items

    End Function

    ''' <summary>
    ''' Function - CheckTransactionMovement;
    ''' 22 Feb 11 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="itemID"></param>
    ''' <param name="status"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CheckTransactionMovement(ByVal storeID As String, ByVal itemID As String, _
                                                    Optional ByVal status As String = "") As Boolean

        Try

            Dim sqlStoredProc As String = "spCheckTransactionMovment"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strItemID", DbType.String, itemID)

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
    ''' Function - CheckTransactionMonth;
    ''' 22 Feb 11 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="itemID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CheckTransactionMonth(ByVal storeID As String, ByVal itemID As String, _
                                                    Optional ByVal status As String = "") As Boolean

        Try

            Dim sqlStoredProc As String = "spCheckTransactionMonth"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strItemID", DbType.String, itemID)

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
    ''' Function - CheckZeroQtyCost;
    ''' 22 Feb 11 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="itemID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CheckZeroQtyCost(ByVal storeID As String, _
                                                ByVal itemID As String) As Boolean

        Try
            Dim sqlStoredProc As String = "spCheckZeroQtyCost"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strItemID", DbType.String, itemID)

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

        Return False

    End Function

    ''' <summary>
    ''' Function - CheckStockBalanceQty;
    ''' 10 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="itemID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CheckStockBalanceQty(ByVal storeID As String, _
                                                ByVal itemID As String) As DataTable

        Dim Items As New DataSet
        Try
            Dim sqlStoredProc As String = "spCheckStockQtyBalance"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strItemID", DbType.String, itemID)

                    Items.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "StockQtyBalance")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Items.Tables(0)

    End Function

    ''' <summary>
    ''' Sub Proc - Update Item;
    ''' 10 Jan 09 - Jianfa
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
    ''' <remarks></remarks>
    Public Shared Sub UpdateItem(ByVal storeID As String, _
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
                              ByVal loginUser As String)

        Dim trans As DbTransaction
        Dim sqlStoredProc As String = "spUpdateItem"
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim conn As DbConnection = db.CreateConnection()

        conn.Open()
        trans = conn.BeginTransaction

        Try

            Dim dbCommand As DbCommand

            dbCommand = db.GetStoredProcCommand(sqlStoredProc)
            dbCommand.Transaction = trans

            db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
            db.AddInParameter(dbCommand, "@strItemID", DbType.String, itemID)
            db.AddInParameter(dbCommand, "@strEquipmentID", DbType.String, IIf(equipmentID.Trim = String.Empty, DBNull.Value, equipmentID))
            db.AddInParameter(dbCommand, "@strItemDescription", DbType.String, itemDescription)
            db.AddInParameter(dbCommand, "@strPartNo", DbType.String, partNo)
            db.AddInParameter(dbCommand, "@strStockType", DbType.String, stockType)
            db.AddInParameter(dbCommand, "@strSubType", DbType.String, subType)
            db.AddInParameter(dbCommand, "@strUOM", DbType.String, UOM)
            db.AddInParameter(dbCommand, "@strLocation", DbType.String, location)
            db.AddInParameter(dbCommand, "@strLocation2", DbType.String, location2)
            db.AddInParameter(dbCommand, "@dblMinLevel", DbType.Decimal, minLevel)
            db.AddInParameter(dbCommand, "@dblReorderLevel", DbType.Decimal, reOrderLevel)
            db.AddInParameter(dbCommand, "@strMaxLevel", DbType.Decimal, maxLevel)
            db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
            db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

            db.ExecuteNonQuery(dbCommand, trans)

            If openingBalance > -1.0 Then

                sqlStoredProc = "spUpdateStockTransaction"
                dbCommand.Parameters.Clear()

                dbCommand = db.GetStoredProcCommand(sqlStoredProc)
                db.AddInParameter(dbCommand, "@strTransType", DbType.String, "BALANCE")
                db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                db.AddInParameter(dbCommand, "@strItemID", DbType.String, itemID)
                db.AddInParameter(dbCommand, "@dblTransQty", DbType.Decimal, openingBalance)
                db.AddInParameter(dbCommand, "@dblTransTotalCost", DbType.Decimal, openingTotalValue)
                db.AddInParameter(dbCommand, "@strTransRemarks", DbType.String, "Opening Balance")
                db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                db.ExecuteNonQuery(dbCommand, trans)

            End If

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
    ''' Sub Proc - UpdateItemStatus;
    ''' 11 Jan - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="itemID"></param>
    ''' <param name="status"></param>
    ''' <param name="loginUser"></param>
    ''' <remarks></remarks>
    Public Shared Sub UpdateItemStatus(ByVal storeID As String, ByVal itemID As String, _
                                       ByVal status As String, ByVal loginUser As String)

        Try

            Dim sqlStoredProc As String = "spUpdateItemStatus"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strItemID", DbType.String, itemID)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
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
    ''' Sub Proc - DeleteItem;
    ''' 12 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="itemID"></param>
    ''' <param name="status"></param>
    ''' <param name="loginUser"></param>
    ''' <remarks></remarks>
    Public Shared Sub DeleteItem(ByVal storeID As String, ByVal itemID As String, _
                                       ByVal status As String, ByVal loginUser As String)

        Try

            Dim sqlStoredProc As String = "spDeleteItem"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strItemID", DbType.String, itemID)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
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
            Dim sqlStoredProc As String = "spSelectItemsSearch"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strItemId", DbType.String, itemID)

                    Items.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "Items")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Items

    End Function

#End Region

#Region " ROLES "

    ''' <summary>
    ''' Function - GetModuleAccessRights;
    ''' 11 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="userID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetModuleAccessRights(ByVal storeID As String, ByVal userID As String) As DataSet

        Dim ModuleAccessRights As New DataSet

        Try

            Dim sqlStoredProc As String = "spSelectModuleAccessRights"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strUserID", DbType.String, userID)

                    ModuleAccessRights.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "ModuleAccessRights")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ModuleAccessRights

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

            Dim sqlStoredProc As String = "spSelectModuleAccess"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strUserID", DbType.String, userID)

                    ModuleAccess.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "ModuleAccess")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ModuleAccess

    End Function

    ''' <summary>
    ''' Function - GetModuleRoles;
    ''' 14 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="role"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetModuleRoles(ByVal storeID As String, ByVal role As String) As DataSet

        Dim Roles As New DataSet

        Try

            Dim sqlStoredProc As String = "spSelectModules"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strRole", DbType.String, role)

                    Roles.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "ModuleRoles")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
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

            Dim sqlStoredProc As String = "spSelectRoles"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strRole", DbType.String, role)

                    Roles.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "UserRoles")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Roles

    End Function

    ''' <summary>
    ''' Function - CheckModuleRoles;
    ''' 16 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="role"></param>
    ''' <param name="moduleID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CheckModuleRoles(ByVal storeID As String, ByVal role As String, _
                                            ByVal moduleID As Integer) As Boolean

        Try

            Dim sqlStoredProc As String = "spCheckModuleRole"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strRole", DbType.String, role)
                    db.AddInParameter(dbCommand, "@intModuleID", DbType.Int32, moduleID)

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

        Return False

    End Function

    ''' <summary>
    ''' Sub Proc - UpdateModuleRoles;
    ''' 16 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="moduleID"></param>
    ''' <remarks></remarks>
    Public Shared Sub UpdateModuleRoles(ByVal storeID As String, ByVal role As String, _
                                        ByVal moduleID As Integer, ByVal selectRight As Boolean, _
                                        ByVal insertRight As Boolean, ByVal updateRight As Boolean, _
                                        ByVal deleteRight As Boolean, ByVal status As String, _
                                        ByVal loginUser As String)

        Try

            Dim sqlStoredProc As String = "spUpdateModuleRole"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strRole", DbType.String, role)
                    db.AddInParameter(dbCommand, "@intModuleID", DbType.Int32, moduleID)
                    db.AddInParameter(dbCommand, "@bnlSelectRight", DbType.Boolean, selectRight)
                    db.AddInParameter(dbCommand, "@bnlInsertRight", DbType.Boolean, insertRight)
                    db.AddInParameter(dbCommand, "@bnlUpdateRight", DbType.Boolean, updateRight)
                    db.AddInParameter(dbCommand, "@bnlDeleteRight", DbType.Boolean, deleteRight)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
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
    ''' Sub Proc - InsertModuleRoles;
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
    ''' <remarks></remarks>
    Public Shared Sub InsertModuleRoles(ByVal storeID As String, ByVal role As String, _
                                        ByVal moduleID As Integer, ByVal selectRight As Boolean, _
                                        ByVal insertRight As Boolean, ByVal updateRight As Boolean, _
                                        ByVal deleteRight As Boolean, ByVal status As String, _
                                        ByVal loginUser As String)

        Try

            Dim sqlStoredProc As String = "spInsertModuleRole"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strRole", DbType.String, role)
                    db.AddInParameter(dbCommand, "@intModuleID", DbType.Int32, moduleID)
                    db.AddInParameter(dbCommand, "@bnlSelectRight", DbType.Boolean, selectRight)
                    db.AddInParameter(dbCommand, "@bnlInsertRight", DbType.Boolean, insertRight)
                    db.AddInParameter(dbCommand, "@bnlUpdateRight", DbType.Boolean, updateRight)
                    db.AddInParameter(dbCommand, "@bnlDeleteRight", DbType.Boolean, deleteRight)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
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
    ''' Sub Proc - UpdateUserRole;
    ''' 20 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="userID"></param>
    ''' <param name="role"></param>
    ''' <param name="status"></param>
    ''' <param name="consumerList"></param>
    ''' <remarks></remarks>
    Public Shared Sub UpdateUserRole(ByVal storeID As String, ByVal userID As String, ByVal soeID As String,
                                     ByVal isUserDeleted As Boolean, ByVal changeReason As String,
                                     ByVal role As String, ByVal status As String,
                                     ByVal loginUser As String,
                                     ByVal consumerList As List(Of String))

        Dim trans As DbTransaction
        Dim sqlStoredProc As String = "spUpdateUserRole"
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim conn As DbConnection = db.CreateConnection()

        conn.Open()
        trans = conn.BeginTransaction

        Try

            Dim dbCommand As DbCommand

            dbCommand = db.GetStoredProcCommand(sqlStoredProc)
            dbCommand.Transaction = trans

            db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeID)
            db.AddInParameter(dbCommand, "@strUserId", DbType.String, userID)
            db.AddInParameter(dbCommand, "@strSoeId", DbType.String, soeID)
            db.AddInParameter(dbCommand, "@strRole", DbType.String, role)
            db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
            db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)
            db.AddInParameter(dbCommand, "@strChangeReason", DbType.String, changeReason)
            'db.AddInParameter(dbCommand, "@isUserDeleted", DbType.Boolean, isUserDeleted)
            db.AddInParameter(dbCommand, "@isUserDeleted", DbType.Boolean, DBNull.Value)

            db.ExecuteNonQuery(dbCommand, trans)

            sqlStoredProc = "spCloseUserConsumerRef"
            dbCommand.Parameters.Clear()

            dbCommand = db.GetStoredProcCommand(sqlStoredProc)
            db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeID)
            db.AddInParameter(dbCommand, "@strUserId", DbType.String, userID)
            db.AddInParameter(dbCommand, "@strRole", DbType.String, role)
            db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
            db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

            db.ExecuteNonQuery(dbCommand, trans)

            If consumerList.Count > 0 Then

                For Each consumerID As String In consumerList

                    sqlStoredProc = "spUpdateUserConsumerRef"
                    dbCommand.Parameters.Clear()

                    dbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strUserId", DbType.String, userID)
                    db.AddInParameter(dbCommand, "@strConsumerId", DbType.String, consumerID)
                    db.AddInParameter(dbCommand, "@strRole", DbType.String, role)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                    db.ExecuteNonQuery(dbCommand, trans)

                Next

            End If

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
    ''' Function - CheckUserID;
    ''' 23 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="userID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CheckUserID(ByVal userID As String) As DataSet

        Dim Roles As New DataSet

        Try

            Dim sqlStoredProc As String = "spCheckNRIC"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strUserID", DbType.String, userID)

                    Roles.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "UserRoles")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Roles

    End Function

    ''' <summary>
    ''' Function -  ValidNRIC;
    ''' 24 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="userID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ValidNRIC(ByVal userID As String) As Boolean

        Try

            Dim sqlStoredProc As String = "spCheckValidNRIC"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strUserID", DbType.String, userID)

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

        Return False

    End Function

    ''' <summary>
    ''' Function -  ValidUserID;
    ''' 24 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="userID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ValidUserID(ByVal storeID As String, ByVal userID As String, ByVal role As String) As Boolean

        Try

            Dim sqlStoredProc As String = "spCheckValidUserID"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strUserId", DbType.String, userID)
                    db.AddInParameter(dbCommand, "@strRole", DbType.String, role)

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

        Return False

    End Function

    ''' <summary>
    ''' Sub Proc - InsertUserRole;
    ''' 20 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="userID"></param>
    ''' <param name="role"></param>
    ''' <param name="status"></param>
    ''' <param name="consumerList"></param>
    ''' <remarks></remarks>
    Public Shared Sub InsertUserRole(ByVal storeID As String, ByVal userID As String, ByVal soeID As String,
                                     ByVal role As String, ByVal status As String,
                                     ByVal loginUser As String,
                                     ByVal consumerList As List(Of String))

        Dim trans As DbTransaction
        Dim sqlStoredProc As String = "spInsertUserRole"
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim conn As DbConnection = db.CreateConnection()

        conn.Open()
        trans = conn.BeginTransaction

        Try

            Dim dbCommand As DbCommand

            dbCommand = db.GetStoredProcCommand(sqlStoredProc)
            dbCommand.Transaction = trans

            db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeID)
            db.AddInParameter(dbCommand, "@strUserId", DbType.String, userID)
            db.AddInParameter(dbCommand, "@strUserSoeId", DbType.String, soeID)
            db.AddInParameter(dbCommand, "@strRole", DbType.String, role)
            db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
            db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

            db.ExecuteNonQuery(dbCommand, trans)

            If consumerList.Count > 0 Then

                sqlStoredProc = "spCloseUserConsumerRef"
                dbCommand.Parameters.Clear()

                dbCommand = db.GetStoredProcCommand(sqlStoredProc)
                db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeID)
                db.AddInParameter(dbCommand, "@strUserId", DbType.String, userID)
                db.AddInParameter(dbCommand, "@strRole", DbType.String, role)
                db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
                db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                db.ExecuteNonQuery(dbCommand, trans)

                For Each consumerID As String In consumerList

                    sqlStoredProc = "spUpdateUserConsumerRef"
                    dbCommand.Parameters.Clear()

                    dbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strUserId", DbType.String, userID)
                    db.AddInParameter(dbCommand, "@strConsumerId", DbType.String, consumerID)
                    db.AddInParameter(dbCommand, "@strRole", DbType.String, role)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                    db.ExecuteNonQuery(dbCommand, trans)

                Next

            End If

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
    ''' Sub Proc - DeleteUserRole;
    ''' 26 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="userID"></param>
    ''' <param name="role"></param>
    ''' <remarks></remarks>
    Public Shared Sub DeleteUserRole(ByVal storeID As String, ByVal userID As String, ByVal role As String)

        Dim trans As DbTransaction
        Dim sqlStoredProc As String = "spDeleteUserConsumerRef"
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim conn As DbConnection = db.CreateConnection()

        conn.Open()
        trans = conn.BeginTransaction

        Try

            Dim dbCommand As DbCommand

            dbCommand = db.GetStoredProcCommand(sqlStoredProc)
            dbCommand.Transaction = trans

            db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeID)
            db.AddInParameter(dbCommand, "@strUserId", DbType.String, userID)
            db.AddInParameter(dbCommand, "@strRole", DbType.String, role)

            db.ExecuteNonQuery(dbCommand, trans)

            sqlStoredProc = "spDeleteUserRole"
            dbCommand.Parameters.Clear()

            dbCommand = db.GetStoredProcCommand(sqlStoredProc)
            db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeID)
            db.AddInParameter(dbCommand, "@strUserId", DbType.String, userID)
            db.AddInParameter(dbCommand, "@strRole", DbType.String, role)

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

    Public Shared Function GetUserRoleIDBySoeID(ByVal soeID As String) As DataSet

        Dim Roles As New DataSet

        Try

            Dim sqlStoredProc As String = "spSelectSoeIDByUserRoleID"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@soeID", DbType.String, soeID)

                    Roles.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "UserRoles")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Roles
    End Function

#End Region

#Region " CONSUMERS "

    ''' <summary>
    ''' Function - GetConsumers;
    ''' 21 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetConsumers(ByVal storeID As String, ByVal consumerID As String, _
                                        ByVal consumerDescription As String, ByVal status As String) As DataSet

        Dim Consumers As New DataSet

        Try

            Dim sqlStoredProc As String = "spSelectConsumers"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strConsumerID", DbType.String, consumerID)
                    db.AddInParameter(dbCommand, "@strConsumerDescription", DbType.String, consumerDescription)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)

                    Consumers.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "Consumers")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Consumers

    End Function

    ''' <summary>
    ''' Function - GetConsumerRef
    ''' 22 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="status"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetConsumerRef(ByVal storeID As String, _
                                          ByVal status As String) As DataSet

        Dim ConsumerRef As New DataSet

        Try

            Dim sqlStoredProc As String = "spSelectConsumerRef"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)

                    ConsumerRef.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "ConsumerRef")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ConsumerRef

    End Function

    ''' <summary>
    ''' Function - GetConsumerRefByUserID;
    ''' 22 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="status"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetConsumerRefByUserID(ByVal storeID As String, ByVal userID As String, _
                                                  ByVal role As String, ByVal status As String) As DataSet

        Dim ConsumerRef As New DataSet

        Try

            Dim sqlStoredProc As String = "spSelectConsumerRefByUserID"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strUserId", DbType.String, userID)
                    db.AddInParameter(dbCommand, "@strRole", DbType.String, role)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)

                    ConsumerRef.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "ConsumerRefByUserID")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return ConsumerRef

    End Function

    ''' <summary>
    ''' Function - CheckConsumerID;
    ''' 29 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="consumerID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CheckConsumerID(ByVal storeID As String, ByVal consumerID As String) As Boolean

        Try
            Dim sqlStoredProc As String = "spCheckConsumerID"
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strConsumerId", DbType.String, consumerID)

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
    ''' Function - InsertConsumer;
    ''' 29 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="consumerId"></param>
    ''' <param name="consumerDescription"></param>
    ''' <param name="userList"></param>
    ''' <remarks></remarks>
    Public Shared Sub InsertConsumer(ByVal storeId As String, ByVal consumerId As String, _
                                     ByVal consumerDescription As String, ByVal loginUser As String, _
                                     ByVal status As String, _
                                     ByVal userList As List(Of String))

        Dim trans As DbTransaction
        Dim sqlStoredProc As String = "spInsertConsumer"
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim conn As DbConnection = db.CreateConnection()

        conn.Open()
        trans = conn.BeginTransaction

        Try

            Dim dbCommand As DbCommand

            dbCommand = db.GetStoredProcCommand(sqlStoredProc)
            dbCommand.Transaction = trans

            db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
            db.AddInParameter(dbCommand, "@strConsumerId", DbType.String, consumerId)
            db.AddInParameter(dbCommand, "@strConsumerDescription", DbType.String, consumerDescription)
            db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
            db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

            db.ExecuteNonQuery(dbCommand, trans)

            If userList.Count > 0 Then

                sqlStoredProc = "spCloseConsumerUserRef"
                dbCommand.Parameters.Clear()

                dbCommand = db.GetStoredProcCommand(sqlStoredProc)
                db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
                db.AddInParameter(dbCommand, "@strConsumerId", DbType.String, consumerId)
                db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
                db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                db.ExecuteNonQuery(dbCommand, trans)

                For Each userID As String In userList

                    sqlStoredProc = "spUpdateConsumerUserRef"
                    dbCommand.Parameters.Clear()

                    dbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
                    db.AddInParameter(dbCommand, "@strUserId", DbType.String, userID)
                    db.AddInParameter(dbCommand, "@strConsumerId", DbType.String, consumerId)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                    db.ExecuteNonQuery(dbCommand, trans)

                Next

            End If

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
    ''' Function - GetUsers;
    ''' 29 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetUsers(ByVal storeID As String) As DataSet

        Dim Users As New DataSet

        Try

            Dim sqlStoredProc As String = "spSelectUsers"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)

                    Users.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "Users")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Users

    End Function

    ''' <summary>
    ''' Function - GetUsersRef;
    ''' 29 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="consumerID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetUsersRef(ByVal storeID As String, ByVal consumerID As String, _
                                       ByVal status As String) As DataSet

        Dim UsersRef As New DataSet

        Try

            Dim sqlStoredProc As String = "spSelectUserRef"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)

                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@strConsumerId", DbType.String, consumerID)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)

                    UsersRef.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges, "Users")

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return UsersRef

    End Function

    ''' <summary>
    ''' Sub Proc - UpdateConsumer;
    ''' 29 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeId"></param>
    ''' <param name="consumerId"></param>
    ''' <param name="consumerDescription"></param>
    ''' <param name="loginUser"></param>
    ''' <param name="userList"></param>
    ''' <remarks></remarks>
    Public Shared Sub UpdateConsumer(ByVal storeId As String, ByVal consumerId As String, _
                                     ByVal consumerDescription As String, ByVal loginUser As String, _
                                     ByVal status As String, _
                                     ByVal userList As List(Of String))

        Dim trans As DbTransaction
        Dim sqlStoredProc As String = "spUpdateConsumer"
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim conn As DbConnection = db.CreateConnection()

        conn.Open()
        trans = conn.BeginTransaction

        Try

            Dim dbCommand As DbCommand

            dbCommand = db.GetStoredProcCommand(sqlStoredProc)
            dbCommand.Transaction = trans

            db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
            db.AddInParameter(dbCommand, "@strConsumerId", DbType.String, consumerId)
            db.AddInParameter(dbCommand, "@strConsumerDescription", DbType.String, consumerDescription)
            db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
            db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

            db.ExecuteNonQuery(dbCommand, trans)

            If userList.Count > 0 Then

                sqlStoredProc = "spCloseConsumerUserRef"
                dbCommand.Parameters.Clear()

                dbCommand = db.GetStoredProcCommand(sqlStoredProc)
                db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
                db.AddInParameter(dbCommand, "@strConsumerId", DbType.String, consumerId)
                db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
                db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                db.ExecuteNonQuery(dbCommand, trans)

                For Each userID As String In userList

                    sqlStoredProc = "spUpdateConsumerUserRef"
                    dbCommand.Parameters.Clear()

                    dbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
                    db.AddInParameter(dbCommand, "@strUserId", DbType.String, userID)
                    db.AddInParameter(dbCommand, "@strConsumerId", DbType.String, consumerId)
                    db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
                    db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                    db.ExecuteNonQuery(dbCommand, trans)

                Next

            Else

                sqlStoredProc = "spCloseConsumerUserRef"
                dbCommand.Parameters.Clear()

                dbCommand = db.GetStoredProcCommand(sqlStoredProc)
                db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeId)
                db.AddInParameter(dbCommand, "@strConsumerId", DbType.String, consumerId)
                db.AddInParameter(dbCommand, "@strStatus", DbType.String, status)
                db.AddInParameter(dbCommand, "@strLoginUser", DbType.String, loginUser)

                db.ExecuteNonQuery(dbCommand, trans)

            End If

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
    ''' Sub Proc - DeleteConsumer;
    ''' 29 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="consumerID"></param>
    ''' <remarks></remarks>
    Public Shared Sub DeleteConsumer(ByVal storeID As String, ByVal consumerID As String)

        Dim trans As DbTransaction
        Dim sqlStoredProc As String = "spDeleteConsumerUserRef"
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim conn As DbConnection = db.CreateConnection()

        conn.Open()
        trans = conn.BeginTransaction

        Try

            Dim dbCommand As DbCommand

            dbCommand = db.GetStoredProcCommand(sqlStoredProc)
            dbCommand.Transaction = trans

            db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeID)
            db.AddInParameter(dbCommand, "@strConsumerId", DbType.String, consumerID)

            db.ExecuteNonQuery(dbCommand, trans)

            sqlStoredProc = "spDeleteConsumer"
            dbCommand.Parameters.Clear()

            dbCommand = db.GetStoredProcCommand(sqlStoredProc)
            db.AddInParameter(dbCommand, "@strStoreId", DbType.String, storeID)
            db.AddInParameter(dbCommand, "@strConsumerId", DbType.String, consumerID)

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

#End Region

#Region "EmailContent"

    Public Shared Sub AddEmailContent(ByVal storeID As String, _
                                         ByVal emailFormat As String, _
                              ByVal toAddr As String, _
                              ByVal ccAddr As String, _
                              ByVal subject As String, _
                              ByVal msgFormat As String, _
                              ByVal firstRemainder As String, _
                              ByVal secondRemainder As String, ByVal loginUser As String)

        Dim trans As DbTransaction
        Dim sqlStoredProc As String





        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim conn As DbConnection = db.CreateConnection()

        conn.Open()
        trans = conn.BeginTransaction

        Try

            Dim dbCommand As DbCommand


            If emailFormat = "reorderitem" Then
                sqlStoredProc = "spInsertUpdateEmailRestockLevel"
                dbCommand = db.GetStoredProcCommand(sqlStoredProc)
                db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                db.AddInParameter(dbCommand, "@strEmailTo", DbType.String, toAddr)
                db.AddInParameter(dbCommand, "@strEmailCc", DbType.String, ccAddr)
                db.AddInParameter(dbCommand, "@strEmailSubject", DbType.String, subject)
                db.AddInParameter(dbCommand, "@strEmailBody", DbType.String, msgFormat)
                db.AddInParameter(dbCommand, "@strUserID", DbType.String, loginUser)
            Else
                sqlStoredProc = "spInsertUpdateEmailLoginReminder"
                dbCommand = db.GetStoredProcCommand(sqlStoredProc)
                db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                db.AddInParameter(dbCommand, "@strEmailCc", DbType.String, ccAddr)
                db.AddInParameter(dbCommand, "@strEmailSubject", DbType.String, subject)
                db.AddInParameter(dbCommand, "@strEmailBody", DbType.String, msgFormat)
                db.AddInParameter(dbCommand, "@intFirstReminder", DbType.Int32, firstRemainder)
                db.AddInParameter(dbCommand, "@intSecondReminder", DbType.Int32, secondRemainder)
                db.AddInParameter(dbCommand, "@strUserID", DbType.String, loginUser)
            End If


            dbCommand.Transaction = trans


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

    Public Shared Function GetEmailContent(ByVal storeID As String, ByVal emailFormat As String) As DataTable

        Dim dtEmailContent As New DataTable

        Try

            Dim sqlStoredProc As String = "EmailContent_Get"
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Using conn As DbConnection = db.CreateConnection()
                Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlStoredProc)
                    db.AddInParameter(dbCommand, "@strStoreID", DbType.String, storeID)
                    db.AddInParameter(dbCommand, "@emailFormat", DbType.String, emailFormat)

                    dtEmailContent.Load(db.ExecuteReader(dbCommand), LoadOption.OverwriteChanges)

                End Using
            End Using

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "DataAccess Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return dtEmailContent

    End Function


#End Region
End Class
