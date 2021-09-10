<%@ Page Language="vb" AutoEventWireup="false"  %>

<%@ Import Namespace="NEA_ICS.UserInterface.NEA_ICS.WcfService" %>
<HTML>
<Title>Verify.aspx</Title>
<Body bgColor="#66FFFF" background="Images/orgbkg.gif">

<%
    Dim onlyUserName
    Dim oPassword
    Dim oUsername
    Dim MyNamespace
    Dim LoginStatus
    Dim HTML = ""
    Dim X
    Dim DN
    
    On Error Resume Next
 const ADS_SECURE_AUTHENTICATION=&h0001
 
 '----PARAMETER STRING FOR ADO COMMAND-----------
 onlyUserName = Request.Form("UserID")
 '------------------------------------------------
    
    'UAT Domain
    oUsername = "neatest.net\" & onlyUserName
    'oUsername = "nea.net\" & onlyUserName
 dim oRetURL 
 oRetURL = Request("hiddenURL")
 oPassword=Request.Form("getPassword")

 'DN="LDAP://nea.sb/OU=HQ,OU=NEA Users,DC=nea,DC=sb"
    'changed above line to test on single sign on by mun fai on 15 May 08
    'UAT Domain
    DN = "LDAP://neatest.net"
    'DN = "LDAP://nea.net"
    MyNamespace = GetObject("LDAP:")
    X = MyNamespace.OpenDSObject(DN, oUsername, oPassword, ADS_SECURE_AUTHENTICATION)
  HTML = HTML & "<br> err.number:" & err.number
    
    If Err.Number <= 0 Then
			
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

    Else
        Dim userId As String = Trim(onlyUserName.ToString.ToUpper)
        Session("UserID") = userId
        Dim Client As ServiceClient
        Client = New ServiceClient
        Dim RoleDetails As New RoleDetails
        RoleDetails.UserID = userId

        Dim userExist As Boolean
        userExist = Client.CheckUserIdExist(userId)
        Client.Close()
        
        Client = New ServiceClient
        If Not userExist Then
            ' if invalid/non-nea user or non-ics user then tag as 1 (Invalid account report)
            
            Client.AddUserAudit("", userId, Request.UserHostAddress, "", True, False, True)
            Client.Close()
        Else
            
            'Response.Redirect("frmSelectStore.aspx?err=1")
            ' List of unsuccessful login attempt
            ' tag as ics unsuccessful login for invalid password
            
            Dim storeCodes As String
            storeCodes = Client.GetUserStoreCodes(userId)
            Client.Close()
            
            If storeCodes.Length > 0 Then
                storeCodes = Left(storeCodes, storeCodes.Length - 1)
                Dim code As String = ""
                Dim arrStore As String() = Split(storeCodes, ",")
                For idx As Integer = 0 To UBound(arrStore)
                    code = arrStore(idx)
                    code = Replace(code, ",", "")
                    'idx+=1
                    Client = New ServiceClient
                    Client.AddUserAudit(code, userId, Request.UserHostAddress, "", False, False, True)
                    Client.Close()
                Next
            End If
            
        End If
    End If
        
    Response.Write("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<p><font size='+1'>Error : </font><br>")
    Response.Write("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Login failed. Please verify your login information.</p>")
    Response.Write("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Try <a href='login.aspx'>Logging in</a> again")
    Response.End()
%>

</Body>
</HTML>