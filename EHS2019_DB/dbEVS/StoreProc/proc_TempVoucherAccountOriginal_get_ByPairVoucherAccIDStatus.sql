 IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccountOriginal_get_ByPairVoucherAccIDStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccountOriginal_get_ByPairVoucherAccIDStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Paul Yip
-- Create date: 14 Oct 2010
-- Description: Retrieve TempVoucherAccount (Original) By Pair Voucher Account ID & Status
-- =============================================

CREATE PROCEDURE [dbo].[proc_TempVoucherAccountOriginal_get_ByPairVoucherAccIDStatus]
	@Voucher_Acc_ID	char(15),
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

	SELECT
		TVA.Voucher_Acc_ID,
		TVA.Scheme_Code,
		--TVA.Voucher_Used,
		--TVA.Total_Voucher_Amt_Used,
		TVA.Validated_Acc_ID,
		TVA.Record_Status,
		TVA.Account_Purpose,
		TVA.Confirm_Dtm,
		TVA.Last_Fail_Validate_Dtm,
		TVA.Create_Dtm,
		TVA.Create_By,
		TVA.Update_Dtm,
		TVA.Update_By,
		TVA.DataEntry_By,
		TVA.TSMP,
		TVA.Create_by_BO
	FROM [dbo].[TempVoucherAccount] TVA, [dbo].[TempVoucherAccount] Paired
			
	WHERE 
		Paired.Voucher_Acc_ID = @Voucher_Acc_ID AND Paired.Validated_Acc_ID = TVA.Validated_Acc_ID
		AND TVA.Voucher_Acc_ID <> Paired.Voucher_Acc_ID AND TVA.Record_Status = @Record_Status
		AND Paired.Original_Amend_Acc_ID = TVA.Voucher_Acc_ID

END

GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccountOriginal_get_ByPairVoucherAccIDStatus] TO HCVU
GO
