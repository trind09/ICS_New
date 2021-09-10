
''' <summary>
''' Data Contract for DirectIssue;
''' 12 Feb 09 - Jianfa;
''' </summary>
''' <remarks></remarks>
<DataContract()> _
Public Class DirectIssueDetails

    Private _directIssueID As Integer
    <DataMember()> _
    Public Property DirectIssueID() As Integer
        Get
            Return _directIssueID
        End Get
        Set(ByVal value As Integer)
            _directIssueID = value
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

    Private _consumerID As String
    <DataMember()> _
    Public Property ConsumerID() As String
        Get
            Return _consumerID
        End Get
        Set(ByVal value As String)
            _consumerID = value
        End Set
    End Property


    Private _issueType As String
    <DataMember()> _
    Public Property IssueType() As String
        Get
            Return _issueType
        End Get
        Set(ByVal value As String)
            _issueType = value
        End Set
    End Property

    Private _serialNo As String
    <DataMember()> _
    Public Property SerialNo() As String
        Get
            Return _serialNo
        End Get
        Set(ByVal value As String)
            _serialNo = value
        End Set
    End Property

    Private _documentNo As String
    <DataMember()> _
    Public Property DocumentNo() As String
        Get
            Return _documentNo
        End Get
        Set(ByVal value As String)
            _documentNo = value
        End Set
    End Property

    Private _directIssueDate As Date
    <DataMember()> _
    Public Property DirectIssueDate() As Date
        Get
            Return _directIssueDate
        End Get
        Set(ByVal value As Date)
            _directIssueDate = value
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

    Private _itemQty As Decimal
    <DataMember()> _
    Public Property ItemQty() As Decimal
        Get
            Return _itemQty
        End Get
        Set(ByVal value As Decimal)
            _itemQty = value
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

    Private _totalCost As Decimal
    <DataMember()> _
    Public Property TotalCost() As Decimal
        Get
            Return _totalCost
        End Get
        Set(ByVal value As Decimal)
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
