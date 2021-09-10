Imports Microsoft.Reporting.WebForms
Imports System.Web.Services.Description
Imports NEA_ICS.UserInterface.NEA_ICS.WcfService
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ServiceModel
Imports DBauer.Web.UI.WebControls
Imports System.Web.Services
Imports System.Reflection

''' <summary>
''' Code behind for frmIssueItem Page;
''' 18Feb09 - KG;
''' </summary>
''' <remarks>
''' CHANGE LOG:
''' ddMMMyy  AuthorName  RefID  Description;
''' 20Mar09  KG          UAT02  cannot accomodate requester info in Issue report;
''' </remarks>
Partial Public Class frmIssueItem
    Inherits clsCommonFunction

#Region " Page Control "

    Private Message As String = EMPTY
    Private IssueItem As IssueItem

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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CheckValidSession()

        Try
            If Not Page.IsPostBack Then

                aceStockCode.ContextKey = Session("StoreID").ToString
                aceStockCodeFrom.ContextKey = Session("StoreID").ToString
                aceStockCodeTo.ContextKey = Session("StoreID").ToString

                Dim ConsumerByUserList As New List(Of ConsumerDetails)

                ''trApprove.Visible = True
                ''trRequest.Visible = True
                ' @@@ START OF ACCESS RIGHTS @@@
                Dim AccessRights As New List(Of RoleDetails)

                tbcIssueItem.Visible = False
                AccessRights = clsCommonFunction.AssignAccessRights(Cache(Session(ESession.UserID.ToString) & Session(ESession.StoreID.ToString) & "AccessRights") _
                                                                    , moduleID.IssueFromStore _
                                                                    )

                ' Select Rights
                tbpLocate.Visible = False
                tbpPrint.Visible = False
                If AccessRights(0).SelectRight Then
                    tbpLocate.Visible = True
                    tbpPrint.Visible = True
                    tbcIssueItem.Visible = True
                    tbpLocate.Focus()

                Else
                    Server.Transfer("..\frmUnauthorisedPage.aspx")
                    Exit Sub
                End If


                ' Store Request Rights
                tbpNew.Visible = False
                If Session(ESession.UserRoleType.ToString).ToString.Contains(STOREBROWSER) Then
                    ViewState("_Locate") = "NEW"
                    tbpNew.Visible = True
                    tbcIssueItem.ActiveTabIndex = 0
                    MainPanel(True)
                Else
                    ViewState("_Locate") = "LOCATE"
                End If


                ' Store Approve Rights
                btnLocateApprove.Visible = False
                btnLocateReject.Visible = False
                If Session(ESession.UserRoleType.ToString).ToString.Contains(APPROVALOFFICER) Then
                    btnLocateApprove.Visible = True
                    btnLocateReject.Visible = True
                End If


                ' Insert Store Issue Rights
                btnLocateSave.Visible = False
                btnLocateCancel.Visible = False
                ViewState(EViewState.IssueInsert) = False
                If AccessRights(0).InsertRight Then
                    btnLocateSave.Visible = True
                    btnLocateCancel.Visible = True
                    ViewState(EViewState.IssueInsert) = True
                End If


                ' Update or Delete Rights
                pnlLocateAccess.Visible = False
                btnLocateEdit.Visible = False
                btnLocateDeleteAll.Visible = False
                ViewState(EViewState.IssueUpdate) = False
                ViewState(EViewState.IssueDelete) = False
                '  Update Rights
                If AccessRights(0).UpdateRight Then
                    pnlLocateAccess.Visible = True
                    btnLocateEdit.Visible = True
                    btnLocateSave.Visible = True
                    btnLocateCancel.Visible = True
                    ViewState(EViewState.IssueUpdate) = True
                End If

                '  Delete Rights
                If AccessRights(0).DeleteRight Then
                    pnlLocateAccess.Visible = True
                    btnLocateDeleteAll.Visible = True
                    ViewState(EViewState.IssueDelete) = True
                End If
                ' @@@ END OF ACCESS RIGHTS @@@


                ' retrieve listing to be bind to ddl
                If CacheListIsEmpty(Cache(Session(ESession.StoreID.ToString) & ECache.ConsumerList), GetType(ConsumerDetails)) Then GetConsumerList(Cache(Session(ESession.StoreID.ToString) & ECache.ConsumerList), Session(ESession.StoreID.ToString))
                If CacheListIsEmpty(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), GetType(ItemDetails)) Then GetItemList(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), Session(ESession.StoreID.ToString))
                If CacheListIsEmpty(Cache(Session(ESession.StoreID.ToString) & ECache.RequestList), GetType(RequestDetails)) Then GetRequestList(Cache(Session(ESession.StoreID.ToString) & ECache.RequestList), Session(ESession.StoreID.ToString), ALL)
                GetUserList(Cache(Session(ESession.StoreID.ToString) & ECache.UserNameList), Session(ESession.StoreID.ToString))

                ' New Tab ' UAT02.13 - always get list from db
                If tbpNew.Visible Then
                    GetConsumerListByUserID(ConsumerByUserList, Session(ESession.StoreID.ToString), Session(ESession.UserID.ToString), STOREBROWSER)

                    BindDropDownList(ddlConsumerID, ConsumerByUserList, "ConsumerID", "ConsumerDescription")
                    BindDropDownList(ddlDocType, GetCommonDataByCodeGroup(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), codeGroup.IssueDocType, OPEN), "CommonCodeID", "CommonCodeDescription")
                    '   to allow Exit button click on Modal to invoke page postback
                    btnExit.OnClientClick = String.Format("postbackFromJS('{0}', '{1}')", btnExit.UniqueID, String.Empty)
                End If

                ' Locate Tab
                pnlSearchResults.Visible = False
                pnllocate.Visible = False
                pnlLocateAccess.Enabled = False
                ddlLocateDocType.Enabled = False
                txtLocateIssueDate.Enabled = False
                txtLocateSerialNo.Enabled = False
                pnlLocateSearchItem.Enabled = False
                btnLocateApprove.Enabled = False
                btnLocateReject.Enabled = False
                btnLocateSave.Enabled = False
                btnLocateCancel.Enabled = False

                BindDropDownList(ddlLocateConsumerSearch, Cache(Session(ESession.StoreID.ToString) & ECache.ConsumerList), "ConsumerID", "ConsumerID_Description")
                BindDropDownList(ddlLocateDocType, GetCommonDataByCodeGroup(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), codeGroup.IssueDocType, OPEN), "CommonCodeID", "CommonCodeDescription")
                '   to allow Exit button click on Modal to invoke page postback
                btnLocateExit.OnClientClick = String.Format("postbackFromJS('{0}', '{1}')", btnLocateExit.UniqueID, String.Empty)


                ' Report Tab 
                btnClear_Click(Nothing, Nothing)
                BindDropDownList(ddlIssueReference, Cache(Session(ESession.StoreID.ToString) & ECache.RequestList), "RequestID", "RequestID")
                '--=================================================
                '-- ERSS 12714444: 
                '--=================================================
                '-- 1) Enhance to have consumer ID in the report
                '--=================================================
                '-- Last Update By: Jianfa
                '-- Last Update Date: 20 Dec 2010
                '--=================================================
                BindDropDownList(ddlPrintConsumerID, Cache(Session(ESession.StoreID.ToString) & ECache.ConsumerList), "ConsumerID", "ConsumerID_Description")
                '--=================================================

            Else

                If tbcIssueItem.ActiveTabIndex - 1 < 0 Then
                    tbcIssueItem.ActiveTabIndex = 0
                Else
                    tbcIssueItem.ActiveTabIndex = tbcIssueItem.ActiveTabIndex - 1
                End If

            End If

        Catch ex As FaultException

            Message = ex.Message

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    ''' <summary>
    ''' Page_PreRender;
    ''' display error message if any
    ''' 24Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ' alert user with message (if any)
        If Message <> EMPTY Then
            ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", "ShowAlertMessage('" & Message & "');", True)
        End If
    End Sub

#End Region

#Region " New Tab "

    Private Function IsValidEntry() As Boolean
        If (ddlConsumerID.SelectedValue <> EMPTY _
            And ddlDocType.SelectedValue <> EMPTY _
            And cbxSought.Checked _
            ) Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub btnAddIssueItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddIssueItem.Click
        Try
            If IsValidEntry() Then
                ViewState(EViewState.RequestedStockItemID) = New List(Of String)

                ''- NOT IN USE [01/05/2009]
                'If CacheListIsEmpty(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), GetType(ItemDetails)) Then GetItemList(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), Session(ESession.StoreID.ToString))
                'BindDropDownList(ddlStockCode, Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), "ItemID", "ItemID_Description")

                MainPanel(False)

            Else
                Message = GetMessage(messageID.MandatoryField)
            End If

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    Protected Sub btnAddItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddItem.Click
        Try
            If txtStockCode.Text.Trim <> EMPTY Then

                '-- VALIDATE STOCK CODE
                Dim StockCode As String
                StockCode = Split(txtStockCode.Text, " | ")(0).Trim

                Using Client As New ServiceClient

                    Dim ItemDetails As New ItemDetails
                    ItemDetails.StoreID = Session("StoreID").ToString
                    ItemDetails.ItemID = StockCode

                    If Not Client.IsValidStockCode(ItemDetails) Then

                        Message = GetMessage(messageID.InvalidStockCode, StockCode.ToUpper)
                        Client.Close()
                        Exit Sub

                    End If

                    If Not Client.IsValidStatus(ItemDetails) Then

                        Message = GetMessage(messageID.StockCodeClosed, StockCode.ToUpper)
                        Client.Close()
                        Exit Sub

                    End If

                    Client.Close()

                End Using

                Dim IssueItemDetailsItem As New IssueItemDetails
                Dim MoreItemInfo As MoreItemInfoDetails

                Dim Requested As Boolean = DirectCast(ViewState(EViewState.RequestedStockItemID), List(Of String)).Exists(Function(i As String) (i = StockCode))

                If Requested Then
                    Throw New ApplicationException(GetMessage(messageID.StockCodeHasOrdered, StockCode))
                End If

                ' retrieve Cache more Info for the Stock Item to be display using Modal
                Using Client As New ServiceClient
                    MoreItemInfo = Client.GetMoreItemInfo(Session(ESession.StoreID.ToString).ToString _
                                                          , StockCode _
                                                          , Today _
                                                          )

                    IssueItemDetailsItem.StockItemID = StockCode
                    IssueItemDetailsItem.Mode = INSERT
                    IssueItemDetailsItem.BalanceQty = MoreItemInfo.Balance

                    Client.Close()
                End Using

                ' UAT02.24 restrict request when balance is zero
                If MoreItemInfo.Balance <= 0 Then Throw New ApplicationException(String.Format("[{0}] cannot be request when available balance is Zero.", IssueItemDetailsItem.StockItemID))

                AddIssueItem(DCP, IssueItemDetailsItem, EViewState.IssueItem.ToString)

                ' keep a list of Requested stock item, to check against to prevent duplication
                DirectCast(ViewState(EViewState.RequestedStockItemID), List(Of String)).Add(IssueItemDetailsItem.StockItemID)

                uplUserControl.Update()

            Else
                ' alert message
                Throw New ApplicationException(GetMessage(messageID.StockCodeNotSelected))
            End If

        Catch ex As ApplicationException
            Message = ex.Message

        Catch ex As FaultException
            Message = ex.Message

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    ''' <summary>
    ''' btnSubmit_Click;
    ''' Save data to database;
    ''' 1)call a function to Collect request information;
    ''' 2)Process to update when Issuelist is filled;
    ''' 24Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Try
            'save data to database
            Dim Request As RequestDetails
            Dim DetailsList As List(Of IssueItemDetails)
            Dim RequestID As String = EMPTY

            ' Request Details
            Request = New RequestDetails
            Request.StoreID = Session(ESession.StoreID.ToString).ToString
            Request.ConsumerID = ddlConsumerID.SelectedValue
            Request.Type = ddlDocType.SelectedValue
            Request.Status = OPEN  'default value
            Request.Sought = cbxSought.Checked
            Request.RequestDte = Today
            Request.RequestBy = Session(ESession.UserID.ToString).ToString
            Request.LoginUser = Session(ESession.UserID.ToString).ToString

            ViewState("_Locate") = "NEW"
            DetailsList = RetrieveDCPInfo(EViewState.IssueItem.ToString)

            ' UAT02 - alert user when no stockitem
            If DetailsList.Count = 0 Then Throw New ApplicationException(GetMessage(messageID.StockCodeNotAdded))

            Using Client As New ServiceClient
                RequestID = Client.AddRequest(Request _
                                              , DetailsList _
                                              )
            End Using

            If RequestID.Length <= 12 Then
                ' Reset Control n Clear screen
                ddlConsumerID.SelectedIndex = -1
                ddlDocType.SelectedIndex = -1
                cbxSought.Checked = False
                btnCancelAll_Click(sender, e)

                Message = GetMessage(messageID.Success, "Saved", String.Format("Store Request[{0}]", RequestID))

                Request.RequestID = RequestID
                ' Add new Request to list and rebind (if needed)
                If Not (DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.RequestList), List(Of RequestDetails)).Exists(Function(i As RequestDetails) i.RequestID = RequestID)) Then
                    EditCache(Cache(Session(ESession.StoreID.ToString) & ECache.RequestList), GetType(RequestDetails), Request)
                    BindDropDownList(ddlIssueReference, Cache(Session(ESession.StoreID.ToString) & ECache.RequestList), "RequestID", "RequestID")
                    uplPrintRequestID.Update()
                End If

            Else
                Throw New ApplicationException(GetMessage(messageID.TryLastOperation))
            End If

        Catch ex As FaultException
            Message = ex.Message

        Catch ex As ApplicationException
            Message = ex.Message

        Catch ex As ArgumentNullException
            Message = GetMessage(messageID.MandatoryField)

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    Protected Sub btnCancelAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelAll.Click
        Try
            ViewState(EViewState.RequestedStockItemID) = New List(Of String)
            ViewState.Remove(EViewState.IssueItem.ToString)
            DCP.Controls.Clear()

            MainPanel(True)

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    Protected Sub ShowModal_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ShowModal.Click
        mpuStockAvailability.Show()
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExit.Click
        ShowModal.Visible = False
    End Sub

#End Region

#Region " Locate Tab "
    Protected Sub btnLocateGo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLocateGo.Click
        Try
            ' UAT02 - always get data from the service and store in Cache prefix with UserID
            GetRequestList(Cache(Session(ESession.UserID.ToString) & ECache.RequestListSearch), Session(ESession.StoreID.ToString), ddlLocateConsumerSearch.SelectedValue, txtLocateRequestID.Text, ddlLocateStatus.SelectedValue)
            gdvLocate.DataSource = Cache(Session(ESession.UserID.ToString) & ECache.RequestListSearch)
            gdvLocate.DataBind()

            pnlSearchResults.Visible = True
            pnllocate.Visible = False

        Catch ex As FaultException
            Message = ex.Message

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    Protected Sub btnLocateClear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLocateClear.Click
        txtLocateRequestID.Text = EMPTY
        ddlLocateStatus.SelectedValue = ALL
        ddlLocateConsumerSearch.SelectedIndex = -1

        ' Enable/Disable control for editing
        pnlSearchResults.Visible = False
        pnllocate.Visible = False
        pnlLocateAccess.Enabled = False
        ddlLocateDocType.Enabled = False
        txtLocateIssueDate.Enabled = False
        txtLocateSerialNo.Enabled = False
        pnlLocateSearchItem.Enabled = False
        btnLocateApprove.Enabled = False
        btnLocateReject.Enabled = False
        btnLocateSave.Enabled = False
        btnLocateCancel.Enabled = False
        DCPLocate.Controls.Clear()
        ViewState.Remove(EViewState.Mode)
    End Sub

    ''' <summary>
    ''' gdvLocate - PageIndexChaning;
    ''' 09 Jan 09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 17Mar09 KG UAT02 - Cache with UserID as the prefix;
    ''' </remarks>
    Private Sub gdvLocate_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gdvLocate.PageIndexChanging
        gdvLocate.PageIndex = e.NewPageIndex
        gdvLocate.DataSource = Cache(Session(ESession.UserID.ToString) & ECache.RequestListSearch) 'UAT02
        gdvLocate.DataBind()
    End Sub

    ''' <summary>
    ''' gdvLocate - RowDataBound;
    ''' 09 Jan 09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gdvLocate_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvLocate.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Select Case CType(e.Row.FindControl("hidStatus"), HiddenField).Value.ToUpper
                Case OPEN
                    CType(e.Row.FindControl("lblStatus"), Label).Text = "Open"

                Case APPROVED
                    CType(e.Row.FindControl("lblStatus"), Label).Text = "Approved"

                Case REJECTED
                    CType(e.Row.FindControl("lblStatus"), Label).Text = "Rejected"

                Case CLOSED
                    CType(e.Row.FindControl("lblStatus"), Label).Text = "Closed"
            End Select

            Dim userItem = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.UserNameList), List(Of ConsumerDetails)).Find(Function(i As ConsumerDetails) i.UserID.ToUpper = CType(e.Row.FindControl("hidRequestBy"), HiddenField).Value.ToUpper)
            If userItem IsNot Nothing Then
                CType(e.Row.FindControl("lblRequestBy"), Label).Text = userItem.UserName

            Else
                ' display UserID when name if not available
                CType(e.Row.FindControl("lblRequestBy"), Label).Text = CType(e.Row.FindControl("hidRequestBy"), HiddenField).Value.ToUpper
            End If
        End If
    End Sub

    ''' <summary>
    ''' gdvLocate - SelectedIndexChanged;
    ''' 09 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 17Mar09  KG  UAT02 - check and prompt user with AO rights only;
    ''' 11May09  JF  UAT04 - Approval and Issuer Logic WRONG
    ''' </remarks>
    Private Sub gdvLocate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvLocate.SelectedIndexChanged
        Try
            Dim DetailsList As List(Of IssueItemDetails)
            Dim DetailsItem As IssueItemDetails
            'Dim Consumer As ConsumerDetails
            Dim UserList As List(Of RoleDetails)
            Dim UserProfile As RoleDetails

            lblLocateConsumerID.Text = Replace(gdvLocate.SelectedRow.Cells(1).Text, "&amp;", "&")
            lblLocateRequestID.Text = gdvLocate.SelectedRow.Cells(2).Text
            lblLocateRequestBy.Text = CType(gdvLocate.SelectedRow.FindControl("lblRequestBy"), Label).Text
            lblLocateStatus.Text = CType(gdvLocate.SelectedRow.FindControl("lblStatus"), Label).Text

            ddlLocateDocType.SelectedValue = BindActiveCodes(ddlLocateDocType, CType(gdvLocate.SelectedRow.FindControl("hidType"), HiddenField).Value)

            lblLocateRequestDate.Text = IIf(CType(gdvLocate.SelectedRow.FindControl("hidRequestDte"), HiddenField).Value > Date.MinValue, CDate((CType(gdvLocate.SelectedRow.FindControl("hidRequestDte"), HiddenField).Value)).ToString("dd/MM/yyyy"), EMPTY)
            lblLocateApproveDate.Text = IIf(CType(gdvLocate.SelectedRow.FindControl("hidApproveDte"), HiddenField).Value > Date.MinValue, CDate((CType(gdvLocate.SelectedRow.FindControl("hidApproveDte"), HiddenField).Value)).ToString("dd/MM/yyyy"), EMPTY)
            ' UAT02.57
            txtLocateIssueDate.Text = IIf(CType(gdvLocate.SelectedRow.FindControl("hidIssueDte"), HiddenField).Value > Date.MinValue, CDate((CType(gdvLocate.SelectedRow.FindControl("hidIssueDte"), HiddenField).Value)).ToString("dd/MM/yyyy"), Today.ToString("dd/MM/yyyy"))

            '------ 11May09  JF  UAT04 - Approval and Issuer Logic WRONG --------------------
            '' get the Issuer User ID
            'Consumer = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.UserNameList), List(Of ConsumerDetails)).Find(Function(i As ConsumerDetails) i.UserID = CType(gdvLocate.SelectedRow.FindControl("hidIssueBy"), HiddenField).Value)
            'If (Consumer IsNot Nothing) Then lblLocateIssueBy.Text = Consumer.UserName

            '' get the Approver User ID
            'Consumer = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.UserNameList), List(Of ConsumerDetails)).Find(Function(i As ConsumerDetails) i.UserID = CType(gdvLocate.SelectedRow.FindControl("hidApproveBy"), HiddenField).Value)
            'If (Consumer IsNot Nothing) Then lblLocateApproveBy.Text = Consumer.UserName
            '--------------------------------------------------------------------------------

            txtLocateSerialNo.Text = CType(gdvLocate.SelectedRow.FindControl("hidSerialNo"), HiddenField).Value

            DetailsItem = New IssueItemDetails
            DetailsList = New List(Of IssueItemDetails)
            UserList = New List(Of RoleDetails)

            Using Client As New ServiceClient

                If CType(gdvLocate.SelectedRow.FindControl("hidIssueBy"), HiddenField).Value.Trim <> String.Empty Then

                    UserProfile = New RoleDetails

                    'Get Issuer
                    UserProfile.StoreID = Session(ESession.StoreID.ToString)
                    UserProfile.UserID = CType(gdvLocate.SelectedRow.FindControl("hidIssueBy"), HiddenField).Value

                    If Client.GetUserProfile(UserProfile).Count = 0 Then
                        lblLocateIssueBy.Text = CType(gdvLocate.SelectedRow.FindControl("hidIssueBy"), HiddenField).Value.Trim
                        lblLocateApproveBy.Text = CType(gdvLocate.SelectedRow.FindControl("hidApproveBy"), HiddenField).Value.Trim
                        Exit Sub
                    End If

                    lblLocateIssueBy.Text = Client.GetUserProfile(UserProfile)(0).Name

                End If

                If CType(gdvLocate.SelectedRow.FindControl("hidApproveBy"), HiddenField).Value.Trim <> String.Empty Then

                    UserProfile = New RoleDetails

                    'Get Approver
                    UserProfile.StoreID = Session(ESession.StoreID.ToString)
                    UserProfile.UserID = CType(gdvLocate.SelectedRow.FindControl("hidApproveBy"), HiddenField).Value

                    lblLocateApproveBy.Text = Client.GetUserProfile(UserProfile)(0).Name

                End If

                ' Get last serial no
                lblLocateLastSerialNo.Text = Client.GetLastSerialNo(Session(ESession.StoreID.ToString) _
                                                                    , ServiceModuleName.Request _
                                                                    )
                DetailsList = Client.GetRequestItem(Session(ESession.StoreID.ToString).ToString _
                                                    , lblLocateRequestID.Text _
                                                    )

                Client.Close()

            End Using


            ' reset placeholder
            DCPLocate.Controls.Clear()
            ViewState.Remove(EViewState.IssueItemLocate.ToString)
            ViewState(EViewState.RequestedStockItemIDLocate) = New List(Of String)
            For Each item In DetailsList
                ' keep a list of Requested stock item, to check against to prevent duplication
                DirectCast(ViewState(EViewState.RequestedStockItemIDLocate), List(Of String)).Add(item.StockItemID)

                ' add request/issue user control
                AddIssueItem(DCPLocate, item, EViewState.IssueItemLocate.ToString)
            Next

            ' disable all controls 1st
            pnlLocateAccess.Enabled = False
            ddlLocateDocType.Enabled = False
            txtLocateIssueDate.Enabled = False
            txtLocateSerialNo.Enabled = False
            pnlLocateSearchItem.Enabled = False
            btnLocateApprove.Enabled = False
            btnLocateReject.Enabled = False
            btnLocateSave.Enabled = False
            btnLocateCancel.Enabled = False

            trRequest.Visible = True
            trApprove.Visible = True
            Select Case CType(gdvLocate.SelectedRow.FindControl("hidStatus"), HiddenField).Value
                Case OPEN ' able to approve
                    trApprove.Visible = False

                    ' disallow approving when login user is the requester ' UAT02 - check and prompt user with AO rights only
                    If Session(ESession.UserRoleType.ToString).ToString.Contains(APPROVALOFFICER) Then
                        If Session(ESession.UserID.ToString).ToString = CType(gdvLocate.SelectedRow.FindControl("hidRequestBy"), HiddenField).Value Then
                            Message = GetMessage(messageID.OwnRequest)

                        Else
                            btnLocateApprove.Enabled = True
                            btnLocateReject.Enabled = True
                        End If
                    End If

                Case APPROVED ' able to issue
                    If ViewState(EViewState.IssueInsert) Then
                        ddlLocateDocType.Enabled = True
                        txtLocateIssueDate.Enabled = True
                        txtLocateSerialNo.Enabled = True
                        pnlLocateSearchItem.Enabled = True

                        btnLocateSave.Enabled = True
                        btnLocateCancel.Enabled = True

                    End If

                Case CLOSED ' able to edit/delete issued
                    ' UAT02 - Edit and Delete allow only when withinFinanceCutoffDate
                    If IsWithinFinanceCutoffDate(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), Session(ESession.StoreID.ToString), ConvertToDate(txtLocateIssueDate.Text)) Then
                        If ViewState(EViewState.IssueDelete) Then
                            pnlLocateAccess.Enabled = True
                        End If

                        If ViewState(EViewState.IssueUpdate) Then
                            pnlLocateAccess.Enabled = True
                        End If
                    End If

            End Select
            pnllocate.Visible = True
            uplLocateIssue.Update()

        Catch ex As FaultException
            Message = ex.Message

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    ''' <summary>
    ''' gdvLocateItem - Sorting
    ''' 09 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 17Mar09  KG  UAT02 - Cache with UserID as the prefix;
    ''' </remarks>
    Private Sub gdvLocate_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gdvLocate.Sorting
        Try
            If CacheListIsEmpty(Cache(Session(ESession.UserID.ToString) & ECache.RequestListSearch), GetType(RequestDetails)) Then GetRequestList(Cache(Session(ESession.UserID.ToString) & ECache.RequestListSearch), Session(ESession.StoreID.ToString), ddlLocateConsumerSearch.SelectedValue, txtLocateRequestID.Text, ddlLocateStatus.SelectedValue)
            '-- SORT THE CACHE CONTENT
            Dim Sorter As New clsSorter(Of RequestDetails)
            Sorter.SortString = e.SortExpression
            DirectCast(Cache(Session(ESession.UserID.ToString) & ECache.RequestListSearch), List(Of RequestDetails)).Sort(Sorter)

            gdvLocate.DataSource = Cache(Session(ESession.UserID.ToString) & ECache.RequestListSearch)
            gdvLocate.DataBind()

            pnlSearchResults.Visible = True

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

    Protected Sub btnLocateEdit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLocateEdit.Click
        ' Enable/Disable control for editing
        pnlLocateAccess.Enabled = False
        ddlLocateDocType.Enabled = True
        txtLocateIssueDate.Enabled = True
        txtLocateSerialNo.Enabled = True
        pnlLocateSearchItem.Enabled = True

        btnLocateSave.Enabled = True
        btnLocateCancel.Enabled = True
    End Sub

    Protected Sub btnLocateDeleteAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLocateDeleteAll.Click
        Try
            Using Client As New ServiceClient
                Client.DeleteAllStockTransaction(Session(ESession.StoreID.ToString).ToString _
                                                 , lblLocateRequestID.Text _
                                                 , ServiceModuleName.Request _
                                                 , ConvertToDate(txtLocateIssueDate.Text) _
                                                 , Session(ESession.UserID.ToString).ToString _
                                                 )
            End Using

            Message = GetMessage(messageID.Success, "deleted", String.Format("Store Request[{0}]", lblLocateRequestID.Text))

            ' Reset Control n Clear screen
            btnLocateClear_Click(sender, e)
        Catch ex As FaultException
            Message = ex.Message

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    Protected Sub btnLocateSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLocateSave.Click
        Try
            'save data to database
            Dim Request As RequestDetails
            Dim DetailsList As List(Of IssueItemDetails)
            Dim IssueDate As Date = ConvertToDate(txtLocateIssueDate.Text)
            Dim ApproveDate As Date = ConvertToDate(lblLocateApproveDate.Text)
            Dim IssueItemForDelete As Integer

            ' Request Details
            Request = New RequestDetails
            Request.StoreID = Session(ESession.StoreID.ToString).ToString
            Request.Type = ddlLocateDocType.SelectedValue
            Request.RequestID = lblLocateRequestID.Text
            Request.Status = "C"  ' Closed aka Issued
            Request.SerialNo = Trim(txtLocateSerialNo.Text)
            Request.IssueDte = ConvertToDate(txtLocateIssueDate.Text)
            Request.IssueBy = Session(ESession.UserID.ToString).ToString
            Request.LoginUser = Session(ESession.UserID.ToString).ToString

            ' validate mandatory fields
            If (Request.StoreID = EMPTY _
                Or Request.Type = EMPTY _
                Or Request.RequestID = EMPTY _
                Or Request.IssueDte = Date.MinValue _
                Or Request.LoginUser = EMPTY _
                ) Then
                Throw New ApplicationException(GetMessage(messageID.MandatoryField))
            End If

            '' validate date
            If (IssueDate > Today) Then Throw New ApplicationException(GetMessage(messageID.MoreLessThan, , , "Issue Date", Today.ToString("dd/MM/yyyy"), "earlier or same as"))
            If (IssueDate < ApproveDate) Then Throw New ApplicationException(GetMessage(messageID.MoreLessThan, , , "Issue Date", "Approve Date", "later or same as"))
            'If Not (IsWithinFinanceCutoffDate(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), Session(ESession.StoreID.ToString), IssueDate)) Then Throw New ApplicationException(GetMessage(messageID.NotInFinancial, "Issue Date"))

            Using Client As New ServiceClient
                ' validate serial no is unique
                If (Trim(txtLocateSerialNo.Text) <> EMPTY) Then
                    If Not (Client.FieldIsUnique(Session(ESession.StoreID.ToString) _
                                                 , ServiceColumnName.IssueSerialNo _
                                                 , Trim(txtLocateSerialNo.Text) _
                                                 , lblLocateRequestID.Text _
                                                 ) _
                            ) Then Throw New ApplicationException(GetMessage(messageID.FieldNotUnique, "Serial No"))
                End If
            End Using


            ViewState("_Locate") = "LOCATE"
            DetailsList = RetrieveDCPInfo(EViewState.IssueItemLocate.ToString)

            ''If DetailsList.Count > 0 Then ' UAT02 - don't check to allow closing of Request without Issue

            ' if all IssueItem is deem for deletion, invoke 'btnLocateDeleteAll' code instead
            IssueItemForDelete = DetailsList.FindAll(Function(i As IssueItemDetails) i.Mode = DELETE).Count
            If DirectCast(ViewState(EViewState.RequestedStockItemIDLocate), List(Of String)).Count = IssueItemForDelete Then
                btnLocateDeleteAll_Click(Nothing, Nothing)

            Else
                Using Client As New ServiceClient
                    Message = Client.UpdateIssueItem(Request _
                                                     , DetailsList _
                                                     )
                End Using

                If Message = EMPTY Then
                    Message = GetMessage(messageID.Success, "Saved", String.Format("Store Request[{0}]", Request.RequestID))

                    ' Add edited Request to list and rebind (if needed)
                    EditCache(Cache(Session(ESession.StoreID.ToString) & ECache.RequestList), GetType(RequestDetails), Request, True)
                    BindDropDownList(ddlIssueReference, Cache(Session(ESession.StoreID.ToString) & ECache.RequestList), "RequestID", "RequestID")
                    uplPrintRequestID.Update()

                    ' Reset Control n Clear screen
                    btnLocateClear_Click(sender, e)
                End If
            End If

        Catch ex As FaultException
            Message = ex.Message

        Catch ex As ApplicationException
            Message = ex.Message

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    Protected Sub btnLocateCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLocateCancel.Click
        ' Reload original info
        btnLocateGo_Click(sender, e)
    End Sub

    Protected Sub btnLocateApprove_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLocateApprove.Click
        UpdateRequestStatus(lblLocateRequestID.Text, APPROVED, "Approved")
    End Sub

    Protected Sub btnLocateReject_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLocateReject.Click
        UpdateRequestStatus(lblLocateRequestID.Text, REJECTED, "Rejected")
    End Sub

    Protected Sub ShowLocateModal_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ShowLocateModal.Click
        mpuLocateStockAvailability.Show()
    End Sub

    Protected Sub btnLocateExit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLocateExit.Click
        ShowLocateModal.Visible = False
    End Sub
#End Region

#Region " Print "
    Protected Sub btnPDF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPDF.Click

        If Not ValidPrint() Then
            Exit Sub
        End If

        '-- VALIDATE STOCK CODE
        Dim StockCodeFrom, StockCodeTo As String
        StockCodeFrom = Split(txtStockCodeFrom.Text, " | ")(0).Trim
        StockCodeTo = Split(txtStockCodeTo.Text, " | ")(0).Trim

        Using Client As New ServiceClient

            Dim ItemDetails As New ItemDetails
            ItemDetails.StoreID = Session("StoreID").ToString
            ItemDetails.ItemID = StockCodeFrom

            If Not Client.IsValidStockCode(ItemDetails) And ddlIssueReference.SelectedIndex = 0 Then

                Message = GetMessage(messageID.InvalidStockCode, StockCodeFrom.ToUpper)
                Client.Close()
                Exit Sub

            End If
            Client.Close()

        End Using

        rvr.LocalReport.DataSources.Clear()
        rvr.LocalReport.DataSources.Add(New ReportDataSource("IssueList", ObjectDataSource1))

        Dim parameterlist As New List(Of ReportParameter)
        '' UAT02 - cannot accomodate requester info in this report
        'Dim p1 As New ReportParameter("Status", ddlPrintOption.SelectedValue)
        Dim p1 As New ReportParameter("PODateFrom", txtIssueDateFrom.Text)
        Dim p2 As New ReportParameter("PODateTo", txtIssueDateTo.Text)
        Dim p3 As New ReportParameter("StockCodeFrom", StockCodeFrom)  'ddlStockCodeFrom.SelectedValue)
        Dim p4 As New ReportParameter("StockCodeTo", StockCodeTo) 'ddlStockCode2.SelectedValue)
        Dim p5 As New ReportParameter("DocNo", ddlIssueReference.SelectedValue)
        Dim p6 As New ReportParameter("StoreName", Session(ESession.StoreName.ToString).ToString)
        Dim p7 As New ReportParameter("ConsumerID", ddlPrintConsumerID.SelectedValue)
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

        'If NoRecordFond = "Y" Then
        '    Message = GetMessage(messageID.NoRecordFound)
        '    Exit Sub
        'End If

        Response.Buffer = True
        Response.Clear()
        Response.ContentType = "application/pdf"
        Response.AddHeader("content-disposition", "attachment;filename=IssueList.pdf")
        Response.BinaryWrite(bytValue)
        Response.Flush()
        Response.End()

    End Sub

    Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExcel.Click
        If Not ValidPrint() Then
            Exit Sub
        End If

        '-- VALIDATE STOCK CODE
        Dim StockCodeFrom, StockCodeTo As String
        StockCodeFrom = Split(txtStockCodeFrom.Text, " | ")(0).Trim
        StockCodeTo = Split(txtStockCodeTo.Text, " | ")(0).Trim

        Using Client As New ServiceClient

            Dim ItemDetails As New ItemDetails
            ItemDetails.StoreID = Session("StoreID").ToString
            ItemDetails.ItemID = StockCodeFrom

            If Not Client.IsValidStockCode(ItemDetails) And ddlIssueReference.SelectedIndex = 0 Then

                Message = GetMessage(messageID.InvalidStockCode, StockCodeFrom.ToUpper)
                Client.Close()
                Exit Sub

            End If

            Client.Close()
        End Using

        rvr.LocalReport.DataSources.Clear()
        rvr.LocalReport.DataSources.Add(New ReportDataSource("IssueList", ObjectDataSource1))

        Dim parameterlist As New List(Of ReportParameter)
        '' UAT02 - cannot accomodate requester info in this report
        'Dim p1 As New ReportParameter("Status", ddlPrintOption.SelectedValue)
        Dim p1 As New ReportParameter("PODateFrom", txtIssueDateFrom.Text)
        Dim p2 As New ReportParameter("PODateTo", txtIssueDateTo.Text)
        Dim p3 As New ReportParameter("StockCodeFrom", StockCodeFrom) 'ddlStockCodeFrom.SelectedValue)
        Dim p4 As New ReportParameter("StockCodeTo", StockCodeTo) 'ddlStockCode2.SelectedValue)
        Dim p5 As New ReportParameter("DocNo", ddlIssueReference.SelectedValue)
        Dim p6 As New ReportParameter("StoreName", Session(ESession.StoreName.ToString).ToString)
        Dim p7 As New ReportParameter("ConsumerID", ddlPrintConsumerID.SelectedValue)

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

        'If NoRecordFond = "Y" Then
        '    Message = GetMessage(messageID.NoRecordFound)
        '    Exit Sub
        'End If

        Response.Buffer = True
        Response.Clear()
        Response.ContentType = "application/ms-excel"
        Response.AddHeader("content-disposition", "attachment;filename=IssueList.xls")
        Response.BinaryWrite(bytValue)
        Response.Flush()
        Response.End()

    End Sub

    Protected Sub ObjectDataSource1_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs) Handles ObjectDataSource1.Selecting
        e.InputParameters("storeID") = Session(ESession.StoreID.ToString).ToString
        e.InputParameters("fromDte") = DateTime.ParseExact(Me.txtIssueDateFrom.Text, "dd/MM/yyyy", Nothing)
        e.InputParameters("toDte") = DateTime.ParseExact(Me.txtIssueDateTo.Text, "dd/MM/yyyy", Nothing)
        e.InputParameters("fromStockItemID") = Split(txtStockCodeFrom.Text, " | ")(0).Trim.ToUpper 'ddlStockCodeFrom.SelectedValue
        e.InputParameters("toStockItemID") = Split(txtStockCodeTo.Text, " | ")(0).Trim.ToUpper 'ddlStockCode2.SelectedValue
        e.InputParameters("requestId") = ddlIssueReference.SelectedValue
        e.InputParameters("consumerId") = ddlPrintConsumerID.SelectedValue
    End Sub

    Protected Sub ObjectDataSource1_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles ObjectDataSource1.Selected
        Dim returnList As List(Of IssueList) = e.ReturnValue
        If returnList.Count <= 0 Then
            NoRecordFond = "Y"
        Else
            NoRecordFond = "N"
        End If
    End Sub

    Private Function ValidPrint() As Boolean
        ' validate filtering criteria
        If ddlIssueReference.SelectedValue = EMPTY Then
            If (Trim(txtIssueDateFrom.Text) = EMPTY _
                Or Trim(txtIssueDateTo.Text) = EMPTY _
                Or txtStockCodeFrom.Text = EMPTY _
                ) Then
                Message = GetMessage(messageID.MandatoryField)
                Exit Function
            End If

            If ConvertToDate(txtIssueDateFrom.Text) > ConvertToDate(txtIssueDateTo.Text) Then
                Message = GetMessage(messageID.DateToEarlierDateFrom)
                Exit Function
            End If

            If Split(txtStockCodeFrom.Text, " | ")(0).Trim.ToUpper > Split(txtStockCodeTo.Text, " | ")(0).Trim.ToUpper Then
                Message = GetMessage(messageID.StockCodeToEarlierStockCodeFrom)
                Exit Function
            End If
        End If

        Return True
    End Function

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClear.Click
        ''ddlPrintOption.SelectedIndex = -1
        txtIssueDateFrom.Text = Today.ToString("01/MM/yyyy")
        txtIssueDateTo.Text = Today.ToString("dd/MM/yyyy")
        'ddlStockCodeFrom.SelectedIndex = -1
        'ddlStockCode2.SelectedIndex = -1
        txtStockCodeFrom.Text = String.Empty
        txtStockCodeTo.Text = String.Empty
        ddlIssueReference.SelectedIndex = -1
    End Sub

#End Region

#Region " Sub Procedures and Functions "
    ''' <summary>
    ''' Manage the Main and details screen display;
    ''' To enable/disable UI controls for Edit
    ''' 26 Jan 09 - KG;
    ''' </summary>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Private Sub MainPanel(ByVal enabled As Boolean)
        Try
            'Select Case tbcIssueItem.ActiveTab.ID
            'Case NEWTAB
            ddlConsumerID.Enabled = enabled
            ddlDocType.Enabled = enabled
            cbxSought.Enabled = enabled
            divAdd.Visible = enabled
            uplMain.Update()

            pnlNewIssue.Visible = (Not enabled)
            uplDetail.Update()

            'Case LOCATETAB

            'End Select

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Add the IssueItem User Control in the PlaceHolder;
    ''' Issue Qty is stored and retrieve as negative value, convert to positive for display;
    ''' 12Feb09 - KG;
    ''' </summary>
    ''' <param name="PlaceHolder">position</param>
    ''' <param name="issueItemDetails">request/issue info</param>
    ''' <param name="UserControlName"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Private Sub AddIssueItem(ByRef PlaceHolder As DynamicControlsPlaceholder, ByVal issueItemDetails As IssueItemDetails, ByVal UserControlName As String)
        Try
            Dim IssueItem = New IssueItem
            Dim MoreItemInfo = New MoreItemInfoDetails
            Dim Client As New ServiceClient

            Dim ItemDetails As ItemDetails = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), List(Of ItemDetails)).Find(Function(i As ItemDetails) i.ItemID = issueItemDetails.StockItemID)

            'If Stock Item is not found in the Item List Cache then get the said Stock Item from the DB.
            'The Item List Cache only contains Stock Items that are Open.
            If ItemDetails Is Nothing Then
                Dim ItemSearch As New ItemDetails
                ItemSearch.StoreID = Session(ESession.StoreID.ToString)
                ItemSearch.ItemID = issueItemDetails.StockItemID
                ItemSearch.Location = EMPTY
                ItemSearch.Status = EMPTY

                Dim ItemSearchResult As List(Of ItemDetails)
                ItemSearchResult = Client.GetItems(ItemSearch, "StockItemID", EMPTY)

                ItemDetails = ItemSearchResult.First()
            End If

            MoreItemInfo = Client.GetMoreItemInfo(Session(ESession.StoreID.ToString).ToString, ItemDetails.ItemID, Today)
            Client.Close()

            ' increment user control ID
            If IsNothing(ViewState(UserControlName)) Then
                ViewState(UserControlName) = 1
            Else
                ViewState(UserControlName) += 1
            End If

            ' load user control at top of placeholder
            IssueItem = LoadControl("IssueItem.ascx")
            IssueItem.ID = UserControlName + Convert.ToString(ViewState(UserControlName))

            ' Need to add control to page before assigning value to the control
            PlaceHolder.Controls.AddAt(0, IssueItem)

            ' clear viewstate info
            ViewState.Remove(IssueItem.UniqueID)

            ' Assign value to controls
            DirectCast(IssueItem.FindControl("hfMode"), HiddenField).Value = issueItemDetails.Mode
            DirectCast(IssueItem.FindControl("hfTranID"), HiddenField).Value = issueItemDetails.TranID
            DirectCast(IssueItem.FindControl("hfRequestItemID"), HiddenField).Value = issueItemDetails.RequestItemID
            DirectCast(IssueItem.FindControl("hfRequestItemStatus"), HiddenField).Value = issueItemDetails.RequestItemStatus
            DirectCast(IssueItem.FindControl("hfBalanceQty"), HiddenField).Value = MoreItemInfo.Balance 'issueItemDetails.BalanceQty

            DirectCast(IssueItem.FindControl("hfOrgIssueQty"), HiddenField).Value = issueItemDetails.Qty
            DirectCast(IssueItem.FindControl("hfOrgRemarks"), HiddenField).Value = issueItemDetails.Remarks

            DirectCast(IssueItem.FindControl("lblStockCode"), Label).Text = issueItemDetails.StockItemID
            DirectCast(IssueItem.FindControl("lblDescription"), Label).Text = ItemDetails.ItemDescription
            DirectCast(IssueItem.FindControl("lblUOM"), Label).Text = ItemDetails.UOM
            DirectCast(IssueItem.FindControl("lblUOM2"), Label).Text = ItemDetails.UOM

            DirectCast(IssueItem.FindControl("txtRequestQty"), TextBox).Text = IIf(issueItemDetails.RequestItemQty > 0, issueItemDetails.RequestItemQty.ToString("0.00"), 0.0)
            DirectCast(IssueItem.FindControl("txtIssueQty"), TextBox).Text = IIf(issueItemDetails.Qty <> 0, (-issueItemDetails.Qty).ToString("0.00"), (-issueItemDetails.RequestItemQty).ToString("0.00"))
            DirectCast(IssueItem.FindControl("txtRemarks"), TextBox).Text = issueItemDetails.Remarks
            ' ''  Client Side Java Scripting
            ''DirectCast(IssueItem.FindControl("txtReceiveQty"), TextBox).Attributes.Add("onkeyup", "computeTotal('" & DirectCast(IssueItem.FindControl("txtReceiveQty"), TextBox).ClientID & "','" & DirectCast(IssueItem.FindControl("hfUnitCost"), HiddenField).ClientID & "','" & DirectCast(IssueItem.FindControl("lblTotalCost"), Label).ClientID & "');")
            ' ''  truncate value to 4 decimal place for display
            ''DirectCast(IssueItem.FindControl("lblUnitCost"), Label).Text = DisplayValue(issueItemDetails.)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Used by the User Control IssueItem to display its Stock Item info;
    ''' 18 Feb 2009 - KG;
    ''' </summary>
    ''' <param name="uniqueID"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Friend Sub ViewStock(ByVal uniqueID As String, ByVal stockItemID As String, ByVal stockItemDesc As String, ByVal stockItemUOM As String, ByVal issueQtyDiff As Decimal)
        Try
            ' Retrieve and viewstate more Info for the Stock Item. AUC, Balance, etc
            If ViewState(uniqueID) Is Nothing Then
                Using Client As New ServiceClient
                    ViewState(uniqueID) = Client.GetMoreItemInfo(Session(ESession.StoreID.ToString).ToString _
                                                                 , stockItemID _
                                                                 , Today _
                                                                 )
                End Using
            End If

            'get info from viewstate
            Dim MoreItem As MoreItemInfoDetails = DirectCast(ViewState(uniqueID), MoreItemInfoDetails)
            Dim UOM As String = " " + stockItemUOM
            Dim BalanceNew As Decimal = MoreItem.Balance - issueQtyDiff
            Dim TotalValueNew As Double = BalanceNew * MoreItem.AvgUnitCost

            If tbcIssueItem.ActiveTab.ID = NEWTAB Then

                If Not ViewState("_Locate") Is Nothing And ViewState("_Locate") = "LOCATE" Then

                    ViewLocateStockCode.Text = MoreItem.ItemID
                    ViewLocateDesc.Text = stockItemDesc
                    ViewLocateBalance.Text = MoreItem.Balance.ToString("0.00") + UOM
                    ViewLocateAUC.Text = "$ " + DisplayValue(MoreItem.AvgUnitCost)
                    ViewLocateTotalValue.Text = "$ " + DisplayValue(MoreItem.TotalValue)

                    If lblLocateStatus.Text.ToUpper = "APPROVED" Or lblLocateStatus.Text.ToUpper = "OPEN" Then
                        ViewLocateBalanceNew.Text = BalanceNew.ToString("0.00") + UOM
                        ViewLocateAUCNew.Text = "$ " + DisplayValue(MoreItem.AvgUnitCost)
                        ViewLocateTotalValueNew.Text = "$ " + DisplayValue(TotalValueNew)
                    Else
                        ViewLocateBalanceNew.Text = "Already been issued"
                        ViewLocateAUCNew.Text = String.Empty
                        ViewLocateTotalValueNew.Text = String.Empty
                    End If

                    ShowLocateModal.Visible = True
                    ShowLocateModal_Click(ShowModal, New EventArgs)
                    uplLocateIssue.Update()
                Else

                    ViewStockCode.Text = MoreItem.ItemID
                    ViewDesc.Text = stockItemDesc
                    ViewBalance.Text = MoreItem.Balance.ToString("0.00") + UOM
                    ViewAUC.Text = "$ " + DisplayValue(MoreItem.AvgUnitCost)
                    ViewTotalValue.Text = "$ " + DisplayValue(MoreItem.TotalValue)

                    ''ViewBalanceNew.Text = BalanceNew.ToString("0.00") + UOM
                    ''ViewAUCNew.Text = "$ " + DisplayValue(MoreItem.AvgUnitCost)
                    ''ViewTotalValueNew.Text = "$ " + DisplayValue(TotalValueNew)

                    ShowModal.Visible = True
                    ShowModal_Click(ShowModal, New EventArgs)
                    uplUserControl.Update()
                End If


            ElseIf tbcIssueItem.ActiveTab.ID = LOCATETAB Then
                ViewLocateStockCode.Text = MoreItem.ItemID
                ViewLocateDesc.Text = stockItemDesc
                ViewLocateBalance.Text = MoreItem.Balance.ToString("0.00") + UOM
                ViewLocateAUC.Text = "$ " + DisplayValue(MoreItem.AvgUnitCost)
                ViewLocateTotalValue.Text = "$ " + DisplayValue(MoreItem.TotalValue)

                If lblLocateStatus.Text.ToUpper = "APPROVED" Or lblLocateStatus.Text.ToUpper = "OPEN" Then
                    ViewLocateBalanceNew.Text = BalanceNew.ToString("0.00") + UOM
                    ViewLocateAUCNew.Text = "$ " + DisplayValue(MoreItem.AvgUnitCost)
                    ViewLocateTotalValueNew.Text = "$ " + DisplayValue(TotalValueNew)
                Else
                    ViewLocateBalanceNew.Text = "Already been Issued"
                    ViewLocateAUCNew.Text = String.Empty
                    ViewLocateTotalValueNew.Text = String.Empty
                End If

                ShowLocateModal.Visible = True
                ShowLocateModal_Click(ShowModal, New EventArgs)
                uplLocateIssue.Update()
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Friend Sub CancelIssueItem(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim issueItem As New IssueItem
            issueItem = DirectCast(FindControl(sender), IssueItem)
            If tbcIssueItem.ActiveTab.ID = NEWTAB Then
                DCP.Controls.Remove(FindControl(sender))
                uplUserControl.Update()

                ''    ' Locate TAB currently cannot delete
                ''ElseIf tbcIssueItem.ActiveTab.ID = LOCATETAB Then
                ''    'TODO: Check this deletion doesn't cause a negative value to the stockitem
                ''    If True Then
                ''        If DirectCast(issueItem.FindControl("hfMode"), HiddenField).Value = INSERT Then
                ''            DCPLocate.Controls.Remove(FindControl(sender))
                ''        Else
                ''            DirectCast(issueItem.FindControl("hfMode"), HiddenField).Value = DELETE
                ''            issueItem.Visible = False
                ''        End If
                ''    Else
                ''        Message = "This action will cause the Stock Item Qty to fall below Zero value."
                ''    End If
            End If

            ' clear viewstate info
            ViewState.Remove(sender)

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    ''' <summary>
    ''' RetrieveDCPInfo;
    ''' Retrieve the request/issue information from DCP and return as a list of issue item details;
    ''' 1)Collect request/issue information when Mode is NOT empty, also check qty is a valid number;
    ''' 2)Issue Qty is display as positive value, convert to negative for storage;
    ''' 25Feb09 - KG;
    ''' </summary>
    ''' <param name="UserControlName"></param>
    ''' <returns>list of issueitemdetails</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' 15Apr09  KG          UAT03  handle empty IssueItem;
    ''' </remarks>
    Protected Function RetrieveDCPInfo(ByVal UserControlName As String) As List(Of IssueItemDetails)
        Dim IssueItem As IssueItem
        Dim DetailsItem As IssueItemDetails
        Dim DetailsList As List(Of IssueItemDetails)

        ' Request/issue item Details
        DetailsList = New List(Of IssueItemDetails)
        For i As Integer = 1 To ViewState(UserControlName)
            If tbcIssueItem.ActiveTab.ID = NEWTAB Then

                If Not ViewState("_Locate") Is Nothing And ViewState("_Locate") = "LOCATE" Then
                    IssueItem = TryCast(DCPLocate.FindControl(UserControlName + i.ToString), IssueItem)
                Else
                    IssueItem = TryCast(DCP.FindControl(UserControlName + i.ToString), IssueItem)
                End If

            ElseIf tbcIssueItem.ActiveTab.ID = LOCATETAB Then
                IssueItem = TryCast(DCPLocate.FindControl(UserControlName + i.ToString), IssueItem)
            Else

                Throw New Exception("something wrong?")

            End If

            DetailsItem = New IssueItemDetails
            If IssueItem IsNot Nothing Then
                Dim Value As String

                'UAT03 - bring check within the IF block
                ' capture user control exception
                If IssueItem.Message <> EMPTY Then Throw New ApplicationException(IssueItem.Message)

                DetailsItem.TranID = CInt(DirectCast(IssueItem.FindControl("hfTranID"), HiddenField).Value)

                DetailsItem.StockItemID = DirectCast(IssueItem.FindControl("lblStockCode"), Label).Text
                DetailsItem.RequestItemID = DirectCast(IssueItem.FindControl("hfRequestItemID"), HiddenField).Value

                Value = Trim(DirectCast(IssueItem.FindControl("txtRequestQty"), TextBox).Text)
                If IsNumeric(Value) Then DetailsItem.RequestItemQty = CDec(Value)

                Value = Trim(DirectCast(IssueItem.FindControl("txtIssueQty"), TextBox).Text)
                If IsNumeric(Value) Then DetailsItem.Qty = CDec(-Value)

                ' RequestItemID = 0 means not request yet
                If DetailsItem.RequestItemID = 0 Then
                    ' check request qty is entered
                    If DetailsItem.RequestItemQty = 0 Then Throw New ApplicationException(String.Format("[{0}] {1}", DetailsItem.StockItemID, GetMessage(messageID.MandatoryField)))
                    DetailsItem.RequestItemStatus = OPEN    ' open for approval or rejection
                    DetailsItem.Mode = INSERT ' default mode to insert for request creation

                Else
                    ' Set Item Mode
                    DetailsItem.Mode = GetItemMode(DetailsItem.TranID _
                                                   , DetailsItem.Qty _
                                                   , CDec(DirectCast(IssueItem.FindControl("hfOrgIssueQty"), HiddenField).Value) _
                                                   , Trim(DirectCast(IssueItem.FindControl("txtRemarks"), TextBox).Text) _
                                                   , DirectCast(IssueItem.FindControl("hfOrgRemarks"), HiddenField).Value _
                                                   )
                    ' check issue qty is with entry
                    If Trim(DirectCast(IssueItem.FindControl("txtIssueQty"), TextBox).Text) = EMPTY Then
                        Throw New ApplicationException(String.Format("[{0}] Indicate as [0] to close the request WITHOUT issue.", DetailsItem.StockItemID))
                    End If

                    ' check issue qty is entered when mode is Insert or Update
                    If DetailsItem.Mode = INSERT Or DetailsItem.Mode = UPDATE Then
                        If DetailsItem.Qty = 0 Then Throw New ApplicationException(String.Format("[{0}] {1}", DetailsItem.StockItemID, GetMessage(messageID.MandatoryField)))
                    End If

                    DetailsItem.Remarks = Trim(DirectCast(IssueItem.FindControl("txtRemarks"), TextBox).Text)
                    DetailsItem.Status = OPEN ' transaction will be system closed on monthly financial cutoff date
                End If

                ' Process those with mode ONLY
                If DetailsItem.Mode <> EMPTY Then
                    DetailsList.Add(DetailsItem)
                End If
            End If
        Next

        Return DetailsList
    End Function

    Private Sub UpdateRequestStatus(ByVal requestID As String, ByVal status As String, ByVal messageType As String)
        Try
            'update status to database
            Using Client As New ServiceClient
                Client.UpdateRequestStatus(Session(ESession.StoreID.ToString) _
                                           , requestID _
                                           , status _
                                           , Session(ESession.UserID.ToString) _
                                           )
            End Using

            Message = GetMessage(messageID.Success, messageType, String.Format("Store Request[{0}]", requestID))

            ' Reset Control n Clear screen
            btnLocateClear_Click(Nothing, Nothing)
        Catch ex As FaultException
            Message = ex.Message

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try

    End Sub

    ''' <summary>
    ''' GetItemMode;
    ''' Determine the Item Mode by using the various parameters
    ''' 24Feb09 - KG;
    ''' </summary>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Private Function GetItemMode(ByVal hfTranID As Integer, ByVal IssueQty As String, ByVal hfOrgIssueQty As String, ByVal txtRemarks As String, ByVal hfOrgRemarks As String) As String
        ' TranID = 0 means not issue yet
        ' use the issue qty to determine its mode
        If hfTranID = 0 Then
            If IssueQty = 0D Then
                Return EMPTY

            Else
                ' Insert only when there is issue qty
                Return INSERT
            End If

        Else
            ' delete when the issue qty is set to zero
            If IssueQty = 0D Then
                Return DELETE

            Else
                ' Update when screen value diff from original value
                If (hfOrgIssueQty <> IssueQty Or hfOrgRemarks <> Trim(txtRemarks)) Then
                    Return UPDATE

                Else
                    Return EMPTY
                End If
            End If
        End If

        Return EMPTY

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
    <WebMethod()> _
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

            ItemList = Client.GetItemSearch(ItemSearch) 'Client.GetItems(ItemSearch, "StockItemID", "ASC")

            Client.Close()

        Catch ex As Exception
            Throw
        End Try

        Return ItemList

    End Function

#End Region

End Class