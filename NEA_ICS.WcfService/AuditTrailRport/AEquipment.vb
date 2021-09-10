Public Class AEquipment
    Private _aEquipmentStoreID As String
    Public Property AEquipmentStoreID() As String
        Get
            Return _aEquipmentStoreID
        End Get
        Set(ByVal value As String)
            _aEquipmentStoreID = value
        End Set
    End Property

    Private _aEquipmentID As String
    Public Property AEquipmentID() As String
        Get
            Return _aEquipmentID
        End Get
        Set(ByVal value As String)
            _aEquipmentID = value
        End Set
    End Property

    Private _aEquipmentType As String
    Public Property AEquipmentType() As String
        Get
            Return _aEquipmentType
        End Get
        Set(ByVal value As String)
            _aEquipmentType = value
        End Set
    End Property

    Private _aEquipmentDescription As String
    Public Property AEquipmentDescription() As String
        Get
            Return _aEquipmentDescription
        End Get
        Set(ByVal value As String)
            _aEquipmentDescription = value
        End Set
    End Property

    Private _aEquipmentStatus As String
    Public Property AEquipmentStatus() As String
        Get
            Return _aEquipmentStatus
        End Get
        Set(ByVal value As String)
            _aEquipmentStatus = value
        End Set
    End Property

    Private _aEquipmentCreateDte As DateTime
    Public Property AEquipmentCreateDte() As DateTime
        Get
            Return _aEquipmentCreateDte
        End Get
        Set(ByVal value As DateTime)
            _aEquipmentCreateDte = value
        End Set
    End Property

    Private _aEquipmentCreateUserID As String
    Public Property AEquipmentCreateUserID() As String
        Get
            Return _aEquipmentCreateUserID
        End Get
        Set(ByVal value As String)
            _aEquipmentCreateUserID = value
        End Set
    End Property

    Private _aEquipmentUpdateDte As DateTime
    Public Property AEquipmentUpdateDte() As DateTime
        Get
            Return _aEquipmentUpdateDte
        End Get
        Set(ByVal value As DateTime)
            _aEquipmentUpdateDte = value
        End Set
    End Property

    Private _aEquipmentUpdateUserID As String
    Public Property AEquipmentUpdateUserID() As String
        Get
            Return _aEquipmentUpdateUserID
        End Get
        Set(ByVal value As String)
            _aEquipmentUpdateUserID = value
        End Set
    End Property

    Private _aEquipmentAuditType As String
    Public Property AEquipmentAuditType() As String
        Get
            Return _aEquipmentAuditType
        End Get
        Set(ByVal value As String)
            _aEquipmentAuditType = value
        End Set
    End Property

End Class
