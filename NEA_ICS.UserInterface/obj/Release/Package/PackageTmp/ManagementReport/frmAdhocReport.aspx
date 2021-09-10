<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmAdhocReport.aspx.vb"
    Inherits="NEA_ICS.UserInterface.frmAdhocReport" ValidateRequest="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Inventory Control System</title>
    <link href="../style/ICS.css" type="text/css" rel="Stylesheet" />
    <meta http-equiv='refresh' content='1800;url=..\frmUnauthorisedPage.aspx' />

    <script type="text/javascript">
        function pageLoad() {
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
    </div>
    <table class="moduleTitle" width="96%" cellspacing="1" cellpadding="1">
        <tr>
            <td class="moduleTitleBorder">
                Management Report > Adhoc Report
            </td>
        </tr>
    </table>    
    <br />
    
    <asp:UpdatePanel ID="uplItem" runat="server">
    <ContentTemplate>
    <asp:Panel ID="pnlAdhocReport" runat="server" Width="96%">
    <table class="tblModule" cellspacing="1">
        <tr>
            <td class="colMod" width="20%">
                Select your View
            </td>
            <td class="colDesc" width="80%" colspan="3">
                <asp:DropDownList ID="ddlView" runat="server" CssClass="formsCombo">
                    <asp:ListItem Text=" - Please Select - " Value=""></asp:ListItem>
                    <asp:ListItem Text="Stock Transaction" Value=" StockTransaction t "></asp:ListItem>
                    <asp:ListItem Text="Order" Value=" [Order] o INNER JOIN OrderItem oi ON o.OrderID = oi.OrderItemOrderID "></asp:ListItem>
                    <asp:ListItem Text="Request/Issue" Value=" Request r INNER JOIN RequestItem ri ON r.RequestID = ri.RequestItemRequestID "></asp:ListItem> 
                    <asp:ListItem Text="Adjustment" Value=" Adjust a INNER JOIN AdjustItem ai ON a.AdjustID = ai.AdjustItemAdjustID "></asp:ListItem>  
                    <asp:ListItem Text="Direct Issue" Value=" DirectIssue d INNER JOIN DirectIssueItem di ON d.DirectIssueID = di.DirectIssueItemDirectIssueID "></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <br />
    <div align="center">
        <asp:Button ID="btnGo" CssClass="formsButton" Text="Go" runat="server" />&nbsp;
        <asp:Button ID="btnClear" CssClass="formsButton" Text="Clear" runat="server" />
    </div>
    </asp:Panel>
    <asp:Panel ID="pnlDetails" runat="server" Width="96%" Visible="false">
    <img src="../Images/select_data_items.gif" alt="Select your data items" />
    
    <table class="tblModule" cellspacing="1">
        <tr>
            <td class="colDesc" width="100%">
            <asp:CheckBoxList ID="chkAdHocReport" runat="server" RepeatDirection="Horizontal" RepeatColumns="5" RepeatLayout="Table"></asp:CheckBoxList>
            </td>
        </tr>
    </table>
    <br />
    <div align="center">
        <asp:Button ID="btnSelectAll" CssClass="formsButtonLarge" Text="Select All" runat="server" />&nbsp;
        <asp:Button ID="btnUnSelectAll" CssClass="formsButtonLarge" Text="UnSelect All" runat="server" />&nbsp;
    </div>
    <img src="../Images/select_conditions.gif" alt="Select your condition" />
    <asp:GridView ID="gdvCondition" runat="server" CssClass="formsGrid" AllowPaging="True"
    Width="100%" CellSpacing="1" BorderWidth="0px" AutoGenerateColumns="False">
    <FooterStyle CssClass="colFooter" />
    <RowStyle CssClass="colRow" />
    <AlternatingRowStyle CssClass="colAltRow" />
    <PagerStyle CssClass="colPager" />
    <SelectedRowStyle CssClass="colSelected" />
    <HeaderStyle CssClass="colHeader" />
    <EmptyDataRowStyle CssClass="colEmpty" />
    <EmptyDataTemplate>
    <p>No records are found.</p>
    </EmptyDataTemplate>
    <Columns>
    <asp:TemplateField HeaderText="Include?">
        <ItemTemplate>
            <asp:CheckBox ID="chkInclude" runat="server" />
        </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Operand">
        <ItemTemplate>
            <asp:DropDownList ID="ddlOperand" runat="server" CssClass="formsCombo" Width="100px">
               <asp:ListItem Text="AND" Value=" AND "></asp:ListItem>
               <asp:ListItem Text="OR" Value=" OR "></asp:ListItem>
            </asp:DropDownList>
        </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Data Item">
        <ItemTemplate>
            <asp:DropDownList ID="ddlDataItem" runat="server" CssClass="formsCombo">
            </asp:DropDownList>
        </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Operator">
        <ItemTemplate>
            <asp:DropDownList ID="ddlOperator" runat="server" CssClass="formsCombo">
                <asp:ListItem Text="Equal To (=)" Value="0"></asp:ListItem>
                <asp:ListItem Text="Not Equal To (<>)" Value="1" ></asp:ListItem>
                <asp:ListItem Text="Greater Than (>)" Value="2"></asp:ListItem>
                <asp:ListItem Text="Greater Than or Equal (>=)" Value="3"></asp:ListItem>
                <asp:ListItem Text="Less Than (<)" Value="4"></asp:ListItem>
                <asp:ListItem Text="Less Than or Equal (<=)" Value="5"></asp:ListItem>
                <asp:ListItem Text="Like (%)" Value="6"></asp:ListItem>
            </asp:DropDownList>
        </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Enter Criteria">
        <ItemTemplate>
            <asp:TextBox ID="txtCriteria" runat="server" CssClass="formsText"></asp:TextBox>
        </ItemTemplate>    
    </asp:TemplateField>                                            
    </Columns>
    </asp:GridView>
    <br />
    <table class="tblModule" cellspacing="1">
        <tr>
            <td class="colHeader" width="20%">
                Sort By
            </td>
            <td class="colAltRow" width="30%">
                <asp:DropDownList ID="ddlSortBy" runat="server" CssClass="formsCombo"></asp:DropDownList>
            </td>
            <td class="colHeader" width="20%">
                Sort Direction
            </td>
            <td class="colAltRow" width="30%">
                <asp:DropDownList ID="ddlSortDirection" runat="server" CssClass="formsCombo">
                    <asp:ListItem Text="Ascending" Value=" ASC "></asp:ListItem>
                    <asp:ListItem Text="Descending" Value=" DESC "></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <br />
    <div align="center">
        <asp:Button ID="btnExcel" CssClass="formsButtonLarge" Text="Export to Excel" runat="server" />
    </div>
    <br />
    <rsweb:ReportViewer ID="rvr" runat="server" Font-Size="8pt" Height="400px" Visible="False"
    Width="100%" Font-Names="Verdana">
    <LocalReport ReportPath="ManagementReport\rptAdHocReport.rdlc">
    <DataSources>
        <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="AdHocReport" />
    </DataSources>
    </LocalReport>
    </rsweb:ReportViewer>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetAdHocReport"
    TypeName="NEA_ICS.UserInterface.NEA_ICS.WcfService.ServiceClient">
    <SelectParameters>
        <asp:Parameter Name="AdHocReport" Type="Object" />
        <asp:Parameter DefaultValue="" Direction="InputOutput" Name="returnMessage" Type="String" />
    </SelectParameters>
    </asp:ObjectDataSource>
    <br />
    </asp:Panel>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnExcel" />
    </Triggers>
    </asp:UpdatePanel>
    
    <asp:UpdateProgress ID="upgItem" runat="server" AssociatedUpdatePanelID="uplItem">
    <ProgressTemplate>
        <br />
        <img src="../images/progress.gif" alt="Processing" />
        <asp:Label ID="lblProgress" CssClass="progress" Text="&nbsp;Processing ..." runat="server"></asp:Label>
    </ProgressTemplate>
    </asp:UpdateProgress>    
    </form>
</body>
</html>
