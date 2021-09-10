Imports Microsoft.Reporting.WebForms
Imports System.Web.Services.Description
Imports NEA_ICS.UserInterface.NEA_ICS.WcfService
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ServiceModel
Imports System.Reflection

Partial Public Class frmPrintVerificationWorksheet
    Inherits clsCommonFunction

#Region " PAGE LOAD "
    Private Message As String = EMPTY
    Public Property NoRecordFond() As String
        Get
            If ViewState("NoRecordFond") Is Nothing Then
                lblErrVerificationRefNo.Visible = False
                Return "N"
            Else
                Return ViewState("NoRecordFond").ToString()
            End If
        End Get
        Set(ByVal value As String)
            ViewState("NoRecordFond") = value
        End Set
    End Property

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ' alert user with message (if any)
        If Message <> EMPTY Then
            ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", "ShowAlertMessage('" & Message & "');", True)
        End If
    End Sub

    ''' <summary>
    ''' Sub Proc - Page Load
    ''' 05 Feb 2009 - Jianfa; 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim AccessRights As New List(Of RoleDetails)

        CheckValidSession()

        If Not Page.IsPostBack Then

            AccessRights = clsCommonFunction.AssignAccessRights(Cache(Session(ESession.UserID.ToString) & Session(ESession.StoreID.ToString) & "AccessRights"), _
                                                               clsCommonFunction.moduleID.PrintVerificationWorksheet)

            If AccessRights(0).SelectRight = False Then
                Server.Transfer("..\frmUnauthorisedPage.aspx")
                Exit Sub
            End If

            If AccessRights(0).DeleteRight = False Then
                btnCancelWorksheet.Visible = False
            End If

            If Not Request.QueryString("WorkSheetID") Is Nothing Then

                txtVerificationRefNo.Text = Request.QueryString("WorkSheetID")
                lblGeneratedDate.Text = Day(Now).ToString.PadLeft(2, "0") & "/" & Month(Now).ToString.PadLeft(2, "0") & "/" & Year(Now).ToString

            End If

            txtOfficerName.Text = Session("UserName").ToString
            txtDesignation.Text = Session("UserDesignation").ToString

        End If

    End Sub
#End Region

#Region " Print Verification Worksheet "

    ''' <summary>
    ''' txtVerficationRefNo - TextChanged;
    ''' 05 Feb 2009 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtVerificationRefNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtVerificationRefNo.TextChanged

        Try

            Dim Client As New ServiceClient
            Dim WorksheetDetails As New WorksheetDetails

            WorksheetDetails.StoreID = Session("StoreID")
            WorksheetDetails.WorkSheetID = txtVerificationRefNo.Text

            lblGeneratedDate.Text = Client.GetWorksheetGeneratedDate(WorksheetDetails)

            Client.Close()

        Catch ex As FaultException

            lblErrVerificationRefNo.Text = ex.Message
            lblErrVerificationRefNo.Visible = True

        Catch ex As Exception

            lblErrVerificationRefNo.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            lblErrVerificationRefNo.Visible = True
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If

        End Try

    End Sub

