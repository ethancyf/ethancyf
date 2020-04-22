IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccManualMatchLOGRowCount_get_ByAccID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccManualMatchLOGRowCount_get_ByAccID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kathy LEE
-- Create date: 24 March 2010
-- Description:	Get row count of the TempVoucherAccManualMatchLOG by voucher
--				account id
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_TempVoucherAccManualMatchLOGRowCount_get_ByAccID]
	@Voucher_Acc_ID char(15)
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
declare @cnt_Submission as int
declare @cnt_Match as int
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

select @cnt_Submission = count(voucher_acc_id)
from TempVoucherAccManualSubLOG
where voucher_acc_id = @Voucher_Acc_Id
group by voucher_acc_id

select @cnt_Match = count(voucher_acc_id)
from TempVoucherAccManualMatchLOG
where voucher_acc_id = @Voucher_Acc_Id
group by voucher_acc_id

if isnull(@cnt_Submission,'') = isnull(@cnt_Match,'')
	begin
		select 0
	end
else
	begin
		select 1
	end

END

GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccManualMatchLOGRowCount_get_ByAccID] TO HCVU
GO
