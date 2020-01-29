IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Proc_TempPersonalInformation_upd_ValidatedUser]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[Proc_TempPersonalInformation_upd_ValidatedUser]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	11 Mar 2010
-- Description:		Grant right to [HCSP]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Vincent
-- Modified date: 	25 FEB 2010
-- Description:		Update Validation Status for Voucher Account Manual Match LOG
-- =============================================
-- =============================================  
-- Author:  Pak Ho LEE  
-- Create date: 26 Aug 2008  
-- Description: Update TempPersonalInformation, Mark Validating = 'N'  
-- =============================================   
CREATE PROCEDURE [dbo].[Proc_TempPersonalInformation_upd_ValidatedUser]  
 @Voucher_Acc_ID char(15),  
 @User_ID varchar(20)  
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
-- =============================================  
-- Initialization  
-- =============================================  
	SET @return_dtm = GETDATE()
-- =============================================  
-- Return results  
-- =============================================  
  
 UPDATE [dbo].[TempPersonalInformation]  
 SET  
  Validating = 'N',  
  Check_Dtm = GetDate(),  
  Update_dtm = GetDate(),  
  Update_By = @User_ID  
 WHERE   
  Voucher_Acc_ID = @Voucher_Acc_ID
  
	-- Statistic handling
	exec proc_TempVoucherAccManualMatchLOG_add @Voucher_Acc_ID, 'Y', @return_dtm
	--
   
END
GO

GRANT EXECUTE ON [dbo].[Proc_TempPersonalInformation_upd_ValidatedUser] TO HCSP
GO

GRANT EXECUTE ON [dbo].[Proc_TempPersonalInformation_upd_ValidatedUser] TO HCVU
GO
