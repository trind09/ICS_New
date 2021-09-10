<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmTreeView.aspx.vb" Inherits="NEA_ICS.UserInterface.frmTreeView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
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
    
</head>
<link href="style/ICS.css" type="text/css" rel="Stylesheet" />
<body bgcolor="#EEEEEE">
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:UpdatePanel ID="uplTreeView" runat="server">
        <ContentTemplate>
            <table width="190px" cellpadding="0" cellspacing="0" style="background-image: url('Images/background.jpg');">
            <tr>
                <td class="tableMenu">
                    <asp:TreeView ID="tvICS" BorderWidth="0" Target="mainFrame" Width="100px" runat="server" ExpandDepth="1" ShowLines="true" RootNodeStyle-Height="0" 
                    RootNodeStyle-CssClass="rootNode" ParentNodeStyle-CssClass="parentNode" LeafNodeStyle-CssClass="leafNode">
                    <Nodes>
                    <asp:TreeNode PopulateOnDemand="true"></asp:TreeNode>
                    </Nodes>
                    </asp:TreeView><br />
                </td>
            </tr>
            </table>
        </ContentTemplate>
        </asp:UpdatePanel>

    </div>
    </form>
</body>
</html>
