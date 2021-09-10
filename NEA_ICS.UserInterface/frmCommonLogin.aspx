<%@ Page Language="vb" AutoEventWireup="false" EnableSessionState="True" CodeBehind="frmCommonLogin.aspx.vb" Inherits="NEA_ICS.UserInterface.frmCommonLogin" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Inventory Control System</title>
    <script type="text/javascript">
    
      function pageLoad() {
      }
    
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <br />
        User ID:
        <asp:TextBox ID="txtUserID" runat="server"></asp:TextBox>
        <br />
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" />
        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtUserID" CompletionSetCount="20"
    UseContextKey="true" MinimumPrefixLength="1" ServiceMethod="GetStockItems">
    </cc1:AutoCompleteExtender>
    
    </div>
    
    
    </form>
    
</body>
</html>
