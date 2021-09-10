Imports NEA_ICS.UserInterface.NEA_ICS.WcfService
Imports System.ServiceModel
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports NEA_ICS.UserInterface.clsCommonFunction

''' <summary>
''' User Control for ReceiveItem;
''' Dynamically added to the page;
''' 11Feb09 - KG;
''' </summary>
''' <remarks>
''' CHANGE LOG:
''' ddMMMyy  AuthorName  RefID    Description;
''' 20Mar09  KG          UAT02.58 remove autopostback;
''' </remarks>
Partial Public Class ReceiveItem
    Inherits System.Web.UI.UserControl

    Private _message As String = EMPTY
    Public ReadOnly Property Message() As String
        Get
            Return _message
        End Get
    End Property

    Protected Sub btnViewDetails_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnViewDetails.Click
        Try
            Dim BasePage As frmReceiveItem = TryCast(Me.Page, frmReceiveItem)
            Dim ReceiveDiff As Decimal = -CDec(hfOrgReceiveQty.Value)

            If IsNumeric(txtReceiveQty.Text) Then
                ReceiveDiff += CDec(txtReceiveQty.Text)
            End If

            BasePage.ViewStock(UniqueID, lblStockCode.Text, lblDescription.Text, lblUOM.Text, ReceiveDiff, ReceiveDiff * CDbl(hfUnitCost.Value), CDec(hfQtyOutstanding.Value) - ReceiveDiff)

        Catch ex As Exception
            _message = GetMessage(messageID.TryLastOperation)
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    Protected Sub txtReceiveQty_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtReceiveQty.TextChanged
        Try
            Dim ReceiveQty As Decimal = 0D

            ' alert when wrong format
            revReceiveQty.Validate()
            If Not revReceiveQty.IsValid Then Throw New ApplicationException(String.Format("[{0}] receive quantity of [{1}] is not in the correct format. \nPlease enter a numeric value with 2 decimal places only.", lblStockCode.Text, txtReceiveQty.Text))

            If IsNumeric(txtReceiveQty.Text) Then ReceiveQty = CDec(txtReceiveQty.Text)

            ' alert user on the allowed stock qty to receive
            Dim AllowQty As Decimal = CDec(hfOrgReceiveQty.Value) + CDec(hfQtyOutstanding.Value)
            If ReceiveQty > AllowQty Then Throw New ApplicationException(String.Format("[{0}] receive quantity entered [{1}] excess the allow quantity of [{2}].", lblStockCode.Text, txtReceiveQty.Text, AllowQty.ToString("0.00")))

            SetMode()

        Catch ex As ApplicationException
            txtReceiveQty.Text = CDec(hfOrgReceiveQty.Value).ToString("0.00")
            txtReceiveQty.Focus()
            _message = ex.Message

        Catch ex As Exception
            _message = GetMessage(messageID.TryLastOperation)
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    Protected Sub txtWarrantyDte_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtWarrantyDte.TextChanged
        Dim Warranty As Date

        Warranty = ConvertToDate(txtWarrantyDte.Text)
        If Warranty = DateTime.MinValue Then
            ' optional field
            If Trim(txtWarrantyDte.Text).Length > 0 Then
                _message = GetMessage(messageID.NotIsDate, "Warranty Date")
                txtWarrantyDte.Text = String.Empty
            End If
        End If
    End Sub

    Private Sub SetMode()
        Dim ReceiveQty As Decimal = 0D

        If IsNumeric(txtReceiveQty.Text) Then ReceiveQty = CDec(txtReceiveQty.Text)

        ' TranID = 0 means not receive yet
        ' use the receive qty to determine its mode
        If CInt(hfTranID.Value) = 0 Then
            If ReceiveQty = 0D Then
                hfMode.Value = EMPTY
            Else

                ' Insert only when there is receive qty
                hfMode.Value = INSERT
            End If
        Else

            ' delete when the receive qty is set to zero
            If ReceiveQty = 0D Then
                hfMode.Value = DELETE
            Else

                ' Update when screen value diff from original value
                If (CDec(hfOrgReceiveQty.Value) <> ReceiveQty Or hfOrgRemarks.Value <> Trim(txtRemarks.Text)) Then
                    hfMode.Value = UPDATE
                Else

                    hfMode.Value = EMPTY
                End If
            End If
        End If

        ViewState(EViewState.Mode) = hfMode.Value
    End Sub

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        hfMode.Value = ViewState(EViewState.Mode)
    End Sub

    ''' <summary>
    ''' Page PreRender;
    ''' 1)display order qty left to be received
    ''' 2)compute the total cost for display before place is render;
    ''' 3)alert user for error (if any)
    ''' 4)determine the receive item mode
    ''' 23Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ' alert user on the allowed stock qty to receive
        lblInfo.Text = GetMessage(messageID.MoreLessThan, , , "Receive Quantity", CDec(hfOrgReceiveQty.Value) + CDec(hfQtyOutstanding.Value), "<= (Less Than or Equal)")

        If IsNumeric(txtReceiveQty.Text) Then
            ' handle the rounding value by truncating to 4 decimal place for display
            lblTotalCost.Text = DisplayValue(CDbl(hfUnitCost.Value) * CDec(txtReceiveQty.Text))
        End If

        If Message <> EMPTY Then
            ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", "ShowAlertMessage('" & Message & "');", True)
        End If
    End Sub
End Class