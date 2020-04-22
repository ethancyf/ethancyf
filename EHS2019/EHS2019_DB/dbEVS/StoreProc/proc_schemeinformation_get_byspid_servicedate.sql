IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_schemeinformation_get_byspid_servicedate]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_schemeinformation_get_byspid_servicedate]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE15-004
-- Modified by:		Lawrence TSANG
-- Modified date:	21 July 2015
-- Description:		SProc is replaced by [proc_SchemeInformation_get_bySPID]
-- =============================================
-- =============================================  
-- Author:  Derek LEUNG  
-- Create date: 20 August 2010  
-- Description: Retrieve the Scheme Information from Table SchemeInformation  
--    Copy from [proc_SchemeInformation_WithServiceDate_get_bySPID_ServiceDate]  
--              But do not check effective date and return all  
-- =============================================  
  
/*  
CREATE PROCEDURE [dbo].[proc_SchemeInformation_get_bySPID_ServiceDate]  
 @sp_id   char(8),  
 @service_date datetime  
AS  
BEGIN  
  
 SET NOCOUNT ON;  
  
-- =============================================  
-- Declaration  
-- =============================================  
-- =============================================  
-- Validation   
-- =============================================  
-- =============================================  
-- Initialization  
-- =============================================  
-- =============================================  
-- Return results  
-- =============================================  
  
/***********************************************   
 [Record_Status] reference:  
  Active      A    
  Suspended     W    
  DelistedVoluntary   V    
  DelistedInvoluntary   I    
  ActivePendingSuspend  S    
  ActivePendingDelist   D    
  SuspendedPendingDelist  X    
  SuspendedPendingReactivate Y    
 **********************************************/  
  
 SELECT  S.SP_ID,  
    S.Scheme_Code,  
    CASE S.Record_Status  
     WHEN 'A' THEN  
      ISNULL(M.Upd_Type, 'A')  
     WHEN 'S' THEN  
      CASE ISNUll(M.Upd_Type, '')  
       WHEN 'R' THEN  
        'Y'  
       WHEN 'D' THEN  
        'X'  
       ELSE 'W'  
      END  
     WHEN 'D' THEN  
      ISNUll(S.Delist_Status, '')  
    END [Record_Status],  
    CASE S.Record_Status  
     WHEN 'A' THEN   
      CASE ISNUll(M.Upd_Type, '')  
       WHEN '' THEN  
        ''  
       ELSE ISNUll(M.Remark, '')  
      END  
     WHEN 'S' THEN  
      CASE ISNULL(M.Upd_Type, '')  
       WHEN 'D' THEN  
        ISNULL(M.Remark, '')  
       ELSE ISNUll(S.Remark, '')  
      END  
     WHEN 'D' THEN   
      ISNUll(S.Remark, '')  
    END [Remark],  
    S.Delist_Status,  
    S.Effective_Dtm,  
    S.Delist_Dtm,  
    S.Logo_Return_Dtm,  
    S.Create_Dtm,  
    S.Create_By,  
    S.Update_Dtm,  
    S.Update_By,  
    S.TSMP,  
    B.Display_Seq  
      
 FROM  SchemeInformation S  
     LEFT JOIN SPAccountMaintenance M  
      ON S.SP_ID = M.SP_ID  
       AND S.Scheme_Code = M.Scheme_Code  
       AND M.Record_Status = 'A'  
       AND M.SP_Practice_Display_Seq IS NULL  
          INNER JOIN SchemeBackOffice B    
      ON S.Scheme_Code = B.Scheme_Code    
      AND @service_date BETWEEN B.Effective_Dtm AND B.Expiry_Dtm    
       
 WHERE  S.SP_ID = @sp_id  
   
 ORDER BY M.System_Dtm DESC  
   
END  
  
--GO


GRANT EXECUTE ON [dbo].[proc_schemeinformation_get_byspid_servicedate] TO HCSP
--GO

GRANT EXECUTE ON [dbo].[proc_schemeinformation_get_byspid_servicedate] TO HCVU
--GO

GRANT EXECUTE ON [dbo].[proc_schemeinformation_get_byspid_servicedate] TO WSEXT
--GO
*/
