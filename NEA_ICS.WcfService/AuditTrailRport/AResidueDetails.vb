''' <summary>
''' Data Contract - For AAR004  Residue Cost Adjustments
''' 18 Oct 2010 - Jianfa
''' </summary>
''' <remarks></remarks>
<DataContract()> _
Public Class AResidueDetails

    Private _aStockItemImage As String
    <DataMember()> _
    Public Property StockItemImage() As String
        Get
            Return _aStockItemImage
        End Get
        Set(ByVal value As String)
            _aStockItemImage = value
        End Set
    End Property

    Private _aStockTransactionType As String
    <DataMember()> _
    Public Property StockTransactionType() As String
        Get
            Return _aStockTransactionType
        End Get
        Set(ByVal value As String)
            _aStockTransactionType = value
        End Set
    End Property

    Private _aStockItemID As String
    <DataMember()> _
    Public Property StockItemID() As String
        Get
            Return _aStockItemID
        End Get
        Set(ByVal value As String)
            _aStockItemID = value
        End Set
    End Property

    Private _aStockItemDescription As String
    <DataMember()> _
    Public Property StockItemDescription() As String
        Get
            Return _aStockItemDescription
        End Get
        Set(ByVal value As String)
            _aStockItemDescription = value
        End Set
    End Property

    Private _aStockItemUOM As String
    <DataMember()> _
    Public Property StockItemUOM() As String
        Get
            Return _aStockItemUOM
        End Get
        Set(ByVal value As String)
            _aStockItemUOM = value
        End Set
    End Property

    Private _aStockItemQty As Decimal
    <DataMember()> _
    Public Property StockItemQty() As Decimal
        Get
            Return _aStockItemQty
        End Get
        Set(ByVal value As Decimal)
            _aStockItemQty = value
        End Set
    End Property

    Private _aStockItemTotalValue As Decimal
    <DataMember()> _
    Public Property StockItemTotalValue() As Decimal
        Get
            Return _aStockItemTotalValue
        End Get
        Set(ByVal value As Decimal)
            _aStockItemTotalValue = value
        End Set
    End Property

    Private _aStockTransactionDte As DateTime
    <DataMember()> _
    Public Property StockTransactionDte() As DateTime
        Get
            Return _aStockTransactionDte
        End Get
        Set(ByVal value As DateTime)
            _aStockTransactionDte = value
        End Set
    End Property

    Private _aStockItemRemarks As String
    <DataMember()> _
    Public Property StockItemRemarks() As String
        Get
            Return _aStockItemRemarks
        End Get
        Set(ByVal value As String)
            _aStockItemRemarks = value
        End Set
    End Property

End Class
