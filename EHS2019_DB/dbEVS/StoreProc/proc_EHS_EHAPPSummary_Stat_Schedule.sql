IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_EHAPPSummary_Stat_Schedule]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_EHAPPSummary_Stat_Schedule]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	11 Sep 2017
-- CR No.:			CRE17-003 (Stop vaccine daily stat)
-- Description:		Stored procedure is no longer used
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Winnie SUEN
-- Modified date: 21 Apr 2015
-- Description:	1. Refine District Struture
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE13-018 Change Voucher Amount to 1 Dollar
-- Modified by:		Tommy LAM
-- Modified date:	29 Jan 2014
-- Description:		Rename Table:
--						1. [_TransactionSummaryCopaymentEHAPP] to [RpteHSD0023TransactionSummaryCopaymentEHAPP]
--						2. [_TransactionSummaryEHAPP] to [RpteHSD0023TransactionSummaryEHAPP]
--						3. [_VoucherAccSummaryEHAPP] to [RpteHSD0023VoucherAccSummaryEHAPP]
--					Re-structure of Table - [RpteHSD0023TransactionSummaryCopaymentEHAPP]:
--					Only keep the report data for 14 days
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRP13-003 Fix on counting method in EHAPP daily report (EHSD0023)
-- Modified by:		Tommy LAM
-- Modified date:	10 MAR 2014
-- Description:		Remove the usage of function - [CHECKSUM] for the counting
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			INT13-0029
-- Modified by:		Karl LAM
-- Modified date:	19 DEC 2013
-- Description:		Fix unable to relate the HCVS and EHAPP Claim if
--						1. Same Doc ID 
--						2. Only one of the temp account can be validated
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			INT13-0023 Wrong calculation on EHAPP report account summary
-- Modified by:		Koala CHENG
-- Modified date:	21 Oct 2013
-- Description:		Count Validated & temp account by Doc ID, Doc Type rather than voucher_acc_id
-- =============================================
-- Author:		Karl LAM
-- Create date: 16 Apr 2013
-- Description:	Get Stat for EHAPPSummary
-- =============================================
--exec proc_EHS_EHAPPSummary_Stat_Schedule
/*
CREATE PROCEDURE [dbo].[proc_EHS_EHAPPSummary_Stat_Schedule]

AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Report setting
-- =============================================
	DECLARE @CutOffDtm datetime
	SET @cutOffDtm = CONVERT(varchar(11), GETDATE(), 106) + ' 00:00'

	DECLARE @Report_ID Char(30)
	SET @Report_ID = 'eHSD0023'

	DECLARE @ReportDtm datetime
	SET @ReportDtm = DATEADD(day, -1, @cutOffDtm)

	DECLARE @ExpiryReportDtm datetime
	SET @ExpiryReportDtm = DATEADD(day, -14, @ReportDtm)

	DECLARE @Scheme_Code char(10)
	SET @Scheme_Code = 'HCVS'

	DECLARE @Scheme_Code_EHAPP char(10)
	SET @Scheme_Code_EHAPP = 'EHAPP'

	DECLARE @eligibileAge int

	DECLARE @noOfVoucher int

	DECLARE @ReasonForVisitEHAPPClaimMap table(
	Professional_Code	char(5),
	Reason_L1_Code	smallint,
	Reason_L2_Code	smallint)

	DECLARE @TxSummaryDuration int
	SET @TxSummaryDuration = 14 -- 2 weeks

-- =============================================
-- System data
-- =============================================
	SET @eligibileAge = 69 -- Hard coded 69 to handle case of selecting eligible age before scheme effective
-- SELECT  
--  @eligibileAge = er.[value]
-- FROM  
--  EligibilityRule er with(nolock) INNER JOIN SchemeClaim sc with(nolock) on er.scheme_code = sc.scheme_code and er.scheme_seq = sc.scheme_seq
-- WHERE  
--     sc.Scheme_Code = @Scheme_Code_EHAPP  
--      AND @reportDtm >= sc.Effective_Dtm  
--      AND @reportDtm < sc.Expiry_Dtm  


INSERT INTO @ReasonForVisitEHAPPClaimMap (Professional_Code,Reason_L1_Code,Reason_L2_Code) VALUES ('ENU',1,4)
INSERT INTO @ReasonForVisitEHAPPClaimMap (Professional_Code,Reason_L1_Code,Reason_L2_Code) VALUES ('RMP',1,2)
INSERT INTO @ReasonForVisitEHAPPClaimMap (Professional_Code,Reason_L1_Code,Reason_L2_Code) VALUES ('RNU',1,4)

-- =============================================
-- Create temporary table
-- =============================================
	CREATE table #vouchertransactionEHAPP (
		transaction_id			char(20) collate database_default,
		transaction_dtm			datetime,
		voucher_acc_id			char(15) collate database_default,
		temp_voucher_acc_id		char(15) collate database_default,
		service_type			char(5) collate database_default,
		sp_id					char(8) collate database_default,
		practice_display_seq	smallint,
		doc_code				char(20) collate database_default,
		Unit					int,
		Reason_for_visit_L1		smallint default 0,
		Reason_for_visit_L2		smallint default 0,
		identity_num			varbinary(100),
		age						int,
		Create_By_SmartID		char(1) collate database_default,		
		Scheme_Code				char(10) collate database_default,
		Practice_Name	nVarchar(100) collate database_default NULL ,
		Practice_Name_Chi nVarchar(100) collate database_default NULL,
		Service_Receive_Dtm datetime NOT NULL,
		CoPayment		varchar(50) NULL,
		HCVAmount		varchar(50) NULL,
		NetServiceFee	varchar(50) NULL,
		Record_Status	char(1) collate database_default NOT NULL,
		District char(4) collate database_default NULL,  
		District_name char(15) collate database_default NULL,  
		District_board char(15) collate database_default NULL,  
		Area_name char(50) collate database_default NULL,  
		Address_Code int NULL,
		WithHCVS bit NOT NULL default 0
	)

	CREATE INDEX IX_VAT on #vouchertransactionEHAPP (transaction_id)
	CREATE NONCLUSTERED INDEX IX_VAT1 ON #vouchertransactionEHAPP (identity_num)
	CREATE NONCLUSTERED INDEX IX_VAT2 ON #vouchertransactionEHAPP (service_type)
	CREATE NONCLUSTERED INDEX IX_VAT3 ON #vouchertransactionEHAPP (service_type,Reason_for_visit_L1)

	CREATE table #Account (
		voucher_acc_id			char(15) collate database_default,
		temp_voucher_acc_id		char(15) collate database_default,
		identity_num			varbinary(100),
		doc_code				char(10) collate database_default,
		dob						datetime,
		eligibile				char(1) collate database_default,
		age						int,
		Create_Dtm				datetime,
		First_Create_Dtm		datetime,
		Is_Terminate			char(1) collate database_default,
		Record_Status			char(1) collate database_default,		
		acc_type				char(1) collate database_default			-- V = Validated, T = Temp, S = Special
	)
	
	CREATE INDEX IX_VAT on #Account (identity_num)
	CREATE NONCLUSTERED INDEX IX_VAT1 ON #Account (identity_num, eligibile)
	CREATE NONCLUSTERED INDEX IX_VAT2 ON #Account (dob)

	 CREATE table #AccountFirstCreate (      
	  Identity_Num   varbinary(100),      
	  Doc_Code    char(10) collate database_default,      
	  First_Create_Dtm  datetime      
	 )      

BEGIN TRY
-- =============================================
-- Remove the data in the statistics tables for today
-- =============================================
	DELETE FROM RpteHSD0023TransactionSummaryEHAPP WHERE (report_dtm = @reportDtm) OR (report_dtm <= @ExpiryReportDtm)
	DELETE FROM RpteHSD0023TransactionSummaryCopaymentEHAPP WHERE (report_dtm = @reportDtm) OR (report_dtm <= @ExpiryReportDtm)
	DELETE FROM RpteHSD0023VoucherAccSummaryEHAPP WHERE (report_dtm = @reportDtm) OR (report_dtm <= @ExpiryReportDtm)

-- Get EHAPP account summary and the transaction related to those account
	INSERT INTO #vouchertransactionEHAPP (
		transaction_id,
		transaction_dtm,
		voucher_acc_id,
		temp_voucher_acc_id,
		service_type,
		sp_id,
		practice_display_seq,
		doc_code,
		Unit,
		Reason_for_visit_L1,
		Reason_for_visit_L2,
		identity_num,
		age,
		Create_By_SmartID,
		Scheme_Code,
		Practice_Name,
		Practice_Name_Chi,
		Service_Receive_Dtm,
		CoPayment,
		HCVAmount,
		NetServiceFee,
		Record_Status,
		District,
		Address_Code
	)
	SELECT
		VT.Transaction_ID,
		VT.Transaction_Dtm,
		ISNULL(VT.Voucher_Acc_ID, ''),
		ISNULL(VT.Temp_Voucher_Acc_ID, ''),
		VT.Service_Type,
		VT.SP_ID,
		VT.Practice_Display_Seq,
		VT.Doc_Code,
		TD.Unit,
		CONVERT(smallint, ISNULL(TAF1.AdditionalFieldValueCode, 0)) AS Reason_for_Visit_L1, 
		CONVERT(smallint, ISNULL(TAF2.AdditionalFieldValueCode, 0)) AS Reason_for_Visit_L2,
		Case isnull(VT.Voucher_Acc_ID,'') when '' then TP.Encrypt_Field1 else VP.Encrypt_Field1 end AS [Identity_Num],
		Case isnull(VT.Voucher_Acc_ID,'') when '' then DATEDIFF(yy, TP.DOB, @reportDtm) else DATEDIFF(yy, VP.DOB, @reportDtm) end AS [Identity_Num],
		VT.Create_By_SmartID,
		VT.Scheme_Code,
		p.Practice_Name,
		p.Practice_Name_Chi,
		VT.Service_Receive_Dtm,
		TAF3.AdditionalFieldValueCode,
		TAF4.AdditionalFieldValueCode,
		TAF5.AdditionalFieldValueCode,
		VT.Record_Status,
		p.district,  
		p.address_code
	FROM
		VoucherTransaction VT with(nolock)
		INNER JOIN TransactionDetail TD with(nolock)
				ON VT.Transaction_ID = TD.Transaction_ID
		LEFT OUTER JOIN TransactionAdditionalField TAF1 with(nolock)
				ON VT.Transaction_ID = TAF1.Transaction_ID
					AND TAF1.AdditionalFieldID = 'Reason_for_Visit_L1'
		LEFT OUTER JOIN TransactionAdditionalField TAF2 with(nolock)
				ON VT.Transaction_ID = TAF2.Transaction_ID
					AND TAF2.AdditionalFieldID = 'Reason_for_Visit_L2'
		LEFT OUTER JOIN TransactionAdditionalField TAF3 with(nolock)
				ON VT.Transaction_ID = TAF3.Transaction_ID
					AND TAF3.AdditionalFieldID = 'CoPayment'
		LEFT OUTER JOIN TransactionAdditionalField TAF4 with(nolock)
				ON VT.Transaction_ID = TAF4.Transaction_ID
					AND TAF4.AdditionalFieldID = 'HCVAmount'
		LEFT OUTER JOIN TransactionAdditionalField TAF5 with(nolock)
				ON VT.Transaction_ID = TAF5.Transaction_ID
					AND TAF5.AdditionalFieldID = 'NetServiceFee'
		INNER JOIN PracticeSchemeInfo info  with(nolock)
				ON VT.SP_ID = info.SP_ID and VT.Practice_Display_Seq = info.Practice_Display_Seq
		INNER JOIN SchemeEnrolClaimMap map with(nolock)
				ON info.Scheme_code = map.Scheme_Code_Claim and map.Scheme_Code_Enrol = @Scheme_Code_EHAPP
		LEFT OUTER JOIN PersonalInformation VP with(nolock)
				ON VT.Voucher_Acc_ID = VP.Voucher_Acc_ID COLLATE DATABASE_DEFAULT
		LEFT OUTER JOIN TempPersonalInformation TP with(nolock)
				ON VT.Temp_Voucher_Acc_ID = TP.Voucher_Acc_ID COLLATE DATABASE_DEFAULT
		INNER JOIN Practice p with(nolock)
				ON VT.SP_ID = p.SP_ID and VT.Practice_Display_Seq = p.Display_Seq

	WHERE
		VT.Record_Status NOT IN 
						(SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = @Report_ID) 
						AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Record_Status' 
						AND ((Effective_Date is null or Effective_Date>= @cutOffDtm) AND (Expiry_Date is null or Expiry_Date < @cutOffDtm)))
		AND VT.Scheme_Code in (@scheme_code, @Scheme_Code_EHAPP)
		AND VT.Transaction_Dtm < @cutOffDtm
		AND (VT.Invalidation IS NULL OR VT.Invalidation NOT In 
			(SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = @Report_ID) 
			AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'
			AND ((Effective_Date is null or Effective_Date>= @cutOffDtm) AND (Expiry_Date is null or Expiry_Date < @cutOffDtm))))
		
		

-- ---------------------------------------------
-- Retrieve all validated and temporary accounts (voucher will not have special account)
-- ---------------------------------------------

-- Validated

	insert into #account
	(
		voucher_acc_id,
		temp_voucher_acc_id,
		identity_num,
		doc_code,
		dob,
		Create_Dtm,
		Is_Terminate,
		Record_Status,	-- CRP11-010: 06 Sep 2011
		acc_type,
		age,
		eligibile
	)
	select p.voucher_acc_id,
			NULL,
			p.Encrypt_Field1,
			Case P.Doc_Code when 'HKBC' then 'HKIC' else P.Doc_Code end,
			p.dob,
			CASE p.Doc_Code
				WHEN 'HKIC' THEN '1900-01-01'
				ELSE '1900-01-02'
			END AS [Create_Dtm],
			CASE VA.Record_Status
				WHEN 'D' THEN 'Y'
				ELSE 'N'
			END AS [Is_Terminate],
			va.Record_Status,
			'V'	AS acc_type,
			DATEDIFF(yy, P.DOB, @reportDtm) ,
			'N'
	FROM 
		VoucherAccount va with(nolock)
			INNER JOIN PersonalInformation p with(nolock)
				ON va.Voucher_Acc_ID = p.Voucher_Acc_ID
	WHERE
			VA.Create_Dtm < @cutOffDtm


-- Temporary

	INSERT INTO #account
	(
		voucher_acc_id,
		temp_voucher_acc_id,
		identity_num,
		doc_code,
		dob,
		Create_Dtm,
		acc_type,
		age,
		eligibile,
		Is_Terminate
	)
	SELECT NULL,
			p.voucher_acc_id,		
			p.Encrypt_Field1,
			Case P.Doc_Code when 'HKBC' then 'HKIC' else P.Doc_Code end,
			p.dob,
			ta.Create_Dtm,
			CASE  WHEN va.voucher_acc_id IS NOT NULL THEN 'V' ELSE 'T' END	AS acc_type,
			DATEDIFF(yy, P.DOB, @reportDtm) ,
			'N',
			'N'
	FROM
		TempVoucherAccount ta with(nolock)
			INNER JOIN TempPersonalInformation p with(nolock)
				ON ta.voucher_acc_id = p.voucher_acc_id
			LEFT JOIN #account va 
				ON p.doc_code = va.doc_code
				   AND p.Encrypt_Field1 = va.Identity_Num
	WHERE 
		ta.Record_Status NOT IN ('V', 'D')
	and	ta.account_purpose in ('C', 'V')
	and ta.create_dtm < @cutOffDtm


-- Special

	INSERT INTO #account (
		voucher_acc_id,
		temp_voucher_acc_id,
		identity_num,
		doc_code,
		dob,
		Create_Dtm,
		acc_type,
		age,
		eligibile,
		Is_Terminate
	)
	SELECT
		NULL AS [voucher_acc_id],
		NULL AS [temp_voucher_acc_id], -- Special accounts will never have transaction related to EHAPP, so NULL these 2 fields
		SP.Encrypt_Field1,
		Case SP.Doc_Code when 'HKBC' then 'HKIC' else SP.Doc_Code end,
		SP.DOB,
		SA.Create_Dtm,
		'S'	AS acc_type,
		DATEDIFF(yy, SP.DOB, @reportDtm) ,
		'N',
		'N'
	FROM
		SpecialAccount SA with(nolock)
			INNER JOIN SpecialPersonalInformation SP with(nolock)
				ON SA.Special_Acc_ID = SP.Special_Acc_ID
	WHERE
		SA.Record_Status NOT IN ('V', 'D')
			AND SA.Account_Purpose IN ('C', 'V')
			AND SA.Create_Dtm < @cutOffDtm	



      
--Handle First Create account (delete those created afterwards)         
 INSERT INTO #AccountFirstCreate (      
  Identity_Num,      
  Doc_Code,      
  First_Create_Dtm      
 )      
 SELECT      
  Identity_Num,      
  Doc_Code,      
  MIN(Create_Dtm) AS [First_Create_Dtm]      
 FROM      
  #account      
 GROUP BY      
  Identity_Num,      
  Doc_Code      
        
 UPDATE      
  #account      
 SET      
  First_Create_Dtm = AFC.First_Create_Dtm      
 FROM      
  #account A      
   INNER JOIN #AccountFirstCreate AFC      
    ON A.Identity_Num = AFC.Identity_Num      
  AND A.Doc_Code = AFC.Doc_Code       
       
       
 DELETE      
  #account     
 WHERE      
  Create_Dtm <> First_Create_Dtm  AND isnull(temp_voucher_acc_id,'')  = ''  
  AND NOT EXISTS (  
  SELECT  distinct Voucher_Acc_Id FROM vouchertransaction  VT
  WHERE #account.Voucher_Acc_Id = VT.Voucher_Acc_Id 
  and
  VT.Record_Status NOT IN       
      (SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = @Report_ID)       
      AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Record_Status'       
  AND ((Effective_Date is null or Effective_Date>= @cutOffDtm) AND (Expiry_Date is null or Expiry_Date < @cutOffDtm)))      
  AND VT.Scheme_Code in (@scheme_code, @Scheme_Code_EHAPP)      
  AND VT.Transaction_Dtm < @cutOffDtm      
  AND (VT.Invalidation IS NULL OR VT.Invalidation NOT In       
   (SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = @Report_ID)       
   AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'      
   AND ((Effective_Date is null or Effective_Date>= @cutOffDtm) AND (Expiry_Date is null or Expiry_Date < @cutOffDtm))))                
  )  
    
 DELETE      
  #account     
 WHERE      
  Create_Dtm <> First_Create_Dtm  AND isnull(Voucher_Acc_Id,'')  = ''  
  AND NOT EXISTS (  
  SELECT  distinct temp_voucher_acc_id FROM vouchertransaction  VT
  WHERE #account.temp_voucher_acc_id = VT.temp_voucher_acc_id 
  and
  VT.Record_Status NOT IN       
      (SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = @Report_ID)       
      AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Record_Status'       
  AND ((Effective_Date is null or Effective_Date>= @cutOffDtm) AND (Expiry_Date is null or Expiry_Date < @cutOffDtm)))      
  AND VT.Scheme_Code in (@scheme_code, @Scheme_Code_EHAPP)      
  AND VT.Transaction_Dtm < @cutOffDtm      
  AND (VT.Invalidation IS NULL OR VT.Invalidation NOT In       
   (SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = @Report_ID)       
   AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'      
   AND ((Effective_Date is null or Effective_Date>= @cutOffDtm) AND (Expiry_Date is null or Expiry_Date < @cutOffDtm))))    )  
    



--Patch Data
	update #account
	set eligibile = 'Y'
	where age > @eligibileAge

 
-- =============================================
-- Process data for account statistics
-- =============================================
	
	DECLARE @totalVA as int
	DECLARE @ValidVA as int
	DECLARE @ValidateAccountActive		int
	DECLARE @ValidateAccountSuspend		int
	DECLARE @ValidateAccountTerminate	int
	DECLARE @ValidateAccountTotal		int
	DECLARE @ValidateAccWithEHAPP int
	DECLARE @TempAccWithEHAPP int
	DECLARE @AccWithEHAPP int
	DECLARE @AccWithoutEHAPP int

	DECLARE @ValidateAccount table (    
	  Record_Status   Char(1),    
	  Record_Count    int    
	)


	SELECT @totalVA = COUNT(DISTINCT identity_num) FROM #account 
	WHERE age > @eligibileAge and 	Is_Terminate = 'N'

	SELECT	@ValidateAccWithEHAPP = Count (DISTINCT acc.Identity_Num)  
	--SELECT	@ValidateAccWithEHAPP = Count (DISTINCT acc.voucher_acc_id)
	FROM	#account acc INNER JOIN #vouchertransactionEHAPP tx
			on acc.Doc_COde = tx.Doc_COde  
				AND acc.Identity_Num = tx.Identity_Num 
			--on acc.voucher_acc_id = tx.voucher_acc_id
	WHERE	acc.Is_Terminate = 'N' 
			AND acc.acc_type = 'V'
			--AND isnull(acc.voucher_acc_id,'') <> '' 
			AND tx.scheme_code = @Scheme_Code_EHAPP 

	SELECT	@TempAccWithEHAPP = Count (DISTINCT acc.Identity_Num)  
	FROM	#account acc INNER JOIN #vouchertransactionEHAPP tx  
			on acc.Doc_COde = tx.Doc_COde  
				AND acc.Identity_Num = tx.Identity_Num  
	WHERE	acc.Is_Terminate = 'N'   
			AND acc.acc_type = 'T'
			--AND isnull(acc.Temp_voucher_acc_id,'') <> ''   
			AND tx.scheme_code = @Scheme_Code_EHAPP    

	SET @AccWithEHAPP = @ValidateAccWithEHAPP + @TempAccWithEHAPP
 
	SELECT @AccWithoutEHAPP = @totalVA - @AccWithEHAPP


SET @ValidateAccountActive = 0  
SET @ValidateAccountSuspend = 0  
SET @ValidateAccountTerminate = 0

INSERT INTO @ValidateAccount   
 (    
  Record_Status, Record_Count    
 )    
 Select Record_Status, Count(1)   
 FROM #account  
 Where voucher_acc_id IS NOT NULL AND Create_Dtm < @cutOffDtm And YEAR(@reportDtm) - YEAR(dob) > @eligibileAge  
 GROUP BY Record_Status

 SELECT @ValidVA = SUM(Record_Count) FROM @ValidateAccount WHERE Record_Status <> 'D'    
  
 SELECT @ValidateAccountActive = ISNULL(Record_Count,0) FROM @ValidateAccount WHERE Record_Status = 'A'    
  
 SELECT @ValidateAccountSuspend = ISNULL(Record_Count,0) FROM @ValidateAccount WHERE Record_Status = 'S'    
  
 SELECT @ValidateAccountTerminate = ISNULL(Record_Count,0) FROM @ValidateAccount WHERE Record_Status = 'D'


	SELECT
		@ValidateAccountTotal = 
			@ValidateAccountActive +
			@ValidateAccountSuspend +
			@ValidateAccountTerminate

insert into RpteHSD0023VoucherAccSummaryEHAPP
(
	System_dtm,
	report_dtm,
	TotalAcc,
	ValidAcc,
	ValidateAccountActive,
	ValidateAccountSuspend,
	ValidateAccountTerminate,
	ValidateAccountTotal,
	WithEHAPPExcludeTeminatedAcc,
	ValidateAccountWithEHAPPExcludeTeminatedAcc,
	TempAccountWithEHAPPExcludeTeminatedAcc,
	WithoutEHAPPExcludeTeminatedAcc
)
select getdate(), @reportDtm, isnull(@totalVA,0), isnull(@ValidVA,0),
		ISNULL(@ValidateAccountActive, 0),
		ISNULL(@ValidateAccountSuspend, 0),
		ISNULL(@ValidateAccountTerminate, 0),
		ISNULL(@ValidateAccountTotal, 0),
		ISNULL(@AccWithEHAPP, 0),
		ISNULL(@ValidateAccWithEHAPP, 0),
		ISNULL(@TempAccWithEHAPP, 0),
		ISNULL(@AccWithOutEHAPP, 0)


-- =============================================
-- Get Transaction Details
-- =============================================


--Update Address Deatils
  
	UPDATE #vouchertransactionEHAPP  
	SET #vouchertransactionEHAPP.district_name = district.district_name,  
	 #vouchertransactionEHAPP.district_board = district.district_board,  
	 #vouchertransactionEHAPP.area_name = district_area.area_name  
	FROM district, district_area, DistrictBoard
	WHERE #vouchertransactionEHAPP.district = district.district_code collate database_default  
	  and district.District_Board = DistrictBoard.District_Board
	  and DistrictBoard.Area_Code = district_area.area_code  


--With HCVS Claim
	UPDATE	#vouchertransactionEHAPP SET WithHCVS = 1
	--SELECT	Distinct EHAPP.Transaction_ID, OTHERS.*
	FROM 	(SELECT * FROM #vouchertransactionEHAPP where Scheme_Code = @Scheme_Code_EHAPP) EHAPP,
			(SELECT * FROM #vouchertransactionEHAPP where Scheme_Code <> @Scheme_Code_EHAPP) OTHERS,
			SchemeEnrolClaimMap map, PracticeSchemeInfo pinfo, @ReasonForVisitEHAPPClaimMap reasonMap
	WHERE	#vouchertransactionEHAPP.Transaction_ID = EHAPP.Transaction_ID and			
			EHAPP.SP_ID = OTHERS.SP_ID and 
			EHAPP.Practice_Display_Seq = OTHERS.Practice_Display_Seq and 
			EHAPP.Doc_Code = OTHERS.Doc_Code and 			
			EHAPP.identity_num = OTHERS.identity_num and 			
			Datediff(day, EHAPP.Service_Receive_Dtm, OTHERS.Service_Receive_Dtm) = 0 and
			reasonMap.Professional_Code collate database_default = OTHERS.Service_Type and
			reasonMap.Reason_L1_Code = OTHERS.Reason_for_visit_L1 and
			reasonMap.Reason_L2_Code = OTHERS.Reason_for_visit_L2 



--Insert data
	INSERT INTO RpteHSD0023TransactionSummaryEHAPP(
	System_dtm,
	Transaction_ID,
	Transaction_Dtm,
	SP_ID,
	Practice_No,
	Practice_Name,
	Practice_Name_Chi,
	Service_Type,
	District_Name,
	District_Board,
	Area_Name,
	Service_Receive_Dtm,
	Doc_Code,
	CoPayment,
	Record_Status,
	Report_Dtm,
	Create_By_SmartID,
	WithHCVS,
	HCVAmount,
	NetServiceFee
	) 
	SELECT 
	getdate(),  
	transaction_id,
	transaction_dtm,  	
	SP_ID,
	practice_display_seq,
	Practice_Name,
	Practice_Name_Chi,
	Service_Type,
	District_Name,
	District_Board,
	Area_Name,
	Service_Receive_Dtm,
	Doc_Code,
	CoPayment,
	Record_Status,
	@reportDtm,  
	case when Create_By_SmartID = 'Y' THEN 'Card Reader' ELSE 'Manual' END Create_By_SmartID,
	WithHCVS,
	HCVAmount,
	NetServiceFee
	from #vouchertransactionEHAPP 
	WHERE Scheme_Code = @Scheme_Code_EHAPP 
		and transaction_dtm between dateadd(day, -@TxSummaryDuration, @cutOffDtm) and @cutOffDtm

	-- No. of Transaction
	INSERT INTO RpteHSD0023TransactionSummaryCopaymentEHAPP(System_dtm,Report_Dtm,Field_Name,Field_Value)
		SELECT GETDATE(), @reportDtm, 'No_Of_Trans', COUNT(Transaction_ID)
		FROM #vouchertransactionEHAPP
		WHERE Scheme_Code = @Scheme_Code_EHAPP

	-- No. of involved SP
	INSERT INTO RpteHSD0023TransactionSummaryCopaymentEHAPP(System_dtm,Report_Dtm,Field_Name,Field_Value)
		SELECT GETDATE(), @reportDtm, 'No_Of_Involved_SP', COUNT(DISTINCT SP_ID)
		FROM #vouchertransactionEHAPP
		WHERE Scheme_Code = @Scheme_Code_EHAPP

	-- With HCVS Claim for EHAPP Co-payment
	INSERT INTO RpteHSD0023TransactionSummaryCopaymentEHAPP(System_dtm,Report_Dtm,Field_Name,Field_Value)
		SELECT GETDATE(), @reportDtm, 'HCV_With_HCVSClaim', COUNT(Transaction_ID)
		FROM #vouchertransactionEHAPP
		WHERE Scheme_Code = @Scheme_Code_EHAPP AND CoPayment = 'HCV' AND WithHCVS = 1

	-- Without HCVS Claim for EHAPP Co-payment
	INSERT INTO RpteHSD0023TransactionSummaryCopaymentEHAPP(System_dtm,Report_Dtm,Field_Name,Field_Value)
		SELECT GETDATE(), @reportDtm, 'HCV_Without_HCVSClaim', COUNT(Transaction_ID)
		FROM #vouchertransactionEHAPP
		WHERE Scheme_Code = @Scheme_Code_EHAPP AND CoPayment = 'HCV' AND WithHCVS = 0

	-- $100
	INSERT INTO RpteHSD0023TransactionSummaryCopaymentEHAPP(System_dtm,Report_Dtm,Field_Name,Field_Value)
		SELECT GETDATE(), @reportDtm, 'F100', COUNT(Transaction_ID)
		FROM #vouchertransactionEHAPP
		WHERE Scheme_Code = @Scheme_Code_EHAPP AND CoPayment = 'F100'

	-- CSSA
	INSERT INTO RpteHSD0023TransactionSummaryCopaymentEHAPP(System_dtm,Report_Dtm,Field_Name,Field_Value)
		SELECT GETDATE(), @reportDtm, 'CSSA', COUNT(Transaction_ID)
		FROM #vouchertransactionEHAPP
		WHERE Scheme_Code = @Scheme_Code_EHAPP AND CoPayment = 'CSSA'

	-- Medical Wavier
	INSERT INTO RpteHSD0023TransactionSummaryCopaymentEHAPP(System_dtm,Report_Dtm,Field_Name,Field_Value)
		SELECT GETDATE(), @reportDtm, 'MED_WAIVE', COUNT(Transaction_ID)
		FROM #vouchertransactionEHAPP
		WHERE Scheme_Code = @Scheme_Code_EHAPP AND CoPayment = 'MED_WAIVE'

	-- Total HCV Amount
	INSERT INTO RpteHSD0023TransactionSummaryCopaymentEHAPP(System_dtm,Report_Dtm,Field_Name,Field_Value)
		SELECT GETDATE(), @reportDtm, 'Total_HCVAmount', SUM(CONVERT(int, HCVAmount))
		FROM #vouchertransactionEHAPP
		WHERE Scheme_Code = @Scheme_Code_EHAPP AND CoPayment = 'HCV'

--Drop tables
	Drop table #vouchertransactionEHAPP
	Drop table #Account
	Drop table #AccountFirstCreate

--Return 'S' if success
	SELECT 'S' as [Result]
END TRY

BEGIN CATCH
	SELECT 'F' as [Result]
END CATCH
 
END
GO

GRANT EXECUTE ON proc_EHS_EHAPPSummary_Stat_Schedule TO HCVU
GO
*/