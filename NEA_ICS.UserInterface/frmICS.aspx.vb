
Partial Public Class frmICS
    Inherits clsCommonFunction

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        CheckValidSessionFromLogin()

        Cache(Session(ESession.StoreID.ToString) & "dtCommon") = clsCommonFunction.GetCommonDataTable(Session("StoreID"))
        Cache(Session(ESession.UserID.ToString) & Session(ESession.StoreID.ToString) & "AccessRights") = clsCommonFunction.PopulateAccessRights(Session("StoreID"), Session("UserID"))

    End Sub

End Class