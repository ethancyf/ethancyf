IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_FileGenerationQueueAdditionalParameter_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_FileGenerationQueueAdditionalParameter_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		7 June 2016
-- CR No.:			CRE15-016
-- Description:		Insert FileGenerationQueueAdditionalParameter
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.:			
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_FileGenerationQueueAdditionalParameter_add]
	@Generation_ID	char(12),
	@Parm_Name		varchar(30),
	@Parm_Seq		int,
	@Parm_Value		varchar(100)

AS BEGIN
-- =============================================
-- Execution
-- =============================================
	INSERT INTO FileGenerationQueueAdditionalParameter (
		Generation_ID,
		Parm_Name,
		Parm_Seq,
		Parm_Value
	) VALUES (
		@Generation_ID,
		@Parm_Name,
		@Parm_Seq,
		@Parm_Value
	)

END
GO

GRANT EXECUTE ON [dbo].[proc_FileGenerationQueueAdditionalParameter_add] TO HCVU
GO
