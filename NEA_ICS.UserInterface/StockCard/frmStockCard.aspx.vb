Imports Microsoft.Reporting.WebForms
Imports System.Web.Services.Description
Imports NEA_ICS.UserInterface.NEA_ICS.WcfService
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ServiceModel
Imports System.Reflection
Imports System.Web.Services

''' <summary>
''' Class for Stock Card
''' 15 Jan 09 - Jianfa;
''' </summary>
''' <remarks></remarks>
Partial Public Class frmStockCard
    Inherits clsCommonFunction

#Region " PAGE LOAD "
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
            lblErrStockCard.Visible = False
            ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", "alert('" & Message & "');", True)
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim AccessRights As New List(Of RoleDetails)

        CheckValidSession()

        If Not Page.IsPostBack Then

            aceStockCode.ContextKey = Session("StoreID").ToString

            AccessRights = clsCommonFunction.AssignAccessRights(Cache(Session(ESession.UserID.ToString) & Session(ESession.StoreID.ToString) & "AccessRights"), _
                                clsCommonFunction.moduleID.StockCard)

            If AccessRights(0).SelectRight = False Then
                Server.Transfer("..\frmUnauthorisedPage.aspx")
                Exit Sub
            End If

            BindStockCode()

            txtTransactionFrom.Text = "01/" & Month(Now).ToString.PadLeft(2, "0") & "/" & Year(Now)
            txtTransactionTo.Text = Day(Now).ToString.PadLeft(2, "0") & "/" & Month(Now).ToString.PadLeft(2, "0") & "/" & Year(Now)
            
        End If

    End Sub
#End Region

#Region " Stock Card UI "

    ''' <summary>
    ''' lbtnCheckStockCode - Click;
    ''' 02 May 09 - Jianfa
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub lbtnCheckStockCode_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnCheckStockCode.Click

        Try

            If txtStockCode.Text.Trim = EMPTY Then
                Exit Sub
            End If

            '-- VALIDATE STOCK CODE
            Dim StockCode As String
            StockCode = Split(txtStockCode.Text, " | ")(0).Trim

            Dim Client As New ServiceClient

            Using Client

                Dim ItemDetails As New ItemDetails
                ItemDetails.StoreID = Session("StoreID").ToString
                ItemDetails.ItemID = StockCode

                If Not Client.IsValidStockCode(ItemDetails) Then

                    Message = GetMessage(messageID.InvalidStockCode, StockCode.ToUpper)
                    Exit Sub

                End If

                Dim MoreItemInfoDetails As New MoreItemInfoDetails

                Dim item As ItemDetails = DirectCast(Cache(Session(ESession.StoreID.ToString) & "StockCard"), List(Of ItemDetails)).Find(Function(i As ItemDetails) i.ItemID = StockCode.ToUpper())

                'For Each item As ItemDetails In Cache(Session(ESession.StoreID.ToString) & "StockCard")

                If item IsNot Nothing Then
                    'If item.ItemID = StockCode Then

                    If item.EquipmentID = String.Empty Then
                        lblEquipmentCode.Text = String.Empty
                        hidEquipmentCode.Value = String.Empty
                    Else
                        lblEquipmentCode.Text = BindEquipmentNo(item.EquipmentID)
                        hidEquipmentCode.Value = item.EquipmentID
                    End If


                    '-- Part No
                    lblPartNo.Text = item.PartNo

                    '-- Stock Description
                    lblDescription.Text = item.ItemDescription

                    '-- UOM
                    '-- UAT.02.50 -- Display UOM for Stock Card
                    lblUOM.Text = GetCodeDescription(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), codeGroup.UOM, item.UOM)

                    '-- Location 1
                    lblLocation1.Text = item.Location

                    '-- Location 2
                    lblLocation2.Text = item.Location2

                    '-- Type
                    lblType.Text = GetCodeDescription(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), codeGroup.StockType, item.StockType)

                    '-- Sub Type
                    If item.SubType.Trim = String.Empty Then
                        lblSubType.Text = String.Empty
                    Else
                        lblSubType.Text = GetCodeDescription(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), codeGroup.SubType, item.SubType)
                    End If

                    '-- Max Level
                    lblMaxLevel.Text = item.MaxLevel.ToString("0.00")

                    '-- Min Level
                    lblMinLevel.Text = item.MinLevel.ToString("0.00")

                    '-- Re order Level
                    lblReorderLevel.Text = item.ReorderLevel.ToString("0.00")

                    MoreItemInfoDetails = Client.GetMoreItemInfo(Session("StoreID"), _
                                                                 item.ItemID, Today)
                    Client.Close()

                    lblCurrentBal.Text = MoreItemInfoDetails.Balance.ToString("0.00")
                    lblCurrentVal.Text = MoreItemInfoDetails.TotalValue.ToString("0.0000")
                    lblAvgUnitPrice.Text = MoreItemInfoDetails.AvgUnitCost.ToString("0.0000")

                    uplStockCardInfo.Update()

                    '-- [ TO AVOID REPETITIVE LOOPING ]
                    'Exit For

                    'End If
                Else
                    Dim stockDescription As String = txtStockCode.Text.Trim()
                    btnClear_Click(Nothing, Nothing)
                    txtStockCode.Text = stockDescription ' retain stock description
                End If

                'Next

                Client.Close()

            End Using

        Catch ex As FaultException

            lblErrStockCard.Text = ex.Message
            lblErrStockCard.Visible = True

        Catch ex As Exception

            If Not ex.Message.ToString.Contains("Update") Then
                lblErrStockCard.Text = ex.Message
                lblErrStockCard.Visible = True
            Else
                Exit Try
            End If

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If

        End Try

    End Sub

    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click

        '-- UAT.02.17 -- Clear away selection
        txtStockCode.Text = String.Empty
        lblAvgUnitPrice.Text = String.Empty
        lblCurrentBal.Text = String.Empty
        lblCurrentVal.Text = String.Empty
        lblDescription.Text = String.Empty
        lblEquipmentCode.Text = String.Empty
        lblLocation1.Text = String.Empty
        lblLocation2.Text = String.Empty
        lblMaxLevel.Text = String.Empty
        lblMinLevel.Text = String.Empty
        lblPartNo.Text = String.Empty
        lblReorderLevel.Text = String.Empty
        lblSubType.Text = String.Empty
        lblType.Text = String.Empty
        lblUOM.Text = String.Empty
        txtTransactionFrom.Text = "01/" & Month(Now).ToString.PadLeft(2, "0") & "/" & Year(Now)
        txtTransactionTo.Text = Day(Now).ToString.PadLeft(2, "0") & "/" & Month(Now).ToString.PadLeft(2, "0") & "/" & Year(Now)

        uplStockCard.Update()

    End Sub

