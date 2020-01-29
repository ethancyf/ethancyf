IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccount_getByVoucherAccIDStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccount_getByVoucherAccIDStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:	Paul Yip
-- Modified date: 20 Jul 2010
-- Description:	Add Create_By_BO
-- ===========================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	11 Mar 2010
-- Description:		1. Grant right to [HCSP]
--					2. Retrieve 'Original_Amend_Acc_ID'
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 26 Aug 2008
-- Description:	Retrieve TempVoucherAccount By Voucher Account ID & Status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Dedrick Ng
-- Modified date: 29 Sep 2009
-- Description:	Remove obsolete fields
-- =============================================
CREATE PROCEDURE [dbo].[proc_TempVoucherAccount_getByVoucherAccIDStatus]
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
		Voucher_Acc_ID,
		Scheme_Code,
		--Voucher_Used,
		--Total_Voucher_Amt_Used,
		Validated_Acc_ID,
		Record_Status,
		Account_Purpose,
		Confirm_Dtm,
		Last_Fail_Validate_Dtm,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By,
		DataEntry_By,
		Original_Amend_Acc_ID,
		TSMP,
		Create_by_BO	
	FROM [dbo].[TempVoucherAccount]
	WHERE 
		Voucher_Acc_ID = @Voucher_Acc_ID AND Record_Status = @Record_Status
	
END

GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_getByVoucherAccIDStatus] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_getByVoucherAccIDStatus] TO HCVU
GO
