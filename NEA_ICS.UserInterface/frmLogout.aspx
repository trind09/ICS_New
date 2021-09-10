<%@ Page Language="vb" EnableSessionState="True" AutoEventWireup="false" CodeBehind="frmLogout.aspx.vb" Inherits="NEA_ICS.UserInterface.frmLogout" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Inventory Control System (ICS)</title>
    <script type="text/javascript">
    
      function pageLoad() {
        window.close();
      }
    
    </script>
</head>
<body onload="pageLoad()">
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
    </div>
    </form>
</body>
</html>
