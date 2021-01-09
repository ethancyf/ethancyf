IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_eVaccination_get_byAccountDemographic_v2]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_eVaccination_get_byAccountDemographic_v2]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	01 Sep 2020
-- CR No.:			INT20-0028 (Fix eVaccination result)
-- Description:		
--					Issue:
--						If a validated account contains HKIC and HKBC but the demographics are not matched (e.g. name, DOB)
--						Then no vaccine will be returned
--					Solution:
--						If either HKIC or HKBC is demogprahics matched, return all vaccine
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	28 Oct 2019
-- CR No.:			CRE19-005-02 (Share MMR between DH CIMS and eHS(S) - Phase 2 - Interface)
-- Description:		VaccineDoseSeqCodeMapping
--					- Handle with new column [Subsidize_Item_Code_Source] 
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM		
-- Modified date:	25 Jul 2019
-- CR No.			CRE19-001
-- Description:		Block to return vaccination record if the "For_Enquiry" of vaccine is "N"
-- =============================================
-- =============================================
-- Modification History
-- CR#:				CRE18-013
-- Modified by:		Koala Cheng 
-- Modified date:	15 Nov 2018
-- Description:		Revise demographics checking logic
--					1. DOB match year part only
--					2. Keep invalid account if all demographics matched
--					3. Return "Partial record returned" if the unmatched account has transaction
--					4. Return "Full record returned" or "No record returned" if the unmatched account has no transaction
-- =============================================
-- =============================================
-- Modification History
-- CR#:				INT18-0012
-- Modified by:		Koala Cheng 
-- Modified date:	27 Sep 2018
-- Description:		Performance tuning
--					1. Change @Temp table to #Temp table
-- =============================================
-- =============================================  
-- Author:			Koala Cheng  
-- CR#:				CRE18-004
-- Modified by:		Koala CHENG
-- Modified date:	28 Apr 2018
-- Description:		Modify from existing SProc [proc_eVaccination_get_byAccountDemographic]
--					New DOB matching logic
--					Remove more special characters for patient name matching (Space, comma, hyphen, single quotation mark)
--					Support document type DOCI,REPMT,VISA, ADOPC
-- =============================================  

--===========================================
--Sample Query: 
--exec proc_eVaccination_get_byAccountDemographic_v2 'HKIC', 'CC2001254','HUI, MOK CHUNG', 'M', '1938-02-07', 'D','001-0910,001-1011,001-1112,001-1213,001-1314','1999-01-01','2015-01-01','EHS','CMS',null,null
--===========================================

CREATE PROCEDURE [dbo].[proc_eVaccination_get_byAccountDemographic_v2] (  
 -- Add the parameters for the stored procedure here  
 @Doc_Code char(20),  
 @IdentityNum varchar(20),  
 @Eng_Name varchar(320),  
 @Sex char(1),  
 @DOB datetime,  
 @ExactDOB char(1),  
 @VaccineCode NVARCHAR(100),  
 @InjectionDateStart DATETIME,  
 @InjectionDateEND DATETIME,  
 @Source_System VARCHAR(10),  
 @Target_System VARCHAR(10),  
 @Out_PatientResultCode INT output,  
 @Out_VaccineResultCode INT output  
)  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  

-- Solve Parameter Sniffing 
-- ==============================================
DECLARE	@In_Doc_Code char(20)
DECLARE	@In_IdentityNum varchar(20)
DECLARE	@In_SerialNo varchar(20)
DECLARE	@In_Eng_Name varchar(320)
DECLARE	@In_Sex char(1)
DECLARE	@In_DOB datetime
DECLARE	@In_ExactDOB char(1)
DECLARE	@In_VaccineCode NVARCHAR(100)
DECLARE	@In_InjectionDateStart DATETIME
DECLARE	@In_InjectionDateEND DATETIME
DECLARE	@In_Source_System VARCHAR(10)
DECLARE	@In_Target_System VARCHAR(10)
DECLARE @In_ADOPC_IdentityNum_Prefix VARCHAR(7)
DECLARE @In_ADOPC_IdentityNum_Entry VARCHAR(8)

