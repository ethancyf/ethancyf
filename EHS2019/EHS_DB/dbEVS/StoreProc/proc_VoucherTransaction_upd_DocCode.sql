IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransaction_upd_DocCode]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransaction_upd_DocCode]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.:			
-- Description:		
-- =============================================	
-- =============================================
-- Author:			Winnie SUEN
-- Modified date:	26 Augest 2020
-- CR No.:			CRE20-003 (Enhancement on Programme or Scheme using batch upload)
-- Description:		Update Doc Code of Transaction when change doc code in Vaccination File Rectification
-- =============================================

CREATE PROCEDURE [dbo].[proc_VoucherTransaction_upd_DocCode]
	@Transaction_ID		CHAR(20),
	@Doc_Code			CHAR(20),
	@Update_by			VARCHAR(20),
	@Update_dtm			DATETIME
AS BEGIN

	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================
	UPDATE
		VoucherTransaction
		
	SET 				
		Doc_Code = @Doc_Code,
		Update_By = @Update_by,
		Update_Dtm = @Update_dtm
		
	WHERE 
		Transaction_ID = @transaction_id

END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransaction_upd_DocCode] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransaction_upd_DocCode] TO HCSP
GO
