IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSSF_Report_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSSF_Report_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	17 Sep 2020
-- CR No.			CRE20-003-03 (Enhancement on Programme or Scheme using batch upload)
-- Description:		Fix Report VF000 Display Content issue
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	17 Aug 2020
-- CR No.			CRE20-003 (Batch Upload)
-- Description:		Add columns (Second Vaccination Date)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	20 Jul 2020
-- CR No.			CRE19-031 (VSS MMR Upload)
-- Description:		Add columns (HKICSymbol, Service_Receive_Dtm)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	18 Oct 2019
-- CR No.:			CRE19-014 (Update vaccination checking report)
-- Description:		Show No. of Clients available to inject "Only Dose/1st Dose/2nd Dose" in EHSVF001 report
-- =============================================  
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	06 Sep 2019
-- CR No.:			CRE19-001-04 (VSS 2019/20 - Precheck)
-- Description:		Add EHSVF001 RVP precheck report
-- =============================================  
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	05 Aug 2019
-- CR No.:			CRE19-001 (VSS 2019/20)
-- Description:		[Batch] worksheet
--					- Add 2nd dose information
--					- Revise to eHS(S)VF00X Report
-- =============================================  
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	30 Oct 2018
-- CR No.:			CRE18-011 (Check vaccination record of students with rectified information in rectification file)
-- Description:		Include HA/DH demographics not matched information to summary
-- =============================================  
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	19 October 2018
-- CR No.:			CRE18-010 (Adding one short form of student vaccination file upon the first upload)
-- Description:		Add new report eHS(S)SF001B
--					Include HA/DH connection fail information to summary
-- ============================================= 
-- =============================================
-- Modification History
-- Modified by:		Marco CHOI
-- Modified date:	17 August 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Get StudentFileExcelReport
-- =============================================    

