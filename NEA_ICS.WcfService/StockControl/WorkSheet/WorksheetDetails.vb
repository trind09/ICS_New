
''' <summary>
''' Data Contract - for Worksheet Details
''' 30 Jan 09 - Jianfa;
''' </summary>
''' <remarks></remarks>
''' 
<DataContract()> _
Public Class WorksheetDetails

#Region " New "
    Public Sub New()
    End Sub
#End Region

    Private _workSheetID As Integer
    <DataMember()> _
    Public Property WorkSheetID() As Integer
        Get
            Return _workSheetID
        End Get
        Set(ByVal value As Integer)
            _workSheetID = value
        End Set
    End Property

    Private _workSheetItemID As String
    <DataMember()> _
    Public Property WorkSheetItemID() As String
        Get
            Return _workSheetItemID
        End Get
        Set(ByVal value As String)
            _workSheetItemID = value
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

    Private _stockCodeFrom As String
    <DataMember()> _
    Public Property StockCodeFrom() As String
        Get
            Return _stockCodeFrom
        End Get
        Set(ByVal value As String)
            _stockCodeFrom = value
        End Set
    End Property

    Private _stockCodeTo As String
    <DataMember()> _
    Public Property StockCodeTo() As String
        Get
            Return _stockCodeTo
        End Get
        Set(ByVal value As String)
            _stockCodeTo = value
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


    Private _stockTypeDescription As String
    <DataMember()> _
    Public Property StockTypeDescription() As String
        Get
            Return _stockTypeDescription
        End Get
        Set(ByVal value As String)
            _stockTypeDescription = value
        End Set
    End Property

    Private _stockQty As Decimal
    <DataMember()> _
    Public Property StockQty() As Decimal
        Get
            Return _stockQty
        End Get
        Set(ByVal value As Decimal)
            _stockQty = value
        End Set
    End Property

    Private _totalValue As Decimal
    <DataMember()> _
    Public Property TotalValue() As Decimal
        Get
            Return _totalValue
        End Get
        Set(ByVal value As Decimal)
            _totalValue = value
        End Set
    End Property

    Private _location As String
    <DataMember()> _
    Public Property Location() As String
        Get
            Return _location
        End Get
        Set(ByVal value As String)
            _location = value
        End Set
    End Property

    Private _location2 As String
    <DataMember()> _
    Public Property Location2() As String
        Get
            Return _location2
        End Get
        Set(ByVal value As String)
            _location2 = value
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

    Private _workSheetStatus As String
    <DataMember()> _
    Public Property WorkSheetStatus() As String
        Get
            Return _workSheetStatus
        End Get
        Set(ByVal value As String)
            _workSheetStatus = value
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

    Private _worksheetGeneratedDate As String
    <DataMember()> _
    Public Property WorksheetGeneratedDate() As String
        Get
            Return _worksheetGeneratedDate
        End Get
        Set(ByVal value As String)
            _worksheetGeneratedDate = value
        End Set
    End Property

    Private _verifierName As String
    <DataMember()> _
    Public Property VerifierName() As String
        Get
            Return _verifierName
        End Get
        Set(ByVal value As String)
            _verifierName = value
        End Set
    End Property

    Private _checkerName As String
    <DataMember()> _
    Public Property CheckerName() As String
        Get
            Return _checkerName
        End Get
        Set(ByVal value As String)
            _checkerName = value
        End Set
    End Property

    Private _approverName As String
    <DataMember()> _
    Public Property ApproverName() As String
        Get
            Return _approverName
        End Get
        Set(ByVal value As String)
            _approverName = value
        End Set
    End Property

    Private _verifyDate As Date
    <DataMember()> _
    Public Property VerifyDate() As Date
        Get
            Return _verifyDate
        End Get
        Set(ByVal value As Date)
            _verifyDate = value
        End Set
    End Property

    Private _checkDate As Date
    <DataMember()> _
    Public Property CheckDate() As Date
        Get
            Return _checkDate
        End Get
        Set(ByVal value As Date)
            _checkDate = value
        End Set
    End Property

    Private _approveDate As Date
    <DataMember()> _
    Public Property ApproveDate() As Date
        Get
            Return _approveDate
        End Get
        Set(ByVal value As Date)
            _approveDate = value
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


End Class
