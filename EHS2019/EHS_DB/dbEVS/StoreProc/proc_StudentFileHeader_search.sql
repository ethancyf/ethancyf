IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileHeader_search]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileHeader_search]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	13 Aug 2020
-- CR No.			CRE20-003 (Batch Upload)
-- Description:		Add columns
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN	
-- Modified date:	16 Jul 2019
-- CR No.			CRE19-001 (PPP)
-- Description:		1. Retrieve new column 
--					2. Add searching criteria [Scheme_Code]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM		
-- Modified date:	09 Jul 2019
-- CR No.			CRE19-001
-- Description:		Grant EXECUTE right for role HCSP
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	23 August 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Search StudentFileHeader and StudentFileHeaderStaging
-- =============================================    

CREATE PROCEDURE [dbo].[proc_StudentFileHeader_search]
	@Student_File_ID		VARCHAR(15),
	@School_Code			VARCHAR(30),
	@SPID					VARCHAR(8),
	@DataEntryAccount		VARCHAR(20),
	@USERID					VARCHAR(20),
	@Scheme_Code			VARCHAR(5000),
	@Subsidize_Code			VARCHAR(10),
	@VaccinationDateFrom	DATETIME,
	@VaccinationDateTo		DATETIME,
	@CurrentSeason			BIT,
	@PreCheck				BIT,
	@PreCheckCompleted		BIT,
	@Record_Status			VARCHAR(5000)
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
	DECLARE @IN_Student_File_ID			VARCHAR(15)
	DECLARE @IN_School_Code				VARCHAR(30)
	DECLARE @IN_SPID					VARCHAR(8)
	DECLARE @IN_DataEntryAccount		VARCHAR(20)
	DECLARE @IN_USERID					VARCHAR(20)
	DECLARE @IN_Scheme_Code				VARCHAR(5000)
	DECLARE @IN_Subsidize_Code			VARCHAR(10)
	DECLARE @IN_VaccinationDateFrom		DATETIME
	DECLARE @IN_VaccinationDateTo		DATETIME
	DECLARE @IN_CurrentSeason			BIT
	DECLARE @IN_PreCheck				BIT
	DECLARE @IN_PreCheckCompleted		BIT
	DECLARE @IN_Record_Status			VARCHAR(5000)

	DECLARE @MinDate					DATETIME
	DECLARE @ChangeSeasonDate			DATETIME
	DECLARE @CurrentDate				DATETIME
	DECLARE @StartDate					DATETIME
	DECLARE @EndDate					DATETIME
	DECLARE @IsPreCheck					CHAR(1)
	DECLARE @IsIncludePreCheckCompleted CHAR(1)

	DECLARE @delimiter					VARCHAR(5)
	DECLARE @chrIsAllRecordStatus		CHAR(1)
	DECLARE @chrIsAllSchemeCode			CHAR(1)

	DECLARE @tblSchemeCode TABLE(
		Scheme_Code VARCHAR(10)
	)

	DECLARE @tblRecordStatus TABLE(
		Record_Status VARCHAR(2)
	)

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	SET @IN_Student_File_ID			= @Student_File_ID
	SET @IN_School_Code				= @School_Code
	SET @IN_SPID					= @SPID
	SET @IN_DataEntryAccount		= @DataEntryAccount
	SET @IN_USERID					= @USERID
	SET @IN_Scheme_Code				= @Scheme_Code
	SET @IN_Subsidize_Code			= @Subsidize_Code
	SET @IN_VaccinationDateFrom		= @VaccinationDateFrom
	SET @IN_VaccinationDateTo		= @VaccinationDateTo
	SET @IN_CurrentSeason			= @CurrentSeason
	SET @IN_PreCheck				= @PreCheck
	SET @IN_PreCheckCompleted		= @PreCheckCompleted
	SET @IN_Record_Status			= @Record_Status

	SET @MinDate					= NULL		
	SET @ChangeSeasonDate			= NULL		
	SET @CurrentDate				= (SELECT CONVERT(DATETIME,CONVERT(VARCHAR(10),GETDATE(),121)))
	SET @StartDate					= NULL		
	SET @EndDate					= NULL		

	SET @delimiter = ';'

	-- ---------------------------------------------
	-- Mininium Date to retrieve batch upload file
	-- ---------------------------------------------
	IF @IN_USERID IS NULL
		BEGIN
			SET @MinDate = CONVERT(DATETIME,(SELECT Parm_Value1 FROM SystemParameters WHERE Parameter_Name = 'Batch_Upload_MinDate'),121)
		END

	-- ---------------------------------------------
	-- Vaccination Date - Start Date
	-- ---------------------------------------------
	IF @IN_VaccinationDateFrom IS NULL 
		BEGIN
			SET @IN_VaccinationDateFrom = @MinDate
		END
	ELSE
		BEGIN
			IF @MinDate > @IN_VaccinationDateFrom 
			BEGIN
				SET @IN_VaccinationDateFrom = @MinDate
			END
		END

	-- ---------------------------------------------
	-- Vaccination Date - End Date
	-- ---------------------------------------------
	IF @IN_VaccinationDateTo IS NULL 
		BEGIN
			SET @IN_VaccinationDateTo = NULL		
		END
	ELSE
		BEGIN
			IF @MinDate > @IN_VaccinationDateTo
			BEGIN
				SET @IN_VaccinationDateFrom = @MinDate
			END
		END

	-- ---------------------------------------------
	-- Change Season Date
	-- ---------------------------------------------
	SET @ChangeSeasonDate = CONVERT(DATETIME,(SELECT CONVERT(VARCHAR(4),YEAR(GETDATE())) + Parm_Value1 FROM SystemParameters WHERE Parameter_Name = 'Batch_Upload_ChangeSeasonDate'),121)

	IF @IN_CurrentSeason IS NOT NULL
		BEGIN
			IF @IN_CurrentSeason = 0
				BEGIN
					IF @CurrentDate >= @ChangeSeasonDate
						BEGIN
							SET @StartDate = NULL
							SET @EndDate = DATEADD(D, -1, @ChangeSeasonDate)
						END
					ELSE
						BEGIN
							SET @StartDate = NULL
							SET @EndDate = DATEADD(YYYY, -1, DATEADD(D, -1, @ChangeSeasonDate))
						END
				END

			IF @IN_CurrentSeason = 1
				BEGIN
					IF @CurrentDate >= @ChangeSeasonDate
						BEGIN
							SET @StartDate = @ChangeSeasonDate
							SET @EndDate = NULL
						END
					ELSE
						BEGIN
							SET @StartDate =  DATEADD(YYYY, -1, @ChangeSeasonDate)
							SET @EndDate = NULL
						END
				END
		END

	--Checking
	--SELECT @IN_VaccinationDateFrom, @IN_VaccinationDateTo
	--SELECT @StartDate, @EndDate

	-- ---------------------------------------------
	-- Vaccination Date (Final)
	-- ---------------------------------------------
	IF @IN_VaccinationDateFrom IS NULL AND @StartDate IS NULL
		BEGIN
			SET @IN_VaccinationDateFrom = NULL		
		END
	ELSE IF @IN_VaccinationDateFrom IS NULL AND @StartDate IS NOT NULL
		BEGIN
			SET @IN_VaccinationDateFrom = @StartDate	
		END
	--ELSE IF @IN_VaccinationDateFrom IS NOT NULL AND @StartDate IS NULL
	--	BEGIN
	--		SET @IN_VaccinationDateFrom = @IN_VaccinationDateFrom			
	--	END
	ELSE IF @IN_VaccinationDateFrom IS NOT NULL AND @StartDate IS NOT NULL
		BEGIN
			IF @IN_VaccinationDateFrom < @StartDate 
			--	BEGIN
			--		SET @IN_VaccinationDateFrom = @IN_VaccinationDateFrom	
			--	END
			--ELSE
				BEGIN
					SET @IN_VaccinationDateFrom = @StartDate	
				END	
		END

	IF @IN_VaccinationDateTo IS NULL AND @EndDate IS NULL
		BEGIN
			SET @IN_VaccinationDateTo = NULL		
		END
	ELSE IF @IN_VaccinationDateTo IS NULL AND @EndDate IS NOT NULL
		BEGIN
			SET @IN_VaccinationDateTo = @EndDate	
		END
	--ELSE IF @IN_VaccinationDateTo IS NOT NULL AND @EndDate IS NULL
	--	BEGIN
	--		SET @IN_VaccinationDateTo = @IN_VaccinationDateTo			
	--	END
	ELSE IF @IN_VaccinationDateTo IS NOT NULL AND @EndDate IS NOT NULL
		BEGIN
			IF @IN_VaccinationDateTo > @EndDate 
			--	BEGIN
			--		SET @IN_VaccinationDateTo = @IN_VaccinationDateTo	
			--	END
			--ELSE
				BEGIN
					SET @IN_VaccinationDateTo = @EndDate	
				END	
		END

	--Checking
	--SELECT @IN_VaccinationDateFrom, @IN_VaccinationDateTo

	-- ---------------------------------------------
	-- Pre-Check
	-- ---------------------------------------------
		IF @IN_PreCheck IS NOT NULL
		BEGIN
			IF @IN_PreCheck = 1
				BEGIN
					SET @IsPreCheck = 'Y'
				END

			IF @IN_PreCheck = 0
				BEGIN
					SET @IsPreCheck = 'N'
				END
		END

	-- ---------------------------------------------
	-- Pre-Check Completed
	-- ---------------------------------------------
		IF @IN_PreCheckCompleted IS NOT NULL
		BEGIN
			IF @IN_PreCheckCompleted = 1
				BEGIN
					SET @IsIncludePreCheckCompleted = 'Y'
				END

			IF @IN_PreCheckCompleted = 0
				BEGIN
					SET @IsIncludePreCheckCompleted = 'N'
				END
		END

	-- ---------------------------------------------
	-- Scheme Code
	-- ---------------------------------------------
	IF @IN_Scheme_Code IS NOT NULL
	BEGIN

		INSERT @tblSchemeCode
				SELECT * FROM func_split_string(@IN_Scheme_Code, @delimiter)

		SET @chrIsAllSchemeCode = 'N'
	END
	ELSE 
		SET @chrIsAllSchemeCode = 'Y'

	-- ---------------------------------------------
	-- Record Status
	-- ---------------------------------------------
	IF @IN_Record_Status IS NOT NULL
	BEGIN

		INSERT @tblRecordStatus
				SELECT * FROM func_split_string(@IN_Record_Status, @delimiter)

		SET @chrIsAllRecordStatus = 'N'
	END
	ELSE 
		SET @chrIsAllRecordStatus = 'Y'

