<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AdjustReturnItem.ascx.vb"
    Inherits="NEA_ICS.UserInterface.AdjustReturnItem" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:HiddenField ID="hfMode" runat="server" />
<asp:HiddenField ID="hfTranID" runat="server" />
<asp:HiddenField ID="hfTranType" runat="server" />
<asp:HiddenField ID="hfAdjustItemID" runat="server" />
<asp:HiddenField ID="hfItemReturn" runat="server" />
<asp:HiddenField ID="hfBalanceQty" runat="server" />
<asp:HiddenField ID="hfMaxLevel" runat="server" />
<asp:HiddenField ID="hfOrgAdjustQty" runat="server" />
<asp:HiddenField ID="hfOrgTotalCost" runat="server" />
<%--<asp:HiddenField ID="hfNewTotalCost" runat="server" />--%>
<asp:HiddenField ID="hfUnitCost" runat="server" />
<asp:HiddenField ID="hfOrgRemarks" runat="server" />
<asp:HiddenField ID="hfReceivedCorrectly" runat="server" />
<asp:HiddenField ID="hfAdjustItemStatus" runat="server" />
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
            Return Quantity #<sup>1</sup>
        </td>
        <td class="colDesc" width="30%">
            <asp:TextBox ID="txtAdjustQty" runat="server" CssClass="formsTextNum" TabIndex="52" />
            &nbsp;<asp:Label ID="lblUOM" runat="server" />
            <act:FilteredTextBoxExtender ID="fteAdjustQty" runat="server" TargetControlID="txtAdjustQty"
                FilterMode="ValidChars" ValidChars="0123456789.">
            </act:FilteredTextBoxExtender>
            <asp:RegularExpressionValidator ID="revAdjustQty" runat="server" ValidationExpression="^\d{1,15}(\.\d{0,2})?$"
                ControlToValidate="txtAdjustQty" ErrorMessage="2 decimal places"></asp:RegularExpressionValidator>
        </td>
        <td class="colMod" width="20%">
            Receive Quantity #
        </td>
        <td class="colDesc" width="30%">
            <asp:TextBox ID="txtReceiveQty" runat="server" CssClass="formsTextNum" TabIndex="53" />
            <act:FilteredTextBoxExtender ID="fteReceiveQty" runat="server" TargetControlID="txtReceiveQty"
                FilterMode="ValidChars" ValidChars="0123456789.">
            </act:FilteredTextBoxExtender>
            <asp:RegularExpressionValidator ID="revReceiveQty" runat="server" ValidationExpression="^\d{1,15}(\.\d{0,2})?$"
                ControlToValidate="txtReceiveQty" ErrorMessage="2 decimal places"></asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <td class="colMod" width="20%">
            Remarks
        </td>
        <td class="colDesc" width="80%" colspan="3">
            <asp:TextBox ID="txtRemarks" runat="server" CssClass="formsTextMultiLine" MaxLength="500"
                TextMode="MultiLine" TabIndex="54" />
        </td>
    </tr>
    <tr>
        <td class="colMod" width="20%">
            &nbsp;
        </td>
        <td class="colDesc" width="80%" colspan="3">
             <asp:CheckBox ID="checkReceived" runat="server" Text="All above items are returned correctly and received by Store Officer."
             Visible="true" /> *
        </td>
    </tr>
</table>
<asp:UpdatePanel ID="uplUnitCost" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table class="tblModule" cellspacing="1">
            <tr id="trUnitCost" runat="server" visible="false" >
                <td class="colMod" width="20%">
                    Unit Cost (indicative)
                </td>
                <td class="colDesc" width="80%">
                    <asp:Label ID="lblUnitCost" runat="server" Text="0.0000" CssClass="colLabel" />
                </td>
            </tr>
            <tr id="trCancel" runat="server" visible="true">
                <td class="colMod" width="20%">
                    &#160;
                </td>
                <td class="colDesc" width="80%">
                    <asp:LinkButton ID="btnCancel" runat="server" CssClass="linkButton" Text="[Cancel Item]"
                        TabIndex="55" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
