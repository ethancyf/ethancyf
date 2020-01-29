IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccManualSubLOG_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccManualSubLOG_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 29 Oct 2009
-- Description:	Add TempVoucherAccManualSubLOG
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_TempVoucherAccManualSubLOG_add]
	@System_dtm Datetime,
	@Voucher_Acc_ID char(15),
	@Encrypt_Field1 varbinary(100),
	@Doc_Code char(20),
	@Record_Status char(1)
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

	INSERT INTO [dbo].[TempVoucherAccManualSubLOG]
	(
		[System_Dtm], [Voucher_Acc_ID], [Encrypt_Field1], [Doc_Code], [Record_Status]
	) VALUES 
	(
		@System_dtm, @Voucher_Acc_ID, @Encrypt_Field1, @Doc_Code, @Record_Status
	)
	
END

GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccManualSubLOG_add] TO HCVU
GO
