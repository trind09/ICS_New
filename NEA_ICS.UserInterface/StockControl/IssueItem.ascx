<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="IssueItem.ascx.vb"
    Inherits="NEA_ICS.UserInterface.IssueItem" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<!-- Insert, Update or Delete, Approved -->
<asp:HiddenField ID="hfMode" runat="server" />
<!-- StockTransactionID -->
<asp:HiddenField ID="hfTranID" runat="server" />
<!-- StockTransactionItemRef -->
<asp:HiddenField ID="hfRequestItemID" runat="server" />
<!-- RequestItemStatus -->
<asp:HiddenField ID="hfRequestItemStatus" runat="server" />
<!-- ComputedBalanceQty -->
<asp:HiddenField ID="hfBalanceQty" runat="server" />
<asp:HiddenField ID="hfOrgIssueQty" runat="server" />
<asp:HiddenField ID="hfOrgRemarks" runat="server" />
<table class="tblModule" cellspacing="1">
    <tr>
        <td class="infoMsg" colspan="4">
            &nbsp;<asp:Label ID="lblInfo" runat="server" Visible="True" />
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
            <asp:LinkButton ID="btnViewDetails" runat="server" CssClass="linkButton" Text="(View Stock Availability)" TabIndex="11" />
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
            Request Quantity *<sup>1</sup>
        </td>
        <td class="colDesc" width="30%">
            <!-- enabled for requester; disabled for issuer -->
            <asp:TextBox ID="txtRequestQty" runat="server" CssClass="formsTextNum" Enabled="true" TabIndex="12" />
            &nbsp;<asp:Label ID="lblUOM" runat="server" />
            <act:FilteredTextBoxExtender ID="fteRequestQty" runat="server" TargetControlID="txtRequestQty"
                FilterMode="ValidChars" ValidChars="0123456789.">
            </act:FilteredTextBoxExtender>
            <asp:RegularExpressionValidator ID="revRequestQty" runat="server" ValidationExpression="^\d{1,15}(\.\d{0,2})?$"
                ControlToValidate="txtRequestQty" ErrorMessage="2 decimal only"></asp:RegularExpressionValidator>
        </td>
        <td class="colMod" width="20%">
            Issue Quantity *<sup>2</sup>
        </td>
        <td class="colDesc" width="30%">
            <!-- disabled for requester; enabled for issuer -->
            <asp:TextBox ID="txtIssueQty" runat="server" CssClass="formsTextNum" Enabled="false" TabIndex="13" />
            &nbsp;<asp:Label ID="lblUOM2" runat="server" />
            <act:FilteredTextBoxExtender ID="fteIssueQty" runat="server" TargetControlID="txtIssueQty"
                FilterMode="ValidChars" ValidChars="0123456789.">
            </act:FilteredTextBoxExtender>
            <asp:RegularExpressionValidator ID="revIssueQty" runat="server" ValidationExpression="^\d{1,15}(\.\d{0,2})?$"
                ControlToValidate="txtIssueQty" ErrorMessage="2 decimal only"></asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <td class="colMod" width="20%">
            Remarks
        </td>
        <td class="colDesc" width="80%" valign="bottom" colspan="3" TabIndex="14">
            <asp:TextBox ID="txtRemarks" CssClass="formsTextMultiLine" runat="server" MaxLength="500"
                TextMode="MultiLine"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="colMod" width="20%">
            &#160;
        </td>
        <td class="colDesc" width="80%" colspan="3">
            <asp:LinkButton ID="btnCancel" CssClass="linkButton" Text="[Cancel Item]" runat="server" TabIndex="15"></asp:LinkButton>
        </td>
    </tr>
    <tr>
        <td colspan="4" class="errMsg">
            * denotes mandatory fields (*<sup>1</sup> for requester; *<sup>2</sup> for Issuer)
        </td>
    </tr>
</table>
