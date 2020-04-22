IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementVoidTxn_get_byAuthoriseDtmBy]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementVoidTxn_get_byAuthoriseDtmBy]
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
-- Description:		Get Void reimbursement summary (by Txn)(ych480:Encryption)
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
-- Modified date:   24 Dec 2008
-- Description:	    Modify the alias name as to fit the middle tier
-- =============================================
--CREATE PROCEDURE 	[dbo].[proc_ReimbursementVoidTxn_get_byAuthoriseDtmBy] @tran_status		 char(1)
--							,@authorised_status		char(1)
--							,@authorised_dtm		datetime
--							,@authorised_by			varchar(20)
--							,@scheme_code			char(10)

--as
--BEGIN
---- =============================================
---- Declaration
---- =============================================
---- =============================================
---- Validation 
---- =============================================
---- =============================================
---- Initialization
---- =============================================
--OPEN SYMMETRIC KEY sym_Key 
--	DECRYPTION BY ASYMMETRIC KEY asym_Key
---- =============================================
---- Return results
---- =============================================

--select t.transaction_id as transNum, 
--t.transaction_dtm as transDate,
--convert(varchar(40), DecryptByKey(a.[Encrypt_Field2])) as ServiceProvider,
--convert(nvarchar, DecryptByKey(a.[Encrypt_Field3])) as SPChiName,
--t.sp_id as SPID,
--t.bank_account_no as bankAccount, 
--p.Practice_name as practice, 
--t.Voucher_Claim as VoucherRedeem, 
--t.Per_Voucher_Value as VoucherValue,
--t.authorised_status as transstatus,
--t.tsmp	as tsmp,
--t.Claim_Amount as totalAmount,
--ROW_NUMBER() over(order by t.transaction_id) as lineNum
--from ServiceProvider a, Practice p, VoucherTransaction t, reimbursementAuthTran rt, ReimbursementAuthorisation ra
--where t.transaction_id = rt.transaction_id AND t.Record_Status='A' AND t.sp_id = a.sp_id 
--AND a.sp_id = p.sp_id AND t.Practice_display_seq = p.display_seq AND rt.authorised_status = @authorised_status
--AND ((@authorised_status = '1' AND DATEDIFF("mi", @authorised_dtm, rt.first_authorised_dtm) = 0) OR 
--((@authorised_status = '2' AND DATEDIFF("mi", @authorised_dtm, rt.second_authorised_dtm) = 0)))
--AND t.scheme_code = @scheme_code
--and t.reimburse_id = ra.reimburse_id and rt.authorised_status = @authorised_status and ra.reimburse_id = rt.reimburse_id
--and t.authorised_status = ra.authorised_status
--order by t.transaction_id

--/*AND sd.Column_name = 'REIMBURSEAUTHTRAN' and sd.status_value = @authorised_status and sd.record_status='A' 
--and(sd.expiry_dtm is null OR (sd.expiry_dtm is not null AND sd.expiry_dtm <= {fn now()}))*/

--/*and rt.authorised_dtm = ra.authorised_dtm and rt.authorised_status = ra.authorised_status
--and ra.Authorised_by=@authorised_by*/
--CLOSE SYMMETRIC KEY sym_Key
--END
--GO

--GRANT EXECUTE ON [dbo].[proc_ReimbursementVoidTxn_get_byAuthoriseDtmBy] TO HCVU
--GO
