''' <summary>
''' DataContract - for AR003 Stock Transaction;
''' 4 Jan 08 - Liu Guo Feng;
''' </summary>
<DataContract()> _
Public Class AStockTransactionDetails

    Private _aStockTransactionStockItemID As String
    <DataMember()> _
    Public Property AStockTransactionStockItemID() As String
        Get
            Return _aStockTransactionStockItemID
        End Get
        Set(ByVal value As String)
            _aStockTransactionStockItemID = value
        End Set
    End Property

    Private _aStockTransactionStoreID As String
    <DataMember()> _
    Public Property AStockTransactionStoreID() As String
        Get
            Return _aStockTransactionStoreID
        End Get
        Set(ByVal value As String)
            _aStockTransactionStoreID = value
        End Set
    End Property

    Private _aStockItemDescription As String
    <DataMember()> _
    Public Property AStockItemDescription() As String
        Get
            Return _aStockItemDescription
        End Get
        Set(ByVal value As String)
            _aStockItemDescription = value
        End Set
    End Property

    Private _aStockTransactionType As String
    <DataMember()> _
    Public Property AStockTransactionType() As String
        Get
            Return _aStockTransactionType
        End Get
        Set(ByVal value As String)
            _aStockTransactionType = value
        End Set
    End Property

    Private _aStockTransactionInvolveID As String
    <DataMember()> _
    Public Property AStockTransactionInvolveID() As String
        Get
            Return _aStockTransactionInvolveID
        End Get
        Set(ByVal value As String)
            _aStockTransactionInvolveID = value
        End Set
    End Property

    Private _aStockTransactionDocNo As String
    <DataMember()> _
    Public Property AStockTransactionDocNo() As String
        Get
            Return _aStockTransactionDocNo
        End Get
        Set(ByVal value As String)
            _aStockTransactionDocNo = value
        End Set
    End Property

    Private _aStockTransactionSerialNo As String
    <DataMember()> _
    Public Property AStockTransactionSerialNo() As String
        Get
            Return _aStockTransactionSerialNo
        End Get
        Set(ByVal value As String)
            _aStockTransactionSerialNo = value
        End Set
    End Property

    Private _aStockTransactionDte As DateTime
    <DataMember()> _
    Public Property AStockTransactionDte() As DateTime
        Get
            Return _aStockTransactionDte
        End Get
        Set(ByVal value As DateTime)
            _aStockTransactionDte = value
        End Set
    End Property

    Private _aStockTransactionQty As Decimal
    <DataMember()> _
    Public Property AStockTransactionQty() As Decimal
        Get
            Return _aStockTransactionQty
        End Get
        Set(ByVal value As Decimal)
            _aStockTransactionQty = value
        End Set
    End Property

    Private _aStockTransactionUnitCost As Decimal
    <DataMember()> _
    Public Property AStockTransactionUnitCost() As Decimal
        Get
            Return _aStockTransactionUnitCost
        End Get
        Set(ByVal value As Decimal)
            _aStockTransactionUnitCost = value
        End Set
    End Property

    Private _aStockTransactionTotalCost As Decimal
    <DataMember()> _
    Public Property AStockTransactionTotalCost() As Decimal
        Get
            Return _aStockTransactionTotalCost
        End Get
        Set(ByVal value As Decimal)
            _aStockTransactionTotalCost = value
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

    Private _aStockTransactionRemarks As String
    <DataMember()> _
    Public Property AStockTransactionRemarks() As String
        Get
            Return _aStockTransactionRemarks
        End Get
        Set(ByVal value As String)
            _aStockTransactionRemarks = value
        End Set
    End Property

    Private _aStockTransactionCreateDte As DateTime
    <DataMember()> _
    Public Property AStockTransactionCreateDte() As DateTime
        Get
            Return _aStockTransactionCreateDte
        End Get
        Set(ByVal value As DateTime)
            _aStockTransactionCreateDte = value
        End Set
    End Property

    Private _aStockTransactionUserID As String
    <DataMember()> _
    Public Property AStockTransactionUserID() As String
        Get
            Return _aStockTransactionUserID
        End Get
        Set(ByVal value As String)
            _aStockTransactionUserID = value
        End Set
    End Property

    Private _aStockTransactionAuditType As String
    <DataMember()> _
    Public Property AStockTransactionAuditType() As String
        Get
            Return _aStockTransactionAuditType
        End Get
        Set(ByVal value As String)
            _aStockTransactionAuditType = value
        End Set
    End Property

End Class
