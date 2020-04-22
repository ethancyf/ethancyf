
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSU0007_Report_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSU0007_Report_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- ==============================================
-- Modification History
-- Modified by:		Winnie SUEN	
-- Modified date:	11 Sep 2019
-- CR No.			CRE19-006 (DHC)
-- Description:		1. Add Scheme [HCVSDHC]
--					2. Display column [Scheme], [DHC-related Services]
-- ===============================================
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	25 Aug 2017
-- CR No.:			I-CRP17-002
-- Description:		Fix the filter of service date and transaction date
-- =============================================
-- =============================================
-- Author:			Dickson Law
-- Create date:		01 Aug 2017
-- CR No.:			CRE16-013 (Voucher aberrant and new monitoring)
-- Description:		Get Report for eHSU0007
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_eHSU0007_Report_get]
	@request_time	datetime,
	@period_type	char(1),
	@From_Date		varchar(17),
	@To_Date		varchar(17)
AS BEGIN	


-- =============================================
-- Declaration
-- =============================================

	DECLARE @current_dtm		datetime
	DECLARE @seq				int
	DECLARE @RStatus			VARCHAR(100)
	DECLARE @DateTypeFN			char(30)
	DECLARE @From_Dtm			datetime
	DECLARE @To_Dtm				datetime
	DECLARE @DisplayFromDate	varchar(17)
	DECLARE @DisplayToDate		varchar(17)

	DECLARE @ServiceProviderStatus TABLE (
		Status_Value		char(3),
		Status_Description	varchar(100)
	)

	DECLARE @SchemeInfoStatus TABLE (
		Status_Value		char(3),
		Status_Description	varchar(100)
	)

	DECLARE @PracticeStatus TABLE (
		Status_Value		char(3),
		Status_Description	varchar(100)
	)

	DECLARE @FirstClaimStatus TABLE (
		RecordStatus_Value		char(3),
		Invalidation_Status_Value		VARCHAR(100),
		FirstClaim_Status_Description	VARCHAR(300)
	)

	-- Create First Claim Master list
	CREATE TABLE #FC (
		SP_ID					CHAR(8),
		Reimbursed_Date			DATETIME,
		Transaction_Date		DATETIME,
		Service_Date			DATETIME,
		Practice_Display_Seq	SMALLINT,
		Record_Status			CHAR(1),
		Invalidation			CHAR(1),
		DHC_Service				CHAR(1),
		Scheme_Code				CHAR(10)
	)
	
	-- Create First Claim Master list with detail
	CREATE TABLE #FC_MasterList (
		SP_ID					CHAR(8),
		SP_Name					VARCHAR(100),
		SP_Name_Chi				NVARCHAR(100),
		SP_Status				VARCHAR(100),
		FC_Reimbursed_Date		DATETIME,
		FC_Transaction_Date		DATETIME,
		FC_Service_Date			DATETIME,
		FC_DHC_Service			CHAR(10),
		Scheme_Code				CHAR(10),
		Scheme_Effective_Dtm	DATETIME,
		Scheme_Status			VARCHAR(100),
		Practice_Display_Seq	SMALLINT,
		Practice_Name			VARCHAR(200),
		Practice_Name_Chi		NVARCHAR(200),
		Practice_Address		VARCHAR(300),
		Practice_Address_Chi	NVARCHAR(300),
		Practice_district_name	CHAR(15),
		Practice_district_board	CHAR(15),
		Practice_area_name		CHAR(50),
		Practice_Status			VARCHAR(100),
		Professional_Code		CHAR(5),
		SP_Registration_Code	VARCHAR(15),
		Phone_Daytime			VARCHAR(20),
		Record_Status			VARCHAR(50)
	)
	
	-- Create Worksheet 02 Result Table
	CREATE TABLE #WS02 (
		Seq		INT,
		Col01	VARCHAR(1000),
		Col02	VARCHAR(1000)
	)

	-- Create Worksheet 03 Result Table
	CREATE TABLE #WS03 (
		Seq		INT,							
		Col01	VARCHAR(500),	
		Col02	VARCHAR(100),	
		Col03	NVARCHAR(100),	
		Col04	VARCHAR(100),	
		Col05	VARCHAR(50),	
		Col06	VARCHAR(50),	
		Col07	VARCHAR(50),		
		Col08	VARCHAR(50),		
		Col09	VARCHAR(50),	
		Col10	VARCHAR(100),	
		Col11	VARCHAR(20),	
		Col12	VARCHAR(200),	
		Col13	NVARCHAR(200),	
		Col14	VARCHAR(300),	
		Col15	NVARCHAR(300),	
		Col16	VARCHAR(50),	
		Col17	VARCHAR(50),	
		Col18	VARCHAR(50),	
		Col19	VARCHAR(100),	
		Col20	VARCHAR(50),	
		Col21	VARCHAR(50),	
		Col22	VARCHAR(50),	
		Col23	VARCHAR(50),
		
	)

	-- Create Worksheet 04 Result Table
	CREATE TABLE #WS04 (
		Seq		int,
		Col01	varchar(1000),
		Col02	varchar(1000)
	)

	-- Scheme
	DECLARE @Scheme AS TABLE(
		Scheme_Code		CHAR(10) 
	)

