IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DeathRecordEntry_get_byFileID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DeathRecordEntry_get_byFileID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE20-023
-- Modified by:		Martin Tang
-- Modified date:	20 Apr 2021
-- Description:		Extend patient name's maximum length
-- =============================================
-- =============================================
-- Modification History
-- CR# :			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Author:			Chris YIM
-- Create date:		13 Dec 2017
-- CR No.:			CRE14-016 (To introduce "Deceased" status into eHS)
-- Description:		Get DeathRecordEntry(s) by File ID
-- =============================================

CREATE PROCEDURE [dbo].[proc_DeathRecordEntry_get_byFileID]
	@Death_Record_File_ID	char(15)
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
	DECLARE @Performance_Start_Dtm datetime
	SET @Performance_Start_Dtm = GETDATE()
	
	DECLARE @In_Death_Record_File_ID char(15)
	SET @In_Death_Record_File_ID = @Death_Record_File_ID
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	EXEC [proc_SymmetricKey_open]
-- =============================================
-- Return results
-- =============================================

	SELECT
		CONVERT(varchar, DecryptByKey(Encrypt_Field1)) AS [Document_No],
		Death_Record_File_ID,
		DOD,
		Exact_DOD,
		DOR,
		CONVERT(varchar(100), DecryptByKey(Encrypt_Field2)) AS [Death_English_Name],
		Record_Status
	FROM
		DeathRecordEntry
	WHERE
		Death_Record_File_ID = @In_Death_Record_File_ID
		AND Record_Status = 'A' -- A = Active

-- =============================================
-- Finialize
-- =============================================
	EXEC [proc_SymmetricKey_close]


	IF (SELECT Parm_Value1 FROM SystemParameters WHERE Parameter_Name = 'EnableSProcPerformCapture' AND Scheme_Code = 'ALL') = 'Y' BEGIN
		DECLARE @Performance_End_Dtm datetime
		SET @Performance_End_Dtm = GETDATE()
		
		EXEC proc_SProcPerformance_add 'proc_DeathRecordEntry_get_byFileID',
									   @In_Death_Record_File_ID,
									   @Performance_Start_Dtm,
									   @Performance_End_Dtm
	END
	
END
GO

GRANT EXECUTE ON [dbo].[proc_DeathRecordEntry_get_byFileID] TO PUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_DeathRecordEntry_get_byFileID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_DeathRecordEntry_get_byFileID] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_DeathRecordEntry_get_byFileID] TO WSEXT
GO

