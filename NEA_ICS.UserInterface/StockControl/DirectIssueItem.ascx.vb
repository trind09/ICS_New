Public Partial Class DirectIssueItem
    Inherits System.Web.UI.UserControl

#Region " PAGE LOAD "

    ''' <summary>
    ''' Page Load;
    ''' 12 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#End Region

    ''' <summary>
    ''' lbtnCancel - Click;
    ''' 12 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lbtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnCancel.Click

        Dim BasePage As frmDirectIssueItem = TryCast(Me.Page, frmDirectIssueItem)

        If IsNumeric(Trim(txtTotalCost.Text)) Then
            BasePage.UpdateGTotal(-Convert.ToDouble(txtTotalCost.Text))
        End If

        BasePage.CancelDirectIssue(Me.UniqueID, New EventArgs)

    End Sub

    ''' <summary>
    ''' txtTotalCost - TextChanged;
    ''' 12 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtTotalCost_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTotalCost.TextChanged

        Try

            ''ADD NEW DIRECT ISSUE MODE
            If hidMode.Value Is Nothing Or hidMode.Value = String.Empty Then

                Dim TotalCost As Decimal = 0.0
                Dim BasePage As frmDirectIssueItem = TryCast(Me.Page, frmDirectIssueItem)

                If ViewState("_TotalCost") Is Nothing Then
                    ViewState("_TotalCost") = 0.0
                End If

                TotalCost = IIf(revTotalCost.IsValid, Convert.ToDecimal(txtTotalCost.Text), 0.0)
                BasePage.UpdateGTotal(TotalCost - ViewState("_TotalCost"))
                ViewState("_TotalCost") = TotalCost

            Else

                ''EDIT DIRECT ISSUE MODE

                Dim TotalCost As Decimal = 0.0
                Dim BasePage As frmDirectIssueItem = TryCast(Me.Page, frmDirectIssueItem)

                TotalCost = IIf(revTotalCost.IsValid, Convert.ToDecimal(txtTotalCost.Text), 0.0)

                BasePage.UpdateGTotal(TotalCost, IIf(hidTotalCost.Value = String.Empty, 0.0, hidTotalCost.Value))

            End If
            
        Catch ex As Exception
            revTotalCost.IsValid = False
        End Try

    End Sub
End Class