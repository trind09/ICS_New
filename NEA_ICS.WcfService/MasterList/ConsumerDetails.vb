''' <summary>
''' DataContract - for Consumer Details;
''' 20 Jan 09 - Jianfa
''' </summary>
''' <remarks>
''' CHANGE LOG:
''' ddMMMyy  AuthorName  RefID  Description;
''' </remarks>
<DataContract()> _
Public Class ConsumerDetails

    Public Sub New()
    End Sub

    Private _consumerID As String
    <DataMember()> _
    Public Property ConsumerID() As String
        Get
            Return _consumerID
        End Get
        Set(ByVal value As String)
            _consumerID = value
        End Set
    End Property

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

    Private _userID As String
    <DataMember()> _
    Public Property UserID() As String
        Get
            Return _userID
        End Get
        Set(ByVal value As String)
            _userID = value
        End Set
    End Property

    Private _userRole As String
    <DataMember()> _
    Public Property UserRole() As String
        Get
            Return _userRole
        End Get
        Set(ByVal value As String)
            _userRole = value
        End Set
    End Property

    Private _consumerID_Description As String
    <DataMember()> _
    Public Property ConsumerID_Description() As String
        Get
            Return _consumerID_Description
        End Get
        Set(ByVal value As String)
            _consumerID_Description = value
        End Set
    End Property

    Private _consumerDescription As String
    <DataMember()> _
    Public Property ConsumerDescription() As String
        Get
            Return _consumerDescription
        End Get
        Set(ByVal value As String)
            _consumerDescription = value
        End Set
    End Property

    Private _consumerStatus As String
    <DataMember()> _
    Public Property ConsumerStatus() As String
        Get
            Return _consumerStatus
        End Get
        Set(ByVal value As String)
            _consumerStatus = value
        End Set
    End Property

    Private _consumerRefStatus As String
    <DataMember()> _
    Public Property ConsumerRefStatus() As String
        Get
            Return _consumerRefStatus
        End Get
        Set(ByVal value As String)
            _consumerRefStatus = value
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


    Private _userName As String
    <DataMember()> _
    Public Property UserName() As String
        Get
            Return _userName
        End Get
        Set(ByVal value As String)
            _userName = value
        End Set
    End Property


End Class
