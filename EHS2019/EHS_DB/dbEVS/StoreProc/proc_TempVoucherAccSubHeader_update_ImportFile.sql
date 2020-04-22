IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccSubHeader_update_ImportFile]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccSubHeader_update_ImportFile]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 30 Jun 2008
-- Description:	Update TempVoucherAccSubHeader Import File
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_TempVoucherAccSubHeader_update_ImportFile]
	@File_Name as varchar(100),	
	@File_Import_Content as image
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

	UPDATE [dbEVS_File].[dbo].[TempVoucherAccSubHeader_File]
	SET 
		[File_Import_Content] = @File_Import_Content
	WHERE
		[File_Name] = @File_Name		
END

GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccSubHeader_update_ImportFile] TO HCVU
GO
