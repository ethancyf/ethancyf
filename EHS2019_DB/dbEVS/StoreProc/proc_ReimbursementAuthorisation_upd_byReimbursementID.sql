IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementAuthorisation_upd_byReimbursementID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementAuthorisation_upd_byReimbursementID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	24 March 2015
-- CR No.:			INT15-0002
-- Description:		Set the stored procedure to recompile each time
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		13 August 2009
-- Description:		Update [ReimbursementAuthorisation]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================
CREATE PROCEDURE [dbo].[proc_ReimbursementAuthorisation_upd_byReimbursementID]
	@reimburse_id		char(15),
	@record_status		char(1),
	@update_by			varchar(20),
	@tsmp				timestamp
WITH RECOMPILE
AS BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF (
			SELECT TOP 1	TSMP 
			FROM			ReimbursementAuthorisation
			WHERE			Reimburse_ID = @reimburse_id
		) != @TSMP BEGIN
		RAISERROR('00011', 16, 1)
		RETURN @@error
	END
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================
	UPDATE		ReimbursementAuthorisation
	SET			Record_Status = @record_status,
				Update_By = @update_by,
				Update_Dtm = GETDATE()
	WHERE		Reimburse_ID = @reimburse_id

END
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementAuthorisation_upd_byReimbursementID] TO HCVU
GO
