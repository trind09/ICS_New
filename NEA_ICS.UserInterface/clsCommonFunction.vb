Imports System.Text.RegularExpressions
Imports NEA_ICS.UserInterface.NEA_ICS.WcfService
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

''' <summary>
''' BASE CLASS for Common Functions - clsCommonFunction
''' 23 Dec 2008 - Jianfa
''' </summary>
''' <remarks>
''' ddMMMyyyy  AuthorName  RefID  Description;
''' 23Dec2008  Jianfa      1.0    Baseline
''' </remarks>
Public Class clsCommonFunction
    Inherits System.Web.UI.Page

#Region " Constants "
    ' OperationType
    Public Shared ReadOnly UPDATE As String = "U"
    Protected Friend Shared ReadOnly DELETE As String = "D"
    Protected Friend Shared ReadOnly OPEN As String = "O"
    Protected Friend Shared ReadOnly APPROVED As String = "A"
    Protected Friend Shared ReadOnly PENDING As String = "P"
    Protected Friend Shared ReadOnly REJECTED As String = "R"
    Protected Friend Shared ReadOnly EMPTY As String = String.Empty
    Protected Shared ReadOnly DEFAULTED As String = "E"
    Protected Friend Shared ReadOnly INSERT As String = "I"
    Protected Friend Shared ReadOnly CLOSED As String = "C"
    Protected Shared ReadOnly CURRENT As String = "C"
    Protected Shared ReadOnly ALL As String = String.Empty
    Protected Shared ReadOnly STOREBROWSER As String = "SB"
    Protected Shared ReadOnly APPROVALOFFICER As String = "AO"
    Protected Friend Shared ReadOnly STOREOFFICER As String = "SO"

    ' Tab
    Protected ReadOnly NEWTAB As String = "tbpNew"
    Protected ReadOnly LOCATETAB As String = "tbpLocate"
    Protected ReadOnly PRINTTAB As String = "tbpPrint"

    ' Adjustment Type
    Protected Friend Shared ReadOnly ADJUSTIN As String = "AI"
    Protected Friend Shared ReadOnly ADJUSTOUT As String = "AO"
    Protected Friend Shared ReadOnly ADJUSTODL As String = "ODL" ' Obsolete,Damage & Loss adjust type
    Protected Friend Shared ReadOnly RETURNED As String = "AIRETURN"
    Protected Friend Shared ReadOnly DAMAGE As String = "AODAMAGE"
    'Protected Friend Shared ReadOnly LOSS As String = "AOLOSS"
    Protected Friend Shared ReadOnly OBSOLETE As String = "AOOBSOLETE"

    ' Add more constants
#End Region

