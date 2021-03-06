USE [ICS_IA]
GO
/****** Object:  StoredProcedure [dbo].[spAR002GetUserUnsuccessfulLogin]    Script Date: 7/5/2020 6:57:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================        
-- Author:  <Author,Christina>        
-- Create date: <27 Aug 2013>        
-- Description: <User Activity Report>        
-- =============================================        
ALTER PROCEDURE [dbo].[spAR002GetUserUnsuccessfulLogin]        
 @strStoreId   varchar(10)        
 ,@strUserId  varchar(50)         
 ,@dateTransFrom     smalldatetime       
 ,@dateTransTo       smalldatetime       
AS        
BEGIN        
 SET NOCOUNT ON;        
--exec [spAR002GetUserUnsuccessfulLogin]  'RO', 'S1234567D', '2013-08-27', '2013-09-25'  
--exec [spAR002GetUserUnsuccessfulLogin] 'RO', null, '2012-08-27', '2013-09-25'  
--DECLARE @strStoreId   varchar(4)        
--,@strUserName varchar(50)        
--,@dateTransFrom     smalldatetime        
--,@dateTransTo       smalldatetime        
--SET @strStoreId='TSIP'        
-- set @strUserName='S1234567D'        
--SET @dateTransFrom='2011-08-27'        
--SET @dateTransTo=GETDATE()        
  SELECT         
   a.AUserStoreID        
   -- Mask NRIC Start           
   --,a.AUserID 
   ,CASE WHEN a.AUserID IS NULL THEN '' collate Latin1_General_CI_AI ELSE CONCAT(Substring(a.AUserID,1,1),(CONCAT('****' collate Latin1_General_CI_AI,Substring(a.AUserID,6,4)))) END AS AUserID
   -- Mask NRIC End          
   ,a.AUserLoginDte        
   ,a.AUserIP        
   ,v.VUserProfileName        
  FROM         
   AUser a        
  INNER JOIN         
   vUserProfile v        
  ON         
   a.AUserID = v.VUserProfileUserID    
   COLLATE Latin1_General_CI_AI              
  WHERE        
    a.AUserStoreID = @strStoreId      
    and (ISNULL(@strUserId,'') = '' OR v.VUserProfileUserId  = @strUserId)        
    and (UnsuccessfulLogin = 1) -- generate report with unsuccessful login of ics user  
    and (NonIcsUser = 0 or NonIcsUser is null) -- only valid ICS w/unsuccessful login user will be listed  
    and (InactiveUser = 0 or InactiveUser is null)   
    and DATEDIFF(day, a.AUserLoginDte, isnull(@dateTransFrom, CONVERT(smalldatetime, '01/01/1900', 103))) <= 0        
    and DATEDIFF(day, a.AUserLoginDte, isnull(@dateTransTo, CONVERT(smalldatetime, '06/06/2079', 103))) >= 0        
END 

