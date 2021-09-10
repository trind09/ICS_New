''' <summary>
''' DataContract - for Order Details;
''' 05Feb09 - KG;
''' </summary>
''' <remarks>
''' CHANGE LOG:
''' ddMMMyy  AuthorName  RefID  Description;
''' </remarks>
<DataContract()> _
Public Class OrderDetails

#Region " Constructor "
    Public Sub New()
        Me.New(True)
    End Sub

    ''' <summary>
    ''' Initialise value with blank value, True;
    ''' 05Feb09 - KG;
    ''' </summary>
    ''' <param name="initialise"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Sub New(ByVal initialise As Boolean)
        If initialise Then
            _storeID = ""
            _orderID = ""
            _gebizPONo = ""
            _type = ""
            _dte = DateTime.MinValue
            _supplierID = ""
            _status = ""
            _loginUser = ""
            _mode = ""
        End If
    End Sub
#End Region

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

    Private _orderID As String
    <DataMember()> _
    Public Property OrderID() As String
        Get
            Return _orderID
        End Get
        Set(ByVal value As String)
            _orderID = value
        End Set
    End Property

    Private _gebizPONo As String
    <DataMember()> _
    Public Property GebizPONo() As String
        Get
            Return _gebizPONo
        End Get
        Set(ByVal value As String)
            _gebizPONo = value
        End Set
    End Property

    Private _type As String
    <DataMember()> _
    Public Property Type() As String
        Get
            Return _type
        End Get
        Set(ByVal value As String)
            _type = value
        End Set
    End Property

    Private _dte As Date
    <DataMember()> _
    Public Property Dte() As Date
        Get
            Return _dte
        End Get
        Set(ByVal value As Date)
            _dte = value
        End Set
    End Property

    Private _supplierID As String
    <DataMember()> _
    Public Property SupplierID() As String
        Get
            Return _supplierID
        End Get
        Set(ByVal value As String)
            _supplierID = value
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
