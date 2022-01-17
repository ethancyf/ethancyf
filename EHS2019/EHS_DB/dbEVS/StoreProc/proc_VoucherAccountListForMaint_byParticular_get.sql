IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherAccountListForMaint_byParticular_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[proc_VoucherAccountListForMaint_byParticular_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE20-023-68 (Add Chinese Name)
-- Modified by:		Winnie SUEN
-- Modified date:	20 Dec 2021
-- Description:		1. Change [@Chi_Name] nvarchar(6) -> nvarchar(30)
--					2. Use Collate "Chinese_Taiwan_Stroke_90_CI_AS_SC" for comparing chinese name
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			INT21-0022 (HCVU Claim Transaction Performance Tuning)
-- Modified by:		Winnie SUEN
-- Modified date:	02 Sep 2021
-- Description:		(1) Search with Raw Doc No. to handle "Search by any doc type issue"
--					(2) Fine Tune performance with adding "OPTION (RECOMPILE)"
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			INT21-0016
-- Modified by:		Martin Tang
-- Modified date:	06 Aug 2021
-- Description:		Roll back search raw doc no. to handle search account timeout issue
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-023
-- Modified by:		Martin Tang
-- Modified date:	19 July 2021
-- Description:		1. Fix "Search by any doc type issue"
--					2. Add WITH (NOLOCK)
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-023
-- Modified by:		Martin Tang
-- Modified date:	16 June 2021
-- Description:		Extend patient name's maximum length (varbinary 100->200)
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-023
-- Modified by:		Martin Tang
-- Modified date:	20 Apr 2021
-- Description:		Extend patient name's maximum length
-- =============================================
-- =============================================  
-- Modification History
-- Modified by:		Winnie SUEN
-- CR No.			CRE19-026 (HCVS hotline service) 
-- Modified date:	03 Feb 2020
-- Description:		1. Add [Gender] as Search Criteria
--					2. Support multiple doc code
-- ============================================= 
-- =============================================  
-- Modification History
-- Modified by:		Koala CHENG
-- CR No.			CRE17-018 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19
-- Modified date:	15 Aug 2018 
-- Description:		Retrieve temp account with status Restricted "R" (Created by student file upload and cannot be validated)
-- ============================================= 
-- =============================================
-- Modification History
-- Modified by:		Marco CHOI
-- Modified date:	01 Feb 2018
-- CR No.:			CRE17-012
-- Description:		Add Chinese name search for SP and EHA
-- =============================================
-- =============================================
-- CR No.:		CRE14-016 (To introduce 'Deceased' status into eHS)
-- Author:		Marco CHOI
-- Create date: 08 Dec 2017
-- Description:	VR Account Maintenance
-- =============================================

CREATE PROCEDURE [dbo].[proc_VoucherAccountListForMaint_byParticular_get]
	-- Add the parameters for the stored procedure here
	@Doc_Code varchar(5000),
	@IdentityNum varchar(20),
	@Adoption_Prefix_Num char(7),
	@Eng_Name varchar(100),
	@Chi_Name nvarchar(30),
	@DOB datetime,
	@Voucher_Acc_ID char(15),
	@ReferenceNo char(15),
	@Gender CHAR(1),
	@AccountType char(1),
	@AccountStatus char(1),
	@CreationDateFrom datetime,
	@CreationDateTo datetime,	
	@result_limit_1st_enable BIT, 
	@result_limit_override_enable BIT,
	@override_result_limit BIT,
	@RawIdentityNum varchar(20)
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
	
declare @IdentityNum2 varchar(20)
declare @IdentityNum3 varchar(20)
DECLARE @delimiter		varchar(3)

--
DECLARE @DocTypeList TABLE (
	Doc_Code	CHAR(20)
)

DECLARE @SortingTable TABLE(	
	Source	char(1),
	Sorting INT 	
)

-- =============================================
-- Initialization
-- =============================================

SET @IdentityNum2 = ' ' + @IdentityNum;
SET @delimiter = ',';

SET @IdentityNum3 = @RawIdentityNum;


-- ---------------------------------------------
-- @DocTypeList
-- ---------------------------------------------

IF @Doc_Code = ''
	BEGIN
		INSERT INTO @DocTypeList (Doc_Code)
		SELECT Doc_Code FROM DocType WITH (NOLOCK)
	END
ELSE
	BEGIN
		INSERT INTO @DocTypeList (
			Doc_Code
		)
		SELECT Item FROM func_split_string(@Doc_Code, @delimiter)
	END


INSERT INTO @SortingTable (Source, Sorting) SELECT 'V', 1
INSERT INTO @SortingTable (Source, Sorting) SELECT 'T', 2
INSERT INTO @SortingTable (Source, Sorting) SELECT 'S', 3


OPEN SYMMETRIC KEY sym_Key 
DECRYPTION BY ASYMMETRIC KEY asym_Key

	create table #temptable
	(
		Voucher_Acc_ID char(15),
		Scheme_Code char(10), 
		Remark nvarchar(255),
		Public_Enquiry_Status char(1),
		Public_Enq_Status_Remark nvarchar(255),
		Encrypt_Field1 varbinary(100),
		Encrypt_Field2 varbinary(200),
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
		PersonalInformation_Status char(1),		
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
		Create_By_BO char(1),
		Transaction_id char(20),
		Deceased char(1)
	)
	
	CREATE INDEX IX_VAT on #temptable (Voucher_Acc_ID, Doc_Code)
	
-- =============================================
-- Temporary Voucher Account Search
-- =============================================
IF ISNULL(@AccountType,'') = ''
BEGIN
	insert into #temptable
	(
		Voucher_Acc_ID,
		Scheme_Code, 
		Remark,
		Public_Enquiry_Status,
		Public_Enq_Status_Remark,
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
		PersonalInformation_Status,
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
		Deceased
	)
	select  TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable))
		TVA.Voucher_Acc_ID, 
		TVA.Scheme_Code,
		'',
		'',
		'',
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
		TP.Record_Status,
		'T',
		TVA.Create_Dtm,
		C.SP_ID,
		C.SP_Practice_Display_Seq,
		TP.EC_Age,
		TP.EC_Date_of_Registration,
		--c.sp_id + '('+ convert(varchar,C.SP_Practice_Display_Seq) +')' as Create_By,
		C.Create_By,
		TP.Doc_Code,
		TVA.Account_Purpose,
		TP.Other_Info,
		C.Create_By_BO,
		TVA.Deceased
	FROM TempVoucherAccount TVA WITH (NOLOCK), TempPersonalInformation TP WITH (NOLOCK), VoucherAccountCreationLOG C WITH (NOLOCK), @DocTypeList DTL
	WHERE
		TVA.Voucher_Acc_ID = TP.Voucher_Acc_ID 
		and C.Voucher_Acc_Type = 'T'
		and 
		(
			(TVA.Account_Purpose in ('V','C') and TVA.Record_Status in ('P', 'C', 'A', 'V', 'I', 'D','R')) 
			or 
			(TVA.Account_Purpose = 'O' and TVA.Record_Status = 'I')
			or
			(TVA.Account_Purpose = 'A' and TVA.Record_Status <> 'D' )
		)
		and (	(	(@IdentityNum = ''
						OR TP.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @IdentityNum)
						OR TP.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @IdentityNum2))
					AND (@Adoption_Prefix_Num = ''
						OR TP.Encrypt_Field11 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @Adoption_Prefix_Num)))
				OR (@IdentityNum3 = ''
					OR TP.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @IdentityNum3)))		
		and (@Eng_Name = '' or TP.Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @Eng_Name))
		and (@Chi_Name = '' COLLATE Chinese_Taiwan_Stroke_90_CI_AS_SC or TP.Encrypt_Field3 = EncryptByKey(KEY_GUID('sym_Key'), @Chi_Name))
		and (@DOB is NULL or TP.DOB = @DOB)
		and (@Gender = '' or TP.Sex = @Gender)
		and (@Voucher_Acc_ID = '')
		--and ((@ReferenceNo = '' and TVA.Record_Status <> 'V') or TVA.Voucher_Acc_ID=@ReferenceNo)
		and (@ReferenceNo = '' or TVA.Voucher_Acc_ID = @ReferenceNo)
		and (TVA.Voucher_Acc_ID = C.Voucher_Acc_ID)		
		AND TP.Doc_Code = DTL.Doc_Code
		and (TVA.Create_Dtm >= @CreationDateFrom or @CreationDateFrom is null)
		and (TVA.Create_Dtm < DateAdd(Day,1,@CreationDateTo) or @CreationDateTo is null)
		OPTION (RECOMPILE);


		-- =============================================    
		-- Max Row Checking  
		-- =============================================   
		BEGIN TRY       
			SELECT	@rowcount = count(Voucher_Acc_ID)
			FROM #temptable

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
END

