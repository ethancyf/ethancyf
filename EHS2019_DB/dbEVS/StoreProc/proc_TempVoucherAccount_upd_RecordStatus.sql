IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccount_upd_RecordStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccount_upd_RecordStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Billy Lam
-- Create date:		31 July 2008
-- Description:		Delete TempVoucher Account
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE Procedure proc_TempVoucherAccount_upd_RecordStatus
@Temp_Voucher_Acc_ID	char(15)
, @Update_By			varchar(20)
, @Update_Dtm			datetime
, @Record_Status		char(1)
, @tsmp					timestamp
as

-- =============================================
-- Validation 
-- =============================================
if (select tsmp from TempVoucherAccount
		where Voucher_Acc_ID = @Temp_Voucher_Acc_ID) != @tsmp
begin
	Raiserror('00011', 16, 1)
	return @@error
end

-- =============================================
-- Update Transaction 
-- =============================================
if @Record_Status = 'P' 
begin
		Update TempVoucherAccount
		set Record_Status = @Record_Status
		, Update_By = @Update_By
		, Update_Dtm = @Update_Dtm
		, confirm_dtm = @Update_Dtm
		where Voucher_Acc_ID = @Temp_Voucher_Acc_ID
end
else
begin
		Update TempVoucherAccount
		set Record_Status = @Record_Status
		, Update_By = @Update_By
		, Update_Dtm = @Update_Dtm
		where Voucher_Acc_ID = @Temp_Voucher_Acc_ID
end


GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_upd_RecordStatus] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_upd_RecordStatus] TO HCVU
GO
