Imports NEA_ICS.UserInterface.NEA_ICS.WcfService
Imports System.ServiceModel
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports NEA_ICS.UserInterface.clsCommonFunction

''' <summary>
''' User Control for IssueItem;
''' Dynamically added to the page;
''' 26Jan09 - KG;
''' </summary>
''' <remarks>
''' CHANGE LOG:
''' ddMMMyy  AuthorName  RefID  Description;
''' </remarks>
Partial Public Class IssueItem
    Inherits System.Web.UI.UserControl

    Private _message As String = EMPTY
    Public ReadOnly Property Message() As String
        Get
            Return _message
        End Get
    End Property

    ''' <summary>
    ''' Issue Qty Changed;
    ''' Validate issue Qty is within the request Qty;
    ''' Validate issue Qty is within the available Balance Qty;
    ''' Only available for Issued; OrgIssueQty is kept as it value in the database which is negative number;
    ''' 18Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' 17Mar09  KG          UAT02  1)check issue within request qty; 2)check format;
    ''' </remarks>
    Protected Sub txtIssueQty_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtIssueQty.TextChanged
        Try
            Dim RequestQty As Decimal = 0D
            Dim IssueQty As Decimal = 0D
            Dim BalanceQty As Decimal = CDec(hfBalanceQty.Value) - CDec(hfOrgIssueQty.Value)

            ' UAT02
            revIssueQty.Validate()
            If Not revIssueQty.IsValid Then Throw New ApplicationException(String.Format("[{0}] issue quantity of [{1}] is not in the correct format. \nPlease enter a numeric value with 2 decimal places only.", lblStockCode.Text, txtIssueQty.Text))

            If IsNumeric(txtIssueQty.Text) Then IssueQty = CDec(txtIssueQty.Text)

            ' UAT02
            If IsNumeric(txtRequestQty.Text) Then RequestQty = CDec(txtRequestQty.Text)
            If IssueQty > RequestQty Then Throw New ApplicationException(String.Format("[{0}] issue quantity of [{1}] excess the request quantity of [{2}].", lblStockCode.Text, IssueQty.ToString("0.00"), RequestQty.ToString("0.00")))

            ' alert user when issue excess available balance
            If IssueQty > BalanceQty Then Throw New ApplicationException(String.Format("[{0}] issue quantity of [{2}] excess the available quantity of [{2}].", lblStockCode.Text, IssueQty.ToString("0.00"), BalanceQty.ToString("0.00")))

        Catch ex As ApplicationException
            txtIssueQty.Text = EMPTY
            _message = ex.Message

        Catch ex As Exception
            txtIssueQty.Text = EMPTY
            _message = GetMessage(messageID.TryLastOperation)
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub

    ''' <summary>
    ''' request Qty Changed;
    ''' UAT02.24 Validate request Qty is within the available Balance Qty;
    ''' 17Mar09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Sub txtRequestQty_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtRequestQty.TextChanged
        Try
            Dim RequestQty As Decimal = 0D
            Dim BalanceQty As Decimal = CDec(hfBalanceQty.Value)

            ' alert requester when wrong format
            revRequestQty.Validate()
            If Not revRequestQty.IsValid Then Throw New ApplicationException(String.Format("[{0}] request quantity of [{1}] is not in the correct format. \nPlease enter a numeric value with 2 decimal places only.", lblStockCode.Text, txtRequestQty.Text))

            If IsNumeric(txtRequestQty.Text) Then RequestQty = CDec(txtRequestQty.Text)

            ' alert requester when request excess available balance
            If RequestQty > BalanceQty Then Throw New ApplicationException(String.Format("[{0}] request quantity of [{1}] excess the available quantity of [{2}].", lblStockCode.Text, RequestQty.ToString("0.00"), BalanceQty.ToString("0.00")))

        Catch ex As ApplicationException
            txtRequestQty.Text = EMPTY
            _message = ex.Message

        Catch ex As Exception
            txtRequestQty.Text = EMPTY
            _message = GetMessage(messageID.TryLastOperation)
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try

    End Sub

    ''' <summary>
    ''' Cancel Item;
    ''' Call the parent to cancel itself;
    ''' Only available for new Request;
    ''' 18Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        Dim BasePage As frmIssueItem = TryCast(Me.Page, frmIssueItem)

        BasePage.CancelIssueItem(Me.UniqueID, New EventArgs)

        '' Add below if estimated Total Request/Issue value is required for display
        'If IsNumeric(Trim(txtTotalCost.Text)) Then
        '    BasePage.UpdateGTotal(-Convert.ToDouble(txtTotalCost.Text))
        'End If
    End Sub

    ''' <summary>
    ''' View Stock Details;
    ''' Call the parent display info of this item in Modal window;
    ''' 18Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Sub btnViewDetails_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnViewDetails.Click
        Try
            Dim BasePage As frmIssueItem = TryCast(Me.Page, frmIssueItem)
            Dim IssueDiff As Decimal = CDec(hfOrgIssueQty.Value)

            If IsNumeric(txtIssueQty.Text) Then
                IssueDiff = CDec(txtIssueQty.Text)
            End If

            ' this UniqueID is used as the parameter to Cache info on this Item, Parent will used for displaying info
            BasePage.ViewStock(UniqueID, lblStockCode.Text, lblDescription.Text, lblUOM.Text, IssueDiff)

        Catch ex As FaultException
            _message = ex.Message

        Catch ex As Exception
            _message = GetMessage(messageID.TryLastOperation)
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    ''' <summary>
    ''' OBOSLETE; replace with GetItemMode in frmIssueItem.aspx;
    ''' SetMode;
    ''' Required only when status is Approved (New Issue) or Closed (Update/Delete Issue)
    ''' 24Feb09 - KG;
    ''' </summary>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Private Sub SetMode()
        Dim IssueQty As Decimal = 0D

        ' required only when status is Approved or Closed
        If hfRequestItemStatus.Value = APPROVED Or hfRequestItemStatus.Value = CLOSED Then
            If IsNumeric(txtIssueQty.Text) Then IssueQty = CDec(txtIssueQty.Text)

            ' TranID = 0 means not issue yet
            ' use the issue qty to determine its mode
            If CInt(hfTranID.Value) = 0 Then
                If IssueQty = 0D Then
                    hfMode.Value = EMPTY

                Else
                    ' Insert only when there is issue qty
                    hfMode.Value = INSERT
                End If

            Else
                ' delete when the issue qty is set to zero
                If IssueQty = 0D Then
                    hfMode.Value = DELETE

                Else
                    ' Update when screen value diff from original value
                    If (CDec(hfOrgIssueQty.Value) <> IssueQty Or hfOrgRemarks.Value <> Trim(txtRemarks.Text)) Then
                        hfMode.Value = UPDATE

                    Else
                        hfMode.Value = EMPTY
                    End If
                End If
            End If
        End If
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
        ' Disable all entry

        Dim AllowedToIssue As Decimal

        txtRequestQty.Enabled = False
        txtIssueQty.Enabled = False
        txtRemarks.Visible = False
        btnCancel.Visible = False

        If hfRequestItemStatus.Value = EMPTY Then
            ' New Request
            '  Set access rights for requester to proceed with request
            txtRequestQty.Enabled = True
            btnCancel.Visible = True

        ElseIf hfRequestItemStatus.Value = APPROVED Or hfTranID.Value <> EMPTY Then
            ' Ready for Issuing or Amendment to past Issued Transaction
            txtIssueQty.Enabled = True
            txtRemarks.Visible = True
        End If

        ' UAT02 - Alert all on available quantity
        If Not IsNumeric(hfBalanceQty.Value) Then hfBalanceQty.Value = 0D
        If Not IsNumeric(hfOrgIssueQty.Value) Then hfOrgIssueQty.Value = 0
        If Not IsNumeric(txtRequestQty.Text) Then txtRequestQty.Text = 0D

        If CDec(hfBalanceQty.Value) - CDec(txtRequestQty.Text) >= 0D Then
            AllowedToIssue = CDec(txtRequestQty.Text)
        Else
            AllowedToIssue = CDec(txtRequestQty.Text) + (CDec(hfBalanceQty.Value) - CDec(txtRequestQty.Text))
        End If

        If hfMode.Value = "I" Then
            AllowedToIssue = CDec(hfBalanceQty.Value)
        End If

        ''-----------------------------------------------------------
        ''-- BUG RESOLUTION - JF 03/09/2009
        ''-----------------------------------------------------------
        If hfRequestItemStatus.Value = CLOSED And hfOrgIssueQty.Value = 0 Then
            txtIssueQty.Text = hfOrgIssueQty.Value
        End If
        ''-----------------------------------------------------------

        lblInfo.Text = GetMessage(messageID.MoreLessThan, , , "Quantity", AllowedToIssue, "<= (Less Than or Equal)")

    End Sub

End Class