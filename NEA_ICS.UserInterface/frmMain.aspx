<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmMain.aspx.vb" Inherits="NEA_ICS.UserInterface.frmMain" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Inventory Control System (ICS)</title>
    <script type="text/javascript">
    
      function pageLoad() {
      }
    
    </script>
</head>
<link href="style/ICS.css" type="text/css" rel="Stylesheet" />
<body style="margin-top: 0; margin-left: 1;">
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <br /><br />
<%--<img src="Images/Reorder.gif" alt="Reorder Level" visible="false" />--%>
<br />
<asp:GridView ID="gdvReorder" Width="88%" runat="server" CssClass="formsGrid" AllowPaging="false" PageSize="5" 
AllowSorting="True" CellSpacing="1" BorderWidth="0px" AutoGenerateColumns="False"> 
<FooterStyle cssclass="colFooter" />
<RowStyle CssClass="colRow" />
<AlternatingRowStyle CssClass="colAltRow" />
<PagerStyle CssClass="colPager" />
<SelectedRowStyle cssclass="colSelected" />
<HeaderStyle cssclass="colHeader" />  
<EmptyDataRowStyle CssClass="colEmpty" />
<EmptyDataTemplate>
    <p>No records are found.</p>
</EmptyDataTemplate>               
<Columns>
<asp:TemplateField ItemStyle-Width="10%" SortExpression="Stock_Code" HeaderText="Stock Code">
    <ItemTemplate>
        <asp:Label ID="lblStockCode" runat="server" Text='<%# Bind("Stock_Code") %>'></asp:Label>
    </ItemTemplate>
    <ItemStyle Width="10%" />
</asp:TemplateField>
<asp:TemplateField ItemStyle-Width="20%" SortExpression="Description" HeaderText="Description">
    <ItemTemplate>
        <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
    </ItemTemplate>
    <ItemStyle Width="20%" />
</asp:TemplateField>
<asp:TemplateField ItemStyle-Width="10%" SortExpression="Document_No" HeaderText="Equipment Code">
    <ItemTemplate>
        <asp:Label ID="lblEquipmentNo" runat="server" Text='<%# Bind("Document_No") %>'></asp:Label>
    </ItemTemplate>
    <ItemStyle Width="10%" />
</asp:TemplateField>
<asp:TemplateField ItemStyle-Width="15%" SortExpression="Reorder_Level" HeaderText="Reorder Level">
    <ItemTemplate>
        <asp:Label ID="lblReorderLevel" runat="server" Text='<%# Bind("Reorder_Level") %>'></asp:Label>
    </ItemTemplate>
    <ItemStyle Width="10%" />
</asp:TemplateField>
<asp:TemplateField ItemStyle-Width="15%" SortExpression="Balanced_Qty" HeaderText="Current Balance">
    <ItemTemplate>
        <asp:Label ID="lblReorderLevel" runat="server" Text='<%# Bind("Balanced_Qty") %>'></asp:Label>
    </ItemTemplate>
    <ItemStyle Width="10%" />
</asp:TemplateField>
</Columns>
</asp:GridView>
<br />
<%--<img src="Images/ItemReceived.gif" alt="Reorder Level" visible="false" runat="server" />--%>
<br />
<asp:GridView ID="GridView1" Width="88%" runat="server" CssClass="formsGrid" AllowPaging="false" PageSize="5" 
AllowSorting="True" CellSpacing="1" BorderWidth="0px" AutoGenerateColumns="False"> 
<FooterStyle cssclass="colFooter" />
<RowStyle CssClass="colRow" />
<AlternatingRowStyle CssClass="colAltRow" />
<PagerStyle CssClass="colPager" />
<SelectedRowStyle cssclass="colSelected" />
<HeaderStyle cssclass="colHeader" />  
<EmptyDataRowStyle CssClass="colEmpty" />
<EmptyDataTemplate>
    <p>No records are found.</p>
</EmptyDataTemplate>               
<Columns>
<asp:TemplateField ItemStyle-Width="15%" SortExpression="Stock_Code" HeaderText="Stock Code">
    <ItemTemplate>
        <asp:Label ID="lblStockCode" runat="server" Text='<%# Bind("Stock_Code") %>'></asp:Label>
    </ItemTemplate>
    <ItemStyle Width="15%" />
</asp:TemplateField>
<asp:TemplateField ItemStyle-Width="20%" SortExpression="Order_Reference" HeaderText="Order Reference">
    <ItemTemplate>
        <asp:Label ID="lblEquipmentNo" runat="server" Text='<%# Bind("Order_Reference") %>'></asp:Label>
    </ItemTemplate>
    <ItemStyle Width="20%" />
</asp:TemplateField>
<asp:TemplateField ItemStyle-Width="10%" SortExpression="Date_Of_Delivery" HeaderText="Date Item Received">
    <ItemTemplate>
        <asp:Label ID="lblEquipmentNo" runat="server" Text='<%# Bind("Date_Of_Delivery") %>'></asp:Label>
    </ItemTemplate>
    <ItemStyle Width="10%" />
</asp:TemplateField>
<asp:TemplateField ItemStyle-Width="10%" SortExpression="Unit_Cost" HeaderText="Unit Cost">
    <ItemTemplate>
        <asp:Label ID="lblUnitCost" runat="server" Text='<%# Bind("Unit_Cost","{0:c}") %>'></asp:Label>
    </ItemTemplate>
    <ItemStyle Width="10%" />
</asp:TemplateField>
<asp:TemplateField ItemStyle-Width="10%" SortExpression="Total_Cost" HeaderText="Total Cost">
    <ItemTemplate>
        <asp:Label ID="lblReorderLevel" runat="server" Text='<%# Bind("Total_Cost", "{0:c}") %>'></asp:Label>
    </ItemTemplate>
    <ItemStyle Width="10%" />
</asp:TemplateField>
<asp:TemplateField ItemStyle-Width="20%" SortExpression="Remarks" HeaderText="Remarks">
    <ItemTemplate>
        <asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("Remarks") %>'></asp:Label>
    </ItemTemplate>
    <ItemStyle Width="20%" />
</asp:TemplateField>
</Columns>
</asp:GridView>

    </div>
    </form>
</body>
</html>
