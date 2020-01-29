IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementSummaryTxnRow_get_byTranID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementSummaryTxnRow_get_byTranID]
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
-- Description:		Get Reimbursement Transaction (ych480:Encryption)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

--CREATE PROCEDURE 	[dbo].[proc_ReimbursementSummaryTxnRow_get_byTranID] @tran_id			varchar(20)
--							,@scheme_code		 char(10)

--as
--BEGIN
---- =============================================
---- Declaration
---- =============================================
--declare   @first_authorised_by    as  varchar(20)
--declare   @first_authorised_date    as  datetime
--declare   @second_authorised_by   as  varchar(20)
--declare   @second_authorised_date	as	datetime

---- =============================================
---- Validation 
---- =============================================
---- =============================================
---- Initialization
---- =============================================

--OPEN SYMMETRIC KEY sym_Key 
--	DECRYPTION BY ASYMMETRIC KEY asym_Key
----select @first_authorised_by = b.First_Authorised_By from ReimbursementAuthorisation a, ReimbursementAuthTran b where b.authorised_status = '1' and a.authorised_dtm = b.first_authorised_dtm and a.authorised_status = b.authorised_status and b.transaction_id = @tran_id
----select @first_authorised_date =  b.first_Authorised_dtm from ReimbursementAuthorisation a, ReimbursementAuthTran b where b.authorised_status = '1' and a.authorised_dtm = b.first_authorised_dtm and a.authorised_status = b.authorised_status and b.transaction_id = @tran_id
----select @second_authorised_by =  b.Second_Authorised_By from ReimbursementAuthorisation a, ReimbursementAuthTran b where b.authorised_status = '2' and a.authorised_dtm = b.second_authorised_dtm and a.authorised_status = b.authorised_status and b.transaction_id = @tran_id
----select @second_authorised_date = b.Second_Authorised_dtm from ReimbursementAuthorisation a, ReimbursementAuthTran b where b.authorised_status = '2' and a.authorised_dtm = b.second_authorised_dtm and a.authorised_status = b.authorised_status and b.transaction_id = @tran_id

--select @first_authorised_by = First_Authorised_By from ReimbursementAuthTran where transaction_id = @tran_id
--select @first_authorised_date = First_Authorised_dtm from ReimbursementAuthTran where transaction_id = @tran_id
--select @second_authorised_by = Second_Authorised_By from ReimbursementAuthTran where transaction_id = @tran_id
--select @second_authorised_date = Second_Authorised_dtm from ReimbursementAuthTran where transaction_id = @tran_id

---- =============================================
---- Return results
---- =============================================

--select t.transaction_id as tranNum, 
--t.transaction_dtm as tranDate,
----a.sp_eng_name as SPName,
--convert(varchar(40), DecryptByKey(a.[Encrypt_Field2])) as SPName,
--t.sp_id as SPID,
--t.bank_account_no as bankAccountid, 
--p.Practice_name as practiceid, 
--t.Voucher_Claim as voucherRedeem, 
--t.Per_Voucher_Value as voucherAmount,
--t.Record_status as status,
--t.Service_receive_dtm as serviceDate,
--t.Service_type as serviceType,
--t.reason_for_visit_L1 as visitReason_L1,
--t.reason_for_visit_L2 as visitReason_L2,
--@first_authorised_by as firstAuthorizedBy,
--@first_authorised_date as firstAuthorizedDate,
--@second_authorised_by as secondAuthorizedBy,
--@second_authorised_date as secondAuthorizedDate,
----i.hkid as acctHKID,
----i.Eng_name as ename,
----i.Chi_name as cname,
--convert(varchar, DecryptByKey(i.[Encrypt_Field1])) as acctHKID,
--convert(varchar(40), DecryptByKey(i.[Encrypt_Field2])) as ename,
--convert(nvarchar, DecryptByKey(i.[Encrypt_Field3])) as cname,
--i.sex as gender,
--i.exact_DOB as exactDOB,
--i.DOB as DOB,
--t.Authorised_status as Authorised_status,
--t.tsmp as tsmp
--from ServiceProvider a, Practice p, VoucherTransaction t,
--		PersonalInformation i
--where a.SP_ID = t.SP_ID and a.SP_ID = p.SP_ID  and t.voucher_acc_id = i.voucher_acc_id
--		and t.transaction_id = @tran_id
--		and t.scheme_code = @scheme_code
--		/*and t.Record_Status=@tran_status and p.SP_ID = @sp_id 
--		and t.bank_account_no = @bank_acc_no */
		
--CLOSE SYMMETRIC KEY sym_Key
--END
--GO

--GRANT EXECUTE ON [dbo].[proc_ReimbursementSummaryTxnRow_get_byTranID] TO HCVU
--GO
