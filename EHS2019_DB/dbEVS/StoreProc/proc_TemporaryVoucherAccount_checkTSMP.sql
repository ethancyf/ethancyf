IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TemporaryVoucherAccount_checkTSMP]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TemporaryVoucherAccount_checkTSMP]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 09 Oct 2009
-- Description:	Check TSMP of TempVoucherAccount
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date:
-- Description:
-- =============================================

CREATE PROCEDURE [dbo].[proc_TemporaryVoucherAccount_checkTSMP]
	@Voucher_Acc_ID char(15),
	@TSMP timestamp	
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF (SELECT [TSMP] from [TempVoucherAccount] WHERE Voucher_Acc_ID = @Voucher_Acc_ID) != @TSMP 
	BEGIN
		Raiserror('00011', 16,1)
		RETURN @@error
	END

-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================	
	
END
GO

GRANT EXECUTE ON [dbo].[proc_TemporaryVoucherAccount_checkTSMP] TO HCSP
GO
