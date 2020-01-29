IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_eVaccination_get_byAccountDemographic]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_eVaccination_get_byAccountDemographic]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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
-- CR#:				INT18-0002
-- Modified by:		Chris YIM
-- Modified date:	10 May 2018
-- Description:		Fix to maintain patient name's length in 48 characters
-- =============================================
-- =============================================
-- Modification History
-- CR#:				INT18-0001
-- Modified by:		Koala CHENG
-- Modified date:	26 April 2018
-- Description:		Fix patient name matching by remove comma, space and hyphen only
-- =============================================
-- =============================================
-- Modification History
-- CR#:				INT15-0019
-- Modified by:		Lawrence TSANG
-- Modified date:	26 November 2015
-- Description:		Fix eVacc by exclude validated special account
-- =============================================
-- =============================================
-- Modification History
-- CR#:			CRE13-019-02
-- Modified by:	Karl LAM	
-- Modified date:	2015 Mar 10
-- Description:		1. Remove hardcoding 'HCVS'
-- =============================================
-- =============================================== 
-- Modification History  
-- Modified by:  Koala Cheng  
-- Modified date: 16 Feb 2011
-- Description:  1. [CRE11-017] Performance Tuning (Parameter Sniffing)
-- =============================================  
-- ============================================= 
-- Modification History  
-- Modified by:  Koala Cheng  
-- Modified date: 23 Nov 2010  
-- Description:  1. Fix Null Chinese Practice  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:  Koala Cheng  
-- Modified date: 03 Nov 2010 (04 Nov 2010 Rollout version)  
-- Description:  1. Filter I, W, D transaction  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:  Koala Cheng  
-- Modified date: 29 Oct 2010  
-- Description:  Performance Tuning  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:  Koala Cheng  
-- Modified date: 27 Oct 2010  
-- Description:  Fix to allow HKBC enquiry  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:  Koala Cheng  
-- Modified date: 07 Sep 2010  
-- Description:  If no Exact DOB then match Year only  
-- =============================================  
-- =============================================  
-- Author:   Koala Cheng  
-- Create date:  08 Jun 2010  
-- Description:  Get vaccination record by CMS XML request  
-- =============================================  

--===========================================
--Sample Query: 
--exec proc_eVaccination_get_byAccountDemographic 'HKIC', 'CC2001254','HUI, MOK CHUNG', 'M', '1938-02-07', 'D','001-0910,001-1011,001-1112,001-1213,001-1314','1999-01-01','2015-01-01','EHS','CMS',null,null
--===========================================

