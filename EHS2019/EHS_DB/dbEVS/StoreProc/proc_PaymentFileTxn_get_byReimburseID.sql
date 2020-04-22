IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PaymentFileTxn_get_byReimburseID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PaymentFileTxn_get_byReimburseID]
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
-- Modified by:	    Clark Yip
-- Modified date:   02 Dec 2008
-- Description:	    Add to select the total amount. Total amount will be calculated based on the Claim_Amount field
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   25 Dec 2008
-- Description:	    Fine tune the sql
-- =============================================

/*
CREATE PROCEDURE 	[dbo].[proc_PaymentFileTxn_get_byReimburseID] @tran_status		 char(1)							
							,@submitted_by			varchar(20)
							,@reimburse_id			char(15)
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

select t.transaction_id as transNum, 
t.transaction_dtm as transDate,
convert(varchar(40), DecryptByKey(a.[Encrypt_Field2])) as ServiceProvider,
convert(nvarchar, DecryptByKey(a.[Encrypt_Field3])) as SPChiName,
t.sp_id as spID,
t.bank_account_no as bankAccount, 
p.Practice_name as practice, 
t.Voucher_Claim as VoucherRedeem, 
t.Per_Voucher_Value as VoucherValue,
t.record_status as transstatus,
--t.authorised_status as authorised_status,
t.Claim_Amount as totalAmount,
ROW_NUMBER() over(order by t.transaction_id) as lineNum
from ServiceProvider a, Practice p, VoucherTransaction t
where 
t.sp_id = p.sp_id AND t.practice_display_seq = p.display_seq and t.sp_id = a.sp_id
AND t.record_status=@tran_status
and t.reimburse_id = @reimburse_id
AND t.scheme_code=@scheme_code
order by t.transaction_id

CLOSE SYMMETRIC KEY sym_Key
END
GO

GRANT EXECUTE ON [dbo].[proc_PaymentFileTxn_get_byReimburseID] TO HCVU
GO
*/
