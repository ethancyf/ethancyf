IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeTSMP_get_bySPID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeTSMP_get_bySPID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kathy LEE
-- Create date: 29 June 2008
-- Description:	Get timestamp in Table "Practice"
-- =============================================
CREATE PROCEDURE [dbo].[proc_PracticeTSMP_get_bySPID]
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

	Select	Display_Seq, TSMP
	FROM	Practice
	WHERE	SP_ID = @sp_id 
END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeTSMP_get_bySPID] TO HCVU
GO
