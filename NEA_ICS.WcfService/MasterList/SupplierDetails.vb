''' <summary>
''' DataContract - for Supplier Details;
''' 17 Dec 08 - Jianfa CHEN;
''' </summary>
''' <remarks>
''' CHANGE LOG:
''' ddMMMyy  AuthorName  RefID  Description;
''' </remarks>
<DataContract()> _
Public Class SupplierDetails

#Region " Constructor "
    Public Sub New()
    End Sub

    ''' <summary>
    ''' Initialise value with blank value, True;
    ''' 18 Dec 08 - KG;
    ''' </summary>
    ''' <param name="initialise"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal initialise As Boolean)
        If initialise Then
            StoreId = ""
            SupplierId = ""
            UEN = ""
            CompanyName = ""
            AddressType = ""
            AddressBlockHouseNo = ""
            AddressStreetName = ""
            AddressFloorNo = ""
            AddressUnitNo = ""
            AddressBuildingName = ""
            AddressPostalCode = ""
            ContactPerson = ""
            TelephoneNo = ""
            FaxNo = ""
            OtherInfo = ""
            Status = ""
            OriginalCompanyName = ""
            LoginUser = ""
        End If
    End Sub
#End Region

    Private _storeId As String
    <DataMember()> _
    Public Property StoreId() As String
        Get
            Return _storeId
        End Get
        Set(ByVal value As String)
            _storeId = value
        End Set
    End Property

    Private _supplierId As String
    <DataMember()> _
    Public Property SupplierId() As String
        Get
            Return _supplierId
        End Get
        Set(ByVal value As String)
            _supplierId = value
        End Set
    End Property

    Private _UEN As String
    <DataMember()> _
    Public Property UEN() As String
        Get
            Return _UEN
        End Get
        Set(ByVal value As String)
            _UEN = value
        End Set
    End Property

    Private _companyName As String
    <DataMember()> _
    Public Property CompanyName() As String
        Get
            Return _companyName
        End Get
        Set(ByVal value As String)
            _companyName = value
        End Set
    End Property

    Private _addressType As String
    <DataMember()> _
    Public Property AddressType() As String
        Get
            Return _addressType
        End Get
        Set(ByVal value As String)
            _addressType = value
        End Set
    End Property

    Private _addressBlockHouseNo As String
    <DataMember()> _
    Public Property AddressBlockHouseNo() As String
        Get
            Return _addressBlockHouseNo
        End Get
        Set(ByVal value As String)
            _addressBlockHouseNo = value
        End Set
    End Property

    Private _addressStreetName As String
    <DataMember()> _
    Public Property AddressStreetName() As String
        Get
            Return _addressStreetName
        End Get
        Set(ByVal value As String)
            _addressStreetName = value
        End Set
    End Property

    Private _addressFloorNo As String
    <DataMember()> _
    Public Property AddressFloorNo() As String
        Get
            Return _addressFloorNo
        End Get
        Set(ByVal value As String)
            _addressFloorNo = value
        End Set
    End Property

    Private _addressUnitNo As String
    <DataMember()> _
    Public Property AddressUnitNo() As String
        Get
            Return _addressUnitNo
        End Get
        Set(ByVal value As String)
            _addressUnitNo = value
        End Set
    End Property

    Private _addressBuildingName As String
    <DataMember()> _
    Public Property AddressBuildingName() As String
        Get
            Return _addressBuildingName
        End Get
        Set(ByVal value As String)
            _addressBuildingName = value
        End Set
    End Property

    Private _addressPostalCode As String
    <DataMember()> _
    Public Property AddressPostalCode() As String
        Get
            Return _addressPostalCode
        End Get
        Set(ByVal value As String)
            _addressPostalCode = value
        End Set
    End Property

    Private _contactPerson As String
    <DataMember()> _
    Public Property ContactPerson() As String
        Get
            Return _contactPerson
        End Get
        Set(ByVal value As String)
            _contactPerson = value
        End Set
    End Property

    Private _telephoneNo As String
    <DataMember()> _
    Public Property TelephoneNo() As String
        Get
            Return _telephoneNo
        End Get
        Set(ByVal value As String)
            _telephoneNo = value
        End Set
    End Property

    Private _faxNo As String
    <DataMember()> _
    Public Property FaxNo() As String
        Get
            Return _faxNo
        End Get
        Set(ByVal value As String)
            _faxNo = value
        End Set
    End Property

    Private _otherInfo As String
    <DataMember()> _
    Public Property OtherInfo() As String
        Get
            Return _otherInfo
        End Get
        Set(ByVal value As String)
            _otherInfo = value
        End Set
    End Property

    Private _status As String
    <DataMember()> _
    Public Property Status() As String
        Get
            Return _status
        End Get
        Set(ByVal value As String)
            _status = value
        End Set
    End Property

    Private _loginUser As String
    <DataMember()> _
    Public Property LoginUser() As String
        Get
            Return _loginUser
        End Get
        Set(ByVal value As String)
            _loginUser = value
        End Set
    End Property

    Private _originalCompanyName As String
    <DataMember()> _
    Public Property OriginalCompanyName() As String
        Get
            Return _originalCompanyName
        End Get
        Set(ByVal value As String)
            _originalCompanyName = value
        End Set
    End Property

End Class
