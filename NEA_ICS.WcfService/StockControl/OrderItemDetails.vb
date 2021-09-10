''' <summary>
''' DataContract - for Order Item Details;
''' 18Dec08 - KG;
''' </summary>
''' <remarks>
''' CHANGE LOG:
''' ddMMMyy  AuthorName  RefID  Description;
''' </remarks>
<DataContract()> _
Public Class OrderItemDetails

#Region " Constructor "
    Public Sub New()
        Me.New(True)
    End Sub

    ''' <summary>
    ''' Initialise value with blank value, True;
    ''' 18Dec08 - KG;
    ''' </summary>
    ''' <param name="initialise"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Sub New(ByVal initialise As Boolean)
        If initialise Then
            _orderItemID = 0
            _stockItemID = ""
            _qty = 0D
            _totalCost = 0.0
            _expectedDeliveryDte = DateTime.MinValue
            _warrantyDte = DateTime.MinValue
            _remarks = ""
            _status = ""
            _receiveItemQtyReceived = 0D
            _mode = ""
        End If
    End Sub
#End Region

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

    Private _expectedDeliveryDte As Date
    <DataMember()> _
    Public Property ExpectedDeliveryDte() As Date
        Get
            Return _expectedDeliveryDte
        End Get
        Set(ByVal value As Date)
            _expectedDeliveryDte = value
        End Set
    End Property

    Private _warrantyDte As Date
    <DataMember()> _
    Public Property WarrantyDte() As Date
        Get
            Return _warrantyDte
        End Get
        Set(ByVal value As Date)
            _warrantyDte = value
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

    Private _receiveItemQtyReceived As Decimal
    <DataMember()> _
    Public Property ReceiveItemQtyReceived() As Decimal
        Get
            Return _receiveItemQtyReceived
        End Get
        Set(ByVal value As Decimal)
            _receiveItemQtyReceived = value
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
