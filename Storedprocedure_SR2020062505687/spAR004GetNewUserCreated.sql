USE [ICS_IA]
GO
/****** Object:  StoredProcedure [dbo].[spAR004GetNewUserCreated]    Script Date: 7/9/2020 1:59:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================              
-- Author:  <Author,Christina>              
-- Create date: <27 Aug 2013>              
-- Description: <User Activity Report>              
-- =============================================              
ALTER PROCEDURE [dbo].[spAR004GetNewUserCreated]              
 @strStoreId   varchar(10)              
 --,@strUserId  varchar(50)               
 ,@dateTransFrom     smalldatetime             
 ,@dateTransTo       smalldatetime             
AS              
BEGIN              
 SET NOCOUNT ON;              
              
--exec [spAR004GetNewUserCreated]  'RO', '2011-08-27', '2013-09-25'        
-- select * from UserRole        
-- select * from vUserProfile        
--DECLARE @strStoreId   varchar(4)              
--,@dateTransFrom     smalldatetime              
--,@dateTransTo       smalldatetime              
--SET @strStoreId='RO'              
-- set @strUserName=null              
--SET @dateTransFrom='2011-08-27'              
--SET @dateTransTo=GETDATE()              
        
  SELECT  
  --Mask NRIC Start             
   --v.VUserProfileUserID 
   CASE WHEN v.VUserProfileUserID IS NULL THEN '' ELSE CONCAT(Substring(v.VUserProfileUserID,1,1) collate Latin1_General_CI_AI,(CONCAT('****' collate Latin1_General_CI_AI,Substring(v.VUserProfileUserID,6,4) collate Latin1_General_CI_AI))) END AS VUserProfileUserID
   --Mask NRIC End       
   ,v.VUserProfileName        
   ,UserRoleCreateDte 
   --Mask NRIC Start        
   --,UserRoleCreateUserID 
   ,vUser.VUserProfileName as UserRoleCreateUserID
   --Mask NRIC End       
   ,r.UserRoleStoreID         
   ,r.UserRoleCode        
   ,r.UserRoleDescription        
   --,r.UserRoleStatus         
   ,(case when (r.UserRoleStatus='O') then ('Active')        
    else ('Inactive') end) as UserRoleStatus       
  FROM               
   UserRole r              
  INNER JOIN               
   vUserProfile v              
  ON               
   r.UserRoleUserID = v.VUserProfileUserID         
   COLLATE Latin1_General_CI_AI 
   
   -- NRIC mask Start
    LEFT JOIN vUserProfile vUser on r.UserRoleCreateUserID = vUser.VUserProfileUserID COLLATE Latin1_General_CI_AI
   -- NRIC mask End
                 
  WHERE              
    r.UserRoleStoreID = @strStoreId            
    --and (ISNULL(@strUserId,'') = '' OR v.VUserProfileUserID = @strUserId)              
    --and (IsUserDeleted= 0 or IsUserDeleted is null)         
    and DATEDIFF(day, r.UserRoleCreateDte, isnull(@dateTransFrom, CONVERT(smalldatetime, '01/01/1900', 103))) <= 0              
    and DATEDIFF(day, r.UserRoleCreateDte, isnull(@dateTransTo, CONVERT(smalldatetime, '06/06/2079', 103))) >= 0              
  ORDER BY UserRoleCreateDte,VUserProfileName                 
END 