IF @AccountType='T'
BEGIN
	If ISNULL(@AccountStatus,'') ='' 
	BEGIN 
		insert into #temptable
		(
			Voucher_Acc_ID,
			Scheme_Code, 
			Remark,
			Public_Enquiry_Status,
			Public_Enq_Status_Remark,
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
			PersonalInformation_Status,
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
			Deceased
		)
		select  TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable))
			TVA.Voucher_Acc_ID, 
			TVA.Scheme_Code,
			'',
			'',
			'',
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
			TP.Record_Status,
			'T',
			TVA.Create_Dtm,
			C.SP_ID,
			C.SP_Practice_Display_Seq,
			TP.EC_Age,
			TP.EC_Date_of_Registration,
			--c.sp_id + '('+ convert(varchar,C.SP_Practice_Display_Seq) +')' as Create_By,
			C.Create_By,
			TP.Doc_Code,
			TVA.Account_Purpose,
			TP.Other_Info,
			C.Create_By_BO,
			TVA.Deceased
		from TempVoucherAccount TVA WITH (NOLOCK), TempPersonalInformation TP WITH (NOLOCK), VoucherAccountCreationLOG C WITH (NOLOCK), @DocTypeList DTL
		where 
			TVA.Voucher_Acc_ID = TP.Voucher_Acc_ID 
			and C.Voucher_Acc_Type = 'T'
			--and 
			--(
			--	(TVA.Account_Purpose in ('V','C') and TVA.Record_Status in ('P', 'C', 'A', 'V', 'I', 'D')) 
			--	or 
			--	(TVA.Account_Purpose = 'O' and TVA.Record_Status = 'I')
			--	or
			--	(TVA.Account_Purpose = 'A' and TVA.Record_Status <> 'D' )
			--)
			and (	(	(@IdentityNum = ''
							OR TP.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @IdentityNum)
							OR TP.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @IdentityNum2))
						AND (@Adoption_Prefix_Num = ''
							OR TP.Encrypt_Field11 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @Adoption_Prefix_Num)))
					OR (@IdentityNum3 = ''
						OR TP.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @IdentityNum3)))				
			and (@Eng_Name = '' or TP.Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @Eng_Name))
			and (@Chi_Name = '' COLLATE Chinese_Taiwan_Stroke_90_CI_AS_SC or TP.Encrypt_Field3 = EncryptByKey(KEY_GUID('sym_Key'), @Chi_Name))
			and (@DOB is NULL or TP.DOB = @DOB)
			and (@Gender = '' or TP.Sex = @Gender)
			and (@Voucher_Acc_ID = '')
			--and ((@ReferenceNo = '' AND TVA.Record_Status <> 'V') or TVA.Voucher_Acc_ID=@ReferenceNo)
			and (@ReferenceNo = '' or TVA.Voucher_Acc_ID = @ReferenceNo)
			and (TVA.Voucher_Acc_ID = C.Voucher_Acc_ID)			
			AND TP.Doc_Code = DTL.Doc_Code
			and (TVA.Create_Dtm >= @CreationDateFrom or @CreationDateFrom is null)
			and (TVA.Create_Dtm < DateAdd(Day,1,@CreationDateTo) or @CreationDateTo is null)
			and TVA.Record_Status in ('P','I','C','D','V','R')
			and TVA.Account_Purpose in ('C','V')
			OPTION (RECOMPILE);
			
	END
	ELSE
	BEGIN 
		If @AccountStatus = 'N'
		BEGIN	
			--Temporary Account Remained Outstanding for 29 Days
		
			DECLARE @day_level int
			SELECT @day_level=convert(int, Parm_Value1) from SystemParameters where Parameter_Name='Alert_L5_OutstandingDay'	--28day

			insert into #temptable
			(
				Voucher_Acc_ID,
				Scheme_Code, 
				Remark,
				Public_Enquiry_Status,
				Public_Enq_Status_Remark,
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
				PersonalInformation_Status,
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
				Deceased
			)
			select  TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable))
				TVA.Voucher_Acc_ID, 
				TVA.Scheme_Code,
				'',
				'',
				'',
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
				TP.Record_Status,
				'T',
				TVA.Create_Dtm,
				C.SP_ID,
				C.SP_Practice_Display_Seq,
				TP.EC_Age,
				TP.EC_Date_of_Registration,
				--c.sp_id + '('+ convert(varchar,C.SP_Practice_Display_Seq) +')' as Create_By,
				C.Create_By,
				TP.Doc_Code,
				TVA.Account_Purpose,
				TP.Other_Info,
				C.Create_By_BO,
				TVA.Deceased
			from TempVoucherAccount TVA WITH (NOLOCK), TempPersonalInformation TP WITH (NOLOCK), VoucherAccountCreationLOG C WITH (NOLOCK), TempVoucherAccPendingVerify PV WITH (NOLOCK), @DocTypeList DTL
			where 
				TVA.Voucher_Acc_ID = TP.Voucher_Acc_ID 
				and TP.Voucher_Acc_ID = C.Voucher_Acc_ID 
				and TVA.Voucher_Acc_ID = C.Voucher_Acc_ID
				and TVA.Voucher_Acc_ID = PV.Voucher_Acc_ID	
				and TVA.Record_Status='I' and (TVA.Account_Purpose='C' or TVA.Account_Purpose='V')
				and DATEDIFF(Day, PV.First_Validate_Dtm, getdate()) > @day_level
				and (	(	(@IdentityNum = ''
								OR TP.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @IdentityNum)
								OR TP.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @IdentityNum2))
							AND (@Adoption_Prefix_Num = ''
								OR TP.Encrypt_Field11 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @Adoption_Prefix_Num)))
						OR (@IdentityNum3 = ''
							OR TP.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @IdentityNum3)))					
				and (@Eng_Name = '' or TP.Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @Eng_Name))
				and (@Chi_Name = '' COLLATE Chinese_Taiwan_Stroke_90_CI_AS_SC or TP.Encrypt_Field3 = EncryptByKey(KEY_GUID('sym_Key'), @Chi_Name))
				and (@DOB is NULL or TP.DOB = @DOB)
				and (@Gender = '' or TP.Sex = @Gender)
				and (@Voucher_Acc_ID = '')
				and (@ReferenceNo = '' or TVA.Voucher_Acc_ID=@ReferenceNo)				
				AND TP.Doc_Code = DTL.Doc_Code
				and (TVA.Create_Dtm >= @CreationDateFrom or @CreationDateFrom is null)
				and (TVA.Create_Dtm < DateAdd(Day,1,@CreationDateTo) or @CreationDateTo is null)
				OPTION (RECOMPILE);
		END	
	END
	
					
	-- =============================================    
	-- Max Row Checking  
	-- =============================================   
	BEGIN TRY       
		SELECT	@rowcount = count(Voucher_Acc_ID)
		FROM #temptable

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
END