#Region " MESSAGES AND EMUMERATORS "
    ''' <summary>
    ''' Enumerator - messageID;
    ''' 23 Dec 2008 - Jianfa
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum messageID As Integer
        MandatoryField = 1
        TryLastOperation = 2
        Success = 3
        Confirm = 4
        NumericFields = 5
        MoreLessThan = 6
        RangeFromTo = 7
        AtLeastOneItemSelected = 8
        NoRecordFound = 9
        DateToEarlierDateFrom = 10
        StockCodeToEarlierStockCodeFrom = 11
        InvalidStockCode = 12
        StockCodeClosed = 13
        MultipleLogin = 15

        Exception = 101
        ApplicationException = 102
        NotIsDate = 103
        NotInFinancial = 104
        ddlNotSelected = 105
        InvalidValue = 106
        StockCodeHasOrdered = 107
        StockCodeNotSelected = 108
        StockCodeNotAdded = 109
        DateHasReceived = 110
        NoChange = 111
        FieldNotUnique = 112
        OwnRequest = 113
        AtLeast1Item = 114

        'TODO: To add few more enumerators in future to define message ID
    End Enum

    ''' <summary>
    ''' Enumerator - codeType
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum codeGroup As Integer
        Supplier = 1
        UserRole = 2
        UOM = 3
        StockType = 4
        SubType = 5
        EquipmentType = 6
        OrderDocType = 7
        IssueDocType = 8
        DirectDocType = 9
        InwardsDocType = 10
        OutwardsDocType = 11
        ODLdocType = 13
        DeliveryLapseDay = 12

        'TODO: To add few more enumerators in future to define code group
    End Enum

    ''' <summary>
    ''' Enumerator - Tab;
    ''' 05Feb09 - KG;
    ''' </summary>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Enum Tab As Integer
        NewTab = 1
        LocateTab = 2
        PrintTab = 3
    End Enum

    ''' <summary>
    ''' Enumerator - Cache;
    ''' 05Feb09 - KG;
    ''' </summary>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Enum ECache As Integer
        ' master list
        ItemList = 11
        SupplierList = 12
        ConsumerList = 13
        EquipmentList = 14
        UserNameList = 16


        ' stock control's order
        OrderList = 21
        UnfulfilledOrder = 23
        ReceivedOrder = 24

        ' stock control's receive
        ReceiveItem = 31

        ' stock control's issue from store
        IssueDte = 41
        RequestList = 42
        RequestListSearch = 43

        ' Adjustment
        AdjustInList = 51
        AdjustOutList = 52

    End Enum

    ''' <summary>
    ''' Enumerator - Session;
    ''' NOTE: the string of the ENum is used instead of the Integer value;
    ''' 05Feb09 - KG;
    ''' </summary>
    ''' <remarks>
    ''' <example>
    ''' Session(ESession.StoreID.toString)
    ''' </example>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Enum ESession As Integer
        StoreID = 1
        StoreName = 2
        UserID = 3
        UserName = 4
        UserDesignation = 5
        UserRoleType = 6
        UserLastLogin = 7

        OrderDte = 11
    End Enum

    ''' <summary>
    ''' Enumerator - ViewState;
    ''' 05Feb09 - KG;
    ''' </summary>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Enum EViewState As Integer
        ' stock control's all pages
        SortDirection = 11
        Mode = 12
        RecordStatus = 13

        ' stock control's order
        DeliveryLapseDay = 21
        OrderItem = 22
        MaxLevel = 23
        TotalCost = 24
        OrderQty = 25
        OrderItemLocate = 26
        OrderIdIsValid = 27
        GebizIsValid = 28
        OrderedStockItemID = 29
        OrderedStockItemIDLocate = 30

        ' stock control's receive
        ReceiveItem = 31
        ReceiveItemLocate = 32
        ReceiveQty = 33

        ' stock control's issue from store aka request
        RequestedStockItemID = 41
        IssueItem = 42
        IssueItemLocate = 43
        IssueQty = 44
        IssueInsert = 45
        IssueUpdate = 46
        IssueDelete = 47
        RequestedStockItemIDLocate = 48

        ' Adjustment
        AdjustItem = 51
        AdjustItemLocate = 52
        AdjustedStockItemID = 53
        AdjustedStockItemIDLocate = 54
        AdjustOutSelected = 55
        AdjustInSelected = 56

    End Enum

    ''' <summary>
    ''' Enumerator - ModuleID
    ''' 15 Feb 09 - Jianfa;
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum moduleID As Integer

        '-- Master List Modules
        Item = 101
        Supplier = 102
        Consumer = 103
        Equipment = 104
        Role = 105
        Store = 106
        Common = 107

        '-- Stock Control Modules
        OrderItem = 201
        ReceivedItem = 202
        IssueFromStore = 203
        DirectIssue = 204

        '-- Adjustment Modules
        AdjustmentInwards = 301
        AdjustmentOutwards = 302
        StockReturnInwards = 303
        StockReturnOutwards = 304

        '-- Stock Card Modules
        StockCard = 401

        '-- Verification Worksheet Modules
        VerificationWorkSheet = 501
        PrintVerificationWorksheet = 502

        '-- Audit Trail Report Modules
        StockItem = 601
        Order = 602
        StockTransactions = 603
        ResidueCostAdjustment = 604
        AConsumer = 605
        ASupplier = 606
        ARole = 607
        AEquipment = 608
        ACommon = 609
        AStore = 610

        '-- Management Report Modules
        LocationStockBalanceListing = 701
        TransactionListing = 702
        IssueDocumentDetails = 703
        StockReviewList = 704
        StockReturn = 705
        StockReturnCheckList = 706
        PeriodicIssues = 707
        QuantityIssueSummary = 708
        ReorderStockItemList = 709
        StockAdjustmentEntries = 710
        AdhocReport = 711

    End Enum

    ''' <summary>
    ''' Function - GetMessage;
    ''' 23 Dec 2008 - Jianfa
    ''' </summary>
    ''' <param name="messageID"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' CODE LOGIC:
    ''' 1) Return specific messages
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' 23Dec2008  Jianfa      1      As indicated
    ''' 01Jan2009  Jianfa      2      Include an generic param messageModule
    ''' </remarks>
    Protected Friend Shared Function GetMessage(ByVal messageID As Integer, _
                                Optional ByVal messageType As String = "", _
                                Optional ByVal messageModule As String = "", _
                                Optional ByVal compare1 As String = "", _
                                Optional ByVal compare2 As String = "", _
                                Optional ByVal compareType As String = "") As String

        'TODO: Add messages accordingly based on enumerators added
        Select Case messageID

            Case 1 'Mandatory Field
                Return "Please enter mandatory fields indicated as (*)."
            Case 2 'Try Last operation
                Return "Error: Please try last operation again."
            Case 3 'Sucess
                Return "Your " & messageModule & " has been " & messageType & " successfully."
            Case 4 'Confirm
                Return "Are you sure you want to save your " & messageModule & "? "
            Case 5 'Numeric Fields
                Return "Please enter a numeric value for " & messageType & ". "
            Case 6 'More or Less Than
                Return "Please ensure that " & compare1 & " is " & compareType & " " & compare2 & ". "
            Case 7 'Range From To
                Return "Please ensure that " & messageType & " is within the range from " & compare1 & " to " & compare2 & ". "
            Case 8 'At Least One Item is Selected
                Return "Please ensure that at least one " & messageType & " is selected for " & messageModule & ". "
            Case 9 'No record found
                Return "No Record Found. "
            Case 10 'Date From must not be later than Date To
                Return "Please ensure that Date To must not be earlier than Date From. "
            Case 11 'Stock Code From must not be later than Stock Code To
                Return "Please ensure that Stock Code To must not be smaller than Stock Code From. "
            Case 12 'Stock Code Invalid
                Return "Invalid Stock Code [" & messageType & "]. Please re-enter again. "
            Case 13
                Return "Stock Code [" & messageType & "] is Closed. "
            Case 15
                Return "Sorry you have been logged out because the system detected that you are logged in on another machine."


            Case 101 'Exception
                Return "Error: Exception Occur"
            Case 102 'Application Exception
                Return "Error: Application Exception Occur"
            Case 103 'Not is Date
                Return "Please ensure that " & messageType & " is a valid formatted date. "
            Case 104 'Not in Financial
                Return "Please ensure that " & messageType & " is within the Financial Cut Off Date. "
            Case 105 'dropdownlist not selected
                Return "Please ensure that the drop down list [" & messageType & "] is Selected. "
            Case 106 'dropdownlist not selected
                Return "Please ensure that the correct [" & messageType & "] is input. "
            Case 107 'Stock Code has Ordered
                Return String.Format("Stock Code[{0}] already used.  Please select another Stock Code.", messageType)
            Case 108 'Stock Code Not Selected
                Return String.Format("Please ensure that a Stock Code is selected.")
            Case 109 'Stock Code Not Added
                Return String.Format("Please ensure that a Stock Code is added.")
            Case 110 'DateHasReceived
                Return String.Format("Date[{0}] already received. Please select another date.", messageType)
            Case 111 'NoChange
                Return String.Format("There is nothing to save, please update and try again.")
            Case 112 'fieldNotUnique
                Return "Please ensure that the [" & messageType & "] is a unique value. "
            Case 113 'OwnRequest
                Return "Approval cannot be performed on your own request."
            Case 114 'AtLeast1Item
                Return String.Format("Please ensure at least 1 item is selected and filled with information.")
        End Select

        Return String.Empty
    End Function

#End Region

#Region " COMMON Functions "

    ''' <summary>
    ''' Function - GetCommonDataTable()
    ''' 24 Dec 2008 - Jianfa
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    Protected Shared Function GetCommonDataTable(ByVal storeID As String) As DataTable

        Dim dtCommon As New DataTable

        Try

            Dim Client As New ServiceClient
            Dim CommonDetails As New CommonDetails
            Dim CommonList As New List(Of CommonDetails)

            CommonDetails.StoreID = storeID
            CommonDetails.CodeGroup = ""
            CommonDetails.Status = ""

            CommonList = Client.GetCommon(CommonDetails)
            Client.Close()

            dtCommon = ConvertCommonListToDataTable(CommonList)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return dtCommon

    End Function

    ''' <summary>
    ''' Function - GetCodeDescription;
    ''' 23 Dec 2008 
    ''' </summary>
    ''' <param name="codeID"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' CODE LOGIC:
    ''' 1) Return Code Description
    ''' </remarks>
    Protected Shared Function GetCodeDescription(ByVal dtCommon As DataTable, ByVal codeGroup As Integer, _
                                                 Optional ByVal codeID As String = "", _
                                                 Optional ByVal codeStatus As String = "") As String

        Dim CodeDescription As String = String.Empty
        Dim View As DataView = New DataView(dtCommon)

        Select Case codeGroup

            Case 1 'Supplier
                View.RowFilter = " CommonCodeGroup = 'Supplier' and CommonStatus Like '%" & codeStatus & "%' "

            Case 2 'User Role
                View.RowFilter = " CommonCodeGroup = 'User Role' and CommonStatus Like '%" & codeStatus & "%' "

            Case 3 'UOM
                View.RowFilter = " CommonCodeGroup = 'UOM' and CommonStatus Like '%" & codeStatus & "%' and CommonCodeID = '" & codeID & "'"
            Case 4 'StockType 
                View.RowFilter = " CommonCodeGroup = 'Stock Type' and CommonStatus Like '%" & codeStatus & "%' and CommonCodeID Like '%" & codeID & "%'"

            Case 5 'SubType 
                View.RowFilter = " CommonCodeGroup = 'Sub Type' and CommonStatus Like '%" & codeStatus & "%' and CommonCodeID Like '%" & codeID & "%'"

            Case 6 'Equipment Type
                View.RowFilter = " CommonCodeGroup = 'Equipment Group' and CommonStatus Like '%" & codeStatus & "%' "
                View.Sort = " CommonCodeDescription ASC"

        End Select

        For Each Row As DataRowView In View
            CodeDescription = CodeDescription & Row.Item("CommonCodeDescription").ToString & ","
        Next

        If CodeDescription.Length <> 0 Then
            CodeDescription = Left(CodeDescription, CodeDescription.Length - 1)
        End If

        Return CodeDescription

    End Function

    ''' <summary>
    ''' Function - Get Code ID;
    ''' 01 Jan 2009 - Jianfa;
    ''' </summary>
    ''' <param name="codeGroup"></param>
    ''' <param name="codeStatus"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Shared Function GetCodeID(ByVal dtCommon As DataTable, ByVal storeID As String, ByVal codeGroup As Integer, _
                                        Optional ByVal codeStatus As String = "") As String

        Dim CodeID As String = String.Empty
        Dim View As DataView = New DataView(dtCommon)

        Select Case codeGroup

            Case 1 'Supplier
                View.RowFilter = " CommonCodeGroup = 'Supplier' and CommonStatus Like '%" & codeStatus & "%' "

            Case 2 'Role
                View.RowFilter = " CommonCodeGroup = 'User Role' and CommonStatus Like '%" & codeStatus & "%' "

            Case 3 'UOM
                View.RowFilter = " CommonCodeGroup = 'UOM' and CommonStatus Like '%" & codeStatus & "%' "

            Case 4 'StockType 
                View.RowFilter = " CommonCodeGroup = 'Stock Type' and CommonStatus Like '%" & codeStatus & "%' "

            Case 5 'SubType 
                View.RowFilter = " CommonCodeGroup = 'Sub Type' and CommonStatus Like '%" & codeStatus & "%' "

            Case 6 'Equipment Type
                View.RowFilter = " CommonCodeGroup = 'Equipment Group' and CommonStatus Like '%" & codeStatus & "%' "
                View.Sort = " CommonCodeDescription ASC"

            Case 9 'Direct Doc Type
                View.RowFilter = " CommonCodeGroup = 'Doc Type' and CommonStatus Like '%" & codeStatus & "%' and CommonCodeID Like 'D%' "

            Case 12 'Delivery Lapse Day
                View.RowFilter = " CommonCodeGroup = 'Delivery Lapse Day' and CommonStatus Like '%" & codeStatus & "%' "

        End Select

        For Each Row As DataRowView In View
            CodeID = CodeID & Row.Item("CommonCodeID").ToString & ","
        Next

        CodeID = Left(CodeID, CodeID.Length - 1)
        Return CodeID

    End Function

    ''' <summary>
    ''' Function - GetCommonData
    ''' 07 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="codeGroup"></param>
    ''' <param name="codeStatus"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Shared Function GetCommonDataByCodeGroup(ByVal dtCommon As DataTable, ByVal codeGroup As Integer, _
                                                       Optional ByVal codeStatus As String = "", _
                                                       Optional ByVal exclude As String = "") As DataView

        Dim View As DataView = New DataView(dtCommon)

        Select Case codeGroup

            Case 1 'Supplier
                View.RowFilter = " CommonCodeGroup = 'Supplier' and CommonStatus Like '%" & codeStatus & "%' "

            Case 2 'Role
                View.RowFilter = " CommonCodeGroup = 'User Role' and CommonStatus Like '%" & codeStatus & "%' "

            Case 3 'UOM
                View.RowFilter = " CommonCodeGroup = 'UOM' and CommonStatus Like '%" & codeStatus & "%' "

            Case 4 'StockType 
                View.RowFilter = IIf(exclude = String.Empty, _
                " CommonCodeGroup = 'Stock Type' and CommonStatus Like '%" & codeStatus & "%' ", _
                " CommonCodeGroup = 'Stock Type' and CommonStatus Like '%" & codeStatus & "%' and CommonCodeID <> '" & exclude & "' ")
                
            Case 5 'SubType 
                View.RowFilter = " CommonCodeGroup = 'Sub Type' and CommonStatus Like '%" & codeStatus & "%' "

            Case 6 'Equipment Type
                View.RowFilter = " CommonCodeGroup = 'Equipment Group' and CommonStatus Like '%" & codeStatus & "%' "
                View.Sort = " CommonCodeDescription ASC"

            Case 7 'Order Doc Type
                View.RowFilter = " CommonCodeGroup = 'Doc Type' and CommonStatus Like '%" & codeStatus & "%' and CommonCodeID Like 'O%' "

            Case 8 'Issue Doc Type
                View.RowFilter = " CommonCodeGroup = 'Doc Type' and CommonStatus Like '%" & codeStatus & "%' and CommonCodeID Like 'I%' "

            Case 9 'Direct Doc Type
                View.RowFilter = " CommonCodeGroup = 'Doc Type' and CommonStatus Like '%" & codeStatus & "%' and CommonCodeID Like 'D%' "

            Case 10 'Adjust Inwards Doc Type
                View.RowFilter = " CommonCodeGroup = 'Doc Type' and CommonStatus Like '%" & codeStatus & "%' and CommonCodeID Like 'AI%' and CommonCodeID <> 'AIRETURN' "

            Case 11 'Adjust Outwards Doc Type
                'View.RowFilter = " CommonCodeGroup = 'Doc Type' and CommonStatus Like '%" & codeStatus & "%' and CommonCodeID Like 'AO%' "
                View.RowFilter = " CommonCodeGroup = 'Doc Type' and CommonStatus Like '%" & codeStatus & "%' and CommonCodeID Like 'AO%' and CommonCodeID NOT IN ('AODAMAGE','AOOBSOLETE','AOLOSS')  "
            Case 13 'Adjust Obsolete, Damage Doc Type
                View.RowFilter = " CommonCodeGroup = 'Doc Type' and CommonStatus Like '%" & codeStatus & "%' and CommonCodeID IN ('AODAMAGE','AOOBSOLETE') "
        End Select

        Return View

    End Function

    ''' <summary>
    ''' Function - IsWithinFinanceCutOffDate;
    ''' 09 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="TransactionDate"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' CODE LOGIC: 
    ''' ------------------------------------------------------------------------------
    ''' 1) Check if param date is of current system month and year, return true
    ''' 2) If param date is accrued 2 months of current system date, return false
    ''' 3) If param date is accrued 1 month, retrieve cutoff date for current month. 
    ''' E.g 05/01/2009 (cutoff date) => compare with current system date
    ''' If current system date > 05/01/2009 ==> return False Else return True 
    ''' ------------------------------------------------------------------------------
    ''' </remarks>
    Protected Shared Function IsWithinFinanceCutoffDate(ByVal dtCommon As DataTable, ByVal storeID As String, ByVal transactionDate As Date) As Boolean

        If Month(transactionDate) = Month(Date.Now) And Year(transactionDate) = Year(Date.Now) Then
            Return True
        Else

            If DateDiff(DateInterval.Month, transactionDate, Date.Now) >= 2 Then
                Return False
            Else

                Dim View As DataView = New DataView(dtCommon)
                Dim Days As Integer
                Dim CutoffDate As Date
                View.RowFilter = " CommonStoreID = '" & storeID & "' and CommonCodeGroup = 'Finance Cutoff Day' and CommonCodeID = '" & Left(MonthName(Month(Now)).ToUpper, 3) & "' "

                For Each row As DataRowView In View
                    Days = CInt(row.Item("CommonCodeDescription"))
                Next

                CutoffDate = New Date(Date.Now.Year, Date.Now.Month, Days)

                If Date.Now > CutoffDate Then
                    Return False
                Else
                    Return True
                End If

            End If
        End If

    End Function

    ''' <summary>
    ''' Function - BindActiveCode;
    ''' 08 Jan 09 - Jianfa;
    ''' </summary>
    ''' <param name="dropDownList"></param>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Shared Function BindActiveCodes(ByVal dropDownList As DropDownList, ByVal value As String) As String

        For idx As Integer = 0 To dropDownList.Items.Count - 1

            If dropDownList.Items(idx).Value = value Then
                Return value
                Exit For
            End If
        Next

        Return String.Empty

    End Function

    ''' <summary>
    ''' Convert text to date;
    ''' 31 Jan 09 - KG;
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns>if invalid, return DateTime.MinValue</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Friend Shared Function ConvertToDate(ByVal value As String, Optional ByVal format As String = "dmy", Optional ByVal separator As Char = "/") As Date
        Dim Formatted As Date
        Try
            If format = "dmy" Or format = "mdy" Then

                Dim Day As String
                Dim Month As String
                Dim Year As String
                Dim Position As Integer

                Position = value.IndexOf(separator)
                If format = "dmy" Then

                    Day = value.Substring(0, Position)
                    value = value.Substring(Position + 1)
                    Position = value.IndexOf(separator)
                    Month = value.Substring(0, Position)
                Else

                    Month = value.Substring(0, Position)
                    value = value.Substring(Position + 1)
                    Position = value.IndexOf(separator)
                    Day = value.Substring(0, Position)
                End If

                Year = value.Substring(Position + 1)

                Formatted = Convert.ToDateTime(Year + separator + Month + separator + Day)
            Else

                Formatted = Convert.ToDateTime(value)
            End If

        Catch ex As Exception
            Formatted = DateTime.MinValue
        End Try
        Return Formatted
    End Function

    ''' <summary>
    ''' BindDropDownList;
    ''' 4 Feb 09 - KG;
    ''' accept a dropdownlist control and bind it with the datasource, value n text
    ''' </summary>
    ''' <param name="ddl">dropdownlist to be bound</param>
    ''' <param name="dataSource">datasource use for binding</param>
    ''' <param name="value">dropdownlist's value</param>
    ''' <param name="text">dropdownlist's display text</param>
    ''' <remarks></remarks>
    Protected Shared Sub BindDropDownList(ByRef ddl As DropDownList, ByRef dataSource As Object, ByVal value As String, ByVal text As String)
        Try
            ddl.DataSource = dataSource
            ddl.DataValueField = value
            ddl.DataTextField = text
            ddl.DataBind()

            If ddl.Items.Count > 0 Then
                ddl.Items.Insert(0, New ListItem(" - Please Select - ", EMPTY))
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' BindDropDownListDte;
    ''' 25Feb09 - KG;
    ''' accept a dropdownlist control and bind it with the datelist;
    ''' </summary>
    ''' <param name="ddl">dropdownlist to be bound</param>
    ''' <param name="dateList">datelist use for binding</param>
    ''' <remarks></remarks>
    Protected Shared Sub BindDropDownListDte(ByRef ddl As DropDownList, ByVal dateList As List(Of Date))
        Try
            ddl.Items.Clear()
            For Each item In dateList
                ddl.Items.Add(item.ToString("dd/MM/yyyy"))
            Next
            If ddl.Items.Count > 1 Then
                ddl.Items.Insert(0, New ListItem(" - Please Select - ", EMPTY))

            ElseIf ddl.Items.Count = 0 Then
            ddl.Items.Add(New ListItem(" - No Record - ", EMPTY))
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' GetItemList;
    ''' 26 Jan 09 - KG;
    ''' Get StockItem List with status = "Open" and fill the provided parameter
    ''' </summary>
    ''' <param name="itemList">param to be filled</param>
    ''' <param name="storeID">Store ID</param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Shared Sub GetItemList(ByRef itemList As List(Of ItemDetails), ByVal storeID As String)
        Try
            Dim Client As New ServiceClient
            Dim ItemSearch As New ItemDetails

            ItemSearch.StoreID = storeID
            ItemSearch.ItemID = EMPTY
            ItemSearch.Location = EMPTY
            ItemSearch.Status = OPEN


            itemList = Client.GetItems(ItemSearch, "StockItemID", EMPTY)

            Client.Close()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' GetSupplierList;
    ''' 4 Feb 09 - KG;
    ''' Get Supplier List with status = "Open" and fill the provided parameter
    ''' </summary>
    ''' <param name="supplierList">param to be filled</param>
    ''' <param name="storeID">Store ID</param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Shared Sub GetSupplierList(ByRef supplierList As List(Of SupplierDetails), ByVal storeID As String)
        Try
            Dim Client As New ServiceClient
            Dim SupplierDetails As New SupplierDetails

            SupplierDetails.StoreId = storeID
            SupplierDetails.SupplierId = EMPTY
            SupplierDetails.CompanyName = EMPTY
            SupplierDetails.Status = OPEN

            supplierList = Client.GetSuppliers(SupplierDetails, "SupplierId", EMPTY)

            Client.Close()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' GetOrderList;
    ''' 08Feb09 - KG;
    ''' Get Order List with status = both "Open" n "Closed" order by "Open" 1st and fill the provided parameter
    ''' </summary>
    ''' <param name="orderList">param to be filled</param>
    ''' <param name="storeID">Store ID</param>
    ''' <param name="status">"O" will includes Open and Partial filled orders</param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Shared Sub GetOrderList(ByRef orderList As List(Of OrderDetails), ByVal storeID As String, Optional ByVal status As String = "O")
        Try
            Dim Client As New ServiceClient
            orderList = Client.GetOrder(storeID, status)
            Client.Close()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' GetRequestList;
    ''' Get Request List by status;
    ''' 08Feb09 - KG;
    ''' </summary>
    ''' <param name="requestList">param to be filled</param>
    ''' <param name="storeID">Store ID</param>
    ''' <param name="status">empty will retrieve all</param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Shared Sub GetRequestList(ByRef requestList As List(Of RequestDetails), ByVal storeID As String, Optional ByVal status As String = "O")
        Try
            Dim Client As New ServiceClient
            requestList = Client.GetRequest(storeID, status)
            Client.Close()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' GetRequestList;
    ''' Get Request List by search critera;
    ''' 08Feb09 - KG;
    ''' </summary>
    ''' <param name="requestList">param to be filled</param>
    ''' <param name="storeID">Store ID</param>
    ''' <param name="status">empty will retrieve all</param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Shared Sub GetRequestList(ByRef requestList As List(Of RequestDetails), ByVal storeID As String, ByVal consumerID As String, ByVal requestID As String, Optional ByVal status As String = "O")
        Try
            Dim requestSearch As New RequestDetails
            Dim Client As New ServiceClient
            requestSearch.StoreID = storeID
            requestSearch.ConsumerID = consumerID
            requestSearch.RequestID = requestID
            requestSearch.Status = status

            requestList = Client.GetRequestBySearch(requestSearch)
            Client.Close()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' GetConsumerList;
    ''' 18Feb09 - KG;
    ''' Get Consumer List with status = "Open" and fill the provided parameter
    ''' <param name="supplierList">param to be filled</param>
    ''' <param name="storeID">Store ID</param>
    ''' </summary>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Shared Sub GetConsumerList(ByRef consumerList As List(Of ConsumerDetails), ByVal storeID As String)
        Try
            Dim Client As New ServiceClient
            Dim ConsumerDetails As New ConsumerDetails

            ConsumerDetails.StoreID = storeID
            ConsumerDetails.ConsumerID = EMPTY
            ConsumerDetails.ConsumerDescription = EMPTY
            ConsumerDetails.ConsumerStatus = OPEN

            consumerList = Client.GetConsumers(ConsumerDetails, EMPTY, EMPTY)

            Client.Close()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' GetConsumerListByUserID;
    ''' 18Feb09 - KG;
    ''' Get Consumer List by User ID and fill the provided parameter;
    ''' <param name="supplierList">param to be filled</param>
    ''' <param name="storeID">Store ID</param>
    ''' </summary>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Shared Sub GetConsumerListByUserID(ByRef consumerList As List(Of ConsumerDetails), ByVal storeID As String, ByVal userID As String, ByVal userRole As String)
        Try
            Dim Client As New ServiceClient
            Dim ConsumerDetails As New ConsumerDetails

            ConsumerDetails.StoreID = storeID
            ConsumerDetails.UserID = userID
            ConsumerDetails.UserRole = userRole
            ConsumerDetails.ConsumerRefStatus = OPEN

            consumerList = Client.GetConsumerRefByUserID(ConsumerDetails)

            Client.Close()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' GetUserList;
    ''' 18Feb09 - KG;
    ''' Get User Nric Name List;
    ''' <param name="userNameList">param to be filled</param>
    ''' <param name="storeID">Store ID</param>
    ''' </summary>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Shared Sub GetUserList(ByRef userNameList As List(Of ConsumerDetails), ByVal storeID As String)
        Try
            Dim Client As New ServiceClient
            Dim UserDetails As New ConsumerDetails

            UserDetails.StoreID = storeID
            userNameList = Client.GetUsers(UserDetails)
            Client.Close()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' GetAdjustList;
    ''' 28Feb09 - KG;
    ''' Get Adjust List by Adjust Type;
    ''' </summary>
    ''' <param name="adjustList">param to be filled</param>
    ''' <param name="storeID">Store ID</param>
    ''' <param name="adjustType">Inward or Outward type</param>
    ''' <param name="status">"C" all adjustment's status is closed only</param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Shared Sub GetAdjustList(ByRef adjustList As List(Of AdjustDetails), ByVal storeID As String, ByVal adjustType As String, Optional ByVal status As String = "C")
        Try
            Dim Client As New ServiceClient
            adjustList = Client.GetAdjust(storeID _
                                          , AdjustType _
                                          , status _
                                          )
            Client.Close()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' GetAdjustListBySearch;
    ''' Get Adjust List by search critera;
    ''' 09 Mar 12 - Christina;
    ''' </summary>
    ''' <param name="adjustList">param to be filled</param>
    ''' <param name="storeID">Store ID</param>
    ''' <param name="status">empty will retrieve all</param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Shared Sub GetAdjustListBySearch(ByRef adjustList As List(Of AdjustDetails), ByVal storeID As String, ByVal consumerID As String, ByVal adjustID As String, ByVal adjustType As String, Optional ByVal status As String = "O")
        Try
            Dim adjustSearch As New AdjustDetails
            Dim Client As New ServiceClient
            adjustSearch.StoreID = storeID
            adjustSearch.InvolveID = consumerID
            adjustSearch.AdjustID = adjustID
            adjustSearch.Status = status
            adjustSearch.Type = adjustType

            adjustList = Client.GetAdjustBySearch(adjustSearch)
            Client.Close()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' CacheListIsEmpty;
    ''' Return True when List is either Nothing or Count is Zero;
    ''' 24Feb09 - KG;
    ''' </summary>
    ''' <param name="cacheList"></param>
    ''' <param name="objectType"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Shared Function CacheListIsEmpty(ByVal cacheList As Object _
                                               , ByVal objectType As Type _
                                               ) As Boolean
        If cacheList Is Nothing Then
            Return True

        Else
            Try
                Select Case objectType.ToString
                    'Master List
                    Case GetType(CommonDetails).ToString
                        Return IIf(DirectCast(cacheList, List(Of CommonDetails)).Count = 0, True, False)
                    Case GetType(ConsumerDetails).ToString
                        Return IIf(DirectCast(cacheList, List(Of ConsumerDetails)).Count = 0, True, False)
                    Case GetType(EquipmentDetails).ToString
                        Return IIf(DirectCast(cacheList, List(Of EquipmentDetails)).Count = 0, True, False)
                    Case GetType(ItemDetails).ToString
                        Return IIf(DirectCast(cacheList, List(Of ItemDetails)).Count = 0, True, False)
                    Case GetType(MoreItemInfoDetails).ToString
                        Return IIf(DirectCast(cacheList, List(Of MoreItemInfoDetails)).Count = 0, True, False)
                    Case GetType(RoleDetails).ToString
                        Return IIf(DirectCast(cacheList, List(Of RoleDetails)).Count = 0, True, False)
                    Case GetType(StoreDetails).ToString
                        Return IIf(DirectCast(cacheList, List(Of StoreDetails)).Count = 0, True, False)
                    Case GetType(SupplierDetails).ToString
                        Return IIf(DirectCast(cacheList, List(Of SupplierDetails)).Count = 0, True, False)

                        ' Stock Control
                    Case GetType(AdjustDetails).ToString
                        Return IIf(DirectCast(cacheList, List(Of AdjustDetails)).Count = 0, True, False)
                    Case GetType(AdjustItemDetails).ToString
                        Return IIf(DirectCast(cacheList, List(Of AdjustItemDetails)).Count = 0, True, False)
                    Case GetType(AdjustList).ToString
                        Return IIf(DirectCast(cacheList, List(Of AdjustList)).Count = 0, True, False)
                    Case GetType(DirectIssueDetails).ToString
                        Return IIf(DirectCast(cacheList, List(Of DirectIssueDetails)).Count = 0, True, False)
                    Case GetType(IssueItemDetails).ToString
                        Return IIf(DirectCast(cacheList, List(Of IssueItemDetails)).Count = 0, True, False)
                    Case GetType(IssueList).ToString
                        Return IIf(DirectCast(cacheList, List(Of IssueList)).Count = 0, True, False)
                    Case GetType(OrderDetails).ToString
                        Return IIf(DirectCast(cacheList, List(Of OrderDetails)).Count = 0, True, False)
                    Case GetType(OrderItemDetails).ToString
                        Return IIf(DirectCast(cacheList, List(Of OrderItemDetails)).Count = 0, True, False)
                    Case GetType(OrderList).ToString
                        Return IIf(DirectCast(cacheList, List(Of OrderList)).Count = 0, True, False)
                    Case GetType(ReceiveItemDetails).ToString
                        Return IIf(DirectCast(cacheList, List(Of ReceiveItemDetails)).Count = 0, True, False)
                    Case GetType(ReceiveList).ToString
                        Return IIf(DirectCast(cacheList, List(Of ReceiveList)).Count = 0, True, False)
                    Case GetType(RequestDetails).ToString
                        Return IIf(DirectCast(cacheList, List(Of RequestDetails)).Count = 0, True, False)
                End Select
                Throw New Exception(String.Format("List Type[{0}] is not currently handled. If required, please add the new type as part of the check.", objectType.ToString))

            Catch ex As Exception
                Throw ex
            End Try
        End If
    End Function

    ''' <summary>
    ''' Truncate a double value to 4 decimal place for display;
    ''' 24Feb09 - KG;
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    Protected Friend Shared Function DisplayValue(ByVal value As Double) As String
        Return (Math.Floor(value * 10000) / 10000).ToString("0.0000")
    End Function

    Protected Shared Function CheckMultipleLogin(ByVal userID As String, ByVal sessionId As String, ByVal checkIfLogout As Boolean) As Integer

        Dim NumLogins As Integer

        Dim Client As ServiceClient
        Client = New ServiceClient

        NumLogins = Client.GetUserLogins(userID, sessionId, checkIfLogout)

        Client.Close()

        Return NumLogins

    End Function

    Public Shared Sub LogError(ByVal errorMessage As String)
        Dim strFile As String = "C:\\inetpub\\wwwroot\\ICS\\log\\IcsLog.txt"
        Try
            strFile = String.Format("C:\ErrorLog_{0}.txt", DateTime.Today.ToString("dd-MMM-yyyy"))
            IO.File.AppendAllText(strFile, String.Format("Error Message in  Occured at-- {0}{1}", errorMessage, Environment.NewLine))
        Catch Ex As Exception

        End Try
    End Sub
