﻿Imports Microsoft.Reporting.WebForms
Imports System.Web.Services.Description
Imports NEA_ICS.UserInterface.NEA_ICS.WcfService
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ServiceModel
Imports System.Reflection
'Imports DBauer.Web.UI.WebControls
Imports System.Web.Services

Partial Public Class frmUserActivities
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

            'CheckValidSession()

            If Not Page.IsPostBack Then
                AccessRights = clsCommonFunction.AssignAccessRights(Cache(Session(ESession.UserID.ToString) & Session(ESession.StoreID.ToString) & "AccessRights"), _
                                        clsCommonFunction.moduleID.TransactionListing)

                If AccessRights(0).SelectRight = False Then
                    Server.Transfer("..\frmUnauthorisedPage.aspx")
                    Exit Sub
                End If
         
                'txtDateFrom.Text = (New Date(Now.Year, Now.Month - 1, 1)).ToString("dd/MM/yyyy")
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
        Me.ddlSortBy.Items.Clear()
        ddlSortBy.Items.Add(New ListItem("Name", "VUserProfileName"))
        ddlSortBy.Items.Add(New ListItem("Date/Time", "AUserLoginDte"))
        Me.ddlSortBy.Items.Insert(0, New ListItem(" - Please Select - ", ""))

        Me.ddlFilterBy.Items.Clear()
        ddlFilterBy.Items.Add(New ListItem("All", ""))
        ddlFilterBy.Items.Add(New ListItem("Weekdays", "weekdays"))
        ddlFilterBy.Items.Add(New ListItem("Weekends", "weekends"))
        Me.ddlFilterBy.Items.Insert(0, New ListItem(" - Please Select - ", ""))
    End Sub

    Protected Sub btnPDF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPDF.Click
        Try
            If txtDateFrom.Text.Trim = String.Empty Or txtDateTo.Text.Trim = String.Empty Then
                Message = GetMessage(messageID.MandatoryField)
                Exit Sub
            End If

            If ConvertToDate(txtDateFrom.Text) > ConvertToDate(txtDateTo.Text) Then
                Message = GetMessage(messageID.DateToEarlierDateFrom)
                Exit Sub
            End If

            Dim byTimeStamp As Boolean = False
            If Me.checkRetrieveByTimeStamp.Checked Then
                byTimeStamp = True
            End If

            rvr.LocalReport.DataSources.Clear()
            rvr.LocalReport.DataSources.Add(New ReportDataSource("RoleDetails", ObjectDataSource1))

            Dim parameterlist As New List(Of ReportParameter)
            Dim p1 As New ReportParameter("StoreId", Session("StoreID").ToString())
            Dim p2 As New ReportParameter("UserId", txtUserName.Text.Trim())
            Dim p3 As New ReportParameter("DateFrom", Me.txtDateFrom.Text)
            Dim p4 As New ReportParameter("DateTo", Me.txtDateTo.Text)

            Dim p5 As New ReportParameter("ByTimeStamp", byTimeStamp)
            Dim p6 As New ReportParameter("FilterBy", ddlFilterBy.SelectedValue)
            Dim p7 As New ReportParameter("Store", Session("StoreName").ToString)
            parameterlist.Add(p1)
            parameterlist.Add(p2)
            parameterlist.Add(p3)
            parameterlist.Add(p4)
            parameterlist.Add(p5)
            parameterlist.Add(p6)
            parameterlist.Add(p7)

            rvr.LocalReport.SetParameters(parameterlist)
            rvr.LocalReport.Refresh()

            Dim bytValue As Byte()
            bytValue = rvr.LocalReport.Render("PDF", Nothing, "application/pdf", "", "pdf", Nothing, Nothing)

            Response.Buffer = True
            Response.Clear()
            Response.ContentType = "application/pdf"
            Response.AddHeader("content-disposition", "attachment;filename=UserAccessList.pdf")
            Response.BinaryWrite(bytValue)
            Response.Flush()
            Response.End()

        Catch ex As Exception
            Response.Write(ConvertToDate(txtDateTo.Text).ToString("MM/dd/yyyy") & "<br />")
            Response.Write(Today.ToString("dd/MM/yyyy") & "<br />")
            Response.Write(ex.InnerException)
            Response.End()
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExcel.Click
        Try
            If txtDateFrom.Text.Trim = String.Empty Or txtDateTo.Text.Trim = String.Empty Then
                Message = GetMessage(messageID.MandatoryField)
                Exit Sub
            End If

            If ConvertToDate(txtDateFrom.Text) > ConvertToDate(txtDateTo.Text) Then
                Message = GetMessage(messageID.DateToEarlierDateFrom)
                Exit Sub
            End If

            Dim byTimeStamp As Boolean = False
            If Me.checkRetrieveByTimeStamp.Checked Then
                byTimeStamp = True
            End If

            rvr.LocalReport.DataSources.Clear()
            rvr.LocalReport.DataSources.Add(New ReportDataSource("RoleDetails", ObjectDataSource1))

            Dim parameterlist As New List(Of ReportParameter)
            Dim p1 As New ReportParameter("StoreId", Session("StoreID").ToString())
            Dim p2 As New ReportParameter("UserId", txtUserName.Text.Trim())
            Dim p3 As New ReportParameter("DateFrom", Me.txtDateFrom.Text)
            Dim p4 As New ReportParameter("DateTo", Me.txtDateTo.Text)
            Dim p5 As New ReportParameter("ByTimeStamp", byTimeStamp)
            Dim p6 As New ReportParameter("FilterBy", ddlFilterBy.SelectedValue)
            Dim p7 As New ReportParameter("Store", Session("StoreName").ToString)
            parameterlist.Add(p1)
            parameterlist.Add(p2)
            parameterlist.Add(p3)
            parameterlist.Add(p4)
            parameterlist.Add(p5)
            parameterlist.Add(p6)
            parameterlist.Add(p7)

            rvr.LocalReport.SetParameters(parameterlist)
            rvr.LocalReport.Refresh()
            
            Dim bytValue As Byte()
            bytValue = rvr.LocalReport.Render("Excel", Nothing, "application/ms-excel", "", "xls", Nothing, Nothing)

            Response.Buffer = True
            Response.Clear()
            Response.ContentType = "application /ms-excel"
            Response.AddHeader("content-disposition", "attachment;filename=UserAccessList.xls")
            Response.BinaryWrite(bytValue)
            Response.Flush()
            Response.End()

        Catch ex As Exception
            Response.Write(ConvertToDate(txtDateTo.Text).ToString("MM/dd/yyyy") & "<br />")
            Response.Write(Today.ToString("dd/MM/yyyy") & "<br />")
            Response.Write(ex.InnerException)
            Response.End()
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    Protected Sub ObjectDataSource1_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles ObjectDataSource1.Selected
        Try
            Dim returnList As List(Of RoleDetails) = e.ReturnValue
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
        Dim sortBy As String
        Select Case ddlSortBy.SelectedIndex
            Case 0
                sortBy = ""
            Case 1
                sortBy = "VUserProfileName"
            Case 2
                sortBy = "AUserLoginDte"
            Case Else
                sortBy = ""
        End Select

        Try
            e.InputParameters("storeId") = Session(ESession.StoreID.ToString).ToString
            e.InputParameters("userId") = txtUserName.Text.Trim()
            e.InputParameters("fromDte") = DateTime.ParseExact(Me.txtDateFrom.Text, "dd/MM/yyyy", Nothing)
            e.InputParameters("toDte") = DateTime.ParseExact(Me.txtDateTo.Text, "dd/MM/yyyy", Nothing)
            Dim byTimeStamp As Boolean = False
            If Me.checkRetrieveByTimeStamp.Checked Then
                byTimeStamp = True
            End If
            e.InputParameters("byTimeStamp") = byTimeStamp
            e.InputParameters("filterBy") = ddlFilterBy.SelectedValue.ToString()
            e.InputParameters("sortBy") = sortBy

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    
End Class