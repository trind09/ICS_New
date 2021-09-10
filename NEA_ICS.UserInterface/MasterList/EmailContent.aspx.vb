Imports Microsoft.Reporting.WebForms
Imports System.Web.Services.Description
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ServiceModel
Imports System.Reflection
Imports NEA_ICS.UserInterface.NEA_ICS.WcfService



Partial Public Class EmailContent
    Inherits clsCommonFunction


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim storeID As String = Session("StoreID")
            Dim emailFormat As String = ddlFormatName.SelectedValue

            bindValues(storeID, emailFormat)
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs)
        Dim storeID As String = Session("StoreID")
        Dim emailFormat As String = ddlFormatName.SelectedValue
        Dim toAddr As String = txtTo.Text
        Dim ccAddr As String = txtCC.Text
        Dim subject As String = txtSubject.Text
        Dim msgFormat As String = txtBody.Text
        Dim firstRemainder As String = txtFirstRemainder.Text
        Dim secondRemainder As String = txtSecondRemainder.Text
        Dim loginUser As String = Session("UserID").ToString
        Dim errorMessage As String = ""
        lblMsg.Text = ""
        Try
            If Page.IsValid Then
                If emailFormat = "reorderitem" Then
                    If storeID = "" _
                         Or ccAddr = "" _
                         Or subject = "" _
                         Or msgFormat = "" _
                          Or loginUser = "" Then
                        errorMessage = "Missing Mandatory Fields"
                    End If
                Else
                    If storeID = "" _
                         Or ccAddr = "" _
                         Or subject = "" _
                         Or msgFormat = "" _
                         Or firstRemainder = "" _
                         Or secondRemainder = "" _
                         Or loginUser = "" Then
                        errorMessage = "Missing Mandatory Fields"
                    End If
                End If



                If errorMessage <> "" Then
                    lblMsg.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.MandatoryField)
                    lblMsg.Visible = True

                    Dim Script As String = "ShowAlertMessage('" & lblMsg.Text & "');"
                    ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

                Else
                    Dim Client As New ServiceClient
                    Dim emailContent As New NEA_ICS.WcfService.EmailContent

                    emailContent.StoreId = storeID
                    emailContent.ToAddress = toAddr
                    emailContent.CCAddress = ccAddr
                    emailContent.EmailFormat = emailFormat
                    emailContent.msgFormat = msgFormat
                    emailContent.FirstRemainder = firstRemainder
                    emailContent.SecondRemainder = secondRemainder
                    emailContent.Subject = subject
                    emailContent.LoginUser = loginUser

                    lblMsg.Text = Client.AddEmailContent(emailContent)

                    If lblMsg.Text = "" Then
                        lblMsg.Text = "Data Saved Successfully."
                    End If
                    Client.Close()



                End If
            End If

        Catch ex As FaultException

            Dim fault As ServiceFault = ex.Data

            lblMsg.Text = fault.MessageText
            lblMsg.Visible = True

        Catch ex As Exception
            lblMsg.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            lblMsg.Visible = True
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try


















    End Sub

    Protected Sub ddlFormatName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlFormatName.SelectedIndexChanged
        Dim storeID As String = Session("StoreID")
        Dim emailFormat As String = ddlFormatName.SelectedValue

        bindValues(storeID, emailFormat)
    End Sub
    Private Sub bindValues(ByVal storeID As String, ByVal emailFormat As String)

        If (ddlFormatName.SelectedItem.ToString = "90 days Remainder") Then
            trToAddr.Visible = False
            trFirstRemainder.Visible = True
            trSecondRemainder.Visible = True
            txtBody.Text = "#Name#<br>#Days#"
        Else
            trFirstRemainder.Visible = False
            trSecondRemainder.Visible = False
            trToAddr.Visible = True
            txtBody.Text = "#Team#<br>#Date#"
        End If

        lblMsg.Text = ""
        Dim client As New ServiceClient
        Dim emailOutput As New List(Of String)
        emailOutput = client.GetEmailContent(storeID, emailFormat)

        txtTo.Text = ""
        txtCC.Text = ""
        txtSubject.Text = ""
        txtBody.Text = ""
        txtFirstRemainder.Text = ""
        txtSecondRemainder.Text = ""

        If (emailOutput.Count > 0) Then
            txtTo.Text = emailOutput(0).ToString
            txtCC.Text = emailOutput(1).ToString
            txtSubject.Text = emailOutput(2).ToString
            txtBody.Text = emailOutput(3).ToString
            txtFirstRemainder.Text = emailOutput(4).ToString
            txtSecondRemainder.Text = emailOutput(5).ToString
        End If

    End Sub

    Protected Sub CV_txtbody(source As Object, args As ServerValidateEventArgs)
        If (ddlFormatName.SelectedValue = "reorderitem") Then
            If ((txtBody.Text.Contains("#Team#")) And (txtBody.Text.Contains("#Date#"))) Then
                args.IsValid = True
            Else
                args.IsValid = False
                customtxtbody.ErrorMessage = "Please use #Team# and #Date# format"
            End If
        Else
            If ((txtBody.Text.Contains("#Name#")) And (txtBody.Text.Contains("#Days#"))) Then
                args.IsValid = True
            Else
                args.IsValid = False
                customtxtbody.ErrorMessage = "Please use #Name# and #Days# format"
            End If
        End If
    End Sub

    'Private Function lblMsg() As Object
    '    Throw New NotImplementedException
    'End Function

    'Private Function ddlFormatName() As Object
    '    Throw New NotImplementedException
    'End Function

    'Private Function txtTo() As Object
    '    Throw New NotImplementedException
    'End Function

    'Private Function txtCC() As Object
    '    Throw New NotImplementedException
    'End Function

    'Private Function txtSubject() As Object
    '    Throw New NotImplementedException
    'End Function

    'Private Function txtBody() As Object
    '    Throw New NotImplementedException
    'End Function

    'Private Function txtFirstRemainder() As Object
    '    Throw New NotImplementedException
    'End Function

    'Private Function txtSecondRemainder() As Object
    '    Throw New NotImplementedException
    'End Function

    'Private Function trToAddr() As Object
    '    Throw New NotImplementedException
    'End Function

    'Private Function trFirstRemainder() As Object
    '    Throw New NotImplementedException
    'End Function

    'Private Function trSecondRemainder() As Object
    '    Throw New NotImplementedException
    'End Function

    'Private Function customtxtbody() As Object
    '    Throw New NotImplementedException
    'End Function

    'Private Function customtxtbody() As Object
    '    Throw New NotImplementedException
    'End Function


End Class