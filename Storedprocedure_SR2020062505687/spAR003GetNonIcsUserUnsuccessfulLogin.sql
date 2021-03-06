USE [ICS_IA]
GO
/****** Object:  StoredProcedure [dbo].[spAR003GetNonIcsUserUnsuccessfulLogin]    Script Date: 7/5/2020 7:06:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

       
-- =============================================                
-- Author:  <Author,Christina>                
-- Create date: <27 Aug 2013>                
-- Description: <User Activity Report>                
-- =============================================                
ALTER PROCEDURE [dbo].[spAR003GetNonIcsUserUnsuccessfulLogin]                
 @strStoreId varchar(10)          
 ,@dateTransFrom     smalldatetime               
 ,@dateTransTo       smalldatetime               
AS                
BEGIN                
 SET NOCOUNT ON;                
-- exec [spAR003GetNonIcsUserUnsuccessfulLogin]  'RO', '2011-08-27', '2013-09-25'          
--select * from auser where AUserStoreID='RO' and AUserID='S1234567D'          
--select * from userRole where UserRoleStoreID='RO' and UserRoleUserID='s1234567d' and UserRoleStatus='O' -- if zero means no active          
--DECLARE @dateTransFrom     smalldatetime                
--,@dateTransTo       smalldatetime                
--SET @dateTransFrom='2011-08-27'                
--SET @dateTransTo=GETDATE()                
          
  SELECT distinct          
   a.AUserStoreID                
    -- Mask NRIC Start           
   --,a.AUserID 
   ,CASE WHEN a.AUserID IS NULL THEN '' collate Latin1_General_CI_AI ELSE CONCAT(Substring(a.AUserID,1,1),(CONCAT('****' collate Latin1_General_CI_AI,Substring(a.AUserID,6,4)))) END AS AUserID
   -- Mask NRIC End               
   ,a.AUserLoginDte                
   ,a.AUserIP                
   --,r.ChangeStatusReason          
   --,(case when (a.AUserStoreID = '' and a.NonIcsUser = 1) then ('Not a Valid ICS User')          
   ,(case when (a.NonIcsUser = 1) then ('Not a Valid ICS User')          
  else ('User is Inactive') end) as ChangeStatusReason            
   ,v.VUserProfileName                
  FROM                 
   AUser a                
  LEFT JOIN       -- to retrive including non-NEA or non-AD user          
   vUserProfile v                
  ON                 
   a.AUserID = v.VUserProfileUserID                
   COLLATE Latin1_General_CI_AI          
  left JOIN       -- to retrive non-NEA or non-AD user          
   UserRole r          
  ON                 
   a.AUserID = r.UserRoleUserID                
  WHERE                
 --(ISNULL(@strStoreId,'') = '' OR a.AUserStoreID = @strStoreId)                  
 (a.AUserStoreID = @strStoreId OR a.AUserStoreID = '')                  
 and           
 (a.NonIcsUser  = 1 -- non-ics and non-nea users who has been tag as non-ics user during login          
  or a.InactiveUser  = 1) -- or inactive ics user login attempt         
    and DATEDIFF(day, a.AUserLoginDte, isnull(@dateTransFrom, CONVERT(smalldatetime, '01/01/1900', 103))) <= 0                
    and DATEDIFF(day, a.AUserLoginDte, isnull(@dateTransTo, CONVERT(smalldatetime, '06/06/2079', 103))) >= 0                
    order by a.AUserLoginDte desc          
END 

