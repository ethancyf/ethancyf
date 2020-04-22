IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccount_update_void]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccount_update_void]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Stanley, Chan
-- Create date: 29/5/2008	
-- Description:	update voucher transaction for void
-- =============================================
CREATE PROCEDURE [dbo].[proc_TempVoucherAccount_update_void]
	@voucher_acc_id char(15),
	@voucher_used smallint, 
	@total_voucher_amt_used money, 
	@update_by char(20),
	@dataEntry_by char(20), 
	@tsmp timestamp
as
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

if (select tsmp from TempVoucherAccount 
		where [Voucher_Acc_ID] = @voucher_acc_id) != @tsmp
begin
		RAISERROR('00011', 16, 1)
		return @@error
end

    -- Insert statements for procedure here
	UPDATE TempVoucherAccount
	SET 
	[Voucher_Used] = @voucher_used,
	[Total_Voucher_Amt_Used] = @total_voucher_amt_used,
	[Update_By] = @update_by,
	[DataEntry_By] = @dataEntry_by,
	[Update_Dtm] = getdate(),
	[Record_Status] = 'D'
	WHERE 
	[Voucher_Acc_ID] =@voucher_acc_id

END
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_update_void] TO HCSP
GO
