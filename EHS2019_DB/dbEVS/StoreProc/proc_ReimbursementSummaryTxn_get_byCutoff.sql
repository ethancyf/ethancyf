IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementSummaryTxn_get_byCutoff]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementSummaryTxn_get_byCutoff]
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
-- Description:		Reimbursement Summary (By Cutoff) (ych480:Encryption)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   02 Dec 2008
-- Description:	    Add to select the total amount. Total amount will be calculated based on the Claim_Amount field
-- =============================================

/*
CREATE PROCEDURE 	[dbo].[proc_ReimbursementSummaryTxn_get_byCutoff] @tran_status		 char(1)							
							,@cutoff_dtm	      	 datetime
							,@scheme_code			char(10)						
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

select t.transaction_id as tranNum, 
t.transaction_dtm as tranDate,
--a.sp_eng_name as SPName,
convert(varchar(40), DecryptByKey(a.[Encrypt_Field2])) as SPName,
t.sp_id as SPID,
t.bank_account_no as bankAccountid, 
p.Practice_name as practiceid, 
t.Voucher_Claim as voucherRedeem, 
t.Per_Voucher_Value as voucherAmount,
t.record_status as status,
t.authorised_status as authorised_status,
t.Claim_Amount as totalAmount
from ServiceProvider a, Practice p, VoucherTransaction t
where a.SP_ID = t.SP_ID and a.SP_ID = p.SP_ID and t.Record_Status='A' AND
t.transaction_dtm <= @cutoff_dtm AND t.Practice_display_seq = p.display_seq AND
t.Authorised_status = '2'
AND t.scheme_code = @scheme_code

CLOSE SYMMETRIC KEY sym_Key
END
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementSummaryTxn_get_byCutoff] TO HCVU
GO
*/
