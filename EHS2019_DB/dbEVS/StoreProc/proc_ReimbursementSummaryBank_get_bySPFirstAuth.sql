IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementSummaryBank_get_bySPFirstAuth]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementSummaryBank_get_bySPFirstAuth]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	2 January 2015
-- CR No.:			CRE13-019-02
-- Description:		Extend HCVS to China
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	24 March 2015
-- CR No.:			INT15-0002
-- Description:		Set the stored procedure to recompile each time
-- =============================================
-- =============================================
-- Author:			Clark Yip
-- Create date:		22 Apr 2008
-- Description:		Reimbursement Bank Summary (By SP, First Auth)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   02 Dec 2008
-- Description:	    Total amount will be calculated based on the Claim_Amount field
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   14 Aug 2009
-- Description:	    1. Use the ReimbursementAuthTran table Authorised_Status
--					2. Remove the @tran_status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	4 September 2009
-- Description:		Retrieve [TransactionDetail].[Unit] and [TransactionDetail].[Total_Amount]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   08 Sep 2009
-- Description:	    Add distinct when count the no of transaction
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE 	[dbo].[proc_ReimbursementSummaryBank_get_bySPFirstAuth] 
							@sp_id         	 varchar(8)
							,@practice_display_seq	smallint							
							,@first_authorized_dtm	datetime
							,@first_authorized_by	varchar(20)
							,@scheme_code		char(10)
WITH RECOMPILE
AS BEGIN
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

	select
		t.bank_account_no as bankAccountid, p.Practice_name as practiceid,
		count(Distinct t.Transaction_ID) as noTran,
		sum(TD.Unit) as vouchersClaimed,
		SUM(TD.Total_Amount) as totalAmount,
		SUM(ISNULL(TD.Total_Amount_RMB, 0)) AS [TotalAmountRMB]
	
	FROM
		ServiceProvider a, Practice p, reimbursementAuthTran rt, VoucherTransaction t
			INNER JOIN TransactionDetail TD
				ON T.Transaction_ID = TD.Transaction_ID
		
	where
		a.SP_ID = t.SP_ID and a.SP_ID = p.SP_ID and t.Record_Status= 'A' 
		and p.SP_ID = @sp_id AND t.Practice_display_seq = p.display_seq
		AND rt.Authorised_status = '1'
		and t.scheme_code = @scheme_code
		and rt.scheme_code = @scheme_code
		and t.transaction_id = rt.transaction_id
		and DATEDIFF("mi", @first_authorized_dtm, rt.first_authorised_dtm) = 0
		and rt.first_authorised_by = @first_authorized_by
		and t.practice_display_seq=@practice_display_seq
	
	GROUP BY
		t.bank_account_no, p.Practice_name

END
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementSummaryBank_get_bySPFirstAuth] TO HCVU
GO
