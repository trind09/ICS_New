Imports Microsoft.Reporting.WebForms
Imports System.Web.Services.Description
Imports NEA_ICS.UserInterface.NEA_ICS.WcfService
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ServiceModel
Imports System.Reflection

Partial Public Class frmAResidue
    Inherits clsCommonFunction

    Private Message As String = EMPTY
    Public Property NoRecordFound() As String
        Get
            If ViewState("NoRecordFound") Is Nothing Then
                Return "N"
            Else
                Return ViewState("NoRecordFound").ToString()
            End If
        End Get
        Set(ByVal value As String)
            ViewState("NoRecordFound") = value
        End Set
    End Property

#Region " PAGE LOAD / PRE RENDER "

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ' alert user with message (if any)
        If Message <> EMPTY Then
            ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", "alert('" & Message & "');", True)
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim AccessRights As New List(Of RoleDetails)
        CheckValidSession()

        If Not Page.IsPostBack Then

            AccessRights = clsCommonFunction.AssignAccessRights(Cache(Session(ESession.UserID.ToString) & Session(ESession.StoreID.ToString) & "AccessRights"), _
                                clsCommonFunction.moduleID.ResidueCostAdjustment)

            If AccessRights(0).SelectRight = False Then
                Server.Transfer("..\frmUnauthorisedPage.aspx")
                Exit Sub
            End If

            BindYear()

        End If

    End Sub
#End Region

#Region " Sub Procedures and Functions "
    ''' <summary>
    ''' Sub Proc - BindYear();
    ''' 06 March 09 - Jianfa;
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub BindYear()

        ddlMonth.SelectedValue = Month(Today)

        For idx As Integer = 2010 To Year(Today)

            ddlYear.Items.Add(New ListItem(idx, idx))

        Next

        ddlYear.SelectedValue = Year(Today)

    End Sub
#End Region

#Region " PDF Button Clicked "
    ''' <summary>
    ''' Jianfa
    ''' 18 Oct 2010
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnPDF_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPDF.Click

        rvr.LocalReport.DataSources.Clear()
        rvr.LocalReport.DataSources.Add(New ReportDataSource("AResidueDetails", ObjectDataSource1))

        Dim parameterlist As New List(Of ReportParameter)
        Dim p1 As New ReportParameter("Report_Name", Session("StoreName").ToString())
        Dim p2 As New ReportParameter("Month", ddlMonth.SelectedItem.Text)
        Dim p3 As New ReportParameter("Year", ddlYear.SelectedItem.Text)
        parameterlist.Add(p1)
        parameterlist.Add(p2)
        parameterlist.Add(p3)

        rvr.LocalReport.SetParameters(parameterlist)
        rvr.LocalReport.Refresh()

        Dim bytValue As Byte()
        bytValue = rvr.LocalReport.Render("PDF", Nothing, "application/pdf", "", "pdf", Nothing, Nothing)

        If NoRecordFound = "Y" Then
            rvrBlank.LocalReport.DataSources.Clear()

            Dim parameterlistBlank As New List(Of ReportParameter)
            Dim bp1 As New ReportParameter("Report_Name", Session("StoreName").ToString())
            Dim bp2 As New ReportParameter("DateFrom", "")
            Dim bp3 As New ReportParameter("DateTo", "")
            Dim bp4 As New ReportParameter("AuditType", "")
            Dim bp5 As New ReportParameter("AuditTrail", "Residue Cost Adjustment")
            Dim bp6 As New ReportParameter("TransType", "")
            Dim bp7 As New ReportParameter("OrderBy", "")
            Dim bp8 As New ReportParameter("Month", ddlMonth.SelectedItem.Text)
            Dim bp9 As New ReportParameter("Year", ddlYear.SelectedItem.Text)
            Dim bp10 As New ReportParameter("FilterByStatus", "")

            parameterlistBlank.Add(bp1)
            parameterlistBlank.Add(bp2)
            parameterlistBlank.Add(bp3)
            parameterlistBlank.Add(bp4)
            parameterlistBlank.Add(bp5)
            parameterlistBlank.Add(bp6)
            parameterlistBlank.Add(bp7)
            parameterlistBlank.Add(bp8)
            parameterlistBlank.Add(bp9)
            parameterlistBlank.Add(bp10)

            rvrBlank.LocalReport.SetParameters(parameterlistBlank)
            rvrBlank.LocalReport.Refresh()

            Dim bytValueBlank As Byte()
            bytValueBlank = rvrBlank.LocalReport.Render("PDF", Nothing, "application/pdf", "", "pdf", Nothing, Nothing)

            Response.Buffer = True
            Response.Clear()
            Response.ContentType = "application/pdf"
            Response.AddHeader("content-disposition", "attachment;filename=AResidueCostAdj.pdf")
            Response.BinaryWrite(bytValueBlank)
            Response.Flush()
            Response.End()

            Exit Sub
        End If

        Response.Buffer = True
        Response.Clear()
        Response.ContentType = "application/pdf"
        Response.AddHeader("content-disposition", "attachment;filename=AResidueCostAdj.pdf")
        Response.BinaryWrite(bytValue)
        Response.Flush()
        Response.End()


    End Sub
