IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempPersonalInformation_upd_Validating]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempPersonalInformation_upd_Validating]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kathy LEE
-- Create date: 23 Oct 2008
-- Description:	Make as ImmD Validating.
--				Update TempPersonalInformation, Mark Validating = 'Y' and Check_dtm = getdate()
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	29 Oct 2009
-- Description:		Add Handling for Stat for Manual Mark Validating
-- =============================================
CREATE PROCEDURE [dbo].[proc_TempPersonalInformation_upd_Validating]
	@Voucher_Acc_ID	char(15),
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
IF (SELECT TSMP FROM TempPersonalInformation
		WHERE Voucher_Acc_ID = @Voucher_Acc_ID) != @tsmp
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

	UPDATE [dbo].[TempPersonalInformation]
	SET
		Validating = 'Y',
		Check_Dtm = GetDate(),
		Update_by = @update_by,
		update_dtm = getdate()
	WHERE 
		Voucher_Acc_ID = @Voucher_Acc_ID
		
	
	-- Statistic handling
	
	INSERT INTO [dbo].[TempVoucherAccManualSubLOG]
	(
		[System_Dtm], [Voucher_Acc_ID], [Encrypt_Field1], [Doc_Code], [Record_Status]
	)
	SELECT
		GetDate(),
		@Voucher_Acc_ID,
		[Encrypt_Field1], 
		[Doc_Code],
		'A'
	FROM [TempPersonalInformation]
	WHERE [Voucher_Acc_ID] = @Voucher_Acc_ID
		
	--
	
END

GO

GRANT EXECUTE ON [dbo].[proc_TempPersonalInformation_upd_Validating] TO HCVU
GO
