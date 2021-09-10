Imports Microsoft.Reporting.WebForms
Imports System.Web.Services.Description
Imports NEA_ICS.UserInterface.NEA_ICS.WcfService
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ServiceModel
Imports DBauer.Web.UI.WebControls
Imports System.Web.Services
Imports System.Reflection

''' <summary>
''' Code behind for frmReceiveItem Page;
''' 11Feb09 - KG;
''' </summary>
''' <remarks>
''' CHANGE LOG:
''' ddMMMyy  AuthorName  RefID  Description;
''' </remarks>
Partial Public Class frmReceiveItem
    Inherits clsCommonFunction

#Region " Page Control "
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

    ''' <summary>
    ''' Page_Load;
    ''' 1)Assign controls to user base on Access Rights;
    ''' 2)Retrieve Unfulfilled Order list;
    ''' 3)Bind dropdownlists with Lists;
    ''' 24Feb09 - KG;
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

                aceStockCodeFrom.ContextKey = Session("StoreID").ToString
                aceStockCodeTo.ContextKey = Session("StoreID").ToString

                ' @@@ START OF ACCESS RIGHTS @@@
                Dim AccessRights As New List(Of RoleDetails)

                tbcReceiveItem.Visible = False
                AccessRights = clsCommonFunction.AssignAccessRights(Cache(Session(ESession.UserID.ToString) & Session(ESession.StoreID.ToString) & "AccessRights") _
                                                                    , moduleID.ReceivedItem _
                                                                    )

                ' Select Rights
                tbpLocate.Visible = False
                tbpPrint.Visible = False
                If AccessRights(0).SelectRight Then
                    tbpLocate.Visible = True
                    tbpPrint.Visible = True
                    tbcReceiveItem.Visible = True
                    tbcReceiveItem.ActiveTabIndex = 1 ' Locate tab
                    MainPanel(True)
                    tbpLocate.Focus()

                Else
                    Server.Transfer("..\frmUnauthorisedPage.aspx")
                    Exit Sub
                End If


                ' Insert Rights
                tbpNew.Visible = False
                If AccessRights(0).InsertRight Then
                    tbpNew.Visible = True
                    tbcReceiveItem.ActiveTabIndex = 0 ' New tab
                    MainPanel(True)
                    tbpNew.Focus()
                End If


                ' Update or Delete Rights
                pnlLocateAccess.Visible = False
                btnLocateEdit.Visible = False
                btnLocateDeleteAll.Visible = False

                '  Update Rights
                If AccessRights(0).UpdateRight Then
                    pnlLocateAccess.Visible = True
                    btnLocateEdit.Visible = True
                End If

                '  Delete Rights
                If AccessRights(0).DeleteRight Then
                    pnlLocateAccess.Visible = True
                    btnLocateDeleteAll.Visible = True
                End If
                ' @@@ END OF ACCESS RIGHTS @@@


                ' Disable All tab Control first
                tbpNew.Enabled = False
                tbpLocate.Enabled = False
                tbpPrint.Enabled = False


                ' retrieve Order List - status = All
                GetOrderList(Cache(Session(ESession.StoreID.ToString) & ECache.OrderList), Session(ESession.StoreID.ToString), ALL)

                If CacheListIsEmpty(Cache(Session(ESession.StoreID.ToString) & ECache.OrderList), GetType(OrderDetails)) Then
                    Message = String.Format("No Order placed in the System.")

                Else
                    ' New Tab
                    If AccessRights(0).InsertRight Then
                        ' retrieve Unfulfilled Order list - filtered criteria by status <> Closed
                        Cache(Session(ESession.StoreID.ToString) & ECache.UnfulfilledOrder) = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.OrderList), List(Of OrderDetails)).FindAll(Function(i As OrderDetails) i.Status <> CLOSED)

                        If CacheListIsEmpty(Cache(Session(ESession.StoreID.ToString) & ECache.UnfulfilledOrder), GetType(OrderDetails)) Then
                            ' Alert when user has Insert Rights
                            Message = String.Format("No Order pending to receive.")

                        Else
                            ' Check Item list
                            If CacheListIsEmpty(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), GetType(ItemDetails)) Then GetItemList(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), Session(ESession.StoreID.ToString))

                            BindDropDownList(ddlOrderID, Cache(Session(ESession.StoreID.ToString) & ECache.UnfulfilledOrder), "OrderID", "OrderID")
                            txtReceiveDate.Text = Today.ToString("dd/MM/yyyy")
                            '   to allow Exit button click on Modal to invoke page postback
                            btnExit.OnClientClick = String.Format("postbackFromJS('{0}', '{1}')", btnExit.UniqueID, EMPTY)
                            tbpNew.Enabled = True
                        End If
                    End If

                    ' retrieve Received Order list - filtered criteria by status <> Open
                    Cache(Session(ESession.StoreID.ToString) & ECache.ReceivedOrder) = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.OrderList), List(Of OrderDetails)).FindAll(Function(i As OrderDetails) i.Status <> OPEN)

                    ' Locate Tab
                    BindDropDownList(ddlLocateOrderID, Cache(Session(ESession.StoreID.ToString) & ECache.ReceivedOrder), "OrderID", "OrderID")
                    '' cbxLocateFullFilled.Checked = False
                    '  to allow Exit button click on Modal to invoke page postback
                    btnLocateExit.OnClientClick = String.Format("postbackFromJS('{0}', '{1}')", btnLocateExit.UniqueID, EMPTY)
                    tbpLocate.Enabled = True

                    ' Report Tab 
                    btnClear_Click(Nothing, Nothing)
                    'BindDropDownList(ddlStockCodeFrom, Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), "ItemID", "ItemID_Description")
                    'BindDropDownList(ddlStockCode2, Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), "ItemID", "ItemID_Description")
                    BindDropDownList(ddlOrderReference, Cache(Session(ESession.StoreID.ToString) & ECache.ReceivedOrder), "OrderID", "OrderID")
                    tbpPrint.Enabled = True
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
    ''' <summary>
    ''' btnGo_Click;
    ''' 1)Receive only those order item with outstanding;
    ''' 2)Add user control to placeholder;
    ''' 24Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGo.Click
        Try
            Dim List As List(Of OrderItemDetails)
            Dim ReceiveItemDetails As New ReceiveItemDetails

            ' Populate record, reset control 1st
            DCP.Controls.Clear()
            ViewState.Remove(EViewState.ReceiveItem.ToString)
            If MainIsValid(ddlOrderID, Trim(txtReceiveDate.Text)) Then
                ' Get only unfulfilled Order Item records
                Using Client = New ServiceClient
                    List = Client.GetOrderItem(Session(ESession.StoreID.ToString).ToString _
                                               , ddlOrderID.SelectedValue _
                                               , True)
                End Using
                If List.Count = 0 Then
                    ' remove from the unfulfilled order list and rebind
                    Dim OrderID As String = ddlOrderID.SelectedValue
                    DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.UnfulfilledOrder), List(Of OrderDetails)).RemoveAt(ddlOrderID.SelectedIndex - 1)
                    BindDropDownList(ddlOrderID, Cache(Session(ESession.StoreID.ToString) & ECache.UnfulfilledOrder), "OrderID", "OrderID")
                    Throw New ApplicationException(String.Format("All Order item is received for this Order Reference: [{0}]", OrderID))
                End If

                ' transfer OrderItem info to ReceiveItem
                For Each item In List
                    'ReceiveItemDetails = New ReceiveItemDetails
                    ReceiveItemDetails.StockItemID = item.StockItemID
                    ReceiveItemDetails.OrderItemID = item.OrderItemID
                    ReceiveItemDetails.OrderItemWarrantyDte = item.WarrantyDte
                    ReceiveItemDetails.OrderItemQtyOutstanding = item.Qty - item.ReceiveItemQtyReceived
                    ReceiveItemDetails.OrderItemUnitCost = IIf(item.Qty > 0D, (item.TotalCost / item.Qty), 0D)
                    ReceiveItemDetails.Mode = INSERT

                    ' add receive user control
                    AddReceiveItem(DCP, ReceiveItemDetails, EViewState.ReceiveItem.ToString)
                Next

                MainPanel(False)
            Else

                MainPanel(True)
            End If

        Catch ex As FaultException
            'Dim fault As ServiceFault = ex.Data
            'Message = fault.MessageText
            Message = ex.Message

        Catch ex As ApplicationException
            Message = ex.Message

        Catch ex As NullReferenceException
            Message = GetMessage(messageID.InvalidValue, "Order Reference")
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    ''' <summary>
    ''' btnSave_Click;
    ''' Save data to database;
    ''' 1)call a function to Collect receive information;
    ''' 2)Process to update when Receivelist is filled;
    ''' 24Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        Try
            ' Save data to database
            Dim Order As OrderDetails
            Dim ReceiveDetailsList As List(Of ReceiveItemDetails)

            ' Order Details
            Order = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.UnfulfilledOrder), List(Of OrderDetails)).Find(Function(i As OrderDetails) i.OrderID = ddlOrderID.SelectedValue)

            ' Receive item Details
            ReceiveDetailsList = RetrieveDCPInfo(EViewState.ReceiveItem.ToString)

            If ReceiveDetailsList.Count > 0 Then
                Using Client = New ServiceClient
                    Message = Client.UpdateReceiveItem(Session(ESession.StoreID.ToString) _
                                                       , Order.Type _
                                                       , ConvertToDate(txtReceiveDate.Text) _
                                                       , Session(ESession.UserID.ToString) _
                                                       , ReceiveDetailsList _
                                                       )
                End Using

                If Message = EMPTY Then
                    ' Reset Control n Clear screen
                    ddlOrderID.SelectedIndex = -1
                    txtReceiveDate.Text = Today.ToString("dd/MM/yyyy")
                    MainPanel(True)

                    Message = GetMessage(messageID.Success, "Saved", "Receive Order")

                    ' Add new received Order to list and rebind (if needed)
                    If Not (DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.ReceivedOrder), List(Of OrderDetails)).Exists(Function(i As OrderDetails) i.OrderID = Order.OrderID)) Then
                        EditCache(Cache(Session(ESession.StoreID.ToString) & ECache.ReceivedOrder), GetType(OrderDetails), Order)
                        BindDropDownList(ddlLocateOrderID, Cache(Session(ESession.StoreID.ToString) & ECache.ReceivedOrder), "OrderID", "OrderID")
                        uplLocateMain.Update()
                        BindDropDownList(ddlOrderReference, Cache(Session(ESession.StoreID.ToString) & ECache.ReceivedOrder), "OrderID", "OrderID")
                        uplPrintOrderID.Update()
                    End If
                End If

            Else
                If Message = EMPTY Then Message = GetMessage(messageID.NoChange)
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

    ''' <summary>
    ''' btnCancel_Click;
    ''' Reset Control;
    ''' 24Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    '''</remarks>
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        MainPanel(True)
    End Sub

    ''' <summary>
    ''' ShowModal_Click;
    ''' Show the Stock Information on Modal;
    ''' 24Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Sub ShowModal_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ShowModal.Click
        mpuStockAvailability.Show()
    End Sub

    ''' <summary>
    ''' btnExit_Click;
    ''' Hide the Modal button;
    ''' 24Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExit.Click
        ShowModal.Visible = False
    End Sub

