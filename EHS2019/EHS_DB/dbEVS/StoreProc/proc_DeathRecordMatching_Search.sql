IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DeathRecordMatching_Search]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DeathRecordMatching_Search]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- CR No.:			CRE20-023
-- Modified by:		Martin Tang
-- Modified date:	16 June 2021
-- Description:		Extend patient name's maximum length (varbinary 100->200)
-- =============================================
-- =============================================
-- Modification History
-- CR# :			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- =============================================  
-- Modification History      
-- CR No.:		   CRE12-014  
-- Modified by:    Karl LAM   
-- Modified date:  03 Jan 2013  
-- Description:    Add parameters: @result_limit_1st_enable,@result_limit_override_enable, @override_result_limit  for relax 500 rows limitation  
-- =============================================      
-- =============================================   
-- Modification History
-- Modified by:		Tommy TSE
-- Modified date:	07 Dec 2011
-- CR No.:			CRP11-015
-- Description:		Fix the RowCount bug
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		05 May 2011
-- CR No.:			CRE11-007
-- Description:		Search function for Death Record Matching
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.:			
-- Description:		
-- =============================================

--exec proc_DeathRecordMatching_Search null,null,null,null,null,null,null,'1980','2012',1,1,0

CREATE PROCEDURE [dbo].[proc_DeathRecordMatching_Search]
	@Doc_Code				char(20),
	@Doc_No					varchar(20),
	@Account_Type			char(1),
	@Account_Status			char(1),
	@With_Claim				char(1),
	@With_Suspicious_Claim	char(1),
	@Name_Match				char(1),
	@DOB_From				int,
	@DOB_To					int,
	@result_limit_1st_enable BIT, 
	@result_limit_override_enable BIT,
	@override_result_limit BIT

AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
	DECLARE @FilterDeathRecordMatchResult table (
		EHA_Acc_ID				char(15),
		EHA_Doc_Code			char(20),
		EHA_Acc_Type			char(1),
		With_Suspicious_Claim	char(1),
		Encrypt_Field2			varbinary(200),
		DOD						datetime,
		Exact_DOD				char(1)
	)

	DECLARE @ResultTable table (
		Account_ID				char(15),
		Account_Type			char(1),
		Document_Code			char(20),
		Encrypt_Field1			varbinary(100),
		DOB						datetime,
		Exact_DOB				char(1),
		EC_Age					smallint,
		EC_Date_of_Registration	datetime,
		DOD						datetime,
		Exact_DOD				char(1),
		Account_Status			char(1),
		With_Suspicious_Claim	char(1),
		With_Claim				char(1)
	)

	DECLARE @E_Doc_No		varbinary(100)
	DECLARE @DOB_Dtm_From	datetime
	DECLARE @DOB_Dtm_To		datetime
	
	DECLARE @rowcount INT
	DECLARE @row_cnt_error varchar(max)
	
	
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	IF @Doc_No IS NULL BEGIN
		SET @E_Doc_No = NULL
	
	END ELSE BEGIN
		EXEC [proc_SymmetricKey_open]

		SET @E_Doc_No = EncryptByKey(KEY_GUID('sym_Key'), @Doc_No)

		EXEC [proc_SymmetricKey_close]
		
	END
	
--

	IF @DOB_From IS NULL BEGIN
		SET @DOB_Dtm_From = NULL
	END ELSE BEGIN
		SET @DOB_Dtm_From = CONVERT(datetime, CONVERT(varchar, @DOB_From) + '-01-01', 120)
	END
	
	IF @DOB_To IS NULL BEGIN
		SET @DOB_Dtm_To = NULL
	END ELSE BEGIN
		SET @DOB_Dtm_To = CONVERT(datetime, CONVERT(varchar, @DOB_To) + '-12-31', 120)
	END

--


-- =============================================
-- Retrieve data
-- =============================================
	
	INSERT INTO @FilterDeathRecordMatchResult (
		EHA_Acc_ID,
		EHA_Doc_Code,
		EHA_Acc_Type,
		With_Suspicious_Claim,
		Encrypt_Field2,
		DOD,
		Exact_DOD
	)
	SELECT 
		R.EHA_Acc_ID,
		R.EHA_Doc_Code,
		R.EHA_Acc_Type,
		R.With_Suspicious_Claim,
		E.Encrypt_Field2,
		E.DOD,
		E.Exact_DOD
	FROM
		(SELECT Death_Record_File_ID FROM DeathRecordFileHeader WHERE Record_Status = 'S') H
		INNER JOIN DeathRecordMatchResult R
			ON H.Death_Record_File_ID = R.Death_Record_File_ID
			INNER JOIN DeathRecordEntry E
				ON R.Death_Record_File_ID = E.Death_Record_File_ID
					AND R.Encrypt_Field1 = E.Encrypt_Field1
	WHERE
		(@Doc_Code IS NULL OR R.EHA_Doc_Code = @Doc_Code)
			AND (@E_Doc_No IS NULL OR R.Encrypt_Field1 = @E_Doc_No)
			AND (@Account_Type IS NULL OR R.EHA_Acc_Type = @Account_Type)
			AND (@With_Claim IS NULL OR R.With_Claim = @With_Claim)
			AND (@With_Suspicious_Claim IS NULL OR R.With_Suspicious_Claim = @With_Suspicious_Claim)
	
