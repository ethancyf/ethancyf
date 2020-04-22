IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementSummary_get_byFirstAuthorization]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementSummary_get_byFirstAuthorization]
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
-- Description:		Reimbursement Summary by First Authorization
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   02 Dec 2008
-- Description:	    Total amount will be calculated based on the Claim_Amount field
-- =============================================

--CREATE PROCEDURE 	[dbo].[proc_ReimbursementSummary_get_byFirstAuthorization] @tran_status		 char(1)							
--							--,@cutoff_dtm	      	 datetime
--							,@authorised_status         	 char(1)
--							--,@first_authorized_dtm	datetime
--							--,@first_authorized_by	varchar(20)
--							,@reimburse_id char(15)
--							,@scheme_code		char(10)

--as
--BEGIN
----***********************************************/
----* Declaration                                 */                        
----***********************************************/
--declare   @txn_tot    as  int
--declare   @distinct_sp_count    as  int
--declare   @voucher_tot   as  int 
--declare   @amt_tot   as  money

----***********************************************/
----*   Validation                                */
----***********************************************/

----***********************************************/
----*   Initialization                            */
----***********************************************/
--select @txn_tot = count(1) from VoucherTransaction t, reimbursementAuthTran rt
--where t.Record_status = 'A' AND @tran_status='2' AND t.Authorised_status = '1'
----and t.transaction_dtm <= @cutoff_dtm 
----and t.Confirmed_dtm <= @cutoff_dtm 
----and DATEDIFF(hh, t.Confirmed_dtm, @cutoff_dtm) > 24
--and t.Scheme_code = @scheme_code
--and t.transaction_id = rt.transaction_id
--AND t.reimburse_id = @reimburse_id
----and DATEDIFF("mi", @first_authorized_dtm, rt.first_authorised_dtm) = 0
----and rt.first_authorised_by = @first_authorized_by

----select @distinct_sp_count = count (distinct SP_ID) from VoucherTransaction t, reimbursementAuthTran rt
--select @distinct_sp_count = count (distinct cast(t.SP_ID as varchar)+' ('+cast(t.practice_display_seq as varchar)+')') from VoucherTransaction t, reimbursementAuthTran rt
--where t.Record_status = 'A' AND @tran_status='2' AND t.Authorised_status = '1'
----and t.transaction_dtm <= @cutoff_dtm 
----and t.Confirmed_dtm <= @cutoff_dtm
----and DATEDIFF(hh, t.Confirmed_dtm, @cutoff_dtm) > 24
--and t.Scheme_code = @scheme_code
--and t.transaction_id = rt.transaction_id
--AND t.reimburse_id = @reimburse_id
----and DATEDIFF("mi", @first_authorized_dtm, rt.first_authorised_dtm) = 0
----and rt.first_authorised_by = @first_authorized_by

--select @voucher_tot = sum(Voucher_Claim) from VoucherTransaction t, reimbursementAuthTran rt
--where t.Record_status = 'A' AND @tran_status='2' AND t.Authorised_status = '1'
----and t.transaction_dtm <= @cutoff_dtm 
----and t.Confirmed_dtm <= @cutoff_dtm
----and DATEDIFF(hh, t.Confirmed_dtm, @cutoff_dtm) > 24
--and t.Scheme_code = @scheme_code
--and t.transaction_id = rt.transaction_id
--AND t.reimburse_id = @reimburse_id
----and DATEDIFF("mi", @first_authorized_dtm, rt.first_authorised_dtm) = 0
----and rt.first_authorised_by = @first_authorized_by

----select @amt_tot = sum(Voucher_Claim * Per_Voucher_Value) from VoucherTransaction t, reimbursementAuthTran rt
--select @amt_tot = sum(t.Claim_Amount) from VoucherTransaction t, reimbursementAuthTran rt
--where t.Record_status = 'A' AND @tran_status='2' AND t.Authorised_status = '1'
----and t.transaction_dtm <= @cutoff_dtm 
----and t.Confirmed_dtm <= @cutoff_dtm
----and DATEDIFF(hh, t.Confirmed_dtm, @cutoff_dtm) > 24
--and t.Scheme_code = @scheme_code
--and t.transaction_id = rt.transaction_id
--AND t.reimburse_id = @reimburse_id
----and DATEDIFF("mi", @first_authorized_dtm, rt.first_authorised_dtm) = 0
----and rt.first_authorised_by = @first_authorized_by

----***********************************************/
----*   Return results                            */
----***********************************************/
--IF @txn_tot > 0
--BEGIN
--	select @txn_tot as noTran, @distinct_sp_count as noSP, @voucher_tot as vouchersClaimed, @amt_tot as totalAmount
--END

--END
--GO

--GRANT EXECUTE ON [dbo].[proc_ReimbursementSummary_get_byFirstAuthorization] TO HCVU
--GO
