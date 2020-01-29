IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SpecialPersonalInformation_upd_Validating]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SpecialPersonalInformation_upd_Validating]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kathy LEE
-- Create date: 23 Oct 2008
-- Description:	Make as ImmD Validating.
--				Update Specail PersonalInformation, Mark Validating = 'Y' and Check_dtm = getdate()
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_SpecialPersonalInformation_upd_Validating]
	@Special_Acc_ID	char(15),
	@tsmp	timestamp,
	@update_by varchar(20)
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
IF (SELECT TSMP FROM SpecialPersonalInformation
		WHERE special_acc_id = @Special_Acc_ID) != @tsmp
	BEGIN
		RAISERROR('00011', 16, 1)
		return @@error
	END
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	UPDATE [dbo].[SpecialPersonalInformation]
	SET
		Validating = 'Y',
		Check_Dtm = GetDate(),
		Update_by = @update_by,
		update_dtm = getdate()
	WHERE 
		special_acc_id = @Special_Acc_ID

	-- Statistic handling
	
	INSERT INTO [dbo].[TempVoucherAccManualSubLOG]
	(
		[System_Dtm], [Voucher_Acc_ID], [Encrypt_Field1], [Doc_Code], [Record_Status]
	)
	SELECT
		GetDate(),
		@Special_Acc_ID,
		[Encrypt_Field1], 
		[Doc_Code],
		'A'
	FROM [SpecialPersonalInformation]
	WHERE [Special_Acc_ID] = @Special_Acc_ID
		
	--	
END

GO

GRANT EXECUTE ON [dbo].[proc_SpecialPersonalInformation_upd_Validating] TO HCVU
GO
