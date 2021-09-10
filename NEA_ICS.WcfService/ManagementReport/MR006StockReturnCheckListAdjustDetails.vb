<DataContract()> _
Public Class MR006StockReturnCheckListAdjustDetails

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

    Private _AdjustID As String
    <DataMember()> _
    Public Property AdjustID() As String
        Get
            Return _AdjustID
        End Get
        Set(ByVal value As String)
            _AdjustID = value
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

    Private _AdjustType As String
    <DataMember()> _
    Public Property AdjustType() As String
        Get
            Return _AdjustType
        End Get
        Set(ByVal value As String)
            _AdjustType = value
        End Set
    End Property

    Private _AdjustDte As DateTime
    <DataMember()> _
    Public Property AdjustDte() As DateTime
        Get
            Return _AdjustDte
        End Get
        Set(ByVal value As DateTime)
            _AdjustDte = value
        End Set
    End Property

    Private _AdjustQty As Decimal
    <DataMember()> _
    Public Property AdjustQty() As Decimal
        Get
            Return _AdjustQty
        End Get
        Set(ByVal value As Decimal)
            _AdjustQty = value
        End Set
    End Property
    Private _AdjustUOM As String
    <DataMember()> _
    Public Property AdjustUOM() As String
        Get
            Return _AdjustUOM
        End Get
        Set(ByVal value As String)
            _AdjustUOM = value
        End Set
    End Property

    Private _AdjustUnitCost As Decimal
    <DataMember()> _
    Public Property AdjustUnitCost() As Decimal
        Get
            Return _AdjustUnitCost
        End Get
        Set(ByVal value As Decimal)
            _AdjustUnitCost = value
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
    Private _AdjustSerialNo As String
    <DataMember()> _
    Public Property AdjustSerialNo() As String
        Get
            Return _AdjustSerialNo
        End Get
        Set(ByVal value As String)
            _AdjustSerialNo = value
        End Set
    End Property
    Private _AdjustRemarks As String
    <DataMember()> _
    Public Property AdjustRemarks() As String
        Get
            Return _AdjustRemarks
        End Get
        Set(ByVal value As String)
            _AdjustRemarks = value
        End Set
    End Property
End Class