-- =============================================
-- Initialization
-- =============================================
   	IF @request_time IS NULL BEGIN    
	  SET @request_time = DATEADD(dd, -1, CONVERT(VARCHAR(11), GETDATE(), 106)) -- "106" gives "dd MMM yyyy"    
	END    

	SET @current_dtm = GETDATE()

	-- Scheme Code
	INSERT INTO @Scheme (Scheme_Code) VALUES 
	('HCVS'), ('HCVSDHC')
	
	--Filter Date (DATETIME)
	SELECT @From_Dtm = Cast(CONVERT(VARCHAR(11), @From_Date, 111) + ' 00:00:00' as datetime) 

	SELECT @To_Dtm = Cast(CONVERT(VARCHAR(11), DATEADD(D, +1, @To_Date)) + ' 00:00:00' as datetime) 

	--Display Date (VARCHAR)
	SELECT @DisplayFromDate = FORMAT(@From_Dtm, 'yyyy/MM/dd' , 'en-US')

	SELECT @DisplayToDate = FORMAT(DATEADD(D, -1, @To_Dtm), 'yyyy/MM/dd' , 'en-US')

	--Set Date type full name
	IF @period_type ='T'
	BEGIN 
		SET @DateTypeFN ='Transaction date'
	END

	IF @period_type ='R'
	BEGIN 
		SET @DateTypeFN ='Reimbursed date'
	END

	IF @period_type ='S'
	BEGIN 
		SET @DateTypeFN ='Service date'
	END

	--Record status table
	INSERT INTO 
		@ServiceProviderStatus (Status_Value, Status_Description)
	SELECT 
		Status_Value, 
		Status_Description
	FROM 
		StatusData WITH (NOLOCK)
	WHERE 
		Enum_Class = 'ServiceProviderStatus'

	INSERT INTO 
		@SchemeInfoStatus (Status_Value, Status_Description)
	VALUES
		('A','Active'),
		('D','Delisted'),
		('S','Suspended')

	INSERT INTO 
		@PracticeStatus (Status_Value, Status_Description)
	SELECT 
		Status_Value, 
		Status_Description
	FROM 
		StatusData WITH (NOLOCK)
	WHERE
		Enum_Class = 'PracticeStatus'

	SELECT 
		@RStatus = Status_Description 
	FROM 
		StatusData WITH (NOLOCK)
	WHERE 
		Enum_Class = 'ClaimTransStatus'
			AND Status_Value = 'R'

	INSERT INTO 
		@FirstClaimStatus (RecordStatus_Value, Invalidation_Status_Value,FirstClaim_Status_Description)
	SELECT 
		Status_Value,
		Status_Description,
	CASE 
		WHEN Status_Value ='I' THEN @RStatus + ' ('+Status_Description+')'
		WHEN Status_Value ='P' THEN @RStatus + ' ('+Status_Description+')'
		ELSE @RStatus 
	END 
		FROM StatusData WITH (NOLOCK)
	WHERE 
		Enum_Class = 'TransactionInvalidationStatus'

