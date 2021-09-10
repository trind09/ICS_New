''' <summary>
''' DataContract - for AR001 Stock Item;
''' 4 Jan 08 - Liu Guo Feng;
''' </summary>
<DataContract()> _
Public Class AStockItemDetails

    Private _aStockItemImage As String
    <DataMember()> _
    Public Property AStockItemImage() As String
        Get
            Return _aStockItemImage
        End Get
        Set(ByVal value As String)
            _aStockItemImage = value
        End Set
    End Property

    Private _aStockItemAuditID As Integer
    <DataMember()> _
    Public Property AStockItemAuditID() As Integer
        Get
            Return _aStockItemAuditID
        End Get
        Set(ByVal value As Integer)
            _aStockItemAuditID = value
        End Set
    End Property

    Private _aStockItemStoreID As String
    <DataMember()> _
    Public Property AStockItemStoreID() As String
        Get
            Return _aStockItemStoreID
        End Get
        Set(ByVal value As String)
            _aStockItemStoreID = value
        End Set
    End Property

    Private _aStockItemID As String
    <DataMember()> _
    Public Property AStockItemID() As String
        Get
            Return _aStockItemID
        End Get
        Set(ByVal value As String)
            _aStockItemID = value
        End Set
    End Property

    Private _aStockItemDescription As String
    <DataMember()> _
    Public Property AStockItemDescription() As String
        Get
            Return _aStockItemDescription
        End Get
        Set(ByVal value As String)
            _aStockItemDescription = value
        End Set
    End Property

    Private _aStockItemPartNo As String
    <DataMember()> _
    Public Property AStockItemPartNo() As String
        Get
            Return _aStockItemPartNo
        End Get
        Set(ByVal value As String)
            _aStockItemPartNo = value
        End Set
    End Property

    Private _aStockItemLocation As String
    <DataMember()> _
    Public Property AStockItemLocation() As String
        Get
            Return _aStockItemLocation
        End Get
        Set(ByVal value As String)
            _aStockItemLocation = value
        End Set
    End Property

    Private _aStockItemUOM As String
    <DataMember()> _
    Public Property AStockItemUOM() As String
        Get
            Return _aStockItemUOM
        End Get
        Set(ByVal value As String)
            _aStockItemUOM = value
        End Set
    End Property

    Private _aStockItemStockType As String
    <DataMember()> _
    Public Property AStockItemStockType() As String
        Get
            Return _aStockItemStockType
        End Get
        Set(ByVal value As String)
            _aStockItemStockType = value
        End Set
    End Property

    Private _aStockItemMaxLevel As Decimal
    <DataMember()> _
    Public Property AStockItemMaxLevel() As Decimal
        Get
            Return _aStockItemMaxLevel
        End Get
        Set(ByVal value As Decimal)
            _aStockItemMaxLevel = value
        End Set
    End Property

    Private _aStockItemMinLevel As Decimal
    <DataMember()> _
    Public Property AStockItemMinLevel() As Decimal
        Get
            Return _aStockItemMinLevel
        End Get
        Set(ByVal value As Decimal)
            _aStockItemMinLevel = value
        End Set
    End Property

    Private _aStockItemReorderLevel As Decimal
    <DataMember()> _
    Public Property AStockItemReorderLevel() As Decimal
        Get
            Return _aStockItemReorderLevel
        End Get
        Set(ByVal value As Decimal)
            _aStockItemReorderLevel = value
        End Set
    End Property

    Private _aStockItemStatus As String
    <DataMember()> _
    Public Property AStockItemStatus() As String
        Get
            Return _aStockItemStatus
        End Get
        Set(ByVal value As String)
            _aStockItemStatus = value
        End Set
    End Property

    Private _aStockItemCreateDte As DateTime
    <DataMember()> _
    Public Property AStockItemCreateDte() As DateTime
        Get
            Return _aStockItemCreateDte
        End Get
        Set(ByVal value As DateTime)
            _aStockItemCreateDte = value
        End Set
    End Property

    Private _aStockItemUserID As String
    <DataMember()> _
    Public Property AStockItemUserID() As String
        Get
            Return _aStockItemUserID
        End Get
        Set(ByVal value As String)
            _aStockItemUserID = value
        End Set
    End Property

    Private _aStockItemAuditType As String
    <DataMember()> _
    Public Property AStockItemAuditType() As String
        Get
            Return _aStockItemAuditType
        End Get
        Set(ByVal value As String)
            _aStockItemAuditType = value
        End Set
    End Property

End Class
