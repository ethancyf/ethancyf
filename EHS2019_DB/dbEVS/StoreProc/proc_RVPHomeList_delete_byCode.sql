IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_RVPHomeList_delete_byCode]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_RVPHomeList_delete_byCode]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
-- =============================================
-- Author:		Twinsen CHAN
-- CR No.:		CRE11-013
-- Create date: 25 Feb 2013
-- Description:	Delete RCH Home record
-- =============================================
CREATE PROCEDURE [dbo].[proc_RVPHomeList_delete_byCode]
	@RCH_code	varchar(10),
	@TSMP		timestamp
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

	IF (SELECT TSMP
		FROM [dbo].[RVPHomeList] 
		WHERE RCH_code = @RCH_code) <> @TSMP
	BEGIN
			RAISERROR('00006', 16, 1)
			RETURN @@error
	END

	DELETE FROM RVPHomeList 
	WHERE 
		RCH_code = @RCH_code 
		AND TSMP = @TSMP
END
GO

GRANT EXECUTE ON [dbo].[proc_RVPHomeList_delete_byCode] TO HCVU
GO
