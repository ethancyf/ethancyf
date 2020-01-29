IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccManualSubLOG_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccManualSubLOG_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 29 Oct 2009
-- Description:	Retrieve TempVoucherAccManualSubLOG
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_TempVoucherAccManualSubLOG_get]
AS
BEGIN
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
	SELECT
		[System_Dtm], [Voucher_Acc_ID], [Encrypt_Field1], [Doc_Code], [Record_Status]
	FROM [dbo].[TempVoucherAccManualSubLOG]
	ORDER BY [System_Dtm] ASC	
	
END

GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccManualSubLOG_get] TO HCVU
GO