SET @In_Doc_Code = @Doc_Code
SET @In_IdentityNum = @IdentityNum
SET @In_SerialNo = ''
SET @In_Eng_Name = @Eng_Name
SET @In_Sex = @Sex
SET @In_DOB = @DOB
SET @In_ExactDOB = @ExactDOB -- D or M or Y [Ignored since CRE18-013]
SET @In_VaccineCode = @VaccineCode
SET @In_InjectionDateStart = @InjectionDateStart
SET @In_InjectionDateEND = @InjectionDateEND
SET @In_Source_System = @Source_System
SET @In_Target_System = @Target_System
-- ==============================================

DECLARE @OnSite VARCHAR(1)  
DECLARE @Out_KeyFound BIT  
DECLARE @Out_PatientMatch BIT  
DECLARE @Out_PartialMatch BIT  
  
SET @OnSite = 'Y'  
SET @Out_KeyFound = 0  
SET @Out_PatientMatch = 0  
SET @Out_PartialMatch = 0  
    
DECLARE @rowcount int  
DECLARE @rowcount_PatientMatch int  
DECLARE @rowcount_PatientNotMatch int  
   
DECLARE @IdentityNum2 varchar(20)  
   
 -- =============================================  
 -- Filter not support document type (HA CMS Support HKIC/HKID/HKBC/CE only)  
 -- Filter not support document type (DH CIMS Support HKIC/HKID/HKBC/CE/"Doc/I"/REPMT/VISA/ADOPC only )  
  -- =============================================  
 IF LTRIM(RTRIM(@In_Target_System)) = 'CMS' 
 BEGIN	
	IF @In_Doc_Code<>'HKID' AND @In_Doc_Code<>'HKIC' AND @In_Doc_Code<>'HKBC' AND @In_Doc_Code<>'EC' 
	BEGIN   
		SET @Out_PatientResultCode = 1 -- Patient NOT found  
		SET @Out_VaccineResultCode = 2 -- No record returned  

		RETURN
	END
 END
 ELSE IF LTRIM(RTRIM(@In_Target_System)) = 'CIMS' 
 BEGIN
	IF @In_Doc_Code NOT IN ('HKID','HKIC','HKBC','EC','DOCI','REPMT','VISA','ADOPC')
	BEGIN   
		SET @Out_PatientResultCode = 1 -- Patient NOT found  
		SET @Out_VaccineResultCode = 2 -- No record returned  

		RETURN
	END

	--DECLARE @CIMS_Doc_Type_Check INT

	--SELECT @CIMS_Doc_Type_Check = COUNT(1)
	--FROM CodeMapping
	--WHERE Code_Source = @In_Doc_Code
	--AND Source_System = 'EHS'
	--AND Target_System = 'CIMS'

	--IF @CIMS_Doc_Type_Check = 0
	--BEGIN   
	--	SET @Out_PatientResultCode = 1 -- Patient NOT found  
	--	SET @Out_VaccineResultCode = 2 -- No record returned  
		  
	--	RETURN
	--END
 END
 ELSE
 BEGIN
	SET @Out_PatientResultCode = 1 -- Patient NOT found  
	SET @Out_VaccineResultCode = 2 -- No record returned  
	
	RETURN
 END  
  
 -- =============================================  
 -- Prepare system parameter setting  
 -- =============================================  
 IF @In_InjectionDateStart IS NULL  
	SELECT @In_InjectionDateStart = Parm_Value1 FROM systemparameters   
	WHERE Parameter_Name = 'EHS_Get_Vaccine_WS_InjectionDateStart'  
 ELSE  
	SELECT @In_InjectionDateStart = Parm_Value1 FROM systemparameters   
	WHERE Parameter_Name = 'EHS_Get_Vaccine_WS_InjectionDateStart'  
	AND Parm_Value1 > @In_InjectionDateStart  
  
 -- =============================================  
 -- Prepare temp table  
 -- =============================================  
SET NOCOUNT ON;  
  
-- Create vaccine code (CMS) table  
CREATE TABLE #tempVaccineCode  
(  
	Vaccine_Code_CMS CHAR(10) 
)  


-- Create table for processing patient  
DECLARE  @temptable TABLE
(  
	Acc_Type char(1),
	Voucher_Acc_ID char(15) ,  
	Scheme_Code char(10),   
	Encrypt_Field1 varbinary(100),  
	Encrypt_Field2 varbinary(100),  
	DOB datetime,  
	Exact_DOB char(1),  
	Sex char(1),  
	Date_of_Issue datetime,  
	Account_Status char(1),  
	Doc_Code char(20) COLLATE Chinese_Taiwan_Stroke_CI_AS,
	WithClaim bit,
	DemographicUnmatched bit
)  

 -- Create table for exact match account ID  
