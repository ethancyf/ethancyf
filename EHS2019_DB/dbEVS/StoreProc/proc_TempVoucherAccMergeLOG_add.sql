IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccMergeLOG_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccMergeLOG_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 26 Aug 2008
-- Description:	Add TempVoucherAccMergeLOG
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_TempVoucherAccMergeLOG_add]
	@Voucher_Acc_ID char(15),
	@Temp_Voucher_Acc_ID char(15),
	@Scheme_Code char(10)
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
	INSERT INTO [dbo].[TempVoucherAccMergeLOG]
	(
		System_Dtm,
		Voucher_Acc_ID,
		Temp_Voucher_Acc_ID,
		Scheme_Code
	)
	VALUES
	(
		GetDate(),
		@Voucher_Acc_ID,
		@Temp_Voucher_Acc_ID,
		@Scheme_Code
	)
	
END

GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccMergeLOG_add] TO HCVU
GO
