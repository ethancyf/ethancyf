IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementAuthoriseSecondAuth_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementAuthoriseSecondAuth_get]
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
CREATE PROCEDURE 	[dbo].[proc_ReimbursementAuthoriseSecondAuth_get] @tran_status		 char(1)							
							,@authorised_status		char(1)
							,@first_authorized_dtm	datetime
							,@first_authorized_by	varchar(20)
							,@scheme_code			char(10)
							,@reimburse_id			char(15)
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
OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key
-- =============================================
-- Return results
-- =============================================

SELECT t.transaction_id, t.bank_account_no as bankAccount, t.voucher_claim as voucherRedeem
--, t.per_voucher_value as voucherAmount, t.Voucher_Claim * t.Per_Voucher_Value as totalAmount, t.tsmp as tsmp, rt.tsmp as RTTSMP, t.SP_ID, t.practice_display_seq, convert(varchar(40), DecryptByKey(sp.[Encrypt_Field2])) as SP_Eng_Name
, t.per_voucher_value as voucherAmount, t.Claim_Amount as totalAmount, t.tsmp as tsmp, rt.tsmp as RTTSMP, t.SP_ID, t.practice_display_seq, convert(varchar(40), DecryptByKey(sp.[Encrypt_Field2])) as SP_Eng_Name
FROM VoucherTransaction t, reimbursementAuthTran rt, ServiceProvider sp
WHERE t.Record_status = 'A' 
and @authorised_status='2' and t.Authorised_status='1'
--and DATEDIFF("mi", @first_authorized_dtm, rt.first_authorised_dtm) = 0
and rt.first_authorised_by = @first_authorized_by
--and t.transaction_dtm <= @cutoff_dtm
and t.scheme_code = @scheme_code
and t.transaction_id = rt.transaction_id
and t.reimburse_id = @reimburse_id and rt.reimburse_id = @reimburse_id and t.reimburse_id = rt.reimburse_id
and t.sp_id = sp.sp_id
CLOSE SYMMETRIC KEY sym_Key

END
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementAuthoriseSecondAuth_get] TO HCVU
GO
*/
