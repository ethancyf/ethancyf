IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccMatchLOG_count_Processed]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccMatchLOG_count_Processed]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Dedrick Ng
-- Create date: 2 Oct 2009
-- Description:	Count records with Processed = 'Y'
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_TempVoucherAccMatchLOG_count_Processed]
	@System_Dtm datetime,
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
	SELECT COUNT(1) 
	From [dbo].[TempVoucherAccMatchLOG]
	WHERE System_Dtm = @System_Dtm
	AND File_Name = @File_Name
	AND Processed = 'N'
	
END

GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccMatchLOG_count_Processed] TO HCVU
GO
