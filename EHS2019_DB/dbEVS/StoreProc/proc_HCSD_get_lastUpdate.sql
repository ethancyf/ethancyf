IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCSD_get_lastUpdate]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCSD_get_lastUpdate]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	22 Aug 2016
-- CR No.:			CRE16-002
-- Description:		Obsolete sproc
-- =============================================
-- =============================================
-- Author:		Mattie LO
-- Create date: 02 0OCT 2009
-- Description:	 Retrieve the last update date
-- =============================================
/*
CREATE PROCEDURE [dbo].[proc_HCSD_get_lastUpdate] 
AS
BEGIN
	SELECT [UpdDate]
		FROM [dbo].[SDSystemResource]

END
GO

GRANT EXECUTE ON [dbo].[proc_HCSD_get_lastUpdate] TO HCPUBLIC
GO
*/
