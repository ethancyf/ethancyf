IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SpecialPersonalInfoVoucherAcct_upd_ValidatingStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SpecialPersonalInfoVoucherAcct_upd_ValidatingStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date: 	12 Apr
-- Description:		1. Only Mark Validating = 'N' in TempVoucherAccount
--					   since we only want to keep the status same as before marking for validating
--					2. Mark 'C' (Cancel) in TempVoucherAccManualMatchLOG
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Vincent
-- Modified date: 	25 FEB 2010
-- Description:		Update Validation Status for Voucher Account Manual Match LOG
-- =============================================
-- =============================================  
-- Author:  Kathy LEE  
-- Create date: 23 Oct 2008  
-- Description: Release For Rectification  
--    1. Mark Validating = 'N' in (validation fail) in TempVoucherAccount  
--    2. Record_Status = 'I' (Ivalid) in TempPersonalInformation  
--    3. Update Last_Fail_Validate_dtm in TempPersonalInformation  
-- =============================================  
CREATE PROCEDURE [dbo].[proc_SpecialPersonalInfoVoucherAcct_upd_ValidatingStatus]  
 @Special_Acc_ID char(15),  
 @tsmp timestamp,  
 @pitsmp timestamp,  
 @update_by varchar(20)  
AS  
BEGIN  
 SET NOCOUNT ON;  
-- =============================================  
-- Declaration  
-- =============================================  
	DECLARE @return_dtm DATETIME
	
-- =============================================  
-- Validation   
-- =============================================  
IF (((SELECT TSMP FROM SpecialPersonalInformation  
  WHERE Special_Acc_ID = @Special_Acc_ID) != @pitsmp) and  
 ((SELECT TSMP FROM SpecialAccount  
  WHERE Special_Acc_ID = @Special_Acc_ID) != @tsmp))  
    
 BEGIN  
  RAISERROR('00011', 16, 1)  
  return @@error  
 END  
-- =============================================  
-- Initialization  
-- =============================================  
	SET @return_dtm = GETDATE()
	
-- =============================================  
-- Return results  
-- =============================================  
  
 UPDATE [dbo].[SpecialPersonalInformation]  
 SET  
  Validating = 'N',  
  update_by = @update_by,  
  update_dtm = getdate()  
  -- Check_Dtm = GetDate()  
 WHERE   
  Special_Acc_ID = @Special_Acc_ID  
    
 /*UPDATE [dbo].[SpecialAccount]  
 SET  
  Record_Status = 'I',  
  Last_Fail_Validate_Dtm = getdate(),  
  update_by = @update_by,  
  update_dtm = getdate()  
 WHERE  
  Special_Acc_ID = @Special_Acc_ID  */


	-- Statistic handling
	exec proc_TempVoucherAccManualMatchLOG_add @Special_Acc_ID, 'C', @return_dtm
	--

END  
  
GO

GRANT EXECUTE ON [dbo].[proc_SpecialPersonalInfoVoucherAcct_upd_ValidatingStatus] TO HCVU
GO
