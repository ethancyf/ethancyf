IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccount_del]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccount_del]
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
-- Modified by: Clark YIP
-- Modified date:  27 Oct 2008
-- Description:	 Mark the record status to D and no physically delete the record
-- =============================================
CREATE Procedure dbo.proc_TempVoucherAccount_del
@Temp_Voucher_Acc_ID	char(15)
, @Update_By			char(8)
, @Update_Dtm			datetime
, @tsmp					timestamp
as

-- =============================================
-- Validation 
-- =============================================
if not @tsmp is null
begin
	if (select tsmp from TempVoucherAccount
			where Voucher_Acc_ID = @Temp_Voucher_Acc_ID) != @tsmp
	begin
		Raiserror('00011', 16, 1)
		return @@error
	end
end

-- Update TempVoucherAccount info to log into log table
Update TempVoucherAccount
set Update_By = @Update_By
, Update_Dtm = @Update_Dtm
, Record_Status= 'D'
where Voucher_Acc_ID = @Temp_Voucher_Acc_ID


-- =============================================
-- Delete Transaction
-- =============================================
--Delete from TempVoucherAccount
--where Voucher_Acc_ID = @Temp_Voucher_Acc_ID


GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_del] TO HCSP
GO
