<DataContract()> _
Public Class MR006StockReturnCheckListReceiveDetails

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

    Private _ReceiveType As String
    <DataMember()> _
    Public Property ReceiveType() As String
        Get
            Return _ReceiveType
        End Get
        Set(ByVal value As String)
            _ReceiveType = value
        End Set
    End Property

    Private _ReceiveDte As DateTime
    <DataMember()> _
    Public Property ReceiveDte() As DateTime
        Get
            Return _ReceiveDte
        End Get
        Set(ByVal value As DateTime)
            _ReceiveDte = value
        End Set
    End Property

    Private _ReceiveQty As Decimal
    <DataMember()> _
    Public Property ReceiveQty() As Decimal
        Get
            Return _ReceiveQty
        End Get
        Set(ByVal value As Decimal)
            _ReceiveQty = value
        End Set
    End Property
    Private _ReceiveUOM As String
    <DataMember()> _
    Public Property ReceiveUOM() As String
        Get
            Return _ReceiveUOM
        End Get
        Set(ByVal value As String)
            _ReceiveUOM = value
        End Set
    End Property

    Private _ReceiveUnitCost As Decimal
    <DataMember()> _
    Public Property ReceiveUnitCost() As Decimal
        Get
            Return _ReceiveUnitCost
        End Get
        Set(ByVal value As Decimal)
            _ReceiveUnitCost = value
        End Set
    End Property
    Private _ReceiveTotalCost As Decimal
    <DataMember()> _
    Public Property ReceiveTotalCost() As Decimal
        Get
            Return _ReceiveTotalCost
        End Get
        Set(ByVal value As Decimal)
            _ReceiveTotalCost = value
        End Set
    End Property
    Private _ReceiveDocNo As String
    <DataMember()> _
    Public Property ReceiveDocNo() As String
        Get
            Return _ReceiveDocNo
        End Get
        Set(ByVal value As String)
            _ReceiveDocNo = value
        End Set
    End Property
    Private _ReceiveRemarks As String
    <DataMember()> _
    Public Property ReceiveRemarks() As String
        Get
            Return _ReceiveRemarks
        End Get
        Set(ByVal value As String)
            _ReceiveRemarks = value
        End Set
    End Property
End Class
