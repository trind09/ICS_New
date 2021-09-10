Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF

''' <summary>
''' Interface Layer - for ICS;
''' 17 Dec 08 - Kenny GOH, Jianfa CHEN;
''' </summary>
''' <remarks>
''' CHANGE LOG:
''' 12Dec08 Kenny    GetSuppliers   To populate active suppliers;
''' 18Dec08 Jianfa   AddSupplier    To insert supplier details into DB;
''' </remarks>
<ServiceContract()> _
<ServiceKnownType(GetType(Service.ColumnName))> _
<ExceptionShielding("Service Policy")> _
Public Interface IService

#Region " Common "
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetCommon(ByVal commonDetails As CommonDetails) As List(Of CommonDetails)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetDistinctCommon(ByVal commonDetails As CommonDetails) As List(Of CommonDetails)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function AddCommon(ByVal commonDetails As CommonDetails) As String

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function UpdateCommon(ByVal commonDetails As CommonDetails) As String

#End Region

#Region " Access Rights "

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetStoreAccess(ByVal roleDetails As RoleDetails) As List(Of StoreDetails)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetUserProfile(ByVal roleDetails As RoleDetails) As List(Of RoleDetails)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetUserStoreCodes(ByVal userId As String) As String

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetUserLogins(ByVal userID As String, ByVal sessionId As String, ByVal checkIfLogout As Boolean) As Integer

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function CheckUserIdExist(ByVal userID As String) As Boolean

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetUserRoleStatus(ByVal storeID As String, ByVal userID As String) As List(Of RoleDetails)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetUserActivityList(ByVal storeId As String _
                              , ByVal userId As String _
                              , ByVal fromDte As Date _
                              , ByVal toDte As Date _
                              , ByVal byTimeStamp As Boolean _
                              , ByVal filterBy As String _
                              , ByVal sortBy As String _
                              ) As List(Of RoleDetails)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetUserUnsuccessfulLoginList(ByVal storeId As String _
                              , ByVal userId As String _
                              , ByVal fromDte As Date _
                              , ByVal toDte As Date _
                              , ByVal sortBy As String _
                              ) As List(Of RoleDetails)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetNewUserAccountList(ByVal storeId As String _
                              , ByVal fromDte As Date _
                              , ByVal toDte As Date _
                              , ByVal sortBy As String _
                              ) As List(Of RoleDetails)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetNonIcsUserUnsuccessfulLogin(ByVal storeId As String _
                              , ByVal fromDte As Date _
                              , ByVal toDte As Date _
                              ) As List(Of RoleDetails)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetInactiveUsers(ByVal storeID As String, ByVal dateFrom As DateTime, ByVal dateTo As DateTime) As List(Of RoleDetails)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function AddUserAudit(ByVal storeId As String, ByVal userId As String _
                          , ByVal userIP As String, ByVal sessionID As String _
                          , Optional ByVal isNonIcsUser As Boolean = False _
                          , Optional ByVal isInactiveUser As Boolean = False _
                          , Optional ByVal isUnsuccessfulLogin As Boolean = False) As String

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function UpdateUserAudit(ByVal roleDetails As RoleDetails, ByVal sessionId As String) As String

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function ManageInactiveUser(ByVal storeID As String) As String


    ''' <summary>
    ''' Manage Financial Closing, update past finanical cutoff date record details;
    ''' 28Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="loginUser"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Sub ManageFinancialClosing(ByVal storeID As String, ByVal loginUser As String)

#End Region

