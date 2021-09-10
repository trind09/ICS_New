''' <summary>
''' DataContract - for Request Details;
''' 19Feb09 - KG;
''' </summary>
''' <remarks>
''' CHANGE LOG:
''' ddMMMyy  AuthorName  RefID  Description;
''' </remarks>
<DataContract()> _
Public Class RequestDetails

#Region " Constructor "
    Public Sub New()
        Me.New(True)
    End Sub

    ''' <summary>
    ''' Initialise value with blank value, True;
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
            _consumerID = ""
            _requestID = ""
            _type = ""
            _serialNo = ""
            _sought = False
            _requestDte = DateTime.MinValue
            _requestBy = ""
            _approveDte = DateTime.MinValue
            _approveBy = ""
            _issueDte = DateTime.MinValue
            _issueBy = ""
            _status = "O"
            _loginUser = ""
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

    Private _requestID As String
    <DataMember()> _
    Public Property RequestID() As String
        Get
            Return _requestID
        End Get
        Set(ByVal value As String)
            _requestID = value
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

    Private _sought As Boolean
    <DataMember()> _
    Public Property Sought() As Boolean
        Get
            Return _sought
        End Get
        Set(ByVal value As Boolean)
            _sought = value
        End Set
    End Property

    Private _requestDte As Date
    <DataMember()> _
    Public Property RequestDte() As Date
        Get
            Return _requestDte
        End Get
        Set(ByVal value As Date)
            _requestDte = value
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

    Private _approveBy As String
    <DataMember()> _
    Public Property ApproveBy() As String
        Get
            Return _approveBy
        End Get
        Set(ByVal value As String)
            _approveBy = value
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

    Private _issueBy As String
    <DataMember()> _
    Public Property IssueBy() As String
        Get
            Return _issueBy
        End Get
        Set(ByVal value As String)
            _issueBy = value
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

End Class