CREATE PROCEDURE [dbo].[proc_EHS_eHSSF_Report_get]
	@Input_Student_File_ID		VARCHAR(15),
	@File_ID					VARCHAR(9),
	@scheme_code				CHAR(10),
	@scheme_code_display		VARCHAR(25),
	@Visit						INT = 0
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
	DECLARE @SchoolOrRCH AS VARCHAR(10)
	DECLARE @ClassOrCategory AS VARCHAR(10)
	DECLARE @Upload_Precheck AS VARCHAR(1)
	DECLARE @SubsidizeCode AS VARCHAR(10)
	DECLARE @Subsidize_Item_Code AS VARCHAR(10)
	DECLARE @Dose AS VARCHAR(20)

	DECLARE @OnlyDose AS VARCHAR(20)
	DECLARE @1stDose AS VARCHAR(20)
	DECLARE @2ndDose AS VARCHAR(20)
	DECLARE @3rdDose AS VARCHAR(20)

	DECLARE @AvailableDose_Item1 AS VARCHAR(20)
	DECLARE @AvailableDose_Item2 AS VARCHAR(20)
	DECLARE @AvailableDose_Item3 AS VARCHAR(20)
	DECLARE @DoseRemark AS VARCHAR(100) = ''


	SET @File_ID = UPPER(@File_ID)
		
	CREATE TABLE #BatchTT (
		R1 INT,
		Col1 NVARCHAR(MAX),
		Col2 NVARCHAR(MAX),
		Col3 NVARCHAR(MAX),
		Col4 NVARCHAR(MAX),
		Col5 NVARCHAR(MAX),
		Col6 NVARCHAR(MAX),
		Col7 NVARCHAR(MAX),
		Col8 NVARCHAR(MAX),
		Col9 NVARCHAR(MAX),
		Col10 NVARCHAR(MAX),
		Col11 NVARCHAR(MAX),
		Col12 NVARCHAR(MAX),
		Col13 NVARCHAR(MAX),
		Col14 NVARCHAR(MAX),
		Col15 NVARCHAR(MAX),
		Col16 NVARCHAR(MAX),
		Col17 NVARCHAR(MAX),
		Col18 NVARCHAR(MAX),
		Col19 NVARCHAR(MAX),
		Col20 NVARCHAR(MAX),
		Col21 NVARCHAR(MAX),
		Col22 NVARCHAR(MAX),
		Col23 NVARCHAR(MAX),
		Col24 NVARCHAR(MAX),
		Col25 NVARCHAR(MAX),
		Col26 NVARCHAR(MAX),
		Col27 NVARCHAR(MAX),
		Col28 NVARCHAR(MAX),
		Col29 NVARCHAR(MAX),
		Col30 NVARCHAR(MAX),
		Col31 NVARCHAR(MAX),
		Col32 NVARCHAR(MAX),
		Col33 NVARCHAR(MAX),
		Col34 NVARCHAR(MAX),
		Col35 NVARCHAR(MAX),
		Col36 NVARCHAR(MAX),
		Col37 NVARCHAR(MAX),
		Col38 NVARCHAR(MAX),
		Col39 NVARCHAR(MAX),
		Col40 NVARCHAR(MAX)
	)
	
	CREATE TABLE #RemarkTT (			
		Result01 NVARCHAR(100),  
		Result02 NVARCHAR(100),    
		DisplaySeq INT       
	)

	CREATE TABLE #Summary (
		ColIndex INT,
		Col1 NVARCHAR(MAX),
		Col2 NVARCHAR(MAX),
		Col3 NVARCHAR(MAX)
	)
		
	DECLARE @wsRemark_ct INT 
	SET @wsRemark_ct = 1   
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	
	SET @SchoolOrRCH = 'School'
	SET @ClassOrCategory = 'Class'

	IF @scheme_code = 'RVP' OR @scheme_code = 'VSS'
	BEGIN
		SET @SchoolOrRCH = 'RCH'
		SET @ClassOrCategory = 'Category'
	END
	
	SELECT 
		@Upload_Precheck = H.Upload_Precheck, 
		@Dose = H.Dose,
		@Subsidize_Item_Code = S.Subsidize_Item_Code,
		@SubsidizeCode = H.Subsidize_Code
	FROM 
		StudentFileHeader H
		LEFT JOIN Subsidize S ON H.Subsidize_Code = S.Subsidize_Code
	WHERE Student_File_ID = @Input_Student_File_ID

	-- Available Dose
	SET @OnlyDose = 'Only Dose'
	SET @1stDose = '1st Dose'
	SET @2ndDose = '2nd Dose'
	SET @3rdDose = '3rd Dose'

	
	IF @Subsidize_Item_Code = 'SIV'
	BEGIN
		SET @AvailableDose_Item1 = @OnlyDose
		SET @AvailableDose_Item2 = @1stDose
		SET @AvailableDose_Item3 = @2ndDose

		SET @DoseRemark = (SELECT CASE WHEN @Dose = '1STDOSE' THEN '* No. of clients for only dose + 1st dose' ELSE '' END)
	END
	ELSE IF @Subsidize_Item_Code IN ('PV', 'PV13')
	BEGIN
		SET @AvailableDose_Item1 = @OnlyDose
		SET @AvailableDose_Item2 = ''
		SET @AvailableDose_Item3 = ''

		SET @DoseRemark = ''
	END
	ELSE IF @Subsidize_Item_Code = 'MMR'
	BEGIN
		IF @SubsidizeCode = 'VNIAMMR' 
			BEGIN
				SET @AvailableDose_Item1 = @1stDose
				SET @AvailableDose_Item2 = @2ndDose
				SET @AvailableDose_Item3 = @3rdDose

				SET @DoseRemark = (SELECT CASE 
											WHEN @Dose = '1STDOSE' THEN '* No. of clients for 1st dose' 
											WHEN @Dose = '2NDDOSE' THEN '* No. of clients for 2nd dose' 
											ELSE '' END)
			END
		ELSE
			BEGIN
				SET @AvailableDose_Item1 = @1stDose
				SET @AvailableDose_Item2 = @2ndDose
				SET @AvailableDose_Item3 = ''

				SET @DoseRemark = (SELECT CASE WHEN @Dose = '1STDOSE' THEN '* No. of clients for 1st dose' ELSE '' END)
			END
	END

	--

	OPEN SYMMETRIC KEY sym_Key
	DECRYPTION BY ASYMMETRIC KEY asym_Key
	
	IF @File_ID = 'EHSVF000'
	BEGIN
		INSERT INTO #BatchTT
		(
			R1, 
			Col1, Col2, Col3, Col4, Col5, Col6, Col7,
			Col8, Col9, Col10, Col11, Col12,
			Col13, Col14, Col15, Col16, 
			Col17, Col18, Col19, Col20,
			Col21, Col22, Col23, Col24, Col25
		)
		SELECT 1, 
				'Vaccination File ID', @SchoolOrRCH + ' Code', @SchoolOrRCH + ' Name', '', @SchoolOrRCH +' Address', '', '', 
				'Service Provider ID', 'Service Provider Name', '', 'Practice Name (Practice No.)', '', 
				'', 'Scheme' , 'No. of ' + @ClassOrCategory, 
				'No. of Clients', '','No. of Clients failed to connect HA CMS/DH CIMS on vaccination checking','No. of Clients found in HA CMS/DH CIMS but demographics not match',
				'','No. of Clients available to inject', 'No. of Clients confirmed NOT to inject', '', 'Report Generation Time', ''	
	END
	ELSE IF @File_ID = 'EHSVF001'
	BEGIN
					
		INSERT INTO #BatchTT
		(
			R1, 
			Col1, Col2, Col3, Col4, Col5, Col6, Col7,
			Col8, Col9, Col10, Col11, Col12,
			Col13, Col14, Col15, Col16, Col17,	
			Col18, Col19, Col20, Col21, Col22, 
			Col23, Col24, Col25, Col26, Col27, 
			Col28, Col29, Col30, Col31, Col32, 
			Col33, Col34, Col35, Col36, Col37, Col38, Col39
		)
		SELECT 1, 
				'Vaccination File ID', @SchoolOrRCH + ' Code', @SchoolOrRCH + ' Name', '', @SchoolOrRCH +' Address', '', '', 
				'Service Provider ID', 'Service Provider Name', '', 'Practice Name (Practice No.)', '', 
				'', '', 
				CASE WHEN @scheme_code = 'RVP' OR @scheme_code = 'VSS' THEN 'Vaccination Date' ELSE '1st Vaccination Date' END, 
				CASE WHEN @scheme_code = 'RVP' OR @scheme_code = 'VSS' THEN 'Vaccination Report Generation Date' ELSE '1st Vaccination Report Generation Date' END,
				CASE WHEN @scheme_code = 'RVP' OR @scheme_code = 'VSS' THEN '' ELSE '2nd Vaccination Date' END, 
				CASE WHEN @scheme_code = 'RVP' OR @scheme_code = 'VSS' THEN '' ELSE '2nd Vaccination Report Generation Date' END,
				'Scheme' ,'Subsidy', 'Dose to inject', 'No. of ' + @ClassOrCategory, 
				'No. of Clients', '','No. of Clients failed to connect HA CMS/DH CIMS on vaccination checking','No. of Clients found in HA CMS/DH CIMS but demographics not match','',				
				'No. of Clients available to inject' + CASE WHEN @DoseRemark <> '' THEN ' *' ELSE '' END,
				CASE WHEN @AvailableDose_Item1 <> '' THEN ' - ' + @AvailableDose_Item1 ELSE '' END,
				CASE WHEN @AvailableDose_Item2 <> '' THEN ' - ' + @AvailableDose_Item2 ELSE '' END,
				CASE WHEN @AvailableDose_Item3 <> '' THEN ' - ' + @AvailableDose_Item3 ELSE '' END,
				'No. of Clients confirmed NOT to inject', '', '', 'Checking Date', '', 'Report Generation Time', '', @DoseRemark

	END
	ELSE IF @File_ID = 'EHSVF002'
	BEGIN
		INSERT INTO #BatchTT
			(
				R1, 
				Col1, Col2, Col3, Col4, Col5, Col6, Col7,
				Col8, Col9, Col10, Col11, Col12,
				Col13, Col14, Col15, Col16, Col17,	
				Col18, Col19, Col20, Col21, Col22, 
				Col23, Col24, Col25, Col26, Col27, 
				Col28, Col29, Col30, col31, Col32
			)
			SELECT 1, 
				'Vaccination File ID', @SchoolOrRCH + ' Code', @SchoolOrRCH + ' Name', '', @SchoolOrRCH +' Address', '', '', 
				'Service Provider ID', 'Service Provider Name', '', 'Practice Name (Practice No.)', '', 
				'', '', 
				CASE WHEN @scheme_code = 'RVP' OR @scheme_code = 'VSS' THEN 'Vaccination Date' ELSE '1st Vaccination Date' END, 
				CASE WHEN @scheme_code = 'RVP' OR @scheme_code = 'VSS' THEN 'Vaccination Report Generation Date' ELSE '1st Vaccination Report Generation Date' END,
				CASE WHEN @scheme_code = 'RVP' OR @scheme_code = 'VSS' THEN '' ELSE '2nd Vaccination Date' END, 
				CASE WHEN @scheme_code = 'RVP' OR @scheme_code = 'VSS' THEN '' ELSE '2nd Vaccination Report Generation Date' END,
				'Scheme' ,'Subsidy', 'Dose to inject', 'No. of ' + @ClassOrCategory, 
				'No. of Clients', '','No. of Clients available to inject', 'No. of Clients confirmed NOT to inject', '', '', 'Checking Date', '', 'Report Generation Time', ''	
	END
	ELSE IF @File_ID = 'EHSVF003'
	BEGIN
		INSERT INTO #BatchTT
		(
			R1, 
			Col1, Col2, Col3, Col4, Col5, 
			Col6, Col7, Col8, Col9, Col10,
			Col11, Col12, Col13, Col14, Col15,	
			Col16, Col17, Col18, Col19, Col20,
			Col21, Col22, Col23, Col24, Col25,	
			Col26, Col27, Col28, Col29, Col30,
			Col31, Col32, Col33, Col34, Col35,
			Col36
		)
		SELECT 1, 
				'Vaccination File ID', @SchoolOrRCH + ' Code', @SchoolOrRCH + ' Name', '', @SchoolOrRCH +' Address', 
				'', '', 'Service Provider ID', 'Service Provider Name',
				'', 'Practice Name (Practice No.)', '', '', '', 
				CASE WHEN @scheme_code = 'RVP' OR @scheme_code = 'VSS' THEN 'Vaccination Date' ELSE '1st Vaccination Date' END, 
				CASE WHEN @scheme_code = 'RVP' OR @scheme_code = 'VSS' THEN 'Vaccination Report Generation Date' ELSE '1st Vaccination Report Generation Date' END,
				CASE WHEN @scheme_code = 'RVP' OR @scheme_code = 'VSS' THEN '' ELSE '2nd Vaccination Date' END, 
				CASE WHEN @scheme_code = 'RVP' OR @scheme_code = 'VSS' THEN '' ELSE '2nd Vaccination Report Generation Date' END,
				'Scheme', 'Subsidy', 'Dose to inject', 'No. of ' + @ClassOrCategory, 'No. of Clients', '',
				'No. of Clients failed to connect HA CMS/DH CIMS on vaccination checking','No. of Clients found in HA CMS/DH CIMS but demographics not match', '',
				'No. of Clients available to inject', 'No. of Clients confirmed NOT to inject', '', 'Date to upload the injection record', 'No. of Clients uploaded with injection record', 
				'No. of Clients created claims', 'No. of Clients failed to create claim',	'', 'Report Generation Time'	
	END
	ELSE IF @File_ID = 'EHSVF005' -- Rectification File
	BEGIN
		INSERT INTO #BatchTT
		(
			R1, 
			Col1, Col2, Col3, Col4, Col5, 
			Col6, Col7, Col8, Col9, Col10,
			Col11, Col12, Col13, Col14, Col15,	
			Col16
		)
		SELECT 1, 
				'Vaccination File ID', @SchoolOrRCH + ' Code', @SchoolOrRCH + ' Name', '', @SchoolOrRCH +' Address', 
				'', '', 'Service Provider ID', 'Service Provider Name',
				'', 'Practice Name (Practice No.)', '', '', 'Scheme', '', 'Report Generation Time'	
	END
	ELSE IF @File_ID IN ('EHSVF006') AND @Upload_Precheck = 'N' -- Name list
	BEGIN
		INSERT INTO #BatchTT
		(
			R1, 
			Col1, Col2, Col3, Col4, Col5, Col6, Col7,
			Col8, Col9, Col10, Col11, Col12,
			Col13, Col14, Col15, Col16, Col17,	
			Col18, Col19, Col20, Col21, Col22, Col23, Col24, Col25, Col26
		)
		SELECT 1, 
				'Vaccination File ID', @SchoolOrRCH + ' Code', @SchoolOrRCH + ' Name', '', @SchoolOrRCH +' Address', '', '', 
				'Service Provider ID', 'Service Provider Name', '', 'Practice Name (Practice No.)', '', 
				'', '', 
				CASE WHEN @scheme_code = 'RVP' OR @scheme_code = 'VSS' THEN 'Vaccination Date' ELSE '1st Vaccination Date' END, 
				CASE WHEN @scheme_code = 'RVP' OR @scheme_code = 'VSS' THEN 'Vaccination Report Generation Date' ELSE '1st Vaccination Report Generation Date' END,
				CASE WHEN @scheme_code = 'RVP' OR @scheme_code = 'VSS' THEN '' ELSE '2nd Vaccination Date' END, 
				CASE WHEN @scheme_code = 'RVP' OR @scheme_code = 'VSS' THEN '' ELSE '2nd Vaccination Report Generation Date' END,
				'Scheme' ,'Subsidy', 'Dose to inject', 'No. of ' + @ClassOrCategory, 
				'No. of Clients', '', 'Report Generation Time', ''	
	END	
	ELSE IF @File_ID IN ('EHSVF006') AND @Upload_Precheck = 'Y' -- Name list (Precheck)
	BEGIN
		INSERT INTO #BatchTT
		(
			R1, 
			Col1, Col2, Col3, Col4, Col5, Col6, Col7,
			Col8, Col9, Col10, Col11, Col12,
			Col13, Col14, Col15, Col16, Col17,	
			Col18, Col19, Col20, Col21, Col22, Col23, Col24, Col25, Col26
		)
		SELECT 1, 
				'Vaccination File ID', @SchoolOrRCH + ' Code', @SchoolOrRCH + ' Name', '', @SchoolOrRCH +' Address', '', '', 
				'Service Provider ID', 'Service Provider Name', '', 'Practice Name (Practice No.)', '', 
				'', '', 
				'', '','', '',
				'Scheme' ,'', '', 'No. of ' + @ClassOrCategory, 
				'No. of Clients', '', 'Report Generation Time', ''	
	END	
	--
	
	
	IF @File_ID = 'EHSVF000'
	BEGIN
		INSERT INTO #BatchTT
		(
			R1,
			Col1, Col2, Col3, Col4, Col5, Col6, Col7,
			Col8, Col9, Col10, Col11, Col12,
			Col13, Col14, Col15, Col16, 
			Col17, Col18, Col19, Col20,
			Col21, Col22, Col23, Col24, Col25, Col26
		)
		SELECT  DISTINCT
			2,
			E.Student_File_ID,
			H.School_Code,
			CASE WHEN H.Scheme_Code = 'RVP' THEN RCH.Homename_Eng ELSE S.Name_Eng END AS SchoolName_EN,
			CASE WHEN H.Scheme_Code = 'RVP' THEN RCH.Homename_Chi ELSE S.Name_Chi END AS SchoolName_TC,
			CASE WHEN H.Scheme_Code = 'RVP' THEN RCH.Address_Eng ELSE S.Address_Eng END AS SchoolAddr_EN,
			CASE WHEN H.Scheme_Code = 'RVP' THEN RCH.Address_Chi ELSE S.Address_Chi END AS SchoolAddr_TC,
			'',
			H.SP_ID,
			CONVERT(VARCHAR(MAX), DecryptByKey(SP.Encrypt_Field2)) AS SPName_EN,
			CONVERT(NVARCHAR(MAX), DecryptByKey(SP.Encrypt_Field3)) AS SPName_Chi,
			[Practice_Name] = P.Practice_Name + '(' + CONVERT(VARCHAR(10), Practice_Display_Seq) +')',
			[Practice_Name_Chi] =
				CASE 
					WHEN P.Practice_Name_Chi IS NULL THEN '' 
					WHEN P.Practice_Name_Chi = '' THEN '' 
					ELSE P.Practice_Name_Chi + '(' + CONVERT(VARCHAR(10), Practice_Display_Seq) +')'
				END,
			'',
			RTRIM(SC.Display_Code),
			ET.ClassCount,
			ET.StudentCount,
			'',
			ET.VaccineConnectionFail,
			ET.VaccineDemographicsNotMatch,
			'',
			AvailbleToInjectCount,
			NotInjectCount,
			'',
			CONVERT(varchar, GETDATE(), 111) + ' ' + CONVERT(VARCHAR(5), GETDATE(), 108), '', ''
		FROM StudentFileEntry E
			INNER JOIN StudentFileHeader H
				ON E.Student_File_ID = H.Student_File_ID
			LEFT JOIN School S
				ON S.School_Code = H.School_Code
					AND S.Scheme_Code = H.Scheme_Code
			LEFT JOIN RVPHomeList RCH
				ON RCH.RCH_code = H.School_Code
			INNER JOIN ServiceProvider SP
				ON SP.SP_ID = H.SP_ID
			INNER JOIN (
				SELECT Student_File_ID, 
						MAX(Student_Seq) AS StudentCount,
						COUNT(DISTINCT Class_Name) AS ClassCount,
						(SELECT COUNT(DISTINCT Student_Seq) FROM StudentFileEntrySubsidizePrecheck 
							WHERE  student_file_ID = @Input_Student_File_ID
							AND (Entitle_ONLYDOSE = 'Y' OR Entitle_1STDOSE = 'Y' OR Entitle_2NDDOSE = 'Y')
						) AS AvailbleToInjectCount,
						SUM(
							CASE WHEN ISNULL(Reject_Injection, '') = 'Y' THEN 1 ELSE 0 END
						) AS NotInjectCount,
						SUM(
							CASE WHEN ISNULL(Injected, '') = 'Y' THEN 1 ELSE 0 END
						) AS WithInjectRecordCount,
						SUM(
							CASE WHEN ISNULL(Transaction_ID, '') <> '' THEN 1 ELSE 0 END
						) AS CreatedClaimCount,
						SUM(
							CASE WHEN ISNULL(Transaction_ID, '') = '' THEN 1 ELSE 0 END
						) AS NotCreatedClaimCount,
						SUM(
							CASE WHEN Ext_Ref_Status LIKE '_C_' OR Ext_Ref_Status LIKE '_S_' OR DH_Vaccine_Ref_Status LIKE '_C_'OR DH_Vaccine_Ref_Status LIKE '_S_' THEN 1 ELSE 0 END
						) AS VaccineConnectionFail,
						SUM(
							CASE WHEN Ext_Ref_Status LIKE '_P_' OR DH_Vaccine_Ref_Status LIKE '_P_' THEN 1 ELSE 0 END
						) AS VaccineDemographicsNotMatch
					FROM StudentFileEntry
					WHERE Student_File_ID= @Input_Student_File_ID
					GROUP BY Student_File_ID
				) AS ET
				ON ET.Student_File_ID = E.Student_File_ID
			INNER JOIN Practice P
				ON P.SP_ID = H.SP_ID
					AND P.Display_Seq = H.Practice_Display_Seq
			INNER JOIN SchemeClaim SC
				ON SC.Scheme_Code = H.Scheme_Code
		WHERE 
			E.Student_File_ID = @Input_Student_File_ID

		-- 2nd Dose Column
		INSERT INTO #BatchTT
		(
			R1,
			Col1, Col2, Col3, Col4, Col5, Col6, Col7,
			Col8, Col9, Col10, Col11, Col12,
			Col13, Col14, Col15, Col16, 
			Col17, Col18, Col19, Col20,
			Col21, Col22, Col23, Col24, Col25, Col26
		)
		SELECT  DISTINCT
			3,
			'',
			'',
			'' AS SchoolName_EN,
			'' AS SchoolName_TC,
			'' AS SchoolAddr_EN,
			'' AS SchoolAddr_TC,
			'',
			'',
			'' AS SPName_EN,
			'' AS SPName_Chi,
			'' AS Practice_Name,
			'' AS Practice_Name_Chi,
			'',
			'',
			'',
			'',
			'',
			'',
			'',
			'',
			'',
			'',
			'',
			'', '', ''
		FROM StudentFileHeader
		WHERE Student_File_ID= @Input_Student_File_ID

	END
	ELSE IF @File_ID = 'EHSVF001'
	BEGIN
		INSERT INTO #BatchTT
		(
			R1,
			Col1, Col2, Col3, Col4, Col5, Col6, Col7,
			Col8, Col9, Col10, Col11, Col12,
			Col13, Col14, Col15, Col16, Col17,	
			Col18, Col19, Col20, Col21, Col22, 
			Col23, Col24, Col25, Col26, Col27, 
			Col28, Col29, col30, col31,	Col32, 
			Col33, Col34, Col35, Col36, col37, Col38, Col39
		)
		SELECT  DISTINCT
			2,
			E.Student_File_ID,
			[School_Code] =	
				CASE 
					WHEN H.Scheme_Code = 'VSS' AND H.Subsidize_Code = 'VNIAMMR' THEN NULL 
					ELSE H.School_Code
				END,
			CASE WHEN H.Scheme_Code = 'RVP' THEN RCH.Homename_Eng ELSE S.Name_Eng END AS SchoolName_EN,
			CASE WHEN H.Scheme_Code = 'RVP' THEN RCH.Homename_Chi ELSE S.Name_Chi END AS SchoolName_TC,
			CASE WHEN H.Scheme_Code = 'RVP' THEN RCH.Address_Eng ELSE S.Address_Eng END AS SchoolAddr_EN,
			CASE WHEN H.Scheme_Code = 'RVP' THEN RCH.Address_Chi ELSE S.Address_Chi END AS SchoolAddr_TC,
			'',
			H.SP_ID,
			CONVERT(VARCHAR(MAX), DecryptByKey(SP.Encrypt_Field2)) AS SPName_EN,
			CONVERT(NVARCHAR(MAX), DecryptByKey(SP.Encrypt_Field3)) AS SPName_Chi,
			[Practice_Name] = P.Practice_Name + '(' + CONVERT(VARCHAR(10), Practice_Display_Seq) +')',
			[Practice_Name_Chi] =
				CASE 
					WHEN P.Practice_Name_Chi IS NULL THEN '' 
					WHEN P.Practice_Name_Chi = '' THEN '' 
					ELSE P.Practice_Name_Chi + '(' + CONVERT(VARCHAR(10), Practice_Display_Seq) +')'
				END,
			'',
			[Headings] = 
				CASE 
					WHEN H.Scheme_Code = 'VSS' AND H.Subsidize_Code = 'VNIAMMR' THEN NULL
					WHEN H.Dose = '2NDDOSE' THEN 
						(SELECT Data_value FROM StaticData WITH (NOLOCK) WHERE Column_Name = 'StudentFileDoseToInject' AND Item_No = '2NDDOSE')
					ELSE
						(SELECT Data_value FROM StaticData WITH (NOLOCK) WHERE Column_Name = 'StudentFileDoseToInject' AND Item_No = '1STDOSE')
				END,
			[Service_Receive_Dtm] = 
				CASE
					WHEN H.Scheme_Code = 'VSS' AND H.Subsidize_Code = 'VNIAMMR' THEN NULL
					ELSE FORMAT(H.Service_Receive_Dtm, 'dd MMM yyyy')
				END,
			[Final_Checking_Report_Generation_Date] = FORMAT(Final_Checking_Report_Generation_Date, 'dd MMM yyyy'),
			[Service_Receive_Dtm_2] = 
				CASE 
					WHEN H.Scheme_Code = 'RVP' THEN '' 
					WHEN H.Scheme_Code = 'VSS' THEN NULL
					WHEN H.Service_Receive_Dtm_2 IS NULL THEN 'N/A'
					ELSE FORMAT(H.Service_Receive_Dtm_2, 'dd MMM yyyy')
				END,
			[Final_Checking_Report_Generation_Date_2] =
				CASE 
					WHEN H.Scheme_Code = 'RVP' THEN '' 
					WHEN H.Scheme_Code = 'VSS' THEN NULL
					WHEN H.Final_Checking_Report_Generation_Date_2 IS NULL THEN 'N/A'
					ELSE FORMAT(H.Final_Checking_Report_Generation_Date_2, 'dd MMM yyyy')
				END,
			[Scheme] = RTRIM(SC.Display_Code),

			-- If RVP + QIV THEN display 'QIV 20XX/XX' Else display 'QIV-C 2018/19','23vPPV'
			[Subsidy] = 
				CASE 
					WHEN H.Scheme_Code = 'RVP' AND SUB.vaccine_Type = 'QIV' THEN
						RTRIM(SUB.vaccine_Type) + ' ' +  RTRIM(VS.Season_Desc)
					ELSE
						sgc.Display_Code_For_Claim
					END,
			[DOSE] = 
				CASE 
					WHEN H.Scheme_Code = 'VSS' AND H.Subsidize_Code = 'VNIAMMR' THEN 
						CASE 
							WHEN H.DOSE = '1STDOSE' THEN @1stDose
							WHEN H.DOSE = '2NDDOSE' THEN @2ndDose
							WHEN H.DOSE = '3RDDOSE' THEN @3rdDose
							ELSE '' 
						END
					ELSE SD.Data_Value 
				END,
			ET.ClassCount,
			ET.StudentCount,
			'',
			ET.VaccineConnectionFail,
			ET.VaccineDemographicsNotMatch,
			'',
			AvailableToInjectCount,

			-- No. of client for dose
			[AvailableToInjectCount_Item1] = 
				CASE	
					WHEN @AvailableDose_Item1 = @OnlyDose THEN 
						CASE WHEN H.Dose = '2NDDOSE' THEN 'N/A' ELSE CAST(AvailableToInjectCount_OnlyDose AS VARCHAR(10)) END
					WHEN @AvailableDose_Item1 = @1stDose THEN 
						CASE WHEN H.Dose = '2NDDOSE' THEN 'N/A' ELSE CAST(AvailableToInjectCount_1stDose AS VARCHAR(10)) END
					WHEN @AvailableDose_Item1 = @2ndDose THEN 
						CAST(AvailableToInjectCount_2ndDose AS VARCHAR(10))
					ELSE
						''
				END,
			[AvailableToInjectCount_Item2] = 
				CASE
					WHEN @AvailableDose_Item2 = @1stDose THEN 
						CASE WHEN H.Dose = '2NDDOSE' THEN 'N/A' ELSE CAST(AvailableToInjectCount_1stDose AS VARCHAR(10)) END
					WHEN @AvailableDose_Item2 = @2ndDose THEN 
						CAST(AvailableToInjectCount_2ndDose AS VARCHAR(10))
					ELSE
						''
				END,
			[AvailableToInjectCount_Item3] = 
				CASE
					WHEN @AvailableDose_Item3 = @2ndDose THEN 
						CAST(AvailableToInjectCount_2ndDose AS VARCHAR(10))
					WHEN @AvailableDose_Item3 = @3rdDose THEN 
						CAST(AvailableToInjectCount_3rdDose AS VARCHAR(10))
					ELSE
						''
				END,

			NotInjectCount,
			[Checking Date_1] =
				CASE 
					WHEN H.Scheme_Code = 'VSS' THEN NULL
					ELSE ''
				END,
			[Checking Date_2] =
				CASE 
					WHEN H.Scheme_Code = 'RVP' THEN '' 
					WHEN H.Scheme_Code = 'VSS' THEN NULL
					WHEN H.Service_Receive_Dtm_2 IS NULL THEN NULL
					WHEN @Visit = 1 THEN NULL
					WHEN @Visit = 2 THEN NULL
					ELSE NULL
				END,
			[Checking Date_3] = 
				CASE 
					WHEN H.Scheme_Code = 'RVP' THEN NULL
					WHEN H.Scheme_Code = 'VSS' THEN NULL
					WHEN H.Service_Receive_Dtm_2 IS NULL THEN '1st Vaccination Date'
					WHEN @Visit = 1 THEN '1st Vaccination Date'
					WHEN @Visit = 2 THEN '2nd Vaccination Date'
					ELSE NULL
				END,
			'',
			CONVERT(varchar, GETDATE(), 111) + ' ' + CONVERT(VARCHAR(5), GETDATE(), 108), '', ''
		FROM StudentFileEntry E
		INNER JOIN StudentFileHeader H
		ON E.Student_File_ID = H.Student_File_ID
		LEFT JOIN School S
		ON S.School_Code = H.School_Code
			AND S.Scheme_Code = H.Scheme_Code
		LEFT JOIN RVPHomeList RCH
		ON RCH.RCH_code = H.School_Code
			--AND RCH.RCH_code = H.Scheme_Code
		INNER JOIN ServiceProvider SP
		ON SP.SP_ID = H.SP_ID
		INNER JOIN (
			SELECT Student_File_ID, 
				MAX(Student_Seq) AS StudentCount,
				COUNT(DISTINCT Class_Name) AS ClassCount,
				SUM(
					CASE WHEN ISNULL(Entitle_Inject, '') = 'Y' THEN 1 ELSE 0 END
				) AS AvailableToInjectCount,
				SUM(
					CASE WHEN ISNULL(Entitle_Inject, '') = 'Y' AND
							  ISNULL(Entitle_ONLYDOSE, '') = 'Y' THEN 1 ELSE 0 END
				) AS AvailableToInjectCount_OnlyDose,
				SUM(
					CASE WHEN ISNULL(Entitle_Inject, '') = 'Y' AND
							  ISNULL(Entitle_1STDOSE, '') = 'Y' THEN 1 ELSE 0 END
				) AS AvailableToInjectCount_1stDose,
				SUM(
					CASE WHEN ISNULL(Entitle_Inject, '') = 'Y' AND
							  ISNULL(Entitle_2NDDOSE, '') = 'Y' THEN 1 ELSE 0 END
				) AS AvailableToInjectCount_2ndDose,
				SUM(
					CASE WHEN ISNULL(Entitle_Inject, '') = 'Y' AND
							  ISNULL(Entitle_3RDDOSE, '') = 'Y' THEN 1 ELSE 0 END
				) AS AvailableToInjectCount_3rdDose,
				SUM(
					CASE WHEN ISNULL(Reject_Injection, '') = 'Y' THEN 1 ELSE 0 END
				) AS NotInjectCount,
				SUM(
					CASE WHEN ISNULL(Injected, '') = 'Y' THEN 1 ELSE 0 END
				) AS WithInjectRecordCount,
				SUM(
					CASE WHEN ISNULL(Transaction_ID, '') <> '' THEN 1 ELSE 0 END
				) AS CreatedClaimCount,
				SUM(
					CASE WHEN ISNULL(Transaction_ID, '') = '' THEN 1 ELSE 0 END
				) AS NotCreatedClaimCount,
				SUM(
					CASE WHEN Ext_Ref_Status LIKE '_C_' OR Ext_Ref_Status LIKE '_S_' OR DH_Vaccine_Ref_Status LIKE '_C_'OR DH_Vaccine_Ref_Status LIKE '_S_' THEN 1 ELSE 0 END
				) AS VaccineConnectionFail,
				SUM(
					CASE WHEN Ext_Ref_Status LIKE '_P_' OR DH_Vaccine_Ref_Status LIKE '_P_' THEN 1 ELSE 0 END
				) AS VaccineDemographicsNotMatch
			FROM StudentFileEntry
			WHERE Student_File_ID= @Input_Student_File_ID
			GROUP BY Student_File_ID
		) AS ET
		ON ET.Student_File_ID = E.Student_File_ID
		INNER JOIN Practice P
		ON P.SP_ID = H.SP_ID
		AND P.Display_Seq = H.Practice_Display_Seq
		LEFT JOIN StaticData SD
		ON H.Dose = SD.Item_No
		AND SD.Column_Name = 'StudentFileDoseToInject'
		LEFT JOIN SubsidizeGroupClaim SGC
		ON SGC.Scheme_Code = H.Scheme_Code
			AND sgc.Scheme_Seq = H.Scheme_Seq
			AND sgc.Subsidize_Code = H.Subsidize_Code
		INNER JOIN Subsidize SUB
		ON H.Subsidize_Code = SUB.Subsidize_Code
		INNER JOIN VaccineSeason VS
		ON VS.Scheme_Code = H.Scheme_Code
			AND VS.Scheme_Seq = H.Scheme_Seq
			AND VS.Subsidize_Item_Code = SUB.Subsidize_Item_Code
		INNER JOIN SchemeClaim SC
		ON SC.Scheme_Code = H.Scheme_Code
		WHERE E.Student_File_ID = @Input_Student_File_ID

		-- 2nd Dose Column
		INSERT INTO #BatchTT
		(
			R1,
			Col1, Col2, Col3, Col4, Col5, Col6, Col7,
			Col8, Col9, Col10, Col11, Col12,
			Col13, Col14, Col15, Col16, Col17,	
			Col18, Col19, Col20, Col21, Col22, 
			Col23, Col24, Col25, Col26, Col27, 
			Col28, Col29, col30, col31,	Col32, 
			Col33, Col34, Col35, Col36, Col37, 
			Col38, Col39
		)
		SELECT  DISTINCT
			3,
			'',
			'',
			'' AS SchoolName_EN,
			'' AS SchoolName_TC,
			'' AS SchoolAddr_EN,
			'' AS SchoolAddr_TC,
			'',
			'',
			'' AS SPName_EN,
			'' AS SPName_Chi,
			'' AS Practice_Name,
			'' AS Practice_Name_Chi,
			'',
			[Headings] = 
				CASE 
					WHEN H.Scheme_Code = 'VSS' AND RTRIM(H.Subsidize_Code) = 'VNIAMMR' THEN '' 
					WHEN Dose = '2NDDOSE' THEN '' 
					ELSE (SELECT Data_value FROM StaticData WITH (NOLOCK) WHERE Column_Name = 'StudentFileDoseToInject' AND Item_No = '2NDDOSE') 
				END,
			[Service_Receive_Dtm] = 
				CASE 
					WHEN H.Scheme_Code = 'VSS' AND RTRIM(H.Subsidize_Code) = 'VNIAMMR' THEN ''
					WHEN Service_Receive_Dtm_2ndDose IS NOT NULL THEN FORMAT(Service_Receive_Dtm_2ndDose, 'dd MMM yyyy') 
					ELSE 
						CASE 
							WHEN Dose = '2NDDOSE' THEN '' 
							ELSE 'N/A' 
						END
				END, 
			[Final_Checking_Report_Generation_Date] =
				CASE 
					WHEN H.Scheme_Code = 'VSS' AND RTRIM(H.Subsidize_Code) = 'VNIAMMR' THEN ''
					WHEN Final_Checking_Report_Generation_Date_2ndDose IS NOT NULL THEN FORMAT(Final_Checking_Report_Generation_Date_2ndDose, 'dd MMM yyyy') 
					ELSE
						CASE 
							WHEN Dose = '2NDDOSE' THEN '' 
							ELSE 'N/A'
						END 
				END, 
			[Service_Receive_Dtm_2] = 
				CASE 
					WHEN H.Scheme_Code = 'VSS' AND RTRIM(H.Subsidize_Code) = 'VNIAMMR' THEN ''
					WHEN H.Scheme_Code = 'RVP' THEN '' 
					WHEN H.Service_Receive_Dtm_2ndDose_2 IS NOT NULL THEN FORMAT(H.Service_Receive_Dtm_2ndDose_2, 'dd MMM yyyy')
					ELSE
						CASE 
							WHEN Dose = '2NDDOSE' THEN '' 
							ELSE 'N/A'
						END 
				END,
			[Final_Checking_Report_Generation_Date_2] =
				CASE 
					WHEN H.Scheme_Code = 'VSS' AND RTRIM(H.Subsidize_Code) = 'VNIAMMR' THEN ''
					WHEN H.Scheme_Code = 'RVP' THEN '' 
					WHEN H.Final_Checking_Report_Generation_Date_2ndDose_2 IS NOT NULL THEN FORMAT(H.Final_Checking_Report_Generation_Date_2ndDose_2, 'dd MMM yyyy')
					ELSE
						CASE 
							WHEN Dose = '2NDDOSE' THEN '' 
							ELSE 'N/A'
						END 
				END,
			'',
			'',
			'',
			'',
			'',
			'',
			'',
			'',
			'',
			'',
			'',
			'',
			'', '', '',
			'', '', '', '', '', ''
		FROM StudentFileHeader H
		WHERE Student_File_ID= @Input_Student_File_ID
			

	END
	ELSE IF @File_ID = 'EHSVF002'
	BEGIN
		INSERT INTO #BatchTT
		(
			R1,
			Col1, Col2, Col3, Col4, Col5, Col6, Col7,
			Col8, Col9, Col10, Col11, Col12,
			Col13, Col14, Col15, Col16, Col17,	
			Col18, Col19, Col20, Col21, Col22, 
			Col23, Col24, Col25, Col26, Col27, 
			Col28, Col29, col30, col31, col32, Col33
		)
		SELECT  DISTINCT
			2,
			E.Student_File_ID,
			[School_Code] =	
				CASE 
					WHEN H.Scheme_Code = 'VSS' AND H.Subsidize_Code = 'VNIAMMR' THEN NULL 
					ELSE H.School_Code
				END,
			CASE WHEN H.Scheme_Code = 'RVP' THEN RCH.Homename_Eng ELSE S.Name_Eng END AS SchoolName_EN,
			CASE WHEN H.Scheme_Code = 'RVP' THEN RCH.Homename_Chi ELSE S.Name_Chi END AS SchoolName_TC,
			CASE WHEN H.Scheme_Code = 'RVP' THEN RCH.Address_Eng ELSE S.Address_Eng END AS SchoolAddr_EN,
			CASE WHEN H.Scheme_Code = 'RVP' THEN RCH.Address_Chi ELSE S.Address_Chi END AS SchoolAddr_TC,
			'',
			H.SP_ID,
			CONVERT(VARCHAR(MAX), DecryptByKey(SP.Encrypt_Field2)) AS SPName_EN,
			CONVERT(NVARCHAR(MAX), DecryptByKey(SP.Encrypt_Field3)) AS SPName_Chi,
			[Practice_Name] = P.Practice_Name + '(' + CONVERT(VARCHAR(10), Practice_Display_Seq) +')',
			[Practice_Name_Chi] =
				CASE 
					WHEN P.Practice_Name_Chi IS NULL THEN '' 
					WHEN P.Practice_Name_Chi = '' THEN '' 
					ELSE P.Practice_Name_Chi + '(' + CONVERT(VARCHAR(10), Practice_Display_Seq) +')'
				END,
			'',
			[Headings] = 
				CASE 
					WHEN H.Scheme_Code = 'VSS' AND H.Subsidize_Code = 'VNIAMMR' THEN NULL
					WHEN H.Dose = '2NDDOSE' THEN 
						(SELECT Data_value FROM StaticData WITH (NOLOCK) WHERE Column_Name = 'StudentFileDoseToInject' AND Item_No = '2NDDOSE')
					ELSE
						(SELECT Data_value FROM StaticData WITH (NOLOCK) WHERE Column_Name = 'StudentFileDoseToInject' AND Item_No = '1STDOSE')
				END,
			[Service_Receive_Dtm] = 
				CASE
					WHEN H.Scheme_Code = 'VSS' AND H.Subsidize_Code = 'VNIAMMR' THEN NULL
					ELSE FORMAT(H.Service_Receive_Dtm, 'dd MMM yyyy')
				END,
			[Final_Checking_Report_Generation_Date] = FORMAT(Final_Checking_Report_Generation_Date, 'dd MMM yyyy'),
			[Service_Receive_Dtm_2] = 
				CASE 
					WHEN H.Scheme_Code = 'RVP' THEN '' 
					WHEN H.Scheme_Code = 'VSS' THEN NULL
					WHEN H.Service_Receive_Dtm_2 IS NULL THEN 'N/A'
					ELSE FORMAT(H.Service_Receive_Dtm_2, 'dd MMM yyyy')
				END,
			[Final_Checking_Report_Generation_Date_2] =
				CASE 
					WHEN H.Scheme_Code = 'RVP' THEN '' 
					WHEN H.Scheme_Code = 'VSS' THEN NULL
					WHEN H.Final_Checking_Report_Generation_Date_2 IS NULL THEN 'N/A'
					ELSE FORMAT(H.Final_Checking_Report_Generation_Date_2, 'dd MMM yyyy')
				END,
			[Scheme] = RTRIM(SC.Display_Code),

			-- If RVP + QIV THEN display 'QIV 20XX/XX' Else display 'QIV-C 2018/19','23vPPV'
			[Subsidy] = 
				CASE 
					WHEN H.Scheme_Code = 'RVP' AND SUB.vaccine_Type = 'QIV' THEN
						RTRIM(SUB.vaccine_Type) + ' ' +  RTRIM(VS.Season_Desc)
					ELSE
						sgc.Display_Code_For_Claim
					END,
			[DOSE] = 
				CASE 
					WHEN H.Scheme_Code = 'VSS' AND H.Subsidize_Code = 'VNIAMMR' THEN 
						CASE 
							WHEN H.DOSE = '1STDOSE' THEN @1stDose
							WHEN H.DOSE = '2NDDOSE' THEN @2ndDose
							WHEN H.DOSE = '3RDDOSE' THEN @3rdDose
							ELSE '' 
						END
					ELSE SD.Data_Value 
				END,
			ET.ClassCount,
			ET.StudentCount,
			--'',
			--ET.VaccineConnectionFail,
			--ET.VaccineDemographicsNotMatch,
			'',
			AvailbleToInjectCount,
			NotInjectCount,
			[Checking Date_1] =
				CASE 
					WHEN H.Scheme_Code = 'VSS' THEN NULL
					ELSE ''
				END,
			[Checking Date_2] =
				CASE 
					WHEN H.Scheme_Code = 'RVP' THEN '' 
					WHEN H.Scheme_Code = 'VSS' THEN NULL
					WHEN H.Service_Receive_Dtm_2 IS NULL THEN NULL
					WHEN @Visit = 1 THEN NULL
					WHEN @Visit = 2 THEN NULL
					ELSE NULL
				END,
			[Checking Date_3] = 
				CASE 
					WHEN H.Scheme_Code = 'RVP' THEN NULL
					WHEN H.Scheme_Code = 'VSS' THEN NULL
					WHEN H.Service_Receive_Dtm_2 IS NULL THEN '1st Vaccination Date'
					WHEN @Visit = 1 THEN '1st Vaccination Date'
					WHEN @Visit = 2 THEN '2nd Vaccination Date'
					ELSE NULL
				END,
			'',
			CONVERT(varchar, GETDATE(), 111) + ' ' + CONVERT(VARCHAR(5), GETDATE(), 108), '', ''
		FROM StudentFileEntry E
		INNER JOIN StudentFileHeader H
		ON E.Student_File_ID = H.Student_File_ID
		LEFT JOIN School S
		ON S.School_Code = H.School_Code
			AND S.Scheme_Code = H.Scheme_Code
		LEFT JOIN RVPHomeList RCH
		ON RCH.RCH_code = H.School_Code
			--AND RCH.RCH_code = H.Scheme_Code
		INNER JOIN ServiceProvider SP
		ON SP.SP_ID = H.SP_ID
		INNER JOIN (
			SELECT Student_File_ID, 
				MAX(Student_Seq) AS StudentCount,
				COUNT(DISTINCT Class_Name) AS ClassCount,
				SUM(
					CASE WHEN ISNULL(Entitle_Inject, '') = 'Y' THEN 1 ELSE 0 END
				) AS AvailbleToInjectCount,
				SUM(
					CASE WHEN ISNULL(Entitle_Inject, '') = 'Y' AND
							  ISNULL(Entitle_ONLYDOSE, '') = 'Y' THEN 1 ELSE 0 END
				) AS AvailableToInjectCount_OnlyDose,
				SUM(
					CASE WHEN ISNULL(Entitle_Inject, '') = 'Y' AND
							  ISNULL(Entitle_1STDOSE, '') = 'Y' THEN 1 ELSE 0 END
				) AS AvailableToInjectCount_1stDose,
				SUM(
					CASE WHEN ISNULL(Entitle_Inject, '') = 'Y' AND
							  ISNULL(Entitle_2NDDOSE, '') = 'Y' THEN 1 ELSE 0 END
				) AS AvailableToInjectCount_2ndDose,
				SUM(
					CASE WHEN ISNULL(Reject_Injection, '') = 'Y' THEN 1 ELSE 0 END
				) AS NotInjectCount,
				SUM(
					CASE WHEN ISNULL(Injected, '') = 'Y' THEN 1 ELSE 0 END
				) AS WithInjectRecordCount,
				SUM(
					CASE WHEN ISNULL(Transaction_ID, '') <> '' THEN 1 ELSE 0 END
				) AS CreatedClaimCount,
				SUM(
					CASE WHEN ISNULL(Transaction_ID, '') = '' THEN 1 ELSE 0 END
				) AS NotCreatedClaimCount,
				SUM(
					CASE WHEN Ext_Ref_Status LIKE '_C_' OR Ext_Ref_Status LIKE '_S_' OR DH_Vaccine_Ref_Status LIKE '_C_'OR DH_Vaccine_Ref_Status LIKE '_S_' THEN 1 ELSE 0 END
				) AS VaccineConnectionFail,
				SUM(
					CASE WHEN Ext_Ref_Status LIKE '_P_' OR DH_Vaccine_Ref_Status LIKE '_P_' THEN 1 ELSE 0 END
				) AS VaccineDemographicsNotMatch
			FROM StudentFileEntry
			WHERE Student_File_ID= @Input_Student_File_ID
			GROUP BY Student_File_ID
		) AS ET
		ON ET.Student_File_ID = E.Student_File_ID
		INNER JOIN Practice P
		ON P.SP_ID = H.SP_ID
		AND P.Display_Seq = H.Practice_Display_Seq
		LEFT JOIN StaticData SD
		ON H.Dose = SD.Item_No
		AND SD.Column_Name = 'StudentFileDoseToInject'
		LEFT JOIN SubsidizeGroupClaim SGC
		ON SGC.Scheme_Code = H.Scheme_Code
			AND sgc.Scheme_Seq = H.Scheme_Seq
			AND sgc.Subsidize_Code = H.Subsidize_Code
		INNER JOIN Subsidize SUB
		ON H.Subsidize_Code = SUB.Subsidize_Code
		INNER JOIN VaccineSeason VS
		ON VS.Scheme_Code = H.Scheme_Code
			AND VS.Scheme_Seq = H.Scheme_Seq
			AND VS.Subsidize_Item_Code = SUB.Subsidize_Item_Code
		INNER JOIN SchemeClaim SC
		ON SC.Scheme_Code = H.Scheme_Code
		WHERE E.Student_File_ID = @Input_Student_File_ID

		-- 2nd Dose Column
		INSERT INTO #BatchTT
		(
			R1,
			Col1, Col2, Col3, Col4, Col5, Col6, Col7,
			Col8, Col9, Col10, Col11, Col12,
			Col13, Col14, Col15, Col16, Col17,	
			Col18, Col19, Col20, Col21, Col22, 
			Col23, Col24, Col25, Col26, Col27, 
			Col28, Col29, col30, col31, col32, col33
		)
		SELECT  DISTINCT
			3,
			'',
			'',
			'' AS SchoolName_EN,
			'' AS SchoolName_TC,
			'' AS SchoolAddr_EN,
			'' AS SchoolAddr_TC,
			'',
			'',
			'' AS SPName_EN,
			'' AS SPName_Chi,
			'' AS Practice_Name,
			'' AS Practice_Name_Chi,
			'',
			[Headings] = 
				CASE 
					WHEN H.Scheme_Code = 'VSS' AND RTRIM(H.Subsidize_Code) = 'VNIAMMR' THEN '' 
					WHEN Dose = '2NDDOSE' THEN '' 
					ELSE (SELECT Data_value FROM StaticData WITH (NOLOCK) WHERE Column_Name = 'StudentFileDoseToInject' AND Item_No = '2NDDOSE') 
				END,
			[Service_Receive_Dtm] = 
				CASE 
					WHEN H.Scheme_Code = 'VSS' AND RTRIM(H.Subsidize_Code) = 'VNIAMMR' THEN ''
					WHEN Service_Receive_Dtm_2ndDose IS NOT NULL THEN FORMAT(Service_Receive_Dtm_2ndDose, 'dd MMM yyyy') 
					ELSE 
						CASE 
							WHEN Dose = '2NDDOSE' THEN '' 
							ELSE 'N/A' 
						END
				END, 
			[Final_Checking_Report_Generation_Date] =
				CASE 
					WHEN H.Scheme_Code = 'VSS' AND RTRIM(H.Subsidize_Code) = 'VNIAMMR' THEN ''
					WHEN Final_Checking_Report_Generation_Date_2ndDose IS NOT NULL THEN FORMAT(Final_Checking_Report_Generation_Date_2ndDose, 'dd MMM yyyy') 
					ELSE
						CASE 
							WHEN Dose = '2NDDOSE' THEN '' 
							ELSE 'N/A'
						END 
				END, 
			[Service_Receive_Dtm_2] = 
				CASE 
					WHEN H.Scheme_Code = 'VSS' AND RTRIM(H.Subsidize_Code) = 'VNIAMMR' THEN ''
					WHEN H.Scheme_Code = 'RVP' THEN '' 
					WHEN H.Service_Receive_Dtm_2ndDose_2 IS NOT NULL THEN FORMAT(H.Service_Receive_Dtm_2ndDose_2, 'dd MMM yyyy')
					ELSE
						CASE 
							WHEN Dose = '2NDDOSE' THEN '' 
							ELSE 'N/A'
						END 
				END,
			[Final_Checking_Report_Generation_Date_2] =
				CASE 
					WHEN H.Scheme_Code = 'VSS' AND RTRIM(H.Subsidize_Code) = 'VNIAMMR' THEN ''
					WHEN H.Scheme_Code = 'RVP' THEN '' 
					WHEN H.Final_Checking_Report_Generation_Date_2ndDose_2 IS NOT NULL THEN FORMAT(H.Final_Checking_Report_Generation_Date_2ndDose_2, 'dd MMM yyyy')
					ELSE
						CASE 
							WHEN Dose = '2NDDOSE' THEN '' 
							ELSE 'N/A'
						END 
				END,
			'',
			'',
			'',
			'',
			'',
			--'',
			--'',
			--'',
			'',
			'',
			'',
			'',
			'', '', '', '', '', ''
		FROM StudentFileHeader H
		WHERE Student_File_ID= @Input_Student_File_ID
	END	
	ELSE IF @File_ID = 'EHSVF003'
	BEGIN
		INSERT INTO #BatchTT
		(
			R1,
			Col1, Col2, Col3, Col4, Col5, 
			Col6, Col7, Col8, Col9,	Col10,
			Col11, Col12, Col13, Col14, Col15,		
			Col16, Col17, Col18, Col19, Col20,
			Col21, Col22, Col23, Col24, Col25,	
			Col26, Col27, Col28, Col29, Col30,
			Col31, Col32, Col33, Col34, Col35,
			Col36
		)
		SELECT  DISTINCT
			2,
			E.Student_File_ID,
			[School_Code] =	
				CASE 
					WHEN H.Scheme_Code = 'VSS' AND H.Subsidize_Code = 'VNIAMMR' THEN NULL 
					ELSE H.School_Code
				END,
			CASE WHEN H.Scheme_Code = 'RVP' THEN RCH.Homename_Eng ELSE S.Name_Eng END AS SchoolName_EN,
			CASE WHEN H.Scheme_Code = 'RVP' THEN RCH.Homename_Chi ELSE S.Name_Chi END AS SchoolName_TC,
			CASE WHEN H.Scheme_Code = 'RVP' THEN RCH.Address_Eng ELSE S.Address_Eng END AS SchoolAddr_EN,
			CASE WHEN H.Scheme_Code = 'RVP' THEN RCH.Address_Chi ELSE S.Address_Chi END AS SchoolAddr_TC,
			'', 
			H.SP_ID,
			CONVERT(VARCHAR(MAX), DecryptByKey(SP.Encrypt_Field2)) AS SPName_EN,
			CONVERT(NVARCHAR(MAX), DecryptByKey(SP.Encrypt_Field3)) AS SPName_Chi,
			[Practice_Name] = P.Practice_Name + '(' + CONVERT(VARCHAR(10), Practice_Display_Seq) +')',
			[Practice_Name_Chi] =
				CASE 
					WHEN P.Practice_Name_Chi IS NULL THEN '' 
					WHEN P.Practice_Name_Chi = '' THEN '' 
					ELSE P.Practice_Name_Chi + '(' + CONVERT(VARCHAR(10), Practice_Display_Seq) +')'
				END,
			'',
			[Headings] = 
				CASE 
					WHEN H.Scheme_Code = 'VSS' AND H.Subsidize_Code = 'VNIAMMR' THEN NULL
					WHEN H.Dose = '2NDDOSE' THEN 
						(SELECT Data_value FROM StaticData WITH (NOLOCK) WHERE Column_Name = 'StudentFileDoseToInject' AND Item_No = '2NDDOSE')
					ELSE
						(SELECT Data_value FROM StaticData WITH (NOLOCK) WHERE Column_Name = 'StudentFileDoseToInject' AND Item_No = '1STDOSE')
				END,
			[Service_Receive_Dtm] = 
				CASE
					WHEN H.Scheme_Code = 'VSS' AND H.Subsidize_Code = 'VNIAMMR' THEN NULL
					ELSE FORMAT(H.Service_Receive_Dtm, 'dd MMM yyyy')
				END,
			[Final_Checking_Report_Generation_Date] = FORMAT(Final_Checking_Report_Generation_Date, 'dd MMM yyyy'),
			[Service_Receive_Dtm_2] = 
				CASE 
					WHEN H.Scheme_Code = 'RVP' THEN '' 
					WHEN H.Scheme_Code = 'VSS' THEN NULL
					WHEN H.Service_Receive_Dtm_2 IS NULL THEN 'N/A'
					ELSE FORMAT(H.Service_Receive_Dtm_2, 'dd MMM yyyy')
				END,
			[Final_Checking_Report_Generation_Date_2] =
				CASE 
					WHEN H.Scheme_Code = 'RVP' THEN '' 
					WHEN H.Scheme_Code = 'VSS' THEN NULL
					WHEN H.Final_Checking_Report_Generation_Date_2 IS NULL THEN 'N/A'
					ELSE FORMAT(H.Final_Checking_Report_Generation_Date_2, 'dd MMM yyyy')
				END,
			[Scheme] = RTRIM(SC.Display_Code),

			-- If RVP + QIV THEN display 'QIV 20XX/XX' Else display 'QIV-C 2018/19','23vPPV'
			[Subsidy] = 
				CASE 
					WHEN H.Scheme_Code = 'RVP' AND SUB.vaccine_Type = 'QIV' THEN
						RTRIM(SUB.vaccine_Type) + ' ' +  RTRIM(VS.Season_Desc)
					ELSE
						sgc.Display_Code_For_Claim
					END,
			[DOSE] = 
				CASE 
					WHEN H.Scheme_Code = 'VSS' AND H.Subsidize_Code = 'VNIAMMR' THEN 
						CASE 
							WHEN H.DOSE = '1STDOSE' THEN @1stDose
							WHEN H.DOSE = '2NDDOSE' THEN @2ndDose
							WHEN H.DOSE = '3RDDOSE' THEN @3rdDose
							ELSE '' 
						END
					ELSE SD.Data_Value 
				END,
			ET.ClassCount,
			ET.StudentCount,
			'',
			ET.VaccineConnectionFail,
			ET.VaccineDemographicsNotMatch,
			'',
			AvailbleToInjectCount,
			NotInjectCount,
			'',
			FORMAT(H.Upload_Dtm, 'dd MMM yyyy') AS Upload_Dtm, 
			WithInjectRecordCount,
			CreatedClaimCount,
			NotCreatedClaimCount,
			'', CONVERT(varchar, GETDATE(), 111) + ' ' + CONVERT(VARCHAR(5), GETDATE(), 108)
		FROM StudentFileEntry E
		INNER JOIN StudentFileHeader H
		ON E.Student_File_ID = H.Student_File_ID
		LEFT JOIN School S
		ON S.School_Code = H.School_Code
			AND S.Scheme_Code = H.Scheme_Code
		LEFT JOIN RVPHomeList RCH
		ON RCH.RCH_code = H.School_Code
			--AND RCH.RCH_code = H.Scheme_Code
		INNER JOIN ServiceProvider SP
		ON SP.SP_ID = H.SP_ID
		INNER JOIN (
			SELECT Student_File_ID, 
				MAX(Student_Seq) AS StudentCount,
				COUNT(DISTINCT Class_Name) AS ClassCount,
				SUM(
					CASE WHEN ISNULL(Entitle_Inject, '') = 'Y' THEN 1 ELSE 0 END
				) AS AvailbleToInjectCount,
				SUM(
					CASE WHEN ISNULL(Entitle_Inject, '') = 'Y' AND
							  ISNULL(Entitle_ONLYDOSE, '') = 'Y' THEN 1 ELSE 0 END
				) AS AvailableToInjectCount_OnlyDose,
				SUM(
					CASE WHEN ISNULL(Entitle_Inject, '') = 'Y' AND
							  ISNULL(Entitle_1STDOSE, '') = 'Y' THEN 1 ELSE 0 END
				) AS AvailableToInjectCount_1stDose,
				SUM(
					CASE WHEN ISNULL(Entitle_Inject, '') = 'Y' AND
							  ISNULL(Entitle_2NDDOSE, '') = 'Y' THEN 1 ELSE 0 END
				) AS AvailableToInjectCount_2ndDose,
				SUM(
					CASE WHEN ISNULL(Reject_Injection, '') = 'Y' THEN 1 ELSE 0 END
				) AS NotInjectCount,
				SUM(
					CASE WHEN ISNULL(Injected, '') = 'Y' THEN 1 ELSE 0 END
				) AS WithInjectRecordCount,
				SUM(
					CASE WHEN ISNULL(Transaction_ID, '') <> '' THEN 1 ELSE 0 END
				) AS CreatedClaimCount,
				SUM(
					CASE WHEN ISNULL(Transaction_ID, '') = '' AND Transaction_Result IS NOT NULL THEN 1 ELSE 0 END
				) AS NotCreatedClaimCount,
				SUM(
					CASE WHEN Ext_Ref_Status LIKE '_C_' OR Ext_Ref_Status LIKE '_S_' OR DH_Vaccine_Ref_Status LIKE '_C_'OR DH_Vaccine_Ref_Status LIKE '_S_' THEN 1 ELSE 0 END
				) AS VaccineConnectionFail,
				SUM(
					CASE WHEN Ext_Ref_Status LIKE '_P_' OR DH_Vaccine_Ref_Status LIKE '_P_' THEN 1 ELSE 0 END
				) AS VaccineDemographicsNotMatch
			FROM StudentFileEntry
			WHERE Student_File_ID= @Input_Student_File_ID
			GROUP BY Student_File_ID
		) AS ET
		ON ET.Student_File_ID = E.Student_File_ID
		INNER JOIN Practice P
		ON P.SP_ID = H.SP_ID
		AND P.Display_Seq = H.Practice_Display_Seq
		LEFT JOIN StaticData SD
		ON H.Dose = SD.Item_No
		AND SD.Column_Name = 'StudentFileDoseToInject'
		LEFT JOIN SubsidizeGroupClaim SGC
		ON SGC.Scheme_Code = H.Scheme_Code
			AND sgc.Scheme_Seq = H.Scheme_Seq
			AND sgc.Subsidize_Code = H.Subsidize_Code
		INNER JOIN Subsidize SUB
		ON H.Subsidize_Code = SUB.Subsidize_Code
		INNER JOIN VaccineSeason VS
		ON VS.Scheme_Code = H.Scheme_Code
			AND VS.Scheme_Seq = H.Scheme_Seq
			AND VS.Subsidize_Item_Code = SUB.Subsidize_Item_Code
		INNER JOIN SchemeClaim SC
		ON SC.Scheme_Code = H.Scheme_Code
		WHERE E.Student_File_ID = @Input_Student_File_ID	


		-- 2nd Dose Column
		INSERT INTO #BatchTT
		(
			R1,
			Col1, Col2, Col3, Col4, Col5, 
			Col6, Col7, Col8, Col9,	Col10,
			Col11, Col12, Col13, Col14, Col15,		
			Col16, Col17, Col18, Col19, Col20,
			Col21, Col22, Col23, Col24, Col25,	
			Col26, Col27, Col28, Col29, Col30,
			Col31, Col32, Col33, Col34, Col35,
			Col36
		)
		SELECT  DISTINCT
			3,
			'',
			'',
			'' AS SchoolName_EN,
			'' AS SchoolName_TC,
			'' AS SchoolAddr_EN,
			'' AS SchoolAddr_TC,
			'',
			'' AS SPName_EN,
			'' AS SPName_Chi,
			'' AS Practice_Name,
			'' AS Practice_Name_Chi,
			'',
			'',
			[Headings] = 
				CASE 
					WHEN H.Scheme_Code = 'VSS' AND RTRIM(H.Subsidize_Code) = 'VNIAMMR' THEN '' 
					WHEN Dose = '2NDDOSE' THEN '' 
					ELSE (SELECT Data_value FROM StaticData WITH (NOLOCK) WHERE Column_Name = 'StudentFileDoseToInject' AND Item_No = '2NDDOSE') 
				END,
			[Service_Receive_Dtm] = 
				CASE 
					WHEN H.Scheme_Code = 'VSS' AND RTRIM(H.Subsidize_Code) = 'VNIAMMR' THEN ''
					WHEN Service_Receive_Dtm_2ndDose IS NOT NULL THEN FORMAT(Service_Receive_Dtm_2ndDose, 'dd MMM yyyy') 
					ELSE 
						CASE 
							WHEN Dose = '2NDDOSE' THEN '' 
							ELSE 'N/A' 
						END
				END, 
			[Final_Checking_Report_Generation_Date] =
				CASE 
					WHEN H.Scheme_Code = 'VSS' AND RTRIM(H.Subsidize_Code) = 'VNIAMMR' THEN ''
					WHEN Final_Checking_Report_Generation_Date_2ndDose IS NOT NULL THEN FORMAT(Final_Checking_Report_Generation_Date_2ndDose, 'dd MMM yyyy') 
					ELSE
						CASE 
							WHEN Dose = '2NDDOSE' THEN '' 
							ELSE 'N/A'
						END 
				END, 
			[Service_Receive_Dtm_2] = 
				CASE 
					WHEN H.Scheme_Code = 'VSS' AND RTRIM(H.Subsidize_Code) = 'VNIAMMR' THEN ''
					WHEN H.Scheme_Code = 'RVP' THEN '' 
					WHEN H.Service_Receive_Dtm_2ndDose_2 IS NOT NULL THEN FORMAT(H.Service_Receive_Dtm_2ndDose_2, 'dd MMM yyyy')
					ELSE
						CASE 
							WHEN Dose = '2NDDOSE' THEN '' 
							ELSE 'N/A'
						END 
				END,
			[Final_Checking_Report_Generation_Date_2] =
				CASE 
					WHEN H.Scheme_Code = 'VSS' AND RTRIM(H.Subsidize_Code) = 'VNIAMMR' THEN ''
					WHEN H.Scheme_Code = 'RVP' THEN '' 
					WHEN H.Final_Checking_Report_Generation_Date_2ndDose_2 IS NOT NULL THEN FORMAT(H.Final_Checking_Report_Generation_Date_2ndDose_2, 'dd MMM yyyy')
					ELSE
						CASE 
							WHEN Dose = '2NDDOSE' THEN '' 
							ELSE 'N/A'
						END 
				END,
			'',
			'',
			'',
			'',
			'',
			'',
			'',
			'',
			'',
			'',
			'',
			'',
			'',
			'',
			'',
			'', '', ''
		FROM StudentFileHeader H
		WHERE Student_File_ID= @Input_Student_File_ID
	END

	ELSE IF @File_ID = 'EHSVF005'
	BEGIN
		INSERT INTO #BatchTT
		(
			R1,
			Col1, Col2, Col3, Col4, Col5, 
			Col6, Col7, Col8, Col9,	Col10,
			Col11, Col12, Col13, Col14, Col15,		
			Col16
		)
		SELECT  DISTINCT
			2,
			E.Student_File_ID,
			[School_Code] =	
				CASE 
					WHEN H.Scheme_Code = 'VSS' AND H.Subsidize_Code = 'VNIAMMR' THEN NULL 
					ELSE H.School_Code
				END,
			CASE WHEN H.Scheme_Code = 'RVP' THEN RCH.Homename_Eng ELSE S.Name_Eng END AS SchoolName_EN,
			CASE WHEN H.Scheme_Code = 'RVP' THEN RCH.Homename_Chi ELSE S.Name_Chi END AS SchoolName_TC,
			CASE WHEN H.Scheme_Code = 'RVP' THEN RCH.Address_Eng ELSE S.Address_Eng END AS SchoolAddr_EN,
			CASE WHEN H.Scheme_Code = 'RVP' THEN RCH.Address_Chi ELSE S.Address_Chi END AS SchoolAddr_TC,
			'',
			H.SP_ID,
			CONVERT(VARCHAR(MAX), DecryptByKey(SP.Encrypt_Field2)) AS SPName_EN,
			CONVERT(NVARCHAR(MAX), DecryptByKey(SP.Encrypt_Field3)) AS SPName_Chi,
			[Practice_Name] = P.Practice_Name + '(' + CONVERT(VARCHAR(10), Practice_Display_Seq) +')',
			[Practice_Name_Chi] =
				CASE 
					WHEN P.Practice_Name_Chi IS NULL THEN '' 
					WHEN P.Practice_Name_Chi = '' THEN '' 
					ELSE P.Practice_Name_Chi + '(' + CONVERT(VARCHAR(10), Practice_Display_Seq) +')'
				END,
			'',
			RTRIM(SC.Display_Code),
			'',
			CONVERT(varchar, GETDATE(), 111) + ' ' + CONVERT(VARCHAR(5), GETDATE(), 108)
		FROM StudentFileEntry E
		INNER JOIN StudentFileHeader H
		ON E.Student_File_ID = H.Student_File_ID
		LEFT JOIN School S
		ON S.School_Code = H.School_Code
			AND S.Scheme_Code = H.Scheme_Code
		LEFT JOIN RVPHomeList RCH
		ON RCH.RCH_code = H.School_Code
			--AND RCH.RCH_code = H.Scheme_Code
		INNER JOIN ServiceProvider SP
		ON SP.SP_ID = H.SP_ID
		INNER JOIN Practice P
		ON P.SP_ID = H.SP_ID
		AND P.Display_Seq = H.Practice_Display_Seq
		INNER JOIN SchemeClaim SC
		ON SC.Scheme_Code = H.Scheme_Code
		WHERE E.Student_File_ID = @Input_Student_File_ID	


		-- 2nd Dose Column
		INSERT INTO #BatchTT
		(
			R1,
			Col1, Col2, Col3, Col4, Col5, 
			Col6, Col7, Col8, Col9,	Col10,
			Col11, Col12, Col13, Col14, Col15,		
			Col16
		)
		SELECT  DISTINCT
			3,
			'',
			'',
			'' AS SchoolName_EN,
			'' AS SchoolName_TC,
			'' AS SchoolAddr_EN,
			'' AS SchoolAddr_TC,
			'',
			'',
			'' AS SPName_EN,
			'' AS SPName_Chi,
			'' AS Practice_Name,
			'' AS Practice_Name_Chi,
			'',
			'',
			'',
			''
		FROM StudentFileHeader
		WHERE Student_File_ID= @Input_Student_File_ID
	END
		
	ELSE IF @File_ID = 'EHSVF006'
	BEGIN
		INSERT INTO #BatchTT
		(
			R1,
			Col1, Col2, Col3, Col4, Col5, Col6, Col7,
			Col8, Col9, Col10, Col11, Col12,
			Col13, Col14, Col15, Col16, Col17,	
			Col18, Col19, Col20, Col21, Col22, Col23, Col24, Col25, Col26, Col27
		)
		SELECT  DISTINCT
			2,
			E.Student_File_ID,
			[School_Code] =	
				CASE 
					WHEN H.Scheme_Code = 'VSS' AND H.Subsidize_Code = 'VNIAMMR' THEN NULL 
					ELSE H.School_Code
				END,
			CASE WHEN H.Scheme_Code = 'RVP' THEN RCH.Homename_Eng ELSE S.Name_Eng END AS SchoolName_EN,
			CASE WHEN H.Scheme_Code = 'RVP' THEN RCH.Homename_Chi ELSE S.Name_Chi END AS SchoolName_TC,
			CASE WHEN H.Scheme_Code = 'RVP' THEN RCH.Address_Eng ELSE S.Address_Eng END AS SchoolAddr_EN,
			CASE WHEN H.Scheme_Code = 'RVP' THEN RCH.Address_Chi ELSE S.Address_Chi END AS SchoolAddr_TC,
			'',
			H.SP_ID,
			CONVERT(VARCHAR(MAX), DecryptByKey(SP.Encrypt_Field2)) AS SPName_EN,
			CONVERT(NVARCHAR(MAX), DecryptByKey(SP.Encrypt_Field3)) AS SPName_Chi,
			[Practice_Name] = P.Practice_Name + '(' + CONVERT(VARCHAR(10), Practice_Display_Seq) +')',
			[Practice_Name_Chi] =
				CASE 
					WHEN P.Practice_Name_Chi IS NULL THEN '' 
					WHEN P.Practice_Name_Chi = '' THEN '' 
					ELSE P.Practice_Name_Chi + '(' + CONVERT(VARCHAR(10), Practice_Display_Seq) +')'
				END,
			'',
			[Headings] = 
				CASE 
					WHEN H.Scheme_Code = 'VSS' AND H.Subsidize_Code = 'VNIAMMR' THEN NULL
					WHEN H.Upload_Precheck = 'Y' THEN '' 
					ELSE 
						CASE 
							WHEN H.Dose = '2NDDOSE' THEN 
								(SELECT Data_value FROM StaticData WITH (NOLOCK) WHERE Column_Name = 'StudentFileDoseToInject' AND Item_No = '2NDDOSE')
							ELSE
								(SELECT Data_value FROM StaticData WITH (NOLOCK) WHERE Column_Name = 'StudentFileDoseToInject' AND Item_No = '1STDOSE')
						END
				END,
			[Service_Receive_Dtm] = 
				CASE 
					WHEN H.Scheme_Code = 'VSS' AND H.Subsidize_Code = 'VNIAMMR' THEN NULL
					WHEN H.Upload_Precheck = 'Y' THEN '' 
					ELSE FORMAT(H.Service_Receive_Dtm, 'dd MMM yyyy')
				END, 
			[Final_Checking_Report_Generation_Date] = CASE WHEN H.Upload_Precheck = 'Y' THEN '' ELSE FORMAT(Final_Checking_Report_Generation_Date, 'dd MMM yyyy') END, 
			[Service_Receive_Dtm_2] = 
				CASE 
					WHEN H.Upload_Precheck = 'Y' THEN '' 
					WHEN H.Scheme_Code = 'RVP' THEN '' 
					WHEN H.Scheme_Code = 'VSS' THEN NULL
					WHEN H.Service_Receive_Dtm_2 IS NULL THEN 'N/A'
					ELSE FORMAT(H.Service_Receive_Dtm_2, 'dd MMM yyyy')
				END,
			[Final_Checking_Report_Generation_Date_2] =
				CASE 
					WHEN H.Upload_Precheck = 'Y' THEN '' 
					WHEN H.Scheme_Code = 'RVP' THEN '' 
					WHEN H.Scheme_Code = 'VSS' THEN NULL
					WHEN H.Final_Checking_Report_Generation_Date_2 IS NULL THEN 'N/A'
					ELSE FORMAT(H.Final_Checking_Report_Generation_Date_2, 'dd MMM yyyy')
				END,			
			[Scheme] = RTRIM(SC.Display_Code),
			[Subsidy] = CASE WHEN H.Upload_Precheck = 'Y' THEN '' ELSE sgc.Display_Code_For_Claim END,
			[DOSE] = 
				CASE 
					WHEN H.Scheme_Code = 'VSS' AND H.Subsidize_Code = 'VNIAMMR' THEN 
						CASE 
							WHEN H.DOSE = '1STDOSE' THEN @1stDose
							WHEN H.DOSE = '2NDDOSE' THEN @2ndDose
							WHEN H.DOSE = '3RDDOSE' THEN @3rdDose
							ELSE '' 
						END
					WHEN H.Upload_Precheck = 'Y' THEN '' 
					ELSE SD.Data_Value 
				END,
			ET.ClassCount,
			ET.StudentCount,
			'',
			CONVERT(varchar, GETDATE(), 111) + ' ' + CONVERT(VARCHAR(5), GETDATE(), 108), '', ''
		FROM StudentFileEntry E
		INNER JOIN StudentFileHeader H
		ON E.Student_File_ID = H.Student_File_ID
		LEFT JOIN School S
		ON S.School_Code = H.School_Code
			AND S.Scheme_Code = H.Scheme_Code
		LEFT JOIN RVPHomeList RCH
		ON RCH.RCH_code = H.School_Code
		INNER JOIN ServiceProvider SP
		ON SP.SP_ID = H.SP_ID
		INNER JOIN (
			SELECT Student_File_ID, 
				MAX(Student_Seq) AS StudentCount,
				COUNT(DISTINCT Class_Name) AS ClassCount,
				SUM(
					CASE WHEN ISNULL(Entitle_Inject, '') = 'Y' THEN 1 ELSE 0 END
				) AS AvailbleToInjectCount,
				SUM(
					CASE WHEN ISNULL(Entitle_Inject, '') = 'Y' AND
							  ISNULL(Entitle_ONLYDOSE, '') = 'Y' THEN 1 ELSE 0 END
				) AS AvailableToInjectCount_OnlyDose,
				SUM(
					CASE WHEN ISNULL(Entitle_Inject, '') = 'Y' AND
							  ISNULL(Entitle_1STDOSE, '') = 'Y' THEN 1 ELSE 0 END
				) AS AvailableToInjectCount_1stDose,
				SUM(
					CASE WHEN ISNULL(Entitle_Inject, '') = 'Y' AND
							  ISNULL(Entitle_2NDDOSE, '') = 'Y' THEN 1 ELSE 0 END
				) AS AvailableToInjectCount_2ndDose,
				SUM(
					CASE WHEN ISNULL(Reject_Injection, '') = 'Y' THEN 1 ELSE 0 END
				) AS NotInjectCount,
				SUM(
					CASE WHEN ISNULL(Injected, '') = 'Y' THEN 1 ELSE 0 END
				) AS WithInjectRecordCount,
				SUM(
					CASE WHEN ISNULL(Transaction_ID, '') <> '' THEN 1 ELSE 0 END
				) AS CreatedClaimCount,
				SUM(
					CASE WHEN ISNULL(Transaction_ID, '') = '' THEN 1 ELSE 0 END
				) AS NotCreatedClaimCount,
				SUM(
					CASE WHEN Ext_Ref_Status LIKE '_C_' OR Ext_Ref_Status LIKE '_S_' OR DH_Vaccine_Ref_Status LIKE '_C_'OR DH_Vaccine_Ref_Status LIKE '_S_' THEN 1 ELSE 0 END
				) AS VaccineConnectionFail,
				SUM(
					CASE WHEN Ext_Ref_Status LIKE '_P_' OR DH_Vaccine_Ref_Status LIKE '_P_' THEN 1 ELSE 0 END
				) AS VaccineDemographicsNotMatch
			FROM StudentFileEntry
			WHERE Student_File_ID= @Input_Student_File_ID
			GROUP BY Student_File_ID
		) AS ET
		ON ET.Student_File_ID = E.Student_File_ID
		INNER JOIN Practice P
		ON P.SP_ID = H.SP_ID
		AND P.Display_Seq = H.Practice_Display_Seq
		LEFT JOIN StaticData SD
		ON H.Dose = SD.Item_No
		AND SD.Column_Name = 'StudentFileDoseToInject'
		LEFT JOIN SubsidizeGroupClaim SGC
		ON SGC.Scheme_Code = H.Scheme_Code
			AND sgc.Scheme_Seq = H.Scheme_Seq
			AND sgc.Subsidize_Code = H.Subsidize_Code
		INNER JOIN SchemeClaim SC
		ON SC.Scheme_Code = H.Scheme_Code
		WHERE E.Student_File_ID = @Input_Student_File_ID

		-- 2nd Dose Column
		INSERT INTO #BatchTT
		(
			R1,
			Col1, Col2, Col3, Col4, Col5, Col6, Col7,
			Col8, Col9, Col10, Col11, Col12,
			Col13, Col14, Col15, Col16, Col17,	
			Col18, Col19, Col20, Col21, Col22, Col23, Col24, Col25, Col26, Col27
		)
		SELECT  DISTINCT
			3,
			'',
			'',
			'' AS SchoolName_EN,
			'' AS SchoolName_TC,
			'' AS SchoolAddr_EN,
			'' AS SchoolAddr_TC,
			'',
			'',
			'' AS SPName_EN,
			'' AS SPName_Chi,
			'' AS Practice_Name,
			'' AS Practice_Name_Chi,
			'',
			[Headings] = 
				CASE 
					WHEN H.Scheme_Code = 'VSS' AND RTRIM(H.Subsidize_Code) = 'VNIAMMR' THEN '' 
					WHEN Upload_Precheck = 'Y' OR Dose = '2NDDOSE' THEN '' 
					ELSE (SELECT Data_value FROM StaticData WITH (NOLOCK) WHERE Column_Name = 'StudentFileDoseToInject' AND Item_No = '2NDDOSE') 
				END,
			[Service_Receive_Dtm] = 
				CASE 
					WHEN H.Scheme_Code = 'VSS' AND RTRIM(H.Subsidize_Code) = 'VNIAMMR' THEN ''
					WHEN Service_Receive_Dtm_2ndDose IS NOT NULL THEN FORMAT(Service_Receive_Dtm_2ndDose, 'dd MMM yyyy') 
					ELSE 
						CASE 
							WHEN Upload_Precheck = 'Y' OR Dose = '2NDDOSE' THEN '' 
							ELSE 'N/A' 
						END
				END, 
			[Final_Checking_Report_Generation_Date] =
				CASE 
					WHEN H.Scheme_Code = 'VSS' AND RTRIM(H.Subsidize_Code) = 'VNIAMMR' THEN ''
					WHEN Final_Checking_Report_Generation_Date_2ndDose IS NOT NULL THEN FORMAT(Final_Checking_Report_Generation_Date_2ndDose, 'dd MMM yyyy') 
					ELSE
						CASE 
							WHEN Upload_Precheck = 'Y' OR Dose = '2NDDOSE' THEN '' 
							ELSE 'N/A' 
						END 
				END, 
			[Service_Receive_Dtm_2] = 
				CASE 
					WHEN H.Scheme_Code = 'VSS' AND RTRIM(H.Subsidize_Code) = 'VNIAMMR' THEN ''
					WHEN H.Scheme_Code = 'RVP' THEN '' 
					WHEN H.Service_Receive_Dtm_2ndDose_2 IS NOT NULL THEN FORMAT(H.Service_Receive_Dtm_2ndDose_2, 'dd MMM yyyy')
					ELSE
						CASE 
							WHEN Upload_Precheck = 'Y' OR Dose = '2NDDOSE' THEN '' 
							ELSE 'N/A'
						END 
				END,
			[Final_Checking_Report_Generation_Date_2] =
				CASE 
					WHEN H.Scheme_Code = 'VSS' AND RTRIM(H.Subsidize_Code) = 'VNIAMMR' THEN ''
					WHEN H.Scheme_Code = 'RVP' THEN '' 
					WHEN H.Final_Checking_Report_Generation_Date_2ndDose_2 IS NOT NULL THEN FORMAT(H.Final_Checking_Report_Generation_Date_2ndDose_2, 'dd MMM yyyy')
					ELSE
						CASE 
							WHEN Upload_Precheck = 'Y' OR Dose = '2NDDOSE' THEN '' 
							ELSE 'N/A'
						END 
				END,
			'',
			'',
			'',
			'',
			'',
			'',
			'', '', ''
		FROM StudentFileHeader H
		WHERE Student_File_ID= @Input_Student_File_ID

	END

	CLOSE SYMMETRIC KEY sym_Key

