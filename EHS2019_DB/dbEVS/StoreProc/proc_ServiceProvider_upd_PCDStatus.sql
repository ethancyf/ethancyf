IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProvider_upd_PCDStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProvider_upd_PCDStatus]
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
-- Author:		Koala CHENG
-- CR No.:		CRE17-016
-- Create Date:	17 Jul 2018
-- Description:	Update PCD Status in table [ServiceProvider] after check PCD status
-- ==========================================================================================
CREATE PROCEDURE [dbo].[proc_ServiceProvider_upd_PCDStatus]
	@SP_ID	CHAR(8),
	@PCD_Account_Status CHAR(1),
	@PCD_Enrolment_Status CHAR(1),
	@PCD_Professional VARCHAR(20),
	@PCD_Status_Last_Check_Dtm DATETIME,
	@Update_By VARCHAR(20), 
	@TSMP TIMESTAMP
AS
BEGIN
	
	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF (SELECT TSMP FROM ServiceProvider
		WHERE SP_ID = @SP_ID) != @TSMP
	BEGIN
		RAISERROR('00011', 16, 1)
		RETURN @@error
	END
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================
	UPDATE ServiceProvider
	SET PCD_Account_Status = @PCD_Account_Status,
		PCD_Enrolment_Status = @PCD_Enrolment_Status,
		PCD_Professional = @PCD_Professional,
		PCD_Status_Last_Check_Dtm = @PCD_Status_Last_Check_Dtm,
		update_by = @Update_By,
		update_dtm = GETDATE()
	WHERE SP_ID = @SP_ID
		AND TSMP = @TSMP

END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProvider_upd_PCDStatus] TO HCVU
GO


GRANT EXECUTE ON [dbo].[proc_ServiceProvider_upd_PCDStatus] TO HCSP
GO
