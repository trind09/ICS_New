Public Class IssueItem

#Region " Constructor "
    Public Sub New()
        Me.New(True)
    End Sub

    ''' <summary>
    ''' Initialise value with blank value, True;
    ''' 20Feb08 - KG;
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
            _remarks = ""
            _itemRef = 0
            _status = ""
            _mode = ""
        End If
    End Sub

    ''' <summary>
    ''' Initialise value with given value;
    ''' 20Feb08 - KG;
    ''' </summary>
    ''' <param name="tranID"></param>
    ''' <param name="stockItemID"></param>
    ''' <param name="qty"></param>
    ''' <param name="remarks"></param>
    ''' <param name="itemRef"></param>
    ''' <param name="status"></param>
    ''' <param name="mode"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Sub New(ByVal tranID As Integer _
                   , ByVal stockItemID As String _
                   , ByVal qty As Decimal _
                   , ByVal remarks As String _
                   , ByVal itemRef As Integer _
                   , ByVal status As String _
                   , ByVal mode As String _
                   )
        _tranID = tranID
        _stockItemID = stockItemID
        _qty = qty
        _remarks = remarks
        _itemRef = itemRef
        _status = status
        _mode = mode
    End Sub

#End Region

    Private _tranID As Integer
    Public Property TranID() As Integer
        Get
            Return _tranID
        End Get
        Set(ByVal value As Integer)
            _tranID = value
        End Set
    End Property

    Private _stockItemID As String
    Public Property StockItemID() As String
        Get
            Return _stockItemID
        End Get
        Set(ByVal value As String)
            _stockItemID = value
        End Set
    End Property

    Private _qty As Decimal
    Public Property Qty() As Decimal
        Get
            Return _qty
        End Get
        Set(ByVal value As Decimal)
            _qty = value
        End Set
    End Property

    Private _remarks As String
    Public Property Remarks() As String
        Get
            Return _remarks
        End Get
        Set(ByVal value As String)
            _remarks = value
        End Set
    End Property

    Private _itemRef As Integer
    Public Property ItemRef() As Integer
        Get
            Return _itemRef
        End Get
        Set(ByVal value As Integer)
            _itemRef = value
        End Set
    End Property

    Private _status As String
    Public Property Status() As String
        Get
            Return _status
        End Get
        Set(ByVal value As String)
            _status = value
        End Set
    End Property

    Private _mode As String
    Public Property Mode() As String
        Get
            Return _mode
        End Get
        Set(ByVal value As String)
            _mode = value
        End Set
    End Property
End Class
