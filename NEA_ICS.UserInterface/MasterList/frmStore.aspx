<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmStore.aspx.vb" Inherits="NEA_ICS.UserInterface.frmStore" ValidateRequest="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Inventory Control System</title>

    <script type="text/javascript" src="../Script/NEA_ICS.js">

        function pageLoad() {
        }
    
    </script>

    <style type="text/css">
        body
        {
            margin-top: 0;
            margin-left: 0;
        }
    </style>
    <link href="../style/ICS.css" type="text/css" rel="Stylesheet" />

    <meta http-equiv='refresh' content='1800;url=..\frmUnauthorisedPage.aspx' />

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <table class="moduleTitle" width="98%" cellspacing="1" cellpadding="1">
            <tr>
                <td class="moduleTitleBorder">
                    &nbsp;Master List > Store
                </td>
            </tr>
        </table>
        <br />
        <act:TabContainer ID="tbcStore" Width="98%" Font-Bold="true" Font-Size="Medium" runat="server"
            ForeColor="#4D36C2" BackColor="#FFFFFF" Font-Names="Verdana" ActiveTabIndex="0"
            Visible="False">
            <act:TabPanel ID="tbpNewStore" HeaderText="New Store" runat="server">
                <HeaderTemplate>
                    New Store
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:UpdatePanel ID="uplNewStore" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlAddStore" runat="server">
                            <asp:Label ID="lblErrAddStore" runat="server" CssClass="errMsg"></asp:Label>
                                <table class="tblModule" cellspacing="1">
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Store Code *
                                        </td>
                                        <td class="colDesc" width="30%" colspan="3">
                                            <asp:TextBox ID="txtStoreCode" CssClass="formsTextNum" runat="server" MaxLength="4"></asp:TextBox>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Store Name *
                                        </td>
                                        <td class="colDesc" width="80%" colspan="3">
                                            <asp:TextBox ID="txtStoreName" CssClass="formsText" runat="server" Width="99%" MaxLength="100"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%" valign="top">
                                            Address 
                                        </td>
                                        <td class="colDesc" width="30%" colspan="3">
                                            <table border=0>
                                            <tr>
                                                <td>Type *</td>
                                                <td>
                                                <asp:DropDownList ID="ddlAddressType" runat="server" CssClass="formsCombo">
                                                <asp:ListItem Text=" - Please Select - " Value=""></asp:ListItem>
                                                <asp:ListItem Text="Apt Blk" Value="A"></asp:ListItem>
                                                <asp:ListItem Text="Without Apt Blk" Value="B"></asp:ListItem>
                                                <asp:ListItem Text="Reverse of Apt Blk & Str Name" Value="X"></asp:ListItem>
                                                <asp:ListItem Text="Overseas Address" Value="C"></asp:ListItem>
                                                <asp:ListItem Text="Private Flats with Apt Blk" Value="D"></asp:ListItem>
                                                <asp:ListItem Text="C/O Apt Blk" Value="E"></asp:ListItem>
                                                <asp:ListItem Text="C/O Without Apt Blk" Value="F"></asp:ListItem>
                                                <asp:ListItem Text="Quarter Address" Value="Q"></asp:ListItem>
                                                <asp:ListItem Text="Island Address" Value="I"></asp:ListItem>
                                                </asp:DropDownList>
                                                </td>
                                                <td>Block / House No *</td>
                                                <td>
                                                <asp:TextBox ID="txtAddressBlockHouseNo" CssClass="formsTextNum" runat="server" MaxLength="10"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Street Name *</td>
                                                <td>
                                                <asp:TextBox ID="txtStreetName" CssClass="formsText" runat="server" MaxLength="32"></asp:TextBox>
                                                </td>
                                                <td>Floor &amp; Unit No</td>
                                                <td>
                                                <asp:TextBox ID="txtFloorNo" CssClass="formsTextNum" runat="server" MaxLength="3"></asp:TextBox>
                                                &nbsp;-&nbsp;<asp:TextBox ID="txtUnitNo" CssClass="formsTextNum" runat="server" MaxLength="5"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Building Name</td>
                                                <td>
                                                 <asp:TextBox ID="txtBuildingName" CssClass="formsText" runat="server" MaxLength="32"></asp:TextBox>
                                                </td>
                                                <td>Postal Code *</td>
                                                <td>
                                                <asp:TextBox ID="txtPostalCode" CssClass="formsTextNum" runat="server" MaxLength="6"></asp:TextBox>
                                                </td>
                                            </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Contact Person *
                                        </td>
                                        <td class="colDesc" width="30%" colspan="3">
                                            <asp:TextBox ID="txtContactPerson" CssClass="formsText" runat="server" Width="99%" MaxLength="66"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Tel Number *
                                        </td>
                                        <td class="colDesc" width="30%" colspan="1">
                                            <asp:TextBox ID="txtTelNo" CssClass="formsTextNum" runat="server" MaxLength="20"></asp:TextBox>
                                        </td>
                                        <td class="colMod" width="20%">
                                            Fax Number
                                        </td>
                                        <td class="colDesc" width="30%" colspan="1">
                                            <asp:TextBox ID="txtFaxNo" CssClass="formsTextNum" runat="server" MaxLength="20"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Other Information
                                        </td>
                                        <td class="colDesc" width="30%" colspan="3">
                                            <asp:TextBox ID="txtOtherInfo" CssClass="formsText" runat="server" Width="99%" MaxLength="500"></asp:TextBox>
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
                                    <asp:Button ID="btnAddStore" CssClass="formsButton" Text="Save" runat="server" OnClientClick="return confirm('Are you sure you want to save your store?')" />&nbsp;
                                    <asp:Button ID="btnClear" CssClass="formsButton" Text="Clear" runat="server" />
                                </div>
                                <br />
                            </asp:Panel>
                            <div id="divMsgBoxAddStore" class="msg" runat="server" visible="false">
                                <table class="moduleTitle" width="100%" cellspacing="1" cellpadding="1">
                                    <tr>
                                        <td class="moduleTitleBorder">
                                            Store Status
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                Your Store has been saved successfully. Here are the details of your Store:
                                <br />
                                <br />
                                Store Information
                                <br />
                                <table class="tblModule" cellspacing="1">
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Store Code
                                        </td>
                                        <td class="colRow" width="70%">
                                            <asp:Label ID="lblMsgStoreCode" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Store Name
                                        </td>
                                        <td class="colAltRow" width="70%">
                                            <asp:Label ID="lblMsgStoreName" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Address Type
                                        </td>
                                        <td class="colRow" width="70%">
                                            <asp:Label ID="lblMsgAddressType" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Block House No
                                        </td>
                                        <td class="colAltRow" width="70%">
                                            <asp:Label ID="lblMsgAddressNo" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Street Name
                                        </td>
                                        <td class="colRow" width="70%">
                                            <asp:Label ID="lblMsgStreetName" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Floor No
                                        </td>
                                        <td class="colAltRow" width="70%">
                                            <asp:Label ID="lblMsgFloorNo" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Unit No
                                        </td>
                                        <td class="colRow" width="70%">
                                            <asp:Label ID="lblMsgUnitNo" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Building Name
                                        </td>
                                        <td class="colAltRow" width="70%">
                                            <asp:Label ID="lblMsgBuildingName" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Postal Code
                                        </td>
                                        <td class="colRow" width="70%">
                                            <asp:Label ID="lblMsgPostalCode" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Contact Person
                                        </td>
                                        <td class="colAltRow" width="70%">
                                            <asp:Label ID="lblMsgContactPerson" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Tel Number
                                        </td>
                                        <td class="colRow" width="70%">
                                            <asp:Label ID="lblMsgTelephoneNo" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Fax Number
                                        </td>
                                        <td class="colAltRow" width="70%">
                                            <asp:Label ID="lblMsgFaxNo" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Other Information
                                        </td>
                                        <td class="colRow" width="70%">
                                            <asp:Label ID="lblMsgOtherInfo" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <div align="center">
                                    <asp:Button ID="btnAddStoreOK" CssClass="formsButton" Text="OK" runat="server" />
                                </div>
                                <br />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdateProgress ID="upgNewStore" runat="server" AssociatedUpdatePanelID="uplNewStore">
                        <ProgressTemplate>
                            <br />
                            <img src="../images/progress.gif" alt="Processing" />
                            <asp:Label ID="lblProgress" CssClass="progress" Text="&nbsp;Processing ..." runat="server"></asp:Label>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </ContentTemplate>
            </act:TabPanel>
            <act:TabPanel ID="tbpLocateStore" HeaderText="Locate Store" runat="server">
                <ContentTemplate>
                    <asp:UpdatePanel ID="uplLocateStore" runat="server">
                        <ContentTemplate>
                             <asp:Label ID="lblErrLocateStore" runat="server" CssClass="errMsg"></asp:Label>
                            <table class="tblModule" cellspacing="1">
                                <tr>
                                    <td class="colMod" width="20%">
                                        Store Code
                                    </td>
                                    <td class="colDesc" width="80%" colspan="3">
                                        <asp:DropDownList ID="ddlLocateStore" CssClass="formsCombo" runat="server" Width="300px">
                                        </asp:DropDownList>
                                    </td>                                    
                                </tr>
                                <tr>
                                    <td class="colMod" width="20%">
                                        Store Name
                                    </td>
                                    <td class="colDesc" width="80%" colspan="3">
                                        <asp:TextBox ID="txtLocateStoreName" CssClass="formsText" runat="server" Width="99%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="colMod" width="20%">
                                        Status
                                    </td>
                                    <td class="colDesc" width="80%" colspan="3">
                                        <asp:DropDownList ID="ddlLocateStatus" CssClass="formsCombo" runat="server">
                                            <asp:ListItem Text="All" Value=""></asp:ListItem>
                                            <asp:ListItem Text="Closed" Value="C" ></asp:ListItem>
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
                                <asp:GridView ID="gdvLocateStore" runat="server" CssClass="formsGrid" AllowPaging="True"
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
                                        <asp:CommandField ShowSelectButton="True" ButtonType="Image" SelectImageUrl="~/Images/select.gif" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="storeId" HeaderText="Store Code" SortExpression="StoreID" />
                                        <asp:BoundField DataField="storeName" HeaderText="Store Name" SortExpression="StoreName" />
                                        <asp:TemplateField HeaderText="Status" SortExpression="StoreStatus">
                                            <ItemTemplate>                                                
                                                <asp:HiddenField ID="hidStatus" runat="server" value='<%# Bind("status") %>' />
                                                <asp:HiddenField ID="hidAddressType" runat="server" Value='<%# Bind("addressType") %>' />
                                                <asp:HiddenField ID="hidAddressBlockHouseNo" runat="server" Value='<%# Bind("addressBlockHouseNo") %>' />
                                                <asp:HiddenField ID="hidAddressStreetName" runat="server" Value='<%# Bind("addressStreetName") %>' />
                                                <asp:HiddenField ID="hidAddressFloorNo" runat="server" Value='<%# Bind("addressFloorNo") %>' />
                                                <asp:HiddenField ID="hidAddressUnitNo" runat="server" Value='<%# Bind("addressUnitNo") %>' />
                                                <asp:HiddenField ID="hidAddressBuildingName" runat="server" Value='<%# Bind("addressBuildingName") %>' />
                                                <asp:HiddenField ID="hidAddressPostalCode" runat="server" Value='<%# Bind("addressPostalCode") %>' />
                                                <asp:HiddenField ID="hidContactPerson" runat="server" Value='<%# Bind("contactPerson") %>' />
                                                <asp:HiddenField ID="hidTelephoneNo" runat="server" Value='<%# Bind("telephoneNo") %>' />
                                                <asp:HiddenField ID="hidFaxNo" runat="server" Value='<%# Bind("faxNo") %>' />
                                                <asp:HiddenField ID="hidOtherInfo" runat="server" Value='<%# Bind("otherInfo") %>' />
                                                <asp:Label ID="lblStatus" runat="server" ></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>                                        
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                            <br />
                            <asp:Panel ID="pnlStoreInfo" runat="server" Visible="false">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/store_details.gif" AlternateText="Store Details"/>
                                <br />
                                <asp:Label ID="lblErrSaveStore" runat="server" CssClass="errMsg" Visible="false"></asp:Label>                             
                                <table class="tblModule" cellspacing="1">
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Store Code 
                                        </td>
                                        <td class="colDesc" width="30%" colspan="3">
                                            <asp:Label ID="lblEditStoreCode" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Store Name *
                                        </td>
                                        <td class="colDesc" width="80%" colspan="3">
                                            <asp:TextBox ID="txtEditStoreName" CssClass="formsText" runat="server" Width="99%" MaxLength="100"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%" valign="top">
                                            Address 
                                        </td>
                                        <td class="colDesc" width="30%" colspan="3">
                                            <table border=0>
                                            <tr>
                                                <td>Type *</td>
                                                <td>
                                                <asp:DropDownList ID="ddlEditAddressType" runat="server" CssClass="formsCombo">
                                                <asp:ListItem Text=" - Please Select - " Value=""></asp:ListItem>
                                                <asp:ListItem Text="Apt Blk" Value="A"></asp:ListItem>
                                                <asp:ListItem Text="Without Apt Blk" Value="B"></asp:ListItem>
                                                <asp:ListItem Text="Reverse of Apt Blk & Str Name" Value="X"></asp:ListItem>
                                                <asp:ListItem Text="Overseas Address" Value="C"></asp:ListItem>
                                                <asp:ListItem Text="Private Flats with Apt Blk" Value="D"></asp:ListItem>
                                                <asp:ListItem Text="C/O Apt Blk" Value="E"></asp:ListItem>
                                                <asp:ListItem Text="C/O Without Apt Blk" Value="F"></asp:ListItem>
                                                <asp:ListItem Text="Quarter Address" Value="Q"></asp:ListItem>
                                                <asp:ListItem Text="Island Address" Value="I"></asp:ListItem>
                                                </asp:DropDownList>
                                                </td>
                                                <td>Block / House No *</td>
                                                <td>
                                                <asp:TextBox ID="txtEditBlockHouseNo" CssClass="formsTextNum" runat="server" maxlength="10"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Street Name *</td>
                                                <td>
                                                <asp:TextBox ID="txtEditStreetName" CssClass="formsText" runat="server" MaxLength="32"></asp:TextBox>
                                                </td>
                                                <td>Floor &amp; Unit No</td>
                                                <td>
                                                <asp:TextBox ID="txtEditFloorNo" CssClass="formsTextNum" runat="server" MaxLength="3"></asp:TextBox>
                                                &nbsp;-&nbsp;<asp:TextBox ID="txtEditUnitNo" CssClass="formsTextNum" runat="server" MaxLength="5"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Building Name</td>
                                                <td>
                                                 <asp:TextBox ID="txtEditBuildingName" CssClass="formsText" runat="server" MaxLength="32"></asp:TextBox>
                                                </td>
                                                <td>Postal Code *</td>
                                                <td>
                                                <asp:TextBox ID="txtEditPostalCode" CssClass="formsTextNum" runat="server" MaxLength="6"></asp:TextBox>
                                                </td>
                                            </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Contact Person *
                                        </td>
                                        <td class="colDesc" width="30%" colspan="3">
                                            <asp:TextBox ID="txtEditContactPerson" CssClass="formsText" runat="server" Width="99%" MaxLength="66"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Tel Number *
                                        </td>
                                        <td class="colDesc" width="30%" colspan="1">
                                            <asp:TextBox ID="txtEditTelephoneNo" CssClass="formsTextNum" runat="server" MaxLength="20"></asp:TextBox>
                                        </td>
                                        <td class="colMod" width="20%">
                                            Fax Number
                                        </td>
                                        <td class="colDesc" width="30%" colspan="1">
                                            <asp:TextBox ID="txtEditFaxNo" CssClass="formsTextNum" runat="server" MaxLength="20"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Other Information
                                        </td>
                                        <td class="colDesc" width="30%" colspan="3">
                                            <asp:TextBox ID="txtEditOtherInfo" CssClass="formsText" runat="server" Width="99%" MaxLength="500"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Status
                                        </td>
                                        <td class="colDesc" width="30%" colspan="3">
                                            <asp:Label ID="lblDisplayStatus" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                        </td>
                                        <td class="colDesc" width="80%" colspan="3">
                                            <asp:LinkButton ID="lbtnLocateEdit" runat="server" CssClass="linkButton" Text=" [Edit Store]"></asp:LinkButton>&nbsp;&nbsp;
                                            <asp:LinkButton ID="lbtnLocateDel" runat="server" CssClass="linkButton" Text=" [Delete Store]"
                                                OnClientClick="return confirm('Are you sure you want to Delete this Store?')" Visible="false"></asp:LinkButton>&nbsp;&nbsp;
                                            <asp:LinkButton ID="lbtnLocateClose" runat="server" CssClass="linkButton" Text=" [Close Store]"
                                                OnClientClick="return confirm('Are you sure you want to Close this Store?')"></asp:LinkButton>&nbsp;&nbsp;
                                            <asp:LinkButton ID="lbtnLocateReopen" runat="server" CssClass="linkButton" Text=" [Reopen Store]"
                                                OnClientClick="return confirm('Are you sure you want to Reopen this Store?')"
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
                    </asp:UpdatePanel>
                    <asp:UpdateProgress ID="upgLocateStore" runat="server" AssociatedUpdatePanelID="uplLocateStore">
                        <ProgressTemplate>
                            <br />
                            <img src="../images/progress.gif" alt="Processing" />
                            <asp:Label ID="lblProgress" CssClass="progress" Text="&nbsp;Processing ..." runat="server"></asp:Label></ProgressTemplate>
                    </asp:UpdateProgress>
                </ContentTemplate>
            </act:TabPanel>
            <act:TabPanel ID="tbpPrintStore" HeaderText="Store Report" runat="server">
                <ContentTemplate>
                    <asp:UpdatePanel ID="uplPrintStore" runat="server">
                        <ContentTemplate>                            
                            <br />
                            <div align="center">
                                <asp:Button ID="btnPDF" CssClass="formsButton" Text="Export to PDF" runat="server"
                                    Width="100px" />&nbsp;
                                <asp:Button ID="btnExcel" CssClass="formsButton" Text="Export to Excel" runat="server"
                                    Width="100px" />&nbsp;                              
                            </div>
                            <rsweb:ReportViewer ID="rvr" runat="server" Font-Names="Verdana" Font-Size="8pt"
                                Height="400px" Visible="False" Width="400px">
                                <LocalReport ReportPath="MasterList\rptStore.rdlc">
                                    <DataSources>
                                        <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="StoreDetails" />
                                    </DataSources>
                                </LocalReport>
                            </rsweb:ReportViewer>
                            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
                                SelectMethod="GetStores" 
                                TypeName="NEA_ICS.UserInterface.NEA_ICS.WcfService.ServiceClient">
                                <SelectParameters>
                                    <asp:Parameter DefaultValue="" Name="storeDetails" Type="Object" />
                                    <asp:Parameter DefaultValue="" Name="sortExpression" Type="String" />
                                    <asp:Parameter DefaultValue="" Name="sortDirection" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <br />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnPDF" />
                            <asp:PostBackTrigger ControlID="btnExcel" />
                        </Triggers>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </act:TabPanel>
        </act:TabContainer>
    </div>
    </form>
</body>
</html>
