<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmCommon.aspx.vb" Inherits="NEA_ICS.UserInterface.frmCommon" ValidateRequest="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Inventory Control System (ICS)</title>
    <script type="text/javascript" src="../Script/NEA_ICS.js">
    
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
    <link href="../style/ICS.css" type="text/css" rel="Stylesheet" />
    <meta http-equiv='refresh' content='1800;url=..\frmUnauthorisedPage.aspx' />
    
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        
        <table class="moduleTitle" width="94%" cellspacing="1" cellpadding="1">
            <tr>
                <td class="moduleTitleBorder">
                    &nbsp;Master List > Common
                </td>
            </tr>
        </table>
            <asp:Panel ID="pnlMain" runat="server" Width="94%">
            <asp:UpdatePanel ID="uplCommonItems" runat="server">
                        <ContentTemplate>
                        <asp:Label ID="lblErrLocate" CssClass="errMsg" runat="server" Text="<br />"></asp:Label>
                            <asp:Panel ID="pnlCode" runat="server">
                                <table class="tblModule" cellspacing="1">
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Code Group *                                            
                                        </td>
                                        <td class="colDesc" width="80%" colspan="3">
                                            <asp:DropDownList ID="ddlCommonItems" runat="server" CssClass="formsCombo" Width="50%">                                           
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                    <td colspan="4" class="errMsg">
                                    * denotes mandatory fields
                                    </td>
                                </tr>                                    
                                </table>
                                
                                <br />
                                <div align="center">
                                    <asp:Button ID="btnGo" CssClass="formsButton" Text="Go" runat="server" />
                                    <asp:Button ID="btnClear" CssClass="formsButton" Text="Clear" runat="server" />
                                </div>
                                <br />
                            </asp:Panel>
                            <asp:Panel ID="pnlCommonInfo" runat="server" Visible="False">
                            <asp:Image ID="imgSearchResults" runat="server" ImageUrl="~/Images/search_results.gif" />
                            <br />
                            <asp:Label ID="lblErrSaveCode" runat="server" CssClass="errMsg" Visible="false"></asp:Label>                             
                            <asp:GridView ID="gdvLocateCode" runat="server" CssClass="formsGrid" Width="100%" AllowSorting="False" CellSpacing="1"
                                    BorderWidth="0px" AutoGenerateColumns="False">
                                    <FooterStyle CssClass="colFooter" />
                                    <RowStyle CssClass="colRow" />
                                    <AlternatingRowStyle CssClass="colAltRow" />
                                    <PagerStyle CssClass="colPager" />
                                    <SelectedRowStyle CssClass="colSelected" />
                                    <HeaderStyle CssClass="colHeader" />
                                    <EmptyDataRowStyle CssClass="colEmpty" />
                                    <EmptyDataTemplate>
                                        <p>
                                            No records are found.</p>
                                    </EmptyDataTemplate>
                                    <Columns>
                                    <asp:CommandField ShowEditButton="true" ButtonType="Image" UpdateImageUrl="~/Images/save.gif" CancelImageUrl="~/Images/cancel.gif" EditImageUrl="~/Images/edit.gif" ItemStyle-Width="7%" ItemStyle-HorizontalAlign="Center" HeaderText="Edit?" />
                                    <asp:TemplateField HeaderText="Code ID" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCodeID" runat="server" Text='<%# Bind("CodeID") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblDisplayCodeID" runat="server" Text='<%# Bind("CodeID") %>' Visible="False" ></asp:Label>
                                            <asp:HiddenField ID="hidCommonID" runat="server" Value='<%# Bind("CommonID") %>' />
                                            <asp:HiddenField ID="hidCodeGroup" runat="server" Value='<%# Bind("CodeGroup") %>' />
                                            <asp:TextBox ID="txtCodeID" runat="server" MaxLength="10" CssClass="formsTextNum" Text='<%# Bind("CodeID") %>'></asp:TextBox>&nbsp;*&nbsp;
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Code Description" ItemStyle-Width="40%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCodeDescription" runat="server" Text='<%# Bind("CodeDescription") %>' ></asp:Label>
                                        </ItemTemplate>   
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtCodeDescription" runat="server" CssClass="formsText" MaxLength="50" TextMode="MultiLine" Text='<%# Bind("CodeDescription") %>' ></asp:TextBox>&nbsp;*
                                        </EditItemTemplate>
                                    </asp:TemplateField>                                    
                                    <asp:TemplateField HeaderText="Status" ItemStyle-Width="38%">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hidCommonID" runat="server" Value='<%# Bind("CommonID") %>' />
                                            <asp:HiddenField ID="hidStatus" runat="server" Value='<%# Bind("Status") %>' />
                                            <asp:Label ID="lblStatus" runat="server" ></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:HiddenField ID="hidEditStatus" runat="server" Value='<%# Bind("Status") %>' />
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="formsCombo">
                                            <asp:ListItem Value="O" Text="Open"></asp:ListItem>
                                            <asp:ListItem Value="C" Text="Closed"></asp:ListItem>
                                            <asp:ListItem Value="D" Text="Deleted"></asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    </Columns>
                                    </asp:GridView> 
                                    <br />
                                    <div align="center">
                                        <asp:Button ID="btnAddNewCode" CssClass="formsButton" Text="Add New Code" Width="120px" runat="server" />
                                        <asp:Button ID="btnCancel" CssClass="formsButton" Text="Cancel" runat="server" />
                                    </div>
                                    <br />
                            </asp:Panel>
                            <asp:Panel ID="pnlAddCode" runat="server" Visible="false">
                            <asp:Image ID="imgSearchResults2" runat="server" ImageUrl="~/Images/search_results.gif" />
                            <br />
                            <asp:Label ID="lblErrAddCode" runat="server" CssClass="errMsg" Visible="false"></asp:Label>
                            <table class="tblModule" cellspacing="1">
                                <tr>
                                    <td class="colMod" width="20%">
                                        Code Group
                                    </td>
                                    <td class="colDesc" width="80%" colspan="3">                                          
                                        <asp:Label ID="lblDisplayCodeGroup" runat="server"></asp:Label>                                            
                                    </td>
                                </tr>
                                <tr>
                                    <td class="colMod" width="20%">
                                        Code ID *
                                    </td>
                                    <td class="colDesc" width="80%" colspan="3">
                                        <asp:TextBox ID="txtAddCodeID" runat="server" CssClass="formsTextNum" MaxLength="10"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="colMod" width="20%">
                                        Code Description *
                                    </td>
                                    <td class="colDesc" width="30%">
                                        <asp:TextBox ID="txtAddCodeDescription" runat="server" CssClass="formsText" MaxLength="50"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" class="errMsg">
                                    * denotes mandatory fields
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <div align="center">
                                <asp:Button ID="btnAddCodeSave" CssClass="formsButton" Text="Save" runat="server" />
                                <asp:Button ID="btnAddCodeClear" CssClass="formsButton" Text="Clear" runat="server" />
                                <asp:Button ID="btnAddCodeCancel" CssClass="formsButton" Text="Cancel" runat="server" />
                            </div>
                            </asp:Panel>  
                            <div id="divMsgBoxAddCode" class="msg" runat="server" visible="false">
                                <table class="moduleTitle" width="100%" cellspacing="1" cellpadding="1">
                                    <tr>
                                        <td class="moduleTitleBorder">
                                            Code Status
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                Your Code has been saved successfully. Here are the details of your Code:
                                <br />
                                <br />
                                <span lang="en-sg">Code Information</span>
                                <br />
                                <table class="tblModule" cellspacing="1">
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Code Group
                                        </td>
                                        <td class="colRow" width="70%">
                                            <asp:Label ID="lblMsgCodeGroup" runat="server"></asp:Label>
                                        </td>
				                    </tr>
				                    <tr>
				                        <td class="colMod" width="20%">
				                            Code ID
				                        </td>
				                        <td class="colAltRow" width="70%">
				                            <asp:Label ID="lblMsgCodeID" runat="server"></asp:Label>
				                        </td>
				                    </tr>
				                    <tr>
				                        <td class="colMod" width="20%">
				                            Code Description
				                        </td>
				                        <td class="colRow" width="70%">
				                            <asp:Label ID="lblMsgCodeDescription" runat="server"></asp:Label>
				                        </td>
				                    </tr>
				                </table>
				                <br />
                                <div align="center">
                                    <asp:Button ID="btnAddCodeOK" CssClass="formsButton" Text="OK" runat="server" />
                                </div>
                                <br />
                                </div>                                                 
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdateProgress ID="upgCommonItems" runat="server" AssociatedUpdatePanelID="uplCommonItems">
                        <ProgressTemplate>
                            <br />
                            <img src="../images/progress.gif" alt="Processing" />
                            <asp:Label ID="lblProgress" CssClass="progress" Text="&nbsp;Processing ..." runat="server"></asp:Label>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
            </asp:Panel>
                    
        
    </div>
    </form>
</body>
</html>
