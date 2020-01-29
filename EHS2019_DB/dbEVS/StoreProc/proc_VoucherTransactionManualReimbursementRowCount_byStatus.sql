 IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransactionManualReimbursementRowCount_byStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransactionManualReimbursementRowCount_byStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History 
-- Modified by:		Derek LEUNG
-- Modified date:	3 Nov 2010
-- Description:		Use record status on vouchertransaction table instead of manualreimbursement
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 25 Jul 2010
-- Description:	Retrieve No of row in ManualReimbusement &
--				VoucherTransaction by record status
-- =============================================

CREATE PROCEDURE [dbo].[proc_VoucherTransactionManualReimbursementRowCount_byStatus] 
	@User_ID	varchar(20),
	@Record_Status char(1)
AS
BEGIN

	SET NOCOUNT ON;
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

	/**
	select count(vt.transaction_id) as NoOfTran from vouchertransaction vt, manualreimbursement mr
	where vt.transaction_id = mr.transaction_id
	and isnull(vt.manual_reimburse,'N') = 'Y'
	and vt.record_status = 'A'
	and mr.record_status = @Record_Status
	and vt.scheme_code in 
	(
		select distinct scheme_code from userrole ur, roletype rt
		where ur.role_type = rt.role_type
		and ur.user_id = @user_id
	)
**/

select count(vt.transaction_id) as NoOfTran from vouchertransaction vt
	where 
		vt.record_status = 'B'	
	and vt.scheme_code in 
	(
		select distinct scheme_code from userrole ur, roletype rt
		where ur.role_type = rt.role_type
		and ur.user_id = @user_id
	)

END

GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransactionManualReimbursementRowCount_byStatus] TO HCVU
GO
