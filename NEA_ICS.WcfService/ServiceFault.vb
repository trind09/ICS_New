''' <summary>
''' DataContract - ServiceFault for Exception Handling;
''' 21 Dec 08 - KG;
''' </summary>
''' <remarks>
''' CHANGE LOG:
''' ddMMMyy  AuthorName  RefID  Description;
''' </remarks>
<DataContract()> _
Public Class ServiceFault
    Public Sub New()
    End Sub

    Private m_id As Guid
    Private message As String
    Private m_data As IDictionary

    <DataMember()> _
    Public Property MessageText() As String
        Get
            Return message
        End Get
        Set(ByVal value As String)
            message = value
        End Set
    End Property

    <DataMember()> _
    Public Property Data() As IDictionary
        Get
            Return m_data
        End Get
        Set(ByVal value As IDictionary)
            m_data = value
        End Set
    End Property

    <DataMember()> _
    Public Property Id() As Guid
        Get
            Return m_id
        End Get
        Set(ByVal value As Guid)
            m_id = value
        End Set
    End Property
End Class