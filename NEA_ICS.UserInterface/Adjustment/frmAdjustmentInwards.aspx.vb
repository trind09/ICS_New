Imports Microsoft.Reporting.WebForms
Imports System.Web.Services
Imports System.Web.Services.Description
Imports NEA_ICS.UserInterface.NEA_ICS.WcfService
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ServiceModel
Imports DBauer.Web.UI.WebControls
Imports System.Reflection

''' <summary>
''' Code behind for frmAdjustmentInwards Page;
''' 29Feb09 - KG;
''' </summary>
''' <remarks>
''' CHANGE LOG:
''' ddMMMyy  AuthorName  RefID  Description;
''' </remarks>
Partial Public Class frmAdjustmentInwards
    Inherits clsCommonFunction

#Region " Page Control "
    Private Message As String = EMPTY
    Private AdjustItem As AdjustItem
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

    ''' <summary>
    ''' Page_Load;
    ''' 1)Assign controls to user base on Access Rights;
    ''' 2)Retrieve all lists;
    ''' 3)Bind dropdownlists with Lists;
    ''' 29Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CheckValidSession()

        Try
            If Not Page.IsPostBack Then

                aceStockCode.ContextKey = Session("StoreID").ToString
                aceLocateStockCode.ContextKey = Session("StoreID").ToString

                ' @@@ START OF ACCESS RIGHTS @@@
                Dim AccessRights As New List(Of RoleDetails)

                tbcAdjustItem.Visible = False
                AccessRights = clsCommonFunction.AssignAccessRights(Cache(Session(ESession.UserID.ToString) & Session(ESession.StoreID.ToString) & "AccessRights") _
                                                                    , moduleID.AdjustmentInwards _
                                                                    )
                ' Select Rights
                tbpLocate.Visible = False
                tbpPrint.Visible = False
                If AccessRights(0).SelectRight Then
                    tbpLocate.Visible = True
                    tbpPrint.Visible = True
                    tbcAdjustItem.Visible = True
                    tbpLocate.Focus()

                Else
                    Server.Transfer("frmUnauthorisedPage.aspx")
                    Exit Sub
                End If


                ' Insert Rights
                tbpNew.Visible = False
                If AccessRights(0).InsertRight Then
                    tbpNew.Visible = True
                    tbcAdjustItem.ActiveTabIndex = 0
                    MainPanel(True)
                    tbpNew.Focus()
                End If


                ' Update or Delete Rights
                pnlLocateAccess.Visible = False
                btnLocateEdit.Visible = False
                btnLocateDeleteAll.Visible = False
                pnlLocateAction.Visible = False

                '  Update Rights
                If AccessRights(0).UpdateRight Then
                    pnlLocateAccess.Visible = True
                    btnLocateEdit.Visible = True
                    pnlLocateAction.Visible = True
                End If

                '  Delete Rights
                If AccessRights(0).DeleteRight Then
                    pnlLocateAccess.Visible = True
                    btnLocateDeleteAll.Visible = True
                End If
                ' @@@ END OF ACCESS RIGHTS @@@


                ' retrieve listing to be bind to ddl
                If CacheListIsEmpty(Cache(Session(ESession.StoreID.ToString) & ECache.AdjustInList), GetType(AdjustDetails)) Then GetAdjustList(Cache(Session(ESession.StoreID.ToString) & ECache.AdjustInList), Session(ESession.StoreID.ToString), ADJUSTIN, ALL)
                If CacheListIsEmpty(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), GetType(ItemDetails)) Then GetItemList(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), Session(ESession.StoreID.ToString))


                ' New Tab
                BindDropDownList(ddlDocType, GetCommonDataByCodeGroup(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), codeGroup.InwardsDocType, OPEN), "CommonCodeID", "CommonCodeDescription")
                txtAdjustDte.Text = Today.ToString("dd/MM/yyyy")
                ddlRequestID.Items.Clear()

                '   to allow Exit button click on Modal to invoke page postback
                btnExit.OnClientClick = String.Format("postbackFromJS('{0}', '{1}')", btnExit.UniqueID, String.Empty)

                ' '' NOT REQUIRED FOR INWARD
                ' '' Get last serial no
                ''Using Client = New ServiceClient
                ''lblLastSerialNo.Text = Client.GetLastSerialNo(Session(ESession.StoreID.ToString) _
                ''                                                    , ServiceModuleName.AdjustIn _
                ''                                                    )
                ''End Using


                ' Locate Tab
                BindDropDownList(ddlLocateAdjustID, Cache(Session(ESession.StoreID.ToString) & ECache.AdjustInList), "AdjustID", "AdjustID")
                '   to allow Exit button click on Modal to invoke page postback
                btnLocateExit.OnClientClick = String.Format("postbackFromJS('{0}', '{1}')", btnLocateExit.UniqueID, String.Empty)

                pnlLocateAdjust.Enabled = False
                pnlLocateAdjustItem.Enabled = False

                ' Report Tab 
                btnClear_Click(Nothing, Nothing)
                BindDropDownList(ddlRptDocumentNo, Cache(Session(ESession.StoreID.ToString) & ECache.AdjustInList), "AdjustID", "AdjustID")

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
    ''' 29Feb09 - KG;
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
            ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", "alert('" & Message & "');", True)
        End If
    End Sub

#End Region

#Region " New Tab "
    Private Function ValidAdjust() As Boolean
        Try
            Dim AdjustDte As Date

            AdjustDte = ConvertToDate(txtAdjustDte.Text)

            ' mandatory field check
            If (ddlDocType.SelectedValue = EMPTY _
                Or Trim(txtAdjustDte.Text) = EMPTY _
                ) Then
                Throw New ApplicationException(GetMessage(messageID.MandatoryField))
            End If

            ' validate date
            If AdjustDte = DateTime.MinValue Then
                Throw New ApplicationException(GetMessage(messageID.NotIsDate, "Adjust Date"))
            End If
            If AdjustDte > Today Then
                Throw New ApplicationException(GetMessage(messageID.MoreLessThan, , , "Adjustment Date", Today.ToString("dd/MM/yyyy"), "earlier or same as"))
            End If
            If Not (IsWithinFinanceCutoffDate(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), Session(ESession.StoreID.ToString), AdjustDte)) Then
                Throw New ApplicationException(GetMessage(messageID.NotInFinancial, "Adjustment Date"))
            End If


            Using Client As New ServiceClient
                ' validate serial no is unique
                If Trim(txtSerialNo.Text) <> EMPTY Then
                    If Not (Client.FieldIsUnique(Session(ESession.StoreID.ToString) _
                                                 , ServiceColumnName.AdjustSerialNo _
                                                 , Trim(txtSerialNo.Text) _
                                                 , EMPTY _
                                                 ) _
                            ) Then Throw New ApplicationException(GetMessage(messageID.FieldNotUnique, "Serial No"))
                End If
            End Using

            ' validate Adjust ID is unique
            If Trim(txtAdjustID.Text) <> EMPTY Then
                Using Client As New ServiceClient
                    If Not (Client.FieldIsUnique(Session(ESession.StoreID.ToString) _
                                                 , ServiceColumnName.AdjustInID _
                                                 , Trim(txtAdjustID.Text) _
                                                 , EMPTY _
                                                 ) _
                            ) Then
                        Throw New ApplicationException(String.Format("Adjust reference:({0}) exists in the system", Trim(txtAdjustID.Text)))
                    End If
                End Using
            End If

            Return True
        Catch ex As ApplicationException
            Message = ex.Message
            Return False

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub btnAddAdjustItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddAdjustItem.Click
        Try
            If ValidAdjust() Then
                If trRequestID.Visible = False Then
                    ViewState(EViewState.AdjustedStockItemID) = New List(Of String)

                    'btnAddItem_Click(sender, e) [NOT IN USE]
                    'If CacheListIsEmpty(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), GetType(ItemDetails)) Then GetItemList(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), Session(ESession.StoreID.ToString))
                    'BindDropDownList(ddlStockCode, Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), "ItemID", "ItemID_Description")

                    ' Show Stock Code Item List for selection
                    uplStockCode.Visible = True

                Else
                    ' load all the selected Issue reference Item for processing
                    If ddlRequestID.SelectedValue = EMPTY Then Throw New ApplicationException(GetMessage(messageID.MandatoryField))
                    LoadReturnItem(DCP _
                                   , EViewState.AdjustItem.ToString _
                                   , ddlDocType.SelectedValue _
                                   , ddlRequestID.SelectedValue _
                                   )

                    ' Hide Stock Code Item List from selection
                    uplStockCode.Visible = False
                End If

                MainPanel(False)
            End If

        Catch ex As ApplicationException
            Message = ex.Message

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub

    Protected Sub btnAddItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddItem.Click
        Try
            Dim MoreItemInfo As MoreItemInfoDetails

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

                Dim DetailsItem As New AdjustItemDetails

                Dim Adjusted As Boolean = DirectCast(ViewState(EViewState.AdjustedStockItemID), List(Of String)).Exists(Function(i As String) (i = StockCode))

                If Adjusted Then
                    Throw New ApplicationException(GetMessage(messageID.StockCodeHasOrdered, StockCode))
                End If

                DetailsItem.StockItemID = StockCode

                Using Client As New ServiceClient
                    MoreItemInfo = Client.GetMoreItemInfo(Session(ESession.StoreID.ToString) _
                                                          , StockCode _
                                                          , Today _
                                                          )
                    DetailsItem.BalanceQty = MoreItemInfo.Balance
                    DetailsItem.MaxLevel = MoreItemInfo.MaxLevel
                End Using

                ' UAT02 - set mode as Insert
                DetailsItem.Mode = INSERT

                AddAdjustItem(DCP _
                              , DetailsItem _
                              , EViewState.AdjustItem.ToString _
                              , ddlDocType.SelectedValue _
                              )

                ' keep a list of Adjusted stock item, to check against to restrict duplicate
                DirectCast(ViewState(EViewState.AdjustedStockItemID), List(Of String)).Add(DetailsItem.StockItemID)

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

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Try
            'save data to database
            Dim Adjust As AdjustDetails
            Dim DetailsList As List(Of AdjustItemDetails)
            Dim AdjustDte As Date = ConvertToDate(txtAdjustDte.Text)
            Dim AdjustIDReturn As String = EMPTY


            ' Adjust Details
            Adjust = New AdjustDetails
            Adjust.StoreID = Session(ESession.StoreID.ToString).ToString
            Adjust.AdjustID = Trim(txtAdjustID.Text)
            Adjust.Type = ddlDocType.SelectedValue
            Adjust.SerialNo = Trim(txtSerialNo.Text)
            Adjust.Dte = ConvertToDate(txtAdjustDte.Text)
            Adjust.Status = CLOSED  'all adjustment is 'Closed'
            Adjust.LoginUser = Session(ESession.UserID.ToString).ToString


            ' validate mandatory fields; when AdjustID not entered, system will generates a unique number instead
            If (Adjust.StoreID = EMPTY _
                Or Adjust.Type = EMPTY _
                Or Adjust.Dte = Date.MinValue _
                Or Adjust.LoginUser = EMPTY _
                ) Then
                Throw New ApplicationException(GetMessage(messageID.MandatoryField))
            End If

            ' validate again before proceeding to save record
            If ValidAdjust() Then
                ' check list count
                If Adjust.Type = RETURNED Then
                    Adjust.DocReturn = ddlRequestID.SelectedValue
                    Adjust.InvolveID = lblConsumerID.Text
                    DetailsList = RetrieveReturnDCPInfo(EViewState.AdjustItem.ToString, Adjust.Type)

                Else
                    DetailsList = RetrieveDCPInfo(EViewState.AdjustItem.ToString, Adjust.Type)
                End If
                If DetailsList.Count > 0 Then
                    Using Client As New ServiceClient
                        AdjustIDReturn = Client.AddAdjust(Adjust _
                                                          , DetailsList _
                                                          )
                    End Using

                    ' Assign the AdjustID for adding new item
                    Adjust.AdjustID = AdjustIDReturn

                    ''If Message = EMPTY Then
                    Message = GetMessage(messageID.Success, "Saved", String.Format("Adjustment[{0}]", AdjustIDReturn))

                    ' Add new Request to list and rebind (if needed)
                    If Not (DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.AdjustInList), List(Of AdjustDetails)).Exists(Function(i As AdjustDetails) i.AdjustID = Adjust.AdjustID)) Then
                        EditCache(Cache(Session(ESession.StoreID.ToString) & ECache.AdjustInList) _
                                  , GetType(AdjustDetails) _
                                  , Adjust _
                                  )
                        BindDropDownList(ddlLocateAdjustID _
                                         , Cache(Session(ESession.StoreID.ToString) & ECache.AdjustInList) _
                                         , "AdjustID" _
                                         , "AdjustID" _
                                         )
                        BindDropDownList(ddlRptDocumentNo _
                                         , Cache(Session(ESession.StoreID.ToString) & ECache.AdjustInList) _
                                         , "AdjustID" _
                                         , "AdjustID" _
                                         )
                        uplPrintAdjust.Update()

                        ' display the new Serial No(if any) as the Last Serial No
                        If Trim(txtSerialNo.Text) <> EMPTY Then
                            lblLastSerialNo.Text = Trim(txtSerialNo.Text)
                        End If

                        ' Reset Control n Clear screen
                        txtAdjustID.Text = EMPTY
                        ddlDocType.SelectedIndex = -1
                        txtSerialNo.Text = EMPTY
                        ddlRequestID.SelectedIndex = -1
                        lblConsumerID.Text = EMPTY
                        txtAdjustDte.Text = EMPTY
                        trRequestID.Visible = False
                        btnCancelAll_Click(sender, e)
                    End If
                    ''End If

                Else
                    Message = GetMessage(messageID.AtLeast1Item)
                End If
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
            ViewState(EViewState.AdjustedStockItemID) = New List(Of String)
            ViewState.Remove(EViewState.AdjustItem)
            DCP.Controls.Clear()
            ''UpdateGTotal(0.0, True)

            MainPanel(True)

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    Protected Sub ShowModal_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ShowModal.Click
        mpuStockAvailability.Show()
        uplUserControl.Update()
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExit.Click
        ShowModal.Visible = False
        uplUserControl.Update()
    End Sub

    Protected Sub ddlDocType_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlDocType.SelectedIndexChanged
        If ddlDocType.SelectedValue = RETURNED Then
            If ddlRequestID.Items.Count = 0 Then
                GetRequestList(Cache(Session(ESession.StoreID.ToString) & ECache.RequestListSearch), Session(ESession.StoreID.ToString), CLOSED)
                BindDropDownList(ddlRequestID _
                                 , Cache(Session(ESession.StoreID.ToString) & ECache.RequestListSearch) _
                                 , "RequestID" _
                                 , "RequestID" _
                                 )

                If ddlRequestID.Items.Count = 1 Then
                    ddlRequestID_SelectedIndexChanged(Nothing, Nothing)
                End If
            End If

            trRequestID.Visible = True

        Else
            trRequestID.Visible = False
        End If

        ''DOESN'T WORK UAT02 - when a postback is performed at Locate tab, and without clearing then goes to New tab, the active tab still as Locate, hence setting here
        ''tbpNew.Focus()
    End Sub

    Protected Sub ddlRequestID_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlRequestID.SelectedIndexChanged
        ' Retrieve the consumer Code from the cache
        If ddlRequestID.SelectedValue <> EMPTY Then
            Dim Request As RequestDetails = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.RequestListSearch), List(Of RequestDetails)).Find(Function(i As RequestDetails) i.RequestID = ddlRequestID.SelectedValue)
            lblConsumerID.Text = Request.ConsumerID
        End If

        '' DOESN'T WORK UAT02 - when a postback is performed at Locate tab, and without clearing then goes to New tab, the active tab still as Locate, hence setting here
        ''tbcAdjustItem.ActiveTab.ID = NEWTAB
    End Sub

    Private Sub LoadReturnItem(ByRef dcp As DynamicControlsPlaceholder, ByVal userControlName As String, ByVal docType As String, ByVal requestID As String)
        Dim DetailsList As List(Of IssueItemDetails)
        Dim ReturnUnitCost As Double = 0.0

        Using Client As New ServiceClient
            ' Get Issue Item records
            DetailsList = Client.GetRequestItem(Session(ESession.StoreID.ToString).ToString _
                                                , requestID _
                                                )
        End Using

        For Each item In DetailsList
            Dim DetailsItem As New AdjustItemDetails

            ' Issue qty is stored as negative, hence need to convert to positive value
            If (item.Qty <> 0) Then ReturnUnitCost = item.TotalCost / item.Qty
            ''DetailsItem.TranID = 0
            DetailsItem.StockItemID = item.StockItemID
            ''DetailsItem.Qty = 0
            ''DetailsItem.TotalCost = 0.0
            ''DetailsItem.Remarks = EMPTY
            ''DetailsItem.AdjustItemID = 0
            DetailsItem.ItemReturn = item.TranID
            DetailsItem.BalanceQty = item.BalanceQty
            DetailsItem.MaxLevel = (-item.Qty)
            ''UpdateGTotal(item.TotalCost)

            AddAdjustItem(dcp _
                          , DetailsItem _
                          , userControlName _
                          , docType _
                          , ReturnUnitCost _
                          )
        Next
    End Sub

#End Region

#Region " Locate Tab "
    ''' <summary>
    ''' btnLocateGo - Click;
    ''' 08Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' 15Apr09  KG          UAT03  compare against RequestID instead of AdjustID;
    ''' </remarks>
    Private Sub btnLocateGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLocateGo.Click
        Try
            ' Enable/Disable control for editing
            ddlLocateAdjustID.Enabled = False
            pnlLocateAdjust.Enabled = False
            pnlLocateAccess.Enabled = False
            pnlStockCode.Enabled = False
            pnlLocateAdjustItem.Enabled = False
            pnlLocateAction.Enabled = False

            ' Populate record
            DCPLocate.Controls.Clear()
            ViewState.Remove(EViewState.AdjustItemLocate.ToString)
            ViewState(EViewState.AdjustedStockItemIDLocate) = New List(Of String)

            If ddlLocateAdjustID.SelectedValue <> EMPTY Then
                If CacheListIsEmpty(Cache(Session(ESession.StoreID.ToString) & ECache.AdjustInList), GetType(AdjustDetails)) Then GetAdjustList(Cache(Session(ESession.StoreID.ToString) & ECache.AdjustInList), Session(ESession.StoreID.ToString), ADJUSTIN, ALL)
                Dim Adjust As AdjustDetails = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.AdjustInList), List(Of AdjustDetails)).Find(Function(i As AdjustDetails) i.AdjustID = ddlLocateAdjustID.SelectedValue)
                If Adjust Is Nothing Then Throw New NullReferenceException
                Dim AdjustItemList As List(Of AdjustItemDetails)
                Dim ReturnUnitCost As Double = 0.0
                Dim IssueDetailsList As List(Of IssueItemDetails)

                lblLocateAdjustID.Text = Adjust.AdjustID
                lblLocateDocType.Text = Adjust.Type
                txtLocateSerialNo.Text = Adjust.SerialNo
                lblLocateRequestID.Text = Adjust.DocReturn
                lblLocateConsumerID.Text = Adjust.InvolveID
                txtLocateAdjustDte.Text = Adjust.Dte.ToString("dd/MM/yyyy")
                ''lblLocateGTotalCost.Text = "0.0000" 'to be computed later

                ' UAT02 - Edit and Delete allow only when withinFinanceCutoffDate
                If IsWithinFinanceCutoffDate(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), Session(ESession.StoreID.ToString), Adjust.Dte) Then
                    pnlLocateAccess.Enabled = True
                End If

                If lblLocateDocType.Text = RETURNED Then
                    trLocateRequestID.Visible = True
                    uplLocateStockCode.Visible = False

                Else
                    trLocateRequestID.Visible = False
                    uplLocateStockCode.Visible = True
                End If

                Using Client As New ServiceClient
                    ' '' NOT REQUIRED FOR INWARD
                    ' '' Get last serial no
                    ''lblLocateLastSerialNo.Text = Client.GetLastSerialNo(Session(ESession.StoreID.ToString) _
                    ''                                                    , ServiceModuleName.AdjustIn _
                    ''                                                    )
                    IssueDetailsList = New List(Of IssueItemDetails)
                    If lblLocateRequestID.Text <> EMPTY Then
                        ' Get Issue Item records
                        IssueDetailsList = Client.GetRequestItem(Session(ESession.StoreID.ToString).ToString _
                                                                 , lblLocateRequestID.Text _
                                                                 )

                        ' UAT02 - alert user when the return request is within the cut off date
                        If CacheListIsEmpty(Cache(Session(ESession.StoreID.ToString) & ECache.RequestList), GetType(RequestDetails)) Then GetRequestList(Cache(Session(ESession.StoreID.ToString) & ECache.RequestList), Session(ESession.StoreID.ToString), CLOSED)
                        Dim Request As RequestDetails = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.RequestList), List(Of RequestDetails)).Find(Function(i As RequestDetails) i.RequestID = lblLocateRequestID.Text)   'UAT03
                        If (IsWithinFinanceCutoffDate(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), Session(ESession.StoreID.ToString), Request.IssueDte)) Then
                            Message = String.Format("[{0}] Return request is still within financial cut off date. The indicative unit and total cost is not available yet.", lblLocateRequestID)
                        End If

                    End If
                    ' Get Adjust Item records
                    AdjustItemList = Client.GetAdjustItem(Session(ESession.StoreID.ToString).ToString _
                                                          , lblLocateAdjustID.Text _
                                                          , Adjust.Type)
                End Using

                ' Save the selected Adjustment to be used for comparision during saving
                ViewState(EViewState.AdjustInSelected) = Adjust

                For Each item In AdjustItemList
                    Dim DetailsItem As New AdjustItemDetails

                    DetailsItem.TranID = item.TranID
                    DetailsItem.StockItemID = item.StockItemID
                    DetailsItem.Qty = item.Qty
                    DetailsItem.TotalCost = item.TotalCost
                    DetailsItem.Remarks = item.Remarks
                    DetailsItem.AdjustItemID = item.AdjustItemID
                    DetailsItem.ItemReturn = item.ItemReturn
                    DetailsItem.BalanceQty = item.BalanceQty
                    DetailsItem.MaxLevel = item.MaxLevel
                    ReturnUnitCost = IIf(DetailsItem.Qty > 0, DetailsItem.TotalCost / DetailsItem.Qty, 0)
                    ''UpdateGTotal(item.TotalCost)

                    If lblLocateRequestID.Text <> EMPTY Then
                        Dim IssueItemDetails As IssueItemDetails = IssueDetailsList.Find(Function(i As IssueItemDetails) i.TranID = DetailsItem.ItemReturn)
                        If (IssueItemDetails.Qty <> 0) Then ReturnUnitCost = IssueItemDetails.TotalCost / IssueItemDetails.Qty
                        DetailsItem.MaxLevel = (-IssueItemDetails.Qty)
                        DetailsItem.BalanceQty = IssueItemDetails.BalanceQty
                    End If


                    AddAdjustItem(DCPLocate _
                                  , DetailsItem _
                                  , EViewState.AdjustItemLocate.ToString _
                                  , Adjust.Type _
                                  , ReturnUnitCost _
                                  )

                    ' keep a list of Adjusted stock item, to check against to restrict duplicate
                    DirectCast(ViewState(EViewState.AdjustedStockItemIDLocate), List(Of String)).Add(DetailsItem.StockItemID)

                Next

                pnlLocate.Visible = True

            Else
                ddlLocateAdjustID.Enabled = True
                pnlLocateAdjust.Enabled = False

                pnlLocate.Visible = False
                Message = GetMessage(messageID.MandatoryField)
            End If

        Catch ex As FaultException
            ddlLocateAdjustID.Enabled = True
            Message = ex.Message

        Catch ex As NullReferenceException
            ddlLocateAdjustID.Enabled = True
            Message = GetMessage(messageID.InvalidValue, "Adjust Reference")

        Catch ex As Exception
            ddlLocateAdjustID.Enabled = True
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    ''' <summary>
    ''' btnLocateClear - Click;
    ''' 08Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Sub btnLocateClear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLocateClear.Click
        ddlLocateAdjustID.SelectedIndex = -1

        ' Enable/Disable control for editing
        ddlLocateAdjustID.Enabled = True
        pnlLocateAccess.Enabled = False
        pnlLocateAdjust.Enabled = False
        pnlLocateAdjustItem.Enabled = False
        pnlLocateAction.Enabled = False

        pnlLocate.Visible = False
        'ddlLocateStockCode.Items.Clear()
        txtLocateStockCode.Text = String.Empty
        uplLocateStockCode.Visible = False

        DCPLocate.Controls.Clear()
        ViewState.Remove(EViewState.Mode)
        ViewState.Remove(EViewState.AdjustedStockItemIDLocate)
    End Sub

    Protected Sub btnLocateEdit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLocateEdit.Click
        ' Enable/Disable control for editing
        pnlLocateAdjust.Enabled = True
        pnlLocateAccess.Enabled = False
        pnlStockCode.Enabled = True
        pnlLocateAdjustItem.Enabled = True
        pnlLocateAction.Enabled = True

        If lblLocateDocType.Text <> RETURNED Then

            ' bind stock code DDL [NOT NEEDED]
            'If CacheListIsEmpty(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), GetType(ItemDetails)) Then GetItemList(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), Session(ESession.StoreID.ToString))
            'BindDropDownList(ddlLocateStockCode, Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), "ItemID", "ItemID_Description")

            If Not uplLocateStockCode.Visible Then
                uplLocateStockCode.Visible = True
            End If
        End If
        uplLocateAdjust.Update()

    End Sub

    Protected Sub btnLocateAddItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLocateAddItem.Click
        Try
            Dim MoreItemInfo As MoreItemInfoDetails

            If txtLocateStockCode.Text.Trim <> EMPTY Then

                '-- VALIDATE STOCK CODE
                Dim StockCode As String
                StockCode = Split(txtLocateStockCode.Text, " | ")(0).Trim

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

                Dim AdjustItemDetailsItem As New AdjustItemDetails

                Dim Adjusted As Boolean = DirectCast(ViewState(EViewState.AdjustedStockItemIDLocate), List(Of String)).Exists(Function(i As String) (i = StockCode))

                If Adjusted Then
                    Throw New ApplicationException(GetMessage(messageID.StockCodeHasOrdered, StockCode))
                End If

                AdjustItemDetailsItem.StockItemID = StockCode

                ' UAT02.46 - retrieve the current balance for display
                Using Client As New ServiceClient
                    MoreItemInfo = Client.GetMoreItemInfo(Session(ESession.StoreID.ToString) _
                                                          , StockCode _
                                                          , Today _
                                                          )
                    AdjustItemDetailsItem.BalanceQty = MoreItemInfo.Balance
                    AdjustItemDetailsItem.MaxLevel = MoreItemInfo.MaxLevel
                End Using

                ' UAT02 - set mode as Insert
                AdjustItemDetailsItem.Mode = INSERT

                AddAdjustItem(DCPLocate _
                              , AdjustItemDetailsItem _
                              , EViewState.AdjustItemLocate.ToString _
                              , lblLocateDocType.Text _
                              )

                ' keep a list of Adjusted stock item, to check against to restrict duplicate
                DirectCast(ViewState(EViewState.AdjustedStockItemIDLocate), List(Of String)).Add(AdjustItemDetailsItem.StockItemID)

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

    Protected Sub btnLocateSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLocateSave.Click
        Try
            'save data to database
            Dim Adjust As AdjustDetails
            Dim DetailsList As List(Of AdjustItemDetails)
            Dim AdjustDte As Date = ConvertToDate(txtLocateAdjustDte.Text)
            Dim AdjustUpdated As Boolean = False

            ' Adjust Details
            Adjust = New AdjustDetails
            Adjust.StoreID = Session(ESession.StoreID.ToString).ToString
            Adjust.AdjustID = lblLocateAdjustID.Text
            Adjust.Type = lblLocateDocType.Text
            Adjust.SerialNo = Trim(txtLocateSerialNo.Text)
            Adjust.Dte = ConvertToDate(txtLocateAdjustDte.Text)
            Adjust.Status = CLOSED  'all adjustment is 'Closed'
            Adjust.LoginUser = Session(ESession.UserID.ToString).ToString


            ' validate mandatory fields
            If (Adjust.StoreID = EMPTY _
                Or Adjust.AdjustID = EMPTY _
                Or Adjust.Type = EMPTY _
                Or Adjust.Dte = Date.MinValue _
                Or Adjust.LoginUser = EMPTY _
                ) Then
                Throw New ApplicationException(GetMessage(messageID.MandatoryField))
            End If


            ' validate date
            If (AdjustDte > Today) Then Throw New ApplicationException(GetMessage(messageID.MoreLessThan, , , "Adjustment Date", Today.ToString("dd/MM/yyyy"), "earlier or same as"))
            If Not (IsWithinFinanceCutoffDate(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), Session(ESession.StoreID.ToString), AdjustDte)) Then Throw New ApplicationException(GetMessage(messageID.NotInFinancial, "Adjustment Date"))


            ' check if adjust is updated
            If (DirectCast(ViewState(EViewState.AdjustInSelected), AdjustDetails).SerialNo <> Adjust.SerialNo _
                Or DirectCast(ViewState(EViewState.AdjustInSelected), AdjustDetails).Dte <> Adjust.Dte _
                ) Then
                AdjustUpdated = True
            End If


            ' check list count
            If Adjust.Type = RETURNED Then
                Adjust.DocReturn = lblLocateRequestID.Text
                Adjust.InvolveID = lblLocateConsumerID.Text
                DetailsList = RetrieveReturnDCPInfo(EViewState.AdjustItemLocate.ToString, Adjust.Type)

            Else
                DetailsList = RetrieveDCPInfo(EViewState.AdjustItemLocate.ToString, Adjust.Type)
            End If

            ' Proceed with update when either Adjust or AdjustItem changed
            If DetailsList.Count > 0 Or AdjustUpdated Then
                Using Client As New ServiceClient

                    ' '' Not applicable for inward
                    ' '' validate serial no is unique
                    ''If (Trim(txtLocateSerialNo.Text) <> EMPTY _
                    ''    And DirectCast(ViewState(EViewState.AdjustInSelected), AdjustDetails).SerialNo <> Adjust.SerialNo _
                    ''    ) Then
                    ''    If Not (Client.FieldIsUnique(Session(ESession.StoreID.ToString) _
                    ''                                 , ServiceColumnName.AdjustInID _
                    ''                                 , Trim(txtLocateSerialNo.Text) _
                    ''                                 , lblLocateAdjustID.Text _
                    ''                                 ) _
                    ''            ) Then Client.Close() : Throw New ApplicationException(GetMessage(messageID.FieldNotUnique, "Serial No"))
                    ''End If


                    Message = Client.UpdateAdjust(Adjust _
                                                  , DetailsList _
                                                  )
                End Using

                If Message = EMPTY Then
                    Message = GetMessage(messageID.Success, "Edited", String.Format("Adjustment[{0}]", Adjust.AdjustID))

                    ' Add edited Adjustment to list and rebind (if needed)
                    EditCache(Cache(Session(ESession.StoreID.ToString) & ECache.AdjustInList) _
                              , GetType(AdjustDetails) _
                              , Adjust _
                              , True _
                              )
                    BindDropDownList(ddlLocateAdjustID _
                                     , Cache(Session(ESession.StoreID.ToString) & ECache.AdjustInList) _
                                     , "AdjustID" _
                                     , "AdjustID" _
                                     )
                    BindDropDownList(ddlRptDocumentNo _
                                     , Cache(Session(ESession.StoreID.ToString) & ECache.AdjustInList) _
                                     , "AdjustID" _
                                     , "AdjustID" _
                                     )
                    uplPrintAdjust.Update()

                    ' UAT02 - Clear dropdownlist to ease Ajax loading
                    'ddlLocateStockCode.Items.Clear()
                    txtLocateStockCode.Text = String.Empty

                    ' Reset Control n Clear screen
                    btnLocateClear_Click(sender, e)
                End If

            Else
                Message = GetMessage(messageID.NoChange)
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

    ''' <summary>
    ''' btnLocateCancel - Click;
    ''' 08Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Private Sub btnLocateCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLocateCancel.Click
        ' UAT02 - Clear dropdownlist to ease Ajax loading
        'ddlLocateStockCode.Items.Clear()
        txtLocateStockCode.Text = String.Empty

        ' Reload original info
        btnLocateGo_Click(sender, e)
    End Sub

    ''' <summary>
    ''' btnLocateCancel - Click;
    ''' 08Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Private Sub btnLocateDel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLocateDeleteAll.Click
        Try
            Using Client As New ServiceClient
                Message = Client.DeleteAdjust(Session(ESession.StoreID.ToString).ToString _
                                              , ddlLocateAdjustID.SelectedValue _
                                              , lblLocateDocType.Text _
                                              , ConvertToDate(txtLocateAdjustDte.Text) _
                                              , Session(ESession.UserID.ToString).ToString _
                                              )

            End Using

            If Message = EMPTY Then
                ddlLocateAdjustID.Items.Remove(ddlLocateAdjustID.SelectedValue)
                ddlRptDocumentNo.Items.Remove(ddlRptDocumentNo.SelectedValue)

                ' remove deleted order from cache
                Dim AdjustDetailsItem As AdjustDetails = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.AdjustInList), List(Of AdjustDetails)).Find(Function(i As AdjustDetails) i.AdjustID = lblLocateAdjustID.Text)
                If AdjustDetailsItem IsNot Nothing Then DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.AdjustInList), List(Of AdjustDetails)).Remove(AdjustDetailsItem)

                ' UAT02 - Clear dropdownlist to ease Ajax loading
                'ddlLocateStockCode.Items.Clear()
                txtLocateStockCode.Text = String.Empty

                'clear screen
                btnLocateClear_Click(sender, e)

                'display Alert Message
                Message = GetMessage(messageID.Success, "deleted", "Adjust")
            Else

                Throw New ApplicationException(Message)
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

    Protected Sub btnLocateExit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLocateExit.Click
        ShowLocateModal.Visible = False
    End Sub

    Protected Sub ShowLocateModal_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ShowLocateModal.Click
        mpuLocateStockAvailability.Show()
        uplLocateUserControl.Update()
    End Sub

