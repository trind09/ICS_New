<DataContract()> _
Public Class MR007PeriodIssuesDetails

    Private _ConsumerID As String
    <DataMember()> _
    Public Property ConsumerID() As String
        Get
            Return _ConsumerID
        End Get
        Set(ByVal value As String)
            _ConsumerID = value
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

    Private _IssueType As String
    <DataMember()> _
    Public Property IssueType() As String
        Get
            Return _IssueType
        End Get
        Set(ByVal value As String)
            _IssueType = value
        End Set
    End Property
    Private _IssueSerialNo As String
    <DataMember()> _
    Public Property IssueSerialNo() As String
        Get
            Return _IssueSerialNo
        End Get
        Set(ByVal value As String)
            _IssueSerialNo = value
        End Set
    End Property

    Private _IssueDte As DateTime
    <DataMember()> _
    Public Property IssueDte() As DateTime
        Get
            Return _IssueDte
        End Get
        Set(ByVal value As DateTime)
            _IssueDte = value
        End Set
    End Property

    Private _IssueQty As Decimal
    <DataMember()> _
    Public Property IssueQty() As Decimal
        Get
            Return _IssueQty
        End Get
        Set(ByVal value As Decimal)
            _IssueQty = value
        End Set
    End Property
    Private _IssueUOM As String
    <DataMember()> _
    Public Property IssueUOM() As String
        Get
            Return _IssueUOM
        End Get
        Set(ByVal value As String)
            _IssueUOM = value
        End Set
    End Property

    Private _IssueUnitCost As Decimal
    <DataMember()> _
    Public Property IssueUnitCost() As Decimal
        Get
            Return _IssueUnitCost
        End Get
        Set(ByVal value As Decimal)
            _IssueUnitCost = value
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
    Private _ConsumerDescription As String
    <DataMember()> _
    Public Property ConsumerDescription() As String
        Get
            Return _ConsumerDescription
        End Get
        Set(ByVal value As String)
            _ConsumerDescription = value
        End Set
    End Property
    Private _RequestID As String
    <DataMember()> _
    Public Property RequestID() As String
        Get
            Return _RequestID
        End Get
        Set(ByVal value As String)
            _RequestID = value
        End Set
    End Property
End Class
