Imports Microsoft.Reporting.WebForms
Imports System.Web.Services.Description
Imports NEA_ICS.UserInterface.NEA_ICS.WcfService
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ServiceModel
Imports System.Reflection

Partial Public Class frmStockReturnCheckList
    Inherits clsCommonFunction
    Private Message As String = EMPTY
    Public Property NoRecordFond() As String
        Get
            If ViewState("NoRecordFond") Is Nothing Then
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
            ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", "alert('" & Message & "');", True)
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim AccessRights As New List(Of RoleDetails)

            CheckValidSession()

            If Not Page.IsPostBack Then
                AccessRights = clsCommonFunction.AssignAccessRights(Cache(Session(ESession.UserID.ToString) & Session(ESession.StoreID.ToString) & "AccessRights"), _
                                        clsCommonFunction.moduleID.StockReturnCheckList)

                If AccessRights(0).SelectRight = False Then
                    Server.Transfer("..\frmUnauthorisedPage.aspx")
                    Exit Sub
                End If
            End If

            If Me.ckbTransDate.Checked Then
                txtDateFrom.Text = ""
                txtDateTo.Text = ""
                txtDateFrom.Enabled = False
                txtDateTo.Enabled = False

            Else
                txtDateFrom.Enabled = True
                txtDateTo.Enabled = True

            End If
            If Not Page.IsPostBack Then
                txtDateFrom.Text = (New Date(Now.AddMonths(-1).Year, Now.AddMonths(-1).Month, 1)).ToString("dd/MM/yyyy")
                txtDateTo.Text = (New Date(Now.Year, Now.Month, 1).AddDays(-1)).ToString("dd/MM/yyyy")
            End If

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    Protected Sub btnPDF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPDF.Click
        Try
            If txtDateFrom.Text.Trim = String.Empty Or txtDateTo.Text.Trim = String.Empty Then
                If Not ckbTransDate.Checked Then
                    Message = GetMessage(messageID.MandatoryField)
                    Exit Sub
                End If
            End If

            ' UAT02
            If ddlReportType.SelectedValue = EMPTY Then
                Message = GetMessage(messageID.MandatoryField)
                Exit Sub
            End If

            If ConvertToDate(txtDateFrom.Text) > ConvertToDate(txtDateTo.Text) Then
                Message = GetMessage(messageID.DateToEarlierDateFrom)
                Exit Sub
            End If

            rvr.Reset()
            rvr.LocalReport.DataSources.Clear()
            Select Case ddlReportType.SelectedValue
                Case "R"
                    rvr.LocalReport.ReportPath = "ManagementReport\rptMR006StockReturnCheckListReceive.rdlc"
                    rvr.LocalReport.DataSources.Add(New ReportDataSource("MR006StockReturnCheckListReceiveDetails", ObjectDataSource1))
                Case "I"
                    If ddlGroupBy.SelectedValue = "S" Then
                        rvr.LocalReport.ReportPath = "ManagementReport\rptMR006StockReturnCheckListIssue.rdlc"
                    Else
                        rvr.LocalReport.ReportPath = "ManagementReport\rptMR006StockReturnCheckListIssueConsumer.rdlc"
                    End If
                    rvr.LocalReport.DataSources.Add(New ReportDataSource("MR006StockReturnCheckListIssueDetails", ObjectDataSource2))
                Case "A"
                    rvr.LocalReport.ReportPath = "ManagementReport\rptMR006StockReturnCheckListAdjust.rdlc"
                    rvr.LocalReport.DataSources.Add(New ReportDataSource("MR006StockReturnCheckListAdjustDetails", ObjectDataSource3))
                Case "B"
                    rvr.LocalReport.ReportPath = "ManagementReport\rptMR006StockReturnCheckListStockItem.rdlc"
                    rvr.LocalReport.DataSources.Add(New ReportDataSource("MR006StockReturnCheckListStockItemDetails", ObjectDataSource4))
            End Select

            Dim paraDateFrom As String
            Dim paraDateTo As String
            If Me.ckbTransDate.Checked Then
                paraDateFrom = "All"
                paraDateTo = "All"
            Else
                paraDateFrom = Me.txtDateFrom.Text
                paraDateTo = Me.txtDateTo.Text
            End If

            Dim parameterlist As New List(Of ReportParameter)
            Dim p1 As New ReportParameter("Report_Name", Session("StoreName").ToString())
            Dim p2 As New ReportParameter("DateFrom", paraDateFrom)
            Dim p3 As New ReportParameter("DateTo", paraDateTo)
            parameterlist.Add(p1)
            parameterlist.Add(p2)
            parameterlist.Add(p3)
            If ddlReportType.SelectedValue = "I" Then
                Dim p4 As New ReportParameter("GroupBy", ddlGroupBy.SelectedItem.Text)
                parameterlist.Add(p4)
            End If

            rvr.LocalReport.SetParameters(parameterlist)
            rvr.LocalReport.Refresh()

            Dim bytValue As Byte()
            bytValue = rvr.LocalReport.Render("PDF", Nothing, "application/pdf", "", "pdf", Nothing, Nothing)

            'If NoRecordFond = "Y" Then
            '    Message = GetMessage(messageID.NoRecordFound)
            '    Exit Sub
            'End If

            Response.Buffer = True
            Response.Clear()
            Response.ContentType = "application/pdf"
            Response.AddHeader("content-disposition", "attachment;filename=StockReturnCheckList.pdf")
            Response.BinaryWrite(bytValue)
            Response.Flush()
            Response.End()

        Catch ex As Exception
            Response.Write(ex.InnerException.Message)
            Response.End()
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExcel.Click
        Try
            If txtDateFrom.Text.Trim = String.Empty Or txtDateTo.Text.Trim = String.Empty Then
                If Not ckbTransDate.Checked Then
                    Message = GetMessage(messageID.MandatoryField)
                    Exit Sub
                End If
            End If

            ' UAT02
            If ddlReportType.SelectedValue = EMPTY Then
                Message = GetMessage(messageID.MandatoryField)
                Exit Sub
            End If

            If ConvertToDate(txtDateFrom.Text) > ConvertToDate(txtDateTo.Text) Then
                Message = GetMessage(messageID.DateToEarlierDateFrom)
                Exit Sub
            End If

            rvr.Reset()
            rvr.LocalReport.DataSources.Clear()
            Select Case ddlReportType.SelectedValue
                Case "R"
                    rvr.LocalReport.ReportPath = "ManagementReport\rptMR006StockReturnCheckListReceive.rdlc"
                    rvr.LocalReport.DataSources.Add(New ReportDataSource("MR006StockReturnCheckListReceiveDetails", ObjectDataSource1))
                Case "I"
                    If ddlGroupBy.SelectedValue = "S" Then
                        rvr.LocalReport.ReportPath = "ManagementReport\rptMR006StockReturnCheckListIssue.rdlc"
                    Else
                        rvr.LocalReport.ReportPath = "ManagementReport\rptMR006StockReturnCheckListIssueConsumer.rdlc"
                    End If
                    rvr.LocalReport.DataSources.Add(New ReportDataSource("MR006StockReturnCheckListIssueDetails", ObjectDataSource2))
                Case "A"
                    rvr.LocalReport.ReportPath = "ManagementReport\rptMR006StockReturnCheckListAdjust.rdlc"
                    rvr.LocalReport.DataSources.Add(New ReportDataSource("MR006StockReturnCheckListAdjustDetails", ObjectDataSource3))
                Case "B"
                    rvr.LocalReport.ReportPath = "ManagementReport\rptMR006StockReturnCheckListStockItem.rdlc"
                    rvr.LocalReport.DataSources.Add(New ReportDataSource("MR006StockReturnCheckListStockItemDetails", ObjectDataSource4))
            End Select

            Dim paraDateFrom As String
            Dim paraDateTo As String
            If Me.ckbTransDate.Checked Then
                paraDateFrom = "All"
                paraDateTo = "All"
            Else
                paraDateFrom = Me.txtDateFrom.Text
                paraDateTo = Me.txtDateTo.Text
            End If

            Dim parameterlist As New List(Of ReportParameter)
            Dim p1 As New ReportParameter("Report_Name", Session("StoreName").ToString())
            Dim p2 As New ReportParameter("DateFrom", paraDateFrom)
            Dim p3 As New ReportParameter("DateTo", paraDateTo)
            parameterlist.Add(p1)
            parameterlist.Add(p2)
            parameterlist.Add(p3)
            If ddlReportType.SelectedValue = "I" Then
                Dim p4 As New ReportParameter("GroupBy", ddlGroupBy.SelectedItem.Text)
                parameterlist.Add(p4)
            End If

            rvr.LocalReport.SetParameters(parameterlist)
            rvr.LocalReport.Refresh()
            rvr.AsyncRendering = False

            Dim bytValue As Byte()
            bytValue = rvr.LocalReport.Render("Excel", Nothing, "application/ms-excel", "", "xls", Nothing, Nothing)


            'If NoRecordFond = "Y" Then
            '    Message = GetMessage(messageID.NoRecordFound)
            '    Exit Sub
            'End If

            Response.Buffer = True
            Response.Clear()
            Response.ContentType = "application/ms-excel"
            Response.AddHeader("content-disposition", "attachment;filename=StockReturnCheckList.xls")
            Response.BinaryWrite(bytValue)
            Response.Flush()
            Response.End()

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    Private Sub ddlReportType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReportType.SelectedIndexChanged

        If ddlReportType.SelectedValue = "B" Then

            chkExcludeZero.Visible = True

        Else

            chkExcludeZero.Visible = False

        End If

    End Sub

    Protected Sub ObjectDataSource1_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles ObjectDataSource1.Selected
        Try
            Dim returnList As List(Of MR006StockReturnCheckListReceiveDetails) = e.ReturnValue
            If returnList.Count <= 0 Then
                NoRecordFond = "Y"
            Else
                NoRecordFond = "N"
            End If

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    Protected Sub ObjectDataSource1_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs) Handles ObjectDataSource1.Selecting
        Try
            e.InputParameters("storeId") = Session("StoreID")
            If Me.ckbTransDate.Checked Then
                e.InputParameters("transDateFrom") = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", Nothing)
                e.InputParameters("transDateTo") = DateTime.ParseExact("01/01/2079", "dd/MM/yyyy", Nothing)
            Else
                e.InputParameters("transDateFrom") = DateTime.ParseExact(Me.txtDateFrom.Text, "dd/MM/yyyy", Nothing)
                e.InputParameters("transDateTo") = DateTime.ParseExact(Me.txtDateTo.Text, "dd/MM/yyyy", Nothing)
            End If

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    Protected Sub ObjectDataSource2_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles ObjectDataSource2.Selected
        Try
            Dim returnList As List(Of MR006StockReturnCheckListIssueDetails) = e.ReturnValue
            If returnList.Count <= 0 Then
                NoRecordFond = "Y"
            Else
                NoRecordFond = "N"
            End If

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    Protected Sub ObjectDataSource2_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs) Handles ObjectDataSource2.Selecting
        Try
            e.InputParameters("storeId") = Session("StoreID")
            If Me.ckbTransDate.Checked Then
                e.InputParameters("transDateFrom") = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", Nothing)
                e.InputParameters("transDateTo") = DateTime.ParseExact("01/01/2079", "dd/MM/yyyy", Nothing)
            Else
                e.InputParameters("transDateFrom") = DateTime.ParseExact(Me.txtDateFrom.Text, "dd/MM/yyyy", Nothing)
                e.InputParameters("transDateTo") = DateTime.ParseExact(Me.txtDateTo.Text, "dd/MM/yyyy", Nothing)
            End If

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    Protected Sub ObjectDataSource3_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles ObjectDataSource3.Selected
        Try
            Dim returnList As List(Of MR006StockReturnCheckListAdjustDetails) = e.ReturnValue
            If returnList.Count <= 0 Then
                NoRecordFond = "Y"
            Else
                NoRecordFond = "N"
            End If

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    Protected Sub ObjectDataSource3_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs) Handles ObjectDataSource3.Selecting
        Try
            e.InputParameters("storeId") = Session("StoreID")
            If Me.ckbTransDate.Checked Then
                e.InputParameters("transDateFrom") = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", Nothing)
                e.InputParameters("transDateTo") = DateTime.ParseExact("01/01/2079", "dd/MM/yyyy", Nothing)
            Else
                e.InputParameters("transDateFrom") = DateTime.ParseExact(Me.txtDateFrom.Text, "dd/MM/yyyy", Nothing)
                e.InputParameters("transDateTo") = DateTime.ParseExact(Me.txtDateTo.Text, "dd/MM/yyyy", Nothing)
            End If

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    Protected Sub ObjectDataSource4_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles ObjectDataSource4.Selected
        Try
            Dim returnList As List(Of MR006StockReturnCheckListStockItemDetails) = e.ReturnValue
            If returnList.Count <= 0 Then
                NoRecordFond = "Y"
            Else
                NoRecordFond = "N"
            End If

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    Protected Sub ObjectDataSource4_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs) Handles ObjectDataSource4.Selecting
        Try
            e.InputParameters("storeId") = Session("StoreID")
            If Me.ckbTransDate.Checked Then
                e.InputParameters("transDateFrom") = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", Nothing)
                e.InputParameters("transDateTo") = DateTime.ParseExact("01/01/2079", "dd/MM/yyyy", Nothing)
            Else
                e.InputParameters("transDateFrom") = DateTime.ParseExact(Me.txtDateFrom.Text, "dd/MM/yyyy", Nothing)
                e.InputParameters("transDateTo") = DateTime.ParseExact(Me.txtDateTo.Text, "dd/MM/yyyy", Nothing)
            End If

            e.InputParameters("excludeZero") = chkExcludeZero.Checked

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub


End Class