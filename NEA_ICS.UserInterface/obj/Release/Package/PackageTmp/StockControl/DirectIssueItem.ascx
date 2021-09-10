<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="DirectIssueItem.ascx.vb" Inherits="NEA_ICS.UserInterface.DirectIssueItem" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>

    <table class="tblModule" cellspacing="1">
        <tr>
            <td class="colMod" width="20%">
                Stock Code
            </td>
            <td class="colDesc" width="30%" colspan="1">
                <asp:Label ID="lblStockCode" CssClass="colLabel" runat="server" Text="DIRECT"></asp:Label>
                <asp:HiddenField ID="hidStockCode" runat="server" />
            </td>
            <td class="colMod" width="20%">
                Stock Type
            </td>
            <td class="colDesc" width="30%" colspan="1">
                <asp:Label ID="lblStockType" CssClass="colLabel" runat="server" Text="D"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="colMod" width="20%">
                Description *
            </td>
            <td class="colDesc" width="80%" colspan="3">
                <asp:TextBox ID="txtDescription" CssClass="formsText" runat="server" Text="" Width="99%"></asp:TextBox>
                <asp:HiddenField ID="hidDescription" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="colMod" width="20%" valign="top">
                Remarks 
            </td>
            <td class="colDesc" width="80%" colspan="3">
                <asp:TextBox ID="txtRemarks" CssClass="formsTextMultiLine" TextMode="MultiLine" runat="server" Width="98%"></asp:TextBox>
                <asp:HiddenField ID="hidRemarks" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="colMod" width="20%">
                Qty Issued *
            </td>
            <td class="colDesc" width="30%">
                <asp:TextBox ID="txtQtyIssued" runat="server" CssClass="formsTextNum" Text="0.00"></asp:TextBox>
                <act:FilteredTextBoxExtender ID="fteQtyIssued" runat="server" TargetControlID="txtQtyIssued" FilterMode="ValidChars" ValidChars="0123456789."></act:FilteredTextBoxExtender>
                <asp:RegularExpressionValidator ID="revQtyIssued" runat="server" ValidationExpression="^\d{1,15}(\.\d{0,2})?$" ControlToValidate="txtQtyIssued" ErrorMessage="Invalid format"></asp:RegularExpressionValidator>
                <asp:HiddenField ID="hidQtyIssued" runat="server" />
            </td>
            <td class="colMod" width="20%">
                UOM *
            </td>
            <td class="colDesc" width="30%" colspan="1">
                <asp:DropDownList ID="ddlUOM" CssClass="formsCombo" runat="server">
                </asp:DropDownList>
                <asp:HiddenField ID="hidUOM" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="colMod" width="20%">
                Total Cost *
            </td>
            <td class="colDesc" width="80%" colspan="3">
                <asp:TextBox ID="txtTotalCost" runat="server" CssClass="formsTextNum" Text="0.00" AutoPostBack="true"></asp:TextBox>
                <act:FilteredTextBoxExtender ID="fteTotalCost" runat="server" TargetControlID="txtTotalCost" FilterMode="ValidChars" ValidChars="0123456789."></act:FilteredTextBoxExtender>
                <asp:RegularExpressionValidator ID="revTotalCost" runat="server" ValidationExpression="^\d{1,15}(\.\d{0,2})?$" ControlToValidate="txtTotalCost" ErrorMessage="Invalid format"></asp:RegularExpressionValidator>
                <asp:HiddenField ID="hidTotalCost" runat="server" />
            </td>
       </tr>
        <tr>
            <td class="colMod" width="20%">
            &#160;
            </td>
            <td class="colDesc" width="80%" colspan="3">
            <asp:LinkButton ID="lbtnCancel" CssClass="linkButton" Text="Cancel Direct Issue Item" runat="server"></asp:LinkButton>
            <asp:HiddenField ID="hidMode" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="4" class="errMsg">
                * denotes mandatory fields
            </td>
        </tr>
  </table>
  <br />
  <br />