IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemVariable_upd]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemVariable_upd]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		19 October 2016
-- CR No.:			CRE16-019 (To implement token sharing between eHS(S) and eHRSS)
-- Description:		Update SystemVariable
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.:			
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_SystemVariable_upd]
	@Variable_ID		varchar(50),
	@Variable_Value		nvarchar(500),
	@Update_By			varchar(20),
	@TSMP				binary(8)
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================

	IF @TSMP IS NOT NULL BEGIN
		IF (SELECT TSMP FROM SystemVariable WHERE Variable_ID = @Variable_ID) != @TSMP BEGIN
			RAISERROR('00011', 16, 1)
			RETURN @@error
		END

	END


-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Process
-- =============================================

	UPDATE
		SystemVariable
	SET
		Variable_Value = @Variable_Value,
		Update_Dtm = GETDATE(),
		Update_By = @Update_By
	WHERE
		Variable_ID = @Variable_ID


END
GO

GRANT EXECUTE ON [dbo].[proc_SystemVariable_upd] TO HCVU, HCSP, WSEXT
GO
