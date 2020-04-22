IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_checkTSMP]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_checkTSMP]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	11 Feb 2010
-- Description:		Grant right to [HCSP]
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 23 June 2008
-- Description:	Check TSMP Stamp
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	31 July 2009
-- Description:		Handle DBNull TSMP
-- =============================================
CREATE PROCEDURE [dbo].[proc_checkTSMP]
	@tsmp_1 timestamp,
	@tsmp_2 timestamp
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================

	if @tsmp_1 != @tsmp_2 or @tsmp_1 IS NULL or @tsmp_2 IS NULL
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
END

GO

GRANT EXECUTE ON [dbo].[proc_checkTSMP] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_checkTSMP] TO HCVU
GO
