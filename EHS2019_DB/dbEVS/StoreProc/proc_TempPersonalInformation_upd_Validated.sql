IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempPersonalInformation_upd_Validated]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempPersonalInformation_upd_Validated]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 26 Aug 2008
-- Description:	Update TempPersonalInformation, Mark Validating = 'N'
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_TempPersonalInformation_upd_Validated]
	@Voucher_Acc_ID	char(15)
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

	UPDATE [dbo].[TempPersonalInformation]
	SET
		Validating = 'N',
		Check_Dtm = GetDate()
	WHERE 
		Voucher_Acc_ID = @Voucher_Acc_ID
	
END

GO

GRANT EXECUTE ON [dbo].[proc_TempPersonalInformation_upd_Validated] TO HCVU
GO
