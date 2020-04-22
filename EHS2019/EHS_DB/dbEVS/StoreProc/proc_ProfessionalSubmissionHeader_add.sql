IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionalSubmissionHeader_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionalSubmissionHeader_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No:			CRE13-016 Upgrade Excel verion to 2007
-- Modified by:		Karl LAM
-- Modified date:	21 Oct 2013
-- Description:		Change File Name to Varchar(50)
--					Retrive file extension by dbo.FileGeneration
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 26 May 2008
-- Description:	Insert Professional Submission Header
-- =============================================
--exec proc_ProfessionalSubmissionHeader_add 'RCM', 'HAADMIN', '',''
CREATE PROCEDURE [dbo].[proc_ProfessionalSubmissionHeader_add]
	@Profession_Code char(5),
	@Create_By varchar(20),
	@File_Name varchar(50) OUTPUT,
	@Export_Dtm datetime OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
	-- Search File_Name Pharse
	Declare @Search_File_Name varchar(50)

	-- Retrieved File_Name From Database
	Declare @Tmp_File_Name varchar(50)

	-- Return File Name
	Declare @Return_File_Name varchar(50)

	-- Sequence No. -- String
	Declare @Tmp_str_Index char(2)

	-- Sequence No. -- SmallInt
	Declare @Tmp_Index SmallInt

	-- File Type --varchar(50)
	Declare @File_Type varchar(50)
	
	-- File ID --varchar(30)
	Declare @File_ID varchar(30)

-- =============================================
-- Validation 
-- =============================================

-- 1. Validate Profession Code
-- 2. If no record for the Profession Code, No need to generate file.
-- 3. If the Sequence No. of the file excess 99, Abort.

-- [The Logic of Check Record Exist in <proc_ProfessionalSubmissionHeader_add> is the same as proc_ProfessionalVerification_get_ToExportByProCode]

/*	
	IF (
		SELECT COUNT(*) as RecordCount FROM 
			[dbo].[ProfessionalVerification] PV 
				INNER JOIN [dbo].[ProfessionalStaging] PS
					ON PV.Enrolment_Ref_No = PS.Enrolment_Ref_No AND PV.Professional_Seq = PS.Professional_Seq

				INNER JOIN [dbo].[SPAccountUpdate] SPAU 
					ON PV.Enrolment_Ref_No = SPAU.Enrolment_Ref_No
		WHERE 
			PV.Export_By Is NULL AND PV.Export_Dtm IS NULL AND
			PV.Import_By Is Null AND PV.Import_Dtm Is NULL AND
			SPAU.Progress_Status = 'P' AND PS.Service_Category_Code = @Profession_Code
	) <= 0 
	BEGIN
		Set @File_Name = ''
		Set @Export_Dtm = null
		Return 0 
	END
*/
-- =============================================
-- Initialization
-- =============================================

--Set file id of BNC File
	SET @File_ID = 'BNC'
	
--Get File Type
	SELECT @File_Type = lower(File_Type) FROM FileGeneration WHERE [File_ID] = @File_ID	
	
-- To Generate File Name
-- 1. <Profession Code> + <Date in YYMMDD> + <2 Digit Sequence#>

	Set @Search_File_Name = LTRIM(RTRIM(@Profession_Code)) + CONVERT(Varchar(8), GetDate(), 12)

	SELECT TOP (1) @Tmp_File_Name = File_Name FROM [dbo].[ProfessionalSubmissionHeader]
	WHERE File_Name Like @Search_File_Name + '%'
	ORDER BY File_Name DESC

-- If Previous Submission Exist, Get Sequence# = Sequence# + 1
	IF @Tmp_File_Name IS NOT NULL AND LTRIM(RTRIM(@Tmp_File_Name)) != ''
	BEGIN
		Set @Tmp_str_Index = SubString(LTRIM(RTRIM(@Tmp_File_Name)),LEN(LTRIM(RTRIM(@Search_File_Name)))+1,2)
		Set @Tmp_Index = Convert(smallint,@Tmp_str_Index)
		Set @Tmp_Index = @Tmp_Index + 1

		IF @Tmp_Index <= 1 AND @Tmp_Index > 99
		begin
			Raiserror('00007', 16,1)
			return @@error
		end

		IF @Tmp_Index < 10 
			--Set @Return_File_Name = LTRIM(RTRIM(@Search_File_Name)) + '0' + RTRIM(CAST(@Tmp_Index as char)) + '.xls'
			Set @Return_File_Name = LTRIM(RTRIM(@Search_File_Name)) + '0' + RTRIM(CAST(@Tmp_Index as char)) + '.' + @File_Type
		ELSE
			--Set @Return_File_Name = LTRIM(RTRIM(@Search_File_Name)) + RTRIM(CAST(@Tmp_Index as char)) + '.xls'
			Set @Return_File_Name = LTRIM(RTRIM(@Search_File_Name)) + RTRIM(CAST(@Tmp_Index as char)) + '.' + @File_Type
	END
	ELSE
		--Set @Return_File_Name = LTRIM(RTRIM(@Search_File_Name)) + '01' + '.xls'
		Set @Return_File_Name = LTRIM(RTRIM(@Search_File_Name)) + '01' + '.' + @File_Type

-- =============================================
-- Return results
-- =============================================

	INSERT INTO [dbo].[ProfessionalSubmissionHeader]
		([File_Name],
		[Export_Dtm],
		[Export_By],
		[Service_Category_Code],
		[Import_Dtm],
		[Import_By],
		[Record_Status]
		)
	VALUES
		(@Return_File_Name,
		GetDate(),
		@Create_By,
		@Profession_Code,
		null,
		null,
		'A')

	SELECT
		@File_Name = File_Name, @Export_Dtm = Export_Dtm 
	FROM [dbo].[ProfessionalSubmissionHeader]
	WHERE
		File_Name = @Return_File_Name
	Return 1
END

GO

GRANT EXECUTE ON [dbo].[proc_ProfessionalSubmissionHeader_add] TO HCVU
GO
