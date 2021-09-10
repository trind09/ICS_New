<%@ Page Language="vb" AutoEventWireup="false"  %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Inventory Control System (ICS)</title>
    
    <script type="text/javascript">
    
      	function encrypt()
		{

			document.forms[0].UserPassword.value=document.forms[0].des.hexEncrypt(document.forms[0].getPassword.value);
			return false;
		}
 
    </script>
    
</head>
<body bgcolor="#ffffff" background="Images/login.jpg">
    <%
        Dim errorMessage As String = Request("err")
        If errorMessage <> "" Then ' if no error found from prev. process
            'ScriptManager.RegisterStartupScript(Page, GetType(Page), "AlertRegister", "alert('Login failed. Please verify your login information.');", True)
            Response.Write("<font align='center' face='Arial' size='2' color='#ff0000'>Login failed. Please verify your login information.</font>")
        End If
    %>
        
		<FORM ACTION="verify.aspx" METHOD="post" NAME="FORM1" ID="Form2">


		    <TABLE width="100%" border="0" align="center" cellpadding="1" cellspacing="1">			
				<TR>
					<TD height="70" colspan="2">&nbsp;</TD>
				</TR>
				<TR>
					<TD width="35%"><div align="right"><IMG src="images/mainmenuv2_nealogo.gif" alt="" border="0" align="absMiddle"></div>
					</TD>
					<TD width="65%" align="center" valign="bottom">
						<div align="left">&nbsp;&nbsp;&nbsp;&nbsp;<FONT face="Geneva, Arial, Helvetica, sans-serif" color="#00604d" size="6"><STRONG><font color="#df6a00">NEA</font>
									INTRANET</STRONG></FONT></div>
					</TD>
				</TR>
		    </TABLE>
		    <table width="100%">
         		<TR>
					<TD align="center">
							<OBJECT id="Shockwaveflash1" codeBase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=4,0,2,0"
								height="93" width="391" classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" VIEWASTEXT>
								<PARAM NAME="_cx" VALUE="10345">
								<PARAM NAME="_cy" VALUE="2461">
								<PARAM NAME="FlashVars" VALUE="">
								<%--<PARAM NAME="Movie" VALUE="neaintranet.swf">
								<PARAM NAME="Src" VALUE="neaintranet.swf">--%>
								<PARAM NAME="WMode" VALUE="Window">
								<PARAM NAME="Play" VALUE="-1">
								<PARAM NAME="Loop" VALUE="-1">
								<PARAM NAME="Quality" VALUE="High">
								<PARAM NAME="SAlign" VALUE="">
								<PARAM NAME="Menu" VALUE="-1">
								<PARAM NAME="Base" VALUE="">
								<PARAM NAME="AllowScriptAccess" VALUE="always">
								<PARAM NAME="Scale" VALUE="ShowAll">
								<PARAM NAME="DeviceFont" VALUE="0">
								<PARAM NAME="EmbedMovie" VALUE="0">
								<PARAM NAME="BGColor" VALUE="">
								<PARAM NAME="SWRemote" VALUE="">
								<PARAM NAME="MovieData" VALUE="">
								<PARAM NAME="SeamlessTabbing" VALUE="1">
								<%--<embed src="neaintranet.swf" type="application/x-shockwave-flash" width="391" height="93"
									pluginspage="http://www.macromedia.com/shockwave/download/index.cgi?P1_Prod_Version=ShockwaveFlash"
									quality="best" play="true">--%>
							</OBJECT>
						<P align="center"><IMG src="images/login_14.gif" alt="" width="382" height="37" border="0" align="absMiddle"></P>
					</TD>
				</TR>

				<TR>
					<TD align="center" style="width: 1038px">&nbsp;</TD>
				</TR>
				<TR>
					<TD align="center" style="width: 1038px">&nbsp;</TD>
				</TR>
				<TR>
					<TD align="center" style="width: 1038px">
						<TABLE width="100%" border="0" align="center" cellpadding="1" cellspacing="1" id="Table1">
							<!-- Added in on 09 Oct 08 SR108184929 : START -->
							<tr>
								<TD colspan="3"><font size="2" face="Arial, Helvetica, sans-serif"><strong>For authorised use only. Unauthorised use is strictly prohibited.</strong></font></TD>
							</tr>
							<!-- Added in on 09 Oct 08 SR108184929 : END -->
							<tr>
								<td width="40%" vAlign="top" align="right"><br /><strong><font size="2" face="Arial, Helvetica, sans-serif">User
											ID :</font></strong></td>
								<td width="60%" align="left"><br /><input id="Text1" type="text" size="20" name="UserID" AUTOCOMPLETE="Off"> &nbsp;<font face="arial" color="blue" size="1">(eg.
										S1234567A)</font></td>
							</tr>
							<tr>
								<td width="40%" vAlign="middle" align="right"><font size="2" face="Arial"><strong>Password :</strong></font></td>
								<td width="60%" vAlign="middle" align="left"><input id="Hidden1" type="hidden" size="20" name="UserPassword">
							 <input align="left" type="password" size="20" name="getPassword" AUTOCOMPLETE="Off">&nbsp;&nbsp;&nbsp;
 									<input type="image" name="login" src="images/login.gif" border="0" onClick='' ID="Image1">
									 
								</td>
							</tr>
    </table>
    
    
    

   
    </form>
</body>
</html>
