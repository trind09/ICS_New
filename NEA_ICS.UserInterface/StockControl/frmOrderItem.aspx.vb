Imports Microsoft.Reporting.WebForms
Imports System.Web.Services.Description
Imports NEA_ICS.UserInterface.NEA_ICS.WcfService
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ServiceModel
Imports DBauer.Web.UI.WebControls
Imports System.Web.Services
Imports System.Reflection

''' <summary>
''' Code behind for frmOrderItem Page;
''' 23Jan09 - KG;
''' </summary>
''' <remarks>
''' CHANGE LOG:
''' ddMMMyy  AuthorName  RefID  Description;
''' 20Mar09  KG          UAT02  fix UAT bugs;
''' </remarks>
Partial Public Class frmOrderItem
    Inherits clsCommonFunction

#Region " Page Control "
    Private Message As String = EMPTY
    Private OrderItem As OrderItem
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

    'REMOVED: Private Shared _orderList As List(Of OrderDetails) 'AjaxToolKit autocomplete requires a Shared variable

    ''' <summary>
    ''' Page_Load;
    ''' 1)Assign controls to user base on Access Rights;
    ''' 2)Retrieve all lists;
    ''' 3)Bind dropdownlists with Lists;
    ''' 08Feb09 - KG;
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
                aceStockCodeFrom.ContextKey = Session("StoreID").ToString
                aceStockCodeTo.ContextKey = Session("StoreID").ToString

                ' @@@ START OF ACCESS RIGHTS @@@
                Dim AccessRights As New List(Of RoleDetails)

                tbcOrderItem.Visible = False
                AccessRights = clsCommonFunction.AssignAccessRights(Cache(Session(ESession.UserID.ToString) & Session(ESession.StoreID.ToString) & "AccessRights") _
                                                                    , moduleID.OrderItem _
                                                                    )
                ' Select Rights
                tbpLocate.Visible = False
                tbpPrint.Visible = False
                If AccessRights(0).SelectRight Then
                    tbpLocate.Visible = True
                    tbpPrint.Visible = True
                    tbcOrderItem.Visible = True
                    tbpLocate.Focus()

                Else
                    Server.Transfer("..\frmUnauthorisedPage.aspx")
                    Exit Sub
                End If


                ' Insert Rights
                tbpNew.Visible = False
                If AccessRights(0).InsertRight Then
                    tbpNew.Visible = True
                    tbcOrderItem.ActiveTabIndex = 0
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


                ViewState(EViewState.OrderIdIsValid) = False  'Mandatory hence Blank is not valid
                ViewState(EViewState.GebizIsValid) = True     'Optional hence Blank is valid

                ' retrieve listing to be bind to ddl ' UAT02 refresh the Orderlist cache
                GetOrderList(Cache(Session(ESession.StoreID.ToString) & ECache.OrderList), Session(ESession.StoreID.ToString), ALL)
                Cache(Session(ESession.StoreID.ToString) & ECache.UnfulfilledOrder) = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.OrderList), List(Of OrderDetails)).FindAll(Function(i As OrderDetails) i.Status <> CLOSED)
                'If CacheListIsEmpty(Cache(Session(ESession.StoreID.ToString) & ECache.SupplierList), GetType(SupplierDetails)) Then GetSupplierList(Cache(Session(ESession.StoreID.ToString) & ECache.SupplierList), Session(ESession.StoreID.ToString))
                If CacheListIsEmpty(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), GetType(ItemDetails)) Then GetItemList(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), Session(ESession.StoreID.ToString))
                GetSupplierList(Cache(Session(ESession.StoreID.ToString) & ECache.SupplierList), Session(ESession.StoreID.ToString))

                ' New Tab
                BindDropDownList(ddlSupplierID, Cache(Session(ESession.StoreID.ToString) & ECache.SupplierList), "SupplierId", "SupplierId")
                BindDropDownList(ddlDocType, GetCommonDataByCodeGroup(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), codeGroup.OrderDocType, OPEN), "CommonCodeID", "CommonCodeDescription")

                '  to be access by its User Control
                Session(ESession.OrderDte.ToString) = Today
                txtOrderDte.Text = Today.ToString("dd/MM/yyyy")
                ViewState(EViewState.DeliveryLapseDay) = Convert.ToDouble(GetCodeID(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), Session(ESession.StoreID.ToString).ToString, codeGroup.DeliveryLapseDay))

                '   to allow Exit button click on Modal to invoke page postback
                btnExit.OnClientClick = String.Format("postbackFromJS('{0}', '{1}')", btnExit.UniqueID, String.Empty)


                ' Locate Tab

                '  bind order ddl default with unfulfilled orders
                BindDropDownList(ddlLocateOrderID, Cache(Session(ESession.StoreID.ToString) & ECache.UnfulfilledOrder), "OrderId", "OrderId")
                BindDropDownList(ddlLocateSupplierID, Cache(Session(ESession.StoreID.ToString) & ECache.SupplierList), "SupplierId", "SupplierId")
                BindDropDownList(ddlLocateDocType, GetCommonDataByCodeGroup(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), codeGroup.OrderDocType, OPEN), "CommonCodeID", "CommonCodeDescription")

                '   to allow Exit button click on Modal to invoke page postback
                btnLocateExit.OnClientClick = String.Format("postbackFromJS('{0}', '{1}')", btnLocateExit.UniqueID, String.Empty)

                pnlLocateOrder.Enabled = False
                pnlLocateOrderItem.Enabled = False

                ' Report Tab 
                btnClear_Click(Nothing, Nothing)
                'BindDropDownList(ddlStockCodeFrom, Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), "ItemID", "ItemID_Description")
                'BindDropDownList(ddlStockCode2, Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), "ItemID", "ItemID_Description")
                BindDropDownList(ddlOrderReference, Cache(Session(ESession.StoreID.ToString) & ECache.OrderList), "OrderID", "OrderID")

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
    ''' ddMMMyy   AuthorName  RefID            Description;
    ''' 26 Oct 10 Jianfa      ERSS 1013133256  To use Blank Report instead
    ''' </remarks>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        If Message <> EMPTY Then
            ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", "ShowAlertMessage('" & Message & "');", True)
        End If

    End Sub
#End Region

