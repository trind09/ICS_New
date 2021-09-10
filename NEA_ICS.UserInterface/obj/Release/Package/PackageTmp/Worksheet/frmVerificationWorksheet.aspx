<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmVerificationWorksheet.aspx.vb" Inherits="NEA_ICS.UserInterface.frmVerificationWorksheet" ValidateRequest="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %> 
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Inventory Control System (ICS)</title>
     <script language="javascript" src="../Script/NEA_ICS.js" type="text/javascript" />
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
    <link href="../style/ICS.css" type="text/css" rel="Stylesheet" />
    
    <meta http-equiv='refresh' content='1800;url=..\frmUnauthorisedPage.aspx' />
    
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />        
        
        <asp:UpdatePanel ID="uplChild2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table class="moduleTitle" width="96%" cellspacing="1" cellpadding="1">
                <tr>
                    <td class="moduleTitleBorder">
                        &nbsp;Verification Worksheet > Verification Worksheet
                    </td>
                </tr>
            </table>
            <br />
        </ContentTemplate>
        </asp:UpdatePanel>
            
        <asp:UpdatePanel ID="uplParent" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <ContentTemplate>
            
            <asp:Panel ID="pnlParent" runat="server" Width="96%">
            <asp:Label ID="lblErrLocateWorksheet" runat="server" CssClass="errMsg" Visible="false"></asp:Label>
            <table class="tblModule" cellspacing="0" style="border-right: #888888 1px solid;" width="96%">
                <tr>
                    <td class="colMod" width="20%" style="border-top: #888888 1px solid; border-left: #888888 1px solid;">
                    Stock Code From #
                    </td>
                    <td class="colDesc" width="30%" style="border-top: #888888 1px solid; border-left: #888888 1px solid;">
                    <asp:DropDownList ID="ddlStockCodeFrom" runat="server" CssClass="formsCombo">
                    </asp:DropDownList>
                    </td>
                    <td class="colMod" width="20%" style="border-top: #888888 1px solid; border-left: #888888 1px solid;">
                    Stock Code To #
                    </td>
                    <td class="colDesc" width="30%" style="border-top: #888888 1px solid; border-left: #888888 1px solid;">
                    <asp:DropDownList ID="ddlStockCodeTo" runat="server" CssClass="formsCombo">
                    </asp:DropDownList>
                    </td>
                </tr> 
           </table>
           </asp:Panel>
           
        </ContentTemplate>
        </asp:UpdatePanel> 
        
        <asp:UpdatePanel id="uplParent2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <ContentTemplate>
            
            <asp:Panel ID="pnlParent2" runat="server" Width="96%">
            <table class="tblModule" cellspacing="1" width="96%">
                                          
                <tr>
                    <td class="colMod" width="20%" valign="top">
                    Stock Type *
                    </td>
                    <td class="colDesc" width="80%" colspan="3">
                    <asp:CheckBoxList ID="chkStockType" runat="server" RepeatLayout="Table" RepeatDirection="Horizontal" RepeatColumns="3">
                    </asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td class="colMod" width="20%" valign="top">
                    Sub Type *
                    </td>
                    <td class="colDesc" width="80%" colspan="3">
                    <asp:CheckBoxList ID="chkSubType" runat="server" RepeatLayout="Table" RepeatDirection="Horizontal" RepeatColumns="3">
                    </asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td class="colMod" width="20%">
                    With value more than or equal to (>=)
                    </td>
                    <td class="colDesc" width="80%" colspan="3">
                    <asp:TextBox ID="txtValueMoreThan" runat="server" CssClass="formsText" Width="80px" Text="0.0000"></asp:TextBox>
                    <act:FilteredTextBoxExtender ID="fteValueMoreThan" TargetControlID="txtValueMoreThan" runat="server" FilterType="Custom" ValidChars="0123456789."></act:FilteredTextBoxExtender>
                    <asp:RegularExpressionValidator ID="revValueMoreThan" runat="server" ValidationExpression="^\d{1,15}(\.\d{0,4})?$" ControlToValidate="txtValueMoreThan" ErrorMessage="Invalid format"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" class="errMsg">
                    * denotes mandatory fields ; # denotes either one of the input fields must be entered.
                    </td>
                </tr>
            </table>
            <br />
            <div align="center">
                <asp:Button ID="btnGo" CssClass="formsButton" Text="Add" runat="server" />&nbsp;
                <asp:Button ID="btnClear" CssClass="formsButton" Text="Clear" runat="server" />
            </div>
            </asp:Panel>              
     </ContentTemplate> 
     <Triggers>
        <asp:PostBackTrigger ControlID="btnGenerateMarkedItem" />
        <asp:PostBackTrigger ControlID="btnGenerateAllItem" />
        <asp:PostBackTrigger ControlID="btnClear" />
     </Triggers>
     </asp:UpdatePanel>  
     
     <asp:UpdatePanel ID="uplChild" runat="server" UpdateMode="Conditional">
     <ContentTemplate>
            <asp:Panel ID="pnlLocateWorksheet" runat="server" Width="96%" Visible="false">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/verification_worksheet.gif" />
            <asp:Label ID="lblErrSaveWorksheet" runat="server" CssClass="errMsg" Visible="false"></asp:Label>
            <asp:GridView ID="gdvVerificationWorksheet" runat="server" CssClass="formsGrid"
            Width="100%" CellSpacing="1"
            BorderWidth="0px" AutoGenerateColumns="False">
            <FooterStyle CssClass="colFooter" />
            <RowStyle CssClass="colRow" />
            <AlternatingRowStyle CssClass="colAltRow" />
            <PagerStyle CssClass="colPager" />
            <SelectedRowStyle CssClass="colSelected" />
            <HeaderStyle CssClass="colHeader" />
            <EmptyDataRowStyle CssClass="colEmpty" />
            <EmptyDataTemplate>
            <p>No records are found.</p>
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField ItemStyle-Width="2%" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center" HeaderText="Mark?">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkMark" runat="server" />&nbsp;<%# gdvVerificationWorksheet.PageSize * gdvVerificationWorksheet.PageIndex + DataBinder.Eval(Container, "RowIndex") + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>                              
                <asp:BoundField DataField="ItemID" ItemStyle-VerticalAlign="Top" HeaderText="Stock Code" SortExpression="FStockItemByStoreStockItemID" />
                <asp:BoundField DataField="ItemDescription" itemStyle-VerticalAlign="Top" HeaderText="Description" SortExpression="FStockItemByStoreStockItemDescription" />
                <asp:BoundField DataField="StockTypeDescription" ItemStyle-VerticalAlign="Top" HeaderText="Stock Type" SortExpression="FStockItemByStockRangeStockType" />
                <asp:BoundField DataField="EquipmentID" ItemStyle-VerticalAlign="Top" HeaderText="Equipment Code" SortExpression="FStockItemByStoreStockItemEquipmentID" />
                <asp:BoundField DataField="StockQty" ItemStyle-VerticalAlign="Top" HeaderText="Balance" SortExpression="FStockItemByStoreStockItemStockBal" Dataformatstring="{0:F2}" />
                <asp:BoundField DataField="TotalValue" ItemStyle-VerticalAlign="Top" HeaderText="Total Value" SortExpression="FStockItemByStoreStockItemTotalCost" Dataformatstring="{0:F2}" />
                <asp:BoundField DataField="Location" ItemStyle-VerticalAlign="Top" HeaderText="Location1" SortExpression="FStockItemByStoreStockItemLocation" />
                <asp:BoundField DataField="Location2" ItemStyle-VerticalAlign="Top" HeaderText="Location2" SortExpression="FStockItemByStoreStockItemLocation2" />
            </Columns>
            </asp:GridView>
            <br />
            
            <div align="center">
                <asp:Button ID="btnGenerateMarkedItem" runat="server" CssClass="formsButton" Text="Generate Marked Items" Width="150px" OnClientClick="return confirm('Are you sure you want to generate worksheet for marked items?');" />&nbsp;
                <asp:Button ID="btnGenerateAllItem" runat="server" CssClass="formsButton" Text="Generate All Items" Width="150px" OnClientClick="return confirm('Are you sure you want to generate worksheet for all items?');" />
            </div>
            <br />
            </asp:Panel>
     </ContentTemplate>
     <Triggers>
        <asp:PostBackTrigger ControlID="btnGenerateMarkedItem" />
        <asp:PostBackTrigger ControlID="btnGenerateAllItem" />
        <asp:PostBackTrigger ControlID="btnClear" />
        <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
     </Triggers>
     </asp:UpdatePanel> 
     
     <asp:UpdateProgress ID="upgChild" runat="server" AssociatedUpdatePanelID="uplChild">
        <ProgressTemplate>
        <br />
            <img src="../images/progress.gif" alt="Processing" />
            <asp:Label ID="lblProgress1" CssClass="progress" Text="&nbsp;Processing ..." runat="server"></asp:Label>
        </ProgressTemplate>
     </asp:UpdateProgress>
     
     <asp:UpdateProgress ID="upgParent" runat="server" AssociatedUpdatePanelID="uplParent">
        <ProgressTemplate>
        <br />
            <img src="../images/progress.gif" alt="Processing" />
            <asp:Label ID="lblProgress2" CssClass="progress" Text="&nbsp;Processing ..." runat="server"></asp:Label>
        </ProgressTemplate>
     </asp:UpdateProgress>
     
     <asp:UpdateProgress ID="upgParent2" runat="server" AssociatedUpdatePanelID="uplParent2">
        <ProgressTemplate>
        <br />
            <img src="../images/progress.gif" alt="Processing" />
            <asp:Label ID="lblProgress3" CssClass="progress" Text="&nbsp;Processing ..." runat="server"></asp:Label>
        </ProgressTemplate>
     </asp:UpdateProgress>
     
     <asp:UpdateProgress ID="upgChild2" runat="server" AssociatedUpdatePanelID="uplChild2">
        <ProgressTemplate>
        <br />
            <img src="../images/progress.gif" alt="Processing" />
            <asp:Label ID="lblProgress4" CssClass="progress" Text="&nbsp;Processing ..." runat="server"></asp:Label>
        </ProgressTemplate>
     </asp:UpdateProgress>
        
    </div>
    </form>
</body>
</html>
