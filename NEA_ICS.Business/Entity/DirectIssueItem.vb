Imports NEA_ICS.DataAccess

Public Class DirectIssueItem

#Region " Constructor "
    Public Sub New()
        Me.New(True)
    End Sub

    ''' <summary>
    ''' Initialise value with blank value, True;
    ''' 5 Feb 09 - KG;
    ''' </summary>
    ''' <param name="initialise"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Sub New(ByVal initialise As Boolean)
        If initialise Then
            
        End If
    End Sub

    ''' <summary>
    ''' Sub New
    ''' 13 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="itemID"></param>
    ''' <param name="itemDescription"></param>
    ''' <param name="stockType"></param>
    ''' <param name="itemQty"></param>
    ''' <param name="UOM"></param>
    ''' <param name="totalCost"></param>
    ''' <param name="remarks"></param>
    ''' <param name="itemStatus"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal itemID As String, _
                   ByVal itemDescription As String, _
                   ByVal stockType As String, _
                   ByVal itemQty As Decimal, _
                   ByVal UOM As String, _
                   ByVal totalCost As Decimal, _
                   ByVal remarks As String, _
                   ByVal itemStatus As String, _
                   ByVal mode As String)

        Me.ItemID = itemID
        Me.ItemDescription = itemDescription
        Me.StockType = stockType
        Me.ItemQty = itemQty
        Me.UOM = UOM
        Me.TotalCost = totalCost
        Me.Remarks = remarks
        Me.ItemStatus = itemStatus
        Me.Mode = mode

    End Sub
#End Region

    Private _itemID As String
    Public Property ItemID() As String
        Get
            Return _itemID
        End Get
        Set(ByVal value As String)
            _itemID = value
        End Set
    End Property

    Private _itemDescription As String
    Public Property ItemDescription() As String
        Get
            Return _itemDescription
        End Get
        Set(ByVal value As String)
            _itemDescription = value
        End Set
    End Property

    Private _stockType As String
    Public Property StockType() As String
        Get
            Return _stockType
        End Get
        Set(ByVal value As String)
            _stockType = value
        End Set
    End Property

    Private _itemQty As Decimal
    Public Property ItemQty() As Decimal
        Get
            Return _itemQty
        End Get
        Set(ByVal value As Decimal)
            _itemQty = value
        End Set
    End Property

    Private _UOM As String
    Public Property UOM() As String
        Get
            Return _UOM
        End Get
        Set(ByVal value As String)
            _UOM = value
        End Set
    End Property

    Private _totalCost As Decimal
    Public Property TotalCost() As Decimal
        Get
            Return _totalCost
        End Get
        Set(ByVal value As Decimal)
            _totalCost = value
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

    Private _itemStatus As String
    Public Property ItemStatus() As String
        Get
            Return _itemStatus
        End Get
        Set(ByVal value As String)
            _itemStatus = value
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