-- =============================================
-- Active Validated Account / Suspended Account / Terminated Account Search
-- =============================================
IF @AccountType = 'A' or @AccountType = 'S' or @AccountType = 'D' OR ISNULL(@AccountType,'') = ''
BEGIN
	insert into #temptable
	(
		Voucher_Acc_ID,
		Scheme_Code, 
		Remark,
		Public_Enquiry_Status,
		Public_Enq_Status_Remark,
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
		PersonalInformation_Status,
		Source,
		Create_Dtm,
		SP_ID,
		SP_Practice_Display_Seq,
		EC_Age,
		EC_Date_of_Registration,
		Create_by,
		Doc_Code,
		Account_Purpose,
		Other_Info,
		Create_By_BO,
		Deceased
	)
	select TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable))
		VA.Voucher_Acc_ID, 
		VA.Scheme_Code, 
		VA.Remark,
		CASE WHEN va.Deceased = 'Y' THEN 'U' ELSE VA.Public_Enquiry_Status END,
		VA.Public_Enq_Status_Remark,
		P.Encrypt_Field1,
		P.Encrypt_Field2,
		P.Encrypt_Field3,
		P.Encrypt_Field4,
		P.Encrypt_Field5,
		P.Encrypt_Field6,
		P.Encrypt_Field7,
		P.Encrypt_Field8,
		P.Encrypt_Field9,
		P.Encrypt_Field11,
		P.DOB,
		P.Exact_DOB,
		P.Sex,
		P.Date_of_Issue,
		VA.Record_Status,
		P.Record_status,
		'V',
		VA.Create_Dtm,
		C.SP_ID,
		C.SP_Practice_Display_Seq,
		P.EC_Age,
		P.EC_Date_of_Registration,
		--c.sp_id + '('+ convert(varchar,C.SP_Practice_Display_Seq) +')' as Create_By,
		C.Create_By,
		P.Doc_Code,
		'',
		P.Other_Info,
		C.Create_By_BO,
		VA.Deceased
	FROM
		PersonalInformation P WITH (NOLOCK)
		inner join VoucherAccount VA WITH (NOLOCK)
			on P.Voucher_Acc_id = VA.voucher_acc_id	
		inner join VoucherAccountCreationLOG C WITH (NOLOCK)
			on VA.Voucher_Acc_ID = C.Voucher_Acc_ID and C.Voucher_Acc_Type = 'V'
		INNER JOIN @DocTypeList DTL
			ON P.Doc_Code = DTL.Doc_Code
	where 
	 (	(	(@IdentityNum = ''
				OR P.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @IdentityNum)
				OR P.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @IdentityNum2))
			AND (@Adoption_Prefix_Num = ''
				OR P.Encrypt_Field11 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @Adoption_Prefix_Num)))
		OR (@IdentityNum3 = ''
			OR P.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @IdentityNum3)))		
	and (@Eng_Name = '' or P.Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @Eng_Name))
	and (@Chi_Name = '' COLLATE Chinese_Taiwan_Stroke_90_CI_AS_SC or P.Encrypt_Field3 = EncryptByKey(KEY_GUID('sym_Key'), @Chi_Name))
	and (@DOB is NULL or P.DOB = @DOB)
	and (@Gender = '' or P.Sex = @Gender)
	and (@Voucher_Acc_ID = '' or VA.Voucher_Acc_ID=@Voucher_Acc_ID)
	and (@ReferenceNo = '')
	and (VA.Create_Dtm >= @CreationDateFrom or @CreationDateFrom is null)
	and (VA.Create_Dtm < DateAdd(Day,1,@CreationDateTo) or @CreationDateTo is null)
	and (VA.Record_Status = @AccountType or ISNULL(@AccountType,'') = '')
	OPTION (RECOMPILE);


	-- =============================================    
	-- Max Row Checking  
	-- ============================================= 
	BEGIN TRY       
		SELECT	@rowcount = count(Voucher_Acc_ID)
		FROM #temptable

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
END	