CREATE PROCEDURE [dbo].[proc_eVaccination_get_byAccountDemographic] (  
 -- Add the parameters for the stored procedure here  
 @Doc_Code char(20),  
 @IdentityNum varchar(20),  
 @Eng_Name varchar(48),  
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
DECLARE	@In_Eng_Name varchar(48)
DECLARE	@In_Sex char(1)
DECLARE	@In_DOB datetime
DECLARE	@In_ExactDOB char(1)
DECLARE	@In_VaccineCode NVARCHAR(100)
DECLARE	@In_InjectionDateStart DATETIME
DECLARE	@In_InjectionDateEND DATETIME
DECLARE	@In_Source_System VARCHAR(10)
DECLARE	@In_Target_System VARCHAR(10)

SET @In_Doc_Code = @Doc_Code
SET @In_IdentityNum = @IdentityNum
SET @In_Eng_Name = @Eng_Name
SET @In_Sex = @Sex
SET @In_DOB = @DOB
SET @In_ExactDOB = @ExactDOB -- D or Y
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
 -- Filter not support document type (Support HKIC/HKID/HKBC only)  
 -- =============================================  
 IF @In_Doc_Code<>'HKID' AND @In_Doc_Code<>'HKIC' AND @In_Doc_Code<>'HKBC'  
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
 DECLARE @tempVaccineCode TABLE  
 (  
  Vaccine_Code_CMS CHAR(10) COLLATE Chinese_Taiwan_Stroke_CI_AS  
 )  
  
 -- Create table for processing patient  
 DECLARE @temptable AS TABLE  
 (  
 Voucher_Acc_ID char(15) COLLATE Chinese_Taiwan_Stroke_CI_AS ,  
 Scheme_Code char(10),   
 Encrypt_Field1 varbinary(100),  
 Encrypt_Field2 varbinary(100),  
 DOB datetime,  
 Exact_DOB char(1),  
 Sex char(1),  
 Date_of_Issue datetime,  
 Account_Status char(1),  
 Doc_Code char(20) COLLATE Chinese_Taiwan_Stroke_CI_AS)  
   
 -- Create table for exact match account ID  
 DECLARE @Account TABLE  
 (   
  Acc_Type CHAR(1),  
  Voucher_Acc_ID char(15) COLLATE Chinese_Taiwan_Stroke_CI_AS  
 )  
 --CREATE INDEX IX_VAT on @temptable (Voucher_Acc_ID, Doc_Code)  
  
  
 -- Create table for processing vaccine records  
 DECLARE @tempVaccine TABLE  
 (  
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
 INSERT INTO @AvailableDocCode VALUES('HKIC')  
 INSERT INTO @AvailableDocCode VALUES('HKBC')  
 INSERT INTO @AvailableDocCode VALUES('EC')  
  
 -- =============================================  
 -- Initialization  
 -- =============================================  
 set @In_IdentityNum = REPLACE(@In_IdentityNum,' ','')
 set @IdentityNum2 = ' ' + @In_IdentityNum
 set @In_Eng_Name = REPLACE(REPLACE(REPLACE(@In_Eng_Name,',',''),' ',''),'-','')  
  
  
 -- Convert string ('001,002,003,001-0910,002-0910') to XML   
 declare @xml xml  
 set @xml = N'<root><r>' + replace(@In_VaccineCode,',','</r><r>') + '</r></root>'  
  
 -- XML to temp table  
 INSERT INTO @tempVaccineCode  
 select   
   t.value('.','varchar(10)') as [delimited items]  
 from @xml.nodes('//root/r') as a(t)  
  
  
 OPEN SYMMETRIC KEY sym_Key   
  DECRYPTION BY ASYMMETRIC KEY asym_Key  
  
  
 -- ****************************************************************  
 -- Query Validate Account Vaccination  
 -- ****************************************************************  
  
 -- =============================================  
 -- Find Patient by Key (HKIC,HKBC, EC)  
 -- =============================================  
 --- Find Validate Account  
 insert into @temptable  
 (  
 Voucher_Acc_ID,  
 Scheme_Code,   
 Encrypt_Field1,  
 Encrypt_Field2,  
 DOB,  
 Exact_DOB,  
 Sex,  
 Date_of_Issue,  
 Account_Status,  
 Doc_Code  
 )  
 select   
 VA.Voucher_Acc_ID,   
 VA.Scheme_Code,   
 P.Encrypt_Field1,  
 P.Encrypt_Field2,  
 P.DOB,  
 P.Exact_DOB,  
 P.Sex,  
 P.Date_of_Issue,  
 VA.Record_Status,  
 P.Doc_Code  
 FROM  
  PersonalInformation P WITH (NOLOCK)  
  inner join VoucherAccount VA WITH (NOLOCK)  
   on P.Voucher_Acc_id = VA.voucher_acc_id   
  INNER JOIN @AvailableDocCode ADC  
   ON P.Doc_Code = ADC.Doc_Code  
 where   
 (P.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @In_IdentityNum)  
  or P.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @IdentityNum2))  
  
 -- Mark KeyFound if record found  
 SELECT @rowcount = count(Voucher_Acc_ID) FROM @temptable  
 IF @rowcount > 0  
 BEGIN  
  SET @Out_KeyFound = 1  
 END  
  
 -- =============================================  
 -- Find Patient who match Eng Name, DOB, Gender  
 -- (Delete other if not match in @temptable  
 -- =============================================  
 IF @In_ExactDOB = 'D'  
 BEGIN  
  SELECT @rowcount_PatientNotMatch = count(Voucher_Acc_ID)   
  FROM @temptable  
  WHERE REPLACE(REPLACE(REPLACE(CONVERT(VARCHAR(MAX), DecryptByKey(Encrypt_Field2)),',',''),' ',''),'-','') <> @In_Eng_Name  
        OR Sex <> @In_Sex  
        OR Account_Status='I'  
        OR (Exact_DOB <> 'D' AND DATEPART (year, DOB) <> DATEPART (year, @In_DOB))  
        OR (Exact_DOB = 'D'  AND DOB <> @In_DOB)  
  
 END  
 ELSE   
  BEGIN  
   -- @ExactDOB = 'Y' (Query all other type if ExactDOB is not 'D'  
   SELECT @rowcount_PatientNotMatch = count(Voucher_Acc_ID)   
   FROM @temptable  
   WHERE DATEPART (year, DOB) <> DATEPART (year, @In_DOB)  
        OR REPLACE(REPLACE(REPLACE(CONVERT(VARCHAR(MAX), DecryptByKey(Encrypt_Field2)),',',''),' ',''),'-','') <> @In_Eng_Name  
        OR Sex <> @In_Sex  
        -- OR Exact_DOB = 'D'   
        OR Account_Status='I'  

  END  
  
 -- Mark Patient Match if no record deleted  
 -- Mark Partial Match if some record deleted  
 IF @rowcount_PatientNotMatch < @rowcount  
 BEGIN  
  SET @Out_PatientMatch = 1  
  
  -- Collect exact match account ID  
  INSERT INTO @Account  
  SELECT DISTINCT 'V', Voucher_Acc_ID FROM @temptable  
 END  
   
 -- ****************************************************************  
 -- Query Temporary Account Vaccination  
 -- ****************************************************************  
 --- Find Temporary Account  
 DELETE @temptable  
 insert into @temptable  
 (  
 Voucher_Acc_ID,  
 Scheme_Code,   
 Encrypt_Field1,  
 Encrypt_Field2,  
 DOB,  
 Exact_DOB,  
 Sex,  
 Date_of_Issue,  
 Account_Status,  
 Doc_Code  
 )  
 select   
 TVA.Voucher_Acc_ID,   
 TVA.Scheme_Code,  
 TP.Encrypt_Field1,  
 TP.Encrypt_Field2,  
 TP.DOB,  
 TP.Exact_DOB,  
 TP.Sex,  
 TP.Date_of_Issue,  
 TVA.Record_Status,  
 TP.Doc_Code  
 from TempVoucherAccount TVA WITH (NOLOCK)  
  INNER JOIN TempPersonalInformation TP WITH (NOLOCK)  
   ON TVA.Voucher_Acc_ID = TP.Voucher_Acc_ID AND Account_Purpose NOT IN ('A','O') and  TVA.Record_Status NOT IN ('V', 'D')  
  INNER JOIN @AvailableDocCode ADC  
   ON TP.Doc_Code = ADC.Doc_Code  
 where   
  (TP.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @In_IdentityNum)  
   or TP.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @IdentityNum2))  
  
 -- Mark KeyFound if record found  
 SELECT @rowcount = count(Voucher_Acc_ID) FROM @temptable  
 IF @rowcount > 0  
 BEGIN  
  SET @Out_KeyFound = 1  
 END  
  
 -- =============================================  
 -- Find Patient who match Eng Name, DOB, Gender  
 -- (Delete other if not match in @temptable  
 -- =============================================  
 IF @In_ExactDOB = 'D'  
 BEGIN  
  DELETE @temptable WHERE REPLACE(REPLACE(REPLACE(CONVERT(VARCHAR(MAX), DecryptByKey(Encrypt_Field2)),',',''),' ',''),'-','') <> @In_Eng_Name  
        OR Sex <> @In_Sex  
        OR Account_Status='I'  
        OR (Exact_DOB <> 'D' AND DATEPART (year, DOB) <> DATEPART (year, @In_DOB))  
        OR (Exact_DOB = 'D'  AND DOB <> @In_DOB)  
 END  
 ELSE   
  BEGIN  
   -- @ExactDOB = 'Y' (Query all other type if ExactDOB is not 'D'  
   DELETE @temptable WHERE DATEPART (year, DOB) <> DATEPART (year, @In_DOB)  
        OR REPLACE(REPLACE(REPLACE(CONVERT(VARCHAR(MAX), DecryptByKey(Encrypt_Field2)),',',''),' ',''),'-','') <> @In_Eng_Name  
        OR Sex <> @In_Sex  
        -- OR Exact_DOB = 'D'   
        OR Account_Status='I'  
  END  
  
 -- Mark Patient Match if no record deleted  
 -- Mark Partial Match if some record deleted  
 SELECT @rowcount_PatientMatch = count(Voucher_Acc_ID) FROM @temptable  
 IF @rowcount_PatientMatch > 0 AND @rowcount_PatientMatch <> @rowcount  
 BEGIN  
  SET @Out_PartialMatch = 1  
  SET @Out_PatientMatch = 1  
 END  
 ELSE  
  IF @rowcount_PatientMatch <> 0  
  BEGIN   
   SET @Out_PatientMatch = 1  
  END  
  
 -- Collect exact match account ID  
 INSERT INTO @Account  
 SELECT 'T', Voucher_Acc_ID FROM @temptable  
  
 -- ****************************************************************  
 -- Query Special Account Vaccination  
 -- ****************************************************************  
 --- Find Special Account  
 DELETE @temptable  
 insert into @temptable  
 (  
 Voucher_Acc_ID,  
 Scheme_Code,   
 Encrypt_Field1,  
 Encrypt_Field2,  
 DOB,  
 Exact_DOB,  
 Sex,  
 Date_of_Issue,  
 Account_Status,  
 Doc_Code  
 )  
 select   
 TVA.Special_Acc_ID,   
 TVA.Scheme_Code,  
 TP.Encrypt_Field1,  
 TP.Encrypt_Field2,  
 TP.DOB,  
 TP.Exact_DOB,  
 TP.Sex,  
 TP.Date_of_Issue,  
 TVA.Record_Status,  
 TP.Doc_Code  
 FROM SpecialAccount TVA WITH (NOLOCK)  
  INNER JOIN SpecialPersonalInformation TP WITH (NOLOCK)  
   ON TVA.Special_Acc_ID = TP.Special_Acc_ID  
    AND TVA.Record_Status NOT IN ('V', 'D')
  INNER JOIN @AvailableDocCode ADC  
   ON TP.Doc_Code = ADC.Doc_Code  
 where   
  (TP.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @In_IdentityNum)  
  or TP.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @IdentityNum2))  
  
   
 -- Mark KeyFound if record found  
 SELECT @rowcount = count(Voucher_Acc_ID) FROM @temptable  
 IF @rowcount > 0  
 BEGIN  
  SET @Out_KeyFound = 1  
 END  
  
 -- =============================================  
 -- Find Patient who match Eng Name, DOB, Gender  
 -- (Delete other if not match in @temptable  
 -- =============================================  
 IF @In_ExactDOB = 'D'  
 BEGIN  
  DELETE @temptable WHERE REPLACE(REPLACE(REPLACE(CONVERT(VARCHAR(MAX), DecryptByKey(Encrypt_Field2)),',',''),' ',''),'-','') <> @In_Eng_Name  
        OR Sex <> @In_Sex  
        OR Account_Status='I'  
        OR (Exact_DOB <> 'D' AND DATEPART (year, DOB) <> DATEPART (year, @In_DOB))  
        OR (Exact_DOB = 'D'  AND DOB <> @In_DOB)  
 END  
 ELSE   
  BEGIN  
   -- @ExactDOB = 'Y' (Query all other type if ExactDOB is not 'D'  
   DELETE @temptable WHERE DATEPART (year, DOB) <> DATEPART (year, @In_DOB)  
        OR REPLACE(REPLACE(REPLACE(CONVERT(VARCHAR(MAX), DecryptByKey(Encrypt_Field2)),',',''),' ',''),'-','') <> @In_Eng_Name  
        OR Sex <> @In_Sex  
        -- OR Exact_DOB = 'D'   
        OR Account_Status='I'  
  END  
  
 -- Mark Patient Match if no record deleted  
 -- Mark Partial Match if some record deleted  
 SELECT @rowcount_PatientMatch = count(Voucher_Acc_ID) FROM @temptable  
 IF @rowcount_PatientMatch > 0 AND @rowcount_PatientMatch <> @rowcount  
 BEGIN  
  SET @Out_PartialMatch = 1  
  SET @Out_PatientMatch = 1  
 END  
 ELSE  
  IF @rowcount_PatientMatch <> 0  
  BEGIN   
   SET @Out_PatientMatch = 1  
  END  
  
 -- Collect exact match account ID  
 INSERT INTO @Account  
 SELECT 'S', Voucher_Acc_ID FROM @temptable  
  
 -- =============================================  
 -- Query vaccination record by Patient in @temptable  
 -- =============================================  
 INSERT INTO @tempVaccine  
 SELECT   
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
 (SELECT   
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
  from   
  (  
  SELECT  
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
   FROM VoucherTransaction VVT with (NoLock) , @Account VVR  
      WHERE 
      VVT.Record_Status NOT IN ('I','W','D') and  
      (@In_InjectionDateStart IS NULL OR VVT.Service_Receive_Dtm >= @In_InjectionDateStart) AND  
      (@In_InjectionDateEnd IS NULL OR VVT.Service_Receive_Dtm <= @In_InjectionDateEnd)    
   and VVT.Voucher_Acc_Id = VVR.Voucher_Acc_ID  
   AND VVR.Acc_Type = 'V'  
   AND VVT.Voucher_Acc_ID <> ''  
   AND VVT.Invalid_Acc_ID IS NULL  
  union  
  SELECT    
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
   FROM VoucherTransaction TVT with (NoLock) , @Account TVR  
      WHERE 
      TVT.Record_Status NOT IN ('I','W','D') and  
      (@In_InjectionDateStart IS NULL OR TVT.Service_Receive_Dtm >= @In_InjectionDateStart) AND  
      (@In_InjectionDateEnd IS NULL OR TVT.Service_Receive_Dtm <= @In_InjectionDateEnd)    
   and TVT.Temp_Voucher_Acc_ID = TVR.Voucher_Acc_ID   
   AND TVR.Acc_Type = 'T'  
   AND TVT.Temp_Voucher_Acc_ID <> ''   
   AND TVT.Voucher_Acc_ID = ''  
   AND TVT.Special_Acc_ID IS NULL  
  union   
  SELECT   
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
   FROM VoucherTransaction SVT with (NoLock) , @Account SVR  
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
     WHERE Source_System=@In_Source_System AND Target_System=@In_Target_System) VCM  
   ON REPLACE(TD.Scheme_Code + '|' + STR(TD.Scheme_Seq) + '|' + TD.Subsidize_Code,' ','') = VCM.Vaccine_Code_Source  
    AND EXISTS (SELECT * FROM @tempVaccineCode   
       WHERE VCM.Vaccine_Code_Target = Vaccine_Code_CMS OR SUBSTRING(VCM.Vaccine_Code_Target, 1,LEN(Vaccine_Code_CMS)) = Vaccine_Code_CMS)  
  INNER JOIN (SELECT Vaccine_dose_seq_code_Target, Vaccine_Dose_Seq_Code_Source, Vaccine_Dose_Seq_Code_Desc, Vaccine_Dose_Seq_Code_Desc_Chi  
     FROM VaccineDoseSeqCodeMapping WITH (NOLOCK)  
     WHERE Source_System=@In_Source_System AND Target_System=@In_Target_System AND [Subsidize_Item_Code_Source] = 'ALL') VDSCM   
   ON SIDs.Available_Item_Code = VDSCM.Vaccine_Dose_Seq_Code_Source  
  
 SELECT @rowcount = COUNT(1) FROM @tempVaccine  
   
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
  BEGIN SET @Out_VaccineResultCode =  1 END -- No 'partial record returned' for single demographic enquiry  
 END  
 ELSE  
 BEGIN SET @Out_VaccineResultCode =  2 END -- No record returned  
-- BEGIN SET @Out_VaccineResultCode =  1 END -- No 'partial record returned' for single demographic enquiry  
  
 CLOSE SYMMETRIC KEY sym_Key   
  
 -- Return result  
 DECLARE @CurrentSeasonStartDate AS DATETIME  
 SELECT @CurrentSeasonStartDate = Parm_Value1 FROM systemparameters   
  WHERE Parameter_Name = 'EHS_Get_Vaccine_WS_SeasonDateStart'  
    
 SELECT @RowCount = COUNT(injection_date) FROM @tempVaccine  
 WHERE injection_date >= @CurrentSeasonStartDate  
   
 IF @RowCount = 0  
  SELECT *, @OnSite AS onsite FROM @tempVaccine  
  WHERE injection_date >= @CurrentSeasonStartDate  
 ELSE  
  SELECT *, @OnSite AS onsite FROM @tempVaccine  
END  
GO

GRANT EXECUTE ON [dbo].[proc_eVaccination_get_byAccountDemographic] TO WSINT
GO

