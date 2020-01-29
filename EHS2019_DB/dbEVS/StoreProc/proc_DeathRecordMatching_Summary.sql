IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DeathRecordMatching_Summary]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DeathRecordMatching_Summary]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		24 May 2011
-- CR No.:			CRE11-007
-- Description:		Get the summary of Death Record Matching for inbox
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.:			
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_DeathRecordMatching_Summary]
	@Death_Record_File_ID	char(15)
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Note 
-- =============================================
-- The matching result of Death Record File [ @Death_Record_File_ID ] was generated successfully.
-- Please click here to visit the "eHealth Account Death Record Matching Result" to review the result.
-- 
-- Matching Result Summary
-- Matching time							[ @MatchTime ]
-- Total no. of death records in file		[ %TotalNoOfRecord% ]
-- No. of death records with HKID provided	[ @No_of_Record_With_HKID ]
-- No. of death records with HKID provided	[ @No_of_Record_With_HKID ]
-- No. of validated account matched			[ @No_of_Validated_Account ]
-- No. of temporary account matched			[ @No_of_Temporary_Account ]
-- 
-- Matched Account Type		Name Matched	With Claims		With Suspicious Claims		No. of Matched eHA Account
-- Validated				Y				N				N/A							[ @VYN ]
-- Validated				Y				Y				N							[ @VYYN ]
-- Validated				Y				Y				Y							[ @VYYY ]
-- Validated				N				N				N/A							[ @VNN ]
-- Validated				N				Y				N							[ @VNYN ]
-- Validated				N				Y				Y							[ @VNYY ]
-- Temporary																			[ @T ]
-- 
-- eHealth System Administrator


-- =============================================
-- Declaration
-- =============================================
	DECLARE @Match_Dtm					datetime
	DECLARE @Total_No_of_Record			int
	DECLARE @No_of_Record_With_HKID		int
	DECLARE @No_of_Validated_Account	int
	DECLARE @No_of_Temporary_Account	int
	DECLARE @VYN						int
	DECLARE @VYYN						int
	DECLARE @VYYY						int
	DECLARE @VNN						int
	DECLARE @VNYN						int
	DECLARE @VNYY						int
	DECLARE @T							int
	
	
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Retrieve data
-- =============================================
	SELECT
		@Match_Dtm = Match_Dtm
	FROM
		DeathRecordFileHeader
	WHERE
		Death_Record_File_ID = @Death_Record_File_ID

--

	SELECT
		@Total_No_of_Record = COUNT(1)
	FROM
		DeathRecordEntryStaging
	WHERE
		Death_Record_File_ID = @Death_Record_File_ID

--

	SELECT
		@No_of_Record_With_HKID = COUNT(1)
	FROM
		DeathRecordEntry
	WHERE
		Death_Record_File_ID = @Death_Record_File_ID

--
	
	SELECT
		@No_of_Validated_Account = COUNT(1)
	FROM
		DeathRecordMatchResult
	WHERE
		Death_Record_File_ID = @Death_Record_File_ID
			AND EHA_Acc_Type = 'V'

--

	SELECT
		@No_of_Temporary_Account = COUNT(1)
	FROM
		DeathRecordMatchResult
	WHERE
		Death_Record_File_ID = @Death_Record_File_ID
			AND EHA_Acc_Type = 'T'

--

	SELECT
		@VYN = COUNT(1)
	FROM
		DeathRecordMatchResult R
			INNER JOIN DeathRecordEntry E
				ON R.Encrypt_Field1 = E.Encrypt_Field1
					AND R.Death_Record_File_ID = E.Death_Record_File_ID
			INNER JOIN PersonalInformation VP
				ON R.EHA_Acc_ID = VP.Voucher_Acc_ID
					AND R.EHA_Doc_Code = VP.Doc_Code
					AND R.EHA_Acc_Type = 'V'
					AND E.Encrypt_Field2 = VP.Encrypt_Field2
	WHERE
		R.Death_Record_File_ID = @Death_Record_File_ID
			AND R.With_Claim = 'N'

