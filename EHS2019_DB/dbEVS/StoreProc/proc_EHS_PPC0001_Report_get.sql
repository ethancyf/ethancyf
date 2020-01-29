IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_PPC0001_Report_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_PPC0001_Report_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.			
-- Description:		
-- =============================================
-- =============================================
-- Author:			Chris YIM
-- Create date:		27 May 2016
-- CR No.			CRE15-016
-- Description:		Randomly generate the valid claim transaction - PPC0001
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_PPC0001_Report_get]
	@request_time DATETIME,
	@NoOfServiceProvider INT,
	@ProfessionList VARCHAR(5000),
	@Scheme VARCHAR(5000),
	@Period_Type CHAR(1),
	@Period_Format CHAR(1),
	@Period_From DATETIME,
	@Period_To DATETIME,
	@Transaction_Status VARCHAR(5000),
	@IncludeBackOfficeClaims BIT,
	@IncludeInvalidationClaims BIT
AS
BEGIN
	SET NOCOUNT ON;
	-- ===========================================================
	-- 1.0	DECLARATION
	-- ===========================================================
	DECLARE @IN_intNumberOfSPs INT
	DECLARE @IN_varProfessional VARCHAR(5000)
	DECLARE @IN_varSchemeCode VARCHAR(5000)
	DECLARE @IN_chrPeriod_Type CHAR(1)
	DECLARE @IN_dtmPeriod_From DATETIME
	DECLARE @IN_dtmPeriod_To DATETIME
	DECLARE @IN_chrPeriod_Format CHAR(1)
	DECLARE @IN_varRecordStatus VARCHAR(5000)
	DECLARE @IN_chrIncludeBackOfficeClaim BIT
	DECLARE @IN_chrIncludeInvalidatedClaim BIT

	DECLARE @IN_dtmTransactionDateStart DATETIME
	DECLARE @IN_dtmTransactionDateEnd DATETIME
	DECLARE @IN_dtmServiceDateStart DATETIME
	DECLARE @IN_dtmServiceDateEnd DATETIME

	DECLARE @chrIsAllProfessional CHAR(1)
	DECLARE @chrIsAllRecordStatus CHAR(1)
	DECLARE @chrIsAllSchemeCode CHAR(1)

	DECLARE @current_dtm DATETIME
	DECLARE @delimiter varchar(5)

	DECLARE @seq int

	DECLARE @tblProfessional TABLE(
		Service_Category_Code VARCHAR(5)
	)

	DECLARE @tblRecordStatus TABLE(
		Record_Status CHAR(1)
	)

	DECLARE @tblSchemeCode TABLE(
		Scheme_Code CHAR(10)
	)

	CREATE TABLE #ServiceProviderBase (
		SP_ID CHAR(8) NOT NULL
	)

	--CREATE NONCLUSTERED INDEX IX_ServiceProviderBase_SP_ID
	--	ON #ServiceProviderBase (SP_ID); 

	CREATE TABLE #VoucherTransactionBase (
		SP_ID CHAR(8),
		Transaction_ID CHAR(20),
		Practice_Display_Seq SMALLINT,
	)

	CREATE NONCLUSTERED INDEX IX_VoucherTransactionBase_SP_ID
		ON #VoucherTransactionBase (SP_ID); 

	CREATE TABLE #ResultTable    
	(    
		DisplaySeq INT IDENTITY(1,1),     
		Col01 VARCHAR(255) DEFAULT '',    
		Col02 VARCHAR(255) DEFAULT '',    
		Col03 VARCHAR(255) DEFAULT '',    
		Col04 VARCHAR(255) DEFAULT '',    
		Col05 VARCHAR(255) DEFAULT '',    
		Col06 VARCHAR(255) DEFAULT '',    
		Col07 VARCHAR(255) DEFAULT '',    
		Col08 VARCHAR(255) DEFAULT '',    
		Col09 VARCHAR(255) DEFAULT '',    
		Col10 VARCHAR(255) DEFAULT ''
	)  

	CREATE NONCLUSTERED INDEX IX_ResultTable_Col01
		ON #ResultTable (Col01); 

	CREATE NONCLUSTERED INDEX IX_ResultTable_Col02
		ON #ResultTable (Col02); 

	-- ===========================================================
	-- 1.1	Initialization
	-- ===========================================================
	SET @current_dtm = GETDATE()
	SET @delimiter = ','

	-- ===========================================================
	-- 1.2	COPY PARAMETERS
	-- ===========================================================
	SET @IN_intNumberOfSPs = @NoOfServiceProvider

	SET @IN_chrPeriod_Type = @Period_Type
	SET @IN_dtmPeriod_From = @Period_From
	SET @IN_dtmPeriod_To = @Period_To
	SET @IN_chrPeriod_Format = @Period_Format

	SET @IN_chrIncludeBackOfficeClaim = @IncludeBackOfficeClaims
	SET @IN_chrIncludeInvalidatedClaim = @IncludeInvalidationClaims

	SET @IN_dtmTransactionDateStart = NULL
	SET @IN_dtmTransactionDateEnd = NULL
	SET @IN_dtmServiceDateStart = NULL
	SET @IN_dtmServiceDateEnd = NULL

	SET @IN_varProfessional = NULL
	SET @IN_varSchemeCode = NULL
	SET @IN_varRecordStatus = NULL

	IF @ProfessionList IS NOT NULL
	BEGIN
		IF @ProfessionList = ''
			SET @IN_varProfessional = NULL
		ELSE 
			SET @IN_varProfessional = @ProfessionList
	END

	IF @Scheme IS NOT NULL
	BEGIN
		IF @Scheme = ''
			SET @IN_varSchemeCode = NULL
		ELSE 
			SET @IN_varSchemeCode = @Scheme
	END

	IF @Transaction_Status IS NOT NULL
	BEGIN
		IF @Transaction_Status = ''
			SET @IN_varRecordStatus = NULL
		ELSE 
			SET @IN_varRecordStatus = @Transaction_Status
	END

	-- Selected 'Service Date'
	IF @IN_chrPeriod_Type = 'S'
		BEGIN
			SET @IN_dtmServiceDateStart = @IN_dtmPeriod_From
			SET @IN_dtmServiceDateEnd = @IN_dtmPeriod_To
		END

	-- Selected 'Transaction Date'
	ELSE IF @IN_chrPeriod_Type = 'T'
		BEGIN
			SET @IN_dtmTransactionDateStart = @IN_dtmPeriod_From
			SET @IN_dtmTransactionDateEnd = DATEADD(DAY, 1, @IN_dtmPeriod_To)
		END

	-- ===========================================================
	-- 2.1 PREPARE TABLE TO STORE INPUTTED PROFESSIONAL
	-- ===========================================================

	IF @IN_varProfessional IS NOT NULL
	BEGIN

		INSERT @tblProfessional
				SELECT * FROM func_split_string(@IN_varProfessional, @delimiter)

		SET @chrIsAllProfessional = 'N'
	END
	ELSE 
		SET @chrIsAllProfessional = 'Y'

	-- ===========================================================
	-- 2.2 PREPARE TABLE TO STORE INPUTTED RECORD STATUS
	-- ===========================================================
	IF @IN_varRecordStatus IS NOT NULL
	BEGIN

		INSERT @tblRecordStatus
				SELECT * FROM func_split_string(@IN_varRecordStatus, @delimiter)

		SET @chrIsAllRecordStatus = 'N'
	END
	ELSE 
		SET @chrIsAllRecordStatus = 'Y'

	-- ===========================================================
	-- 2.3 PREPARE TABLE TO STORE INPUTTED SCHEME CODE
	-- ===========================================================
	IF @IN_varSchemeCode IS NOT NULL
	BEGIN

		INSERT @tblSchemeCode
				SELECT * FROM func_split_string(@IN_varSchemeCode, @delimiter)

		SET @chrIsAllSchemeCode = 'N'
	END
	ELSE 
		SET @chrIsAllSchemeCode = 'Y'

	-- =========================================================================
	-- 3.1 PREPARE TABLE TO RETRIEVE POSSIBLE SERVICE PROVIDER WITH TRANSACTIONS
	-- =========================================================================
	INSERT
		#VoucherTransactionBase
		(SP_ID,
		Transaction_ID,
		Practice_Display_Seq)
	SELECT
		VT.SP_ID,
		VT.Transaction_ID,
		VT.Practice_Display_Seq
	FROM 
		VoucherTransaction VT
			LEFT JOIN @tblProfessional tblProf
				ON LTRIM(RTRIM(VT.Service_Type)) = tblProf.Service_Category_Code
			LEFT JOIN @tblRecordStatus tblRS
				ON LTRIM(RTRIM(VT.Record_Status)) = tblRS.Record_Status
			LEFT JOIN @tblSchemeCode tblSC
				ON LTRIM(RTRIM(VT.Scheme_Code)) = LTRIM(RTRIM(tblSC.Scheme_Code))
	WHERE 
		((@IN_dtmTransactionDateStart IS NULL AND @IN_dtmTransactionDateEnd IS NULL)
			OR (Transaction_Dtm >= @IN_dtmTransactionDateStart AND Transaction_Dtm < @IN_dtmTransactionDateEnd))
		AND ((@IN_dtmServiceDateStart IS NULL AND @IN_dtmServiceDateEnd IS NULL)
			OR (Service_Receive_Dtm >= @IN_dtmServiceDateStart AND Service_Receive_Dtm <= @IN_dtmServiceDateEnd))
		AND (@IN_chrIncludeBackOfficeClaim IS NULL
			OR @IN_chrIncludeBackOfficeClaim = 1
			OR (@IN_chrIncludeBackOfficeClaim = 0 AND (VT.Manual_Reimburse IS NULL OR VT.Manual_Reimburse <> 'Y')))
		AND (@IN_chrIncludeInvalidatedClaim IS NULL 
			OR @IN_chrIncludeInvalidatedClaim = 1
			OR (@IN_chrIncludeInvalidatedClaim = 0 AND (Invalidation IS NULL OR Invalidation NOT IN ('I','P'))))
		AND (@chrIsAllProfessional = 'Y' OR tblProf.Service_Category_Code IS NOT NULL)
		AND (@chrIsAllRecordStatus = 'Y' OR tblRS.Record_Status IS NOT NULL)
		AND (@chrIsAllSchemeCode = 'Y' OR tblSC.Scheme_Code IS NOT NULL)

	--===========================================================
	--3.2. RANDOM SELECTION OF SERVICE PROVIDER
	--===========================================================

	IF @IN_intNumberOfSPs IS NOT NULL
	BEGIN
		INSERT #ServiceProviderBase(SP_ID)
			SELECT TOP (@IN_intNumberOfSPs) SP_ID FROM (SELECT DISTINCT SP_ID FROM #VoucherTransactionBase) AS SP
			ORDER BY NEWID()	

	END

	-- ===========================================================
	-- 4. RETRIEVE SERVICE PROVIDER
	-- ===========================================================

	INSERT #ResultTable(Col01)
			SELECT SP_ID FROM #ServiceProviderBase

	-- =============================================
	-- 5. RETURN RESULTS
	-- =============================================
	-- ---------------------------------------------
	-- To Excel Sheet (01): Content
	-- ---------------------------------------------

		SELECT 'Report Generation Time: ' + CONVERT(varchar(10), @current_dtm, 111) + ' ' + CONVERT(varchar(5), @current_dtm, 114)

	-- ---------------------------------------------
	-- To Excel Sheet (02): Criteria
	-- ---------------------------------------------
		DECLARE @varCriteriaNoOfSP varchar(10)
		DECLARE @varProfessionList varchar(5000)
		DECLARE @varScheme varchar(5000)
		DECLARE @chrPeriod_Type varchar(50)
		DECLARE @chrPeriod_Format varchar(50)
		DECLARE @varPeriod varchar(50)
		DECLARE @varTransactionStatus varchar(5000)
		DECLARE @chrIncludeBackOfficeClaims varchar(3)
		DECLARE @chrIncludeInvalidationClaims varchar(3)

		-- Create Temp Result Table

		CREATE TABLE #Criteria (
			Seq		int,
			Seq2	int,
			Col01	varchar(1000),
			Col02	varchar(1000)
		)

		-- ---------------------------------------------
		-- Prepare Data: Criteria
		-- ---------------------------------------------

		Set @varCriteriaNoOfSP = @NoOfServiceProvider

		IF @chrIsAllProfessional = 'Y'
			SET @varProfessionList = 'Any'
		ELSE
			BEGIN
				SET @varProfessionList = 
					(SELECT 
						[Service_Category_Desc] + ' (' + [Service_Category_Code] + ')' + ', ' 
					FROM Profession WITH (NOLOCK)
					WHERE [Service_Category_Code] IN 
						(SELECT [Service_Category_Code] FROM @tblProfessional)
					for xml path(''))

				SET @varProfessionList = SUBSTRING(@varProfessionList, 1, LEN(@varProfessionList)- 1) 
			END

		IF @chrIsAllSchemeCode = 'Y'
			SET @varScheme = 'Any'
		ELSE
			BEGIN
				SET @varScheme = 
					(SELECT LTRIM(RTRIM(Scheme_Code)) + ', ' FROM @tblSchemeCode	for xml path(''))
				SET @varScheme = SUBSTRING(@varScheme, 1, LEN(@varScheme)- 1) 
			END

		IF @Period_Type = 'S'
			SET @chrPeriod_Type = 'Service Date'
		ELSE IF @Period_Type = 'T'
			SET @chrPeriod_Type = 'Transaction Date'

		IF @Period_Format = 'E'
			BEGIN
				SET @chrPeriod_Format = 'Exact Date'
				SET @varPeriod = FORMAT(@Period_From, 'dd-MM-yyyy') + ' to ' + FORMAT(@Period_To, 'dd-MM-yyyy')
			END
		ELSE IF @Period_Format = 'M'
			BEGIN
				SET @chrPeriod_Format = 'Month and Year'
				SET @varPeriod = FORMAT(@Period_From, 'MMMM, yyyy') + ' to ' + FORMAT(@Period_To, 'MMMM, yyyy')
			END

		IF @chrIsAllRecordStatus= 'Y'
			SET @varTransactionStatus = 'Any'
		ELSE
			BEGIN
				SET @varTransactionStatus = 
					(SELECT 
						[Status_Description] + ', ' 
					FROM StatusData WITH (NOLOCK)
					WHERE [Status_Value] IN 
						(SELECT [Record_Status] FROM @tblRecordStatus)
						AND Enum_Class = 'HCVUClaimTransManagementStatus'
					ORDER BY Display_Order
					for xml path(''))

				SET @varTransactionStatus = SUBSTRING(@varTransactionStatus, 1, LEN(@varTransactionStatus)- 1) 
			END

		IF @IncludeBackOfficeClaims = 1
			SET @chrIncludeBackOfficeClaims = 'Yes'
		ELSE
			SET @chrIncludeBackOfficeClaims = 'No'

		IF @IncludeInvalidationClaims = 1
			SET @chrIncludeInvalidationClaims = 'Yes'
		ELSE
			SET @chrIncludeInvalidationClaims = 'No'

		-- ---------------------------------------------
		-- Insert Data: Criteria
		-- ---------------------------------------------

		SET @seq = 0

		INSERT INTO #Criteria (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'Criteria', '')

		SET @seq = @seq + 1

		INSERT INTO #Criteria (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'Number of Service Provider', @varCriteriaNoOfSP)

		SET @seq = @seq + 1

		INSERT INTO #Criteria (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'Health Profession', @varProfessionList)

		SET @seq = @seq + 1

		INSERT INTO #Criteria (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'Scheme', @varScheme)

		SET @seq = @seq + 1

		INSERT INTO #Criteria (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'Type of Date', @chrPeriod_Type)

		SET @seq = @seq + 1

		INSERT INTO #Criteria (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'Format of Date', @chrPeriod_Format)

		SET @seq = @seq + 1

		INSERT INTO #Criteria (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'Date', @varPeriod)

		SET @seq = @seq + 1

		INSERT INTO #Criteria (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'Transaction Status', @varTransactionStatus)

		SET @seq = @seq + 1

		INSERT INTO #Criteria (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'Include Back Office Claim', @chrIncludeBackOfficeClaims)

		SET @seq = @seq + 1

		INSERT INTO #Criteria (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'Include Pending Invalidation and Invalidated Claims', @chrIncludeInvalidationClaims)

		SET @seq = @seq + 1

		-- ---------------------------------------------
		-- Return Result: Criteria
		-- ---------------------------------------------

		SELECT Col01, Col02 FROM #Criteria ORDER BY Seq, Seq2, Col01

	-- ---------------------------------------------
	-- To Excel Sheet (03): 01-Service Provider
	-- ---------------------------------------------

		OPEN SYMMETRIC KEY sym_Key
		DECRYPTION BY ASYMMETRIC KEY asym_Key

		SELECT
			'Order of Selection' = RT.DisplaySeq,
			'SPID' = RT.Col01,
			'SP Name (In English)' = CONVERT(varchar(MAX), DecryptByKey(SP.Encrypt_Field2)),
			'SP Name (In Chinese)' = CONVERT(nvarchar(MAX), DecryptByKey(SP.Encrypt_Field3)),
			'MO No.' = MO.Display_Seq,
			'MO Name (In English)' = MO.MO_Eng_Name,
			'MO Name (In Chinese)' = MO.MO_Chi_Name,
			'Practice No.' = P.Display_Seq,
			'Practice Name (In English)' = P.Practice_Name,
			'Practice Name (In Chinese)' = P.Practice_Name_Chi,
			'Practice Address (In English)' = (SELECT [dbo].[func_formatEngAddress](P.Room, P.[Floor], P.[Block], P.Building, P.District)),
			'Practice Address (In Chinese)' = (SELECT [dbo].[func_format_Address_Chi](P.Room, P.[Floor], P.[Block], P.Building_Chi, P.District)),
			'District' = D.district_name,
			'District Board' = D.district_board,
			'Area' = DA.area_name, 
			'Practice Status' = SD.Status_Description,
			'Profession' = Prof.Service_Category_Code
		FROM 
			#ResultTable RT
				INNER JOIN (SELECT DISTINCT SP_ID, Practice_Display_Seq FROM #VoucherTransactionBase) VTB
					ON RT.Col01 = VTB.SP_ID
				INNER JOIN ServiceProvider SP
					ON RT.Col01 = SP.SP_ID
				INNER JOIN MedicalOrganization MO
					ON RT.Col01 = MO.SP_ID
				INNER JOIN Practice P
					ON RT.Col01 = P.SP_ID AND VTB.Practice_Display_Seq = P.Display_Seq AND P.MO_Display_Seq = MO.Display_Seq
				INNER JOIN Professional Prof
					ON RT.Col01 = Prof.SP_ID AND P.Professional_Seq = Prof.Professional_Seq
				INNER JOIN district D
					ON P.District = D.district_code
				INNER JOIN district_area DA
					ON D.district_area = DA.area_code
				INNER JOIN StatusData SD
					ON P.Record_Status = SD.Status_Value AND Enum_Class = 'PracticeStatus'
		ORDER BY RT.DisplaySeq, P.Display_Seq

		CLOSE SYMMETRIC KEY sym_Key

	-- ---------------------------------------------
	-- To Excel Sheet (04): Remarks
	-- ---------------------------------------------
		
		-- Create Temp Result Table

		CREATE TABLE #Remarks (
			Seq		int,
			Seq2	int,
			Col01	varchar(1000),
			Col02	varchar(1000)
		)

		DECLARE @SchemeBackOffice TABLE (
			Scheme_Code		char(10),
			Display_Code	char(25),
			Display_Seq		smallint,
			Scheme_Desc		varchar(100)
		)

		INSERT INTO @SchemeBackOffice (Scheme_Code, Display_Code, Display_Seq, Scheme_Desc)
		SELECT
			Scheme_Code,
			MAX(Display_Code),
			MAX(Display_Seq),
			MAX(Scheme_Desc)
		FROM SchemeBackOffice WITH (NOLOCK)
		WHERE Effective_Dtm <= @request_time AND Record_Status = 'A'
		GROUP BY Scheme_Code

		SET @seq = 0

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, '(A) Legend', '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, '1. Scheme Name', '')
	
		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		SELECT @seq, NULL, Display_Code, Scheme_Desc FROM @SchemeBackOffice

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, '', '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, '2. Profession Type', '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		SELECT @seq, NULL, Service_Category_Code, Service_Category_Desc FROM Profession WITH (NOLOCK)

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, '', '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, '(B) Common Note(s) for the report', '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, '1. All service providers are counted including delisted status and those in the exception list.', '')

		SET @seq = @seq + 1

		-- ---------------------------------------------
		-- Return Result: Remarks
		-- ---------------------------------------------

		SELECT Col01, Col02 FROM #Remarks ORDER BY Seq, Seq2, Col01

	-- =============================================
	-- 6. RELEASE RESOURCE
	-- =============================================
	DROP TABLE #ServiceProviderBase
	DROP TABLE #VoucherTransactionBase
	DROP TABLE #ResultTable
END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_PPC0001_Report_get] TO HCVU
GO
