Imports Microsoft.Reporting.WebForms
Imports System.Web.Services.Description
Imports NEA_ICS.UserInterface.NEA_ICS.WcfService
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ServiceModel
Imports System.Reflection

Partial Public Class frmStore
    Inherits clsCommonFunction

    'Private Shared StoreList As List(Of StoreDetails)
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim AccessRights As New List(Of RoleDetails)

        CheckValidSession()

        If Not Page.IsPostBack Then

            AccessRights = clsCommonFunction.AssignAccessRights(Cache(Session(ESession.UserID.ToString) & Session(ESession.StoreID.ToString) & "AccessRights"), _
                                                   clsCommonFunction.moduleID.Store)

            If AccessRights(0).SelectRight = False Then
                Server.Transfer("..\frmUnauthorisedPage.aspx")
                Exit Sub
            End If

            If AccessRights(0).InsertRight = False Then
                tbpNewStore.Visible = False
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

            tbcStore.Visible = True

            BindStore()
        End If
    End Sub
#End Region

#Region " New Tab "

    ''' <summary>
    ''' btnAddStore - Click;
    ''' 31 Dec 2008 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CODE LOGIC:
    ''' 1) Validate Mandatory Fields;
    ''' 2) Validate unique Supplier Code;
    ''' 3) Insert Supplier Details;
    ''' 4) Display Inserted Supplier Details;
    ''' </remarks>
    Private Sub btnAddStore_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddStore.Click

        Try

            ' Check mandatory fields
            If txtStoreCode.Text.Trim = String.Empty _
                Or txtStoreName.Text.Trim = String.Empty _
                Or ddlAddressType.SelectedValue.Trim = String.Empty _
                Or txtAddressBlockHouseNo.Text.Trim = String.Empty _
                Or txtStreetName.Text.Trim = String.Empty _
                Or txtPostalCode.Text.Trim = String.Empty _
                Or txtContactPerson.Text.Trim = String.Empty _
                Or txtTelNo.Text.Trim = String.Empty _
            Then

                lblErrAddStore.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.MandatoryField)
                lblErrAddStore.Visible = True

                Dim Script As String = "ShowAlertMessage('" & lblErrAddStore.Text & "');"
                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

            Else

                Dim Client As New ServiceClient
                Dim StoreDetails As New StoreDetails

                StoreDetails.StoreId = txtStoreCode.Text.Trim.ToUpper
                StoreDetails.StoreName = txtStoreName.Text.Trim
                StoreDetails.AddressType = ddlAddressType.SelectedValue
                StoreDetails.AddressBlockHouseNo = txtAddressBlockHouseNo.Text
                StoreDetails.AddressStreetName = txtStreetName.Text
                StoreDetails.AddressBuildingName = txtBuildingName.Text
                StoreDetails.AddressFloorNo = txtFloorNo.Text
                StoreDetails.AddressUnitNo = txtUnitNo.Text
                StoreDetails.AddressPostalCode = txtPostalCode.Text
                StoreDetails.ContactPerson = txtContactPerson.Text
                StoreDetails.TelephoneNo = txtTelNo.Text
                StoreDetails.FaxNo = txtFaxNo.Text
                StoreDetails.OtherInfo = txtOtherInfo.Text
                StoreDetails.LoginUser = Session("UserID").ToString
                StoreDetails.Status = OPEN
                StoreDetails.UserRoles = clsCommonFunction.GetCodeID(Cache(Session(ESession.StoreID.ToString) & "dtCommon"), Session("StoreID").ToString, clsCommonFunction.codeGroup.UserRole, "O")

                lblErrAddStore.Text = Client.AddStore(StoreDetails)
                Client.Close()

                If lblErrAddStore.Text = String.Empty Then

                    lblMsgStoreCode.Text = txtStoreCode.Text.Trim.ToUpper
                    lblMsgStoreName.Text = txtStoreName.Text.Trim
                    lblMsgAddressType.Text = ddlAddressType.SelectedItem.Text
                    lblMsgAddressNo.Text = txtAddressBlockHouseNo.Text
                    lblMsgStreetName.Text = txtStreetName.Text
                    lblMsgBuildingName.Text = txtBuildingName.Text
                    lblMsgFloorNo.Text = txtFloorNo.Text
                    lblMsgUnitNo.Text = txtUnitNo.Text
                    lblMsgPostalCode.Text = txtPostalCode.Text
                    lblMsgContactPerson.Text = txtContactPerson.Text
                    lblMsgTelephoneNo.Text = txtTelNo.Text
                    lblMsgFaxNo.Text = txtFaxNo.Text
                    lblMsgOtherInfo.Text = txtOtherInfo.Text

                    lblErrAddStore.Visible = False
                    pnlAddStore.Visible = False
                    divMsgBoxAddStore.Visible = True

                    BindStore()

                Else

                    lblErrAddStore.Visible = True

                    Dim Script As String = "ShowAlertMessage('" & lblErrAddStore.Text & "');"
                    ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

                End If

            End If

        Catch ex As FaultException

            Dim fault As ServiceFault = ex.Data

            lblErrAddStore.Text = fault.MessageText
            lblErrAddStore.Visible = True

        Catch ex As Exception
            lblErrAddStore.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            lblErrAddStore.Visible = True
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub

    ''' <summary>
    ''' btnAddStoreOk - Click;
    ''' 01 Jan 2009 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAddStoreOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddStoreOK.Click

        pnlAddStore.Visible = True
        divMsgBoxAddStore.Visible = False
        lblErrAddStore.Visible = False

    End Sub

    ''' <summary>
    ''' btnClear - Click;
    ''' 01 Jan 2009 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 1) To clear away UI controls for adding stores
    ''' </remarks>
    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click

        txtStoreCode.Text = String.Empty
        txtStoreName.Text = String.Empty
        ddlAddressType.SelectedIndex = -1
        txtAddressBlockHouseNo.Text = String.Empty
        txtStreetName.Text = String.Empty
        txtBuildingName.Text = String.Empty
        txtFloorNo.Text = String.Empty
        txtUnitNo.Text = String.Empty
        txtPostalCode.Text = String.Empty
        txtContactPerson.Text = String.Empty
        txtTelNo.Text = String.Empty
        txtFaxNo.Text = String.Empty
        txtOtherInfo.Text = String.Empty

        pnlSearchResults.Visible = False
        pnlAddStore.Visible = False
        pnlStoreInfo.Visible = False

    End Sub

#End Region

#Region " Search Tab "
    ''' <summary>
    ''' btnLocateGo - Click;
    ''' 01 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnLocateGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLocateGo.Click

        Try

            Dim Client As New ServiceClient
            Dim StoreSearch As New StoreDetails

            gdvLocateStore.Visible = False

            StoreSearch.StoreId = ddlLocateStore.SelectedValue
            StoreSearch.StoreName = txtLocateStoreName.Text
            StoreSearch.Status = ddlLocateStatus.SelectedValue

            Cache(Session(ESession.StoreID.ToString) & "StoreList") = Client.GetStores(StoreSearch, String.Empty, String.Empty)
            Client.Close()

            gdvLocateStore.DataSource = Cache(Session(ESession.StoreID.ToString) & "StoreList")
            gdvLocateStore.DataBind()

            pnlSearchResults.Visible = True
            gdvLocateStore.Visible = True


        Catch ex As FaultException

            lblErrLocateStore.Text = ex.Message
            lblErrLocateStore.Visible = True

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

    ''' <summary>
    ''' gdvLocateStore - Page Index Changed;
    ''' 02 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gdvLocateStore_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gdvLocateStore.PageIndexChanging

        gdvLocateStore.PageIndex = e.NewPageIndex
        gdvLocateStore.DataSource = Cache(Session(ESession.StoreID.ToString) & "StoreList")
        gdvLocateStore.DataBind()

    End Sub

    ''' <summary>
    ''' gdvLocateStore - RowDataBound;
    ''' 01 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gdvLocateStore_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvLocateStore.RowDataBound

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
    ''' gdvLocateStore - SelectedIndexChanged;
    ''' 01 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gdvLocateStore_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvLocateStore.SelectedIndexChanged

        lblEditStoreCode.Text = gdvLocateStore.SelectedRow.Cells(1).Text
        txtEditStoreName.Text = gdvLocateStore.SelectedRow.Cells(2).Text
        ddlEditAddressType.SelectedValue = CType(gdvLocateStore.SelectedRow.FindControl("hidAddressType"), HiddenField).Value
        txtEditBlockHouseNo.Text = CType(gdvLocateStore.SelectedRow.FindControl("hidAddressBlockHouseNo"), HiddenField).Value
        txtEditStreetName.Text = CType(gdvLocateStore.SelectedRow.FindControl("hidAddressStreetName"), HiddenField).Value
        txtEditFloorNo.Text = CType(gdvLocateStore.SelectedRow.FindControl("hidAddressFloorNo"), HiddenField).Value
        txtEditUnitNo.Text = CType(gdvLocateStore.SelectedRow.FindControl("hidAddressUnitNo"), HiddenField).Value
        txtEditBuildingName.Text = CType(gdvLocateStore.SelectedRow.FindControl("hidAddressBuildingName"), HiddenField).Value
        txtEditPostalCode.Text = CType(gdvLocateStore.SelectedRow.FindControl("hidAddressPostalCode"), HiddenField).Value
        txtEditContactPerson.Text = CType(gdvLocateStore.SelectedRow.FindControl("hidContactPerson"), HiddenField).Value
        txtEditTelephoneNo.Text = CType(gdvLocateStore.SelectedRow.FindControl("hidTelephoneNo"), HiddenField).Value
        txtEditFaxNo.Text = CType(gdvLocateStore.SelectedRow.FindControl("hidFaxNo"), HiddenField).Value
        txtEditOtherInfo.Text = CType(gdvLocateStore.SelectedRow.FindControl("hidOtherInfo"), HiddenField).Value
        lblDisplayStatus.Text = CType(gdvLocateStore.SelectedRow.FindControl("lblStatus"), Label).Text

        If ViewState("_EDIT") Then
            Select Case CType(gdvLocateStore.SelectedRow.FindControl("hidStatus"), HiddenField).Value.Trim
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
        pnlStoreInfo.Visible = True
        lblErrSaveStore.Visible = False
        lblErrLocateStore.Visible = False

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
    ''' btnLocateSave - Click;
    ''' 01 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' 1) To validate Store Fields
    ''' 2) To update Store
    ''' </remarks>
    Private Sub btnLocateSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLocateSave.Click

        Try
            ' Check mandatory fields
            If txtEditStoreName.Text.Trim = String.Empty Or _
                ddlEditAddressType.SelectedIndex <= 0 Or _
                txtEditBlockHouseNo.Text.Trim = String.Empty Or _
                txtEditStreetName.Text.Trim = String.Empty Or _
                txtEditPostalCode.Text.Trim = String.Empty Or _
                txtEditContactPerson.Text.Trim = String.Empty Or _
                txtEditTelephoneNo.Text.Trim = String.Empty Then

                lblErrSaveStore.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.MandatoryField)
                lblErrSaveStore.Visible = True

                Dim Script As String = "ShowAlertMessage('" & lblErrSaveStore.Text & "');"
                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

            Else

                Dim Client As New ServiceClient
                Dim StoreDetails As New StoreDetails

                StoreDetails.StoreId = lblEditStoreCode.Text
                StoreDetails.StoreName = txtEditStoreName.Text
                StoreDetails.AddressType = ddlEditAddressType.SelectedValue
                StoreDetails.AddressBlockHouseNo = txtEditBlockHouseNo.Text
                StoreDetails.AddressStreetName = txtEditStreetName.Text
                StoreDetails.AddressFloorNo = txtEditFloorNo.Text
                StoreDetails.AddressUnitNo = txtEditUnitNo.Text
                StoreDetails.AddressBuildingName = txtEditBuildingName.Text
                StoreDetails.AddressPostalCode = txtEditPostalCode.Text
                StoreDetails.ContactPerson = txtEditContactPerson.Text
                StoreDetails.TelephoneNo = txtEditTelephoneNo.Text
                StoreDetails.FaxNo = txtEditFaxNo.Text
                StoreDetails.OtherInfo = txtEditOtherInfo.Text
                StoreDetails.OriginalStoreName = gdvLocateStore.SelectedRow.Cells(2).Text
                StoreDetails.LoginUser = Session("UserID").ToString

                lblErrSaveStore.Text = Client.UpdateStore(StoreDetails)
                Client.Close()

                If lblErrSaveStore.Text = String.Empty Then

                    Dim Script As String = "ShowSuccessMessage('" & clsCommonFunction.GetMessage( _
                                            clsCommonFunction.messageID.Success, "saved", "Store") & "');"

                    ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)

                    btnLocateGo_Click(sender, e)
                    gdvLocateStore_SelectedIndexChanged(sender, e)

                Else

                    Dim Script As String = "ShowAlertMessage('" & lblErrSaveStore.Text & "');"
                    ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

                    lblErrSaveStore.Text = Replace(lblErrSaveStore.Text, "\n", String.Empty)
                    lblErrSaveStore.Visible = True

                End If

            End If

        Catch ex As FaultException
            Dim fault As ServiceFault = ex.Data

            lblErrSaveStore.Text = fault.MessageText
            lblErrSaveStore.Visible = True

        Catch ex As Exception
            lblErrSaveStore.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            lblErrSaveStore.Visible = True
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

    ''' <summary>
    ''' btnLocateClear - Click;
    ''' 02 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnLocateClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLocateClear.Click

        ddlLocateStore.SelectedIndex = -1
        ddlLocateStatus.SelectedIndex = -1
        txtLocateStoreName.Text = String.Empty

        pnlSearchResults.Visible = False
        pnlStoreInfo.Visible = False

    End Sub

    ''' <summary>
    ''' btnLocateCancel - Click;
    ''' 02 Jan 09 - Jianfa
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' Toggle visiblity to hidden upon canceling supplier info
    ''' </remarks>
    Private Sub btnLocateCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLocateCancel.Click

        pnlStoreInfo.Visible = False
        EditMode(False)

    End Sub

    ''' <summary>
    ''' lbtnLocateClose - Click;
    ''' 02 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' CODE LOGIC:
    '''  1) To do a logical update to the store status (Close)
    ''' </remarks>
    Private Sub lbtnLocateClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnLocateClose.Click

        Try

            Dim Client As New ServiceClient
            Dim StoreDetails As New StoreDetails

            StoreDetails.StoreId = lblEditStoreCode.Text
            StoreDetails.Status = CLOSED
            StoreDetails.LoginUser = Session("UserID").ToString

            lblErrSaveStore.Text = Client.UpdateStoreStatus(StoreDetails)
            Client.Close()

            If lblErrSaveStore.Text = String.Empty Then

                Dim Script As String = "ShowSuccessMessage('" & clsCommonFunction.GetMessage( _
                                        clsCommonFunction.messageID.Success, "closed", "Store") & "');"

                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)

                btnLocateGo_Click(sender, e)
                gdvLocateStore_SelectedIndexChanged(sender, e)

            Else

                Dim Script As String = "ShowAlertMessage('" & lblErrSaveStore.Text & "');"
                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "AlertRegister", Script, True)

                lblErrSaveStore.Text = Replace(lblErrSaveStore.Text, "\n", String.Empty)
                lblErrSaveStore.Visible = True

            End If

        Catch ex As FaultException
            Dim fault As ServiceFault = ex.Data

            lblErrSaveStore.Text = fault.MessageText
            lblErrSaveStore.Visible = True

        Catch ex As Exception
            lblErrSaveStore.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            lblErrSaveStore.Visible = True
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try


    End Sub

    ''' <summary>
    ''' lbtnLocateDel - Click;
    ''' 02 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' </remarks>
    Private Sub lbtnLocateDel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnLocateDel.Click

        Try

            Dim Client As New ServiceClient
            Dim StoreDetails As New StoreDetails

            StoreDetails.StoreId = lblEditStoreCode.Text
            StoreDetails.Status = "D"
            StoreDetails.LoginUser = Session("UserID").ToString

            lblErrSaveStore.Text = Client.UpdateStoreStatus(StoreDetails)
            Client.Close()

            If lblErrSaveStore.Text = String.Empty Then

                Dim Script As String = "ShowSuccessMessage('" & clsCommonFunction.GetMessage( _
                                        clsCommonFunction.messageID.Success, "deleted", "Store") & "');"

                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)

                btnLocateGo_Click(sender, e)
                gdvLocateStore_SelectedIndexChanged(sender, e)

            Else
                lblErrLocateStore.Visible = True
            End If

        Catch ex As FaultException
            Dim fault As ServiceFault = ex.Data

            lblErrSaveStore.Text = fault.MessageText
            lblErrSaveStore.Visible = True

        Catch ex As Exception
            lblErrSaveStore.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            lblErrSaveStore.Visible = True
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

    ''' <summary>
    ''' lbtnLocateReopen - Click;
    ''' 02 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub lbtnLocateReopen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnLocateReopen.Click

        Try

            Dim Client As New ServiceClient
            Dim StoreDetails As New StoreDetails

            StoreDetails.StoreId = lblEditStoreCode.Text
            StoreDetails.Status = "O"
            StoreDetails.LoginUser = Session("UserID").ToString

            lblErrSaveStore.Text = Client.UpdateStoreStatus(StoreDetails)
            Client.Close()

            If lblErrSaveStore.Text = String.Empty Then

                Dim Script As String = "ShowSuccessMessage('" & clsCommonFunction.GetMessage( _
                                        clsCommonFunction.messageID.Success, "opened", "Store") & "');"

                ScriptManager.RegisterStartupScript(Page, GetType(UpdatePanel), "SuccessRegister", Script, True)

                btnLocateGo_Click(sender, e)
                gdvLocateStore_SelectedIndexChanged(sender, e)

            Else
                lblErrLocateStore.Visible = True
            End If

        Catch ex As FaultException
            Dim fault As ServiceFault = ex.Data

            lblErrSaveStore.Text = fault.MessageText
            lblErrSaveStore.Visible = True

        Catch ex As Exception
            lblErrSaveStore.Text = clsCommonFunction.GetMessage(clsCommonFunction.messageID.TryLastOperation)
            lblErrSaveStore.Visible = True
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

    ''' <summary>
    ''' gdvLocateStore - Sorting;
    ''' 02 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gdvLocateStore_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gdvLocateStore.Sorting

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
            Dim StoreSearch As New StoreDetails

            gdvLocateStore.Visible = False

            StoreSearch.StoreId = ddlLocateStore.SelectedValue
            StoreSearch.StoreName = txtLocateStoreName.Text
            StoreSearch.Status = ddlLocateStatus.SelectedValue

            Cache(Session(ESession.StoreID.ToString) & "StoreList") = Client.GetStores(StoreSearch, e.SortExpression, ViewState("_SortDirection").ToString)
            Client.Close()

            gdvLocateStore.DataSource = Cache(Session(ESession.StoreID.ToString) & "StoreList")
            gdvLocateStore.DataBind()

            pnlSearchResults.Visible = True
            gdvLocateStore.Visible = True


        Catch ex As FaultException

            lblErrLocateStore.Text = ex.Message
            lblErrLocateStore.Visible = True

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try


    End Sub

#End Region

#Region " Print Tab "

#End Region

#Region " Sub Procedures and Functions "

    ''' <summary>
    ''' Sub Proc - BindStore()
    ''' 01 Jan 2009 - Jianfa
    ''' </summary>
    ''' <remarks>
    ''' 1) To Bind Stores into the dropdown list
    ''' </remarks>
    Protected Sub BindStore()

        Try

            Dim Client As New ServiceClient
            Dim StoreSearch As New StoreDetails

            StoreSearch.StoreId = ""
            StoreSearch.StoreName = ""
            StoreSearch.Status = ""

            Cache(Session(ESession.StoreID.ToString) & "StoreList") = Client.GetStores(StoreSearch, String.Empty, String.Empty)
            Client.Close()

            ddlLocateStore.DataSource = Cache(Session(ESession.StoreID.ToString) & "StoreList")
            ddlLocateStore.DataValueField = "StoreId"
            ddlLocateStore.DataTextField = "StoreDescription"
            ddlLocateStore.DataBind()

            ddlLocateStore.Items.Insert(0, New ListItem(" - Please Select - ", ""))


        Catch ex As FaultException

            lblErrLocateStore.Text = ex.Message
            lblErrLocateStore.Visible = True

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

    ''' <summary>
    ''' Sub Procedure - DisableEditMode;
    ''' 01 Jan 09 - To enable/disable UI controls for Edit
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    Protected Sub EditMode(ByVal Edit As Boolean)

        Select Case Edit

            Case False
                ddlEditAddressType.Enabled = False
                txtEditBlockHouseNo.Enabled = False
                txtEditBuildingName.Enabled = False
                txtEditStoreName.Enabled = False
                txtEditContactPerson.Enabled = False
                txtEditFaxNo.Enabled = False
                txtEditFloorNo.Enabled = False
                txtEditUnitNo.Enabled = False
                txtEditOtherInfo.Enabled = False
                txtEditPostalCode.Enabled = False
                txtEditStreetName.Enabled = False
                txtEditTelephoneNo.Enabled = False
                btnLocateSave.Enabled = False
            Case True
                ddlEditAddressType.Enabled = True
                txtEditBlockHouseNo.Enabled = True
                txtEditBuildingName.Enabled = True
                txtEditStoreName.Enabled = True
                txtEditContactPerson.Enabled = True
                txtEditFaxNo.Enabled = True
                txtEditFloorNo.Enabled = True
                txtEditUnitNo.Enabled = True
                txtEditOtherInfo.Enabled = True
                txtEditPostalCode.Enabled = True
                txtEditStreetName.Enabled = True
                txtEditTelephoneNo.Enabled = True
                btnLocateSave.Enabled = True
        End Select

    End Sub

#End Region
    Protected Sub ObjectDataSource1_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles ObjectDataSource1.Selected
        Dim returnList As List(Of StoreDetails) = e.ReturnValue
        If returnList.Count <= 0 Then
            NoRecordFond = "Y"
        Else
            NoRecordFond = "N"
        End If
    End Sub

    Protected Sub ObjectDataSource1_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs) Handles ObjectDataSource1.Selecting
        Dim searchStore As New StoreDetails()
        searchStore.StoreId = ""
        searchStore.StoreName = ""
        searchStore.Status = ""

        e.InputParameters("storeDetails") = searchStore
        e.InputParameters("sortExpression") = "StoreID"
        e.InputParameters("sortDirection") = ""
    End Sub

    Protected Sub btnPDF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPDF.Click
        rvr.LocalReport.DataSources.Clear()
        rvr.LocalReport.DataSources.Add(New ReportDataSource("StoreDetails", ObjectDataSource1))

        Dim parameterlist As New List(Of ReportParameter)
        Dim p1 As New ReportParameter("ReportTitle", "Store Master List Report")
        parameterlist.Add(p1)

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
        Response.AddHeader("content-disposition", "attachment;filename=StoreMasterList.pdf")
        Response.BinaryWrite(bytValue)
        Response.Flush()
        Response.End()
    End Sub

    Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExcel.Click
        rvr.LocalReport.DataSources.Clear()
        rvr.LocalReport.DataSources.Add(New ReportDataSource("StoreDetails", ObjectDataSource1))

        Dim parameterlist As New List(Of ReportParameter)
        Dim p1 As New ReportParameter("ReportTitle", "Store Master List Report")
        parameterlist.Add(p1)

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
        Response.AddHeader("content-disposition", "attachment;filename=StoreMasterList.xls")
        Response.BinaryWrite(bytValue)
        Response.Flush()
        Response.End()
    End Sub
End Class