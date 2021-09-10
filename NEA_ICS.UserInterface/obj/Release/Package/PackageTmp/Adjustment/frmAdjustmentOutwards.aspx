<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmAdjustmentOutwards.aspx.vb"
    Inherits="NEA_ICS.UserInterface.frmAdjustmentOutwards" ValidateRequest="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="DBauer.Web.UI.WebControls.DynamicControlsPlaceholder" Namespace="DBauer.Web.UI.WebControls"
    TagPrefix="dbwc" %>
<%@ Register Src="AdjustItem.ascx" TagName="AdjustItem" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Inventory Control System</title>
    <link href="../style/ICS.css" type="text/css" rel="Stylesheet" />
    <meta http-equiv='refresh' content='1800;url=..\frmUnauthorisedPage.aspx' />

    <script src="../Script/NEA_ICS.js" language="javascript" type="text/javascript">
        function pageLoad() {
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
                    &nbsp;Stock Item Adjustment > Adjustment Outwards
                </td>
            </tr>
        </table>
        <br />
        <asp:UpdatePanel ID="uplPage" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <act:TabContainer ID="tbcAdjustItem" Width="98%" Font-Bold="true" Font-Size="Medium"
                    runat="server" ForeColor="#4D36C2" BackColor="#FFFFFF" Font-Names="Verdana" ActiveTabIndex="0">
                    <act:TabPanel ID="tbpNew" HeaderText="New Adjustment Outwards" runat="server">
                        <ContentTemplate>
                            <asp:UpdatePanel ID="uplNewAdjust" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlMain" runat="server">
                                        <asp:UpdatePanel ID="uplMain" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table class="tblModule" cellspacing="1">
                                                    <tr>
                                                        <td class="colMod" width="20%">
                                                            Document No #
                                                        </td>
                                                        <td class="colDesc" width="30%">
                                                            <asp:TextBox ID="txtAdjustID" CssClass="formsText" runat="server" MaxLength="12"
                                                                TabIndex="11"></asp:TextBox>
                                                        </td>
                                                        <td class="colMod" width="20%">
                                                            Document Type *
                                                        </td>
                                                        <td class="colDesc" width="30%">
                                                            <asp:DropDownList ID="ddlDocType" runat="server" CssClass="formsCombo" TabIndex="14" />
                                                        </td>
                                                    </tr>
                                                    <tr id="trSerial" runat="server">
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
                                                            Adjustment Date
                                                            <br />
                                                            (dd/mm/yyyy) *
                                                        </td>
                                                        <td class="colDesc" width="80%" colspan="3">
                                                            <asp:TextBox ID="txtAdjustDte" CssClass="formsText" runat="server"></asp:TextBox>
                                                            <act:CalendarExtender ID="calVDateStart" CssClass="formsCal" runat="server" TargetControlID="txtAdjustDte"
                                                                Format="dd/MM/yyyy" PopupPosition="BottomLeft">
                                                            </act:CalendarExtender>
                                                            <act:MaskedEditExtender ID="meeVDateStart" runat="server" TargetControlID="txtAdjustDte"
                                                                Mask="99/99/9999" MaskType="Date">
                                                            </act:MaskedEditExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4" class="errMsg">
                                                            * denotes mandatory fields; # system will generates a unique number if field left
                                                            blank;
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <div id="divAddButton" runat="server" align="center">
                                                    <asp:Button ID="btnAddAdjustItem" CssClass="formsButtonLarge" Text="Add Adjustment"
                                                        runat="server" TabIndex="21" />&nbsp;
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <asp:Panel ID="pnlNewAdjust" runat="server" Visible="false">
                                            <asp:Image ID="imgAdjustItem" runat="server" AlternateText="Adjust Details" ImageUrl="~/Images/adjustment_outwards_details.gif" />
                                            <div class="errMsg">
                                                * denotes mandatory fields (*<sup>1</sup> for all Adjustment except Price Adjustment;
                                                *<sup>2</sup> for Price Adjustment)
                                            </div>
                                            <asp:UpdatePanel ID="uplStockCode" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table cellspacing="1" class="tblModule">
                                                        <tr>
                                                            <td class="colMod" width="20%">
                                                                Enter a Stock Code for your Adjustment Item
                                                            </td>
                                                            <td class="colDesc" width="80%">
                                                                <asp:TextBox ID="txtStockCode" runat="server" CssClass="formsTextNumLarge"></asp:TextBox>
                                                                &#160;&#160;&#160;
                                                                <asp:LinkButton ID="btnAddItem" runat="server" CssClass="linkButton" TabIndex="32"
                                                                    Text="[Add Adjustment Item]"></asp:LinkButton>
                                                                <act:AutoCompleteExtender ID="aceStockCode" runat="server" MinimumPrefixLength="2" CompletionSetCount="20" TargetControlID="txtStockCode"
                                                                ServiceMethod="GetStockItems" UseContextKey="true"></act:AutoCompleteExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:UpdatePanel ID="uplUserControl" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnSubmit" />
                                                    <asp:PostBackTrigger ControlID="btnCancelAll" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAddItem" EventName="Click" />
                                                </Triggers>
                                                <ContentTemplate>
                                                    <dbwc:DynamicControlsPlaceholder ID="DCP" runat="server" ControlsWithoutIDs="DontPersist">
                                                    </dbwc:DynamicControlsPlaceholder>
                                                    <br />
                                                    <div align="center">
                                                        <asp:Button ID="btnSubmit" runat="server" CssClass="formsButton" OnClientClick="return confirm('Are you sure you want to save your Adjust?')"
                                                            TabIndex="7" Text="Save" />&#160;&#160;&#160;
                                                        <asp:Button ID="btnCancelAll" runat="server" CssClass="formsButton" OnClientClick="return confirm('Are you sure you want to cancel your action?')"
                                                            TabIndex="8" Text="Cancel" />
                                                    </div>
                                                    <asp:Button ID="ShowModal" runat="server" Height="1px" Text="ShowModal" Visible="false"
                                                        Width="1px" />
                                                    <act:ModalPopupExtender ID="mpuStockAvailability" runat="server" BackgroundCssClass="modalBackground"
                                                        CancelControlID="btnExit" OkControlID="btnExit" PopupControlID="pnlStockAvailability"
                                                        TargetControlID="ShowModal">
                                                    </act:ModalPopupExtender>
                                                    <asp:Panel ID="pnlStockAvailability" runat="server" BackColor="White" BAdjustColor="Gray"
                                                        BAdjustWidth="1" Style="display: none;" Width="60%">
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
                                                                        Before Item Adjustment (indicative)
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
                                                                        After Item Adjustment (indicative)
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
                                                            </table>
                                                            <br />
                                                            <div align="center">
                                                                <asp:Button ID="btnExit" runat="server" CssClass="formsButton" OnClick="btnExit_Click"
                                                                    Text="Exit" />
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </asp:Panel>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdateProgress ID="upgMain" runat="server" AssociatedUpdatePanelID="uplMain">
                                <ProgressTemplate>
                                    <br />
                                    <img src="../images/progress.gif" alt="Processing" />
                                    <asp:Label ID="lblProgress" runat="server" CssClass="progress" Text="&nbsp;Processing ..." />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            <asp:UpdateProgress ID="upgStockCode" runat="server" AssociatedUpdatePanelID="uplStockCode">
                                <ProgressTemplate>
                                    <br />
                                    <img src="../images/progress.gif" alt="Processing" />
                                    <asp:Label ID="lblProgressStockCode" runat="server" CssClass="progress" Text="&nbsp;Processing ..." />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            <asp:UpdateProgress ID="upgUserControl" runat="server" AssociatedUpdatePanelID="uplUserControl">
                                <ProgressTemplate>
                                    <br />
                                    <img src="../images/progress.gif" alt="Processing" />
                                    <asp:Label ID="lblProgressUserControl" runat="server" CssClass="progress" Text="&nbsp;Processing ..." />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </ContentTemplate>
                    </act:TabPanel>
                    <act:TabPanel ID="tbpLocate" HeaderText="Locate Adjustment Outwards" runat="server">
                        <HeaderTemplate>
                            Locate Adjustment
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="uplLocateAdjust" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlLocateAdjustID" EventName="SelectedIndexChanged" />
                                    <asp:PostBackTrigger ControlID="btnLocateClear" />
                                    <asp:PostBackTrigger ControlID="btnLocateSave" />
                                    <asp:PostBackTrigger ControlID="btnLocateCancel" />
                                    <asp:PostBackTrigger ControlID="btnLocateDeleteAll" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="uplLocateMain" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table class="tblModule" cellspacing="1">
                                                <tr>
                                                    <td class="colMod" width="20%">
                                                        Document No *
                                                    </td>
                                                    <td class="colDesc" width="80%">
                                                        <asp:DropDownList ID="ddlLocateAdjustID" runat="server" CssClass="formsCombo" TabIndex="1">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="errMsg" colspan="2">
                                                        * denotes mandatory fields
                                                    </td>
                                                </tr>
                                            </table>
                                            <div align="center">
                                                <asp:Button ID="btnLocateGo" CssClass="formsButton" Text="Go" runat="server" />&nbsp;
                                                <asp:Button ID="btnLocateClear" CssClass="formsButton" Text="Clear" runat="server"
                                                    OnClientClick="return confirm('Are you sure you want to clear your entry?')" />
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:Panel ID="pnlLocate" runat="server" Visible="false">
                                        <asp:Image ID="imgAdjustDetails" runat="server" ImageUrl="~/Images/Adjustment_outwards_details.gif"
                                            AlternateText="Adjustment Details" />
                                        <br />
                                        <asp:UpdatePanel ID="uplLocateMainAdjust" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Panel ID="pnlLocateAdjust" runat="server">
                                                    <table class="tblModule" cellspacing="1">
                                                        <tr>
                                                            <td class="colMod" width="20%">
                                                                Document No
                                                            </td>
                                                            <td class="colDesc" width="30%">
                                                                <asp:Label ID="lblLocateAdjustID" runat="server" CssClass="colLabel"></asp:Label>
                                                            </td>
                                                            <td class="colMod" width="20%">
                                                                Document Type *
                                                            </td>
                                                            <td class="colDesc" width="30%">
                                                                <asp:Label ID="lblLocateDocType" runat="server" CssClass="colLabel"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr id="trLocateSerialNo" runat="server">
                                                            <td class="colMod" width="20%">
                                                                Serial No
                                                            </td>
                                                            <td class="colDesc" width="30%" colspan="1">
                                                                <asp:TextBox ID="txtLocateSerialNo" CssClass="formsText" runat="server" MaxLength="9"></asp:TextBox>
                                                            </td>
                                                            <td class="colMod" width="20%">
                                                                Last Serial No
                                                            </td>
                                                            <td class="colDesc" width="30%" valign="middle">
                                                                <asp:Label ID="lblLocateLastSerialNo" runat="server" CssClass="colLabel"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="colMod" width="20%">
                                                                Adjustment Date
                                                                <br />
                                                                (dd/mm/yyyy) *
                                                            </td>
                                                            <td class="colDesc" width="80%" colspan="3">
                                                                <asp:TextBox ID="txtLocateAdjustDte" CssClass="formsText" runat="server"></asp:TextBox>
                                                                <act:CalendarExtender ID="ceLocateAdjustDte" CssClass="formsCal" runat="server" TargetControlID="txtLocateAdjustDte"
                                                                    Format="dd/MM/yyyy" PopupPosition="BottomLeft">
                                                                </act:CalendarExtender>
                                                                <act:MaskedEditExtender ID="meeLocateAdjustDte" runat="server" TargetControlID="txtLocateAdjustDte"
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
                                                                <asp:LinkButton ID="btnLocateEdit" runat="server" CssClass="linkButton" OnClientClick="return confirm('Are you sure you want to edit this Adjustment?')"
                                                                    Text="[Edit Adjustment]" TabIndex="26" />&nbsp;&nbsp;&nbsp;
                                                                <asp:LinkButton ID="btnLocateDeleteAll" runat="server" CssClass="linkButton" OnClientClick="return confirm('Are you sure you want to delete this Adjustment?')"
                                                                    Text="[Delete Adjustment]" TabIndex="27"></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" class="errMsg">
                                                                * denotes mandatory fields
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <asp:UpdatePanel ID="uplLocateStockCode" runat="server" ChildrenAsTriggers="false"
                                            UpdateMode="Conditional" Visible="false">
                                            <ContentTemplate>
                                                <asp:Panel ID="pnlStockCode" runat="server">
                                                    <table cellspacing="1" class="tblModule">
                                                        <tr>
                                                            <td class="colMod" width="20%">
                                                                Enter a Stock Code for your Adjustment Item
                                                            </td>
                                                            <td class="colDesc" width="80%">
                                                                <asp:TextBox ID="txtLocateStockCode" runat="server" CssClass="formsTextNumLarge"></asp:TextBox>
                                                                &#160;&#160;&#160;
                                                                <asp:LinkButton ID="btnLocateAddItem" runat="server" CssClass="linkButton" TabIndex="32"
                                                                    Text="[Add Adjustment Item]">
                                                                </asp:LinkButton>
                                                                <act:AutoCompleteExtender ID="aceLocateStockCode" runat="server" MinimumPrefixLength="2" CompletionSetCount="20" 
                                                                TargetControlID="txtLocateStockCode"
                                                                ServiceMethod="GetStockItems" UseContextKey="true"></act:AutoCompleteExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" class="errMsg">
                                                                * denotes mandatory fields (*<sup>1</sup> for all Adjustment except Price Adjustment;
                                                                *<sup>2</sup> for Price Adjustment)
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <asp:UpdatePanel ID="uplLocateUserControl" runat="server" UpdateMode="Conditional"
                                            ChildrenAsTriggers="false">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnLocateAddItem" EventName="Click" />
                                                <asp:PostBackTrigger ControlID="btnLocateGo" />
                                                <%--                                                <asp:AsyncPostBackTrigger ControlID="btnLocateGo" EventName="Click" />
--%>
                                            </Triggers>
                                            <ContentTemplate>
                                                <asp:Panel ID="pnlLocateAdjustItem" runat="server" Enabled="false">
                                                    <dbwc:DynamicControlsPlaceholder ID="DCPLocate" runat="server" ControlsWithoutIDs="DontPersist">
                                                    </dbwc:DynamicControlsPlaceholder>
                                                </asp:Panel>
                                                <asp:Panel ID="pnlLocateAction" runat="server" Visible="false">
                                                    <div align="center">
                                                        <asp:Button ID="btnLocateSave" CssClass="formsButton" Text="Save" runat="server"
                                                            OnClientClick="return confirm('Are you sure you want to save the changes?')"
                                                            TabIndex="102" />&nbsp;&#160;
                                                        <asp:Button ID="btnLocateCancel" CssClass="formsButton" Text="Cancel" runat="server"
                                                            OnClientClick="return confirm('Are you sure you want to cancel your action?')"
                                                            TabIndex="103" />
                                                    </div>
                                                </asp:Panel>
                                                <asp:Button ID="ShowLocateModal" runat="server" Text="ShowModal" Visible="false"
                                                    Height="1px" Width="1px" />
                                                <act:ModalPopupExtender ID="mpuLocateStockAvailability" runat="server" TargetControlID="ShowLocateModal"
                                                    PopupControlID="pnlLocateStockAvailability" OkControlID="btnLocateExit" CancelControlID="btnLocateExit"
                                                    BackgroundCssClass="modalBackground">
                                                </act:ModalPopupExtender>
                                                <asp:Panel ID="pnlLocateStockAvailability" Style="display: none;" BackColor="White"
                                                    runat="server" Width="60%" BAdjustWidth="1" BAdjustColor="Gray">
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
                                                                    Before Item Adjustment Update (indicative)
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
                                                                    After Item Adjustment Update (indicative)
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
                                                        </table>
                                                        <br />
                                                        <div align="center">
                                                            <asp:Button ID="btnLocateExit" runat="server" CssClass="formsButton" Text="Exit"
                                                                OnClick="btnLocateExit_Click" />
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdateProgress ID="upgLocateAdjust" runat="server" AssociatedUpdatePanelID="uplLocateAdjust">
                                <ProgressTemplate>
                                    <br />
                                    <img alt="Processing" src="../images/progress.gif" />
                                    <asp:Label ID="lblProgress" runat="server" CssClass="progress" Text="&nbsp;Processing ..."></asp:Label></ProgressTemplate>
                            </asp:UpdateProgress>
                            <asp:UpdateProgress ID="upgLocateStockCode" runat="server" AssociatedUpdatePanelID="uplLocateStockCode">
                                <ProgressTemplate>
                                    <br />
                                    <img alt="Processing" src="../images/progress.gif" />
                                    <asp:Label ID="lblProgressStockCode" runat="server" CssClass="progress" Text="&nbsp;Processing ...">
                                    </asp:Label></ProgressTemplate>
                            </asp:UpdateProgress>
                            <asp:UpdateProgress ID="upgLocateUserControl" runat="server" AssociatedUpdatePanelID="uplLocateUserControl">
                                <ProgressTemplate>
                                    <br />
                                    <img alt="Processing" src="../images/progress.gif" />
                                    <asp:Label ID="lblProgressUserControl" runat="server" CssClass="progress" Text="&nbsp;Processing ...">
                                    </asp:Label></ProgressTemplate>
                            </asp:UpdateProgress>
                        </ContentTemplate>
                    </act:TabPanel>
                    <act:TabPanel ID="tbpPrint" HeaderText="Adjustment Outwards Report" runat="server">
                        <ContentTemplate>
                            <asp:UpdatePanel ID="uplPrintAdjust" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table class="tblModule" cellspacing="1">
                                        <tr>
                                            <td class="colMod" width="20%">
                                                Document No #
                                            </td>
                                            <td class="colDesc" width="80%" colspan="3">
                                                <asp:DropDownList ID="ddlRptDocumentNo" runat="server" CssClass="formsCombo" TabIndex="11" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="colMod" width="20%">
                                                Date Adjusted From (dd/mm/yyyy) *
                                            </td>
                                            <td class="colDesc" width="30%">
                                                <asp:TextBox ID="txtDateFrom" runat="server" CssClass="formsText" TabIndex="12"></asp:TextBox>
                                                <act:CalendarExtender ID="CalendarExtender3" runat="server" CssClass="formsCal" Format="dd/MM/yyyy"
                                                    PopupPosition="BottomLeft" TargetControlID="txtDateFrom">
                                                </act:CalendarExtender>
                                                <act:MaskedEditExtender ID="MaskedEditExtender3" runat="server" Mask="99/99/9999"
                                                    MaskType="Date" TargetControlID="txtDateFrom">
                                                </act:MaskedEditExtender>
                                            </td>
                                            <td class="colMod" width="20%">
                                                Date Adjusted To (dd/mm/yyyy) *
                                            </td>
                                            <td class="colDesc" width="30%">
                                                <asp:TextBox ID="txtDateTo" runat="server" CssClass="formsText" TabIndex="13"></asp:TextBox>
                                                <act:CalendarExtender ID="CalendarExtender4" runat="server" CssClass="formsCal" Format="dd/MM/yyyy"
                                                    PopupPosition="BottomLeft" TargetControlID="txtDateTo">
                                                </act:CalendarExtender>
                                                <act:MaskedEditExtender ID="MaskedEditExtender4" runat="server" Mask="99/99/9999"
                                                    MaskType="Date" TargetControlID="txtDateTo">
                                                </act:MaskedEditExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" class="errMsg">
                                                * denotes mandatory fields ; # denotes if selected other selection will be passed
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <div align="center">
                                        <asp:Button ID="btnPDF" CssClass="formsButton" Text="Export to PDF" runat="server"
                                            Width="100px" TabIndex="21" />&nbsp;
                                        <asp:Button ID="btnExcel" CssClass="formsButton" Text="Export to Excel" runat="server"
                                            Width="100px" TabIndex="22" />&nbsp;
                                        <asp:Button ID="btnClear" CssClass="formsButton" Text="Clear" runat="server" TabIndex="23" />
                                    </div>
                                    <rsweb:ReportViewer ID="rvr" runat="server" Font-Names="Verdana" Font-Size="8pt"
                                        Height="400px" Width="400px" Visible="False">
                                        <LocalReport ReportPath="Adjustment\rptAdjustList.rdlc">
                                            <DataSources>
                                                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="AdjustList" />
                                            </DataSources>
                                        </LocalReport>
                                    </rsweb:ReportViewer>
                                    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetAdjustList"
                                        TypeName="NEA_ICS.UserInterface.NEA_ICS.WcfService.ServiceClient">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="" Name="storeID" Type="String" />
                                            <asp:Parameter DefaultValue="" Name="type" Type="String" />
                                            <asp:Parameter DefaultValue="" Name="fromDte" Type="DateTime" />
                                            <asp:Parameter DefaultValue="" Name="toDte" Type="DateTime" />
                                            <asp:Parameter DefaultValue="" Name="AdjustID" Type="String" />
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
