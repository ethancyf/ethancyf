IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransactionConfirm_upd]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransactionConfirm_upd]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Billy Lam
-- Create date:		31 July 2008
-- Description:		Confirm VoucherTansaction
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE Procedure proc_VoucherTransactionConfirm_upd
@Transaction_ID		char(20)
, @SP_ID			char(8)
, @Confirmed_Dtm	datetime
, @Record_Status	char(1)
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
Update VoucherTransaction
set Record_Status = @Record_Status
, Confirmed_Dtm = @Confirmed_Dtm
, Update_By = @SP_ID
, Update_Dtm = getdate()
where Transaction_ID = @Transaction_ID

GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransactionConfirm_upd] TO HCSP
GO
