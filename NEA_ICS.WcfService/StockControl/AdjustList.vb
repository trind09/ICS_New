''' <summary>
''' DataContract - for Adjust List;
''' 22Feb09, KG
''' </summary>
''' <remarks>
''' CHANGE LOG:
''' ddMMMyy  AuthorName  RefID  Description;
''' </remarks>
<DataContract()> _
Public Class AdjustList

    Private _stockType As String
    <DataMember()> _
    Public Property StockType() As String
        Get
            Return _stockType
        End Get
        Set(ByVal value As String)
            _stockType = value
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

    Private _uom As String
    <DataMember()> _
    Public Property UOM() As String
        Get
            Return _uom
        End Get
        Set(ByVal value As String)
            _uom = value
        End Set
    End Property

    Private _docNo As String
    <DataMember()> _
    Public Property DocNo() As String
        Get
            Return _docNo
        End Get
        Set(ByVal value As String)
            _docNo = value
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

    Private _adjustDte As Date
    <DataMember()> _
    Public Property AdjustDte() As Date
        Get
            Return _adjustDte
        End Get
        Set(ByVal value As Date)
            _adjustDte = value
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

    Private _userName As String
    <DataMember()> _
    Public Property UserName() As String
        Get
            Return _userName
        End Get
        Set(ByVal value As String)
            _userName = value
        End Set
    End Property
End Class