#End Region

#Region " Stock Card Report "

    Protected Sub btnTransListingPDF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnTransListingPDF.Click

        '-- Validation Checks
        If Not ValidateStockCode() Then Exit Sub

        rvr.LocalReport.DataSources.Clear()
        rvr.LocalReport.DataSources.Add(New ReportDataSource("MR002GetTransactionListDetails", ObjectDataSource1))

        Dim parameterlist As New List(Of ReportParameter)
        Dim p1 As New ReportParameter("Report_Name", Session("StoreName").ToString())
        Dim p2 As New ReportParameter("DateFrom", txtTransactionFrom.Text)
        Dim p3 As New ReportParameter("DateTo", txtTransactionTo.Text)
        Dim p4 As New ReportParameter("StockCodeFrom", Split(txtStockCode.Text.Trim, " | ")(0).Trim)
        Dim p5 As New ReportParameter("StockCodeTo", Split(txtStockCode.Text.Trim, " | ")(0).Trim)
        Dim p6 As New ReportParameter("WithDirectIssue", "N")
        'Dim p7 As New ReportParameter("Store", Session("StoreName").ToString)
        parameterlist.Add(p1)
        parameterlist.Add(p2)
        parameterlist.Add(p3)
        parameterlist.Add(p4)
        parameterlist.Add(p5)
        parameterlist.Add(p6)
        'parameterlist.Add(p7)

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
        Response.AddHeader("content-disposition", "attachment;filename=TransactionListing.pdf")
        Response.BinaryWrite(bytValue)
        Response.Flush()
        Response.End()

    End Sub

    Protected Sub btnTransListingExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnTransListingExcel.Click

        '-- Validation Checks
        If Not ValidateStockCode() Then Exit Sub

        rvr.LocalReport.DataSources.Clear()
        rvr.LocalReport.DataSources.Add(New ReportDataSource("MR002GetTransactionListDetails", ObjectDataSource1))

        Dim parameterlist As New List(Of ReportParameter)
        Dim p1 As New ReportParameter("Report_Name", Session("StoreName").ToString())
        Dim p2 As New ReportParameter("DateFrom", txtTransactionFrom.Text)
        Dim p3 As New ReportParameter("DateTo", txtTransactionTo.Text)
        Dim p4 As New ReportParameter("StockCodeFrom", Split(txtStockCode.Text.Trim, " | ")(0).Trim)
        Dim p5 As New ReportParameter("StockCodeTo", Split(txtStockCode.Text.Trim, " | ")(0).Trim)
        Dim p6 As New ReportParameter("WithDirectIssue", "N")
        'Dim p7 As New ReportParameter("Store", Session("StoreName").ToString)
        parameterlist.Add(p1)
        parameterlist.Add(p2)
        parameterlist.Add(p3)
        parameterlist.Add(p4)
        parameterlist.Add(p5)
        parameterlist.Add(p6)
        'parameterlist.Add(p7)

        rvr.LocalReport.SetParameters(parameterlist)
        rvr.LocalReport.Refresh()

        Dim bytValue As Byte()
        bytValue = rvr.LocalReport.Render("Excel", Nothing, "application/ms-excel", "", "xls", Nothing, Nothing)

        'If NoRecordFond = "Y" Then
        '    Message = GetMessage(messageID.NoRecordFound)
        '    Exit Sub
        'End If

        Response.Buffer = True
        Response.Clear()
        Response.ContentType = "application/ms-excel"
        Response.AddHeader("content-disposition", "attachment;filename=TransactionListing.xls")
        Response.BinaryWrite(bytValue)
        Response.Flush()
        Response.End()

    End Sub
    Protected Sub ObjectDataSource1_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles ObjectDataSource1.Selected
        Dim returnList As List(Of MR002GetTransactionListDetails) = e.ReturnValue
        If returnList.Count <= 0 Then
            NoRecordFond = "Y"
        Else
            NoRecordFond = "N"
        End If
    End Sub

    Protected Sub ObjectDataSource1_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs) Handles ObjectDataSource1.Selecting
        e.InputParameters("storeId") = Session("StoreID")
        e.InputParameters("stockCodeFrom") = Split(txtStockCode.Text.Trim, " | ")(0).Trim
        e.InputParameters("stockCodeTo") = Split(txtStockCode.Text.Trim, " | ")(0).Trim
        e.InputParameters("transDateFrom") = DateTime.ParseExact(txtTransactionFrom.Text, "dd/MM/yyyy", Nothing)
        e.InputParameters("transDateTo") = DateTime.ParseExact(txtTransactionTo.Text, "dd/MM/yyyy", Nothing)
        e.InputParameters("directIssue") = "N"
        e.InputParameters("equipmentID") = hidEquipmentCode.Value 'ddlEquipmentCode.SelectedValue
    End Sub

    Protected Sub btnStockCardPDF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnStockCardPDF.Click

        '-- Validation Checks
        'If Not ValidateStockCode() Then Exit Sub

        rvrCard.LocalReport.DataSources.Clear()

        Dim parameterlist As New List(Of ReportParameter)
        Dim p1 As New ReportParameter("ReportTitle", "Stock Card")
        Dim p2 As New ReportParameter("StockCode", Split(txtStockCode.Text.Trim, " | ")(0).Trim)
        Dim p3 As New ReportParameter("Description", lblDescription.Text)
        Dim p4 As New ReportParameter("MinQty", lblMinLevel.Text)
        Dim p5 As New ReportParameter("MaxQty", lblMaxLevel.Text)
        Dim p6 As New ReportParameter("UOM", lblUOM.Text)
        Dim p7 As New ReportParameter("Location", lblLocation1.Text + " " + lblLocation2.Text)
        Dim p8 As New ReportParameter("Store", Session("StoreName").ToString)
        parameterlist.Add(p1)
        parameterlist.Add(p2)
        parameterlist.Add(p3)
        parameterlist.Add(p4)
        parameterlist.Add(p5)
        parameterlist.Add(p6)
        parameterlist.Add(p7)
        parameterlist.Add(p8)

        rvrCard.LocalReport.SetParameters(parameterlist)
        rvrCard.LocalReport.Refresh()

        Dim bytValue As Byte()
        bytValue = rvrCard.LocalReport.Render("PDF", Nothing, "application/pdf", "", "pdf", Nothing, Nothing)

        Response.Buffer = True
        Response.Clear()
        Response.ContentType = "application/pdf"
        Response.AddHeader("content-disposition", "attachment;filename=StockCard.pdf")
        Response.BinaryWrite(bytValue)
        Response.Flush()
        Response.End()
    End Sub

    Protected Sub btnStockCardExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnStockCardExcel.Click
        rvrCard.LocalReport.DataSources.Clear()

        '-- Validation Checks
        'If Not ValidateStockCode() Then Exit Sub

        Dim parameterlist As New List(Of ReportParameter)
        Dim p1 As New ReportParameter("ReportTitle", "Stock Card")
        Dim p2 As New ReportParameter("StockCode", Split(txtStockCode.Text.Trim, " | ")(0).Trim)
        Dim p3 As New ReportParameter("Description", lblDescription.Text)
        Dim p4 As New ReportParameter("MinQty", lblMinLevel.Text)
        Dim p5 As New ReportParameter("MaxQty", lblMaxLevel.Text)
        Dim p6 As New ReportParameter("UOM", lblUOM.Text)
        Dim p7 As New ReportParameter("Location", lblLocation1.Text + " " + lblLocation2.Text)
        Dim p8 As New ReportParameter("Store", Session("StoreName").ToString)
        parameterlist.Add(p1)
        parameterlist.Add(p2)
        parameterlist.Add(p3)
        parameterlist.Add(p4)
        parameterlist.Add(p5)
        parameterlist.Add(p6)
        parameterlist.Add(p7)
        parameterlist.Add(p8)

        rvrCard.LocalReport.SetParameters(parameterlist)
        rvrCard.LocalReport.Refresh()

        Dim bytValue As Byte()
        bytValue = rvrCard.LocalReport.Render("Excel", Nothing, "application/ms-excel", "", "xls", Nothing, Nothing)

        Response.Buffer = True
        Response.Clear()
        Response.ContentType = "application/ms-excel"
        Response.AddHeader("content-disposition", "attachment;filename=StockCard.xls")
        Response.BinaryWrite(bytValue)
        Response.Flush()
        Response.End()
    End Sub

