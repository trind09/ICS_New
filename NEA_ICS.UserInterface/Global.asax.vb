Imports System.Web.SessionState
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports NEA_ICS.UserInterface.NEA_ICS.WcfService
Imports System.Web.UI

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application is started
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Protected Sub Application_PreSendRequestHeaders(ByVal sender As Object, ByVal e As EventArgs) ' Remove the "Server" HTTP Header from response 
        Dim app As HttpApplication = TryCast(sender, HttpApplication)
        If app IsNot Nothing AndAlso app.Request IsNot Nothing AndAlso Not app.Request.IsLocal AndAlso app.Context IsNot Nothing AndAlso app.Context.Response IsNot Nothing Then
            Dim headers As NameValueCollection = app.Context.Response.Headers
            If headers IsNot Nothing Then
                headers.Remove("Server")
            End If
        End If
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an error occurs

        Logout()

    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session ends

        'Logout()

    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends

        'Logout()

    End Sub

#Region " Logout "
    Private Sub Logout()

        Try
            If Session("UserID") Is Nothing Then
                Exit Try
            End If

            Dim Client As New ServiceClient
            Dim RoleDetails As New RoleDetails

            RoleDetails.StoreID = Session("StoreID")
            RoleDetails.UserID = Session("UserID")

            Client.UpdateUserAudit(RoleDetails, Session("UserSessionID"))
            Client.Close()

        Catch ex As Exception

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        Finally
            Response.Cookies("ASP.NET_SessionId").Expires = DateTime.Now
            Response.Cookies.Add(New HttpCookie("ASP.NET_SessionId", Guid.NewGuid().ToString()))

            Response.Cache.SetExpires(Now)
        End Try

    End Sub
#End Region

End Class