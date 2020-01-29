IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSSF004_Report_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSSF004_Report_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- CR No.:		  
-- Modified by:	  
-- Modified date: 
-- Description:	  
-- =============================================
-- =============================================
-- Author:			Winnie SUEN
-- Create date:		10 Sep 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Get Report for eHSSF004 (Weekly vaccination schedule)
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_eHSSF004_Report_get]
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
		School_Code		varchar(10), 
		SP_ID			char(8),
		Vaccine_Date	datetime,
		Report_Gen_Date	datetime,
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
		Col03	varchar(100),	-- School Name (English)
		Col04	nvarchar(100),	-- School Name (Chinese)
		Col05	varchar(100),	-- SPID
		Col06	varchar(100),	-- SP Name (English)
		Col07	nvarchar(100),	-- SP Name (Chinese)
		Col08	varchar(100),	-- Vaccination Date
		Col09	varchar(100),	-- Vaccination Report Generation Date
		Col10	varchar(100),	-- Dose to Inject
		Col11	varchar(100),	-- Total No. of Student for the Batch
		Col12	varchar(100),	-- Total No. of Student to Inject
		Col13	varchar(100),	-- Student File Status
		Col14	varchar(500)	-- Remark
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
	INSERT INTO @StudentFile (Student_File_ID, School_Code, SP_ID, Vaccine_Date, Report_Gen_Date, Dose, Record_Status)
	SELECT 
		Student_File_ID,
		School_Code,
		SP_ID,
		Service_Receive_Dtm,
		Final_Checking_Report_Generation_Date,
		Dose,
		Record_Status
	FROM 
		StudentFileHeaderStaging
	WHERE		
		Record_Status IN (	'CU',	-- Uploaded (Pending Confirmation)
							'PU',	-- Uploaded (Pending Verification)
							'PR'	-- Rectified (Pending Verification)
						)	
		AND (Service_Receive_Dtm >= @Period_From AND Service_Receive_Dtm < @Period_To) 
	UNION
	
	-- Get from Perm if the student file does not have staging record or the staging record not yet confirmed
	SELECT 
		Student_File_ID,
		School_Code,
		SP_ID,
		Service_Receive_Dtm,
		Final_Checking_Report_Generation_Date,
		Dose,
		Record_Status
	FROM 
		StudentFileHeader	
	WHERE
		Record_Status IN (	'FR',	-- Pending Final Report Generation
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
	OPEN SYMMETRIC KEY sym_Key
	DECRYPTION BY ASYMMETRIC KEY asym_Key

	-- Vaccination Period
	INSERT INTO @ResultTable (Col01)
	VALUES (
		'Vaccination Date: ' 
		+ FORMAT(@Period_From, 'yyyy/MM/dd') + ' to ' + FORMAT(DATEADD(dd, -1, @Period_To), 'yyyy/MM/dd'))
		

	INSERT INTO @ResultTable (Col01) VALUES ('')

	-- Column Header
	INSERT INTO @ResultTable (Col01, Col02, Col03, Col04, Col05, Col06, 
							 Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14)
	VALUES (
		'Student File ID', 'School Code', 'School Name (English)', 'School Name (Chinese)',
		'SPID', 'SP Name (English)', 'SP Name (Chinese)', 'Vaccination Date', 'Vaccination Report Generation Date',
		'Dose to Inject', 'Total No. of Student for the Batch', 'Total No. of Student to Inject', 'Student File Status', 'Remark'
		)

	-- Report Result
	INSERT INTO @ResultTable (Col01, Col02, Col03, Col04, Col05, Col06, 
							 Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14)
	SELECT 
		SF.Student_File_ID,
		SCH.School_Code, 
		SCH.Name_Eng,	-- School Name (English)
		SCH.Name_Chi,	-- School Name (Chinese)
		SP.SP_ID,
		CONVERT(varchar(100), DecryptByKey(SP.Encrypt_Field2)),		-- SP Name (English)
		CONVERT(nvarchar(100), DecryptByKey(SP.Encrypt_Field3)),	-- SP Name (Chinese)
		FORMAT(SF.Vaccine_Date, 'yyyy/MM/dd') AS [Vaccination Date],
		FORMAT(SF.Report_Gen_Date, 'yyyy/MM/dd') AS [Vaccination Report Generation Date],
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
	INNER JOIN
		School SCH
		ON SF.School_Code = SCH.School_Code
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

	CLOSE SYMMETRIC KEY sym_Key
	

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
		Col11,Col12,Col13, Col14
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

GRANT EXECUTE ON [dbo].[proc_EHS_eHSSF004_Report_get] TO HCVU
GO

