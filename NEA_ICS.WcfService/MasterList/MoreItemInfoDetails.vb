<DataContract()> _
Public Class MoreItemInfoDetails
    ''' <summary>
    ''' DataContract - for More Item info Details;
    ''' 05Feb09 - KG;
    ''' </summary>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Sub New()
    End Sub

    Public Sub New(ByVal itemID As String _
                   , ByVal maxLevel As Decimal _
                   , ByVal balance As Decimal _
                   , ByVal avgUnitCost As Single _
                   , ByVal totalValue As Single _
                   , ByVal onOrderQty As Decimal _
                   , ByVal onOrderCount As Integer _
                   )
        _itemID = itemID
        _maxLevel = maxLevel
        _balance = balance
        _avgUnitCost = avgUnitCost
        _totalValue = totalValue
        _onOrderQty = onOrderQty
        _onOrderCount = onOrderCount
    End Sub

    Private _itemID As String
    <DataMember()> _
    Public Property ItemID() As String
        Get
            Return _itemID
        End Get
        Set(ByVal value As String)
            _itemID = value
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

    Private _balance As Decimal
    <DataMember()> _
    Public Property Balance() As Decimal
        Get
            Return _balance
        End Get
        Set(ByVal value As Decimal)
            _balance = value
        End Set
    End Property

    Private _avgUnitCost As Single
    <DataMember()> _
    Public Property AvgUnitCost() As Single
        Get
            Return _avgUnitCost
        End Get
        Set(ByVal value As Single)
            _avgUnitCost = value
        End Set
    End Property

    Private _totalValue As Single
    <DataMember()> _
    Public Property TotalValue() As Single
        Get
            Return _totalValue
        End Get
        Set(ByVal value As Single)
            _totalValue = value
        End Set
    End Property

    Private _onOrderQty As Decimal
    <DataMember()> _
    Public Property OnOrderQty() As Decimal
        Get
            Return _onOrderQty
        End Get
        Set(ByVal value As Decimal)
            _onOrderQty = value
        End Set
    End Property

    Private _onOrderCount As Integer
    <DataMember()> _
    Public Property OnOrderCount() As Integer
        Get
            Return _onOrderCount
        End Get
        Set(ByVal value As Integer)
            _onOrderCount = value
        End Set
    End Property

End Class
