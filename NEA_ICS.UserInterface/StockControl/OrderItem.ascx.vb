Imports NEA_ICS.UserInterface.NEA_ICS.WcfService
Imports System.ServiceModel
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports NEA_ICS.UserInterface.clsCommonFunction

''' <summary>
''' User Control for OrderItem;
''' Dynamically added to the page;
''' 26Jan09 - KG;
''' </summary>
''' <remarks>
''' CHANGE LOG:
''' ddMMMyy  AuthorName  RefID  Description;
''' 20Mar09  KG          UAT02  1)fix UAT bugs; 2) remove autopostback;
''' </remarks>
Partial Public Class OrderItem
    Inherits System.Web.UI.UserControl

    Private _message As String = EMPTY
    Public ReadOnly Property Message() As String
        Get
            Return _message
        End Get
    End Property

    ''' <summary>
    ''' Total Cost Changed;
    ''' Update the Grand Total as accordingly;
    ''' 05Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Sub txtTotalCost_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtTotalCost.TextChanged
        Try
            Dim TotalCost As Double = 0.0

            ' alert when wrong format
            revTotalCost.Validate()
            If Not revTotalCost.IsValid Then Throw New ApplicationException(String.Format("[{0}] total cost of [{1}] is not in the correct format. \nPlease enter a numeric value with 2 decimal places only.", lblStockCode.Text, txtTotalCost.Text))

            If IsNumeric(txtTotalCost.Text) Then TotalCost = CDbl(txtTotalCost.Text)
            If ViewState(EViewState.TotalCost) Is Nothing Then ViewState(EViewState.TotalCost) = 0.0

            If DirectCast(ViewState(EViewState.TotalCost), Double) <> TotalCost Then
                Dim BasePage As frmOrderItem = TryCast(Me.Page, frmOrderItem)
                BasePage.UpdateGTotal(TotalCost - DirectCast(ViewState(EViewState.TotalCost), Double))
            End If

            If DirectCast(ViewState(EViewState.TotalCost), Double) <> TotalCost Then If sender IsNot Nothing Then SetMode()
            ViewState(EViewState.TotalCost) = TotalCost

        Catch ex As ApplicationException
            ' UAT02.51 reset to the last valid value
            txtTotalCost.Text = CDbl(ViewState(EViewState.TotalCost)).ToString("0.00")
            _message = ex.Message

        Catch ex As Exception
            txtOrderQty.Text = EMPTY
            _message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    ''' <summary>
    ''' order Qty Changed;
    ''' Validate order Qty is within the max level;
    ''' 08Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Sub txtOrderQty_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtOrderQty.TextChanged
        Try
            Dim OrderQty As Decimal = 0D

            ' alert when wrong format
            revOrderQty.Validate()
            If Not revOrderQty.IsValid Then Throw New ApplicationException(String.Format("[{0}] order quantity of [{1}] is not in the correct format. \nPlease enter a numeric value with 2 decimal places only.", lblStockCode.Text, txtOrderQty.Text))

            If IsNumeric(txtOrderQty.Text) Then OrderQty = Convert.ToDecimal(txtOrderQty.Text)
            If ViewState(EViewState.OrderQty) Is Nothing Then ViewState(EViewState.OrderQty) = 0D

            'validate order qty when is changed to a greater number
            If OrderQty > DirectCast(ViewState(EViewState.OrderQty), Decimal) Then
                If hfAllowQty Is Nothing Then
                    Throw New Exception("hfAllowQty")
                End If
            End If

            If OrderQty > CDec(hfAllowQty.Value) Then
                Throw New ApplicationException(String.Format("[{0}] excess your allowed quantity. Please Order within your limits.", txtOrderQty.Text.ToString))
            End If

            If ViewState(EViewState.OrderQty) <> OrderQty Then If sender IsNot Nothing Then SetMode()
            ViewState(EViewState.OrderQty) = OrderQty

        Catch ex As ApplicationException
            txtOrderQty.Text = CDec(ViewState(EViewState.OrderQty)).ToString("0.00")
            _message = ex.Message

        Catch ex As Exception
            txtOrderQty.Text = EMPTY
            _message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    Protected Sub txtExpectedDeliveryDte_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtExpectedDeliveryDte.TextChanged
        Dim ExpDelivery As Date

        ExpDelivery = ConvertToDate(txtExpectedDeliveryDte.Text)
        If ExpDelivery > DateTime.MinValue Then
            If ExpDelivery < Session(ESession.OrderDte.ToString) Then
                _message = GetMessage(messageID.MoreLessThan, , , "Expected Delivery Date", "Order Date", "=> (Equal or Later Than)")
            End If
        Else
            _message = GetMessage(messageID.NotIsDate, "Expected Delivery Date")
        End If

        If Message = EMPTY Then
            SetMode()
        End If
    End Sub

    Protected Sub txtWarrantyDte_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtWarrantyDte.TextChanged
        Dim Warranty As Date

        Warranty = ConvertToDate(txtWarrantyDte.Text)
        If Warranty > DateTime.MinValue Then
            If Warranty < Session(ESession.OrderDte.ToString) Then
                _message = GetMessage(messageID.MoreLessThan, , , "Warranty Date", "Order Date", "=> (Equal or Later Than)")
            End If
        Else
            ' optional field
            If Trim(txtWarrantyDte.Text).Length > 0 Then
                _message = GetMessage(messageID.NotIsDate, "Warranty Date")
            End If
        End If

        If Message = EMPTY Then
            SetMode()
        End If
    End Sub

    Protected Sub txtRemarks_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtRemarks.TextChanged
        SetMode()
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        Dim BasePage As frmOrderItem = TryCast(Me.Page, frmOrderItem)

        If IsNumeric(Trim(txtTotalCost.Text)) Then
            BasePage.UpdateGTotal(-Convert.ToDouble(txtTotalCost.Text))
        End If

        BasePage.CancelOrderItem(Me.UniqueID, New EventArgs)
        ' UAT02.01 remember the mode after cancellation
        ViewState(EViewState.Mode) = hfMode.Value
    End Sub

    Protected Sub btnViewDetails_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnViewDetails.Click
        Dim BasePage As frmOrderItem = TryCast(Me.Page, frmOrderItem)

        BasePage.ViewStock(UniqueID)
    End Sub

    ''' <summary>
    ''' UAT02.01 - Set OrderItem Mode;
    ''' 23Mar09 - KG;
    ''' </summary>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Private Sub SetMode()
        Dim Qty As Decimal = 0D
        Dim TotalCost As Double = 0.0

        If IsNumeric(txtOrderQty.Text) Then Qty = CDec(txtOrderQty.Text)
        If IsNumeric(txtTotalCost.Text) Then TotalCost = CDbl(txtTotalCost.Text)

        ' OrderItemID = 0 means not receive yet
        ' use the qty to determine its mode
        If CInt(hfOrderItemID.Value) = 0 Then
            If Qty = 0D Then
                hfMode.Value = EMPTY
            Else

                ' Insert only when there is receive qty
                hfMode.Value = INSERT
            End If
        Else

            ' Update when screen value diff from original value
            If (CDec(hfOrgOrderQty.Value) <> Qty Or CDec(hfOrgTotalCost.Value) <> TotalCost Or hfOrgEDDte.Value <> txtExpectedDeliveryDte.Text.Trim Or hfOrgWarrantyDte.Value <> txtWarrantyDte.Text.Trim Or hfOrgRemarks.Value <> txtRemarks.Text.Trim) Then
                hfMode.Value = UPDATE
            Else

                hfMode.Value = EMPTY
            End If
        End If

        ViewState(EViewState.Mode) = hfMode.Value
    End Sub

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        hfMode.Value = ViewState(EViewState.Mode)
    End Sub

    ''' <summary>
    ''' Page_PreRender;
    ''' display error message if any
    ''' 24Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ' UAT02.1 this is to initialise the viewstate when it is used by Locate for the 1st time
        If ViewState(EViewState.TotalCost) Is Nothing And IsNumeric(txtTotalCost.Text) Then
            ViewState(EViewState.TotalCost) = CDbl(txtTotalCost.Text)
        End If

        lblUnitCost.Text = "0.0000"
        If IsNumeric(txtOrderQty.Text) Then
            If CDec(txtOrderQty.Text) > 0 And IsNumeric(txtTotalCost.Text) Then

                ''-----------------------------------------------------------
                ''-- BUG RESOLUTION - JF - 13 Aug 2010
                ''-----------------------------------------------------------
                ''=====================================================================
                '' To resolve truncation of 4 decimal places
                ''=====================================================================
                Dim niDec As New System.Globalization.NumberFormatInfo
                niDec.NumberDecimalDigits = 4
                lblUnitCost.Text = ((CDec(txtTotalCost.Text) * 10000) / (CDec(txtOrderQty.Text) * 10000)).ToString("0.0000") 'DisplayValue(CDec(txtTotalCost.Text) / CDec(txtOrderQty.Text))

            End If
        End If

        If hfReceiveQty.Value.Trim = String.Empty Then
            hfReceiveQty.Value = "0.00"
        End If

        ' hide the cancel when there i received already
        If CDec(hfReceiveQty.Value) > 0 Then
            btnCancel.Visible = False
        Else
            btnCancel.Visible = True
        End If

        ' alert user with message (if any)
        If Message <> EMPTY Then
            ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", "ShowAlertMessage('" & Message & "');", True)
        End If
    End Sub

#Region " Not in Used "
    ''''' <summary>
    ''''' Method Exposed to Parent via the use of reflection;
    ''''' Allow access other methods within the control;
    ''''' 11Feb09 - KG;
    ''''' </summary>
    ''''' <param name="methodName"></param>
    ''''' <param name="value"></param>
    ''''' <remarks>
    ''''' CHANGE LOG:
    ''''' ddMMMyy  AuthorName  RefID  Description;
    ''''' </remarks>
    ''Public Sub PublicMethod(ByVal methodName As String, ByVal value As String)
    ''    If methodName = "ddlStockCode_SelectedIndexChanged" Then
    ''        ddlStockCode.SelectedValue = value
    ''        ddlStockCode_SelectedIndexChanged(Nothing, Nothing)
    ''    End If
    ''End Sub

#End Region

End Class