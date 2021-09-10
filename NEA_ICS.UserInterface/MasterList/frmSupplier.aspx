<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmSupplier.aspx.vb" Inherits="NEA_ICS.UserInterface.frmSupplier" ValidateRequest="true" %>
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
                    &nbsp;Master List > Supplier
                </td>
            </tr>
        </table>
        <br />
        <act:TabContainer ID="tbcSupplier" Width="98%" Font-Bold="true" Font-Size="Medium"
            runat="server" ForeColor="#4D36C2" BackColor="#FFFFFF" 
            Font-Names="Verdana" ActiveTabIndex="0" >
            <act:TabPanel ID="tbpNewSupplier" HeaderText="New Supplier" runat="server">
                <HeaderTemplate>
                    New Supplier
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:UpdatePanel ID="uplNewSupplier" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlAddSupplier" runat="server">
                                <asp:Label ID="lblErrAddSupplier" runat="server" CssClass="errMsg" Visible="false"></asp:Label>
                                <table class="tblModule" cellspacing="1">
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Supplier Code *
                                        </td>
                                        <td class="colDesc" width="30%" colspan="3">                                          
                                            <asp:TextBox ID="txtSupplierCode" CssClass="formsTextNum" runat="server" MaxLength="9"></asp:TextBox>                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Company Name *
                                        </td>
                                        <td class="colDesc" width="80%" colspan="3">
                                            <asp:TextBox ID="txtCompanyName" CssClass="formsText" runat="server" MaxLength="100" Width="99%"></asp:TextBox>
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
                                                <asp:TextBox ID="txtAddressNo" CssClass="formsTextNum" MaxLength="10" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Street Name *</td>
                                                <td>
                                                <asp:TextBox ID="txtStreetName" CssClass="formsText" MaxLength="32" runat="server"></asp:TextBox>
                                                </td>
                                                <td>Floor &amp; Unit No</td>
                                                <td>
                                                <asp:TextBox ID="txtFloorNo" CssClass="formsTextNum" MaxLength="3" runat="server"></asp:TextBox>
                                                &nbsp;-&nbsp;<asp:TextBox ID="txtUnitNo" CssClass="formsTextNum" MaxLength="5" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Building Name</td>
                                                <td>
                                                 <asp:TextBox ID="txtBuildingName" CssClass="formsText" MaxLength="32" runat="server"></asp:TextBox>
                                                </td>
                                                <td>Postal Code *</td>
                                                <td>
                                                <asp:TextBox ID="txtPostalCode" CssClass="formsTextNum" maxlength="6" runat="server"></asp:TextBox>
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
                                            <asp:TextBox ID="txtContactPerson" CssClass="formsText" runat="server" MaxLength="66"></asp:TextBox>
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
                                            Fax Number</td>
                                        <td class="colDesc" width="30%" colspan="1">
                                            <asp:TextBox ID="txtFaxNo" CssClass="formsTextNum" runat="server" MaxLength="20"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%" valign="top">
                                            Other Information
                                        </td>
                                        <td class="colDesc" width="30%" colspan="3">
                                            <asp:TextBox ID="txtOtherInfo" CssClass="formsText" runat="server" Width="99%" Height="20%" TextMode="MultiLine" MaxLength="500"></asp:TextBox>
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
                                    <asp:Button ID="btnAddSupplier" CssClass="formsButton" Text="Save" 
                                        runat="server" OnClientClick="return confirm('Are you sure you want to save your supplier?')" />&nbsp;
                                    <asp:Button ID="btnClear" CssClass="formsButton" Text="Clear" runat="server" />
                                </div>
                                <br />
                            </asp:Panel>
                            <div id="divMsgBoxAddSupplier" class="msg" runat="server" visible="false">
                                <table class="moduleTitle" width="100%" cellspacing="1" cellpadding="1">
                                    <tr>
                                        <td class="moduleTitleBorder">
                                            Supplier Status
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                Your Supplier has been saved successfully. Here are the details of your Supplier:
                                <br />
                                <br />
                                Supplier Information
                                <br />
                                <table class="tblModule" cellspacing="1">
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Supplier Code
                                        </td>
                                        <td class="colRow" width="70%">
                                            <asp:Label ID="lblMsgSupplierCode" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Company Name
                                        </td>
                                        <td class="colAltRow" width="70%">
                                            <asp:Label ID="lblMsgCompanyName" runat="server"></asp:Label>
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
                                    <asp:Button ID="btnAddSupplierOK" CssClass="formsButton" Text="OK" runat="server" />
                                </div>
                                <br />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    
                    <asp:UpdateProgress ID="upgNewSupplier" runat="server" AssociatedUpdatePanelID="uplNewSupplier">
                        <ProgressTemplate>
                            <br />
                            <img src="../images/progress.gif" alt="Processing" />
                            <asp:Label ID="lblProgress" CssClass="progress" Text="&nbsp;Processing ..." runat="server"></asp:Label>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </ContentTemplate>
            </act:TabPanel>
            <act:TabPanel ID="tbpLocateSupplier" HeaderText="Locate Supplier" runat="server">
                <HeaderTemplate>
                    Locate Supplier
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:UpdatePanel ID="uplLocateSupplier" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlLocateSupplier" runat="server">
                            <asp:Label ID="lblErrLocateSupplier" runat="server" CssClass="errMsg" Visible="false"></asp:Label>                             
                            <table class="tblModule" cellspacing="1">
                                <tr>
                                    <td class="colMod" width="20%">
                                        Supplier Code
                                    </td>
                                    <td class="colDesc" width="30%">
                                        <asp:TextBox ID="txtLocateSupplier" CssClass="formsText" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="colMod" width="20%">
                                        Company Name
                                    </td>
                                    <td class="colDesc" width="30%">
                                        <asp:TextBox ID="txtLocateCompanyName" CssClass="formsText" runat="server"></asp:TextBox>
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
                                            <asp:ListItem Text="Open" Selected="True" Value="O"></asp:ListItem>
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
                                <asp:GridView ID="gdvLocateSupplier" runat="server" CssClass="formsGrid" Width="100%" AllowSorting="True" CellSpacing="1"
                                    BorderWidth="0px" AutoGenerateColumns="False" AllowPaging="true" PageSize="5">
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
                                        <asp:CommandField ButtonType="Image" SelectImageUrl="~/Images/select.gif" ShowSelectButton="True" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="supplierId" HeaderText="Supplier Code" SortExpression="SupplierID" />
                                        <asp:BoundField DataField="companyName" HeaderText="Company Name" SortExpression="SupplierCompanyName" />
                                        <asp:TemplateField HeaderText="Status" SortExpression="SupplierStatus">
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
                            <asp:Panel ID="pnlSupplierInfo" runat="server" Visible="false">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/supplier_details.gif" AlternateText="Supplier Details" />
                                <br />
                                <asp:Label ID="lblErrSaveSupplier" runat="server" CssClass="errMsg" Visible="false"></asp:Label>                             
                                <table class="tblModule" cellspacing="1">
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Supplier Code 
                                        </td>
                                        <td class="colDesc" width="30%" colspan="3">
                                              <asp:Label ID="lblEditSupplierCode" runat="server"></asp:Label>                                              
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Company Name *
                                        </td>
                                        <td class="colDesc" width="80%" colspan="3">
                                            <asp:TextBox ID="txtEditCompanyName" CssClass="formsText" runat="server" Width="99%" MaxLength="100"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%" valign="top">
                                            Address
                                        </td>
                                        <td class="colDesc" width="30%" colspan="3">
                                        <table border="0">
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
                                                <td>Block / House No</td>
                                                <td>
                                                <asp:TextBox ID="txtEditAddressNo" CssClass="formsTextNum" runat="server" MaxLength="10"></asp:TextBox>
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
                                            Contact Person *</td>
                                        <td class="colDesc" width="30%" colspan="3">
                                            <asp:TextBox ID="txtEditContactPerson" CssClass="formsText" runat="server" maxlength="66"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Tel Number *</td>
                                        <td class="colDesc" width="30%" colspan="1">
                                            <asp:TextBox ID="txtEditTelNo" CssClass="formsTextNum" runat="server" MaxLength="20"></asp:TextBox>
                                        </td>
                                        <td class="colMod" width="20%">
                                            Fax Number
                                        </td>
                                        <td class="colDesc" width="30%" colspan="1">
                                            <asp:TextBox ID="txtEditFaxNo" CssClass="formsTextNum" runat="server" MaxLength="20"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%" valign="top">
                                            Other Information
                                        </td>
                                        <td class="colDesc" width="80%" colspan="3">
                                            <asp:TextBox ID="txtEditOtherInfo" CssClass="formsText" runat="server" TextMode="MultiLine" Width="99%" Height="20%" MaxLength="500"></asp:TextBox>
                                        </td>
                                    </tr> 
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Status
                                        </td>
                                        <td class="colDesc" width="80%" colspan="3">
                                            <asp:Label ID="lblDisplayStatus" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                        </td>
                                        <td class="colDesc" width="80%" colspan="3">
                                            <asp:LinkButton ID="lbtnLocateEdit" runat="server" CssClass="linkButton" Text="[Edit Supplier]"></asp:LinkButton>&nbsp;&nbsp;
                                            <asp:LinkButton ID="lbtnLocateDel" runat="server" CssClass="linkButton" Text=" [Delete Supplier]"
                                                OnClientClick="return confirm('Are you sure you want to Delete this Supplier?')" Visible="false"></asp:LinkButton>&nbsp;&nbsp;
                                            <asp:LinkButton ID="lbtnLocateClose" runat="server" CssClass="linkButton" Text=" [Close Supplier]"
                                                OnClientClick="return confirm('Are you sure you want to Close this Supplier?')"></asp:LinkButton>&nbsp;&nbsp;
                                            <asp:LinkButton ID="lbtnLocateReopen" runat="server" CssClass="linkButton" Text=" [Reopen Supplier]"
                                                OnClientClick="return confirm('Are you sure you want to Reopen this Supplier?')"
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
                            </asp:Panel>                          
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdateProgress ID="upgLocateSupplier" runat="server" AssociatedUpdatePanelID="uplLocateSupplier">
                        <ProgressTemplate>
                            <br />
                            <img src="../images/progress.gif" alt="Processing" />
                            <asp:Label ID="lblProgress" CssClass="progress" Text="&nbsp;Processing ..." runat="server"></asp:Label></ProgressTemplate>
                    </asp:UpdateProgress>
                </ContentTemplate>
            </act:TabPanel>
            <act:TabPanel ID="tbpPrintSupplier" HeaderText="Supplier Report" runat="server">
                <ContentTemplate>
                    <asp:UpdatePanel ID="uplPrintSupplier" runat="server">
                        <ContentTemplate>                         
                            <br />
                            <div align="center">
                                <asp:Button ID="btnPDF" CssClass="formsButton" Text="Export to PDF" runat="server"
                                    Width="100px" />&nbsp;
                                <asp:Button ID="btnExcel" CssClass="formsButton" Text="Export to Excel" runat="server"
                                    Width="100px" />&nbsp;                                
                            </div>
                            <rsweb:ReportViewer ID="rvr" runat="server" Font-Names="Verdana" 
                                Font-Size="8pt" Height="350px" Visible="False" Width="98%">
                                <LocalReport ReportPath="MasterList\rptSupplier.rdlc">
                                    <DataSources>
                                        <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                                            Name="SupplierDetails" />
                                    </DataSources>
                                </LocalReport>
                            </rsweb:ReportViewer>
                            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetSuppliers" 
                                TypeName="NEA_ICS.UserInterface.clsReportUtility">
                                <SelectParameters><asp:Parameter DefaultValue="" Name="storeId" Type="String" />
                                <asp:Parameter DefaultValue="" Name="supplierId" Type="String" />
                                <asp:Parameter DefaultValue="" Name="companyName" Type="String" />
                                <asp:Parameter DefaultValue="" Name="status" Type="String" /></SelectParameters>
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