#End Region

    ''' <summary>
    ''' btnPDF - Click;
    ''' 06 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnPDF_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPDF.Click

        Try

            Dim Client As New ServiceClient
            Dim WorkSheetDetails As New WorksheetDetails

            WorkSheetDetails.StoreID = Session("StoreID")
            WorkSheetDetails.WorkSheetID = txtVerificationRefNo.Text
            WorkSheetDetails.VerifierName = txtOfficerName.Text
            WorkSheetDetails.CheckerName = txtCheckingOfficer.Text
            WorkSheetDetails.ApproverName = txtApprovingOfficer.Text
            WorkSheetDetails.LoginUser = Session("UserID")

            Client.UpdateWorksheet(WorkSheetDetails)
            Client.Close()

            rvr.LocalReport.DataSources.Clear()
            rvr.LocalReport.DataSources.Add(New ReportDataSource("WorksheetDetails", ObjectDataSource1))

            Dim withQty As String = rdoQty.SelectedValue
            Dim reportName As String
            If withQty = "Y" Then
                reportName = "Stock Verification Check List (With Quantity)"
            Else
                reportName = "Stock Verification Check List"
            End If

            Dim parameterlist As New List(Of ReportParameter)
            Dim p1 As New ReportParameter("ReportTitle", reportName)
            Dim p2 As New ReportParameter("StockTakeTeam", txtVerificationRefNo.Text)
            Dim p3 As New ReportParameter("CountValue", "")
            Dim p4 As New ReportParameter("CountDate", lblGeneratedDate.Text)
            Dim p5 As New ReportParameter("WithBalance", withQty)
            Dim p6 As New ReportParameter("CheckerName", txtCheckingOfficer.Text.ToUpper & " /")
            Dim p7 As New ReportParameter("VerifierName", txtOfficerName.Text.ToUpper & " /")
            Dim p8 As New ReportParameter("ApproverName", txtApprovingOfficer.Text.ToUpper & " /")
            Dim p9 As New ReportParameter("CheckerDesignation", txtCheckerDesignation.Text.ToUpper)
            Dim p10 As New ReportParameter("VerifierDesignation", txtDesignation.Text.ToUpper)
            Dim p11 As New ReportParameter("ApproverDesignation", txtApproverDesignation.Text.ToUpper)
            Dim p12 As New ReportParameter("Store", Session("StoreName").ToString)
            parameterlist.Add(p1)
            parameterlist.Add(p2)
            parameterlist.Add(p3)
            parameterlist.Add(p4)
            parameterlist.Add(p5)
            parameterlist.Add(p6)
            parameterlist.Add(p7)
            parameterlist.Add(p8)
            parameterlist.Add(p9)
            parameterlist.Add(p10)
            parameterlist.Add(p11)
            parameterlist.Add(p12)

            rvr.LocalReport.SetParameters(parameterlist)
            rvr.LocalReport.Refresh()

            Dim bytValue As Byte()
            bytValue = rvr.LocalReport.Render("PDF", Nothing, "application/pdf", "", "pdf", Nothing, Nothing)
            If NoRecordFond = "Y" Then
                Message = GetMessage(messageID.NoRecordFound)
                Exit Sub
            End If

            Response.Buffer = True
            Response.Clear()
            Response.ContentType = "application/pdf"
            Response.AddHeader("content-disposition", "attachment;filename=StockVerificationCheckList.pdf")
            Response.BinaryWrite(bytValue)
            Response.Flush()
            Response.End()

        Catch ex As Exception

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If

        End Try

    End Sub

    ''' <summary>
    ''' btnExcel - Click;
    ''' 06 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcel.Click

        Try

            Dim Client As New ServiceClient
            Dim WorkSheetDetails As New WorksheetDetails

            WorkSheetDetails.StoreID = Session("StoreID")
            WorkSheetDetails.WorkSheetID = txtVerificationRefNo.Text
            WorkSheetDetails.VerifierName = txtOfficerName.Text
            WorkSheetDetails.CheckerName = txtCheckingOfficer.Text
            WorkSheetDetails.ApproverName = txtApprovingOfficer.Text
            WorkSheetDetails.LoginUser = Session("UserID")

            Client.UpdateWorksheet(WorkSheetDetails)
            Client.Close()

            rvr.LocalReport.DataSources.Clear()
            rvr.LocalReport.DataSources.Add(New ReportDataSource("WorksheetDetails", ObjectDataSource1))

            Dim withQty As String = rdoQty.SelectedValue
            Dim reportName As String
            If withQty = "Y" Then
                reportName = "Stock Verification Check List (With Quantity)"
            Else
                reportName = "Stock Verification Check List"
            End If

            Dim parameterlist As New List(Of ReportParameter)
            Dim p1 As New ReportParameter("ReportTitle", reportName)
            Dim p2 As New ReportParameter("StockTakeTeam", txtVerificationRefNo.Text)
            Dim p3 As New ReportParameter("CountValue", "")
            Dim p4 As New ReportParameter("CountDate", lblGeneratedDate.Text)
            Dim p5 As New ReportParameter("WithBalance", withQty)
            Dim p6 As New ReportParameter("CheckerName", txtCheckingOfficer.Text.ToUpper & " /")
            Dim p7 As New ReportParameter("VerifierName", txtOfficerName.Text.ToUpper & " /")
            Dim p8 As New ReportParameter("ApproverName", txtApprovingOfficer.Text.ToUpper & " /")
            Dim p9 As New ReportParameter("CheckerDesignation", txtCheckerDesignation.Text.ToUpper)
            Dim p10 As New ReportParameter("VerifierDesignation", txtDesignation.Text.ToUpper)
            Dim p11 As New ReportParameter("ApproverDesignation", txtApproverDesignation.Text.ToUpper)
            Dim p12 As New ReportParameter("Store", Session("StoreName").ToString)
            parameterlist.Add(p1)
            parameterlist.Add(p2)
            parameterlist.Add(p3)
            parameterlist.Add(p4)
            parameterlist.Add(p5)
            parameterlist.Add(p6)
            parameterlist.Add(p7)
            parameterlist.Add(p8)
            parameterlist.Add(p9)
            parameterlist.Add(p10)
            parameterlist.Add(p11)
            parameterlist.Add(p12)

            rvr.LocalReport.SetParameters(parameterlist)
            rvr.LocalReport.Refresh()

            Dim bytValue As Byte()
            bytValue = rvr.LocalReport.Render("Excel", Nothing, "application/ms-excel", "", "xls", Nothing, Nothing)
            If NoRecordFond = "Y" Then
                Message = GetMessage(messageID.NoRecordFound)
                Exit Sub
            End If

            Response.Buffer = True
            Response.Clear()
            Response.ContentType = "application/ms-excel"
            Response.AddHeader("content-disposition", "attachment;filename=StockVerificationCheckList.xls")
            Response.BinaryWrite(bytValue)
            Response.Flush()
            Response.End()

        Catch ex As Exception

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If

        End Try

    End Sub

    ''' <summary>
    ''' btnCancelWorksheet - Click;
    ''' 06 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnCancelWorksheet_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelWorksheet.Click

        If txtVerificationRefNo.Text.Trim = String.Empty Then
            lblErrVerificationRefNo.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.MandatoryField)
            Exit Sub
        End If

        Try

            Dim Client As New ServiceClient
            Dim WorksheetDetails As New WorksheetDetails

            WorksheetDetails.StoreID = Session("StoreID")
            WorksheetDetails.WorkSheetID = txtVerificationRefNo.Text

            lblErrVerificationRefNo.Text = Client.DeleteWorksheet(WorksheetDetails)
            Client.Close()

            If lblErrVerificationRefNo.Text.Trim = String.Empty Then

                lblErrVerificationRefNo.Visible = False

                Dim Script As String = "ShowSuccessMessage('" & clsCommonFunction.GetMessage( _
                           clsCommonFunction.messageID.Success, "deleted", "Worksheet") & "');"

                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)

            Else
                lblErrVerificationRefNo.Visible = True
            End If

        Catch ex As FaultException

            lblErrVerificationRefNo.Text = ex.Message
            lblErrVerificationRefNo.Visible = True

        Catch ex As Exception

            lblErrVerificationRefNo.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            lblErrVerificationRefNo.Visible = True
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If

        End Try

    End Sub

    Protected Sub ObjectDataSource1_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs) Handles ObjectDataSource1.Selecting
        Dim searchWorksheet As New WorksheetDetails()
        searchWorksheet.StoreID = Session("StoreID")
        If Not String.IsNullOrEmpty(txtVerificationRefNo.Text.Trim()) Then
            searchWorksheet.WorkSheetID = Convert.ToInt32(txtVerificationRefNo.Text)
        End If

        e.InputParameters("workSheetDetails") = searchWorksheet
        e.InputParameters("sortExpression") = ddlSortBy.SelectedValue
        e.InputParameters("sortDirection") = ddlSortDirection.SelectedValue

    End Sub
    Protected Sub ObjectDataSource1_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles ObjectDataSource1.Selected
        Dim returnList As List(Of WorksheetDetails) = e.ReturnValue
        If returnList.Count <= 0 Then
            NoRecordFond = "Y"
        Else
            NoRecordFond = "N"
        End If
    End Sub
End Class