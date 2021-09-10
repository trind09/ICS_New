<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmLogin.aspx.vb" Inherits="NEA_ICS.UserInterface.frmLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Inventory Control System (ICS)</title>

    <script type="text/javascript">

        function pageLoad() {
        }
    
    </script>

    <style type="text/css">
        body
        {
            margin-top: 0;
            margin-left: 0;
        }
    </style>
    <link href="style/ICS.css" type="text/css" rel="Stylesheet" />

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <%--
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:UpdatePanel ID="uplClock" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:Timer ID="timer1" runat="server" Interval="1000">
                </asp:Timer>
            </ContentTemplate>
        </asp:UpdatePanel>
--%>
        <table width="95%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td align="left">
                    <asp:Label ID="lblLoginName" CssClass="login" runat="server"></asp:Label>
                </td>
                <td align="right">
                    <asp:Label ID="lblLoginDateTime" CssClass="login" runat="server"></asp:Label>
                    <a id="btnLogout" class="logout" style="color: #666666;" target="_parent" href="frmLogout.aspx"
                        onclick="return confirm('Are you sure you want to log out of ICS?');">Logout</a>
                    <a id="btnUserManual" class="logout" style="color: #666666;" target="_blank" href="UserManual/UserManual.pdf"> | Manual</a>
                    
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
