<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmIssueItem.aspx.vb"
    Inherits="NEA_ICS.UserInterface.frmIssueItem" ValidateRequest="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="DBauer.Web.UI.WebControls.DynamicControlsPlaceholder" Namespace="DBauer.Web.UI.WebControls"
    TagPrefix="dbwc" %>
<%@ Register Src="IssueItem.ascx" TagName="IssueItem" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Inventory Control System</title>
    <link href="../style/ICS.css" type="text/css" rel="Stylesheet" />

    <script src="../Script/NEA_ICS.js" language="javascript" type="text/javascript">
    </script>

    <meta http-equiv='refresh' content='1800;url=..\frmUnauthorisedPage.aspx' />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" EnableScriptGlobalization="true" runat="server" />
        <table class="moduleTitle" width="98%" cellspacing="1" cellpadding="1">
            <tr>
                <td class="moduleTitleBorder">
                    &nbsp;Stock Control > Store Request / Approval and Issue
                </td>
            </tr>
        </table>
        <br />
        <asp:UpdatePanel ID="uplPage" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <act:TabContainer ID="tbcIssueItem" Width="98%" Font-Bold="true" Font-Size="Medium"
                    runat="server" ForeColor="#4D36C2" BackColor="#FFFFFF" 
                    Font-Names="Verdana" ActiveTabIndex="1">
                    <act:TabPanel ID="tbpNew" HeaderText="Store Request" runat="server">
                        <HeaderTemplate>
                            Store Request
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="uplNewIssue" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="uplMain" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlMain" runat="server">
                                                <table class="tblModule" cellspacing="1">
                                                    <tr>
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
                                                            Consumer Code *
                                                        </td>
                                                        <td class="colDesc" width="80%" colspan="3">
                                                            <asp:DropDownList ID="ddlConsumerID" runat="server" CssClass="formsComboLarge">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="colMod" width="20%">
                                                        </td>
                                                        <td class="colDesc" width="80%" colspan="3">
                                                            &nbsp;*&nbsp;
                                                            <asp:CheckBox ID="cbxSought" runat="server" Checked="false" Text="I confirm that I have obtained prior approval from my Manager for this store request." />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4" class="errMsg">
                                                            * denotes mandatory fields
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div id="divAdd" runat="server" align="center">
                                                    <br />
                                                    <asp:Button ID="btnAddIssueItem" CssClass="formsButtonLarge" Text="Add Issue" runat="server" />
                                                    <br />
                                                </div>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:UpdatePanel ID="uplDetail" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlNewIssue" runat="server" Visible="False">
                                                <br />
                                                <asp:Image ID="imgIssueItem" runat="server" ImageUrl="~/Images/request_details.gif"
                                                    AlternateText="Request Details" />
                                                <asp:UpdatePanel ID="uplStockCode" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                                    <ContentTemplate>
                                                        <table class="tblModule" cellspacing="1">
                                                            <tr>
                                                                <td class="colMod" width="20%">
                                                                    Enter a Stock Code for your Request Item
                                                                </td>
                                                                <td class="colDesc" width="80%">                                                       
                                                                    <asp:TextBox ID="txtStockCode" runat="server" CssClass="formsTextNumLarge"></asp:TextBox>
                                                                    &#160;&#160;&#160;
                                                                    <asp:LinkButton ID="btnAddItem" runat="server" CssClass="linkButton" Text="[Add Request Item]"
                                                                        TabIndex="32" />
                                                                    <act:AutoCompleteExtender ID="aceStockCode" runat="server" MinimumPrefixLength="2" CompletionSetCount="20" TargetControlID="txtStockCode"
                                                                     ServiceMethod="GetStockItems" UseContextKey="true"></act:AutoCompleteExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <asp:UpdatePanel ID="uplUserControl" runat="server" UpdateMode="Conditional">
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="btnSubmit" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <dbwc:DynamicControlsPlaceholder ID="DCP" runat="server" ControlsWithoutIDs="DontPersist">
                                                        </dbwc:DynamicControlsPlaceholder>
                                                        <br />
                                                        <div align="center">
                                                            <br />
                                                            <asp:Button ID="btnSubmit" CssClass="formsButton" Text="Save" runat="server" OnClientClick="return confirm('Are you sure you want to save your request?')"
                                                                TabIndex="7" />&#160;&#160;&#160;
                                                            <asp:Button ID="btnCancelAll" CssClass="formsButton" Text="Cancel All" runat="server"
                                                                TabIndex="8" /></div>
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
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdateProgress ID="upgNewIssue" runat="server" AssociatedUpdatePanelID="uplNewIssue">
                                <ProgressTemplate>
                                    <br />
                                    <img src="../images/progress.gif" alt="Processing" />
                                    <asp:Label ID="lblProgress" CssClass="progress" Text="&nbsp;Processing ..." runat="server"></asp:Label></ProgressTemplate>
                            </asp:UpdateProgress>
                        </ContentTemplate>
                    </act:TabPanel>
                    <act:TabPanel ID="tbpLocate" HeaderText="Locate Issue" runat="server">
                        <HeaderTemplate>
                            Locate Issue
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="uplLocateIssue" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlLocateMain" runat="server">
                                        <table class="tblModule" cellspacing="1">
                                            <tr>
                                                <td class="colMod" width="20%">
                                                    Issue Reference
                                                </td>
                                                <td class="colDesc" width="30%">
                                                    <asp:TextBox ID="txtLocateRequestID" runat="server" CssClass="formsText"></asp:TextBox>
                                                </td>
                                                <td class="colMod" width="20%">
                                                    Status
                                                </td>
                                                <td class="colDesc" width="30%">
                                                    <asp:DropDownList ID="ddlLocateStatus" runat="server" CssClass="formsCombo">
                                                        <asp:ListItem Text="All" Value="" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="Approved" Value="A"></asp:ListItem>
                                                        <asp:ListItem Text="Closed" Value="C"></asp:ListItem>
                                                        <asp:ListItem Text="Open" Value="O"></asp:ListItem>
                                                        <asp:ListItem Text="Rejected" Value="R"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="colMod" width="20%">
                                                    Consumer Code
                                                </td>
                                                <td class="colDesc" width="80%" colspan="3">
                                                    <asp:DropDownList ID="ddlLocateConsumerSearch" CssClass="formsComboLarge" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                    </asp:Panel>
                                    <div align="center">
                                        <asp:Button ID="btnLocateGo" CssClass="formsButton" Text="Go" runat="server" />&nbsp;
                                        <asp:Button ID="btnLocateClear" CssClass="formsButton" Text="Clear" runat="server" />
                                    </div>
                                    <asp:Panel ID="pnlSearchResults" runat="server" Visible="false">
                                        <asp:Image ID="imgSearchResults" runat="server" ImageUrl="~/Images/search_results.gif" />
                                        <asp:GridView ID="gdvLocate" runat="server" CssClass="formsGrid" AllowPaging="True"
                                            PageSize="5" Width="100%" AllowSorting="True" CellSpacing="1" BorderWidth="0px"
                                            AutoGenerateColumns="False">
                                            <FooterStyle CssClass="colFooter" />
                                            <RowStyle CssClass="colRow" />
                                            <AlternatingRowStyle CssClass="colAltRow" />
                                            <PagerStyle CssClass="colPager" />
                                            <SelectedRowStyle CssClass="colSelected" />
                                            <HeaderStyle CssClass="colHeader" />
                                            <EmptyDataRowStyle CssClass="colEmpty" />
                                            <EmptyDataTemplate>
                                                <p>
                                                    No records are found.</p>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:CommandField SelectImageUrl="~/Images/select.gif" ButtonType="Image" ShowSelectButton="true"
                                                    ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ConsumerID" HeaderText="Consumer Code" SortExpression="ConsumerID" />
                                                <asp:BoundField DataField="RequestID" HeaderText="Request Reference" SortExpression="RequestID" />
                                                <asp:TemplateField HeaderText="Request By" SortExpression="RequestBy">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hidRequestBy" runat="server" Value='<%# Bind("RequestBy") %>' />
                                                        <asp:Label ID="lblRequestBy" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hidType" runat="server" Value='<%# Bind("Type") %>' />
                                                        <asp:HiddenField ID="hidSerialNo" runat="server" Value='<%# Bind("SerialNo") %>' />
                                                        <asp:HiddenField ID="hidSought" runat="server" Value='<%# Bind("Sought") %>' />
                                                        <asp:HiddenField ID="hidRequestDte" runat="server" Value='<%# Bind("RequestDte") %>' />
                                                        <asp:HiddenField ID="hidApproveDte" runat="server" Value='<%# Bind("ApproveDte") %>' />
                                                        <asp:HiddenField ID="hidApproveBy" runat="server" Value='<%# Bind("ApproveBy") %>' />
                                                        <asp:HiddenField ID="hidIssueDte" runat="server" Value='<%# Bind("IssueDte") %>' />
                                                        <asp:HiddenField ID="hidIssueBy" runat="server" Value='<%# Bind("IssueBy") %>' />
                                                        <asp:HiddenField ID="hidStatus" runat="server" Value='<%# Bind("Status") %>' />
                                                        <asp:Label ID="lblStatus" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                    <br />
                                    <asp:Panel ID="pnllocate" runat="server">
                                        <asp:Image ID="imgLocate" runat="server" ImageUrl="~/Images/issue_details.gif" AlternateText="Issue Details" />
                                        <br />
                                        <asp:Panel ID="pnlLocateSearch" runat="server">
                                            <table class="tblModule" cellspacing="1">
                                                <tr>
                                                    <td class="colMod" width="20%">
                                                        Issue Reference
                                                    </td>
                                                    <td class="colDesc" width="30%" colspan="1">
                                                        <asp:Label ID="lblLocateRequestID" runat="server" CssClass="colLabel"></asp:Label>
                                                    </td>
                                                    <td class="colMod" width="20%">
                                                        Status
                                                    </td>
                                                    <td class="colDesc" valign="middle" width="30%">
                                                        <asp:Label ID="lblLocateStatus" runat="server" CssClass="colLabel"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="colMod" width="20%">
                                                        Consumer Code *
                                                    </td>
                                                    <td class="colDesc" width="30%">
                                                        <asp:Label ID="lblLocateConsumerID" runat="server" CssClass="colLabel"></asp:Label>
                                                    </td>
                                                    <td class="colMod" width="20%">
                                                        Document Type *
                                                    </td>
                                                    <td class="colDesc" width="30%">
                                                        <asp:DropDownList ID="ddlLocateDocType" runat="server" CssClass="formsCombo">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr id="trRequest" runat="server" visible="false">
                                                    <td class="colMod" width="20%">
                                                        Request By
                                                    </td>
                                                    <td class="colDesc" width="30%">
                                                        <asp:Label ID="lblLocateRequestBy" runat="server" Text=""></asp:Label>
                                                    </td>
                                                    <td class="colMod" width="20%">
                                                        Request Date
                                                    </td>
                                                    <td class="colDesc" width="30%">
                                                        <asp:Label ID="lblLocateRequestDate" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="trApprove" runat="server" visible="false">
                                                    <td class="colMod" width="20%">
                                                        Approve By
                                                    </td>
                                                    <td class="colDesc" width="30%">
                                                        <asp:Label ID="lblLocateApproveBy" runat="server" Text=""></asp:Label>
                                                    </td>
                                                    <td class="colMod" width="20%">
                                                        Approve Date
                                                    </td>
                                                    <td class="colDesc" width="30%">
                                                        <asp:Label ID="lblLocateApproveDate" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="colMod" width="20%">
                                                        Issue Date
                                                        <br />
                                                        (dd/mm/yyyy) *
                                                    </td>
                                                    <td class="colDesc" colspan="1" width="30%">
                                                        <asp:TextBox ID="txtLocateIssueDate" runat="server" CssClass="formsTextNum" Enabled="False"></asp:TextBox>
                                                        <act:CalendarExtender ID="calLocateIssueDate" runat="server" CssClass="formsCal"
                                                            Format="dd/MM/yyyy" PopupPosition="BottomLeft" TargetControlID="txtLocateIssueDate">
                                                        </act:CalendarExtender>
                                                        <act:MaskedEditExtender ID="meeLocateIssueDate" runat="server" Mask="99/99/9999"
                                                            MaskType="Date" TargetControlID="txtLocateIssueDate">
                                                        </act:MaskedEditExtender>
                                                    </td>
                                                    <td class="colMod" width="20%">
                                                        Serial No (Last Serial No)
                                                    </td>
                                                    <td class="colDesc" colspan="1" width="30%">
                                                        <asp:TextBox ID="txtLocateSerialNo" runat="server" CssClass="formsTextNum" Enabled="False" MaxLength="12"></asp:TextBox>
                                                        &nbsp;(<asp:Label ID="lblLocateLastSerialNo" runat="server" />)
                                                    </td>
                                                </tr>
                                                <tr id="trIssue" runat="server" visible="true">
                                                    <td class="colMod" width="20%">
                                                        Issue By
                                                    </td>
                                                    <td class="colDesc" width="80%" colspan="3">
                                                        <asp:Label ID="lblLocateIssueBy" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlLocateAccess" runat="server" Enabled="false">
                                            <table cellspacing="1" class="tblModule">
                                                <tr id="trLocateAccess" runat="server">
                                                    <td class="colMod" width="20%">
                                                        &#160;&#160;
                                                    </td>
                                                    <td class="colDesc" colspan="3" width="80%">
                                                        <asp:LinkButton ID="btnLocateEdit" runat="server" CssClass="linkButton" OnClientClick="return confirm('Are you sure you want to edit this Issue?')"
                                                            Text="[Edit Issue]" type="Button"></asp:LinkButton>&#160;
                                                        <asp:LinkButton ID="btnLocateDeleteAll" runat="server" CssClass="linkButton" OnClientClick="return confirm('Are you sure you want to delete this Issue?')"
                                                            Text="[Delete Issue]" type="Button"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="errMsg" colspan="4">
                                                        * denotes mandatory fields
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlLocateSearchItem" runat="server" Enabled="false">
                                            <dbwc:DynamicControlsPlaceholder ID="DCPLocate" runat="server" ControlsWithoutIDs="DontPersist">
                                            </dbwc:DynamicControlsPlaceholder>
                                        </asp:Panel>
                                        <div align="center">
                                            <br />
                                            <asp:Button ID="btnLocateApprove" CssClass="formsButton" Text="Approve" runat="server"
                                                OnClientClick="return confirm('Are you sure you want to Approve the Issue Request?')"
                                                Visible="True" />&#160;&#160;&#160;
                                            <asp:Button ID="btnLocateReject" CssClass="formsButton" Text="Reject" runat="server"
                                                Visible="True" OnClientClick="return confirm('Are you sure you want to Reject the Issue Request?')" />&#160;&#160;&#160;
                                            <asp:Button ID="btnLocateSave" runat="server" CssClass="formsButton" Text="Save"
                                                OnClientClick="return confirm('Are you sure you want to save the changes?')" />&#160;&#160;&#160;
                                            <asp:Button ID="btnLocateCancel" runat="server" CssClass="formsButton" Text="Cancel"
                                                OnClientClick="return confirm('Are you sure you want to cancel the changes?')" /></div>
                                        <br />
                                    </asp:Panel>
                                    <asp:Button ID="ShowLocateModal" runat="server" Text="ShowModal" Visible="false"
                                        Height="1px" Width="1px" />
                                    <act:ModalPopupExtender ID="mpuLocateStockAvailability" runat="server" BackgroundCssClass="modalBackground"
                                        CancelControlID="btnLocateExit" OkControlID="btnLocateExit" PopupControlID="pnlLocateStockAvailability"
                                        TargetControlID="ShowLocateModal">
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
                                                    <td class="colHeader" colspan="2">
                                                        Before Stock Issue (indicative)
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
                                                    <td class="colHeader" colspan="2">
                                                        After Stock Issue (indicative)
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
                                                <asp:Button ID="btnLocateExit" CssClass="formsButton" Text="Exit" runat="server"
                                                    OnClick="btnLocateExit_Click" />
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdateProgress ID="upgLocate" runat="server" AssociatedUpdatePanelID="uplLocateIssue">
                                <ProgressTemplate>
                                    <br />
                                    <img src="../images/progress.gif" alt="Processing" />
                                    <asp:Label ID="lblProgress" CssClass="progress" Text="&nbsp;Processing ..." runat="server"></asp:Label>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </ContentTemplate>
                    </act:TabPanel>
                    <act:TabPanel ID="tbpPrint" HeaderText="Issue Report" runat="server">
                        <HeaderTemplate>
                            Issue Report
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="uplPrintIssue" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnPDF" />
                                    <asp:PostBackTrigger ControlID="btnExcel" />
                                    <asp:PostBackTrigger ControlID="btnClear" />
                                </Triggers>
                                <ContentTemplate>
                                    <table class="tblModule" cellspacing="1">
                                        <tr id="trStatus" runat="server" visible="false" >
                                            <td class="colMod" width="20%">
                                                Issue Status
                                            </td>
                                            <td class="colDesc" width="30%" colspan="3">
                                                <asp:DropDownList ID="ddlPrintOption" CssClass="formsCombo" runat="server">
                                                    <asp:ListItem Text="All" Value=""></asp:ListItem>
                                                    <asp:ListItem Text="Approved" Value="A"></asp:ListItem>
                                                    <asp:ListItem Text="Closed" Value="C"></asp:ListItem>
                                                    <asp:ListItem Text="Open" Value="O"></asp:ListItem>
                                                    <asp:ListItem Text="Rejected" Value="R"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="colMod" width="20%">
                                                Issue Date From *<br />(dd/mm/yyyy)
                                            </td>
                                            <td class="colDesc" width="30%">
                                                <asp:TextBox ID="txtIssueDateFrom" runat="server" CssClass="formsText" Text="01/11/2008"></asp:TextBox>
                                                <act:CalendarExtender ID="CalendarExtender3" runat="server" CssClass="formsCal" Format="dd/MM/yyyy"
                                                    PopupPosition="BottomLeft" TargetControlID="txtIssueDateFrom">
                                                </act:CalendarExtender>
                                                <act:MaskedEditExtender ID="MaskedEditExtender3" runat="server" Mask="99/99/9999"
                                                    MaskType="Date" TargetControlID="txtIssueDateFrom">
                                                </act:MaskedEditExtender>
                                            </td>
                                            <td class="colMod" width="20%">
                                                Issue Date To *<br />(dd/mm/yyyy)
                                            </td>
                                            <td class="colDesc" width="30%">
                                                <asp:TextBox ID="txtIssueDateTo" runat="server" CssClass="formsText"></asp:TextBox>
                                                <act:CalendarExtender ID="CalendarExtender4" runat="server" CssClass="formsCal" Format="dd/MM/yyyy"
                                                    PopupPosition="BottomLeft" TargetControlID="txtIssueDateTo">
                                                </act:CalendarExtender>
                                                <act:MaskedEditExtender ID="MaskedEditExtender4" runat="server" Mask="99/99/9999"
                                                    MaskType="Date" TargetControlID="txtIssueDateTo">
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
                                        <tr>
                                            <td class="colMod" width="20%">
                                                Consumer Code 
                                            </td>
                                            <td class="colDesc" width="80%" colspan="3">
                                                <asp:DropDownList ID="ddlPrintConsumerID" runat="server" CssClass="formsComboLarge">
                                                </asp:DropDownList>
                                            </td>  
                                        </tr>
                                    </table>
                                    <asp:UpdatePanel ID="uplPrintRequestID" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table class="tblModule" cellspacing="1">
                                                <tr>
                                                    <td class="colMod" width="20%">
                                                        Issue Reference #
                                                    </td>
                                                    <td class="colDesc" width="80%" colspan="3">
                                                        <asp:DropDownList ID="ddlIssueReference" runat="server" CssClass="formsCombo">
                                                        </asp:DropDownList>
                                                    </td>                                                    
                                                </tr>
                                                <tr>
                                                    <td colspan="4" class="errMsg">
                                                        * denotes mandatory fields ; # When this is selected, other selection will be by
                                                        passed
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
                                        <asp:Button ID="btnClear" CssClass="formsButton" Text="Clear" runat="server" TabIndex="23"
                                             />
                                    </div>
                                    <rsweb:ReportViewer ID="rvr" runat="server" Font-Size="8pt" Height="400px" Visible="False"
                                        Width="100%" Font-Names="Verdana">
                                        <LocalReport ReportPath="StockControl\rptIssueList.rdlc">
                                            <DataSources>
                                                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="IssueList" />
                                            </DataSources>
                                        </LocalReport>
                                    </rsweb:ReportViewer>
                                    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetIssueList"
                                        TypeName="NEA_ICS.UserInterface.NEA_ICS.WcfService.ServiceClient">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="" Name="storeID" Type="String" />
                                            <asp:Parameter DefaultValue="" Name="fromDte" Type="DateTime" />
                                            <asp:Parameter DefaultValue="" Name="toDte" Type="DateTime" />
                                            <asp:Parameter DefaultValue="" Name="fromStockItemID" Type="String" />
                                            <asp:Parameter DefaultValue="" Name="toStockItemID" Type="String" />
                                            <asp:Parameter DefaultValue="" Name="requestId" Type="String" />
                                            <asp:Parameter DefaultValue="" Name="consumerID" Type="String" />
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
