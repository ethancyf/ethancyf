IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccount_get_ByPairVoucherAccIDStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccount_get_ByPairVoucherAccIDStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Paul Yip
-- Create date: 24 Jan 2011
-- Description:	Rely on 'Original_Amend_Acc_ID' to retrieve records
-- =============================================
-- =============================================
-- Author:		Paul Yip
-- Create date: 07 Oct 2010
-- Description: Retrieve 'Create_by_bo'
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 26 Aug 2008
-- Description:	Retrieve TempVoucherAccount By Pair Voucher Account ID & Status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Dedrick Ng
-- Modified date: 12 Oct 2009
-- Description:	Remove obsolete fields
-- =============================================
CREATE PROCEDURE [dbo].[proc_TempVoucherAccount_get_ByPairVoucherAccIDStatus]
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
		AND (
			(TVA.Original_Amend_Acc_ID is not null AND TVA.Original_Amend_Acc_ID <> '' AND Paired.Voucher_Acc_ID = TVA.Original_Amend_Acc_ID) 
			Or 
			(Paired.Original_Amend_Acc_ID is not null AND Paired.Original_Amend_Acc_ID <> '' AND TVA.Voucher_Acc_ID = Paired.Original_Amend_Acc_ID)
		)
END

GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_get_ByPairVoucherAccIDStatus] TO HCVU
GO
