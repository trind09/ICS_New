Imports Microsoft.Reporting.WebForms
Imports System.Web.Services.Description
Imports NEA_ICS.UserInterface.NEA_ICS.WcfService
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ServiceModel
Imports System.Reflection
'Imports DBauer.Web.UI.WebControls
Imports System.Web.Services

Partial Public Class frmInactiveUsers
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
                'BindInactiveAccounts()
                AccessRights = clsCommonFunction.AssignAccessRights(Cache(Session(ESession.UserID.ToString) & Session(ESession.StoreID.ToString) & "AccessRights"), _
                                        clsCommonFunction.moduleID.TransactionListing)

                If AccessRights(0).SelectRight = False Then
                    Server.Transfer("..\frmUnauthorisedPage.aspx")
                    Exit Sub
                End If
                'txtDateFrom.Text = (New Date(Now.Year, Now.Month - 1, 1)).ToString("dd/MM/yyyy")
                txtDateFrom.Text = (New Date(Now.AddMonths(-1).Year, Now.AddMonths(-1).Month, 1)).ToString("dd/MM/yyyy")
                txtDateTo.Text = (New Date(Now.Year, Now.Month, Now.Day)).ToString("dd/MM/yyyy")
            End If

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    Private Sub BindInactiveAccounts()
        Dim Client As New ServiceClient
        Dim RoleDetails As New RoleDetails
        Dim List As New List(Of RoleDetails)

        'test start
        'Session("StoreID") = "RO"
        'end test

        List = Client.GetInactiveUsers(Session("StoreID"), txtDateFrom.Text, txtDateTo.Text)
        Client.Close()

        gdvLocateUser.DataSource = List
        gdvLocateUser.DataBind()
    End Sub

    ''' <summary>
    ''' gdvLocateUser - RowDataBound;
    ''' 13 Sept 13 - Christina;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gdvLocateUser_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvLocateUser.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            'Dim RoleDetails As New RoleDetails
            'RoleDetails.StoreID = Session("StoreID")

            ''Dim Client As New ServiceClient
            ''Cache(Session(ESession.StoreID.ToString) & "ModuleRoleList") = Client.GetModuleRoles(RoleDetails)
            ''Client.Close()

            'Dim View As DataView
            'View = clsCommonFunction.GetCommonDataByCodeGroup(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), clsCommonFunction.codeGroup.UserRole)
            'View.RowFilter = " CommonCodeID = '" & e.Row.Cells(4).Text & "' and CommonCodeGroup = 'User Role'"

            If e.Row.Cells(4).Text = "" Then
                e.Row.Cells(4).Text = "Status was updated by Admin"
            End If
        End If

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
            rvr.LocalReport.DataSources.Clear()
            rvr.LocalReport.DataSources.Add(New ReportDataSource("RoleDetails", ObjectDataSource1))

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
            Dim p1 As New ReportParameter("StoreId", Session("StoreID").ToString())
            Dim p2 As New ReportParameter("DateFrom", paraDateFrom)
            Dim p3 As New ReportParameter("DateTo", paraDateTo)
            Dim p5 As New ReportParameter("Store", Session("StoreName").ToString)

            parameterlist.Add(p1)
            parameterlist.Add(p2)
            parameterlist.Add(p3)
            parameterlist.Add(p5)

            rvr.LocalReport.SetParameters(parameterlist)
            rvr.LocalReport.Refresh()

            Dim bytValue As Byte()
            bytValue = rvr.LocalReport.Render("PDF", Nothing, "application/pdf", "", "pdf", Nothing, Nothing)

            Response.Buffer = True
            Response.Clear()
            Response.ContentType = "application/pdf"
            Response.AddHeader("content-disposition", "attachment;filename=InactiveAccount.pdf")
            Response.BinaryWrite(bytValue)
            Response.Flush()
            Response.End()

        Catch ex As Exception
            'Throw New FaultException(ex.Message)
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExcel.Click
        Try
            ''test start
            'Session("StoreID") = "RO"
            'Session("StoreName") = "General Store"
            ''end test
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
            rvr.LocalReport.DataSources.Clear()
            rvr.LocalReport.DataSources.Add(New ReportDataSource("RoleDetails", ObjectDataSource1))

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
            Dim p1 As New ReportParameter("StoreId", Session("StoreID").ToString())
            Dim p2 As New ReportParameter("DateFrom", paraDateFrom)
            Dim p3 As New ReportParameter("DateTo", paraDateTo)

            Dim p5 As New ReportParameter("Store", Session("StoreName").ToString)
            parameterlist.Add(p1)
            parameterlist.Add(p2)
            parameterlist.Add(p3)
            parameterlist.Add(p5)

            rvr.LocalReport.SetParameters(parameterlist)
            rvr.LocalReport.Refresh()

            Dim bytValue As Byte()
            bytValue = rvr.LocalReport.Render("Excel", Nothing, "application/ms-excel", "", "xls", Nothing, Nothing)

            Response.Buffer = True
            Response.Clear()
            Response.ContentType = "application/ms-excel"
            Response.AddHeader("content-disposition", "attachment;filename=InactiveAccount.xls")
            Response.BinaryWrite(bytValue)
            Response.Flush()
            Response.End()

        Catch ex As Exception
            'Throw New FaultException(ex.Message)
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

     Protected Sub ObjectDataSource1_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs) Handles ObjectDataSource1.Selecting
        Try
            e.InputParameters("storeId") = Session("StoreID")
            If Me.ckbTransDate.Checked Then
                e.InputParameters("dateFrom") = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", Nothing)
                e.InputParameters("dateTo") = DateTime.ParseExact("01/01/2079", "dd/MM/yyyy", Nothing)
            Else
                e.InputParameters("dateFrom") = DateTime.ParseExact(Me.txtDateFrom.Text, "dd/MM/yyyy", Nothing)
                e.InputParameters("dateTo") = DateTime.ParseExact(Me.txtDateTo.Text, "dd/MM/yyyy", Nothing)
            End If

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub
    Protected Sub ObjectDataSource1_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles ObjectDataSource1.Selected
        Try
            Dim returnList As List(Of AConsumer) = e.ReturnValue
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



End Class