<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmRackItemBalance.aspx.vb"
    Inherits="NEA_ICS.UserInterface.frmRackItemBalance" ValidateRequest="true" %>

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

        function setlocation(checklocation) {
            var locationfrom = document.getElementById("ddlLocationFrom");
            var locationto = document.getElementById("ddlLocationTo");

            if (checklocation.checked == true) {
                locationfrom.disabled = true;
                locationto.disabled = true;
            }
            else {
                locationfrom.disabled = false;
                locationto.disabled = false;

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
                    Management Report > Location Stock Balance Listing
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
                                Location From *
                            </td>
                            <td class="colDesc" width="30%">
                                <asp:DropDownList ID="ddlLocationFrom" runat="server" CssClass="formsCombo">
                                </asp:DropDownList>                                
                            </td>
                            <td class="colMod" width="20%">
                                Location To *
                            </td>
                            <td class="colDesc" width="30%">
                                <asp:DropDownList ID="ddlLocationTo" runat="server" CssClass="formsCombo">
                                </asp:DropDownList>
                                
                            </td>
                        </tr>
                        <tr>
                            <td class="colMod" width="20%">
                            </td>
                            <td class="colDesc" width="80%" colspan="3">
                                <asp:CheckBox ID="ckbAllLocation" runat="server" Text="All Location" OnClick="setlocation(this)" />
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
                    <rsweb:ReportViewer ID="rvr" runat="server" Font-Names="Verdana" Font-Size="8pt"
                        Height="350px" Visible="False" Width="98%">
                        <LocalReport ReportPath="ManagementReport\rptMR001GetRackItemBalance.rdlc">
                            <DataSources>
                                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="MR001GetRackItemBalanceDetails" />
                            </DataSources>
                        </LocalReport>
                    </rsweb:ReportViewer>
                    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetMR001GetRackItemBalance"
                        TypeName="NEA_ICS.UserInterface.NEA_ICS.WcfService.ServiceClient">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="" Name="storeId" Type="String" />
                            <asp:Parameter DefaultValue="" Name="rackLocationFrom" Type="String" />
                            <asp:Parameter DefaultValue="" Name="rackLocationTo" Type="String" />
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
