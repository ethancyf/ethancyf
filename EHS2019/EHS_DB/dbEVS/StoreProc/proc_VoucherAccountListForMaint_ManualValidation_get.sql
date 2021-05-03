IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherAccountListForMaint_ManualValidation_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[proc_VoucherAccountListForMaint_ManualValidation_get]
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
-- Modified by:		Chris YIM		
-- Modified date:	19 Aug 2019
-- CR No.			CRE19-001
-- Description:		Add doc. type OW, TW and RFNo8 for manual validation
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			INT17-0023	
-- Modified by:	    Chris YIM
-- Modified date:   14 Mar 2018
-- Description:		Fix to handle NULL value on deceased status
-- =============================================
-- =============================================
-- CR No.:		CRE14-016 (To introduce 'Deceased' status into eHS)
-- Author:		Marco CHOI
-- Create date: 08 Dec 2017
-- Description:	VR Account Maintenance
-- =============================================

CREATE PROCEDURE [dbo].[proc_VoucherAccountListForMaint_ManualValidation_get]
	-- Add the parameters for the stored procedure here	
	@SPID varchar(20),
	@ManualValidationStatus varchar(1),
	@CreationDateFrom datetime,
	@CreationDateTo datetime,
	@WithClaims	varchar(1),
	@Scheme	varchar(10),
	@Deceased varchar(1),
	@DateofDeathFrom datetime,
	@DateofDeathTo datetime,
	@AccountType char(1),
	@UserID varchar(20),
	@result_limit_1st_enable BIT,
	@result_limit_override_enable BIT,
	@override_result_limit BIT
AS
BEGIN

SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
DECLARE @rowcount int  
DECLARE @row_cnt_error varchar(max)
DECLARE @errCode_lower char(5) 
DECLARE @errCode_upper char(5) 
SET @errCode_lower = '00009'
SET @errCode_upper = '00017'
	
	
-- =============================================
-- Initialization
-- =============================================

	
OPEN SYMMETRIC KEY sym_Key 
DECRYPTION BY ASYMMETRIC KEY asym_Key

	create table #temptable
	(
		Voucher_Acc_ID char(15),
		Scheme_Code char(10), 
		Remark nvarchar(255),
		--Public_Enquiry_Status char(1),
		--Public_Enq_Status_Remark nvarchar(255),
		Encrypt_Field1 varbinary(100),
		Encrypt_Field2 varbinary(100),
		Encrypt_Field3 varbinary(100),
		Encrypt_Field4 varbinary(50),
		Encrypt_Field5 varbinary(50),
		Encrypt_Field6 varbinary(50),
		Encrypt_Field7 varbinary(50),
		Encrypt_Field8 varbinary(50),
		Encrypt_Field9 varbinary(50),
		Encrypt_Field11 varbinary(50),
		DOB datetime,
		Exact_DOB char(1),
		Sex char(1),
		Date_of_Issue datetime,
		Account_Status char(1),
		--PersonalInformation_Status char(1),		
		Source char(1),
		Create_Dtm datetime,
		SP_ID char(8),
		SP_Practice_Display_Seq smallint,
		EC_Age smallint,
		EC_Date_of_Registration datetime,
		Create_by varchar(20),
		Doc_Code char(20),
		Account_Purpose char(1),
		Other_Info varchar(10),
		sub_cnt int,
		match_cnt int,
		Create_By_BO char(1),
		Validating char(1),
		Scheme_Claim char(10),
		Transaction_id char(20),
		Deceased Char(1),
		DOD datetime,
		Exact_DOD char(1)
	)
	
	--CREATE INDEX IX_VAT on #temptable (Voucher_Acc_ID, Doc_Code)
	
	create table #tempResult
	(
		Voucher_Acc_ID char(15),
		Scheme_Code char(10), 
		Remark nvarchar(255),
		--Public_Enquiry_Status char(1),
		--Public_Enq_Status_Remark nvarchar(255),
		Encrypt_Field1 varbinary(100),
		Encrypt_Field2 varbinary(100),
		Encrypt_Field3 varbinary(100),
		Encrypt_Field4 varbinary(50),
		Encrypt_Field5 varbinary(50),
		Encrypt_Field6 varbinary(50),
		Encrypt_Field7 varbinary(50),
		Encrypt_Field8 varbinary(50),
		Encrypt_Field9 varbinary(50),
		Encrypt_Field11 varbinary(50),
		DOB datetime,
		Exact_DOB char(1),
		Sex char(1),
		Date_of_Issue datetime,
		Account_Status char(1),
		--PersonalInformation_Status char(1),		
		Source char(1),
		Create_Dtm datetime,
		SP_ID char(8),
		SP_Practice_Display_Seq smallint,
		EC_Age smallint,
		EC_Date_of_Registration datetime,
		Create_by varchar(20),
		Doc_Code char(20),
		Doc_Display_Code varchar(20),
		Account_Purpose char(1),
		Other_Info varchar(10),
		Create_By_BO char(1),
		ManualValidationStatus char(1),
		Scheme_Claim char(10),
		Trans_id char(20),
		Deceased Char(1),
		DOD datetime,
		Exact_DOD char(1)
	)
	
	CREATE INDEX IX_VAT on #tempResult (Voucher_Acc_ID, Doc_Code)
