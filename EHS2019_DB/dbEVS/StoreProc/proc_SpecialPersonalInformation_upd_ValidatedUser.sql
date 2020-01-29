IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Proc_SpecialPersonalInformation_upd_ValidatedUser]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[Proc_SpecialPersonalInformation_upd_ValidatedUser]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Vincent
-- Modified date: 	25 FEB 2010
-- Description:		Update Validation Status for Voucher Account Manual Match LOG
-- =============================================
-- =============================================
-- Author:		Dedrick Ng
-- Create date: 27 Sep 2009
-- Description:	Update TempPersonalInformation, Mark Validating = 'N'
-- =============================================
CREATE PROCEDURE [dbo].[Proc_SpecialPersonalInformation_upd_ValidatedUser]
	@Special_Acc_ID	char(15),
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

	UPDATE [dbo].[SpecialPersonalInformation]
	SET
		Validating = 'N',
		Check_Dtm = GetDate(),
		Update_dtm = GetDate(),
		Update_By = @User_ID
	WHERE 
		Special_Acc_ID = @Special_Acc_ID

	-- Statistic handling
	exec proc_TempVoucherAccManualMatchLOG_add @Special_Acc_ID, 'Y', @return_dtm
	--
	
END

GO

GRANT EXECUTE ON [dbo].[Proc_SpecialPersonalInformation_upd_ValidatedUser] TO HCVU
GO
