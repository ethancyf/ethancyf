IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementSummaryTxn_get_bySPCutoffStatusBankFirstAuth]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementSummaryTxn_get_bySPCutoffStatusBankFirstAuth]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
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
-- Modified by:		Lawrence TSANG
-- Modified date:	24 March 2015
-- CR No.:			INT15-0002
-- Description:		Set the stored procedure to recompile each time
-- =============================================
-- =============================================
-- Author:			Clark Yip
-- Create date:		22 Apr 2008
-- Description:		Reimbursement Summary (By Txn)(ych480:Encryption)
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
-- Modified date:   14 Aug 2009
-- Description:	    Remove the @tran_status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	4 September 2009
-- Description:		Retrieve [TransactionDetail].[Unit], [TransactionDetail].[Per_Unit_Value], [TransactionDetail].[Total_Amount]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   10 Sep 2009
-- Description:	    Group the transaction by scheme
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Clark YIP
-- Modified date:	30 Sep 2009
-- Description:		Handle the drop of obsolete columns
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE 	[dbo].[proc_ReimbursementSummaryTxn_get_bySPCutoffStatusBankFirstAuth]
							@sp_id         	 varchar(8)
							,@practice_display_seq	smallint
							,@bank_acc_no		varchar(30)
							,@cutoff_dtm	      	 datetime
							,@first_authorized_dtm	datetime
							,@first_authorized_by	varchar(20)
							,@scheme_code		 char(10)
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
	EXEC [proc_SymmetricKey_open]
-- =============================================
-- Return results
-- =============================================

	select
		t.transaction_id as tranNum, 
		t.transaction_dtm as tranDate,
		convert(varchar(40), DecryptByKey(a.[Encrypt_Field2])) as SPName,
		t.sp_id as SPID,
		t.bank_account_no as bankAccountid, 
		p.Practice_name as practiceid, 
		SUM(TD.Unit) as voucherRedeem, 
		--TD.Per_Unit_Value as voucherAmount,
		t.record_status as status,
		RT.authorised_status as authorised_status,
		SUM(TD.Total_Amount) as totalAmount,
		SUM(ISNULL(TD.Total_Amount_RMB, 0)) as [TotalAmountRMB]
	
	from ServiceProvider a, Practice p, reimbursementAuthTran rt, VoucherTransaction t
		INNER JOIN TransactionDetail TD
				ON T.Transaction_ID = TD.Transaction_ID		
	
	where 
		a.SP_ID = t.SP_ID and a.SP_ID = p.SP_ID and t.Record_Status='A' AND
		t.Confirmed_Dtm <= @cutoff_dtm
		AND t.Practice_display_seq = p.display_seq AND
		p.SP_ID = @sp_id and t.bank_account_no = @bank_acc_no
		AND rt.Authorised_status = '1'
		AND t.scheme_code = @scheme_code
		AND rt.scheme_code = @scheme_code
		and t.transaction_id = rt.transaction_id
		and DATEDIFF("mi", @first_authorized_dtm, rt.first_authorised_dtm) = 0
		and rt.first_authorised_by = @first_authorized_by
		and t.practice_display_seq=@practice_display_seq
	GROUP BY
		t.transaction_id, 
		t.transaction_dtm,
		a.[Encrypt_Field2],
		t.sp_id,
		t.bank_account_no, 
		p.Practice_name, 		
		t.record_status,
		rt.authorised_status

	EXEC [proc_SymmetricKey_close]
END
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementSummaryTxn_get_bySPCutoffStatusBankFirstAuth] TO HCVU
GO
