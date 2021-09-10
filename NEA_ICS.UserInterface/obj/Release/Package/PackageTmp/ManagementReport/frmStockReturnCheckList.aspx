<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmStockReturnCheckList.aspx.vb"
    Inherits="NEA_ICS.UserInterface.frmStockReturnCheckList" ValidateRequest="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Inventory Control System</title>
    <link href="../style/ICS.css" type="text/css" rel="Stylesheet" />
    <meta http-equiv='refresh' content='1800;url=..\frmUnauthorisedPage.aspx' />

    <script type="text/javascript">
        function pageLoad() {
        }

        function setTransDate(checkTransDate) {
            var txtDateFrom = document.getElementById("txtDateFrom");
            var txtDateTo = document.getElementById("txtDateTo");

            if (checkTransDate.checked == true) {
                txtDateFrom.disabled = true;
                txtDateTo.disabled = true;
             
            }
            else {
                txtDateFrom.disabled = false;
                txtDateTo.disabled = false;
                
            }
        }
    
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" EnableScriptGlobalization="true" runat="server" />
        <table class="moduleTitle" width="98%" cellspacing="1" cellpadding="1">
            <tr>
                <td class="moduleTitleBorder">
                    Management Report > Stock Return Checklist
                </td>
            </tr>
        </table>
        <br />
        <asp:Panel ID="pnlModule" runat="server" Width="98%">
            <asp:UpdatePanel ID="uplItem" runat="server">
                <ContentTemplate>
                    <table class="tblModule" cellspacing="1">
                        <tr>
                            <td class="colMod" width="20%">
                                Type of Checklist *</td>
                            <td class="colDesc" width="80%" colspan="3">
                                <asp:DropDownList ID="ddlReportType" runat="server" CssClass="formsCombo" AutoPostBack="true">
                                    <asp:ListItem Text=" - Please Select - " Value=""></asp:ListItem>
                                    <asp:ListItem Text="Receive" Value="R"></asp:ListItem>
                                    <asp:ListItem Text="Issue" Value="I"></asp:ListItem>
                                    <asp:ListItem Text="Adjustment" Value="A"></asp:ListItem>
                                    <asp:ListItem Text="Stock Item Balance" Value="B"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="colMod" width="20%">
                                Date From (dd/mm/yyyy) *
                            </td>
                            <td class="colDesc" width="30%">
                                <asp:TextBox ID="txtDateFrom" CssClass="formsText" runat="server"></asp:TextBox>
                                <act:CalendarExtender ID="calDateFrom" runat="server" TargetControlID="txtDateFrom"
                                    CssClass="formsCal" Format="dd/MM/yyyy">
                                </act:CalendarExtender>
                                <act:MaskedEditExtender ID="meeDateFrom" runat="server" TargetControlID="txtDateFrom"
                                    Mask="99/99/9999" MaskType="Date">
                                </act:MaskedEditExtender>
                                
                            </td>
                            <td class="colMod" width="20%">
                                Date To (dd/mm/yyyy) *
                            </td>
                            <td class="colDesc" width="30%">
                                <asp:TextBox ID="txtDateTo" CssClass="formsText" runat="server"></asp:TextBox>
                                <act:CalendarExtender ID="calDateTo" runat="server" TargetControlID="txtDateTo" CssClass="formsCal"
                                    Format="dd/MM/yyyy">
                                </act:CalendarExtender>
                                <act:MaskedEditExtender ID="meeDateTo" runat="server" TargetControlID="txtDateTo"
                                    Mask="99/99/9999" MaskType="Date">
                                </act:MaskedEditExtender>
                                
                            </td>
                        </tr>
                        <tr>
                            <td class="colMod" width="20%">
                                &nbsp;
                            </td>
                            <td class="colDesc" width="80%" colspan="3">
                                <asp:CheckBox ID="ckbTransDate" runat="server" Text="All Date" OnClick="setTransDate(this)" />
                            </td>
                        </tr>
                        <tr>
                            <td class="colMod" width="20%">
                                Group By
                            </td>
                            <td class="colDesc" width="80%" colspan="3" valign="top">
                                <asp:DropDownList ID="ddlGroupBy" runat="server" CssClass="formsCombo">
                                    <asp:ListItem Text="Stock Type" Value="S" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Consumer" Value="C"></asp:ListItem>
                                </asp:DropDownList>                                
                            </td>
                        </tr>
                        <tr>
                            <td class="colMod" width="20%">
                                &nbsp;
                            </td>
                            <td class="colDesc" width="80%" colspan="3">
                                <asp:CheckBox ID="chkExcludeZero" Text="Exclude Zero Value Stock Items" Visible="false" runat="server" />
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
                        <asp:Button ID="btnPDF" CssClass="formsButton" Text="Export to PDF" runat="server"
                            Width="100px" />&nbsp;
                        <asp:Button ID="btnExcel" CssClass="formsButton" Text="Export to Excel" runat="server"
                            Width="100px" />&nbsp;
                        <input class="formsButton" value="Clear" type="reset" id="btnReset" />
                    </div>
                    <rsweb:ReportViewer ID="rvr" runat="server" Font-Size="8pt" Height="400px" Visible="False"
                        Width="100%" Font-Names="Verdana">
                        <LocalReport ReportPath="ManagementReport\rptMR006StockReturnCheckListReceive.rdlc">
                            <DataSources>
                                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="MR006StockReturnCheckListReceiveDetails" />
                                <rsweb:ReportDataSource DataSourceId="ObjectDataSource2" Name="MR006StockReturnCheckListIssueDetails" />
                                <rsweb:ReportDataSource DataSourceId="ObjectDataSource3" Name="MR006StockReturnCheckListAdjustDetails" />
                                <rsweb:ReportDataSource DataSourceId="ObjectDataSource4" Name="MR006StockReturnCheckListStockItemDetails" />
                            </DataSources>
                        </LocalReport>
                    </rsweb:ReportViewer>
                    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetMR006StockReturnCheckListReceive"
                        TypeName="NEA_ICS.UserInterface.NEA_ICS.WcfService.ServiceClient" DataObjectTypeName="MR006StockReturnCheckListReceiveDetails">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="" Name="storeId" Type="String" />
                            <asp:Parameter DefaultValue="" Name="transDateFrom" Type="DateTime" />
                            <asp:Parameter DefaultValue="" Name="transDateTo" Type="DateTime" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" SelectMethod="GetMR006StockReturnCheckListIssue"
                        TypeName="NEA_ICS.UserInterface.NEA_ICS.WcfService.ServiceClient" DataObjectTypeName="MR006StockReturnCheckListIssueDetails">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="" Name="storeId" Type="String" />
                            <asp:Parameter DefaultValue="" Name="transDateFrom" Type="DateTime" />
                            <asp:Parameter DefaultValue="" Name="transDateTo" Type="DateTime" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <asp:ObjectDataSource ID="ObjectDataSource3" runat="server" SelectMethod="GetMR006StockReturnCheckListAdjust"
                        TypeName="NEA_ICS.UserInterface.NEA_ICS.WcfService.ServiceClient" DataObjectTypeName="MR006StockReturnCheckListAdjustDetails">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="" Name="storeId" Type="String" />
                            <asp:Parameter DefaultValue="" Name="transDateFrom" Type="DateTime" />
                            <asp:Parameter DefaultValue="" Name="transDateTo" Type="DateTime" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <asp:ObjectDataSource ID="ObjectDataSource4" runat="server" SelectMethod="GetMR006StockReturnCheckListStockItem"
                        TypeName="NEA_ICS.UserInterface.NEA_ICS.WcfService.ServiceClient" DataObjectTypeName="MR006StockReturnCheckListStockItemDetails">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="" Name="storeId" Type="String" />
                            <asp:Parameter DefaultValue="" Name="transDateFrom" Type="DateTime" />
                            <asp:Parameter DefaultValue="" Name="transDateTo" Type="DateTime" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <br />
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnPDF" />
                    <asp:PostBackTrigger ControlID="btnExcel" />
                </Triggers>
            </asp:UpdatePanel>
            <asp:UpdateProgress ID="upgItem" runat="server" AssociatedUpdatePanelID="uplItem">
                <ProgressTemplate>
                    <br />
                    <img src="../images/progress.gif" alt="Processing" />
                    <asp:Label ID="lblProgress" CssClass="progress" Text="&nbsp;Processing ..." runat="server"></asp:Label></ProgressTemplate>
            </asp:UpdateProgress>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
