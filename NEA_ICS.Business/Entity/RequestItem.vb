Public Class RequestItem

#Region " Constructor "
    Public Sub New()
        Me.New(True)
    End Sub

    ''' <summary>
    ''' Initialise value with blank value, True;
    ''' 20Feb09 - KG;
    ''' </summary>
    ''' <param name="initialise"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Sub New(ByVal initialise As Boolean)
        If initialise Then
            _storeID = ""
            _requestID = ""
            _requestItemID = 0
            _stockItemID = ""
            _qty = 0D
            _status = ""
            _loginUser = ""
            _mode = ""
        End If
    End Sub

    ''' <summary>
    ''' Initialise value with given value;
    ''' 20Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="RequestID"></param>
    ''' <param name="RequestItemID"></param>
    ''' <param name="StockItemID"></param>
    ''' <param name="Qty"></param>
    ''' <param name="Status"></param>
    ''' <param name="LoginUser"></param>
    ''' <param name="Mode"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Sub New(ByVal storeID As String _
                   , ByVal requestID As String _
                   , ByVal requestItemID As Integer _
                   , ByVal stockItemID As String _
                   , ByVal qty As Decimal _
                   , ByVal status As String _
                   , ByVal loginUser As String _
                   , ByVal mode As String _
            )
        Me.StoreID = storeID
        Me.RequestID = requestID
        Me.RequestItemID = requestItemID
        Me.StockItemID = stockItemID
        Me.Qty = qty
        Me.Status = status
        Me.LoginUser = loginUser
        Me.Mode = mode
    End Sub
#End Region

    Private _storeID As String
    Public Property StoreID() As String
        Get
            Return _storeID
        End Get
        Set(ByVal value As String)
            _storeID = value
        End Set
    End Property

    Private _requestID As String
    Public Property RequestID() As String
        Get
            Return _requestID
        End Get
        Set(ByVal value As String)
            _requestID = value
        End Set
    End Property

    Private _requestItemID As Integer
    Public Property RequestItemID() As Integer
        Get
            Return _requestItemID
        End Get
        Set(ByVal value As Integer)
            _requestItemID = value
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

    Private _status As String
    Public Property Status() As String
        Get
            Return _status
        End Get
        Set(ByVal value As String)
            _status = value
        End Set
    End Property

    Private _loginUser As String
    Public Property LoginUser() As String
        Get
            Return _loginUser
        End Get
        Set(ByVal value As String)
            _loginUser = value
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
