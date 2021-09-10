
''' <summary>
''' Class for frmLogin
''' </summary>
''' <remarks></remarks>
Partial Public Class frmLogin
    Inherits clsCommonFunction

#Region " PAGE LOAD "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        CheckValidSessionFromLogin()

        'System.Threading.Thread.Sleep(100)
        lblLoginName.Text = "Welcome " & Session("UserName") & " to " & Session("StoreName")
        lblLoginDateTime.Text = "Last Login: " & CDate(Session("UserLastLogin")).ToString("dd/MM/yyyy hh:mm tt") & " | "

    End Sub
#End Region

#Region " LOGOUT "

    '''''' <summary>
    '''''' btnLogout - Click;
    '''''' 16 Feb 09 - Jianfa;
    '''''' </summary>
    '''''' <param name="sender"></param>
    '''''' <param name="e"></param>
    '''''' <remarks></remarks>
    'Private Sub btnLogout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogout.Click

    '    Server.Transfer("frmLogout.aspx")

    'End Sub

#End Region

End Class