IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransactionConfirm_upd_ConfirmedTransactionStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransactionConfirm_upd_ConfirmedTransactionStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Karl Lam
-- Create date:		08 Apr 2013
-- Description:		proc_VoucherTransactionConfirm_upd_ConfirmedTransactionStatus
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE Procedure proc_VoucherTransactionConfirm_upd_ConfirmedTransactionStatus
@Transaction_ID		char(20)
, @SP_ID			char(8)
, @Confirmed_Dtm	datetime
, @tsmp				timestamp
as
-- =============================================
-- Validation 
-- =============================================
if (select tsmp from VoucherTransaction 
		where Transaction_ID = @Transaction_ID) != @tsmp
begin
	Raiserror('00011', 16, 1)
	return @@error
end

-- =============================================
-- Update Transaction 
-- =============================================
UPDATE	VoucherTransaction
SET		Record_Status	= sc.Confirmed_Transaction_Status
		, Confirmed_Dtm = @Confirmed_Dtm
		, Update_By		= @SP_ID
		, Update_Dtm	= getdate()
FROM	voucherTransaction vt INNER JOIN schemeClaim sc
		on sc.Scheme_Code = vt.Scheme_Code and sc.Record_Status = 'A'
WHERE	vt.Transaction_ID = @Transaction_ID

GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransactionConfirm_upd_ConfirmedTransactionStatus] TO HCSP
GO
