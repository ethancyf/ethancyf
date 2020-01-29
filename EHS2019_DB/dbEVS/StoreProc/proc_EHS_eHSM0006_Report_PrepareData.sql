IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSM0006_Report_PrepareData]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSM0006_Report_PrepareData]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	17 Jan 2018
-- CR No.:			CRE14-016
-- Description:		Introduce "Deceased" status into eHS
-- =============================================
-- =============================================    
-- CR No.:			CRE16-025 (Lowering voucher eligibility age)
-- Author:			Winnie SUEN
-- Create date:		26 May 2017
-- Description:		New Monthly report eHSM0006 which use the SFC logic
-- =============================================  
  
CREATE PROCEDURE [dbo].[proc_EHS_eHSM0006_Report_PrepareData]   
	@Cutoff_Dtm datetime = NULL
AS BEGIN
SET NOCOUNT ON;

-- =============================================  
-- Declaration  
-- =============================================  
    DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;
-- =============================================  
-- Validation   
-- =============================================  
-- =============================================  
-- Initialization  
-- =============================================  
	IF @Cutoff_Dtm IS NULL BEGIN
		SET @Cutoff_Dtm = CONVERT(VARCHAR(10), GETDATE(), 120)
	END

-- =============================================  
-- Generate data
-- =============================================  
	BEGIN TRY		
		-- Step 1: Prepare the account with SFC logic
		EXEC [proc_Prepare_RptAccountSFC] @Cutoff_Dtm, 0
	
		-- Step 2: Write report data to table
		EXEC [proc_EHS_eHSM0006_Report_Write] @Cutoff_Dtm
	

		SELECT 'S' as [Result]
	END TRY
	BEGIN CATCH

		-- Throw Error with the sproc name
		SELECT 
			@ErrorMessage = 'Execute Procedure ' + ERROR_PROCEDURE() + ' at Line ' + CAST(ERROR_LINE() AS VARCHAR(50)) + ' failed. ' + ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE()

		RAISERROR (	@ErrorMessage, @ErrorSeverity, @ErrorState )

	END CATCH

END       
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSM0006_Report_PrepareData] TO HCVU
GO
