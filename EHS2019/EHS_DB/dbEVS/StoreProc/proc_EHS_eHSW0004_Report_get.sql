IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSW0004_Report_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSW0004_Report_get]
GO

SET ANSI_NULLS ON
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
-- Modified by:		Chris YIM
-- Modified date:	16 Nov 2020
-- CR No.			INT20-0049
-- Description:		Fix column length of Subsidy(Display code for claim)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	11 Nov 2019
-- CR No.			INT19-0026
-- Description:		Fix column length of School/RCH name
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM		
-- Modified date:	16 Sep 2019
-- CR No.			CRE19-001
-- Description:		Change length of school code
-- =============================================
-- =============================================
-- Author:			Koala CHENG
-- Create date:		15 Aug 2019
-- CR No.:			CRE19-001 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Get Report for eHSW0004 (Weekly vaccination schedule)
--					Copy and modify from existing SProc [proc_EHS_eHSSF004_Report_get]
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_eHSW0004_Report_get]
	@Report_Dtm			DATETIME = NULL
AS BEGIN

-- =============================================
-- Declaration
-- =============================================

	DECLARE @Period_From	DATETIME
	DECLARE @Period_To		DATETIME
	DECLARE @PeriodDays		INT = 14		-- following 14 days

	IF @Report_Dtm IS NOT NULL BEGIN
		SELECT @Report_Dtm = CONVERT(varchar, @Report_Dtm, 106)
	END ELSE BEGIN
		SELECT @Report_Dtm = CONVERT(varchar, GETDATE(), 106) -- "106" gives "dd MMM yyyy"  
	END

	SELECT @Period_From = @Report_Dtm					--  (datetime compare logic use ">=")
	SELECT @Period_To = DATEADD(DD, @PeriodDays, @Period_From)	--  (datetime compare logic use "<")
	
	DECLARE @Vaccine_Remark VARCHAR(500)
	SET @Vaccine_Remark = 'The interval between vaccination report generation date and vaccination date is more than 1 week'


	-- Create Temp table for Student File
	DECLARE @StudentFile table (
		Student_File_ID	varchar(15),
		Scheme_Code varchar(10),
		Scheme_Code_Display varchar(25),
		School_Code		varchar(30), 
		SP_ID			char(8),
		Vaccine_Date	datetime,
		Report_Gen_Date	datetime,
		Subsidy			varchar(25),
		Dose			varchar(20),
		Record_Status	varchar(2)
	)
	
	-- Create Temp table for Student File Entry
	DECLARE @StudentFileEntry table (
		Student_File_ID		varchar(15),
		Student_Seq			int,
		Reject_Injection	char(1),
		Entitle_Inject		char(1)
	)

	-- Create Temp Result Table
	DECLARE @ResultTable table (
		Display_Seq		INT IDENTITY(1,1),
		Col01	varchar(100),	-- Student File ID
		Col02	varchar(100),	-- School Code
		Col03	varchar(255),	-- School Name (English)
		Col04	nvarchar(255),	-- School Name (Chinese)
		Col05	varchar(100),	-- SPID
		Col06	varchar(100),	-- SP Name (English)
		Col07	nvarchar(100),	-- SP Name (Chinese)
		Col08	varchar(100),	-- Vaccination Date
		Col09	varchar(100),	-- Vaccination Report Generation Date
		Col10	varchar(100),	-- Scheme
		Col11	varchar(100),	-- Subsidy
		Col12	varchar(100),	-- Dose to Inject
		Col13	varchar(100),	-- Total No. of Student for the Batch
		Col14	varchar(100),	-- Total No. of Student to Inject
		Col15	varchar(100),	-- Student File Status
		Col16	varchar(500)	-- Remark
	)

	DECLARE @RemarkTable table (   
		Display_Seq		INT IDENTITY(1,1),     
		Col01			VARCHAR(500) 
	)
-- =============================================
-- Initialization
-- =============================================

