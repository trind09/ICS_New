Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Partial Public Class frmUnauthorisedPageFromLogin
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            '-- CLEAR AWAY SESSIONS AND CACHES
            If Not Session.IsNewSession Then
                Session.Clear()
            End If
            Session.RemoveAll()
            Session.Abandon()

        Catch ex As Exception

            '-- TO DO: To re-direct to common login page if any error occurs
            Response.Redirect(ConfigurationManager.AppSettings("LoginURL").ToString())

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw

        End Try

    End Sub

End Class