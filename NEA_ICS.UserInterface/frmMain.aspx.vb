Imports NEA_ICS.UserInterface.NEA_ICS.WcfService
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Partial Public Class frmMain
    Inherits clsCommonFunction

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim UserRoleType

            ' Manage Financial Closing
            Using Client As New ServiceClient
                Client.ManageFinancialClosing(Session(ESession.StoreID.ToString) _
                                              , Session(ESession.UserID.ToString) _
                                              )

                Client.Close()
            End Using

            ' Default Startup page based on its role
            ' UAT02.61 - only default for AO
            UserRoleType = Session(ESession.UserRoleType.ToString)
            If UserRoleType.ToString.Contains(APPROVALOFFICER) Then
                Response.Redirect("StockControl/frmIssueItem.aspx")
                ''Else

                ''-- To add on for the rest of users
                '' TODO to select TOP 1 from the left menu
                ''Response.Redirect("StockControl/frmOrderItem.aspx")

            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

End Class