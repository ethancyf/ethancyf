IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PCDStatusUpdateQueue_gen]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[proc_PCDStatusUpdateQueue_gen]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- ==========================================================================================
-- Modification History
-- CR No.:			
-- Modified by:	    
-- Modified date:   
-- Description:		
-- ==========================================================================================
-- ==========================================================================================
-- Author:	Koala CHENG
-- CR No.:	CRE17-016
-- Create Date:	17 Jul 2018
-- Description:	Generate record to table - [PCDStatusUpdateQueue]
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_PCDStatusUpdateQueue_gen]
AS BEGIN
-- ============================================================
-- Declaration
-- ============================================================
	DECLARE @current_date AS datetime
-- ============================================================
-- Validation
-- ============================================================
-- ============================================================
-- Initialization
-- ============================================================
	SET @current_date = GETDATE()
-- ============================================================
-- Return results
-- ============================================================

	-- Delete all Records in [PCDStatusUpdateQueue]
	DELETE FROM PCDStatusUpdateQueue

	-- Generate Record into [PCDStatusUpdateQueue]
	INSERT INTO PCDStatusUpdateQueue (
		SP_ID,
		Encrypt_Field1,
		Record_Status,
		Create_Dtm,
		Update_Dtm
	)
	SELECT DISTINCT SP.SP_ID, 
					SP.Encrypt_Field1,
					'P',
					@current_date,
					@current_date
	FROM ServiceProvider SP
		INNER JOIN Professional P
		ON SP.SP_ID = P.SP_ID AND P.Record_Status <> 'D' AND SP.Record_Status <> 'D'
		INNER JOIN ProfessionPracticeType PPT
		ON P.Service_Category_Code = PPT.Service_Category_Code

	-- Display Result
	SELECT 'Record(s) generated successfully:' + CONVERT(VARCHAR(10), COUNT(1)) FROM PCDStatusUpdateQueue

END
GO

GRANT EXECUTE ON [dbo].[proc_PCDStatusUpdateQueue_gen] TO HCVU
GO

