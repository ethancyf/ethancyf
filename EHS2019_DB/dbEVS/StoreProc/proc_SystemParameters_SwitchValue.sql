IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemParameters_SwitchValue]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemParameters_SwitchValue]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		1 December 2010
-- Description:		Switch the Parm_Value1 and Parm_Value2 of parameter @Parameter_Name
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Koala Cheng
-- Modified date:	28 January 2011 
-- Description:		[CRE11-002] Force switch old OC4J url 'CMS_Get_Vaccine_WS_Url'
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Koala Cheng
-- Modified date:	09 February 2011 
-- Description:		[INT11-0016] Rollback the fix on last promotion, 'CMS_Get_Vaccine_WS_Url' will be obsolete
-- =============================================

CREATE PROCEDURE [dbo].[proc_SystemParameters_SwitchValue]
	@Parameter_Name		char(50),
	@Update_By			varchar(20)
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
	DECLARE @Parm_Value1	nvarchar(510)
	DECLARE @Parm_Value2	nvarchar(510)
	
	
-- =============================================
-- Retrieve data
-- =============================================
	SELECT
		@Parm_Value1 = Parm_Value1,
		@Parm_Value2 = Parm_Value2
	FROM
		SystemParameters
	WHERE
		Parameter_Name = @Parameter_Name


-- =============================================
-- Execute
-- =============================================
	UPDATE
		SystemParameters
	SET
		Parm_Value1 = @Parm_Value2,
		Parm_Value2 = @Parm_Value1,
		Update_Dtm = GETDATE(),
		Update_By = @Update_By
	WHERE
		Parameter_Name = @Parameter_Name


END
GO

GRANT EXECUTE ON [dbo].[proc_SystemParameters_SwitchValue] TO HCVU
GO
