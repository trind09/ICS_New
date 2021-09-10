<%@ Page Language="vb" AutoEventWireup="false"  %>

<%@ Import Namespace="NEA_ICS.UserInterface.NEA_ICS.WcfService" %>
<HTML>
<Title>Verify.aspx</Title>
<Body bgColor="#66FFFF" background="Images/orgbkg.gif">

<%
    Dim onlyUserName As String = ""
    Dim LoginStatus

    If HttpContext.Current.User.Identity.IsAuthenticated = True Then
        Dim username = HttpContext.Current.User.Identity.Name
        Dim Client As New ServiceClient
        Dim userRole = Client.GetUserRoleIDBySoeID(username)
        If userRole IsNot Nothing Then
            onlyUserName = userRole.Tables(0).Rows(0)("UserRoleUserID")
        End If
    End If

    If onlyUserName IsNot Nothing Then

        Session("UserID") = Trim(onlyUserName.ToString.ToUpper)

        Response.Write("Session(UserID) = " & Session("UserID"))
        'Response.End()        

        Session("Authenticated") = 1
        ID = onlyUserName

        Dim Client As New ServiceClient
        Dim userExist As Boolean
        userExist = Client.CheckUserIdExist(onlyUserName)
        Client.Close()

        If Not userExist Then
            ' if nea user but nont ics user then tag as invalid ics user
            Client.AddUserAudit("", onlyUserName, Request.UserHostAddress, "", True, False, True)
            Client.Close()
            Response.Redirect(ConfigurationManager.AppSettings("LoginURL").ToString() & "?err=1")
        End If

        LoginStatus = "Pass" ' AD authentication passed

        Response.Redirect("frmSelectStore.aspx")

        '-----------------------PREVIOUS CODE---------------------------------------	
        'sqlpwd2 = "update ACCESSPERT set Last_login_DT = getdate() where NRIC = '" & Request.Form("UserID") & "'"	
        'set rstemp2=conntemp.Execute(sqlpwd2)
        '------------------- END OF PREVIOUS CODE-----------------------------------

        '---------------AMENDED BY SHIRLEY ON 21-01-2009 (WEDNESDAY) FOR SQL INJECTION-------------------------------
        'STEP 3:
        '-----------REDECLARE ADO COMMAND (21-01-2009)------------
        '	Set objLastCmd  = Server.CreateObject("ADODB.Command")
        '	Set rstemp2= Server.CreateObject("Adodb.Recordset")
        '------------------------------------------------------
        'sqlpwd2 = "update ACCESSPERT set Last_login_DT = getdate() where NRIC = ?"			

        '		Set tmpParam = ObjLastCmd.CreateParameter("@onlyUserName",adVarChar,1,1000,onlyUserName)
        '	objLastCmd.Parameters.Append tmpParam
        '	objLastCmd.Commandtext = sqlpwd2
        '	objLastCmd.CommandType = adCmdText
        '	objLastCmd.ActiveConnection = conntemp

        '		with rstemp2
        '			.CursorLocation = 3 'adUserClient
        '	.CursorType = adOpenDynamic
        '	.LockType = 3 'adOpenLockOptimistic
        '	.Open objLastCmd
        '	Set .ActiveConnection = Nothing
        '	End With

        '		objlastcmd.Execute sqlpwd2

    End If

    Response.Write("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<p><font size='+1'>Error : </font><br>")
    Response.Write("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Login failed. Please verify your login information.</p>")
    Response.Write("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Try <a href='login.aspx'>Logging in</a> again")
    Response.End()
%>

</Body>
</HTML>