#End Region

#Region " Print Tab "
    Protected Sub btnPDF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPDF.Click
        If Not ValidPrint() Then
            Exit Sub
        End If

        rvr.LocalReport.DataSources.Clear()
        rvr.LocalReport.DataSources.Add(New ReportDataSource("AdjustList", ObjectDataSource1))

        Dim parameterlist As New List(Of ReportParameter)
        Dim p1 As New ReportParameter("PlantName", Session("StoreName").ToString())
        Dim p2 As New ReportParameter("PODateFrom", txtDateFrom.Text)
        Dim p3 As New ReportParameter("PODateTo", txtDateTo.Text)
        Dim p4 As New ReportParameter("ReportTitle", "Adjustment Inwards Report")
        Dim p5 As New ReportParameter("DocNo", ddlRptDocumentNo.SelectedValue)
        parameterlist.Add(p1)
        parameterlist.Add(p2)
        parameterlist.Add(p3)
        parameterlist.Add(p4)
        parameterlist.Add(p5)

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
        Response.AddHeader("content-disposition", "attachment;filename=AdjustmentInward.pdf")
        Response.BinaryWrite(bytValue)
        Response.Flush()
        Response.End()
    End Sub

    Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExcel.Click
        If Not ValidPrint() Then
            Exit Sub
        End If

        rvr.LocalReport.DataSources.Clear()
        rvr.LocalReport.DataSources.Add(New ReportDataSource("AdjustList", ObjectDataSource1))

        Dim parameterlist As New List(Of ReportParameter)
        Dim p1 As New ReportParameter("PlantName", Session("StoreName").ToString())
        Dim p2 As New ReportParameter("PODateFrom", txtDateFrom.Text)
        Dim p3 As New ReportParameter("PODateTo", txtDateTo.Text)
        Dim p4 As New ReportParameter("ReportTitle", "Adjustment Inwards Report")
        Dim p5 As New ReportParameter("DocNo", ddlRptDocumentNo.SelectedValue)
        parameterlist.Add(p1)
        parameterlist.Add(p2)
        parameterlist.Add(p3)
        parameterlist.Add(p4)
        parameterlist.Add(p5)

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
        Response.AddHeader("content-disposition", "attachment;filename=AdjustmentInward.xls")
        Response.BinaryWrite(bytValue)
        Response.Flush()
        Response.End()
    End Sub

    Protected Sub ObjectDataSource1_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs) Handles ObjectDataSource1.Selecting
        e.InputParameters("storeID") = Session("StoreID")
        e.InputParameters("type") = "AI"
        e.InputParameters("fromDte") = DateTime.ParseExact(Me.txtDateFrom.Text, "dd/MM/yyyy", Nothing)
        e.InputParameters("toDte") = DateTime.ParseExact(Me.txtDateTo.Text, "dd/MM/yyyy", Nothing)
        e.InputParameters("adjustID") = ddlRptDocumentNo.SelectedValue
    End Sub

    Protected Sub ObjectDataSource1_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles ObjectDataSource1.Selected
        Dim returnList As List(Of AdjustList) = e.ReturnValue
        If returnList.Count <= 0 Then
            NoRecordFond = "Y"
        Else
            NoRecordFond = "N"
        End If
    End Sub

    Private Function ValidPrint() As Boolean
        ' validate filtering criteria
        If ddlRptDocumentNo.SelectedValue = EMPTY Then
            If (Trim(txtDateFrom.Text) = EMPTY _
                Or Trim(txtDateTo.Text) = EMPTY _
                ) Then
                Message = GetMessage(messageID.MandatoryField)
                Exit Function
            End If

            If ConvertToDate(txtDateFrom.Text) > ConvertToDate(txtDateTo.Text) Then
                Message = GetMessage(messageID.DateToEarlierDateFrom)
                Exit Function
            End If
        End If

        Return True
    End Function

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClear.Click
        ddlRptDocumentNo.SelectedIndex = -1
        txtDateFrom.Text = Today.ToString("01/MM/yyyy")
        txtDateTo.Text = Today.ToString("dd/MM/yyyy")
    End Sub

