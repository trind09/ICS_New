''' <summary>
''' DataContract - for Adjust Item Details;
''' 19Feb09, KG
''' </summary>
''' <remarks>
''' CHANGE LOG:
''' ddMMMyy  AuthorName  RefID  Description;
''' </remarks>
<DataContract()> _
Public Class AdjustItemDetails

#Region " Constructor "
    Public Sub New()
        Me.New(True)
    End Sub

    ''' <summary>
    ''' Initialise value with blank value, True;
    ''' transaction record will be created directly, hence status as Closed;
    ''' 19Feb09 - KG;
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
            _adjustItemID = 0
            _itemReturn = 0
            _status = "C"
            _balanceQty = 0D
            _maxLevel = 0D
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

    Private _adjustItemID As Integer
    <DataMember()> _
    Public Property AdjustItemID() As Integer
        Get
            Return _adjustItemID
        End Get
        Set(ByVal value As Integer)
            _adjustItemID = value
        End Set
    End Property

    Private _itemReturn As Integer
    <DataMember()> _
    Public Property ItemReturn() As Integer
        Get
            Return _itemReturn
        End Get
        Set(ByVal value As Integer)
            _itemReturn = value
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

    Private _maxLevel As Decimal
    <DataMember()> _
    Public Property MaxLevel() As Decimal
        Get
            Return _maxLevel
        End Get
        Set(ByVal value As Decimal)
            _maxLevel = value
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