DECLARE @Account TABLE  
(   
	Acc_Type CHAR(1),  
	Voucher_Acc_ID char(15) COLLATE Chinese_Taiwan_Stroke_CI_AS  
)  
 --CREATE INDEX IX_VAT on @temptable (Voucher_Acc_ID, Doc_Code)  
  
  
 -- Create table for processing vaccine records  
CREATE TABLE #tempVaccine   
 (  
	Acc_Type CHAR(1),  
	Voucher_Acc_ID char(15) COLLATE Chinese_Taiwan_Stroke_CI_AS,
	record_creation_dtm DATETIME NOT NULL,  
	injection_date DATETIME NOT NULL,  
	vaccine_code VARCHAR(25)NOT NULL,  
	vaccine_desc VARCHAR(100)NOT NULL,  
	vaccine_desc_chinese NVARCHAR(100)NOT NULL,  
	dose_seq_code VARCHAR(20)NOT NULL,  
	dose_seq_desc VARCHAR(100)NOT NULL,  
	dose_seq_desc_chinese NVARCHAR(100)NOT NULL,  
	provider NVARCHAR(20) NOT NULL,  
	location NVARCHAR(100)NOT NULL,  
	location_chinese NVARCHAR(100)NOT NULL  
 )  
  
 DECLARE @AvailableDocCode TABLE  
 (  
	Doc_Code CHAR(20) NOT NULL  
 )  

 IF LTRIM(RTRIM(@In_Target_System)) = 'CIMS'
 BEGIN
	IF @In_Doc_Code='HKID' OR @In_Doc_Code='HKIC' OR @In_Doc_Code='HKBC'  OR @In_Doc_Code='EC' 
	BEGIN
		INSERT INTO @AvailableDocCode (Doc_Code) VALUES('HKIC')  
		INSERT INTO @AvailableDocCode (Doc_Code) VALUES('HKBC')  
		INSERT INTO @AvailableDocCode (Doc_Code) VALUES('EC')  
	END
	ELSE
	BEGIN 	  
		-- Convert input doc type "DOCI" to database doc type "Doc/I"
		INSERT INTO @AvailableDocCode (Doc_Code) VALUES (IIF(@In_Doc_Code = 'DOCI','Doc/I',@In_Doc_Code))
	END
 END
 ELSE IF LTRIM(RTRIM(@In_Target_System)) = 'CMS'
 BEGIN
	INSERT INTO @AvailableDocCode (Doc_Code) VALUES('HKIC')  
	INSERT INTO @AvailableDocCode (Doc_Code) VALUES('HKBC')  
	INSERT INTO @AvailableDocCode (Doc_Code) VALUES('EC')  
 END
  
 -- =============================================  
 -- Initialization  
 -- =============================================  
 
 IF @In_Doc_Code='HKID' OR @In_Doc_Code='HKIC' OR @In_Doc_Code='HKBC'  OR @In_Doc_Code='EC' 
 BEGIN
	 SET @In_IdentityNum = REPLACE(@In_IdentityNum,' ','') 
	 SET @IdentityNum2 = ' ' + @In_IdentityNum
	 SET @In_SerialNo = @In_IdentityNum
 END
 ELSE IF @Doc_Code = 'ADOPC'
 BEGIN
	SET @In_ADOPC_IdentityNum_Prefix = SUBSTRING(@In_IdentityNum, 0, CHARINDEX('/',@In_IdentityNum,0))
	SET @In_ADOPC_IdentityNum_Entry = SUBSTRING(@In_IdentityNum, CHARINDEX('/',@In_IdentityNum,0) + 1, LEN(@In_IdentityNum) )
	-- Remove @In_IdentityNum
	-- Remove @IdentityNum2
	SET @In_IdentityNum = ''
	SET @IdentityNum2 = ''
 END
 ELSE IF @Doc_Code = 'VISA'
 BEGIN
	 -- Identity no. format: JAPN-9900204-88(8)
	 -- Remove - ( ) in correct position for comparing with encrypt_field1
	 IF CHARINDEX('-', @In_IdentityNum) = 5 
		SET @In_IdentityNum =  STUFF(@In_IdentityNum, 5, 1, '')

	 IF CHARINDEX('-', @In_IdentityNum) = 12 
		SET @In_IdentityNum =  STUFF(@In_IdentityNum, 12, 1, '')

	 IF CHARINDEX('(', @In_IdentityNum) = 14
		SET @In_IdentityNum =  STUFF(@In_IdentityNum, 14, 1, '')

	 IF CHARINDEX(')', @In_IdentityNum) = 15
		SET @In_IdentityNum =  STUFF(@In_IdentityNum, 15, 1, '')

	 -- Remove @IdentityNum2
	 SET @IdentityNum2 = ''
 END
 ELSE
 BEGIN
	-- Keep @In_IdentityNum
	-- Remove @IdentityNum2
	SET @IdentityNum2 = ''
 END
 

 -- For name,  Remove comma, space, single quotation mark, hyphen for comparing with encrypt_field2
 SET @In_Eng_Name = REPLACE(REPLACE(REPLACE(REPLACE(@In_Eng_Name,',',''),' ',''),'-','') ,'''','') 
 
  
 -- Convert string ('001,002,003,001-0910,002-0910') to XML   
INSERT INTO #tempVaccineCode  
SELECT * FROM dbo.func_split_string(@In_VaccineCode,',')

IF (SELECT COUNT(1) FROM #tempVaccineCode) = 0 
BEGIN 
	INSERT INTO #tempVaccineCode(Vaccine_Code_CMS) VALUES ('')
END
 
  
 EXEC [proc_SymmetricKey_open]
  
  
 -- ****************************************************************  
 -- Query Account Vaccination  
 -- ****************************************************************  
  
 -- =============================================  
 -- Find Patient by Key (e.g. HKIC,HKBC,EC)  
 -- =============================================  
 --- Find Validate/Temp/Special Account  
 INSERT INTO @temptable  
 (  
	 Acc_Type,
	 Voucher_Acc_ID,  
	 Scheme_Code,   
	 Encrypt_Field1,  
	 Encrypt_Field2,  
	 DOB,  
	 Exact_DOB,  
	 Sex,  
	 Date_of_Issue,  
	 Account_Status,  
	 Doc_Code,
	 WithClaim,
	 DemographicUnmatched
 )  
 -- +++++++++++++++++++++  
 -- Validate Account  
 -- +++++++++++++++++++++
 SELECT   
	 'V',
	 VA.Voucher_Acc_ID,   
	 VA.Scheme_Code,   
	 P.Encrypt_Field1,  
	 P.Encrypt_Field2,  
	 P.DOB,  
	 P.Exact_DOB,
	 P.Sex,  
	 P.Date_of_Issue,  
	 VA.Record_Status,  
	 P.Doc_Code,
	 0,
	 0
FROM  
	PersonalInformation P WITH (NOLOCK)  
	INNER JOIN VoucherAccount VA WITH (NOLOCK)  
	ON P.Voucher_Acc_id = VA.voucher_acc_id   
	INNER JOIN @AvailableDocCode ADC  
	ON P.Doc_Code = ADC.Doc_Code  
WHERE   
	(P.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @In_IdentityNum)  
	OR P.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @IdentityNum2)
	OR (@In_Doc_Code = 'EC' AND P.EC_Serial_No = @In_IdentityNum)
	OR (@In_Doc_Code = 'ADOPC' AND P.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @In_ADOPC_IdentityNum_Entry)
							   AND P.Encrypt_Field11 = EncryptByKey(KEY_GUID('sym_Key'), @In_ADOPC_IdentityNum_Prefix)
		)
	)
	UNION ALL
-- +++++++++++++++++++++
-- Temp Account 
-- +++++++++++++++++++++
SELECT   
	'T',
	TVA.Voucher_Acc_ID,   
	TVA.Scheme_Code,  
	TP.Encrypt_Field1,  
	TP.Encrypt_Field2,  
	TP.DOB,  
	TP.Exact_DOB,   
	TP.Sex,  
	TP.Date_of_Issue,  
	TVA.Record_Status,  
	TP.Doc_Code,
	0,
	0 
FROM TempVoucherAccount TVA WITH (NOLOCK)  
	INNER JOIN TempPersonalInformation TP WITH (NOLOCK)  
	ON TVA.Voucher_Acc_ID = TP.Voucher_Acc_ID AND Account_Purpose NOT IN ('A','O') AND  TVA.Record_Status NOT IN ('V', 'D')  
	INNER JOIN @AvailableDocCode ADC  
	ON TP.Doc_Code = ADC.Doc_Code  
WHERE   
	(TP.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @In_IdentityNum)  
	OR TP.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @IdentityNum2)
	OR (@In_Doc_Code = 'EC' AND TP.EC_Serial_No = @In_IdentityNum)
	OR (@In_Doc_Code = 'ADOPC' AND TP.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @In_ADOPC_IdentityNum_Entry)
							   AND TP.Encrypt_Field11 = EncryptByKey(KEY_GUID('sym_Key'), @In_ADOPC_IdentityNum_Prefix)
		)
	)
UNION ALL
-- +++++++++++++++++++++
-- Special Account
-- +++++++++++++++++++++
SELECT 
	'S',  
	TVA.Special_Acc_ID,   
	TVA.Scheme_Code,  
	TP.Encrypt_Field1,  
	TP.Encrypt_Field2,  
	TP.DOB,  
	TP.Exact_DOB,   
	TP.Sex,  
	TP.Date_of_Issue,  
	TVA.Record_Status,  
	TP.Doc_Code,
	0,
	0 
FROM SpecialAccount TVA WITH (NOLOCK)  
	INNER JOIN SpecialPersonalInformation TP WITH (NOLOCK)  
	ON TVA.Special_Acc_ID = TP.Special_Acc_ID  
	AND TVA.Record_Status NOT IN ('V', 'D')
	INNER JOIN @AvailableDocCode ADC  
	ON TP.Doc_Code = ADC.Doc_Code  
	
WHERE   
	(TP.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @In_IdentityNum)  
	OR TP.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @IdentityNum2)
	OR (@In_Doc_Code = 'EC' AND TP.EC_Serial_No = @In_IdentityNum)
	OR (@In_Doc_Code = 'ADOPC' AND TP.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @In_ADOPC_IdentityNum_Entry)
							   AND TP.Encrypt_Field11 = EncryptByKey(KEY_GUID('sym_Key'), @In_ADOPC_IdentityNum_Prefix)
		)
	)


 -- Mark KeyFound if record found  
 SELECT @rowcount = COUNT(DISTINCT Voucher_Acc_ID) FROM @temptable  
 IF @rowcount > 0  
 BEGIN  
  SET @Out_KeyFound = 1  
 END  
  
 
-- Collect account ID  
 INSERT INTO @Account  
 SELECT DISTINCT Acc_Type, Voucher_Acc_ID FROM @temptable 
 
 
 -- =============================================  
 -- Query vaccination record by Patient in @temptable  
 -- =============================================  
INSERT INTO #tempVaccine  
SELECT   
	Acc_Type,
	Acc_ID,
	T.Create_Dtm AS record_creation_dtm,  
	T.Service_Receive_Dtm AS injection_date,  
	VCM.Vaccine_Code_Common AS vaccine_code,  
	VCM.Vaccine_Code_Desc AS vaccine_desc,  
	VCM.Vaccine_Code_Desc_Chi AS vaccine_desc_chinese,  
	VDSCM.Vaccine_dose_seq_code_Target AS dose_seq_code,  
	VDSCM.Vaccine_Dose_Seq_Code_Desc AS dose_seq_desc,  
	VDSCM.Vaccine_Dose_Seq_Code_Desc_Chi AS dose_seq_desc_chinese,  
	VCM.Provider AS provider,  
	P.Practice_Name AS location,  
	ISNULL(P.Practice_Name_chi, '') AS location_chinese  
FROM  
	(
	-- +++++++++++++++++++++
	-- Validated/Temp/Special Account Transaction
	-- +++++++++++++++++++++
	SELECT   
		Acc_Type,
		Acc_ID,
		Scheme_Code, 
		Create_Dtm ,  
		Service_Receive_Dtm,  
		Voucher_Acc_ID,  
		Temp_Voucher_Acc_ID,  
		Special_Acc_ID,  
		Invalid_Acc_ID,  
		Transaction_ID,  
		SP_ID,  
		Practice_Display_Seq  
	FROM   
		(
		-- +++++++++++++++++++++
		-- Validated Account Transaction
		-- +++++++++++++++++++++
		SELECT  
			VVR.Acc_Type,
			VVR.Voucher_Acc_ID AS [Acc_ID],
			VVT.Scheme_Code,  
			VVT.Create_Dtm ,  
			VVT.Service_Receive_Dtm,  
			VVT.Voucher_Acc_ID,  
			VVT.Temp_Voucher_Acc_ID,  
			VVT.Special_Acc_ID,  
			VVT.Invalid_Acc_ID,  
			VVT.Transaction_ID,  
			VVT.SP_ID,  
			VVT.Practice_Display_Seq  
		FROM VoucherTransaction VVT WITH (NOLOCK) , @Account VVR  
		WHERE 
			VVT.Record_Status NOT IN ('I','W','D') and  
			(@In_InjectionDateStart IS NULL OR VVT.Service_Receive_Dtm >= @In_InjectionDateStart) AND  
			(@In_InjectionDateEnd IS NULL OR VVT.Service_Receive_Dtm <= @In_InjectionDateEnd)    
			and VVT.Voucher_Acc_Id = VVR.Voucher_Acc_ID  
			AND VVR.Acc_Type = 'V'  
			AND VVT.Voucher_Acc_ID <> ''  
			AND VVT.Invalid_Acc_ID IS NULL  
		UNION  
		-- +++++++++++++++++++++
		-- Temp Account Transaction
		-- +++++++++++++++++++++
		SELECT   
			TVR.Acc_Type,
			TVR.Voucher_Acc_ID AS [Acc_ID], 
			TVT.Scheme_Code,
			TVT.Create_Dtm ,  
			TVT.Service_Receive_Dtm,  
			TVT.Voucher_Acc_ID,  
			TVT.Temp_Voucher_Acc_ID,  
			TVT.Special_Acc_ID,  
			TVT.Invalid_Acc_ID,  
			TVT.Transaction_ID,  
			TVT.SP_ID,  
			TVT.Practice_Display_Seq  
		FROM VoucherTransaction TVT WITH (NOLOCK) , @Account TVR  
		WHERE 
			TVT.Record_Status NOT IN ('I','W','D') and  
			(@In_InjectionDateStart IS NULL OR TVT.Service_Receive_Dtm >= @In_InjectionDateStart) AND  
			(@In_InjectionDateEnd IS NULL OR TVT.Service_Receive_Dtm <= @In_InjectionDateEnd)    
			and TVT.Temp_Voucher_Acc_ID = TVR.Voucher_Acc_ID   
			AND TVR.Acc_Type = 'T'  
			AND TVT.Temp_Voucher_Acc_ID <> ''   
			AND TVT.Voucher_Acc_ID = ''  
			AND TVT.Special_Acc_ID IS NULL  
		UNION   
		-- +++++++++++++++++++++
		-- Special Account Transaction
		-- +++++++++++++++++++++
		SELECT   
			SVR.Acc_Type,
			SVR.Voucher_Acc_ID AS [Acc_ID],
			SVT.Scheme_Code, 
			SVT.Create_Dtm ,  
			SVT.Service_Receive_Dtm,  
			SVT.Voucher_Acc_ID,  
			SVT.Temp_Voucher_Acc_ID,  
			SVT.Special_Acc_ID,  
			SVT.Invalid_Acc_ID,  
			SVT.Transaction_ID,  
			SVT.SP_ID,  
			SVT.Practice_Display_Seq  
		FROM VoucherTransaction SVT WITH (NOLOCK) , @Account SVR  
		WHERE 
			SVT.Record_Status NOT IN ('I','W','D') and  
			(@In_InjectionDateStart IS NULL OR SVT.Service_Receive_Dtm >= @In_InjectionDateStart) AND  
			(@In_InjectionDateEnd IS NULL OR SVT.Service_Receive_Dtm <= @In_InjectionDateEnd)    
			and SVT.Special_Acc_ID = SVR.Voucher_Acc_ID   
			AND SVR.Acc_Type = 'S'  
			AND SVT.Special_Acc_ID is not null  
			AND SVT.Voucher_Acc_ID = ''  
			AND SVT.Invalid_Acc_ID IS NULL  
		) T1  
	) T  
	INNER JOIN 
	(
		SELECT Distinct sc.Scheme_Code 
		FROM  SchemeClaim sc WITH (NOLOCK) inner join SubsidizeGroupClaim sgc WITH (NOLOCK) on sc.Scheme_Code = sgc.Scheme_Code
		inner join Subsidize s WITH (NOLOCK) on sgc.Subsidize_Code = s.Subsidize_Code
		inner join SubsidizeItem si WITH (NOLOCK) on s.Subsidize_Item_Code = si.Subsidize_Item_Code
		WHERE si.Subsidize_Type = 'VACCINE'  
	) v
	ON v.Scheme_Code = T.Scheme_Code
	INNER JOIN TransactionDetail TD WITH (NOLOCK)  
	ON T.Transaction_ID = TD.Transaction_ID  
	INNER JOIN Subsidize S WITH (NOLOCK)  
	ON TD.Subsidize_Code = S.Subsidize_Code AND  
		TD.Subsidize_Item_Code = S.Subsidize_Item_Code  
	INNER JOIN SubsidizeItem SI WITH (NOLOCK)  
	ON S.Subsidize_Item_Code = SI.Subsidize_Item_Code  
	INNER JOIN SubsidizeItemDetails SIDs WITH (NOLOCK)  
	ON S.Subsidize_Item_Code = SIDs.Subsidize_Item_Code  
	AND TD.Available_Item_Code = SIDs.Available_Item_Code  
	INNER JOIN Practice P WITH (NOLOCK)  
	ON T.SP_ID = P.SP_ID  
    AND T.Practice_Display_Seq = P.Display_Seq  
	INNER JOIN (SELECT Vaccine_Code_Target, Vaccine_Code_Source, Vaccine_Code_Common, Vaccine_Code_Desc, Vaccine_Code_Desc_Chi, Provider  
				FROM VaccineCodeMapping WITH (NOLOCK)  
				WHERE Source_System=@In_Source_System AND Target_System=@In_Target_System
				AND For_Enquiry = 'Y'
				) VCM  
	ON REPLACE(TD.Scheme_Code + '|' + STR(TD.Scheme_Seq) + '|' + TD.Subsidize_Code,' ','') = VCM.Vaccine_Code_Source  
	AND EXISTS (SELECT * FROM #tempVaccineCode   
				WHERE VCM.Vaccine_Code_Target = Vaccine_Code_CMS OR SUBSTRING(VCM.Vaccine_Code_Target, 1,LEN(Vaccine_Code_CMS)) = Vaccine_Code_CMS)  
	INNER JOIN (SELECT Vaccine_dose_seq_code_Target, Vaccine_Dose_Seq_Code_Source, Vaccine_Dose_Seq_Code_Desc, Vaccine_Dose_Seq_Code_Desc_Chi  
	FROM VaccineDoseSeqCodeMapping WITH (NOLOCK)  
	WHERE Source_System=@In_Source_System AND Target_System=@In_Target_System AND [Subsidize_Item_Code_Source] = 'ALL') VDSCM   
	ON SIDs.Available_Item_Code = VDSCM.Vaccine_Dose_Seq_Code_Source  
  


  
 -- =============================================== 
 -- Find Patient who match Eng Name, DOB, Gender  
  -- ===============================================  
 
 -- ------------------------------------------------
 -- Input DOB   | Matching in eHS(S)
 -- ------------------------------------------------
 -- DD/MM/YYYY  | Just match year of DOB
 -- MM/YYYY     | Just match year of DOB
 -- YYYY        | Just match year of DOB
 -- ------------------------------------------------

 -- -------------------------
 -- Exact_DOB equivalent table
 -- -------------------------
 -- D     | D    
 -- M     | M   
 -- Y     | Y   
 -- A     | D   
 -- R     | Y     
 -- T     | D     
 -- U     | M     
 -- V     | Y    
 -- ------------------------- 
 -- Update account if name or sex or year of DOB not match
 UPDATE @temptable SET DemographicUnmatched = 1
 WHERE (REPLACE(REPLACE(REPLACE(REPLACE(CONVERT(VARCHAR(MAX), DecryptByKey(Encrypt_Field2)),',',''),' ',''),'-',''),'''','') <> @In_Eng_Name  --Eng Name not match
	OR Sex <> @In_Sex  -- Sex not match
	OR DATEPART (YEAR, DOB) <> DATEPART (YEAR, @In_DOB) -- and not match the year of input DOB
	) 
 -- Update account if has transaction
 UPDATE @temptable SET WithClaim = 1
 FROM #tempVaccine v
	INNER JOIN @temptable t
	ON v.Acc_Type = t.Acc_Type
		AND v.Voucher_Acc_ID = t.Voucher_Acc_ID
 
 -- Mark Patient Match if no record deleted  
 -- Mark Partial Match if some record deleted  
 SELECT @rowcount_PatientMatch = COUNT(DISTINCT Voucher_Acc_ID) FROM @temptable WHERE DemographicUnmatched = 0
 IF @rowcount_PatientMatch > 0 AND @rowcount_PatientMatch <> @rowcount  
 BEGIN  
  --SET @Out_PartialMatch = 1  
  SELECT @Out_PartialMatch = CASE WHEN COUNT(DISTINCT Voucher_Acc_ID) > 0 THEN 1 ELSE 0 END FROM @temptable WHERE DemographicUnmatched = 1 AND WithClaim = 1
  SET @Out_PatientMatch = 1  
 END  
 ELSE  
  IF @rowcount_PatientMatch <> 0  
  BEGIN   
   SET @Out_PatientMatch = 1  
  END  
 
 -- Remove Vaccine if the account is not matched
 DELETE FROM #tempVaccine WHERE EXISTS (SELECT a.Acc_Type, a.Voucher_Acc_ID  
										FROM @temptable a 
										GROUP BY a.Acc_Type, a.Voucher_Acc_ID
										HAVING MIN(CAST(DemographicUnmatched AS INT)) = 1 
												AND #tempVaccine.Acc_Type = a.Acc_Type
												AND #tempVaccine.Voucher_Acc_ID = a.Voucher_Acc_ID)

 SELECT @rowcount = COUNT(1) FROM #tempVaccine  
   
 -- Set PatientResultCode  
 IF @Out_PatientMatch = 1  
 BEGIN SET @Out_PatientResultCode = 0 END -- Patient found, Demographic match  
 ELSE  
  IF @Out_KeyFound = 0  
  BEGIN SET @Out_PatientResultCode = 1 END -- Patient not found (ID no match)  
  ELSE  
  BEGIN SET @Out_PatientResultCode = 2 END -- Patient found but demographic does not match  
  
 -- Set VaccineResultCode  
 IF @rowcount > 0  
 BEGIN  
 IF @Out_PartialMatch = 0  
  BEGIN SET @Out_VaccineResultCode =  0 END -- Full record returned  
 ELSE  
  BEGIN SET @Out_VaccineResultCode =  1 END -- Partial record returned
 END  
 ELSE  
 IF @Out_PartialMatch = 0 
  BEGIN SET @Out_VaccineResultCode =  2 END -- No record returned  
 ELSE
  IF @Out_PatientResultCode = 2 -- Patient found but demographic does not match
    BEGIN SET @Out_VaccineResultCode =  2 END -- No record returned
  ELSE
    BEGIN SET @Out_VaccineResultCode =  1 END -- Partial record returned
  
 EXEC [proc_SymmetricKey_close]  
  
 -- Return result  
 DECLARE @CurrentSeasonStartDate AS DATETIME  
 SELECT @CurrentSeasonStartDate = Parm_Value1 FROM systemparameters   
  WHERE Parameter_Name = 'EHS_Get_Vaccine_WS_SeasonDateStart'  
    
 SELECT @RowCount = COUNT(injection_date) FROM #tempVaccine  
 WHERE injection_date >= @CurrentSeasonStartDate  
   
 IF @RowCount = 0  
  SELECT record_creation_dtm,  
	injection_date,  
	vaccine_code,  
	vaccine_desc,  
	vaccine_desc_chinese,  
	dose_seq_code,  
	dose_seq_desc,  
	dose_seq_desc_chinese,  
	provider,  
	location,  
	location_chinese, 
	@OnSite AS onsite FROM #tempVaccine  
  WHERE injection_date >= @CurrentSeasonStartDate  
 ELSE  
  SELECT record_creation_dtm,  
	injection_date,  
	vaccine_code,  
	vaccine_desc,  
	vaccine_desc_chinese,  
	dose_seq_code,  
	dose_seq_desc,  
	dose_seq_desc_chinese,  
	provider,  
	location,  
	location_chinese,
	@OnSite AS onsite FROM #tempVaccine  

-- Housekeeping
DROP TABLE #tempVaccineCode 
DROP TABLE #tempVaccine 

END  
GO

GRANT EXECUTE ON [dbo].[proc_eVaccination_get_byAccountDemographic_v2] TO WSINT
GO

