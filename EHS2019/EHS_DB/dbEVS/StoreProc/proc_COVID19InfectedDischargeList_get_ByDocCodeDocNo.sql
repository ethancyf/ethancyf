IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_COVID19InfectedDischargeList_get_ByDocCodeDocNo]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_COVID19InfectedDischargeList_get_ByDocCodeDocNo]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	28 Mar 2022
-- CR No.:			CRE20-0023-XX
-- Description:		Add 4 columns
-- =============================================
-- =============================================
-- Modification History
-- Created by:		Chris YIM
-- Created date:	16 Apr 2021
-- CR No.:			CRE20-0023
-- Description:		Immu Record
-- =============================================

CREATE PROCEDURE [dbo].[proc_COVID19InfectedDischargeList_get_ByDocCodeDocNo]
	@Doc_Code		VARCHAR(20),
	@Identity_No	VARCHAR(30),
	@Eng_Name		VARCHAR(320),  
	@Sex			CHAR(1),  
	@DOB			DATETIME,  
	@ExactDOB		CHAR(1)
AS
BEGIN
-- =============================================
-- Declaration
-- =============================================
	DECLARE @IN_Doc_Code	VARCHAR(20),
			@IN_Identity_No VARCHAR(30),
			@IN_Identity_No2 VARCHAR(30)

	DECLARE	@In_Eng_Name	VARCHAR(320)
	DECLARE	@In_Sex			CHAR(1)
	DECLARE	@In_DOB			DATETIME
	DECLARE	@In_ExactDOB	CHAR(1)

	DECLARE @Type_Of_Doc	VARCHAR(20)

	DECLARE @Result TABLE(
		[Demographic_Match]	CHAR(1),
		[Discharge_Date]	DATETIME,
		[Infection_Date]	DATETIME,
		[Recovery_Date]		DATETIME,
		[Death_Indicator]	CHAR(1),
		[File_ID]			VARCHAR(100)
	)
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================

	-- Initial Doc Code
	SET @IN_Doc_Code = @Doc_Code
	SET @IN_Identity_No = @Identity_No
	SET @In_Eng_Name = @Eng_Name
	SET @In_Sex = @Sex
	SET @In_DOB = @DOB
	SET @In_ExactDOB = @ExactDOB
	SET @Type_Of_Doc = ''

	-- Initial Doc_Type
	IF @In_Doc_Code='HKIC' OR @In_Doc_Code='HKBC' OR @In_Doc_Code='EC' OR @In_Doc_Code='CCIC' OR @In_Doc_Code='ROP140'
	BEGIN
		SET @Type_Of_Doc = 'HKIC'
	END

	IF @In_Doc_Code='PASS' OR @In_Doc_Code='OC' OR @In_Doc_Code='HKP'
	BEGIN
		SET @Type_Of_Doc = 'PASS'
	END

	-- Initial Doc No.
	IF @Type_Of_Doc = 'HKIC'
	BEGIN
		SET @IN_Identity_No = REPLACE(@IN_Identity_No,' ','') 
		SET @IN_Identity_No2 = ' ' + @IN_Identity_No
	END

	-- For name,  Remove comma, space, single quotation mark, hyphen for comparing with encrypt_field2
	SET @In_Eng_Name = REPLACE(REPLACE(REPLACE(REPLACE(@In_Eng_Name,',',''),' ',''),'-','') ,'''','') 

-- =============================================
-- Return results
-- =============================================

    EXEC [proc_SymmetricKey_open]

	IF @Type_Of_Doc = 'HKIC'
	BEGIN
		INSERT @Result(
			[Demographic_Match],
			[Discharge_Date],
			[Infection_Date],
			[Recovery_Date],
			[Death_Indicator],
			[File_ID]
		)
		SELECT
			[Demographic_Match] = CASE
								--WHEN [Discharge_Date] IS NULL AND [Recovery_Date] IS NULL THEN 'N'
								WHEN [DOB] IS NULL THEN 'P'
								WHEN (REPLACE(REPLACE(REPLACE(REPLACE(CONVERT(VARCHAR(MAX), DecryptByKey(Encrypt_Field2_1)) + CONVERT(VARCHAR(MAX), DecryptByKey(Encrypt_Field2_2)),',',''),' ',''),'-',''),'''','') <> @In_Eng_Name  --Eng Name not match
										OR Sex <> @In_Sex  -- Sex not match
										OR ((@ExactDOB = 'Y' OR @ExactDOB = 'V' OR  @ExactDOB = 'R' OR  @ExactDOB = 'A') AND (DOB_Format <> 'EY' OR DATEPART (YEAR, DOB) <> DATEPART (YEAR, @In_DOB)))
										OR ((@ExactDOB = 'M' OR @ExactDOB = 'U') AND (DOB_Format <> 'EMY' OR DATEPART (YEAR, DOB) <> DATEPART (YEAR, @In_DOB) OR DATEPART (MONTH, DOB) <> DATEPART (MONTH, @In_DOB)))
										OR ((@ExactDOB = 'D' OR @ExactDOB = 'T') AND (DOB_Format <> 'EDMY' OR DATEPART (YEAR, DOB) <> DATEPART (YEAR, @In_DOB) OR DATEPART (MONTH, DOB) <> DATEPART (MONTH, @In_DOB) OR DATEPART (DAY, DOB) <> DATEPART (DAY, @In_DOB)))
										)
								THEN 'P'
								ELSE 'F' 
							END,
			[Discharge_Date] = CASE
								WHEN [Discharge_Date] IS NULL THEN NULL
								ELSE [Discharge_Date]
							END,
			[Infection_Date] = CASE
								WHEN [Infection_Date] IS NULL THEN NULL
								ELSE [Infection_Date]
							END,
			[Recovery_Date] = CASE
								WHEN [Recovery_Date] IS NULL THEN NULL
								ELSE [Recovery_Date]
							END,
			[Death_Indicator] = CASE
								WHEN [Death_Indicator] IS NULL THEN 'N'
								ELSE [Death_Indicator]
							END,
			[File_ID] = CASE
								WHEN [Discharge_Date] IS NULL THEN NULL
								ELSE [File_ID]
							END
		FROM 
			[COVID19DischargePatient] DP
		WHERE   
			(DP.[Encrypt_Field1] = EncryptByKey(KEY_GUID('sym_Key'), @IN_Identity_No)  
			OR DP.[Encrypt_Field1] = EncryptByKey(KEY_GUID('sym_Key'), @IN_Identity_No2))
	END

	ELSE IF @Type_Of_Doc = 'PASS'
	BEGIN
		INSERT @Result(
			[Demographic_Match],
			[Discharge_Date],
			[Infection_Date],
			[Recovery_Date],
			[Death_Indicator],
			[File_ID]
		)
		SELECT
			[Demographic_Match] = CASE
								--WHEN [Discharge_Date] IS NULL AND [Recovery_Date] IS NULL THEN 'N'
								WHEN [DOB] IS NULL THEN 'P'
								WHEN (REPLACE(REPLACE(REPLACE(REPLACE(CONVERT(VARCHAR(MAX), DecryptByKey(Encrypt_Field2_1)) + CONVERT(VARCHAR(MAX), DecryptByKey(Encrypt_Field2_2)),',',''),' ',''),'-',''),'''','') <> @In_Eng_Name  --Eng Name not match
										OR Sex <> @In_Sex  -- Sex not match
										OR ((@ExactDOB = 'Y' OR @ExactDOB = 'V' OR  @ExactDOB = 'R' OR  @ExactDOB = 'A')  AND  (DOB_Format <> 'EY' OR DATEPART (YEAR, DOB) <> DATEPART (YEAR, @In_DOB)))
										OR ((@ExactDOB = 'M' OR @ExactDOB = 'U') AND (DOB_Format <> 'EMY' OR DATEPART (YEAR, DOB) <> DATEPART (YEAR, @In_DOB) OR DATEPART (MONTH, DOB) <> DATEPART (MONTH, @In_DOB)))
										OR ((@ExactDOB = 'D' OR @ExactDOB = 'T') AND (DOB_Format <> 'EDMY' OR DATEPART (YEAR, DOB) <> DATEPART (YEAR, @In_DOB) OR DATEPART (MONTH, DOB) <> DATEPART (MONTH, @In_DOB) OR DATEPART (DAY, DOB) <> DATEPART (DAY, @In_DOB)))
										)
								THEN 'P'
								ELSE 'F' 
							END,
			[Discharge_Date] = CASE
								WHEN [Discharge_Date] IS NULL THEN NULL
								ELSE [Discharge_Date]
							END,
			[Infection_Date] = CASE
								WHEN [Infection_Date] IS NULL THEN NULL
								ELSE [Infection_Date]
							END,
			[Recovery_Date] = CASE
								WHEN [Recovery_Date] IS NULL THEN NULL
								ELSE [Recovery_Date]
							END,
			[Death_Indicator] = CASE
								WHEN [Death_Indicator] IS NULL THEN 'N'
								ELSE [Death_Indicator]
							END,
			[File_ID] = CASE
								WHEN [Discharge_Date] IS NULL THEN NULL
								ELSE [File_ID]
							END
		FROM 
			[COVID19DischargePatient] DP
		WHERE   
			DP.[Encrypt_Field12] = EncryptByKey(KEY_GUID('sym_Key'), @IN_Identity_No) 

	END

	IF (SELECT COUNT(1) FROM @Result) > 0
	BEGIN
		SELECT 
			[Demographic_Match], 
			[Discharge_Date],
			[Infection_Date], 
			[Recovery_Date], 
			[Death_Indicator],
			[File_ID] 
		FROM @Result
	END
	ELSE
		SELECT 
			[Demographic_Match] = 'N', 
			[Discharge_Date] = NULL, 
			[Infection_Date] = NULL, 
			[Recovery_Date] = NULL, 
			[Death_Indicator] = NULL,
			[File_ID] = NULL

    EXEC [proc_SymmetricKey_close]

END
GO

GRANT EXECUTE ON [dbo].[proc_COVID19InfectedDischargeList_get_ByDocCodeDocNo] TO HCSP

GRANT EXECUTE ON [dbo].[proc_COVID19InfectedDischargeList_get_ByDocCodeDocNo] TO HCVU

GO


