IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccount_upd_delete]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccount_upd_delete]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Clark YIP
-- Create date: 16 Oct 2008
-- Description:	Delete the TempVoucherAccount
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_TempVoucherAccount_upd_delete]
	@Voucher_Acc_ID	char(15),
	@Scheme_Code char(10),
	@User_ID varchar(20),
	@tsmp timestamp
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
if (select tsmp from TempVoucherAccount 
		where Voucher_Acc_ID = @Voucher_Acc_ID AND Scheme_Code = @Scheme_Code) != @tsmp
begin
		RAISERROR('00011', 16, 1)
		return @@error
end
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	UPDATE [dbo].[TempVoucherAccount]
	SET
		Record_Status = 'D',
		Update_by=@User_ID,
		Update_dtm=getdate()
		--Voucher_Used=0,
		--Total_Voucher_Amt_Used=0
	WHERE 
		Voucher_Acc_ID = @Voucher_Acc_ID AND Scheme_Code = @Scheme_Code AND Record_Status = 'I'
	
END

GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_upd_delete] TO HCVU
GO
