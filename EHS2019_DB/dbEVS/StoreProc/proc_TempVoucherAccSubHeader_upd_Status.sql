IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccSubHeader_upd_Status]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccSubHeader_upd_Status]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 26 Aug 2008
-- Description:	Update TempVoucherAccSubHeader Status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_TempVoucherAccSubHeader_upd_Status]
	@File_Name varchar(100),
	@Record_Status char(1),
	@Original_Status char(1)
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================

	IF (
		SELECT Count(1) FROM [dbo].[TempVoucherAccSubHeader]
		WHERE [File_Name] = @File_Name AND [Record_Status] = @Original_Status
	) != 1
	BEGIN
		RAISERROR('00011', 16, 1)
		return @@error
	END
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================
	UPDATE [dbo].[TempVoucherAccSubHeader]
		Set [Record_Status] = @Record_Status
	WHERE [File_Name] = @File_Name

END

GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccSubHeader_upd_Status] TO HCVU
GO