#Region " New Tab "
    Protected Sub ddlSupplierID_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlSupplierID.SelectedIndexChanged
        Try
            If ddlSupplierID.SelectedValue <> EMPTY Then
                lblCompanyName.Text = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.SupplierList), List(Of SupplierDetails)).Item(ddlSupplierID.SelectedIndex - 1).CompanyName
            Else
                lblCompanyName.Text = EMPTY
            End If

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    Protected Sub txtOrderID_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtOrderID.TextChanged
        Try
            If Trim(txtOrderID.Text).Length > 0 Then
                'TODO: check unique order id
                Dim Client As New ServiceClient

                If (Client.FieldIsUnique(Session(ESession.StoreID.ToString) _
                                         , ServiceColumnName.OrderId _
                                         , txtOrderID.Text _
                                         , EMPTY _
                                         ) _
                    ) Then
                    ViewState(EViewState.OrderIdIsValid) = True

                Else
                    ViewState(EViewState.OrderIdIsValid) = False
                    txtOrderID.Focus()
                    Message = String.Format("Order reference:({0}) exists in the system", Trim(txtOrderID.Text))
                End If
                Client.Close()
            End If

        Catch ex As FaultException
            Message = ex.Message

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    Protected Sub txtGebizPONo_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtGebizPONo.TextChanged
        Try
            If Trim(txtGebizPONo.Text).Length > 0 Then
                'TODO: check unique order id
                Dim Client As New ServiceClient

                If (Client.FieldIsUnique(Session(ESession.StoreID.ToString) _
                                         , ServiceColumnName.OrderGebizPONo _
                                         , txtGebizPONo.Text _
                                         , txtOrderID.Text _
                                         ) _
                    ) Then
                    ViewState(EViewState.GebizIsValid) = True

                Else
                    ViewState(EViewState.GebizIsValid) = False
                    txtGebizPONo.Focus()
                    Message = "ShowAlertMessage('" & String.Format("Gebiz PO No:({0}) exists in the system", Trim(txtGebizPONo.Text)) & "');"
                End If
                Client.Close()
            Else
                ViewState(EViewState.GebizIsValid) = True
            End If

        Catch ex As FaultException
            Message = ex.Message

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    Private Function ValidateOrder() As Boolean
        Try
            Dim OrderDte As Date
            Dim Valid As Boolean = False

            OrderDte = ConvertToDate(txtOrderDte.Text)

            If OrderDte = DateTime.MinValue Then
                Throw New ApplicationException(GetMessage(messageID.NotIsDate, "Order Date"))

            ElseIf Not ViewState(EViewState.OrderIdIsValid) And Trim(txtOrderID.Text).Length > 0 Then
                Throw New ApplicationException(String.Format("Order reference:({0}) exists in the system", Trim(txtOrderID.Text)))

            ElseIf Not ViewState(EViewState.GebizIsValid) Then
                Throw New ApplicationException(String.Format("Gebiz PO No:({0}) exists in the system", Trim(txtGebizPONo.Text)))

            ElseIf (ddlSupplierID.SelectedValue <> EMPTY _
                    And Trim(txtOrderID.Text).Length > 0 _
                    And ddlDocType.SelectedValue <> EMPTY _
                    And OrderDte > DateTime.MinValue _
                    ) Then
                Valid = True

            Else
                Throw New ApplicationException(GetMessage(messageID.MandatoryField))
            End If

            Return Valid
        Catch ex As ApplicationException
            Message = ex.Message

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub btnAddOrderItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddOrderItem.Click
        Try
            If ValidateOrder() Then
                ViewState(EViewState.OrderedStockItemID) = New List(Of String)

                '-- [NOT NEEDED] - Jianfa [02/05/2009]
                ''btnAddItem_Click(sender, e)
                'If CacheListIsEmpty(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), GetType(ItemDetails)) Then GetItemList(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), Session(ESession.StoreID.ToString))
                'BindDropDownList(ddlStockCode, Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), "ItemID", "ItemID_Description")

                MainPanel(False)
            End If

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
            If txtStockCode.Text.Trim <> EMPTY Then

                '-- VALIDATE STOCK CODE
                Dim StockCode As String
                StockCode = Split(txtStockCode.Text, " | ")(0).Trim.ToUpper

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

                Dim OrderItemDetailsItem As New OrderItemDetails

                Dim Ordered As Boolean = DirectCast(ViewState(EViewState.OrderedStockItemID), List(Of String)).Exists(Function(i As String) (i = StockCode))

                If Ordered Then
                    Throw New ApplicationException(GetMessage(messageID.StockCodeHasOrdered, StockCode))
                End If

                OrderItemDetailsItem.ExpectedDeliveryDte = ConvertToDate(txtOrderDte.Text).AddDays(ViewState(EViewState.DeliveryLapseDay))
                OrderItemDetailsItem.StockItemID = StockCode

                AddOrderItem(DCP, OrderItemDetailsItem, EViewState.OrderItem.ToString, True)

                ' keep a list of ordered stock item, to check against to restrict duplicate
                DirectCast(ViewState(EViewState.OrderedStockItemID), List(Of String)).Add(OrderItemDetailsItem.StockItemID)

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
            Dim Client As ServiceClient
            Dim Order As OrderDetails
            Dim OrderItem As OrderItem
            Dim OrderItemDetails As OrderItemDetails
            Dim OrderItemDetailsList As List(Of OrderItemDetails)

            ' Order Details
            Order = New OrderDetails
            Order.StoreID = Session(ESession.StoreID.ToString).ToString
            Order.OrderID = txtOrderID.Text
            Order.GebizPONo = txtGebizPONo.Text
            Order.Type = ddlDocType.SelectedValue
            Order.Dte = ConvertToDate(txtOrderDte.Text)
            Order.SupplierID = ddlSupplierID.SelectedValue
            Order.Status = OPEN  'default to 'Open'
            Order.LoginUser = Session(ESession.UserID.ToString).ToString

            ' Order item Details
            OrderItemDetailsList = New List(Of OrderItemDetails)
            ' loop the orderitem user control

            If ViewState(EViewState.OrderItem.ToString) IsNot Nothing Then
                For i As Integer = 1 To ViewState(EViewState.OrderItem.ToString)
                    OrderItem = TryCast(DCP.FindControl(EViewState.OrderItem.ToString + i.ToString), OrderItem)

                    If OrderItem IsNot Nothing Then
                        If OrderItem.Message <> EMPTY Then
                            Message = OrderItem.Message
                            Exit Sub
                        End If
                    End If

                    OrderItemDetails = New OrderItemDetails
                    If OrderItem IsNot Nothing Then
                        Dim Value As String
                        Dim Qty As Decimal
                        Dim TotalCost As Double

                        Value = Trim(DirectCast(OrderItem.FindControl("lblStockCode"), Label).Text)
                        If Value.Length <= 0 Then Throw New ArgumentNullException
                        OrderItemDetails.StockItemID = Value

                        Value = Trim(DirectCast(OrderItem.FindControl("txtOrderQty"), TextBox).Text)
                        If Not IsNumeric(Value) Then Throw New ArgumentNullException
                        Qty = Convert.ToDecimal(Value)
                        OrderItemDetails.Qty = Qty

                        Value = Trim(DirectCast(OrderItem.FindControl("txtTotalCost"), TextBox).Text)
                        If Not IsNumeric(Value) Then Throw New ArgumentNullException
                        TotalCost = Convert.ToDouble(Value)
                        OrderItemDetails.TotalCost = TotalCost

                        Value = Trim(DirectCast(OrderItem.FindControl("txtExpectedDeliveryDte"), TextBox).Text)
                        If ConvertToDate(Value) = DateTime.MinValue Then Throw New ArgumentNullException
                        OrderItemDetails.ExpectedDeliveryDte = ConvertToDate(Value)

                        ' Optional field, if blank let it be null value
                        Value = Trim(DirectCast(OrderItem.FindControl("txtWarrantyDte"), TextBox).Text)
                        If Value.Length > 0 Then
                            If ConvertToDate(Value) = DateTime.MinValue Then : Throw New ArgumentNullException
                            Else : OrderItemDetails.WarrantyDte = ConvertToDate(Value)
                            End If
                        End If

                        Value = Trim(DirectCast(OrderItem.FindControl("txtRemarks"), TextBox).Text)
                        If Value.Length > 0 Then OrderItemDetails.Remarks = Value Else Throw New ArgumentNullException

                        OrderItemDetails.Status = OPEN
                        OrderItemDetails.Mode = INSERT

                        OrderItemDetailsList.Add(OrderItemDetails)
                    End If
                Next
            Else
                Throw New FaultException(GetMessage(messageID.StockCodeNotAdded))
            End If


            Client = New ServiceClient
            Message = Client.AddOrder(Order, OrderItemDetailsList)
            Client.Close()

            If Message = String.Empty Then
                Message = GetMessage(messageID.Success, "saved", "Order")

                ' add new order to cache and sort it
                If Not CacheListIsEmpty(Cache(Session(ESession.StoreID.ToString) & ECache.OrderList), GetType(OrderDetails)) Then
                    EditCache(Cache(Session(ESession.StoreID.ToString) & ECache.OrderList) _
                              , GetType(OrderDetails) _
                              , Order _
                              )
                End If
                If Not CacheListIsEmpty(Cache(Session(ESession.StoreID.ToString) & ECache.UnfulfilledOrder), GetType(OrderDetails)) Then
                    EditCache(Cache(Session(ESession.StoreID.ToString) & ECache.UnfulfilledOrder) _
                              , GetType(OrderDetails) _
                              , Order _
                              )
                End If

                ' bind the new order for selection
                cbxLocateUnfulfillOnly_CheckedChanged(Nothing, Nothing)
                uplLocateMain.Update()
                BindDropDownList(ddlOrderReference, Cache(Session(ESession.StoreID.ToString) & ECache.OrderList), "OrderID", "OrderID")
                uplPrintOrderID.Update()

                'clear screen
                ddlSupplierID.SelectedIndex = -1
                lblCompanyName.Text = String.Empty
                txtOrderID.Text = String.Empty
                ViewState(EViewState.OrderIdIsValid) = False
                txtGebizPONo.Text = String.Empty
                ViewState(EViewState.GebizIsValid) = True
                ddlDocType.SelectedIndex = -1
                txtOrderDte.Text = Today.ToString("dd/MM/yyyy")

                btnCancelAll_Click(sender, e)
            End If

        Catch ex As FaultException
            Message = ex.Message

        Catch ex As ApplicationException
            Message = GetMessage(messageID.StockCodeNotAdded)

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
            ViewState(EViewState.OrderedStockItemID) = New List(Of String)
            ViewState.Remove(EViewState.OrderItem)
            DCP.Controls.Clear()
            UpdateGTotal(0.0, True)

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
    ''' 16Mar09  KG          UAT01  1)always get all orderitems; 2)disable editing when Order is Closed/Fulfilled;
    ''' </remarks>
    Private Sub btnLocateGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLocateGo.Click
        Try
            ' Enable/Disable control for editing
            ddlLocateOrderID.Enabled = False
            cbxLocateUnfulfillOnly.Enabled = False
            pnlLocateAccess.Enabled = True
            pnlLocateOrder.Enabled = False
            pnlLocateOrderItem.Enabled = False
            pnlLocateAction.Enabled = False

            ' Populate record
            DCPLocate.Controls.Clear()
            ViewState.Remove(EViewState.OrderItemLocate.ToString)
            ViewState(EViewState.OrderedStockItemIDLocate) = New List(Of String)
            If ddlLocateOrderID.SelectedValue <> EMPTY Then
                If CacheListIsEmpty(Cache(Session(ESession.StoreID.ToString) & ECache.OrderList), GetType(OrderDetails)) Then GetOrderList(Cache(Session(ESession.StoreID.ToString) & ECache.OrderList), Session(ESession.StoreID.ToString), ALL)
                Dim Order As OrderDetails = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.OrderList), List(Of OrderDetails)).Find(Function(orderItem As OrderDetails) orderItem.OrderID = ddlLocateOrderID.SelectedValue)
                'REMOVED: (for Ajax Autocomplete) Dim Order As OrderDetails = DirectCast(_orderList, List(Of OrderDetails)).Find(Function(orderItem As OrderDetails) orderItem.OrderId = txtLocateOrderID.Text)
                If Order Is Nothing Then Throw New NullReferenceException
                Dim Supplier As SupplierDetails = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.SupplierList), List(Of SupplierDetails)).Find(Function(supplierItem As SupplierDetails) supplierItem.SupplierId = Order.SupplierID)
                Dim OrderItemList As List(Of OrderItemDetails)

                lblLocateOrderID.Text = Order.OrderID
                If Supplier IsNot Nothing Then
                    ddlLocateSupplierID.SelectedValue = Order.SupplierID
                    lblLocateCompanyName.Text = Supplier.CompanyName
                End If
                txtLocateGebizPONo.Text = Order.GebizPONo
                ddlLocateDocType.SelectedValue = Order.Type
                txtLocateOrderDte.Text = Order.Dte.ToString("dd/MM/yyyy")
                Session(ESession.OrderDte.ToString) = Order.Dte
                lblLocateGTotalCost.Text = "0.0000" 'to be computed later
                ViewState(EViewState.RecordStatus) = Order.Status
                ViewState(EViewState.Mode) = EMPTY

                ' UAT01 - always get all the Order Items from the selected Order Reference
                Using Client = New ServiceClient
                    OrderItemList = Client.GetOrderItem(Session(ESession.StoreID.ToString).ToString _
                                                        , lblLocateOrderID.Text _
                                                        , False)
                End Using

                For Each item In OrderItemList
                    Dim OrderItemDetailsItem As New OrderItemDetails

                    OrderItemDetailsItem.OrderItemID = item.OrderItemID
                    OrderItemDetailsItem.StockItemID = item.StockItemID
                    OrderItemDetailsItem.Qty = item.Qty
                    OrderItemDetailsItem.TotalCost = item.TotalCost
                    UpdateGTotal(item.TotalCost)
                    OrderItemDetailsItem.ExpectedDeliveryDte = item.ExpectedDeliveryDte
                    OrderItemDetailsItem.WarrantyDte = item.WarrantyDte
                    OrderItemDetailsItem.Remarks = item.Remarks
                    OrderItemDetailsItem.ReceiveItemQtyReceived = item.ReceiveItemQtyReceived

                    AddOrderItem(DCPLocate, OrderItemDetailsItem, EViewState.OrderItemLocate.ToString)

                    ' keep a list of ordered stock item, to check against to restrict duplicate
                    DirectCast(ViewState(EViewState.OrderedStockItemIDLocate), List(Of String)).Add(OrderItemDetailsItem.StockItemID)
                Next

                ' UAT01 - disable deletion when Order Status is Closed
                If Order.Status = CLOSED Then
                    pnlLocateAccess.Enabled = False
                End If

                pnlLocate.Visible = True
            Else

                ddlLocateOrderID.Enabled = True
                cbxLocateUnfulfillOnly.Enabled = True
                pnlLocateOrder.Enabled = False

                pnlLocate.Visible = False
                Message = GetMessage(messageID.MandatoryField)
            End If

        Catch ex As FaultException
            ddlLocateOrderID.Enabled = True
            cbxLocateUnfulfillOnly.Enabled = True
            Message = ex.Message

        Catch ex As NullReferenceException
            ddlLocateOrderID.Enabled = True
            cbxLocateUnfulfillOnly.Enabled = True
            Message = GetMessage(messageID.InvalidValue, "Order Reference")

        Catch ex As Exception
            ddlLocateOrderID.Enabled = True
            cbxLocateUnfulfillOnly.Enabled = True
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
        ddlLocateOrderID.SelectedIndex = -1

        ' Enable/Disable control for editing
        ddlLocateOrderID.Enabled = True
        cbxLocateUnfulfillOnly.Enabled = True
        pnlLocateAccess.Enabled = False
        pnlLocateOrder.Enabled = False
        pnlLocateOrderItem.Enabled = False
        pnlLocateAction.Enabled = False

        pnlLocate.Visible = False
        'ddlLocateStockCode.Items.Clear()
        txtLocateStockCode.Text = String.Empty
        uplLocateStockCode.Visible = False

        DCPLocate.Controls.Clear()
        ViewState.Remove(EViewState.Mode)
        ViewState.Remove(EViewState.OrderedStockItemIDLocate)
    End Sub

    Protected Sub btnLocateEdit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLocateEdit.Click
        ' Enable/Disable control for editing
        pnlLocateAccess.Enabled = False
        pnlLocateOrder.Enabled = True
        pnlLocateOrderItem.Enabled = True
        pnlLocateAction.Enabled = True

        ' bind stock code DDL
        'If CacheListIsEmpty(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), GetType(ItemDetails)) Then GetItemList(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), Session(ESession.StoreID.ToString))
        'BindDropDownList(ddlLocateStockCode, Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), "ItemID", "ItemID_Description")
        '
        If Not uplLocateStockCode.Visible Then
            uplLocateStockCode.Visible = True
            uplLocateOrder.Update()
        End If
    End Sub

    Protected Sub btnLocateAddItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLocateAddItem.Click
        Try
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

                Dim OrderItemDetailsItem As New OrderItemDetails

                Dim Ordered As Boolean = DirectCast(ViewState(EViewState.OrderedStockItemIDLocate), List(Of String)).Exists(Function(i As String) (i = StockCode))

                If Ordered Then
                    Throw New ApplicationException(GetMessage(messageID.StockCodeHasOrdered, StockCode))
                End If

                OrderItemDetailsItem.ExpectedDeliveryDte = ConvertToDate(txtLocateOrderDte.Text).AddDays(ViewState(EViewState.DeliveryLapseDay))
                OrderItemDetailsItem.StockItemID = StockCode
                AddOrderItem(DCPLocate, OrderItemDetailsItem, EViewState.OrderItemLocate.ToString, True)

                ' keep a list of ordered stock item, to check against to restrict duplicate
                DirectCast(ViewState(EViewState.OrderedStockItemIDLocate), List(Of String)).Add(OrderItemDetailsItem.StockItemID)

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

            If DirectCast(ViewState(EViewState.OrderedStockItemIDLocate), List(Of String)).Count = 0 Then
                ' Delete Order when no item left
                btnLocateDel_Click(Nothing, Nothing)

            Else
                'save data to database
                Dim Client As ServiceClient
                Dim Order As OrderDetails
                Dim OrderItem As OrderItem
                Dim OrderItemDetails As OrderItemDetails
                Dim OrderItemDetailsList As List(Of OrderItemDetails)

                ' Order Details
                Order = New OrderDetails
                Order.StoreID = Session(ESession.StoreID.ToString).ToString
                Order.OrderID = ddlLocateOrderID.SelectedValue
                Order.GebizPONo = txtLocateGebizPONo.Text
                Order.Type = ddlLocateDocType.SelectedValue
                Order.Dte = ConvertToDate(txtLocateOrderDte.Text)
                Order.SupplierID = ddlLocateSupplierID.SelectedValue
                'Order.Status = OPEN  'will check and update accordingly using [spUpdateOrder] 
                Order.LoginUser = Session(ESession.UserID.ToString).ToString
                Order.Mode = ViewState(EViewState.Mode).ToString

                ' Order item Details
                OrderItemDetailsList = New List(Of OrderItemDetails)
                For i As Integer = 1 To ViewState(EViewState.OrderItemLocate.ToString)
                    OrderItem = TryCast(DCPLocate.FindControl(EViewState.OrderItemLocate.ToString + i.ToString), OrderItem)

                    If OrderItem.Message <> EMPTY Then
                        Message = OrderItem.Message
                        Exit Sub
                    End If

                    OrderItemDetails = New OrderItemDetails
                    If OrderItem IsNot Nothing Then
                        Dim Value As String

                        Value = Trim(DirectCast(OrderItem.FindControl("hfMode"), HiddenField).Value)
                        If Value.Length > 0 Then
                            OrderItemDetails.Mode = Value

                            Value = Trim(DirectCast(OrderItem.FindControl("hfOrderItemID"), HiddenField).Value)
                            If Not IsNumeric(Value) Then Throw New ArgumentNullException
                            OrderItemDetails.OrderItemID = CInt(Value)

                            Value = Trim(DirectCast(OrderItem.FindControl("lblStockCode"), Label).Text)
                            If Value.Length <= 0 Then Throw New ArgumentNullException
                            OrderItemDetails.StockItemID = Value

                            If OrderItemDetails.Mode = INSERT Or OrderItemDetails.Mode = UPDATE Then
                                Value = Trim(DirectCast(OrderItem.FindControl("txtOrderQty"), TextBox).Text)
                                If Not IsNumeric(Value) Then Throw New ArgumentNullException
                                OrderItemDetails.Qty = Convert.ToDecimal(Value)

                                Value = Trim(DirectCast(OrderItem.FindControl("txtTotalCost"), TextBox).Text)
                                If Not IsNumeric(Value) Then Throw New ArgumentNullException
                                OrderItemDetails.TotalCost = Convert.ToDouble(Value)

                                Value = Trim(DirectCast(OrderItem.FindControl("txtExpectedDeliveryDte"), TextBox).Text)
                                If ConvertToDate(Value) = DateTime.MinValue Then Throw New ArgumentNullException
                                OrderItemDetails.ExpectedDeliveryDte = ConvertToDate(Value)

                                ' Optional field, if blank let it be null value
                                Value = Trim(DirectCast(OrderItem.FindControl("txtWarrantyDte"), TextBox).Text)
                                If Value.Length > 0 Then
                                    If ConvertToDate(Value) = DateTime.MinValue Then : Throw New ArgumentNullException
                                    Else : OrderItemDetails.WarrantyDte = ConvertToDate(Value)
                                    End If
                                End If

                                Value = Trim(DirectCast(OrderItem.FindControl("txtRemarks"), TextBox).Text)
                                If Value.Length > 0 Then OrderItemDetails.Remarks = Value Else Throw New ArgumentNullException

                            End If
                            'OrderItemDetails.Status = ' [spUpdateOrderItem] will check and update accordingly

                            OrderItemDetailsList.Add(OrderItemDetails)
                        End If
                    End If
                Next

                If Not (Order.Mode = EMPTY And OrderItemDetailsList.Count = 0) Then
                    Client = New ServiceClient
                    Message = Client.UpdateOrder(Order, OrderItemDetailsList)
                    Client.Close()

                    If Message = String.Empty Then
                        ' unload orderlist cache to allow reload
                        If Order.Mode = UPDATE Then
                            ' add new order to cache and sort it
                            If Cache(Session(ESession.StoreID.ToString) & ECache.OrderList) IsNot Nothing Then
                                EditCache(Cache.Remove(ECache.OrderList) _
                                          , GetType(OrderDetails) _
                                          , Order _
                                          , True _
                                          )
                            End If
                            If Cache(Session(ESession.StoreID.ToString) & ECache.UnfulfilledOrder) IsNot Nothing Then
                                EditCache(Cache(Session(ESession.StoreID.ToString) & ECache.UnfulfilledOrder) _
                                          , GetType(OrderDetails) _
                                          , Order _
                                          , True _
                                          )
                            End If

                        End If

                        'clear screen
                        btnLocateClear_Click(sender, e)
                        MainPanel(True)

                        'display Alert Message
                        Message = GetMessage(messageID.Success, "Edited", "Order")
                    End If

                Else
                    Message = "Record not amended, please update and try again"
                End If
            End If

        Catch ex As FaultException
            Message = ex.Message

        Catch ex As ArgumentNullException
            Message = clsCommonFunction.GetMessage(clsCommonFunction.messageID.MandatoryField)

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
            Dim Client As New ServiceClient

            Message = Client.DeleteOrder(Session(ESession.StoreID.ToString).ToString _
                                                   , ddlLocateOrderID.SelectedValue _
                                                   , ViewState(EViewState.RecordStatus) _
                                                   , Session(ESession.UserID.ToString).ToString _
                                                   )

            Client.Close()
            If Message = EMPTY Then
                ddlOrderReference.Items.Remove(ddlLocateOrderID.SelectedValue)
                ddlLocateOrderID.Items.Remove(ddlLocateOrderID.SelectedValue)

                ' remove deleted order from cache
                Dim OrderDetailsItem As OrderDetails = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.OrderList), List(Of OrderDetails)).Find(Function(orderItem As OrderDetails) orderItem.OrderID = ddlLocateOrderID.SelectedValue)
                If OrderDetailsItem IsNot Nothing Then DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.OrderList), List(Of OrderDetails)).Remove(OrderDetailsItem)

                OrderDetailsItem = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.UnfulfilledOrder), List(Of OrderDetails)).Find(Function(orderItem As OrderDetails) orderItem.OrderID = ddlLocateOrderID.SelectedValue)
                If OrderDetailsItem IsNot Nothing Then DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.UnfulfilledOrder), List(Of OrderDetails)).Remove(OrderDetailsItem)

                'clear screen
                btnLocateClear_Click(sender, e)

                'display Alert Message
                Message = GetMessage(messageID.Success, "deleted", "Order")
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

    Protected Sub txtLocateGebizPONo_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtLocateGebizPONo.TextChanged
        Try
            If Trim(txtLocateGebizPONo.Text).Length > 0 Then
                'TODO: check unique id
                Dim Client As New ServiceClient

                If (Client.FieldIsUnique(Session(ESession.StoreID.ToString) _
                                         , ServiceColumnName.OrderGebizPONo _
                                         , txtLocateGebizPONo.Text _
                                         , lblLocateOrderID.Text _
                                         ) _
                    ) Then
                    IsDirty()
                Else
                    Message = String.Format("Gebiz PO No:({0}) exists in the system", Trim(txtLocateGebizPONo.Text))

                    txtLocateGebizPONo.Focus()
                End If
                Client.Close()
            End If

        Catch ex As FaultException
            Message = ex.Message

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

    Protected Sub ddlLocateSupplierID_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlLocateSupplierID.SelectedIndexChanged
        If CacheListIsEmpty(Cache(Session(ESession.StoreID.ToString) & ECache.SupplierList), GetType(SupplierDetails)) Then GetSupplierList(Cache(Session(ESession.StoreID.ToString) & ECache.SupplierList), Session(ESession.StoreID.ToString))
        Dim Supplier As SupplierDetails = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.SupplierList), List(Of SupplierDetails)).Find(Function(supplierItem As SupplierDetails) supplierItem.SupplierId = ddlLocateSupplierID.SelectedValue)

        If Supplier IsNot Nothing Then
            If Supplier.CompanyName <> EMPTY Then lblLocateCompanyName.Text = Supplier.CompanyName
            IsDirty()
        End If
    End Sub

    Protected Sub ddlLocateDocType_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlLocateDocType.SelectedIndexChanged
        IsDirty()
    End Sub

    Protected Sub txtLocateOrderDte_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtLocateOrderDte.TextChanged
        IsDirty()
    End Sub

    Protected Sub ShowLocateModal_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ShowLocateModal.Click
        mpuLocateStockAvailability.Show()
        uplLocateUserControl.Update()
    End Sub

    Private Sub IsDirty()
        ViewState(EViewState.Mode) = UPDATE
    End Sub

    Protected Sub cbxLocateUnfulfillOnly_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbxLocateUnfulfillOnly.CheckedChanged
        Try

            If cbxLocateUnfulfillOnly.Checked Then
                If CacheListIsEmpty(Cache(Session(ESession.StoreID.ToString) & ECache.UnfulfilledOrder), GetType(OrderDetails)) Then
                    If CacheListIsEmpty(Cache(Session(ESession.StoreID.ToString) & ECache.OrderList), GetType(OrderDetails)) Then GetOrderList(Cache(Session(ESession.StoreID.ToString) & ECache.OrderList), Session(ESession.StoreID.ToString), ALL)
                    Cache(Session(ESession.StoreID.ToString) & ECache.UnfulfilledOrder) = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.OrderList), List(Of OrderDetails)).FindAll(Function(i As OrderDetails) i.Status <> CLOSED)
                End If
                BindDropDownList(ddlLocateOrderID, Cache(Session(ESession.StoreID.ToString) & ECache.UnfulfilledOrder), "OrderId", "OrderId")
            Else
                If CacheListIsEmpty(Cache(Session(ESession.StoreID.ToString) & ECache.OrderList), GetType(OrderDetails)) Then GetOrderList(Cache(Session(ESession.StoreID.ToString) & ECache.OrderList), Session(ESession.StoreID.ToString), ALL)
                BindDropDownList(ddlLocateOrderID, Cache(Session(ESession.StoreID.ToString) & ECache.OrderList), "OrderId", "OrderId")
            End If

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try

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

            ItemDetails.StoreID = Session("StoreID").ToString
            ItemDetails.ItemID = StockCodeTo

            If Not Client.IsValidStockCode(ItemDetails) And ddlOrderReference.SelectedIndex = 0 Then

                Message = GetMessage(messageID.InvalidStockCode, StockCodeTo.ToUpper)
                Client.Close()
                Exit Sub

            End If

            Client.Close()
        End Using

        rvr.LocalReport.DataSources.Clear()
        rvr.LocalReport.DataSources.Add(New ReportDataSource("OrderList", ObjectDataSource1))

        Dim parameterlist As New List(Of ReportParameter)
        Dim p1 As New ReportParameter("StoreName", Session(ESession.StoreName.ToString).ToString)
        Dim p2 As New ReportParameter("PrintOption", ddlPrintOption.SelectedValue)
        Dim p3 As New ReportParameter("PODateFrom", txtPODateFrom.Text)
        Dim p4 As New ReportParameter("PODateTo", txtPODateTo.Text)
        Dim p5 As New ReportParameter("StockCodeFrom", StockCodeFrom)
        Dim p6 As New ReportParameter("StockCodeTo", StockCodeTo)
        Dim p7 As New ReportParameter("OrderReference", ddlOrderReference.SelectedValue)
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
        Response.AddHeader("content-disposition", "attachment;filename=OrderList.pdf")
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

            If Not Client.IsValidStockCode(ItemDetails) And ddlOrderReference.SelectedIndex = 0 Then

                Message = GetMessage(messageID.InvalidStockCode, StockCodeFrom.ToUpper)
                Client.Close()
                Exit Sub

            End If

            ItemDetails.StoreID = Session("StoreID").ToString
            ItemDetails.ItemID = StockCodeTo

            If Not Client.IsValidStockCode(ItemDetails) Then

                Message = GetMessage(messageID.InvalidStockCode, StockCodeTo.ToUpper)
                Client.Close()
                Exit Sub

            End If

            Client.Close()
        End Using

        rvr.LocalReport.DataSources.Clear()
        rvr.LocalReport.DataSources.Add(New ReportDataSource("OrderList", ObjectDataSource1))

        Dim parameterlist As New List(Of ReportParameter)
        Dim p1 As New ReportParameter("StoreName", Session(ESession.StoreName.ToString).ToString)
        Dim p2 As New ReportParameter("PrintOption", ddlPrintOption.SelectedValue)
        Dim p3 As New ReportParameter("PODateFrom", txtPODateFrom.Text)
        Dim p4 As New ReportParameter("PODateTo", txtPODateTo.Text)
        Dim p5 As New ReportParameter("StockCodeFrom", StockCodeFrom)
        Dim p6 As New ReportParameter("StockCodeTo", StockCodeTo)
        Dim p7 As New ReportParameter("OrderReference", ddlOrderReference.SelectedValue)
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
        Response.AddHeader("content-disposition", "attachment;filename=OrderList.xls")
        Response.BinaryWrite(bytValue)
        Response.Flush()
        Response.End()
    End Sub

    Protected Sub ObjectDataSource1_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs) Handles ObjectDataSource1.Selecting

        '-- VALIDATE STOCK CODE
        Dim StockCodeFrom, StockCodeTo As String
        StockCodeFrom = Split(txtStockCodeFrom.Text, " | ")(0).Trim
        StockCodeTo = Split(txtStockCodeTo.Text, " | ")(0).Trim

        e.InputParameters("storeID") = Session(ESession.StoreID.ToString)
        e.InputParameters("status") = ddlPrintOption.SelectedValue
        e.InputParameters("fromDte") = DateTime.ParseExact(Me.txtPODateFrom.Text, "dd/MM/yyyy", Nothing)
        e.InputParameters("toDte") = DateTime.ParseExact(Me.txtPODateTo.Text, "dd/MM/yyyy", Nothing)
        e.InputParameters("fromStockItemID") = StockCodeFrom 'ddlStockCodeFrom.SelectedValue
        e.InputParameters("toStockItemID") = StockCodeTo 'ddlStockCode2.SelectedValue
        e.InputParameters("orderId") = ddlOrderReference.SelectedValue
    End Sub

    Protected Sub ObjectDataSource1_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles ObjectDataSource1.Selected
        Dim returnList As List(Of OrderList) = e.ReturnValue
        If returnList.Count <= 0 Then
            NoRecordFond = "Y"
        Else
            NoRecordFond = "N"
        End If
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClear.Click
        ddlPrintOption.SelectedIndex = -1
        txtPODateFrom.Text = Today.ToString("01/MM/yyyy")
        txtPODateTo.Text = Today.ToString("dd/MM/yyyy")
        'ddlStockCodeFrom.SelectedIndex = -1
        'ddlStockCode2.SelectedIndex = -1
        txtStockCodeFrom.Text = String.Empty
        txtStockCodeTo.Text = String.Empty
        ddlOrderReference.SelectedIndex = -1
    End Sub

    Private Function ValidPrint() As Boolean
        ' validate filtering criteria

        '-- VALIDATE STOCK CODE
        Dim StockCodeFrom, StockCodeTo As String
        StockCodeFrom = Split(txtStockCodeFrom.Text, " | ")(0).Trim
        StockCodeTo = Split(txtStockCodeTo.Text, " | ")(0).Trim

        If ddlOrderReference.SelectedValue = EMPTY Then
            If (ddlPrintOption.SelectedValue = EMPTY _
                Or Trim(txtPODateFrom.Text) = EMPTY _
                Or Trim(txtPODateTo.Text) = EMPTY _
                Or txtStockCodeFrom.Text = EMPTY _
                ) Then
                Message = GetMessage(messageID.MandatoryField)
                Exit Function
            End If

            If ConvertToDate(txtPODateFrom.Text) > ConvertToDate(txtPODateTo.Text) Then
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
            Select Case tbcOrderItem.ActiveTab.ID
                Case NEWTAB
                    txtOrderID.Enabled = enabled
                    txtGebizPONo.Enabled = enabled
                    ddlSupplierID.Enabled = enabled
                    ddlDocType.Enabled = enabled
                    txtOrderDte.Enabled = enabled
                    divAddButton.Visible = enabled

                    pnlNewOrder.Visible = (Not enabled)
                    uplNewOrder.Update()
                    uplUserControl.Update()

            End Select

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
    Protected Friend Sub ViewStock(ByVal uniqueID As String)
        Try
            'get info from Cache
            Dim StockCode As String = DirectCast(ViewState(uniqueID), MoreItemInfoDetails).ItemID()
            Dim DetailsItem As ItemDetails = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), List(Of ItemDetails)).Find(Function(item As ItemDetails) item.ItemID = StockCode)
            Dim UOM As String = " " + DetailsItem.UOM

            If tbcOrderItem.ActiveTab.ID = NEWTAB Then
                ViewStockCode.Text = StockCode
                ViewDesc.Text = DetailsItem.ItemDescription
                ViewBalance.Text = DirectCast(ViewState(uniqueID), MoreItemInfoDetails).Balance.ToString("0.00") + UOM
                ViewAUC.Text = "$ " + DisplayValue(DirectCast(ViewState(uniqueID), MoreItemInfoDetails).AvgUnitCost)
                ViewTotalValue.Text = "$ " + DisplayValue(DirectCast(ViewState(uniqueID), MoreItemInfoDetails).TotalValue)
                ViewMax.Text = DetailsItem.MaxLevel.ToString("0.00") + UOM
                ViewOnOrderQty.Text = DirectCast(ViewState(uniqueID), MoreItemInfoDetails).OnOrderQty.ToString("0.00") + UOM
                ViewOnOrderCount.Text = DirectCast(ViewState(uniqueID), MoreItemInfoDetails).OnOrderCount

                ShowModal.Visible = True
                ShowModal_Click(ShowModal, New EventArgs)

            ElseIf tbcOrderItem.ActiveTab.ID = LOCATETAB Then
                ViewLocateStockCode.Text = StockCode
                ViewLocateDesc.Text = DetailsItem.ItemDescription
                ViewLocateBalance.Text = DirectCast(ViewState(uniqueID), MoreItemInfoDetails).Balance.ToString("0.00") + UOM
                ViewLocateAUC.Text = "$ " + DisplayValue(DirectCast(ViewState(uniqueID), MoreItemInfoDetails).AvgUnitCost)
                ViewLocateTotalValue.Text = "$ " + DisplayValue(DirectCast(ViewState(uniqueID), MoreItemInfoDetails).TotalValue)
                ViewLocateMax.Text = DetailsItem.MaxLevel.ToString("0.00") + UOM
                ViewLocateOnOrderQty.Text = DirectCast(ViewState(uniqueID), MoreItemInfoDetails).OnOrderQty.ToString("0.00") + UOM
                ViewLocateOnOrderCount.Text = DirectCast(ViewState(uniqueID), MoreItemInfoDetails).OnOrderCount

                ShowLocateModal.Visible = True
                ShowLocateModal_Click(ShowLocateModal, New EventArgs)
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' To compute the displayed Grand Total value;
    ''' 8 Feb 2009 - KG;
    ''' </summary>
    ''' <param name="value"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Friend Sub UpdateGTotal(Optional ByVal value As Double = 0.0, Optional ByVal reset As Boolean = False)
        Try
            'Dim OrderItem As OrderItem
            'Dim UserControlName As String = EMPTY
            'Dim GTotal As Double = 0.0
            'Dim Value As String = EMPTY
            'If tbcOrderItem.ActiveTab.ID = NEWTAB Then
            '    UserControlName = EViewState.OrderItem.ToString
            '    For i As Integer = 1 To ViewState(UserControlName)
            '        OrderItem = TryCast(DCP.FindControl(UserControlName + i.ToString), OrderItem)

            '        Value = DirectCast(OrderItem.FindControl("txtTotalCost"), TextBox).Text
            '        If (IsNumeric(Value)) Then
            '            GTotal += Value
            '        End If
            '    Next
            '    lblGTotal.Text = DisplayValue(GTotal)
            '    uplMain.Update()

            'ElseIf tbcOrderItem.ActiveTab.ID = LOCATETAB Then
            '    UserControlName = EViewState.OrderItemLocate.ToString
            '    For i As Integer = 1 To ViewState(UserControlName)
            '        OrderItem = TryCast(DCPLocate.FindControl(UserControlName + i.ToString), OrderItem)

            '        Value = DirectCast(OrderItem.FindControl("txtTotalCost"), TextBox).Text
            '        If (IsNumeric(Value)) Then
            '            GTotal += Value
            '        End If
            '    Next
            '    lblLocateGTotalCost.Text = DisplayValue(GTotal)
            '    uplLocateMainOrder.Update()
            'End If

            If reset Then
                If tbcOrderItem.ActiveTab.ID = NEWTAB Then
                    lblGTotal.Text = DisplayValue(Value)

                ElseIf tbcOrderItem.ActiveTab.ID = LOCATETAB Then
                    lblLocateGTotalCost.Text = DisplayValue(Value)
                End If
            Else
                Dim Total As Double = 0.0
                If tbcOrderItem.ActiveTab.ID = NEWTAB Then
                    If IsNumeric(lblGTotal.Text) Then Total = Convert.ToDouble(lblGTotal.Text)
                    lblGTotal.Text = DisplayValue(Total + Value)
                    uplMain.Update()
                ElseIf tbcOrderItem.ActiveTab.ID = LOCATETAB Then
                    If IsNumeric(lblLocateGTotalCost.Text) Then Total = Convert.ToDouble(lblLocateGTotalCost.Text)
                    lblLocateGTotalCost.Text = DisplayValue(Total + Value)
                    uplLocateMain.Update()
                End If
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Add the User Control in the PlaceHolder;
    ''' 8 Feb 2009 - KG;
    ''' </summary>
    ''' <param name="PlaceHolder">position</param>
    ''' <param name="orderItemDetailsItem">Control Info</param>
    ''' <param name="UserControlName"></param>
    ''' <param name="newItem">new item added</param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Private Sub AddOrderItem(ByRef PlaceHolder As DynamicControlsPlaceholder, ByVal orderItemDetailsItem As OrderItemDetails, ByVal UserControlName As String, Optional ByVal newItem As Boolean = False)
        Try
            Dim OrderItem = New OrderItem

            ' increment user control ID
            If IsNothing(ViewState(UserControlName)) Then
                ViewState(UserControlName) = 1
            Else
                ViewState(UserControlName) += 1
            End If

            ' load user control at top of placeholder
            OrderItem = LoadControl("OrderItem.ascx")
            OrderItem.ID = UserControlName + Convert.ToString(ViewState(UserControlName))
            PlaceHolder.Controls.AddAt(0, OrderItem)


            ' retrieve Cache more Info for the Stock Item to be display using Modal
            Using Client = New ServiceClient
                ViewState(OrderItem.UniqueID) = Client.GetMoreItemInfo(Session(ESession.StoreID.ToString).ToString, orderItemDetailsItem.StockItemID, Today)
            End Using

            ' Assign value to controls
            If CacheListIsEmpty(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), GetType(ItemDetails)) Then GetItemList(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), Session(ESession.StoreID.ToString))
            Dim Index As Integer = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), List(Of ItemDetails)).IndexOf(DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), List(Of ItemDetails)).Find(Function(i As ItemDetails) (i.ItemID = orderItemDetailsItem.StockItemID)))
            Dim MaxLevel As Decimal = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), List(Of ItemDetails)).Item(Index).MaxLevel
            Dim Balance As Decimal = DirectCast(ViewState(OrderItem.UniqueID), MoreItemInfoDetails).Balance
            Dim OnOrderQty As Decimal = DirectCast(ViewState(OrderItem.UniqueID), MoreItemInfoDetails).OnOrderQty
            Dim AllowQty As Decimal = MaxLevel - Balance - OnOrderQty + orderItemDetailsItem.Qty ' UAT02.51
            Dim ErrorMessage As String = String.Format("Maximum level [{0}] reached. The Current Balance [{1}] plus on Order quantity [{2}] reached maximum limits.", MaxLevel.ToString("0.00"), Balance.ToString("0.00"), OnOrderQty.ToString("0.00"))
            DirectCast(OrderItem.FindControl("hfAllowQty"), HiddenField).Value = AllowQty.ToString("0.00")
            DirectCast(OrderItem.FindControl("lblErr"), Label).Text = IIf((AllowQty > 0), (GetMessage(messageID.MoreLessThan, , , "Order Quantity", AllowQty.ToString, "<= (Less Than or Equal)")), ErrorMessage)
            DirectCast(OrderItem.FindControl("lblStockCode"), Label).Text = orderItemDetailsItem.StockItemID.ToString
            DirectCast(OrderItem.FindControl("lblDescription"), Label).Text = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), List(Of ItemDetails)).Item(Index).ItemDescription
            DirectCast(OrderItem.FindControl("lblUOM"), Label).Text = DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.ItemList), List(Of ItemDetails)).Item(Index).UOM
            DirectCast(OrderItem.FindControl("txtTotalCost"), TextBox).Text = IIf(orderItemDetailsItem.TotalCost > 0.0, orderItemDetailsItem.TotalCost.ToString("0.00"), EMPTY)
            DirectCast(OrderItem.FindControl("txtOrderQty"), TextBox).Text = IIf(orderItemDetailsItem.Qty > 0D, orderItemDetailsItem.Qty.ToString("0.00"), EMPTY)
            DirectCast(OrderItem.FindControl("txtExpectedDeliveryDte"), TextBox).Text = IIf(orderItemDetailsItem.ExpectedDeliveryDte > DateTime.MinValue, orderItemDetailsItem.ExpectedDeliveryDte.Day.ToString.PadLeft(2, "0") & "/" & orderItemDetailsItem.ExpectedDeliveryDte.Month.ToString.PadLeft(2, "0") & "/" & orderItemDetailsItem.ExpectedDeliveryDte.Year.ToString, EMPTY)
            DirectCast(OrderItem.FindControl("txtWarrantyDte"), TextBox).Text = IIf(orderItemDetailsItem.WarrantyDte > DateTime.MinValue, orderItemDetailsItem.WarrantyDte.Day.ToString.PadLeft(2, "0") & "/" & orderItemDetailsItem.WarrantyDte.Month.ToString.PadLeft(2, "0") & "/" & orderItemDetailsItem.WarrantyDte.Year.ToString, EMPTY)
            If (orderItemDetailsItem.Remarks IsNot Nothing) Then DirectCast(OrderItem.FindControl("txtRemarks"), TextBox).Text = orderItemDetailsItem.Remarks.ToString
            DirectCast(OrderItem.FindControl("hfMode"), HiddenField).Value = IIf(newItem, INSERT, EMPTY)
            DirectCast(OrderItem.FindControl("hfOrderItemID"), HiddenField).Value = IIf(newItem, 0, orderItemDetailsItem.OrderItemID)
            DirectCast(OrderItem.FindControl("hfReceiveQty"), HiddenField).Value = IIf(newItem, 0, orderItemDetailsItem.ReceiveItemQtyReceived)

            'UAT02.01 - used within user control to check its update mode
            DirectCast(OrderItem.FindControl("hfOrgTotalCost"), HiddenField).Value = orderItemDetailsItem.TotalCost
            DirectCast(OrderItem.FindControl("hfOrgOrderQty"), HiddenField).Value = orderItemDetailsItem.Qty
            DirectCast(OrderItem.FindControl("hfOrgEDDte"), HiddenField).Value = IIf(orderItemDetailsItem.ExpectedDeliveryDte > DateTime.MinValue, orderItemDetailsItem.ExpectedDeliveryDte.ToString("dd/MM/yyyy"), EMPTY)
            DirectCast(OrderItem.FindControl("hfOrgWarrantyDte"), HiddenField).Value = IIf(orderItemDetailsItem.WarrantyDte > DateTime.MinValue, orderItemDetailsItem.WarrantyDte.ToString("dd/MM/yyyy"), EMPTY)
            If (orderItemDetailsItem.Remarks IsNot Nothing) Then DirectCast(OrderItem.FindControl("hfOrgRemarks"), HiddenField).Value = orderItemDetailsItem.Remarks.ToString

            ' Client Side Java Scripting
            DirectCast(OrderItem.FindControl("txtOrderQty"), TextBox).Attributes.Add("onkeyup", "computeItemUnit('" & DirectCast(OrderItem.FindControl("txtOrderQty"), TextBox).ClientID & "','" & DirectCast(OrderItem.FindControl("txtTotalCost"), TextBox).ClientID & "','" & DirectCast(OrderItem.FindControl("lblUnitCost"), Label).ClientID & "');")
            DirectCast(OrderItem.FindControl("txtTotalCost"), TextBox).Attributes.Add("onkeyup", "computeItemUnit('" & DirectCast(OrderItem.FindControl("txtOrderQty"), TextBox).ClientID & "','" & DirectCast(OrderItem.FindControl("txtTotalCost"), TextBox).ClientID & "','" & DirectCast(OrderItem.FindControl("lblUnitCost"), Label).ClientID & "');")

            If (newItem) Then
                ' extra check for new item
                If AllowQty <= 0 Then
                    PlaceHolder.Controls.RemoveAt(0)

                    Message = ErrorMessage
                End If
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Friend Sub CancelOrderItem(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim orderItem As New OrderItem

            ' sender will tell if from New or Locate
            orderItem = DirectCast(FindControl(sender), OrderItem)

            If tbcOrderItem.ActiveTab.ID = NEWTAB Then
                ' remove from EViewState.OrderedStockItemID
                DirectCast(ViewState(EViewState.OrderedStockItemID), List(Of String)).Remove(DirectCast(orderItem.FindControl("lblStockCode"), Label).Text)

                DCP.Controls.Remove(FindControl(sender))
                uplUserControl.Update()
            ElseIf tbcOrderItem.ActiveTab.ID = LOCATETAB Then
                'TODO: Check this deletion doesn't cause a negative value to the stockitem
                If False = False Then
                    ' remove from EViewState.OrderedStockItemIDLocate
                    DirectCast(ViewState(EViewState.OrderedStockItemIDLocate), List(Of String)).Remove(DirectCast(orderItem.FindControl("lblStockCode"), Label).Text)

                    If DirectCast(orderItem.FindControl("hfMode"), HiddenField).Value = INSERT Then
                        DCPLocate.Controls.Remove(FindControl(sender))
                    Else
                        DirectCast(orderItem.FindControl("hfMode"), HiddenField).Value = DELETE
                        orderItem.Visible = False
                    End If

                    ' Prompt whole Order will be deleted when its at the last item
                    If DirectCast(ViewState(EViewState.OrderedStockItemIDLocate), List(Of String)).Count = 0 Then
                        Message = "The last item is also removed.  The whole Order reference will be deleted upon clicking Save."
                    End If

                Else
                    Message = "This action will cause the Stock Item Qty to fall below Zero value."
                End If
                uplLocateUserControl.Update()

            End If

        Catch ex As Exception
            Message = GetMessage(messageID.TryLastOperation)
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then Throw
        End Try
    End Sub

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

#Region " Unused Code & Web Method for Ajax Auto complete (REMOVED: cannot use Auto Complete as it need Shared Variable) "


    'Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOK.Click
    '    uplMain.Visible = True
    '    divMsgBox.Visible = False
    'End Sub

    '<WebMethod()> _
    '<Script.Services.ScriptMethod()> _
    'Public Shared Function GetOrders(ByVal prefixText As String, _
    '                                 ByVal count As Integer _
    '                                 ) As List(Of String)
    '    Dim list As New List(Of String)
    '    Try
    '        If _orderList Is Nothing Then Throw New Exception

    '        For Each item In _orderList
    '            'If String.Compare(item.OrderId, prefixText, True) >= 0 Then
    '            'If item.OrderId.StartsWith(prefixText) Then
    '            If item.OrderId.Substring(0, prefixText.Length).ToUpper = prefixText.ToUpper Then
    '                list.Add(item.OrderId)
    '            End If

    '            If list.Count > count Then Exit For
    '        Next

    '    Catch ex As Exception
    '        Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
    '        If (rethrow) Then Throw
    '    End Try

    '    Return list
    'End Function

    'If orderItemDetailsItem.StockItemID IsNot Nothing Then
    '    ' Uses reflection to invoke method in the User Control
    '    Dim typUserControl As Type = Nothing
    '    Dim mthdUserControl As MethodInfo = Nothing
    '    Dim arrParameter As String() = New String(1) {}

    '    arrParameter(0) = "ddlStockCode_SelectedIndexChanged"
    '    arrParameter(1) = orderItemDetailsItem.StockItemID
    '    typUserControl = OrderItem.[GetType]()
    '    mthdUserControl = typUserControl.GetMethod("PublicMethod")
    '    mthdUserControl.Invoke(OrderItem, arrParameter)
    'End If

#End Region

End Class