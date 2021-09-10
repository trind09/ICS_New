<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmICS.aspx.vb" Inherits="NEA_ICS.UserInterface.frmICS" ValidateRequest="true" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Inventory Control System (ICS)</title>
    <script type="text/javascript">
    
      function pageLoad() {
      }
    
    </script>
</head>

<frameset cols="55, 900, *">

    <frame name="fillerleft" src="frmFiller.aspx" frameborder="0" framespacing="0" border="0" marginheight="0" marginwidth="0" noreSize scrolling="No">
    
    <frameset rows="85, *">
        <frame name="ICSBanner" src="frmFrameTop.aspx" frameborder="0" framespacing="0" border="0" marginheight="0" marginwidth="0" noResize scrolling="No">
        <frameset cols="208, 0, *">
            <frame name="leftFrame" src="frmTreeView.aspx" frameborder="0" framespacing="0" border="0" marginheight="0" marginwidth="0" noResize scrolling="auto">
            <frame name="midFrame" src="frmMidFrame.aspx" frameborder="0" framespacing="0" border="0" marginheight="0" marginwidth="0" scrolling="No">
            <frameset rows="20, *">
                <frame name="login" src="frmLogin.aspx" frameborder="0" framespacing="0" border="0" marginheight="0" marginwidth="0" noResize scrolling="No">
                <frame name="mainFrame" src="frmMain.aspx" frameborder="0" framespacing="0" border="0" marginheight="0" marginwidth="0" noResize scrolling="auto">
            </frameset>        
        </frameset>
    </frameset>   
     
    <frame name="fillerright" src="frmFiller.aspx" frameborder="0" framespacing="0" border="0" marginheight="0" marginwidth="0" noreSize scrolling="No">        

</frameset>

    <noframes>
		<p id="p1">
			This HTML frameset displays multiple Web pages. To view this frameset, use a 
			Web browser that supports HTML 4.0 and later.
		</p>
	</noframes>
</frameset>

</html>
