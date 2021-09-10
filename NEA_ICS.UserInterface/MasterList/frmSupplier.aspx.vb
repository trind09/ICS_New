Imports Microsoft.Reporting.WebForms
Imports System.Web.Services.Description
Imports NEA_ICS.UserInterface.NEA_ICS.WCFService
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ServiceModel
Imports System.Reflection

Partial Public Class frmSupplier
    Inherits clsCommonFunction

    'Private Shared SupplierList As List(Of SupplierDetails)
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
            ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", "ShowAlertMessage('" & Message & "');", True)
        End If
    End Sub

#Region " Page Load "
    ''' <summary>
    ''' Page Load;
    ''' 08 Dec 08 - Jianfa
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CODE LOGIC:
    ''' 1) To initialise disabled fields
    ''' </remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim AccessRights As New List(Of RoleDetails)

        CheckValidSession()

        If Not Page.IsPostBack Then

            AccessRights = clsCommonFunction.AssignAccessRights(Cache(Session(ESession.UserID.ToString) & Session(ESession.StoreID.ToString) & "AccessRights"), _
                                                   clsCommonFunction.moduleID.Supplier)

            If AccessRights(0).SelectRight = False Then
                Server.Transfer("..\frmUnauthorisedPage.aspx")
                Exit Sub
            End If

            If AccessRights(0).InsertRight = False Then
                tbpNewSupplier.Visible = False
            End If

            If AccessRights(0).UpdateRight = False Then
                lbtnLocateEdit.Visible = False
                ViewState("_EDIT") = False
            Else
                ViewState("_EDIT") = True
            End If

            If AccessRights(0).DeleteRight = False Then
                lbtnLocateDel.Visible = False
            End If

            tbcSupplier.Visible = True

        End If

    End Sub
#End Region

#Region " New Tab "
    ''' <summary>
    ''' btnAddSupplier -  Click;
    ''' 18 Dec 08 - Jianfa Chen;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CODE LOGIC:
    ''' 1) Validate Mandatory Fields;
    ''' 2) Validate unique Supplier Code;
    ''' 3) Insert Supplier Details;
    ''' 4) Display Inserted Supplier Details;
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' 18Dec08  Jianfa      01     As indicated     
    ''' </remarks>
    Private Sub btnAddSupplier_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddSupplier.Click

        Try
            ' Check mandatory fields
            If txtSupplierCode.Text.Trim = String.Empty _
                Or txtCompanyName.Text.Trim = String.Empty _
                Or ddlAddressType.SelectedValue.Trim = String.Empty _
                Or txtAddressNo.Text.Trim = String.Empty _
                Or txtStreetName.Text.Trim = String.Empty _
                Or txtPostalCode.Text.Trim = String.Empty _
                Or txtContactPerson.Text.Trim = String.Empty _
                Or txtTelNo.Text.Trim = String.Empty _
            Then

                lblErrAddSupplier.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.MandatoryField)

                Dim Script As String = "ShowAlertMessage('" & lblErrAddSupplier.Text & "');"
                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

                lblErrAddSupplier.Visible = True

            Else

                Dim Client As New ServiceClient
                Dim SupplierDetails As New SupplierDetails

                SupplierDetails.StoreId = Session("StoreID").ToString
                SupplierDetails.SupplierId = txtSupplierCode.Text.ToUpper
                SupplierDetails.CompanyName = txtCompanyName.Text
                SupplierDetails.AddressType = ddlAddressType.SelectedValue
                SupplierDetails.AddressBlockHouseNo = txtAddressNo.Text
                SupplierDetails.AddressStreetName = txtStreetName.Text
                SupplierDetails.AddressFloorNo = txtFloorNo.Text
                SupplierDetails.AddressUnitNo = txtUnitNo.Text
                SupplierDetails.AddressBuildingName = txtBuildingName.Text
                SupplierDetails.AddressPostalCode = txtPostalCode.Text
                SupplierDetails.ContactPerson = txtContactPerson.Text
                SupplierDetails.TelephoneNo = txtTelNo.Text
                SupplierDetails.FaxNo = txtFaxNo.Text
                SupplierDetails.OtherInfo = txtOtherInfo.Text
                SupplierDetails.Status = "O"
                SupplierDetails.LoginUser = Session("UserID").ToString

                lblErrAddSupplier.Text = Client.AddSupplier(SupplierDetails)
                Client.Close()

                If lblErrAddSupplier.Text = String.Empty Then
                    lblErrAddSupplier.Visible = False
                    pnlAddSupplier.Visible = False

                    lblMsgSupplierCode.Text = txtSupplierCode.Text.ToUpper
                    lblMsgCompanyName.Text = txtCompanyName.Text
                    lblMsgAddressType.Text = ddlAddressType.SelectedItem.Text
                    lblMsgAddressNo.Text = txtAddressNo.Text
                    lblMsgStreetName.Text = txtStreetName.Text
                    lblMsgFloorNo.Text = txtFloorNo.Text
                    lblMsgUnitNo.Text = txtUnitNo.Text
                    lblMsgBuildingName.Text = txtBuildingName.Text
                    lblMsgPostalCode.Text = txtPostalCode.Text
                    lblMsgContactPerson.Text = txtContactPerson.Text
                    lblMsgTelephoneNo.Text = txtTelNo.Text
                    lblMsgFaxNo.Text = txtFaxNo.Text
                    lblMsgOtherInfo.Text = txtOtherInfo.Text

                    divMsgBoxAddSupplier.Visible = True


                    '-- UPDATE CACHE
                    If Not Cache(Session(ESession.StoreID.ToString) & ECache.SupplierList) Is Nothing Then
                        clsCommonFunction.EditCache(Cache(Session(ESession.StoreID.ToString) & ECache.SupplierList), GetType(SupplierDetails), SupplierDetails)
                    End If

                Else

                    Dim Script As String = "ShowAlertMessage('" & lblErrAddSupplier.Text & "');"
                    ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

                    lblErrAddSupplier.Visible = True

                End If
            End If

        Catch ex As FaultException
            Dim fault As ServiceFault = ex.Data

            lblErrAddSupplier.Text = fault.MessageText
            lblErrAddSupplier.Visible = True

        Catch ex As Exception
            lblErrAddSupplier.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            lblErrAddSupplier.Visible = True
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub

    ''' <summary>
    ''' btnClear - Click;
    ''' 26 Dec 2008 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' To clear away the UI controls for Add supplier panel
    ''' </remarks>
    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click

        txtSupplierCode.Text = String.Empty
        txtCompanyName.Text = String.Empty
        ddlAddressType.SelectedIndex = -1
        txtAddressNo.Text = String.Empty
        txtStreetName.Text = String.Empty
        txtFloorNo.Text = String.Empty
        txtUnitNo.Text = String.Empty
        txtBuildingName.Text = String.Empty
        txtPostalCode.Text = String.Empty
        txtContactPerson.Text = String.Empty
        txtTelNo.Text = String.Empty
        txtFaxNo.Text = String.Empty
        txtOtherInfo.Text = String.Empty

    End Sub

    ''' <summary>
    ''' btnAddSupplierOK - Click;
    ''' 18 Dec 08 - Jianfa Chen;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CODE LOGIC:
    ''' 1) Display Add Supplier panel
    ''' 2) Hide Status Message Box
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' 18Dec08  Jianfa      01     As indicated
    ''' </remarks>
    Private Sub btnAddSupplierOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddSupplierOK.Click

        divMsgBoxAddSupplier.Visible = False
        pnlAddSupplier.Visible = True

    End Sub
#End Region

#Region " Search Tab "
    ''' <summary>
    ''' btnLocateGO - Click;
    ''' 22 Dec 08 - Jianfa Chen;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CODE LOGIC:
    ''' 1) To populate Supplier search results based on filtering parameters into a gridview
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Private Sub btnLocateGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLocateGo.Click
        Try

            Dim Client As New ServiceClient
            Dim SupplierSearch As New SupplierDetails

            gdvLocateSupplier.Visible = False

            SupplierSearch.StoreId = Session("StoreID")
            SupplierSearch.SupplierId = txtLocateSupplier.Text.Trim
            SupplierSearch.CompanyName = txtLocateCompanyName.Text.Trim
            SupplierSearch.Status = ddlLocateStatus.SelectedValue

            Cache(Session(ESession.StoreID.ToString) & "SupplierList") = Client.GetSuppliers(SupplierSearch, String.Empty, String.Empty)
            Client.Close()

            gdvLocateSupplier.DataSource = Cache(Session(ESession.StoreID.ToString) & "SupplierList")
            gdvLocateSupplier.DataBind()

            pnlSearchResults.Visible = True
            gdvLocateSupplier.Visible = True

        Catch ex As FaultException

            lblErrLocateSupplier.Text = ex.Message
            lblErrLocateSupplier.Visible = True

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub

    ''' <summary>
    ''' btnLocateClear - Click;
    ''' 26 Dec 2008 - Jianfa
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' To clear away search results
    ''' </remarks>
    Private Sub btnLocateClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLocateClear.Click

        txtLocateSupplier.Text = String.Empty
        txtLocateCompanyName.Text = String.Empty
        ddlLocateStatus.SelectedValue = "O"
        pnlSearchResults.Visible = False
        pnlSupplierInfo.Visible = False

    End Sub

    ''' <summary>
    ''' gdvLocateSupplier - PageIndexChanging;
    ''' 26Dec08 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CODE LOGIC:
    ''' 1) To populate data according to selected page no
    ''' </remarks>
    Private Sub gdvLocateSupplier_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gdvLocateSupplier.PageIndexChanging

        gdvLocateSupplier.PageIndex = e.NewPageIndex
        gdvLocateSupplier.DataSource = Cache(Session(ESession.StoreID.ToString) & "SupplierList")
        gdvLocateSupplier.DataBind()

    End Sub

    ''' <summary>
    ''' gdvLocateSupplier - Sortin;
    ''' 31 Dec 2008 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gdvLocateSupplier_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gdvLocateSupplier.Sorting

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
            Dim SupplierSearch As New SupplierDetails

            gdvLocateSupplier.Visible = False

            SupplierSearch.StoreId = Session("StoreID")
            SupplierSearch.SupplierId = txtLocateSupplier.Text.Trim
            SupplierSearch.CompanyName = txtLocateCompanyName.Text.Trim
            SupplierSearch.Status = ddlLocateStatus.SelectedValue

            Cache(Session(ESession.StoreID.ToString) & "SupplierList") = Client.GetSuppliers(SupplierSearch, e.SortExpression, ViewState("_SortDirection"))
            Client.Close()

            gdvLocateSupplier.DataSource = Cache(Session(ESession.StoreID.ToString) & "SupplierList")
            gdvLocateSupplier.DataBind()

            pnlSearchResults.Visible = True
            gdvLocateSupplier.Visible = True

        Catch ex As FaultException

            lblErrLocateSupplier.Text = ex.Message
            lblErrLocateSupplier.Visible = True

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

    ''' <summary>
    ''' gdvLocateSupplier - SelectedIndexChanged;
    ''' 22 Dec 08 - Jianfa Chen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CODE LOGIC:
    ''' 1) To disable UI controls in Non-Editable Mode
    ''' 2) To display results in Supplier Panel
    ''' 3) To hide/unhide Close/Reopen Supplier
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Private Sub gdvLocateSupplier_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvLocateSupplier.SelectedIndexChanged

        lblEditSupplierCode.Text = gdvLocateSupplier.SelectedRow.Cells(1).Text
        txtEditCompanyName.Text = gdvLocateSupplier.SelectedRow.Cells(2).Text
        ddlEditAddressType.SelectedValue = CType(gdvLocateSupplier.SelectedRow.FindControl("hidAddressType"), HiddenField).Value
        txtEditAddressNo.Text = CType(gdvLocateSupplier.SelectedRow.FindControl("hidAddressBlockHouseNo"), HiddenField).Value
        txtEditStreetName.Text = CType(gdvLocateSupplier.SelectedRow.FindControl("hidAddressStreetName"), HiddenField).Value
        txtEditFloorNo.Text = CType(gdvLocateSupplier.SelectedRow.FindControl("hidAddressFloorNo"), HiddenField).Value
        txtEditUnitNo.Text = CType(gdvLocateSupplier.SelectedRow.FindControl("hidAddressUnitNo"), HiddenField).Value
        txtEditBuildingName.Text = CType(gdvLocateSupplier.SelectedRow.FindControl("hidAddressBuildingName"), HiddenField).Value
        txtEditPostalCode.Text = CType(gdvLocateSupplier.SelectedRow.FindControl("hidAddressPostalCode"), HiddenField).Value
        txtEditContactPerson.Text = CType(gdvLocateSupplier.SelectedRow.FindControl("hidContactPerson"), HiddenField).Value
        txtEditTelNo.Text = CType(gdvLocateSupplier.SelectedRow.FindControl("hidTelephoneNo"), HiddenField).Value
        txtEditFaxNo.Text = CType(gdvLocateSupplier.SelectedRow.FindControl("hidFaxNo"), HiddenField).Value
        txtEditOtherInfo.Text = CType(gdvLocateSupplier.SelectedRow.FindControl("hidOtherInfo"), HiddenField).Value
        lblDisplayStatus.Text = CType(gdvLocateSupplier.SelectedRow.FindControl("lblStatus"), Label).Text

        If ViewState("_EDIT") Then
            Select Case CType(gdvLocateSupplier.SelectedRow.FindControl("hidStatus"), HiddenField).Value.Trim
                Case "O"
                    lbtnLocateClose.Visible = True
                    lbtnLocateReopen.Visible = False
                Case "C"
                    lbtnLocateClose.Visible = False
                    lbtnLocateReopen.Visible = True
                Case Else
                    lbtnLocateClose.Visible = False
                    lbtnLocateReopen.Visible = False
            End Select
        Else
            lbtnLocateClose.Visible = False
            lbtnLocateReopen.Visible = False
        End If

        EditMode(False)
        pnlSupplierInfo.Visible = True
        lblErrSaveSupplier.Visible = False
        lblErrLocateSupplier.Visible = False

    End Sub

    ''' <summary>
    ''' gdvLocateSupplier - RowDataBound;
    ''' 22 Dec 08 - Jianfa
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CODE LOGIC:
    ''' 1) To manipulate code description based on code ID from gridview
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' 29Dec2008  Jianfa      1      To hard-code code description
    ''' </remarks>
    Private Sub gdvLocateSupplier_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvLocateSupplier.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            Select Case CType(e.Row.FindControl("hidStatus"), HiddenField).Value.ToUpper
                Case "O"
                    CType(e.Row.FindControl("lblStatus"), Label).Text = "Open"
                Case "C"
                    CType(e.Row.FindControl("lblStatus"), Label).Text = "Closed"
                Case "D"
                    CType(e.Row.FindControl("lblStatus"), Label).Text = "Deleted"
            End Select
        End If

    End Sub

    ''' <summary>
    ''' lbtnLocateEdit - Click;
    ''' 23 Dec 08 - Jianfa
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CODE LOGIC:
    ''' 1) To enable UI controls to Edit Mode
    ''' 2) To enable lbtnLocateEdit button
    ''' </remarks>
    Private Sub lbtnLocateEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnLocateEdit.Click

        EditMode(True)

    End Sub

    ''' <summary>
    ''' btnLocateSave -  Click;
    ''' 19 Dec 08 - Jianfa Chen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Private Sub btnLocateSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLocateSave.Click

        Try
            ' Check mandatory fields
            If txtEditCompanyName.Text.Trim = String.Empty Or _
                ddlEditAddressType.SelectedIndex <= 0 Or _
                txtEditAddressNo.Text.Trim = String.Empty Or _
                txtEditStreetName.Text.Trim = String.Empty Or _
                txtEditPostalCode.Text.Trim = String.Empty Or _
                txtEditContactPerson.Text.Trim = String.Empty Or _
                txtEditTelNo.Text.Trim = String.Empty Then

                lblErrSaveSupplier.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.MandatoryField)
                lblErrSaveSupplier.Visible = True

                Dim Script As String = "ShowAlertMessage('" & lblErrSaveSupplier.Text & "');"
                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

            Else

                Dim Client As New ServiceClient
                Dim SupplierDetails As New SupplierDetails

                SupplierDetails.StoreId = Session("StoreID").ToString
                SupplierDetails.SupplierId = lblEditSupplierCode.Text
                SupplierDetails.CompanyName = txtEditCompanyName.Text
                SupplierDetails.AddressType = ddlEditAddressType.SelectedValue
                SupplierDetails.AddressBlockHouseNo = txtEditAddressNo.Text
                SupplierDetails.AddressStreetName = txtEditStreetName.Text
                SupplierDetails.AddressFloorNo = txtEditFloorNo.Text
                SupplierDetails.AddressUnitNo = txtEditUnitNo.Text
                SupplierDetails.AddressBuildingName = txtEditBuildingName.Text
                SupplierDetails.AddressPostalCode = txtEditPostalCode.Text
                SupplierDetails.ContactPerson = txtEditContactPerson.Text
                SupplierDetails.TelephoneNo = txtEditTelNo.Text
                SupplierDetails.FaxNo = txtEditFaxNo.Text
                SupplierDetails.OtherInfo = txtEditOtherInfo.Text
                SupplierDetails.OriginalCompanyName = gdvLocateSupplier.SelectedRow.Cells(2).Text
                SupplierDetails.LoginUser = Session("UserID").ToString

                lblErrSaveSupplier.Text = Client.UpdateSupplier(SupplierDetails)
                Client.Close()

                If lblErrSaveSupplier.Text = String.Empty Then

                    Dim Script As String = "ShowSuccessMessage('" & clsCommonFunction.GetMessage( _
                                            clsCommonFunction.messageID.Success, "saved", "Supplier") & "');"

                    ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)

                    '-- UPDATE CACHE
                    If Not Cache(Session(ESession.StoreID.ToString) & ECache.SupplierList) Is Nothing Then
                        clsCommonFunction.EditCache(Cache(Session(ESession.StoreID.ToString) & ECache.SupplierList), GetType(SupplierDetails), SupplierDetails, True)
                    End If

                    btnLocateGo_Click(sender, e)

                Else

                    Dim Script As String = "ShowAlertMessage('" & lblErrSaveSupplier.Text & "');"
                    ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

                    lblErrSaveSupplier.Visible = True

                End If

            End If

        Catch ex As FaultException
            Dim fault As ServiceFault = ex.Data

            lblErrSaveSupplier.Text = fault.MessageText
            lblErrSaveSupplier.Visible = True

        Catch ex As Exception
            lblErrSaveSupplier.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            lblErrSaveSupplier.Visible = True
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub

    ''' <summary>
    ''' btnLocateCancel - Click;
    ''' 26 Dec 2008 - Jianfa
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' Toggle visiblity to hidden upon canceling supplier info
    ''' </remarks>
    Private Sub btnLocateCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLocateCancel.Click

        pnlSupplierInfo.Visible = False
        EditMode(False)

    End Sub

    ''' <summary>
    ''' lbtnLocateDel - Click;
    ''' 23 Dec 08 - Jianfa
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CODE LOGIC:
    ''' 1) To do a logical update to the supplier status (Delete)
    ''' </remarks>
    Private Sub lbtnLocateDel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnLocateDel.Click

        Try

            Dim Client As New ServiceClient
            Dim SupplierDetails As New SupplierDetails

            SupplierDetails.StoreId = Session("StoreID").ToString
            SupplierDetails.SupplierId = lblEditSupplierCode.Text
            SupplierDetails.Status = "D"
            SupplierDetails.LoginUser = Session("UserID").ToString

            lblErrSaveSupplier.Text = Client.UpdateSupplierStatus(SupplierDetails)
            Client.Close()

            If lblErrSaveSupplier.Text = String.Empty Then

                Dim Script As String = "ShowSuccessMessage('" & clsCommonFunction.GetMessage( _
                                        clsCommonFunction.messageID.Success, "deleted", "Supplier") & "');"

                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)

                btnLocateGo_Click(sender, e)
                gdvLocateSupplier_SelectedIndexChanged(sender, e)

            Else

                Dim Script As String = "ShowAlertMessage('" & lblErrLocateSupplier.Text & "');"
                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

                lblErrLocateSupplier.Visible = True

            End If

        Catch ex As FaultException
            Dim fault As ServiceFault = ex.Data

            lblErrSaveSupplier.Text = fault.MessageText
            lblErrSaveSupplier.Visible = True

        Catch ex As Exception
            lblErrSaveSupplier.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            lblErrSaveSupplier.Visible = True
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If

        End Try

    End Sub

    ''' <summary>
    ''' lbtnLocateClose - Click;
    ''' 23 Dec 08 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CODE LOGIC:
    '''  1) To do a logical update to the supplier status (Delete)
    ''' </remarks>
    Private Sub lbtnLocateClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnLocateClose.Click

        Try

            '----------------------------------------------------------------------------------
            '-- TO FIND IF THERE ARE ANY PENDING RECEIVE OPEN ORDERS
            '----------------------------------------------------------------------------------
            If CacheListIsEmpty(Cache(Session(ESession.StoreID.ToString) & ECache.OrderList), GetType(OrderDetails)) Then
                GetOrderList(Cache(Session(ESession.StoreID.ToString) & ECache.OrderList), Session("StoreID").ToString, ALL)
            End If

            If Not CacheListIsEmpty(Cache(Session(ESession.StoreID.ToString) & ECache.OrderList), GetType(OrderDetails)) Then

                If DirectCast(Cache(Session(ESession.StoreID.ToString) & ECache.OrderList), List(Of OrderDetails)).Exists _
                   (Function(i As OrderDetails) i.Status <> CLOSED And i.SupplierID = lblEditSupplierCode.Text) Then

                    lblErrSaveSupplier.Text = "Unable to close Supplier Code " & lblEditSupplierCode.Text & ". \nThere are pending receive orders found in this Supplier. "

                    Dim Script As String = "ShowAlertMessage('" & lblErrSaveSupplier.Text & "');"
                    ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

                    Exit Sub

                End If

            End If
            '----------------------------------------------------------------------------------

            Dim Client As New ServiceClient
            Dim SupplierDetails As New SupplierDetails

            SupplierDetails.StoreId = Session("StoreID").ToString
            SupplierDetails.SupplierId = lblEditSupplierCode.Text
            SupplierDetails.Status = CLOSED
            SupplierDetails.LoginUser = Session("UserID").ToString

            lblErrSaveSupplier.Text = Client.UpdateSupplierStatus(SupplierDetails)
            Client.Close()

            If lblErrSaveSupplier.Text = String.Empty Then

                Dim Script As String = "ShowSuccessMessage('" & clsCommonFunction.GetMessage( _
                                        clsCommonFunction.messageID.Success, "closed", "Supplier") & "');"

                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)

                btnLocateGo_Click(sender, e)
                gdvLocateSupplier_SelectedIndexChanged(sender, e)

            Else

                Dim Script As String = "ShowAlertMessage('" & lblErrLocateSupplier.Text & "');"
                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

                lblErrLocateSupplier.Visible = True
            End If

        Catch ex As FaultException
            Dim fault As ServiceFault = ex.Data

            lblErrSaveSupplier.Text = fault.MessageText
            lblErrSaveSupplier.Visible = True

        Catch ex As Exception
            lblErrSaveSupplier.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            lblErrSaveSupplier.Visible = True
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If

        End Try

    End Sub

    ''' <summary>
    ''' lbtnLocateReopen - Click;
    ''' 23 Dec 08 - Jianfa
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CODE LOGIC:
    ''' 1) To do a logical update to Supplier Status (Open)
    ''' </remarks>
    Private Sub lbtnLocateReopen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnLocateReopen.Click

        Try

            Dim Client As New ServiceClient
            Dim SupplierDetails As New SupplierDetails

            SupplierDetails.StoreId = Session("StoreID").ToString
            SupplierDetails.SupplierId = lblEditSupplierCode.Text
            SupplierDetails.Status = "O"
            SupplierDetails.LoginUser = Session("UserID").ToString

            lblErrSaveSupplier.Text = Client.UpdateSupplierStatus(SupplierDetails)
            Client.Close()

            If lblErrSaveSupplier.Text = String.Empty Then

                Dim Script As String = "ShowSuccessMessage('" & clsCommonFunction.GetMessage( _
                                        clsCommonFunction.messageID.Success, "opened", "Supplier") & "');"

                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)

                btnLocateGo_Click(sender, e)
                gdvLocateSupplier_SelectedIndexChanged(sender, e)

            Else

                Dim Script As String = "ShowAlertMessage('" & lblErrLocateSupplier.Text & "');"
                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

                lblErrLocateSupplier.Visible = True
            End If

        Catch ex As FaultException
            Dim fault As ServiceFault = ex.Data

            lblErrSaveSupplier.Text = fault.MessageText
            lblErrSaveSupplier.Visible = True

        Catch ex As Exception
            lblErrSaveSupplier.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            lblErrSaveSupplier.Visible = True
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If

        End Try

    End Sub