-- =============================================
-- Temporary Voucher Account Search
-- =============================================
	if ISNULL(@AccountType, '') = '' OR @AccountType = 'T'
	BEGIN
	insert into #temptable
	(
		Voucher_Acc_ID,
		Scheme_Code, 
		Remark,
		--Public_Enquiry_Status,
		--Public_Enq_Status_Remark,
		Encrypt_Field1,
		Encrypt_Field2,
		Encrypt_Field3,
		Encrypt_Field4,
		Encrypt_Field5,
		Encrypt_Field6,
		Encrypt_Field7,
		Encrypt_Field8,
		Encrypt_Field9,
		Encrypt_Field11,
		DOB,
		Exact_DOB,
		Sex,
		Date_of_Issue,
		Account_Status,
		--PersonalInformation_Status,
		Source,
		Create_Dtm,
		SP_ID,
		SP_Practice_Display_Seq,
		EC_Age,
		EC_Date_of_Registration,
		Create_By,
		Doc_Code,
		Account_Purpose,
		Other_Info,
		Create_By_BO,
		Validating,
		Scheme_Claim,	
		Transaction_id,	
		Deceased,
		DOD,
		Exact_DOD 
	)
	select  
	--TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable))
		TVA.Voucher_Acc_ID, 
		TVA.Scheme_Code,
		'',
		--'',
		--'',
		TP.Encrypt_Field1,
		TP.Encrypt_Field2,
		TP.Encrypt_Field3,
		TP.Encrypt_Field4,
		TP.Encrypt_Field5,
		TP.Encrypt_Field6,
		TP.Encrypt_Field7,
		TP.Encrypt_Field8,
		TP.Encrypt_Field9,
		TP.Encrypt_Field11,
		TP.DOB,
		TP.Exact_DOB,
		TP.Sex,
		TP.Date_of_Issue,
		TVA.Record_Status,
		--TP.Record_Status,
		'T',
		TVA.Create_Dtm,
		C.SP_ID,
		C.SP_Practice_Display_Seq,
		TP.EC_Age,
		TP.EC_Date_of_Registration,
		C.Create_By,
		TP.Doc_Code,
		TVA.Account_Purpose,
		TP.Other_Info,
		C.Create_By_BO,
		TP.Validating,
		T.Scheme_Code,
		ISNULL(T.Transaction_id,''),	
		TP.Deceased,
		TP.DOD,
		TP.Exact_DOD 
	from TempVoucherAccount TVA 
	INNER JOIN TempPersonalInformation TP
	ON TVA.Voucher_Acc_ID = TP.Voucher_Acc_ID 
	INNER JOIN  VoucherAccountCreationLOG C
	ON TVA.Voucher_Acc_ID = C.Voucher_Acc_ID
	LEFT JOIN VoucherTransaction T
	ON TVA.Voucher_Acc_ID = T.Temp_Voucher_Acc_ID
	where 		
		TVA.Record_Status = 'P'
		and TVA.Account_Purpose in ('V', 'A', 'C')
		and C.voucher_acc_type = 'T'
		AND (ISNULL(T.Voucher_Acc_ID, '') ='' OR ISNULL(T.Special_Acc_ID, '') = '')
		and (ISNULL(@SPID, '') = '' OR C.SP_ID = @SPID )
		and (@CreationDateFrom is null OR TVA.Create_Dtm >= @CreationDateFrom )
		and (@CreationDateTo is null OR TVA.Create_Dtm < DateAdd(Day,1,@CreationDateTo) )
		--and (ISNULL(@Deceased, '') = '' OR TVA.Deceased = @Deceased )
		and (ISNULL(@Deceased, '') = '' OR (@Deceased = 'Y' AND TVA.Deceased = 'Y') OR (@Deceased = 'N' and TVA.Deceased IS NULL) )
		and (@DateofDeathFrom is null OR 
					(
						(TP.Exact_DOD = 'Y' AND YEAR(TP.DOD) >= YEAR(@DateofDeathFrom)) OR
						(TP.Exact_DOD = 'M' AND TP.DOD >= DATEADD(MONTH, DATEDIFF(MONTH, 0, @DateofDeathFrom), 0))OR
						(TP.Exact_DOD = 'D' AND TP.DOD >= @DateofDeathFrom)
					)
			)
		and (@DateofDeathTo is null OR TP.DOD < DateAdd(Day,1,@DateofDeathTo))
		and (T.Scheme_Code IS NULL OR EXISTS (Select distinct Scheme_Code from UserRole where User_ID = @UserID AND Scheme_Code = T.Scheme_Code))
		AND (
			(ISNULL(@WithClaims, '') = '' AND ISNULL(@Scheme, '') = '')
			OR
			(ISNULL(@WithClaims, '') = 'Y' AND ISNULL(@Scheme, '') = '' AND T.Temp_Voucher_Acc_ID IS NOT NULL)
			OR
			(ISNULL(@WithClaims, '') = 'Y' AND ISNULL(@Scheme, '') <> '' AND T.Temp_Voucher_Acc_ID IS NOT NULL AND T.Scheme_Code = @Scheme)
			OR
			(ISNULL(@WithClaims, '') = 'N' AND T.Temp_Voucher_Acc_ID IS NULL)
			)
	END