-- =============================================
-- Retrieve Data
-- =============================================

	--Get all SP first claim
	INSERT INTO #FC
	(
	SP_ID,
	Reimbursed_Date,
	Transaction_Date,
	Service_Date,
	Practice_Display_Seq,
	Record_Status,
	Invalidation,
	DHC_Service,
	Scheme_Code
	)
	SELECT 
		FC.SP_ID,
		FC.Reimbursed_Date,
		FC.Transaction_Date,
		FC.Service_Receive_Dtm,
		FC.Practice_Display_Seq,
		FC.Record_Status,
		FC.Invalidation,
		FC.DHC_Service,
		FC.Scheme_Code
	FROM (
			SELECT
				ROW_NUMBER() OVER(PARTITION BY SP_ID ORDER BY Authorised_Dtm,Transaction_Dtm ASC) AS claim_seq,
				VT.SP_ID,
				RA.Authorised_Dtm AS Reimbursed_Date,
				VT.Transaction_Dtm AS Transaction_Date,
				VT.Service_Receive_Dtm,
				VT.DHC_Service,
				VT.Practice_Display_Seq,
				VT.Record_Status,
				VT.Invalidation,  
				RAT.Transaction_ID,
				RAT.Reimburse_ID,
				RAT.Authorised_Status,
				RAT.Scheme_Code,
				RA.Authorised_Dtm 
			FROM 
				ReimbursementAuthTran RAT
			INNER JOIN 
			(
				SELECT 
					Authorised_Dtm,
					Authorised_Status,
					Reimburse_ID,
					Record_Status,
					Scheme_Code
				FROM 
					ReimbursementAuthorisation
				WHERE 
					Authorised_Status = 'R'
						AND Record_Status = 'A'
			) RA ON RAT.Reimburse_ID = RA.Reimburse_ID AND RAT.Scheme_Code = RA.Scheme_Code
			INNER JOIN 
			(
			SELECT 
				SP_ID,				
				Transaction_ID,
				Transaction_Dtm,				
				Service_Receive_Dtm,
				Scheme_Code,
				Practice_Display_Seq,
				Record_Status,
				ISNULL(Invalidation,'') AS Invalidation,
				ISNULL(DHC_Service,'N') AS [DHC_Service]
			FROM 
				VoucherTransaction
			WHERE 
				Scheme_Code IN (SELECT Scheme_Code FROM @Scheme) 
			AND Record_Status IN ('R','A')
			AND (manual_reimburse='N' or manual_reimburse is null)
			) VT ON RAT.Transaction_ID = VT.Transaction_ID
			AND (
				(@period_type = 'R' AND RA.Authorised_Dtm < @To_Dtm)
					OR
				(@period_type = 'S' AND VT.Service_Receive_Dtm < @To_Dtm)
					OR
				(@period_type = 'T' AND VT.Transaction_Dtm < @To_Dtm)
			)
		) FC 
	WHERE FC.claim_seq = 1 ORDER BY FC.SP_ID


	OPEN SYMMETRIC KEY sym_Key
	DECRYPTION BY ASYMMETRIC KEY asym_Key

	--Get all SP first claim with detail
	INSERT INTO #FC_MasterList
	(
		SP_ID,					
		SP_Name,				
		SP_Name_Chi,
		SP_Status,
		FC_Reimbursed_Date,
		FC_Transaction_Date,	
		FC_Service_Date,
		FC_DHC_Service,
		Scheme_Code,
		Scheme_Effective_Dtm,		
		Scheme_Status,			
		Practice_Display_Seq,	
		Practice_Name,			
		Practice_Name_Chi,		
		Practice_Address,		
		Practice_Address_Chi,	
		Practice_district_name,	
		Practice_district_board,	
		Practice_area_name,	
		Practice_Status,			
		Professional_Code,		
		SP_Registration_Code,	
		Phone_Daytime,			
		Record_Status					
	)
	SELECT 
		FCML.SP_ID,
		CONVERT(VARCHAR(MAX), DECRYPTBYKEY(SP.Encrypt_Field2)) AS [Account Name (In English)],
		CONVERT(NVARCHAR(MAX), DECRYPTBYKEY(SP.Encrypt_Field3)) AS [Account Name (In Chinese)],
		SPS.Status_Description,
		FCML.Reimbursed_Date,
		FCML.Transaction_Date,
		FCML.Service_Date,
		[DHC_Service] = CASE WHEN FCML.Scheme_Code = 'HCVS'
							 THEN FCML.DHC_Service
							 ELSE 'N/A' END,
		FCML.Scheme_Code,
		SInfo.Effective_Dtm,
		SIS.Status_Description,
		FCML.Practice_Display_Seq,
		P.Practice_Name,
		P.Practice_Name_Chi,
		[dbo].[func_formatEngAddress](P.[Room], P.[Floor], P.[Block], P.[Building], P.[District]) AS Practice_Address,
		[dbo].[func_format_Address_Chi](P.[Room], P.[Floor], P.[Block], P.[Building_Chi], P.[District]) AS  Practice_Address_Chi,
		P.district_name,
		P.district_board,
		P.area_name,
		PS.Status_Description,
		Pro.Service_Category_Code,
		Pro.Registration_Code,
		P.Phone_Daytime,
		FCS.FirstClaim_Status_Description
	FROM 
		#FC FCML
	INNER JOIN 
	(
		SELECT  
			SP_ID,
			Encrypt_Field2,
			Encrypt_Field3,
			Record_Status
		FROM 
			ServiceProvider
	) SP ON FCML.SP_ID = SP.SP_ID
	INNER JOIN
	(
		SELECT 
			SP_ID,
			Display_Seq,
			Practice_Name,
			Practice_Name_Chi,
			Room,
			Floor,
			Block,
			Building,
			Building_Chi,
			District,
			Phone_Daytime,
			Record_Status,
			Professional_Seq,
			district_name,
			district_board,
			area_name
		FROM 
			Practice 
		INNER JOIN 
		(
			SELECT
				district_code,
				district_name,	
				district_board,	
				district_area
			FROM 
				district 
		) D ON Practice.District = D.district_code
		INNER JOIN 
		(
			SELECT 
				area_code,
				area_name
			FROM 
				district_area 
		) DA ON d.district_area =DA.area_code
	) P ON FCML.SP_ID = P.SP_ID 
		AND FCML.Practice_Display_Seq = P.Display_Seq
	INNER JOIN 
	(
		SELECT 
			SP_ID,
			Professional_Seq,
			Service_Category_Code,
			Registration_Code
		FROM 
			Professional
	) Pro ON FCML.SP_ID = Pro.SP_ID 
		AND P.Professional_Seq = Pro.Professional_Seq 
	INNER JOIN 
	(
		SELECT 
			SP_ID,
			Scheme_Code,
			Record_Status,    
			Effective_Dtm
		FROM 
			SchemeInformation
	) SInfo ON FCML.SP_ID = SInfo.SP_ID AND FCML.Scheme_Code = SInfo.Scheme_Code
	INNER JOIN 
	(
		SELECT * FROM @ServiceProviderStatus
	) SPS ON SP.Record_Status = SPS.Status_Value
	INNER JOIN 
	(
		SELECT * FROM @SchemeInfoStatus
	) SIS ON SInfo.Record_Status = SIS.Status_Value
	INNER JOIN 
	(
		SELECT * FROM @PracticeStatus
	) PS ON P.Record_Status = PS.Status_Value
	INNER JOIN 
	(
		SELECT * FROM @FirstClaimStatus
	) FCS ON FCML.Invalidation = FCS.RecordStatus_Value
	WHERE ((@period_type = 'R' AND FCML.Reimbursed_Date >= @From_Dtm AND FCML.Reimbursed_Date < @To_Dtm) 
				OR 
				(@period_type = 'S' AND FCML.Service_Date >= @From_Dtm AND FCML.Service_Date < @To_Dtm)
				OR
				(@period_type = 'T' AND FCML.Transaction_Date >= @From_Dtm AND FCML.Transaction_Date < @To_Dtm)  
				)

	CLOSE SYMMETRIC KEY sym_Key

