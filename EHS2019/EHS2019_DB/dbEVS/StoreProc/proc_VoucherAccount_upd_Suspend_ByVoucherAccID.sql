IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherAccount_upd_Suspend_ByVoucherAccID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherAccount_upd_Suspend_ByVoucherAccID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 26 Aug 2008
-- Description:	Suspend Voucher Account By VoucherAccID (with all Scheme Code)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_VoucherAccount_upd_Suspend_ByVoucherAccID]
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

	UPDATE [dbo].[VoucherAccount]
	SET
		Record_Status = 'S',
		Remark = 'Suspend Account: HKID Validation Fail From IMMD'
	WHERE 
		Voucher_Acc_ID = @Voucher_Acc_ID

END

GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccount_upd_Suspend_ByVoucherAccID] TO HCVU
GO
