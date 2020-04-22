IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_AccountChangeMaintenance_upd_ForceReject]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_AccountChangeMaintenance_upd_ForceReject]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- CR No.:			
-- Description:	
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		3 February 2017
-- CR No.:			CRE16-019 (To implement token sharing between eHS(S) and eHRSS)
-- Description:		Force reject the outstanding account change confirmation request
-- =============================================

CREATE PROCEDURE [dbo].[proc_AccountChangeMaintenance_upd_ForceReject]
	@SP_ID			char(8),
	@Upd_Type		varchar(2),
	@Update_By		char(20)
AS BEGIN

-- =============================================
-- Process
-- =============================================

	UPDATE
		SPAccountMaintenance
	SET
		Record_Status = 'R',
		Confirmed_By = @Update_By,
		Update_By = @Update_By,
		Confirm_Dtm = GETDATE()
	WHERE
		SP_ID = @SP_ID
			AND Upd_Type = @Upd_Type
			AND Record_Status = 'A'


END
GO

GRANT EXECUTE ON [dbo].[proc_AccountChangeMaintenance_upd_ForceReject] TO WSEXT
GO
