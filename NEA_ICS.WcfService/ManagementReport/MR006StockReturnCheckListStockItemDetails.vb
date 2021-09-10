<DataContract()> _
Public Class MR006StockReturnCheckListStockItemDetails

    Private _StockType As String
    <DataMember()> _
    Public Property StockType() As String
        Get
            Return _StockType
        End Get
        Set(ByVal value As String)
            _StockType = value
        End Set
    End Property

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

    Private _StockItemPartNo As String
    <DataMember()> _
    Public Property StockItemPartNo() As String
        Get
            Return _StockItemPartNo
        End Get
        Set(ByVal value As String)
            _StockItemPartNo = value
        End Set
    End Property

    Private _StockItemLocation As String
    <DataMember()> _
    Public Property StockItemLocation() As String
        Get
            Return _StockItemLocation
        End Get
        Set(ByVal value As String)
            _StockItemLocation = value
        End Set
    End Property
    Private _StockItemLocation2 As String
    <DataMember()> _
    Public Property StockItemLocation2() As String
        Get
            Return _StockItemLocation2
        End Get
        Set(ByVal value As String)
            _StockItemLocation2 = value
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

    Private _StockItemUnitCost As Decimal
    <DataMember()> _
    Public Property StockItemUnitCost() As Decimal
        Get
            Return _StockItemUnitCost
        End Get
        Set(ByVal value As Decimal)
            _StockItemUnitCost = value
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

    Private _ExcludeZero As Boolean
    <DataMember()> _
    Public Property ExcludeZero() As Boolean
        Get
            Return _ExcludeZero
        End Get
        Set(ByVal value As Boolean)
            _ExcludeZero = value
        End Set
    End Property
End Class
