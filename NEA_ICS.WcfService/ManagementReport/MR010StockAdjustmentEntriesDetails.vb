<DataContract()> _
Public Class MR010StockAdjustmentEntriesDetails

    Private _AdjustDocReturn As String
    <DataMember()> _
    Public Property AdjustDocReturn() As String
        Get
            Return _AdjustDocReturn
        End Get
        Set(ByVal value As String)
            _AdjustDocReturn = value
        End Set
    End Property

    Private _AdjustAdjustID As String
    <DataMember()> _
    Public Property AdjustAdjustID() As String
        Get
            Return _AdjustAdjustID
        End Get
        Set(ByVal value As String)
            _AdjustAdjustID = value
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

    Private _AdjustType As String
    <DataMember()> _
    Public Property AdjustType() As String
        Get
            Return _AdjustType
        End Get
        Set(ByVal value As String)
            _AdjustType = value
        End Set
    End Property

    Private _AdjustDte As DateTime
    <DataMember()> _
    Public Property AdjustDte() As DateTime
        Get
            Return _AdjustDte
        End Get
        Set(ByVal value As DateTime)
            _AdjustDte = value
        End Set
    End Property

    Private _AdjustQty As Decimal
    <DataMember()> _
    Public Property AdjustQty() As Decimal
        Get
            Return _AdjustQty
        End Get
        Set(ByVal value As Decimal)
            _AdjustQty = value
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

    Private _AdjustTotalCost As Decimal
    <DataMember()> _
    Public Property AdjustTotalCost() As Decimal
        Get
            Return _AdjustTotalCost
        End Get
        Set(ByVal value As Decimal)
            _AdjustTotalCost = value
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
    Private _AdjustRemarks As String
    <DataMember()> _
    Public Property AdjustRemarks() As String
        Get
            Return _AdjustRemarks
        End Get
        Set(ByVal value As String)
            _AdjustRemarks = value
        End Set
    End Property
End Class
