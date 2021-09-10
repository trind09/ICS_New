<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmReceiveItem.aspx.vb"
    Inherits="NEA_ICS.UserInterface.frmReceiveItem" ValidateRequest="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="DBauer.Web.UI.WebControls.DynamicControlsPlaceholder" Namespace="DBauer.Web.UI.WebControls"
    TagPrefix="dbwc" %>
<%@ Register Src="ReceiveItem.ascx" TagName="ReceiveItem" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Inventory Control System</title>
    <link href="../style/ICS.css" type="text/css" rel="Stylesheet" />

    <script src="../Script/NEA_ICS.js" language="javascript" type="text/javascript">
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
                    &nbsp;Stock Control > Receive Item
                </td>
            </tr>
        </table>
        <br />
        <asp:UpdatePanel ID="uplPage" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <act:TabContainer ID="tbcReceiveItem" Width="98%" Font-Bold="true" Font-Size="Medium"
                    runat="server" ForeColor="#4D36C2" BackColor="#FFFFFF" 
                    Font-Names="Verdana" ActiveTabIndex="0">
                    <act:TabPanel ID="tbpNew" HeaderText="New Receive" runat="server">
                        <HeaderTemplate>
                            New Receive
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="uplNewReceive" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="uplMain" runat="server" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlMain" runat="server">
                                                <table class="tblModule" cellspacing="1">
                                                    <tr>
                                                        <td class="colMod" width="20%">
                                                            Order Reference *
                                                        </td>
                                                        <td class="colDesc" width="30%" colspan="1">
                                                            <asp:DropDownList ID="ddlOrderID" runat="server" CssClass="formsCombo" TabIndex="1">
                                                            </asp:DropDownList>
                                                            <br />
                                                        </td>
                                                        <td class="colMod" width="20%">
                                                            Receive Date (dd/mm/yyyy) *
                                                        </td>
                                                        <td class="colDesc" width="30%" colspan="1">
                                                            <asp:TextBox ID="txtReceiveDate" CssClass="formsText" runat="server"></asp:TextBox>
                                                            <act:CalendarExtender ID="calReceiveDate" CssClass="formsCal" runat="server" TargetControlID="txtReceiveDate"
                                                                Format="dd/MM/yyyy" PopupPosition="BottomLeft">
                                                            </act:CalendarExtender>
                                                            <act:MaskedEditExtender ID="meeReceiveDate" runat="server" TargetControlID="txtReceiveDate"
                                                                Mask="99/99/9999" MaskType="Date">
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
                                                    <asp:Button ID="btnGo" CssClass="formsButton" Text="Go" runat="server" />
                                                </div>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:UpdatePanel ID="uplDetail" runat="server" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlReceive" runat="server" Visible="False">
                                                <asp:Image ID="imgReceive" runat="server" ImageUrl="~/Images/receive_details.gif"
                                                    AlternateText="Receive Details" />
                                                <br />
                                                <dbwc:DynamicControlsPlaceholder ID="DCP" runat="server" ControlsWithoutIDs="DontPersist">
                                                </dbwc:DynamicControlsPlaceholder>
                                                <asp:Panel ID="pnlAction" runat="server">
                                                    <div align="center">
                                                        <br />
                                                        <asp:Button ID="btnSave" runat="server" CssClass="formsButton" OnClientClick="return confirm('Are you sure you want to save your receive?')"
                                                            TabIndex="102" Text="Save" />&nbsp;
                                                        <asp:Button ID="btnCancel" runat="server" CssClass="formsButton" TabIndex="103" Text="Cancel" />
                                                    </div>
                                                </asp:Panel>
                                            </asp:Panel>
                                            <asp:Button ID="ShowModal" runat="server" Text="ShowModal" Visible="false" Height="1px"
                                                Width="1px" />
                                            <act:ModalPopupExtender ID="mpuStockAvailability" runat="server" BackgroundCssClass="modalBackground"
                                                CancelControlID="btnExit" OkControlID="btnExit" PopupControlID="pnlStockAvailability"
                                                TargetControlID="ShowModal">
                                            </act:ModalPopupExtender>
                                            <asp:Panel ID="pnlStockAvailability" Style="display: none;" BackColor="White" runat="server"
                                                Width="60%" BorderWidth="1" BorderColor="Gray">
                                                <div style="margin: 10px;">
                                                    <table class="moduleTitle" width="100%" cellspacing="1" cellpadding="1">
                                                        <tr>
                                                            <td class="moduleTitleBorder" align="center">
                                                                Current Stock Availability
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <br />
                                                    <table class="tblModule" cellspacing="1">
                                                        <tr>
                                                            <td class="colMod" width="30%">
                                                                Stock Code
                                                            </td>
                                                            <td class="colRow" width="70%" align="center">
                                                                <asp:Label ID="ViewStockCode" runat="server" CssClass="colLabel"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="colMod" width="30%">
                                                                Description
                                                            </td>
                                                            <td class="colAltRow" width="70%" align="center">
                                                                <asp:Label ID="ViewDesc" runat="server" CssClass="colLabel"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="colHeader" colspan="2" align="center">
                                                                Before Item Receive (indicative)
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="colMod" width="30%">
                                                                Available
                                                            </td>
                                                            <td class="colRow" width="70%" align="center">
                                                                <asp:Label ID="ViewBalance" runat="server" CssClass="colLabel"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="colMod" width="30%">
                                                                Avg Unit Cost
                                                            </td>
                                                            <td class="colAltRow" width="70%" align="center">
                                                                <asp:Label ID="ViewAUC" runat="server" CssClass="colLabel"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="colMod" width="30%">
                                                                Total Item Value
                                                            </td>
                                                            <td class="colRow" width="70%" align="center">
                                                                <asp:Label ID="ViewTotalValue" runat="server" CssClass="colLabel"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="colHeader" colspan="2" align="center">
                                                                After Item Receive (indicative)
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="colMod" width="30%">
                                                                New Available
                                                            </td>
                                                            <td class="colAltRow" width="70%" align="center">
                                                                <asp:Label ID="ViewBalanceNew" runat="server" CssClass="colLabel"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="colMod" width="30%">
                                                                New Avg Unit Cost
                                                            </td>
                                                            <td class="colRow" width="70%" align="center">
                                                                <asp:Label ID="ViewAUCNew" runat="server" CssClass="colLabel"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="colMod" width="30%">
                                                                New Total Item Value
                                                            </td>
                                                            <td class="colAltRow" width="70%" align="center">
                                                                <asp:Label ID="ViewTotalValueNew" runat="server" CssClass="colLabel"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="colHeader" colspan="2">
                                                                <br />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="colMod" width="30%">
                                                                Order Item Outstanding
                                                            </td>
                                                            <td class="colRow" width="70%" align="center">
                                                                <asp:Label ID="ViewOutstanding" runat="server" CssClass="colLabel"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <br />
                                                    <div align="center">
                                                        <asp:Button ID="btnExit" CssClass="formsButton" Text="Exit" runat="server" />
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdateProgress ID="upgMain" runat="server" AssociatedUpdatePanelID="uplMain">
                                <ProgressTemplate>
                                    <br />
                                    <img src="../images/progress.gif" alt="Processing" />
                                    <asp:Label ID="lblProgressMain" CssClass="progress" Text="&nbsp;Processing ..." runat="server"></asp:Label>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            <asp:UpdateProgress ID="upgDetail" runat="server" AssociatedUpdatePanelID="uplDetail">
                                <ProgressTemplate>
                                    <br />
                                    <img src="../images/progress.gif" alt="Processing" />
                                    <asp:Label ID="lblProgressDetail" CssClass="progress" Text="&nbsp;Processing ..."
                                        runat="server"></asp:Label>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </ContentTemplate>
                    </act:TabPanel>
                    <act:TabPanel ID="tbpLocate" HeaderText="Locate Receive" runat="server">
                        <HeaderTemplate>
                            Locate Receive
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="uplLocateReceive" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="uplLocateMain" runat="server" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnLocateSave" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnLocateCancel" EventName="Click" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlLocateMain" runat="server">
                                                <table class="tblModule" cellspacing="1">
                                                    <tr>
                                                        <td class="colMod" width="20%">
                                                            Order Reference *
                                                        </td>
                                                        <td class="colDesc" width="30%">
                                                            <asp:DropDownList ID="ddlLocateOrderID" runat="server" CssClass="formsCombo" AutoPostBack="true"
                                                                TabIndex="1">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="colMod" width="20%">
                                                            Receive Date *
                                                        </td>
                                                        <td class="colDesc" width="30%">
                                                            <asp:DropDownList ID="ddlLocateReceiveDate" runat="server" CssClass="formsCombo"
                                                                TabIndex="3">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="errMsg" colspan="4">
                                                            * denotes mandatory fields
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <div id="divLocateSearch" runat="server" align="center">
                                                <br />
                                                <asp:Button ID="btnLocateGo" CssClass="formsButton" Text="Go" runat="server" TabIndex="4" />
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:UpdatePanel ID="uplLocateDetail" runat="server" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnLocateGo" EventName="Click" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlLocate" runat="server" Visible="false">
                                                <br />
                                                <asp:Image ID="imgLocate" runat="server" ImageUrl="~/Images/receive_details.gif"
                                                    AlternateText="Receive Details" />
                                                <br />
                                                <asp:Panel ID="pnlLocateSearch" runat="server" Enabled="false">
                                                    <table class="tblModule" cellspacing="1">
                                                        <tr>
                                                            <td class="colMod" width="20%">
                                                                Order Reference
                                                            </td>
                                                            <td class="colDesc" width="30%">
                                                                <asp:Label ID="lblLocateOrderID" runat="server" Text="" CssClass="colLabel"></asp:Label>
                                                            </td>
                                                            <td class="colMod" width="20%">
                                                                Receive Date (dd/mm/yyyy) *
                                                            </td>
                                                            <td class="colDesc" width="30%">
                                                                <asp:TextBox ID="txtLocateReceiveDate" CssClass="formsText" runat="server" AutoPostBack="true"
                                                                    TabIndex="24"></asp:TextBox>
                                                                <act:CalendarExtender ID="celLocateOrderDate" CssClass="formsCal" runat="server"
                                                                    TargetControlID="txtLocateReceiveDate" Format="dd/MM/yyyy" PopupPosition="BottomLeft">
                                                                </act:CalendarExtender>
                                                                <act:MaskedEditExtender ID="meeLocateOrderDate" runat="server" TargetControlID="txtLocateReceiveDate"
                                                                    Mask="99/99/9999" MaskType="Date">
                                                                </act:MaskedEditExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <asp:Panel ID="pnlLocateAccess" runat="server" Enabled="false">
                                                    <table class="tblModule" cellspacing="1">
                                                        <tr>
                                                            <td class="colMod" width="20%">
                                                                &nbsp;
                                                            </td>
                                                            <td class="colDesc" width="80%" colspan="3">
                                                                <asp:LinkButton ID="btnLocateEdit" runat="server" CssClass="linkButton" OnClientClick="return confirm('Are you sure you want to edit this Receive?');"
                                                                    Text="[Edit Receive]" TabIndex="51"></asp:LinkButton>&nbsp;
                                                                <asp:LinkButton ID="btnLocateDeleteAll" runat="server" CssClass="linkButton" OnClientClick="return confirm('Are you sure you want to delete this Receive?')"
                                                                    Text="[Delete Receive]" TabIndex="52"></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" class="errMsg">
                                                                * denotes mandatory fields
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <asp:Panel ID="pnlLocateSearchItem" runat="server" Enabled="false">
                                                    <dbwc:DynamicControlsPlaceholder ID="DCPLocate" runat="server" ControlsWithoutIDs="DontPersist">
                                                    </dbwc:DynamicControlsPlaceholder>
                                                </asp:Panel>
                                                <asp:Panel ID="pnlLocateAction" runat="server">
                                                    <div align="center">
                                                        <br />
                                                        <asp:Button ID="btnLocateSave" CssClass="formsButton" Text="Save" runat="server"
                                                            OnClientClick="return confirm('Are you sure you want to save the changes?')"
                                                            TabIndex="102" />&nbsp;
                                                        <asp:Button ID="btnLocateCancel" CssClass="formsButton" Text="Cancel" runat="server"
                                                            TabIndex="103" />
                                                    </div>
                                                </asp:Panel>
                                            </asp:Panel>
                                            <asp:Button ID="ShowLocateModal" runat="server" Text="ShowModal" Visible="false"
                                                Height="1px" Width="1px" />
                                            <act:ModalPopupExtender ID="mpuLocateStockAvailability" runat="server" TargetControlID="ShowLocateModal"
                                                PopupControlID="pnlLocateStockAvailability" OkControlID="btnLocateExit" CancelControlID="btnLocateExit"
                                                BackgroundCssClass="modalBackground">
                                            </act:ModalPopupExtender>
                                            <asp:Panel ID="pnlLocateStockAvailability" Style="display: none;" BackColor="White"
                                                runat="server" Width="60%" BorderWidth="1" BorderColor="Gray">
                                                <div style="margin: 10px;">
                                                    <table class="moduleTitle" width="100%" cellspacing="1" cellpadding="1">
                                                        <tr>
                                                            <td class="moduleTitleBorder" align="center">
                                                                Current Stock Availability
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <br />
                                                    <table class="tblModule" cellspacing="1">
                                                        <tr>
                                                            <td class="colMod" width="30%">
                                                                Stock Code
                                                            </td>
                                                            <td class="colRow" width="70%" align="center">
                                                                <asp:Label ID="ViewLocateStockCode" runat="server" CssClass="colLabel"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="colMod" width="30%">
                                                                Description
                                                            </td>
                                                            <td class="colAltRow" width="70%" align="center">
                                                                <asp:Label ID="ViewLocateDesc" runat="server" CssClass="colLabel"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="colHeader" colspan="2" align="center">
                                                                Before Item Receive Update (indicative)
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="colMod" width="30%">
                                                                Available
                                                            </td>
                                                            <td class="colRow" width="70%" align="center">
                                                                <asp:Label ID="ViewLocateBalance" runat="server" CssClass="colLabel"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="colMod" width="30%">
                                                                Avg Unit Cost
                                                            </td>
                                                            <td class="colAltRow" width="70%" align="center">
                                                                <asp:Label ID="ViewLocateAUC" runat="server" CssClass="colLabel"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="colMod" width="30%">
                                                                Total Item Value
                                                            </td>
                                                            <td class="colRow" width="70%" align="center">
                                                                <asp:Label ID="ViewLocateTotalValue" runat="server" CssClass="colLabel"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="colHeader" colspan="2" align="center">
                                                                After Item Receive Update (indicative)
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="colMod" width="30%">
                                                                New Available
                                                            </td>
                                                            <td class="colAltRow" width="70%" align="center">
                                                                <asp:Label ID="ViewLocateBalanceNew" runat="server" CssClass="colLabel"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="colMod" width="30%">
                                                                New Avg Unit Cost
                                                            </td>
                                                            <td class="colRow" width="70%" align="center">
                                                                <asp:Label ID="ViewLocateAUCNew" runat="server" CssClass="colLabel"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="colMod" width="30%">
                                                                New Total Item Value
                                                            </td>
                                                            <td class="colAltRow" width="70%" align="center">
                                                                <asp:Label ID="ViewLocateTotalValueNew" runat="server" CssClass="colLabel"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="colHeader" colspan="2">
                                                                <br />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="colMod" width="30%">
                                                                Order Item Outstanding
                                                            </td>
                                                            <td class="colRow" width="70%" align="center">
                                                                <asp:Label ID="ViewLocateOutstanding" runat="server" CssClass="colLabel"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <br />
                                                    <div align="center">
                                                        <asp:Button ID="btnLocateExit" CssClass="formsButton" Text="Exit" runat="server" />
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdateProgress ID="upgLocateMain" runat="server" AssociatedUpdatePanelID="uplLocateMain">
                                <ProgressTemplate>
                                    <br />
                                    <img src="../images/progress.gif" alt="Processing" />
                                    <asp:Label ID="lblProgressLocateMain" CssClass="progress" Text="&nbsp;Processing ..."
                                        runat="server"></asp:Label></ProgressTemplate>
                            </asp:UpdateProgress>
                            <asp:UpdateProgress ID="upgLocateDetail" runat="server" AssociatedUpdatePanelID="uplLocateDetail">
                                <ProgressTemplate>
                                    <br />
                                    <img src="../images/progress.gif" alt="Processing" />
                                    <asp:Label ID="lblProgressLocateDetail" CssClass="progress" Text="&nbsp;Processing ..."
                                        runat="server"></asp:Label></ProgressTemplate>
                            </asp:UpdateProgress>
                        </ContentTemplate>
                    </act:TabPanel>
                    <act:TabPanel ID="tbpPrint" HeaderText="Receive Report" runat="server">
                        <ContentTemplate>
                            <asp:UpdatePanel ID="uplPrintReceive" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnPDF" />
                                    <asp:PostBackTrigger ControlID="btnExcel" />
                                    <asp:PostBackTrigger ControlID="btnClear" />
                                </Triggers>
                                <ContentTemplate>
                                    <table class="tblModule" cellspacing="1">
                                        <tr>
                                            <td class="colMod" width="20%">
                                                Receive Date From *<br />
                                                (dd/mm/yyyy)
                                            </td>
                                            <td class="colDesc" width="30%">
                                                <asp:TextBox ID="txtReceiveDateFrom" runat="server" CssClass="formsText" TabIndex="11"></asp:TextBox>
                                                <act:CalendarExtender ID="CalendarExtender3" runat="server" CssClass="formsCal" Format="dd/MM/yyyy"
                                                    PopupPosition="BottomLeft" TargetControlID="txtReceiveDateFrom">
                                                </act:CalendarExtender>
                                                <act:MaskedEditExtender ID="MaskedEditExtender3" runat="server" Mask="99/99/9999"
                                                    MaskType="Date" TargetControlID="txtReceiveDateFrom">
                                                </act:MaskedEditExtender>
                                            </td>
                                            <td class="colMod" width="20%">
                                                Receive Date To *<br />
                                                (dd/mm/yyyy)
                                            </td>
                                            <td class="colDesc" width="30%">
                                                <asp:TextBox ID="txtReceiveDateTo" runat="server" CssClass="formsText" TabIndex="12"></asp:TextBox>
                                                <act:CalendarExtender ID="CalendarExtender4" runat="server" CssClass="formsCal" Format="dd/MM/yyyy"
                                                    PopupPosition="BottomLeft" TargetControlID="txtReceiveDateTo">
                                                </act:CalendarExtender>
                                                <act:MaskedEditExtender ID="MaskedEditExtender4" runat="server" Mask="99/99/9999"
                                                    MaskType="Date" TargetControlID="txtReceiveDateTo">
                                                </act:MaskedEditExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="colMod" width="20%">
                                                Stock Code From *
                                            </td>
                                            <td class="colDesc" width="80%" colspan="3">
                                                <asp:TextBox ID="txtStockCodeFrom" runat="server" CssClass="formsTextNumLarge"></asp:TextBox>
                                                <act:AutoCompleteExtender ID="aceStockCodeFrom" runat="server" MinimumPrefixLength="2" CompletionSetCount="20" TargetControlID="txtStockCodeFrom"
                                                ServiceMethod="GetStockItems" UseContextKey="true"></act:AutoCompleteExtender>
                                            </td>
                                       </tr>
                                       <tr>
                                            <td class="colMod" width="20%">
                                                Stock Code To *
                                            </td>
                                            <td class="colDesc" width="80%" colspan="3">
                                                <asp:TextBox ID="txtStockCodeTo" runat="server" CssClass="formsTextNumLarge"></asp:TextBox>
                                                <act:AutoCompleteExtender ID="aceStockCodeTo" runat="server" MinimumPrefixLength="2" CompletionSetCount="20" TargetControlID="txtStockCodeTo"
                                                ServiceMethod="GetStockItems" UseContextKey="true"></act:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:UpdatePanel ID="uplPrintOrderID" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table class="tblModule" cellspacing="1">
                                                <tr>
                                                    <td class="colMod" width="20%">
                                                        Order Reference #
                                                    </td>
                                                    <td class="colDesc" width="80%" colspan="3">
                                                        <asp:DropDownList ID="ddlOrderReference" runat="server" CssClass="formsCombo" TabIndex="15">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" class="errMsg">
                                                        * denotes mandatory fields ; # When this is selected, other selection will be passed
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <br />
                                    <div align="center">
                                        <asp:Button ID="btnPDF" CssClass="formsButton" Text="Export to PDF" runat="server"
                                            Width="100px" TabIndex="21" />&nbsp;
                                        <asp:Button ID="btnExcel" CssClass="formsButton" Text="Export to Excel" runat="server"
                                            Width="100px" TabIndex="22" />&nbsp;
                                        <asp:Button ID="btnClear" CssClass="formsButton" Text="Clear" runat="server"
                                            TabIndex="23" />
                                    </div>
                                    <rsweb:ReportViewer ID="rvr" runat="server" Font-Size="8pt" Height="400px" Visible="False"
                                        Width="100%" Font-Names="Verdana">
                                        <LocalReport ReportPath="StockControl\rptRecieveList.rdlc">
                                            <DataSources>
                                                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="ReceiveList" />
                                            </DataSources>
                                        </LocalReport>
                                    </rsweb:ReportViewer>
                                    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetReceiveList"
                                        TypeName="NEA_ICS.UserInterface.NEA_ICS.WcfService.ServiceClient">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="" Name="storeID" Type="String" />
                                            <asp:Parameter DefaultValue="" Name="fromDte" Type="DateTime" />
                                            <asp:Parameter DefaultValue="" Name="toDte" Type="DateTime" />
                                            <asp:Parameter DefaultValue="" Name="fromStockItemID" Type="String" />
                                            <asp:Parameter DefaultValue="" Name="toStockItemID" Type="String" />
                                            <asp:Parameter DefaultValue="" Name="orderId" Type="String" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                    <br />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </act:TabPanel>
                </act:TabContainer>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
