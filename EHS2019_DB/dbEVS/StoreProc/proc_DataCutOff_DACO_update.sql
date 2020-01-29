IF EXISTS (
SELECT 1 FROM Sys.Objects
WHERE [name] = 'proc_DataCutOff_DACO_update' AND [type] = 'P')
BEGIN
	DROP PROCEDURE proc_DataCutOff_DACO_update
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ==========================================================================================
-- Author:	Timothy LEUNG
-- Create Date:	17 Dec 2012
-- Description:	Update the cutoff time in DataCutOff_DACO
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_DataCutOff_DACO_update]
	@DataTypeID varchar(20),
	@CutOffDtm datetime
AS
BEGIN
-- ============================================================
-- Declaration
-- ============================================================
-- ============================================================
-- Validation
-- ============================================================
-- ============================================================
-- Initialization
-- ============================================================
-- ============================================================
-- Return results
-- ============================================================
Update DataCutOff_DACO
	set DACO_CutOff_Dtm = @CutOffDtm, DACO_Update_Dtm = getdate()
where 
	DACO_DataType_ID = @DataTypeID
	
END
GO

GRANT EXECUTE ON [dbo].[proc_DataCutOff_DACO_update] TO HCVU
GO