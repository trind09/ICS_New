''' <summary>
''' DataContract - for Order List;
''' 22Feb09 - KG;
''' </summary>
''' <remarks>
''' CHANGE LOG:
''' ddMMMyy  AuthorName  RefID  Description;
''' </remarks>
<DataContract()> _
Public Class OrderList

    Private _stockType As String
    <DataMember()> _
    Public Property StockType() As String
        Get
            Return _stockType
        End Get
        Set(ByVal value As String)
            _stockType = value
        End Set
    End Property

    Private _stockItemID As String
    <DataMember()> _
    Public Property StockItemID() As String
        Get
            Return _stockItemID
        End Get
        Set(ByVal value As String)
            _stockItemID = value
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

    Private _uom As String
    <DataMember()> _
    Public Property UOM() As String
        Get
            Return _uom
        End Get
        Set(ByVal value As String)
            _uom = value
        End Set
    End Property

    Private _orderID As String
    <DataMember()> _
    Public Property OrderID() As String
        Get
            Return _orderID
        End Get
        Set(ByVal value As String)
            _orderID = value
        End Set
    End Property

    Private _supplierID As String
    <DataMember()> _
    Public Property SupplierID() As String
        Get
            Return _supplierID
        End Get
        Set(ByVal value As String)
            _supplierID = value
        End Set
    End Property

    Private _orderDte As Date
    <DataMember()> _
    Public Property OrderDte() As Date
        Get
            Return _orderDte
        End Get
        Set(ByVal value As Date)
            _orderDte = value
        End Set
    End Property

    Private _orderQty As Decimal
    <DataMember()> _
    Public Property OrderQty() As Decimal
        Get
            Return _orderQty
        End Get
        Set(ByVal value As Decimal)
            _orderQty = value
        End Set
    End Property

    Private _totalCost As Double
    <DataMember()> _
    Public Property TotalCost() As Double
        Get
            Return _totalCost
        End Get
        Set(ByVal value As Double)
            _totalCost = value
        End Set
    End Property

    Private _remarks As String
    <DataMember()> _
    Public Property Remarks() As String
        Get
            Return _remarks
        End Get
        Set(ByVal value As String)
            _remarks = value
        End Set
    End Property

    Private _status As String
    <DataMember()> _
    Public Property Status() As String
        Get
            Return _status
        End Get
        Set(ByVal value As String)
            _status = value
        End Set
    End Property

    Private _lastReceiveDte As Date
    <DataMember()> _
    Public Property LastReceiveDte() As Date
        Get
            Return _lastReceiveDte
        End Get
        Set(ByVal value As Date)
            _lastReceiveDte = value
        End Set
    End Property

    Private _totalReceiveQty As Decimal
    <DataMember()> _
    Public Property TotalReceiveQty() As Decimal
        Get
            Return _totalReceiveQty
        End Get
        Set(ByVal value As Decimal)
            _totalReceiveQty = value
        End Set
    End Property

    Private _lastReceiveQty As Decimal
    <DataMember()> _
    Public Property LastReceiveQty() As Decimal
        Get
            Return _lastReceiveQty
        End Get
        Set(ByVal value As Decimal)
            _lastReceiveQty = value
        End Set
    End Property
End Class
