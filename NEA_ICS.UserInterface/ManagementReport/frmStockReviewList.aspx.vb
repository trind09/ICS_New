Imports Microsoft.Reporting.WebForms
Imports System.Web.Services.Description
Imports NEA_ICS.UserInterface.NEA_ICS.WcfService
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ServiceModel
Imports System.Reflection

Partial Public Class frmStockReviewList
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
                                        clsCommonFunction.moduleID.StockReviewList)

                If AccessRights(0).SelectRight = False Then
                    Server.Transfer("..\frmUnauthorisedPage.aspx")
                    Exit Sub
                End If
            End If

            If Me.ckbAllStockCode.Checked Then
                ddlStockCodeFrom.SelectedIndex = -1
                ddlStockCodeTo.SelectedIndex = -1
                ddlStockCodeFrom.Enabled = False
                ddlStockCodeTo.Enabled = False

            Else
                ddlStockCodeFrom.Enabled = True
                ddlStockCodeTo.Enabled = True

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
                Me.BindDropdownLists()
                txtDateFrom.Text = (New Date(Now.AddMonths(-1).Year, Now.AddMonths(-1).Month, 1)).ToString("dd/MM/yyyy")
                txtDateTo.Text = (New Date(Now.Year, Now.Month, 1).AddDays(-1)).ToString("dd/MM/yyyy")

            End If

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    Private Sub BindDropdownLists()
        Dim Client As New ServiceClient

        Dim searchItem As New ItemDetails
        searchItem.StoreID = Session("StoreID")
        searchItem.ItemID = ""
        searchItem.Status = ""
        searchItem.Location = ""
        Dim itemList As New List(Of ItemDetails)
        itemList = Client.GetItems(searchItem, "StockItemID", "")
        Client.Close()

        Me.ddlStockCodeFrom.Items.Clear()
        Me.ddlStockCodeTo.Items.Clear()

        If itemList.Count > 0 Then

            For Each item In itemList
                ddlStockCodeFrom.Items.Add(New ListItem(item.ItemID_Description, item.ItemID))
                ddlStockCodeTo.Items.Add(New ListItem(item.ItemID_Description, item.ItemID))
            Next
        End If

        Me.ddlStockCodeFrom.Items.Insert(0, New ListItem(" - Please Select - ", ""))
        Me.ddlStockCodeTo.Items.Insert(0, New ListItem(" - Please Select - ", ""))
    End Sub

    Protected Sub btnPDF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPDF.Click
        Try
            If txtDateFrom.Text.Trim = String.Empty Or txtDateTo.Text.Trim = String.Empty Then
                If Not ckbTransDate.Checked Then
                    Message = GetMessage(messageID.MandatoryField)
                    Exit Sub
                End If
            End If

            If ConvertToDate(txtDateFrom.Text) > ConvertToDate(txtDateTo.Text) Then
                Message = GetMessage(messageID.DateToEarlierDateFrom)
                Exit Sub
            End If

            ' UAT02
            If ddlStockCodeFrom.SelectedValue = EMPTY Or ddlStockCodeTo.SelectedValue = EMPTY Then
                If Not ckbAllStockCode.Checked Then
                    Message = GetMessage(messageID.MandatoryField)
                    Exit Sub
                End If
            End If

            If ddlStockCodeFrom.SelectedIndex > ddlStockCodeTo.SelectedIndex Then
                Message = GetMessage(messageID.StockCodeToEarlierStockCodeFrom)
                Exit Sub
            End If

            rvr.LocalReport.DataSources.Clear()
            rvr.LocalReport.DataSources.Add(New ReportDataSource("MR004StockReviewListDetails", ObjectDataSource1))

            Dim paraDateFrom As String
            Dim paraDateTo As String
            If Me.ckbTransDate.Checked Then
                paraDateFrom = "All"
                paraDateTo = "All"
            Else
                paraDateFrom = Me.txtDateFrom.Text
                paraDateTo = Me.txtDateTo.Text
            End If
            Dim paraStockFrom As String
            Dim paraStockTo As String
            If Me.ckbAllStockCode.Checked Then
                paraStockFrom = "All"
                paraStockTo = "All"
            Else
                paraStockFrom = ddlStockCodeFrom.SelectedValue
                paraStockTo = ddlStockCodeTo.SelectedValue
            End If

            Dim parameterlist As New List(Of ReportParameter)
            Dim p1 As New ReportParameter("Report_Name", Session("StoreName").ToString())
            Dim p2 As New ReportParameter("DateFrom", paraDateFrom)
            Dim p3 As New ReportParameter("DateTo", paraDateTo)
            Dim p4 As New ReportParameter("StockCodeFrom", paraStockFrom)
            Dim p5 As New ReportParameter("StockCodeTo", paraStockTo)
            parameterlist.Add(p1)
            parameterlist.Add(p2)
            parameterlist.Add(p3)
            parameterlist.Add(p4)
            parameterlist.Add(p5)

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
            Response.AddHeader("content-disposition", "attachment;filename=StockReviewList.pdf")
            Response.BinaryWrite(bytValue)
            Response.Flush()
            Response.End()

        Catch ex As Exception
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

            If ConvertToDate(txtDateFrom.Text) > ConvertToDate(txtDateTo.Text) Then
                Message = GetMessage(messageID.DateToEarlierDateFrom)
                Exit Sub
            End If

            ' UAT02
            If ddlStockCodeFrom.SelectedValue = EMPTY Or ddlStockCodeTo.SelectedValue = EMPTY Then
                If Not ckbAllStockCode.Checked Then
                    Message = GetMessage(messageID.MandatoryField)
                    Exit Sub
                End If
            End If

            If ddlStockCodeFrom.SelectedIndex > ddlStockCodeTo.SelectedIndex Then
                Message = GetMessage(messageID.StockCodeToEarlierStockCodeFrom)
                Exit Sub
            End If

            rvr.LocalReport.DataSources.Clear()
            rvr.LocalReport.DataSources.Add(New ReportDataSource("MR004StockReviewListDetails", ObjectDataSource1))

            Dim paraDateFrom As String
            Dim paraDateTo As String
            If Me.ckbTransDate.Checked Then
                paraDateFrom = "All"
                paraDateTo = "All"
            Else
                paraDateFrom = Me.txtDateFrom.Text
                paraDateTo = Me.txtDateTo.Text
            End If
            Dim paraStockFrom As String
            Dim paraStockTo As String
            If Me.ckbAllStockCode.Checked Then
                paraStockFrom = "All"
                paraStockTo = "All"
            Else
                paraStockFrom = ddlStockCodeFrom.SelectedValue
                paraStockTo = ddlStockCodeTo.SelectedValue
            End If

            Dim parameterlist As New List(Of ReportParameter)
            Dim p1 As New ReportParameter("Report_Name", Session("StoreName").ToString())
            Dim p2 As New ReportParameter("DateFrom", paraDateFrom)
            Dim p3 As New ReportParameter("DateTo", paraDateTo)
            Dim p4 As New ReportParameter("StockCodeFrom", paraStockFrom)
            Dim p5 As New ReportParameter("StockCodeTo", paraStockTo)
            parameterlist.Add(p1)
            parameterlist.Add(p2)
            parameterlist.Add(p3)
            parameterlist.Add(p4)
            parameterlist.Add(p5)

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
            Response.AddHeader("content-disposition", "attachment;filename=StockReviewList.xls")
            Response.BinaryWrite(bytValue)
            Response.Flush()
            Response.End()

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    Protected Sub ObjectDataSource1_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles ObjectDataSource1.Selected
        Try
            Dim returnList As List(Of MR004StockReviewListDetails) = e.ReturnValue
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
            If Me.ckbAllStockCode.Checked And ddlStockCodeFrom.Items.Count > 1 Then
                ddlStockCodeFrom.SelectedIndex = 1
                ddlStockCodeTo.SelectedIndex = ddlStockCodeTo.Items.Count - 1
            End If
            e.InputParameters("storeId") = Session("StoreID")
            e.InputParameters("stockCodeFrom") = ddlStockCodeFrom.SelectedValue.ToString()
            e.InputParameters("stockCodeTo") = ddlStockCodeTo.SelectedValue.ToString()
            If Me.ckbTransDate.Checked Then
                e.InputParameters("transDateFrom") = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", Nothing)
                e.InputParameters("transDateTo") = DateTime.ParseExact("01/01/2079", "dd/MM/yyyy", Nothing)
            Else
                e.InputParameters("transDateFrom") = DateTime.ParseExact(Me.txtDateFrom.Text, "dd/MM/yyyy", Nothing)
                e.InputParameters("transDateTo") = DateTime.ParseExact(Me.txtDateTo.Text, "dd/MM/yyyy", Nothing)
            End If
            If Me.ckbAllStockCode.Checked Then
                ddlStockCodeFrom.SelectedIndex = -1
                ddlStockCodeTo.SelectedIndex = -1
            End If

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub
End Class