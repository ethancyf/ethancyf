IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransaction_upd_status]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransaction_upd_status]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
	
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	26 February 2018
-- CR No.:			I-CRE17-007
-- Description:		Performance Tuning
--					1. Not allow to suspend transaction while status on/after "Ready to Reimburse (Hold For First Authorization)"
-- =============================================
-- =============================================
-- Modification History
-- CR No:		CRE11-024-02 HCVS Pilot Extension Part 2
-- Modified by:		Koala CHENG	
-- Modified date:	16 Augest 2011
-- Description:		Allow HCSP to update status after fill defer value for "Claim Transaction Management"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	17 September 2009
-- Description:		Allow suspend transaction with status "Ready to Reimburse", "Pending Confirmation", and "Pending eHealth Account Validation"
-- =============================================
-- =============================================
-- Author:		Clark YIP
-- Create date: 17/6/2008	
-- Description:	update voucher transaction status
-- =============================================
CREATE PROCEDURE [dbo].[proc_VoucherTransaction_upd_status]
	@transaction_id		char(20),
	@record_status		char(1),
	@update_by			varchar(20),
	@update_dtm			datetime,
	@tsmp				timestamp
AS BEGIN

	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF (SELECT TSMP FROM VoucherTransaction WHERE Transaction_ID = @Transaction_ID) != @tsmp
	BEGIN
		RAISERROR('00011', 16, 1)
		RETURN @@error
	END

	-- Not allow to suspend transaction while status on/after "Ready to Reimburse (Hold For First Authorization)"
	IF @record_status IN ('S','I')
	BEGIN
		IF (SELECT Transaction_ID FROM ReimbursementAuthTran WHERE Transaction_ID = @Transaction_ID) IS NOT NULL
		BEGIN
			RAISERROR('00011', 16, 1)
			RETURN @@error
		END
	END
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================
	UPDATE
		VoucherTransaction
		
	SET 				
		Update_By = @update_by,
		Update_Dtm = @update_dtm,
		Record_Status = @record_status
		
	WHERE 
		Transaction_ID = @transaction_id

END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransaction_upd_status] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransaction_upd_status] TO HCSP
GO
