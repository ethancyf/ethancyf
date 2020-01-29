IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemParameters_UpdateValue]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemParameters_UpdateValue]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE15-001
-- Modified by:		Winnie SUEN
-- Modified date:	26 Nov 2015
-- Description:		Grant to HCSP user
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		1 December 2010
-- Description:		Update the Parm_Value1 of parameter @Parameter_Name
-- =============================================

CREATE PROCEDURE [dbo].[proc_SystemParameters_UpdateValue]
	@Parameter_Name		char(50),
	@Parm_Value1		nvarchar(510),
	@Update_By			varchar(20)
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Execute
-- =============================================
	UPDATE
		SystemParameters
	SET
		Parm_Value1 = @Parm_Value1,
		Update_Dtm = GETDATE(),
		Update_By = @Update_By
	WHERE
		Parameter_Name = @Parameter_Name
	

END
GO

GRANT EXECUTE ON [dbo].[proc_SystemParameters_UpdateValue] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_SystemParameters_UpdateValue] TO HCSP
GO
