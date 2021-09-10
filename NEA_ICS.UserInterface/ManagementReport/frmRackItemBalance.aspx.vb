Imports Microsoft.Reporting.WebForms
Imports System.Web.Services.Description
Imports NEA_ICS.UserInterface.NEA_ICS.WcfService
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ServiceModel
Imports System.Reflection

Partial Public Class frmRackItemBalance
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

            If Me.ckbAllLocation.Checked Then

                ddlLocationFrom.SelectedIndex = -1
                ddlLocationTo.SelectedIndex = -1
                ddlLocationFrom.Enabled = False
                ddlLocationTo.Enabled = False

            Else
                ddlLocationFrom.Enabled = True
                ddlLocationTo.Enabled = True

            End If

            If Not Page.IsPostBack Then

                AccessRights = clsCommonFunction.AssignAccessRights(Cache(Session(ESession.UserID.ToString) & Session(ESession.StoreID.ToString) & "AccessRights"), _
                                        clsCommonFunction.moduleID.LocationStockBalanceListing)

                If AccessRights(0).SelectRight = False Then
                    Server.Transfer("..\frmUnauthorisedPage.aspx")
                    Exit Sub
                End If

                Me.BindDropdownLists()
            End If

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try

    End Sub

    Private Sub BindDropdownLists()
        Dim Client As New ServiceClient
        Dim RackLocationList As New List(Of String)
        RackLocationList = Client.GetRackLocation(Session("StoreID"))
        Client.Close()

        Me.ddlLocationFrom.Items.Clear()
        Me.ddlLocationTo.Items.Clear()

        If RackLocationList.Count > 0 Then

            For Each item In RackLocationList
                If item.Trim() <> "" Then
                    ddlLocationFrom.Items.Add(New ListItem(item, item))
                    ddlLocationTo.Items.Add(New ListItem(item, item))
                End If
            Next
        End If

        Me.ddlLocationFrom.Items.Insert(0, New ListItem(" - Please Select - ", ""))
        Me.ddlLocationTo.Items.Insert(0, New ListItem(" - Please Select - ", ""))
    End Sub

    Protected Sub btnPDF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPDF.Click
        Try
            ' UAT02 - skip check when All is checked
            If ckbAllLocation.Checked = False Then
                If ddlLocationFrom.SelectedIndex = 0 Or ddlLocationTo.SelectedIndex = 0 Then
                    Message = GetMessage(messageID.MandatoryField)
                    Exit Sub
                End If
            End If

            rvr.LocalReport.DataSources.Clear()
            rvr.LocalReport.DataSources.Add(New ReportDataSource("MR001GetRackItemBalanceDetails", ObjectDataSource1))

            Dim paraLocationFrom As String
            Dim paraLocationTo As String
            If Me.ckbAllLocation.Checked Then
                paraLocationFrom = "All"
                paraLocationTo = "All"
            Else
                paraLocationFrom = ddlLocationFrom.SelectedValue
                paraLocationTo = ddlLocationTo.SelectedValue
            End If

            Dim parameterlist As New List(Of ReportParameter)
            Dim p1 As New ReportParameter("Report_Name", Session("StoreName").ToString())
            Dim p2 As New ReportParameter("RackLocationFrom", paraLocationFrom)
            Dim p3 As New ReportParameter("RackLocationTo", paraLocationTo)
            parameterlist.Add(p1)
            parameterlist.Add(p2)
            parameterlist.Add(p3)

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
            Response.AddHeader("content-disposition", "attachment;filename=RackItemBalance.pdf")
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
            ' UAT02 - skip check when All is checked
            If ckbAllLocation.Checked = False Then
                If ddlLocationFrom.SelectedIndex = 0 Or ddlLocationTo.SelectedIndex = 0 Then
                    Message = GetMessage(messageID.MandatoryField)
                    Exit Sub
                End If
            End If

            rvr.LocalReport.DataSources.Clear()
            rvr.LocalReport.DataSources.Add(New ReportDataSource("MR001GetRackItemBalanceDetails", ObjectDataSource1))

            Dim paraLocationFrom As String
            Dim paraLocationTo As String
            If Me.ckbAllLocation.Checked Then
                paraLocationFrom = "All"
                paraLocationTo = "All"
            Else
                paraLocationFrom = ddlLocationFrom.SelectedValue
                paraLocationTo = ddlLocationTo.SelectedValue
            End If

            Dim parameterlist As New List(Of ReportParameter)
            Dim p1 As New ReportParameter("Report_Name", Session("StoreName").ToString())
            Dim p2 As New ReportParameter("RackLocationFrom", paraLocationFrom)
            Dim p3 As New ReportParameter("RackLocationTo", paraLocationTo)
            parameterlist.Add(p1)
            parameterlist.Add(p2)
            parameterlist.Add(p3)

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
            Response.AddHeader("content-disposition", "attachment;filename=RackItemBalance.xls")
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
            Dim returnList As List(Of MR001GetRackItemBalanceDetails) = e.ReturnValue
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
            If Me.ckbAllLocation.Checked And ddlLocationFrom.Items.Count > 1 Then
                ddlLocationFrom.SelectedIndex = 1
                ddlLocationTo.SelectedIndex = ddlLocationTo.Items.Count - 1
            End If
            e.InputParameters("storeId") = Session("StoreID")
            e.InputParameters("rackLocationFrom") = ddlLocationFrom.SelectedValue.ToString()
            e.InputParameters("rackLocationTo") = ddlLocationTo.SelectedValue.ToString()
            If Me.ckbAllLocation.Checked Then
                ddlLocationFrom.SelectedIndex = -1
                ddlLocationTo.SelectedIndex = -1
            End If

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub
End Class