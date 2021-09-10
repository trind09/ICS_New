<DataContract()> _
Public Class MR003IssueDocumentDetails

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
    Private _IssueID As String
    <DataMember()> _
    Public Property IssueID() As String
        Get
            Return _IssueID
        End Get
        Set(ByVal value As String)
            _IssueID = value
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
    Private _IssueConsumerID As String
    <DataMember()> _
    Public Property IssueConsumerID() As String
        Get
            Return _IssueConsumerID
        End Get
        Set(ByVal value As String)
            _IssueConsumerID = value
        End Set
    End Property
    Private _IssueStockItemID As String
    <DataMember()> _
    Public Property IssueStockItemID() As String
        Get
            Return _IssueStockItemID
        End Get
        Set(ByVal value As String)
            _IssueStockItemID = value
        End Set
    End Property
    Private _IssueStockItemDesc As String
    <DataMember()> _
    Public Property IssueStockItemDesc() As String
        Get
            Return _IssueStockItemDesc
        End Get
        Set(ByVal value As String)
            _IssueStockItemDesc = value
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

End Class