-- ---------------------------------------------
-- For Excel Sheet (02): Criteria
-- ---------------------------------------------

	SET @seq = 0

	INSERT INTO #WS02 (Seq, Col01, Col02)
	VALUES (@seq, 'Criteria', '')

	SET @seq = @seq + 1

	INSERT INTO #WS02 (Seq, Col01, Col02)
	VALUES (@seq, 'Type of Date', @DateTypeFN)
	
	SET @seq = @seq + 1

	INSERT INTO #WS02 (Seq, Col01, Col02)
	VALUES (@seq, 'Date', @DisplayFromDate +' to '+ @DisplayToDate)
						  	
-- ---------------------------------------------
-- For Excel Sheet (03): 01-Service Provider (Part 1)
-- ---------------------------------------------

	SET @seq = 0

	INSERT INTO #WS03 (Seq, Col01)
	VALUES (@seq,
	RTRIM(@DateTypeFN) + ' period: '+ @DisplayFromDate + ' to '+ @DisplayToDate)
	
	SET @seq = @seq + 1

	INSERT INTO #WS03 (Seq) VALUES (@seq)

	SET @seq = @seq + 1

	-- Check record exsit for generate 'Record Result' or generate 'No Record Result' 
	IF EXISTS
	(	
		SELECT TOP 1 * FROM #FC_MasterList
	)
	BEGIN
	-- Record Exsit
		-- Create Report Header
		INSERT INTO #WS03 (
			Seq,
			Col01, Col02, Col03, Col04, Col05,
			Col06, Col07, Col08, Col09, Col10,
			Col11, Col12, Col13, Col14, Col15, 
			Col16, Col17, Col18, Col19, Col20, 
			Col21, Col22, Col23)
		VALUES (
			@seq,
			'SPID', 'SP Name (In English)', 'SP Name (In Chinese)', 'SP Status', 'Transaction Time of the First Claim',
			'Service Date of the First Claim', 'Scheme', 'DHC-related Services', 'SP Scheme Enrolment Effective Date', 'SP Scheme Status', 
			'Practice No.', 'Practice Name (In English)','Practice Name (In Chinese)','Practice Address (In English)', 'Practice Address (In Chinese)',
			'District of Practice', 'District Board of Practice', 'Area of Practice','Practice Status',
			'Profession', 'Professional Registration No.', 'Phone No. of Practice', 'Status of the First Claim'		
		)
																	
		-- Create Report Result
		SET @seq = @seq + 1

		INSERT INTO #WS03 (
			Seq,
			Col01, Col02, Col03, Col04, Col05,
			Col06, Col07, Col08, Col09, Col10,
			Col11, Col12, Col13, Col14, Col15, 
			Col16, Col17, Col18, Col19, Col20, 
			Col21, Col22, Col23)
		SELECT 
			@seq,
			SP_ID,
			SP_Name,
			SP_Name_Chi,
			SP_Status,
			CONVERT(VARCHAR(20),FC_Transaction_Date,20),
			CONVERT(VARCHAR(10),FC_Service_Date,20),
			Scheme_Code,
			FC_DHC_Service,
			CONVERT(VARCHAR(10),Scheme_Effective_Dtm,20),			
			Scheme_Status,
			Practice_Display_Seq,
			Practice_Name,
			Practice_Name_Chi,
			Practice_Address,
			Practice_Address_Chi,
			Practice_district_name,
			Practice_district_board,
			Practice_area_name,
			Practice_Status,
			Professional_Code,
			SP_Registration_Code,
			Phone_Daytime,
			Record_Status 
		FROM #FC_MasterList
		ORDER BY SP_ID	

	END
	ELSE
	BEGIN
	-- Record Not Exsit
		-- Create Empty Row
		INSERT INTO #WS03 (
			Seq,
			Col01, Col02, Col03, Col04, Col05,
			Col06, Col07, Col08, Col09, Col10,
			Col11, Col12, Col13, Col14, Col15, 
			Col16, Col17, Col18, Col19, Col20, 
			Col21, Col22, Col23)
		VALUES (@seq, 
		'', '', '', '','',
		'', '', '', '','',
		'', '', '','', '', 
		'', '', '','', '', 
		'', '', '')    
		
		SET @seq = @seq + 1

		-- Create No Record Result
		INSERT INTO #WS03 (
			Seq,
			Col01, Col02, Col03, Col04, Col05,
			Col06, Col07, Col08, Col09, Col10,
			Col11, Col12, Col13, Col14, Col15, 
			Col16, Col17, Col18, Col19, Col20, 
			Col21, Col22, Col23)
		VALUES (@seq,
		'There is no record in the '+LOWER(RTRIM(@DateTypeFN))+' period', '', '', '', '',
		'', '', '', '','',
		'', '', '', '','',
		'', '', '','', '', 
		'', '', '')  
	END
