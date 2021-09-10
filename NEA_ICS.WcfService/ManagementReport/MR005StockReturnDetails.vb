<DataContract()> _
Public Class MR005StockReturnDetails

    Private _StockTypeDesc As String
    <DataMember()> _
    Public Property StockTypeDesc() As String
        Get
            Return _StockTypeDesc
        End Get
        Set(ByVal value As String)
            _StockTypeDesc = value
        End Set
    End Property

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

    Private _StartBalanceTotalCost As Decimal
    <DataMember()> _
    Public Property StartBalanceTotalCost() As Decimal
        Get
            Return _StartBalanceTotalCost
        End Get
        Set(ByVal value As Decimal)
            _StartBalanceTotalCost = value
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
    Private _IssueTotalCost As Decimal
    <DataMember()> _
    Public Property IssueTotalCost() As Decimal
        Get
            Return _IssueTotalCost
        End Get
        Set(ByVal value As Decimal)
            _IssueTotalCost = value
        End Set
    End Property
    Private _AdjustTotalCost As Decimal
    <DataMember()> _
    Public Property AdjustTotalCost() As Decimal
        Get
            Return _AdjustTotalCost
        End Get
        Set(ByVal value As Decimal)
            _AdjustTotalCost = value
        End Set
    End Property
    Private _AdjustIssueTotalCost As Decimal
    <DataMember()> _
    Public Property AdjustIssueTotalCost() As Decimal
        Get
            Return _AdjustIssueTotalCost
        End Get
        Set(ByVal value As Decimal)
            _AdjustIssueTotalCost = value
        End Set
    End Property
    Private _EndBalanceTotalCost As Decimal
    <DataMember()> _
    Public Property EndBalanceTotalCost() As Decimal
        Get
            Return _EndBalanceTotalCost
        End Get
        Set(ByVal value As Decimal)
            _EndBalanceTotalCost = value
        End Set
    End Property

End Class