-- =============================================
-- Erased Account Search
-- =============================================
IF @AccountType = 'E' 
BEGIN
	insert into #temptable
	(
		Voucher_Acc_ID,
		Scheme_Code, 
		Remark,
		Public_Enquiry_Status,
		Public_Enq_Status_Remark,
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
		PersonalInformation_Status,
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
		Deceased
	)
	select  TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable))
		TVA.Voucher_Acc_ID, 
		TVA.Scheme_Code,
		'',
		'',
		'',
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
		TP.Record_Status,
		'T',
		TVA.Create_Dtm,
		C.SP_ID,
		C.SP_Practice_Display_Seq,
		TP.EC_Age,
		TP.EC_Date_of_Registration,
		--c.sp_id + '('+ convert(varchar,C.SP_Practice_Display_Seq) +')' as Create_By,
		C.Create_By,
		TP.Doc_Code,
		TVA.Account_Purpose,
		TP.Other_Info,
		C.Create_By_BO,
		TVA.Deceased
	from TempVoucherAccount TVA WITH (NOLOCK), TempPersonalInformation TP WITH (NOLOCK), VoucherAccountCreationLOG C WITH (NOLOCK), @DocTypeList DTL
	where 
		TVA.Voucher_Acc_ID = TP.Voucher_Acc_ID 
		and C.Voucher_Acc_Type = 'T'
		--and 
		--(
		--	(TVA.Account_Purpose in ('V','C') and TVA.Record_Status in ('P', 'C', 'A', 'V', 'I', 'D')) 
		--	or 
		--	(TVA.Account_Purpose = 'O' and TVA.Record_Status = 'I')
		--	or
		--	(TVA.Account_Purpose = 'A' and TVA.Record_Status <> 'D' )
		--)
		and (	(	(@IdentityNum = ''
						OR TP.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @IdentityNum)
						OR TP.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @IdentityNum2))
					AND (@Adoption_Prefix_Num = ''
						OR TP.Encrypt_Field11 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @Adoption_Prefix_Num)))
				OR (@IdentityNum3 = ''
					OR TP.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @IdentityNum3)))			
		and (@Eng_Name = '' or TP.Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @Eng_Name))
		and (@Chi_Name = '' COLLATE Chinese_Taiwan_Stroke_90_CI_AS_SC or TP.Encrypt_Field3 = EncryptByKey(KEY_GUID('sym_Key'), @Chi_Name))
		and (@DOB is NULL or TP.DOB = @DOB)
		and (@Gender = '' or TP.Sex = @Gender)
		and (@Voucher_Acc_ID = '')
		and (@ReferenceNo = '' or TVA.Voucher_Acc_ID=@ReferenceNo)
		and (TVA.Voucher_Acc_ID = C.Voucher_Acc_ID)
		AND TP.Doc_Code = DTL.Doc_Code
		and (TVA.Create_Dtm >= @CreationDateFrom or @CreationDateFrom is null)
		and (TVA.Create_Dtm < DateAdd(Day,1,@CreationDateTo) or @CreationDateTo is null)
		and TVA.Record_Status = 'I'
		and TVA.Account_Purpose = 'O'
		OPTION (RECOMPILE);
				

	-- =============================================    
	-- Max Row Checking  
	-- =============================================   
	BEGIN TRY       
		SELECT	@rowcount = count(Voucher_Acc_ID)
		FROM #temptable

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
END

