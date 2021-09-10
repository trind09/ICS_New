''' <summary>
''' DataContract - for AR002 Order;
''' 4 Jan 08 - Liu Guo Feng;
''' </summary>
<DataContract()> _
Public Class AOrderDetails

    Private _aOrderImage As String
    <DataMember()> _
    Public Property AOrderImage() As String
        Get
            Return _aOrderImage
        End Get
        Set(ByVal value As String)
            _aOrderImage = value
        End Set
    End Property

    Private _aOrderItemStockItemID As String
    <DataMember()> _
    Public Property AOrderItemStockItemID() As String
        Get
            Return _aOrderItemStockItemID
        End Get
        Set(ByVal value As String)
            _aOrderItemStockItemID = value
        End Set
    End Property

    Private _aOrderStoreID As String
    <DataMember()> _
    Public Property AOrderStoreID() As String
        Get
            Return _aOrderStoreID
        End Get
        Set(ByVal value As String)
            _aOrderStoreID = value
        End Set
    End Property

    Private _aOrderID As String
    <DataMember()> _
    Public Property AOrderID() As String
        Get
            Return _aOrderID
        End Get
        Set(ByVal value As String)
            _aOrderID = value
        End Set
    End Property

    Private _aStockItemDescriptionn As String
    <DataMember()> _
    Public Property AStockItemDescription() As String
        Get
            Return _aStockItemDescriptionn
        End Get
        Set(ByVal value As String)
            _aStockItemDescriptionn = value
        End Set
    End Property

    Private _aOrderDte As DateTime
    <DataMember()> _
    Public Property AOrderDte() As DateTime
        Get
            Return _aOrderDte
        End Get
        Set(ByVal value As DateTime)
            _aOrderDte = value
        End Set
    End Property

    Private _aOrderSupplierID As String
    <DataMember()> _
    Public Property AOrderSupplierID() As String
        Get
            Return _aOrderSupplierID
        End Get
        Set(ByVal value As String)
            _aOrderSupplierID = value
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

    Private _aOrderItemQty As Decimal
    <DataMember()> _
    Public Property AOrderItemQty() As Decimal
        Get
            Return _aOrderItemQty
        End Get
        Set(ByVal value As Decimal)
            _aOrderItemQty = value
        End Set
    End Property

    Private _aOrderItemUnitCost As Decimal
    <DataMember()> _
    Public Property AOrderItemUnitCost() As Decimal
        Get
            Return _aOrderItemUnitCost
        End Get
        Set(ByVal value As Decimal)
            _aOrderItemUnitCost = value
        End Set
    End Property

    Private _aOrderItemTotalCost As Decimal
    <DataMember()> _
    Public Property AOrderItemTotalCost() As Decimal
        Get
            Return _aOrderItemTotalCost
        End Get
        Set(ByVal value As Decimal)
            _aOrderItemTotalCost = value
        End Set
    End Property

    Private _aOrderCreateDte As DateTime
    <DataMember()> _
    Public Property AOrderCreateDte() As DateTime
        Get
            Return _aOrderCreateDte
        End Get
        Set(ByVal value As DateTime)
            _aOrderCreateDte = value
        End Set
    End Property

    Private _aOrderUserID As String
    <DataMember()> _
    Public Property AOrderUserID() As String
        Get
            Return _aOrderUserID
        End Get
        Set(ByVal value As String)
            _aOrderUserID = value
        End Set
    End Property

    Private _aOrderAuditType As String
    <DataMember()> _
    Public Property AOrderAuditType() As String
        Get
            Return _aOrderAuditType
        End Get
        Set(ByVal value As String)
            _aOrderAuditType = value
        End Set
    End Property

End Class
