<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmItem.aspx.vb" Inherits="NEA_ICS.UserInterface.frmItem" ValidateRequest="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Inventory Control System</title>

    <style type="text/css">
        body
        {
            margin-top: 0;
            margin-left: 0;
        }
    </style>
    <link href="../style/ICS.css" type="text/css" rel="Stylesheet" />
    <script src="../Script/NEA_ICS.js" language="javascript" type="text/javascript"></script>
    
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"  />
        <table class="moduleTitle" width="98%" cellspacing="1" cellpadding="1">
            <tr>
                <td class="moduleTitleBorder">
                     Master List > Item
                </td>
            </tr>
        </table>
        <br />
        <act:TabContainer ID="tbcItem" Width="98%" Font-Bold="true" Font-Size="Medium" runat="server"
            ForeColor="#4D36C2" BackColor="#FFFFFF" Font-Names="Verdana" ActiveTabIndex="0"
            Visible="False">
            <act:TabPanel ID="tbpNewItem" HeaderText="New Item" runat="server">
                <HeaderTemplate>
                    New Item
                </HeaderTemplate>
                <ContentTemplate>
                     
                    <asp:UpdatePanel ID="uplNewItem" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                                <asp:UpdatePanel ID="uplChild2" runat="server" UpdateMode="Conditional">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnAddItem" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnAddClear" EventName="Click" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <asp:Label ID="lblErrAddItem" runat="server" CssClass="errMsg" Visible="true"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="uplParent" runat="server" UpdateMode="Conditional">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="lbtnGenerateStockCode" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnAddItem" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnAddClear" EventName="Click" />
                                    </Triggers>
                                    <ContentTemplate>
                                    <asp:Panel ID="pnlAddItem" runat="server">
                                    <table class="tblModule" cellspacing="0" style="border-right: #888888 1px solid;">
                                        <tr>
                                            <td class="colMod" width="20%" valign="top" style="border-top: #888888 1px solid; border-left: #888888 1px solid;">
                                                Stock Code *
                                            </td>
                                            <td class="colDesc" width="30%" valign="top" style="border-top: #888888 1px solid; border-left: #888888 1px solid;">
                                                <asp:TextBox ID="txtAddStockCode" CssClass="formsText" AutoPostBack="true" runat="server" MaxLength="10"></asp:TextBox>                                                
                                            </td>
                                            <td class="colMod" width="20%" valign="top" style="border-top: #888888 1px solid; border-left: #888888 1px solid;">
                                                Part No
                                            </td>
                                            <td class="colDesc" width="30%" valign="top" style="border-top: #888888 1px solid; border-left: #888888 1px solid;">
                                                <asp:TextBox ID="txtAddPartNo" CssClass="formsText" runat="server" MaxLength="20"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="uplChild" runat="server" UpdateMode="Conditional">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="txtAddStockCode" EventName="TextChanged" />
                                    </Triggers>
                                    <ContentTemplate>
                                    <asp:Panel ID="pnlAddItem2" runat="server">
                                    <table class="tblModule" cellspacing="1">
                                    <tr>
                                        <td class="colMod" width="20%">
                                            &nbsp;
                                        </td>
                                        <td class="colDesc" width="80%" colspan="3">
                                            <asp:LinkButton ID="lbtnGenerateStockCode" CssClass="linkButton" Text="Generate Stock Code" runat="server" Visible="false"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Description *
                                        </td>
                                        <td class="colDesc" width="80%" colspan="3">
                                            <asp:TextBox ID="txtAddDescription" CssClass="formsText" runat="server" Width="99%" MaxLength="100"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Stock Type *
                                        </td>
                                        <td class="colDesc" width="30%">
                                            <asp:DropDownList ID="ddlAddStockType" runat="server" CssClass="formsCombo">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="colMod" width="20%">
                                            Sub Type *
                                        </td>
                                        <td class="colDesc" width="30%">
                                            <asp:DropDownList ID="ddlAddSubType" runat="server" CssClass="formsCombo">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            UOM *
                                        </td>
                                        <td class="colDesc" width="80%" colspan="3">
                                            <asp:DropDownList ID="ddlAddUOM" runat="server" CssClass="formsCombo">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Equipment Code
                                        </td>                                   
                                        <td class="colDesc" width="80%" colspan="3">                                            
                                            <asp:DropDownList ID="ddlAddEquipmentNo" runat="server" CssClass="formsComboLarge">
                                            </asp:DropDownList>                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Location *
                                        </td>
                                        <td class="colDesc" width="30%">
                                            <asp:TextBox ID="txtAddLocation1" CssClass="formsText" runat="server" MaxLength="15"></asp:TextBox>
                                        </td>
                                        <td class="colMod" width="20%">
                                            2nd Location
                                        </td>
                                        <td class="colDesc" width="30%">
                                            <asp:TextBox ID="txtAddLocation2" CssClass="formsText" runat="server" MaxLength="15"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Min Level *
                                        </td>
                                        <td class="colDesc" width="30%" colspan="1">
                                            <asp:TextBox ID="txtAddMinLevel" CssClass="formsTextNum" runat="server" Text="0.00"></asp:TextBox>
                                            <act:FilteredTextBoxExtender ID="ftbeAddMinLevel" runat="server" ValidChars="0123456789." TargetControlID="txtAddMinLevel"></act:FilteredTextBoxExtender>
                                            <asp:RegularExpressionValidator ID="revAddMinLevel" runat="server" ValidationExpression="^\d{1,15}(\.\d{0,2})?$" ControlToValidate="txtAddMinLevel" ErrorMessage="Invalid format"></asp:RegularExpressionValidator>
                                        </td>
                                        <td class="colMod" width="20%">
                                            Max Level *
                                        </td>
                                        <td class="colDesc" width="30%" colspan="1">
                                            <asp:TextBox ID="txtAddMaxLevel" CssClass="formsTextNum" runat="server" Text="1.00"></asp:TextBox>
                                            <act:FilteredTextBoxExtender ID="ftbeAddMaxLevel" runat="server" ValidChars="0123456789." TargetControlID="txtAddMaxLevel"></act:FilteredTextBoxExtender>
                                            <asp:RegularExpressionValidator ID="revAddMaxLevel" runat="server" ValidationExpression="^\d{1,15}(\.\d{0,2})?$" ControlToValidate="txtAddMaxLevel" ErrorMessage="Invalid format"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Reorder Level *
                                        </td>
                                        <td class="colDesc" width="30%" colspan="3">
                                            <asp:TextBox ID="txtAddReorderLevel" CssClass="formsTextNum" runat="server" Text="0.00"></asp:TextBox>
                                            <act:FilteredTextBoxExtender ID="ftbeAddReorderLevel" runat="server" ValidChars="0123456789." TargetControlID="txtAddReorderLevel"></act:FilteredTextBoxExtender>
                                            <asp:RegularExpressionValidator ID="revAddReorderLevel" runat="server" ValidationExpression="^\d{1,15}(\.\d{0,2})?$" ControlToValidate="txtAddReorderLevel" ErrorMessage="Invalid format"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Initial Opening Balance *
                                        </td>
                                        <td class="colDesc" width="30%">
                                            <asp:TextBox ID="txtAddOpeningBal" CssClass="formsTextNum" runat="server" Text="0.00"></asp:TextBox>
                                            <act:FilteredTextBoxExtender ID="ftbeAddOpeningBal" runat="server" ValidChars="0123456789." TargetControlID="txtAddOpeningBal"></act:FilteredTextBoxExtender>
                                            <asp:RegularExpressionValidator ID="revAddOpeningBal" runat="server" ValidationExpression="^\d{1,15}(\.\d{0,2})?$" ControlToValidate="txtAddOpeningBal" ErrorMessage="Invalid format"></asp:RegularExpressionValidator>
                                        </td>
                                        <td class="colMod" width="20%">
                                            Initial Opening Value *
                                        </td>
                                        <td class="colDesc" width="30%">
                                            <asp:TextBox ID="txtAddOpeningTotalValue" CssClass="formsTextNum" runat="server" Text="0.00"></asp:TextBox>
                                            <act:FilteredTextBoxExtender ID="ftbeAddOpeningTotalValue" runat="server" ValidChars="0123456789." TargetControlID="txtAddOpeningTotalValue"></act:FilteredTextBoxExtender>
                                            <asp:RegularExpressionValidator ID="revAddOpeningTotalValue" runat="server" ValidationExpression="^\d{1,15}(\.\d{0,2})?$" ControlToValidate="txtAddOpeningTotalValue" ErrorMessage="Invalid format"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Avg Unit Cost <br /> (display purpose)
                                        </td>
                                        <td class="colDesc" width="30%" colspan="3">
                                            <asp:Label ID="lblAUC" runat="server" Text="0.0000" CssClass="colLabel"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" class="errMsg">
                                            * denotes mandatory fields
                                        </td>
                                    </tr>
                                    </table>                                    
                                    <br />
                                    <div align="center">
                                        <asp:Button ID="btnAddItem" CssClass="formsButton" Text="Save" runat="server" OnClientClick="return confirm('Are you sure you want to save your item?')" />
                                        <asp:Button ID="btnAddClear" CssClass="formsButton" Text="Clear" runat="server" />
                                    </div>
                                    <br />
                                    
                                    </asp:Panel>    
                                    
                                    <div id="divMsgBox" class="msg" runat="server" visible="false" width="98%">
                                    <table class="moduleTitle" width="98%" cellspacing="1" cellpadding="1">
                                    <tr>
                                        <td class="moduleTitleBorder">
                                            Item Status
                                        </td>
                                    </tr>
                                    </table>
                                    <br />
                                    Your Item has been saved successfully. Here are the detail of your item:
                                    <br />
                                    <br />
                                    <span lang="en-sg">Item Information</span>
                                    <br />
                                    <table class="tblModule" cellspacing="1">
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Stock Code
                                        </td>
                                        <td class="colRow" width="70%">
                                            <asp:Label ID="lblMsgAddStockCode" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Part No
                                        </td>
                                        <td class="colAltRow" width="70%">
                                            <asp:Label ID="lblMsgAddPartNo" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Description
                                        </td>
                                        <td class="colRow" width="70%">
                                            <asp:Label ID="lblMsgAddDescription" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Stock Type
                                        </td>
                                        <td class="colAltRow" width="70%">
                                            <asp:Label ID="lblMsgAddStockType" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Sub Type
                                        </td>
                                        <td class="colRow" width="70%">
                                            <asp:Label ID="lblMsgAddSubType" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            UOM
                                        </td>
                                        <td class="colAltRow" width="70%">
                                            <asp:Label ID="lblMsgAddUOM" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Equipment No
                                        </td>
                                        <td class="colRow" width="70%">
                                            <asp:Label ID="lblMsgAddEquipmentNo" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Location
                                        </td>
                                        <td class="colAltRow" width="70%">
                                            <asp:Label ID="lblMsgAddLocation1" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            2nd Location
                                        </td>
                                        <td class="colRow" width="70%">
                                            <asp:Label ID="lblMsgAddLocation2" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Min Level
                                        </td>
                                        <td class="colAltRow" width="70%">
                                            <asp:Label ID="lblMsgAddMinLevel" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Max Level
                                        </td>
                                        <td class="colRow" width="70%">
                                            <asp:Label ID="lblMsgAddMaxLevel" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Reorder Level
                                        </td>
                                        <td class="colAltRow" width="70%">
                                            <asp:Label ID="lblMsgAddReorderLevel" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Opening Balance
                                        </td>
                                        <td class="colRow" width="70%">
                                            <asp:Label ID="lblMsgAddOpeningBalance" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Opening Total Value
                                        </td>
                                        <td class="colAltRow" width="70%">
                                            $ <asp:Label ID="lblMsgAddOpeningTotalValue" runat="server"></asp:Label>
                                        </td>
                                    </tr>                                    
                                    </table>
                                    <br />
                                    <div align="center">
                                        <asp:Button ID="btnAddItemOK" CssClass="formsButton" Text="OK" runat="server" />
                                    </div>
                                    <br />
                                </div>
                                                      
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnAddItemOK" />
                                </Triggers>
                                </asp:UpdatePanel>                                                                 
                                
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdateProgress ID="upgNewItem" runat="server" AssociatedUpdatePanelID="uplNewItem">
                        <ProgressTemplate>
                            <br />
                            <img src="../images/progress.gif" alt="Processing" />
                            <asp:Label ID="lblProgress" CssClass="progress" Text="&nbsp;Processing ..." runat="server"></asp:Label>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </ContentTemplate>
            </act:TabPanel>
            <act:TabPanel ID="tbpLocateItem" HeaderText="Locate Item" runat="server">
                <HeaderTemplate>
                    Locate Item
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:UpdatePanel ID="uplLocateItem" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblErrLocateItem" runat="server" CssClass="errMsg" Visible="false"></asp:Label>
                            <table class="tblModule" cellspacing="1">
                                <tr>
                                    <td class="colMod" width="20%">
                                        Stock Code
                                    </td>
                                    <td class="colDesc" width="30%">
                                        <asp:TextBox ID="txtLocateStockCode" CssClass="formsText" runat="server" MaxLength="10"></asp:TextBox>
                                    </td>
                                    <td class="colMod" width="20%">
                                        Location
                                    </td>
                                    <td class="colDesc" width="30%">
                                        <asp:TextBox ID="txtLocateLocation" CssClass="formsText" runat="server" MaxLength="10"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="colMod" width="20%">
                                        Description
                                    </td>
                                    <td class="colDesc" width="30%" colspan="3">
                                        <asp:TextBox ID="txtLocateDescription" CssClass="formsText" Width="99%" runat="server" MaxLength="100"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="colMod" width="20%">
                                        Status
                                    </td>
                                    <td class="colDesc" width="30%" colspan="3">
                                        <asp:DropDownList ID="ddlLocateStatus" CssClass="formsCombo" runat="server">
                                            <asp:ListItem Text="All" Value=""></asp:ListItem>
                                            <asp:ListItem Text="Closed" Value="C"></asp:ListItem>                                            
                                            <asp:ListItem Text="Open" Value="O" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <div align="center">
                                <asp:Button ID="btnLocateGo" CssClass="formsButton" Text="Go" runat="server" />&nbsp;
                                <asp:Button ID="btnLocateClear" CssClass="formsButton" Text="Clear" runat="server" />
                            </div>
                            <asp:Panel ID="pnlSearchResults" runat="server" Visible="false">
                                <asp:Image ID="imgSearchResults" runat="server" ImageUrl="~/Images/search_results.gif" />
                                <asp:GridView ID="gdvLocateItem" runat="server" CssClass="formsGrid" AllowPaging="True"
                                    PageSize="5" Width="100%" AllowSorting="True" CellSpacing="1"
                                    BorderWidth="0px" AutoGenerateColumns="False">
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
                                        <asp:CommandField SelectImageUrl="~/Images/select.gif" ButtonType="Image" ShowSelectButton="true" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="ItemID" HeaderText="Stock Code" SortExpression="StockItemID" />
                                        <asp:BoundField DataField="ItemDescription" HeaderText="Stock Description" SortExpression="StockItemDescription" />
                                        <asp:BoundField DataField="Location" HeaderText="Location" SortExpression="StockItemLocation" />
                                        <asp:BoundField DataField="Location2" HeaderText="Location 2" SortExpression="StockItemLocation2" />
                                        <asp:TemplateField HeaderText="Status" SortExpression="StockItemStatus">
                                            <ItemTemplate>                                                                                              
                                                <asp:HiddenField ID="hidEquipmentID" runat="server" Value='<%# Bind("EquipmentID") %>' />
                                                <asp:HiddenField ID="hidPartNo" runat="server" Value='<%# Bind("PartNo") %>' />
                                                <asp:HiddenField ID="hidStockType" runat="server" Value='<%# Bind("StockType") %>' />
                                                <asp:HiddenField ID="hidSubType" runat="server" Value='<%# Bind("SubType") %>' />
                                                <asp:HiddenField ID="hidUOM" runat="server" Value='<%# Bind("UOM") %>' />
                                                <asp:HiddenField ID="hidMinLevel" runat="server" Value='<%# Bind("MinLevel") %>' />
                                                <asp:HiddenField ID="hidReorderLevel" runat="server" Value='<%# Bind("ReorderLevel") %>' />
                                                <asp:HiddenField ID="hidMaxLevel" runat="server" Value='<%# Bind("MaxLevel") %>' />
                                                <asp:HiddenField ID="hidStatus" runat="server" Value='<%# Bind("Status") %>' />
                                                <asp:Label ID="lblStatus" runat="server" ></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField> 
                                    </Columns>
                                </asp:GridView>               
                            </asp:Panel>
                            <br />
                            <asp:Panel ID="pnlItemInfo" runat="server" Visible="false">
                                <asp:Image ID="imgItemDetails" runat="server" ImageUrl="~/Images/item_details.gif" AlternateText="Item Details" />
                                <br />
                                <asp:Label ID="lblErrSaveItem" runat="server" CssClass="errMsg" Visible="false"></asp:Label>
                                <table class="tblModule" cellspacing="1">
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Stock Code
                                        </td>
                                        <td class="colDesc" width="30%">
                                            <asp:Label ID="lblEditStockCode" runat="server"></asp:Label>
                                        </td>
                                        <td class="colMod" width="20%">
                                            Part No
                                        </td>
                                        <td class="colDesc" width="30%">
                                            <asp:TextBox ID="txtEditPartNo" CssClass="formsText" runat="server" maxlength="20"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Description *
                                        </td>
                                        <td class="colDesc" width="80%" colspan="3">
                                            <asp:TextBox ID="txtEditDescription" CssClass="formsText" runat="server" Width="99%" maxlength="100"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Stock Type *
                                        </td>
                                        <td class="colDesc" width="30%">
                                            <asp:DropDownList ID="ddlEditStockType" runat="server" CssClass="formsCombo">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="colMod" width="20%">
                                            Sub Type *
                                        </td>
                                        <td class="colDesc" width="30%">
                                            <asp:DropDownList ID="ddlEditSubType" runat="server" CssClass="formsCombo">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            UOM *
                                        </td>
                                        <td class="colDesc" width="80%" colspan="3">
                                            <asp:DropDownList ID="ddlEditUOM" runat="server" CssClass="formsCombo">
                                            </asp:DropDownList>
                                        </td>
                                   </tr>
                                   <tr>
                                        <td class="colMod" width="20%">
                                            Equipment No
                                        </td>
                                        <td class="colDesc" width="80%" colspan="3">
                                            <asp:DropDownList ID="ddlEditEquipmentNo" runat="server" CssClass="formsComboLarge">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Location *
                                        </td>
                                        <td class="colDesc" width="30%">
                                            <asp:TextBox ID="txtEditLocation" CssClass="formsText" runat="server" maxlength="15"></asp:TextBox>
                                        </td>
                                        <td class="colMod" width="20%">
                                            2nd Location
                                        </td>
                                        <td class="colDesc" width="30%">
                                            <asp:TextBox ID="txtEditLocation2" CssClass="formsText" runat="server" maxlength="15"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Min Level *
                                        </td>
                                        <td class="colDesc" width="30%" colspan="1">
                                            <asp:TextBox ID="txtEditMinLevel" CssClass="formsTextNum" runat="server"></asp:TextBox>
                                            <act:FilteredTextBoxExtender ID="ftbeEditMinLevel" runat="server" ValidChars="1234567890." TargetControlID="txtEditMinLevel"></act:FilteredTextBoxExtender>
                                            <asp:RegularExpressionValidator ID="revEditMinLevel" runat="server" ValidationExpression="^\d{1,15}(\.\d{0,2})?$" ControlToValidate="txtEditMinLevel" ErrorMessage="Invalid format"></asp:RegularExpressionValidator>
                                        </td>
                                        <td class="colMod" width="20%">
                                            Max Level *
                                        </td>
                                        <td class="colDesc" width="30%" colspan="1">
                                            <asp:TextBox ID="txtEditMaxLevel" CssClass="formsTextNum" runat="server"></asp:TextBox>
                                            <act:FilteredTextBoxExtender ID="ftbeEditMaxLevel" runat="server" ValidChars="1234567890." TargetControlID="txtEditMaxLevel"></act:FilteredTextBoxExtender>
                                            <asp:RegularExpressionValidator ID="revEditMaxLevel" runat="server" ValidationExpression="^\d{1,15}(\.\d{0,2})?$" ControlToValidate="txtEditMaxLevel" ErrorMessage="Invalid format"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Reorder Level *
                                        </td>
                                        <td class="colDesc" width="30%" colspan="3">
                                            <asp:TextBox ID="txtEditReOrderLevel" CssClass="formsTextNum" runat="server"></asp:TextBox>
                                            <act:FilteredTextBoxExtender ID="ftbeEditReOrderLevel" runat="server" ValidChars="1234567890." TargetControlID="txtEditReOrderLevel"></act:FilteredTextBoxExtender>
                                            <asp:RegularExpressionValidator ID="revEditReorderLevel" runat="server" ValidationExpression="^\d{1,15}(\.\d{0,2})?$" ControlToValidate="txtEditReorderLevel" ErrorMessage="Invalid format"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Opening Balance *
                                        </td>
                                        <td class="colDesc" width="30%">
                                            <asp:TextBox ID="txtEditOpeningBal" CssClass="formsTextNum" runat="server" Text="0.00"></asp:TextBox>
                                            <act:FilteredTextBoxExtender ID="ftbeEditOpeningBal" runat="server" ValidChars="1234567890." TargetControlID="txtEditOpeningBal"></act:FilteredTextBoxExtender>                                            
                                            <asp:RegularExpressionValidator ID="revEditOpeningBal" runat="server" ValidationExpression="^\d{1,15}(\.\d{0,2})?$" ControlToValidate="txtEditOpeningBal" ErrorMessage="Invalid format"></asp:RegularExpressionValidator>
                                        </td>
                                        <td class="colMod" width="20%">
                                            Opening Total Value *
                                        </td>
                                        <td class="colDesc" width="30%">
                                            <asp:TextBox ID="txtEditOpeningTotalValue" CssClass="formsTextNum" runat="server" Text="0.00"></asp:TextBox>
                                            <act:FilteredTextBoxExtender ID="ftbeEditOpeningTotalValue" runat="server" ValidChars="1234567890." TargetControlID="txtEditOpeningTotalValue"></act:FilteredTextBoxExtender>
                                            <asp:RegularExpressionValidator ID="revEditOpeningTotalValue" runat="server" ValidationExpression="^\d{1,15}(\.\d{0,2})?$" ControlToValidate="txtEditOpeningTotalValue" ErrorMessage="Invalid format"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Avg Unit Cost <br /> (indicative)
                                        </td>
                                        <td class="colDesc" width="30%" colspan="1">
                                            <asp:Label ID="lblDisplayAUC" runat="server" Text="0.0000"></asp:Label>
                                            <asp:HiddenField ID="hidTransactiondate" runat="server" />
                                        </td>
                                        <td class="colMod" width="20%">
                                            Status
                                        </td>
                                        <td class="colDesc" width="30%" colspan="1">
                                            <asp:Label ID="lblDisplayStatus" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                        </td>
                                        <td class="colDesc" width="80%" colspan="3">
                                            <asp:LinkButton ID="lbtnLocateEdit" runat="server" CssClass="linkButton" Text="[Edit Item]"></asp:LinkButton>&nbsp;&nbsp;
                                            <asp:LinkButton ID="lbtnLocateDel" runat="server" CssClass="linkButton" Text=" [Delete Item]"
                                                OnClientClick="return confirm('Are you sure you want to Delete this Item?')"></asp:LinkButton>&nbsp;&nbsp;
                                            <asp:LinkButton ID="lbtnLocateClose" runat="server" CssClass="linkButton" Text=" [Close Item]"
                                                OnClientClick="return confirm('Are you sure you want to Close this Item?')"></asp:LinkButton>&nbsp;&nbsp;
                                            <asp:LinkButton ID="lbtnLocateReopen" runat="server" CssClass="linkButton" Text=" [Reopen Item]"
                                                OnClientClick="return confirm('Are you sure you want to Reopen this Item?')"
                                                Visible="false"></asp:LinkButton>                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" class="errMsg">
                                            * denotes mandatory fields
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colRow" colspan="4">
                                            <br />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <div align="center">
                                    <asp:Button ID="btnLocateSave" CssClass="formsButton" Text="Save" runat="server"
                                        OnClientClick="return confirm('Are you sure you want to save the changes?')"
                                        Enabled="False" />&nbsp;
                                    <asp:Button ID="btnLocateCancel" CssClass="formsButton" Text="Cancel" runat="server" /></div>
                            </asp:Panel>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lbtnLocateDel" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <asp:UpdateProgress ID="upgLocateItem" runat="server" AssociatedUpdatePanelID="uplLocateItem">
                        <ProgressTemplate>
                            <br />
                            <img src="../images/progress.gif" alt="Processing" />
                            <asp:Label ID="lblProgress" CssClass="progress" Text="&nbsp;Processing ..." runat="server"></asp:Label></ProgressTemplate>
                    </asp:UpdateProgress>
                </ContentTemplate>
            </act:TabPanel>
            <act:TabPanel ID="tbpPrintItem" HeaderText="Item Report" runat="server">
                <ContentTemplate>
                     <asp:UpdatePanel ID="uplPrintItem" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblErrPrintItem" runat="server" CssClass="errMsg" Visible="false"></asp:Label>
                            <table class="tblModule" cellspacing="1">
                                <tr>
                                    <td class="colMod" width="20%">
                                        Print Option *
                                    </td>
                                    <td class="colDesc" width="30%" colspan="1">
                                        <asp:DropDownList ID="ddlPrintOption" CssClass="formsCombo" runat="server">
                                            <asp:ListItem Text=" - Please Select - " Value=""></asp:ListItem>
                                            <asp:ListItem Text="All" Value="A"></asp:ListItem>
                                            <asp:ListItem Text="Minimum Level" Value="M"></asp:ListItem>
                                            <asp:ListItem Text="Reorder Level" Value="R"></asp:ListItem>
                                            <asp:ListItem Text="Slow Moving Items" Value="S"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="colMod" width="20%">
                                        Sort By
                                    </td>
                                    <td class="colDesc" width="30%" colspan="1">
                                        <asp:DropDownList ID="ddlSortBy" CssClass="formsCombo" runat="server">
                                            <asp:ListItem Text=" - Please Select - " Value=""></asp:ListItem>
                                            <asp:ListItem Text="Stock Code"></asp:ListItem>
                                            <asp:ListItem Text="Stock Type"></asp:ListItem>
                                            <asp:ListItem Text="Sub Type"></asp:ListItem>
                                            <asp:ListItem Text="Equipment Type"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="colMod" width="20%">
                                        All Stock Code *
                                    </td>
                                    <td class="colDesc" width="30%">
                                        <asp:RadioButtonList ID="rblStockCode" runat="server" AutoPostBack="true" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td class="colMod" width="20%">
                                        Stock Code From<br />
                                        Stock Code To
                                    </td>
                                    <td class="colDesc" width="30%">
                                        <asp:TextBox ID="txtStockCodeFrom" runat="server" CssClass="formsText" Enabled="false"></asp:TextBox>
                                        <act:AutoCompleteExtender ID="aceStockCodeFrom" runat="server" MinimumPrefixLength="2" 
                                        CompletionSetCount="20" TargetControlID="txtStockCodeFrom" 
                                        ServiceMethod="GetStockItems" UseContextKey="true"></act:AutoCompleteExtender>
                                        <br />
                                        <asp:TextBox ID="txtStockCodeTo" runat="server" CssClass="formsText" Enabled="false"></asp:TextBox>
                                        <act:AutoCompleteExtender ID="aceStockCodeTo" runat="server" MinimumPrefixLength="2" 
                                        CompletionSetCount="20" TargetControlID="txtStockCodeTo" 
                                        ServiceMethod="GetStockItems" UseContextKey="true"></act:AutoCompleteExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="colMod" width="20%">
                                        Stock Type to Exclude
                                    </td>
                                    <td class="colDesc" width="30%" colspan="3">
                                        <asp:CheckBoxList ID="chkStockType" runat="server" RepeatDirection="Horizontal" RepeatColumns="3">
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="colMod" width="20%">
                                        Item Status
                                    </td>
                                    <td class="colDesc" width="30%" colspan="3">
                                        <asp:DropDownList ID="ddlItemStatus" runat="server" CssClass="formsCombo">
                                            <asp:ListItem Text="All" Value=""></asp:ListItem>
                                            <asp:ListItem Selected="True" Text="Open" Value="O"></asp:ListItem>
                                            <asp:ListItem Text="Closed" Value="C"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td> 
                                </tr>
                                <tr>
                                    <td colspan="4" class="errMsg">
                                        * denotes mandatory fields
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <div align="center">
                                <asp:Button ID="btnPDF" CssClass="formsButton" Text="Export to PDF" runat="server"
                                    Width="100px" />&nbsp;
                                <asp:Button ID="btnExcel" CssClass="formsButton" Text="Export to Excel" runat="server"
                                    Width="100px" />&nbsp;
                                <asp:Button ID="btnReportClear" CssClass="formsButton" Text="Clear" runat="server" />&nbsp;
                            </div>
                            <rsweb:ReportViewer ID="rvr" runat="server" Font-Names="Verdana" Font-Size="8pt"
                                Height="400px" Visible="False" Width="400px">
                                <LocalReport ReportPath="MasterList\rptItem.rdlc">
                                    <DataSources>
                                        <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="ItemDetails" />
                                    </DataSources>
                                </LocalReport>
                            </rsweb:ReportViewer>
                            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetItemsMasterList"
                                TypeName="NEA_ICS.UserInterface.NEA_ICS.WcfService.ServiceClient">
                                <SelectParameters>
                                    <asp:Parameter DefaultValue="" Name="storeID" Type="String" />
                                    <asp:Parameter DefaultValue="" Name="printOption" Type="String" />
                                    <asp:Parameter DefaultValue="" Name="sortBy" Type="String" />
                                    <asp:Parameter DefaultValue="" Name="stockCodeFrom" Type="String" />
                                    <asp:Parameter DefaultValue="" Name="stockCodeTo" Type="String" />
                                    <asp:Parameter DefaultValue="" Name="excludeStockCodeTypes" Type="String" />
                                    <asp:Parameter DefaultValue="" Name="itemStatus" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <br />                            
                            <asp:GridView ID="gdvItemReport" runat="server" BorderColor="#C0C0C0"
                                    BorderWidth="1px" AutoGenerateColumns="False">
                                    <FooterStyle BorderColor="#C0C0C0" />
                                    <RowStyle ForeColor="Black" BackColor="White" BorderColor="#C0C0C0" />
                                    <AlternatingRowStyle ForeColor="Black" BackColor="White" BorderColor="#C0C0C0" />
                                    <PagerStyle BorderColor="#C0C0C0" />
                                    <SelectedRowStyle ForeColor="Black" BackColor="White" />
                                    <HeaderStyle ForeColor="White" Font-Bold="true" BackColor="#666699" />
                                    <EmptyDataRowStyle CssClass="colEmpty" />
                                    <EmptyDataTemplate>
                                        <p>
                                            No records are found.</p>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="S/No" ItemStyle-BorderColor="#C0C0C0">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ItemID" ItemStyle-BorderColor="#C0C0C0" HeaderText="Stock Code" />
                                        <asp:BoundField DataField="ItemDescription" ItemStyle-BorderColor="#C0C0C0" HeaderText="Description" ItemStyle-Wrap="true" />
                                        <asp:BoundField DataField="PartNo" ItemStyle-BorderColor="#C0C0C0" HeaderText="Part No." />
                                        <asp:BoundField DataField="UOM" ItemStyle-BorderColor="#C0C0C0" HeaderText="UOM" />
                                        <asp:BoundField DataField="StockType" ItemStyle-BorderColor="#C0C0C0" HeaderText="Stock Type" ItemStyle-Wrap="true" />
                                        <asp:BoundField DataField="SubType" ItemStyle-BorderColor="#C0C0C0" HeaderText="Sub Type" ItemStyle-Wrap="true" />
                                        <asp:BoundField DataField="Location" ItemStyle-BorderColor="#C0C0C0" HeaderText="Location" ItemStyle-Wrap="true" />
                                        <asp:BoundField DataField="MinLevel" ItemStyle-BorderColor="#C0C0C0" HeaderText="Min Level" DataFormatString="{0:F2}" />
                                        <asp:BoundField DataField="MaxLevel" ItemStyle-BorderColor="#C0C0C0" HeaderText="Max Level" DataFormatString="{0:F2}" />  
                                        <asp:BoundField DataField="ReorderLevel" ItemStyle-BorderColor="#C0C0C0" HeaderText="Reorder Level" DataFormatString="{0:F2}" />
                                        <asp:BoundField DataField="OpeningBalance" ItemStyle-BorderColor="#C0C0C0" HeaderText="Stock Balance" DataFormatString="{0:F4}" />  
                                        <asp:BoundField DataField="OpeningTotalValue" ItemStyle-BorderColor="#C0C0C0" HeaderText="Stock Value (S$)" DataFormatString="{0:F4}" />
                                        <asp:BoundField DataField="AUCost" ItemStyle-BorderColor="#C0C0C0" HeaderText="AU Cost (S$)" DataFormatString="{0:F4}" />              
                                    </Columns>                      
                            </asp:GridView> 
                                        
                           </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnPDF" />
                            <asp:PostBackTrigger ControlID="btnExcel" />
                            <asp:PostBackTrigger ControlID="btnReportClear" />                                                         
                        </Triggers>
                    </asp:UpdatePanel>
                                
                </ContentTemplate>
            </act:TabPanel>
        </act:TabContainer>
        
    </div>
    </form>
</body>
</html>