-- =============================================
-- Amended Account Search
-- =============================================
IF @AccountType = 'U' 	
BEGIN
	insert into #temptable
	(
		Voucher_Acc_ID,
		Scheme_Code, 
		Remark,
		Public_Enquiry_Status,
		Public_Enq_Status_Remark,
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
		PersonalInformation_Status,
		Source,
		Create_Dtm,
		SP_ID,
		SP_Practice_Display_Seq,
		EC_Age,
		EC_Date_of_Registration,
		Create_by,
		Doc_Code,
		Account_Purpose,
		Other_Info,
		Create_By_BO,
		Deceased
	)
	select TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable))
		VA.Voucher_Acc_ID, 
		VA.Scheme_Code, 
		VA.Remark,
		CASE WHEN va.Deceased = 'Y' THEN 'U' ELSE VA.Public_Enquiry_Status END,
		VA.Public_Enq_Status_Remark,
		P.Encrypt_Field1,
		P.Encrypt_Field2,
		P.Encrypt_Field3,
		P.Encrypt_Field4,
		P.Encrypt_Field5,
		P.Encrypt_Field6,
		P.Encrypt_Field7,
		P.Encrypt_Field8,
		P.Encrypt_Field9,
		P.Encrypt_Field11,
		P.DOB,
		P.Exact_DOB,
		P.Sex,
		P.Date_of_Issue,
		VA.Record_Status,
		P.Record_status,
		'V',
		VA.Create_Dtm,
		C.SP_ID,
		C.SP_Practice_Display_Seq,
		P.EC_Age,
		P.EC_Date_of_Registration,
		--c.sp_id + '('+ convert(varchar,C.SP_Practice_Display_Seq) +')' as Create_By,
		C.Create_By,
		P.Doc_Code,
		'',
		P.Other_Info,
		C.Create_By_BO,
		VA.Deceased
	FROM
		PersonalInformation P WITH (NOLOCK)
		inner join VoucherAccount VA WITH (NOLOCK)
			on P.Voucher_Acc_id = VA.voucher_acc_id	
		inner join VoucherAccountCreationLOG C WITH (NOLOCK)
			on VA.Voucher_Acc_ID = C.Voucher_Acc_ID and C.Voucher_Acc_Type = 'V'
		INNER JOIN @DocTypeList DTL
			ON P.Doc_Code = DTL.Doc_Code
	where 
	(	(	(@IdentityNum = ''
				OR P.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @IdentityNum)
				OR P.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @IdentityNum2))
			AND (@Adoption_Prefix_Num = ''
				OR P.Encrypt_Field11 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @Adoption_Prefix_Num)))
		OR (@IdentityNum3 = ''
			OR P.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @IdentityNum3)))		
	and (@Eng_Name = '' or P.Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @Eng_Name))
	and (@Chi_Name = '' COLLATE Chinese_Taiwan_Stroke_90_CI_AS_SC or P.Encrypt_Field3 = EncryptByKey(KEY_GUID('sym_Key'), @Chi_Name))
	and (@DOB is NULL or P.DOB = @DOB)
	and (@Gender = '' or P.Sex = @Gender)
	and (@Voucher_Acc_ID = '' or VA.Voucher_Acc_ID=@Voucher_Acc_ID)
	and (@ReferenceNo = '')
	and (VA.Create_Dtm >= @CreationDateFrom or @CreationDateFrom is null)
	and (VA.Create_Dtm < DateAdd(Day,1,@CreationDateTo) or @CreationDateTo is null)
	AND EXISTS (
		SELECT
			1
		FROM
			PersonalInfoAmendHistory WITH (NOLOCK)
		WHERE
			Voucher_Acc_ID = P.Voucher_Acc_ID
				AND Doc_Code = P.Doc_Code
				AND Record_Status = 'A'
				AND Action_Type = 'A'
	)
	OPTION (RECOMPILE);


	-- =============================================    
	-- Max Row Checking  
	-- ============================================= 
	BEGIN TRY       
		SELECT	@rowcount = count(DISTINCT Voucher_Acc_ID)
		FROM #temptable

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
END

