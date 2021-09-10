<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmRole.aspx.vb" Inherits="NEA_ICS.UserInterface.frmRole" ValidateRequest="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Inventory Control System</title>

    <script type="text/javascript" src="../Script/NEA_ICS.js" >

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
                    &nbsp;Master List > Role
                </td>
            </tr>
        </table>
        <br />
        <act:TabContainer ID="tbcRole" Width="98%" Font-Bold="true" Font-Size="Medium" runat="server"
            ForeColor="#4D36C2" BackColor="#FFFFFF" Font-Names="Verdana" ActiveTabIndex="0"
            Visible="False">
            <act:TabPanel ID="tbpModuleRole" HeaderText="Module Role" runat="server">
                <HeaderTemplate>
                    Module Role             
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:UpdatePanel ID="uplModuleRole" runat="server"><ContentTemplate>
                            <asp:Label ID="lblErrLocateModule" runat="server" CssClass="errMsg" Visible="false"></asp:Label>
                            <table class="tblModule" cellspacing="1">
                                <tr>
                                    <td class="colMod" width="20%">
                                        Store Code 
                                    </td>
                                    <td class="colDesc" width="30%">
                                        <asp:DropDownList ID="ddlModuleStore" CssClass="formsCombo" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="colMod" width="20%">                             
                                        Role 
                                    </td>
                                    <td class="colDesc" width="30%">
                                        <asp:DropDownList ID="ddlModuleRole" CssClass="formsCombo" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <div align="center">
                                <asp:Button ID="btnModuleGo" CssClass="formsButton" Text="Go" runat="server" />&nbsp;
                                <asp:Button ID="btnModuleClear" CssClass="formsButton" Text="Clear" runat="server" />
                            </div>
                            <asp:Panel ID="pnlModuleSearchResults" runat="server" Visible="false">
                                <asp:Image ID="imgSearchResults" runat="server" ImageUrl="~/Images/module_details.gif" AlternateText="Module Details" />
                                <br />
                                <asp:Label ID="lblModuleNote" runat="server" Text="Note: When assigning Add/Edit/Delete access rights, View access rights will be assigned as well." CssClass="errMsg" ForeColor="Navy"></asp:Label><br />
                                <asp:Label ID="lblErrSaveModule" runat="server" CssClass="errMsg" Visible="false"></asp:Label> 
                                <asp:GridView ID="gdvLocateModule" runat="server" CssClass="formsGrid" Width="100%" 
                                    CellSpacing="1" BorderWidth="0px" AutoGenerateColumns="False">
                                    <FooterStyle CssClass="colFooter" />
                                    <RowStyle CssClass="colRow" />
                                    <AlternatingRowStyle CssClass="colAltRow" />
                                    <PagerStyle CssClass="colPager" />
                                    <EditRowStyle CssClass="colSelected" />
                                    <SelectedRowStyle CssClass="colSelected" />
                                    <HeaderStyle CssClass="colHeader" />
                                    <EmptyDataRowStyle CssClass="colEmpty" />
                                    <EmptyDataTemplate>
                                        <p>
                                            No records are found.</p>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:CommandField ShowEditButton="true" ButtonType="Image" UpdateImageUrl="~/Images/save.gif" CancelImageUrl="~/Images/cancel.gif" EditImageUrl="~/Images/edit.gif" ItemStyle-Width="7%" ItemStyle-HorizontalAlign="Center" HeaderText="Edit?" />
                                        <asp:TemplateField HeaderText="Module">
                                            <ItemTemplate>
                                                <asp:Label ID="lblModuleTitle" runat="server" Text='<%# Bind("ModuleTitle") %>'></asp:Label>
                                                <asp:HiddenField ID="hidModuleID" runat="server" Value='<%# Bind("ModuleID") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:HiddenField ID="hidDisplayModuleID" runat="server" Value='<%# Bind("ModuleID") %>' />
                                                <asp:Label ID="lblDisplayModuleTitle" runat="server" Text='<%# Bind("ModuleTitle") %>' ></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="View Access" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkViewAccess" runat="server" Checked='<%# Bind("SelectRight") %>' Enabled="false" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="chkEditViewAccess" runat="server" Checked='<%# Bind("SelectRight") %>' Enabled="true" OnCheckedChanged="chkEditViewAccess_CheckChanged" AutoPostBack="true" /> 
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Add Access" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkAddAccess" runat="server" Checked='<%# Bind("InsertRight") %>' Enabled="false" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="chkEditAddAccess" runat="server" Checked='<%# Bind("InsertRight") %>' Enabled="true" OnCheckedChanged="chkEditAddAccess_CheckChanged" AutoPostBack="true" /> 
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Edit Access" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkUpdateAccess" runat="server" Checked='<%# Bind("UpdateRight") %>' Enabled="false" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="chkEditUpdateAccess" runat="server" Checked='<%# Bind("UpdateRight") %>' Enabled="true" OnCheckedChanged="chkEditUpdateAccess_CheckChanged" AutoPostBack="true" /> 
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delete Access" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkDeleteAccess" runat="server" Checked='<%# Bind("DeleteRight") %>' Enabled="false" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="chkEditDeleteAccess" runat="server" Checked='<%# Bind("DeleteRight") %>' Enabled="true" OnCheckedChanged="chkEditDeleteAccess_CheckChanged" AutoPostBack="true" /> 
                                            </EditItemTemplate>
                                        </asp:TemplateField>      
                                    </Columns>
                                </asp:GridView>                               
                            </asp:Panel>
                            <br />                        
                            </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdateProgress ID="upgModuleRole" runat="server" AssociatedUpdatePanelID="uplModuleRole"><ProgressTemplate>
                            <br />
                            <img src="../images/progress.gif" alt="Processing" />
                            <asp:Label ID="lblProgress" CssClass="progress" Text="&nbsp;Processing ..." runat="server"></asp:Label>
                            </ProgressTemplate>
                            </asp:UpdateProgress>                
                 </ContentTemplate>
            </act:TabPanel>
            <act:TabPanel ID="tbpUserRole" HeaderText="User Role" runat="server">
                <HeaderTemplate>
                    User Role        
            </HeaderTemplate>
           <ContentTemplate>
                <asp:UpdatePanel ID="uplUserRole" runat="server">
                <ContentTemplate>
                    <asp:Label ID="lblErrLocateUser" runat="server" CssClass="errMsg" Visible="false"></asp:Label>
                        <table class="tblModule" cellspacing="1">
                                <tr>
                                    <td class="colMod" width="20%">
                                        Store Code 
                                    </td>
                                    <td class="colDesc" width="30%">
                                        <asp:DropDownList ID="ddlUserStore" CssClass="formsCombo" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="colMod" width="20%">                             
                                        Role 
                                    </td>
                                    <td class="colDesc" width="30%">
                                        <asp:DropDownList ID="ddlUserRole" CssClass="formsCombo" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                    <br />
                    <div align="center">
                        <asp:Button ID="btnUserGo" CssClass="formsButton" Text="Go" runat="server" />&nbsp;
                        <asp:Button ID="btnUserClear" CssClass="formsButton" Text="Clear" runat="server" />
                    </div>
                    <asp:Panel ID="pnlUserRole" runat="server" Visible="false">
                    <asp:Image ID="imgUserRoleDetails" runat="server" ImageUrl="~/Images/role_details.gif" AlternateText="Role Details" />
                    <asp:GridView ID="gdvLocateUser" runat="server" CssClass="formsGrid" Width="100%" 
                    AllowSorting="True" CellSpacing="1" BorderWidth="0px" AutoGenerateColumns="False">
                    <FooterStyle CssClass="colFooter" />
                    <RowStyle CssClass="colRow" />
                    <AlternatingRowStyle CssClass="colAltRow" />
                    <PagerStyle CssClass="colPager" />
                    <EditRowStyle CssClass="colSelected" />
                    <SelectedRowStyle CssClass="colSelected" />
                    <HeaderStyle CssClass="colHeader" />
                    <EmptyDataRowStyle CssClass="colEmpty" />
                    <EmptyDataTemplate>
                    <p>
                    No records are found.</p>
                    </EmptyDataTemplate>
                    <Columns>
                    <asp:CommandField ShowSelectButton="true" ButtonType="Image" SelectImageUrl="~/Images/edit.gif" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" HeaderText="Edit?" />
                    <asp:TemplateField HeaderText="Delete?" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgbtnDelete" runat="server" CommandName="DELETE" OnClientClick="return confirm('Are you sure you want to delete this user?');" ImageUrl="~/Images/delete.gif" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="UserID" HeaderText="User ID" SortExpression="VUserProfileUserID" />
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="VUserProfileName" />
                    <asp:BoundField DataField="Designation" HeaderText="Designation" SortExpression="VUserProfileDesignation" />
                    <asp:BoundField DataField="Division" HeaderText="Division" SortExpression="VUserProfileDivisionCode" />
                    <asp:BoundField DataField="Department" HeaderText="Department" SortExpression="VUserProfileDepartCode" />
                    <asp:BoundField DataField="Installation" HeaderText="Installation" SortExpression="VUserProfileInstallCode" />
                    <asp:BoundField DataField="Section" HeaderText="Section" SortExpression="VUserProfileSectionCode" />
                    <asp:TemplateField HeaderText="Status" SortExpression="UserRoleStatus">              
                    <ItemTemplate>
                        <asp:HiddenField ID="hidStatus" runat="server" Value='<%# Bind("UserStatus") %>' />
                        <asp:Label ID="lblStatus" runat="server"></asp:Label>
                    </ItemTemplate>
                    </asp:TemplateField>
                    </Columns>
                    </asp:GridView>
                    <br />
                    <div align="center">
                        <asp:Button ID="btnAddUser" CssClass="formsButton" Text="Add User" runat="server" Width="100px" OnClientClick="return confirm('Are you sure you want to add user?');" />
                    </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlAddEditUser" runat="server" Visible="false">
                    <asp:Image ID="imgRoleDetails" runat="server" ImageUrl="~/Images/role_details.gif" AlternateText="Role Details" />
                                <br />
                                <asp:Label ID="lblErrSaveUser" runat="server" CssClass="errMsg" Visible="false"></asp:Label>
                                <table class="tblModule" cellspacing="1">
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Store Code
                                        </td>
                                        <td class="colDesc" width="30%">
                                            <asp:Label ID="lblStoreCode" runat="server"></asp:Label>
                                        </td>
                                        <td class="colMod" width="20%">
                                            Role
                                        </td>
                                        <td class="colDesc" width="30%">
                                            <asp:Label ID="lblRole" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            User NRIC
                                        </td>
                                        <td class="colDesc" width="80%" colspan="3">
                                            <asp:TextBox ID="txtUserNRIC" runat="server" CssClass="formsTextNum" MaxLength="9" Text="S"></asp:TextBox>
                                            <act:MaskedEditExtender ID="meeUserNRIC" runat="server" TargetControlID="txtUserNRIC" Mask="A9999999C" Filtered="AaBbCcDdEeFfGgHhIiJjZzMm">
                                            </act:MaskedEditExtender>
                                            <asp:Label ID="lblUserNRIC" runat="server" Visible="false"></asp:Label>
                                            &nbsp;
                                            <asp:LinkButton ID="btnUserNRICCheck" Text="Check NRIC" runat="server" CssClass="linkButton"></asp:LinkButton>                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            User Name
                                        </td>
                                        <td class="colDesc" width="30%">
                                            <asp:Label ID="lblUserName" runat="server"></asp:Label>
                                        </td>
                                        <td class="colMod" width="20%">
                                            Designation
                                        </td>
                                        <td class="colDesc" width="30%">
                                            <asp:Label ID="lblDesignation" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Division
                                        </td>
                                        <td class="colDesc" width="30%">
                                            <asp:Label ID="lblDivision" runat="server"></asp:Label>
                                        </td>
                                        <td class="colMod" width="20%">
                                            Department
                                        </td>
                                        <td class="colDesc" width="30%">
                                            <asp:Label ID="lblDepartment" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Installation
                                        </td>
                                        <td class="colDesc" width="30%">
                                            <asp:Label ID="lblInstallationCode" runat="server"></asp:Label>
                                        </td>
                                        <td class="colMod" width="20%">
                                            Section
                                        </td>
                                        <td class="colDesc" width="30%">
                                            <asp:Label ID="lblSection" runat="server"></asp:Label>
                                        </td>
                                    </tr>                                    
                                    <tr>
                                        <td class="colMod" width="20%" valign="top">
                                            Consumers
                                        </td>
                                        <td class="colDesc" width="80%"colspan="3">
                                            <table border="0">
                                                <tr>
                                                    <td rowspan="2">
                                                        List of Existing Consumers
                                                        <br />
                                                        <asp:ListBox ID="lstConsumerLists" runat="server" CssClass="formsText" Height="100px" Width="200px">                                        
                                                        </asp:ListBox>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnAddConsumer" runat="server" CssClass="formsButton" Text="Add >>>" Width="80px" />
                                                    </td>
                                                    <td rowspan="2">
                                                        Selected Consumers 
                                                        <br />
                                                        <asp:ListBox ID="lstSelectedConsumers" runat="server" CssClass="formsText" Height="100px" Width="200px">                                        
                                                        </asp:ListBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnRemoveConsumer" runat="server" CssClass="formsButton" Text="<<< Remove" Width="80px" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colMod" width="20%">
                                            Activate ?
                                        </td>
                                        <td class="colDesc" width="80%" colspan="3">
                                            <asp:RadioButton ID="rdoActivateY" runat="server" GroupName="Activate" Text="Yes" Checked="true" />
                                            <asp:RadioButton ID="rdoActivateN" runat="server" GroupName="Activate" Text="No" Checked="false" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <div align="center">
                                    <asp:Button id="btnAddUserSave" runat="server" CssClass="formsButton" Text="Save" OnClientClick="return confirm('Are you sure you want to add this user?');" Visible="false" />
                                    <asp:Button ID="btnSaveUser" runat="server" CssClass="formsButton" Text="Save" OnClientClick="return confirm('Are you sure you want to save this user?');" />
                                    <asp:Button ID="btnCancelUser" runat="server" CssClass="formsButton" Text="Cancel" />
                                </div>
                            </asp:Panel>
                            <br />
                            </ContentTemplate>
                            </asp:UpdatePanel>

                            <asp:UpdateProgress ID="upgUserRole" runat="server" AssociatedUpdatePanelID="uplUserRole">
                                <ProgressTemplate>
                                <br />
                                <img src="../images/progress.gif" alt="Processing" />
                                <asp:Label ID="lblProgress" CssClass="progress" Text="&nbsp;Processing ..." runat="server"></asp:Label>
                                </ProgressTemplate>
                            </asp:UpdateProgress>                             
                        </ContentTemplate>
            
                </act:TabPanel>
                </act:TabContainer>
                </div>
                </form>
            </body>
    </html>
