IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransaction_upd_ReplaceVRAccIDbyTranID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransaction_upd_ReplaceVRAccIDbyTranID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Clark YIP
-- Create date: 15 Dec 2008
-- Description:	Update Voucher Transaction Set Voucher_Acc_ID By Temp_Voucher_Acc_ID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Clark YIP
-- Modified date: 20 Jan 2009
-- Description:	  remove the update of bank and practice display seq in voucher transaction table
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Pak Ho LEE
-- Modified date: 08 Sep 2009
-- Description:	  Add Checking
-- =============================================
CREATE PROCEDURE [dbo].[proc_VoucherTransaction_upd_ReplaceVRAccIDbyTranID]
	@Transaction_ID char(20),
	@Temp_Voucher_Acc_ID char(15),
	@Old_Temp_Voucher_Acc_ID char(15)
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================

	IF (SELECT COUNT(*) FROM [VoucherTransaction] WHERE	
		[Temp_Voucher_Acc_ID] = @Old_Temp_Voucher_Acc_ID AND [Transaction_ID] = @Transaction_ID) != 1
	BEGIN
		RAISERROR('00011', 16, 1)
		return @@error
	END
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================
	UPDATE [VoucherTransaction]
	SET 
		[Temp_Voucher_Acc_ID] = @Temp_Voucher_Acc_ID
	WHERE
		[Transaction_ID] = @Transaction_ID AND [Temp_Voucher_Acc_ID] = @Old_Temp_Voucher_Acc_ID
		
END

GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransaction_upd_ReplaceVRAccIDbyTranID] TO HCSP
GO
