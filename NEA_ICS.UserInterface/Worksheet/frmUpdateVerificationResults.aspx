<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmUpdateVerificationResults.aspx.vb" Inherits="NEA_ICS.UserInterface.frmUpdateVerificationResults" ValidateRequest="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Inventory Control System</title>
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
        
        <table class="moduleTitle" width="98%" cellspacing="1" cellpadding="1">
            <tr>
                <td class="moduleTitleBorder">
                    &nbsp;Verification Worksheet > Update Verification Results
                </td>
            </tr>
        </table>
        <br />
        <asp:UpdatePanel ID="uplVerificationWorksheet" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlVerificationWorksheet" runat="server" Width="98%">
            <table class="tblModule" cellspacing="1">
                <tr>
                    <td class="colMod" width="20%">
                        Verification Reference <br /> No *
                    </td>
                    <td class="colDesc" width="80%" colspan="3" valign="top">
                    <asp:TextBox ID="txtWorksheetRef" runat="server" CssClass="formsText"></asp:TextBox>
                    </td> 
                </tr>
                <tr>
                    <td class="colMod" width="20%">
                    Worksheet Date From *
                    </td>
                    <td class="colDesc" width="30%">
                    <asp:TextBox ID="txtWorksheetFrom" runat="server" CssClass="formsText" Text="01/12/2008"></asp:TextBox>
                    <act:CalendarExtender ID="calWorksheetFrom" runat="server" CssClass="formsCal" TargetControlID="txtWorksheetFrom"></act:CalendarExtender>
                    <act:MaskedEditExtender ID="meeWorksheetFrom" runat="server" TargetControlID="txtWorksheetFrom" Mask="99/99/9999" MaskType="Date"></act:MaskedEditExtender>
                    </td>
                    <td class="colMod" width="20%">
                    Worksheet Date To *
                    </td>
                    <td class="colDesc" width="30%">
                    <asp:TextBox ID="txtWorksheetTo" runat="server" CssClass="formsText" Text="12/12/2008"></asp:TextBox>
                    <act:CalendarExtender ID="calWorksheetTo" runat="server" CssClass="formsCal" TargetControlID="txtWorksheetTo"></act:CalendarExtender>
                    <act:MaskedEditExtender ID="meeWorksheetTo" runat="server" TargetControlID="txtWorksheetTo" Mask="99/99/9999" MaskType="Date"></act:MaskedEditExtender>
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
                <asp:Button ID="btnGo" CssClass="formsButton" Text="Go" runat="server" />&nbsp;
                <asp:Button ID="btnClear" CssClass="formsButton" Text="Clear" runat="server" />
            </div>
            
            <asp:Panel ID="pnlSearchResults" runat="server" Width="100%" Visible="false">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/search_results.gif" />
            
            <asp:GridView ID="gdvVerifier" runat="server" CssClass="formsGrid"
            Width="100%" DataSourceID="srcVerifier" AllowSorting="True" CellSpacing="1"
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
                <asp:CommandField ShowSelectButton="true" ButtonType="Image" SelectImageUrl="~/Images/edit.gif" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Reference_No" SortExpression="Reference_No" HeaderText="Verification Reference No" />
                <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Verifiying Officer" />
                <asp:BoundField DataField="Designation" SortExpression="Designation" HeaderText="Designation" />              
                <asp:BoundField DataField="Generated" DataFormatString="{0:dd/MM/yyyy}" SortExpression="Generated" HeaderText="Generated Date" />
                <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="Status" />    
            </Columns> 
            </asp:GridView> 
            <asp:SqlDataSource ID="srcVerifier" runat="server" ConnectionString='<%$ ConnectionStrings:ICS_PrototypeConnectionString %>'
            CancelSelectOnNullParameter="False" SelectCommand="spVerifier"
            SelectCommandType="StoredProcedure" >
            </asp:SqlDataSource>
            <br />
            </asp:Panel> 
            
            <asp:Panel ID="pnlUpdateWorksheet" runat="server" Visible="false">
            <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/verification_worksheet.gif" />
            
            
            <asp:GridView ID="gdvVerificationWorksheet" runat="server" CssClass="formsGrid"
            Width="100%" DataSourceID="srcVerificationWorksheet" AllowSorting="True" CellSpacing="1"
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
                <asp:BoundField DataField="StockCode" HeaderText="Stock Code" SortExpression="StockCode" />
                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                <asp:BoundField DataField="StockType" HeaderText="Stock Type" SortExpression="StockType" />
                <asp:BoundField DataField="EquipmentCode" HeaderText="Equipment Code" SortExpression="EquipmentCode" />
                <asp:BoundField DataField="TotalValue" HeaderText="Total Value" SortExpression="TotalValue" Visible="false" />
                <asp:BoundField DataField="Location1" HeaderText="Location 1" SortExpression="Location1" Visible="false" />
                <asp:BoundField DataField="Location2" HeaderText="Location 2" SortExpression="Location2" Visible="false" />
                <asp:TemplateField HeaderText="Quantity Found">
                    <ItemTemplate>
                        <asp:TextBox ID="txtQtyFound" runat="server" CssClass="formsText" Width="50px"></asp:TextBox>
                        <act:FilteredTextBoxExtender ID="fteQtyFound" runat="server" TargetControlID="txtQtyFound" FilterType="Custom" ValidChars="1234567890."></act:FilteredTextBoxExtender>
                        <act:TextBoxWatermarkExtender ID="watQtyFound" runat="server" TargetControlID="txtQtyFound" WatermarkText=" " WatermarkCssClass="waterMark"></act:TextBoxWatermarkExtender>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Balance" HeaderText="Balance" />
                <asp:TemplateField HeaderText="Remarks">
                    <ItemTemplate>
                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="formsText" TextMode="MultiLine" Width="150px"></asp:TextBox>
                        <act:TextBoxWatermarkExtender ID="watRemarks" runat="server" WatermarkText="Enter Remarks" TargetControlID="txtRemarks" WatermarkCssClass="waterMark"></act:TextBoxWatermarkExtender>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="srcVerificationWorksheet" runat="server" ConnectionString='<%$ ConnectionStrings:ICS_PrototypeConnectionString %>'
            CancelSelectOnNullParameter="False" SelectCommand="spWorksheet"
            SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            <br />
            <div align="center">
                <asp:Button ID="btnSave" CssClass="formsButton" Text="Save" runat="server" />&nbsp;
                <asp:Button ID="btnClear2" CssClass="formsButton" Text="Clear" runat="server" />
            </div>
            <br />
            </asp:Panel>
             
            </asp:Panel> 
        </ContentTemplate>
        </asp:UpdatePanel> 
        
        <asp:UpdateProgress ID="upgVerificationWorksheet" runat="server" AssociatedUpdatePanelID="uplVerificationWorksheet">
        <ProgressTemplate>
        <br />
            <img src="../images/progress.gif" alt="Processing" />
            <asp:Label ID="lblProgress" CssClass="progress" Text="&nbsp;Processing ..." runat="server"></asp:Label>
        </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    </form>
</body>
</html>