#Region " Master List "
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetSuppliers(ByVal supplierDetails As SupplierDetails, _
                           Optional ByVal sortExpression As String = "", _
                           Optional ByVal sortDirection As String = "") As List(Of SupplierDetails)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function AddSupplier(ByVal supplierDetails As SupplierDetails) As String

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function UpdateSupplier(ByVal supplierDetails As SupplierDetails) As String

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function UpdateSupplierStatus(ByVal supplierDetails As SupplierDetails) As String

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function AddEquipment(ByVal equipmentDetails As EquipmentDetails) As String

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetEquipments(ByVal equipmentDetails As EquipmentDetails, _
                           Optional ByVal sortExpression As String = "", _
                           Optional ByVal sortDirection As String = "") As List(Of EquipmentDetails)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function UpdateEquipment(ByVal equipmentDetails As EquipmentDetails) As String

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function UpdateEquipmentStatus(ByVal equipmentDetails As EquipmentDetails) As String

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function AddStore(ByVal storeDetails As StoreDetails) As String

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetStores(ByVal storeDetails As StoreDetails, _
                       Optional ByVal sortExpression As String = "", _
                       Optional ByVal sortDirection As String = "") As List(Of StoreDetails)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function UpdateStore(ByVal storeDetails As StoreDetails) As String

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function UpdateStoreStatus(ByVal storeDetails As StoreDetails) As String

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GenerateItemID(ByVal itemDetails As ItemDetails, _
                            ByRef generatedItemID As String) As String

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function AddItem(ByVal itemDetails As ItemDetails) As String

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetItems(ByVal itemDetails As ItemDetails, _
                      Optional ByVal sortExpression As String = "", _
                      Optional ByVal sortDirection As String = "") As List(Of ItemDetails)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetItemsMasterList(ByVal storeID As String, _
                    ByVal printOption As String, _
                      ByVal sortBy As String, _
                      ByVal stockCodeFrom As String, _
                      ByVal stockCodeTo As String, _
                      ByVal excludeStockCodeTypes As String, _
                      ByVal itemStatus As String) As List(Of ItemDetails)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetStockTransaction(ByVal itemDetails As ItemDetails, ByVal transactionType As String) _
                                 As List(Of ItemDetails)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function UpdateItem(ByVal itemDetails As ItemDetails, _
                        ByRef returnMessage As String) As String

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function UpdateItemStatus(ByVal itemDetails As ItemDetails, ByRef returnMessage As String) As String

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetItemSearch(ByVal itemDetails As ItemDetails) As List(Of String)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function IsValidStockCode(ByVal itemDetails As ItemDetails) As Boolean

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function IsValidStatus(ByVal itemDetails As ItemDetails) As Boolean

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetModuleRoles(ByVal roleDetails As RoleDetails) As List(Of RoleDetails)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetUserRoles(ByVal roleDetails As RoleDetails, _
                          Optional ByVal sortExpression As String = "", _
                          Optional ByVal sortDirection As String = "") As List(Of RoleDetails)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function UpdateModuleRole(ByVal roleDetails As RoleDetails) As String

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function UpdateUserRole(ByVal roleDetails As RoleDetails, ByVal consumerList As List(Of String)) As String

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function CheckNRIC(ByVal roledetails As RoleDetails) As List(Of RoleDetails)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function AddUserRole(ByVal roleDetails As RoleDetails, ByVal consumerList As List(Of String)) As String

    <OperationContract()>
    <FaultContract(GetType(ServiceFault))>
    Function DeleteUserRole(ByVal roleDetails As RoleDetails) As String

    <OperationContract()>
    <FaultContract(GetType(ServiceFault))>
    Function GetUserRoleIDBySoeID(ByVal soeID As String) As DataSet

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetConsumers(ByVal consumerDetails As ConsumerDetails, _
                         Optional ByVal sortExpression As String = "", _
                         Optional ByVal sortDirection As String = "") As List(Of ConsumerDetails)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function AddConsumer(ByVal consumerDetails As ConsumerDetails, _
                         ByVal userList As List(Of String)) As String

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetConsumerRef(ByVal consumerDetails As ConsumerDetails) As List(Of ConsumerDetails)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetConsumerRefByUserID(ByVal consumerDetails As ConsumerDetails) As List(Of ConsumerDetails)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetUsers(ByVal consumerDetails As ConsumerDetails) As List(Of ConsumerDetails)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetUserRef(ByVal consumerDetails As ConsumerDetails) As List(Of ConsumerDetails)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function UpdateConsumer(ByVal consumerDetails As ConsumerDetails, _
                         ByVal userList As List(Of String)) As String

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetModuleAccess(ByVal roleDetails As RoleDetails) As List(Of RoleDetails)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetModuleAccessRights(ByVal roleDetails As RoleDetails) As List(Of RoleDetails)