#End Region

#Region " Excel Button Clicked "

    ''' <summary>
    ''' Jianfa 18 Oct 2010
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcel.Click

        rvr.LocalReport.DataSources.Clear()
        rvr.LocalReport.DataSources.Add(New ReportDataSource("AResidueDetails", ObjectDataSource1))

        Dim parameterlist As New List(Of ReportParameter)
        Dim p1 As New ReportParameter("Report_Name", Session("StoreName").ToString())
        Dim p2 As New ReportParameter("Month", ddlMonth.SelectedItem.Text)
        Dim p3 As New ReportParameter("Year", ddlYear.SelectedItem.Text)
        parameterlist.Add(p1)
        parameterlist.Add(p2)
        parameterlist.Add(p3)

        rvr.LocalReport.SetParameters(parameterlist)
        rvr.LocalReport.Refresh()

        Dim bytValue As Byte()
        bytValue = rvr.LocalReport.Render("Excel", Nothing, "application/ms-excel", "", "xls", Nothing, Nothing)

        If NoRecordFound = "Y" Then
            rvrBlank.LocalReport.DataSources.Clear()

            Dim parameterlistBlank As New List(Of ReportParameter)
            Dim bp1 As New ReportParameter("Report_Name", Session("StoreName").ToString())
            Dim bp2 As New ReportParameter("DateFrom", "")
            Dim bp3 As New ReportParameter("DateTo", "")
            Dim bp4 As New ReportParameter("AuditType", "")
            Dim bp5 As New ReportParameter("AuditTrail", "Residue Cost Adjustment")
            Dim bp6 As New ReportParameter("TransType", "")
            Dim bp7 As New ReportParameter("OrderBy", "")
            Dim bp8 As New ReportParameter("Month", ddlMonth.SelectedItem.Text)
            Dim bp9 As New ReportParameter("Year", ddlYear.SelectedItem.Text)
            Dim bp10 As New ReportParameter("FilterByStatus", "")

            parameterlistBlank.Add(bp1)
            parameterlistBlank.Add(bp2)
            parameterlistBlank.Add(bp3)
            parameterlistBlank.Add(bp4)
            parameterlistBlank.Add(bp5)
            parameterlistBlank.Add(bp6)
            parameterlistBlank.Add(bp7)
            parameterlistBlank.Add(bp8)
            parameterlistBlank.Add(bp9)
            parameterlistBlank.Add(bp10)

            rvrBlank.LocalReport.SetParameters(parameterlistBlank)
            rvrBlank.LocalReport.Refresh()

            Dim bytValueBlank As Byte()
            bytValueBlank = rvrBlank.LocalReport.Render("Excel", Nothing, "application/ms-excel", "", "xls", Nothing, Nothing)

            Response.Buffer = True
            Response.Clear()
            Response.ContentType = "application/ms-excel"
            Response.AddHeader("content-disposition", "attachment;filename=AResidueCostAdj.xls")
            Response.BinaryWrite(bytValueBlank)
            Response.Flush()
            Response.End()

            Exit Sub
        End If

        Response.Buffer = True
        Response.Clear()
        Response.ContentType = "application/ms-excel"
        Response.AddHeader("content-disposition", "attachment;filename=AResidueCostAdj.xls")
        Response.BinaryWrite(bytValue)
        Response.Flush()
        Response.End()

    End Sub
#End Region

#Region " Object Data Source "

    ''' <summary>
    ''' Object Data Source Selected
    ''' 18 Oct 2010 - Jianfa
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ObjectDataSource1_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles ObjectDataSource1.Selected

        Dim returnList As List(Of AResidueDetails) = e.ReturnValue

        If returnList.Count <= 0 Then
            NoRecordFound = "Y"
        Else
            NoRecordFound = "N"
        End If

    End Sub

    ''' <summary>
    ''' Object Data Source Selecting
    ''' 18 Oct 2010 - Jianfa
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ObjectDataSource1_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs) Handles ObjectDataSource1.Selecting

        e.InputParameters("storeId") = Session("StoreID")
        e.InputParameters("month") = ddlMonth.SelectedValue
        e.InputParameters("year") = ddlYear.SelectedValue

    End Sub

#End Region


End Class