-- =============================================
-- Return results
-- =============================================

	OPEN SYMMETRIC KEY sym_Key
	DECRYPTION BY ASYMMETRIC KEY asym_Key

	SELECT
		S.Student_File_ID,
		S.Table_Location,
		S.Upload_Precheck,
		S.School_Code,
		[Name_Eng] = 
			CASE
				WHEN S.Scheme_Code = 'PPP' OR S.Scheme_Code = 'PPPKG' THEN Sch.Name_Eng
				WHEN S.Scheme_Code = 'RVP' THEN RCH.Homename_Eng
				ELSE ''
			END,
		[Name_Chi] =
			CASE
				WHEN S.Scheme_Code = 'PPP' OR S.Scheme_Code = 'PPPKG' THEN Sch.Name_Chi
				WHEN S.Scheme_Code = 'RVP' THEN RCH.Homename_Chi
				ELSE ''
			END,
		S.SP_ID,
		S.Practice_Display_Seq,
		S.Service_Receive_Dtm,
		S.Scheme_Code,
		S.Scheme_Seq,
		SC.Display_Code as [Scheme_Display_Code],
		S.Subsidize_Code,
		S.Dose,
		SGC.Display_Code_For_Claim,
		-- If RVP + QIV THEN display 'QIV 20XX/XX' Else display 'QIV-C 2018/19','23vPPV'
		CASE WHEN S.Scheme_Code = 'RVP' AND SUB.vaccine_Type = 'QIV' THEN
				RTRIM(SUB.vaccine_Type) + ' ' +  RTRIM(VS.Season_Desc)
			ELSE
				sgc.Display_Code_For_Claim
			END AS SubsidizeDisplayName,
		[Scheme_Subsidize_Dose_Display_Name] = RTRIM(SC.Display_Code) + ' ' + 
												(CASE 
													WHEN S.Scheme_Code = 'RVP' AND SUB.vaccine_Type = 'QIV' THEN 
														RTRIM(SUB.vaccine_Type) + ' ' +  RTRIM(VS.Season_Desc)
													ELSE 
														sgc.Display_Code_For_Claim
												END) + ' ' + 
												SD.Data_Value,
		S.Final_Checking_Report_Generation_Date,
		S.Service_Receive_Dtm_2,
		S.Final_Checking_Report_Generation_Date_2,
		S.Service_Receive_Dtm_2ndDose,
		S.Final_Checking_Report_Generation_Date_2ndDose,
		S.Service_Receive_Dtm_2ndDose_2,
		S.Final_Checking_Report_Generation_Date_2ndDose_2,
		S.Remark,
		S.Record_Status,
		S.Upload_By,
		S.Upload_Dtm,
		S.Last_Rectify_By,
		S.Last_Rectify_Dtm,
		S.Claim_Upload_By,
		S.Claim_Upload_Dtm,
		S.File_Confirm_By,
		S.File_Confirm_Dtm,
		S.Request_Remove_By,
		S.Request_Remove_Dtm,
		S.Request_Remove_Function,
		S.Confirm_Remove_By,
		S.Confirm_Remove_Dtm,
		S.Request_Claim_Reactivate_By,
		S.Request_Claim_Reactivate_Dtm,
		S.Confirm_Claim_Reactivate_By,
		S.Confirm_Claim_Reactivate_Dtm,
		S.Name_List_File_ID,
		S.Vaccination_Report_File_ID,
		S.Vaccination_Report_File_ID_2,
		S.Onsite_Vaccination_File_ID,
		S.Onsite_Vaccination_File_ID_2,
		S.Claim_Creation_Report_File_ID,
		S.Rectification_File_ID,
		FG4.[File_Name] as [Name_List_File_Name],
		FGQ4.Output_File AS [Name_List_File_Output_Name],
		CONVERT(VARCHAR(MAX), DecryptByKey(FGQ4.File_Password)) AS [Name_List_File_Default_Password],
		FG.[File_Name] as [Vaccination_Report_File_Name],
		FGQ.Output_File AS [Vaccination_Report_File_Output_Name],
		CONVERT(VARCHAR(MAX), DecryptByKey(FGQ.File_Password)) AS [Vaccination_Report_File_Default_Password],
		FG2.[File_Name] as [Onsite_Vaccination_File_Name],
		FGQ2.Output_File AS [Onsite_Vaccination_File_Output_Name],
		CONVERT(VARCHAR(MAX), DecryptByKey(FGQ2.File_Password)) AS [Onsite_Vaccination_File_Default_Password],
		FG5.[File_Name] as [Vaccination_Report_File_Name_2],
		FGQ5.Output_File AS [Vaccination_Report_File_Output_Name_2],
		CONVERT(VARCHAR(MAX), DecryptByKey(FGQ5.File_Password)) AS [Vaccination_Report_File_Default_Password_2],
		FG6.[File_Name] as [Onsite_Vaccination_File_Name_2],
		FGQ6.Output_File AS [Onsite_Vaccination_File_Output_Name_2],
		CONVERT(VARCHAR(MAX), DecryptByKey(FGQ6.File_Password)) AS [Onsite_Vaccination_File_Default_Password_2],
		FG3.[File_Name] as [Claim_Creation_Report_File_Name],
		FGQ3.Output_File AS [Claim_Creation_Report_File_Output_Name],
		CONVERT(VARCHAR(MAX), DecryptByKey(FGQ3.File_Password)) AS [Claim_Creation_Report_File_Default_Password],
		S.Update_By,
		S.Update_Dtm,
		S.TSMP,
		[Complete_Input_Injected] = CASE WHEN S.Complete IS NOT NULL THEN CASE WHEN S.Complete = 0 THEN 'Y' ELSE 'N' END ELSE NULL END,
		S.Original_Student_File_ID,		
		S.Request_Rectify_Status,
		[PreCheck_Input_Inject] = CASE WHEN SFEPCSI_1.Student_File_ID IS NOT NULL THEN 'Y' ELSE NULL END,
		[PreCheck_Complete_Inject] = CASE WHEN SFEPCSI_2.Complete IS NOT NULL THEN CASE WHEN SFEPCSI_2.Complete = SFEPCSI_3.Total_Client THEN 'Y' ELSE 'N' END ELSE NULL END,
		[PreCheck_Total_Client] = CASE WHEN SFEPCSI_3.Total_Client IS NOT NULL THEN SFEPCSI_3.Total_Client ELSE NULL END		
	FROM
		(
			SELECT 
				S.Student_File_ID,
				'P' AS [Table_Location],
				S.Upload_Precheck,
				S.School_Code,
				S.SP_ID,
				S.Practice_Display_Seq,
				S.Service_Receive_Dtm,
				S.Scheme_Code,
				S.Scheme_Seq,
				S.Subsidize_Code,
				S.Dose,
				S.Final_Checking_Report_Generation_Date,
				S.Service_Receive_Dtm_2,
				S.Final_Checking_Report_Generation_Date_2,
				S.Service_Receive_Dtm_2ndDose,
				S.Final_Checking_Report_Generation_Date_2ndDose,
				S.Service_Receive_Dtm_2ndDose_2,
				S.Final_Checking_Report_Generation_Date_2ndDose_2,
				S.Remark,
				S.Record_Status,
				S.Upload_By,
				S.Upload_Dtm,
				S.Last_Rectify_By,
				S.Last_Rectify_Dtm,
				S.Claim_Upload_By,
				S.Claim_Upload_Dtm,
				S.File_Confirm_By,
				S.File_Confirm_Dtm,
				S.Request_Remove_By,
				S.Request_Remove_Dtm,
				S.Request_Remove_Function,
				S.Confirm_Remove_By,
				S.Confirm_Remove_Dtm,
				S.Request_Claim_Reactivate_By,
				S.Request_Claim_Reactivate_Dtm,
				S.Confirm_Claim_Reactivate_By,
				S.Confirm_Claim_Reactivate_Dtm,
				S.Name_List_File_ID,
				S.Vaccination_Report_File_ID,
				S.Vaccination_Report_File_ID_2,
				S.Onsite_Vaccination_File_ID,
				S.Onsite_Vaccination_File_ID_2,
				S.Claim_Creation_Report_File_ID,
				S.Rectification_File_ID,
				S.Update_By,
				S.Update_Dtm,
				S.TSMP,
				SFE.Complete,
				S.Original_Student_File_ID,
				S.Request_Rectify_Status
			FROM
				StudentFileHeader S
					LEFT OUTER JOIN (SELECT 
										Student_File_ID, 
										SUM(CASE WHEN Injected IS NULL THEN 1 ELSE 0 END) AS [Complete] 
									FROM StudentFileEntryStaging GROUP BY Student_File_ID) SFE 
										ON S.Student_File_ID = SFE.Student_File_ID AND S.Record_Status = 'ST'
					LEFT JOIN @tblSchemeCode tblSC
						ON LTRIM(RTRIM(S.Scheme_Code)) = LTRIM(RTRIM(tblSC.Scheme_Code))
					LEFT JOIN @tblRecordStatus tblRS
						ON LTRIM(RTRIM(S.Record_Status)) = LTRIM(RTRIM(tblRS.Record_Status))
			WHERE
				(@Student_File_ID IS NULL OR S.Student_File_ID = @IN_Student_File_ID)
				AND (@IN_School_Code IS NULL OR S.School_Code = @IN_School_Code)
				AND (@IN_SPID IS NULL OR S.SP_ID = @IN_SPID)
				AND (@IN_Subsidize_Code IS NULL OR LTRIM(RTRIM(S.Subsidize_Code)) = @IN_Subsidize_Code)
				AND (
						(
							(@IN_VaccinationDateFrom IS NULL OR S.Service_Receive_Dtm IS NULL OR @IN_VaccinationDateFrom <= S.Service_Receive_Dtm) 
							AND (@IN_VaccinationDateTo IS NULL OR S.Service_Receive_Dtm <= @IN_VaccinationDateTo) 
						)
						OR
						(
							(@IN_VaccinationDateFrom IS NULL OR S.Service_Receive_Dtm_2 IS NULL OR @IN_VaccinationDateFrom <= S.Service_Receive_Dtm_2) 
							AND (@IN_VaccinationDateTo IS NULL OR S.Service_Receive_Dtm_2 <= @IN_VaccinationDateTo) 
						)
					)
				AND (@IN_PreCheck IS NULL OR S.Upload_Precheck = @IsPreCheck)
				AND S.Record_Status <> 'R'
				AND (@chrIsAllSchemeCode = 'Y' OR tblSC.Scheme_Code IS NOT NULL)		
				AND (@chrIsAllRecordStatus = 'Y' OR tblRS.Record_Status IS NOT NULL)		
		
			UNION ALL
			
			SELECT 
				S.Student_File_ID,
				'S' AS [Table_Location],
				S.Upload_Precheck,
				S.School_Code,
				S.SP_ID,
				S.Practice_Display_Seq,
				S.Service_Receive_Dtm,
				S.Scheme_Code,
				S.Scheme_Seq,
				S.Subsidize_Code,
				S.Dose,
				S.Final_Checking_Report_Generation_Date,
				S.Service_Receive_Dtm_2,
				S.Final_Checking_Report_Generation_Date_2,
				S.Service_Receive_Dtm_2ndDose,
				S.Final_Checking_Report_Generation_Date_2ndDose,
				S.Service_Receive_Dtm_2ndDose_2,
				S.Final_Checking_Report_Generation_Date_2ndDose_2,
				S.Remark,
				S.Record_Status,
				S.Upload_By,
				S.Upload_Dtm,
				S.Last_Rectify_By,
				S.Last_Rectify_Dtm,
				S.Claim_Upload_By,
				S.Claim_Upload_Dtm,
				S.File_Confirm_By,
				S.File_Confirm_Dtm,
				S.Request_Remove_By,
				S.Request_Remove_Dtm,
				S.Request_Remove_Function,
				S.Confirm_Remove_By,
				S.Confirm_Remove_Dtm,
				S.Request_Claim_Reactivate_By,
				S.Request_Claim_Reactivate_Dtm,
				S.Confirm_Claim_Reactivate_By,
				S.Confirm_Claim_Reactivate_Dtm,
				S.Name_List_File_ID,
				S.Vaccination_Report_File_ID,
				S.Vaccination_Report_File_ID_2,
				S.Onsite_Vaccination_File_ID,
				S.Onsite_Vaccination_File_ID_2,
				S.Claim_Creation_Report_File_ID,
				S.Rectification_File_ID,
				S.Update_By,
				S.Update_Dtm,
				S.TSMP,
				SFE.Complete,
				S.Original_Student_File_ID,
				S.Request_Rectify_Status
			FROM
				StudentFileHeaderStaging S
					LEFT OUTER JOIN (SELECT 
										Student_File_ID, 
										SUM(CASE WHEN Injected IS NULL THEN 1 ELSE 0 END) AS [Complete] 
									FROM StudentFileEntryStaging GROUP BY Student_File_ID) SFE 
										ON S.Student_File_ID = SFE.Student_File_ID AND S.Record_Status = 'ST'
					LEFT JOIN @tblSchemeCode tblSC
						ON LTRIM(RTRIM(S.Scheme_Code)) = LTRIM(RTRIM(tblSC.Scheme_Code))
					LEFT JOIN @tblRecordStatus tblRS
						ON LTRIM(RTRIM(S.Record_Status)) = LTRIM(RTRIM(tblRS.Record_Status))
			WHERE
				(@IN_Student_File_ID IS NULL OR S.Student_File_ID = @IN_Student_File_ID)
				AND (@IN_School_Code IS NULL OR S.School_Code = @IN_School_Code)
				AND (@IN_SPID IS NULL OR S.SP_ID = @IN_SPID)
				AND (@IN_Subsidize_Code IS NULL OR LTRIM(RTRIM(S.Subsidize_Code)) = @IN_Subsidize_Code)
				AND (
						(
							(@IN_VaccinationDateFrom IS NULL OR S.Service_Receive_Dtm IS NULL OR @IN_VaccinationDateFrom <= S.Service_Receive_Dtm) 
							AND (@IN_VaccinationDateTo IS NULL OR S.Service_Receive_Dtm <= @IN_VaccinationDateTo) 
						)
						OR
						(
							(@IN_VaccinationDateFrom IS NULL OR S.Service_Receive_Dtm_2 IS NULL OR @IN_VaccinationDateFrom <= S.Service_Receive_Dtm_2) 
							AND (@IN_VaccinationDateTo IS NULL OR S.Service_Receive_Dtm_2 <= @IN_VaccinationDateTo) 
						)
					)
				AND (@IN_PreCheck IS NULL OR S.Upload_Precheck = @IsPreCheck)
				AND S.Record_Status <> 'R'
				AND S.Student_File_ID NOT IN (
					SELECT Student_File_ID FROM StudentFileHeader WHERE Record_Status <> 'R'
				)
				AND (@chrIsAllSchemeCode = 'Y' OR tblSC.Scheme_Code IS NOT NULL)	
				AND (@chrIsAllRecordStatus = 'Y' OR tblRS.Record_Status IS NOT NULL)		
					
		) S
		LEFT OUTER JOIN SubsidizeGroupClaim SGC ON
			S.Scheme_Code = SGC.Scheme_Code AND S.Scheme_Seq = SGC.Scheme_Seq AND S.Subsidize_Code = SGC.Subsidize_Code
		LEFT OUTER JOIN Subsidize SUB ON 
			S.Subsidize_Code = SUB.Subsidize_Code
		LEFT OUTER JOIN VaccineSeason VS ON 
			VS.Scheme_Code = S.Scheme_Code
				AND VS.Scheme_Seq = S.Scheme_Seq
				AND VS.Subsidize_Item_Code = SUB.Subsidize_Item_Code
		LEFT OUTER JOIN SchemeClaim SC ON
			S.Scheme_Code = SC.Scheme_Code
		LEFT OUTER JOIN FileGenerationQueue FGQ ON
			S.Vaccination_Report_File_ID = FGQ.Generation_ID AND FGQ.[Status] = 'C'
		LEFT OUTER JOIN FileGenerationQueue FGQ2 ON
			S.Onsite_Vaccination_File_ID = FGQ2.Generation_ID AND FGQ2.[Status] = 'C'
		LEFT OUTER JOIN FileGenerationQueue FGQ3 ON
			S.Claim_Creation_Report_File_ID = FGQ3.Generation_ID AND FGQ3.[Status] = 'C'
		LEFT OUTER JOIN FileGenerationQueue FGQ4 ON
			S.Name_List_File_ID = FGQ4.Generation_ID AND FGQ4.[Status] = 'C'
		LEFT OUTER JOIN FileGenerationQueue FGQ5 ON
			S.Vaccination_Report_File_ID_2 = FGQ5.Generation_ID AND FGQ5.[Status] = 'C'
		LEFT OUTER JOIN FileGenerationQueue FGQ6 ON
			S.Onsite_Vaccination_File_ID_2 = FGQ6.Generation_ID AND FGQ6.[Status] = 'C'
		LEFT OUTER JOIN FileGeneration FG ON
			FGQ.[File_ID] = FG.[File_ID]
		LEFT OUTER JOIN FileGeneration FG2 ON
			FGQ2.[File_ID] = FG2.[File_ID]
		LEFT OUTER JOIN FileGeneration FG3 ON
			FGQ3.[File_ID] = FG3.[File_ID]
		LEFT OUTER JOIN FileGeneration FG4 ON
			FGQ4.[File_ID] = FG4.[File_ID]
		LEFT OUTER JOIN FileGeneration FG5 ON
			FGQ5.[File_ID] = FG5.[File_ID]
		LEFT OUTER JOIN FileGeneration FG6 ON
			FGQ6.[File_ID] = FG6.[File_ID]
		LEFT OUTER JOIN School Sch ON
			S.School_Code = Sch.School_Code
		LEFT OUTER JOIN RVPHomeList RCH ON
			S.School_Code = RCH.RCH_code
		LEFT OUTER JOIN DataEntryPracticeMapping DEPM ON 
			S.SP_ID = DEPM.SP_ID AND S.Practice_Display_Seq = DEPM.SP_Practice_Display_Seq AND DEPM.Data_Entry_Account = @IN_DataEntryAccount
		LEFT OUTER JOIN (SELECT DISTINCT Student_File_ID FROM StudentFileEntryPrecheckSubsidizeInject) SFEPCSI_1 ON
			S.Student_File_ID = SFEPCSI_1.Student_File_ID AND S.Record_Status IN ('PC','CR','PR')			
		LEFT OUTER JOIN (SELECT 
							Student_File_ID, 
							SUM(CASE WHEN Mark_Injection IS NOT NULL THEN 1 ELSE 0 END) AS [Complete] 
						FROM StudentFileEntryPrecheckSubsidizeInject GROUP BY Student_File_ID) SFEPCSI_2
							ON S.Student_File_ID = SFEPCSI_2.Student_File_ID AND S.Record_Status IN ('PC','CR','PR')	
		LEFT OUTER JOIN (SELECT 
							SFHPCD.Student_File_ID, 
							COUNT(1) AS [Total_Client] 
						FROM StudentFileHeaderPrecheckDate SFHPCD
								INNER JOIN StudentFileEntry SFE
									ON SFHPCD.Student_File_ID = SFE.Student_File_ID AND SFHPCD.Class_Name = SFE.Class_Name
						GROUP BY SFHPCD.Student_File_ID) SFEPCSI_3
							ON S.Student_File_ID = SFEPCSI_3.Student_File_ID AND S.Record_Status IN ('PC','CR','PR')
		LEFT OUTER JOIN StaticData SD ON
			SD.Column_Name = 'StudentFileDoseToInject' AND LTRIM(RTRIM(SD.Item_No)) = S.Dose

		WHERE
			(@IN_DataEntryAccount IS NULL OR DEPM.Data_Entry_Account IS NOT NULL)
			AND (@IN_USERID IS NULL OR EXISTS (SELECT DISTINCT Scheme_Code from UserRole where [User_ID] = @IN_USERID AND Scheme_Code = S.Scheme_Code))
			AND (@IN_PreCheckCompleted IS NULL 
				OR (@IsIncludePreCheckCompleted = 'Y' 
					OR NOT EXISTS (SELECT 
										Student_File_ID 
									FROM 
										StudentFileHeader 
									WHERE 
										Student_File_ID = S.Student_File_ID
										AND Upload_Precheck = 'Y' 
										AND Record_Status = 'C' )
										))
		ORDER BY
			S.Upload_Dtm
	
	CLOSE SYMMETRIC KEY sym_Key

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileHeader_search] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileHeader_search] TO HCVU
GO

