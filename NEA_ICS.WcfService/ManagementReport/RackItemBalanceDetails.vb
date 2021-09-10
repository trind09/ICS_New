''' <summary>
''' DataContract - for MR001 Rack Item Balance;
''' 29 Dec 08 - Liu Guo Feng;
''' </summary>
<DataContract()> _
Public Class RackItemBalanceDetails

    Private _stockBalance As Decimal
    <DataMember()> _
    Public Property StockBalance() As Decimal
        Get
            Return _stockBalance
        End Get
        Set(ByVal value As Decimal)
            _stockBalance = value
        End Set
    End Property

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

    Private _stockID As String
    <DataMember()> _
    Public Property StockID() As String
        Get
            Return _stockID
        End Get
        Set(ByVal value As String)
            _stockID = value
        End Set
    End Property

    Private _description As String
    <DataMember()> _
    Public Property Description() As String
        Get
            Return _description
        End Get
        Set(ByVal value As String)
            _description = value
        End Set
    End Property

    Private _UOM As String
    <DataMember()> _
    Public Property UOM() As String
        Get
            Return _UOM
        End Get
        Set(ByVal value As String)
            _UOM = value
        End Set
    End Property

    Private _location As String
    <DataMember()> _
    Public Property Location() As String
        Get
            Return _location
        End Get
        Set(ByVal value As String)
            _location = value
        End Set
    End Property

    Private _location2 As String
    <DataMember()> _
    Public Property Location2() As String
        Get
            Return _location2
        End Get
        Set(ByVal value As String)
            _location2 = value
        End Set
    End Property

End Class
