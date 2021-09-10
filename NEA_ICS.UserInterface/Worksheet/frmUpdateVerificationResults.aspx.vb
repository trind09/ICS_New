Public Partial Class frmUpdateVerificationResults
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGo.Click
        pnlSearchResults.Visible = True
    End Sub

    Private Sub gdvVerifier_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvVerifier.SelectedIndexChanged

        pnlUpdateWorksheet.Visible = True
    End Sub
End Class