<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmSelectStore.aspx.vb" Inherits="NEA_ICS.UserInterface.frmSelectStore" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Inventory Control System (ICS)</title>
    
    <link href="Style/ICS.css" type="text/css" rel="Stylesheet" />
    
    <script type="text/javascript">

         
    </script>
    
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        
        <asp:LinkButton ID="ShowModal" runat="server" Text="" Visible="false"></asp:LinkButton>
        
        <act:ModalPopupExtender ID="mpuSelectStore" runat="server" 
        TargetControlID="ShowModal" PopupControlID="pnlSelectStore" OkControlID="ExitModal"
        BackgroundCssClass="modalStore">
        </act:ModalPopupExtender>
        
        <asp:Panel ID="pnlSelectStore" Style="display: none;" BackColor="#EDF4FC"
        runat="server" Width="50%" Height="150px" BorderWidth="1" BorderColor="#BFBFBF">
        <div style="margin: 10px;">
        
            <table width="98%" cellspacing="1" cellpadding="1" style="border-bottom: #BFBFBF 1px solid; border-top: #BFBFBF 1px solid; border-left: #BFBFBF 1px solid; border-right: #BFBFBF 1px solid;">
                <tr>
                    <td>
                    <table class="moduleTitle" width="98%" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="moduleTitleBorder" colspan="2">
                            &nbsp;Sign in to Inventory Control System (ICS)
                        </td>
                    </tr>
                    <tr>
                        <td style="background-color: #F5F5F5; color: Black;"><br />&nbsp;
                            Select your Store <br />&nbsp;
                        </td>
                        <td style="background-color: #F5F5F5; color: Black;"><br />&nbsp;
                            <asp:DropDownList ID="ddlSelectStore" runat="server" CssClass="formsComboLarge">
                            </asp:DropDownList>
                        <br />&nbsp;
                        </td>
                    </tr>
                    </table>  
                    </td>
                </tr>
            </table>
                      
            <br />
            <div align="center">
                <asp:Button ID="btnGo" Text="Go" CssClass="formsButton" runat="server" /><br />
                <asp:LinkButton ID="ExitModal" runat="server" Text="" Visible="true"></asp:LinkButton>
            </div>
        </div> 
        </asp:Panel> 
        
    </div>
    </form>
</body>
</html>
