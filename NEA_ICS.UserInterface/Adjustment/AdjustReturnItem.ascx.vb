Imports NEA_ICS.UserInterface.NEA_ICS.WcfService
Imports System.ServiceModel
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports NEA_ICS.UserInterface.clsCommonFunction

''' <summary>
''' User Control for AdjustItem;
''' Dynamically added to the page;
''' 28Feb09 - KG;
''' </summary>
''' <remarks>
''' CHANGE LOG:
''' ddMMMyy  AuthorName  RefID     Description;
''' 21Mar09  KG          UAT02.39  cancel not functioning;
''' </remarks>
Partial Public Class AdjustReturnItem
    Inherits System.Web.UI.UserControl

    Private _message As String = EMPTY
    Public ReadOnly Property Message() As String
        Get
            Return _message
        End Get
    End Property

    ''' <summary>
    ''' Total Cost Changed;
    ''' 28Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Sub txtReceiveQty_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtReceiveQty.TextChanged
        Try
            ' UAT02
            revReceiveQty.Validate()
            If Not revReceiveQty.IsValid Then Throw New ApplicationException(String.Format("[{0}] return total cost of [{1}] is not in the correct format. \nPlease enter a numeric value with 2 decimal places only.", lblStockCode.Text, txtReceiveQty.Text))

            SetMode()

        Catch ex As ApplicationException
            txtReceiveQty.Text = IIf(CDbl(hfOrgTotalCost.Value) > 0.0, CDec(hfOrgTotalCost.Value).ToString("0.00"), EMPTY)
            _message = ex.Message

        Catch ex As Exception
            txtReceiveQty.Text = IIf(CDbl(hfOrgTotalCost.Value) > 0, CDec(hfOrgTotalCost.Value).ToString("0.00"), EMPTY)
            _message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    ''' <summary>
    ''' Return Qty Changed;
    ''' 1)Validate Return inward Qty is within the max level;
    ''' 2)Validate Return outward Qty is within the balance qty;
    ''' 28Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' 21Mar09  KG          UAT02.
    ''' 15Apr09  KG          UAT03  use MaxLevel as the allowQty as it is;
    ''' </remarks>
    Protected Sub txtAdjustQty_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtAdjustQty.TextChanged
        Try
            Dim AdjustQty As Decimal = 0D
            Dim BalanceQty As Decimal = 0   ' outwards return cannot below the balanceQty
            Dim AllowQty As Decimal = 0D    ' inwards return cannot over the allowQty

            ' UAT02
            revAdjustQty.Validate()
            If Not revAdjustQty.IsValid Then Throw New ApplicationException(String.Format("[{0}] return quantity of [{1}] is not in the correct format. \nPlease enter a numeric value with 2 decimal places only.", lblStockCode.Text, txtAdjustQty.Text))

            If IsNumeric(txtAdjustQty.Text) Then AdjustQty = CDec(txtAdjustQty.Text)
            BalanceQty = CDec(hfBalanceQty.Value) - CDec(hfOrgAdjustQty.Value)

            Select Case hfTranType.Value.Substring(0, 2)
                Case ADJUSTIN
                    ' UAT02.46 - find the allow Qty
                    If IsNumeric(hfMaxLevel.Value) Then AllowQty = CDec(hfMaxLevel.Value) - BalanceQty

                    ' when its return type, the maxLevel is assigned with its original issued qty instead
                    If hfTranType.Value = RETURNED Then
                        'BalanceQty = 0D
                        AllowQty = CDec(hfMaxLevel.Value)  'UAT03
                    End If

                    ' UAT02.46 - compare against allowQty
                    ' alert user when return inward excess the Max level allow
                    If AdjustQty > AllowQty Then
                        Throw New ApplicationException(String.Format("[{0}] The Return quantity entered [{1}] excess the allow Max level of [{2}].", lblStockCode.Text, AdjustQty.ToString("0.00"), AllowQty.ToString("0.00")))
                    End If

                Case ADJUSTOUT
                    ' alert user when Return outward excess available balance
                    If AdjustQty > BalanceQty Then
                        Throw New ApplicationException(String.Format("[{0}] The Return quantity entered [{1}] excess the available quantity of [{2}].", lblStockCode.Text, AdjustQty.ToString("0.00"), BalanceQty.ToString("0.00")))
                    End If
            End Select

            SetMode()
        Catch ex As ApplicationException
            txtAdjustQty.Text = IIf(CDec(hfOrgAdjustQty.Value) > 0, CDec(hfOrgAdjustQty.Value).ToString("0.00"), EMPTY)
            _message = ex.Message

        Catch ex As Exception
            txtAdjustQty.Text = IIf(CDec(hfOrgAdjustQty.Value) > 0, CDec(hfOrgAdjustQty.Value).ToString("0.00"), EMPTY)
            _message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    ''' <summary>
    ''' Remarks Changed;
    ''' 28Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Sub txtRemarks_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtRemarks.TextChanged
        SetMode()
    End Sub

    ''' <summary>
    ''' Cancel Item;
    ''' Call the parent to cancel itself;
    ''' 18Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID     Description;
    ''' 21Mar09  KG          UAT02.39  uncomment cancel for inward;
    ''' </remarks>
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        If hfTranType.Value.StartsWith(ADJUSTIN) Then
            Dim BasePage As frmReturnAdjustment = TryCast(Me.Page, frmReturnAdjustment)
            BasePage.CancelAdjustItem(Me.UniqueID, New EventArgs)

        Else
            Dim BasePage As frmODL = TryCast(Me.Page, frmODL)
            BasePage.CancelAdjustItem(Me.UniqueID, New EventArgs)
        End If
    End Sub

    ''' <summary>
    ''' View Stock Details;
    ''' Call the parent display info of this item in Modal window;
    ''' 28Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' 15Apr09  KG          UAT03  adjustDiff x unitcost is the AdjustDiffCost;
    ''' </remarks>
    Protected Sub btnViewDetails_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnViewDetails.Click
        Dim AdjustDiff As Decimal = -CDec(hfOrgAdjustQty.Value)
        Dim AdjustDiffCost As Decimal = -CDec(hfOrgTotalCost.Value)
        Dim UnitCost As Decimal = CDec(hfUnitCost.Value)

        If IsNumeric(txtAdjustQty.Text) Then
            AdjustDiff = CDec(txtAdjustQty.Text) ' get the updated adjust diff.
        Else
            AdjustDiff = 0
        End If

        'If IsNumeric(txtAdjustQty.Text) Then
        '    AdjustDiff += CDec(txtAdjustQty.Text)
        'End If

        '' Apply to Inward Return ONLY
        If hfTranType.Value = RETURNED Then
            AdjustDiffCost = AdjustDiff * CDbl(UnitCost)
        Else
            AdjustDiffCost += CDbl(hfOrgTotalCost.Value)
        End If


        If hfTranType.Value = RETURNED Then 'add for SR 51312448
            Dim BasePage As frmReturnAdjustment = TryCast(Me.Page, frmReturnAdjustment)
            BasePage.ViewStock(UniqueID _
                               , lblStockCode.Text _
                               , lblDescription.Text _
                               , lblUOM.Text _
                               , AdjustDiff _
                               , AdjustDiffCost _
                               )
        Else
            Dim BasePage As frmODL = TryCast(Me.Page, frmODL)
            BasePage.ViewStock(UniqueID _
                               , lblStockCode.Text _
                               , lblDescription.Text _
                               , lblUOM.Text _
                               , AdjustDiff _
                               , AdjustDiffCost _
                               )
        End If
    End Sub

    Private Sub SetMode()
        Dim AdjustQty As Decimal = 0D
        'Dim TotalCost As Double = 0.0
        Dim ActualReceiveQty As Decimal = 0D

        If hfAdjustItemStatus.Value = APPROVED Or hfAdjustItemStatus.Value = PENDING Or hfAdjustItemStatus.Value = CLOSED Then
            If IsNumeric(txtReceiveQty.Text) Then ActualReceiveQty = CDec(txtReceiveQty.Text)

            If checkReceived.Checked = True Then hfMode.Value = UPDATE ' approve is clicked

            'ElseIf ActualReceiveQty = 0D Then

        Else ' edit return
            If IsNumeric(txtAdjustQty.Text) Then AdjustQty = CDec(txtAdjustQty.Text)
            If IsNumeric(txtReceiveQty.Text) Then ActualReceiveQty = CDec(txtReceiveQty.Text)

            If (CDec(hfOrgAdjustQty.Value) <> AdjustQty _
                Or hfOrgRemarks.Value <> Trim(txtRemarks.Text) _
                ) Then
                hfMode.Value = UPDATE
            Else
                hfMode.Value = EMPTY
            End If
        End If
    End Sub

    ''' <summary>
    ''' Page_PreRender;
    ''' display error message if any
    ''' 28Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' 15Apr09  KG          UAT03  use MaxLevel as the allowQty as it is;
    ''' </remarks>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim AdjustQty As Decimal = 0D

        ' Enable/Disable Entries Control
        'txtAdjustQty.Enabled = True
        'txtTotalCost.Enabled = True

        txtAdjustQty.Enabled = False
        txtReceiveQty.Enabled = False
        btnCancel.Visible = False

        'If hfAdjustItemStatus.Value = EMPTY Or hfMode.Value = EMPTY Then
        If hfAdjustItemStatus.Value = EMPTY Or hfTranID.Value <> "0" Or hfTranID.Value <> EMPTY Then
            ' New return
            ' Set access rights for returner to proceed with adjustment
            txtAdjustQty.Enabled = True
            btnCancel.Visible = True

        End If

        'If hfAdjustItemStatus.Value = APPROVED Or hfTranID.Value <> "0" Or hfTranID.Value <> EMPTY Then
        ' Ready for receiving / acknowledgement of the return 
        'If Session(ESession.UserRoleType.ToString).ToString.Contains(STOREOFFICER) Then
        If Session(ESession.UserRoleType.ToString).ToString.Contains(STOREOFFICER) Then
            txtAdjustQty.Enabled = False
            txtReceiveQty.Enabled = True
        End If
        'End If

        'If hfMode.Value = UPDATE Then
        '    txtAdjustQty.Enabled = True ' enable for Edit mode
        'End If

        If Not IsNumeric(hfBalanceQty.Value) Then hfBalanceQty.Value = 0D
        If Not IsNumeric(hfOrgAdjustQty.Value) Then hfOrgAdjustQty.Value = 0
        If Not IsNumeric(txtAdjustQty.Text) Then txtAdjustQty.Text = 0D

        ''lblUnitCost.Text = "0.0000"
        ''If IsNumeric(txtAdjustQty.Text) Then
        ''    If CDec(txtAdjustQty.Text) > 0 And IsNumeric(txtTotalCost.Text) Then
        ''        lblUnitCost.Text = DisplayValue(CDbl(txtTotalCost.Text) / CDec(txtAdjustQty.Text))
        ''    End If
        ''End If


        ' Alert user the available quantity
        If hfTranType.Value.Contains(ADJUSTOUT) Then
            lblInfo.Text = GetMessage(messageID.MoreLessThan, , , "Quantity", CDec(hfBalanceQty.Value) + CDec(hfOrgAdjustQty.Value), "<= (Less Than or Equal)")

            ' Disable Cancel button when the deletion will cause the balance excess the max level
            If (CDec(hfBalanceQty.Value) + CDec(hfOrgAdjustQty.Value)) > CDec(hfMaxLevel.Value) Then
                btnCancel.Enabled = False

            Else
                btnCancel.Enabled = True
            End If

        ElseIf hfTranType.Value.Contains(ADJUSTIN) Then
            ' Display UnitCost label
            If hfTranType.Value = RETURNED Then
                trCancel.Visible = False

                ' can only return qty up to the issue qty
                lblInfo.Text = GetMessage(messageID.MoreLessThan, , , "Quantity", CDec(hfMaxLevel.Value), "<= (Less Than or Equal)")    'UAT03

                ''  truncate value to 2 decimal place for display
                'If IsNumeric(txtAdjustQty.Text) Then AdjustQty = CDec(txtAdjustQty.Text)
                'txtTotalCost.Text = IIf(AdjustQty > 0, Math.Floor(AdjustQty * CDbl(lblUnitCost.Text) * 100) / 100, "0.00")
            End If
        End If

        ' alert user with message (if any)
        If Message <> EMPTY Then
            ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", "ShowAlertMessage('" & Message & "');", True)
        End If
    End Sub

End Class