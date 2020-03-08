IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DeathRecordEntry_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DeathRecordEntry_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================  
-- Modification History  
-- CR No.:			CRE14-019
-- Modified by:		Lawrence TSANG
-- Modified date:	21 January 2015
-- Description:		Insert into [SProcPerformance] to record sproc performance
-- =============================================  
-- =============================================
-- Modification History
-- CR No.:			INT12-0008
-- Modified by:	Tommy TSE
-- Modified date:	29 Aug 2012
-- Description:	Rectify stored procedure permission
-- =============================================
-- =============================================
-- Author:			Koala Cheng
-- Create date:	19 May 2011
-- CR No.:			CRE11-007
-- Description:	Get DeathRecordEntry
-- =============================================

CREATE PROCEDURE [dbo].[proc_DeathRecordEntry_get]
	@Doc_No	VARCHAR(20)
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
	DECLARE @Performance_Start_Dtm datetime
	SET @Performance_Start_Dtm = GETDATE()
	
	DECLARE @In_Doc_No varchar(20)
	SET @In_Doc_No = @Doc_No
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	OPEN SYMMETRIC KEY sym_Key
	DECRYPTION BY ASYMMETRIC KEY asym_Key
-- =============================================
-- Return results
-- =============================================

	SELECT
		CONVERT(varchar, DecryptByKey(Encrypt_Field1)) AS [Document_No],
		Death_Record_File_ID,
		DOD,
		Exact_DOD,
		DOR,
		CONVERT(varchar, DecryptByKey(Encrypt_Field2)) AS [Death_English_Name],
		Record_Status
	FROM
		DeathRecordEntry
	WHERE
		Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @In_Doc_No)
		AND Record_Status = 'A' -- A = Active

-- =============================================
-- Finialize
-- =============================================
	CLOSE SYMMETRIC KEY sym_Key


	IF (SELECT Parm_Value1 FROM SystemParameters WHERE Parameter_Name = 'EnableSProcPerformCapture' AND Scheme_Code = 'ALL') = 'Y' BEGIN
		DECLARE @Performance_End_Dtm datetime
		SET @Performance_End_Dtm = GETDATE()
		
		EXEC proc_SProcPerformance_add 'proc_DeathRecordEntry_get',
									   @In_Doc_No,
									   @Performance_Start_Dtm,
									   @Performance_End_Dtm
	END
	
END
GO

GRANT EXECUTE ON [dbo].[proc_DeathRecordEntry_get] TO PUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_DeathRecordEntry_get] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_DeathRecordEntry_get] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_DeathRecordEntry_get] TO WSEXT
GO