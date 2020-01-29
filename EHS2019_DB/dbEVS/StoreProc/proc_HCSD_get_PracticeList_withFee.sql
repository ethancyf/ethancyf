IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCSD_get_PracticeList_withFee]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCSD_get_PracticeList_withFee]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CE19-001-03
-- Modified by:		Koala CHENG
-- Modified date:	02 Sep 2019
-- Description:		Handle sorting of * and */** 
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			INT17-0022
-- Modified by:		I-CRE17-007
-- Modified date:	20 February 2018
-- Description:		Performance Tuning
--					1. Add WITH (NOLOCK)
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE17-005
-- Modified by:		Marco CHOI
-- Modified date:	31 October 2017
-- Description:		Enhance Searching function
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			INT16-0013
-- Modified by:		Koala CHENG
-- Modified date:	12 October 2016
-- Description:		Enlarge length of practice name for '(Non-clinic)'
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE16-002 (Revamp VSS)
-- Modified by:		Chris YIM
-- Modified date:	11 Aug 2016
-- Description:		Re-write the search
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE15-004 (TIV and QIV)
-- Modified by:		Philip Chau
-- Modified date:	08 July 2015
-- Description:		Bug fix for filtering out the expiry subsidies at the second return table
-- =============================================
-- =============================================
-- 
-- Modification History
-- CR No.:			CRE14-008
-- Modified by:		Karl LAM
-- Modified date:	29 Aug 2014
-- Description:		Update to handle service provide do not provide service fee
-- =============================================
-- =============================================
-- Modification History
-- CR No:			CRE13-017
-- Modified by:		Karl LAM
-- Modified date:	02 Dec 2013
-- Description:		Correct display for EVSS service fee after adding CVSSPCV13
-- =============================================
-- =============================================
-- =============================================
-- Author:		Mattie LO
-- Update date: 21 October 2009
-- Description:	 Bug fix for truncating the English Name of the SP when Chinese Name is NULL
-- =============================================
-- =============================================
-- Author:		Mattie LO
-- Create date: 30 August 2009
-- Description:	 Retrieve the Practice List with Service Fee for specific vaccine
-- =============================================

CREATE PROCEDURE [dbo].[proc_HCSD_get_PracticeList_withFee] 
	@ServiceProvideName nvarchar(200) = NULL
	,@PracticeName nvarchar(200) = NULL
	,@PracticeAddress nvarchar(200) = NULL
	,@Professional varchar(5) =  NULL
	,@subsidize_items varchar(200) = NULL
	,@DistrictList varchar(200) = NULL
	,@language varchar(10) = NULL
AS
BEGIN

-- =============================================
-- Massage input data
-- =============================================	
	IF @ServiceProvideName IS NOT NULL BEGIN
		SET @ServiceProvideName = [dbo].[func_MassageFreeTextInput](@ServiceProvideName)
	END

	IF @practiceName IS NOT NULL BEGIN
		SET @practiceName = [dbo].[func_MassageFreeTextInput](@practiceName)
	END

	IF @practiceaddress IS NOT NULL BEGIN
		SET @practiceaddress = [dbo].[func_MassageFreeTextInput](@practiceaddress)
	END
	
	DECLARE @subsidize_item_01 varchar(20)
	DECLARE @subsidize_item_02 varchar(20)
	DECLARE @subsidize_item_03 varchar(20)
	DECLARE @subsidize_item_04 varchar(20)
	DECLARE @subsidize_item_05 varchar(20)
	DECLARE @subsidize_item_06 varchar(20)
	DECLARE @subsidize_item_07 varchar(20)
	DECLARE @subsidize_item_08 varchar(20)
	DECLARE @subsidize_item_09 varchar(20)
	DECLARE @subsidize_item_10 varchar(20)

	DECLARE @RecordLimit varchar(10)

	SET @subsidize_item_01 = NULL
	SET @subsidize_item_02 = NULL
	SET @subsidize_item_03 = NULL
	SET @subsidize_item_04 = NULL
	SET @subsidize_item_05 = NULL
	SET @subsidize_item_06 = NULL
	SET @subsidize_item_07 = NULL
	SET @subsidize_item_08 = NULL
	SET @subsidize_item_09 = NULL
	SET @subsidize_item_10 = NULL

	SET @RecordLimit = CONVERT(varchar(10),(SELECT Parm_Value1 FROM SystemParameters WITH (NOLOCK) WHERE Parameter_Name = 'SDIR_returnRecordLimit') + 1)

	CREATE TABLE #SDSPPracticeFee(
		[sp_id] [char](8) NOT NULL,
		[practice_display_seq] [smallint] NOT NULL,
		[sp_name] [varchar](40) NULL,
		[sp_chi_name] [nvarchar](6) NULL,
		[practice_name] [nvarchar](150) NULL,
		[practice_name_chi] [nvarchar](150) NULL,
		[phone_daytime] [varchar](20) NULL,
		[service_category_code_SD] [char](5) NULL,
		[district_code] [char](4) NULL,
		[district_board_shortname_SD] [char](5) NOT NULL,
		[area_code] [char](1) NULL,
		[address_eng] [varchar](150) NULL,
		[district_name] [char](15) NULL,
		[district_board] [char](15) NULL,
		[area_name] [char](50) NULL,
		[address_chi] [nvarchar](150) NULL,
		[district_name_chi] [nchar](30) NULL,
		[district_board_chi] [nchar](30) NULL,
		[area_name_chi] [nchar](50) NULL,
		[subsidize_item_01] [char](1) NULL,
		[subsidize_item_02] [char](1) NULL,
		[subsidize_item_03] [char](1) NULL,
		[subsidize_item_04] [char](1) NULL,
		[subsidize_item_05] [char](1) NULL,
		[subsidize_item_06] [char](1) NULL,
		[subsidize_item_07] [char](1) NULL,
		[subsidize_item_08] [char](1) NULL,
		[subsidize_item_09] [char](1) NULL,
		[subsidize_item_10] [char](1) NULL,
		[subsidize_fee_01] [varchar](15) NULL,
		[subsidize_fee_02] [varchar](15) NULL,
		[subsidize_fee_03] [varchar](15) NULL,
		[subsidize_fee_04] [varchar](15) NULL,
		[subsidize_fee_05] [varchar](15) NULL,
		[subsidize_fee_06] [varchar](15) NULL,
		[subsidize_fee_07] [varchar](15) NULL,
		[subsidize_fee_08] [varchar](15) NULL,
		[subsidize_fee_09] [varchar](15) NULL,
		[subsidize_fee_10] [varchar](15) NULL,
		[joined_scheme] [varchar](100) NULL,
		[Subsidy_with_fee] [INT] NULL
	)

	CREATE CLUSTERED INDEX PK_SDPracticeServiceFee_SP_ID_Practice_Display_Seq
		ON #SDSPPracticeFee (SP_ID ASC, Practice_Display_Seq ASC); 

	DECLARE @delimiter VARCHAR(5)

	SET @delimiter = ';'

	--Largest service fee can be inputted only 9999, so set the value for sorting purpose only to 99999
	DECLARE @SortAsteriskServiceFeeToLast bit
	DECLARE @Service_fee_For_Sorting varchar(5)

	DECLARE @Symbol_For_No_QIV_Service_Fee char(4)
	SET @Symbol_For_No_QIV_Service_Fee = '*/**'

	DECLARE @Symbol_For_No_QIV_No_LAIV_Service_Fee char(1)
	SET @Symbol_For_No_QIV_No_LAIV_Service_Fee = '*'

	DECLARE @Symbol_For_No_QIV_No_TIV_Service_Fee char(2)
	SET @Symbol_For_No_QIV_No_TIV_Service_Fee = '**'
	

	DECLARE @Symbol_For_To_Be_Provide_Service_Fee char(5)
	SET @Symbol_For_To_Be_Provide_Service_Fee = '{TBP}'

	DECLARE @Symbol_For_NA_Service_Fee char(4)
	SET @Symbol_For_NA_Service_Fee = '{NA}'

	SET @SortAsteriskServiceFeeToLast = 1

	IF @SortAsteriskServiceFeeToLast = 1
		BEGIN
			SET @Service_fee_For_Sorting = '99999'
		END
	ELSE
		BEGIN
			SET @Service_fee_For_Sorting = '0'
		END
	
	CREATE TABLE #DistrictList(DistrictBoard varchar(5) COLLATE Chinese_Taiwan_Stroke_CI_AS)

	IF @DistrictList IS NOT NULL
	BEGIN
	   INSERT #DistrictList EXECUTE proc_HCSDUtilStringToTable @DistrictList
	END

	DECLARE @tblSearch_Group TABLE(
		Search_Group VARCHAR(10)
	)
	
	DECLARE @tblSubsidize_Item_Column_Name TABLE(
		--SEQ_ID INT IDENTITY(1,1) PRIMARY KEY,
		Scheme_Code VARCHAR(10),
		Scheme_Display_Seq SMALLINT,
		Operator_Priority VARCHAR(10),
		Operator VARCHAR(10),
		Subsidize_Item_Column_Name VARCHAR(20)
	)
	

	DECLARE @SQL_STAT NVARCHAR(MAX)
	DECLARE @SQL_STAT_PATTERN_1 NVARCHAR(MAX)
	DECLARE @SQL_STAT_PATTERN_2 NVARCHAR(MAX)
	DECLARE @SQL_STAT_PATTERN_3 NVARCHAR(MAX)
	DECLARE @PARM_DEFINITION NVARCHAR(MAX)

	SET @SQL_STAT_PATTERN_1	= NULL
	SET @SQL_STAT_PATTERN_2	= NULL
	SET @SQL_STAT_PATTERN_3	= NULL

	--=================================================================

	INSERT INTO #SDSPPracticeFee
        ([sp_id]
        ,[practice_display_seq]
        ,[sp_name]
        ,[sp_chi_name]
        ,[practice_name]
        ,[practice_name_chi]
        ,[phone_daytime]
        ,[service_category_code_SD]
        ,[district_code]
        ,[district_board_shortname_SD]
        ,[area_code]
        ,[address_eng]
        ,[district_name]
        ,[district_board]
        ,[area_name]
        ,[address_chi]
        ,[district_name_chi]
        ,[district_board_chi]
        ,[area_name_chi]
        ,[subsidize_item_01]
        ,[subsidize_item_02]
        ,[subsidize_item_03]
        ,[subsidize_item_04]
        ,[subsidize_item_05]
        ,[subsidize_item_06]
        ,[subsidize_item_07]
        ,[subsidize_item_08]
        ,[subsidize_item_09]
        ,[subsidize_item_10]
        ,[subsidize_fee_01]
        ,[subsidize_fee_02]
        ,[subsidize_fee_03]
        ,[subsidize_fee_04]
        ,[subsidize_fee_05]
        ,[subsidize_fee_06]
        ,[subsidize_fee_07]
        ,[subsidize_fee_08]
        ,[subsidize_fee_09]
        ,[subsidize_fee_10]
        ,[joined_scheme]
		,[Subsidy_with_fee])
	SELECT
		[sp_id]
		,[practice_display_seq]
		,[sp_name]
		,[sp_chi_name]
		,[practice_name]
		,[practice_name_chi]
		,[phone_daytime]
		,[service_category_code_SD]
		,[district_code] 
		,[district_board_shortname_SD]
		,[area_code]
		,[address_eng]
		,[district_name]
		,[district_board]
		,[area_name]
		,[address_chi]
		,[district_name_chi]
		,[district_board_chi]
		,[area_name_chi]
		,[subsidize_item_01]
		,[subsidize_item_02]
		,[subsidize_item_03]
		,[subsidize_item_04]
		,[subsidize_item_05]
		,[subsidize_item_06]
		,[subsidize_item_07]
		,[subsidize_item_08]
		,[subsidize_item_09]
		,[subsidize_item_10]
		,[subsidize_fee_01]
		,[subsidize_fee_02]
		,[subsidize_fee_03]
		,[subsidize_fee_04]
		,[subsidize_fee_05]
		,[subsidize_fee_06]
		,[subsidize_fee_07]
		,[subsidize_fee_08]
		,[subsidize_fee_09]
		,[subsidize_fee_10]
		,[joined_scheme]
		,0
		FROM [SDSPPracticeFee] WITH (NOLOCK)
		WHERE joined_scheme IS NOT NULL
			AND (@Professional IS NULL OR [service_category_code_SD] = @Professional)
			AND (@DistrictList IS NULL OR EXISTS (SELECT 1 FROM #DistrictList WHERE DistrictBoard = district_board_shortname_SD))
			AND (@ServiceProvideName IS NULL OR (sp_name LIKE @ServiceProvideName ESCAPE '\' OR sp_chi_name LIKE @ServiceProvideName ESCAPE '\'))
			AND (@PracticeName IS NULL OR (practice_name LIKE @PracticeName ESCAPE '\' OR practice_name_chi LIKE @PracticeName ESCAPE '\'))
			AND (@PracticeAddress IS NULL OR (address_eng LIKE @PracticeAddress ESCAPE '\' OR address_chi LIKE @PracticeAddress ESCAPE '\'))


	IF @subsidize_items IS NOT NULL
	BEGIN

		INSERT @tblSearch_Group
			(Search_Group)
				SELECT ITEM FROM func_split_string(@subsidize_items, @delimiter)

		-- ------------------------------------------------------------------------------------------------------
		-- Prepare
		-- ------------------------------------------------------------------------------------------------------

		INSERT @tblSubsidize_Item_Column_Name
		SELECT DISTINCT
			SDSG.Scheme_Code,
			SDS.Display_Seq,
			'1',
			'AND'
			,NULL
		FROM 
			SDSubsidizeGroup SDSG WITH (NOLOCK)
				INNER JOIN @tblSearch_Group tblSG
					ON SDSG.Search_Group = tblSG.Search_Group
				INNER JOIN SDScheme SDS WITH (NOLOCK)
					ON SDSG.Scheme_Code = SDS.Scheme_Code

		INSERT @tblSubsidize_Item_Column_Name
		SELECT 
			SDSG.Scheme_Code,
			SDS.Display_Seq,
			'2',
			'OR',
			SDSG.Subsidize_Item_Column_Name
		FROM 
			SDSubsidizeGroup SDSG WITH (NOLOCK)
				INNER JOIN @tblSearch_Group tblSG
					ON SDSG.Search_Group = tblSG.Search_Group
				INNER JOIN SDScheme SDS WITH (NOLOCK)
					ON SDSG.Scheme_Code = SDS.Scheme_Code
		ORDER BY SDS.Display_Seq, SDSG.Subsidize_Item_Column_Name

		INSERT @tblSubsidize_Item_Column_Name
		SELECT DISTINCT
			SDSG.Scheme_Code,
			SDS.Display_Seq,
			'3',
			NULL,
			NULL
		FROM 
			SDSubsidizeGroup SDSG WITH (NOLOCK)
				INNER JOIN @tblSearch_Group tblSG
					ON SDSG.Search_Group = tblSG.Search_Group
				INNER JOIN SDScheme SDS WITH (NOLOCK)
					ON SDSG.Scheme_Code = SDS.Scheme_Code


		DECLARE @Scheme_Code VARCHAR(10)
		DECLARE @Subsidize_Code_Combination VARCHAR(100)
		DECLARE @Subsidize_Item_Column_Name VARCHAR(20)
		DECLARE @Subsidize_Item_Count INTEGER

		DECLARE Search_Discount_Cursor CURSOR FOR 
				-- Example : EVSSHSIVSS		EPV|EQIV	subsidize_fee_10	2
				SELECT
					Scheme_Code,
					Subsidize_Code_Combination,
					Subsidize_Item_Column_Name,
					(LEN(Subsidize_Code_Combination) - LEN(REPLACE(Subsidize_Code_Combination,'|',''))) + 1
				FROM 
					SDSubsidizeGroup SDSG WITH (NOLOCK)
				WHERE 
					EXISTS
						(SELECT DISTINCT
							I_SDSG.Scheme_Code
						FROM 
							SDSubsidizeGroup I_SDSG WITH (NOLOCK)
						WHERE EXISTS
							(SELECT Search_Group FROM @tblSearch_Group WHERE Search_Group = I_SDSG.Search_Group)
							AND I_SDSG.Scheme_Code = SDSG.Scheme_Code
						)
					AND Subsidize_Code_Combination IS NOT NULL

		OPEN Search_Discount_Cursor

		FETCH NEXT FROM Search_Discount_Cursor INTO @Scheme_Code, @Subsidize_Code_Combination, @Subsidize_Item_Column_Name, @Subsidize_Item_Count

		WHILE @@FETCH_STATUS = 0
		BEGIN

			IF (SELECT 
					COUNT(1)
				FROM 
					SDSubsidizeGroup SDSG WITH (NOLOCK)
				WHERE 
					Scheme_Code = @Scheme_Code
					AND EXISTS (SELECT ITEM FROM func_split_string(@Subsidize_Code_Combination,'|') WHERE ITEM = SDSG.Subsidize_Code) 
					) = @Subsidize_Item_Count
			BEGIN
				INSERT @tblSubsidize_Item_Column_Name
				SELECT 
					@Scheme_Code,
					SDS.Display_Seq,
					'2',
					'OR',
					@Subsidize_Item_Column_Name
				FROM 
					SDScheme SDS WITH (NOLOCK)
				WHERE
					Scheme_Code = @Scheme_Code
			END
			
		FETCH NEXT FROM Search_Discount_Cursor INTO @Scheme_Code, @Subsidize_Code_Combination, @Subsidize_Item_Column_Name, @Subsidize_Item_Count
		END

		CLOSE Search_Discount_Cursor
		DEALLOCATE Search_Discount_Cursor
		
		--select @RecordLimit
		IF (SELECT COUNT(1) FROM @tblSubsidize_Item_Column_Name WHERE Subsidize_Item_Column_Name = 'subsidize_item_01') > 0
		BEGIN
			SET @subsidize_item_01 = 'subsidize_item_01'
		END

		IF (SELECT COUNT(1) FROM @tblSubsidize_Item_Column_Name WHERE Subsidize_Item_Column_Name = 'subsidize_item_02') > 0
		BEGIN
			SET @subsidize_item_02 = 'subsidize_item_02'
		END

		IF (SELECT COUNT(1) FROM @tblSubsidize_Item_Column_Name WHERE Subsidize_Item_Column_Name = 'subsidize_item_03') > 0
		BEGIN
			SET @subsidize_item_03 = 'subsidize_item_03'
		END

		IF (SELECT COUNT(1) FROM @tblSubsidize_Item_Column_Name WHERE Subsidize_Item_Column_Name = 'subsidize_item_04') > 0
		BEGIN
			SET @subsidize_item_04 = 'subsidize_item_04'
		END

		IF (SELECT COUNT(1) FROM @tblSubsidize_Item_Column_Name WHERE Subsidize_Item_Column_Name = 'subsidize_item_05') > 0
		BEGIN
			SET @subsidize_item_05 = 'subsidize_item_05'
		END

		IF (SELECT COUNT(1) FROM @tblSubsidize_Item_Column_Name WHERE Subsidize_Item_Column_Name = 'subsidize_item_06') > 0
		BEGIN
			SET @subsidize_item_06 = 'subsidize_item_06'
		END

		IF (SELECT COUNT(1) FROM @tblSubsidize_Item_Column_Name WHERE Subsidize_Item_Column_Name = 'subsidize_item_07') > 0
		BEGIN
			SET @subsidize_item_07 = 'subsidize_item_07'
		END

		IF (SELECT COUNT(1) FROM @tblSubsidize_Item_Column_Name WHERE Subsidize_Item_Column_Name = 'subsidize_item_08') > 0
		BEGIN
			SET @subsidize_item_08 = 'subsidize_item_08'
		END

		IF (SELECT COUNT(1) FROM @tblSubsidize_Item_Column_Name WHERE Subsidize_Item_Column_Name = 'subsidize_item_09') > 0
		BEGIN
			SET @subsidize_item_09 = 'subsidize_item_09'
		END

		IF (SELECT COUNT(1) FROM @tblSubsidize_Item_Column_Name WHERE Subsidize_Item_Column_Name = 'subsidize_item_10') > 0
		BEGIN
			SET @subsidize_item_10 = 'subsidize_item_10'
		END

		SET @SQL_STAT_PATTERN_2 =
		(SELECT 
			REPLACE(
			(SELECT 
				CASE
					WHEN Operator = 'AND' THEN Operator + ' ('
		
					WHEN Operator = 'OR' THEN  ' ((['+Subsidize_Item_Column_Name+'] is not null and @IN_'+Subsidize_Item_Column_Name+' = '''+Subsidize_Item_Column_Name+''') OR @IN_'+Subsidize_Item_Column_Name+' IS NULL) ' + Operator
		
					-- Example: (([subsidize_item_01] is not null and @IN_subsidize_item_01 = ''subsidize_item_01'') OR @IN_subsidize_item_01 IS NULL)
		
					ELSE ') '
				END		
			FROM @tblSubsidize_Item_Column_Name 
			ORDER BY Scheme_Display_Seq, Operator_Priority
			FOR XML PATH('')),
			'OR)',
			')')
		)
		
	END

	UPDATE #SDSPPracticeFee	SET Subsidy_with_fee += CASE WHEN subsidize_fee_02 IS NOT NULL THEN 1 ELSE 0  END
	UPDATE #SDSPPracticeFee	SET Subsidy_with_fee += CASE WHEN subsidize_fee_03 IS NOT NULL THEN 1 ELSE 0  END
	UPDATE #SDSPPracticeFee SET Subsidy_with_fee += CASE WHEN subsidize_fee_04 IS NOT NULL THEN 1 ELSE 0  END
	UPDATE #SDSPPracticeFee SET Subsidy_with_fee += CASE WHEN subsidize_fee_05 IS NOT NULL THEN 1 ELSE 0  END
	UPDATE #SDSPPracticeFee SET Subsidy_with_fee += CASE WHEN subsidize_fee_06 IS NOT NULL THEN 1 ELSE 0  END
	UPDATE #SDSPPracticeFee SET Subsidy_with_fee += CASE WHEN subsidize_fee_07 IS NOT NULL THEN 1 ELSE 0  END
	UPDATE #SDSPPracticeFee SET Subsidy_with_fee += CASE WHEN subsidize_fee_08 IS NOT NULL THEN 1 ELSE 0  END
	UPDATE #SDSPPracticeFee SET Subsidy_with_fee += CASE WHEN subsidize_fee_09 IS NOT NULL THEN 1 ELSE 0  END
	UPDATE #SDSPPracticeFee SET Subsidy_with_fee += CASE WHEN subsidize_fee_10 IS NOT NULL THEN 1 ELSE 0  END

	SET @SQL_STAT_PATTERN_1 = 
		N'SELECT TOP ' + @RecordLimit + ' ' +
			N'
			[sp_id]
			,[practice_display_seq]
			,[sp_name]
			,isnull([sp_chi_name], '''') as [sp_chi_name]
			,[practice_name]
			,isnull([practice_name_chi], '''') as [practice_name_chi]
			,isnull([phone_daytime], '''') as [phone_daytime]
			,[service_category_code_SD]
			,[district_code] 
			,[district_board_shortname_SD]
			,[area_code]
			,[address_eng]
			,[district_name]
			,[district_board]
			,[area_name]
			,isnull([address_chi], '''') as [address_chi]
			,[district_name_chi]
			,[district_board_chi]
			,[area_name_chi]
			,isnull([joined_scheme], '''') as [joined_scheme]
			,[subsidize_fee_01]
			,[subsidize_fee_02]
			,[subsidize_fee_03]
			,[subsidize_fee_04]
			,[subsidize_fee_05]
			,[subsidize_fee_06]
			,[subsidize_fee_07]
			,[subsidize_fee_08]
			,[subsidize_fee_09]
			,[subsidize_fee_10]
			,case [subsidize_fee_01] when @IN_Symbol_For_No_QIV_Service_Fee then @IN_Service_fee_For_Sorting else [subsidize_fee_01] end as [subsidize_fee_01_sort]
			,case [subsidize_fee_02] when @IN_Symbol_For_No_QIV_Service_Fee then @IN_Service_fee_For_Sorting else [subsidize_fee_02] end  as [subsidize_fee_02_sort]
			,case [subsidize_fee_03] when @IN_Symbol_For_No_QIV_Service_Fee then @IN_Service_fee_For_Sorting else [subsidize_fee_03] end  as [subsidize_fee_03_sort]
			,case [subsidize_fee_04] when @IN_Symbol_For_No_QIV_Service_Fee then @IN_Service_fee_For_Sorting else [subsidize_fee_04] end  as [subsidize_fee_04_sort]
			,case [subsidize_fee_05] when @IN_Symbol_For_No_QIV_Service_Fee then @IN_Service_fee_For_Sorting else [subsidize_fee_05] end  as [subsidize_fee_05_sort]
			,case [subsidize_fee_06] when @IN_Symbol_For_No_QIV_Service_Fee then @IN_Service_fee_For_Sorting else [subsidize_fee_06] end  as [subsidize_fee_06_sort]
			,case [subsidize_fee_07] when @IN_Symbol_For_No_QIV_Service_Fee then @IN_Service_fee_For_Sorting else [subsidize_fee_07] end  as [subsidize_fee_07_sort]
			,case when [subsidize_fee_01] IN (@IN_Symbol_For_No_QIV_Service_Fee, @IN_Symbol_For_No_QIV_No_LAIV_Service_Fee, @IN_Symbol_For_No_QIV_No_TIV_Service_Fee) then @IN_Service_fee_For_Sorting else [subsidize_fee_01] end  as [subsidize_fee_01_sort]
			,case when [subsidize_fee_02] IN (@IN_Symbol_For_No_QIV_Service_Fee, @IN_Symbol_For_No_QIV_No_LAIV_Service_Fee, @IN_Symbol_For_No_QIV_No_TIV_Service_Fee) then @IN_Service_fee_For_Sorting else [subsidize_fee_02] end  as [subsidize_fee_02_sort]
			,case when [subsidize_fee_03] IN (@IN_Symbol_For_No_QIV_Service_Fee, @IN_Symbol_For_No_QIV_No_LAIV_Service_Fee, @IN_Symbol_For_No_QIV_No_TIV_Service_Fee) then @IN_Service_fee_For_Sorting else [subsidize_fee_03] end  as [subsidize_fee_03_sort]
			,case when [subsidize_fee_04] IN (@IN_Symbol_For_No_QIV_Service_Fee, @IN_Symbol_For_No_QIV_No_LAIV_Service_Fee, @IN_Symbol_For_No_QIV_No_TIV_Service_Fee) then @IN_Service_fee_For_Sorting else [subsidize_fee_04] end  as [subsidize_fee_04_sort]
			,case when [subsidize_fee_05] IN (@IN_Symbol_For_No_QIV_Service_Fee, @IN_Symbol_For_No_QIV_No_LAIV_Service_Fee, @IN_Symbol_For_No_QIV_No_TIV_Service_Fee) then @IN_Service_fee_For_Sorting else [subsidize_fee_05] end  as [subsidize_fee_05_sort]
			,case when [subsidize_fee_06] IN (@IN_Symbol_For_No_QIV_Service_Fee, @IN_Symbol_For_No_QIV_No_LAIV_Service_Fee, @IN_Symbol_For_No_QIV_No_TIV_Service_Fee) then @IN_Service_fee_For_Sorting else [subsidize_fee_06] end  as [subsidize_fee_06_sort]
			,case when [subsidize_fee_07] IN (@IN_Symbol_For_No_QIV_Service_Fee, @IN_Symbol_For_No_QIV_No_LAIV_Service_Fee, @IN_Symbol_For_No_QIV_No_TIV_Service_Fee) then @IN_Service_fee_For_Sorting else [subsidize_fee_07] end  as [subsidize_fee_07_sort]
			,case when [subsidize_fee_08] IN (@IN_Symbol_For_No_QIV_Service_Fee, @IN_Symbol_For_No_QIV_No_LAIV_Service_Fee, @IN_Symbol_For_No_QIV_No_TIV_Service_Fee) then @IN_Service_fee_For_Sorting else [subsidize_fee_08] end  as [subsidize_fee_08_sort]
			,case when [subsidize_fee_09] IN (@IN_Symbol_For_No_QIV_Service_Fee, @IN_Symbol_For_No_QIV_No_LAIV_Service_Fee, @IN_Symbol_For_No_QIV_No_TIV_Service_Fee) then @IN_Service_fee_For_Sorting else [subsidize_fee_09] end  as [subsidize_fee_09_sort]
			,case when [subsidize_fee_10] IN (@IN_Symbol_For_No_QIV_Service_Fee, @IN_Symbol_For_No_QIV_No_LAIV_Service_Fee, @IN_Symbol_For_No_QIV_No_TIV_Service_Fee) then @IN_Service_fee_For_Sorting else [subsidize_fee_10] end  as [subsidize_fee_10_sort]
			,case when [subsidize_fee_01] = @IN_Symbol_For_No_QIV_Service_Fee then ''2'' when [subsidize_fee_01] = @IN_Symbol_For_No_QIV_No_LAIV_Service_Fee then ''3'' when [subsidize_fee_01] = @IN_Symbol_For_No_QIV_No_TIV_Service_Fee then ''4''  when [subsidize_fee_01] = @IN_Symbol_For_To_Be_Provide_Service_Fee then ''5'' when [subsidize_fee_01] = @IN_Symbol_For_NA_Service_Fee then ''6'' when [subsidize_fee_01] IS NULL then ''7'' else ''1'' end as [subsidize_fee_01_sort_type]
			,case when [subsidize_fee_02] = @IN_Symbol_For_No_QIV_Service_Fee then ''2'' when [subsidize_fee_02] = @IN_Symbol_For_No_QIV_No_LAIV_Service_Fee then ''3'' when [subsidize_fee_02] = @IN_Symbol_For_No_QIV_No_TIV_Service_Fee then ''4''  when [subsidize_fee_02] = @IN_Symbol_For_To_Be_Provide_Service_Fee then ''5'' when [subsidize_fee_02] = @IN_Symbol_For_NA_Service_Fee then ''6'' when [subsidize_fee_02] IS NULL then ''7'' else ''1'' end as [subsidize_fee_02_sort_type]
			,case when [subsidize_fee_03] = @IN_Symbol_For_No_QIV_Service_Fee then ''2'' when [subsidize_fee_03] = @IN_Symbol_For_No_QIV_No_LAIV_Service_Fee then ''3'' when [subsidize_fee_03] = @IN_Symbol_For_No_QIV_No_TIV_Service_Fee then ''4''  when [subsidize_fee_03] = @IN_Symbol_For_To_Be_Provide_Service_Fee then ''5'' when [subsidize_fee_03] = @IN_Symbol_For_NA_Service_Fee then ''6'' when [subsidize_fee_03] IS NULL then ''7'' else ''1'' end as [subsidize_fee_03_sort_type]
			,case when [subsidize_fee_04] = @IN_Symbol_For_No_QIV_Service_Fee then ''2'' when [subsidize_fee_04] = @IN_Symbol_For_No_QIV_No_LAIV_Service_Fee then ''3'' when [subsidize_fee_04] = @IN_Symbol_For_No_QIV_No_TIV_Service_Fee then ''4''  when [subsidize_fee_04] = @IN_Symbol_For_To_Be_Provide_Service_Fee then ''5'' when [subsidize_fee_04] = @IN_Symbol_For_NA_Service_Fee then ''6'' when [subsidize_fee_04] IS NULL then ''7'' else ''1'' end as [subsidize_fee_04_sort_type]
			,case when [subsidize_fee_05] = @IN_Symbol_For_No_QIV_Service_Fee then ''2'' when [subsidize_fee_05] = @IN_Symbol_For_No_QIV_No_LAIV_Service_Fee then ''3'' when [subsidize_fee_05] = @IN_Symbol_For_No_QIV_No_TIV_Service_Fee then ''4''  when [subsidize_fee_05] = @IN_Symbol_For_To_Be_Provide_Service_Fee then ''5'' when [subsidize_fee_05] = @IN_Symbol_For_NA_Service_Fee then ''6'' when [subsidize_fee_05] IS NULL then ''7'' else ''1'' end as [subsidize_fee_05_sort_type]
			,case when [subsidize_fee_06] = @IN_Symbol_For_No_QIV_Service_Fee then ''2'' when [subsidize_fee_06] = @IN_Symbol_For_No_QIV_No_LAIV_Service_Fee then ''3'' when [subsidize_fee_06] = @IN_Symbol_For_No_QIV_No_TIV_Service_Fee then ''4''  when [subsidize_fee_06] = @IN_Symbol_For_To_Be_Provide_Service_Fee then ''5'' when [subsidize_fee_06] = @IN_Symbol_For_NA_Service_Fee then ''6'' when [subsidize_fee_06] IS NULL then ''7'' else ''1'' end as [subsidize_fee_06_sort_type]
			,case when [subsidize_fee_07] = @IN_Symbol_For_No_QIV_Service_Fee then ''2'' when [subsidize_fee_07] = @IN_Symbol_For_No_QIV_No_LAIV_Service_Fee then ''3'' when [subsidize_fee_07] = @IN_Symbol_For_No_QIV_No_TIV_Service_Fee then ''4''  when [subsidize_fee_07] = @IN_Symbol_For_To_Be_Provide_Service_Fee then ''5'' when [subsidize_fee_07] = @IN_Symbol_For_NA_Service_Fee then ''6'' when [subsidize_fee_07] IS NULL then ''7'' else ''1'' end as [subsidize_fee_07_sort_type]
			,case when [subsidize_fee_08] = @IN_Symbol_For_No_QIV_Service_Fee then ''2'' when [subsidize_fee_08] = @IN_Symbol_For_No_QIV_No_LAIV_Service_Fee then ''3'' when [subsidize_fee_08] = @IN_Symbol_For_No_QIV_No_TIV_Service_Fee then ''4''  when [subsidize_fee_08] = @IN_Symbol_For_To_Be_Provide_Service_Fee then ''5'' when [subsidize_fee_08] = @IN_Symbol_For_NA_Service_Fee then ''6'' when [subsidize_fee_08] IS NULL then ''7'' else ''1'' end as [subsidize_fee_08_sort_type]
			,case when [subsidize_fee_09] = @IN_Symbol_For_No_QIV_Service_Fee then ''2'' when [subsidize_fee_09] = @IN_Symbol_For_No_QIV_No_LAIV_Service_Fee then ''3'' when [subsidize_fee_09] = @IN_Symbol_For_No_QIV_No_TIV_Service_Fee then ''4''  when [subsidize_fee_09] = @IN_Symbol_For_To_Be_Provide_Service_Fee then ''5'' when [subsidize_fee_09] = @IN_Symbol_For_NA_Service_Fee then ''6'' when [subsidize_fee_09] IS NULL then ''7'' else ''1'' end as [subsidize_fee_09_sort_type]
			,case when [subsidize_fee_10] = @IN_Symbol_For_No_QIV_Service_Fee then ''2'' when [subsidize_fee_10] = @IN_Symbol_For_No_QIV_No_LAIV_Service_Fee then ''3'' when [subsidize_fee_10] = @IN_Symbol_For_No_QIV_No_TIV_Service_Fee then ''4''  when [subsidize_fee_10] = @IN_Symbol_For_To_Be_Provide_Service_Fee then ''5'' when [subsidize_fee_10] = @IN_Symbol_For_NA_Service_Fee then ''6'' when [subsidize_fee_10] IS NULL then ''7'' else ''1'' end as [subsidize_fee_10_sort_type]
			,[Subsidy_with_fee]		
		FROM [#SDSPPracticeFee]
		WHERE joined_scheme IS NOT NULL
			AND (@IN_Professional IS NULL OR [service_category_code_SD] = @IN_Professional)
			AND (@IN_DistrictList IS NULL OR EXISTS (SELECT 1 FROM #DistrictList WHERE DistrictBoard = district_board_shortname_SD))
			AND (@IN_ServiceProvideName IS NULL OR (sp_name LIKE @IN_ServiceProvideName ESCAPE ''\'' OR sp_chi_name LIKE @IN_ServiceProvideName ESCAPE ''\''))
			AND (@IN_PracticeName IS NULL OR (practice_name LIKE @IN_PracticeName ESCAPE ''\'' OR practice_name_chi LIKE @IN_PracticeName ESCAPE ''\''))
			AND (@IN_PracticeAddress IS NULL OR (address_eng LIKE @IN_PracticeAddress ESCAPE ''\'' OR address_chi LIKE @IN_PracticeAddress ESCAPE ''\''))'

	--SET @SQL_STAT_PATTERN_2 = NULL

	

	IF @language = 'ZH-TW' BEGIN
		SET @SQL_STAT_PATTERN_3 =
			N' ORDER BY [district_board_chi] ASC, [sp_chi_name] ASC'
	END
	ELSE 
	BEGIN
		SET @SQL_STAT_PATTERN_3 =
			N' ORDER BY [district_board_shortname_SD] ASC, [sp_name] ASC'
	END

	SET @SQL_STAT = @SQL_STAT_PATTERN_1 + ISNULL(@SQL_STAT_PATTERN_2,'') + @SQL_STAT_PATTERN_3
		

	SET @PARM_DEFINITION = 
		N'@IN_Professional varchar(5), 
		@IN_subsidize_item_01 varchar(20), 
		@IN_subsidize_item_02 varchar(20), 
		@IN_subsidize_item_03 varchar(20), 
		@IN_subsidize_item_04 varchar(20), 
		@IN_subsidize_item_05 varchar(20), 
		@IN_subsidize_item_06 varchar(20), 
		@IN_subsidize_item_07 varchar(20), 
		@IN_subsidize_item_08 varchar(20), 
		@IN_subsidize_item_09 varchar(20), 
		@IN_subsidize_item_10 varchar(20),
		@IN_Symbol_For_No_QIV_Service_Fee char(4),
		@IN_Symbol_For_No_QIV_No_LAIV_Service_Fee char(1),
		@IN_Symbol_For_No_QIV_No_TIV_Service_Fee char(2),
		@IN_Symbol_For_To_Be_Provide_Service_Fee char(5),
		@IN_Symbol_For_NA_Service_Fee varchar(4),
		@IN_Service_fee_For_Sorting varchar(5),
		@IN_DistrictList varchar(200),
		@IN_ServiceProvideName nvarchar(200),
		@IN_PracticeName nvarchar(200),
		@IN_PracticeAddress nvarchar(200)'

	-- ---------------------------------------------
	-- Result Table 1
	-- ---------------------------------------------
	EXECUTE sp_executesql 
		@SQL_STAT, 
		@PARM_DEFINITION, 
		@IN_Professional = @Professional,
		@IN_subsidize_item_01 = @subsidize_item_01,
		@IN_subsidize_item_02 = @subsidize_item_02,
		@IN_subsidize_item_03 = @subsidize_item_03,
		@IN_subsidize_item_04 = @subsidize_item_04,
		@IN_subsidize_item_05 = @subsidize_item_05,
		@IN_subsidize_item_06 = @subsidize_item_06,
		@IN_subsidize_item_07 = @subsidize_item_07,
		@IN_subsidize_item_08 = @subsidize_item_08,
		@IN_subsidize_item_09 = @subsidize_item_09,
		@IN_subsidize_item_10 = @subsidize_item_10,
		@IN_Symbol_For_No_QIV_Service_Fee = @Symbol_For_No_QIV_Service_Fee,
		@IN_Symbol_For_No_QIV_No_LAIV_Service_Fee = @Symbol_For_No_QIV_No_LAIV_Service_Fee,
		@IN_Symbol_For_No_QIV_No_TIV_Service_Fee = @Symbol_For_No_QIV_No_TIV_Service_Fee,
		@IN_Symbol_For_To_Be_Provide_Service_Fee = @Symbol_For_To_Be_Provide_Service_Fee,
		@IN_Symbol_For_NA_Service_Fee  = @Symbol_For_NA_Service_Fee,
		@IN_Service_fee_For_Sorting = @Service_fee_For_Sorting,
		@IN_DistrictList = @DistrictList,
		@IN_ServiceProvideName = @ServiceProvideName,
		@IN_PracticeName = @PracticeName,
		@IN_PracticeAddress = @PracticeAddress

	DROP TABLE #DistrictList
	DROP TABLE #SDSPPracticeFee

END
GO

GRANT EXECUTE ON [dbo].[proc_HCSD_get_PracticeList_withFee] TO HCPUBLIC
GO
