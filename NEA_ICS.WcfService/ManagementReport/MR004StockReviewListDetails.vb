<DataContract()> _
Public Class MR004StockReviewListDetails


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

    Private _StockItemBalanceQty As Decimal
    <DataMember()> _
    Public Property StockItemBalanceQty() As Decimal
        Get
            Return _StockItemBalanceQty
        End Get
        Set(ByVal value As Decimal)
            _StockItemBalanceQty = value
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
    Private _AvgIssue As Decimal
    <DataMember()> _
    Public Property AvgIssue() As Decimal
        Get
            Return _AvgIssue
        End Get
        Set(ByVal value As Decimal)
            _AvgIssue = value
        End Set
    End Property
    Private _HighestIssue As Decimal
    <DataMember()> _
    Public Property HighestIssue() As Decimal
        Get
            Return _HighestIssue
        End Get
        Set(ByVal value As Decimal)
            _HighestIssue = value
        End Set
    End Property
    Private _AvgIssuePerMth As Decimal
    <DataMember()> _
    Public Property AvgIssuePerMth() As Decimal
        Get
            Return _AvgIssuePerMth
        End Get
        Set(ByVal value As Decimal)
            _AvgIssuePerMth = value
        End Set
    End Property
    Private _NoOfIssue As Decimal
    <DataMember()> _
    Public Property NoOfIssue() As Decimal
        Get
            Return _NoOfIssue
        End Get
        Set(ByVal value As Decimal)
            _NoOfIssue = value
        End Set
    End Property
    Private _TotalIssueQty As Decimal
    <DataMember()> _
    Public Property TotalIssueQty() As Decimal
        Get
            Return _TotalIssueQty
        End Get
        Set(ByVal value As Decimal)
            _TotalIssueQty = value
        End Set
    End Property
    Private _NoOfOrder As Decimal
    <DataMember()> _
    Public Property NoOfOrder() As Decimal
        Get
            Return _NoOfOrder
        End Get
        Set(ByVal value As Decimal)
            _NoOfOrder = value
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

    Private _DesireMaxlevel As Decimal
    <DataMember()> _
    Public Property DesireMaxlevel() As Decimal
        Get
            Return _DesireMaxlevel
        End Get
        Set(ByVal value As Decimal)
            _DesireMaxlevel = value
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
    Private _DesireOrderlevel As Decimal
    <DataMember()> _
    Public Property DesireOrderlevel() As Decimal
        Get
            Return _DesireOrderlevel
        End Get
        Set(ByVal value As Decimal)
            _DesireOrderlevel = value
        End Set
    End Property

End Class
