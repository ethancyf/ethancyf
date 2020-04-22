IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccSubHeader_add_ExportFile]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccSubHeader_add_ExportFile]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 30 Jun 2008
-- Description:	Insert TempVoucherAccSubHeader Export File
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_TempVoucherAccSubHeader_add_ExportFile]
	@File_Name as varchar(100),	
	@File_Export_Content as image
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

	INSERT INTO [dbEVS_File].[dbo].[TempVoucherAccSubHeader_File]
		([File_Name], [File_Export_Content], [File_Import_Content])
	VALUES
		(@File_Name, @File_Export_Content, Null)
	
END

GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccSubHeader_add_ExportFile] TO HCVU
GO
