 IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccount_upd_lastfaildtm]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccount_upd_lastfaildtm]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Paul Yip
-- Create date: 27 July 2010
-- Description:	Update TempVoucherAccount Last_Fail_Validate_Dtm
-- =============================================

CREATE PROCEDURE [dbo].[proc_TempVoucherAccount_upd_lastfaildtm]
	@Voucher_Acc_ID char(15),
	@Last_Fail_Validate_Dtm datetime,
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
	if (select TSMP from TempVoucherAccount 
		where Voucher_Acc_ID = @Voucher_Acc_ID) != @TSMP 
	begin
		Raiserror('00011', 16,1)
		return @@error
	end

-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================
	UPDATE [TempVoucherAccount]
	SET
		[Last_Fail_Validate_Dtm] = @Last_Fail_Validate_Dtm
	WHERE [Voucher_Acc_ID] = @Voucher_Acc_ID 
	
END
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_upd_lastfaildtm] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_upd_lastfaildtm] TO HCVU
GO
