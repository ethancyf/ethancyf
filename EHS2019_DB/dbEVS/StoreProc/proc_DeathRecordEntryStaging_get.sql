IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DeathRecordEntryStaging_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DeathRecordEntryStaging_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		29 April 2011
-- CR No.:			CRE11-007
-- Description:		Get DeathRecordEntryStaging
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.:			
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_DeathRecordEntryStaging_get]
	@Death_Record_File_ID	char(15),
	@Start_Row				int,
	@Get_Summary			char(1),
	@Show_Fail_Record_Only	char(1)
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
	DECLARE @Total_Record			int
	DECLARE @Record_With_HKID		int
	DECLARE @Record_Without_HKID	int
	DECLARE @Fail_Record			int
	
	DECLARE @Row_Limit				int


-- =============================================
-- Initialization
-- =============================================
	SELECT
		@Row_Limit = Parm_Value1
	FROM
		SystemParameters
	WHERE
		Parameter_Name = 'DRMaxRowRetrieve'


	OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key

	
-- =============================================
-- Retrieve data
-- =============================================
	IF @Get_Summary = 'Y' BEGIN
		SELECT 
			@Total_Record = COUNT(1)
		FROM
			DeathRecordEntryStaging
		WHERE
			Death_Record_File_ID = @Death_Record_File_ID
			
		SELECT 
			@Record_With_HKID = COUNT(1)
		FROM
			DeathRecordEntryStaging
		WHERE
			Death_Record_File_ID = @Death_Record_File_ID
				AND Encrypt_Field1 <> EncryptByKey(KEY_GUID('sym_Key'), 'XXXXXXXXX')

		SELECT 
			@Record_Without_HKID = COUNT(1)
		FROM
			DeathRecordEntryStaging
		WHERE
			Death_Record_File_ID = @Death_Record_File_ID
				AND Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), 'XXXXXXXXX')
	
		SELECT 
			@Fail_Record = COUNT(1)
		FROM
			DeathRecordEntryStaging
		WHERE
			Death_Record_File_ID = @Death_Record_File_ID
				AND Result = 'F'
				
	END


-- =============================================
-- Return results
-- =============================================
	SET ROWCOUNT @Row_Limit

	SELECT
		Death_Record_File_ID,
		Seq_No,
		CONVERT(varchar, DecryptByKey(Encrypt_Field1)) AS [HKID],
		DOD,
		Exact_DOD,
		DOR,
		CONVERT(varchar, DecryptByKey(Encrypt_Field2)) AS [English_Name],
		Result,
		Fail_Type
	FROM
		DeathRecordEntryStaging
	WHERE
		Death_Record_File_ID = @Death_Record_File_ID
			AND Seq_No >= @Start_Row
			AND ((@Show_Fail_Record_Only = 'N') OR (@Show_Fail_Record_Only = 'Y' AND Result = 'F'))
	ORDER BY
		Seq_No
	
	SET ROWCOUNT 0
		
--

	IF @Get_Summary = 'Y' BEGIN
		SELECT
			@Total_Record AS [Total_Record],
			@Record_With_HKID AS [Record_With_HKID],
			@Record_Without_HKID AS [Record_Without_HKID],
			@Fail_Record AS [Fail_Record]
	END
	

-- =============================================
-- Finalizer
-- =============================================
	CLOSE SYMMETRIC KEY sym_Key		


END
GO

GRANT EXECUTE ON [dbo].[proc_DeathRecordEntryStaging_get] TO HCVU
GO
