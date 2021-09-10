<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ReceiveItem.ascx.vb"
    Inherits="NEA_ICS.UserInterface.ReceiveItem" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:HiddenField ID="hfMode" runat="server" />
<asp:HiddenField ID="hfTranID" runat="server" />
<asp:HiddenField ID="hfItemRef" runat="server" />
<asp:HiddenField ID="hfUnitCost" runat="server" />
<asp:HiddenField ID="hfQtyOutstanding" runat="server" />
<asp:HiddenField ID="hfOrgReceiveQty" runat="server" />
<asp:HiddenField ID="hfOrgWarrantyDte" runat="server" />
<asp:HiddenField ID="hfOrgRemarks" runat="server" />
<table class="tblModule" cellspacing="1">
    <tr>
        <td class="infoMsg" colspan="4">
            <asp:Label ID="lblInfo" runat="server" Visible="True"></asp:Label>
            <asp:HiddenField ID="hfErr" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="colMod" width="20%">
            Stock Code
        </td>
        <td class="colDesc" width="50%" colspan="2">
            <asp:Label ID="lblStockCode" runat="server" Text=""></asp:Label>
        </td>
        <td class="colDesc" width="30%" colspan="1">
            <asp:LinkButton ID="btnViewDetails" CssClass="linkButton" Text="(View Stock Information)"
                Enabled="True" runat="server" TabIndex="51"></asp:LinkButton>
        </td>
    </tr>
    <tr>
        <td class="colMod" width="20%">
            Description
        </td>
        <td class="colDesc" width="80%" colspan="3">
            <asp:Label ID="lblDescription" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="colMod" width="20%">
            Receive Quantity
        </td>
        <td class="colDesc" width="30%">
            <asp:TextBox ID="txtReceiveQty" runat="server" CssClass="formsTextNum" TabIndex="52"></asp:TextBox>
            &nbsp;<asp:Label ID="lblUOM" runat="server" Text=""></asp:Label>
            <act:FilteredTextBoxExtender ID="fteReceiveQty" runat="server" TargetControlID="txtReceiveQty"
                FilterMode="ValidChars" ValidChars="0123456789.">
            </act:FilteredTextBoxExtender>
            <asp:RegularExpressionValidator ID="revReceiveQty" runat="server" ValidationExpression="^\d{1,15}(\.\d{0,2})?$"
                ControlToValidate="txtReceiveQty" ErrorMessage="2 decimal places"></asp:RegularExpressionValidator>
        </td>
        <td class="colMod" width="20%" valign="bottom">
            Warranty Date
            <br />
            (dd/mm/yyyy)
        </td>
        <td class="colDesc" width="30%" valign="bottom">
            <asp:TextBox ID="txtWarrantyDte" CssClass="formsText" runat="server" TabIndex="53"></asp:TextBox>
            <act:CalendarExtender ID="CalendarExtenderWarrantyDte" CssClass="formsCal" runat="server"
                TargetControlID="txtWarrantyDte" Format="dd/MM/yyyy" PopupPosition="BottomLeft">
            </act:CalendarExtender>
            <act:MaskedEditExtender ID="MaskedEditExtenderWarrantyDte" runat="server" TargetControlID="txtWarrantyDte"
                Mask="99/99/9999" MaskType="Date">
            </act:MaskedEditExtender>
        </td>
    </tr>
    <tr>
        <td class="colMod" width="20%" valign="bottom">
            Unit Cost (indicative)
        </td>
        <td class="colDesc" width="30%" valign="middle">
            <asp:Label ID="lblUnitCost" runat="server" Text="0.0000" CssClass="colLabel"></asp:Label>
        </td>
        <td class="colMod" width="20%">
            Total Cost (indicative)
        </td>
        <td class="colDesc" width="30%">
            <asp:Label ID="lblTotalCost" runat="server" CssClass="colLabel" Text="0.0000"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="colMod" width="20%">
            Remarks
        </td>
        <td class="colDesc" width="99%" colspan="3" valign="bottom">
            <asp:TextBox ID="txtRemarks" CssClass="formsTextMultiLine" runat="server" MaxLength="500"
                TextMode="MultiLine" TabIndex="54"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td colspan="4" class="errMsg">
        </td>
    </tr>
</table>
