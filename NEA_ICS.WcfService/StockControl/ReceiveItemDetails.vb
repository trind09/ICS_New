''' <summary>
''' DataContract - for Receive Details;
''' 11Feb09 - KG;
''' </summary>
''' <remarks>
''' CHANGE LOG:
''' ddMMMyy  AuthorName  RefID  Description;
''' </remarks>
<DataContract()> _
Public Class ReceiveItemDetails

#Region " Constructor "
    Public Sub New()
        Me.New(True)
    End Sub

    ''' <summary>
    ''' Initialise value with blank value, True;
    ''' 11Feb09 - KG;
    ''' </summary>
    ''' <param name="initialise"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Sub New(ByVal initialise As Boolean)
        If initialise Then
            _tranID = 0
            _stockItemID = ""
            _qty = 0D
            _totalCost = 0.0
            _remarks = ""
            _orderItemID = 0
            _status = ""
            _orderItemWarrantyDte = DateTime.MinValue
            _orderItemQtyOutstanding = 0.0
            _orderItemUnitCost = 0.0
            _mode = ""
        End If
    End Sub
#End Region

    Private _tranID As Integer
    <DataMember()> _
    Public Property TranID() As Integer
        Get
            Return _tranID
        End Get
        Set(ByVal value As Integer)
            _tranID = value
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

    Private _qty As Decimal
    <DataMember()> _
    Public Property Qty() As Decimal
        Get
            Return _qty
        End Get
        Set(ByVal value As Decimal)
            _qty = value
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

    Private _orderItemID As Integer
    <DataMember()> _
    Public Property OrderItemID() As Integer
        Get
            Return _orderItemID
        End Get
        Set(ByVal value As Integer)
            _orderItemID = value
        End Set
    End Property

    Private _orderItemWarrantyDte As Date
    <DataMember()> _
    Public Property OrderItemWarrantyDte() As Date
        Get
            Return _orderItemWarrantyDte
        End Get
        Set(ByVal value As Date)
            _orderItemWarrantyDte = value
        End Set
    End Property

    Private _orderItemQtyOutstanding As Decimal
    <DataMember()> _
    Public Property OrderItemQtyOutstanding() As Decimal
        Get
            Return _orderItemQtyOutstanding
        End Get
        Set(ByVal value As Decimal)
            _orderItemQtyOutstanding = value
        End Set
    End Property

    Private _orderItemUnitCost As Double
    <DataMember()> _
    Public Property OrderItemUnitCost() As Double
        Get
            Return _orderItemUnitCost
        End Get
        Set(ByVal value As Double)
            _orderItemUnitCost = value
        End Set
    End Property

    Private _mode As String
    <DataMember()> _
    Public Property Mode() As String
        Get
            Return _mode
        End Get
        Set(ByVal value As String)
            _mode = value
        End Set
    End Property

End Class
