IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementVoidSummary_get_byAuthoriseStatusSecurity]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementVoidSummary_get_byAuthoriseStatusSecurity]
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
-- Description:		Get Void reimbursement summary
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   24 Nov 2008
-- Description:	    Add the grouping logic
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   02 Dec 2008
-- Description:	    Total amount will be calculated based on the Claim_Amount field
-- =============================================

--CREATE PROCEDURE 	[dbo].[proc_ReimbursementVoidSummary_get_byAuthoriseStatusSecurity] @tran_status		 char(1)
--							,@authorised_status		char(1)
--							,@function_code			char(6)
--							,@scheme_code		 char(10)

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
---- =============================================
---- Return results
---- =============================================
--/*
--SELECT t.transaction_id as tranNum, 
--rt.first_authorised_dtm as firstAuthorizedDate, 
--rt.first_authorised_by as firstAuthorizedBy,
--rt.second_authorised_dtm as secondAuthorizedDate, 
--rt.second_authorised_by as secondAuthorizedBy,
--rt.reimburse_id as reimburseID,
--t.Voucher_Claim as voucherRedeem, 
--t.Per_Voucher_Value as voucherAmount,
--t.authorised_status as status 
--FROM VoucherTransaction t, ReimbursementAuthTran rt
--WHERE t.transaction_id = rt.transaction_id
--and t.Record_Status='A'
--and t.authorised_status is not null
--and t.Authorised_status IN (SELECT '1' union select '2')
--and t.scheme_code = @scheme_code
--*/

--SELECT 
--rt.first_authorised_dtm as firstAuthorizedDate, 
--rt.first_authorised_by as firstAuthorizedBy,
--rt.reimburse_id as reimburseID,
--count(t.bank_account_no) as noTran,
--sum(t.Voucher_Claim) as voucherRedeem,
----SUM(t.Voucher_Claim*t.Per_Voucher_Value) as totalAmount,
--SUM(t.Claim_Amount) as totalAmount,
--t.authorised_status as status 
--FROM VoucherTransaction t, ReimbursementAuthTran rt
--WHERE t.transaction_id = rt.transaction_id
--and t.Record_Status='A'
--and t.Authorised_status ='1'
--and t.scheme_code = @scheme_code
--group by rt.first_authorised_dtm, rt.first_authorised_by, rt.reimburse_id, t.authorised_status

----and rt.authorised_dtm = ra.authorised_dtm and rt.authorised_status = ra.authorised_status

--END
--GO

--GRANT EXECUTE ON [dbo].[proc_ReimbursementVoidSummary_get_byAuthoriseStatusSecurity] TO HCVU
--GO
