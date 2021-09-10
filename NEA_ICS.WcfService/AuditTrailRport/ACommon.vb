<Serializable> _
<DataContract()> _
Public Class ACommon

    Private _aCommonStoreID As String
    <DataMember()> _
    Public Property ACommonStoreID() As String
        Get
            Return _aCommonStoreID
        End Get
        Set(ByVal value As String)
            _aCommonStoreID = value
        End Set
    End Property

    Private _aCommonID As String
    <DataMember()> _
    Public Property ACommonID() As String
        Get
            Return _aCommonID
        End Get
        Set(ByVal value As String)
            _aCommonID = value
        End Set
    End Property

    Private _aCommonCodeGroup As String
    <DataMember()> _
    Public Property ACommonCodeGroup() As String
        Get
            Return _aCommonCodeGroup
        End Get
        Set(ByVal value As String)
            _aCommonCodeGroup = value
        End Set
    End Property

    Private _aCommonCodeID As String
    <DataMember()> _
    Public Property ACommonCodeID() As String
        Get
            Return _aCommonCodeID
        End Get
        Set(ByVal value As String)
            _aCommonCodeID = value
        End Set
    End Property

    Private _aCommonCodeDescription As String
    <DataMember()> _
    Public Property ACommonCodeDescription() As String
        Get
            Return _aCommonCodeDescription
        End Get
        Set(ByVal value As String)
            _aCommonCodeDescription = value
        End Set
    End Property

    Private _aCommonStatus As String
    <DataMember()> _
    Public Property ACommonStatus() As String
        Get
            Return _aCommonStatus
        End Get
        Set(ByVal value As String)
            _aCommonStatus = value
        End Set
    End Property

    Private _aCommonCreateDte As DateTime
    <DataMember()> _
    Public Property ACommonCreateDte() As DateTime
        Get
            Return _aCommonCreateDte
        End Get
        Set(ByVal value As DateTime)
            _aCommonCreateDte = value
        End Set
    End Property

    Private _aCommonCreateUserID As String
    <DataMember()> _
    Public Property ACommonCreateUserID() As String
        Get
            Return _aCommonCreateUserID
        End Get
        Set(ByVal value As String)
            _aCommonCreateUserID = value
        End Set
    End Property

    Private _aCommonUpdateDte As DateTime
    <DataMember()> _
    Public Property ACommonUpdateDte() As DateTime
        Get
            Return _aCommonUpdateDte
        End Get
        Set(ByVal value As DateTime)
            _aCommonUpdateDte = value
        End Set
    End Property

    Private _aCommonUpdateUserID As String
    <DataMember()> _
    Public Property ACommonUpdateUserID() As String
        Get
            Return _aCommonUpdateUserID
        End Get
        Set(ByVal value As String)
            _aCommonUpdateUserID = value
        End Set
    End Property

    Private _aCommonAuditType As String
    <DataMember()> _
    Public Property ACommonAuditType() As String
        Get
            Return _aCommonAuditType
        End Get
        Set(ByVal value As String)
            _aCommonAuditType = value
        End Set
    End Property


End Class