-- =============================================
-- Special Account Search
-- =============================================
	if ISNULL(@AccountType, '') = '' OR @AccountType = 'P'
	BEGIN
	insert into #temptable
	(
		Voucher_Acc_ID,
		Scheme_Code, 
		Remark,
		--Public_Enquiry_Status,
		--Public_Enq_Status_Remark,
		Encrypt_Field1,
		Encrypt_Field2,
		Encrypt_Field3,
		Encrypt_Field4,
		Encrypt_Field5,
		Encrypt_Field6,
		Encrypt_Field7,
		Encrypt_Field8,
		Encrypt_Field9,
		Encrypt_Field11,
		DOB,
		Exact_DOB,
		Sex,
		Date_of_Issue,
		Account_Status,
		--PersonalInformation_Status,
		Source,
		Create_Dtm,
		SP_ID,
		SP_Practice_Display_Seq,
		EC_Age,
		EC_Date_of_Registration,
		Create_By,
		Doc_Code,
		Account_Purpose,
		Other_Info,
		Create_By_BO,
		Validating,
		Scheme_Claim,	
		Transaction_id,		
		Deceased,
		DOD,
		Exact_DOD 
	)
	select  
	--TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable))
		TVA.Special_Acc_ID, 
		TVA.Scheme_Code,
		'',
		--'',
		--'',
		TP.Encrypt_Field1,
		TP.Encrypt_Field2,
		TP.Encrypt_Field3,
		TP.Encrypt_Field4,
		TP.Encrypt_Field5,
		TP.Encrypt_Field6,
		TP.Encrypt_Field7,
		TP.Encrypt_Field8,
		TP.Encrypt_Field9,
		TP.Encrypt_Field11,
		TP.DOB,
		TP.Exact_DOB,
		TP.Sex,
		TP.Date_of_Issue,
		TVA.Record_Status,
		--TP.Record_Status,
		'S',
		TVA.Create_Dtm,
		C.SP_ID,
		C.SP_Practice_Display_Seq,
		TP.EC_Age,
		TP.EC_Date_of_Registration,
		C.Create_By,
		TP.Doc_Code,
		'',
		TP.Other_Info,
		C.Create_By_BO,
		TP.Validating,
		T.Scheme_Code,
		'',
		TP.Deceased,
		TP.DOD,
		TP.Exact_DOD
	from SpecialAccount TVA 
	INNER JOIN SpecialPersonalInformation TP
	ON TVA.Special_acc_id = TP.Special_acc_id
	INNER JOIN VoucherAccountCreationLOG C
	ON TVA.Special_acc_id = C.Voucher_Acc_ID
	LEFT JOIN VoucherTransaction T
	ON TVA.Special_Acc_ID = T.Special_Acc_ID
	where 
		TVA.Record_Status = 'P'
		and TVA.Account_Purpose in ('V', 'C')
		and C.voucher_acc_type = 'S'
		AND (ISNULL(T.Voucher_Acc_ID, '') = '' OR ISNULL(T.Temp_Voucher_Acc_ID, '') = '')
		and (ISNULL(@SPID, '') = ''  OR C.SP_ID = @SPID)
		and (@CreationDateFrom is null OR TVA.Create_Dtm >= @CreationDateFrom )
		and (@CreationDateTo is null OR TVA.Create_Dtm < DateAdd(Day,1,@CreationDateTo) )
		--and (ISNULL(@Deceased, '') = '' OR TVA.Deceased = @Deceased )
		and (ISNULL(@Deceased, '') = '' OR (@Deceased = 'Y' AND TVA.Deceased = 'Y') OR (@Deceased = 'N' and TVA.Deceased IS NULL) )
		and (@DateofDeathFrom is null OR 
					(
						(TP.Exact_DOD = 'Y' AND YEAR(TP.DOD) >= YEAR(@DateofDeathFrom)) OR
						(TP.Exact_DOD = 'M' AND TP.DOD >= DATEADD(MONTH, DATEDIFF(MONTH, 0, @DateofDeathFrom), 0))OR
						(TP.Exact_DOD = 'D' AND TP.DOD >= @DateofDeathFrom)
					)
			)
		and (@DateofDeathTo is null OR TP.DOD < DateAdd(Day,1,@DateofDeathTo))
		and (T.Scheme_Code IS NULL OR EXISTS (Select distinct Scheme_Code from UserRole where User_ID = @UserID AND Scheme_Code = T.Scheme_Code))
		AND (
			(ISNULL(@WithClaims, '') = '' AND ISNULL(@Scheme, '') = '')
			OR
			(ISNULL(@WithClaims, '') = 'Y' AND ISNULL(@Scheme, '') = '' AND T.Special_Acc_ID IS NOT NULL)
			OR
			(ISNULL(@WithClaims, '') = 'Y' AND ISNULL(@Scheme, '') <> '' AND T.Special_Acc_ID IS NOT NULL AND T.Scheme_Code = @Scheme)
			OR
			(ISNULL(@WithClaims, '') = 'N' AND T.Special_Acc_ID IS NULL)
			)
	END
	

	--Manual Validation
	If ISNULL(@ManualValidationStatus ,'') ='' OR @ManualValidationStatus = 'O'
	--Outstanding Acct for Manual Validation
	BEGIN		
		insert into #tempResult
		(
			Voucher_Acc_ID,
			Scheme_Code, 
			Remark,
			--Public_Enquiry_Status,
			--Public_Enq_Status_Remark,
			Encrypt_Field1,
			Encrypt_Field2,
			Encrypt_Field3,
			Encrypt_Field4,
			Encrypt_Field5,
			Encrypt_Field6,
			Encrypt_Field7,
			Encrypt_Field8,
			Encrypt_Field9,
			Encrypt_Field11,
			DOB,
			Exact_DOB,
			Sex,
			Date_of_Issue,
			Account_Status,
			--PersonalInformation_Status,
			Source,
			Create_Dtm,
			SP_ID,
			SP_Practice_Display_Seq,
			EC_Age,
			EC_Date_of_Registration,
			Create_By,
			Doc_Code,
			Doc_Display_Code,
			Account_Purpose,
			Other_Info,
			Create_By_BO,
			ManualValidationStatus,
			Scheme_Claim,
			Trans_id,	
			Deceased,
			DOD,
			Exact_DOD 
		)	
		SELECT			
			Voucher_Acc_ID,
			Scheme_Code, 
			Remark,
			--Public_Enquiry_Status,
			--Public_Enq_Status_Remark,
			Encrypt_Field1,
			Encrypt_Field2,
			Encrypt_Field3,
			Encrypt_Field4,
			Encrypt_Field5,
			Encrypt_Field6,
			Encrypt_Field7,
			Encrypt_Field8,
			Encrypt_Field9,
			Encrypt_Field11,
			DOB,
			Exact_DOB,
			Sex,
			Date_of_Issue,
			Account_Status,
			--PersonalInformation_Status,
			Source,
			Create_Dtm,
			SP_ID,
			SP_Practice_Display_Seq,
			EC_Age,
			EC_Date_of_Registration,
			Create_By,
			t.Doc_Code,
			dt.Doc_Display_Code,
			Account_Purpose,
			Other_Info,
			Create_By_BO,
			'O',
			t.Scheme_Claim,
			t.Transaction_id,	
			t.Deceased,
			t.DOD,
			t.Exact_DOD 
		from #temptable t, doctype dt
		where t.doc_code = dt.doc_code collate database_default
		AND t.Deceased  = 'Y'
		and (Validating is null or Validating <> 'Y')

		insert into #tempResult
		(
			Voucher_Acc_ID,
			Scheme_Code, 
			Remark,
			--Public_Enquiry_Status,
			--Public_Enq_Status_Remark,
			Encrypt_Field1,
			Encrypt_Field2,
			Encrypt_Field3,
			Encrypt_Field4,
			Encrypt_Field5,
			Encrypt_Field6,
			Encrypt_Field7,
			Encrypt_Field8,
			Encrypt_Field9,
			Encrypt_Field11,
			DOB,
			Exact_DOB,
			Sex,
			Date_of_Issue,
			Account_Status,
			--PersonalInformation_Status,
			Source,
			Create_Dtm,
			SP_ID,
			SP_Practice_Display_Seq,
			EC_Age,
			EC_Date_of_Registration,
			Create_By,
			Doc_Code,
			Doc_Display_Code,
			Account_Purpose,
			Other_Info,
			Create_By_BO,
			ManualValidationStatus,
			Scheme_Claim,
			Trans_id,	
			Deceased,
			DOD,
			Exact_DOD 
		)	
		SELECT			
			Voucher_Acc_ID,
			Scheme_Code, 
			Remark,
			--Public_Enquiry_Status,
			--Public_Enq_Status_Remark,
			Encrypt_Field1,
			Encrypt_Field2,
			Encrypt_Field3,
			Encrypt_Field4,
			Encrypt_Field5,
			Encrypt_Field6,
			Encrypt_Field7,
			Encrypt_Field8,
			Encrypt_Field9,
			Encrypt_Field11,
			DOB,
			Exact_DOB,
			Sex,
			Date_of_Issue,
			Account_Status,
			--PersonalInformation_Status,
			Source,
			Create_Dtm,
			SP_ID,
			SP_Practice_Display_Seq,
			EC_Age,
			EC_Date_of_Registration,
			Create_By,
			t.Doc_Code,
			dt.Doc_Display_Code,
			Account_Purpose,
			Other_Info,
			Create_By_BO,
			'O',
			t.Scheme_Claim,
			t.Transaction_id,	
			t.Deceased,
			t.DOD,
			t.Exact_DOD 
		from #temptable t, doctype dt
		where t.doc_code = dt.doc_code collate database_default
		AND (t.Deceased = 'N' OR t.Deceased IS NULL)
		and (Validating is null or Validating <> 'Y')
		and (
			 (t.Doc_Code = 'ADOPC' and (t.Exact_DOB in ('T', 'U', 'V') or DT.Force_Manual_Validate = 'Y')) or
			 (t.Doc_Code = 'Doc/I' and (t.Date_of_Issue < '1 Sep 2003' or DT.Force_Manual_Validate = 'Y')) or
			 (t.Doc_Code = 'EC' and (t.Date_of_issue < '23 Jun 2003' or t.Exact_DOB in ('T', 'U', 'V') or DT.Force_Manual_Validate = 'Y') ) or
			 (t.Doc_Code = 'HKBC' and (t.Exact_DOB in ('T', 'U', 'V') or DT.Force_Manual_Validate = 'Y' )) or
			 (t.doc_code = 'HKIC' and DT.Force_Manual_Validate = 'Y') or
			 (t.doc_code = 'ID235B' and DT.Force_Manual_Validate = 'Y') or
			 (t.doc_code = 'REPMT' and (t.Date_of_Issue < '4 Jun 2007' or DT.Force_Manual_Validate = 'Y')) or
			 (t.doc_code = 'VISA' and DT.Force_Manual_Validate = 'Y') or
			 (t.doc_code = 'OW' and DT.Force_Manual_Validate = 'Y') or
			 (t.doc_code = 'TW' and DT.Force_Manual_Validate = 'Y') or
			 (t.doc_code = 'RFNo8' and DT.Force_Manual_Validate = 'Y')
			)
	END

	IF ISNULL(@ManualValidationStatus ,'') = '' OR @ManualValidationStatus = 'P' 		
	--Pending Validation by ImmD(Manual)
	BEGIN 		
		update #tempTable
		set sub_cnt = tmp.cnt
		from #tempTable t, (select s.voucher_acc_id, count(s.voucher_acc_id) as cnt
							from #tempTable t, TempVoucherAccManualSubLog s
							where t.voucher_acc_id = s.voucher_acc_id collate database_default
							and t.Validating = 'Y'
							group by s.voucher_acc_id)tmp
		where t.voucher_acc_id = tmp.voucher_acc_id collate database_default
		and t.Validating = 'Y'

		update #tempTable
		set match_cnt = tmp.cnt
		from #tempTable t, (select m.voucher_acc_id, count(m.voucher_acc_id) as cnt
							from #tempTable t, TempVoucherAccManualMatchLog m
							where t.voucher_acc_id = m.voucher_acc_id collate database_default
							and t.Validating = 'Y'
							group by m.voucher_acc_id)tmp
		where t.voucher_acc_id = tmp.voucher_acc_id collate database_default
		and t.Validating = 'Y'

		insert into #tempResult
		(
			Voucher_Acc_ID,
			Scheme_Code, 
			Remark,
			--Public_Enquiry_Status,
			--Public_Enq_Status_Remark,
			Encrypt_Field1,
			Encrypt_Field2,
			Encrypt_Field3,
			Encrypt_Field4,
			Encrypt_Field5,
			Encrypt_Field6,
			Encrypt_Field7,
			Encrypt_Field8,
			Encrypt_Field9,
			Encrypt_Field11,
			DOB,
			Exact_DOB,
			Sex,
			Date_of_Issue,
			Account_Status,
			--PersonalInformation_Status,
			Source,
			Create_Dtm,
			SP_ID,
			SP_Practice_Display_Seq,
			EC_Age,
			EC_Date_of_Registration,
			Create_By,
			Doc_Code,
			Doc_Display_Code,
			Account_Purpose,
			Other_Info,
			Create_By_BO,
			ManualValidationStatus,
			Scheme_Claim,
			Trans_id,	
			Deceased,
			DOD,
			Exact_DOD 
		)	
		SELECT			
			Voucher_Acc_ID,
			Scheme_Code, 
			Remark,
			--Public_Enquiry_Status,
			--Public_Enq_Status_Remark,
			Encrypt_Field1,
			Encrypt_Field2,
			Encrypt_Field3,
			Encrypt_Field4,
			Encrypt_Field5,
			Encrypt_Field6,
			Encrypt_Field7,
			Encrypt_Field8,
			Encrypt_Field9,
			Encrypt_Field11,
			DOB,
			Exact_DOB,
			Sex,
			Date_of_Issue,
			Account_Status,
			--PersonalInformation_Status,
			Source,
			Create_Dtm,
			SP_ID,
			SP_Practice_Display_Seq,
			EC_Age,
			EC_Date_of_Registration,
			Create_By,
			t.Doc_Code,
			dt.Doc_Display_Code,
			Account_Purpose,
			Other_Info,
			Create_By_BO,
			'P',
			t.Scheme_Claim,
			t.Transaction_id,	
			t.Deceased,
			t.DOD,
			t.Exact_DOD 
		from #temptable t, doctype dt
		where t.doc_code = dt.doc_code collate database_default
		and isnull(t.sub_cnt,0) <> isnull(t.match_cnt,0)
		and Validating = 'Y'
	END
	

