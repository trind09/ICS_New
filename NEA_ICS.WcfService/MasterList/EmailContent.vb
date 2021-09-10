
Public Class EmailContent
    Public Sub New()
    End Sub

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

    Private _emailFormat As String
    <DataMember()> _
    Public Property EmailFormat() As String
        Get
            Return _emailFormat
        End Get
        Set(ByVal value As String)
            _emailFormat = value
        End Set
    End Property

    Private _toAddress As String
    <DataMember()> _
    Public Property ToAddress() As String
        Get
            Return _toAddress
        End Get
        Set(ByVal value As String)
            _toAddress = value
        End Set
    End Property


    Private _ccAddress As String
    <DataMember()> _
    Public Property CCAddress() As String
        Get
            Return _ccAddress
        End Get
        Set(ByVal value As String)
            _ccAddress = value
        End Set
    End Property

    Private _subject As String
    <DataMember()> _
    Public Property Subject() As String
        Get
            Return _subject
        End Get
        Set(ByVal value As String)
            _subject = value
        End Set
    End Property

    Private _msgFormat As String
    <DataMember()> _
    Public Property msgFormat() As String
        Get
            Return _msgFormat
        End Get
        Set(ByVal value As String)
            _msgFormat = value
        End Set
    End Property

    Private _firstRemainder As String
    <DataMember()> _
    Public Property FirstRemainder() As String
        Get
            Return _firstRemainder
        End Get
        Set(ByVal value As String)
            _firstRemainder = value
        End Set
    End Property

    Private _secondRemainder As String
    <DataMember()> _
    Public Property SecondRemainder() As String
        Get
            Return _secondRemainder
        End Get
        Set(ByVal value As String)
            _secondRemainder = value
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