-- =============================================
-- Retrieve Data
-- =============================================
	-- Retrieve Student File List that will have vaccination in period
	-- From Staging
	INSERT INTO @StudentFile (Student_File_ID, Scheme_Code, Scheme_Code_Display, School_Code, SP_ID, Vaccine_Date, Report_Gen_Date, Subsidy, Dose, Record_Status)
	SELECT 
		Student_File_ID,
		H.Scheme_Code,
		SC.Display_Code,
		School_Code,
		SP_ID,
		Service_Receive_Dtm,
		Final_Checking_Report_Generation_Date,
		SGC.Display_Code_For_Claim,
		Dose,
		H.Record_Status
	FROM 
		StudentFileHeaderStaging H
	INNER JOIN SchemeClaim SC
		ON SC.Scheme_Code = H.Scheme_Code
	INNER JOIN SubsidizeGroupClaim SGC
		ON SGC.Scheme_Code = H.Scheme_Code
			AND SGC.Scheme_Seq = H.Scheme_Seq
			AND SGC.Subsidize_Code = H.Subsidize_Code
	WHERE		
		H.Record_Status IN (	'CU',	-- Uploaded (Pending Confirmation)
							'PU',	-- Uploaded (Pending Verification)
							'PR'	-- Rectified (Pending Verification)
						)	
		AND (Service_Receive_Dtm >= @Period_From AND Service_Receive_Dtm < @Period_To) 
	UNION
	
	-- Get from Perm if the student file does not have staging record or the staging record not yet confirmed
	SELECT 
		Student_File_ID,
		H.Scheme_Code,
		SC.Display_Code,
		School_Code,
		SP_ID,
		Service_Receive_Dtm,
		Final_Checking_Report_Generation_Date,
		SGC.Display_Code_For_Claim,
		Dose,
		H.Record_Status
	FROM 
		StudentFileHeader H
	INNER JOIN SchemeClaim SC
		ON SC.Scheme_Code = H.Scheme_Code
	INNER JOIN SubsidizeGroupClaim SGC
		ON SGC.Scheme_Code = H.Scheme_Code
			AND SGC.Scheme_Seq = H.Scheme_Seq
			AND SGC.Subsidize_Code = H.Subsidize_Code	
	WHERE
		H.Record_Status IN (	'FR',	-- Pending Final Report Generation
							'CR',	-- Rectified (Pending Confirmation)
							'UT'	-- Pending Upload Vaccination Claim
						)
		AND (Service_Receive_Dtm >= @Period_From AND Service_Receive_Dtm < @Period_To) 

	-- Retrieve Student File Entry
	-- Get Student list from Perm
	INSERT INTO @StudentFileEntry (Student_File_ID, Student_Seq, Reject_Injection, Entitle_Inject)
	Select 
		SF.Student_File_ID, 
		SE.Student_Seq,
		SE.Reject_Injection,
		SE.Entitle_Inject
	FROM 
		@StudentFile SF
	INNER JOIN StudentFileEntry SE ON SE.Student_File_ID = SF.Student_File_ID
	WHERE 
		SF.Record_Status NOT IN ('CU', 'PU')

	UNION
	
	Select 
		SF.Student_File_ID, 
		SE.Student_Seq,
		SE.Reject_Injection,
		SE.Entitle_Inject
	FROM 
		@StudentFile SF
	INNER JOIN StudentFileEntryStaging SE ON SE.Student_File_ID = SF.Student_File_ID
	WHERE 
		SF.Record_Status IN ('CU', 'PU')


	-- Update Student File Entry with rectified record for status [Rectified (Pending Verification)]
	UPDATE SE
	SET Reject_Injection = SS.Reject_Injection,
		Entitle_Inject = SS.Entitle_Inject
	FROM
		@StudentFileEntry SE
	INNER JOIN
		@StudentFile SF ON SF.Student_File_ID = SE.Student_File_ID
	INNER JOIN
		StudentFileEntryStaging SS ON SE.Student_File_ID = SS.Student_File_ID AND SE.Student_Seq = SS.Student_Seq
	WHERE 
		SF.Record_Status = 'PR'


