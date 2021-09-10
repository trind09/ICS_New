<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmPrintVerificationWorksheet.aspx.vb"
    Inherits="NEA_ICS.UserInterface.frmPrintVerificationWorksheet" ValidateRequest="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Inventory Control System</title>

    <script language="javascript" src="../Script/NEA_ICS.js" type="text/javascript" />
    <script type="text/javascript">    
      function pageLoad() {
      }    
      
      function ShowSuccessMessage(value){
            
        alert(value);
            
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
                    &nbsp;Verification Worksheet > Print Verification Worksheet
                </td>
            </tr>
        </table>
        <asp:Label ID="lblWorksheetNote" runat="server" Text="Note: Please take note of the Worksheet Verification Reference No for the future use."
            CssClass="errMsg" ForeColor="Navy"></asp:Label>
        <asp:UpdatePanel ID="uplVerificationWorksheet" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlVerificationWorksheet" runat="server" Width="98%">
                    <asp:Label ID="lblErrVerificationRefNo" runat="server" CssClass="errMsg" Visible="false"></asp:Label>
                    <table class="tblModule" cellspacing="1">
                        <tr>
                            <td class="colMod" width="30%">
                                Worksheet Verification Reference No *
                            </td>
                            <td class="colDesc" width="70%" colspan="3">
                                <asp:TextBox ID="txtVerificationRefNo" runat="server" CssClass="formsText" AutoPostBack="true"></asp:TextBox>
                                <act:FilteredTextBoxExtender ID="fteVerificationRefNo" runat="server" FilterType="Numbers"
                                    TargetControlID="txtVerificationRefNo">
                                </act:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="colMod" width="30%">
                                Name of verifying officer
                            </td>
                            <td class="colDesc" width="20%">
                                <asp:TextBox ID="txtOfficerName" runat="server" CssClass="formsTextNum" Width="150px"></asp:TextBox>
                            </td>
                            <td class="colMod" width="30%">
                                Designation of verifying officer
                            </td>
                            <td class="colDesc" width="20%">
                                <asp:TextBox ID="txtDesignation" runat="server" CssClass="formsTextNum" Width="150px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="colMod" width="30%">
                                Name of checking officer
                            </td>
                            <td class="colDesc" width="20%">
                                <asp:TextBox ID="txtCheckingOfficer" runat="server" CssClass="formsTextNum" Width="150px"></asp:TextBox>
                            </td>
                            <td class="colMod" width="30%">
                                Designation of checking officer
                            </td>
                            <td class="colDesc" width="20%">
                                <asp:TextBox ID="txtCheckerDesignation" runat="server" CssClass="formsTextNum" Width="150px"></asp:TextBox>
                            </td>
                       </tr>
                       <tr>
                            <td class="colMod" width="30%">
                                Name of approving officer
                            </td>
                            <td class="colDesc" width="20%">
                                <asp:TextBox ID="txtApprovingOfficer" runat="server" CssClass="formsTextNum" Width="150px"></asp:TextBox>
                            </td>
                            <td class="colMod" width="30%">
                                Designation of approving officer
                            </td>
                            <td class="colDesc" width="20%">
                                <asp:TextBox ID="txtApproverDesignation" runat="server" CssClass="formsTextNum" Width="150px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="colMod" width="30%">
                                Sort By
                            </td>
                            <td class="colDesc" width="20%">
                                <asp:DropDownList ID="ddlSortBy" runat="server" CssClass="formsCombo">
                                <asp:ListItem Text="Stock Code" Value="WorksheetItemStockItemID"></asp:ListItem>
                                <asp:ListItem Text="Location" Value="StockItemLocation"></asp:ListItem>
                                <asp:ListItem Text="Location2" Value="StockItemLocation2"></asp:ListItem>
                                <asp:ListItem Text="Value" Value="WorksheetItemTotalCost"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="colMod" width="30%">
                                Sort Direction
                            </td>
                            <td class="colDesc" width="20%">
                                <asp:DropDownList ID="ddlSortDirection" runat="server" CssClass="formsCombo">
                                    <asp:ListItem Text="Ascending" Value="ASC"></asp:ListItem>
                                    <asp:ListItem Text="Descending" Value="DESC"></asp:ListItem>
                                </asp:DropDownList>
                            </td>                            
                        </tr>
                        <tr>
                            <td class="colMod" width="30%">
                                Print with quantity ?
                            </td>
                            <td class="colDesc" width="70%" colspan="3">
                                <asp:RadioButtonList ID="rdoQty" runat="server" BorderWidth="0" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Yes" Selected="True" Value="Y"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="colMod" width="30%">
                                Generated Date (DD/MM/YYYY)
                            </td>
                            <td class="colDesc" width="70%" colspan="3">
                                <asp:Label ID="lblGeneratedDate" runat="server"></asp:Label>
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
                        <asp:Button ID="btnPDF" CssClass="formsButton" Text="Export to PDF" Width="100px"
                            runat="server" />&nbsp;
                        <asp:Button ID="btnExcel" CssClass="formsButton" Text="Export to Excel" Width="100px"
                            runat="server" />
                        <asp:Button ID="btnCancelWorksheet" CssClass="formsButton" Text="Cancel Worksheet"
                            Width="120px" runat="server" OnClientClick="return confirm('Are you sure you want to cancel the worksheet? \nItems generated by this worksheet will be deleted.');" />
                    </div>
                    <rsweb:ReportViewer ID="rvr" runat="server" Font-Size="8pt" Height="400px" Visible="False"
                        Width="100%" Font-Names="Verdana">
                        <LocalReport ReportPath="Worksheet\rptVerificationWorksheet.rdlc">
                            <DataSources>
                                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                                    Name="WorksheetDetails" />
                            </DataSources>
                        </LocalReport>
                    </rsweb:ReportViewer>
                    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
                        SelectMethod="GetMarkedWorksheetItems" 
                        TypeName="NEA_ICS.UserInterface.NEA_ICS.WcfService.ServiceClient">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="" Name="workSheetDetails" Type="Object" />
                            <asp:Parameter DefaultValue="" Name="sortExpression" Type="String" />
                            <asp:Parameter DefaultValue="" Name="sortDirection" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnPDF" />
                <asp:PostBackTrigger ControlID="btnExcel" />
            </Triggers>
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
