IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCSPUserAC_upd_RecordStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCSPUserAC_upd_RecordStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:			Tommy Cheung
-- Create date:		18-06-2008
-- Description:		Update HCSPUserAC Record Status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE procedure [dbo].[proc_HCSPUserAC_upd_RecordStatus]
@SP_ID			char(8),
@Record_Status  char(1),
@Update_By		varchar(20),
@TSMP			timestamp
as

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF (SELECT TSMP FROM HCSPUserAC
		WHERE SP_ID = @SP_ID) != @TSMP
	BEGIN
		RAISERROR('00011', 16, 1)
		return @@error
	END
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================
UPDATE HCSPUserAC
SET Record_Status=@Record_Status
      ,Update_Dtm=getdate()
	  ,Update_by=@Update_By
WHERE SP_ID = @SP_ID
GO

GRANT EXECUTE ON [dbo].[proc_HCSPUserAC_upd_RecordStatus] TO HCVU
GO
