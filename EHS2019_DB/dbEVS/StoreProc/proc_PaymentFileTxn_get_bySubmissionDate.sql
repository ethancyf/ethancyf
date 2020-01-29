IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PaymentFileTxn_get_bySubmissionDate]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PaymentFileTxn_get_bySubmissionDate]
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
-- Create date:		19 May 2008
-- Description:		Reimbursement Summary (By Txn)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

/*
CREATE PROCEDURE 	[dbo].[proc_PaymentFileTxn_get_bySubmissionDate] @tran_status		 char(1)							
							,@submitted_by			varchar(20)
							,@submission_dtm	    datetime
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
convert(nvarchar, DecryptByKey(a.[Encrypt_Field3])) as SPChiName,
t.sp_id as SPID,
t.bank_account_no as bankAccountid, 
p.Practice_name as practiceid, 
t.Voucher_Claim as voucherRedeem, 
t.Per_Voucher_Value as voucherAmount,
t.record_status as status,
t.authorised_status as authorised_status
from ServiceProvider a, Practice p, VoucherTransaction t
where 
t.sp_id = p.sp_id AND t.practice_display_seq = p.display_seq
AND t.record_status=@tran_status
and t.transaction_id IN
(
select transaction_id from BankInTransaction
where reimburse_id = (select reimburse_id from BankIn where DATEDIFF("mi", @submission_dtm, submission_dtm) = 0 and submitted_by=@submitted_by)
)
AND t.scheme_code=@scheme_code

CLOSE SYMMETRIC KEY sym_Key
END
GO

GRANT EXECUTE ON [dbo].[proc_PaymentFileTxn_get_bySubmissionDate] TO HCVU
GO
*/
