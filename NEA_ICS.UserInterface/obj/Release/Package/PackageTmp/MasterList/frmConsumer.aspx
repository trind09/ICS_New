<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmConsumer.aspx.vb" Inherits="NEA_ICS.UserInterface.frmConsumer" ValidateRequest="true" %>
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
                    &nbsp;Master List > Consumer
                </td>
            </tr>
        </table>
        <br />
        <act:TabContainer ID="tbcConsumer" Width="98%" Font-Bold="true" Font-Size="Medium"
            runat="server" ForeColor="#4D36C2" BackColor="#FFFFFF" 
            Font-Names="Verdana" ActiveTabIndex="0"
            Visible="False">
            <act:TabPanel ID="tbpNewConsumer" HeaderText="New Consumer" runat="server">
                <ContentTemplate>
                    <asp:UpdatePanel ID="uplNewConsumer" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlAddConsumer" runat="server">
                                <asp:Label ID="lblErrAddConsumer" runat="server" CssClass="errMsg" Visible="false"></asp:Label>
                                <table class="tblModule" cellspacing="1">
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Consumer Code *
                                        </td>
                                        <td class="colDesc" width="30%" colspan="3">
                                            <asp:TextBox ID="txtAddConsumer" CssClass="formsTextNum" runat="server" maxlength="5"></asp:TextBox>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Description *
                                        </td>
                                        <td class="colDesc" width="80%" colspan="3">
                                            <asp:TextBox ID="txtAddDescription" CssClass="formsText" runat="server" Width="98%" MaxLength="30"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                    <td class="colMod" width="20%" valign="top">
                                    Users 
                                    </td>
                                    <td class="colDesc" width="80%"colspan="3">
                                    <table border="0">
                                    <tr>
                                        <td rowspan="2">
                                        List of Existing Users <br />
                                        <asp:ListBox ID="lstUserLists" runat="server" CssClass="formsText" Height="100px" Width="200px">
                                        </asp:ListBox>
                                        </td>
                                        <td>
                                        <asp:Button ID="btnAddUser" runat="server" CssClass="formsButton" Text="Add >>>" Width="80px" />
                                        </td>
                                        <td rowspan="2">
                                        Selected Users <br />
                                        <asp:ListBox ID="lstAddUsers" runat="server" CssClass="formsText" Height="100px" Width="200px">
                                        </asp:ListBox>
                                        </td>                                 
                                    </tr>
                                    <tr>
                                        <td>
                                        <asp:Button ID="btnRemoveUser" runat="server" CssClass="formsButton" Text="<<< Remove" Width="80px" />
                                        </td>
                                    </tr>
                                    </table>
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
                                    <asp:Button ID="btnAddConsumer" CssClass="formsButton" Text="Save" runat="server" OnClientClick="return confirm('Are you sure you want to save your consumer?')" />
                                    <asp:Button ID="btnClearConsumer" CssClass="formsButton" Text="Clear" runat="server" />
                                </div>
                                <br />
                            </asp:Panel>
                            <div id="divMsgBox" class="msg" runat="server" visible="false">
                                <table class="moduleTitle" width="100%" cellspacing="1" cellpadding="1">
                                    <tr>
                                        <td class="moduleTitleBorder">
                                            Consumer Status
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                Your Consumer has been saved successfully. Here are the detail of your Consumer:
                                <br />
                                <br />
                                <span lang="en-sg">Consumer Information</span>
                                <br />
                                <table class="tblModule" cellspacing="1">
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Consumer Code
                                        </td>
                                        <td class="colRow" width="70%">
                                            <asp:Label ID="lblMsgConsumerCode" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Description
                                        </td>
                                        <td class="colAltRow" width="70%">
                                            <asp:Label ID="lblMsgDescription" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%" valign="top">
                                            List of Users
                                        </td>
                                        <td class="colRow" width="70%">
                                            <asp:Label ID="lblMsgUserList" runat="server"></asp:Label>
                                        </td>
                                    </tr>                                    
                                </table>
                                <br />
                                <div align="center">
                                    <asp:Button ID="btnAddConsumerOK" CssClass="formsButton" Text="OK" runat="server" />
                                </div>
                                <br />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdateProgress ID="upgNewConsumer" runat="server" AssociatedUpdatePanelID="uplNewConsumer">
                        <ProgressTemplate>
                            <br />
                            <img src="../images/progress.gif" alt="Processing" />
                            <asp:Label ID="lblProgress" CssClass="progress" Text="&nbsp;Processing ..." runat="server"></asp:Label>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </ContentTemplate>
            </act:TabPanel>
            <act:TabPanel ID="tbpLocateConsumer" HeaderText="Locate Consumer" runat="server">
                <HeaderTemplate>
                    Locate Consumer
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:UpdatePanel ID="uplLocateConsumer" runat="server">
                        <ContentTemplate>
                            <asp:Label id="lblErrLocateConsumer" runat="server" CssClass="errMsg" Visible="false"></asp:Label>
                            <table class="tblModule" cellspacing="1">
                                <tr>
                                    <td class="colMod" width="20%">
                                        Consumer Code
                                    </td>
                                    <td class="colDesc" width="30%">
                                        <asp:TextBox ID="txtLocateConsumer" CssClass="formsText" runat="server"></asp:TextBox>
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
                            <asp:Panel ID="pnlLocateConsumer" runat="server" Visible="false">
                                <asp:Image ID="imgSearchResults" runat="server" ImageUrl="~/Images/search_results.gif" />
                                <asp:GridView ID="gdvLocateConsumer" runat="server" CssClass="formsGrid"
                                    PageSize="5" Width="100%" AllowPaging="true" AllowSorting="True" CellSpacing="1"
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
                                        <asp:BoundField DataField="ConsumerID" HeaderText="Consumer Code" SortExpression="ConsumerID" />
                                        <asp:BoundField DataField="ConsumerDescription" HeaderText="Description" SortExpression="ConsumerDescription" />
                                        <asp:TemplateField HeaderText="Status" SortExpression="ConsumerStatus">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hidStatus" runat="server" Value='<%# Bind("ConsumerStatus") %>' />
                                                <asp:Label ID="lblStatus" runat="server" ></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>                                          
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                            <br />
                            <asp:Panel ID="pnlEditConsumer" runat="server" Visible="false">
                                <asp:Image ID="imgConsumerDetails" runat="server" ImageUrl="~/Images/consumer_details.gif" AlternateText="Consumer Details"/>
                                <br />
                                <asp:Label ID="lblErrSaveConsumer" runat="server" CssClass="errMsg" Visible="false"></asp:Label>
                                <table class="tblModule" cellspacing="1">
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Consumer Code 
                                        </td>
                                        <td class="colDesc" width="30%" colspan="3">
                                            <asp:Label ID="lblEditConsumerCode" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Description *
                                        </td>
                                        <td class="colDesc" width="80%" colspan="3">
                                            <asp:TextBox ID="txtEditDescription" CssClass="formsText" runat="server" Width="99%" MaxLength="30" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                    <td class="colMod" width="20%" valign="top">
                                    Users 
                                    </td>
                                    <td class="colDesc" width="80%"colspan="3">
                                    <table border="0">
                                    <tr>
                                        <td rowspan="2">
                                        List of Existing Users <br />
                                        <asp:ListBox ID="lstEditUsersLists" runat="server" CssClass="formsText" Height="100px" Width="200px" Enabled="false">
                                        </asp:ListBox>
                                        </td>
                                        <td>
                                        <asp:Button ID="btnAddEditUsers" runat="server" CssClass="formsButton" Text="Add >>>" Width="80px" Enabled="false" />
                                        </td>
                                        <td rowspan="2">
                                        Selected Users <asp:Label ID="lblSelectedUsers" runat="server"></asp:Label><br />
                                        <asp:ListBox ID="lstEditUsers" runat="server" CssClass="formsText" Height="100px" Width="200px" Enabled="false">
                                        </asp:ListBox>
                                        </td>                                 
                                    </tr>
                                    <tr>
                                        <td>
                                        <asp:Button ID="btnRemoveEditUsers" runat="server" CssClass="formsButton" Text="<<< Remove" Width="80px" Enabled="false" />
                                        </td>
                                    </tr>
                                    </table>
                                    </td>
                                    </tr>
                                    
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Status
                                        </td>
                                        <td class="colDesc" width="30%" colspan="3">
                                            <asp:Label ID="lblEditStatus" runat="server" Text="Open"></asp:Label>
                                            <asp:HiddenField ID="hidEditStatus" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                        </td>
                                        <td class="colDesc" width="80%" colspan="3">
                                            <asp:LinkButton ID="lbtnLocateEdit" runat="server" CssClass="linkButton" Text=" [Edit Consumer]"></asp:LinkButton>&nbsp;&nbsp;
                                            <asp:LinkButton ID="lbtnLocateDel" runat="server" CssClass="linkButton" Text=" [Delete Consumer]"
                                                OnClientClick="return confirm('Are you sure you want to Delete this Consumer?')" Visible="false"></asp:LinkButton>&nbsp;&nbsp;
                                            <asp:LinkButton ID="lbtnLocateClose" runat="server" CssClass="linkButton" Text=" [Close Consumer]"
                                                OnClientClick="return confirm('Are you sure you want to Close this Consumer?')"></asp:LinkButton>&nbsp;&nbsp;
                                            <asp:LinkButton ID="lbtnLocateReopen" runat="server" CssClass="linkButton" Text=" [Reopen Consumer]"
                                                OnClientClick="return confirm('Are you sure you want to Reopen this Consumer?')"></asp:LinkButton>
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
                                    <asp:Button ID="btnEditConsumerSave" CssClass="formsButton" Text="Save" runat="server"
                                        OnClientClick="return confirm('Are you sure you want to save the consumer?')"
                                        Enabled="False" />&nbsp;
                                    <asp:Button ID="btnEditConsumerCancel" CssClass="formsButton" Text="Cancel" runat="server" /></div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdateProgress ID="upgLocateConsumer" runat="server" AssociatedUpdatePanelID="uplLocateConsumer">
                        <ProgressTemplate>
                            <br />
                            <img src="../images/progress.gif" alt="Processing" />
                            <asp:Label ID="lblProgress" CssClass="progress" Text="&nbsp;Processing ..." runat="server"></asp:Label></ProgressTemplate>
                    </asp:UpdateProgress>
                </ContentTemplate>
            </act:TabPanel>
            <act:TabPanel ID="tbpPrintConsumer" HeaderText="Consumer Report" runat="server">
                <ContentTemplate>
                    <asp:UpdatePanel ID="uplPrintConsumer" runat="server">
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
                                <LocalReport ReportPath="MasterList\rptConsumer.rdlc">
                                    <DataSources>
                                        <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="ConsumerDetails" />
                                    </DataSources>
                                </LocalReport>
                            </rsweb:ReportViewer>
                            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetConsumers"
                                TypeName="NEA_ICS.UserInterface.NEA_ICS.WcfService.ServiceClient">
                                <SelectParameters>
                                    <asp:Parameter DefaultValue="" Name="consumerDetails" Type="Object" />
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
