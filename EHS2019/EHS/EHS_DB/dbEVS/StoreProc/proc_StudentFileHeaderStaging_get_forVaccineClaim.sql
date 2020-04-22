IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileHeaderStaging_get_forVaccineClaim]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileHeaderStaging_get_forVaccineClaim]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	26 August 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Get StudentFileHeaderStaging for vaccination checking
--					1. Final report generation (sort by service date)
-- =============================================    


CREATE PROCEDURE [dbo].[proc_StudentFileHeaderStaging_get_forVaccineClaim]
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
	--DECLARE @Check_Dtm AS DATETIME
	--SELECT @Check_Dtm = CONVERT(varchar, GETDATE(), 106) -- "106" gives "dd MMM yyyy"  
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	-- 1st Priority
	--		Reach the date for final report generation
	--		Process earliest service date first
	SELECT DISTINCT H.Student_File_ID, 1 AS [Priority], Service_Receive_Dtm AS [Priority_Date]
	FROM StudentFileheaderStaging H
		--INNER JOIN StudentFileEntryStaging E
		--ON H.Student_File_ID = E.Student_File_ID
	WHERE 
		H.Record_Status = 'PT' -- << Processing vaccination claim
		--AND E.Acc_Type IS NOT NULL -- << Checked account
		--AND E.Vaccination_Process_Stage = 'CALENTITLE' -- << Checked HA, DH vaccine and Dose entitlement
		--AND E.Injected = 'Y' -- << student is vaccinated actually
		--AND (E.Transaction_ID IS NULL AND E.Transaction_Result IS  NULL) -- << Not yet created claim
	ORDER BY [Priority], [Priority_Date]

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileHeaderStaging_get_forVaccineClaim] TO HCVU
GO
