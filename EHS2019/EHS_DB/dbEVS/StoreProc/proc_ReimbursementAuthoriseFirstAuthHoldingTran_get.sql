IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementAuthoriseFirstAuthHoldingTran_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementAuthoriseFirstAuthHoldingTran_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.			CRE16-026-03 (Add PCV13)
-- Modified by:		Lawrence TSANG
-- Modified date:	17 October 2017
-- Description:		Stored procedure not used anymore
-- =============================================
-- =============================================
-- Author:			Clark Yip
-- Create date:		22 Apr 2008
-- Description:		Reimbursement Authorisation
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   02 Dec 2008
-- Description:	    Total amount will be calculated based on the Claim_Amount field
-- =============================================

/*
CREATE PROCEDURE 	[dbo].[proc_ReimbursementAuthoriseFirstAuthHoldingTran_get] @scheme_code			char(10)
as
BEGIN
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

SELECT t.transaction_id, t.bank_account_no as bankAccount, t.voucher_claim as voucherRedeem
--, t.per_voucher_value as voucherAmount, t.Voucher_Claim * t.Per_Voucher_Value as totalAmount, t.reimburse_id as reimburseID, t.tsmp
, t.per_voucher_value as voucherAmount, t.Claim_Amount as totalAmount, t.reimburse_id as reimburseID, t.tsmp
FROM VoucherTransaction t WHERE t.Record_status = 'A' 
and t.authorised_status='P'
--and t.transaction_dtm <= @cutoff_dtm
and t.scheme_code = @scheme_code

END
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementAuthoriseFirstAuthHoldingTran_get] TO HCVU
GO
*/
