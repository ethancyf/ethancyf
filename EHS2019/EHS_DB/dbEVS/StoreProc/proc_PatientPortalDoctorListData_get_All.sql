IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PatientPortalDoctorListData_get_All]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PatientPortalDoctorListData_get_All]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.:			
-- Description:		
-- =============================================
-- =============================================
-- Created by:		Chris YIM
-- Created date:	08 Jan 2019
-- CR No.:			CRE20-005
-- Description:		Extract SDIR data for generating doctor list to eHRSS patient portal
-- =============================================
CREATE PROCEDURE [dbo].[proc_PatientPortalDoctorListData_get_All] 
AS
BEGIN
	-- ===========================================================
	-- DECLARATION
	-- ===========================================================
	DECLARE @profession				NVARCHAR(510)
	DECLARE @delimiter				VARCHAR(5)
	DECLARE @chrIsAllProfessional	CHAR(1)

	DECLARE @tblProfession TABLE(
		Service_Category_Code		VARCHAR(5)
	)

	DECLARE @MobileClinicEngDesc	NVARCHAR(MAX)
	DECLARE @MobileClinicChiDesc	NVARCHAR(MAX)

	-- ===========================================================
	-- Initialization
	-- ===========================================================
	SET @delimiter = ';'

	SET @profession = (SELECT Parm_Value1 FROM SystemParameters WHERE Parameter_Name = 'eHRSS_PP_DoctorList_Profession')

	SET @MobileClinicEngDesc = (SELECT [Description] FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'SPSResultMobileClinic')
	SET @MobileClinicChiDesc = (SELECT [Chinese_Description] FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'SPSResultMobileClinic')

	-- ===========================================================
	-- COPY PARAMETERS
	-- ===========================================================
	IF @profession IS NOT NULL AND @profession <> 'ALL'
	BEGIN
		INSERT @tblProfession
				SELECT * FROM func_split_string(@profession, @delimiter)

		SET @chrIsAllProfessional = 'N'
	END
	ELSE 
		SET @chrIsAllProfessional = 'Y'

	-- ===========================================================
	-- Result
	-- ===========================================================
	-- -----------------------------------------
	-- DataTable 1: System Name
	-- -----------------------------------------
	SELECT 
		[System] = [Description] 
	FROM 
		SystemResource WITH (NOLOCK) 
	WHERE 
		ObjectType = 'Text' 
		AND ObjectName = 'eHealthSystem'

	-- -----------------------------------------
	-- DataTable 2: Generation Time
	-- -----------------------------------------
	SELECT 
		[Generation_DateTime] = CONVERT(VARCHAR(19),DACO_CutOff_Dtm,121)
	FROM 
		DataCutOff_DACO WITH (NOLOCK) 
	WHERE 
		DACO_DataType_ID = 'SDIR_LastUpdateDate'

	-- -----------------------------------------
	-- DataTable 3: Points to Note
	-- -----------------------------------------
	SELECT 
		[Display_Seq] = 0,
		[Item_Level] = 'HEADER',
		[Item_Scheme_Code] = NULL,
		[Item_Desc_EN] = [Description],
		[Item_Desc_TC] = [Chinese_Description]
	FROM 
		SystemResource WITH (NOLOCK) 
	WHERE 
		ObjectType = 'Text' 
		AND ObjectName = 'PointsToNote_Declaration' 
	UNION
	SELECT
		[Display_Seq],
		[Item_Level] = 'SCHEME',
		[Item_Scheme_Code] = Scheme_Code,
		[Item_Desc_EN] = [Description_PlainText],
		[Item_Desc_TC] = [Description_PlainText_Chi]
	FROM 
		SDRemarks WITH (NOLOCK) 
	WHERE 
		Record_Status = 'A' 
		AND	Scheme_Code = 'HCVS'
	ORDER BY 
		Display_Seq

	-- -----------------------------------------
	-- DataTable 4: District Board
	-- -----------------------------------------
	SELECT 
		[District_Code] = CM.Code_Target,
		[District_Desc_EN] = LTRIM(RTRIM(district_board)),
		[District_Desc_TC] = LTRIM(RTRIM(district_board_chi))
	FROM 
		DistrictBoard DB WITH (NOLOCK)  
			INNER JOIN CodeMapping CM ON
				DB.district_board_shortname_SD = CM.Code_Source AND CM.Code_Type = 'EHRPatientPortal_DistrictCode' 
	WHERE
		district_board_shortname_SD NOT IN ('SZFT','OTHER')
	ORDER BY 
		district_board_shortname_SD

	-- -----------------------------------------
	-- DataTable 5: Profession
	-- -----------------------------------------
	SELECT DISTINCT
		SD_Display_Seq,
		[Prof_Code] = 
			CASE 
				WHEN LTRIM(RTRIM(Prof.Service_Category_Code)) = 'RMP' THEN 'RMP'
				WHEN LTRIM(RTRIM(Prof.Service_Category_Code)) = 'RCM' THEN 'CMP'
				WHEN LTRIM(RTRIM(Prof.Service_Category_Code)) = 'RDT' THEN 'RDT'
				WHEN LTRIM(RTRIM(Prof.Service_Category_Code)) = 'ROP' THEN 'ROP'
				ELSE LTRIM(RTRIM(Prof.Service_Category_Code))
			END,
		[Prof_Name_EN] = Service_Category_Desc_SD,
		[Prof_Name_TC] = Service_Category_Desc_SD_Chi
	FROM 
		Profession Prof WITH (NOLOCK) 
			INNER JOIN SDProfessionalMapping SDProf WITH (NOLOCK)
				ON Prof.Service_Category_Code = SDProf.service_category_code
			LEFT JOIN @tblProfession tblProf
				ON LTRIM(RTRIM(Prof.Service_Category_Code)) = tblProf.Service_Category_Code
	WHERE
		(@chrIsAllProfessional = 'Y' OR tblProf.Service_Category_Code IS NOT NULL)
	ORDER BY 
		SD_Display_Seq

	-- -----------------------------------------
	-- DataTable 6: Scheme
	-- -----------------------------------------
	SELECT 
		[Scheme_Code],
		[Scheme_Name_EN] = REPLACE(REPLACE(Scheme_Desc,'(HCVS)',''),'(VSS)',''), 
		[Scheme_Name_TC] = REPLACE(REPLACE(Scheme_Desc_Chi,'(HCVS)',''),'(VSS)',''),
		[Scheme_Email] = RTRIM(LTRIM(Scheme_Email)),
		[Scheme_Tel_No] = RTRIM(LTRIM(Scheme_Tel_No)),
		[Scheme_Website_EN] = RTRIM(LTRIM(Scheme_Website_en)),
		[Scheme_Website_TC] = RTRIM(LTRIM(Scheme_Website_tc)),
		[Scheme_Website_SC] = RTRIM(LTRIM(Scheme_Website_sc))
	FROM 
		SDScheme WITH (NOLOCK) 
	WHERE
		Scheme_Code = 'HCVS'

	-- -----------------------------------------
	-- DataTable 7: Category
	-- -----------------------------------------
	--SELECT DISTINCT
	--	[Category_Code] = CC.Category_Code,
	--	[Category_Name_EN] = CC.Category_Name,
	--	[Category_Name_TC] = CC.Category_Name_Chi
	--FROM 
	--	SubsidizeGroupCategory SGC WITH (NOLOCK) 
	--		INNER JOIN SDSubsidizeGroup SDSG WITH (NOLOCK) 
	--			ON SGC.Scheme_Code = SDSG.Scheme_Code AND SGC.Subsidize_Code = SDSG.Subsidize_Code 
	--		INNER JOIN ClaimCategory CC WITH (NOLOCK) 
	--			ON SGC.Category_Code = CC.Category_Code

	-- -----------------------------------------
	-- DataTable 8: Vaccine
	-- -----------------------------------------
	--SELECT DISTINCT
	--	[Vaccine_Code] = S.Subsidize_Item_Code,
	--	[Vaccine_Name_EN] = SDSG.Subsidize_Desc,
	--	[Vaccine_Name_TC] = SDSG.Subsidize_Desc_Chi, 
	--	[Vaccine_Short_Form] = Subsidize_Short_Form
	--FROM 
	--	SDSubsidizeGroup SDSG WITH (NOLOCK) 
	--		INNER JOIN Subsidize S
	--			ON SDSG.Subsidize_Code = S.Subsidize_Code
	--WHERE 
	--	Subsidize_Short_Form IS NOT NULL

	-- -----------------------------------------
	-- DataTable 9: Practice
	-- -----------------------------------------
	SELECT 
		[SP_ID] = SDPF.sp_id,
		[SP_Name_EN] = sp_name,
		[SP_Name_TC] = CASE WHEN sp_chi_name IS NULL THEN '' ELSE sp_chi_name END,
		[Practice_ID] = practice_display_seq,
		[Practice_Name_EN] = 
			CASE 
				WHEN SDPF.Mobile_Clinic = 'Y' THEN SDPF.practice_name + '(' + @MobileClinicEngDesc + ')'
				ELSE SDPF.practice_name
			END,
		[Practice_Name_TC] = 
			CASE 
				WHEN SDPF.practice_name_chi IS NULL OR SDPF.practice_name_chi = '' THEN 
					CASE 
						WHEN SDPF.Mobile_Clinic = 'Y' THEN SDPF.practice_name + '(' + @MobileClinicChiDesc + ')'
						ELSE SDPF.practice_name
					END
				ELSE 			
					CASE 
						WHEN SDPF.Mobile_Clinic = 'Y' THEN SDPF.practice_name_chi + '(' + @MobileClinicChiDesc + ')'
						ELSE SDPF.practice_name_chi
					END
			END,
		[Practice_Addr_EN] = 
			CASE 
				WHEN SDPF.Remarks_Desc <> '' THEN SDPF.address_eng + '(' + SDPF.Remarks_Desc + ')'
				WHEN SDPF.Remarks_Desc_Chi <> '' THEN SDPF.address_eng + '(' + SDPF.Remarks_Desc_Chi + ')'
				ELSE SDPF.address_eng
			END,
		[Practice_Addr_TC] = 
			CASE 
				WHEN SDPF.address_chi IS NULL OR SDPF.address_chi = '' THEN 
					CASE 
						WHEN SDPF.Remarks_Desc_Chi <> '' THEN SDPF.address_eng + '(' + SDPF.Remarks_Desc_Chi + ')'
						WHEN SDPF.Remarks_Desc <> '' THEN SDPF.address_eng + '(' + SDPF.Remarks_Desc + ')'
						ELSE SDPF.address_eng
					END
				ELSE 	
					CASE 
						WHEN SDPF.Remarks_Desc_Chi <> '' THEN SDPF.address_chi + '(' + SDPF.Remarks_Desc_Chi + ')'
						WHEN SDPF.Remarks_Desc <> '' THEN SDPF.address_chi + '(' + SDPF.Remarks_Desc + ')'
						ELSE SDPF.address_chi
					END
			END,
		[Practice_Tel_No] = SDPF.phone_daytime,
		[Practice_District_Code] = 
			CASE 
				WHEN LTRIM(RTRIM(district_board_shortname_SD)) = 'EAST' THEN 'EST'
				WHEN LTRIM(RTRIM(district_board_shortname_SD)) = 'ISL' THEN 'ILD'
				WHEN LTRIM(RTRIM(district_board_shortname_SD)) = 'KTS' THEN 'KC'
				WHEN LTRIM(RTRIM(district_board_shortname_SD)) = 'KC' THEN 'KLC'
				WHEN LTRIM(RTRIM(district_board_shortname_SD)) = 'NORTH' THEN 'NTH'
				WHEN LTRIM(RTRIM(district_board_shortname_SD)) = 'SOUTH' THEN 'STH'
				ELSE LTRIM(RTRIM(district_board_shortname_SD))
			END,
		[Practice_Prof_Code] = 
			CASE 
				WHEN LTRIM(RTRIM(Pro.Service_Category_Code)) = 'RMP' THEN 'RMP'
				WHEN LTRIM(RTRIM(Pro.Service_Category_Code)) = 'RCM' THEN 'CMP'
				WHEN LTRIM(RTRIM(Pro.Service_Category_Code)) = 'RDT' THEN 'RDT'
				WHEN LTRIM(RTRIM(Pro.Service_Category_Code)) = 'ROP' THEN 'ROP'
				ELSE LTRIM(RTRIM(Pro.Service_Category_Code))
			END,
		[Practice_Prof_Reg_No] = Pro.Registration_Code
	FROM 
		SDSPPracticeFee SDPF WITH (NOLOCK) 
			LEFT OUTER JOIN Practice P WITH (NOLOCK) 
				ON SDPF.sp_id = P.SP_ID AND SDPF.practice_display_seq = P.Display_Seq
			LEFT OUTER JOIN Professional Pro WITH (NOLOCK) 
				ON P.SP_ID = Pro.SP_ID AND P.Professional_Seq = Pro.Professional_Seq
			LEFT JOIN @tblProfession tblProf
				ON LTRIM(RTRIM(Pro.Service_Category_Code)) = tblProf.Service_Category_Code
	WHERE 
		joined_scheme IS NOT NULL
		AND (@chrIsAllProfessional = 'Y' OR tblProf.Service_Category_Code IS NOT NULL)
		AND joined_scheme LIKE '%HCVS%'

	-- -----------------------------------------
	-- DataTable 10: Subsidize Item & Fee
	-- -----------------------------------------
	SELECT
		[SP_ID] = Item.SP_ID
		,[Practice_ID] = Item.Practice_ID
		,[Scheme_Code] = SubsidizeItem.Scheme_Code
		,[Category_Code] = SubsidizeItem.Category_Code
		,[Vaccine_Code] = SubsidizeItem.Subsidize_Item_Code
		,[Vaccine_Service_Fee_Provided] = Subsidize_Item_Value
		,[Vaccine_Service_Fee] = Subsidize_Fee_Value
	FROM
		(SELECT
			[SP_ID] = sp_id
			,[Practice_ID] = practice_display_seq
			,Subsidize_Item_Column_Name
			,Subsidize_Item_Value
		FROM
			(SELECT 
				SDPF.sp_id
				,practice_display_seq
				,subsidize_item_01
				,subsidize_item_02
				,subsidize_item_03
				,subsidize_item_04
				,subsidize_item_05
				,subsidize_item_06
				,subsidize_item_07
				,subsidize_item_08
				,subsidize_item_09
				,subsidize_item_10
			FROM	
				SDSPPracticeFee SDPF WITH (NOLOCK) 
					LEFT OUTER JOIN Practice P WITH (NOLOCK) 
						ON SDPF.sp_id = P.SP_ID AND SDPF.practice_display_seq = P.Display_Seq
					LEFT OUTER JOIN Professional Pro WITH (NOLOCK) 
						ON P.SP_ID = Pro.SP_ID AND P.Professional_Seq = Pro.Professional_Seq
					LEFT JOIN @tblProfession tblProf
						ON LTRIM(RTRIM(Pro.Service_Category_Code)) = tblProf.Service_Category_Code
			WHERE 
				joined_scheme IS NOT NULL
				AND (@chrIsAllProfessional = 'Y' OR tblProf.Service_Category_Code IS NOT NULL)
			) SDPF
		UNPIVOT
			(Subsidize_Item_Value For Subsidize_Item_Column_Name IN 
				(subsidize_item_01
				,subsidize_item_02
				,subsidize_item_03
				,subsidize_item_04
				,subsidize_item_05
				,subsidize_item_06
				,subsidize_item_07
				,subsidize_item_08
				,subsidize_item_09
				,subsidize_item_10)
			) UNPVT
		) Item
		LEFT OUTER JOIN
		(SELECT
			[SP_ID] = sp_id
			,[Practice_ID] = practice_display_seq
			,Subsidize_Fee_Column_Name
			,Subsidize_Fee_Value
		FROM
			(SELECT 
				SDPF.sp_id
				,practice_display_seq
				,subsidize_fee_01
				,subsidize_fee_02
				,subsidize_fee_03
				,subsidize_fee_04
				,subsidize_fee_05
				,subsidize_fee_06
				,subsidize_fee_07
				,subsidize_fee_08
				,subsidize_fee_09
				,subsidize_fee_10
			FROM	
				SDSPPracticeFee SDPF WITH (NOLOCK) 
					LEFT OUTER JOIN Practice P WITH (NOLOCK) 
						ON SDPF.sp_id = P.SP_ID AND SDPF.practice_display_seq = P.Display_Seq
					LEFT OUTER JOIN Professional Pro WITH (NOLOCK) 
						ON P.SP_ID = Pro.SP_ID AND P.Professional_Seq = Pro.Professional_Seq
					LEFT JOIN @tblProfession tblProf
						ON LTRIM(RTRIM(Pro.Service_Category_Code)) = tblProf.Service_Category_Code
			WHERE 
				joined_scheme IS NOT NULL
				AND (@chrIsAllProfessional = 'Y' OR tblProf.Service_Category_Code IS NOT NULL)		
			) SDPF
		UNPIVOT
			(Subsidize_Fee_Value For Subsidize_Fee_Column_Name IN 
				(subsidize_fee_01
				,subsidize_fee_02
				,subsidize_fee_03
				,subsidize_fee_04
				,subsidize_fee_05
				,subsidize_fee_06
				,subsidize_fee_07
				,subsidize_fee_08
				,subsidize_fee_09
				,subsidize_fee_10)
			) UNPVT
		) Fee
			ON Item.SP_ID = Fee.SP_ID 
				AND Item.Practice_ID = Fee.Practice_ID 
				AND REPLACE(Item.Subsidize_Item_Column_Name, '_item', '') = REPLACE(Fee.Subsidize_Fee_Column_Name, '_fee', '')
				AND Fee.Subsidize_Fee_Value <> '*' 
				AND Fee.Subsidize_Fee_Value <> '**'
				AND Fee.Subsidize_Fee_Value <> '*/**'
				AND Fee.Subsidize_Fee_Value <> '{TBP}'
		LEFT OUTER JOIN
		(SELECT 
			SDSG.Scheme_Code,
			CC.Category_Code,
			S.Subsidize_Item_Code,
			SDSG.Subsidize_Item_Column_Name,
			SDSG.Subsidize_Fee_Column_Name
		FROM 
			SDSubsidizeGroup SDSG WITH (NOLOCK) 
				LEFT OUTER JOIN SubsidizeGroupCategory SGC WITH (NOLOCK) 
					ON SDSG.Scheme_Code = SGC.Scheme_Code AND SDSG.Subsidize_Code = SGC.Subsidize_Code 
				LEFT OUTER JOIN ClaimCategory CC WITH (NOLOCK) 
					ON SGC.Category_Code = CC.Category_Code
				LEFT OUTER JOIN Subsidize S WITH (NOLOCK) 
					ON SDSG.Subsidize_Code = S.Subsidize_Code
		) SubsidizeItem
			ON SubsidizeItem.Subsidize_Item_Column_Name = Item.Subsidize_Item_Column_Name 
				AND (
						SubsidizeItem.Subsidize_Fee_Column_Name = Fee.Subsidize_Fee_Column_Name 
						OR 
						(SubsidizeItem.Subsidize_Fee_Column_Name IS NULL AND Fee.Subsidize_Fee_Column_Name IS NULL)
					)
	WHERE
		SubsidizeItem.Scheme_Code IS NOT NULL
		AND SubsidizeItem.Scheme_Code = 'HCVS'
	ORDER BY Item.SP_ID

END
GO

GRANT EXECUTE ON [dbo].[proc_PatientPortalDoctorListData_get_All] TO HCVU
GO

