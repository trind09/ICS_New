﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmAdjustmentInwards.aspx.vb"
    Inherits="NEA_ICS.UserInterface.frmAdjustmentInwards" ValidateRequest="true" %>

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
                    &nbsp;Stock Item Adjustment > Adjustment Inwards
                </td>
            </tr>
        </table>
        <br />
        <asp:UpdatePanel ID="uplPage" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <act:TabContainer ID="tbcAdjustItem" Width="98%" Font-Bold="true" Font-Size="Medium"
                    runat="server" ForeColor="#4D36C2" BackColor="#FFFFFF" Font-Names="Verdana" ActiveTabIndex="0">
                    <act:TabPanel ID="tbpNew" runat="server" HeaderText="New Adjustment Inwards">
                        <ContentTemplate>
                            <asp:UpdatePanel ID="uplNewAdjust" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlMain" runat="server">
                                        <asp:UpdatePanel ID="uplMain" runat="server" UpdateMode="Conditional">
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="ddlDocType" />
                                                <asp:PostBackTrigger ControlID="ddlRequestID" />
                                                <asp:PostBackTrigger ControlID="btnAddAdjustItem" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <table cellspacing="1" class="tblModule">
                                                    <tr>
                                                        <td class="colMod" width="20%">
                                                            Document No #
                                                        </td>
                                                        <td class="colDesc" width="30%">
                                                            <asp:TextBox ID="txtAdjustID" runat="server" CssClass="formsText" MaxLength="12"
                                                                TabIndex="11"></asp:TextBox>
                                                        </td>
                                                        <td class="colMod" width="20%">
                                                            Document Type *
                                                        </td>
                                                        <td class="colDesc" width="30%">
                                                            <asp:DropDownList ID="ddlDocType" runat="server" CssClass="formsCombo" TabIndex="12"
                                                                AutoPostBack="true" />
                                                        </td>
                                                    </tr>
                                                    <tr id="trSerial" runat="server" visible="false">
                                                        <td class="colMod" width="20%">
                                                            Serial No
                                                        </td>
                                                        <td class="colDesc" colspan="1" width="30%">
                                                            <asp:TextBox ID="txtSerialNo" runat="server" CssClass="formsText" MaxLength="12" TabIndex="13"></asp:TextBox>
                                                        </td>
                                                        <td class="colMod" width="20%">
                                                            Last Serial No
                                                        </td>
                                                        <td class="colDesc" valign="middle" width="30%">
                                                            <asp:Label ID="lblLastSerialNo" runat="server" CssClass="colLabel"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr id="trRequestID" runat="server" visible="false">
                                                        <td class="colMod" width="20%">
                                                            Issue Reference 
                                                        </td>
                                                        <td class="colDesc" width="30%">
                                                            <asp:DropDownList ID="ddlRequestID" runat="server" CssClass="formsCombo" AutoPostBack="true"
                                                                TabIndex="14" />
                                                        </td>
                                                        <td class="colMod" width="20%">
                                                            Consumer Code
                                                        </td>
                                                        <td class="colDesc" valign="middle" width="30%">
                                                            <asp:Label ID="lblConsumerID" runat="server" CssClass="colLabel"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="colMod" width="20%">
                                                            Adjustment Date
                                                            <br />
                                                            (dd/mm/yyyy) *
                                                        </td>
                                                        <td class="colDesc" colspan="3" width="80%">
                                                            <asp:TextBox ID="txtAdjustDte" runat="server" CssClass="formsText" TabIndex="15"></asp:TextBox>
                                                            <act:CalendarExtender ID="calVDateStart" runat="server" CssClass="formsCal" Format="dd/MM/yyyy"
                                                                PopupPosition="BottomLeft" TargetControlID="txtAdjustDte">
                                                            </act:CalendarExtender>
                                                            <act:MaskedEditExtender ID="meeVDateStart" runat="server" Mask="99/99/9999" MaskType="Date"
                                                                TargetControlID="txtAdjustDte">
                                                            </act:MaskedEditExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="errMsg" colspan="4">
                                                            * denotes mandatory fields; # system will generates a unique number if field left
                                                            blank;
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <div id="divAddButton" runat="server" align="center">
                                                    <asp:Button ID="btnAddAdjustItem" runat="server" CssClass="formsButtonLarge" TabIndex="21"
                                                        Text="Add Adjustment" />&nbsp;
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <asp:Panel ID="pnlNewAdjust" runat="server" Visible="false">
                                            <asp:Image ID="imgAdjustItem" runat="server" AlternateText="Adjust Details" ImageUrl="~/Images/adjustment_inwards_details.gif" />
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
                                                            <table cellpadding="1" cellspacing="1" class="moduleTitle" width="100%">
                                                                <tr>
                                                                    <td align="center" class="moduleTitleBorder">
                                                                        Current Stock Availability
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <br />
                                                            <table cellspacing="1" class="tblModule">
                                                                <tr>
                                                                    <td class="colMod" width="30%">
                                                                        Stock Code
                                                                    </td>
                                                                    <td align="center" class="colRow" width="70%">
                                                                        <asp:Label ID="ViewStockCode" runat="server" CssClass="colLabel"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="colMod" width="30%">
                                                                        Description
                                                                    </td>
                                                                    <td align="center" class="colAltRow" width="70%">
                                                                        <asp:Label ID="ViewDesc" runat="server" CssClass="colLabel"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center" class="colHeader" colspan="2">
                                                                        Before Item Adjustment (indicative)
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="colMod" width="30%">
                                                                        Available
                                                                    </td>
                                                                    <td align="center" class="colRow" width="70%">
                                                                        <asp:Label ID="ViewBalance" runat="server" CssClass="colLabel"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="colMod" width="30%">
                                                                        Avg Unit Cost
                                                                    </td>
                                                                    <td align="center" class="colAltRow" width="70%">
                                                                        <asp:Label ID="ViewAUC" runat="server" CssClass="colLabel"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="colMod" width="30%">
                                                                        Total Item Value
                                                                    </td>
                                                                    <td align="center" class="colRow" width="70%">
                                                                        <asp:Label ID="ViewTotalValue" runat="server" CssClass="colLabel"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center" class="colHeader" colspan="2">
                                                                        After Item Adjustment (indicative)
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="colMod" width="30%">
                                                                        New Available
                                                                    </td>
                                                                    <td align="center" class="colAltRow" width="70%">
                                                                        <asp:Label ID="ViewBalanceNew" runat="server" CssClass="colLabel"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="colMod" width="30%">
                                                                        New Avg Unit Cost
                                                                    </td>
                                                                    <td align="center" class="colRow" width="70%">
                                                                        <asp:Label ID="ViewAUCNew" runat="server" CssClass="colLabel"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="colMod" width="30%">
                                                                        New Total Item Value
                                                                    </td>
                                                                    <td align="center" class="colAltRow" width="70%">
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
                                    <img alt="Processing" src="../images/progress.gif" />
                                    <asp:Label ID="lblProgressMain" runat="server" CssClass="progress" Text="&nbsp;Processing ...">
                                    </asp:Label>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            <asp:UpdateProgress ID="upgStockCode" runat="server" AssociatedUpdatePanelID="uplStockCode">
                                <ProgressTemplate>
                                    <br />
                                    <img alt="Processing" src="../images/progress.gif" />
                                    <asp:Label ID="lblProgressStockCode" runat="server" CssClass="progress" Text="&nbsp;Processing ...">
                                    </asp:Label>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            <asp:UpdateProgress ID="upgUserControl" runat="server" AssociatedUpdatePanelID="uplUserControl">
                                <ProgressTemplate>
                                    <br />
                                    <img alt="Processing" src="../images/progress.gif" />
                                    <asp:Label ID="lblProgressUserControl" runat="server" CssClass="progress" Text="&nbsp;Processing ...">
                                    </asp:Label>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </ContentTemplate>
                    </act:TabPanel>
                    <act:TabPanel ID="tbpLocate" runat="server" HeaderText="Locate Adjustment Inwards">
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
                                            <table cellspacing="1" class="tblModule">
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
                                                <asp:Button ID="btnLocateGo" runat="server" CssClass="formsButton" Text="Go" />&nbsp;
                                                <asp:Button ID="btnLocateClear" runat="server" CssClass="formsButton" OnClientClick="return confirm('Are you sure you want to clear your entry?')"
                                                    Text="Clear" />
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:Panel ID="pnlLocate" runat="server" Visible="false">
                                        <asp:Image ID="imgAdjustDetails" runat="server" AlternateText="Adjustment Details"
                                            ImageUrl="~/Images/Adjustment_inwards_details.gif" />
                                        <br />
                                        <asp:UpdatePanel ID="uplLocateMainAdjust" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Panel ID="pnlLocateAdjust" runat="server">
                                                    <table cellspacing="1" class="tblModule">
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
                                                        <tr id="trLocateSerialNo" runat="server" visible="false">
                                                            <td class="colMod" width="20%">
                                                                Serial No
                                                            </td>
                                                            <td class="colDesc" colspan="1" width="30%">
                                                                <asp:TextBox ID="txtLocateSerialNo" runat="server" CssClass="formsText" MaxLength="9"></asp:TextBox>
                                                            </td>
                                                            <td class="colMod" width="20%">
                                                                Last Serial No
                                                            </td>
                                                            <td class="colDesc" valign="middle" width="30%">
                                                                <asp:Label ID="lblLocateLastSerialNo" runat="server" CssClass="colLabel"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr id="trLocateRequestID" runat="server" visible="false">
                                                            <td class="colMod" width="20%">
                                                                Issue Reference
                                                            </td>
                                                            <td class="colDesc" valign="middle" width="30%">
                                                                <asp:Label ID="lblLocateRequestID" runat="server" CssClass="colLabel"></asp:Label>
                                                            </td>
                                                            <td class="colMod" width="20%">
                                                                Consumer Code
                                                            </td>
                                                            <td class="colDesc" valign="middle" width="30%">
                                                                <asp:Label ID="lblLocateConsumerID" runat="server" CssClass="colLabel"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="colMod" width="20%">
                                                                Adjustment Date
                                                                <br />
                                                                (dd/mm/yyyy) *
                                                            </td>
                                                            <td class="colDesc" colspan="3" width="80%">
                                                                <asp:TextBox ID="txtLocateAdjustDte" runat="server" CssClass="formsText"></asp:TextBox>
                                                                <act:CalendarExtender ID="ceLocateAdjustDte" runat="server" CssClass="formsCal" Format="dd/MM/yyyy"
                                                                    PopupPosition="BottomLeft" TargetControlID="txtLocateAdjustDte">
                                                                </act:CalendarExtender>
                                                                <act:MaskedEditExtender ID="meeLocateAdjustDte" runat="server" Mask="99/99/9999"
                                                                    MaskType="Date" TargetControlID="txtLocateAdjustDte">
                                                                </act:MaskedEditExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <asp:Panel ID="pnlLocateAccess" runat="server" Enabled="false">
                                                    <table cellspacing="1" class="tblModule">
                                                        <tr>
                                                            <td class="colMod" width="20%">
                                                                &nbsp;
                                                            </td>
                                                            <td class="colDesc" colspan="3" width="80%">
                                                                <asp:LinkButton ID="btnLocateEdit" runat="server" CssClass="linkButton" OnClientClick="return confirm('Are you sure you want to edit this Adjustment?')"
                                                                    TabIndex="26" Text="[Edit Adjustment]">
                                                                </asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                                <asp:LinkButton ID="btnLocateDeleteAll" runat="server" CssClass="linkButton" OnClientClick="return confirm('Are you sure you want to delete this Adjustment?')"
                                                                    TabIndex="27" Text="[Delete Adjustment]"></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="errMsg" colspan="4">
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
                                                                <asp:LinkButton ID="btnLocateAddItem" runat="server" CssClass="linkButton"
                                                                    Text="[Add Adjustment Item]">                                                       
                                                                </asp:LinkButton>
                                                                <act:AutoCompleteExtender ID="aceLocateStockCode" runat="server" MinimumPrefixLength="2" CompletionSetCount="20" 
                                                                TargetControlID="txtLocateStockCode"
                                                                ServiceMethod="GetStockItems" UseContextKey="true"></act:AutoCompleteExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="errMsg" colspan="4">
                                                                * denotes mandatory fields (*<sup>1</sup> for all Adjustment except Price Adjustment;
                                                                *<sup>2</sup> for Price Adjustment)
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <asp:UpdatePanel ID="uplLocateUserControl" runat="server" ChildrenAsTriggers="false"
                                            UpdateMode="Conditional">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnLocateAddItem" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnLocateGo" EventName="Click" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <asp:Panel ID="pnlLocateAdjustItem" runat="server" Enabled="false">
                                                    <dbwc:DynamicControlsPlaceholder ID="DCPLocate" runat="server" ControlsWithoutIDs="DontPersist">
                                                    </dbwc:DynamicControlsPlaceholder>
                                                </asp:Panel>
                                                <asp:Panel ID="pnlLocateAction" runat="server" Visible="false">
                                                    <div align="center">
                                                        <asp:Button ID="btnLocateSave" runat="server" CssClass="formsButton" OnClientClick="return confirm('Are you sure you want to save the changes?')"
                                                            TabIndex="102" Text="Save" />&nbsp;&#160;
                                                        <asp:Button ID="btnLocateCancel" runat="server" CssClass="formsButton" OnClientClick="return confirm('Are you sure you want to cancel your action?')"
                                                            TabIndex="103" Text="Cancel" />
                                                    </div>
                                                </asp:Panel>
                                                <asp:Button ID="ShowLocateModal" runat="server" Height="1px" Text="ShowModal" Visible="false"
                                                    Width="1px" />
                                                <act:ModalPopupExtender ID="mpuLocateStockAvailability" runat="server" BackgroundCssClass="modalBackground"
                                                    CancelControlID="btnLocateExit" OkControlID="btnLocateExit" PopupControlID="pnlLocateStockAvailability"
                                                    TargetControlID="ShowLocateModal">
                                                </act:ModalPopupExtender>
                                                <asp:Panel ID="pnlLocateStockAvailability" runat="server" BackColor="White" BAdjustColor="Gray"
                                                    BAdjustWidth="1" Style="display: none;" Width="60%">
                                                    <div style="margin: 10px;">
                                                        <table cellpadding="1" cellspacing="1" class="moduleTitle" width="100%">
                                                            <tr>
                                                                <td align="center" class="moduleTitleBorder">
                                                                    Current Stock Availability
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <br />
                                                        <table cellspacing="1" class="tblModule">
                                                            <tr>
                                                                <td class="colMod" width="30%">
                                                                    Stock Code
                                                                </td>
                                                                <td align="center" class="colRow" width="70%">
                                                                    <asp:Label ID="ViewLocateStockCode" runat="server" CssClass="colLabel"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="colMod" width="30%">
                                                                    Description
                                                                </td>
                                                                <td align="center" class="colAltRow" width="70%">
                                                                    <asp:Label ID="ViewLocateDesc" runat="server" CssClass="colLabel"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center" class="colHeader" colspan="2">
                                                                    Before Item Adjustment Update (indicative)
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="colMod" width="30%">
                                                                    Available
                                                                </td>
                                                                <td align="center" class="colRow" width="70%">
                                                                    <asp:Label ID="ViewLocateBalance" runat="server" CssClass="colLabel"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="colMod" width="30%">
                                                                    Avg Unit Cost
                                                                </td>
                                                                <td align="center" class="colAltRow" width="70%">
                                                                    <asp:Label ID="ViewLocateAUC" runat="server" CssClass="colLabel"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="colMod" width="30%">
                                                                    Total Item Value
                                                                </td>
                                                                <td align="center" class="colRow" width="70%">
                                                                    <asp:Label ID="ViewLocateTotalValue" runat="server" CssClass="colLabel"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center" class="colHeader" colspan="2">
                                                                    After Item Adjustment Update (indicative)
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="colMod" width="30%">
                                                                    New Available
                                                                </td>
                                                                <td align="center" class="colAltRow" width="70%">
                                                                    <asp:Label ID="ViewLocateBalanceNew" runat="server" CssClass="colLabel"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="colMod" width="30%">
                                                                    New Avg Unit Cost
                                                                </td>
                                                                <td align="center" class="colRow" width="70%">
                                                                    <asp:Label ID="ViewLocateAUCNew" runat="server" CssClass="colLabel"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="colMod" width="30%">
                                                                    New Total Item Value
                                                                </td>
                                                                <td align="center" class="colAltRow" width="70%">
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
                                                            <asp:Button ID="btnLocateExit" runat="server" CssClass="formsButton" OnClick="btnLocateExit_Click"
                                                                Text="Exit" />
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
                                    <asp:Label ID="lblLocateAdjust" runat="server" CssClass="progress" Text="&nbsp;Processing ..."></asp:Label></ProgressTemplate>
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
                    <act:TabPanel ID="tbpPrint" HeaderText="Adjustment Inwards Report" runat="server">
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
                                                Receive Date From (dd/mm/yyyy) *
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
                                                Receive Date To (dd/mm/yyyy) *
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
                                        <asp:Button ID="btnClear" CssClass="formsButton" Text="Clear" runat="server" Width="100px"
                                            TabIndex="23" />
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
                                            <asp:Parameter DefaultValue="" Name="adjustID" Type="String" />
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