--

	SELECT
		@VYYN = COUNT(1)
	FROM
		DeathRecordMatchResult R
			INNER JOIN DeathRecordEntry E
				ON R.Encrypt_Field1 = E.Encrypt_Field1
					AND R.Death_Record_File_ID = E.Death_Record_File_ID
			INNER JOIN PersonalInformation VP
				ON R.EHA_Acc_ID = VP.Voucher_Acc_ID
					AND R.EHA_Doc_Code = VP.Doc_Code
					AND R.EHA_Acc_Type = 'V'
					AND E.Encrypt_Field2 = VP.Encrypt_Field2
	WHERE
		R.Death_Record_File_ID = @Death_Record_File_ID
			AND R.With_Claim = 'Y'
			AND R.With_Suspicious_Claim = 'N'

--

	SELECT
		@VYYY = COUNT(1)
	FROM
		DeathRecordMatchResult R
			INNER JOIN DeathRecordEntry E
				ON R.Encrypt_Field1 = E.Encrypt_Field1
					AND R.Death_Record_File_ID = E.Death_Record_File_ID
			INNER JOIN PersonalInformation VP
				ON R.EHA_Acc_ID = VP.Voucher_Acc_ID
					AND R.EHA_Doc_Code = VP.Doc_Code
					AND R.EHA_Acc_Type = 'V'
					AND E.Encrypt_Field2 = VP.Encrypt_Field2
	WHERE
		R.Death_Record_File_ID = @Death_Record_File_ID
			AND R.With_Claim = 'Y'
			AND R.With_Suspicious_Claim = 'Y'

--
	
	SELECT
		@VNN = COUNT(1)
	FROM
		DeathRecordMatchResult R
			INNER JOIN DeathRecordEntry E
				ON R.Encrypt_Field1 = E.Encrypt_Field1
					AND R.Death_Record_File_ID = E.Death_Record_File_ID
			INNER JOIN PersonalInformation VP
				ON R.EHA_Acc_ID = VP.Voucher_Acc_ID
					AND R.EHA_Doc_Code = VP.Doc_Code
					AND R.EHA_Acc_Type = 'V'
					AND E.Encrypt_Field2 <> VP.Encrypt_Field2
	WHERE
		R.Death_Record_File_ID = @Death_Record_File_ID
			AND R.With_Claim = 'N'

--

	SELECT
		@VNYN = COUNT(1)
	FROM
		DeathRecordMatchResult R
			INNER JOIN DeathRecordEntry E
				ON R.Encrypt_Field1 = E.Encrypt_Field1
					AND R.Death_Record_File_ID = E.Death_Record_File_ID
			INNER JOIN PersonalInformation VP
				ON R.EHA_Acc_ID = VP.Voucher_Acc_ID
					AND R.EHA_Doc_Code = VP.Doc_Code
					AND R.EHA_Acc_Type = 'V'
					AND E.Encrypt_Field2 <> VP.Encrypt_Field2
	WHERE
		R.Death_Record_File_ID = @Death_Record_File_ID
			AND R.With_Claim = 'Y'
			AND R.With_Suspicious_Claim = 'N'

--

	SELECT
		@VNYY = COUNT(1)
	FROM
		DeathRecordMatchResult R
			INNER JOIN DeathRecordEntry E
				ON R.Encrypt_Field1 = E.Encrypt_Field1
					AND R.Death_Record_File_ID = E.Death_Record_File_ID
			INNER JOIN PersonalInformation VP
				ON R.EHA_Acc_ID = VP.Voucher_Acc_ID
					AND R.EHA_Doc_Code = VP.Doc_Code
					AND R.EHA_Acc_Type = 'V'
					AND E.Encrypt_Field2 <> VP.Encrypt_Field2
	WHERE
		R.Death_Record_File_ID = @Death_Record_File_ID
			AND R.With_Claim = 'Y'
			AND R.With_Suspicious_Claim = 'Y'

--	
	
	SELECT @T = @No_of_Temporary_Account
	
	
-- =============================================
-- Return results
-- =============================================
	SELECT
		@Match_Dtm AS [Match_Dtm],
		@Total_No_of_Record AS [Total_No_of_Record],
		@No_of_Record_With_HKID AS [No_of_Record_With_HKID],
		@No_of_Validated_Account AS [No_of_Validated_Account],
		@No_of_Temporary_Account AS [No_of_Temporary_Account],
		@VYN AS [VYN],
		@VYYN AS [VYYN],
		@VYYY AS [VYYY],
		@VNN AS [VNN],
		@VNYN AS [VNYN],
		@VNYY AS [VNYY],
		@T AS [T]
		

END
GO

GRANT EXECUTE ON [dbo].[proc_DeathRecordMatching_Summary] TO HCVU
GO
