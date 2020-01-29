IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherAccount_get_ByVoucherAccID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherAccount_get_ByVoucherAccID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 26 Aug 2008
-- Description:	Retrieve Voucher Account By Voucher Account ID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Dedrick Ng
-- Modified date: 30 Sep 2009
-- Description:	Remove obsolete fields
-- =============================================
CREATE PROCEDURE [dbo].[proc_VoucherAccount_get_ByVoucherAccID]
	@Voucher_Acc_ID char(15)	
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
		--Total_Voucher_Amt_Used,
		Record_Status,
		Remark,
		Public_Enquiry_Status,
		Public_Enq_Status_Remark,
		Effective_Dtm,
		Terminate_Dtm,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By,
		DataEntry_By,
		TSMP
	FROM [dbo].[VoucherAccount]
	WHERE Voucher_Acc_ID = @Voucher_Acc_ID 
	
END

GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccount_get_ByVoucherAccID] TO HCVU
GO
