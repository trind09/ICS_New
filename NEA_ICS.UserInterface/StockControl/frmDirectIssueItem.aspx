<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmDirectIssueItem.aspx.vb"
    Inherits="NEA_ICS.UserInterface.frmDirectIssueItem" ValidateRequest="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register TagPrefix="dbwc" Namespace="DBauer.Web.UI.WebControls" Assembly="DBauer.Web.UI.WebControls.DynamicControlsPlaceholder" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Inventory Control System</title>
    <link href="../style/ICS.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" src="../Script/NEA_ICS.js" type="text/javascript">
        function pageLoad() {
        }        
    </script>

    <meta http-equiv='refresh' content='1800;url=..\frmUnauthorisedPage.aspx' />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <table class="moduleTitle" width="98%" cellspacing="1" cellpadding="1">
            <tr>
                <td class="moduleTitleBorder">
                    &nbsp;Stock Control > Direct Issue
                </td>
            </tr>
        </table>
        <br />
        <act:TabContainer ID="tbcIssueItem" Width="98%" Font-Bold="true" Font-Size="Medium"
            runat="server" ForeColor="#4D36C2" BackColor="#FFFFFF" Font-Names="Verdana" ActiveTabIndex="0">
            <act:TabPanel ID="tbpNew" HeaderText="New Direct Issue" runat="server">
                <ContentTemplate>
                    <asp:UpdatePanel ID="uplNewIssue" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlMain" runat="server">
                                <asp:Label ID="lblErrAddDirectIssue" runat="server" CssClass="errMsg" Visible="false"></asp:Label>
                                <table class="tblModule" cellspacing="1">
                                    <tr>
                                        <%--                                        <td class="colMod" width="20%">
                                            Direct Issue Document No
                                        </td>
                                        <td class="colDesc" width="30%">
                                            <asp:Label ID="lblDocumentNo" runat="server" CssClass="colLabel"></asp:Label>
                                        </td>--%>
                                        <td class="colMod" width="20%">
                                            Document Type *
                                        </td>
                                        <td class="colDesc" width="80%" colspan="3">
                                            <asp:DropDownList ID="ddlDocType" runat="server" CssClass="formsCombo">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Serial No
                                        </td>
                                        <td class="colDesc" width="30%" colspan="1">
                                            <asp:TextBox ID="txtSerialNo" CssClass="formsText" runat="server" MaxLength="12"></asp:TextBox>
                                        </td>
                                        <td class="colMod" width="20%">
                                            Last Serial No
                                        </td>
                                        <td class="colDesc" width="30%" valign="middle">
                                            <asp:Label ID="lblLastSerialNo" runat="server" CssClass="colLabel"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Date of Issue (dd/mm/yyyy) *
                                        </td>
                                        <td class="colDesc" width="30%" colspan="1">
                                            <asp:TextBox ID="txtDateIssue" CssClass="formsText" runat="server"></asp:TextBox>
                                            <act:CalendarExtender ID="calDateIssue" CssClass="formsCal" runat="server" TargetControlID="txtDateIssue"
                                                Format="dd/MM/yyyy" PopupPosition="BottomLeft">
                                            </act:CalendarExtender>
                                            <act:MaskedEditExtender ID="meeDateIssue" runat="server" TargetControlID="txtDateIssue"
                                                Mask="99/99/9999" MaskType="Date">
                                            </act:MaskedEditExtender>
                                        </td>
                                        <td class="colMod" width="20%">
                                            Overall Total
                                        </td>
                                        <td class="colDesc" width="30%" valign="middle">
                                            <asp:Label ID="lblGTotal" runat="server" Text="0.0000" CssClass="colLabel"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Consumer Code *
                                        </td>
                                        <td class="colDesc" width="80%" colspan="3">
                                            <asp:DropDownList ID="ddlConsumerCode" runat="server" CssClass="formsComboLarge">
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
                                    <asp:Button ID="btnAddDirectIssue" CssClass="formsButtonLarge" Text="Add Direct Issue Item"
                                        runat="server" />&nbsp;
                                </div>
                            </asp:Panel>
                            <div id="divMsgBox" class="msg" runat="server" visible="false">
                                <table class="moduleTitle" width="100%" cellspacing="1" cellpadding="1">
                                    <tr>
                                        <td class="moduleTitleBorder">
                                            Issue Status
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                Your Direct Issue have been saved successfully.
                                <br />
                                <div align="center">
                                    <asp:Button ID="btnAddDirectIssueOK" CssClass="formsButton" Text="OK" runat="server" />
                                </div>
                                <br />
                            </div>
                            <asp:Panel ID="pnlNewIssue" runat="server" Visible="False">
                                <asp:Image ID="imgPOItem" runat="server" ImageUrl="~/Images/direct_issue_details.gif"
                                    AlternateText="Direct Issue Details" />
                                <br />
                                <asp:Label ID="lblErrSaveDirectIssue" runat="server" CssClass="errMsg" Visible="false"></asp:Label>
                                <dbwc:DynamicControlsPlaceholder ID="DCP" runat="server" ControlsWithoutIDs="DontPersist">
                                </dbwc:DynamicControlsPlaceholder>
                                <div align="center">
                                    <asp:Button ID="btnAddDirectIssueItem" CssClass="formsButtonLarge" Text="Add Direct Issue Item"
                                        runat="server" />
                                    <asp:Button ID="btnAddDirectIssueSave" CssClass="formsButton" Text="Save" runat="server"
                                        OnClientClick="return confirm('Are you sure you want to save your Direct Issue?')" />
                                    <asp:Button ID="btnAddDirectIssueCancel" CssClass="formsButton" Text="Cancel" runat="server" />
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdateProgress ID="upgNewIssue" runat="server" AssociatedUpdatePanelID="uplNewIssue">
                        <ProgressTemplate>
                            <br />
                            <img src="../images/progress.gif" alt="Processing" />
                            <asp:Label ID="lblProgress" CssClass="progress" Text="&nbsp;Processing ..." runat="server"></asp:Label>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </ContentTemplate>
            </act:TabPanel>
            <act:TabPanel ID="tbpLocate" HeaderText="Locate Direct Issue" runat="server">
                <HeaderTemplate>
                    Locate Direct Issue
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:UpdatePanel ID="uplLocateIssue" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblErrLocateDirectIssue" runat="server" Visible="false" CssClass="errMsg"></asp:Label>
                            <table class="tblModule" cellspacing="1">
                                <tr>
                                    <td class="colMod" width="20%">
                                        Direct Issue Document No #
                                    </td>
                                    <td class="colDesc" width="30%">
                                        <asp:DropDownList ID="ddlLocateIssueRefNo" runat="server" CssClass="formsCombo">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="colMod" width="20%">
                                        Serial No #
                                    </td>
                                    <td class="colDesc" width="30%">
                                        <asp:TextBox ID="txtLocateSerialNo" runat="server" CssClass="formsText"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="errMsg" colspan="4">
                                        # denotes at least one input field must be entered.
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <div align="center">
                                <asp:Button ID="btnLocateGo" CssClass="formsButton" Text="Go" runat="server" />&nbsp;
                                <asp:Button ID="btnLocateClear" CssClass="formsButton" Text="Clear" runat="server" />
                            </div>
                            <br />
                            <asp:Panel ID="pnlEditIssue" runat="server" Visible="false">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/direct_issue_details.gif"
                                    AlternateText="Direct Issue Details" />
                                <br />
                                <asp:Label ID="lblErrLocateSaveDirectIssue" runat="server" CssClass="errMsg" Visible="false"></asp:Label>
                                <table class="tblModule" cellspacing="1">
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Issue Reference No
                                        </td>
                                        <td class="colDesc" width="30%">
                                            <asp:Label ID="lblEditIssueRefNo" runat="server"></asp:Label>
                                            <asp:HiddenField ID="hidEditIssueRefNo" runat="server" />
                                        </td>
                                        <td class="colMod" width="20%">
                                            Document Type *
                                        </td>
                                        <td class="colDesc" width="30%">
                                            <asp:DropDownList ID="ddlEditDocType" runat="server" CssClass="formsCombo">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Serial No
                                        </td>
                                        <td class="colDesc" width="80%" colspan="3">
                                            <asp:Label ID="lblEditSerialNo" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Date of Issue *
                                        </td>
                                        <td class="colDesc" width="30%">
                                            <asp:TextBox ID="txtEditDateIssue" CssClass="formsText" runat="server"></asp:TextBox>
                                            <act:CalendarExtender ID="calEditDateIssue" CssClass="formsCal" runat="server" TargetControlID="txtEditDateIssue"
                                                Format="dd/MM/yyyy" PopupPosition="BottomLeft">
                                            </act:CalendarExtender>
                                            <act:MaskedEditExtender ID="meeEditDateIssue" runat="server" TargetControlID="txtEditDateIssue"
                                                Mask="99/99/9999" MaskType="Date">
                                            </act:MaskedEditExtender>
                                        </td>
                                        <td class="colMod" width="20%">
                                            Overall Total
                                        </td>
                                        <td class="colDesc" width="30%">
                                            <asp:Label ID="lblEditGTotal" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Consumer Code *
                                        </td>
                                        <td class="colDesc" width="80%" colspan="3">
                                            <asp:DropDownList ID="ddlEditConsumerCode" runat="server" CssClass="formsComboLarge">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            &nbsp;
                                        </td>
                                        <td class="colDesc" width="80%" colspan="3">
                                            <asp:LinkButton ID="lbtnLocateEdit" runat="server" CssClass="linkButton" Text="[Edit]"></asp:LinkButton>&nbsp;
                                            <asp:LinkButton ID="lbtnLocateDel" runat="server" CssClass="linkButton" OnClientClick="return confirm('Are you sure you want to delete this Direct Issue?')"
                                                Text="[Delete]"></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <asp:Label ID="lblErrEditSaveDirectIssue" runat="server" CssClass="errMsg" Visible="false"></asp:Label>
                                <dbwc:DynamicControlsPlaceholder ID="DCPEdit" runat="server" ControlsWithoutIDs="DontPersist">
                                </dbwc:DynamicControlsPlaceholder>
                                <div align="center">
                                    <asp:Button ID="btnLocateAddDirectIssueItem" CssClass="formsButtonLarge" Text="Add Direct Issue Item"
                                        runat="server" />&nbsp;
                                    <asp:Button ID="btnLocateSave" CssClass="formsButton" Text="Save" runat="server"
                                        OnClientClick="return confirm('Are you sure you want to save the changes?')"
                                        Enabled="False" />&nbsp;
                                    <asp:Button ID="btnLocateCancel" CssClass="formsButton" Text="Cancel" runat="server" />
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdateProgress ID="upgLocateIssue" runat="server" AssociatedUpdatePanelID="uplLocateIssue">
                        <ProgressTemplate>
                            <br />
                            <img src="../images/progress.gif" alt="Processing" />
                            <asp:Label ID="lblProgress" CssClass="progress" Text="&nbsp;Processing ..." runat="server"></asp:Label></ProgressTemplate>
                    </asp:UpdateProgress>
                </ContentTemplate>
            </act:TabPanel>
            <act:TabPanel ID="tbpPrint" HeaderText="Direct Issue Report" runat="server">
                <ContentTemplate>
                    <asp:UpdatePanel ID="uplPrintIssue" runat="server">
                        <ContentTemplate>
                            <table class="tblModule" cellspacing="1">
                                <tr>
                                    <td class="colMod" width="20%">
                                        Issue Date From (dd/mm/yyyy)
                                    </td>
                                    <td class="colDesc" width="30%">
                                        <asp:TextBox ID="txtPODateFrom" runat="server" CssClass="formsText" Text="01/11/2008"></asp:TextBox>
                                        <act:CalendarExtender ID="CalendarExtender3" runat="server" CssClass="formsCal" Format="dd/MM/yyyy"
                                            PopupPosition="BottomLeft" TargetControlID="txtPODateFrom">
                                        </act:CalendarExtender>
                                        <act:MaskedEditExtender ID="MaskedEditExtender3" runat="server" Mask="99/99/9999"
                                            MaskType="Date" TargetControlID="txtPODateFrom">
                                        </act:MaskedEditExtender>
                                    </td>
                                    <td class="colMod" width="20%">
                                        Issue Date To (dd/mm/yyyy)
                                    </td>
                                    <td class="colDesc" width="30%">
                                        <asp:TextBox ID="txtPODateTo" runat="server" CssClass="formsText"></asp:TextBox>
                                        <act:CalendarExtender ID="CalendarExtender4" runat="server" CssClass="formsCal" Format="dd/MM/yyyy"
                                            PopupPosition="BottomLeft" TargetControlID="txtPODateTo">
                                        </act:CalendarExtender>
                                        <act:MaskedEditExtender ID="MaskedEditExtender4" runat="server" Mask="99/99/9999"
                                            MaskType="Date" TargetControlID="txtPODateTo">
                                        </act:MaskedEditExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="colMod" width="20%">
                                        Issue Reference #
                                    </td>
                                    <td class="colDesc" width="30%" colspan="3">
                                        <asp:DropDownList ID="ddlIssueReference" runat="server" CssClass="formsCombo">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" class="errMsg">
                                        # When this is selected, other selection will be passed
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
                                <LocalReport ReportPath="StockControl\rptDirectIssueList.rdlc">
                                    <DataSources>
                                        <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DirectIssueDetails" />
                                    </DataSources>
                                </LocalReport>
                            </rsweb:ReportViewer>
                            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetDirectIssueList"
                                TypeName="NEA_ICS.UserInterface.NEA_ICS.WcfService.ServiceClient">
                                <SelectParameters>
                                    <asp:Parameter DefaultValue="" Name="storeID" Type="String" />
                                    <asp:Parameter DefaultValue="" Name="dteIssueFrom" Type="DateTime" />
                                    <asp:Parameter DefaultValue="" Name="dteIssueTo" Type="DateTime" />
                                    <asp:Parameter DefaultValue="" Name="docNo" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <br />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnPDF" />
                            <asp:PostBackTrigger ControlID="btnExcel" />
                        </Triggers>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </act:TabPanel>
        </act:TabContainer>
    </div>
    </form>
</body>
</html>
