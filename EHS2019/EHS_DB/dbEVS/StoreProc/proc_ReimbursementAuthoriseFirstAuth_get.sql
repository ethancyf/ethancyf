IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementAuthoriseFirstAuth_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementAuthoriseFirstAuth_get]
GO

--SET ANSI_NULLS ON
--SET QUOTED_IDENTIFIER ON
--GO
--
--
---- =============================================
---- Modification History
---- CR No.:			INT13-0004
---- Modified by:		Koala CHENG
---- Modified date:		07 Feb 2013
---- Description:		Stored Procedure obsoleted
---- =============================================
---- =============================================
---- Modification History
---- Modified by:	    Clark Yip
---- Modified date:   02 Dec 2008
---- Description:	    Total amount will be calculated based on the Claim_Amount field
---- =============================================
---- =============================================
---- Author:			Clark Yip
---- Create date:		22 Apr 2008
---- Description:		Reimbursement Authorisation
---- =============================================
--CREATE PROCEDURE 	[dbo].[proc_ReimbursementAuthoriseFirstAuth_get] @tran_status		 char(1)
--							,@cutoff_dtm			datetime							
--							,@scheme_code			char(10)
--as
--BEGIN
---- =============================================
---- Declaration
---- =============================================
--
---- =============================================
---- Validation 
---- =============================================
--
---- =============================================
---- Initialization
---- =============================================
--
---- =============================================
---- Return results
---- =============================================
--
--SELECT t.transaction_id, t.bank_account_no as bankAccount, t.voucher_claim as voucherRedeem
----, t.per_voucher_value as voucherAmount, t.Voucher_Claim * t.Per_Voucher_Value as totalAmount, t.reimburse_ID as reimburseID, t.tsmp
--, t.per_voucher_value as voucherAmount, t.Claim_Amount as totalAmount, t.reimburse_ID as reimburseID, t.tsmp
--FROM VoucherTransaction t WHERE t.Record_status = 'A' 
--and t.Authorised_status is null
----and t.transaction_dtm <= @cutoff_dtm
--and t.confirmed_dtm <= @cutoff_dtm
----and DATEDIFF(hh, t.Confirmed_dtm, @cutoff_dtm) > 24
--and t.scheme_code = @scheme_code
--
--END
--GO
--
--GRANT EXECUTE ON [dbo].[proc_ReimbursementAuthoriseFirstAuth_get] TO HCVU
--GO
