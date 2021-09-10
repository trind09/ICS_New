<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmUnauthorisedPageFromLogin.aspx.vb" Inherits="NEA_ICS.UserInterface.frmUnauthorisedPageFromLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Inventory Control System</title>
    <link href="Style/ICS.css" type="text/css" rel="Stylesheet" />
</head>
<body style="background-color: #DDDDDD;">
    <form id="form1" runat="server">
    <div align="center">
        <br />
        <br />
        <br />
        <asp:Panel ID="pnlMain" Width="98%" runat="server" BackColor="White" BorderColor="#DDDDDD"
            BorderWidth="1">
            <br />
            <table width="96%" cellspacing="0" cellpadding="0" style="border-bottom: #BFBFBF 1px solid;
                border-top: #BFBFBF 1px solid; border-left: #BFBFBF 1px solid; border-right: #BFBFBF 1px solid;">
                <tr>
                    <td align="left" colspan="2">
                        <table class="moduleTitle" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td class="moduleTitleBorder">
                                    &nbsp;Invalid Page (ICS)
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="background-color: White" colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="background-color: White" align="left">
                        &nbsp;
                        <img src="Images/stop.gif" alt="Page Timeout/Unauthorised Access" />
                    </td>
                    <td style="background-color: White;">
                        <p style="color: #4D36C2; font-family: Verdana; font-size: medium;">
                            &nbsp;&nbsp;Sorry! The page that you are trying to view has been timed out OR you
                            do not have any access rights to the page.
                            <br />
                            <br />
                            Please click on this <a target="_parent" href='<%= ConfigurationManager.AppSettings("LoginURL").ToString() %>'>
                                <u>link</u></a> to login to ICS.</p>
                    </td>
                </tr>
            </table>
            <br />
        </asp:Panel>
    </div>
    </form>
</body>
</html>