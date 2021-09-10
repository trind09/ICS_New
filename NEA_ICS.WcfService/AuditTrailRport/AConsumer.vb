Public Class AConsumer
    Private _aConsumerStoreID As String
    Public Property AConsumerStoreID() As String
        Get
            Return _aConsumerStoreID
        End Get
        Set(ByVal value As String)
            _aConsumerStoreID = value
        End Set
    End Property

    Private _aConsumerID As String
    Public Property AConsumerID() As String
        Get
            Return _aConsumerID
        End Get
        Set(ByVal value As String)
            _aConsumerID = value
        End Set
    End Property

    Private _aConsumerDescription As String
    Public Property AConsumerDescription() As String
        Get
            Return _aConsumerDescription
        End Get
        Set(ByVal value As String)
            _aConsumerDescription = value
        End Set
    End Property

    Private _aConsumerStatus As String
    Public Property AConsumerStatus() As String
        Get
            Return _aConsumerStatus
        End Get
        Set(ByVal value As String)
            _aConsumerStatus = value
        End Set
    End Property

    Private _aConsumerCreateDte As DateTime
    Public Property AConsumerCreateDte() As DateTime
        Get
            Return _aConsumerCreateDte
        End Get
        Set(ByVal value As DateTime)
            _aConsumerCreateDte = value
        End Set
    End Property

    Private _aConsumerCreateUserID As String
    Public Property AConsumerCreateUserID() As String
        Get
            Return _aConsumerCreateUserID
        End Get
        Set(ByVal value As String)
            _aConsumerCreateUserID = value
        End Set
    End Property

    Private _aConsumerUpdateDte As DateTime
    Public Property AConsumerUpdateDte() As DateTime
        Get
            Return _aConsumerUpdateDte
        End Get
        Set(ByVal value As DateTime)
            _aConsumerUpdateDte = value
        End Set
    End Property

    Private _aConsumerUpdateUserID As String
    Public Property AConsumerUpdateUserID() As String
        Get
            Return _aConsumerUpdateUserID
        End Get
        Set(ByVal value As String)
            _aConsumerUpdateUserID = value
        End Set
    End Property

    Private _aConsumerAuditType As String
    Public Property AConsumerAuditType() As String
        Get
            Return _aConsumerAuditType
        End Get
        Set(ByVal value As String)
            _aConsumerAuditType = value
        End Set
    End Property
    Private _consumerStoreRefUserID As String
    Public Property ConsumerStoreRefUserID() As String
        Get
            Return _consumerStoreRefUserID
        End Get
        Set(ByVal value As String)
            _consumerStoreRefUserID = value
        End Set
    End Property

End Class
