IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementHoldSummary_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementHoldSummary_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================  
-- Modification History  
-- Modified by:		Koala CHENG
-- Modified date:	15 January 2018
-- CR No.:			I-CRE17-005
-- Description:		Performance Tuning
-- 					1. Add WITH (NOLOCK)
--					2. Get SUMs in single query
-- ============================================= 
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	24 March 2015
-- CR No.:			INT15-0002
-- Description:		Set the stored procedure to recompile each time
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	2 January 2015
-- CR No.:			CRE13-019-02
-- Description:		Extend HCVS to China
-- =============================================
-- =============================================  
-- Modification History  
-- Modified by:  Clark YIP  
-- Modified date: 15 Oct 2009  
-- Description:  Refine the store proc  
-- =============================================  
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	4 September 2009
-- Description:		Get [TransactionDetail].[Unit] and [TransactionDetail].[Total_Amount]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   13 Aug 2009
-- Description:	    Will look at the ReimbursementAuthTran table
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   02 Dec 2008
-- Description:	    Total amount will be calculated based on the Claim_Amount field
-- =============================================
-- =============================================
-- Author:			Clark Yip
-- Create date:		22 Apr 2008
-- Description:		Reimbursement Hold Summary
-- =============================================

CREATE PROCEDURE [dbo].[proc_ReimbursementHoldSummary_get]
	@scheme_code	char(10)
WITH RECOMPILE
AS BEGIN
--***********************************************/
--* Declaration                                 */                        
--***********************************************/
declare	  @reimburse_id as char(15)
declare   @txn_tot    as  int
declare   @distinct_sp_count    as  int
declare   @voucher_tot   as  int 
declare   @amt_tot   as  money
declare   @amt_total_RMB   as  money
declare	  @authorised_cutoff_dtm as datetime
declare   @authorised_by	as varchar(20)

--***********************************************/
--*   Validation                                */
--***********************************************/

--***********************************************/
--*   Initialization                            */
--***********************************************/
--Since each time each schmem can have ONLY 1 batch of HOLD transaction

select top 1 @reimburse_id = reimburse_id from ReimbursementAuthorisation t WITH (NOLOCK) 
where t.authorised_status='P' and Record_Status='A'  
	and t.Scheme_code = @scheme_code  
	and Reimburse_id not in (Select Reimburse_id from ReimbursementAuthorisation WITH (NOLOCK) where Authorised_Status='R' AND Scheme_code = @scheme_code)  
	order by authorised_dtm desc  
  
IF @reimburse_id IS NOT NULL  
BEGIN  
	 select @txn_tot = count(1) from ReimbursementAuthTran t WITH (NOLOCK)  
	 where t.authorised_status='P'  
		and t.Scheme_code = @scheme_code  
	  
	 --***********************************************/  
	 --*   Return results                            */  
	 --***********************************************/  
	IF @txn_tot > 0  
		BEGIN   
			 select @voucher_tot = sum(TD.Unit), 
					@amt_tot = sum(TD.Total_Amount),
					@amt_total_RMB = sum(TD.Total_Amount_RMB),
					@distinct_sp_count = count (distinct cast(t.SP_ID as varchar)+' ('+cast(t.practice_display_seq as varchar)+')')   
			 from VoucherTransaction t WITH (NOLOCK) 
				INNER JOIN TransactionDetail TD WITH (NOLOCK)
					ON t.Transaction_ID = TD.Transaction_ID  
				INNER JOIN ReimbursementAuthTran RA WITH (NOLOCK)
					 ON t.transaction_id = RA.transaction_id  
						AND RA.Reimburse_ID = @reimburse_id  
						AND RA.authorised_status = 'P'  
						AND RA.Scheme_code = @scheme_code
								
			 SELECT @authorised_cutoff_dtm = t.cutoff_date,
					@authorised_by = t.update_by
			 from ReimbursementAuthorisation t WITH (NOLOCK)
			 where t.authorised_status='P'  
				 and t.Scheme_code = @scheme_code  
				 and t.Reimburse_id = @reimburse_id				  
	 
			select @reimburse_id as reimburseID, @txn_tot as noTran, @distinct_sp_count as noSP, @voucher_tot as vouchersClaimed, @amt_tot as totalAmount, 
			@amt_total_RMB AS [TotalAmountRMB],
			@authorised_cutoff_dtm as AuthorisedCutoffTime, @authorised_by as AuthorisedCutoffBy  
		END   
	END
END
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementHoldSummary_get] TO HCVU
GO