#End Region

#Region " Sub Procedures and Functions "

    '-- NOT NEEDED
    ''' <summary>
    ''' Sub Proc - BindStockCode;
    ''' 22 Jan 2009 - Jianfa;
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindStockCode(Optional ByVal equipmentID As String = "")

        Try

            Dim Client As New ServiceClient
            Dim ItemSearch As New ItemDetails

            ItemSearch.StoreID = Session("StoreID")
            ItemSearch.ItemID = String.Empty
            ItemSearch.Location = String.Empty
            ItemSearch.Status = String.Empty
            ItemSearch.EquipmentID = equipmentID

            Cache(Session(ESession.StoreID.ToString) & "StockCard") = Client.GetItems(ItemSearch, String.Empty, String.Empty)
            Client.Close()

            'ddlStockCode.DataSource = Cache(Session(ESession.StoreID.ToString) & "StockCard")

            'ddlStockCode.DataValueField = "ItemID"
            'ddlStockCode.DataTextField = "ItemID_Description"

            'ddlStockCode.DataBind()

            'ddlStockCode.Items.Insert(0, New ListItem(" - Please Select - ", ""))


        Catch ex As FaultException

            lblErrStockCard.Text = ex.Message
            lblErrStockCard.Visible = True

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

    ''' <summary>
    ''' Sub Proc - BindEquipmentNo;
    ''' 07 Jan 09 - Jianfa;
    ''' </summary>
    ''' <remarks></remarks>
    Private Function BindEquipmentNo(ByVal equipmentID As String) As String


        Dim Client As New ServiceClient
        Dim EquipmentDetails As New EquipmentDetails
        Dim EquipmentList As New List(Of EquipmentDetails)

        Try
            EquipmentDetails.StoreID = Session("StoreID")
            EquipmentDetails.EquipmentID = equipmentID
            EquipmentDetails.EquipmentType = String.Empty
            EquipmentDetails.EquipmentDescription = String.Empty
            EquipmentDetails.Status = String.Empty

            EquipmentList = Client.GetEquipments(EquipmentDetails, String.Empty, String.Empty)
            Client.Close()

        Catch ex As FaultException

            lblErrStockCard.Text = ex.Message
            lblErrStockCard.Visible = True

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        If EquipmentList.Count <= 0 Then
            Return String.Empty
        Else
            Return EquipmentList(0).EquipmentID_Description
        End If


    End Function

    ''' <summary>
    ''' Function - ValidateStockCode;
    ''' 05 March 09 - Jianfa;
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidateStockCode() As Boolean

        If txtStockCode.Text.Trim = EMPTY Or txtTransactionFrom.Text = String.Empty _
            Or txtTransactionTo.Text = String.Empty Then

            Dim message As String = GetMessage(messageID.MandatoryField)

            Dim Script As String = "ShowAlertMessage('" & message & "');"
            ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

            Return False
            Exit Function

        End If

        If ConvertToDate(txtTransactionFrom.Text) = DateTime.MinValue Then

            Dim message As String = GetMessage(messageID.NotIsDate, txtTransactionFrom.Text)

            Dim Script As String = "ShowAlertMessage('" & message & "');"
            ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

            Return False
            Exit Function

        End If

        If ConvertToDate(txtTransactionTo.Text) = DateTime.MinValue Then

            Dim message As String = GetMessage(messageID.NotIsDate, txtTransactionTo.Text)

            Dim Script As String = "ShowAlertMessage('" & message & "');"
            ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

            Return False
            Exit Function

        End If

        Return True

    End Function

    ''' <summary>
    ''' 02 May 09 - Jianfa
    ''' Web Shared Function - GetStockItems
    ''' </summary>
    ''' <param name="prefixText"></param>
    ''' <param name="count"></param>
    ''' <param name="contextKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Services.WebMethod()> _
    <Script.Services.ScriptMethod()> _
    Public Shared Function GetStockItems(ByVal prefixText As String, _
                                         ByVal count As Integer, _
                                         ByVal contextKey As String) _
                                            As List(Of String)

        Dim Client As New ServiceClient
        Dim ItemSearch As New ItemDetails
        Dim ItemList As New List(Of String)

        Try

            ItemSearch.StoreID = contextKey
            ItemSearch.ItemID = prefixText

            ItemList = Client.GetItemSearch(ItemSearch)

            Client.Close()

        Catch ex As Exception
            Throw
        End Try

        Return ItemList

    End Function

#End Region

End Class