-- ---------------------------------------------
-- For Excel Sheet (04): Remarks
-- ---------------------------------------------

	SET @seq = 0

	INSERT INTO #WS04 (Seq, Col01, Col02)
	VALUES (@seq, '(A) Common Note(s) for the report', '')

	SET @seq = @seq + 1

	INSERT INTO #WS04 (Seq, Col01, Col02)
	VALUES (@seq, '1. Transactions:', '')
	
	SET @seq = @seq + 1

	INSERT INTO #WS04 (Seq, Col01, Col02)
	VALUES (@seq, '   a. All claim transactions created by service providers (or the delegated users)', '')

	SET @seq = @seq + 1

	INSERT INTO #WS04 (Seq, Col01, Col02)
	VALUES (@seq, '   b. Include HCVS and HCVSDHC claim only.', '')
	
	SET @seq = @seq + 1

	INSERT INTO #WS04 (Seq, Col01, Col02)
	VALUES (@seq, '   c. Include reimbursed transactions only even with invalidation status marked as Invalidated.', '')


	SET @seq = @seq + 1

	INSERT INTO #WS04 (Seq, Col01, Col02)
	VALUES (@seq, '   d. Exclude claim transactions created by back office users.', '')


	SET @seq = @seq + 1

	INSERT INTO #WS04 (Seq, Col01, Col02)
	VALUES (@seq, '   e. The first claim would be identified according to the date of reimbursement.', '')


	SET @seq = @seq + 1

	INSERT INTO #WS04 (Seq, Col01, Col02)
	VALUES (@seq, '   f. If there are multiple transactions on the same date of reimbursement, transaction time and date would be used to determine which transaction is the first claim.', '')



	DROP TABLE #FC
	DROP TABLE #FC_MasterList
