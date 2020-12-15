IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSW0006_Report_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSW0006_Report_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:	    Chris YIM
-- Modified date:	14 Dec 2020
-- CR No.:			INT20-0060
-- Description:	  	Performance tuning
-- =============================================
-- =============================================
-- Author:			Chris YIM
-- Create date:		29 Jul 2020
-- CR No.			CRE19-031 (VSS MMR Upload)
-- Description:		VSS Non-Immune Adults Weekly Statistic Report
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_eHSW0006_Report_get] 
	@Request_Time			DATETIME = NULL,	-- The reference date to get @CutOff_Dtm. It's [Request_Dtm] from [FileGenerationQueue] Table (* Passed in from Excel Generator. When changing this field, plz also update the corresponding Excel Generator)
	@CutOff_Dtm				DATETIME = NULL	-- Inclusive, The Cut Off Date. If defined, it will override the value from the @request_dtm

AS BEGIN
	-- =============================================    
	-- Declaration    
	-- =============================================  
	DECLARE	@In_Request_Time DATETIME	
	DECLARE	@In_CutOff_Dtm DATETIME	

	DECLARE	@Report_Dtm DATETIME	
	DECLARE	@Subsidy_Start_Dtm DATETIME	

	DECLARE @GenerationDtm VARCHAR(50)  

	DECLARE @Date_Range INT

	CREATE TABLE #StudentFileEntryBase(
		Transaction_ID			CHAR(20),
		Non_immune_to_measles	VARCHAR(2),
		Ethnicity				VARCHAR(100),
		Category1				VARCHAR(100),
		Category2				VARCHAR(100),
		Lot_Number				VARCHAR(15)
	)   

	CREATE NONCLUSTERED INDEX IX_StudentFileEntryBase_Transaction_ID
		ON #StudentFileEntryBase (Transaction_ID); 
  
	CREATE TABLE #Tran(
		Transaction_ID			CHAR(20),
		Transaction_Dtm			DATETIME,
		SP_ID					CHAR(8),
		Service_Receive_Dtm		DATETIME,       
		Vaccine					VARCHAR(20),    
		Scheme_Seq				INT,
		Subsidize_Item_Code		CHAR(10),   
		Dose					CHAR(20),          
		Voucher_Acc_ID			CHAR(15),          
		Temp_Voucher_Acc_ID		CHAR(15),          
		Special_Acc_ID			CHAR(15),          
		Invalid_Acc_ID			CHAR(15),          
		Doc_Code				CHAR(20),  
		HKIC_Symbol				CHAR(1),		
		Transaction_Status		CHAR(1),          
		Reimbursement_Status	CHAR(1),
		Lab_Test_Result			VARCHAR(2),
		Ethnicity				VARCHAR(100),
		Category1				VARCHAR(100),
		Category2				VARCHAR(100),
		Lot_Number				VARCHAR(15)
	)          
          
	CREATE TABLE #Account(          
		Transaction_ID			CHAR(20),
		Transaction_Dtm			DATETIME,
		SP_ID					CHAR(8),
		Service_Receive_Dtm		DATETIME,                
		Vaccine					varCHAR(20),
		Scheme_Seq				INT,
		Dose					CHAR(20),          
		DOB						DATETIME,          
		Exact_DOB				CHAR(1),          
		Sex						CHAR(1),          
		Doc_Code				CHAR(20), 
		HKIC_Symbol				CHAR(1),
		Transaction_Status		CHAR(1),          
		Reimbursement_Status	CHAR(1)
	)   

	CREATE TABLE #Report_01(    
		Display_Seq INT,
		Value1			VARCHAR(200) DEFAULT '',    
		Value2			VARCHAR(200) DEFAULT '',    
		Value3			VARCHAR(200) DEFAULT '',    
		Value4			VARCHAR(200) DEFAULT '',    
		Value5			VARCHAR(200) DEFAULT '',    
		Value6			VARCHAR(200) DEFAULT '',    
		Value7			VARCHAR(200) DEFAULT '',    
		Value8			VARCHAR(200) DEFAULT '',    
		Value9			VARCHAR(200) DEFAULT '',    
		Value10			VARCHAR(200) DEFAULT '',    
		Value11			VARCHAR(200) DEFAULT '',
		Value12			VARCHAR(200) DEFAULT '',
		Value13			VARCHAR(200) DEFAULT '',
		Value14			VARCHAR(200) DEFAULT '',
		Value15			VARCHAR(200) DEFAULT '',
		Value16			VARCHAR(200) DEFAULT ''
	)

	CREATE TABLE #Report_02(    
		Display_Seq		INT,
		Value1			VARCHAR(200) DEFAULT '',    
		Value2			VARCHAR(200) DEFAULT '',    
		Value3			VARCHAR(200) DEFAULT '',    
		Value4			VARCHAR(200) DEFAULT '',    
		Value5			VARCHAR(200) DEFAULT '',    
		Value6			VARCHAR(200) DEFAULT '',    
		Value7			VARCHAR(200) DEFAULT '',    
		Value8			VARCHAR(200) DEFAULT '',    
		Value9			VARCHAR(200) DEFAULT '',    
		Value10			VARCHAR(200) DEFAULT '',    
		Value11			VARCHAR(200) DEFAULT '',
		Value12			VARCHAR(200) DEFAULT '',
		Value13			VARCHAR(200) DEFAULT '',
		Value14			VARCHAR(200) DEFAULT '',
		Value15			VARCHAR(200) DEFAULT '',
		Value16			VARCHAR(200) DEFAULT '',
		Value17			VARCHAR(200) DEFAULT ''
	)

	CREATE TABLE #Remark
	(    
		Display_Seq		INT IDENTITY(1,1),
		Value1			VARCHAR(2000)  default '',    
		Value2			VARCHAR(2000)  default '',    
	)

	-- =============================================    
	-- Validation     
	-- =============================================    
	-- =============================================    
	-- Initialization    
	-- =============================================  
	SET @In_Request_Time = @Request_Time
	SET @In_CutOff_Dtm = @CutOff_Dtm


	IF @In_Request_Time IS NULL
		SET @In_Request_Time = GETDATE()

	-- Ensure the time start from 00:00 (DATETIME compare logic use "<")
	IF @In_CutOff_Dtm IS NULL
		SET @Report_Dtm = CONVERT(VARCHAR, @In_Request_Time, 106) 		
	ELSE
		SET @Report_Dtm = CONVERT(VARCHAR, DATEADD(d, 1, @In_CutOff_Dtm), 106)-- "106" gives "dd MMM yyyy"  

	SET @Date_Range = 7 

	-- Start date of VSS MMR
	SET @Subsidy_Start_Dtm = (SELECT Claim_Period_From FROM SubsidizeGroupClaim WHERE Scheme_Code = 'VSS' AND Scheme_Seq = 1 AND Subsidize_Code = 'VNIAMMR')

	-- =============================================    
	-- Prepare Data
	-- =============================================  

	-- Retrieve Student File Data    
	INSERT INTO #StudentFileEntryBase(
		Transaction_ID,
		Non_immune_to_measles,
		Ethnicity,
		Category1,
		Category2,
		Lot_Number
	)
	SELECT
		SFE.Transaction_ID,
		SFEMMRF.Non_immune_to_measles,
		SFEMMRF.Ethnicity,
		SFEMMRF.Category1,
		SFEMMRF.Category2,
		SFEMMRF.Lot_Number
	FROM
		StudentFileEntry SFE WITH (NOLOCK)
			LEFT JOIN StudentFileEntryMMRField SFEMMRF WITH (NOLOCK)
				ON SFE.Student_File_ID = SFEMMRF.Student_File_ID 
					AND SFE.Student_Seq = SFEMMRF.Student_Seq
	WHERE
		SFE.Transaction_ID IS NOT NULL

	-- Retrieve Transaction Data     
	INSERT INTO #Tran(          
		Transaction_ID,          
		Transaction_Dtm, 
		SP_ID,    
		Service_Receive_Dtm,          
		Vaccine,         
		Scheme_Seq,
		Subsidize_Item_Code,
		Dose,          
		Voucher_Acc_ID,          
		Temp_Voucher_Acc_ID,          
		Special_Acc_ID,          
		Invalid_Acc_ID,          
		Doc_Code,        
		HKIC_Symbol,
		Transaction_Status,          
		Reimbursement_Status,
		Lab_Test_Result,
		Ethnicity,
		Category1,
		Category2,
		Lot_Number
	)          
	SELECT         
		VT.Transaction_ID,          
		VT.Transaction_Dtm, 
		VT.SP_ID,    
		VT.Service_Receive_Dtm,          
		SGC.Display_Code_For_Claim [Vaccine],
		TD.Scheme_Seq,
		TD.Subsidize_Item_Code,          
		TD.Available_Item_Code AS [Dose],          
		ISNULL(VT.Voucher_Acc_ID, ''),          
		ISNULL(VT.Temp_Voucher_Acc_ID, ''),          
		ISNULL(VT.Special_Acc_ID, ''),          
		ISNULL(VT.Invalid_Acc_ID, ''),          
		VT.Doc_Code,       
		VT.HKIC_Symbol,
		VT.Record_Status AS [Transaction_Status],          
		NULL AS [Reimbursement_Status],
		[Lab_Test_Result]=SFEMMRF.Non_immune_to_measles,
		[Ethnicity]=SFEMMRF.Ethnicity,
		[Category1]=SFEMMRF.Category1,
		[Category2]=SFEMMRF.Category2,
		[Lot_Number]=SFEMMRF.Lot_Number
	FROM          
		VoucherTransaction VT WITH (NOLOCK)
			INNER JOIN TransactionDetail TD WITH (NOLOCK)
				ON VT.Transaction_ID = TD.Transaction_ID
			LEFT JOIN SubsidizeGroupClaim SGC WITH (NOLOCK)
				ON	TD.Scheme_Code = SGC.Scheme_Code
					AND	TD.Scheme_Seq = SGC.Scheme_Seq
					AND TD.Subsidize_Code = SGC.Subsidize_Code     
			LEFT JOIN #StudentFileEntryBase SFEMMRF
				ON VT.Transaction_ID = SFEMMRF.Transaction_ID
			--LEFT JOIN StudentFileEntry SFE
			--	ON VT.Transaction_ID = SFE.Transaction_ID
			--LEFT JOIN StudentFileEntryMMRField SFEMMRF
			--	ON SFE.Student_File_ID = SFEMMRF.Student_File_ID 
			--		AND SFE.Student_Seq = SFEMMRF.Student_Seq
	WHERE          
		VT.Scheme_Code = 'VSS'          
		AND TD.Subsidize_Code = 'VNIAMMR' 
		AND TD.Subsidize_Item_Code = 'MMR'       
		AND VT.Transaction_Dtm >= @Subsidy_Start_Dtm AND VT.Transaction_Dtm < @Report_Dtm            
		AND VT.Record_Status NOT IN ('I','D')           
		AND (VT.Invalidation IS NULL OR VT.Invalidation <> 'I')
	ORDER BY 
		VT.Transaction_Dtm

	-- ---------------------------------------------          
	-- Patch the Reimbursement_Status 
	-- for transaction created in payment outside eHS
	-- ---------------------------------------------          

	UPDATE 
		#Tran 
	SET 
		Reimbursement_Status = 'R'
	WHERE 
		Transaction_Status = 'R'

	-- ---------------------------------------------          
	-- Patch the Reimbursement_Status          
	-- In this patching, payment outside claim would not handle
	-- ---------------------------------------------          
          
	UPDATE          
		#Tran          
	SET          
		Reimbursement_Status =           
			CASE RAT.Authorised_Status
				WHEN 'R' THEN 'G'          
				ELSE RAT.Authorised_Status          
			END          
	FROM          
		#Tran VT          
			INNER JOIN ReimbursementAuthTran RAT          
				ON VT.Transaction_ID = RAT.Transaction_ID 
	 WHERE VT.Transaction_Status = 'A'         	
                    
	-- ---------------------------------------------          
	-- Patch the Transaction_Status          
	-- ---------------------------------------------          
          
	UPDATE          
		#Tran          
	SET          
		Transaction_Status = 'R'          
	WHERE          
		Reimbursement_Status = 'G'                    
          
	-- ---------------------------------------------          
	-- Validated accounts          
	-- ---------------------------------------------          
          
	INSERT INTO #Account (                
		Transaction_ID,          
		Transaction_Dtm,  
		SP_ID, 	
		Service_Receive_Dtm,          
		Vaccine,        
		Dose,          
		DOB,          
		Exact_DOB,          
		Sex,          
		Doc_Code,    
		HKIC_Symbol,
		Transaction_Status,          
		Reimbursement_Status
	)          
	SELECT              
		VT.Transaction_ID,          
		VT.Transaction_Dtm,      
		VT.SP_ID,     		
		VT.Service_Receive_Dtm,                  
		VT.Vaccine,          
		VT.Dose,          
		PInfo.DOB,          
		PInfo.Exact_DOB,          
		PInfo.Sex,          
		VT.Doc_Code,    
		VT.HKIC_Symbol,
		VT.Transaction_Status,          
		VT.Reimbursement_Status       
	FROM          
		#Tran VT          
			INNER JOIN PersonalInformation PInfo          
				ON VT.Voucher_Acc_ID = PInfo.Voucher_Acc_ID          
					AND VT.Doc_Code = PInfo.Doc_Code          
	WHERE          
		VT.Voucher_Acc_ID <> ''          
          
	-- ---------------------------------------------          
	-- Temporary accounts          
	-- ---------------------------------------------          
          
	INSERT INTO #Account (                
		Transaction_ID,          
		Transaction_Dtm,  
		SP_ID, 	
		Service_Receive_Dtm,          
		Vaccine,        
		Dose,          
		DOB,          
		Exact_DOB,          
		Sex,          
		Doc_Code,  
		HKIC_Symbol,		
		Transaction_Status,          
		Reimbursement_Status
	)                 
	SELECT            
		VT.Transaction_ID,          
		VT.Transaction_Dtm, 
		VT.SP_ID,     		
		VT.Service_Receive_Dtm,          
		VT.Vaccine,
		VT.Dose,          
		TPInfo.DOB,          
		TPInfo.Exact_DOB,          
		TPInfo.Sex,          
		VT.Doc_Code,  
		VT.HKIC_Symbol,
		VT.Transaction_Status,          
		VT.Reimbursement_Status
	FROM          
		#Tran VT          
			INNER JOIN TempPersonalInformation TPInfo          
				ON VT.Temp_Voucher_Acc_ID = TPInfo.Voucher_Acc_ID          
	WHERE          
		VT.Voucher_Acc_ID = ''          
		AND VT.Temp_Voucher_Acc_ID <> ''          
		AND VT.Special_Acc_ID = ''          
                 
	-- ---------------------------------------------          
	-- Special accounts          
	-- ---------------------------------------------          
          
	INSERT INTO #Account (                
		Transaction_ID,          
		Transaction_Dtm,  
		SP_ID, 	
		Service_Receive_Dtm,          
		Vaccine,        
		Dose,          
		DOB,          
		Exact_DOB,          
		Sex,          
		Doc_Code,   
		HKIC_Symbol,	
		Transaction_Status,          
		Reimbursement_Status
	)          
	SELECT          
		VT.SP_ID,          
		VT.Transaction_ID,          
		VT.Transaction_Dtm,          
		VT.Service_Receive_Dtm,              
		VT.Vaccine,
		VT.Dose,          
		SPInfo.DOB,          
		SPInfo.Exact_DOB,          
		SPInfo.Sex,          
		VT.Doc_Code,  
		VT.HKIC_Symbol,
		VT.Transaction_Status,          
		VT.Reimbursement_Status
	FROM          
		#Tran VT          
			INNER JOIN SpecialPersonalInformation SPInfo          
				ON VT.Special_Acc_ID = SPInfo.Special_Acc_ID          
	WHERE          
		VT.Voucher_Acc_ID = ''          
		AND VT.Special_Acc_ID <> ''          
		AND VT.Invalid_Acc_ID = ''          
                
	-- =============================================          
	-- Process data          
	-- =============================================          

	-- ---------------	
	-- Content		
	-- ---------------	
	SET @GenerationDtm= CONVERT(VARCHAR(11), GETDATE(), 111) + ' ' + CONVERT(VARCHAR(8), GETDATE(), 108)    
	SET @GenerationDtm = LEFT(@GenerationDtm, LEN(@GenerationDtm)-3)  
	
	-- ---------------	
	-- Report_01
	-- ---------------	
	INSERT #Report_01(Display_Seq,Value1,Value2,Value3,Value4)
	VALUES
		(1, 'Reporting period: as at ' + CONVERT(VARCHAR(10), DATEADD(dd, -1, @Report_Dtm), 111),'','',''),
		(2, '','','',''),
		(3, '(i) Measles Vaccination','','',''),
		(4, '','','',''),
		(5, '','Total','','No. of SP involved')

	INSERT #Report_01(Display_Seq,Value1,Value2,Value3,Value4)
	SELECT
		6,
		'Measles',
		(SELECT COUNT(1) AS [Total] FROM #Tran),
		'',
		(SELECT COUNT(1) FROM (SELECT SP_ID FROM #Tran GROUP BY SP_ID) SP)

	INSERT #Report_01(Display_Seq,Value1,Value2,Value3,Value4,Value5,Value6,Value7,Value8,Value9,Value10,Value11,Value12,Value13,Value14,Value15,Value16)
	VALUES
		(7, '','','','','','','','','','','','','','','',''),
		(8, '','','','','','','','','','','','','','','',''),
		(9, '(ii) By Category 1','','','','','','','','','','','','','','',''),
		(10,'','','Local Born','','','Non-Local Born','','','Not Provided','','','Total','','','',''),
		(11,'','1st dose','2nd dose','3rd dose','1st dose','2nd dose','3rd dose','1st dose','2nd dose','3rd dose','1st dose','2nd dose','3rd dose','','','')

--N	EthnicityInChinese	LocalBorn	PostSecondaryStudent
--Y	EthnicityInThai	NotProvided	HavingFrequentContactWithTourists
--N	EthnicityInFilipino	NonLocalBorn	ForeignDomesticHelper
--N	EthnicityInIndonesian	LocalBorn	PostSecondaryStudent
--N	EthnicityInNepalese	NonLocalBorn	HavingFrequentContactWithTourists
--N	EthnicityInPakistani	NonLocalBorn	HavingFrequentContactWithTourists
--N	Others	NotProvided	NotProvided
--N	NotProvided	NotProvided	PostSecondaryStudent

	INSERT #Report_01(Display_Seq,Value1,Value2,Value3,Value4,Value5,Value6,Value7,Value8,Value9,Value10,Value11,Value12,Value13,Value14,Value15,Value16)
	SELECT
		12,
		'Measles',
		(SELECT COUNT(1) AS [Total] FROM #Tran WHERE Category1 = 'LocalBorn' AND Dose = '1STDOSE'),
		(SELECT COUNT(1) AS [Total] FROM #Tran WHERE Category1 = 'LocalBorn' AND Dose = '2NDDOSE'),
		(SELECT COUNT(1) AS [Total] FROM #Tran WHERE Category1 = 'LocalBorn' AND Dose = '3RDDOSE'),
		(SELECT COUNT(1) AS [Total] FROM #Tran WHERE Category1 = 'NonLocalBorn' AND Dose = '1STDOSE'),
		(SELECT COUNT(1) AS [Total] FROM #Tran WHERE Category1 = 'NonLocalBorn' AND Dose = '2NDDOSE'),
		(SELECT COUNT(1) AS [Total] FROM #Tran WHERE Category1 = 'NonLocalBorn' AND Dose = '3RDDOSE'),
		(SELECT COUNT(1) AS [Total] FROM #Tran WHERE Category1 = 'NotProvided' AND Dose = '1STDOSE'),
		(SELECT COUNT(1) AS [Total] FROM #Tran WHERE Category1 = 'NotProvided' AND Dose = '2NDDOSE'),
		(SELECT COUNT(1) AS [Total] FROM #Tran WHERE Category1 = 'NotProvided' AND Dose = '3RDDOSE'),
		'','','','','',''

	UPDATE #Report_01 SET Value11 = CONVERT(VARCHAR(200), CAST(Value2 AS INT) + CAST(Value5 AS INT) + CAST(Value8 AS INT))
	WHERE Display_Seq = 12
	UPDATE #Report_01 SET Value12 = CONVERT(VARCHAR(200), CAST(Value3 AS INT) + CAST(Value6 AS INT) + CAST(Value9 AS INT))
	WHERE Display_Seq = 12
	UPDATE #Report_01 SET Value13 = CONVERT(VARCHAR(200), CAST(Value4 AS INT) + CAST(Value7 AS INT) + CAST(Value10 AS INT))
	WHERE Display_Seq = 12

	INSERT #Report_01(Display_Seq,Value1,Value2,Value3,Value4,Value5,Value6,Value7,Value8,Value9,Value10,Value11,Value12,Value13,Value14,Value15,Value16)
	VALUES
		(13,'','','','','','','','','','','','','','','',''),
		(14,'','','','','','','','','','','','','','','',''),
		(15, '(iii) By Category 2','','','','','','','','','','','','','','',''),
		(16,'','','Post-secondary Student','','','Foreign Domestic Helpers','','','Frequent Contact with Tourists','','','Not Provided','','','Total',''),
		(17,'','1st dose','2nd dose','3rd dose','1st dose','2nd dose','3rd dose','1st dose','2nd dose','3rd dose','1st dose','2nd dose','3rd dose','1st dose','2nd dose','3rd dose')

	INSERT #Report_01(Display_Seq,Value1,Value2,Value3,Value4,Value5,Value6,Value7,Value8,Value9,Value10,Value11,Value12,Value13,Value14,Value15,Value16)
	SELECT
		18,
		'Measles',
		(SELECT COUNT(1) AS [Total] FROM #Tran WHERE Category2 = 'PostSecondaryStudent' AND Dose = '1STDOSE'),
		(SELECT COUNT(1) AS [Total] FROM #Tran WHERE Category2 = 'PostSecondaryStudent' AND Dose = '2NDDOSE'),
		(SELECT COUNT(1) AS [Total] FROM #Tran WHERE Category2 = 'PostSecondaryStudent' AND Dose = '3RDDOSE'),
		(SELECT COUNT(1) AS [Total] FROM #Tran WHERE Category2 = 'ForeignDomesticHelper' AND Dose = '1STDOSE'),
		(SELECT COUNT(1) AS [Total] FROM #Tran WHERE Category2 = 'ForeignDomesticHelper' AND Dose = '2NDDOSE'),
		(SELECT COUNT(1) AS [Total] FROM #Tran WHERE Category2 = 'ForeignDomesticHelper' AND Dose = '3RDDOSE'),
		(SELECT COUNT(1) AS [Total] FROM #Tran WHERE Category2 = 'HavingFrequentContactWithTourists' AND Dose = '1STDOSE'),
		(SELECT COUNT(1) AS [Total] FROM #Tran WHERE Category2 = 'HavingFrequentContactWithTourists' AND Dose = '2NDDOSE'),
		(SELECT COUNT(1) AS [Total] FROM #Tran WHERE Category2 = 'HavingFrequentContactWithTourists' AND Dose = '3RDDOSE'),
		(SELECT COUNT(1) AS [Total] FROM #Tran WHERE Category2 = 'NotProvided' AND Dose = '1STDOSE'),
		(SELECT COUNT(1) AS [Total] FROM #Tran WHERE Category2 = 'NotProvided' AND Dose = '2NDDOSE'),
		(SELECT COUNT(1) AS [Total] FROM #Tran WHERE Category2 = 'NotProvided' AND Dose = '3RDDOSE'),
		'','',''

	UPDATE #Report_01 SET Value14 = CONVERT(VARCHAR(200), CAST(Value2 AS INT) + CAST(Value5 AS INT) + CAST(Value8 AS INT) + CAST(Value11 AS INT))
	WHERE Display_Seq = 18
	UPDATE #Report_01 SET Value15 = CONVERT(VARCHAR(200), CAST(Value3 AS INT) + CAST(Value6 AS INT) + CAST(Value9 AS INT) + CAST(Value12 AS INT))
	WHERE Display_Seq = 18
	UPDATE #Report_01 SET Value16 = CONVERT(VARCHAR(200), CAST(Value4 AS INT) + CAST(Value7 AS INT) + CAST(Value10 AS INT) + CAST(Value13 AS INT))
	WHERE Display_Seq = 18

	-- ---------------	
	-- Report_02
	-- ---------------	
	INSERT #Report_02(Display_Seq,Value1,Value2,Value3,Value4)
	VALUES
		(1, 'Reporting period: the week ending ' + CONVERT(VARCHAR(10), DATEADD(dd, -1, @Report_Dtm), 111),'','',''),
		(2, '','','','')

	INSERT #Report_02(Display_Seq,Value1,Value2,Value3,Value4,Value5,Value6,Value7,Value8,Value9,Value10,Value11,Value12,Value13,Value14,Value15,Value16,Value17)
	VALUES
		(3, 
		'Transaction ID','Transaction Time','SPID','Service Date',
		'Laboratory test result report','Ethnicity','Category 1','Category 2','Lot Number',
		'Subsidy','Dose','DOB','DOB Flag',
		'Gender','Doc Type','HKIC Symbol','Reimbursement Status'
		)

	INSERT #Report_02(Display_Seq,Value1,Value2,Value3,Value4,Value5,Value6,Value7,Value8,Value9,Value10,Value11,Value12,Value13,Value14,Value15,Value16,Value17)
	SELECT
		10,
		dbo.func_format_system_number(A.Transaction_ID),    
		CONVERT(varchar, A.Transaction_Dtm, 20),          
		A.SP_ID,          
		FORMAT(A.Service_Receive_Dtm, 'yyyy/MM/dd'),       
		[Lab_Test_Result] = 
			CASE
				WHEN [Lab_Test_Result] IS NULL THEN ''
				WHEN [Lab_Test_Result] = 'Y' THEN 'Yes'
				WHEN [Lab_Test_Result] = 'N' THEN 'No'
				ELSE 'N/A'
			END,
		[Ethnicity] = ISNULL(SR_Etheicity.[Description], ''),
		[Category1] = ISNULL(SR_Category1.[Description], ''),
		[Category2] = ISNULL(SR_Category2.[Description], ''),
		[Lot_Number]= ISNULL([Lot_Number], ''),
		A.Vaccine AS [Subsidy],
		[Dose] = CASE A.Dose          
			WHEN 'ONLYDOSE' THEN 'Only Dose'          
			ELSE SID.Available_Item_Desc          
		END,   
		FORMAT(A.DOB, 'yyyy/MM/dd'),              
		A.Exact_DOB,          
		A.Sex,          
		A.Doc_Code,
		ISNULL(A.HKIC_Symbol, 'N/A'),      
		ISNULL(SD.Status_Description, '')          
	FROM          
		#Account A 
			INNER JOIN #Tran T 
				ON A.Transaction_id = T.Transaction_ID AND A.Vaccine = T.Vaccine        
					AND T.Transaction_Dtm >= DATEADD(dd, -1 * @Date_Range + 1, @Report_Dtm) AND T.Transaction_Dtm < @Report_Dtm   
			INNER JOIN SubsidizeItemDetails SID          
				ON A.Dose = SID.Available_Item_Code             
					AND SID.Subsidize_Item_Code = T.Subsidize_Item_Code             
			LEFT JOIN StatusData SD          
				ON A.Reimbursement_Status = SD.Status_Value          
					AND SD.Enum_Class = 'ReimbursementStatus'
			LEFT JOIN SystemResource SR_Etheicity
				ON T.Ethnicity = SR_Etheicity.ObjectName AND SR_Etheicity.ObjectType = 'Text'
			LEFT JOIN SystemResource SR_Category1
				ON T.Category1 = SR_Category1.ObjectName AND SR_Category1.ObjectType = 'Text'
			LEFT JOIN SystemResource SR_Category2
				ON T.Category2 = SR_Category2.ObjectName AND SR_Category2.ObjectType = 'Text'
	ORDER BY		
		A.Transaction_Dtm

	-- ---------------	
	-- Remark
	-- ---------------	
	INSERT #Remark(Value1,Value2)
	VALUES
		('(A) Legend',''),
		('1. Identity Document Type',''),	
		('HKIC','Hong Kong Identity Card'),	
		('EC','Certificate of Exemption'),	
		('',''),	
		('2. Subsidy	',''),	
		('MMR','Measles'),	
		('',''),	
		('3. DOB Flag',''),	
		('D','Exact date DD/MM/YYYY'),	
		('M','MM/YYYY'),	
		('Y','Only year YYYY'),	
		('A','Exemption Certificate: Date of registration + age'),	
		('R','Exemption Certificate: Reported year of birth'),	
		('T','Exemption Certificate: Exact date DD/MM/YYYY on travel document'),	
		('','HKBC: Exact date DD/MM/YYYY for DOB in word'),	
		('U','Exemption Certificate: MM/YYYY on travel document'),	
		('','HKBC: MM/YYYY for DOB in word'),
		('V','Exemption Certificate: Only year YYYY on travel document'),
		('','HKBC: Only year YYYY for DOB in word'),
		('',''),
		('(B) Common Note(s) for the report	',''),
		('1. Transactions:',''),
		('   a. All claim transactions created under service providers (either created by back office users or service providers (or the delegated users))',''),
		('   b. Exclude those reimbursed transactions with invalidation status marked as Invalidated.',''),
		('   c. Exclude voided/deleted transactions.','')

	-- =============================================    
	-- Return results
	-- ============================================= 

	-- Content
	SELECT 'Report Generation Time: ' + @GenerationDtm  

	-- Report_01
	SELECT
		Value1,Value2,Value3,Value4,Value5,Value6,Value7,Value8,Value9,Value10,Value11,Value12,Value13,Value14,Value15,Value16
	FROM
		#Report_01
	ORDER BY
		Display_Seq

	-- Report_02
	SELECT
		Value1,Value2,Value3,Value4,Value5,Value6,Value7,Value8,Value9,Value10,Value11,Value12,Value13,Value14,Value15,Value16,Value17
	FROM
		#Report_02
	ORDER BY
		Display_Seq

	-- Remark
	SELECT
		Value1,Value2
	FROM
		#Remark
	ORDER BY
		Display_Seq

	DROP TABLE #StudentFileEntryBase
	DROP TABLE #Tran
	DROP TABLE #Account
	DROP TABLE #Report_01
	DROP TABLE #Report_02	
	DROP TABLE #Remark

END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSW0006_Report_get] TO HCVU
GO


