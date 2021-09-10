Imports Microsoft.Reporting.WebForms
Imports System.Web.Services.Description
Imports System.Web.Services
Imports NEA_ICS.UserInterface.NEA_ICS.WcfService
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ServiceModel
Imports DBauer.Web.UI.WebControls
Imports System.Reflection

Partial Public Class frmODL
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
    ''' 28Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CheckValidSession()

        Try
            If Not Page.IsPostBack Then

                'aceStockCode.ContextKey = Session("StoreID").ToString
                'aceLocateStockCode.ContextKey = Session("StoreID").ToString

                ' @@@ START OF ACCESS RIGHTS @@@
                Dim AccessRights As New List(Of RoleDetails)

                tbcAdjustItem.Visible = False
                AccessRights = clsCommonFunction.AssignAccessRights(Cache(Session(ESession.UserID.ToString) & Session(ESession.StoreID.ToString) & "AccessRights") _
                                                                    , moduleID.StockReturnOutwards _
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

                ' Store Approve Rights
                btnLocateApprove.Visible = False
                btnLocateReject.Visible = False
                If Session(ESession.UserRoleType.ToString).ToString.Contains(APPROVALOFFICER) Then
                    btnLocateApprove.Visible = True
                    btnLocateReject.Visible = True
                End If

                ' Store Officer Received Rights
                'txtReceiveDate.Visible = False
                'If Session(ESession.UserRoleType.ToString).ToString.Contains(STOREOFFICER) Then
                '    txtReceiveDate.Visible = True
                'End If
                ' @@@ END OF ACCESS RIGHTS @@@


                ' retrieve listing to be bind to ddl
                If CacheListIsEmpty(Cache(Session(ESession.StoreID.ToString) & ECache.ConsumerList), GetType(ConsumerDetails)) Then GetConsumerList(Cache(Session(ESession.StoreID.ToString) & ECache.ConsumerList), Session(ESession.StoreID.ToString))
                If CacheListIsEmpty(Cache(Session(ESession.StoreID.ToString) & ECache.AdjustOutList), GetType(AdjustDetails)) Then GetAdjustList(Cache(Session(ESession.StoreID.ToString) & ECache.AdjustOutList), Session(ESession.StoreID.ToString), ADJUSTODL, ALL)
                If CacheListIsEmpty(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), GetType(ItemDetails)) Then GetItemList(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), Session(ESession.StoreID.ToString))

                ' New Tab
                txtAdjustDte.Text = Today.ToString("dd/MM/yyyy")
                BindDropDownList(ddlDocType, GetCommonDataByCodeGroup(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), codeGroup.ODLdocType, OPEN), "CommonCodeID", "CommonCodeDescription")

                'BindDropDownList(ddlLocateAdjustID, Cache(Session(ESession.StoreID.ToString) & ECache.AdjustOutList), "AdjustID", "AdjustID")
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

                If ddlRequestID.Items.Count = 1 Then
                    ddlRequestID_SelectedIndexChanged(Nothing, Nothing)
                End If

                '   to allow Exit button click on Modal to invoke page postback
                btnExit.OnClientClick = String.Format("postbackFromJS('{0}', '{1}')", btnExit.UniqueID, String.Empty)

                pnlLocateAdjust.Enabled = False
                pnlLocateAdjustItem.Enabled = False

                '' Get last serial no
                'Using Client As New ServiceClient
                '    lblLastSerialNo.Text = Client.GetLastSerialNo(Session(ESession.StoreID.ToString) _
                '                                                        , ServiceModuleName.AdjustOut _
                '                                                        )
                'End Using

                ' Locate Tab
                BindDropDownList(ddlLocateConsumerSearch, Cache(Session(ESession.StoreID.ToString) & ECache.ConsumerList), "ConsumerID", "ConsumerID_Description")
                'BindDropDownList(ddlLocateAdjustID, Cache(Session(ESession.StoreID.ToString) & ECache.AdjustOutList), "AdjustID", "AdjustID")
                '   to allow Exit button click on Modal to invoke page postback
                btnLocateExit.OnClientClick = String.Format("postbackFromJS('{0}', '{1}')", btnLocateExit.UniqueID, String.Empty)

                pnlLocateAdjust.Enabled = False
                pnlLocateAdjustItem.Enabled = False

                ' Report Tab 
                Dim ClosedObsoleteDamage As List(Of AdjustDetails) = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.AdjustOutList), List(Of AdjustDetails)).FindAll(Function(item As AdjustDetails) item.Status = CLOSED)
                BindDropDownList(ddlRptDocumentNo, ClosedObsoleteDamage, "AdjustID", "AdjustID")
                btnClear_Click(Nothing, Nothing)

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
    ''' 28Feb09 - KG;
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

