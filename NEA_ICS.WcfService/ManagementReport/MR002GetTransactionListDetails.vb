<DataContract()> _
Public Class MR002GetTransactionListDetails

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

    Private _StockTransactionType As String
    <DataMember()> _
    Public Property StockTransactionType() As String
        Get
            Return _StockTransactionType
        End Get
        Set(ByVal value As String)
            _StockTransactionType = value
        End Set
    End Property

    Private _StockTransactionDte As DateTime
    <DataMember()> _
    Public Property StockTransactionDte() As DateTime
        Get
            Return _StockTransactionDte
        End Get
        Set(ByVal value As DateTime)
            _StockTransactionDte = value
        End Set
    End Property

    Private _StockTransactionID As Integer
    <DataMember()> _
    Public Property StockTransactionID() As Integer
        Get
            Return _StockTransactionID
        End Get
        Set(ByVal value As Integer)
            _StockTransactionID = value
        End Set
    End Property
    Private _StockTransactionItemRef As Integer
    <DataMember()> _
    Public Property StockTransactionItemRef() As Integer
        Get
            Return _StockTransactionItemRef
        End Get
        Set(ByVal value As Integer)
            _StockTransactionItemRef = value
        End Set
    End Property
    Private _SerialNo As String
    <DataMember()> _
    Public Property SerialNo() As String
        Get
            Return _SerialNo
        End Get
        Set(ByVal value As String)
            _SerialNo = value
        End Set
    End Property
    Private _ToOrFrom As String
    <DataMember()> _
    Public Property ToOrFrom() As String
        Get
            Return _ToOrFrom
        End Get
        Set(ByVal value As String)
            _ToOrFrom = value
        End Set
    End Property
    Private _ReceiptsQty As Decimal
    <DataMember()> _
    Public Property ReceiptsQty() As Decimal
        Get
            Return _ReceiptsQty
        End Get
        Set(ByVal value As Decimal)
            _ReceiptsQty = value
        End Set
    End Property
    Private _IssuesQty As Decimal
    <DataMember()> _
    Public Property IssuesQty() As Decimal
        Get
            Return _IssuesQty
        End Get
        Set(ByVal value As Decimal)
            _IssuesQty = value
        End Set
    End Property
    Private _TotalCost As Decimal
    <DataMember()> _
    Public Property TotalCost() As Decimal
        Get
            Return _TotalCost
        End Get
        Set(ByVal value As Decimal)
            _TotalCost = value
        End Set
    End Property
    Private _BalanceQty As Decimal
    <DataMember()> _
    Public Property BalanceQty() As Decimal
        Get
            Return _BalanceQty
        End Get
        Set(ByVal value As Decimal)
            _BalanceQty = value
        End Set
    End Property
    Private _AUCost As Decimal
    <DataMember()> _
    Public Property AUCost() As Decimal
        Get
            Return _AUCost
        End Get
        Set(ByVal value As Decimal)
            _AUCost = value
        End Set
    End Property
    Private _TotalValue As Decimal
    <DataMember()> _
    Public Property TotalValue() As Decimal
        Get
            Return _TotalValue
        End Get
        Set(ByVal value As Decimal)
            _TotalValue = value
        End Set
    End Property
    Private _UnitCost As Decimal
    <DataMember()> _
    Public Property UnitCost() As Decimal
        Get
            Return _UnitCost
        End Get
        Set(ByVal value As Decimal)
            _UnitCost = value
        End Set
    End Property
    Private _TotalQty As Decimal
    <DataMember()> _
    Public Property TotalQty() As Decimal
        Get
            Return _TotalQty
        End Get
        Set(ByVal value As Decimal)
            _TotalQty = value
        End Set
    End Property


    Private _docNo As String
    <DataMember()> _
    Public Property DocNo() As String
        Get
            Return _docNo
        End Get
        Set(ByVal value As String)
            _docNo = value
        End Set
    End Property

    Private _docReturn As String
    <DataMember()> _
    Public Property DocReturn() As String
        Get
            Return _docReturn
        End Get
        Set(ByVal value As String)
            _docReturn = value
        End Set
    End Property

End Class
