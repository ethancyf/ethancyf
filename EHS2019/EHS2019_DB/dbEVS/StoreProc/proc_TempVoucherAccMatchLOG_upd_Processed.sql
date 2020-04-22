IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccMatchLOG_upd_Processed]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccMatchLOG_upd_Processed]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Dedrick Ng
-- Create date: 2 Oct 2009
-- Description:	Update Processed to 'Y'
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_TempVoucherAccMatchLOG_upd_Processed]
	@System_Dtm datetime,
	@Voucher_Acc_ID char(15),
	@File_Name varchar(100)

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
	UPDATE [dbo].[TempVoucherAccMatchLOG]
	SET Processed = 'Y'
	WHERE System_Dtm = @System_Dtm
	AND Voucher_Acc_ID = @Voucher_Acc_ID
	AND File_Name = @File_Name
	
END

GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccMatchLOG_upd_Processed] TO HCVU
GO
