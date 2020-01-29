IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccMatchLOG_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccMatchLOG_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 26 Aug 2008
-- Description:	Insert TempVoucherAccMatchLOG for Import File ImmD
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Dedrick Ng
-- Modified date: 2 Oct 2009
-- Description:	Add New fields
-- =============================================
CREATE PROCEDURE [dbo].[proc_TempVoucherAccMatchLOG_add]
	@System_Dtm datetime,
	@Voucher_Acc_ID char(15),
	@Return_Dtm datetime,
	@Valid_HKID char(1),
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
	INSERT INTO [dbo].[TempVoucherAccMatchLOG]
	(
		System_Dtm,
		Voucher_Acc_ID,
		Return_Dtm,
		Valid_HKID,
		File_Name,
		Processed		
	)
	VALUES
	(
		@System_Dtm,
		@Voucher_Acc_ID,
		@Return_Dtm,
		@Valid_HKID,
		@File_Name,
		'N'
	)
	
END

GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccMatchLOG_add] TO HCVU
GO
