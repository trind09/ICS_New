Imports System.Web.Services
Imports System.Web.Script.Services

Partial Public Class frmCommonLogin
    Inherits System.Web.UI.Page

    Private Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click

        Session("UserID") = txtUserID.Text
        Server.Transfer("frmSelectStore.aspx")

    End Sub

    <WebMethod()> _
    <Script.Services.ScriptMethod()> _
    Public Shared Function GetStockItems(ByVal prefixText As String, ByVal count As Integer, _
                                         ByVal contextKey As String) As List(Of String)

        Dim arr As New List(Of String)

        arr.Add("AA")
        arr.Add("AB")
        arr.Add("AC")

        Return arr

    End Function

End Class