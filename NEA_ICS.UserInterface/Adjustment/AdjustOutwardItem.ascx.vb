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
Partial Public Class AdjustOutwardItem
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
            revTotalCost.Validate()
            If Not revTotalCost.IsValid Then Throw New ApplicationException(String.Format("[{0}] adjust total cost of [{1}] is not in the correct format. \nPlease enter a numeric value with 2 decimal places only.", lblStockCode.Text, txtReceiveQty.Text))

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
    ''' Adjust Qty Changed;
    ''' 1)Validate Adjust inward Qty is within the max level;
    ''' 2)Validate Adjust outward Qty is within the balance qty;
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
            Dim BalanceQty As Decimal = 0   ' outwards adjustment cannot below the balanceQty
            Dim AllowQty As Decimal = 0D    ' inwards adjustment cannot over the allowQty

            ' UAT02
            revAdjustQty.Validate()
            If Not revAdjustQty.IsValid Then Throw New ApplicationException(String.Format("[{0}] adjust quantity of [{1}] is not in the correct format. \nPlease enter a numeric value with 2 decimal places only.", lblStockCode.Text, txtAdjustQty.Text))

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
                    ' alert user when adjust inward excess the Max level allow
                    If AdjustQty > AllowQty Then
                        Throw New ApplicationException(String.Format("[{0}] The Adjust quantity entered [{1}] excess the allow Max level of [{2}].", lblStockCode.Text, AdjustQty.ToString("0.00"), AllowQty.ToString("0.00")))
                    End If

                Case ADJUSTOUT
                    ' alert user when Adjust outward excess available balance
                    If AdjustQty > BalanceQty Then
                        Throw New ApplicationException(String.Format("[{0}] The Adjust quantity entered [{1}] excess the available quantity of [{2}].", lblStockCode.Text, AdjustQty.ToString("0.00"), BalanceQty.ToString("0.00")))
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
            Dim BasePage As frmAdjustmentInwards = TryCast(Me.Page, frmAdjustmentInwards)
            BasePage.CancelAdjustItem(Me.UniqueID, New EventArgs)

        Else
            Dim BasePage As frmAdjustmentOutwards = TryCast(Me.Page, frmAdjustmentOutwards)
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

        If IsNumeric(txtAdjustQty.Text) Then
            AdjustDiff += CDec(txtAdjustQty.Text)
        End If

        If IsNumeric(txtReceiveQty.Text) Then
            '' Apply to Inward Return ONLY
            'If hfTranType.Value = RETURNED Then
            '    If IsNumeric(lblUnitCost.Text) Then AdjustDiffCost = AdjustDiff * CDbl(lblUnitCost.Text) 'UAT03

            'Else
            '    AdjustDiffCost += CDbl(txtTotalCost.Text)
            'End If
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

        ElseIf hfTranType.Value.StartsWith(ADJUSTIN) Then
            Dim BasePage As frmAdjustmentInwards = TryCast(Me.Page, frmAdjustmentInwards)
            BasePage.ViewStock(UniqueID _
                               , lblStockCode.Text _
                               , lblDescription.Text _
                               , lblUOM.Text _
                               , AdjustDiff _
                               , AdjustDiffCost _
                               )

        Else
            Dim BasePage As frmAdjustmentOutwards = TryCast(Me.Page, frmAdjustmentOutwards)
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

        If (hfMode.Value = EMPTY Or hfMode.Value = UPDATE) Then
            If checkReceived.Checked = True Then 'todo: approve is clicked
                hfMode.Value = UPDATE
                'todo: process return quantity here
            Else ' edit adjustment 
                If IsNumeric(txtAdjustQty.Text) Then AdjustQty = CDec(txtAdjustQty.Text)
                If IsNumeric(txtReceiveQty.Text) Then ActualReceiveQty = CDec(txtReceiveQty.Text)

                'todo: confirm hfItemReturn
                If (CDec(hfOrgAdjustQty.Value) <> AdjustQty _
                    Or CDbl(hfItemReturn.Value) <> ActualReceiveQty _
                    Or hfOrgRemarks.Value <> Trim(txtRemarks.Text) _
                    ) Then
                    hfMode.Value = UPDATE
                Else
                    hfMode.Value = EMPTY
                End If
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
        txtAdjustQty.Enabled = True
        'txtTotalCost.Enabled = True

        ' restrict qty entry when its Price adjustment 
        If hfTranType.Value.Contains("PRICE") Then
            txtAdjustQty.Enabled = False
        End If

        ' restrict totalcost entry when its a Stock adjustment or its Adjustment outward except Price
        If (hfTranType.Value.Contains("STOCK") _
            Or (hfTranType.Value.StartsWith(ADJUSTOUT) _
                And Not hfTranType.Value.Contains("PRICE") _
                ) _
            Or hfTranType.Value = RETURNED _
            ) Then
            'txtTotalCost.Enabled = False
        End If


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
                'trUnitCost.Visible = True
                trCancel.Visible = False

                ' can only return qty up to the issue qty
                lblInfo.Text = GetMessage(messageID.MoreLessThan, , , "Quantity", CDec(hfMaxLevel.Value), "<= (Less Than or Equal)")    'UAT03

                ''  truncate value to 2 decimal place for display
                'If IsNumeric(txtAdjustQty.Text) Then AdjustQty = CDec(txtAdjustQty.Text)
                'txtTotalCost.Text = IIf(AdjustQty > 0, Math.Floor(AdjustQty * CDbl(lblUnitCost.Text) * 100) / 100, "0.00")

            Else
                'trUnitCost.Visible = False
                'trCancel.Visible = True

                '' UAT02.46 allow qty is max - bal + orgAdjustQty (if any)
                'lblInfo.Text = GetMessage(messageID.MoreLessThan, , , "Quantity", CDec(hfMaxLevel.Value) - CDec(hfBalanceQty.Value) + CDec(hfOrgAdjustQty.Value), "<= (Less Than or Equal)")

                '' Disable Cancel button when the deletion will cause a negative value to the balance
                'If (CDec(hfBalanceQty.Value) - CDec(hfOrgAdjustQty.Value)) < 0 Then
                '    btnCancel.Enabled = False

                'Else
                '    btnCancel.Enabled = True
                'End If
            End If
        End If

        ' alert user with message (if any)
        If Message <> EMPTY Then
            ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", "ShowAlertMessage('" & Message & "');", True)
        End If
    End Sub

End Class