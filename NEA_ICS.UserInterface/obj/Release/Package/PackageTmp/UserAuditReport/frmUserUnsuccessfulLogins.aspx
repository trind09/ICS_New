<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmUserUnsuccessfulLogins.aspx.vb"
    Inherits="NEA_ICS.UserInterface.frmUserUnsuccessfulLogins" ValidateRequest="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Inventory Control System</title>
    <link href="../style/ICS.css" type="text/css" rel="Stylesheet" />
    <meta http-equiv='refresh' content='1800;url=..\frmUnauthorisedPage.aspx' />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" EnableScriptGlobalization="true" runat="server" />
        <table class="moduleTitle" width="98%" cellspacing="1" cellpadding="1">
            <tr>
                <td class="moduleTitleBorder">
                    User Audit Report &gt; Users with Unsuccessful Login Attempts
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
                                User Name
                            </td>
                            <td class="colDesc" width="30%">
                                <asp:TextBox ID="txtUserName" runat="server" CssClass="formsTextNum" MaxLength="9"></asp:TextBox>
                            </td>
                            <td class="colMod" width="20%">
                                &nbsp;
                            </td>
                            <td class="colDesc" width="30%">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="colMod" width="20%">
                                Date From (dd/mm/yyyy) *
                            </td>
                            <td class="colDesc" width="30%">
                                <asp:TextBox ID="txtDateFrom" runat="server" CssClass="formsText"></asp:TextBox>
                                <act:CalendarExtender ID="calDateFrom" runat="server" CssClass="formsCal" Format="dd/MM/yyyy"
                                    TargetControlID="txtDateFrom">
                                </act:CalendarExtender>
                                <act:MaskedEditExtender ID="meeDateFrom" runat="server" Mask="99/99/9999" MaskType="Date"
                                    TargetControlID="txtDateFrom">
                                </act:MaskedEditExtender>
                            </td>
                            <td class="colMod" width="20%">
                                Date To (dd/mm/yyyy) *
                            </td>
                            <td class="colDesc" width="30%">
                                <asp:TextBox ID="txtDateTo" runat="server" CssClass="formsText"></asp:TextBox>
                                <act:CalendarExtender ID="calDateTo" runat="server" CssClass="formsCal" Format="dd/MM/yyyy"
                                    TargetControlID="txtDateTo">
                                </act:CalendarExtender>
                                <act:MaskedEditExtender ID="meeDateTo" runat="server" Mask="99/99/9999" MaskType="Date"
                                    TargetControlID="txtDateTo">
                                </act:MaskedEditExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="colMod" width="20%">
                                Sort By
                            </td>
                            <td class="colDesc" width="80%" colspan="3">
                                <asp:DropDownList ID="ddlSortBy" CssClass="formsCombo" runat="server">
                                    <asp:ListItem Text=" - Please Select - " Value=""></asp:ListItem>
                                    <asp:ListItem Text="User Name"></asp:ListItem>
                                    <asp:ListItem Text="Date/Time"></asp:ListItem>
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
                        <asp:Button ID="btnPDF" CssClass="formsButton" Text="Export to PDF" runat="server"
                            Width="100px" />&nbsp;
                        <asp:Button ID="btnExcel" CssClass="formsButton" Text="Export to Excel" runat="server"
                            Width="100px" />&nbsp;
                        <input class="formsButton" value="Clear" type="reset" id="btnReset" />
                    </div>
                    <rsweb:ReportViewer ID="rvr" runat="server" Font-Size="8pt" Height="400px" Visible="False"
                        Width="100%" Font-Names="Verdana">
                        <LocalReport ReportPath="UserAuditReport\rptAR002GetUserUnsuccessfulLogin.rdlc">
                            <DataSources>
                                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="RoleDetails" />
                            </DataSources>
                        </LocalReport>
                    </rsweb:ReportViewer>
                    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetUserUnsuccessfulLoginList"
                        TypeName="NEA_ICS.UserInterface.NEA_ICS.WcfService.ServiceClient">
                        <SelectParameters>
                            <asp:Parameter Name="storeId" Type="String" DefaultValue="" />
                            <asp:Parameter Name="userId" Type="String" DefaultValue="" />
                            <asp:Parameter Name="fromDte" Type="DateTime" DefaultValue="" />
                            <asp:Parameter Name="toDte" Type="DateTime" DefaultValue="" />
                            <asp:Parameter Name="sortBy" Type="String" DefaultValue="" />
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
