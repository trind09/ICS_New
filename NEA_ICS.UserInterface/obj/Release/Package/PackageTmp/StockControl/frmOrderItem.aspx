<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmOrderItem.aspx.vb"
    Inherits="NEA_ICS.UserInterface.frmOrderItem" ValidateRequest="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register TagPrefix="dbwc" Namespace="DBauer.Web.UI.WebControls" Assembly="DBauer.Web.UI.WebControls.DynamicControlsPlaceholder" %>
<%@ Register Src="OrderItem.ascx" TagName="OrderItem" TagPrefix="uc1" %>
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

    <script language="javascript" type="text/javascript">
    </script>

</head>
<body class="body">
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
        <table class="moduleTitle" width="98%" cellspacing="1" cellpadding="1">
            <tr>
                <td class="moduleTitleBorder">
                    &nbsp;Stock Control > Order Item
                </td>
            </tr>
        </table>
        <br />
        <asp:UpdatePanel ID="uplPage" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <act:TabContainer ID="tbcOrderItem" Width="98%" Font-Bold="true" Font-Size="Medium"
                    runat="server" ForeColor="#4D36C2" BackColor="#FFFFFF" Font-Names="Verdana" ActiveTabIndex="0">
                    <act:TabPanel ID="tbpNew" HeaderText="New Order" runat="server">
                        <HeaderTemplate>
                            New Order
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="uplNewOrder" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlSupplierID" EventName="SelectedIndexChanged" />
                                    <asp:PostBackTrigger ControlID="btnCancelAll" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:Label ID="lblErrNew" runat="server" CssClass="errMsg" Visible="true"></asp:Label>
                                    <asp:UpdatePanel ID="uplMain" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table class="tblModule" cellspacing="1">
                                                <tr>
                                                    <td class="colMod" width="20%">
                                                        Order Reference *
                                                    </td>
                                                    <td class="colDesc" width="30%">
                                                        <asp:TextBox ID="txtOrderID" CssClass="formsText" runat="server" MaxLength="12" TabIndex="11" />
                                                    </td>
                                                    <td class="colMod" width="20%">
                                                        Gebiz PO No
                                                    </td>
                                                    <td class="colDesc" width="30%">
                                                        <asp:TextBox ID="txtGebizPONo" CssClass="formsText" runat="server" MaxLength="10"
                                                            TabIndex="12" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="colMod" width="20%">
                                                        Supplier Code *
                                                    </td>
                                                    <td class="colDesc" width="30%" colspan="1">
                                                        <asp:DropDownList ID="ddlSupplierID" runat="server" CssClass="formsCombo" AutoPostBack="true"
                                                            TabIndex="13" />
                                                    </td>
                                                    <td class="colMod" width="20%">
                                                        Document Type *
                                                    </td>
                                                    <td class="colDesc" width="30%">
                                                        <asp:DropDownList ID="ddlDocType" runat="server" CssClass="formsCombo" TabIndex="14" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="colMod" width="20%">
                                                        Company Name
                                                    </td>
                                                    <td class="colDesc" width="38%" valign="middle" colspan="3">
                                                        <asp:Label ID="lblCompanyName" runat="server" Text="" CssClass="colLabel" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="colMod" width="20%">
                                                        Order Date (dd/mm/yyyy) *
                                                    </td>
                                                    <td class="colDesc" width="30%" colspan="1">
                                                        <asp:TextBox ID="txtOrderDte" CssClass="formsText" runat="server" TabIndex="15"></asp:TextBox>
                                                        <act:CalendarExtender ID="calOrderDte" CssClass="formsCal" runat="server" TargetControlID="txtOrderDte"
                                                            Format="dd/MM/yyyy" PopupPosition="BottomLeft" />
                                                        <act:MaskedEditExtender ID="meeOrderDte" runat="server" TargetControlID="txtOrderDte"
                                                            Mask="99/99/9999" MaskType="Date" />
                                                    </td>
                                                    <td class="colMod" width="20%">
                                                        Total Cost (w/o GST)
                                                    </td>
                                                    <td class="colDesc" width="30%" valign="middle">
                                                        <asp:Label ID="lblGTotal" runat="server" Text="0.0000" CssClass="colLabel" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" class="errMsg">
                                                        * denotes mandatory fields
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <br />
                                    <div id="divAddButton" runat="server" align="center" visible="true">
                                        <asp:Button ID="btnAddOrderItem" CssClass="formsButtonLarge" Text="Add Order" runat="server"
                                            Enabled="true" TabIndex="21" />
                                        <br />
                                    </div>
                                    <asp:Panel ID="pnlNewOrder" runat="server" Visible="false">
                                        <asp:Image ID="imgPOItem" runat="server" ImageUrl="~/Images/order_details.gif" AlternateText="Order Details" />
                                        <asp:UpdatePanel ID="uplStockCode" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                            <ContentTemplate>
                                                <table class="tblModule" cellspacing="1">
                                                    <tr>
                                                        <td class="colMod" width="20%">
                                                            Enter a Stock Code for your Order Item
                                                        </td>
                                                        <td class="colDesc" width="80%">
                                                            <asp:TextBox ID="txtStockCode" runat="server" CssClass="formsTextNumLarge"></asp:TextBox>
                                                            &#160;&#160;&#160;
                                                            <asp:LinkButton ID="btnAddItem" runat="server" CssClass="linkButton" Text="[Add Order Item]" />
                                                            <act:AutoCompleteExtender ID="aceStockCode" runat="server" MinimumPrefixLength="2" CompletionSetCount="20" TargetControlID="txtStockCode"
                                                            ServiceMethod="GetStockItems" UseContextKey="true"></act:AutoCompleteExtender>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <asp:UpdatePanel ID="uplUserControl" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btnSubmit" />
                                                <asp:AsyncPostBackTrigger ControlID="btnAddItem" EventName="Click" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <dbwc:DynamicControlsPlaceholder ID="DCP" runat="server" ControlsWithoutIDs="DontPersist">
                                                </dbwc:DynamicControlsPlaceholder>
                                                <br />
                                                <div align="center">
                                                    <asp:Button ID="btnSubmit" CssClass="formsButton" Text="Save" runat="server" OnClientClick="return confirm('Are you sure you want to save your order?')"
                                                        TabIndex="7" />&#160;&#160;&#160;
                                                    <asp:Button ID="btnCancelAll" CssClass="formsButton" Text="Cancel All" runat="server"
                                                        OnClientClick="return confirm('Are you sure you want to cancel your action?')"
                                                        TabIndex="8" />
                                                </div>
                                                <asp:Button ID="ShowModal" runat="server" Text="ShowModal" Visible="false" Height="1px"
                                                    Width="1px" />
                                                <act:ModalPopupExtender ID="mpuStockAvailability" runat="server" TargetControlID="ShowModal"
                                                    PopupControlID="pnlStockAvailability" OkControlID="btnExit" CancelControlID="btnExit"
                                                    BackgroundCssClass="modalBackground">
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
                                                                <td class="colMod" width="30%">
                                                                    Available
                                                                </td>
                                                                <td class="colRow" width="70%" align="center">
                                                                    <asp:Label ID="ViewBalance" runat="server" CssClass="colLabel"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="colMod" width="30%">
                                                                    Avg Unit Cost (indicative)
                                                                </td>
                                                                <td class="colAltRow" width="70%" align="center">
                                                                    <asp:Label ID="ViewAUC" runat="server" CssClass="colLabel"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="colMod" width="30%">
                                                                    Total Item Value (indicative)
                                                                </td>
                                                                <td class="colRow" width="70%" align="center">
                                                                    <asp:Label ID="ViewTotalValue" runat="server" CssClass="colLabel"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="colMod" width="30%">
                                                                    Maximum Level
                                                                </td>
                                                                <td class="colAltRow" width="70%" align="center">
                                                                    <asp:Label ID="ViewMax" runat="server" CssClass="colLabel"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="colMod" width="30%">
                                                                    On Order Qty
                                                                </td>
                                                                <td class="colRow" width="70%" align="center">
                                                                    <asp:Label ID="ViewOnOrderQty" runat="server" CssClass="colLabel"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="colMod" width="30%">
                                                                    Pending Order
                                                                </td>
                                                                <td class="colAltRow" width="70%" align="center">
                                                                    <asp:Label ID="ViewOnOrderCount" runat="server" CssClass="colLabel"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <br />
                                                        <div align="center">
                                                            <asp:Button ID="btnExit" CssClass="formsButton" Text="Exit" runat="server" OnClick="btnExit_Click" />
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
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
                    <act:TabPanel ID="tbpLocate" HeaderText="Locate Order" runat="server">
                        <HeaderTemplate>
                            Locate Order
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="uplLocateOrder" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlLocateOrderID" EventName="SelectedIndexChanged" />
                                    <asp:PostBackTrigger ControlID="btnLocateClear" />
                                    <asp:PostBackTrigger ControlID="btnLocateSave" />
                                    <asp:PostBackTrigger ControlID="btnLocateCancel" />
                                    <asp:PostBackTrigger ControlID="btnLocateDeleteAll" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="uplLocateMain" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblErrLocate" runat="server" CssClass="errMsg" Visible="true"></asp:Label>
                                            <table class="tblModule" cellspacing="1">
                                                <tr>
                                                    <td class="colMod" width="20%">
                                                        Order Reference *
                                                    </td>
                                                    <td class="colDesc" width="80%">
                                                        <%-- GetOrders required to be Shared, hence cannot implement here
                                                <asp:TextBox ID="txtLocateOrderID" CssClass="formsText" runat="server" Visible="true"></asp:TextBox>
                                                <act:AutoCompleteExtender runat="server" ID="aceLocateOrderID" TargetControlID="txtLocateOrderID"
                                                    ServiceMethod="GetOrders" MinimumPrefixLength="2" CompletionInterval="1000" EnableCaching="true"
                                                    CompletionSetCount="20" DelimiterCharacters=";, :">
                                                </act:AutoCompleteExtender>