-- =============================================
-- Return results
-- =============================================
	
	--Batch sheet
	INSERT #Summary(ColIndex,Col1,Col2,Col3)
		
	SELECT CAST(REPLACE(T1.C1,'Col','') AS INT), T1.C2, T2.C2, T3.C2 
	FROM (
		SELECT C1, C2 
		FROM (
			SELECT 			
				Col1, Col2, Col3, Col4, Col5, 
				Col6, Col7, Col8, Col9,	Col10,
				Col11, Col12, Col13, Col14, Col15,	
				Col16, Col17, Col18, Col19, Col20,
				Col21, Col22, Col23, Col24, Col25,	
				Col26, Col27, Col28, Col29, Col30,
				Col31, Col32, Col33, Col34, Col35, 
				Col36, Col37, Col38
			FROM #BatchTT
			WHERE R1 = 1
		)p
		UNPIVOT(
			C2 FOR C1 IN (		
				Col1, Col2, Col3, Col4, Col5, 
				Col6, Col7, Col8, Col9,	Col10,
				Col11, Col12, Col13, Col14, Col15,	
				Col16, Col17, Col18, Col19, Col20,
				Col21, Col22, Col23, Col24, Col25,	
				Col26, Col27, Col28, Col29, Col30,
				Col31, Col32, Col33, Col34, Col35,
				Col36, Col37, Col38
			)
		)AS unpvt1
	) T1
	INNER JOIN	
	(
		SELECT C1, C2 
		FROM (
			SELECT 			
				Col1, Col2, Col3, Col4, Col5, 
				Col6, Col7, Col8, Col9,	Col10,
				Col11, Col12, Col13, Col14, Col15,	
				Col16, Col17, Col18, Col19, Col20,
				Col21, Col22, Col23, Col24, Col25,	
				Col26, Col27, Col28, Col29, Col30,
				Col31, Col32, Col33, Col34, Col35,
				Col36, Col37, Col38
			FROM #BatchTT
			WHERE R1 = 2
		)p
		UNPIVOT(
			C2 FOR C1 IN (		
				Col1, Col2, Col3, Col4, Col5, 
				Col6, Col7, Col8, Col9,	Col10,
				Col11, Col12, Col13, Col14, Col15,	
				Col16, Col17, Col18, Col19, Col20,
				Col21, Col22, Col23, Col24, Col25,	
				Col26, Col27, Col28, Col29, Col30,
				Col31, Col32, Col33, Col34, Col35,
				Col36, Col37, Col38
			)
		)AS unpvt2
	) T2
	ON T1.C1 = T2.C1
	LEFT JOIN	
	(
		SELECT C1, C2 
		FROM (
			SELECT 			
				Col1, Col2, Col3, Col4, Col5, 
				Col6, Col7, Col8, Col9,	Col10,
				Col11, Col12, Col13, Col14, Col15,	
				Col16, Col17, Col18, Col19, Col20,
				Col21, Col22, Col23, Col24, Col25,	
				Col26, Col27, Col28, Col29, Col30,
				Col31, Col32, Col33, Col34, Col35,
				Col36, Col37, Col38
			FROM #BatchTT
			WHERE R1 = 3
		)p
		UNPIVOT(
			C2 FOR C1 IN (		
				Col1, Col2, Col3, Col4, Col5, 
				Col6, Col7, Col8, Col9,	Col10,
				Col11, Col12, Col13, Col14, Col15,	
				Col16, Col17, Col18, Col19, Col20,
				Col21, Col22, Col23, Col24, Col25,	
				Col26, Col27, Col28, Col29, Col30,
				Col31, Col32, Col33, Col34, Col35,
				Col36, Col37, Col38
			)
		)AS unpvt3
	) T3
	ON T1.C1 = T3.C1

	INSERT #Summary(ColIndex,Col1,Col2,Col3) VALUES (50,'','','')
	INSERT #Summary(ColIndex,Col1,Col2,Col3) VALUES (50,'','','')
	INSERT #Summary(ColIndex,Col1,Col2,Col3) VALUES (50,'','','')
	INSERT #Summary(ColIndex,Col1,Col2,Col3) VALUES (50,'','','')
	INSERT #Summary(ColIndex,Col1,Col2,Col3) VALUES (50,'','','')
	INSERT #Summary(ColIndex,Col1,Col2,Col3) VALUES (50,'','','')
	INSERT #Summary(ColIndex,Col1,Col2,Col3) VALUES (50,'','','')
	INSERT #Summary(ColIndex,Col1,Col2,Col3) VALUES (50,'','','')
	INSERT #Summary(ColIndex,Col1,Col2,Col3) VALUES (50,'','','')
	INSERT #Summary(ColIndex,Col1,Col2,Col3) VALUES (50,'','','')

	INSERT #Summary(ColIndex,Col1,Col2,Col3) VALUES (50,'','','')
	INSERT #Summary(ColIndex,Col1,Col2,Col3) VALUES (50,'','','')
	INSERT #Summary(ColIndex,Col1,Col2,Col3) VALUES (50,'','','')
	INSERT #Summary(ColIndex,Col1,Col2,Col3) VALUES (50,'','','')
	INSERT #Summary(ColIndex,Col1,Col2,Col3) VALUES (50,'','','')
	INSERT #Summary(ColIndex,Col1,Col2,Col3) VALUES (50,'','','')
	INSERT #Summary(ColIndex,Col1,Col2,Col3) VALUES (50,'','','')
	INSERT #Summary(ColIndex,Col1,Col2,Col3) VALUES (50,'','','')
	INSERT #Summary(ColIndex,Col1,Col2,Col3) VALUES (50,'','','')
	INSERT #Summary(ColIndex,Col1,Col2,Col3) VALUES (50,'','','')

	SELECT Col1,Col2,Col3 FROM #Summary ORDER BY ColIndex

	--

	IF @File_ID <> 'EHSVF000'
		EXEC [proc_EHS_eHSSF_Class_Report_get] @Input_Student_File_ID, @File_ID
	ELSE
		EXEC [proc_EHS_eHSSF_Class_Precheck_Report_get] @Input_Student_File_ID, @File_ID
	
	--
	
	--'Remark'	
	
	INSERT INTO #RemarkTT (Result01, DisplaySeq)   
	VALUES ('(A) Legend' , @wsRemark_ct)    
	SELECT @wsRemark_ct=@wsRemark_ct + 1  

	INSERT INTO #RemarkTT (Result01, DisplaySeq)   
	VALUES ('1. Identity Document Type' , @wsRemark_ct)    
	SELECT @wsRemark_ct=@wsRemark_ct + 1  

	INSERT INTO #RemarkTT  (Result01, Result02, DisplaySeq)   
	SELECT Doc_Display_Code, 
		Doc_Name, 
		row_number() over (order by Display_Seq) + cast(@wsRemark_ct as integer)
	FROM DocType with(nolock)
	
	SELECT Result01, Result02
	FROM #RemarkTT
	ORDER BY DisplaySeq
	
	--
	
	DROP TABLE #BatchTT
	DROP TABLE #RemarkTT
	DROP TABLE #Summary
	
END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSSF_Report_get] TO HCVU
GO

