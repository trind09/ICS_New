
''' <summary>
''' Data Contract - for Role Details;
''' 13 Jan 09 - Jianfa; 
''' </summary>
''' <remarks></remarks>
<DataContract()> _
Public Class RoleDetails

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

    Private _userRole As String
    <DataMember()> _
    Public Property UserRole() As String
        Get
            Return _userRole
        End Get
        Set(ByVal value As String)
            _userRole = value
        End Set
    End Property

    Private _moduleID As Integer
    <DataMember()> _
    Public Property ModuleID() As Integer
        Get
            Return _moduleID
        End Get
        Set(ByVal value As Integer)
            _moduleID = value
        End Set
    End Property

    Private _parentID As Integer
    <DataMember()> _
    Public Property ParentID() As Integer
        Get
            Return _parentID
        End Get
        Set(ByVal value As Integer)
            _parentID = value
        End Set
    End Property

    Private _masterID As Integer
    <DataMember()> _
    Public Property MasterID() As Integer
        Get
            Return _masterID
        End Get
        Set(ByVal value As Integer)
            _masterID = value
        End Set
    End Property

    Private _moduleTitle As String
    <DataMember()> _
    Public Property ModuleTitle() As String
        Get
            Return _moduleTitle
        End Get
        Set(ByVal value As String)
            _moduleTitle = value
        End Set
    End Property

    Private _moduleSource As String
    <DataMember()> _
    Public Property ModuleSource() As String
        Get
            Return _moduleSource
        End Get
        Set(ByVal value As String)
            _moduleSource = value
        End Set
    End Property

    Private _moduleType As String
    <DataMember()> _
    Public Property ModuleType() As String
        Get
            Return _moduleType
        End Get
        Set(ByVal value As String)
            _moduleType = value
        End Set
    End Property

    Private _insertRight As Boolean
    <DataMember()> _
    Public Property InsertRight() As Boolean
        Get
            Return _insertRight
        End Get
        Set(ByVal value As Boolean)
            _insertRight = value
        End Set
    End Property

    Private _updateRight As Boolean
    <DataMember()> _
    Public Property UpdateRight() As Boolean
        Get
            Return _updateRight
        End Get
        Set(ByVal value As Boolean)
            _updateRight = value
        End Set
    End Property

    Private _deleteRight As Boolean
    <DataMember()> _
    Public Property DeleteRight() As Boolean
        Get
            Return _deleteRight
        End Get
        Set(ByVal value As Boolean)
            _deleteRight = value
        End Set
    End Property

    Private _selectRight As Boolean
    <DataMember()> _
    Public Property SelectRight() As Boolean
        Get
            Return _selectRight
        End Get
        Set(ByVal value As Boolean)
            _selectRight = value
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

    Private _userID As String
    <DataMember()>
    Public Property UserID() As String
        Get
            Return _userID
        End Get
        Set(ByVal value As String)
            _userID = value
        End Set
    End Property

    Private _soeID As String
    <DataMember()>
    Public Property SoeID() As String
        Get
            Return _soeID
        End Get
        Set(ByVal value As String)
            _soeID = value
        End Set
    End Property

    Private _name As String
    <DataMember()> _
    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property

    Private _designation As String
    <DataMember()> _
    Public Property Designation() As String
        Get
            Return _designation
        End Get
        Set(ByVal value As String)
            _designation = value
        End Set
    End Property

    Private _division As String
    <DataMember()> _
    Public Property Division() As String
        Get
            Return _division
        End Get
        Set(ByVal value As String)
            _division = value
        End Set
    End Property

    Private _department As String
    <DataMember()> _
    Public Property Department() As String
        Get
            Return _department
        End Get
        Set(ByVal value As String)
            _department = value
        End Set
    End Property

    Private _installation As String
    <DataMember()> _
    Public Property Installation() As String
        Get
            Return _installation
        End Get
        Set(ByVal value As String)
            _installation = value
        End Set
    End Property

    Private _section As String
    <DataMember()> _
    Public Property Section() As String
        Get
            Return _section
        End Get
        Set(ByVal value As String)
            _section = value
        End Set
    End Property

    Private _userStatus As String
    <DataMember()> _
    Public Property UserStatus() As String
        Get
            Return _userStatus
        End Get
        Set(ByVal value As String)
            _userStatus = value
        End Set
    End Property

    Private _isUserDeleted As Boolean
    <DataMember()> _
    Public Property IsUserDeleted() As Boolean
        Get
            Return _isUserDeleted
        End Get
        Set(ByVal value As Boolean)
            _isUserDeleted = value
        End Set
    End Property

    Private _userIP As String
    <DataMember()> _
    Public Property UserIP() As String
        Get
            Return _userIP
        End Get
        Set(ByVal value As String)
            _userIP = value
        End Set
    End Property

    Private _lastLogin As DateTime
    <DataMember()> _
    Public Property LastLogin() As DateTime
        Get
            Return _lastLogin
        End Get
        Set(ByVal value As DateTime)
            _lastLogin = value
        End Set
    End Property

    Private _lastLoginOut As DateTime
    <DataMember()> _
    Public Property LastLogout() As DateTime
        Get
            Return _lastLoginOut
        End Get
        Set(ByVal value As DateTime)
            _lastLoginOut = value
        End Set
    End Property

    Private _createdDate As DateTime
    <DataMember()> _
    Public Property CreatedDate() As DateTime
        Get
            Return _createdDate
        End Get
        Set(ByVal value As DateTime)
            _createdDate = value
        End Set
    End Property

    Private _createdBy As String
    <DataMember()> _
    Public Property CreatedBy() As String
        Get
            Return _createdBy
        End Get
        Set(ByVal value As String)
            _createdBy = value
        End Set
    End Property

    Private _changeStatusReason As String
    <DataMember()> _
    Public Property ChangeStatusReason() As String
        Get
            Return _changeStatusReason
        End Get
        Set(ByVal value As String)
            _changeStatusReason = value
        End Set
    End Property

    Private _updatedDate As DateTime
    <DataMember()> _
    Public Property UpdatedDate() As DateTime
        Get
            Return _updatedDate
        End Get
        Set(ByVal value As DateTime)
            _updatedDate = value
        End Set
    End Property
End Class