--%>
                                                        <asp:DropDownList ID="ddlLocateOrderID" runat="server" CssClass="formsCombo" TabIndex="1">
                                                        </asp:DropDownList>
                                                        <asp:CheckBox ID="cbxLocateUnfulfillOnly" Checked="true" Text="Unfulfill Order Only"
                                                            runat="server" AutoPostBack="true" />
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="errMsg" colspan="2">
                                                        * denotes mandatory fields
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                            <div align="center">
                                                <asp:Button ID="btnLocateGo" CssClass="formsButton" Text="Go" runat="server" TabIndex="5" />&nbsp;
                                                <asp:Button ID="btnLocateClear" CssClass="formsButton" Text="Clear" runat="server"
                                                    TabIndex="6" />
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:Panel ID="pnlLocate" runat="server" Visible="false">
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/order_details.gif" AlternateText="Order Details" />
                                        <br />
                                        <asp:UpdatePanel ID="uplLocateMainOrder" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Panel ID="pnlLocateOrder" runat="server" Enabled="false">
                                                    <table class="tblModule" cellspacing="1">
                                                        <tr>
                                                            <td class="colMod" width="20%">
                                                                Order Reference *
                                                            </td>
                                                            <td class="colDesc" width="30%">
                                                                <asp:Label ID="lblLocateOrderID" runat="server" Text="" CssClass="colLabel"></asp:Label>
                                                            </td>
                                                            <td class="colMod" width="20%">
                                                                Gebiz PO No
                                                            </td>
                                                            <td class="colDesc" width="30%">
                                                                <asp:TextBox ID="txtLocateGebizPONo" CssClass="formsText" runat="server" MaxLength="10"
                                                                    AutoPostBack="true" TabIndex="21"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="colMod" width="20%">
                                                                Supplier Code *
                                                            </td>
                                                            <td class="colDesc" width="30%" colspan="1">
                                                                <asp:DropDownList ID="ddlLocateSupplierID" runat="server" CssClass="formsCombo" AutoPostBack="true"
                                                                    TabIndex="22">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="colMod" width="20%">
                                                                Document Type *
                                                            </td>
                                                            <td class="colDesc" width="30%">
                                                                <asp:DropDownList ID="ddlLocateDocType" runat="server" CssClass="formsCombo" AutoPostBack="true"
                                                                    TabIndex="23">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="colMod" width="20%">
                                                                Company Name
                                                            </td>
                                                            <td class="colDesc" width="38%" valign="middle" colspan="3">
                                                                <asp:Label ID="lblLocateCompanyName" runat="server" Text="" CssClass="colLabel"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="colMod" width="20%">
                                                                Order Date (dd/mm/yyyy) *
                                                            </td>
                                                            <td class="colDesc" width="30%" colspan="1">
                                                                <asp:TextBox ID="txtLocateOrderDte" CssClass="formsText" runat="server" AutoPostBack="true"
                                                                    TabIndex="24"></asp:TextBox>
                                                                <act:CalendarExtender ID="celLocateOrderDte" CssClass="formsCal" runat="server" TargetControlID="txtLocateOrderDte"
                                                                    Format="dd/MM/yyyy" PopupPosition="BottomLeft">
                                                                </act:CalendarExtender>
                                                                <act:MaskedEditExtender ID="meeLocateOrderDte" runat="server" TargetControlID="txtLocateOrderDte"
                                                                    Mask="99/99/9999" MaskType="Date">
                                                                </act:MaskedEditExtender>
                                                            </td>
                                                            <td class="colMod" width="20%">
                                                                Total Cost (w/o GST)
                                                            </td>
                                                            <td class="colDesc" width="30%" valign="middle">
                                                                <asp:Label ID="lblLocateGTotalCost" runat="server" CssClass="colLabel"></asp:Label>
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
                                                                <asp:LinkButton ID="btnLocateEdit" runat="server" CssClass="linkButton" OnClientClick="return confirm('Are you sure you want to edit this Order Reference?')"
                                                                    Text="[Edit Order]" TabIndex="26" />&nbsp;&nbsp;&nbsp;
                                                                <asp:LinkButton ID="btnLocateDeleteAll" runat="server" CssClass="linkButton" OnClientClick="return confirm('Are you sure you want to delete this Order Reference?')"
                                                                    Text="[Delete Order]" TabIndex="27"></asp:LinkButton>
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
                                        <asp:UpdatePanel ID="uplLocateStockCode" runat="server" UpdateMode="Conditional"
                                            ChildrenAsTriggers="false" Visible="false">
                                            <ContentTemplate>
                                                <table class="tblModule" cellspacing="1">
                                                    <tr>
                                                        <td class="colMod" width="20%">
                                                            Enter a Stock Code for your Order Item
                                                        </td>
                                                        <td class="colDesc" width="80%">
                                                            <asp:TextBox ID="txtLocateStockCode" runat="server" CssClass="formsTextNumLarge"></asp:TextBox>
                                                            &#160;&#160;&#160;
                                                            <asp:LinkButton ID="btnLocateAddItem" runat="server" CssClass="linkButton" Text="[Add Order Item]" />
                                                            <act:AutoCompleteExtender ID="aceLocateStockCode" runat="server" MinimumPrefixLength="2" CompletionSetCount="20" 
                                                            TargetControlID="txtLocateStockCode"
                                                            ServiceMethod="GetStockItems" UseContextKey="true"></act:AutoCompleteExtender>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <asp:UpdatePanel ID="uplLocateUserControl" runat="server" UpdateMode="Conditional"
                                            ChildrenAsTriggers="false">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnLocateAddItem" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnLocateGo" EventName="Click" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <asp:Panel ID="pnlLocateOrderItem" runat="server" Enabled="false">
                                                    <dbwc:DynamicControlsPlaceholder ID="DCPLocate" runat="server" ControlsWithoutIDs="DontPersist">
                                                    </dbwc:DynamicControlsPlaceholder>
                                                </asp:Panel>
                                                <asp:Panel ID="pnlLocateAction" runat="server" Visible="false">
                                                    <div align="center">
                                                        <br />
                                                        <asp:Button ID="btnLocateSave" CssClass="formsButton" Text="Save" runat="server"
                                                            OnClientClick="return confirm('Are you sure you want to save the changes?')"
                                                            TabIndex="102" />&nbsp;&#160;
                                                        <asp:Button ID="btnLocateCancel" CssClass="formsButton" Text="Cancel" runat="server"
                                                            OnClientClick="return confirm('Are you sure you want to cancel your action?')"
                                                            TabIndex="103" />
                                                    </div>
                                                    <br />
                                                </asp:Panel>
                                                <asp:Button ID="ShowLocateModal" runat="server" Text="ShowModal" Visible="false"
                                                    Height="1px" Width="1px" />
                                                <act:ModalPopupExtender ID="mpuLocateStockAvailability" runat="server" TargetControlID="ShowLocateModal"
                                                    PopupControlID="pnlLocateStockAvailability" OkControlID="btnLocateExit" CancelControlID="btnLocateExit"
                                                    BackgroundCssClass="modalBackground">
                                                </act:ModalPopupExtender>
                                                <asp:Panel ID="pnlLocateStockAvailability" Style="display: none;" BackColor="White"
                                                    runat="server" Width="50%" BorderWidth="1" BorderColor="Gray">
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
                                                                <td class="colMod" width="30%">
                                                                    Available
                                                                </td>
                                                                <td class="colRow" width="70%" align="center">
                                                                    <asp:Label ID="ViewLocateBalance" runat="server" CssClass="colLabel"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="colMod" width="30%">
                                                                    Avg Unit Cost (indicative)
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
                                                                <td class="colMod" width="30%">
                                                                    Maximum Level
                                                                </td>
                                                                <td class="colAltRow" width="70%" align="center">
                                                                    <asp:Label ID="ViewLocateMax" runat="server" CssClass="colLabel"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="colMod" width="30%">
                                                                    On Order Qty
                                                                </td>
                                                                <td class="colRow" width="70%" align="center">
                                                                    <asp:Label ID="ViewLocateOnOrderQty" runat="server" CssClass="colLabel"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="colMod" width="30%">
                                                                    Pending Order
                                                                </td>
                                                                <td class="colAltRow" width="70%" align="center">
                                                                    <asp:Label ID="ViewLocateOnOrderCount" runat="server" CssClass="colLabel"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <br />
                                                        <div align="center">
                                                            <asp:Button ID="btnLocateExit" CssClass="formsButton" Text="Exit" runat="server" />&#160;
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdateProgress ID="upgLocateOrder" runat="server" AssociatedUpdatePanelID="uplLocateOrder">
                                <ProgressTemplate>
                                    <br />
                                    <img src="../images/progress.gif" alt="Processing" />
                                    <asp:Label ID="lblProgress" CssClass="progress" Text="&nbsp;Processing ..." runat="server"></asp:Label></ProgressTemplate>
                            </asp:UpdateProgress>
                            <asp:UpdateProgress ID="upgLocateStockCode" runat="server" AssociatedUpdatePanelID="uplLocateStockCode">
                                <ProgressTemplate>
                                    <br />
                                    <img src="../images/progress.gif" alt="Processing" />
                                    <asp:Label ID="lblProgressStockCode" runat="server" CssClass="progress" Text="&nbsp;Processing ..." /></ProgressTemplate>
                            </asp:UpdateProgress>
                            <asp:UpdateProgress ID="upgLocateOrderUserControl" runat="server" AssociatedUpdatePanelID="uplLocateUserControl">
                                <ProgressTemplate>
                                    <br />
                                    <img src="../images/progress.gif" alt="Processing" />
                                    <asp:Label ID="lblProgressUserControl" runat="server" CssClass="progress" Text="&nbsp;Processing ..." /></ProgressTemplate>
                            </asp:UpdateProgress>
                        </ContentTemplate>
                    </act:TabPanel>
                    <act:TabPanel ID="tbpPrint" HeaderText="Order Report" runat="server">
                        <ContentTemplate>
                            <asp:UpdatePanel ID="uplPrintOrder" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table class="tblModule" cellspacing="1">
                                        <tr>
                                            <td class="colMod" width="20%">
                                                Print Option *
                                            </td>
                                            <td class="colDesc" width="30%" colspan="3">
                                                <asp:DropDownList ID="ddlPrintOption" CssClass="formsCombo" runat="server" TabIndex="11">
                                                    <asp:ListItem Text=" - Please Select - " Value=""></asp:ListItem>
                                                    <asp:ListItem Text="All" Value="A"></asp:ListItem>
                                                    <asp:ListItem Text="Fulfilled Order" Value="F"></asp:ListItem>
                                                    <asp:ListItem Text="Unfulfilled Order" Value="U"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="colMod" width="20%">
                                                Order Date From *<br />
                                                (dd/mm/yyyy)
                                            </td>
                                            <td class="colDesc" width="30%">
                                                <asp:TextBox ID="txtPODateFrom" runat="server" CssClass="formsText" TabIndex="12"></asp:TextBox>
                                                <act:CalendarExtender ID="CalendarExtender3" runat="server" CssClass="formsCal" Format="dd/MM/yyyy"
                                                    PopupPosition="BottomLeft" TargetControlID="txtPODateFrom">
                                                </act:CalendarExtender>
                                                <act:MaskedEditExtender ID="MaskedEditExtender3" runat="server" Mask="99/99/9999"
                                                    MaskType="Date" TargetControlID="txtPODateFrom">
                                                </act:MaskedEditExtender>
                                            </td>
                                            <td class="colMod" width="20%">
                                                Order Date To *<br />
                                                (dd/mm/yyyy)
                                            </td>
                                            <td class="colDesc" width="30%">
                                                <asp:TextBox ID="txtPODateTo" runat="server" CssClass="formsText" TabIndex="13"></asp:TextBox>
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
                                                        <asp:DropDownList ID="ddlOrderReference" runat="server" CssClass="formsCombo" TabIndex="16">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" class="errMsg">
                                                        * denotes mandatory fields ; # When this is selected, other selection will be bypassed
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
                                        <asp:Button ID="btnClear" CssClass="formsButton" Text="Clear" runat="server" TabIndex="23" />
                                    </div>
                                    <rsweb:ReportViewer ID="rvr" runat="server" Font-Size="8pt" Height="400px" Visible="False"
                                        Width="100%" Font-Names="Verdana">
                                        <LocalReport ReportPath="StockControl\rptOrderList.rdlc">
                                            <DataSources>
                                                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="OrderList" />
                                            </DataSources>
                                        </LocalReport>
                                    </rsweb:ReportViewer>
                                    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetOrderList"
                                        TypeName="NEA_ICS.UserInterface.NEA_ICS.WcfService.ServiceClient">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="" Name="storeID" Type="String" />
                                            <asp:Parameter DefaultValue="" Name="status" Type="String" />
                                            <asp:Parameter DefaultValue="" Name="fromDte" Type="DateTime" />
                                            <asp:Parameter DefaultValue="" Name="toDte" Type="DateTime" />
                                            <asp:Parameter DefaultValue="" Name="fromStockItemID" Type="String" />
                                            <asp:Parameter DefaultValue="" Name="toStockItemID" Type="String" />
                                            <asp:Parameter DefaultValue="" Name="orderId" Type="String" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                    <br />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnPDF" />
                                    <asp:PostBackTrigger ControlID="btnExcel" />
                                    <asp:PostBackTrigger ControlID="btnClear" />
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