#End Region

#Region " ACCESS RIGHTS Functions "

    ''' <summary>
    ''' Sub Proc - PopulateAccessRights;
    ''' 11 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="userID"></param>
    ''' <remarks></remarks>
    Protected Shared Function PopulateAccessRights(ByVal storeID As String, ByVal userID As String) As List(Of RoleDetails)

        Dim AccessRightsList As New List(Of RoleDetails)

        Try

            Dim Client As New ServiceClient
            Dim RoleDetails As New RoleDetails

            RoleDetails.StoreID = storeID
            RoleDetails.UserID = userID

            AccessRightsList = Client.GetModuleAccessRights(RoleDetails)
            Client.Close()

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return AccessRightsList

    End Function

    ''' <summary>
    ''' Function - AssignAccessRights;
    ''' 11 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="moduleID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Shared Function AssignAccessRights(ByVal AccessRightsList As List(Of RoleDetails), _
                                                 ByVal moduleID As Integer) As List(Of RoleDetails)

        Dim RoleDetails As New RoleDetails
        Dim AccessRights As New List(Of RoleDetails)
        Dim SelectRight, InsertRight, UpdateRight, DeleteRight As Boolean

        Try

            '-- Default Values assigned as false 
            SelectRight = False
            InsertRight = False
            UpdateRight = False
            DeleteRight = False

            For Each Item As RoleDetails In AccessRightsList

                '-- Conditional exit to avoid repetitive looping
                Select Case Item.ModuleID
                    Case Is > moduleID
                        Exit For
                End Select

                If Item.ModuleID = moduleID Then

                    '-- Select Right
                    If Item.SelectRight Then
                        SelectRight = True
                    End If

                    '-- Insert Right
                    If Item.InsertRight Then
                        InsertRight = True
                    End If

                    '-- Update Right
                    If Item.UpdateRight Then
                        UpdateRight = True
                    End If

                    '-- Delete Right
                    If Item.DeleteRight Then
                        DeleteRight = True
                    End If

                End If

            Next

            RoleDetails.SelectRight = SelectRight
            RoleDetails.InsertRight = InsertRight
            RoleDetails.UpdateRight = UpdateRight
            RoleDetails.DeleteRight = DeleteRight

            AccessRights.Add(RoleDetails)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return AccessRights

    End Function

#End Region

#Region " Sub Procedures and Functions "

    ''' <summary>
    ''' Function - ConvertCommonListToDataTable;
    ''' 11 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="CommonList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Shared Function ConvertCommonListToDataTable(ByVal CommonList As List(Of CommonDetails)) As DataTable

        Dim dt As New DataTable

        dt.Columns.Add("CommonStoreID", GetType(String))
        dt.Columns.Add("CommonID", GetType(Integer))
        dt.Columns.Add("CommonCodeGroup", GetType(String))
        dt.Columns.Add("CommonCodeID", GetType(String))
        dt.Columns.Add("CommonCodeDescription", GetType(String))
        dt.Columns.Add("CommonCodeID_Description", GetType(String))
        dt.Columns.Add("CommonStatus", GetType(String))

        For Each CommonListItem As CommonDetails In CommonList

            Dim row As DataRow = dt.NewRow()
            row.Item("CommonStoreID") = CommonListItem.StoreID
            row.Item("CommonID") = CommonListItem.CommonID
            row.Item("CommonCodeGroup") = CommonListItem.CodeGroup
            row.Item("CommonCodeID") = CommonListItem.CodeID
            row.Item("CommonCodeDescription") = CommonListItem.CodeDescription
            row.Item("CommonCodeID_Description") = CommonListItem.CodeID & " - " & CommonListItem.CodeDescription
            row.Item("CommonStatus") = CommonListItem.Status

            dt.Rows.Add(row)

        Next

        Return dt

    End Function

    ''' <summary>
    ''' Sub Proc - EditCache
    ''' 25 Feb 09 - KG, Jianfa
    ''' </summary>
    ''' <param name="cacheItem"></param>
    ''' <param name="type"></param>
    ''' <param name="item"></param>
    ''' <param name="Update"></param>
    ''' <remarks></remarks>
    Protected Shared Sub EditCache(ByRef cacheItem As Object _
                                   , ByVal type As Type _
                                   , ByVal item As Object _
                                   , Optional ByVal Update As Boolean = False _
                                   )

        Select Case type.ToString
            Case GetType(ItemDetails).ToString

                If Update Then

                    Dim itemDetail As ItemDetails = _
                    DirectCast(cacheItem, List(Of ItemDetails)).Find(Function(i As ItemDetails) i.ItemID = DirectCast(item, ItemDetails).ItemID)
                    DirectCast(cacheItem, List(Of ItemDetails)).Remove(itemDetail)

                End If

                '-- ADD TO THE CACHE
                DirectCast(cacheItem, List(Of ItemDetails)).Add(item)

                '-- SORT THE CACHE CONTENT
                Dim Sorter As New clsSorter(Of ItemDetails)
                Sorter.SortString = "ItemID"
                DirectCast(cacheItem, List(Of ItemDetails)).Sort(Sorter)

            Case GetType(OrderDetails).ToString
                If Update Then
                    Dim OrderDetails As OrderDetails = _
                    DirectCast(cacheItem, List(Of OrderDetails)).Find(Function(i As OrderDetails) i.OrderID = DirectCast(item, OrderDetails).OrderID)
                    DirectCast(cacheItem, List(Of OrderDetails)).Remove(OrderDetails)
                End If

                '-- ADD TO THE CACHE
                DirectCast(cacheItem, List(Of OrderDetails)).Add(item)

                '-- SORT THE CACHE CONTENT
                Dim Sorter As New clsSorter(Of OrderDetails)
                Sorter.SortString = "OrderID"
                DirectCast(cacheItem, List(Of OrderDetails)).Sort(Sorter)

            Case GetType(RequestDetails).ToString
                If Update Then
                    Dim RequestDetails As RequestDetails = _
                    DirectCast(cacheItem, List(Of RequestDetails)).Find(Function(i As RequestDetails) i.RequestID = DirectCast(item, RequestDetails).RequestID)
                    DirectCast(cacheItem, List(Of RequestDetails)).Remove(RequestDetails)
                End If

                '-- ADD TO THE CACHE
                DirectCast(cacheItem, List(Of RequestDetails)).Add(item)

                '-- SORT THE CACHE CONTENT
                Dim Sorter As New clsSorter(Of RequestDetails)
                Sorter.SortString = "RequestID"
                DirectCast(cacheItem, List(Of RequestDetails)).Sort(Sorter)

            Case GetType(AdjustDetails).ToString
                If Update Then
                    Dim AdjustDetails As AdjustDetails = _
                    DirectCast(cacheItem, List(Of AdjustDetails)).Find(Function(i As AdjustDetails) i.AdjustID = DirectCast(item, AdjustDetails).AdjustID)
                    DirectCast(cacheItem, List(Of AdjustDetails)).Remove(AdjustDetails)
                End If

                '-- ADD TO THE CACHE
                DirectCast(cacheItem, List(Of AdjustDetails)).Add(item)

                '-- SORT THE CACHE CONTENT
                Dim Sorter As New clsSorter(Of AdjustDetails)
                Sorter.SortString = "AdjustID"
                DirectCast(cacheItem, List(Of AdjustDetails)).Sort(Sorter)


            Case GetType(CommonDetails).ToString

                Dim dt As DataTable = DirectCast(cacheItem, DataTable)

                If Update Then

                    For Each rowToDelete As DataRow In dt.Rows

                        If rowToDelete("CommonID") = DirectCast(item, CommonDetails).CommonID Then
                            dt.Rows.Remove(rowToDelete)
                            Exit For
                        End If

                    Next

                End If

                Dim row As DataRow = dt.NewRow()
                Dim CommonDetails As CommonDetails = DirectCast(item, CommonDetails)

                row.Item("CommonStoreID") = CommonDetails.StoreID
                row.Item("CommonID") = CommonDetails.CommonID
                row.Item("CommonCodeGroup") = CommonDetails.CodeGroup
                row.Item("CommonCodeID") = CommonDetails.CodeID
                row.Item("CommonCodeDescription") = CommonDetails.CodeDescription
                row.Item("CommonStatus") = CommonDetails.Status

                dt.Rows.Add(row)

            Case GetType(WorksheetDetails).ToString

                For Each WorkSheetItem As WorksheetDetails In DirectCast(item, List(Of WorksheetDetails))
                    DirectCast(cacheItem, List(Of WorksheetDetails)).Add(WorkSheetItem)
                Next

        End Select

    End Sub

    ''' <summary>
    ''' Sub Proc - CheckValidSession();
    ''' 06 March 09 - Jianfa;
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub CheckValidSession()

        If Session("UserID") Is Nothing Then
            Server.Transfer("..\frmUnauthorisedPage.aspx")
        End If

        If Not Session("UserSessionID") Is Nothing Then
            ' if existing users is kick-out from the session
            If CheckMultipleLogin(Session("UserID"), Session("UserSessionID"), True) > 0 Then
                Session("LoginMessage") = GetMessage(messageID.MultipleLogin)
                Server.Transfer("..\frmUnauthorisedPage.aspx")
            End If
        End If

    End Sub

    ''' <summary>
    ''' Sub Proc - CheckValidSession();
    ''' 06 March 09 - Jianfa;
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub CheckValidSessionFromLogin()

        'If Session("UserID") Is Nothing Then
        '    Server.Transfer("frmUnauthorisedPage.aspx")
        'End If

        'If Not Session("UserSessionID") Is Nothing Then
        '    ' if existing users is found then kick-out from the session
        '    If CheckMultipleLogin(Session("UserID"), Session("UserSessionID"), True) > 0 Then
        '        Server.Transfer("frmUnauthorisedPage.aspx")
        '    End If
        'End If
    End Sub

#End Region

End Class