#End Region

#Region " Print Tab "
    ''' <summary>
    ''' Export List to PDF formatted file;
    ''' Liu Guo Feng, 26Dec08;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' 27Dec08  Jianfa  RefID  include code to export to PDF;
    ''' </remarks>
    Private Sub btnPDF_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPDF.Click
        rvr.LocalReport.DataSources.Clear()
        rvr.LocalReport.DataSources.Add(New ReportDataSource("SupplierDetails", ObjectDataSource1))

        Dim parameterlist As New List(Of ReportParameter)
        Dim p1 As New ReportParameter("Report_Name", "Supplier Master List Report")
        Dim p2 As New ReportParameter("Report_Title", Session("StoreName").ToString())
        parameterlist.Add(p1)
        parameterlist.Add(p2)

        rvr.LocalReport.SetParameters(parameterlist)
        rvr.LocalReport.Refresh()

        Dim bytValue As Byte()
        bytValue = rvr.LocalReport.Render("PDF", Nothing, "application/pdf", "", "pdf", Nothing, Nothing)
        If NoRecordFond = "Y" Then
            Message = GetMessage(messageID.NoRecordFound)
            Exit Sub
        End If

        Response.Buffer = True
        Response.Clear()
        Response.ContentType = "application/pdf"
        Response.AddHeader("content-disposition", "attachment;filename=SupplierMasterList.pdf")
        Response.BinaryWrite(bytValue)
        Response.Flush()
        Response.End()

    End Sub

    ''' <summary>
    ''' Export List to Excel formatted file;
    ''' Jianfa, 27Dec08;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Private Sub btnExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcel.Click
        rvr.LocalReport.DataSources.Clear()
        rvr.LocalReport.DataSources.Add(New ReportDataSource("SupplierDetails", ObjectDataSource1))

        Dim parameterlist As New List(Of ReportParameter)
        Dim p1 As New ReportParameter("Report_Name", "Supplier Master List Report")
        Dim p2 As New ReportParameter("Report_Title", Session("StoreName").ToString())
        parameterlist.Add(p1)
        parameterlist.Add(p2)

        rvr.LocalReport.SetParameters(parameterlist)
        rvr.LocalReport.Refresh()

        Dim bytValue As Byte()
        bytValue = rvr.LocalReport.Render("Excel", Nothing, "application/ms-excel", "", "xls", Nothing, Nothing)
        If NoRecordFond = "Y" Then
            Message = GetMessage(messageID.NoRecordFound)
            Exit Sub
        End If

        Response.Buffer = True
        Response.Clear()
        Response.ContentType = "application/ms-excel"
        Response.AddHeader("content-disposition", "attachment;filename=SupplierMasterList.xls")
        Response.BinaryWrite(bytValue)
        Response.Flush()
        Response.End()

    End Sub
#End Region

#Region " Sub Procedures and Functions "
    ''' <summary>
    ''' Sub Procedure - DisableEditMode;
    ''' 23 Dec 08 - To enable/disable UI controls for Edit
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    Protected Sub EditMode(ByVal Edit As Boolean)

        Select Case Edit

            Case False
                ddlEditAddressType.Enabled = False
                txtEditAddressNo.Enabled = False
                txtEditBuildingName.Enabled = False
                txtEditCompanyName.Enabled = False
                txtEditContactPerson.Enabled = False
                txtEditFaxNo.Enabled = False
                txtEditFloorNo.Enabled = False
                txtEditUnitNo.Enabled = False
                txtEditOtherInfo.Enabled = False
                txtEditPostalCode.Enabled = False
                txtEditStreetName.Enabled = False
                txtEditTelNo.Enabled = False
                btnLocateSave.Enabled = False
            Case True
                ddlEditAddressType.Enabled = True
                txtEditAddressNo.Enabled = True
                txtEditBuildingName.Enabled = True
                txtEditCompanyName.Enabled = True
                txtEditContactPerson.Enabled = True
                txtEditFaxNo.Enabled = True
                txtEditFloorNo.Enabled = True
                txtEditUnitNo.Enabled = True
                txtEditOtherInfo.Enabled = True
                txtEditPostalCode.Enabled = True
                txtEditStreetName.Enabled = True
                txtEditTelNo.Enabled = True
                btnLocateSave.Enabled = True
        End Select

    End Sub
#End Region

    Protected Sub ObjectDataSource1_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles ObjectDataSource1.Selected
        Dim returnList As List(Of SupplierDetails) = e.ReturnValue
        If returnList.Count <= 0 Then
            NoRecordFond = "Y"
        Else
            NoRecordFond = "N"
        End If
    End Sub

    Protected Sub ObjectDataSource1_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs) Handles ObjectDataSource1.Selecting
        e.InputParameters("storeId") = Session("StoreID")
        e.InputParameters("supplierId") = ""
        e.InputParameters("companyName") = ""
        e.InputParameters("status") = ""
    End Sub

End Class