-- =============================================
-- Special Account Search
-- =============================================
IF @AccountType = 'P' OR ISNULL(@AccountType,'') = ''
BEGIN
	insert into #temptable
	(
		Voucher_Acc_ID,
		Scheme_Code, 
		Remark,
		Public_Enquiry_Status,
		Public_Enq_Status_Remark,
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
		PersonalInformation_Status,
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
		Deceased
	)
	select  TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable))
		TVA.Special_Acc_ID, 
		TVA.Scheme_Code,
		'',
		'',
		'',
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
		TP.Record_Status,
		'S',
		TVA.Create_Dtm,
		C.SP_ID,
		C.SP_Practice_Display_Seq,
		TP.EC_Age,
		TP.EC_Date_of_Registration,
		--c.sp_id + '('+ convert(varchar,C.SP_Practice_Display_Seq) +')' as Create_By,
		C.Create_By,
		TP.Doc_Code,
		'',
		TP.Other_Info,
		C.Create_By_BO,
		TVA.Deceased
	from SpecialAccount TVA WITH (NOLOCK), SpecialPersonalInformation TP WITH (NOLOCK), VoucherAccountCreationLOG C WITH (NOLOCK), @DocTypeList DTL
	where 
		TVA.Special_Acc_ID = TP.Special_Acc_ID
		and C.Voucher_Acc_Type = 'S'
		and (	(	(@IdentityNum = ''
						OR TP.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @IdentityNum)
						OR TP.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @IdentityNum2))
					AND (@Adoption_Prefix_Num = ''
						OR TP.Encrypt_Field11 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @Adoption_Prefix_Num)))
				OR (@IdentityNum3 = ''
					OR TP.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @IdentityNum3)))			
		and (@Eng_Name = '' or TP.Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @Eng_Name))
		and (@Chi_Name = '' COLLATE Chinese_Taiwan_Stroke_90_CI_AS_SC or TP.Encrypt_Field3 = EncryptByKey(KEY_GUID('sym_Key'), @Chi_Name))
		and (@DOB is NULL or TP.DOB = @DOB)
		and (@Gender = '' or TP.Sex = @Gender)
		and (@Voucher_Acc_ID = '')
		--and ((@ReferenceNo = '' and tva.Record_Status <> 'V') or TVA.Special_Acc_ID = @ReferenceNo)
		and (@ReferenceNo = '' or TVA.Special_Acc_ID = @ReferenceNo)
		and (TVA.Special_Acc_ID = C.Voucher_Acc_ID)		
		AND TP.Doc_Code = DTL.Doc_Code
		and (TVA.Create_Dtm >= @CreationDateFrom or @CreationDateFrom is null)
		and (TVA.Create_Dtm < DateAdd(Day,1,@CreationDateTo) or @CreationDateTo is null)
		OPTION (RECOMPILE);


	-- =============================================    
	-- Max Row Checking  
	-- =============================================  
	BEGIN TRY       	
		SELECT	@rowcount = count(Voucher_Acc_ID)
		FROM #temptable
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
END

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
UPDATE #temptable
SET Transaction_id = vt.Transaction_ID
FROM #temptable t
LEFT JOIN VoucherTransaction vt WITH (NOLOCK)
ON vt.Temp_Voucher_Acc_ID = t.Voucher_Acc_ID
WHERE t.Source='T'

select 
	t.Voucher_Acc_ID, 
	t.Scheme_Code, 
	t.Remark,
	t.Public_Enquiry_Status,
	t.Public_Enq_Status_Remark,
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
	t.PersonalInformation_Status,
	t.Source,
	t.Create_Dtm,
	t.SP_ID,
	t.SP_Practice_Display_Seq,
	t.EC_Age,
	t.EC_Date_of_Registration,
	t.Create_By,
	t.Doc_Code,
	dt.Doc_Display_Code,
	t.Account_Purpose,
	t.Other_Info,
	t.Create_By_BO,
	ISNULL(t.Transaction_id,'') as trans_id,
	t.Deceased,
	ISNULL(S.Sorting, 99) AS SortingOrder
from #temptable t
INNER JOIN doctype dt WITH (NOLOCK)
ON t.doc_code = dt.doc_code collate database_default
LEFT JOIN @SortingTable S
ON T.Source = S.Source
Order by 
SortingOrder, 
convert(varchar, DecryptByKey(t.Encrypt_Field1)) ASC	

CLOSE SYMMETRIC KEY sym_Key

drop table #temptable

END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccountListForMaint_byParticular_get] TO HCVU
GO