#End Region

#Region " Locate Tab "
    ''' <summary>
    ''' ddlLocateOrderID_SelectedIndexChanged;
    ''' Populate the previously receive date list;
    ''' 25Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;</remarks>
    Protected Sub ddlLocateOrderID_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlLocateOrderID.SelectedIndexChanged
        Try
            Dim DateList As New List(Of Date)

            ' clear receive date ddl
            BindDropDownListDte(ddlLocateReceiveDate, DateList)

            If ddlLocateOrderID.SelectedValue <> EMPTY Then
                ' get list of received date for this order
                Using Client = New ServiceClient
                    DateList = Client.GetReceiveDte(Session(ESession.StoreID.ToString) _
                                                    , ddlLocateOrderID.SelectedValue)
                End Using

                If DateList.Count > 0 Then
                    BindDropDownListDte(ddlLocateReceiveDate, DateList)

                Else
                    Message = String.Format("Order Reference: [{0}] has not been received yet.  Please select another Order Reference.", ddlLocateOrderID.SelectedValue)

                    ' remove from received Order list and rebind
                    DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.ReceivedOrder), List(Of OrderDetails)).RemoveAt(ddlLocateOrderID.SelectedIndex - 1)
                    BindDropDownList(ddlLocateOrderID, Cache(Session(ESession.StoreID.ToString) & ECache.ReceivedOrder), "OrderID", "OrderID")
                    BindDropDownList(ddlOrderReference, Cache(Session(ESession.StoreID.ToString) & ECache.ReceivedOrder), "OrderID", "OrderID")
                    uplPrintOrderID.Update()
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
    ''' btnLocateGo_Click;
    ''' 1)Get all Order items with or with Receive Item records;
    ''' 2)Add user control to placeholder;
    ''' 24Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Sub btnLocateGo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLocateGo.Click
        Try
            Dim ReceiveItemList As List(Of ReceiveItemDetails)

            ' Populate record
            DCPLocate.Controls.Clear()
            ViewState.Remove(EViewState.ReceiveItemLocate.ToString)

            ' Check both ddl is selected with value
            If ddlLocateOrderID.SelectedValue <> EMPTY And ddlLocateReceiveDate.SelectedValue <> EMPTY Then
                ' Get all Order items with or with Receive Item records
                Using Client = New ServiceClient
                    ReceiveItemList = Client.GetReceiveItem(Session(ESession.StoreID.ToString).ToString _
                                                            , ddlLocateOrderID.SelectedValue _
                                                            , ConvertToDate(ddlLocateReceiveDate.SelectedValue) _
                                                            )
                End Using

                lblLocateOrderID.Text = ddlLocateOrderID.SelectedValue
                txtLocateReceiveDate.Text = ddlLocateReceiveDate.SelectedValue

                For Each item In ReceiveItemList
                    ' add receive user control
                    AddReceiveItem(DCPLocate, item, EViewState.ReceiveItemLocate.ToString)
                Next

                MainPanel(False)

                ' UAT02 - Edit and Delete allow only when withinFinanceCutoffDate
                If IsWithinFinanceCutoffDate(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), Session(ESession.StoreID.ToString), ConvertToDate(ddlLocateReceiveDate.SelectedValue)) Then
                    pnlLocateAccess.Enabled = True
                End If
            Else
                MainPanel(True)

                Message = GetMessage(messageID.MandatoryField)
            End If

        Catch ex As FaultException
            Message = ex.Message

        Catch ex As NullReferenceException
            Message = GetMessage(messageID.InvalidValue, "Order Reference")
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    ''' <summary>
    ''' btnLocateEdit_Click;
    ''' Enable controls for editing;
    ''' 24Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Sub btnLocateEdit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLocateEdit.Click
        ' Enable/Disable control for editing
        pnlLocateAccess.Enabled = False
        pnlLocateSearch.Enabled = True
        pnlLocateSearchItem.Enabled = True
        btnLocateSave.Enabled = True
    End Sub

    ''' <summary>
    ''' btnLocateDeleteAll_Click;
    ''' delete all the receive item for the selected order ref and date;
    ''' 24Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Sub btnLocateDeleteAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLocateDeleteAll.Click
        Try
            ' '' Order Details
            ''Dim Order As OrderDetails
            ''If cbxLocateFullFilled.Checked Then
            ''    Order = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.OrderList), List(Of OrderDetails)).Find(Function(i As OrderDetails) i.OrderID = lblLocateOrderID.Text)
            ''Else
            ''    Order = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.UnfulfilledOrder), List(Of OrderDetails)).Find(Function(i As OrderDetails) i.OrderID = lblLocateOrderID.Text)
            ''End If

            Using Client = New ServiceClient
                Client.DeleteAllStockTransaction(Session(ESession.StoreID.ToString).ToString _
                                                 , lblLocateOrderID.Text _
                                                 , ServiceModuleName.Receive _
                                                 , ConvertToDate(txtLocateReceiveDate.Text) _
                                                 , Session(ESession.UserID.ToString).ToString _
                                                 )
            End Using

            ' Reset Control n Clear screen
            ddlLocateOrderID.SelectedIndex = -1
            BindDropDownListDte(ddlLocateReceiveDate, New List(Of Date))
            MainPanel(True)
            uplLocateMain.Update()

            Message = GetMessage(messageID.Success, "deleted", "Receive Order")

            ' Add to unfillfulled Order list and rebind (if needed)
            If Not (DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.UnfulfilledOrder), List(Of OrderDetails)).Exists(Function(i As OrderDetails) i.OrderID = lblLocateOrderID.Text)) Then
                ' Order Details
                Dim Order As OrderDetails
                Order = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.OrderList), List(Of OrderDetails)).Find(Function(i As OrderDetails) i.OrderID = lblLocateOrderID.Text)
                EditCache(Cache(Session(ESession.StoreID.ToString) & ECache.UnfulfilledOrder), GetType(OrderDetails), Order)
                BindDropDownList(ddlOrderID, Cache(Session(ESession.StoreID.ToString) & ECache.UnfulfilledOrder), "OrderID", "OrderID")
                uplMain.Update()
            End If


        Catch ex As FaultException
            'Dim fault As ServiceFault = ex.Data
            'lblErrLocate.Text = fault.MessageText
            Message = ex.Message

        Catch ex As Exception
            Message = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    Protected Sub txtLocateReceiveDate_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtLocateReceiveDate.TextChanged
        Try
            Dim ReceiveDate As Date = ConvertToDate(txtLocateReceiveDate.Text)

            If Trim(txtLocateReceiveDate.Text) <> EMPTY Then
                If Not IsWithinFinanceCutoffDate(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), Session(ESession.StoreID.ToString), ReceiveDate) Then
                    txtLocateReceiveDate.Text = EMPTY
                    Throw New ApplicationException
                Else
                End If
            Else
                Throw New ArgumentNullException
            End If

        Catch ex As ArgumentNullException
            Message = clsCommonFunction.GetMessage(clsCommonFunction.messageID.MandatoryField)

        Catch ex As ApplicationException
            Message = clsCommonFunction.GetMessage(clsCommonFunction.messageID.NotInFinancial, "Receive Date")

        Catch ex As Exception
            Message = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    ''' <summary>
    ''' btnLocateSave_Click;
    ''' Save data to database;
    ''' 1)call a function to Collect receive information;
    ''' 2)Process to update when Receivelist is filled;
    ''' 24Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Sub btnLocateSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLocateSave.Click
        Try
            'save data to database
            Dim Order As OrderDetails
            Dim ReceiveDetailsList As List(Of ReceiveItemDetails)

            ' validate receivedate before proceeding
            If MainIsValid(ddlLocateOrderID, Trim(txtLocateReceiveDate.Text)) Then
                ' Order Details
                Order = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.ReceivedOrder), List(Of OrderDetails)).Find(Function(i As OrderDetails) i.OrderID = lblLocateOrderID.Text)

                ' Receive item Details
                ReceiveDetailsList = RetrieveDCPInfo(EViewState.ReceiveItemLocate.ToString)

                If ReceiveDetailsList.Count > 0 Then
                    Using Client = New ServiceClient
                        Message = Client.UpdateReceiveItem(Session(ESession.StoreID.ToString) _
                                                           , Order.Type _
                                                           , ConvertToDate(txtLocateReceiveDate.Text) _
                                                           , Session(ESession.UserID.ToString) _
                                                           , ReceiveDetailsList _
                                                           )
                    End Using

                    If Message = EMPTY Then
                        ' Reset Control n Clear screen
                        ddlLocateOrderID.SelectedIndex = -1
                        ddlLocateReceiveDate.SelectedIndex = -1
                        MainPanel(True)

                        Message = GetMessage(messageID.Success, "Saved", "Receive Order")
                    End If

                Else
                    If Message = EMPTY Then Message = GetMessage(messageID.NoChange)
                End If
            End If

        Catch ex As FaultException
            Message = ex.Message

        Catch ex As ApplicationException
            Message = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            ExceptionPolicy.HandleException(ex, "UserInterface Policy")

        Catch ex As Exception
            Message = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    ''' <summary>
    ''' btnLocateCancel_Click;
    ''' Reset Control;
    ''' 24Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    '''</remarks>
    Protected Sub btnLocateCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLocateCancel.Click
        MainPanel(True)
    End Sub

    ''' <summary>
    ''' ShowLocateModal_Click;
    ''' Show the Stock Information on Modal;
    ''' 24Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Sub ShowLocateModal_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ShowLocateModal.Click
        mpuLocateStockAvailability.Show()
    End Sub

    ''' <summary>
    ''' btnLocateExit_Click;
    ''' Hide the Modal button;
    ''' 24Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Sub btnLocateExit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLocateExit.Click
        ShowLocateModal.Visible = False
    End Sub
#End Region

#Region " Print Tab "
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
            ItemDetails.ItemID = StockCodeTo

            If Not Client.IsValidStockCode(ItemDetails) And ddlOrderReference.SelectedIndex = 0 Then

                Message = GetMessage(messageID.InvalidStockCode, StockCodeTo.ToUpper)
                Client.Close()
                Exit Sub

            End If

            Client.Close()
        End Using

        rvr.LocalReport.DataSources.Clear()
        rvr.LocalReport.DataSources.Add(New ReportDataSource("ReceiveList", ObjectDataSource1))

        Dim parameterlist As New List(Of ReportParameter)
        Dim p1 As New ReportParameter("StoreName", Session(ESession.StoreName.ToString).ToString)
        Dim p2 As New ReportParameter("PODateFrom", txtReceiveDateFrom.Text)
        Dim p3 As New ReportParameter("PODateTo", txtReceiveDateTo.Text)
        Dim p4 As New ReportParameter("StockCodeFrom", StockCodeFrom)
        Dim p5 As New ReportParameter("StockCodeTo", StockCodeTo)
        Dim p6 As New ReportParameter("OrderReference", ddlOrderReference.SelectedValue)
        parameterlist.Add(p1)
        parameterlist.Add(p2)
        parameterlist.Add(p3)
        parameterlist.Add(p4)
        parameterlist.Add(p5)
        parameterlist.Add(p6)

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
        Response.AddHeader("content-disposition", "attachment;filename=ReceiveList.pdf")
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
            ItemDetails.ItemID = StockCodeTo

            If Not Client.IsValidStockCode(ItemDetails) And ddlOrderReference.SelectedIndex = 0 Then

                Message = GetMessage(messageID.InvalidStockCode, StockCodeTo.ToUpper)
                Client.Close()
                Exit Sub

            End If

            Client.Close()
        End Using

        rvr.LocalReport.DataSources.Clear()
        rvr.LocalReport.DataSources.Add(New ReportDataSource("ReceiveList", ObjectDataSource1))

        Dim parameterlist As New List(Of ReportParameter)
        Dim p1 As New ReportParameter("StoreName", Session(ESession.StoreName.ToString).ToString)
        Dim p2 As New ReportParameter("PODateFrom", txtReceiveDateFrom.Text)
        Dim p3 As New ReportParameter("PODateTo", txtReceiveDateTo.Text)
        Dim p4 As New ReportParameter("StockCodeFrom", StockCodeFrom)
        Dim p5 As New ReportParameter("StockCodeTo", StockCodeTo)
        Dim p6 As New ReportParameter("OrderReference", ddlOrderReference.SelectedValue)
        parameterlist.Add(p1)
        parameterlist.Add(p2)
        parameterlist.Add(p3)
        parameterlist.Add(p4)
        parameterlist.Add(p5)
        parameterlist.Add(p6)

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
        Response.AddHeader("content-disposition", "attachment;filename=ReceiveList.xls")
        Response.BinaryWrite(bytValue)
        Response.Flush()
        Response.End()
    End Sub

    Protected Sub ObjectDataSource1_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs) Handles ObjectDataSource1.Selecting

        Dim StockCodeFrom, StockCodeTo As String
        StockCodeFrom = Split(txtStockCodeFrom.Text, " | ")(0).Trim
        StockCodeTo = Split(txtStockCodeTo.Text, " | ")(0).Trim

        e.InputParameters("storeID") = Session(ESession.StoreID.ToString)
        e.InputParameters("fromDte") = DateTime.ParseExact(Me.txtReceiveDateFrom.Text, "dd/MM/yyyy", Nothing)
        e.InputParameters("toDte") = DateTime.ParseExact(Me.txtReceiveDateTo.Text, "dd/MM/yyyy", Nothing)
        e.InputParameters("fromStockItemID") = StockCodeFrom
        e.InputParameters("toStockItemID") = StockCodeTo
        e.InputParameters("orderId") = ddlOrderReference.SelectedValue
    End Sub

    Protected Sub ObjectDataSource1_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles ObjectDataSource1.Selected
        Dim returnList As List(Of ReceiveList) = e.ReturnValue
        If returnList.Count <= 0 Then
            NoRecordFond = "Y"
        Else
            NoRecordFond = "N"
        End If
    End Sub

    Private Function ValidPrint() As Boolean

        Dim StockCodeFrom, StockCodeTo As String
        StockCodeFrom = Split(txtStockCodeFrom.Text, " | ")(0).Trim
        StockCodeTo = Split(txtStockCodeTo.Text, " | ")(0).Trim

        ' validate filtering criteria
        If ddlOrderReference.SelectedValue = EMPTY Then
            If (Trim(txtReceiveDateFrom.Text) = EMPTY _
                Or Trim(txtReceiveDateTo.Text) = EMPTY _
                Or txtStockCodeFrom.Text = EMPTY _
                ) Then
                Message = GetMessage(messageID.MandatoryField)
                Exit Function
            End If

            If ConvertToDate(txtReceiveDateFrom.Text) > ConvertToDate(txtReceiveDateTo.Text) Then
                Message = GetMessage(messageID.DateToEarlierDateFrom)
                Exit Function
            End If

            If StockCodeFrom > StockCodeTo Then
                Message = GetMessage(messageID.StockCodeToEarlierStockCodeFrom)
                Exit Function
            End If
        End If

        Return True
    End Function

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClear.Click
        txtReceiveDateFrom.Text = Today.ToString("01/MM/yyyy")
        txtReceiveDateTo.Text = Today.ToString("dd/MM/yyyy")
        'ddlStockCodeFrom.SelectedIndex = -1
        'ddlStockCode2.SelectedIndex = -1
        txtStockCodeFrom.Text = String.Empty
        txtStockCodeTo.Text = String.Empty
        ddlOrderReference.SelectedIndex = -1
    End Sub

#End Region

#Region " Sub Procedures and Functions "
    ''' <summary>
    ''' MainIsValid;
    ''' 1)check mandatory fields;
    ''' 2)check receive date is allow;
    ''' 3)check receive date not used;
    ''' 24Feb09 - KG;
    ''' </summary>
    ''' <param name="ddl"></param>
    ''' <param name="receiveDateText"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    '''</remarks>
    Private Function MainIsValid(ByVal ddl As DropDownList, ByVal receiveDateText As String) As Boolean
        Try
            Dim ReceiveDate As Date = ConvertToDate(receiveDateText)
            Dim DateList As List(Of Date)

            If ddl.SelectedValue <> EMPTY And ReceiveDate > Date.MinValue Then
                If receiveDateText = EMPTY Then
                    Throw New ApplicationException(GetMessage(messageID.MandatoryField))

                Else
                    If ReceiveDate > Today Then
                        Throw New ApplicationException(GetMessage(messageID.MoreLessThan, , , "Receive Date", Today.ToString("dd/MM/yyyy"), "earlier or same as"))

                    Else
                        If IsWithinFinanceCutoffDate(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), Session(ESession.StoreID.ToString), ReceiveDate) Then
                            If tbcReceiveItem.ActiveTab.ID = NEWTAB Then
                                Using Client = New ServiceClient
                                    DateList = Client.GetReceiveDte(Session(ESession.StoreID.ToString) _
                                                                    , ddlOrderID.SelectedValue _
                                                                    )
                                End Using

                                If DateList.Exists(Function(i As Date) i = ReceiveDate) Then
                                    Throw New ApplicationException(GetMessage(messageID.DateHasReceived, receiveDateText))
                                End If
                            End If

                        Else
                            Throw New ApplicationException(GetMessage(messageID.NotInFinancial, "Receive Date"))
                        End If
                    End If
                End If

            Else
                Throw New ApplicationException(GetMessage(messageID.MandatoryField))
            End If

            Return True
        Catch ex As ApplicationException
            Message = ex.Message

            Return False
        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw

            Return False
        End Try

    End Function

    ''' <summary>
    ''' Add the ReceiveItem User Control in the PlaceHolder;
    ''' 12Feb09 - KG;
    ''' </summary>
    ''' <param name="PlaceHolder">position</param>
    ''' <param name="receiveItemDetails">receive info</param>
    ''' <param name="UserControlName"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Private Sub AddReceiveItem(ByRef PlaceHolder As DynamicControlsPlaceholder, ByVal receiveItemDetails As ReceiveItemDetails, ByVal UserControlName As String)
        Try
            Dim ReceiveItem = New ReceiveItem
            Dim ItemDetails As ItemDetails = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), List(Of ItemDetails)).Find(Function(i As ItemDetails) i.ItemID = receiveItemDetails.StockItemID)

            If IsNothing(ViewState(UserControlName)) Then
                ViewState(UserControlName) = 1
            Else
                ViewState(UserControlName) += 1
            End If

            ReceiveItem = LoadControl("ReceiveItem.ascx")
            ReceiveItem.ID = UserControlName + Convert.ToString(ViewState(UserControlName))

            ' Need to add control to page before assigning value to the control
            PlaceHolder.Controls.Add(ReceiveItem)

            ' clear MoreItemInfo cache
            Cache.Remove(ReceiveItem.UniqueID)

            ' Assign value to controls
            DirectCast(ReceiveItem.FindControl("hfMode"), HiddenField).Value = EMPTY
            DirectCast(ReceiveItem.FindControl("hfTranID"), HiddenField).Value = receiveItemDetails.TranID
            DirectCast(ReceiveItem.FindControl("hfItemRef"), HiddenField).Value = receiveItemDetails.OrderItemID
            DirectCast(ReceiveItem.FindControl("hfUnitCost"), HiddenField).Value = receiveItemDetails.OrderItemUnitCost
            DirectCast(ReceiveItem.FindControl("hfQtyOutstanding"), HiddenField).Value = receiveItemDetails.OrderItemQtyOutstanding

            DirectCast(ReceiveItem.FindControl("hfOrgReceiveQty"), HiddenField).Value = receiveItemDetails.Qty
            DirectCast(ReceiveItem.FindControl("hfOrgWarrantyDte"), HiddenField).Value = receiveItemDetails.OrderItemWarrantyDte
            DirectCast(ReceiveItem.FindControl("hfOrgRemarks"), HiddenField).Value = receiveItemDetails.Remarks

            DirectCast(ReceiveItem.FindControl("lblStockCode"), Label).Text = receiveItemDetails.StockItemID
            DirectCast(ReceiveItem.FindControl("lblDescription"), Label).Text = ItemDetails.ItemDescription
            DirectCast(ReceiveItem.FindControl("lblUOM"), Label).Text = ItemDetails.UOM

            DirectCast(ReceiveItem.FindControl("txtReceiveQty"), TextBox).Text = receiveItemDetails.Qty.ToString("0.00")
            '  Client Side Java Scripting
            DirectCast(ReceiveItem.FindControl("txtReceiveQty"), TextBox).Attributes.Add("onkeyup", "computeItemTotal('" & DirectCast(ReceiveItem.FindControl("txtReceiveQty"), TextBox).ClientID & "','" & DirectCast(ReceiveItem.FindControl("hfUnitCost"), HiddenField).ClientID & "','" & DirectCast(ReceiveItem.FindControl("lblTotalCost"), Label).ClientID & "');")
            DirectCast(ReceiveItem.FindControl("txtWarrantyDte"), TextBox).Text = IIf(receiveItemDetails.OrderItemWarrantyDte > Date.MinValue, receiveItemDetails.OrderItemWarrantyDte.ToString("dd/MM/yyyy"), EMPTY)
            '  truncate value to 4 decimal place for display
            DirectCast(ReceiveItem.FindControl("lblUnitCost"), Label).Text = DisplayValue(receiveItemDetails.OrderItemUnitCost)
            DirectCast(ReceiveItem.FindControl("txtRemarks"), TextBox).Text = receiveItemDetails.Remarks

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Used by the User Control OrderItem to display its Stock Item info;
    ''' 5 Feb 2009 - KG;
    ''' </summary>
    ''' <param name="uniqueID"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Friend Sub ViewStock(ByVal uniqueID As String, ByVal stockItemID As String, ByVal stockItemDesc As String, ByVal stockItemUOM As String, ByVal ReceiveQtyDiff As Decimal, ByVal TotalDiff As Double, ByVal newOutstanding As Decimal)
        Try
            ' Retrieve and Cache more Info for the Stock Item. AUC, Balance, etc
            If Cache(Session(ESession.StoreID.ToString) & uniqueID) Is Nothing Then
                Using Client = New ServiceClient
                    Cache(Session(ESession.StoreID.ToString) & uniqueID) = Client.GetMoreItemInfo(Session(ESession.StoreID.ToString).ToString _
                                                             , stockItemID _
                                                             , Today _
                                                             )
                End Using
            End If

            'get info from Cache
            Dim MoreItem As MoreItemInfoDetails = DirectCast(Cache(Session(ESession.StoreID.ToString) & uniqueID), MoreItemInfoDetails)
            Dim UOM As String = " " + stockItemUOM
            Dim BalanceNew As Decimal = MoreItem.Balance + ReceiveQtyDiff
            Dim TotalValueNew As Double = MoreItem.TotalValue + TotalDiff
            Dim AUCNew As Double = IIf(BalanceNew > 0, TotalValueNew / BalanceNew, 0.0)

            If tbcReceiveItem.ActiveTab.ID = NEWTAB Then
                ViewStockCode.Text = MoreItem.ItemID
                ViewDesc.Text = stockItemDesc
                ViewBalance.Text = MoreItem.Balance.ToString("0.00") + UOM
                ViewAUC.Text = "$ " + DisplayValue(MoreItem.AvgUnitCost)
                ViewTotalValue.Text = "$ " + DisplayValue(MoreItem.TotalValue)

                ViewBalanceNew.Text = BalanceNew.ToString("0.00") + UOM
                ViewAUCNew.Text = "$ " + DisplayValue(AUCNew)
                ViewTotalValueNew.Text = "$ " + DisplayValue(TotalValueNew)
                ViewOutstanding.Text = newOutstanding.ToString("0.00") + UOM

                ShowModal.Visible = True
                ShowModal_Click(ShowModal, New EventArgs)

            ElseIf tbcReceiveItem.ActiveTab.ID = LOCATETAB Then
                ViewLocateStockCode.Text = MoreItem.ItemID
                ViewLocateDesc.Text = stockItemDesc
                ViewLocateBalance.Text = MoreItem.Balance.ToString("0.00") + UOM
                ViewLocateAUC.Text = "$ " + DisplayValue(MoreItem.AvgUnitCost)
                ViewLocateTotalValue.Text = "$ " + DisplayValue(MoreItem.TotalValue)

                ViewLocateBalanceNew.Text = BalanceNew.ToString("0.00") + UOM
                ViewLocateAUCNew.Text = "$ " + DisplayValue(AUCNew)
                ViewLocateTotalValueNew.Text = "$ " + DisplayValue(TotalValueNew)
                ViewLocateOutstanding.Text = newOutstanding.ToString("0.00") + UOM

                ShowLocateModal.Visible = True
                ShowLocateModal_Click(ShowModal, New EventArgs)
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Manage the Main and details screen display;
    ''' To enable/disable UI controls for Edit
    ''' 25Feb09 - KG;
    ''' </summary>
    ''' <param name="enabled"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Private Sub MainPanel(ByVal enabled As Boolean)
        Select Case tbcReceiveItem.ActiveTab.ID
            Case NEWTAB
                ddlOrderID.Enabled = enabled
                txtReceiveDate.Enabled = enabled

                btnGo.Visible = enabled
                pnlReceive.Visible = (Not enabled)

                ' reset control
                If enabled Then
                    DCP.Controls.Clear()
                    ViewState.Remove(EViewState.ReceiveItem.ToString)
                End If

            Case LOCATETAB
                ddlLocateOrderID.Enabled = enabled
                ''cbxLocateFullFilled.Enabled = enabled
                ddlLocateReceiveDate.Enabled = enabled

                divLocateSearch.Visible = enabled
                pnlLocate.Visible = (Not enabled)

                ' disable all editting action
                pnlLocateAccess.Enabled = False
                pnlLocateSearch.Enabled = False
                pnlLocateSearchItem.Enabled = False
                btnLocateSave.Enabled = False

                ' reset control
                If enabled Then
                    DCPLocate.Controls.Clear()
                    ViewState.Remove(EViewState.ReceiveItemLocate.ToString)
                End If
        End Select
    End Sub

    ''' <summary>
    ''' RetrieveDCPInfo;
    ''' Retrieve the receive information from DCP and return as a list of receive item details;
    ''' 1)Collect receive information when Mode is NOT empty, also check qty is a valid number;
    ''' 2)check if Warranty date is changed, also check for valid date formatted;
    ''' 3)Add to ReceiveList when either (1) or (2);
    ''' 25Feb09 - KG;
    ''' </summary>
    ''' <param name="UserControlName"></param>
    ''' <returns>list of receiveitemdetails</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Function RetrieveDCPInfo(ByVal UserControlName As String) As List(Of ReceiveItemDetails)
        Dim ReceiveItem As ReceiveItem
        Dim ReceiveDetails As ReceiveItemDetails
        Dim ReceiveDetailsList As List(Of ReceiveItemDetails)
        Dim WarrantyChanged As Boolean = False

        ' Receive item Details
        ReceiveDetailsList = New List(Of ReceiveItemDetails)
        For i As Integer = 1 To ViewState(UserControlName)
            If tbcReceiveItem.ActiveTab.ID = NEWTAB Then
                ReceiveItem = TryCast(DCP.FindControl(UserControlName + i.ToString), ReceiveItem)
            ElseIf tbcReceiveItem.ActiveTab.ID = LOCATETAB Then
                ReceiveItem = TryCast(DCPLocate.FindControl(UserControlName + i.ToString), ReceiveItem)
            Else
                Throw New Exception("something wrong?")
            End If

            If ReceiveItem.Message <> EMPTY Then
                Message = ReceiveItem.Message
                Return New List(Of ReceiveItemDetails)
            End If

            ReceiveDetails = New ReceiveItemDetails
            If ReceiveItem IsNot Nothing Then
                Dim Value As String

                ReceiveDetails.Mode = Trim(DirectCast(ReceiveItem.FindControl("hfMode"), HiddenField).Value)
                If ReceiveDetails.Mode <> EMPTY Then
                    ReceiveDetails.TranID = CInt(DirectCast(ReceiveItem.FindControl("hfTranID"), HiddenField).Value)
                    ReceiveDetails.StockItemID = Trim(DirectCast(ReceiveItem.FindControl("lblStockCode"), Label).Text)

                    Value = Trim(DirectCast(ReceiveItem.FindControl("txtReceiveQty"), TextBox).Text)
                    If Not IsNumeric(Value) Then Throw New ApplicationException(String.Format("[{0}] {1}", ReceiveDetails.StockItemID, GetMessage(messageID.MandatoryField)))
                    ReceiveDetails.Qty = CDec(Value)
                    ReceiveDetails.TotalCost = CDec(DirectCast(ReceiveItem.FindControl("hfUnitCost"), HiddenField).Value) * ReceiveDetails.Qty
                    ReceiveDetails.Remarks = Trim(DirectCast(ReceiveItem.FindControl("txtRemarks"), TextBox).Text)
                    ReceiveDetails.Status = OPEN    ' will be closed after financial cut off date
                End If

                ' OrderItemID is required to update the Warranty date (if needed)
                ReceiveDetails.OrderItemID = CInt(DirectCast(ReceiveItem.FindControl("hfItemRef"), HiddenField).Value)

                ' Optional field, if blank let it be null value (Date.MinValue)
                Value = Trim(DirectCast(ReceiveItem.FindControl("txtWarrantyDte"), TextBox).Text)
                If Value.Length > 0 Then
                    If ConvertToDate(Value) = DateTime.MinValue Then : Throw New ApplicationException(GetMessage(messageID.NotIsDate, String.Format("{0}'s Warranty Date", ReceiveDetails.StockItemID)))
                    Else : ReceiveDetails.OrderItemWarrantyDte = ConvertToDate(Value)
                    End If
                End If

                ' Check to trace Warranty date changes
                WarrantyChanged = IIf(ConvertToDate(DirectCast(ReceiveItem.FindControl("hfOrgWarrantyDte"), HiddenField).Value) <> ReceiveDetails.OrderItemWarrantyDte, True, False)


                ' Update either when receive mode is not empty OR warranty date is changed
                If ReceiveDetails.Mode <> EMPTY Or WarrantyChanged Then
                    ReceiveDetailsList.Add(ReceiveDetails)
                End If
            End If
        Next

        Return ReceiveDetailsList
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

#Region " Not in Used "
    ''' <summary>
    ''' OBSOLETE!
    ''' ddlOrderID - SelectedIndexChanged;
    ''' preload the Receive Date as accordingly
    ''' 11Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Sub ddlOrderID_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrderID.SelectedIndexChanged
        'If ddlOrderID.SelectedIndex > 0 Then
        '    If ConvertToDate(txtReceiveDate.Text) = DateTime.MinValue Then txtReceiveDate.Text = Today.ToString("dd/MM/yyyy")
        '    txtReceiveDate.Enabled = True

        'Else
        '    txtReceiveDate.Text = empty
        '    txtReceiveDate.Enabled = False
        'End If
    End Sub

    ''' <summary>
    ''' OBSOLETE!
    ''' txtReceiveDate - SelectedIndexChanged;
    ''' check the receive date is not currently used
    ''' 11Feb09 - KG;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Sub txtReceiveDate_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtReceiveDate.TextChanged
        'Try
        '    Dim ReceiveDate As Date = ConvertToDate(txtReceiveDate.Text)

        '    If Trim(txtReceiveDate.Text) <> empty Then
        '        If Not IsWithinFinanceCutoffDate(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), Session(ESession.StoreID.ToString), ReceiveDate) Then
        '            txtReceiveDate.Text = empty
        '            btnGo.Enabled = False
        '            Throw New ApplicationException
        '        Else
        '            btnGo.Enabled = True
        '        End If
        '    End If

        'Catch ex As ApplicationException
        '    Message = GetMessage(messageID.NotInFinancial, "Receive Date")

        'Catch ex As Exception
        '    Message = GetMessage(messageID.TryLastOperation)
        '    Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
        '    If (rethrow) Then Throw
        'End Try
    End Sub

    ' ''Protected Sub btnLocateClear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLocateClear.Click
    ' ''    ' Reset control
    ' ''    ddlLocateOrderID.SelectedIndex = -1
    ' ''    ddlLocateReceiveDate.Items.Clear()
    ' ''    MainPanel(True)
    ' ''End Sub

    '''''' <summary>
    '''''' Bind with Received Order list instead
    '''''' cbxLocateFullFilled - SelectedIndexChanged;
    '''''' Populate the Locate Order ID ddl with or without unfulfilled orders
    '''''' 11Feb09 - KG;
    '''''' </summary>
    '''''' <param name="sender"></param>
    '''''' <param name="e"></param>
    '''''' <remarks>
    '''''' CHANGE LOG:
    '''''' ddMMMyy  AuthorName  RefID  Description;
    '''''' </remarks>
    ' ''Protected Sub cbxLocateFullFilled_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbxLocateFullFilled.CheckedChanged
    ' ''    Try
    ' ''        If Cache(Session(ESession.StoreID.ToString) & ECache.OrderList) Is Nothing Or DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.OrderList), List(Of OrderDetails)).Count = 0 Then GetOrderList(Cache(Session(ESession.StoreID.ToString) & ECache.OrderList), Session(ESession.StoreID.ToString), ALL)
    ' ''        If cbxLocateFullFilled.Checked = True Then
    ' ''            ' All Order, Status = All
    ' ''            BindDropDownList(ddlLocateOrderID, Cache(Session(ESession.StoreID.ToString) & ECache.OrderList), "OrderID", "OrderID")
    ' ''        Else
    ' ''            ' only unfulfilled order, status <> Closed
    ' ''            If Cache(Session(ESession.StoreID.ToString) & ECache.UnfulfilledOrder) Is Nothing Or DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.UnfulfilledOrder), List(Of OrderDetails)).Count = 0 Then Cache(Session(ESession.StoreID.ToString) & ECache.UnfulfilledOrder) = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.OrderList), List(Of OrderDetails)).FindAll(Function(item As OrderDetails) item.Status <> CLOSED)
    ' ''            BindDropDownList(ddlLocateOrderID, Cache(Session(ESession.StoreID.ToString) & ECache.UnfulfilledOrder), "OrderID", "OrderID")
    ' ''        End If
    ' ''    Catch ex As Exception
    ' ''        Message = GetMessage(messageID.TryLastOperation)
    ' ''        Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
    ' ''        If (rethrow) Then Throw

    ' ''    End Try
    ' ''End Sub


#End Region

End Class