---
	EXEC [proc_SymmetricKey_open]

	-- Vaccination Period
	INSERT INTO @ResultTable (Col01)
	VALUES (
		'Vaccination Date: ' 
		+ FORMAT(@Period_From, 'yyyy/MM/dd') + ' to ' + FORMAT(DATEADD(dd, -1, @Period_To), 'yyyy/MM/dd'))
		

	INSERT INTO @ResultTable (Col01) VALUES ('')

	-- Column Header
	INSERT INTO @ResultTable (Col01, Col02, Col03, Col04,
							 Col05, Col06, Col07, Col08, Col09, Col10, Col11,
							 Col12, Col13, Col14, Col15, Col16)
	VALUES (
		'Vaccination File ID', 'School/RCH Code', 'School/RCH Name (English)', 'School/RCH Name (Chinese)',
		'SPID', 'SP Name (English)', 'SP Name (Chinese)', 'Vaccination Date', 'Vaccination Report Generation Date','Scheme','Subsidy',
		'Dose to Inject', 'Total No. of Client for the Batch', 'Total No. of Client to Inject', 'Vaccination File Status', 'Remark'
		)

	-- Report Result
	INSERT INTO @ResultTable (Col01, Col02, Col03, Col04,
							 Col05, Col06, Col07, Col08, Col09, Col10, Col11,
							 Col12, Col13, Col14, Col15, Col16)
	SELECT 
		SF.Student_File_ID,
		SF.School_Code, 
		CASE WHEN SCH.Name_Eng IS NOT NULL THEN SCH.Name_Eng ELSE RCH.Homename_Eng END,	-- School/RCH Name (English)
		CASE WHEN SCH.Name_Chi IS NOT NULL THEN SCH.Name_Chi ELSE RCH.Homename_Chi END,	-- School/RCH Name (Chinese)
		SP.SP_ID,
		CONVERT(varchar(100), DecryptByKey(SP.Encrypt_Field2)),		-- SP Name (English)
		CONVERT(nvarchar(100), DecryptByKey(SP.Encrypt_Field3)),	-- SP Name (Chinese)
		FORMAT(SF.Vaccine_Date, 'yyyy/MM/dd') AS [Vaccination Date],
		FORMAT(SF.Report_Gen_Date, 'yyyy/MM/dd') AS [Vaccination Report Generation Date],
		SF.Scheme_Code_Display,
		SF.Subsidy,
		SD.Data_Value,	-- Dose to inject
		SS.StudentCount,
		SS.InjectCount,
		SSD.Status_Description,	-- Student File Status
		CASE 
			WHEN DATEDIFF(DD, Report_Gen_Date, Vaccine_Date) >= 7 
				THEN @Vaccine_Remark
			ELSE 
				''
		END AS [Remark]
	FROM
		@StudentFile SF
	INNER JOIN
		ServiceProvider SP WITH (NOLOCK)
		ON SF.SP_ID = SP.SP_ID
	LEFT JOIN
		School SCH
		ON SF.School_Code = SCH.School_Code
			AND SF.Scheme_Code = SCH.Scheme_Code
	LEFT JOIN
		RVPHomeList RCH
		ON SF.School_Code = RCH.RCH_code
			AND SF.Scheme_Code = 'RVP'
	LEFT JOIN 
		StaticData SD
		ON SF.Dose = SD.Item_No AND SD.Column_Name = 'StudentFileDoseToInject'
	LEFT JOIN 
		StatusData SSD
		ON SF.Record_Status = SSD.Status_Value AND SSD.Enum_Class = 'StudentFileHeaderStatus'

	CROSS APPLY (
			SELECT Student_File_ID, 
				MAX(Student_Seq) AS StudentCount,
				SUM(CASE WHEN ISNULL(Reject_Injection, 'N') = 'Y' THEN 
					0 
				 ELSE
					CASE WHEN ISNULL(Entitle_Inject,'N') = 'Y' THEN 1 ELSE 0 END
				 END) AS InjectCount
			FROM 
				@StudentFileEntry SE
			WHERE 
				SE.Student_File_ID = SF.Student_File_ID
			GROUP BY 
				Student_File_ID
		) AS SS 
	ORDER BY
		SF.Vaccine_Date, SF.Student_File_ID

	EXEC [proc_SymmetricKey_close]
	

-- =============================================
-- Return results
-- =============================================
-- ---------------------------------------------
-- To Excel Sheet: Content
-- ---------------------------------------------

	SELECT	'Report Generation Time: ' + FORMAT(GETDATE(), 'yyyy/MM/dd HH:mm') AS Result_Value
		
-- ---------------------------------------------
-- To Excel Sheet: 01
-- ---------------------------------------------

	SELECT
		Col01,Col02,Col03,Col04,Col05,
		Col06,Col07,Col08,Col09,Col10,
		Col11,Col12,Col13,Col14,Col15,Col16
	FROM
		@ResultTable
	ORDER BY
		Display_Seq

-- --------------------------------------------------    
-- To Excel sheet:   Remark    
-- --------------------------------------------------   
	INSERT INTO @RemarkTable (Col01) SELECT '(A) Common Note(s) for the report'
	INSERT INTO @RemarkTable (Col01) SELECT '1. Student File:' 
	INSERT INTO @RemarkTable (Col01) SELECT '    a. The removed student files are excluded'

	SELECT 
		Col01 
	FROM 
		@RemarkTable 
	ORDER BY 
		Display_Seq

END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSW0004_Report_get] TO HCVU
GO

