''' <summary>
''' DataContract - for Equipment Details;
''' 29 Dec 08 - Jianfa
''' </summary>
''' <remarks>
''' CHANGE LOG:
''' ddMMMyy  AuthorName  RefID  Description;
''' </remarks> 

<DataContract()> _
Public Class EquipmentDetails

#Region " Constructor "
    Public Sub New()
    End Sub

    ''' <summary>
    ''' Initialise value with blank value, True;
    ''' 29 Dec 08 - Jianfa;
    ''' </summary>
    ''' <param name="initialise"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal initialise As Boolean)
        If initialise Then

            EquipmentID = ""
            EquipmentType = ""
            EquipmentDescription = ""
            Status = ""
            LoginUser = ""

        End If
    End Sub
#End Region

    Private _storeId As String
    <DataMember()> _
    Public Property StoreID() As String
        Get
            Return _storeId
        End Get
        Set(ByVal value As String)
            _storeId = value
        End Set
    End Property

    Private _equipmentId As String
    <DataMember()> _
    Public Property EquipmentID() As String
        Get
            Return _equipmentId
        End Get
        Set(ByVal value As String)
            _equipmentId = value
        End Set
    End Property

    Private _equipmentType As String
    <DataMember()> _
    Public Property EquipmentType() As String
        Get
            Return _equipmentType
        End Get
        Set(ByVal value As String)
            _equipmentType = value
        End Set
    End Property


    Private _equipmentDescription As String
    <DataMember()> _
    Public Property EquipmentDescription() As String
        Get
            Return _equipmentDescription
        End Get
        Set(ByVal value As String)
            _equipmentDescription = value
        End Set
    End Property

    Private _equipmentID_Description As String
    <DataMember()> _
    Public Property EquipmentID_Description() As String
        Get
            Return _equipmentID_Description
        End Get
        Set(ByVal value As String)
            _equipmentID_Description = value
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
