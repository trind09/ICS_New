''' <summary>
''' DataContract - for MR002 Transaction List;
''' 29 Dec 08 - Liu Guo Feng;
''' </summary>
<DataContract()> _
Public Class TransactionListDetails

    Private _storeID As String
    <DataMember()> _
    Public Property StoreID() As String
        Get
            Return _storeID
        End Get
        Set(ByVal value As String)
            _storeID = value
        End Set
    End Property

    Private _stockID As String
    <DataMember()> _
    Public Property StockID() As String
        Get
            Return _stockID
        End Get
        Set(ByVal value As String)
            _stockID = value
        End Set
    End Property

    Private _description As String
    <DataMember()> _
    Public Property Description() As String
        Get
            Return _description
        End Get
        Set(ByVal value As String)
            _description = value
        End Set
    End Property

    Private _UOM As String
    <DataMember()> _
    Public Property UOM() As String
        Get
            Return _UOM
        End Get
        Set(ByVal value As String)
            _UOM = value
        End Set
    End Property

    Private _transType As String
    <DataMember()> _
    Public Property TransType() As String
        Get
            Return _transType
        End Get
        Set(ByVal value As String)
            _transType = value
        End Set
    End Property

    Private _transDate As DateTime
    <DataMember()> _
    Public Property TransDate() As DateTime
        Get
            Return _transDate
        End Get
        Set(ByVal value As DateTime)
            _transDate = value
        End Set
    End Property

    Private _transID As String
    <DataMember()> _
    Public Property TransID() As String
        Get
            Return _transID
        End Get
        Set(ByVal value As String)
            _transID = value
        End Set
    End Property

    Private _itemRef As Integer
    <DataMember()> _
    Public Property ItemRef() As Integer
        Get
            Return _itemRef
        End Get
        Set(ByVal value As Integer)
            _itemRef = value
        End Set
    End Property
    Private _serialNo As String
    <DataMember()> _
    Public Property SerialNo() As String
        Get
            Return _serialNo
        End Get
        Set(ByVal value As String)
            _serialNo = value
        End Set
    End Property
    Private _toOrFrom As String
    <DataMember()> _
    Public Property ToOrFrom() As String
        Get
            Return _toOrFrom
        End Get
        Set(ByVal value As String)
            _toOrFrom = value
        End Set
    End Property
    Private _receiptsQty As Decimal
    <DataMember()> _
    Public Property ReceiptsQty() As Decimal
        Get
            Return _receiptsQty
        End Get
        Set(ByVal value As Decimal)
            _receiptsQty = value
        End Set
    End Property
    Private _issuesQty As Decimal
    <DataMember()> _
    Public Property IssuesQty() As Decimal
        Get
            Return _issuesQty
        End Get
        Set(ByVal value As Decimal)
            _issuesQty = value
        End Set
    End Property
    Private _totalCost As Decimal
    <DataMember()> _
    Public Property TotalCost() As Decimal
        Get
            Return _totalCost
        End Get
        Set(ByVal value As Decimal)
            _totalCost = value
        End Set
    End Property
    Private _balanceQty As Decimal
    <DataMember()> _
    Public Property BalanceQty() As Decimal
        Get
            Return _balanceQty
        End Get
        Set(ByVal value As Decimal)
            _balanceQty = value
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
    Private _totalValue As Decimal
    <DataMember()> _
    Public Property TotalValue() As Decimal
        Get
            Return _totalValue
        End Get
        Set(ByVal value As Decimal)
            _totalValue = value
        End Set
    End Property
    Private _totalQty As Decimal
    <DataMember()> _
    Public Property TotalQty() As Decimal
        Get
            Return _totalQty
        End Get
        Set(ByVal value As Decimal)
            _totalQty = value
        End Set
    End Property
    Private _unitCost As Decimal
    <DataMember()> _
    Public Property UnitCost() As Decimal
        Get
            Return _unitCost
        End Get
        Set(ByVal value As Decimal)
            _unitCost = value
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
