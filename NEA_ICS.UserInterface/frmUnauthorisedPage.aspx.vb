Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports NEA_ICS.UserInterface.NEA_ICS.WcfService

Partial Public Class UnauthorisedPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            If Not Session("LoginMessage") Is Nothing Then
                ScriptManager.RegisterStartupScript(Page, GetType(Page), "AlertRegister", "alert('" & Session("LoginMessage").ToString() & "');", True)
            End If

            Try

                Dim Client As New ServiceClient
                Dim RoleDetails As New RoleDetails

                RoleDetails.StoreID = Session("StoreID")
                RoleDetails.UserID = Session("UserID")

                Client.UpdateUserAudit(RoleDetails, Session("UserSessionID"))
                Client.Close()

            Catch ex As Exception

                '-- TO DO: To re-direct to common login page if any error occurs
                Response.Redirect(ConfigurationManager.AppSettings("LoginURL").ToString())

                Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
                If (rethrow) Then Throw

            Finally

                '-- CLEAR AWAY SESSIONS AND CACHES
                If Not Session.IsNewSession Then
                    Session.Clear()
                End If
                Session.RemoveAll()
                Session.Abandon()

            End Try
        End If

    End Sub

End Class