''' <summary>
''' DataContract - for Issue List;
''' 22Feb09 - KG;
''' </summary>
''' <remarks>
''' CHANGE LOG:
''' ddMMMyy  AuthorName  RefID  Description;
''' </remarks>
<DataContract()> _
Public Class IssueList

    Private _docNo As String
    <DataMember()> _
    Public Property DocNo() As String
        Get
            Return _docNo
        End Get
        Set(ByVal value As String)
            _docNo = value
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

    Private _requestBy As String
    <DataMember()> _
    Public Property RequestBy() As String
        Get
            Return _requestBy
        End Get
        Set(ByVal value As String)
            _requestBy = value
        End Set
    End Property

    Private _issueDte As Date
    <DataMember()> _
    Public Property IssueDte() As Date
        Get
            Return _issueDte
        End Get
        Set(ByVal value As Date)
            _issueDte = value
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

    Private _issueItemID As Integer
    <DataMember()> _
    Public Property IssueItemID() As Integer
        Get
            Return _issueItemID
        End Get
        Set(ByVal value As Integer)
            _issueItemID = value
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

    Private _description As String
    <DataMember()> _
    Public Property Description() As String
        Get
            Return _description
        End Get
        Set(ByVal value As String)
            _description = value
        End Set
    End Property

    Private _uom As String
    <DataMember()> _
    Public Property UOM() As String
        Get
            Return _uom
        End Get
        Set(ByVal value As String)
            _uom = value
        End Set
    End Property

    Private _issueQty As Decimal
    <DataMember()> _
    Public Property IssueQty() As Decimal
        Get
            Return _issueQty
        End Get
        Set(ByVal value As Decimal)
            _issueQty = value
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

    Private _itemStatus As String
    <DataMember()> _
    Public Property ItemStatus() As String
        Get
            Return _itemStatus
        End Get
        Set(ByVal value As String)
            _itemStatus = value
        End Set
    End Property
End Class
