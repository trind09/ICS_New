<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmFrameTop.aspx.vb" Inherits="NEA_ICS.UserInterface.frmFrameTop" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Inventory Control System</title>
    <script type="text/javascript">
    
      function pageLoad() {
      }
      
      function Logout(){
        window.open('frmLogout.aspx','Logout');
      }
    
    </script>
    
    <style type="text/css">
    
    body
    {
        margin-top: 0;
        margin-left: 0;	  
    }
    
    </style>
</head>
<link href="style/ICS.css" type="text/css" rel="Stylesheet" />

<body onunload="Logout();">
    <form id="form1" runat="server">
    <div align="center">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <img id="ICSBanner" alt="ICS Banner" src="Images/ICSBanner.jpg" />
    </div>
    </form>
</body>
</html>
