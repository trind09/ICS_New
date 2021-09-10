<DataContract()> _
Public Class MR009ReorderStockItemListDetails

    Private _StockItemID As String
    <DataMember()> _
    Public Property StockItemID() As String
        Get
            Return _StockItemID
        End Get
        Set(ByVal value As String)
            _StockItemID = value
        End Set
    End Property

    Private _StockItemDescription As String
    <DataMember()> _
    Public Property StockItemDescription() As String
        Get
            Return _StockItemDescription
        End Get
        Set(ByVal value As String)
            _StockItemDescription = value
        End Set
    End Property


    Private _StockItemStockBal As Decimal
    <DataMember()> _
    Public Property StockItemStockBal() As Decimal
        Get
            Return _StockItemStockBal
        End Get
        Set(ByVal value As Decimal)
            _StockItemStockBal = value
        End Set
    End Property
    Private _StockItemUOM As String
    <DataMember()> _
    Public Property StockItemUOM() As String
        Get
            Return _StockItemUOM
        End Get
        Set(ByVal value As String)
            _StockItemUOM = value
        End Set
    End Property

    Private _OrderItemOrderID As String
    <DataMember()> _
    Public Property OrderItemOrderID() As String
        Get
            Return _OrderItemOrderID
        End Get
        Set(ByVal value As String)
            _OrderItemOrderID = value
        End Set
    End Property
    Private _OrderItemRemarks As String
    <DataMember()> _
    Public Property OrderItemRemarks() As String
        Get
            Return _OrderItemRemarks
        End Get
        Set(ByVal value As String)
            _OrderItemRemarks = value
        End Set
    End Property
    Private _StockItemReorderLevel As Decimal
    <DataMember()> _
    Public Property StockItemReorderLevel() As Decimal
        Get
            Return _StockItemReorderLevel
        End Get
        Set(ByVal value As Decimal)
            _StockItemReorderLevel = value
        End Set
    End Property
    Private _StockItemTotalCost As Decimal
    <DataMember()> _
    Public Property StockItemTotalCost() As Decimal
        Get
            Return _StockItemTotalCost
        End Get
        Set(ByVal value As Decimal)
            _StockItemTotalCost = value
        End Set
    End Property
    Private _StockItemMinLevel As Decimal
    <DataMember()> _
    Public Property StockItemMinLevel() As Decimal
        Get
            Return _StockItemMinLevel
        End Get
        Set(ByVal value As Decimal)
            _StockItemMinLevel = value
        End Set
    End Property
    Private _StockItemMaxLevel As Decimal
    <DataMember()> _
    Public Property StockItemMaxLevel() As Decimal
        Get
            Return _StockItemMaxLevel
        End Get
        Set(ByVal value As Decimal)
            _StockItemMaxLevel = value
        End Set
    End Property
    Private _OrderItemTotalCost As Decimal
    <DataMember()> _
    Public Property OrderItemTotalCost() As Decimal
        Get
            Return _OrderItemTotalCost
        End Get
        Set(ByVal value As Decimal)
            _OrderItemTotalCost = value
        End Set
    End Property
    Private _OrderItemQty As Decimal
    <DataMember()> _
    Public Property OrderItemQty() As Decimal
        Get
            Return _OrderItemQty
        End Get
        Set(ByVal value As Decimal)
            _OrderItemQty = value
        End Set
    End Property
End Class
