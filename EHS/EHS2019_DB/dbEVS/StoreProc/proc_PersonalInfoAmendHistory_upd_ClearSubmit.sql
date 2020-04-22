IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PersonalInfoAmendHistory_upd_ClearSubmit]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PersonalInfoAmendHistory_upd_ClearSubmit]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	11 Feb 2010
-- Description:		1. Add Temp_Voucher_acc_ID in where clause
--					2. Grant right to [HCSP]
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 26 Aug 2008
-- Description:	Update [PersonalInforAmendHistory] Clear SubmitToVerify, Set Record_Status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Dedrick Ng
-- Modified date: 12 Oct 2009
-- Description:	Add new fields and remove obsolete fields
-- =============================================
CREATE PROCEDURE [dbo].[proc_PersonalInfoAmendHistory_upd_ClearSubmit]
	@Voucher_Acc_ID	char(15),
	@Record_Status char(1),
	@Doc_Code	char(20),
	@Temp_Voucher_Acc_ID char(15)
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

	UPDATE [dbo].[PersonalInfoAmendHistory]
	SET
		--SubmitToVerify = 'N',
		Record_Status = @Record_Status 
	WHERE 
		Voucher_Acc_ID = @Voucher_Acc_ID AND Doc_Code = @Doc_Code AND SubmitToVerify = 'Y' AND Temp_Voucher_Acc_ID = @Temp_Voucher_Acc_ID

END

GO

GRANT EXECUTE ON [dbo].[proc_PersonalInfoAmendHistory_upd_ClearSubmit] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_PersonalInfoAmendHistory_upd_ClearSubmit] TO HCVU
GO
