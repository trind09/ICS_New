<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmStockCard.aspx.vb"
    Inherits="NEA_ICS.UserInterface.frmStockCard" ValidateRequest="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
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
        <table class="moduleTitle" width="98%" cellspacing="1" cellpadding="1">
            <tr>
                <td class="moduleTitleBorder">
                    &nbsp;Stock Card > Stock Card
                </td>
            </tr>
        </table>
        <br />
        <asp:UpdatePanel ID="uplStockCard" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlMaster" runat="server" Width="98%">
                    <asp:Label ID="lblErrStockCard" runat="server" CssClass="errMsg" Visible="false"></asp:Label>
                    <table class="tblModule" cellspacing="0" style="border-right: #888888 1px solid;">
                        <tr>
                            <td class="colMod" width="20%" valign="top" style="border-top: #888888 1px solid; border-left: #888888 1px solid;">
                                Enter Stock Code *
                            </td>
                            <td class="colDesc" width="80%" colspan="3" style="border-top: #888888 1px solid;
                                border-left: #888888 1px solid;">                                
                                <asp:TextBox ID="txtStockCode" runat="server" CssClass="formsTextNumLarge" Width="500px"></asp:TextBox>
                                <act:AutoCompleteExtender ID="aceStockCode" runat="server" MinimumPrefixLength="2" CompletionSetCount="20" TargetControlID="txtStockCode"
                                ServiceMethod="GetStockItems" UseContextKey="true"></act:AutoCompleteExtender>
                            <br />
                            <asp:LinkButton ID="lbtnCheckStockCode" runat="server" CssClass="linkButton" Text="Check Stock Code" ></asp:LinkButton>    
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="uplStockCardInfo" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlChild" runat="server" Width="98%">
                    <table class="tblModule" cellspacing="1" cellpadding="1">
                        <tr>
                            <td class="colMod" width="20%">
                                Equipment Code
                            </td>
                            <td class="colDesc" width="80%" colspan="3">
                                <asp:Label ID="lblEquipmentCode" runat="server"></asp:Label>
                                <asp:HiddenField ID="hidEquipmentCode" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="colMod" width="20%">
                                Part Number
                            </td>
                            <td class="colDesc" width="80%" colspan="3">
                                <asp:Label ID="lblPartNo" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="colMod" width="20%">
                                Description
                            </td>
                            <td class="colDesc" width="80%" colspan="3">
                                <asp:Label ID="lblDescription" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="colMod" width="20%">
                                UOM
                            </td>
                            <td class="colDesc" width="80%" colspan="3">
                                <asp:Label ID="lblUOM" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="colMod" width="20%">
                                Location 1
                            </td>
                            <td class="colDesc" width="30%">
                                <asp:Label ID="lblLocation1" runat="server"></asp:Label>
                            </td>
                            <td class="colMod" width="20%">
                                Location 2
                            </td>
                            <td class="colDesc" width="30%">
                                <asp:Label ID="lblLocation2" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="colMod" width="20%">
                                Type
                            </td>
                            <td class="colDesc" width="30%">
                                <asp:Label ID="lblType" runat="server"></asp:Label>
                            </td>
                            <td class="colMod" width="20%">
                                Sub Type
                            </td>
                            <td class="colDesc" width="30%">
                                <asp:Label ID="lblSubType" runat="server"></asp:Label>
                            </td>
                        </tr>                        
                        <tr>
                            <td class="colMod" width="20%">
                                Maximum Level
                            </td>
                            <td class="colDesc" width="30%">
                                <asp:Label ID="lblMaxLevel" runat="server"></asp:Label>
                            </td>
                            <td class="colMod" width="20%">
                                Minimum Level
                            </td>
                            <td class="colDesc" width="30%">
                                <asp:Label ID="lblMinLevel" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="colMod" width="20%">
                                Reorder Level
                            </td>
                            <td class="colDesc" width="80%" colspan="3">
                                <asp:Label ID="lblReorderLevel" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="colMod" width="20%">
                                Avg Unit Price
                            </td>
                            <td class="colDesc" width="30%">
                                <asp:Label ID="lblAvgUnitPrice" runat="server"></asp:Label>
                            </td>
                            <td class="colMod" width="20%">
                                Current Value
                            </td>
                            <td class="colDesc" width="30%">
                                <asp:Label ID="lblCurrentVal" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="colMod" width="20%">
                                Current Balance
                            </td>
                            <td class="colDesc" width="80%" colspan="3">
                                <asp:Label ID="lblCurrentBal" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="colMod" width="20%">
                                Transaction From *
                            </td>
                            <td class="colDesc" width="30%">
                                <asp:TextBox ID="txtTransactionFrom" runat="server" CssClass="formsText"></asp:TextBox>
                                <act:CalendarExtender ID="calTransactionFrom" runat="server" CssClass="formsCal"
                                    TargetControlID="txtTransactionFrom" Format="dd/MM/yyyy">
                                </act:CalendarExtender>
                                <act:MaskedEditExtender ID="meeTransactionFrom" runat="server" Mask="99/99/9999"
                                    MaskType="Date" TargetControlID="txtTransactionFrom">
                                </act:MaskedEditExtender>
                            </td>
                            <td class="colMod" width="20%">
                                Transaction To *
                            </td>
                            <td class="colDesc" width="30%">
                                <asp:TextBox ID="txtTransactionTo" runat="server" CssClass="formsText"></asp:TextBox>
                                <act:CalendarExtender ID="calTransactionTo" runat="server" CssClass="formsCal" TargetControlID="txtTransactionTo"
                                    Format="dd/MM/yyyy">
                                </act:CalendarExtender>
                                <act:MaskedEditExtender ID="meeTransactionTo" runat="server" Mask="99/99/9999" MaskType="Date"
                                    TargetControlID="txtTransactionTo">
                                </act:MaskedEditExtender>                 
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
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:Button ID="btnTransListingPDF" CssClass="formsButton" Text="Export Transaction Listing to PDF"
                                        Width="220px" runat="server" />&nbsp;
                                    <asp:Button ID="btnTransListingExcel" CssClass="formsButton" Text="Export Transaction Listing to Excel"
                                        Width="220px" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnStockCardPDF" CssClass="formsButton" Text="Export Stock Card to PDF"
                                        Width="220px" runat="server" />&nbsp;
                                    <asp:Button ID="btnStockCardExcel" CssClass="formsButton" Text="Export Stock Card to Excel"
                                        Width="220px" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                        <asp:Button ID="btnClear" runat="server" CssClass="formsButton" Text="Clear" />       
                                </td>
                            </tr>
                        </table>
                    </div>
                    <rsweb:ReportViewer ID="rvr" runat="server" Font-Size="8pt" Height="400px" Visible="False"
                        Width="100%" Font-Names="Verdana">
                        <LocalReport ReportPath="ManagementReport\rptMR002GetTransactionList.rdlc">
                            <DataSources>
                                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="MR002GetTransactionListDetails" />
                            </DataSources>
                        </LocalReport>
                    </rsweb:ReportViewer>
                    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetMR002GetTransactionList"
                        TypeName="NEA_ICS.UserInterface.NEA_ICS.WcfService.ServiceClient">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="" Name="storeId" Type="String" />
                            <asp:Parameter DefaultValue="" Name="stockCodeFrom" Type="String" />
                            <asp:Parameter DefaultValue="" Name="stockCodeTo" Type="String" />
                            <asp:Parameter DefaultValue="" Name="transDateFrom" Type="DateTime" />
                            <asp:Parameter DefaultValue="" Name="transDateTo" Type="DateTime" />
                            <asp:Parameter DefaultValue="" Name="directIssue" Type="String" />
                            <asp:Parameter DefaultValue="" Name="equipmentID" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <rsweb:ReportViewer ID="rvrCard" runat="server" Font-Size="8pt" Height="400px" Visible="False"
                        Width="100%" Font-Names="Verdana">
                        <LocalReport ReportPath="StockCard\rptStockCard.rdlc">
                        </LocalReport>
                    </rsweb:ReportViewer>
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnTransListingPDF" />
                <asp:PostBackTrigger ControlID="btnTransListingExcel" />
                <asp:PostBackTrigger ControlID="btnStockCardPDF" />
                <asp:PostBackTrigger ControlID="btnStockCardExcel" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="upgStockCard" runat="server" AssociatedUpdatePanelID="uplStockCard">
            <ProgressTemplate>
                <br />
                <img src="../images/progress.gif" alt="Processing" />
                <asp:Label ID="lblUpdateProgress" CssClass="progress" Text="&nbsp;Processing ..."
                    runat="server"></asp:Label>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdateProgress ID="upgStockCardInfo" runat="server" AssociatedUpdatePanelID="uplStockCardInfo">
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
