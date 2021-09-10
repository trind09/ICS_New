''' <summary>
''' DataContract - for Item Details;
''' 07 Jan 09 - Jianfa
''' </summary>
''' <remarks>
''' CHANGE LOG:
''' ddMMMyy  AuthorName  RefID  Description;
''' </remarks> 
''' 
<DataContract()> _
Public Class ItemDetails

    Public Sub New()
    End Sub

    Private _storeID As String
    <DataMember()> _
    Public Property StoreID() As String
        Get
            Return _storeID
        End Get
        Set(ByVal value As String)
            _storeID = value
        End Set
    End Property

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

    Private _equipmentID As String
    <DataMember()> _
    Public Property EquipmentID() As String
        Get
            Return _equipmentID
        End Get
        Set(ByVal value As String)
            _equipmentID = value
        End Set
    End Property

    Private _itemDescription As String
    <DataMember()> _
    Public Property ItemDescription() As String
        Get
            Return _itemDescription
        End Get
        Set(ByVal value As String)
            _itemDescription = value
        End Set
    End Property

    Private _itemID_Description As String
    <DataMember()> _
    Public Property ItemID_Description() As String
        Get
            Return _itemID_Description
        End Get
        Set(ByVal value As String)
            _itemID_Description = value
        End Set
    End Property


    Private _partNo As String
    <DataMember()> _
    Public Property PartNo() As String
        Get
            Return _partNo
        End Get
        Set(ByVal value As String)
            _partNo = value
        End Set
    End Property

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

    Private _subType As String
    <DataMember()> _
    Public Property SubType() As String
        Get
            Return _subType
        End Get
        Set(ByVal value As String)
            _subType = value
        End Set
    End Property

    Private _UOM As String
    <DataMember()> _
    Public Property UOM() As String
        Get
            Return _UOM
        End Get
        Set(ByVal value As String)
            _UOM = value
        End Set
    End Property

    Private _Location As String
    <DataMember()> _
    Public Property Location() As String
        Get
            Return _Location
        End Get
        Set(ByVal value As String)
            _Location = value
        End Set
    End Property

    Private _Location2 As String
    <DataMember()> _
    Public Property Location2() As String
        Get
            Return _Location2
        End Get
        Set(ByVal value As String)
            _Location2 = value
        End Set
    End Property

    Private _minLevel As Decimal
    <DataMember()> _
    Public Property MinLevel() As Decimal
        Get
            Return _minLevel
        End Get
        Set(ByVal value As Decimal)
            _minLevel = value
        End Set
    End Property

    Private _reorderLevel As Decimal
    <DataMember()> _
    Public Property ReorderLevel() As Decimal
        Get
            Return _reorderLevel
        End Get
        Set(ByVal value As Decimal)
            _reorderLevel = value
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

    Private _Status As String
    <DataMember()> _
    Public Property Status() As String
        Get
            Return _Status
        End Get
        Set(ByVal value As String)
            _Status = value
        End Set
    End Property

    Private _openingBalance As Decimal
    <DataMember()> _
    Public Property OpeningBalance() As Decimal
        Get
            Return _openingBalance
        End Get
        Set(ByVal value As Decimal)
            _openingBalance = value
        End Set
    End Property

    Private _openingTotalValue As Decimal
    <DataMember()> _
    Public Property OpeningTotalValue() As Decimal
        Get
            Return _openingTotalValue
        End Get
        Set(ByVal value As Decimal)
            _openingTotalValue = value
        End Set
    End Property

    Private _AUCost As Decimal
    <DataMember()> _
    Public Property AUCost() As Decimal
        Get
            Return _AUCost
        End Get
        Set(ByVal value As Decimal)
            _AUCost = value
        End Set
    End Property


    Private _transactionDate As Date
    <DataMember()> _
    Public Property TransactionDate() As Date
        Get
            Return _transactionDate
        End Get
        Set(ByVal value As Date)
            _transactionDate = value
        End Set
    End Property


    Private _loginUser As String
    <DataMember()> _
    Public Property LoginUser() As String
        Get
            Return _loginUser
        End Get
        Set(ByVal value As String)
            _loginUser = value
        End Set
    End Property

End Class
