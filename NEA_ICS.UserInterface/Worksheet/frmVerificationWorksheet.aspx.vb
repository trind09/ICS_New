Imports Microsoft.Reporting.WebForms
Imports System.Web.Services.Description
Imports NEA_ICS.UserInterface.NEA_ICS.WcfService
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ServiceModel
Imports System.Reflection

Partial Public Class frmVerificationWorksheet
    Inherits clsCommonFunction

    Private Shared ItemList As List(Of ItemDetails)
    Private Shared WorkSheetItemList As List(Of WorksheetDetails)

#Region " PAGE LOAD "

    ''' <summary>
    ''' Sub Proc - Page Load
    ''' 30 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim AccessRights As New List(Of RoleDetails)

        CheckValidSession()

        If Not Page.IsPostBack Then

            AccessRights = clsCommonFunction.AssignAccessRights(Cache(Session(ESession.UserID.ToString) & Session(ESession.StoreID.ToString) & "AccessRights"), _
                                clsCommonFunction.moduleID.VerificationWorkSheet)

            If AccessRights(0).SelectRight = False Then
                Server.Transfer("..\frmUnauthorisedPage.aspx")
                Exit Sub
            End If

            If AccessRights(0).InsertRight = False Then
                btnGenerateMarkedItem.Visible = False
                btnGenerateAllItem.Visible = False
                gdvVerificationWorksheet.Columns(0).Visible = False
                ViewState("_INSERT") = False
            Else
                ViewState("_INSERT") = True
            End If

            'ddlStockCodeTo.Enabled = False
            BindItem()
            BindStockType()
            BindSubType()

            If Not Cache(Session(ESession.UserID) & Session(ESession.StoreID) & "Worksheet") Is Nothing Then
                Cache.Remove(Session(ESession.UserID) & Session(ESession.StoreID) & "Worksheet")
            End If

        End If

    End Sub
#End Region

#Region " VERIFICATION WORKSHEET "

    '''' <summary>
    '''' ddlStockCodeFrom - SelectedIndexChanged;
    '''' 30 Jan 09 - Jianfa;
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Private Sub ddlStockCodeFrom_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlStockCodeFrom.SelectedIndexChanged

    '    If ddlStockCodeFrom.SelectedIndex > 0 Then

    '        Dim ItemToList As New List(Of ItemDetails)

    '        For Each Item As ItemDetails In ItemList

    '            If Item.ItemID >= ddlStockCodeFrom.SelectedValue Then

    '                Dim ItemDetailsItem As New ItemDetails

    '                ItemDetailsItem.ItemID = Item.ItemID
    '                ItemToList.Add(ItemDetailsItem)

    '            End If

    '        Next

    '        ddlStockCodeTo.Items.Clear()
    '        ddlStockCodeTo.DataSource = ItemToList
    '        ddlStockCodeTo.DataValueField = "ItemID"
    '        ddlStockCodeTo.DataTextField = "ItemID"

    '        ddlStockCodeTo.DataBind()
    '        ddlStockCodeTo.Items.Insert(0, New ListItem(" - Please Select - ", ""))

    '        ddlStockCodeTo.Enabled = True

    '    End If

    'End Sub

    ''' <summary>
    ''' btnGo - Click;
    ''' 30 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGo.Click

        Try

            '-- VALIDATION CHECKS
            If ddlStockCodeFrom.SelectedIndex <= 0 Or ddlStockCodeTo.SelectedIndex <= 0 Or _
            Not CheckStockType() Or Not CheckSubType() Then

                lblErrLocateWorksheet.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.MandatoryField)
                lblErrLocateWorksheet.Visible = True

                Dim Script As String = "ShowAlertMessage('" & lblErrLocateWorksheet.Text & "');"
                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

                Exit Sub

            End If

            If Not revValueMoreThan.IsValid Then

                Exit Sub

            End If

            Dim Client As New ServiceClient
            Dim WorkSheetItemSearch As New WorksheetDetails
            Dim StockType As String = String.Empty
            Dim SubType As String = String.Empty

            WorkSheetItemSearch.StoreID = Session("StoreID")
            WorkSheetItemSearch.StockCodeFrom = ddlStockCodeFrom.SelectedValue.Trim
            WorkSheetItemSearch.StockCodeTo = ddlStockCodeTo.SelectedValue.Trim

            For Each item As ListItem In chkStockType.Items

                If item.Selected = True Then
                    StockType &= item.Value & "','"
                End If
            Next

            '-- UAT.02.63 -- To implement Sub Type as well
            For Each item As ListItem In chkSubType.Items

                If item.Selected = True Then
                    SubType &= item.Value & "','"
                End If
            Next

            StockType = Left(StockType, StockType.Length - 3)

            WorkSheetItemSearch.StockType = StockType
            WorkSheetItemSearch.SubType = SubType
            WorkSheetItemSearch.TotalValue = CDec(txtValueMoreThan.Text)
            WorkSheetItemSearch.WorkSheetStatus = OPEN

            WorkSheetItemList = Client.GetWorkSheetItems(WorkSheetItemSearch, String.Empty, String.Empty)
            Client.Close()

            '-- UAT.02.56 -- Propose appending worksheet selection
            If Cache(Session(ESession.UserID) & Session(ESession.StoreID) & "Worksheet") Is Nothing Then
                Cache(Session(ESession.UserID) & Session(ESession.StoreID) & "Worksheet") = WorkSheetItemList
            Else
                EditCache(Cache(Session(ESession.UserID) & Session(ESession.StoreID) & "Worksheet"), GetType(WorksheetDetails), WorkSheetItemList)
            End If

            If DirectCast(Cache(Session(ESession.UserID) & Session(ESession.StoreID) & "Worksheet"),  _
                List(Of WorksheetDetails)).Count = 0 Then

                btnGenerateAllItem.Visible = False
                btnGenerateMarkedItem.Visible = False

            Else

                If Not DirectCast(ViewState("_INSERT"), Boolean) Then

                    btnGenerateAllItem.Visible = False
                    btnGenerateMarkedItem.Visible = False

                Else

                    btnGenerateAllItem.Visible = True
                    btnGenerateMarkedItem.Visible = True

                End If

                gdvVerificationWorksheet.DataSource = Cache(Session(ESession.UserID) & Session(ESession.StoreID) & "Worksheet")
                gdvVerificationWorksheet.DataBind()

                pnlLocateWorksheet.Visible = True


            End If

        Catch ex As FaultException

            lblErrLocateWorksheet.Text = ex.Message
            lblErrLocateWorksheet.Visible = True

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

    ''' <summary>
    ''' gdvVerificationWorksheet - Sorting
    ''' 30 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gdvVerificationWorksheet_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gdvVerificationWorksheet.Sorting

        If ViewState("_SortDirection") Is Nothing Then
            ViewState("_SortDirection") = "DESC"
        Else

            If ViewState("_SortDirection") = "DESC" Then
                ViewState("_SortDirection") = "ASC"
            Else
                ViewState("_SortDirection") = "DESC"
            End If

        End If

        Try

            Dim Client As New ServiceClient
            Dim WorkSheetItemSearch As New WorksheetDetails
            Dim StockType As String = String.Empty

            WorkSheetItemSearch.StoreID = Session("StoreID")
            WorkSheetItemSearch.StockCodeFrom = ddlStockCodeFrom.SelectedValue.Trim
            WorkSheetItemSearch.StockCodeTo = ddlStockCodeTo.SelectedValue.Trim

            For Each item As ListItem In chkStockType.Items

                If item.Selected = True Then
                    StockType &= item.Value & "','"
                End If
            Next

            StockType = Left(StockType, StockType.Length - 3)

            WorkSheetItemSearch.StockType = StockType
            WorkSheetItemSearch.TotalValue = CDec(txtValueMoreThan.Text)
            WorkSheetItemSearch.WorkSheetStatus = OPEN

            WorkSheetItemList = Client.GetWorkSheetItems(WorkSheetItemSearch, e.SortExpression, ViewState("_SortDirection"))
            Client.Close()

            gdvVerificationWorksheet.DataSource = WorkSheetItemList
            gdvVerificationWorksheet.DataBind()

            pnlLocateWorksheet.Visible = True


        Catch ex As FaultException

            lblErrLocateWorksheet.Text = ex.Message
            lblErrLocateWorksheet.Visible = True

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

    ''' <summary>
    ''' btnClear - Click;
    ''' 02 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click

        ddlStockCodeFrom.SelectedIndex = -1
        ddlStockCodeTo.SelectedIndex = -1

        For Each item As ListItem In chkStockType.Items
            item.Selected = False
        Next

        For Each item As ListItem In chkSubType.Items
            item.Selected = False
        Next

        txtValueMoreThan.Text = "0.0000"
        pnlLocateWorksheet.Visible = False
        lblErrLocateWorksheet.Visible = False
        lblErrSaveWorksheet.Visible = False

        If Not Cache(Session(ESession.UserID) & Session(ESession.StoreID) & "Worksheet") Is Nothing Then
            Cache.Remove(Session(ESession.UserID) & Session(ESession.StoreID) & "Worksheet")
        End If

    End Sub

    ''' <summary>
    ''' btnGenerateAllItem - Click;
    ''' 02 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnGenerateAllItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerateAllItem.Click

        Dim WorkSheetID As Integer

        Try

            Dim Client As New ServiceClient
            Dim WorkSheetDetails As New WorksheetDetails
            Dim WorkSheetItemList As New List(Of WorksheetDetails)

            For Each row As GridViewRow In gdvVerificationWorksheet.Rows

                Dim WorkSheetItem As New WorksheetDetails
                WorkSheetItem.ItemID = row.Cells(1).Text
                WorkSheetItem.StockQty = CDec(row.Cells(5).Text)
                WorkSheetItem.TotalValue = CDec(row.Cells(6).Text)

                WorkSheetItemList.Add(WorkSheetItem)

            Next

            WorkSheetDetails.StoreID = Session("StoreID")
            WorkSheetDetails.LoginUser = Session("UserID")

            lblErrSaveWorksheet.Text = Client.AddWorkSheetItem(WorkSheetDetails, WorkSheetItemList, WorkSheetID)
            Client.Close()

            If lblErrSaveWorksheet.Text = String.Empty Then

                lblErrSaveWorksheet.Visible = False

                Dim MessageWorksheet As String
                MessageWorksheet = "\n\nYour Worksheet Verification Reference No is " & WorkSheetID & ". "

                Dim Script As String = "ShowWorksheetSuccessMessage('" & clsCommonFunction.GetMessage( _
                                       clsCommonFunction.messageID.Success, "generated", "Worksheet") & MessageWorksheet & "','" & WorkSheetID & "');"

                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)
                lblErrSaveWorksheet.Visible = False
                Exit Sub

            Else
                lblErrSaveWorksheet.Visible = True
            End If


        Catch ex As FaultException

            lblErrSaveWorksheet.Text = "<br>" & ex.Message
            lblErrSaveWorksheet.Visible = True

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

    ''' <summary>
    ''' btnGenerateMarkedItem - Click;
    ''' 03 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnGenerateMarkedItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerateMarkedItem.Click

        Dim WorkSheetInsert As Boolean = False
        Dim WorkSheetID As Integer

        Try

            Dim Client As New ServiceClient
            Dim WorkSheetDetails As New WorksheetDetails
            Dim WorkSheetItemList As New List(Of WorksheetDetails)

            For Each row As GridViewRow In gdvVerificationWorksheet.Rows

                If CType(row.FindControl("chkMark"), CheckBox).Checked Then

                    Dim WorkSheetItem As New WorksheetDetails
                    WorkSheetItem.ItemID = row.Cells(1).Text
                    WorkSheetItem.StockQty = CDec(row.Cells(5).Text)
                    WorkSheetItem.TotalValue = CDec(row.Cells(6).Text)

                    WorkSheetItemList.Add(WorkSheetItem)
                    WorkSheetInsert = True

                End If

            Next

            If WorkSheetInsert Then

                WorkSheetDetails.StoreID = Session("StoreID")
                WorkSheetDetails.LoginUser = Session("UserID")

                lblErrSaveWorksheet.Text = Client.AddWorkSheetItem(WorkSheetDetails, WorkSheetItemList, WorkSheetID)
                Client.Close()

                If lblErrSaveWorksheet.Text = String.Empty Then

                    lblErrSaveWorksheet.Visible = False

                    Dim MessageWorksheet As String
                    MessageWorksheet = "\n\nYour Worksheet Verification Reference No is " & WorkSheetID & ". "

                    Dim Script As String = "ShowWorksheetSuccessMessage('" & clsCommonFunction.GetMessage( _
                       clsCommonFunction.messageID.Success, "generated", "Worksheet") & MessageWorksheet & "','" & WorkSheetID & "');"

                    ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)
                    Exit Sub

                Else
                    lblErrSaveWorksheet.Visible = True
                End If

            Else

                lblErrSaveWorksheet.Text = "<br>No item(s) are marked for generation of worksheet."
                lblErrSaveWorksheet.Visible = True

            End If

        Catch ex As FaultException

            lblErrSaveWorksheet.Text = "<br>" & ex.Message
            lblErrSaveWorksheet.Visible = True

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

#End Region

#Region " Sub Procedures and Functions "

    ''' <summary>
    ''' Sub Proc - Bind Item
    ''' 30 Jan 09 - Jianfa;
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindItem()

        Try

            Dim Client As New ServiceClient
            Dim ItemDetails As New ItemDetails

            ItemDetails.StoreID = Session("StoreID")
            ItemDetails.ItemID = String.Empty
            ItemDetails.Location = String.Empty
            ItemDetails.Status = OPEN
            ItemDetails.EquipmentID = String.Empty

            ItemList = Client.GetItems(ItemDetails, String.Empty, String.Empty)
            Client.Close()

            '-- BECAUSE OF AJAX ISSUES HAVE TO BIND StockCodeTo AS WELL
            ddlStockCodeTo.Items.Clear()
            ddlStockCodeTo.DataSource = ItemList
            ddlStockCodeTo.DataValueField = "ItemID"
            ddlStockCodeTo.DataTextField = "ItemID"

            ddlStockCodeTo.DataBind()
            ddlStockCodeTo.Items.Insert(0, New ListItem(" - Please Select - ", ""))

            ddlStockCodeFrom.Items.Clear()
            ddlStockCodeFrom.DataSource = ItemList
            ddlStockCodeFrom.DataValueField = "ItemID"
            ddlStockCodeFrom.DataTextField = "ItemID"

            ddlStockCodeFrom.DataBind()
            ddlStockCodeFrom.Items.Insert(0, New ListItem(" - Please Select - ", ""))


        Catch ex As FaultException

            lblErrLocateWorksheet.Text = ex.Message
            lblErrLocateWorksheet.Visible = True

        Catch ex As Exception
            lblErrLocateWorksheet.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            lblErrLocateWorksheet.Visible = True
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

    ''' <summary>
    ''' Sub Proc - BindStockType;
    ''' 30 Jan 09 - Jianfa;
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindStockType()

        Dim View As DataView
        View = clsCommonFunction.GetCommonDataByCodeGroup(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), clsCommonFunction.codeGroup.StockType, OPEN)

        chkStockType.Items.Clear()
        chkStockType.DataSource = View

        chkStockType.DataTextField = "CommonCodeID_Description"
        chkStockType.DataValueField = "CommonCodeID"
        chkStockType.DataBind()

    End Sub

    ''' <summary>
    ''' Sub Proc - BindSubType;
    ''' 20 Mar 09 - Jianfa;
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindSubType()

        Dim View As DataView
        View = clsCommonFunction.GetCommonDataByCodeGroup(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), clsCommonFunction.codeGroup.SubType, OPEN)

        chkSubType.Items.Clear()
        chkSubType.DataSource = View

        chkSubType.DataTextField = "CommonCodeID_Description"
        chkSubType.DataValueField = "CommonCodeID"
        chkSubType.DataBind()

    End Sub

    ''' <summary>
    ''' Function - CheckStockType;
    ''' 04 March 09 - Jianfa;
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckStockType() As Boolean

        For Each item As ListItem In chkStockType.Items

            If item.Selected Then

                Return True
                Exit Function

            End If

        Next

        Return False

    End Function

    ''' <summary>
    ''' Function - CheckSubType;
    ''' 20 Mar 09 - Jianfa;
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckSubType() As Boolean

        For Each item As ListItem In chkSubType.Items

            If item.Selected Then

                Return True
                Exit Function

            End If

        Next

        Return False

    End Function


#End Region
    
End Class