''' <summary>
''' DataContract - for Common Details;
''' 23 Dec 08 - Jianfa CHEN;
''' </summary>
''' <remarks>
''' CHANGE LOG:
''' 26Dec08  KG  RefID  Add CommonID;
''' </remarks>
<DataContract()> _
Public Class CommonDetails

    Private _storeID As String
    <DataMember()> _
    Public Property StoreID() As String
        Get
            Return _storeID
        End Get
        Set(ByVal value As String)
            _storeID = value
        End Set
    End Property


    Private _commonID As Integer
    <DataMember()> _
    Public Property CommonID() As Integer
        Get
            Return _commonID
        End Get
        Set(ByVal value As Integer)
            _commonID = value
        End Set
    End Property

    Private _codeGroup As String
    <DataMember()> _
    Public Property CodeGroup() As String
        Get
            Return _codeGroup
        End Get
        Set(ByVal value As String)
            _codeGroup = value
        End Set
    End Property

    Private _codeID As String
    <DataMember()> _
    Public Property CodeID() As String
        Get
            Return _codeID
        End Get
        Set(ByVal value As String)
            _codeID = value
        End Set
    End Property

    Private _codeDescription As String
    <DataMember()> _
    Public Property CodeDescription() As String
        Get
            Return _codeDescription
        End Get
        Set(ByVal value As String)
            _codeDescription = value
        End Set
    End Property

    Private _Status As String
    <DataMember()> _
    Public Property Status() As String
        Get
            Return _Status
        End Get
        Set(ByVal value As String)
            _Status = value
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


End Class
