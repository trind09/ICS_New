Public Partial Class frmFrameTop
    Inherits clsCommonFunction

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        CheckValidSessionFromLogin()

    End Sub

End Class