#Region "New Tab"
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
                Throw New ApplicationException(GetMessage(messageID.NotIsDate, "Obsolete/Damage Date"))
            End If
            If AdjustDte > Today Then
                Throw New ApplicationException(GetMessage(messageID.MoreLessThan, , , "Obsolete/Damage Date", Today.ToString("dd/MM/yyyy"), "earlier or same as"))
            End If
            If Not (IsWithinFinanceCutoffDate(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), Session(ESession.StoreID.ToString), AdjustDte)) Then
                Throw New ApplicationException(GetMessage(messageID.NotInFinancial, "Obsolete/Damage Date"))
            End If


            ' validate serial no is unique
            'If Trim(txtSerialNo.Text) <> EMPTY Then
            '    Using Client As New ServiceClient
            '        If Not (Client.FieldIsUnique(Session(ESession.StoreID.ToString) _
            '                                     , ServiceColumnName.AdjustSerialNo _
            '                                     , Trim(txtSerialNo.Text) _
            '                                     , EMPTY _
            '                                     ) _
            '                            ) Then Throw New ApplicationException(GetMessage(messageID.FieldNotUnique, "Serial No"))
            '    End Using
            'End If


            ' validate Adjust ID is unique
            If Trim(txtAdjustID.Text) <> EMPTY Then
                Using Client As New ServiceClient
                    If Not (Client.FieldIsUnique(Session(ESession.StoreID.ToString) _
                                                 , ServiceColumnName.AdjustInID _
                                                 , Trim(txtAdjustID.Text) _
                                                 , EMPTY _
                                                 ) _
                            ) Then
                        Throw New ApplicationException(String.Format("Return reference:({0}) exists in the system", Trim(txtAdjustID.Text)))
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
                'ViewState(EViewState.AdjustedStockItemID) = New List(Of String)

                ' load all the selected Issue reference Item for processing
                If ddlRequestID.SelectedValue = EMPTY Then Throw New ApplicationException(GetMessage(messageID.MandatoryField))
                LoadReturnItem(DCP _
                               , EViewState.AdjustItem.ToString _
                               , ddlDocType.SelectedValue _
                               , ddlRequestID.SelectedValue _
                               )

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

    'Protected Sub btnAddItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddItem.Click
    '    Try
    '        Dim MoreItemInfo As MoreItemInfoDetails

    '        If txtStockCode.Text.Trim <> EMPTY Then

    '            '-- VALIDATE STOCK CODE
    '            Dim StockCode As String
    '            StockCode = Split(txtStockCode.Text, " | ")(0).Trim

    '            Using Client As New ServiceClient

    '                Dim ItemDetails As New ItemDetails
    '                ItemDetails.StoreID = Session("StoreID").ToString
    '                ItemDetails.ItemID = StockCode

    '                If Not Client.IsValidStockCode(ItemDetails) Then

    '                    Message = GetMessage(messageID.InvalidStockCode, StockCode.ToUpper)
    '                    Client.Close()
    '                    Exit Sub

    '                End If

    '                If Not Client.IsValidStatus(ItemDetails) Then

    '                    Message = GetMessage(messageID.StockCodeClosed, StockCode.ToUpper)
    '                    Client.Close()
    '                    Exit Sub

    '                End If

    '                Client.Close()

    '            End Using

    '            Dim DetailsItem As New AdjustItemDetails

    '            Dim Adjusted As Boolean = DirectCast(ViewState(EViewState.AdjustedStockItemID), List(Of String)).Exists(Function(i As String) (i = StockCode))

    '            If Adjusted Then
    '                Throw New ApplicationException(GetMessage(messageID.StockCodeHasOrdered, StockCode))
    '            End If

    '            DetailsItem.StockItemID = StockCode

    '            Using Client As New ServiceClient
    '                MoreItemInfo = Client.GetMoreItemInfo(Session(ESession.StoreID.ToString) _
    '                                                      , StockCode _
    '                                                      , Today _
    '                                                      )
    '                DetailsItem.BalanceQty = MoreItemInfo.Balance
    '                DetailsItem.MaxLevel = MoreItemInfo.MaxLevel
    '            End Using

    '            ' UAT02 - set mode as Insert
    '            DetailsItem.Mode = INSERT

    '            AddAdjustItem(DCP _
    '                          , DetailsItem _
    '                          , EViewState.AdjustItem.ToString _
    '                          , ddlDocType.SelectedValue _
    '                          )

    '            ' keep a list of Adjusted stock item, to check against to restrict duplicate
    '            DirectCast(ViewState(EViewState.AdjustedStockItemID), List(Of String)).Add(DetailsItem.StockItemID)

    '        Else
    '            ' alert message
    '            Throw New ApplicationException(GetMessage(messageID.StockCodeNotSelected))
    '        End If

    '    Catch ex As ApplicationException
    '        Message = ex.Message

    '    Catch ex As FaultException
    '        Message = ex.Message

    '    Catch ex As Exception
    '        Message = GetMessage(messageID.TryLastOperation)

    '        Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
    '        If (rethrow) Then Throw
    '    End Try
    'End Sub


    'Private Sub AddAOutwardItem(ByRef PlaceHolder As DynamicControlsPlaceholder, ByVal AdjustItemDetails As AdjustItemDetails, ByVal UserControlName As String, ByVal adjustType As String, Optional ByVal returnUnitCost As Double = 0.0)
    '    Try
    '        Dim AdjustReturnItem = New AdjustReturnItem
    '        Dim ItemDetails As ItemDetails = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), List(Of ItemDetails)).Find(Function(i As ItemDetails) i.ItemID = AdjustItemDetails.StockItemID)

    '        If IsNothing(ViewState(UserControlName)) Then
    '            ViewState(UserControlName) = 1
    '        Else
    '            ViewState(UserControlName) += 1
    '        End If

    '        ' load user control at top of placeholder
    '        AdjustReturnItem = LoadControl("AdjustReturnItem.ascx")
    '        AdjustReturnItem.ID = UserControlName + Convert.ToString(ViewState(UserControlName))
    '        PlaceHolder.Controls.AddAt(0, AdjustReturnItem)

    '        ' clear MoreItemInfo cache
    '        ViewState.Remove(AdjustReturnItem.UniqueID)

    '        ' Assign value to controls
    '        DirectCast(AdjustReturnItem.FindControl("hfMode"), HiddenField).Value = AdjustItemDetails.Mode
    '        DirectCast(AdjustReturnItem.FindControl("hfTranID"), HiddenField).Value = AdjustItemDetails.TranID
    '        DirectCast(AdjustReturnItem.FindControl("hfTranType"), HiddenField).Value = adjustType
    '        DirectCast(AdjustReturnItem.FindControl("hfAdjustItemID"), HiddenField).Value = AdjustItemDetails.AdjustItemID
    '        DirectCast(AdjustReturnItem.FindControl("hfItemReturn"), HiddenField).Value = AdjustItemDetails.ItemReturn
    '        DirectCast(AdjustReturnItem.FindControl("hfBalanceQty"), HiddenField).Value = AdjustItemDetails.BalanceQty
    '        DirectCast(AdjustReturnItem.FindControl("hfMaxLevel"), HiddenField).Value = AdjustItemDetails.MaxLevel

    '        DirectCast(AdjustReturnItem.FindControl("hfOrgAdjustQty"), HiddenField).Value = (AdjustItemDetails.Qty)
    '        DirectCast(AdjustReturnItem.FindControl("hfOrgTotalCost"), HiddenField).Value = (AdjustItemDetails.TotalCost)
    '        DirectCast(AdjustReturnItem.FindControl("hfOrgRemarks"), HiddenField).Value = AdjustItemDetails.Remarks

    '        DirectCast(AdjustReturnItem.FindControl("lblStockCode"), Label).Text = AdjustItemDetails.StockItemID
    '        DirectCast(AdjustReturnItem.FindControl("lblDescription"), Label).Text = ItemDetails.ItemDescription
    '        DirectCast(AdjustReturnItem.FindControl("lblUOM"), Label).Text = ItemDetails.UOM

    '        DirectCast(AdjustReturnItem.FindControl("txtAdjustQty"), TextBox).Text = IIf(AdjustItemDetails.Qty <> 0, (AdjustItemDetails.Qty).ToString("0.00"), EMPTY)

    '        DirectCast(AdjustReturnItem.FindControl("txtReceiveQty"), TextBox).Text = IIf(AdjustItemDetails.Qty <> 0, (AdjustItemDetails.Qty).ToString("0.00"), EMPTY)
    '        ' Store Officer Received Rights
    '        DirectCast(AdjustReturnItem.FindControl("txtReceiveQty"), TextBox).Enabled = False
    '        If Session(ESession.UserRoleType.ToString).ToString.Contains(STOREOFFICER) And AdjustItemDetails.Status = APPROVED Then
    '            DirectCast(AdjustReturnItem.FindControl("txtReceiveQty"), TextBox).Enabled = True
    '        End If

    '        DirectCast(AdjustReturnItem.FindControl("txtRemarks"), TextBox).Text = AdjustItemDetails.Remarks

    '        If AdjustItemDetails.Status = CLOSED Or AdjustItemDetails.Status = APPROVED Then
    '            DirectCast(AdjustReturnItem.FindControl("checkReceived"), CheckBox).Checked = True
    '            DirectCast(AdjustReturnItem.FindControl("checkReceived"), CheckBox).Enabled = False
    '        Else
    '            DirectCast(AdjustReturnItem.FindControl("checkReceived"), CheckBox).Checked = False
    '            DirectCast(AdjustReturnItem.FindControl("checkReceived"), CheckBox).Enabled = False
    '            If Session(ESession.UserRoleType.ToString).ToString.Contains(STOREOFFICER) Then
    '                DirectCast(AdjustReturnItem.FindControl("checkReceived"), CheckBox).Enabled = True
    '            End If
    '        End If

    '        'If adjustType = RETURNED Then
    '        '  Client Side Java Scripting
    '        'DirectCast(AdjustReturnItem.FindControl("txtAdjustQty"), TextBox).Attributes.Add("onkeyup", "computeTotal2('" & DirectCast(AdjustReturnItem.FindControl("txtAdjustQty"), TextBox).ClientID & "','" & DirectCast(AdjustReturnItem.FindControl("lblUnitCost"), Label).ClientID & "','" & DirectCast(AdjustReturnItem.FindControl("txtTotalCost"), TextBox).ClientID & "');")
    '        '  truncate value to 4 decimal place for display
    '        'DirectCast(AdjustReturnItem.FindControl("lblUnitCost"), Label).Text = DisplayValue(returnUnitCost)

    '        ' ''  Client Side Java Scripting
    '        ''DirectCast(AdjustItem.FindControl("txtAdjustQty"), TextBox).Attributes.Add("onkeyup", "computeUnit('" & DirectCast(AdjustItem.FindControl("txtAdjustQty"), TextBox).ClientID & "','" & DirectCast(AdjustItem.FindControl("txtTotalCost"), TextBox).ClientID & "','" & DirectCast(AdjustItem.FindControl("lblUnitCost"), Label).ClientID & "');")
    '        ''DirectCast(AdjustItem.FindControl("txtTotalCost"), TextBox).Attributes.Add("onkeyup", "computeUnit('" & DirectCast(AdjustItem.FindControl("txtAdjustQty"), TextBox).ClientID & "','" & DirectCast(AdjustItem.FindControl("txtTotalCost"), TextBox).ClientID & "','" & DirectCast(AdjustItem.FindControl("lblUnitCost"), Label).ClientID & "');")
    '        ' ''  truncate value to 4 decimal place for display
    '        ''DirectCast(AdjustItem.FindControl("lblUnitCost"), Label).Text = IIf(AdjustItemDetails.Qty <> 0, DisplayValue(AdjustItemDetails.TotalCost / AdjustItemDetails.Qty), "0.0000")
    '        'End If

    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Sub

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
            'Adjust.SerialNo = Trim(txtSerialNo.Text)
            Adjust.Dte = ConvertToDate(txtAdjustDte.Text)
            Adjust.Status = OPEN  ' adjustment is subject for approval - status is 'OPEN' for new adjustment
            Adjust.LoginUser = Session(ESession.UserID.ToString).ToString
            Adjust.ReturnUserID = Session(ESession.UserID.ToString).ToString
            Adjust.ReturnDte = ConvertToDate(txtAdjustDte.Text)


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
                Adjust.DocReturn = ddlRequestID.SelectedValue
                Adjust.InvolveID = lblConsumerID.Text
                DetailsList = RetrieveDCPInfo(EViewState.AdjustItem.ToString, Adjust.Type)
                If DetailsList.Count > 0 Then
                    Using Client As New ServiceClient
                        AdjustIDReturn = Client.AddAdjust(Adjust _
                                                          , DetailsList _
                                                          )
                    End Using

                    ' Assign the AdjustID for adding new item
                    Adjust.AdjustID = AdjustIDReturn

                    ''If Message = EMPTY Then
                    Message = GetMessage(messageID.Success, "Saved", String.Format("Obsolete/Damage item(s)[{0}]", AdjustIDReturn))

                    ' Add new Request to list and rebind (if needed)
                    If Not (DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.AdjustOutList), List(Of AdjustDetails)).Exists(Function(i As AdjustDetails) i.AdjustID = Adjust.AdjustID)) Then
                        EditCache(Cache(Session(ESession.StoreID.ToString) & ECache.AdjustOutList) _
                                  , GetType(AdjustDetails) _
                                  , Adjust _
                                  )
                        'BindDropDownList(ddlLocateAdjustID _
                        '                 , Cache(Session(ESession.StoreID.ToString) & ECache.AdjustOutList) _
                        '                 , "AdjustID" _
                        '                 , "AdjustID" _
                        '                 )

                        'BindDropDownList(ddlRptDocumentNo _
                        '                 , Cache(Session(ESession.StoreID.ToString) & ECache.AdjustOutListObsDamage) _
                        '                 , "AdjustID" _
                        '                 , "AdjustID" _
                        ')
                        uplPrintAdjust.Update()

                        '' display the new Serial No(if any) as the Last Serial No
                        'If Trim(txtSerialNo.Text) <> EMPTY Then
                        '    lblLastSerialNo.Text = Trim(txtSerialNo.Text)
                        'End If

                        ' Reset Control n Clear screen
                        txtAdjustID.Text = EMPTY
                        'txtLocateSerialNo.Text = EMPTY
                        ddlRequestID.SelectedIndex = -1
                        ddlDocType.SelectedIndex = -1
                        lblConsumerID.Text = EMPTY
                        txtAdjustDte.Text = EMPTY
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
            If (item.Qty <> 0) Then ReturnUnitCost = -CDec(item.TotalCost / item.Qty)
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

            'AddAOutwardItem(dcp _
            '              , DetailsItem _
            '              , userControlName _
            '              , docType _
            '              )
            DetailsItem.Mode = INSERT

            'AddAdjustItem(dcp _
            '              , DetailsItem _
            '              , EViewState.AdjustItem.ToString _
            '              , ddlDocType.SelectedValue _
            '              , ReturnUnitCost _
            '              )
            AddAdjustItem(dcp _
                         , DetailsItem _
                         , userControlName _
                         , ddlDocType.SelectedValue _
                         , ReturnUnitCost _
                         )
        Next
    End Sub

