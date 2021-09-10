''' <summary>
''' DataContract - for Adjust Details;
''' 19Feb09, KG
''' </summary>
''' <remarks>
''' CHANGE LOG:
''' ddMMMyy  AuthorName  RefID  Description;
''' </remarks>
<DataContract()> _
Public Class AdjustDetails

#Region " Constructor "
    Public Sub New()
        Me.New(True)
    End Sub

    ''' <summary>
    ''' Initialise value with blank value, True;
    ''' transaction record will be created directly, hence status as Closed;
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="initialise"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Sub New(ByVal initialise As Boolean)
        If initialise Then
            _storeID = ""
            _adjustID = ""
            _type = ""
            _dte = Date.MinValue
            _serialNo = ""
            _involveID = ""
            _docReturn = ""
            _status = "C"
            _loginUser = ""
            _returnUserID = ""
            _returnDte = Date.MinValue
            _approveUserID = ""
            _approveDte = Date.MinValue
            _receiveUserId = ""
            _receiveDte = Date.MinValue
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

    Private _adjustID As String
    <DataMember()> _
    Public Property AdjustID() As String
        Get
            Return _adjustID
        End Get
        Set(ByVal value As String)
            _adjustID = value
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

    Private _involveID As String
    <DataMember()> _
    Public Property InvolveID() As String
        Get
            Return _involveID
        End Get
        Set(ByVal value As String)
            _involveID = value
        End Set
    End Property

    Private _docReturn As String
    <DataMember()> _
    Public Property DocReturn() As String
        Get
            Return _docReturn
        End Get
        Set(ByVal value As String)
            _docReturn = value
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

    Private _returnUserID As String
    <DataMember()> _
    Public Property ReturnUserID() As String
        Get
            Return _returnUserID
        End Get
        Set(ByVal value As String)
            _returnUserID = value
        End Set
    End Property

    Private _returnDte As Date
    <DataMember()> _
    Public Property ReturnDte() As Date
        Get
            Return _returnDte
        End Get
        Set(ByVal value As Date)
            _returnDte = value
        End Set
    End Property

    Private _approveUserID As String
    <DataMember()> _
    Public Property ApproveUserID() As String
        Get
            Return _approveUserID
        End Get
        Set(ByVal value As String)
            _approveUserID = value
        End Set
    End Property

    Private _approveDte As Date
    <DataMember()> _
    Public Property ApproveDte() As Date
        Get
            Return _approveDte
        End Get
        Set(ByVal value As Date)
            _approveDte = value
        End Set
    End Property

    Private _receiveUserId As String
    <DataMember()> _
    Public Property ReceiveUserID() As String
        Get
            Return _receiveUserId
        End Get
        Set(ByVal value As String)
            _receiveUserId = value
        End Set
    End Property

    Private _receiveDte As Date
    <DataMember()> _
    Public Property ReceiveDte() As Date
        Get
            Return _receiveDte
        End Get
        Set(ByVal value As Date)
            _receiveDte = value
        End Set
    End Property

End Class
