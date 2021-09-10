<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmEquipment.aspx.vb" Inherits="NEA_ICS.UserInterface.frmEquipment" ValidateRequest="true" %>
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
                    &nbsp;Master List > Equipment
                </td>
            </tr>
        </table>
        <br />
        <act:TabContainer ID="tbcEquipment" Width="98%" Font-Bold="true" Font-Size="Medium"
            runat="server" ForeColor="#4D36C2" BackColor="#FFFFFF" 
            Font-Names="Verdana" ActiveTabIndex="0"
            Visible="False">
            <act:TabPanel ID="tbpNewEquipment" HeaderText="New Equipment" runat="server">
                <ContentTemplate>
                    <asp:UpdatePanel ID="uplNewEquipment" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlAddEquipment" runat="server">
                            <asp:Label ID="lblErrAddEquipment" runat="server" CssClass="errMsg" Visible="false"></asp:Label>
                                <table class="tblModule" cellspacing="1">
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Equipment Code *
                                        </td>
                                        <td class="colDesc" width="30%" colspan="3">
                                            <asp:TextBox ID="txtEquipmentCode" CssClass="formsTextNum" runat="server" MaxLength="4"></asp:TextBox>&nbsp;
                                        </td>
                                   </tr>
                                   <tr>
                                        <td class="colMod" width="20%">
                                            Equipment Group *
                                        </td>
                                        <td class="colDesc" width="80%" colspan="3">
                                            <asp:DropDownList ID="ddlEquipmentType" runat="server" CssClass="formsCombo" Width="300px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Description *
                                        </td>
                                        <td class="colDesc" width="80%" colspan="3">
                                            <asp:TextBox ID="txtDescription" CssClass="formsText" runat="server" Width="98%" MaxLength="200"></asp:TextBox>
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
                                    <asp:Button ID="btnAddEquipment" CssClass="formsButton" Text="Save" runat="server" OnClientClick="return confirm('Are you sure you want to save your equipment?')" />&#160;
                                    <asp:Button ID="btnClear" CssClass="formsButton" Text="Clear" runat="server" />
                                </div>
                                <br />
                            </asp:Panel>
                            <div id="divMsgBox" class="msg" runat="server" visible="false">
                                <table class="moduleTitle" width="100%" cellspacing="1" cellpadding="1">
                                    <tr>
                                        <td class="moduleTitleBorder">
                                            Equipment Status
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                Your Equipment has been saved successfully. Here are the detail of your Equipment:
                                <br />
                                <br />
                                <span lang="en-sg">Equipment Information</span>
                                <br />
                                <table class="tblModule" cellspacing="1">
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Equipment Code
                                        </td>
                                        <td class="colRow" width="70%">
                                            <asp:Label ID="lblMsgEquipmentCode" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Equipment Group
                                        </td>
                                        <td class="colAltRow" width="30%">
                                            <asp:Label ID="lblMsgEquipmentType" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Description
                                        </td>
                                        <td class="colAltRow" width="70%">
                                            <asp:Label ID="lblMsgEquipmentDescription" runat="server"></asp:Label>
                                        </td>
                                    </tr>                                    
                                </table>
                                <br />
                                <div align="center">
                                    <asp:Button ID="btnAddEquipmentOK" CssClass="formsButton" Text="OK" runat="server" />
                                </div>
                                <br />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdateProgress ID="upgNewEquipment" runat="server" AssociatedUpdatePanelID="uplNewEquipment">
                        <ProgressTemplate>
                            <br />
                            <img src="../images/progress.gif" alt="Processing" />
                            <asp:Label ID="lblProgress" CssClass="progress" Text="&nbsp;Processing ..." runat="server"></asp:Label>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </ContentTemplate>
            </act:TabPanel>
            <act:TabPanel ID="tbpLocateEquipment" HeaderText="Locate Equipment" runat="server">
                <HeaderTemplate>
                    Locate Equipment
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:UpdatePanel ID="uplLocateEquipment" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblErrLocateEquipment" runat="server" CssClass="errMsg" Visible="false"></asp:Label>
                            <table class="tblModule" cellspacing="1">
                                <tr>
                                    <td class="colMod" width="20%">
                                        Equipment Code
                                    </td>
                                    <td class="colDesc" width="30%">
                                        <asp:TextBox ID="txtLocateEquipmentCode" CssClass="formsText" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="colMod" width="20%">
                                        Description
                                    </td>
                                    <td class="colDesc" width="30%">
                                        <asp:TextBox ID="txtLocateDescription" CssClass="formsText" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="colMod" width="20%">
                                        Equipment Group
                                    </td>
                                    <td class="colDesc" width="80%" colspan="3">
                                        <asp:DropDownList ID="ddlLocateEquipmentType" CssClass="formsCombo" runat="server" Width="300px"></asp:DropDownList>
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
                                <asp:GridView ID="gdvLocateEquipment" runat="server" CssClass="formsGrid" AllowPaging="True"
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
                                        <asp:CommandField ShowSelectButton="True" SelectImageUrl="~/Images/select.gif" ButtonType="Image" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%" />
                                        <asp:BoundField DataField="EquipmentID" SortExpression="EquipmentID" HeaderText="Equipment Code" />
                                        <asp:BoundField DataField="EquipmentDescription" SortExpression="EquipmentDescription" HeaderText="Description" />
                                        <asp:TemplateField HeaderText="Status" SortExpression="EquipmentStatus">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hidEquipmentType" runat="server" Value='<%# Bind("EquipmentType") %>' />
                                                <asp:HiddenField ID="hidStatus" runat="server" Value='<%# Bind("status") %>' />
                                                <asp:Label ID="lblStatus" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                            <br />
                            <asp:Panel ID="pnlEquipmentInfo" runat="server" Visible="false">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/equipment_details.gif" AlternateText="Equipment Details" />
                                <br />
                                <asp:Label ID="lblErrSaveEquipment" runat="server" CssClass="errMsg"></asp:Label>
                                <table class="tblModule" cellspacing="1">
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Equipment Code 
                                        </td>
                                        <td class="colDesc" width="30%" colspan="3">
                                            <asp:Label ID="lblEditEquipmentCode" runat="server"></asp:Label>                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Equipment Group *
                                        </td>
                                        <td class="colDesc" width="30%" colspan="3">
                                            <asp:DropDownList ID="ddlEditEquipmentType" runat="server" CssClass="formsCombo" Width="300px"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Description *
                                        </td>
                                        <td class="colDesc" width="80%" colspan="3">
                                            <asp:TextBox ID="txtEditDescription" CssClass="formsText" runat="server" Width="99%" MaxLength="200"></asp:TextBox>
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
                                            <asp:LinkButton ID="lbtnLocateEdit" runat="server" CssClass="linkButton" Text=" [Edit Equipment]"></asp:LinkButton>&nbsp;&nbsp;
                                            <asp:LinkButton ID="lbtnLocateDel" runat="server" CssClass="linkButton" Text=" [Delete Equipment]"
                                                OnClientClick="return confirm('Are you sure you want to Delete this Equipment?')" Visible="false"></asp:LinkButton>&nbsp;&nbsp;
                                            <asp:LinkButton ID="lbtnLocateClose" runat="server" CssClass="linkButton" Text=" [Close Equipment]"
                                                OnClientClick="return confirm('Are you sure you want to Close this Equipment?')"></asp:LinkButton>&nbsp;&nbsp;
                                            <asp:LinkButton ID="lbtnLocateReopen" runat="server" CssClass="linkButton" Text=" [Reopen Equipment]"
                                                OnClientClick="return confirm('Are you sure you want to Reopen this Equipment?')"
                                                Visible="false"></asp:LinkButton>
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
                                    <asp:Button ID="btnLocateSave" CssClass="formsButton" Text="Save" runat="server"
                                        OnClientClick="return confirm('Are you sure you want to save the changes?')"
                                        Enabled="False" />&nbsp;
                                    <asp:Button ID="btnLocateCancel" CssClass="formsButton" Text="Cancel" runat="server" /></div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdateProgress ID="upgLocateEquipment" runat="server" AssociatedUpdatePanelID="uplLocateEquipment">
                        <ProgressTemplate>
                            <br />
                            <img src="../images/progress.gif" alt="Processing" />
                            <asp:Label ID="lblProgress" CssClass="progress" Text="&nbsp;Processing ..." runat="server"></asp:Label></ProgressTemplate>
                    </asp:UpdateProgress>
                </ContentTemplate>
            </act:TabPanel>
            <act:TabPanel ID="tbpPrintEquipment" HeaderText="Equipment Report" runat="server">
                <ContentTemplate>
                    <asp:UpdatePanel ID="uplPrintEquipment" runat="server">
                        <ContentTemplate>                            
                            <br />
                            <div align="center">
                                <asp:Button ID="btnPDF" CssClass="formsButton" Text="Export to PDF" runat="server"
                                    Width="100px" />&nbsp;
                                <asp:Button ID="btnExcel" CssClass="formsButton" Text="Export to Excel" runat="server"
                                    Width="100px" />&nbsp;
                            </div>
                            <rsweb:ReportViewer ID="rvr" runat="server" Font-Names="Verdana" 
                                Font-Size="8pt" Height="400px" Width="400px" Visible="False">
                                <LocalReport ReportPath="MasterList\rptEquipment.rdlc">
                                    <DataSources>
                                        <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                                            Name="EquipmentDetails" />
                                    </DataSources>
                                </LocalReport>
                            </rsweb:ReportViewer>
                            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
                                SelectMethod="GetEquipments" 
                                TypeName="NEA_ICS.UserInterface.NEA_ICS.WcfService.ServiceClient">
                                <SelectParameters>
                                    <asp:Parameter DefaultValue="" Name="equipmentDetails" Type="Object" />
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
