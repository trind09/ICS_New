Public Class OrderItem

#Region " Constructor "
    Public Sub New()
        Me.New(True)
    End Sub

    ''' <summary>
    ''' Initialise value with blank value, True;
    ''' 05Feb09 - KG;
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
            _mode = ""
        End If
    End Sub

    ''' <summary>
    ''' Initialise value with given value;
    ''' 05Feb09 - KG;
    ''' </summary>
    ''' <param name="OrderItemID"></param>
    ''' <param name="StockItemID"></param>
    ''' <param name="Qty"></param>
    ''' <param name="TotalCost"></param>
    ''' <param name="ExpectedDeliveryDte"></param>
    ''' <param name="WarrantyDte"></param>
    ''' <param name="Remarks"></param>
    ''' <param name="Status"></param>
    ''' <param name="Mode"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Sub New(ByVal orderItemID As Integer _
                   , ByVal stockItemID As String _
                   , ByVal qty As Decimal _
                   , ByVal totalCost As Double _
                   , ByVal expectedDeliveryDte As Date _
                   , ByVal warrantyDte As Date _
                   , ByVal remarks As String _
                   , ByVal status As String _
                   , ByVal mode As String _
            )
        _orderItemID = orderItemID
        _stockItemID = stockItemID
        _qty = qty
        _totalCost = totalCost
        _expectedDeliveryDte = expectedDeliveryDte
        _warrantyDte = warrantyDte
        _remarks = remarks
        _status = status
        _mode = mode
    End Sub
#End Region

    Private _orderItemID As Integer
    Public Property OrderItemID() As Integer
        Get
            Return _orderItemID
        End Get
        Set(ByVal value As Integer)
            _orderItemID = value
        End Set
    End Property

    Private _stockItemID As String
    Public Property StockItemID() As String
        Get
            Return _stockItemID
        End Get
        Set(ByVal value As String)
            _stockItemID = value
        End Set
    End Property

    Private _qty As Decimal
    Public Property Qty() As Decimal
        Get
            Return _qty
        End Get
        Set(ByVal value As Decimal)
            _qty = value
        End Set
    End Property

    Private _totalCost As Double
    Public Property TotalCost() As Double
        Get
            Return _totalCost
        End Get
        Set(ByVal value As Double)
            _totalCost = value
        End Set
    End Property

    Private _expectedDeliveryDte As Date
    Public Property ExpectedDeliveryDte() As Date
        Get
            Return _expectedDeliveryDte
        End Get
        Set(ByVal value As Date)
            _expectedDeliveryDte = value
        End Set
    End Property

    Private _warrantyDte As Date
    Public Property WarrantyDte() As Date
        Get
            Return _warrantyDte
        End Get
        Set(ByVal value As Date)
            _warrantyDte = value
        End Set
    End Property

    Private _remarks As String
    Public Property Remarks() As String
        Get
            Return _remarks
        End Get
        Set(ByVal value As String)
            _remarks = value
        End Set
    End Property

    Private _status As String
    Public Property Status() As String
        Get
            Return _status
        End Get
        Set(ByVal value As String)
            _status = value
        End Set
    End Property

    Private _mode As String
    Public Property Mode() As String
        Get
            Return _mode
        End Get
        Set(ByVal value As String)
            _mode = value
        End Set
    End Property

End Class
