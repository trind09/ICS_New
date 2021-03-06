USE [ICS_IA]
GO
/****** Object:  StoredProcedure [dbo].[spAR002Order]    Script Date: 7/13/2020 4:00:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Liu Guo Feng
-- Create date: 4 Jan 2009
-- Description:	Get report datasource for AR002 Order.
-- =============================================
-- Change History
-- Date:		Author:	RefID:			Description:
-- ---------	-------	-------			----------------
-- 20Oct2010	Jianfa	ERSS 930115645	Include Ordering By
-- =============================================
ALTER PROCEDURE [dbo].[spAR002Order] 
	@storeID varchar(4)
	,@dateFrom smalldatetime
	,@dateTo smalldatetime
	,@auditType varchar(6)
	,@orderBy	varchar(20)
AS
BEGIN
	SET NOCOUNT ON;

	If @orderBy = 'ENTRYDATE'
		Begin

			SELECT a.AOrderStoreID
			,a.AOrderItemStockItemID	
			,b.StockItemDescription AS AStockItemDescription
			,a.AOrderID
			,a.AOrderDte
			,a.AOrderSupplierID
			,a.AOrderItemQty	
			,b.StockItemUOM AS AStockItemUOM
			,(case when a.AOrderItemQty <> 0 
				then (a.AOrderItemTotalCost / a.AOrderItemQty) 
				else 0 end) as AOrderItemUnitCost
			,a.AOrderItemTotalCost
			,a.AOrderCreateDte
			-- Mask NRIC Start
			--,ISNULL(v.VUserProfileName, a.AOrderUserID) AS AOrderUserID
			,v.VUserProfileName AS AOrderUserID
			-- Mask NRIC End
			,a.AOrderAuditType
			from AOrder a 
			left join StockItem b 
			on a.AOrderItemStockItemID = b.StockItemID AND a.AOrderStoreID = b.StockItemStoreID 			
			LEFT OUTER JOIN VUserProfile v 
			ON v.VUserProfileUserID = a.AOrderUserID
			COLLATE Latin1_General_CI_AS
			where a.AOrderStoreID = @storeID
			and (a.AOrderAuditType = @auditType 
				or isnull(@auditType, '')='')
			and DATEDIFF(day, a.AOrderCreateDte, isnull(@dateFrom, CONVERT(smalldatetime, '01/01/1900', 103))) <= 0
			and DATEDIFF(day, a.AOrderCreateDte, isnull(@dateTo, CONVERT(smalldatetime, '06/06/2079', 103))) >= 0
			and a.AOrderUserID <> 'SYSTEM'
			order by 	a.AOrderCreateDte, a.AOrderID, a.AOrderItemStockItemID
		End

	If @orderBy = 'ORDERDATE'
		Begin

			SELECT a.AOrderStoreID
				,a.AOrderItemStockItemID	
				,b.StockItemDescription AS AStockItemDescription
				,a.AOrderID
				,a.AOrderDte
				,a.AOrderSupplierID
				,a.AOrderItemQty	
				,b.StockItemUOM AS AStockItemUOM
				,(case when a.AOrderItemQty <> 0 
					then (a.AOrderItemTotalCost / a.AOrderItemQty) 
					else 0 end) as AOrderItemUnitCost
				,a.AOrderItemTotalCost
				,a.AOrderCreateDte
				-- Mask NRIC Start
				--,ISNULL(v.VUserProfileName, a.AOrderUserID) AS AOrderUserID
				,v.VUserProfileName AS AOrderUserID
				-- Mask NRIC End
				,a.AOrderAuditType
				from AOrder a 
				left join StockItem b 
				on a.AOrderItemStockItemID = b.StockItemID AND a.AOrderStoreID = b.StockItemStoreID 			
				LEFT OUTER JOIN VUserProfile v 
				ON v.VUserProfileUserID = a.AOrderUserID
				COLLATE Latin1_General_CI_AS
				where a.AOrderStoreID = @storeID
				and (a.AOrderAuditType = @auditType 
					or isnull(@auditType, '')='')
				and DATEDIFF(day, a.AOrderDte, isnull(@dateFrom, CONVERT(smalldatetime, '01/01/1900', 103))) <= 0
				and DATEDIFF(day, a.AOrderDte, isnull(@dateTo, CONVERT(smalldatetime, '06/06/2079', 103))) >= 0
				and a.AOrderUserID <> 'SYSTEM'
				order by 	a.AOrderDte, a.AOrderID, a.AOrderItemStockItemID

		End
END