#End Region

#Region "Locate Tab"
    ''' <summary>
    ''' btnLocateGo - Click;
    ''' 08Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Private Sub btnLocateGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLocateGo.Click
        Try
            ' Enable/Disable control for editing
            'txtLocateAdjustID.Enabled = False
            pnlLocateAdjust.Enabled = False
            pnlLocateAccess.Enabled = False
            pnlLocateAdjustItem.Enabled = False
            pnlLocateAction.Visible = True
            pnlLocateAction.Enabled = False

            btnLocateApprove.Enabled = False
            btnLocateReject.Enabled = False
            btnLocateSave.Enabled = False
            btnLocateCancel.Enabled = False

            ' Populate record
            'DCPLocate.Controls.Clear()
            'ViewState.Remove(EViewState.AdjustItemLocate.ToString)
            'ViewState(EViewState.AdjustedStockItemIDLocate) = New List(Of String)

            GetAdjustListBySearch(Cache(Session(ESession.StoreID.ToString) & ECache.AdjustOutList), Session(ESession.StoreID.ToString), ddlLocateConsumerSearch.SelectedValue, txtLocateAdjustID.Text, ADJUSTODL, ddlLocateStatus.SelectedValue)
            gdvLocate.DataSource = Cache(Session(ESession.StoreID.ToString) & ECache.AdjustOutList)
            gdvLocate.DataBind()

            ' Bind Document Type 
            BindDropDownList(ddlLocateDocType, GetCommonDataByCodeGroup(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), codeGroup.ODLdocType, OPEN), "CommonCodeID", "CommonCodeDescription")

            pnlSearchResults.Visible = True
            pnlLocate.Visible = False
            pnlLocateAdjust.Visible = False
            'ddlLocateStatus.Enabled = False
            'ddlLocateConsumerSearch.Enabled = False

            'If Session(ESession.UserRoleType.ToString).ToString.Contains(STOREOFFICER) Then
            '    btnLocateSave.Enabled = True
            '    btnLocateCancel.Enabled = True
            'End If

            '' Populate record
            'DCPLocate.Controls.Clear()
            'ViewState.Remove(EViewState.AdjustItemLocate.ToString)
            'ViewState(EViewState.AdjustedStockItemIDLocate) = New List(Of String)

            'If ddlLocateAdjustID.SelectedValue <> EMPTY Then
            '    If CacheListIsEmpty(Cache(Session(ESession.StoreID.ToString) & ECache.AdjustOutList), GetType(AdjustDetails)) Then GetAdjustList(Cache(Session(ESession.StoreID.ToString) & ECache.AdjustOutList), Session(ESession.StoreID.ToString), ADJUSTODL, ALL)
            '    Dim Adjust As AdjustDetails = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.AdjustOutList), List(Of AdjustDetails)).Find(Function(i As AdjustDetails) i.AdjustID = ddlLocateAdjustID.SelectedValue)
            '    If Adjust Is Nothing Then Throw New NullReferenceException
            '    Dim AdjustItemList As List(Of AdjustItemDetails)
            '    Dim ReturnUnitCost As Double = 0.0
            '    Dim IssueDetailsList As List(Of IssueItemDetails)

            '    lblLocateAdjustID.Text = Adjust.AdjustID
            '    'lblLocateDocType.Text = Adjust.Type
            '    BindDropDownList(ddlLocateDocType, GetCommonDataByCodeGroup(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), codeGroup.ODLdocType, OPEN), "CommonCodeID", "CommonCodeDescription")
            '    ddlLocateDocType.SelectedValue = Adjust.Type
            '    txtLocateSerialNo.Text = Adjust.SerialNo
            '    lblLocateRequestID.Text = Adjust.DocReturn
            '    lblLocateConsumerID.Text = Adjust.InvolveID
            '    lblReturnBy.Text = Adjust.LoginUser
            '    txtLocateAdjustDte.Text = Adjust.Dte.ToString("dd/MM/yyyy") 'todo: if null then don't display 01/01/0001
            '    lblApprovedBy.Text = Adjust.ApproveUserID
            '    lblApprovedDate.Text = IIf(Adjust.ApproveDte <> Date.MinValue, Adjust.ApproveDte.ToString("dd/MM/yyyy"), "")
            '    lblReceiveBy.Text = Adjust.ReceiveUserID


            '    txtReceiveDate.Text = IIf(Adjust.ReceiveDte <> Date.MinValue, Adjust.ReceiveDte.ToString("dd/MM/yyyy"), "")
            '    txtReceiveDate.Enabled = False
            '    If Session(ESession.UserRoleType.ToString).ToString.Contains(STOREOFFICER) And Adjust.Status = APPROVED Then
            '        txtReceiveDate.Enabled = True
            '        pnlLocateAccess.Enabled = False
            '    End If

            '    Select Case Adjust.Status
            '        Case OPEN
            '            lblStatus.Text = "OPEN"
            '        Case CLOSED
            '            lblStatus.Text = "CLOSED"
            '        Case APPROVED
            '            lblStatus.Text = "APPROVED"
            '        Case REJECTED
            '            lblStatus.Text = "REJECTED"
            '        Case PENDING
            '            lblStatus.Text = "APPROVED" ' set APPROVED as status if adjustment is pending for Receive
            '        Case DELETE
            '            lblStatus.Text = "DELETED"
            '    End Select
            '    ''lblLocateGTotalCost.Text = "0.0000" 'to be computed later

            '    pnlLocateAccess.Enabled = False
            '    If Adjust.Status = OPEN Then
            '        pnlLocateAccess.Enabled = True
            '    End If

            '    ' UAT02 - Edit and Delete allow only when withinFinanceCutoffDate
            '    If IsWithinFinanceCutoffDate(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), Session(ESession.StoreID.ToString), Adjust.Dte) _
            '        And Adjust.Status = OPEN Then 'enable edit/approve/reject button when only status is OPEN
            '        IIf(Not Session(ESession.UserRoleType.ToString).ToString.Contains(APPROVALOFFICER), pnlLocateAccess.Enabled = True, False)
            '        btnLocateApprove.Enabled = True
            '        btnLocateReject.Enabled = True
            '    Else
            '        btnLocateApprove.Enabled = False
            '        btnLocateReject.Enabled = False
            '    End If

            '    Using Client As New ServiceClient
            '        IssueDetailsList = New List(Of IssueItemDetails)
            '        If lblLocateRequestID.Text <> EMPTY Then
            '            ' Get Issue Item records
            '            IssueDetailsList = Client.GetRequestItem(Session(ESession.StoreID.ToString).ToString _
            '                                                     , lblLocateRequestID.Text _
            '                                                     )

            '            ' UAT02 - alert user when the return request is within the cut off date
            '            If CacheListIsEmpty(Cache(Session(ESession.StoreID.ToString) & ECache.RequestList), GetType(RequestDetails)) Then GetRequestList(Cache(Session(ESession.StoreID.ToString) & ECache.RequestList), Session(ESession.StoreID.ToString), CLOSED)
            '            Dim Request As RequestDetails = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.RequestList), List(Of RequestDetails)).Find(Function(i As RequestDetails) i.RequestID = lblLocateRequestID.Text)   'UAT03
            '            If (IsWithinFinanceCutoffDate(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), Session(ESession.StoreID.ToString), Request.IssueDte)) Then
            '                Message = String.Format("[{0}] Return request is still within financial cut off date. The indicative unit and total cost is not available yet.", lblLocateRequestID)
            '            End If

            '        End If
            '        ' Get Adjust Item records
            '        AdjustItemList = Client.GetAdjustItem(Session(ESession.StoreID.ToString).ToString _
            '                                              , lblLocateAdjustID.Text _
            '                                              , Adjust.Type)
            '    End Using

            '    ' Save the selected Adjustment to be used for comparision during saving
            '    ViewState(EViewState.AdjustOutSelected) = Adjust

            '    For Each item In AdjustItemList
            '        Dim DetailsItem As New AdjustItemDetails

            '        DetailsItem.TranID = item.TranID
            '        DetailsItem.StockItemID = item.StockItemID
            '        DetailsItem.Qty = item.Qty
            '        DetailsItem.TotalCost = item.TotalCost
            '        DetailsItem.Remarks = item.Remarks
            '        DetailsItem.AdjustItemID = item.AdjustItemID
            '        DetailsItem.ItemReturn = item.ItemReturn
            '        DetailsItem.BalanceQty = item.BalanceQty
            '        DetailsItem.MaxLevel = item.MaxLevel
            '        DetailsItem.Status = item.Status
            '        ReturnUnitCost = IIf(DetailsItem.Qty <> 0, -CDec(DetailsItem.TotalCost / DetailsItem.Qty), 0)
            '        ''UpdateGTotal(item.TotalCost)

            '        If lblLocateRequestID.Text <> EMPTY Then
            '            If DetailsItem.ItemReturn > 0 Then
            '                Dim IssueItemDetails As IssueItemDetails = IssueDetailsList.Find(Function(i As IssueItemDetails) i.TranID = DetailsItem.ItemReturn)

            '                If (IssueItemDetails.Qty <> 0) Then ReturnUnitCost = -CDec(IssueItemDetails.TotalCost / IssueItemDetails.Qty)
            '                DetailsItem.MaxLevel = (-IssueItemDetails.Qty)
            '                DetailsItem.BalanceQty = IssueItemDetails.BalanceQty
            '            End If
            '        End If

            '        AddAdjustItem(DCPLocate _
            '                      , DetailsItem _
            '                      , EViewState.AdjustItemLocate.ToString _
            '                      , Adjust.Type _
            '                      , ReturnUnitCost _
            '                      )

            '        ' keep a list of Adjusted stock item, to check against to restrict duplicate
            '        DirectCast(ViewState(EViewState.AdjustedStockItemIDLocate), List(Of String)).Add(DetailsItem.StockItemID)

            '    Next

            '    pnlLocate.Visible = True
            'Else

            '    ddlLocateAdjustID.Enabled = True
            '    pnlLocateAdjust.Enabled = False

            '    pnlLocate.Visible = False
            '    Message = GetMessage(messageID.MandatoryField)
            'End If

        Catch ex As FaultException
            txtLocateAdjustID.Enabled = True
            Message = ex.Message

        Catch ex As NullReferenceException
            txtLocateAdjustID.Enabled = True
            Message = GetMessage(messageID.InvalidValue, "Return Reference")

        Catch ex As Exception
            txtLocateAdjustID.Enabled = True
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    ''' <summary>
    ''' gdvLocate - PageIndexChaning;
    ''' 09 Mar 12 - Christina;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' Cache with UserID as the prefix;
    ''' </remarks>
    Private Sub gdvLocate_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gdvLocate.PageIndexChanging
        gdvLocate.PageIndex = e.NewPageIndex
        gdvLocate.DataSource = Cache(Session(ESession.StoreID.ToString) & ECache.AdjustOutList)
        gdvLocate.DataBind()
    End Sub

    ''' <summary>
    ''' gdvLocate - RowDataBound;
    ''' 09 Mar 12 - Christina;
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

                    'Case PENDING
                    '    CType(e.Row.FindControl("lblStatus"), Label).Text = "APPROVED" ' set APPROVED as status if adjustment is pending for Receive

                Case CLOSED
                    CType(e.Row.FindControl("lblStatus"), Label).Text = "Closed"

                Case DELETE
                    CType(e.Row.FindControl("lblStatus"), Label).Text = "Deleted"
            End Select

            CType(e.Row.FindControl("lblReturnBy"), Label).Text = CType(e.Row.FindControl("hidReturnBy"), HiddenField).Value.ToUpper

            'Dim userItem = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.UserNameList), List(Of ConsumerDetails)).Find(Function(i As ConsumerDetails) i.UserID.ToUpper = CType(e.Row.FindControl("hidRequestBy"), HiddenField).Value.ToUpper)
            'If userItem IsNot Nothing Then
            'CType(e.Row.FindControl("lblReturnBy"), Label).Text = userItem.UserName

            'Else
            ' display UserID when name if not available
            '            CType(e.Row.FindControl("lblReturnBy"), Label).Text = CType(e.Row.FindControl("hidReturnBy"), HiddenField).Value.ToUpper
            'End If
        End If
    End Sub

    ''' <summary>
    ''' gdvLocate - SelectedIndexChanged;
    ''' 09 Mar 12 - Christina;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' </remarks>
    Private Sub gdvLocate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvLocate.SelectedIndexChanged
        Try
            lblLocateAdjustID.Text = Replace(gdvLocate.SelectedRow.Cells(2).Text, "&amp;", "&")
            'txtLocateSerialNo.Text = CType(gdvLocate.SelectedRow.FindControl("hidSerialNo"), HiddenField).Value
            lblLocateRequestID.Text = CType(gdvLocate.SelectedRow.FindControl("hidRequestId"), HiddenField).Value
            lblLocateConsumerID.Text = Replace(gdvLocate.SelectedRow.Cells(1).Text, "&amp;", "&")

            'txtLocateAdjustDte.Text = IIf(CType(gdvLocate.SelectedRow.FindControl("hidAdjustDte"), HiddenField).Value > Date.MinValue, CDate((CType(gdvLocate.SelectedRow.FindControl("hidAdjustDte"), HiddenField).Value)).ToString("dd/MM/yyyy"), EMPTY)
            'lblReturnBy.Text = CType(gdvLocate.SelectedRow.FindControl("lblReturnBy"), Label).Text

            txtLocateAdjustDte.Enabled = False
            ddlLocateDocType.Enabled = False
            'txtLocateSerialNo.Enabled = False
            pnlLocateAdjust.Enabled = True

            txtReceiveDate.Text = IIf(CType(gdvLocate.SelectedRow.FindControl("hidReceiveDte"), HiddenField).Value > Date.MinValue, CDate((CType(gdvLocate.SelectedRow.FindControl("hidReceiveDte"), HiddenField).Value)).ToString("dd/MM/yyyy"), EMPTY)
            If txtReceiveDate.Text = EMPTY Then
                txtReceiveDate.Visible = False
            End If
            lblReceiveBy.Text = CType(gdvLocate.SelectedRow.FindControl("hidReceiveBy"), HiddenField).Value

            lblApprovedBy.Text = CType(gdvLocate.SelectedRow.FindControl("hidApproveBy"), HiddenField).Value
            lblApprovedDate.Text = IIf(CType(gdvLocate.SelectedRow.FindControl("hidApproveDte"), HiddenField).Value > Date.MinValue, CDate((CType(gdvLocate.SelectedRow.FindControl("hidApproveDte"), HiddenField).Value)).ToString("dd/MM/yyyy"), EMPTY)


            Dim Adjust As AdjustDetails = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.AdjustOutList), List(Of AdjustDetails)).Find(Function(i As AdjustDetails) i.AdjustID = lblLocateAdjustID.Text)
            If Adjust Is Nothing Then Throw New NullReferenceException
            Dim AdjustItemList As List(Of AdjustItemDetails)
            Dim ReturnUnitCost As Double = 0.0
            Dim IssueDetailsList As List(Of IssueItemDetails)

            ddlLocateDocType.SelectedValue = Adjust.Type

            If Adjust.ReturnDte = Date.MinValue Then
                txtLocateAdjustDte.Text = Adjust.Dte.ToString("dd/MM/yyyy")
            Else
                txtLocateAdjustDte.Text = Adjust.ReturnDte.ToString("dd/MM/yyyy")
            End If

            lblReturnBy.Text = Adjust.ReturnUserID

            Dim adjustStatus As String = CType(gdvLocate.SelectedRow.FindControl("hidStatus"), HiddenField).Value
            lblStatus.Text = adjustStatus

            Select Case adjustStatus
                Case OPEN
                    lblStatus.Text = "OPEN"

                    'enable edit/approve/reject button when only status is OPEN
                    If IsWithinFinanceCutoffDate(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), Session(ESession.StoreID.ToString), ConvertToDate(txtLocateAdjustDte.Text)) Then

                        pnlLocateAccess.Enabled = True
                        pnlLocateAction.Enabled = True

                        ' disallow approving when login user is the returner - check and prompt user with AO rights only
                        If Session(ESession.UserRoleType.ToString).ToString.Contains(APPROVALOFFICER) Then
                            If Session(ESession.UserID.ToString).ToString = CType(gdvLocate.SelectedRow.FindControl("hidReturnBy"), HiddenField).Value Then
                                Message = GetMessage(messageID.OwnRequest)
                            Else
                                pnlLocateAccess.Enabled = False
                                btnLocateApprove.Enabled = True
                                btnLocateReject.Enabled = True
                            End If
                        ElseIf Session(ESession.UserRoleType.ToString).ToString.Contains(STOREOFFICER) Then
                            pnlLocateAccess.Enabled = False
                            pnlLocateAction.Enabled = False
                        End If
                    End If
                Case CLOSED
                    lblStatus.Text = "CLOSED"
                Case APPROVED
                    If IsWithinFinanceCutoffDate(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), Session(ESession.StoreID.ToString), ConvertToDate(txtLocateAdjustDte.Text)) Then
                        If Not Session(ESession.UserRoleType.ToString).ToString.Contains(APPROVALOFFICER) Then
                            If Session(ESession.UserID.ToString).ToString = CType(gdvLocate.SelectedRow.FindControl("hidReturnBy"), HiddenField).Value Then
                                Message = GetMessage(messageID.OwnRequest)
                            Else
                                pnlLocateAdjustItem.Enabled = True
                                pnlLocateAccess.Enabled = False
                                pnlLocateAction.Enabled = True
                                btnLocateSave.Enabled = True
                                btnLocateCancel.Enabled = True
                                If Session(ESession.UserRoleType.ToString).ToString.Contains(STOREOFFICER) Then
                                    txtReceiveDate.Text = Today.ToString("dd/MM/yyyy")
                                End If
                            End If
                        End If
                    End If

                    lblStatus.Text = "APPROVED"

                Case REJECTED
                    'If IsWithinFinanceCutoffDate(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), Session(ESession.StoreID.ToString), ConvertToDate(txtLocateAdjustDte.Text)) Then
                    '    If Not Session(ESession.UserRoleType.ToString).ToString.Contains(APPROVALOFFICER) Then
                    '        If Session(ESession.UserID.ToString).ToString = CType(gdvLocate.SelectedRow.FindControl("hidReturnBy"), HiddenField).Value Then
                    '            Message = GetMessage(messageID.OwnRequest)
                    '        Else
                    '            pnlLocateAdjustItem.Enabled = True
                    '            pnlLocateAccess.Enabled = False
                    '            pnlLocateAction.Enabled = True
                    '            btnLocateSave.Enabled = True
                    '            btnLocateCancel.Enabled = True
                    '            txtReceiveDate.Text = Today.ToString("dd/MM/yyyy")
                    '        End If
                    '    End If
                    'End If


                    lblStatus.Text = "REJECTED"

                    'Case PENDING
                    '    If IsWithinFinanceCutoffDate(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), Session(ESession.StoreID.ToString), ConvertToDate(txtLocateAdjustDte.Text)) Then
                    '        If Not Session(ESession.UserRoleType.ToString).ToString.Contains(APPROVALOFFICER) Then
                    '            If Session(ESession.UserID.ToString).ToString = CType(gdvLocate.SelectedRow.FindControl("hidReturnBy"), HiddenField).Value Then
                    '                Message = GetMessage(messageID.OwnRequest)
                    '            Else
                    '                pnlLocateAdjustItem.Enabled = True
                    '                pnlLocateAction.Enabled = True
                    '                btnLocateSave.Enabled = True
                    '                btnLocateCancel.Enabled = True
                    '            End If
                    '        End If
                    '    End If

                    '    lblStatus.Text = "APPROVED" ' set APPROVED as status if adjustment is pending for Receive

                Case DELETE
                    lblStatus.Text = "DELETED"
            End Select

            'lblLocateGTotalCost.Text = "0.0000" 'to be computed later

            Using Client As New ServiceClient
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

            DCPLocate.Controls.Clear()
            ViewState.Remove(EViewState.AdjustItemLocate.ToString)
            ViewState(EViewState.AdjustedStockItemIDLocate) = New List(Of String)

            ' Save the selected Adjustment to be used for comparision during saving
            ViewState(EViewState.AdjustOutSelected) = Adjust

            For Each item In AdjustItemList
                Dim DetailsItem As New AdjustItemDetails

                Dim stockItemId As String = item.StockItemID
                Dim IssueItem As New IssueItemDetails
                IssueItem = IssueDetailsList.Find(Function(i As IssueItemDetails) i.StockItemID = stockItemId)

                DetailsItem.TranID = item.TranID
                DetailsItem.StockItemID = item.StockItemID
                DetailsItem.Qty = item.Qty

                If item.TotalCost <> 0 Then
                    DetailsItem.TotalCost = item.TotalCost
                Else
                    DetailsItem.TotalCost = IssueItem.TotalCost
                End If

                DetailsItem.Remarks = item.Remarks
                DetailsItem.AdjustItemID = item.AdjustItemID

                If item.ItemReturn <= 0 Then
                    DetailsItem.ItemReturn = IssueItem.TranID ' transaction id of request item
                Else
                    DetailsItem.ItemReturn = item.ItemReturn  ' transaction id of stock transaction item
                End If

                DetailsItem.BalanceQty = item.BalanceQty
                DetailsItem.MaxLevel = item.MaxLevel
                DetailsItem.Status = item.Status
                'ReturnUnitCost = IIf(DetailsItem.Qty > 0, -CDec(DetailsItem.TotalCost / DetailsItem.Qty), 0)
                ReturnUnitCost = IIf(DetailsItem.Qty <> 0, -CDec(DetailsItem.TotalCost / DetailsItem.Qty), 0)
                ''UpdateGTotal(item.TotalCost)

                'If lblLocateRequestID.Text <> EMPTY Then
                If DetailsItem.ItemReturn > 0 Then
                    Dim IssueItemDetails As IssueItemDetails = IssueDetailsList.Find(Function(i As IssueItemDetails) i.TranID = DetailsItem.ItemReturn)

                    'If (IssueItemDetails.Qty <> 0) Then ReturnUnitCost = IssueItemDetails.TotalCost / IssueItemDetails.Qty
                    If (IssueItemDetails.Qty <> 0) Then ReturnUnitCost = -CDec(IssueItemDetails.TotalCost / IssueItemDetails.Qty)
                    DetailsItem.MaxLevel = (-IssueItemDetails.Qty)
                    DetailsItem.BalanceQty = IssueItemDetails.BalanceQty
                End If
                'End If

                AddAdjustItem(DCPLocate _
                              , DetailsItem _
                              , EViewState.AdjustItemLocate.ToString _
                              , Adjust.Type _
                              , ReturnUnitCost _
                              )

                ' keep a list of Adjusted stock item, to check against to restrict duplicate
                DirectCast(ViewState(EViewState.AdjustedStockItemIDLocate), List(Of String)).Add(DetailsItem.StockItemID)

                uplLocateMainAdjust.Visible = True
                pnlLocateAdjust.Visible = True
                pnlLocateAccess.Visible = True
                uplLocateUserControl.Visible = True
                pnlLocate.Visible = True

                uplLocateAdjust.Visible = True
                uplLocateAdjust.Update()

            Next
        Catch ex As FaultException
            txtLocateAdjustID.Enabled = True
            Message = ex.Message

        Catch ex As NullReferenceException
            txtLocateAdjustID.Enabled = True
            Message = GetMessage(messageID.InvalidValue, "Return Reference")

        Catch ex As Exception
            txtLocateAdjustID.Enabled = True
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
        'ddlLocateAdjustID.SelectedIndex = -1
        txtLocateAdjustID.Text = EMPTY

        ' Enable/Disable control for editing
        'ddlLocateAdjustID.Enabled = True
        txtLocateAdjustID.Enabled = True
        pnlLocateAccess.Enabled = False
        pnlLocateAdjust.Enabled = False
        pnlLocateAdjustItem.Enabled = False
        pnlLocateAction.Enabled = False

        pnlLocate.Visible = False
        pnlSearchResults.Visible = False
        ddlLocateStatus.Enabled = True
        ddlLocateConsumerSearch.Enabled = True

        btnLocateApprove.Enabled = False
        btnLocateReject.Enabled = False
        btnLocateSave.Enabled = False
        btnLocateCancel.Enabled = False


        'ddlLocateStockCode.Items.Clear()
        'txtLocateStockCode.Text = String.Empty
        'uplLocateStockCode.Visible = False

        uplLocateAdjust.Visible = True
        uplLocateAdjust.Update()


        DCPLocate.Controls.Clear()
        ViewState.Remove(EViewState.Mode)
        ViewState.Remove(EViewState.AdjustedStockItemIDLocate)
    End Sub

    Protected Sub btnLocateEdit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLocateEdit.Click
        ' Enable/Disable control for editing
        txtLocateAdjustDte.Enabled = True
        ddlLocateDocType.Enabled = True
        'txtLocateSerialNo.Enabled = True
        pnlLocateAdjust.Enabled = True
        pnlLocateAccess.Enabled = False
        btnLocateSave.Enabled = True
        btnLocateCancel.Enabled = True

        pnlLocateAdjustItem.Enabled = True
        pnlLocateAction.Enabled = True
        btnLocateApprove.Enabled = False 'disable approve/reject when item is on edit mode
        btnLocateReject.Enabled = False

        uplLocateAdjust.Update()
    End Sub

    'Protected Sub btnLocateAddItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLocateAddItem.Click
    '    Try
    '        Dim MoreItemInfo As MoreItemInfoDetails

    '        If txtLocateStockCode.Text.Trim <> EMPTY Then

    '            '-- VALIDATE STOCK CODE
    '            Dim StockCode As String
    '            StockCode = Split(txtLocateStockCode.Text, " | ")(0).Trim

    '            Using Client As New ServiceClient

    '                Dim ItemDetails As New ItemDetails
    '                ItemDetails.StoreID = Session("StoreID").ToString
    '                ItemDetails.ItemID = StockCode

    '                If Not Client.IsValidStockCode(ItemDetails) Then

    '                    Message = GetMessage(messageID.InvalidStockCode, StockCode.ToUpper)
    '                    Client.Close()
    '                    Exit Sub

    '                End If

    '                If Not Client.IsValidStatus(ItemDetails) Then

    '                    Message = GetMessage(messageID.StockCodeClosed, StockCode.ToUpper)
    '                    Client.Close()
    '                    Exit Sub

    '                End If

    '                Client.Close()

    '            End Using

    '            Dim AdjustItemDetailsItem As New AdjustItemDetails

    '            Dim Adjusted As Boolean = DirectCast(ViewState(EViewState.AdjustedStockItemIDLocate), List(Of String)).Exists(Function(i As String) (i = StockCode))

    '            If Adjusted Then
    '                Throw New ApplicationException(GetMessage(messageID.StockCodeHasOrdered, StockCode))
    '            End If

    '            AdjustItemDetailsItem.StockItemID = StockCode

    '            ' UAT02.47 - retrieve the current balance for display
    '            Using Client As New ServiceClient
    '                MoreItemInfo = Client.GetMoreItemInfo(Session(ESession.StoreID.ToString) _
    '                                                      , StockCode _
    '                                                      , Today _
    '                                                      )
    '                AdjustItemDetailsItem.BalanceQty = MoreItemInfo.Balance
    '                AdjustItemDetailsItem.MaxLevel = MoreItemInfo.MaxLevel
    '            End Using

    '            ' UAT02 - set mode as Insert
    '            AdjustItemDetailsItem.Mode = INSERT

    '            AddAdjustItem(DCPLocate _
    '                          , AdjustItemDetailsItem _
    '                          , EViewState.AdjustItemLocate.ToString _
    '                          , lblLocateDocType.Text _
    '                          )

    '            ' keep a list of Adjusted stock item, to check against to restrict duplicate
    '            DirectCast(ViewState(EViewState.AdjustedStockItemIDLocate), List(Of String)).Add(AdjustItemDetailsItem.StockItemID)

    '        Else
    '            ' alert message
    '            Throw New ApplicationException(GetMessage(messageID.StockCodeNotSelected))
    '        End If

    '    Catch ex As ApplicationException
    '        Message = ex.Message

    '    Catch ex As FaultException
    '        Message = ex.Message

    '    Catch ex As Exception
    '        Message = GetMessage(messageID.TryLastOperation)

    '        Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
    '        If (rethrow) Then Throw
    '    End Try
    'End Sub

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
            Adjust.Type = ddlLocateDocType.SelectedValue
            'Adjust.SerialNo = Trim(txtLocateSerialNo.Text)
            'Adjust.Dte = ConvertToDate(txtLocateAdjustDte.Text)
            Adjust.Dte = ConvertToDate(Today.ToString("dd/MM/yyyy"))
            Adjust.ReturnUserID = lblReturnBy.Text
            Adjust.ReturnDte = ConvertToDate(txtLocateAdjustDte.Text)
            Adjust.ApproveUserID = lblApprovedBy.Text
            Adjust.ApproveDte = ConvertToDate(lblApprovedDate.Text)
            Adjust.ReceiveUserID = lblReceiveBy.Text
            Adjust.ReceiveDte = ConvertToDate(txtReceiveDate.Text)
            Adjust.LoginUser = Session(ESession.UserID.ToString).ToString
            Adjust.DocReturn = lblLocateRequestID.Text
            Adjust.InvolveID = lblLocateConsumerID.Text

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
            If (AdjustDte > Today) Then Throw New ApplicationException(GetMessage(messageID.MoreLessThan, , , "Obsolete/Damage Date", Today.ToString("dd/MM/yyyy"), "earlier or same as"))
            If Not (IsWithinFinanceCutoffDate(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), Session(ESession.StoreID.ToString), AdjustDte)) Then Throw New ApplicationException(GetMessage(messageID.NotInFinancial, "Obsolete/Damage Date"))

            ' check if adjust is updated
            If (DirectCast(ViewState(EViewState.AdjustOutSelected), AdjustDetails).SerialNo <> Adjust.SerialNo _
                Or DirectCast(ViewState(EViewState.AdjustOutSelected), AdjustDetails).Dte <> Adjust.Dte _
                Or DirectCast(ViewState(EViewState.AdjustOutSelected), AdjustDetails).Type <> Adjust.Type _
                ) Then
                AdjustUpdated = True
            End If

            Adjust.Status = lblStatus.Text.Substring(0, 1)

            Dim MessageItemReceived As String = EMPTY
            ' check if adjust is received
            '            If txtReceiveDate.Text <> EMPTY And Session(ESession.UserRoleType.ToString).ToString.Contains(APPROVALOFFICER) Then
            If txtReceiveDate.Text <> EMPTY Then
                Adjust.Status = CLOSED 'Adjustment is Received and 'Closed' 
                Adjust.ReceiveUserID = Session(ESession.UserID.ToString).ToString
                AdjustUpdated = True

                Message = GetMessage(messageID.Success, "Received", String.Format("Stock Obsolete/Damage Request[{0}]", lblLocateAdjustID.Text))
                MessageItemReceived = Message
            Else
                ' check if adjust is updated
                If (DirectCast(ViewState(EViewState.AdjustOutSelected), AdjustDetails).SerialNo <> Adjust.SerialNo _
                    Or DirectCast(ViewState(EViewState.AdjustOutSelected), AdjustDetails).Dte <> Adjust.Dte _
                    Or DirectCast(ViewState(EViewState.AdjustOutSelected), AdjustDetails).Type <> Adjust.Type _
                    ) Then
                    AdjustUpdated = True
                End If
            End If


            ' check list count
            DetailsList = RetrieveDCPInfo(EViewState.AdjustItemLocate.ToString, Adjust.Type)

            ' Proceed with update when either Adjust or AdjustItem changed
            If DetailsList.Count > 0 Or AdjustUpdated Then
                Using Client As New ServiceClient
                    ' validate serial no is unique
                    'If (Trim(txtLocateSerialNo.Text) <> EMPTY _
                    '    And DirectCast(ViewState(EViewState.AdjustOutSelected), AdjustDetails).SerialNo <> Adjust.SerialNo _
                    '    ) Then
                    '    If Not (Client.FieldIsUnique(Session(ESession.StoreID.ToString) _
                    '                                 , ServiceColumnName.AdjustOutID _
                    '                                 , Trim(txtLocateSerialNo.Text) _
                    '                                 , lblLocateAdjustID.Text _
                    '                                 ) _
                    '            ) Then Throw New ApplicationException(GetMessage(messageID.FieldNotUnique, "Serial No"))
                    'End If

                    Message = Client.UpdateAdjust(Adjust _
                                                  , DetailsList _
                                                  )
                End Using

                If Message = EMPTY Then
                    If MessageItemReceived <> EMPTY Then ' display message item is received if return is approved and received
                        Message = MessageItemReceived
                    Else
                        Message = GetMessage(messageID.Success, "Edited", String.Format("Obsolete/Damage item(s)[{0}]", Adjust.AdjustID))
                    End If

                    ' Add edited Adjustment to list and rebind (if needed)
                    EditCache(Cache(Session(ESession.StoreID.ToString) & ECache.AdjustOutList) _
                          , GetType(AdjustDetails) _
                          , Adjust _
                          , True _
                          )

                    If Adjust.Status = CLOSED Then
                        ' bind to report items
                        Dim ClosedObsoleteDamage As List(Of AdjustDetails) = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.AdjustOutList), List(Of AdjustDetails)).FindAll(Function(item As AdjustDetails) item.Status = CLOSED)
                        BindDropDownList(ddlRptDocumentNo, ClosedObsoleteDamage, "AdjustID", "AdjustID")
                        uplPrintAdjust.Update()
                    End If

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
        ' Reload original info
        btnLocateGo_Click(sender, e)
    End Sub

    Protected Sub btnLocateApprove_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLocateApprove.Click
        UpdateAdjustStatus(lblLocateAdjustID.Text _
                            , APPROVED _
                            , lblReturnBy.Text _
                            , ConvertToDate(txtLocateAdjustDte.Text) _
                            , Session(ESession.UserID.ToString).ToString _
                            , Today _
                            , lblReceiveBy.Text _
                            , ConvertToDate(txtReceiveDate.Text) _
                             )

        Message = GetMessage(messageID.Success, "Approved", String.Format("Stock Obsolete/Damage Request[{0}]", lblLocateAdjustID.Text))

        ' Reset Control n Clear screen
        btnLocateClear_Click(Nothing, Nothing)
    End Sub

    Private Sub btnLocateReject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLocateReject.Click
        UpdateAdjustStatus(lblLocateAdjustID.Text _
                                  , REJECTED _
                                  , lblReturnBy.Text _
                                  , ConvertToDate(txtLocateAdjustDte.Text) _
                                  , Session(ESession.UserID.ToString).ToString _
                                  , Today _
                                  , lblReceiveBy.Text _
                                  , ConvertToDate(txtReceiveDate.Text) _
                                   )

        Message = GetMessage(messageID.Success, "Rejected", String.Format("Stock Obsolete/Damage Request[{0}]", lblLocateAdjustID.Text))

        ' Reset Control n Clear screen
        btnLocateClear_Click(Nothing, Nothing)
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
                                              , lblLocateAdjustID.Text _
                                              , ddlLocateDocType.SelectedValue _
                                              , ConvertToDate(txtLocateAdjustDte.Text) _
                                              , Session(ESession.UserID.ToString).ToString _
                                              )
            End Using

            If Message = EMPTY Then
                'ddlLocateAdjustID.Items.Remove(ddlLocateAdjustID.SelectedValue)
                'ddlRptDocumentNo.Items.Remove(ddlLocateAdjustID.SelectedValue)
                ddlRptDocumentNo.Items.Remove(lblLocateAdjustID.Text)

                ' remove deleted order from cache
                Dim AdjustDetailsItem As AdjustDetails = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.AdjustOutList), List(Of AdjustDetails)).Find(Function(i As AdjustDetails) i.AdjustID = lblLocateAdjustID.Text)
                If AdjustDetailsItem IsNot Nothing Then DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.AdjustOutList), List(Of AdjustDetails)).Remove(AdjustDetailsItem)

                'clear screen
                btnLocateClear_Click(sender, e)

                'display Alert Message
                Message = GetMessage(messageID.Success, "deleted", "Obsolete or Damage")
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

#Region "Print Tab"
    Protected Sub btnPDF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPDF.Click
        If Not ValidPrint() Then
            Exit Sub
        End If

        rvr.LocalReport.DataSources.Clear()
        rvr.LocalReport.DataSources.Add(New ReportDataSource("AdjustList", ObjectDataSource1))

        Dim parameterlist As New List(Of ReportParameter)
        Dim p1 As New ReportParameter("PlantName", Session("StoreName").ToString())

        If txtDateFrom.Text.Trim() = EMPTY Then
            txtDateFrom.Text = Today.ToString("dd/MM/yyyy")
        End If
        Dim p2 As New ReportParameter("PODateFrom", txtDateFrom.Text)

        If txtDateTo.Text.Trim() = EMPTY Then
            txtDateTo.Text = Today.ToString("dd/MM/yyyy")
        End If
        Dim p3 As New ReportParameter("PODateTo", txtDateTo.Text)

        Dim p4 As New ReportParameter("ReportTitle", "Stock Obsolescence & Damage Report")
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
        Response.AddHeader("content-disposition", "attachment;filename=StockReturn-ObsolDamage.pdf")
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

        If txtDateFrom.Text.Trim() = EMPTY Then
            txtDateFrom.Text = Today.ToString("dd/MM/yyyy")
        End If
        Dim p2 As New ReportParameter("PODateFrom", txtDateFrom.Text)

        If txtDateTo.Text.Trim() = EMPTY Then
            txtDateTo.Text = Today.ToString("dd/MM/yyyy")
        End If
        Dim p3 As New ReportParameter("PODateTo", txtDateTo.Text)

        Dim p4 As New ReportParameter("ReportTitle", "Stock Obsolescence & Damage Report")
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
        Response.AddHeader("content-disposition", "attachment;filename=StockReturn-ObsolDamage.xls")
        Response.BinaryWrite(bytValue)
        Response.Flush()
        Response.End()
    End Sub

    Protected Sub ObjectDataSource1_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs) Handles ObjectDataSource1.Selecting
        e.InputParameters("storeID") = Session("StoreID")
        e.InputParameters("type") = ADJUSTODL
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
    ''' 28Feb09 - KG;
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
                    'txtLocateSerialNo.Enabled = enabled
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
    ''' Adjust Qty is stored and retrieve as negative value, convert to positive for display;
    ''' 12Feb09 - KG;
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
            Dim AdjustReturnItem = New AdjustReturnItem
            Dim ItemDetails As ItemDetails = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), List(Of ItemDetails)).Find(Function(i As ItemDetails) i.ItemID = AdjustItemDetails.StockItemID)

            If IsNothing(ViewState(UserControlName)) Then
                ViewState(UserControlName) = 1
            Else
                ViewState(UserControlName) += 1
            End If

            ' load user control at top of placeholder
            AdjustReturnItem = LoadControl("AdjustReturnItem.ascx")
            AdjustReturnItem.ID = UserControlName + Convert.ToString(ViewState(UserControlName))
            PlaceHolder.Controls.AddAt(0, AdjustReturnItem)

            ' clear MoreItemInfo cache
            ViewState.Remove(AdjustReturnItem.UniqueID)

            ' Assign value to controls
            DirectCast(AdjustReturnItem.FindControl("hfMode"), HiddenField).Value = AdjustItemDetails.Mode
            DirectCast(AdjustReturnItem.FindControl("hfTranID"), HiddenField).Value = AdjustItemDetails.TranID
            DirectCast(AdjustReturnItem.FindControl("hfTranType"), HiddenField).Value = adjustType
            DirectCast(AdjustReturnItem.FindControl("hfAdjustItemStatus"), HiddenField).Value = AdjustItemDetails.Status
            DirectCast(AdjustReturnItem.FindControl("hfAdjustItemID"), HiddenField).Value = AdjustItemDetails.AdjustItemID
            DirectCast(AdjustReturnItem.FindControl("hfItemReturn"), HiddenField).Value = AdjustItemDetails.ItemReturn
            DirectCast(AdjustReturnItem.FindControl("hfBalanceQty"), HiddenField).Value = AdjustItemDetails.BalanceQty
            DirectCast(AdjustReturnItem.FindControl("hfMaxLevel"), HiddenField).Value = AdjustItemDetails.MaxLevel

            DirectCast(AdjustReturnItem.FindControl("hfOrgAdjustQty"), HiddenField).Value = (AdjustItemDetails.Qty)
            DirectCast(AdjustReturnItem.FindControl("hfOrgTotalCost"), HiddenField).Value = (AdjustItemDetails.TotalCost)
            DirectCast(AdjustReturnItem.FindControl("hfOrgRemarks"), HiddenField).Value = AdjustItemDetails.Remarks

            DirectCast(AdjustReturnItem.FindControl("lblStockCode"), Label).Text = AdjustItemDetails.StockItemID
            DirectCast(AdjustReturnItem.FindControl("lblDescription"), Label).Text = ItemDetails.ItemDescription
            DirectCast(AdjustReturnItem.FindControl("lblUOM"), Label).Text = ItemDetails.UOM

            DirectCast(AdjustReturnItem.FindControl("txtAdjustQty"), TextBox).Text = IIf(AdjustItemDetails.Qty <> 0, (AdjustItemDetails.Qty).ToString("0.00"), EMPTY)
            'DirectCast(AdjustReturnItem.FindControl("txtAdjustQty"), TextBox).Enabled = False

            DirectCast(AdjustReturnItem.FindControl("txtReceiveQty"), TextBox).Text = IIf(AdjustItemDetails.Qty <> 0, (AdjustItemDetails.Qty).ToString("0.00"), EMPTY)

            ' Store Officer Received Rights
            'DirectCast(AdjustReturnItem.FindControl("txtReceiveQty"), TextBox).Enabled = False
            If Session(ESession.UserRoleType.ToString).ToString.Contains(STOREOFFICER) And AdjustItemDetails.Status = PENDING Then

            End If

            DirectCast(AdjustReturnItem.FindControl("txtRemarks"), TextBox).Text = AdjustItemDetails.Remarks

            If Session(ESession.UserRoleType.ToString).ToString.Contains(STOREOFFICER) Then
                DirectCast(AdjustReturnItem.FindControl("txtAdjustQty"), TextBox).Enabled = False ' disable adjust qty for SO
                DirectCast(AdjustReturnItem.FindControl("txtReceiveQty"), TextBox).Enabled = True
                DirectCast(AdjustReturnItem.FindControl("checkReceived"), CheckBox).Enabled = True
            Else
                DirectCast(AdjustReturnItem.FindControl("txtAdjustQty"), TextBox).Enabled = True
                DirectCast(AdjustReturnItem.FindControl("txtReceiveQty"), TextBox).Enabled = False
                DirectCast(AdjustReturnItem.FindControl("checkReceived"), CheckBox).Enabled = False
            End If


            If AdjustItemDetails.Status = CLOSED Then
                DirectCast(AdjustReturnItem.FindControl("checkReceived"), CheckBox).Checked = True
            Else
                DirectCast(AdjustReturnItem.FindControl("checkReceived"), CheckBox).Checked = False
            End If

            '  Client Side Java Scripting
            'DirectCast(AdjustReturnItem.FindControl("txtAdjustQty"), TextBox).Attributes.Add("onkeyup", "computeTotal2('" & DirectCast(AdjustReturnItem.FindControl("txtAdjustQty"), TextBox).ClientID & "','" & DirectCast(AdjustReturnItem.FindControl("lblUnitCost"), Label).ClientID & "','" & DirectCast(AdjustReturnItem.FindControl("txtTotalCost"), TextBox).ClientID & "');")
            '  truncate value to 4 decimal place for display
            'DirectCast(AdjustReturnItem.FindControl("lblUnitCost"), Label).Text = DisplayValue(returnUnitCost)

            ' ''  Client Side Java Scripting
            ''DirectCast(AdjustItem.FindControl("txtAdjustQty"), TextBox).Attributes.Add("onkeyup", "computeUnit('" & DirectCast(AdjustItem.FindControl("txtAdjustQty"), TextBox).ClientID & "','" & DirectCast(AdjustItem.FindControl("txtTotalCost"), TextBox).ClientID & "','" & DirectCast(AdjustItem.FindControl("lblUnitCost"), Label).ClientID & "');")
            ''DirectCast(AdjustItem.FindControl("txtTotalCost"), TextBox).Attributes.Add("onkeyup", "computeUnit('" & DirectCast(AdjustItem.FindControl("txtAdjustQty"), TextBox).ClientID & "','" & DirectCast(AdjustItem.FindControl("txtTotalCost"), TextBox).ClientID & "','" & DirectCast(AdjustItem.FindControl("lblUnitCost"), Label).ClientID & "');")
            ' ''  truncate value to 4 decimal place for display
            ''DirectCast(AdjustItem.FindControl("lblUnitCost"), Label).Text = IIf(AdjustItemDetails.Qty <> 0, DisplayValue(AdjustItemDetails.TotalCost / AdjustItemDetails.Qty), "0.0000")

            DirectCast(AdjustReturnItem.FindControl("hfUnitCost"), HiddenField).Value = DisplayValue(returnUnitCost)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Used by the User Control AdjustItem to display its Stock Item info;
    ''' 18 Feb 2009 - KG;
    ''' </summary>
    ''' <param name="uniqueID"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Friend Sub ViewStock(ByVal uniqueID As String, ByVal stockItemID As String, ByVal stockItemDesc As String, ByVal stockItemUOM As String, ByVal adjustQtyDiff As Decimal, Optional ByVal totalDiff As Double = 0.0)
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

            ' Adjustment Outward, hence substract from the current value
            Dim BalanceNew As Decimal = MoreItem.Balance - adjustQtyDiff
            Dim TotalValueNew As Double = MoreItem.TotalValue - totalDiff
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

        Catch ex As Exception
            Throw ex
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

                ' Locate TAB currently cannot delete
            ElseIf tbcAdjustItem.ActiveTab.ID = LOCATETAB Then
                ' Check this deletion doesn't cause a negative value to the stockitem
                ' User control will check the above and disable the button when neccessary
                If True Then
                    If DirectCast(AdjustItem.FindControl("hfMode"), HiddenField).Value = INSERT Then
                        DCPLocate.Controls.Remove(FindControl(sender))
                    Else
                        DirectCast(AdjustItem.FindControl("hfMode"), HiddenField).Value = DELETE
                        AdjustItem.Visible = False
                    End If
                    ' remove stockcode from adjusted list
                    DirectCast(ViewState(EViewState.AdjustedStockItemIDLocate), List(Of String)).Remove(DirectCast(AdjustItem.FindControl("lblStockCode"), Label).Text)

                    uplLocateUserControl.Update()
                Else
                    Message = "This action will cause the Stock Item Qty to fall below Zero value."
                End If

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
    ''' 2)Adjust out Qty is display as positive value, convert to negative for storage;
    ''' 28Feb09 - KG;
    ''' </summary>
    ''' <param name="UserControlName"></param>
    ''' <returns>list of Adjustitemdetails</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Function RetrieveDCPInfo(ByVal UserControlName As String, ByVal adjustType As String) As List(Of AdjustItemDetails)
        Dim AdjustReturnItem As AdjustReturnItem
        Dim DetailsItem As AdjustItemDetails
        Dim DetailsList As List(Of AdjustItemDetails)

        ' Request/Adjust item Details
        DetailsList = New List(Of AdjustItemDetails)
        For i As Integer = 1 To ViewState(UserControlName)
            DetailsItem = New AdjustItemDetails
            If tbcAdjustItem.ActiveTab.ID = NEWTAB Then
                AdjustReturnItem = TryCast(DCP.FindControl(UserControlName + i.ToString), AdjustReturnItem)
                DetailsItem.Mode = INSERT ' default mode to insert for adjustment creation

            ElseIf tbcAdjustItem.ActiveTab.ID = LOCATETAB Then
                AdjustReturnItem = TryCast(DCPLocate.FindControl(UserControlName + i.ToString), AdjustReturnItem)
            Else
                Throw New Exception("something wrong?")
            End If

            If AdjustReturnItem IsNot Nothing Then
                Dim Value As String

                ' UAT02 capture user control exception
                If AdjustReturnItem.Message <> EMPTY Then Throw New ApplicationException(AdjustReturnItem.Message)

                ' validate mandatory fields
                If txtReceiveDate.Text.Trim() <> EMPTY Then
                    If DirectCast(AdjustReturnItem.FindControl("checkReceived"), CheckBox).Checked = False Then _
                        'And DirectCast(AdjustReturnItem.FindControl("hfTranID"), HiddenField).Value <> "0" Then
                        Throw New ApplicationException(GetMessage(messageID.MandatoryField))
                    End If
                End If

                DetailsItem.TranID = CInt(DirectCast(AdjustReturnItem.FindControl("hfTranID"), HiddenField).Value)

                DetailsItem.StockItemID = DirectCast(AdjustReturnItem.FindControl("lblStockCode"), Label).Text
                DetailsItem.AdjustItemID = CInt(DirectCast(AdjustReturnItem.FindControl("hfAdjustItemID"), HiddenField).Value)
                DetailsItem.ItemReturn = CInt(DirectCast(AdjustReturnItem.FindControl("hfItemReturn"), HiddenField).Value)
                DetailsItem.Remarks = Trim(DirectCast(AdjustReturnItem.FindControl("txtRemarks"), TextBox).Text)
                DetailsItem.Status = OPEN

                ' check if adjust is received
                If DirectCast(AdjustReturnItem.FindControl("checkReceived"), CheckBox).Checked = True Then
                    DetailsItem.Status = CLOSED ' adjustment is received
                    DetailsItem.Mode = UPDATE

                    'update adjust qty to actual adjustment received qty
                    Value = Trim(DirectCast(AdjustReturnItem.FindControl("txtReceiveQty"), TextBox).Text)
                    If IsNumeric(Value) Then DetailsItem.Qty = -CDec(Value)
                Else
                    DetailsItem.Status = OPEN ' adjustment is amended
                    Value = Trim(DirectCast(AdjustReturnItem.FindControl("txtAdjustQty"), TextBox).Text)
                    If IsNumeric(Value) Then DetailsItem.Qty = -CDec(Value)
                End If

               
                'Value = Trim(DirectCast(AdjustReturnItem.FindControl("txtTotalCost"), TextBox).Text)
                'If IsNumeric(Value) Then DetailsItem.TotalCost = -CDec(Value)
                ' compute total cost
                Value = Trim((Math.Floor(DirectCast(AdjustReturnItem.FindControl("hfUnitCost"), HiddenField).Value * DetailsItem.Qty * Math.Pow(10, 2)) / Math.Pow(10, 2)).ToString)
                If IsNumeric(Value) Then DetailsItem.TotalCost = -CDec(Value)

                If DetailsItem.Mode = INSERT Then
                    If DetailsItem.Qty = 0 Then DetailsItem.Mode = EMPTY
                Else
                    ' Set Item Mode
                    If DetailsItem.TranID = 0 And DetailsItem.AdjustItemID = 0 Then
                        If DetailsItem.Qty <> 0 Then DetailsItem.Mode = INSERT

                    Else
                        If DetailsItem.Qty = 0 Then
                            DetailsItem.Mode = DELETE

                        Else
                            ' Update when screen value diff from original value
                            If (CDec(DirectCast(AdjustReturnItem.FindControl("hfOrgAdjustQty"), HiddenField).Value) <> DetailsItem.Qty _
                                Or DirectCast(AdjustReturnItem.FindControl("hfOrgRemarks"), HiddenField).Value <> DetailsItem.Remarks _
                                   Or txtReceiveDate.Text.Trim() <> EMPTY _
                                ) Then
                                'Or DirectCast(ViewState(EViewState.AdjustOutSelected), AdjustDetails).Type <> adjustType _
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

    Private Sub UpdateAdjustStatus(ByVal adjustID As String, ByVal status As String, _
                                     Optional ByVal returnBy As String = "", Optional ByVal returnDte As Date = #12:00:00 AM#, _
                                     Optional ByVal approveBy As String = "", Optional ByVal approveDte As Date = #12:00:00 AM#, _
                                     Optional ByVal receiveBy As String = "", Optional ByVal receiveDte As Date = #12:00:00 AM# _
                                    )
        Try
            'update status to database
            Using Client As New ServiceClient
                Client.UpdateAdjustStatus(Session(ESession.StoreID.ToString) _
                                           , adjustID _
                                           , ddlLocateDocType.SelectedValue _
                                           , status _
                                           , Session(ESession.UserID.ToString) _
                                           , returnBy _
                                           , returnDte _
                                           , approveBy _
                                           , approveDte _
                                           , receiveBy _
                                           , receiveDte _
                                           )
            End Using
        Catch ex As FaultException
            Message = ex.Message

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try

    End Sub

#End Region

End Class