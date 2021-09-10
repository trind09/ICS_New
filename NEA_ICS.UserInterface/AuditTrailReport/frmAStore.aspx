<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmAStore.aspx.vb" Inherits="NEA_ICS.UserInterface.frmAStore" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <table class="moduleTitle" width="98%" cellspacing="1" cellpadding="1">
            <tr>
                <td class="moduleTitleBorder">
                    Audit Trail Report > Store
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
                                &nbsp;</td>
                            <td class="colDesc" width="80%" colspan="3">
                                &nbsp;</td>
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
                        <LocalReport ReportPath="AuditTrailReport\rptAStore.rdlc">
                            <DataSources>
                                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="AStore" />
                            </DataSources>
                        </LocalReport>
                    </rsweb:ReportViewer>
                    <br />
                    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetAuditTrailAStore"
                     TypeName="NEA_ICS.UserInterface.NEA_ICS.WcfService.ServiceClient">
                        <SelectParameters>
                          <asp:Parameter DefaultValue="" Name="storeId" Type="String" />
                                <asp:Parameter DefaultValue="" Name="dateFrom" Type="String" />
                                <asp:Parameter DefaultValue="" Name="dateTo" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <br />
                    <rsweb:ReportViewer ID="rvrBlank" runat="server" Font-Size="8pt" Height="400px" Visible="False"
                        Width="100%" Font-Names="Verdana">
                        <LocalReport ReportPath="AuditTrailReport\rptBlankReport.rdlc">
                        </LocalReport>
                    </rsweb:ReportViewer>
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
