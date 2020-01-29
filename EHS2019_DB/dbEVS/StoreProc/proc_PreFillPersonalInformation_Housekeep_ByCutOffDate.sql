IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PreFillPersonalInformation_Housekeep_ByCutOffDate]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PreFillPersonalInformation_Housekeep_ByCutOffDate]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	25 August 2016
-- CR No.:			CRE16-002
-- Description:		Stored procedure is not used anymore
-- =============================================
-- =============================================
-- Author:		Koala CHENG
-- CR No.:		INT13-0002
-- Create date: 15 Jan 2013
-- Description:	Housekeep prefill consent
--				1. Modify [PreFillPersonalInformation_bak] to actual table before execute
-- =============================================

/*
CREATE PROCEDURE [dbo].[proc_PreFillPersonalInformation_Housekeep_ByCutOffDate]
	@CutOffDate	datetime
AS
BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	-- Backup not used prefill consent
	INSERT INTO PreFillPersonalInformation_bak
	SELECT pfp.* FROM PreFillPersonalInformation pfp
					  LEFT JOIN PreFillUsage pfu
					  ON pfp.Pre_Fill_Consent_ID = pfu.Pre_Fill_Consent_ID
	WHERE pfu.Pre_Fill_Consent_ID IS NULL
		  AND pfp.Create_Dtm < @CutOffDate	

	-- Delete not used prefill consent
	DELETE PreFillPersonalInformation WHERE EXISTS (SELECT 1 FROM PreFillPersonalInformation pfp
																			  LEFT JOIN PreFillUsage pfu
																			  ON pfp.Pre_Fill_Consent_ID = pfu.Pre_Fill_Consent_ID
															WHERE pfu.Pre_Fill_Consent_ID IS NULL
																  AND pfp.Create_Dtm < @CutOffDate	
																  AND pfp.Pre_Fill_Consent_ID = PreFillPersonalInformation.Pre_Fill_Consent_ID)
END
GO

*/

