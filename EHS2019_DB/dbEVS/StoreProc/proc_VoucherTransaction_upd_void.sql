IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransaction_upd_void]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransaction_upd_void]
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
-- Modified by:		Lawrence TSANG
-- Modified date:	18 September 2009
-- Description:		Remove the logic on Voucher_Acc_ID checking
-- =============================================
-- =============================================
-- Author:			Clark YIP
-- Create date:		06/10/2008	
-- Description:		update voucher transaction for void by HCVU only
-- =============================================

CREATE PROCEDURE [dbo].[proc_VoucherTransaction_upd_void]
	@transaction_id			char(20), 
	@void_transaction_id	char(20), 
	@void_remark			nvarchar(255),
	@void_by				char(20),
	@void_dtm				datetime,	
	@tsmp					timestamp
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
	IF (SELECT Transaction_ID FROM ReimbursementAuthTran WHERE Transaction_ID = @Transaction_ID) IS NOT NULL
	BEGIN
		RAISERROR('00011', 16, 1)
		RETURN @@error
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
		Void_Transaction_ID = @void_transaction_id,
		Void_Dtm = @void_dtm,
		Void_Remark = @void_remark,
		Void_By = @void_by,
		Update_By = @void_by,
		Update_Dtm = GETDATE(),
		Record_Status = 'I',
		Void_By_HCVU = 'Y'
	
	WHERE
		Transaction_ID = @transaction_id
	
END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransaction_upd_void] TO HCVU
GO
