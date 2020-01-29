IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PersonalInfoAmendHistory_upd]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PersonalInfoAmendHistory_upd]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	11 Feb 2010
-- Description:		1. Also grant right to HCSP
--					2. Add Temp_Voucher_Acc_ID in where clause
-- =============================================
-- =============================================
-- Author:		Tommy Cheung
-- Create date: 25 August 2008
-- Description:	Update Record_Status and Submit_to_verify field
--				on PersonalInfoAmendHistory table
-- =============================================

CREATE PROCEDURE [dbo].[proc_PersonalInfoAmendHistory_upd]
	@Voucher_Acc_ID char(15),
	@Update_by varchar(20),
	@Record_Status char(1),
	@SubmitToVerify char(1),
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

-- To cancel amendment
IF @Record_Status = 'E'
BEGIN
    Update PersonalInfoAmendHistory
	set Record_Status = @Record_Status,
		SubmitToVerify = @SubmitToVerify,
		Cancel_by = @Update_by,
		Cancel_dtm = getdate()
	where Voucher_Acc_ID = @Voucher_Acc_ID 
		and Temp_Voucher_Acc_ID = @Temp_Voucher_Acc_ID
		and	SubmitToVerify = 'Y'
END
ELSE
BEGIN
	Update PersonalInfoAmendHistory
	set Record_Status = @Record_Status,
		SubmitToVerify = @SubmitToVerify,
		Update_by = @Update_by
	where Voucher_Acc_ID = @Voucher_Acc_ID 
		and Temp_Voucher_Acc_ID = @Temp_Voucher_Acc_ID
		and	SubmitToVerify = 'Y'
END		
END
GO

GRANT EXECUTE ON [dbo].[proc_PersonalInfoAmendHistory_upd] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_PersonalInfoAmendHistory_upd] TO HCVU
GO
