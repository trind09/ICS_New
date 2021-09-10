<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="OrderItem.ascx.vb"
    Inherits="NEA_ICS.UserInterface.OrderItem" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:UpdatePanel ID="uplOrderItem" runat="server" UpdateMode="always" ChildrenAsTriggers="true">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="txtTotalCost" EventName="TextChanged" />
    </Triggers>
    <ContentTemplate>
        <asp:HiddenField ID="hfMode" runat="server" />
        <asp:HiddenField ID="hfOrderItemID" runat="server" />
        <asp:HiddenField ID="hfAllowQty" runat="server" />
        <asp:HiddenField ID="hfReceiveQty" runat="server" />
        <asp:HiddenField ID="hfOrgOrderQty" runat="server" />
        <asp:HiddenField ID="hfOrgTotalCost" runat="server" />
        <asp:HiddenField ID="hfOrgEDDte" runat="server" />
        <asp:HiddenField ID="hfOrgWarrantyDte" runat="server" />
        <asp:HiddenField ID="hfOrgRemarks" runat="server" />
        <table class="tblModule" cellspacing="1">
            <tr>
                <td class="infoMsg" colspan="4">
                    <asp:Label ID="lblErr" runat="server" Visible="True" />
                </td>
            </tr>
            <tr>
                <td class="colMod" width="20%">
                    Stock Code
                </td>
                <td class="colDesc" width="50%" colspan="2">
                    <asp:Label ID="lblStockCode" runat="server" />
                </td>
                <td class="colDesc" width="30%">
                    <asp:LinkButton ID="btnViewDetails" runat="server" CssClass="linkButton" Text="(View Stock Availability)"
                        TabIndex="51" />
                </td>
            </tr>
            <tr>
                <td class="colMod" width="20%">
                    Description
                </td>
                <td class="colDesc" width="80%" colspan="3">
                    <asp:Label ID="lblDescription" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="colMod" width="20%">
                    Order Quantity *
                </td>
                <td class="colDesc" width="30%">
                    <asp:TextBox ID="txtOrderQty" runat="server" CssClass="formsTextNum" TabIndex="52" />
                    &nbsp;<asp:Label ID="lblUOM" runat="server" />
                    <act:FilteredTextBoxExtender ID="fteOrderQty" runat="server" TargetControlID="txtOrderQty"
                        FilterMode="ValidChars" ValidChars="0123456789.">
                    </act:FilteredTextBoxExtender>
                    <asp:RegularExpressionValidator ID="revOrderQty" runat="server" ValidationExpression="^\d{1,15}(\.\d{0,2})?$"
                        ControlToValidate="txtOrderQty" ErrorMessage="2 decimal places"></asp:RegularExpressionValidator>
                </td>
                <td class="colMod" width="20%">
                    Total Cost *<br />
                    (w/o GST)
                </td>
                <td class="colDesc" width="30%">
                    <asp:TextBox ID="txtTotalCost" runat="server" CssClass="formsTextNum" AutoPostBack="true"
                        TabIndex="53" />
                    <act:FilteredTextBoxExtender ID="fteTotalCost" runat="server" TargetControlID="txtTotalCost"
                        FilterMode="ValidChars" ValidChars="0123456789.">
                    </act:FilteredTextBoxExtender>
                    <asp:RegularExpressionValidator ID="revTotalCost" runat="server" ValidationExpression="^\d{1,15}(\.\d{0,2})?$"
                        ControlToValidate="txtTotalCost" ErrorMessage="2 decimal places"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td class="colMod" width="20%">
                    Expected Delivery Date
                    <br />
                    (dd/mm/yyyy) *
                </td>
                <td class="colDesc" width="30%">
                    <asp:TextBox ID="txtExpectedDeliveryDte" CssClass="formsText" runat="server" TabIndex="54"></asp:TextBox>
                    <act:CalendarExtender ID="CalendarExtenderExpectedDeliveryDte" CssClass="formsCal"
                        runat="server" TargetControlID="txtExpectedDeliveryDte" Format="dd/MM/yyyy" PopupPosition="BottomLeft" />
                    <act:MaskedEditExtender ID="MaskedEditExtenderExpectedDeliveryDte" runat="server"
                        TargetControlID="txtExpectedDeliveryDte" Mask="99/99/9999" MaskType="Date" />
                </td>
                <td class="colMod" width="20%">
                    Warranty Date
                    <br />
                    (dd/mm/yyyy)
                </td>
                <td class="colDesc" width="30%">
                    <asp:TextBox ID="txtWarrantyDte" runat="server" CssClass="formsText" TabIndex="55" />
                    <act:CalendarExtender ID="CalendarExtenderWarrantyDte" runat="server" CssClass="formsCal"
                        TargetControlID="txtWarrantyDte" Format="dd/MM/yyyy" PopupPosition="BottomLeft" />
                    <act:MaskedEditExtender ID="MaskedEditExtenderWarrantyDte" runat="server" TargetControlID="txtWarrantyDte"
                        Mask="99/99/9999" MaskType="Date" />
                </td>
            </tr>
            <tr>
                <td class="colMod" width="20%">
                    Remarks *
                </td>
                <td class="colDesc" width="80%" colspan="3">
                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="formsTextMultiLine" MaxLength="500"
                        TextMode="MultiLine" TabIndex="56" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="uplUnitCost" runat="server" UpdateMode="Conditional">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="txtTotalCost" />
        <asp:AsyncPostBackTrigger ControlID="txtOrderQty" />
    </Triggers>
    <ContentTemplate>
        <table class="tblModule" cellspacing="1">
            <tr>
                <td class="colMod" width="20%">
                    Unit Cost (display only)
                </td>
                <td class="colDesc" width="30%">
                    <asp:Label ID="lblUnitCost" runat="server" Text="0.0000" CssClass="colLabel" />
                </td>
            </tr>
            <tr>
                <td class="colMod" width="20%">
                    &#160;
                </td>
                <td class="colDesc" width="80%">
                    <asp:LinkButton ID="btnCancel" runat="server" CssClass="linkButton" Text="[Cancel Item]"
                        TabIndex="57" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