-- =============================================    
-- Max Row Checking  
-- =============================================  
BEGIN TRY       	
	SELECT	@rowcount = count(Voucher_Acc_ID)
	FROM #tempResult
	EXEC proc_CheckFeatureResultRowLimit @row_count=@rowcount, @result_limit_1st_enable=@result_limit_1st_enable, @result_limit_override_enable=@result_limit_override_enable,@override_result_limit=@override_result_limit     
END TRY

BEGIN CATCH    	    
	SET @row_cnt_error = ERROR_MESSAGE()    
		
	IF (isnull(@row_cnt_error,'') <> '' AND @row_cnt_error <> @errCode_lower) or
		(@result_limit_override_enable = 0 AND @row_cnt_error = @errCode_lower )
		BEGIN
			--throw error if upper limit reached (error = @errCode_upper)
			--if upper limit is not enabled, throw error if lower limit is reached
			--if the error is not related to upper / lower limit, there must be sth wrong in the try block, throw the error immediately
			RAISERROR (@row_cnt_error,16,1)    
			CLOSE SYMMETRIC KEY sym_Key  
			RETURN
		END
END CATCH  
		
-- =============================================    
-- Throw out error if lower limit is reached 
-- =============================================  
IF isnull(@row_cnt_error,'') = @errCode_lower 
BEGIN		
	RAISERROR (@row_cnt_error,16,1)    
	CLOSE SYMMETRIC KEY sym_Key  
	RETURN
