<%@ Page Language="vb" AutoEventWireup="false"CodeBehind="EmailContent.aspx.vb" Inherits="NEA_ICS.UserInterface.EmailContent" ValidateRequest="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Inventory Control System (ICS)</title>
    <script type="text/javascript" src="../Script/NEA_ICS.js">
    
      function pageLoad() {
      }
         
    </script>

    <style type="text/css">
        body {
            margin-top: 0;
            margin-left: 0;
        }
    </style>
    <link href="../style/ICS.css" type="text/css" rel="Stylesheet" />
    <meta http-equiv='refresh' content='1800;url=..\frmUnauthorisedPage.aspx' />

</head>
<body>
    <form id="form1" runat="server">
        <table class="moduleTitle" width="94%" cellspacing="1" cellpadding="1">
            <tr>
                <td class="moduleTitleBorder">&nbsp;Master List > Email Content
                </td>
            </tr>
        </table>
        <table class="tblModule" cellspacing="1">
            <tr>
                <td class="colMod" width="20%">Format Name *                                            
                </td>
                <td class="colDesc" width="80%" colspan="3">
                    <asp:DropDownList ID="ddlFormatName" runat="server" CssClass="formsCombo" Width="70%" OnSelectedIndexChanged="ddlFormatName_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Value="reorderitem">Reorder Item</asp:ListItem>
                        <asp:ListItem Value="Reminderitem">90 days Remainder</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr runat="server" id="trToAddr">
                <td class="colMod" width="20%">To *                                            
                </td>
                <td class="colDesc" width="80%" colspan="3">
                    <asp:TextBox ID="txtTo" runat="server" Text="" Width="70%"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqtoemail" runat="server" controltovalidate="txtTo" Text="Please Enter To email address"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ValidationExpression="^([\w+-.%]+@[\w-.]+\.[A-Za-z]{2,4},?)+$" ControlToValidate="txtTo" ErrorMessage="Invalid Email Format"></asp:RegularExpressionValidator>
                </td>
            </tr>
          
              <tr runat="server" id="trFirstRemainder">
                <td class="colMod" width="20%">First Email Reminder On *                                            
                </td>
                <td class="colDesc" width="80%" colspan="3">
                    <asp:TextBox ID="txtFirstRemainder" runat="server" Text="" Width="70%"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="Reqfirstreminder" runat="server" controltovalidate="txtFirstRemainder" Text="Please Enter first Email remainder"></asp:RequiredFieldValidator>
                    <asp:RangeValidator runat="server" id="rngDate" controltovalidate="txtFirstRemainder" type="Integer" minimumvalue="2" maximumvalue="90" errormessage="Please enter a valid number of days and less than 90 days" />
                     <asp:RegularExpressionValidator id="txtFirstRemainder1"  runat="server" ErrorMessage="" ValidationExpression="^[0-9]+$" ControlToValidate="txtFirstRemainder" />
                    </td>
            </tr>
            <tr runat="server" id="trSecondRemainder">
                <td class="colMod" width="20%">Second Email Reminder On *                                            
                </td>
                
                  <td class="colDesc" width="80%" colspan="3">
                    <asp:TextBox ID="txtSecondRemainder" runat="server" Text="" Width="70%"></asp:TextBox>
                     <asp:RequiredFieldValidator ID="txtSecondRemainder1" runat="server" controltovalidate="txtSecondRemainder" Text="Please Enter the Second Reminder"></asp:RequiredFieldValidator>
                      <asp:CompareValidator runat="server" id="cmpNumbers" controltovalidate="txtSecondRemainder" controltocompare="txtFirstRemainder" operator="LessThan" type="Integer" errormessage="Please enter a valid number of days and smaller than the first reminder" />
                       <asp:RegularExpressionValidator id="RegularExpressionValidator2"  runat="server" ErrorMessage="" ValidationExpression="^[0-9]+$" ControlToValidate="txtSecondRemainder" />
                    
                </td>
            </tr>
              <tr>
                <td class="colMod" width="20%">CC *                                            
                </td>
                <td class="colDesc" width="80%" colspan="3">
                    <asp:TextBox ID="txtCC" runat="server" Text="" Width="70%"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" controltovalidate="txtCc" Text="Please Enter Cc email address" Display="Dynamic"></asp:RequiredFieldValidator><br />
                    <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" ValidationExpression="^([\w+-.%]+@[\w-.]+\.[A-Za-z]{2,4},?)+$" ControlToValidate="txtCC" ErrorMessage="Invalid Email Format" Display="Dynamic"></asp:RegularExpressionValidator>
                </td>
            </tr>
            
            <tr>
                <td class="colMod" width="20%">Subject *                                            
                </td>
                <td class="colDesc" width="80%" colspan="3">
                    <asp:TextBox ID="txtSubject" runat="server" Text="" Width="70%"></asp:TextBox></br>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" controltovalidate="txtSubject" Text="Please Enter the subject"></asp:RequiredFieldValidator><br />
                    <asp:RegularExpressionValidator id="RegularExpressionValidator1"  runat="server" ErrorMessage="Please Enter the subject only in text" ValidationExpression="^[a-zA-Z0-9_ ]*$" ControlToValidate="txtSubject" />

                </td>
            </tr>
            <tr>
                <td class="colMod" width="20%">Message Format *                                            
                </td>
                <td class="colDesc" width="80%" colspan="3">
                    <asp:TextBox ID="txtBody" runat="server" Text="" TextMode="MultiLine" Height="150px" Width="70%"></asp:TextBox></br>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" controltovalidate="txtBody" Text="Please Enter the Message"></asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="customtxtbody" runat="server" ControlToValidate="txtBody" OnServerValidate="CV_txtbody"></asp:CustomValidator>
                </td>
            </tr>

            <tr>
                <td colspan="3" class="errMsg">* denotes mandatory fields
                </td>
                 <td class="errMsg" style="text-align:end"><asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Green"></asp:Label>
                </td>
            </tr>
        </table>

        <br />
        <div align="center">
            <asp:Button ID="btnCancel" CssClass="formsButton" Text="Cancel" runat="server" CausesValidation="false" />
            <asp:Button ID="btnSave" CssClass="formsButton" Text="Save" runat="server" OnClick="btnSave_Click" CausesValidation="true" />
        </div>
    </form>
</body>
</html>
