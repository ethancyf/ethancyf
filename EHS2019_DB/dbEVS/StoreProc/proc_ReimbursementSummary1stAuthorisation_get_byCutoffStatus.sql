IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementSummary1stAuthorisation_get_byCutoffStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementSummary1stAuthorisation_get_byCutoffStatus]
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
-- Description:		Reimbursement Summary (by first Authorisation)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

/*
CREATE PROCEDURE 	[dbo].[proc_ReimbursementSummary1stAuthorisation_get_byCutoffStatus] @tran_status		 char(1)
											,@cutoff_dtm	      	 datetime											
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
-- =============================================
-- Return results
-- =============================================

select rt.first_authorised_dtm as firstAuthorizedDate, rt.first_authorised_by as firstAuthorizedBy, t.Voucher_Claim as voucherRedeem, t.Per_Voucher_Value as voucherAmount 
from VoucherTransaction t, ReimbursementAuthTran rt
where t.transaction_id = rt.transaction_id 
and t.Record_status = 'A' AND t.transaction_dtm <= @cutoff_dtm AND
t.authorised_status = '1' and t.authorised_status = rt.authorised_status

--, reimbursementAuthorisation ra
--and rt.authorised_dtm = ra.authorised_dtm and rt.authorised_status = ra.authorised_status

END
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementSummary1stAuthorisation_get_byCutoffStatus] TO HCVU
GO
*/
