IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccountRectify_upd]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccountRectify_upd]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 29 May 2008
-- Description:	Update TempVoucherAccountRectify Status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	28 Aug 2009
-- Description:		Remove Scheme Code
-- =============================================
CREATE PROCEDURE [dbo].[proc_TempVoucherAccountRectify_upd]
	@Voucher_Acc_ID char(15),
	@Update_By char(20),
	@Record_Status char(1),
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
		[Record_Status] = @Record_Status, 
		[Update_By] = @Update_By, 
		[Update_Dtm] = getdate()
	WHERE [Voucher_Acc_ID] = @Voucher_Acc_ID 
	
END
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccountRectify_upd] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccountRectify_upd] TO HCVU
GO