-- =============================================
-- Return results
-- =============================================

-- ---------------------------------------------
-- To Excel Sheet (01): Content
-- ---------------------------------------------

	SELECT 'Report Generation Time: ' + CONVERT(varchar(10), @current_dtm, 111) + ' ' + CONVERT(varchar(5), @current_dtm, 114)
	
-- ---------------------------------------------
-- To Excel Sheet (02): Criteria
-- ---------------------------------------------

	SELECT Col01, RTRIM(Col02) FROM #WS02 ORDER BY Seq

	DROP TABLE #WS02

-- ---------------------------------------------
-- To Excel Sheet (03): 01-Service Provider
-- ---------------------------------------------

	SELECT 	Col01, Col02, Col03, Col04, Col05,
			Col06, Col07, Col08, Col09, Col10,
			Col11, Col12, Col13, Col14, Col15, 
			Col16, Col17, Col18, Col19, Col20, 
			Col21, Col22, Col23
	FROM 
		#WS03 
	ORDER BY 
		Seq , Col01

	DROP TABLE #WS03

-- ---------------------------------------------
-- To Excel Sheet (04): Remarks
-- ---------------------------------------------

	SELECT Col01, Col02 FROM #WS04 ORDER BY Seq, Col01

	DROP TABLE #WS04

END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSU0007_Report_get] TO HCVU
GO
