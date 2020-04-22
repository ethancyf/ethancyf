IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Interface_SystemProfile_Get_ByProfileID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Interface_SystemProfile_Get_ByProfileID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		21 September 2010
-- Description:		Get system profile number
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		(Next developer)
-- Modified date:	(To-do)
-- Description:		Handle different Profile_Type
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_Interface_SystemProfile_Get_ByProfileID]
	@Profile_ID		char(10),
	@Scheme_Code	char(10)
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Initialization
-- =============================================

-- ---------------------------------------------
-- Set @Scheme_Code to ALL if NULL
-- ---------------------------------------------
	IF @Scheme_Code IS NULL BEGIN
		SET @Scheme_Code = 'ALL'
	END


-- ---------------------------------------------
-- Add the profile number by 1 (also lock the table to avoid concurrent access)
-- ---------------------------------------------
	UPDATE
		SystemProfile
	SET
		Current_Num = Current_Num + 1
	WHERE
		Profile_ID = @Profile_ID
			AND Scheme_Code = @Scheme_Code


-- ---------------------------------------------
-- Check if the profile number reaches maximum
-- ---------------------------------------------
	
	IF (
		SELECT
			COUNT(1)
		FROM
			SystemProfile
		WHERE
			Profile_ID = @Profile_ID
				AND Scheme_Code = @Scheme_Code
				AND Current_Num > End_Num
	) <> 0 BEGIN
		UPDATE
			SystemProfile
		SET
			Current_Num = Start_Num + 1
		WHERE
			Profile_ID = @Profile_ID
				AND Scheme_Code = @Scheme_Code

	END
			

-- =============================================
-- Return result
-- =============================================

	SELECT
		Current_Num,
		Profile_Prefix
	FROM
		SystemProfile
	WHERE
		Profile_ID = @Profile_ID
			AND Scheme_Code = @Scheme_Code


END 
GO

GRANT EXECUTE ON [dbo].[proc_Interface_SystemProfile_Get_ByProfileID] TO WSEXT
GO