--

	INSERT INTO @ResultTable (
		Account_ID,
		Account_Type,
		Document_Code,
		Encrypt_Field1,
		DOB,
		Exact_DOB,
		EC_Age,
		EC_Date_of_Registration,
		DOD,
		Exact_DOD,
		Account_Status,
		With_Suspicious_Claim
	)
	SELECT  TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable)) 
		VP.Voucher_Acc_ID,
		R.EHA_Acc_Type,
		VP.Doc_Code,
		VP.Encrypt_Field1,
		VP.DOB,
		VP.Exact_DOB,
		VP.EC_Age,
		VP.EC_Date_of_Registration,
		R.DOD,
		R.Exact_DOD,
		VA.Record_Status,
		R.With_Suspicious_Claim
	FROM
		@FilterDeathRecordMatchResult R
			INNER JOIN VoucherAccount VA
				ON R.EHA_Acc_ID = VA.Voucher_Acc_ID
					AND R.EHA_Acc_Type = 'V'
					AND (@Account_Status IS NULL OR @Account_Status = VA.Record_Status)
					AND VA.Record_Status <> 'D'
			INNER JOIN PersonalInformation VP
				ON R.EHA_Acc_ID = VP.Voucher_Acc_ID
					AND R.EHA_Doc_Code = VP.Doc_Code
					AND (@DOB_Dtm_From IS NULL OR VP.DOB BETWEEN @DOB_Dtm_From AND @DOB_Dtm_To)
					AND (@Name_Match IS NULL
							OR (@Name_Match = 'Y' AND R.Encrypt_Field2 = VP.Encrypt_Field2)
							OR (@Name_Match = 'N' AND R.Encrypt_Field2 <> VP.Encrypt_Field2)
						)
			
	INSERT INTO @ResultTable (
		Account_ID,
		Account_Type,
		Document_Code,
		Encrypt_Field1,
		DOB,
		Exact_DOB,
		EC_Age,
		EC_Date_of_Registration,
		DOD,
		Exact_DOD,
		Account_Status,
		With_Suspicious_Claim
	)
	SELECT  TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable)) 
		TP.Voucher_Acc_ID,
		R.EHA_Acc_Type,
		TP.Doc_Code,
		TP.Encrypt_Field1,
		TP.DOB,
		TP.Exact_DOB,
		TP.EC_Age,
		TP.EC_Date_of_Registration,
		R.DOD,
		R.Exact_DOD,
		TA.Record_Status,
		R.With_Suspicious_Claim
	FROM
		@FilterDeathRecordMatchResult R
			INNER JOIN TempVoucherAccount TA
				ON R.EHA_Acc_ID = TA.Voucher_Acc_ID
					AND R.EHA_Acc_Type = 'T'
					AND TA.Record_Status NOT IN ('V', 'D')
			INNER JOIN TempPersonalInformation TP
				ON R.EHA_Acc_ID = TP.Voucher_Acc_ID
					AND (@DOB_Dtm_From IS NULL OR TP.DOB BETWEEN @DOB_Dtm_From AND @DOB_Dtm_To)
					AND (@Name_Match IS NULL
							OR (@Name_Match = 'Y' AND R.Encrypt_Field2 = TP.Encrypt_Field2)
							OR (@Name_Match = 'N' AND R.Encrypt_Field2 <> TP.Encrypt_Field2)
						)


-- =============================================    
-- Max Row Checking  
-- =============================================  
	BEGIN TRY       
			SELECT	@rowcount = COUNT(1) FROM	@ResultTable
			EXEC proc_CheckFeatureResultRowLimit @row_count=@rowcount, @result_limit_1st_enable=@result_limit_1st_enable, @result_limit_override_enable=@result_limit_override_enable,@override_result_limit=@override_result_limit     
	END TRY

	BEGIN CATCH    	    
		SET @row_cnt_error = ERROR_MESSAGE()    

		RAISERROR (@row_cnt_error,16,1)    
		RETURN
	END CATCH 			

-- =============================================
-- Return results
-- =============================================
	
	EXEC [proc_SymmetricKey_open]

	SELECT
		Account_ID,
		Account_Type,
		Document_Code,
		CONVERT(varchar, DecryptByKey(Encrypt_Field1)) AS [Document_No],
		DOB,
		Exact_DOB,
		EC_Age,
		EC_Date_of_Registration,
		DOD,
		Exact_DOD,
		Account_Status,
		With_Suspicious_Claim
	FROM
		@ResultTable	
	ORDER BY
		Account_ID
		
	EXEC [proc_SymmetricKey_close]

END
GO

GRANT EXECUTE ON [dbo].[proc_DeathRecordMatching_Search] TO HCVU
GO
