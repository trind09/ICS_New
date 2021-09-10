Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports NEA_ICS.UserInterface.NEA_ICS.WcfService

''' <summary>
''' Class - frmLogout
''' 16 Feb 09 - Jianfa;
''' </summary>
''' <remarks></remarks>
Partial Public Class frmLogout
    Inherits clsCommonFunction

#Region " PAGE LOAD "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

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

                ''-- TO USE THE CORRECT URL PATH FOR PROD
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

                Response.Cookies("ASP.NET_SessionId").Expires = DateTime.Now
                Response.Cookies.Add(New HttpCookie("ASP.NET_SessionId", Guid.NewGuid().ToString()))

                Response.Cache.SetExpires(Now)

            End Try

        End If

    End Sub
#End Region

End Class