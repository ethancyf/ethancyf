IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementSummary_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementSummary_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	6 March 2015
-- CR No.:			CRE13-019-02
-- Description:		Extend HCVS to China
-- =============================================
-- =============================================
-- Modification History
-- CR #:			INT15-0001
-- Modified by:		Karl LAM	
-- Modified date:	05 Mar 2015
-- Description:		Add with recompile to avoid (Thread is being aborted error)
-- =============================================
-- =============================================
-- Author:			Clark Yip
-- Create date:		22 Apr 2008
-- Description:		Reimbursement Summary
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   02 Dec 2008
-- Description:	    Total amount will be calculated based on the Claim_Amount field
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark YIP
-- Modified date:   17 Aug 2009
-- Description:	    Adopt to new reimbursement schema
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	4 September 2009
-- Description:		Retrieve [TransactionDetail].[Unit] and [TransactionDetail].[Total_Amount]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark YIP
-- Modified date:   07 Sep 2009
-- Description:	    Combine the select statements into 1
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_ReimbursementSummary_get]
	@cutoff_dtm		datetime,
	@scheme_code	char(10)
WITH RECOMPILE	
AS BEGIN
--***********************************************/
--* Declaration                                 */                        
--***********************************************/
declare   @txn_tot    as  int
declare   @distinct_sp_count    as  int
declare   @voucher_tot   as  int 
declare   @amt_tot   as  money

--***********************************************/
--*   Validation                                */
--***********************************************/

--***********************************************/
--*   Initialization                            */
--***********************************************/


select @txn_tot = count(1) from VoucherTransaction t
left outer join ReimbursementAuthTran rt 
				on t.transaction_id = rt.transaction_id 
where t.Record_status = 'A' AND 
rt.Authorised_status IS NULL AND
t.Confirmed_dtm <= @cutoff_dtm and
t.Scheme_code = @scheme_code and
rt.Scheme_code is null

-- =============================================
-- Return results
-- =============================================
IF @txn_tot > 0
BEGIN

select 
		@txn_tot as noTran, 
		count (distinct cast(t.SP_ID as varchar)+' ('+cast(t.practice_display_seq as varchar)+')') as noSP, 
		SUM(TD.Unit) as vouchersClaimed,
		SUM(TD.Total_Amount) as totalAmount,
		SUM(TD.Total_Amount_RMB) as [TotalAmountRMB]
FROM VoucherTransaction T
LEFT OUTER JOIN ReimbursementAuthTran rt 
				ON T.transaction_id = rt.transaction_id 
INNER JOIN TransactionDetail TD
				ON T.Transaction_ID = TD.Transaction_ID
where t.Record_status = 'A' AND 
rt.Authorised_status IS NULL AND
t.Confirmed_dtm <= @cutoff_dtm and
t.Scheme_code = @scheme_code and
rt.Scheme_code is null

END

END
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementSummary_get] TO HCVU
GO
