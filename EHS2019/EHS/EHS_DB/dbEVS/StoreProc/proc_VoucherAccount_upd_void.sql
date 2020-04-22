IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherAccount_upd_void]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherAccount_upd_void]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.			CRE16-026-03 (Add PCV13)
-- Modified by:		Lawrence TSANG
-- Modified date:	17 October 2017
-- Description:		Stored procedure not used anymore
-- =============================================
-- =============================================
-- Author:		Clark YIP
-- Create date: 06/10/2008	
-- Description:	update voucher account for void by HCVU
-- =============================================

/*
CREATE PROCEDURE [dbo].[proc_VoucherAccount_upd_void]
	-- Add the parameters for the stored procedure here
	@transaction_id char(20),
	@voucher_used smallint, 
	@total_voucher_amt_used money, 
	@update_by char(20)	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
declare @voucher_acc_id char(15)
-- =============================================
-- Initialization
-- =============================================
	select @voucher_acc_id=Voucher_acc_id from VoucherTransaction
	where Transaction_id = @transaction_id

	UPDATE VoucherAccount
	SET 
	[Voucher_Used] = [Voucher_Used] - @voucher_used,
	[Total_Voucher_Amt_Used] = [Total_Voucher_Amt_Used] - @total_voucher_amt_used,
	[Update_By] = @update_by,
	[Update_Dtm] = getdate()
	WHERE 
	[Voucher_Acc_ID] =@voucher_acc_id

END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccount_upd_void] TO HCVU
GO
*/
