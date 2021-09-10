<DataContract()> _
Public Class MR001GetRackItemBalanceDetails

    Private _StockItemStoreID As String
    <DataMember()> _
    Public Property StockItemStoreID() As String
        Get
            Return _StockItemStoreID
        End Get
        Set(ByVal value As String)
            _StockItemStoreID = value
        End Set
    End Property

    Private _StockBalance As Decimal
    <DataMember()> _
    Public Property StockBalance() As Decimal
        Get
            Return _StockBalance
        End Get
        Set(ByVal value As Decimal)
            _StockBalance = value
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

End Class
