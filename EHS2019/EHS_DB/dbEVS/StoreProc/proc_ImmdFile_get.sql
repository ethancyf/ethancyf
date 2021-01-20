IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ImmdFile_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ImmdFile_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 30 Jun 2008
-- Description:	Retrieve File Generation Queue With File Content
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_ImmdFile_get]
	@curDate as DateTime
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
EXEC [proc_SymmetricKey_open]
-- =============================================
-- Return results
-- =============================================

Declare @curDateOnly as datetime
Set @curDateOnly = DATEADD(dd, -DATEDIFF(dd, @curDate, 1), 1)

SELECT
	VASHF.[File_Name],
	VASHF.[File_Export_Content],
	VASHF.[File_Import_Content]

FROM [dbo].[TempVoucherAccSubHeader] VASH, [dbEVS_File].[dbo].[TempVoucherAccSubHeader_File] VASHF

WHERE
	VASH.System_Dtm > @curDateOnly AND 
	VASH.System_Dtm < DateAdd(Day, 1,@curDateOnly) AND
	VASH.File_Name = VASHF.File_Name

EXEC [proc_SymmetricKey_close]

END

GO

GRANT EXECUTE ON [dbo].[proc_ImmdFile_get] TO HCVU
GO