#End Region

#Region " Sub Procedures and Functions "

    ''' <summary>
    ''' Manage the Main and details screen display;
    ''' To enable/disable UI controls for Edit
    ''' 29Feb09 - KG;
    ''' </summary>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Private Sub MainPanel(ByVal enabled As Boolean)
        Try
            Select Case tbcAdjustItem.ActiveTab.ID
                Case NEWTAB
                    txtAdjustID.Enabled = enabled
                    ddlDocType.Enabled = enabled
                    txtSerialNo.Enabled = enabled
                    ddlRequestID.Enabled = enabled
                    txtAdjustDte.Enabled = enabled
                    divAddButton.Visible = enabled

                    pnlNewAdjust.Visible = (Not enabled)
                    uplNewAdjust.Update()
                    uplUserControl.Update()
            End Select

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Add the AdjustItem User Control in the PlaceHolder;
    ''' No convert required for Adjustment inward quantity as the value is store as positive;
    ''' 29Feb09 - KG;
    ''' </summary>
    ''' <param name="PlaceHolder">position</param>
    ''' <param name="AdjustItemDetails">request/Adjust info</param>
    ''' <param name="UserControlName"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Private Sub AddAdjustItem(ByRef PlaceHolder As DynamicControlsPlaceholder, ByVal AdjustItemDetails As AdjustItemDetails, ByVal UserControlName As String, ByVal adjustType As String, Optional ByVal returnUnitCost As Double = 0.0)
        Try
            Dim AdjustItem = New AdjustItem
            Dim ItemDetails As ItemDetails = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), List(Of ItemDetails)).Find(Function(i As ItemDetails) i.ItemID = AdjustItemDetails.StockItemID)

            If IsNothing(ViewState(UserControlName)) Then
                ViewState(UserControlName) = 1
            Else
                ViewState(UserControlName) += 1
            End If

            ' load user control at top of placeholder
            AdjustItem = LoadControl("AdjustItem.ascx")
            AdjustItem.ID = UserControlName + Convert.ToString(ViewState(UserControlName))
            PlaceHolder.Controls.AddAt(0, AdjustItem)

            ' clear MoreItemInfo cache
            ViewState.Remove(AdjustItem.UniqueID)

            ' Assign value to controls
            DirectCast(AdjustItem.FindControl("hfMode"), HiddenField).Value = AdjustItemDetails.Mode
            DirectCast(AdjustItem.FindControl("hfTranID"), HiddenField).Value = AdjustItemDetails.TranID
            DirectCast(AdjustItem.FindControl("hfTranType"), HiddenField).Value = adjustType
            DirectCast(AdjustItem.FindControl("hfAdjustItemID"), HiddenField).Value = AdjustItemDetails.AdjustItemID
            DirectCast(AdjustItem.FindControl("hfItemReturn"), HiddenField).Value = AdjustItemDetails.ItemReturn
            DirectCast(AdjustItem.FindControl("hfBalanceQty"), HiddenField).Value = AdjustItemDetails.BalanceQty
            DirectCast(AdjustItem.FindControl("hfMaxLevel"), HiddenField).Value = AdjustItemDetails.MaxLevel

            DirectCast(AdjustItem.FindControl("hfOrgAdjustQty"), HiddenField).Value = (AdjustItemDetails.Qty)
            DirectCast(AdjustItem.FindControl("hfOrgTotalCost"), HiddenField).Value = (AdjustItemDetails.TotalCost)
            DirectCast(AdjustItem.FindControl("hfOrgRemarks"), HiddenField).Value = AdjustItemDetails.Remarks

            DirectCast(AdjustItem.FindControl("lblStockCode"), Label).Text = AdjustItemDetails.StockItemID
            DirectCast(AdjustItem.FindControl("lblDescription"), Label).Text = ItemDetails.ItemDescription
            DirectCast(AdjustItem.FindControl("lblUOM"), Label).Text = ItemDetails.UOM

            DirectCast(AdjustItem.FindControl("txtAdjustQty"), TextBox).Text = IIf(AdjustItemDetails.Qty <> 0, (AdjustItemDetails.Qty).ToString("0.00"), EMPTY)
            DirectCast(AdjustItem.FindControl("txtTotalCost"), TextBox).Text = IIf(AdjustItemDetails.TotalCost <> 0, (AdjustItemDetails.TotalCost).ToString("0.00"), EMPTY)
            DirectCast(AdjustItem.FindControl("txtRemarks"), TextBox).Text = AdjustItemDetails.Remarks

            If adjustType = RETURNED Then
                '  Client Side Java Scripting
                DirectCast(AdjustItem.FindControl("txtAdjustQty"), TextBox).Attributes.Add("onkeyup", "computeTotal2('" & DirectCast(AdjustItem.FindControl("txtAdjustQty"), TextBox).ClientID & "','" & DirectCast(AdjustItem.FindControl("lblUnitCost"), Label).ClientID & "','" & DirectCast(AdjustItem.FindControl("txtTotalCost"), TextBox).ClientID & "');")
                '  truncate value to 4 decimal place for display
                DirectCast(AdjustItem.FindControl("lblUnitCost"), Label).Text = DisplayValue(returnUnitCost)

                ' ''  Client Side Java Scripting
                ''DirectCast(AdjustItem.FindControl("txtAdjustQty"), TextBox).Attributes.Add("onkeyup", "computeUnit('" & DirectCast(AdjustItem.FindControl("txtAdjustQty"), TextBox).ClientID & "','" & DirectCast(AdjustItem.FindControl("txtTotalCost"), TextBox).ClientID & "','" & DirectCast(AdjustItem.FindControl("lblUnitCost"), Label).ClientID & "');")
                ''DirectCast(AdjustItem.FindControl("txtTotalCost"), TextBox).Attributes.Add("onkeyup", "computeUnit('" & DirectCast(AdjustItem.FindControl("txtAdjustQty"), TextBox).ClientID & "','" & DirectCast(AdjustItem.FindControl("txtTotalCost"), TextBox).ClientID & "','" & DirectCast(AdjustItem.FindControl("lblUnitCost"), Label).ClientID & "');")
                ' ''  truncate value to 4 decimal place for display
                ''DirectCast(AdjustItem.FindControl("lblUnitCost"), Label).Text = IIf(AdjustItemDetails.Qty <> 0, DisplayValue(AdjustItemDetails.TotalCost / AdjustItemDetails.Qty), "0.0000")
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Used by the User Control AdjustItem to display its Stock Item info;
    ''' 29Feb09 - KG;
    ''' </summary>
    ''' <param name="uniqueID"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Friend Sub ViewStock(ByVal uniqueID As String, ByVal stockItemID As String, ByVal stockItemDesc As String, ByVal stockItemUOM As String, ByVal adjustQtyDiff As Decimal, ByVal totalDiff As Double)
        Try
            ' Retrieve and Cache more Info for the Stock Item. AUC, Balance, etc
            If ViewState(uniqueID) Is Nothing Then
                Using Client As New ServiceClient
                    ViewState(uniqueID) = Client.GetMoreItemInfo(Session(ESession.StoreID.ToString).ToString _
                                                                 , stockItemID _
                                                                 , Today _
                                                                 )
                End Using
            End If

            'get info from Cache
            Dim MoreItem As MoreItemInfoDetails = DirectCast(ViewState(uniqueID), MoreItemInfoDetails)
            Dim UOM As String = " " + stockItemUOM

            ' Adjustment inward, hence add to the current value
            Dim BalanceNew As Decimal = MoreItem.Balance + adjustQtyDiff
            Dim TotalValueNew As Double = MoreItem.TotalValue + totalDiff
            Dim AUCNew As Double = IIf(BalanceNew > 0, TotalValueNew / BalanceNew, 0)

            If tbcAdjustItem.ActiveTab.ID = NEWTAB Then
                ViewStockCode.Text = stockItemID
                ViewDesc.Text = stockItemDesc
                ViewBalance.Text = MoreItem.Balance.ToString("0.00") + UOM
                ViewAUC.Text = "$ " + DisplayValue(MoreItem.AvgUnitCost)
                ViewTotalValue.Text = "$ " + DisplayValue(MoreItem.TotalValue)

                ViewBalanceNew.Text = BalanceNew.ToString("0.00") + UOM
                ViewAUCNew.Text = "$ " + DisplayValue(AUCNew)
                ViewTotalValueNew.Text = "$ " + DisplayValue(TotalValueNew)

                ShowModal.Visible = True
                ShowModal_Click(ShowModal, New EventArgs)
                uplUserControl.Update()

            ElseIf tbcAdjustItem.ActiveTab.ID = LOCATETAB Then
                ViewLocateStockCode.Text = stockItemID
                ViewLocateDesc.Text = stockItemDesc
                ViewLocateBalance.Text = MoreItem.Balance.ToString("0.00") + UOM
                ViewLocateAUC.Text = "$ " + DisplayValue(MoreItem.AvgUnitCost)
                ViewLocateTotalValue.Text = "$ " + DisplayValue(MoreItem.TotalValue)

                ViewLocateBalanceNew.Text = BalanceNew.ToString("0.00") + UOM
                ViewLocateAUCNew.Text = "$ " + DisplayValue(AUCNew)
                ViewLocateTotalValueNew.Text = "$ " + DisplayValue(TotalValueNew)

                ShowLocateModal.Visible = True
                ShowLocateModal_Click(ShowModal, New EventArgs)
                uplLocateUserControl.Update()
            End If

        Catch ex As FaultException
            Message = ex.Message

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    Protected Friend Sub CancelAdjustItem(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim AdjustItem As New AdjustItem
            AdjustItem = DirectCast(FindControl(sender), AdjustItem)
            If tbcAdjustItem.ActiveTab.ID = NEWTAB Then
                DCP.Controls.Remove(FindControl(sender))
                uplUserControl.Update()
                ' remove stockcode from adjusted list
                DirectCast(ViewState(EViewState.AdjustedStockItemID), List(Of String)).Remove(DirectCast(AdjustItem.FindControl("lblStockCode"), Label).Text)

            ElseIf tbcAdjustItem.ActiveTab.ID = LOCATETAB Then
                If DirectCast(AdjustItem.FindControl("hfMode"), HiddenField).Value = INSERT Then
                    DCPLocate.Controls.Remove(FindControl(sender))
                Else
                    DirectCast(AdjustItem.FindControl("hfMode"), HiddenField).Value = DELETE
                    AdjustItem.Visible = False
                End If
                ' remove stockcode from adjusted list
                DirectCast(ViewState(EViewState.AdjustedStockItemIDLocate), List(Of String)).Remove(DirectCast(AdjustItem.FindControl("lblStockCode"), Label).Text)

                uplLocateUserControl.Update()
            End If

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    ''' <summary>
    ''' RetrieveDCPInfo;
    ''' Retrieve the Adjust information from DCP and return as a list of Adjust item details;
    ''' 1)Collect Adjust information when Mode is NOT empty, also check qty is a valid number;
    ''' 2)No convert required for Adjustment inward quantity as the value is store as positive;
    ''' 29Feb09 - KG;
    ''' </summary>
    ''' <param name="UserControlName"></param>
    ''' <returns>list of Adjustitemdetails</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Function RetrieveDCPInfo(ByVal UserControlName As String, ByVal adjustType As String) As List(Of AdjustItemDetails)
        Dim AdjustItem As AdjustItem
        Dim DetailsItem As AdjustItemDetails
        Dim DetailsList As List(Of AdjustItemDetails)

        ' adjustment item Details
        DetailsList = New List(Of AdjustItemDetails)
        For i As Integer = 1 To ViewState(UserControlName)
            DetailsItem = New AdjustItemDetails
            If tbcAdjustItem.ActiveTab.ID = NEWTAB Then
                AdjustItem = TryCast(DCP.FindControl(UserControlName + i.ToString), AdjustItem)
                DetailsItem.Mode = INSERT ' default mode to insert for adjustment creation

            ElseIf tbcAdjustItem.ActiveTab.ID = LOCATETAB Then
                AdjustItem = TryCast(DCPLocate.FindControl(UserControlName + i.ToString), AdjustItem)
            Else

                Throw New Exception("something wrong?")
            End If

            If AdjustItem IsNot Nothing Then
                Dim Value As String

                ' UAT02 capture user control exception
                If AdjustItem.Message <> EMPTY Then Throw New ApplicationException(AdjustItem.Message)

                DetailsItem.TranID = CInt(DirectCast(AdjustItem.FindControl("hfTranID"), HiddenField).Value)

                DetailsItem.StockItemID = DirectCast(AdjustItem.FindControl("lblStockCode"), Label).Text
                DetailsItem.AdjustItemID = CInt(DirectCast(AdjustItem.FindControl("hfAdjustItemID"), HiddenField).Value)
                DetailsItem.ItemReturn = CInt(DirectCast(AdjustItem.FindControl("hfItemReturn"), HiddenField).Value)
                DetailsItem.Remarks = Trim(DirectCast(AdjustItem.FindControl("txtRemarks"), TextBox).Text)
                DetailsItem.Status = CLOSED ' adjustment is add direct to stock transaction

                Value = Trim(DirectCast(AdjustItem.FindControl("txtAdjustQty"), TextBox).Text)
                If IsNumeric(Value) Then DetailsItem.Qty = CDec(Value)

                Value = Trim(DirectCast(AdjustItem.FindControl("txtTotalCost"), TextBox).Text)
                If IsNumeric(Value) Then DetailsItem.TotalCost = CDec(Value)

                ' UAT02 use the value assigned within the user control
                DetailsItem.Mode = DirectCast(AdjustItem.FindControl("hfMode"), HiddenField).Value
                ''If DetailsItem.Mode <> INSERT Then
                ''    ' Set Item Mode
                ''    DetailsItem.Mode = GetItemMode(DetailsItem.TranID _
                ''                                   , DetailsItem.Qty _
                ''                                   , CDec(DirectCast(AdjustItem.FindControl("hfOrgAdjustQty"), HiddenField).Value) _
                ''                                   , DetailsItem.TotalCost _
                ''                                   , CDec(DirectCast(AdjustItem.FindControl("hfOrgTotalCost"), HiddenField).Value) _
                ''                                   , Trim(DirectCast(AdjustItem.FindControl("txtRemarks"), TextBox).Text) _
                ''                                   , DirectCast(AdjustItem.FindControl("hfOrgRemarks"), HiddenField).Value _
                ''                                   )

                ''End If

                ' check Adjust qty & total cost are entered when mode is Insert or Update
                If DetailsItem.Mode = INSERT Or DetailsItem.Mode = UPDATE Then
                    ' check adjust qty is entered, except Price
                    If Not adjustType.Contains("PRICE") Then
                        If DetailsItem.Qty = 0 Then Throw New ApplicationException(String.Format("[{0}] {1}", DetailsItem.StockItemID, GetMessage(messageID.MandatoryField)))
                    End If

                    ' check total cost is entered when it is Price
                    If adjustType.Contains("PRICE") Then
                        If DetailsItem.TotalCost = 0 Then Throw New ApplicationException(String.Format("[{0}] {1}", DetailsItem.StockItemID, GetMessage(messageID.MandatoryField)))
                    End If
                End If

                ' Process those with mode ONLY
                If DetailsItem.Mode <> EMPTY Then
                    DetailsList.Add(DetailsItem)
                End If
            End If
        Next

        Return DetailsList
    End Function

    ''' <summary>
    ''' GetItemMode;
    ''' Determine the Item Mode by using the various parameters
    ''' 24Feb09 - KG;
    ''' </summary>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Private Function GetItemMode(ByVal hfTranID As Integer, ByVal adjustQty As String, ByVal hfOrgAdjustQty As String, ByVal totalCost As String, ByVal hfOrgTotalCost As String, ByVal txtRemarks As String, ByVal hfOrgRemarks As String) As String
        ' TranID = 0 means not Adjust yet
        If hfTranID = 0 Then
            Return INSERT

        Else
            ' Update when screen value diff from original value
            If (hfOrgAdjustQty <> adjustQty Or hfOrgTotalCost <> totalCost Or hfOrgRemarks <> Trim(txtRemarks)) Then
                Return UPDATE

            Else
                Return EMPTY
            End If
        End If

        Return EMPTY
    End Function

    ''' <summary>
    ''' RetrieveReturnDCPInfo;
    ''' Retrieve the Return Adjust inward information from DCP and return as a list of Adjust item details;
    ''' 1)Collect Adjust information when Mode is NOT empty;
    ''' 2)No convert required for Adjustment inward quantity as the value is store as positive;
    ''' 29Feb09 - KG;
    ''' </summary>
    ''' <param name="UserControlName"></param>
    ''' <returns>list of Adjustitemdetails</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Function RetrieveReturnDCPInfo(ByVal UserControlName As String, ByVal adjustType As String) As List(Of AdjustItemDetails)
        Dim AdjustItem As AdjustItem
        Dim DetailsItem As AdjustItemDetails
        Dim DetailsList As List(Of AdjustItemDetails)

        ' adjustment item Details
        DetailsList = New List(Of AdjustItemDetails)
        For i As Integer = 1 To ViewState(UserControlName)
            DetailsItem = New AdjustItemDetails
            If tbcAdjustItem.ActiveTab.ID = NEWTAB Then
                AdjustItem = TryCast(DCP.FindControl(UserControlName + i.ToString), AdjustItem)

                DetailsItem.Mode = INSERT ' default mode to insert for adjustment creation

            ElseIf tbcAdjustItem.ActiveTab.ID = LOCATETAB Then
                AdjustItem = TryCast(DCPLocate.FindControl(UserControlName + i.ToString), AdjustItem)
            Else

                Throw New Exception("something wrong?")
            End If

            If AdjustItem IsNot Nothing Then
                Dim Value As String

                ' UAT02 capture user control exception
                If AdjustItem.Message <> EMPTY Then Throw New ApplicationException(AdjustItem.Message)

                DetailsItem.TranID = CInt(DirectCast(AdjustItem.FindControl("hfTranID"), HiddenField).Value)

                DetailsItem.StockItemID = DirectCast(AdjustItem.FindControl("lblStockCode"), Label).Text
                DetailsItem.AdjustItemID = CInt(DirectCast(AdjustItem.FindControl("hfAdjustItemID"), HiddenField).Value)
                DetailsItem.ItemReturn = CInt(DirectCast(AdjustItem.FindControl("hfItemReturn"), HiddenField).Value)
                DetailsItem.Remarks = Trim(DirectCast(AdjustItem.FindControl("txtRemarks"), TextBox).Text)
                DetailsItem.Status = CLOSED ' adjustment is add direct to stock transaction

                Value = Trim(DirectCast(AdjustItem.FindControl("txtAdjustQty"), TextBox).Text)
                If IsNumeric(Value) Then DetailsItem.Qty = CDec(Value)

                Value = Trim(DirectCast(AdjustItem.FindControl("txtTotalCost"), TextBox).Text)
                If IsNumeric(Value) Then DetailsItem.TotalCost = CDec(Value)

                If DetailsItem.Mode = INSERT Then
                    If DetailsItem.Qty = 0 Then DetailsItem.Mode = EMPTY

                Else
                    ' Set Item Mode
                    If DetailsItem.TranID = 0 Then
                        If DetailsItem.Qty <> 0 Then DetailsItem.Mode = INSERT

                    Else
                        If DetailsItem.Qty = 0 Then
                            DetailsItem.Mode = DELETE

                        Else
                            ' Update when screen value diff from original value
                            If (CDec(DirectCast(AdjustItem.FindControl("hfOrgAdjustQty"), HiddenField).Value) <> DetailsItem.Qty _
                                Or DirectCast(AdjustItem.FindControl("hfOrgRemarks"), HiddenField).Value <> DetailsItem.Remarks _
                                ) Then
                                DetailsItem.Mode = UPDATE
                            End If
                        End If
                    End If
                End If

                ' Process those with mode ONLY
                If DetailsItem.Mode <> EMPTY Then
                    DetailsList.Add(DetailsItem)
                End If
            End If
        Next

        Return DetailsList
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

            ItemList = Client.GetItemSearch(ItemSearch)

            Client.Close()

        Catch ex As Exception
            Throw
        End Try

        Return ItemList

    End Function

#End Region

End Class