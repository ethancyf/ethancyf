IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Professional_get_bySPID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Professional_get_bySPID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 12 June 2008
-- Description:	Retrieve professional information from table
--				"Professional"
-- =============================================
CREATE PROCEDURE [dbo].[proc_Professional_get_bySPID]
	@sp_id char(8)
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
	SELECT	Professional_Seq, SP_ID,
			Service_Category_Code, Registration_Code,
			Record_Status, Create_Dtm, Create_By
	FROM	Professional
	WHERE	SP_ID = @sp_id
	ORDER BY Professional_Seq
END
GO

GRANT EXECUTE ON [dbo].[proc_Professional_get_bySPID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_Professional_get_bySPID] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_Professional_get_bySPID] TO WSEXT
Go
