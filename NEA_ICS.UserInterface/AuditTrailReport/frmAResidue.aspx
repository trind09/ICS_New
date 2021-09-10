<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmAResidue.aspx.vb" Inherits="NEA_ICS.UserInterface.frmAResidue"
 ValidateRequest="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Inventory Control System</title>
    <link href="../style/ICS.css" type="text/css" rel="Stylesheet" />
    <meta http-equiv='refresh' content='1800;url=..\frmUnauthorisedPage.aspx' />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        
        <asp:UpdatePanel ID="uplResidue" runat="server">
            <ContentTemplate>
            <table class="moduleTitle" width="98%" cellspacing="1" cellpadding="1">
            <tr>
                <td class="moduleTitleBorder">
                    Audit Trail Report > Residue Cost Adjustment
                </td>
            </tr>
            </table>
            <br />
            <asp:Panel ID="pnlModule" runat="server" Width="98%">
            <table class="tblModule" cellspacing="1">
                <tr>
                    <td class="colMod" width="20%">
                        Month :
                    </td>
                    <td class="colDesc" width="80%">
                        <asp:DropDownList ID="ddlMonth" runat="server" CssClass="formsCombo">
                            <asp:ListItem Value="1" Text="Jan"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Feb"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Mar"></asp:ListItem>
                            <asp:ListItem Value="4" Text="Apr"></asp:ListItem>
                            <asp:ListItem Value="5" Text="May"></asp:ListItem>
                            <asp:ListItem Value="6" Text="Jun"></asp:ListItem>
                            <asp:ListItem Value="7" Text="Jul"></asp:ListItem>
                            <asp:ListItem Value="8" Text="Aug"></asp:ListItem>
                            <asp:ListItem Value="9" Text="Sep"></asp:ListItem>
                            <asp:ListItem Value="10" Text="Oct"></asp:ListItem>
                            <asp:ListItem Value="11" Text="Nov"></asp:ListItem>
                            <asp:ListItem Value="12" Text="Dec"></asp:ListItem>
                        </asp:DropDownList>
                    </td> 
                </tr> 
                <tr>
                     <td class="colMod" width="20%">
                        Year :
                    </td>
                    <td class="colDesc" width="80%">
                        <asp:DropDownList ID="ddlYear" runat="server" CssClass="formsCombo">
                        </asp:DropDownList> 
                    </td>       
                </tr>
            </table> 
            
             <br />
             <div align="center">
                <asp:Button ID="btnPDF" CssClass="formsButton" Text="Export to PDF" runat="server"
                Width="100px" />&nbsp;
                <asp:Button ID="btnExcel" CssClass="formsButton" Text="Export to Excel" runat="server"
                Width="100px" />&nbsp;                
            </div>
            </asp:Panel> 
            <rsweb:ReportViewer ID="rvr" runat="server" Font-Size="8pt" Height="400px" Visible="False"
                        Width="100%" Font-Names="Verdana">
                        <LocalReport ReportPath="AuditTrailReport\rptAStockResidue.rdlc">
                            <DataSources>
                                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="AResidueDetails" />
                            </DataSources>
                        </LocalReport>
                    </rsweb:ReportViewer>
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetAuditTrailResidue"
                        TypeName="NEA_ICS.UserInterface.NEA_ICS.WcfService.ServiceClient">
                        <SelectParameters>                            
                            <asp:Parameter Name="storeID" Type="String" />
                            <asp:Parameter Name="month" Type="Int32" />
                            <asp:Parameter Name="year" Type="Int32" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <br />
                    <rsweb:ReportViewer ID="rvrBlank" runat="server" Font-Size="8pt" Height="400px" Visible="False"
                        Width="100%" Font-Names="Verdana">
                        <LocalReport ReportPath="AuditTrailReport\rptBlankReport.rdlc">
                        </LocalReport>
                    </rsweb:ReportViewer>
            </ContentTemplate>   
            <Triggers>
                <asp:PostBackTrigger ControlID="btnPDF" />
                <asp:PostBackTrigger ControlID="btnExcel" />
            </Triggers>      
        </asp:UpdatePanel> 
        
        <asp:UpdateProgress ID="upgResidue" runat="server" AssociatedUpdatePanelID="uplResidue">
            <ProgressTemplate>
                <br />
                    <img src="../images/progress.gif" alt="Processing" />
                    <asp:Label ID="lblProgress" CssClass="progress" Text="&nbsp;Processing ..." runat="server"></asp:Label>
            </ProgressTemplate>
        </asp:UpdateProgress>
        
    </div>
    </form>
</body>
</html>