END

-- =============================================  
-- Return results  
-- ============================================= 
select 
	t.Voucher_Acc_ID, 
	t.Scheme_Code, 
	t.Remark,
	--t.Public_Enquiry_Status,
	--t.Public_Enq_Status_Remark,
	convert(varchar, DecryptByKey(t.Encrypt_Field1)) as IdentityNum,
	convert(varchar(100), DecryptByKey(t.Encrypt_Field2)) as Eng_name,
	isnull(convert(nvarchar, DecryptByKey(t.Encrypt_Field3)),'') as Chi_Name,
	convert(varchar, DecryptByKey(t.Encrypt_Field4)) as CCcode1,
	convert(varchar, DecryptByKey(t.Encrypt_Field5)) as CCcode2,
	convert(varchar, DecryptByKey(t.Encrypt_Field6)) as CCcode3,
	convert(varchar, DecryptByKey(t.Encrypt_Field7)) as CCcode4,
	convert(varchar, DecryptByKey(t.Encrypt_Field8)) as CCcode5,
	convert(varchar, DecryptByKey(t.Encrypt_Field9)) as CCcode6,
	isnull(convert(char, DecryptByKey(t.Encrypt_Field11)),'') as Adoption_Prefix_Num,
	t.DOB,
	t.Exact_DOB,
	t.Sex,
	t.Date_of_Issue,
	t.Account_Status,
	--t.PersonalInformation_Status,
	t.Source,
	t.Create_Dtm,
	t.SP_ID,
	t.SP_Practice_Display_Seq,
	t.EC_Age,
	t.EC_Date_of_Registration,
	t.Create_By,
	t.Doc_Code,
	t.Doc_Display_Code,
	t.Account_Purpose,
	t.Other_Info,
	t.Create_By_BO,
	t.ManualValidationStatus,
	t.Scheme_Claim,
	t.Trans_id,	
	t.Deceased,
	t.DOD,
	t.Exact_DOD 
from #tempResult t
Order by convert(varchar, DecryptByKey(t.Encrypt_Field1)) ASC	

CLOSE SYMMETRIC KEY sym_Key

drop table #tempResult
drop table #temptable

END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccountListForMaint_ManualValidation_get] TO HCVU
GO
