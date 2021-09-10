''' <summary>
''' DataContract - for Issue Details;
''' 11Feb09 - KG;
''' </summary>
''' <remarks>
''' CHANGE LOG:
''' ddMMMyy  AuthorName  RefID  Description;
''' </remarks>
<DataContract()> _
Public Class IssueItemDetails

#Region " Constructor "
    Public Sub New()
        Me.New(True)
    End Sub

    ''' <summary>
    ''' Initialise value with blank value, True;
    ''' 11Feb09 - KG;
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
            _requestItemID = 0
            _status = ""
            _requestItemQty = 0.0
            _requestItemStatus = ""
            _balanceQty = 0D
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

    Private _requestItemID As Integer
    <DataMember()> _
    Public Property RequestItemID() As Integer
        Get
            Return _requestItemID
        End Get
        Set(ByVal value As Integer)
            _requestItemID = value
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

    Private _requestItemQty As Decimal
    <DataMember()> _
    Public Property RequestItemQty() As Decimal
        Get
            Return _requestItemQty
        End Get
        Set(ByVal value As Decimal)
            _requestItemQty = value
        End Set
    End Property

    Private _requestItemStatus As String
    <DataMember()> _
    Public Property RequestItemStatus() As String
        Get
            Return _requestItemStatus
        End Get
        Set(ByVal value As String)
            _requestItemStatus = value
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
