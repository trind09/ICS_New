<DataContract()> _
Public Class MR008QuantityIssueSummaryDetails

    Private _FIssueConsumerID As String
    <DataMember()> _
    Public Property FIssueConsumerID() As String
        Get
            Return _FIssueConsumerID
        End Get
        Set(ByVal value As String)
            _FIssueConsumerID = value
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

    Private _SumIssueQty As Decimal
    <DataMember()> _
    Public Property SumIssueQty() As Decimal
        Get
            Return _SumIssueQty
        End Get
        Set(ByVal value As Decimal)
            _SumIssueQty = value
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

    Private _SumIssueTotalCost As Decimal
    <DataMember()> _
    Public Property SumIssueTotalCost() As Decimal
        Get
            Return _SumIssueTotalCost
        End Get
        Set(ByVal value As Decimal)
            _SumIssueTotalCost = value
        End Set
    End Property
End Class
