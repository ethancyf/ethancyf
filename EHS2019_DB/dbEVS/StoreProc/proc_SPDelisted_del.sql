IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SPDelisted_del]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SPDelisted_del]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Tommy Cheung
-- Create date: 15 July 2008
-- Description:	Update the Return Info in Table
--				"SPDelisted"
-- =============================================

-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_SPDelisted_del]
	@SP_ID char(8),
	@TSMP timestamp
AS
BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF (SELECT TSMP FROM SPDelisted
		WHERE SP_ID = @SP_ID ) != @TSMP
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

	DELETE FROM SPDelisted
	WHERE	SP_ID = @SP_ID;
END
GO

GRANT EXECUTE ON [dbo].[proc_SPDelisted_del] TO HCVU
GO