#End Region

#Region " Verification Worksheet "

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetWorkSheetItems(ByVal workSheetDetails As WorksheetDetails, _
                               Optional ByVal sortExpression As String = "", _
                               Optional ByVal sortDirection As String = "") As List(Of WorksheetDetails)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function AddWorkSheetItem(ByVal workSheetDetails As WorksheetDetails, _
                              ByVal workSheetItemList As List(Of WorksheetDetails), _
                              ByRef workSheetID As Integer) As String

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetWorksheetGeneratedDate(ByVal workSheetDetails As WorksheetDetails) As String

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function UpdateWorksheet(ByVal workSheetDetails As WorksheetDetails) As String

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function DeleteWorksheet(ByVal workSheetDetails As WorksheetDetails) As String

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetMarkedWorksheetItems(ByVal workSheetDetails As WorksheetDetails, _
                                     Optional ByVal sortExpression As String = "", _
                                     Optional ByVal sortDirection As String = "") As List(Of WorksheetDetails)

#End Region

#Region " Stock Control "

#Region " Common Function "
    ''' <summary>
    ''' Check field is Unique, PK is ONLY required when the check is on existing record and column is not the PK;
    ''' 05Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="columnName">column to check</param>
    ''' <param name="columnValue">value to check against the column</param>
    ''' <param name="pkColumnValue">primary key value</param>
    ''' <returns>True = Unique</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function FieldIsUnique(ByVal storeId As String _
                           , ByVal columnName As Service.ColumnName _
                           , ByVal columnValue As String _
                           , ByVal pkColumnValue As String _
                           ) As Boolean

    ''' <summary>
    ''' Get More Stock Item Info for a single Stock item based on specific date;
    ''' 08Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="stockItem"></param>
    ''' <param name="asOfDte"></param>
    ''' <returns>More Stock Items Info DataSet Collection</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetMoreItemInfo(ByVal storeId As String _
                             , ByVal stockItem As String _
                             , ByVal asOfDte As Date _
                             ) As MoreItemInfoDetails

    ''' <summary>
    ''' Get the Last Serial No base on the provide info;
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="moduleName"></param>
    ''' <returns>last serial no</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetLastSerialNo(ByVal storeID As String _
                             , ByVal moduleName As Service.ModuleName _
                             ) As String

    ''' <summary>
    ''' Check Financial Cutoff date, if within return TRUE;
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="tranDte"></param>
    ''' <returns>True = within; False = Not within</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function WithinFinancialCutoff(ByVal storeID As String _
                                   , ByVal tranDte As Date _
                                   ) As Boolean

    ''' <summary>
    ''' Delete all stock transaction for a single document Reference (receive, issue, adjustment) on a specific date;
    ''' 11Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="docNo"></param>
    ''' <param name="stockControlType"></param>
    ''' <param name="originalDte"></param>
    ''' <param name="loginUser"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Sub DeleteAllStockTransaction(ByVal storeID As String _
                                  , ByVal docNo As String _
                                  , ByVal stockControlType As Service.ModuleName _
                                  , ByVal originalDte As Date _
                                  , ByVal loginUser As String _
                                  )

#End Region

#Region " Order "
    ''' <summary>
    ''' Add Order n Order Items
    ''' 05Feb09 - KG;
    ''' </summary>
    ''' <param name="orderDetails"></param>
    ''' <param name="orderItemDetails"></param>
    ''' <returns>Error Message (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function AddOrder(ByVal orderDetails As OrderDetails _
                      , ByVal orderItemDetails As List(Of OrderItemDetails) _
                      ) As String

    ''' <summary>
    ''' Get Order based on status, default all Open Order only;
    ''' 05Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="status"></param>
    ''' <returns>Orders DataSet Collection</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetOrder(ByVal storeID As String _
                      , Optional ByVal status As String = "O" _
                      ) As List(Of OrderDetails)

    ''' <summary>
    ''' Get all Order Items for a single Order Reference;
    ''' 05Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="orderID"></param>
    ''' <returns>Order Items DataSet Collection</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetOrderItem(ByVal storeId As String _
                          , ByVal orderId As String _
                          , Optional ByVal unfullfillOnly As Boolean = False _
                          ) As List(Of OrderItemDetails)

    ''' <summary>
    ''' Update Order or OrderItem or Both Details, based on the orderitem's mode to either Delete, Insert or Update;
    ''' 05Feb09 - KG;
    ''' </summary>
    ''' <param name="orderDetails"></param>
    ''' <param name="orderItemDetails"></param>
    ''' <returns>Error Message (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function UpdateOrder(ByVal orderDetails As OrderDetails _
                         , ByVal orderItemDetails As List(Of OrderItemDetails) _
                         ) As String

    ''' <summary>
    ''' Delete Order and its Order Items details;
    ''' 08Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="orderID"></param>
    ''' <param name="status"></param>
    ''' <param name="loginUser"></param>
    ''' <returns>error message (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function DeleteOrder(ByVal storeId As String _
                         , ByVal orderId As String _
                         , ByVal status As String _
                         , ByVal loginUser As String _
                         ) As String

    ''' <summary>
    ''' Get all Order list by parameters;
    ''' when orderID is with value, ignore other parameters and get only order items relates to the single Order Reference
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="status"></param>
    ''' <param name="fromDte"></param>
    ''' <param name="toDte"></param>
    ''' <param name="fromStockItemID"></param>
    ''' <param name="toStockItemID"></param>
    ''' <param name="orderID"></param>
    ''' <returns>Orders DataSet</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetOrderList(ByVal storeId As String _
                          , Optional ByVal status As String = "" _
                          , Optional ByVal fromDte As Date = #12:00:00 AM# _
                          , Optional ByVal toDte As Date = #12:00:00 AM# _
                          , Optional ByVal fromStockItemID As String = "" _
                          , Optional ByVal toStockItemID As String = "" _
                          , Optional ByVal orderId As String = "" _
                          ) As List(Of OrderList)

#End Region

#Region " DirectIssue "
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function AddDirectIssue(ByVal directIssue As DirectIssueDetails, _
                            ByVal directIssueItemList As List(Of DirectIssueDetails), _
                            ByRef directIssueDocNo As String) As String

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetDirectIssueID(ByVal directIssue As DirectIssueDetails) As List(Of DirectIssueDetails)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetDirectIssues(ByVal directIssue As DirectIssueDetails) As List(Of DirectIssueDetails)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetDirectIssueList(ByVal storeID As String, _
                                   ByVal dteIssueFrom As Date, ByVal dteIssueTo As Date, ByVal docNo As String) _
                                   As List(Of DirectIssueDetails)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetDirectIssueItems(ByVal directIssue As DirectIssueDetails) As List(Of DirectIssueDetails)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function DeleteDirectIssue(ByVal directIssue As DirectIssueDetails) As String

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function UpdateDirectIssue(ByVal directIssue As DirectIssueDetails, _
                               ByVal directIssueItemList As List(Of DirectIssueDetails)) As String

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetDirectIssueInfo(ByVal directIssue As DirectIssueDetails) As String

#End Region

#Region " Receive "
    ''' <summary>
    ''' Get the Receive Items for a single Order Reference on a specific date
    ''' 14Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="orderID"></param>
    ''' <param name="dte"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetReceiveItem(ByVal storeID As String _
                            , ByVal orderID As String _
                            , ByVal dte As Date _
                            ) As List(Of ReceiveItemDetails)

    ''' <summary>
    ''' Get all Receive Date under the same Order
    ''' 14Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="orderID"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetReceiveDte(ByVal storeID As String _
                           , ByVal orderID As String _
                           ) As List(Of Date)

    ''' <summary>
    ''' Update the Receive Items base on the Mode either Insert, Update or Delete transaction;
    ''' 14Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="type"></param>
    ''' <param name="dte"></param>
    ''' <param name="receiveDetails"></param>
    ''' <returns>error message (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function UpdateReceiveItem(ByVal storeID As String _
                               , ByVal type As String _
                               , ByVal dte As Date _
                               , ByVal loginUser As String _
                               , ByVal receiveDetails As List(Of ReceiveItemDetails) _
                               ) As String

    ''' <summary>
    ''' Get the Receive list base on parameters;
    ''' when orderID is with value, ignore other parameters and get only receive items relates to the single Order Reference
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="fromDte"></param>
    ''' <param name="toDte"></param>
    ''' <param name="fromStockItemID"></param>
    ''' <param name="toStockItemID"></param>
    ''' <param name="orderID">overwrite other parameter and return value for this order only</param>
    ''' <returns>Receive DataSet</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetReceiveList(ByVal storeID As String _
                            , Optional ByVal fromDte As Date = #12:00:00 AM# _
                            , Optional ByVal toDte As Date = #12:00:00 AM# _
                            , Optional ByVal fromStockItemID As String = "" _
                            , Optional ByVal toStockItemID As String = "" _
                            , Optional ByVal orderID As String = "" _
                            ) As List(Of ReceiveList)

#End Region

#Region " Issue from Store (Request, Approve, Issue) "
    ''' <summary>
    ''' Add New Request and its Item Details;
    ''' 20Feb09 - KG;
    ''' </summary>
    ''' <param name="requestDetails"></param>
    ''' <param name="requestItemDetails"></param>
    ''' <returns>Error Message (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function AddRequest(ByVal requestDetails As RequestDetails _
                        , ByVal requestItemDetails As List(Of IssueItemDetails) _
                        ) As String

    ''' <summary>
    ''' Get Request based on status, default all Open Request only;
    ''' 20Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="status"></param>
    ''' <returns>Requests DataSet Collection</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetRequest(ByVal storeID As String _
                        , Optional ByVal status As String = "O" _
                        ) As List(Of RequestDetails)

    ''' <summary>
    ''' Get Request by search criteria list;
    ''' 26Feb09 - KG;
    ''' </summary>
    ''' <param name="requestDetails"></param>
    ''' <returns>Requests DataSet Collection</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetRequestBySearch(ByVal requestDetails As RequestDetails _
                                ) As List(Of RequestDetails)

    ''' <summary>
    ''' Get all Request items with or without Issue items for a single Request Reference;
    ''' 20Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="requestID"></param>
    ''' <returns>Request Items DataSet Collection</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetRequestItem(ByVal storeId As String _
                            , ByVal requestId As String _
                            ) As List(Of IssueItemDetails)

    ''' <summary>
    ''' Update only to RequestItem Details, based on the requestitem's mode to either Delete, Insert or Update;
    ''' 20Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="requestID"></param>
    ''' <param name="loginUser"></param>
    ''' <param name="requestItemDetails"></param>
    ''' <returns>Error Message (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function UpdateRequest(ByVal storeID As String _
                           , ByVal requestID As String _
                           , ByVal loginUser As String _
                           , ByVal requestItemDetails As List(Of IssueItemDetails) _
                           ) As String

    ''' <summary>
    ''' Delete Request and its Request Items details;
    ''' 20Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="requestID"></param>
    ''' <param name="loginUser"></param>
    ''' <returns>error message (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function DeleteRequest(ByVal storeId As String _
                           , ByVal requestId As String _
                           , ByVal status As String _
                           , ByVal loginUser As String _
                           ) As String

    ''' <summary>
    ''' Update Request Status to either Approved or Rejected;
    ''' 23Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="requestID"></param>
    ''' <param name="status">Approved, Rejected</param>
    ''' <param name="loginUser"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function UpdateRequestStatus(ByVal storeID As String _
                                 , ByVal requestID As String _
                                 , ByVal status As String _
                                 , ByVal loginUser As String _
                                 ) As String

    ''' <summary>
    ''' Update Request Status to either Approved or Rejected;
    ''' 03Mar12 - Christina;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="adjustID"></param>
    ''' <param name="adjustType"></param>
    ''' <param name="status">Approved, Rejected</param>
    ''' <param name="loginUser"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyyyy  AuthorName  RefID  Description;
    ''' </remarks>
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function UpdateAdjustStatus(ByVal storeID As String _
                                 , ByVal adjustID As String _
                                 , ByVal adjustType As String _
                                 , ByVal status As String _
                                 , ByVal loginUser As String _
                                 , Optional ByVal returnBy As String = "", Optional ByVal returnDte As Date = #12:00:00 AM# _
                                 , Optional ByVal approveBy As String = "", Optional ByVal approveDte As Date = #12:00:00 AM# _
                                 , Optional ByVal receiveBy As String = "", Optional ByVal receiveDte As Date = #12:00:00 AM# _
                                 ) As String

    ''' <summary>
    ''' Update the Issue Items base on the Mode either Insert, Update or Delete transaction;
    ''' 14Feb09 - KG;
    ''' </summary>
    ''' <param name="requestDetails"></param>
    ''' <param name="issueItemList"></param>
    ''' <returns>error message (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function UpdateIssueItem(ByVal requestDetails As RequestDetails _
                             , ByVal issueItemList As List(Of IssueItemDetails) _
                             ) As String

    ''' <summary>
    ''' Get the Issue list base on parameters;
    ''' when requestID is with value, ignore other parameters and get only issue items relates to the single Request Reference
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="fromDte"></param>
    ''' <param name="toDte"></param>
    ''' <param name="fromStockItemID"></param>
    ''' <param name="toStockItemID"></param>
    ''' <param name="requestID">overwrite other parameter and return value for this request only</param>
    ''' <returns>Issue DataSet</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetIssueList(ByVal storeID As String _
                          , Optional ByVal fromDte As Date = #12:00:00 AM# _
                          , Optional ByVal toDte As Date = #12:00:00 AM# _
                          , Optional ByVal fromStockItemID As String = "" _
                          , Optional ByVal toStockItemID As String = "" _
                          , Optional ByVal requestID As String = "" _
                          , Optional ByVal consumerID As String = "" _
                          ) As List(Of IssueList)

#End Region

#Region " Adjustment "
    ''' <summary>
    ''' Add New Adjust and its Item Details also to stock transaction;
    ''' 20Feb09 - KG;
    ''' </summary>
    ''' <param name="adjustDetails"></param>
    ''' <param name="adjustItemDetails"></param>
    ''' <returns>Error Message (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function AddAdjust(ByVal adjustDetails As AdjustDetails _
                       , ByVal adjustItemDetails As List(Of AdjustItemDetails) _
                       ) As String

    ''' <summary>
    ''' Get Adjust based on type(AI or AO) n status, all adjust is created as "C" only;
    ''' 20Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="type">AI = Inwards, AO = Outwards</param>
    ''' <param name="status"></param>
    ''' <returns>Adjusts DataSet Collection</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetAdjust(ByVal storeID As String _
                       , ByVal type As String _
                       , Optional ByVal status As String = "C" _
                       ) As List(Of AdjustDetails)

    ''' <summary>
    ''' Get all Adjust items for a single Adjust Reference;
    ''' 20Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="adjustID"></param>
    ''' <param name="type">AI = Inwards, AO = Outwards</param>
    ''' <returns>Adjust Items DataSet Collection</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetAdjustItem(ByVal storeID As String _
                           , ByVal adjustID As String _
                           , ByVal type As String _
                           ) As List(Of AdjustItemDetails)

    ''' <summary>
    ''' Get Adjust by search criteria list;
    ''' 09 Mar 12 - Christina;
    ''' </summary>
    ''' <param name="adjustDetails"></param>
    ''' <returns>Adjust DataSet Collection</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetAdjustBySearch(ByVal adjustDetails As AdjustDetails _
                                ) As List(Of AdjustDetails)

    ''' <summary>
    ''' Update to Adjust n AdjustItem Details, based on the adjustitem's mode to either Delete, Insert or Update;
    ''' 20Feb09 - KG;
    ''' </summary>
    ''' <param name="adjustDetails"></param>
    ''' <param name="adjustItemDetails"></param>
    ''' <returns>Error Message (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function UpdateAdjust(ByVal adjustDetails As AdjustDetails _
                          , ByVal adjustItemDetails As List(Of AdjustItemDetails) _
                          ) As String

    ''' <summary>
    ''' Delete Adjust and all its Adjust Items n Transaction details;
    ''' 20Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="adjustType">Inward or Outward</param>
    ''' <param name="adjustID"></param>
    ''' <param name="loginUser"></param>
    ''' <returns>error message (if any)</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function DeleteAdjust(ByVal storeID As String _
                          , ByVal adjustID As String _
                          , ByVal adjustType As String _
                          , ByVal originalDte As Date _
                          , ByVal loginUser As String _
                          ) As String

    ''' <summary>
    ''' Get the Adjust list base on parameters;
    ''' when adjustID is with value, ignore other parameters and get only adjust items relates to the single Adjust Reference
    ''' 19Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="fromDte"></param>
    ''' <param name="toDte"></param>
    ''' <param name="adjustID">overwrite other parameter and return value for this adjust only</param>
    ''' <returns>Adjust DataSet</returns>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetAdjustList(ByVal storeID As String _
                           , ByVal type As String _
                           , Optional ByVal fromDte As Date = #12:00:00 AM# _
                           , Optional ByVal toDte As Date = #12:00:00 AM# _
                           , Optional ByVal adjustID As String = "" _
                           ) As List(Of AdjustList)

#End Region

#End Region

#Region " Audit Trail Rport "
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetAuditTrailStockItem(ByVal storeId As String, ByVal dateFrom As DateTime, ByVal dateTo As DateTime, ByVal auditType As String, ByVal itemStatus As Boolean, ByVal orderBy As String) As List(Of AStockItemDetails)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetAuditTrailOrder(ByVal storeId As String, ByVal dateFrom As DateTime, ByVal dateTo As DateTime, ByVal auditType As String, ByVal orderBy As String) As List(Of AOrderDetails)

    <OperationContract()> _
        <FaultContract(GetType(ServiceFault))> _
    Function GetAuditTrailStockTransaction(ByVal storeId As String, ByVal dateFrom As DateTime, ByVal dateTo As DateTime, ByVal auditType As String, ByVal transType As String, ByVal orderBy As String) As List(Of AStockTransactionDetails)

    <OperationContract()> _
        <FaultContract(GetType(ServiceFault))> _
    Function GetAuditTrailResidue(ByVal storeID As String, ByVal month As Integer, ByVal year As Integer) As List(Of AResidueDetails)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetAuditTrailACommon(ByVal storeID As String, ByVal dateFrom As DateTime, ByVal dateTo As DateTime) As List(Of ACommon)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetAuditTrailAConsumer(ByVal storeID As String, ByVal dateFrom As DateTime, ByVal dateTo As DateTime) As List(Of AConsumer)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetAuditTrailAEquipment(ByVal storeID As String, ByVal dateFrom As DateTime, ByVal dateTo As DateTime) As List(Of AEquipment)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetAuditTrailAStore(ByVal storeID As String, ByVal dateFrom As DateTime, ByVal dateTo As DateTime) As List(Of AStore)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetAuditTrailASupplier(ByVal storeID As String, ByVal dateFrom As DateTime, ByVal dateTo As DateTime) As List(Of ASupplier)

    <OperationContract()> _
 <FaultContract(GetType(ServiceFault))> _
    Function GetAuditTrailAUserRole(ByVal storeID As String, ByVal dateFrom As DateTime, ByVal dateTo As DateTime) As List(Of AUserRole)

#End Region

#Region " Management Report "
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetRackItemBalance(ByVal storeId As String, ByVal rackLocationFrom As String, ByVal rackLocationTo As String) As List(Of RackItemBalanceDetails)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetStockCode(ByVal storeId As String) As List(Of String)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetRackLocation(ByVal storeId As String) As List(Of String)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetTransactionList(ByVal storeId As String, ByVal stockCodeFrom As String, ByVal stockCodeTo As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime) As List(Of TransactionListDetails)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetMR001GetRackItemBalance(ByVal storeId As String, ByVal rackLocationFrom As String, ByVal rackLocationTo As String) As List(Of MR001GetRackItemBalanceDetails)

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetMR002GetTransactionList(ByVal storeId As String, ByVal stockCodeFrom As String, ByVal stockCodeTo As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime, ByVal directIssue As String, ByVal equipmentID As String) As List(Of MR002GetTransactionListDetails)
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetMR003IssueDocumentDetails(ByVal storeId As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime, ByVal directIssue As String) As List(Of MR003IssueDocumentDetails)
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetMR004StockReviewList(ByVal storeId As String, ByVal stockCodeFrom As String, ByVal stockCodeTo As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime) As List(Of MR004StockReviewListDetails)
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetMR005StockReturn(ByVal storeId As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime) As List(Of MR005StockReturnDetails)
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetMR006StockReturnCheckListAdjust(ByVal storeId As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime) As List(Of MR006StockReturnCheckListAdjustDetails)
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetMR006StockReturnCheckListIssue(ByVal storeId As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime) As List(Of MR006StockReturnCheckListIssueDetails)
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetMR006StockReturnCheckListReceive(ByVal storeId As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime) As List(Of MR006StockReturnCheckListReceiveDetails)
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetMR006StockReturnCheckListStockItem(ByVal storeId As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime, ByVal excludeZero As Boolean) As List(Of MR006StockReturnCheckListStockItemDetails)
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetMR007PeriodIssues(ByVal storeId As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime, ByVal consumerID As String, ByVal directIssue As String) As List(Of MR007PeriodIssuesDetails)
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetMR008QuantityIssueSummary(ByVal storeId As String, ByVal stockCodeFrom As String, ByVal stockCodeTo As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime) As List(Of MR008QuantityIssueSummaryDetails)
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetMR009ReorderStockItemList(ByVal storeId As String, ByVal stockCodeFrom As String, ByVal stockCodeTo As String) As List(Of MR009ReorderStockItemListDetails)
    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetMR010StockAdjustmentEntries(ByVal storeId As String, ByVal stockCodeFrom As String, ByVal stockCodeTo As String, ByVal transDateFrom As DateTime, ByVal transDateTo As DateTime) As List(Of MR010StockAdjustmentEntriesDetails)

#End Region

#Region " Ad Hoc Report "

    <OperationContract()> _
    <FaultContract(GetType(ServiceFault))> _
    Function GetAdHocReport(ByVal AdHocReport As AdHocReportDetails, ByRef returnMessage As String) As List(Of AdHocReportDetails)

#End Region

    <OperationContract()> _
  <FaultContract(GetType(ServiceFault))> _
    Function AddEmailContent(ByVal emailContent As EmailContent) As String




    <OperationContract()> _
<FaultContract(GetType(ServiceFault))> _
    Function GetEmailContent(ByVal storeID As String, ByVal emailFormat As String) As List(Of String)

End Interface
