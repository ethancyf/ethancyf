IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCSD_SDSPPracticeFee_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCSD_SDSPPracticeFee_add]
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
-- CR No.			CRE16-022 (SDIR Remark)
-- Modified by:		CHRIS YIM
-- Modified date:	01 Apr 2020
-- Description:		Enlarge the length of column [district_board]
-- =============================================
-- =============================================
-- Modification History
-- CR No.			CRE16-022 (SDIR Remark)
-- Modified by:		CHRIS YIM
-- Modified date:	31 Mar 2020
-- Description:		Add columns [Mobile_Clinic],[Remarks_Desc] & [Remarks_Desc_Chi]
-- =============================================
-- =============================================
-- Modification History
-- CR No.			CRE19-030 (Revamp of SDIR and VBE)
-- Modified by:		CHRIS YIM
-- Modified date:	17 Mar 2020
-- Description:		Revised English Desc on District Board
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE19-001-02
-- Modified by:		Lawrence TSANG
-- Modified date:	12 Aug 2019
-- Description:		Handle multiple [SDSubsidizeGroup].[Subsidize_Code_Secondary]
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			INT18-0005
-- Modified by:		Koala CHENG
-- Modified date:	14 Jun 2018
-- Description:		Exclude suspended scheme [SchemeInformation]
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE17-005
-- Modified by:		Marco CHOI
-- Modified date:	15 Nov 2017
-- Description:		Enhance SDIR searching function
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE16-026
-- Modified by:		Dickson Law
-- Modified date:	19 Sep 2017
-- Description:		Fix the display of PCV13 to "To be Provided" when all other subsidies under VSS are "To be Provided".
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
-- CR No.:			CRE16-002
-- Modified by:		Lawrence TSANG
-- Modified date:	11 August 2016
-- Description:		VSS revamp
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE13-019-05
-- Modified by:		Winnie SUEN
-- Modified date:	22 Sep 2015
-- Description:		1. Change [@area_name_chi] to nCHAR to support simplified chinese 
-- =============================================
-- =============================================
-- Modification History
-- CR No.:		CRE15-004
-- Modified by:	Karl LAM
-- Modified date: 02 Jul 2015
-- Description:	1. Handle BO_Subsidize_Code_Combination and BO_Subsidize_Code_Secondary in SDFeeColumnMapping
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Winnie SUEN
-- Modified date: 21 Apr 2015
-- Description:	1. Refine District Structure
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
-- 
-- Modification History
-- CR No.:			CRE13-001
-- Modified by:		Koala CHENG
-- Modified date:	16 Apr 2013
-- Description:		Exclude the practice enrolled EHAPP (Record Status = 'A')
-- =============================================
-- =============================================
-- Author:	Chowly Li
-- Modified date: 29 Dec 2009
-- Description:	  Modified to view the scheme status of service provider (schemeinformation) 
-- =============================================
-- =============================================
-- Author:	Chowly Li
-- Create date: 9 Sept 2009
-- Description:	 Retreive Practice and vaccine fee for service directory 
-- Tables: 	
--		SDSPPracticeFee
--		SDprofessionalMapping
-- 		SDFeeColumnMapping
--		SDSchemeColumnMapping
--		SDprofessionalMapping 
--		SDDistrictBoard
--		ServiceProvider
--		practice
--		professional
--		practiceschemeinfo
--		district
--		district_area
--		schemeinformation
--
--		#tmpPractice
-- Stored proc: cpi_get_address_detail
--		(table: address_detail, elderly_home_table)
-- =============================================

CREATE PROCEDURE [dbo].[proc_HCSD_SDSPPracticeFee_add] 
AS BEGIN

SET ANSI_NULLS ON
SET NOCOUNT ON

-- =============================================
-- Constant Settings
-- =============================================

DECLARE @SDIR_ExcludePracticeForEHAPP BIT = 1
DECLARE @Symbol_For_No_Service_Fee CHAR(1) = '*'
DECLARE @Symbol_For_No_Provide_Service CHAR(1) = '*'
DECLARE @Symbol_ServiceFeeToBeProvided VARCHAR(15) = '{TBP}'
DECLARE @Symbol_ServiceFeeNA VARCHAR(4) = '{NA}'

DECLARE @Symbol_ServiceFeeSecondary table (Score int, Symbol VARCHAR(15))
INSERT INTO @Symbol_ServiceFeeSecondary VALUES (1, '*'), (2, '**'), (3, '*/**')


-- =============================================
-- Settings retrieved from SystemResource
-- =============================================

DECLARE @NonClinicEn VARCHAR(100)
DECLARE @NonClinicCh NVARCHAR(100)

SELECT @NonClinicEn = Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'NonClinic'
SELECT @NonClinicCh = Chinese_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'NonClinic'


-- =============================================
-- Part 1: Retrieve practice and service provider details
-- =============================================

-- ---------------------------------------------
-- Declaration
-- ---------------------------------------------

create table #tmpPractice ( 
	sp_id							CHAR(8) NOT NULL
	,practice_display_seq			SMALLINT NOT NULL
	,sp_name						VARCHAR(100)
	,sp_chi_name					NVARCHAR(100)
	,practice_name					NVARCHAR(150)
	,practice_name_chi				NVARCHAR(150)
	,phone_daytime					VARCHAR(20)
	,service_category_code_SD		CHAR(5)
	,district_code					CHAR(4)
	,district_board_shortname_SD	CHAR(5)
	,area_code						CHAR(1)

	,address_eng 					VARCHAR(255)
	,district_name					CHAR(15)
	,district_board					VARCHAR(20)
	,area_name						CHAR(50)
	,address_chi 					NVARCHAR(255)
	,district_name_chi				NCHAR(30)
	,district_board_chi				NCHAR(30)
	,area_name_chi					NVARCHAR(50)

	,Room							NVARCHAR(5)
	,[Floor]						NVARCHAR(3)
	,[Block]						NVARCHAR(3)
	,Building						VARCHAR(255)
	,Building_Chi					NVARCHAR(255)
	,Address_Code					INT

	,subsidize_item_01				CHAR(1)
	,subsidize_item_02				CHAR(1)
	,subsidize_item_03				CHAR(1)
	,subsidize_item_04				CHAR(1)
	,subsidize_item_05				CHAR(1)
	,subsidize_item_06				CHAR(1)
	,subsidize_item_07				CHAR(1)
	,subsidize_item_08				CHAR(1)
	,subsidize_item_09				CHAR(1)
	,subsidize_item_10				CHAR(1)	
	,subsidize_fee_01				VARCHAR(15)
	,subsidize_fee_02				VARCHAR(15)
	,subsidize_fee_03				VARCHAR(15)
	,subsidize_fee_04				VARCHAR(15)
	,subsidize_fee_05				VARCHAR(15)
	,subsidize_fee_06				VARCHAR(15)
	,subsidize_fee_07				VARCHAR(15)
	,subsidize_fee_08				VARCHAR(15)
	,subsidize_fee_09				VARCHAR(15)
	,subsidize_fee_10				VARCHAR(15)
	,joined_scheme					VARCHAR(100)

	,[Mobile_Clinic]				CHAR(1)
	,[Non_Clinic]					CHAR(1)
	,[Remarks_Desc]					NVARCHAR(200)
	,[Remarks_Desc_Chi]				NVARCHAR(200)	
	
)


-- ---------------------------------------------
-- Retrieve data
-- ---------------------------------------------

EXEC [proc_SymmetricKey_open]

insert into #tmpPractice 
(
	sp_id
	,practice_display_seq
	,sp_name 
	,sp_chi_name
	,practice_name 
	,practice_name_chi 
	,phone_daytime	
	,service_category_code_SD 
	,district_code  

	,Room
	,[Floor]
	,[Block] 
	,Building
	,Building_Chi
	,Address_Code

	,[Non_Clinic]
	,[Mobile_Clinic]
	,[Remarks_Desc]					
	,[Remarks_Desc_Chi]				
)
select sp.sp_id, 
	p.display_seq
	,convert(VARCHAR(100), DecryptByKey(sp.encrypt_field2))
	,convert(NVARCHAR(100), DecryptByKey(sp.encrypt_field3))
	,p.practice_name, p.practice_name_chi 
	,p.phone_daytime 
	,SDPrMap.service_category_code_SD 

	,p.district 
	,p.Room
	,p.Floor
	,p.Block
	,p.Building
	,p.Building_Chi
	,p.Address_Code

	,'N' AS [Non_Clinic]
	,p.Mobile_Clinic AS [Mobile_Clinic]
	,ISNULL(p.Remarks_Desc,'') AS [Remarks_Desc]
	,ISNULL(p.Remarks_Desc_Chi,'') AS [Remarks_Desc_Chi]

	from practice p LEFT JOIN (SELECT DISTINCT SP_ID, Practice_Display_Seq, Scheme_Code, Record_Status FROM PracticeSchemeInfo ) psi  
		ON  p.sp_id = psi.sp_id AND p.display_seq = psi.Practice_Display_Seq 
			AND  psi.scheme_code = 'EHAPP' AND  psi.record_Status = 'A'-- Indicate the practice joined EHAPP
			AND @SDIR_ExcludePracticeForEHAPP = 1
		, serviceprovider sp, professional pr,
		SDprofessionalMapping SDprMap 
	where p.sp_id = sp.sp_id and p.sp_id = pr.sp_id 
		and p.professional_seq = pr.professional_seq
		and sp.record_status = 'A' and p.record_status ='A'  -- active practice and SP
		and sp.sp_id not in (select sp_id from SPExceptionList) -- not in exception list
		and SDprMap.service_category_code = pr.service_category_code  -- join to retrieve the SD professional
		AND psi.scheme_code IS NULL -- Filter out the practice which has 'EHAPP' indicator

EXEC [proc_SymmetricKey_close]

declare	
	@sp_id CHAR(8)
	,@practice_display_seq smallint
	,@district_code CHAR(5)
	,@Room NVARCHAR(5)
	,@Floor NVARCHAR(3)
	,@Block NVARCHAR(3)
	,@Building VARCHAR(255)
	,@Building_Chi NVARCHAR(255)
	,@Address_Code int
	,@eh_eng VARCHAR(255)
	,@eh_chi NVARCHAR(255)
	,@address_eng VARCHAR(255)
	,@address_chi NVARCHAR(255)
--	,district_code_addr CHAR(4)

	,@district_name	CHAR(15)
	,@district_board VARCHAR(20)
	,@area_name	CHAR(50)
	,@district_name_chi	nCHAR(30)
	,@district_board_chi	nCHAR(30)
	,@area_name_chi	nCHAR(50)

	,@district_board_shortname_SD  CHAR(5)
	,@area_code CHAR(1)


DECLARE practice_cursor cursor 
	FOR	SELECT 	sp_id 	,practice_display_seq	,district_code	,rtrim(Room),rtrim([Floor])
	 ,rtrim(Block),rtrim(Building)
	,rtrim(Building_Chi), Address_Code 
	FROM #tmpPractice 
	for update


-- ---------------------------------------------
-- Format the address into a single string
-- ---------------------------------------------

OPEN practice_cursor 

	FETCH next FROM practice_cursor INTO @sp_id, @practice_display_seq
		,@district_code	,@Room 	,@Floor ,@Block	, @Building, @Building_Chi, @Address_Code

WHILE @@Fetch_status = 0
BEGIN

	SELECT	@address_eng = '',
		@address_chi = '',
		@eh_eng = '',
		@eh_chi = '',
		@area_code =''

	if @address_code IS NOT null
	-- retrieve record from address table

		BEGIN

		exec cpi_get_address_detail   @address_code 
			, @address_eng = @address_eng  OUTPUT 
			, @address_chi = @address_chi    OUTPUT 
			, @district_code = @district_code    OUTPUT 
			, @eh_eng = @eh_eng	OUTPUT
			, @eh_chi = @eh_chi	OUTPUT

		END

	else

		BEGIN

		-- English Address
            If @room is not null and @room <>''
               set @address_eng = @address_eng + 'ROOM ' + @room + ', '


            If @floor is not null and @floor <>''
                set @address_eng = @address_eng + 'FLOOR ' + @floor+ ', '

            If @block is not null and @block <>''
                set @address_eng = @address_eng + 'BLOCK ' + @block+ ', '

            If @building is not null and @building <>''
                set @address_eng = @address_eng + @building + ', '


		-- Chin Address
            If @building_chi is not null and @building_chi <>''
			Begin
                set @address_chi = @address_chi + @building_chi

				If @block is not null and @block <>''  
					set @address_chi = @address_chi +  @block +'座'   
  
				If @floor is not null and @floor <>''  
					set @address_chi = @address_chi +  @floor + '樓'  
  
				If @room is not null and @room <>''  
					set @address_chi = @address_chi + @room + '室'   
			end
		END


	/*  format district + area */
    if @district_code is not null and @district_code <>'' 
		BEGIN 
			SELECT  
				@district_name = d.district_name, 
				@district_name_chi=d.district_chi,
				@district_board= DB.District_Board_SD, 
				@district_board_chi= DB.District_Board_SD_Chi,
				@area_code = d.district_area,
				@area_name= DA.area_name, @area_name_chi= DA.area_chi,
				@district_board_shortname_SD= DB.district_board_shortname_SD
			FROM district D
				JOIN DistrictBoard DB
					ON DB.District_Board = D.District_Board				
				JOIN District_Area DA 
					ON DB.Area_Code = DA.Area_Code
			WHERE 
				D.district_code = @district_code

			set @address_eng = @address_eng + rtrim(@district_name) +', ' + rtrim(@area_name)

			if @address_chi <>'' 
				set @address_chi = rtrim(@area_name_chi) + rtrim(@district_name_chi)  + @address_chi

		END 
		
	update 	#tmpPractice 
		set 
		district_code	= @district_code
		,district_board_shortname_SD=@district_board_shortname_SD
		,address_eng 	=@address_eng 
		,district_name	=@district_name
		,district_board =@district_board 
		,area_name	=@area_name
		,area_code = @area_code
		,address_chi 	=@address_chi 
		,district_name_chi	=@district_name_chi
		,district_board_chi	=@district_board_chi
		,area_name_chi	=@area_name_chi
		where current of practice_cursor
--		where sp_id =@sp_id 
--			and practice_display_seq = @practice_display_seq

	FETCH next FROM practice_cursor INTO @sp_id, @practice_display_seq
		,@district_code	,@Room 	,@Floor ,@Block	,@Building,@Building_Chi,@Address_Code

END


CLOSE practice_cursor 
DEALLOCATE practice_cursor


-- ---------------------------------------------
-- Patch the name of the non-clinic practice
-- ---------------------------------------------

	UPDATE
		#tmpPractice
	SET
		practice_name = practice_name + ' (' + @NonClinicEn + ')',
		practice_name_chi = CASE WHEN ISNULL(practice_name_chi, '') <> ''
								--THEN practice_name_chi + N'（' + @NonClinicCh + N'）'
								THEN practice_name_chi + N'(' + @NonClinicCh + N')'
								ELSE practice_name_chi
							END,
		[Non_Clinic] = 'Y'
	FROM
		(SELECT DISTINCT
			SP_ID,
			Practice_Display_Seq
		FROM
			PracticeSchemeInfo
		WHERE
			Record_Status = 'A'
				AND Clinic_Type = 'N'
		) T
	WHERE
		#tmpPractice.sp_id = T.SP_ID
			AND #tmpPractice.practice_display_seq = T.Practice_Display_Seq


-- =============================================
-- Part 2: Retrieve fee information
-- =============================================

-- ---------------------------------------------
-- Declaration
-- ---------------------------------------------

	CREATE TABLE #tmpPracticeSchemeInfo (
		SP_ID					CHAR(8) not null,
		Practice_Display_Seq	smallint not null,
		Scheme_Code				CHAR(10) not null,
		Subsidize_Code			CHAR(10) not null,
		Record_Status			CHAR(1),
		Provide_Service			CHAR(1),
		Service_Fee				VARCHAR(15),
		--CONSTRAINT PK_tmpPracticeSchemeInfo PRIMARY KEY (SP_ID, Subsidize_Code, Practice_Display_Seq, Scheme_Code)
	)


-- ---------------------------------------------
-- 1. Insert Data
-- ---------------------------------------------

    INSERT INTO #tmpPracticeSchemeInfo (
		SP_ID,
		Practice_Display_Seq,
		Scheme_Code,
		Subsidize_Code,
		Record_Status,
		Provide_Service,
		Service_Fee
    ) 
	SELECT
		PS.SP_ID,
		PS.Practice_Display_Seq,
		PS.Scheme_Code,
		SG.Subsidize_Code,
		ISNULL(PSI.Record_Status, 'A'),
		ISNULL(PSI.Provide_Service, 'N'),
		CASE ISNULL(PSI.Provide_Service, 'N')
			WHEN 'Y' THEN
			      CASE PSI.ProvideServiceFee
			             WHEN 'Y' THEN RIGHT('0000' + CAST(Service_Fee as VARCHAR(4)), 4)
			             WHEN 'N' THEN @Symbol_For_No_Service_Fee
			             ELSE NULL
			      END
			WHEN 'N' THEN @Symbol_For_No_Provide_Service
			ELSE NULL
		END AS [Service_Fee]
    FROM
		SDScheme S
		       INNER JOIN SDSubsidizeGroup SG
		             ON S.Scheme_Code = SG.Scheme_Code
		       -- All practice and scheme with active subsidy
		       INNER JOIN (SELECT DISTINCT SP_ID, Practice_Display_Seq, Scheme_Code FROM PracticeSchemeInfo WHERE Record_Status = 'A') PS
		                    ON SG.Scheme_Code = PS.Scheme_Code
			   INNER JOIN SchemeInformation SI
					ON PS.SP_ID = SI.SP_ID
						AND PS.Scheme_Code = SI.Scheme_Code
						AND SI.Record_Status = 'A'
		       LEFT JOIN PracticeSchemeInfo PSI
		             ON PS.SP_ID = PSI.SP_ID
		                    AND PS.Practice_Display_Seq = PSI.Practice_Display_Seq
		                    AND PS.Scheme_Code = PSI.Scheme_Code
		                    AND SG.Subsidize_Code = PSI.Subsidize_Code
		                    AND PSI.Record_Status = 'A'
	WHERE
		S.Record_Status = 'A'
		       AND SG.Record_Status = 'A'


-- ---------------------------------------------
-- 2. Handle Subsidize_Code_Secondary
-- ---------------------------------------------

	DECLARE @PracticeSchemeSecondary table (
		SP_ID					CHAR(8),
		Practice_Display_Seq	smallint,
		Scheme_Code				CHAR(8),
		Subsidize_Code			CHAR(10),
		Score					int
	)
	
	INSERT INTO @PracticeSchemeSecondary (SP_ID, Practice_Display_Seq, Scheme_Code, Subsidize_Code, Score)
	SELECT
		SP_ID, Practice_Display_Seq, Scheme_Code, Subsidize_Code, 0
	FROM
		#tmpPracticeSchemeInfo

	--
	
	-- MyCursor1: Loop through all SDSubsidizeGroup with Subsidize_Code_Secondary
	
	DECLARE MyCursor1 cursor FOR
		SELECT
			Scheme_Code, Subsidize_Code, Subsidize_Code_Secondary
		FROM
			SDSubsidizeGroup
		WHERE
			Subsidize_Code_Secondary IS NOT NULL

	DECLARE @CC_Scheme_Code					VARCHAR(10)
	DECLARE @CC_Subsidize_Code				VARCHAR(10)
	DECLARE @CC_Subsidize_Code_Secondary	VARCHAR(100)
	DECLARE @CC_Item						VARCHAR(50)
	DECLARE @Power							int
	
	--
	
	OPEN MyCursor1   
	FETCH NEXT FROM MyCursor1 INTO @CC_Scheme_Code, @CC_Subsidize_Code, @CC_Subsidize_Code_Secondary
		
	WHILE @@FETCH_STATUS = 0 BEGIN
		SET @Power = 0
	
		-- MyCursor2: Separate the items in Subsidize_Code_Secondary
		-- If matched: first item = 1 score; second item = 2 score ; third item = 4 score ; fourth item = 8 score etc.
	
		DECLARE MyCursor2 cursor FOR
			SELECT Item FROM dbo.func_split_string(@CC_Subsidize_Code_Secondary, '|||')
			
		OPEN MyCursor2   
		FETCH NEXT FROM MyCursor2 INTO @CC_Item
		
		WHILE @@FETCH_STATUS = 0 BEGIN
			UPDATE
				@PracticeSchemeSecondary
			SET
				Score = Score + POWER(2, @Power)
			FROM
				(SELECT
					PSI.SP_ID,
					PSI.Practice_Display_Seq,
					@CC_Scheme_Code AS [Scheme_Code],
					@CC_Subsidize_Code AS [Subsidize_Code],
					PSI.Provide_Service	
				FROM
					PracticeSchemeInfo PSI
				WHERE
					PSI.Scheme_Code = @CC_Scheme_Code
						AND PSI.Subsidize_Code = @CC_Item
						AND PSI.Record_Status = 'A'	    
						AND PSI.Provide_Service = 'Y'
				) T,
				@PracticeSchemeSecondary P
			WHERE
				P.SP_ID = T.SP_ID
					AND P.Practice_Display_Seq = T.Practice_Display_Seq
					AND P.Scheme_Code = T.Scheme_Code
					AND P.Subsidize_Code = T.Subsidize_Code
				
			--
		
			FETCH NEXT FROM MyCursor2 INTO @CC_Item
			SET @Power = @Power + 1
				
		END
		
		CLOSE MyCursor2  
		DEALLOCATE MyCursor2
	
		FETCH NEXT FROM MyCursor1 INTO @CC_Scheme_Code, @CC_Subsidize_Code, @CC_Subsidize_Code_Secondary
	
	END
		
	CLOSE MyCursor1
	DEALLOCATE MyCursor1

	--

	UPDATE
		#tmpPracticeSchemeInfo
	SET
		Provide_Service = 'Y',
		Service_Fee = T.Symbol
	FROM
		(SELECT 
			SP_ID,
			Practice_Display_Seq,
			Scheme_Code,
			Subsidize_Code,
			S.Symbol
		FROM
			@PracticeSchemeSecondary P
				INNER JOIN @Symbol_ServiceFeeSecondary S
					ON P.Score = S.Score
		) T,
		#tmpPracticeSchemeInfo
	WHERE
		#tmpPracticeSchemeInfo.SP_ID = T.SP_ID
			AND #tmpPracticeSchemeInfo.Practice_Display_Seq = T.Practice_Display_Seq
			AND #tmpPracticeSchemeInfo.Scheme_Code = T.Scheme_Code
			AND #tmpPracticeSchemeInfo.Subsidize_Code = T.Subsidize_Code
			AND #tmpPracticeSchemeInfo.Provide_Service = 'N'


-- ---------------------------------------------
-- 3. Handle Subsidize_Code_Combination: Remove the children (eg. EPV, EQIV) if the parent is not providing service (eg. EPVQIV)
-- ---------------------------------------------

	DECLARE @CC_Subsidize_Code_Combination VARCHAR(100)

	DECLARE @SubsidizeCodeCombination table (
		Subsidize_Code VARCHAR(10)
	)	

	DECLARE CombineCursor cursor FOR  
		SELECT
			Scheme_Code,
			Subsidize_Code,
			Subsidize_Code_Combination
		FROM
			SDSubsidizeGroup
		WHERE
			Record_Status = 'A'
				AND Subsidize_Code_Combination IS NOT NULL

	OPEN CombineCursor   
	FETCH NEXT FROM CombineCursor INTO @CC_Scheme_Code, @CC_Subsidize_Code, @CC_Subsidize_Code_Combination 

	WHILE @@FETCH_STATUS = 0 BEGIN   
		DELETE FROM @SubsidizeCodeCombination

		INSERT INTO @SubsidizeCodeCombination (Subsidize_Code)
		SELECT Item FROM func_split_string(@CC_Subsidize_Code_Combination, '|')

		--
		
		-- Remove child record if the Discount is not yet enrolled (the record does not exist)

		DELETE FROM
			#tmpPracticeSchemeInfo       
		WHERE   
			EXISTS (
				SELECT
					SP_ID
				FROM (
					SELECT
						PSIA.SP_ID,
						PSIA.Practice_Display_Seq,
						PSIA.Scheme_Code,
						SC.Subsidize_Code
					FROM
						#tmpPracticeSchemeInfo PSIA
							CROSS JOIN @SubsidizeCodeCombination SC
					WHERE
						PSIA.Scheme_Code = @CC_Scheme_Code
							AND NOT EXISTS (
								SELECT
									PSIB.SP_ID,
									PSIB.Practice_Display_Seq,
									PSIB.Scheme_Code,
									PSIB.Subsidize_Code
								FROM
									#tmpPracticeSchemeInfo PSIB
								WHERE
									PSIB.Scheme_Code = @CC_Scheme_Code 
										AND PSIB.Subsidize_Code = @CC_Subsidize_Code
										AND PSIA.SP_ID = PSIB.SP_ID
										AND PSIA.Practice_Display_Seq = PSIB.Practice_Display_Seq
							)
					) Child
				WHERE 
					#tmpPracticeSchemeInfo.SP_ID = Child.SP_ID
						AND #tmpPracticeSchemeInfo.Practice_Display_Seq = Child.Practice_Display_Seq
						AND #tmpPracticeSchemeInfo.Scheme_Code = Child.Scheme_Code
						AND #tmpPracticeSchemeInfo.Subsidize_Code = Child.Subsidize_Code
			)  
			

		-- Remove child record if the Discount is not providing service (record exists but Provide_Service = 'N')

		DELETE FROM
			#tmpPracticeSchemeInfo
		WHERE 
			EXISTS (
				SELECT
					1
				FROM (
					SELECT
						PSI.SP_ID,
						PSI.Practice_Display_Seq,
						PSI.Scheme_Code,
						SC.Subsidize_Code
					FROM
						#tmpPracticeSchemeInfo PSI
							CROSS JOIN @SubsidizeCodeCombination SC
					WHERE
						PSI.Scheme_Code = @CC_Scheme_Code
							AND PSI.Subsidize_Code = @CC_Subsidize_Code
							AND PSI.Provide_Service = 'N'
					) Child
				WHERE
					#tmpPracticeSchemeInfo.SP_ID = Child.SP_ID
						AND	#tmpPracticeSchemeInfo.Practice_Display_Seq = Child.Practice_Display_Seq
						AND #tmpPracticeSchemeInfo.Scheme_Code = Child.Scheme_Code
						AND #tmpPracticeSchemeInfo.Subsidize_Code = Child.Subsidize_Code
			)

		--

		FETCH NEXT FROM CombineCursor INTO @CC_Scheme_Code, @CC_Subsidize_Code, @CC_Subsidize_Code_Combination

	END

	CLOSE CombineCursor   
	DEALLOCATE CombineCursor


-- ---------------------------------------------
-- 4. Handle To be Provided
-- ---------------------------------------------

	UPDATE
		#tmpPracticeSchemeInfo
	SET
		Provide_Service = 'Y',
		Service_Fee = @Symbol_ServiceFeeToBeProvided
	FROM
		(SELECT
			T1.SP_ID,
			T1.Practice_Display_Seq,
			T1.Scheme_Code
		FROM
			(SELECT
				PSI1.SP_ID,
				PSI1.Practice_Display_Seq,
				PSI1.Scheme_Code,
				COUNT(1) AS [Subsidize_Count]
			FROM #tmpPractice P1
				INNER JOIN PracticeSchemeInfo PSI1
					ON P1.sp_id = PSI1.SP_ID
						AND P1.practice_display_seq = PSI1.Practice_Display_Seq
			GROUP BY
				PSI1.SP_ID,
				PSI1.Practice_Display_Seq,
				PSI1.Scheme_Code
			) T1 INNER JOIN (
				SELECT
					PSI1.SP_ID,
					PSI1.Practice_Display_Seq,
					PSI1.Scheme_Code,
					COUNT(1) AS [Subsidize_Count]
				FROM #tmpPractice P1
					INNER JOIN PracticeSchemeInfo PSI1
						ON P1.sp_id = PSI1.SP_ID
							AND P1.practice_display_seq = PSI1.Practice_Display_Seq
				WHERE
					PSI1.Provide_Service = 'N'
				GROUP BY
					PSI1.SP_ID,
					PSI1.Practice_Display_Seq,
					PSI1.Scheme_Code
				) T2
				ON T1.SP_ID = T2.SP_ID
					AND T1.Practice_Display_Seq = T2.Practice_Display_Seq 
					AND T1.Scheme_Code = T2.Scheme_Code 
					AND T1.Subsidize_Count = T2.Subsidize_Count 
		) T
	WHERE
		#tmpPracticeSchemeInfo.SP_ID = T.SP_ID
			AND #tmpPracticeSchemeInfo.Practice_Display_Seq = T.Practice_Display_Seq
			AND #tmpPracticeSchemeInfo.Scheme_Code = T.Scheme_Code


-- ---------------------------------------------
-- 5. Remove subsidy which do not providing service
-- ---------------------------------------------

	DELETE FROM
		#tmpPracticeSchemeInfo
	WHERE
		Provide_Service = 'N'

-- ---------------------------------------------
-- 5.1. Prepare Handle N/A service fee temp table
-- ---------------------------------------------
	SELECT 
		PS.SP_ID, PS.Practice_Display_Seq, PS.Scheme_Code, SG.Subsidize_Code 
	INTO #tmp_Subsidies_ServiceFeeNA
	FROM
		(SELECT SP_ID, Practice_Display_Seq, Scheme_Code 
		FROM #tmpPracticeSchemeInfo GROUP BY SP_ID, Practice_Display_Seq, Scheme_Code) PS
		INNER JOIN SDSubsidizeGroup SG
			ON PS.Scheme_Code = SG.Scheme_Code
		-- All practice and scheme with active subsidy
		LEFT JOIN #tmpPracticeSchemeInfo PSI
			ON PS.SP_ID = PSI.SP_ID
				AND PS.Practice_Display_Seq = PSI.Practice_Display_Seq
				AND PS.Scheme_Code = PSI.Scheme_Code
				AND SG.Subsidize_Code = PSI.Subsidize_Code
	WHERE PSI.Subsidize_Code IS NULL

-- ---------------------------------------------
-- 6. Process the subsidize_item_x, subsidize_fee_x (pivot the table manually)
-- ---------------------------------------------

	DECLARE @FC_Scheme_Code VARCHAR(10)
	DECLARE @FC_Subsidize_Code VARCHAR(10)
	DECLARE @FC_Subsidize_Item_Column_Name VARCHAR(20)
	DECLARE @FC_Subsidize_Fee_Column_Name VARCHAR(20)

	DECLARE FeeCursor cursor FOR
		SELECT
			SG.Scheme_Code,
			SG.Subsidize_Code,
			SG.Subsidize_Item_Column_Name,
			SG.Subsidize_Fee_Column_Name
		FROM
			SDScheme S
				INNER JOIN SDSubsidizeGroup SG
					ON S.Scheme_Code = SG.Scheme_Code
		WHERE
			S.Record_Status = 'A'
				AND SG.Record_Status = 'A'
				
	--

	OPEN FeeCursor 
	FETCH NEXT FROM FeeCursor INTO @FC_Scheme_Code, @FC_Subsidize_Code, @FC_Subsidize_Item_Column_Name, @FC_Subsidize_Fee_Column_Name

	WHILE @@FETCH_STATUS = 0 BEGIN
		UPDATE
			#tmpPractice 
		SET
			subsidize_item_01 = 
				CASE @FC_Subsidize_Item_Column_Name
					WHEN 'subsidize_item_01' THEN 'Y'
					ELSE subsidize_item_01
				END,
			subsidize_item_02 = 
				CASE @FC_Subsidize_Item_Column_Name
					WHEN 'subsidize_item_02' THEN 'Y'
					ELSE subsidize_item_02
				END,
			subsidize_item_03 = 
				CASE @FC_Subsidize_Item_Column_Name
					WHEN 'subsidize_item_03' THEN 'Y'
					ELSE subsidize_item_03
				END,
			subsidize_item_04 = 
				CASE @FC_Subsidize_Item_Column_Name
					WHEN 'subsidize_item_04' THEN 'Y'
					ELSE subsidize_item_04
				END,
			subsidize_item_05 = 
				CASE @FC_Subsidize_Item_Column_Name
					WHEN 'subsidize_item_05' THEN 'Y'
					ELSE subsidize_item_05
				END,
			subsidize_item_06 = 
				CASE @FC_Subsidize_Item_Column_Name
					WHEN 'subsidize_item_06' THEN 'Y'
					ELSE subsidize_item_06
				END,
			subsidize_item_07 = 
				CASE @FC_Subsidize_Item_Column_Name
					WHEN 'subsidize_item_07' THEN 'Y'
					ELSE subsidize_item_07
				END,
			subsidize_item_08 = 
				CASE @FC_Subsidize_Item_Column_Name
					WHEN 'subsidize_item_08' THEN 'Y'
					ELSE subsidize_item_08
				END,
			subsidize_item_09 = 
				CASE @FC_Subsidize_Item_Column_Name
					WHEN 'subsidize_item_09' THEN 'Y'
					ELSE subsidize_item_09
				END,
			subsidize_item_10 = 
				CASE @FC_Subsidize_Item_Column_Name
					WHEN 'subsidize_item_10' THEN 'Y'
					ELSE subsidize_item_10
				END,

			subsidize_fee_01 = 
				CASE @FC_Subsidize_Fee_Column_Name
					WHEN 'subsidize_fee_01' THEN PSI.Service_Fee
					ELSE subsidize_fee_01
				END,
			subsidize_fee_02 = 
				CASE @FC_Subsidize_Fee_Column_Name
					WHEN 'subsidize_fee_02' THEN PSI.Service_Fee
					ELSE subsidize_fee_02
				END,
			subsidize_fee_03 = 
				CASE @FC_Subsidize_Fee_Column_Name
					WHEN 'subsidize_fee_03' THEN PSI.Service_Fee
					ELSE subsidize_fee_03
				END,
			subsidize_fee_04 = 
				CASE @FC_Subsidize_Fee_Column_Name
					WHEN 'subsidize_fee_04' THEN PSI.Service_Fee
					ELSE subsidize_fee_04
				END,
			subsidize_fee_05 = 
				CASE @FC_Subsidize_Fee_Column_Name
					WHEN 'subsidize_fee_05' THEN PSI.Service_Fee
					ELSE subsidize_fee_05
				END,
			subsidize_fee_06 = 
				CASE @FC_Subsidize_Fee_Column_Name
					WHEN 'subsidize_fee_06' THEN PSI.Service_Fee
					ELSE subsidize_fee_06
				END,
			subsidize_fee_07 = 
				CASE @FC_Subsidize_Fee_Column_Name
					WHEN 'subsidize_fee_07' THEN PSI.Service_Fee
					ELSE subsidize_fee_07
				END,
			subsidize_fee_08 = 
				CASE @FC_Subsidize_Fee_Column_Name
					WHEN 'subsidize_fee_08' THEN PSI.Service_Fee
					ELSE subsidize_fee_08
				END,
			subsidize_fee_09 = 
				CASE @FC_Subsidize_Fee_Column_Name
					WHEN 'subsidize_fee_09' THEN PSI.Service_Fee
					ELSE subsidize_fee_09
				END,
			subsidize_fee_10 = 
				CASE @FC_Subsidize_Fee_Column_Name
					WHEN 'subsidize_fee_10' THEN PSI.Service_Fee
					ELSE subsidize_fee_10
				END

		FROM
			#tmpPractice P
				INNER JOIN #tmpPracticeSchemeInfo PSI
					ON P.sp_id = PSI.SP_ID
						AND P.practice_display_seq = PSI.Practice_Display_Seq
						AND PSI.Record_Status = 'A'
				INNER JOIN SchemeInformation SI
					ON P.sp_id = SI.SP_ID
						AND PSI.Scheme_Code = SI.Scheme_Code
						AND SI.Record_Status = 'A'
		WHERE
			PSI.Scheme_Code = @FC_Scheme_Code
				AND PSI.Subsidize_Code = @FC_Subsidize_Code
		--

		-- Update ServiceFee = NULL --> N/A (Joined Scheme + SOME subsidies Not provided Service fee) 
		UPDATE
			#tmpPractice 
		SET
			subsidize_fee_01 = CASE @FC_Subsidize_Fee_Column_Name WHEN 'subsidize_fee_01' THEN @Symbol_ServiceFeeNA ELSE subsidize_fee_01 END,
			subsidize_fee_02 = CASE @FC_Subsidize_Fee_Column_Name WHEN 'subsidize_fee_02' THEN @Symbol_ServiceFeeNA ELSE subsidize_fee_02 END,
			subsidize_fee_03 = CASE @FC_Subsidize_Fee_Column_Name WHEN 'subsidize_fee_03' THEN @Symbol_ServiceFeeNA ELSE subsidize_fee_03 END,
			subsidize_fee_04 = CASE @FC_Subsidize_Fee_Column_Name WHEN 'subsidize_fee_04' THEN @Symbol_ServiceFeeNA ELSE subsidize_fee_04 END,
			subsidize_fee_05 = CASE @FC_Subsidize_Fee_Column_Name WHEN 'subsidize_fee_05' THEN @Symbol_ServiceFeeNA ELSE subsidize_fee_05 END,
			subsidize_fee_06 = CASE @FC_Subsidize_Fee_Column_Name WHEN 'subsidize_fee_06' THEN @Symbol_ServiceFeeNA ELSE subsidize_fee_06 END,
			subsidize_fee_07 = CASE @FC_Subsidize_Fee_Column_Name WHEN 'subsidize_fee_07' THEN @Symbol_ServiceFeeNA ELSE subsidize_fee_07 END,
			subsidize_fee_08 = CASE @FC_Subsidize_Fee_Column_Name WHEN 'subsidize_fee_08' THEN @Symbol_ServiceFeeNA ELSE subsidize_fee_08 END,
			subsidize_fee_09 = CASE @FC_Subsidize_Fee_Column_Name WHEN 'subsidize_fee_09' THEN @Symbol_ServiceFeeNA ELSE subsidize_fee_09 END,
			subsidize_fee_10 = CASE @FC_Subsidize_Fee_Column_Name WHEN 'subsidize_fee_10' THEN @Symbol_ServiceFeeNA ELSE subsidize_fee_10 END
		FROM
			#tmpPractice P
				INNER JOIN #tmp_Subsidies_ServiceFeeNA SSNA			
					ON P.sp_id = SSNA.SP_ID
						AND P.practice_display_seq = SSNA.Practice_Display_Seq
		WHERE 
			SSNA.Scheme_Code = @FC_Scheme_Code
				AND SSNA.Subsidize_Code = @FC_Subsidize_Code

		--

		FETCH NEXT FROM FeeCursor INTO @FC_Scheme_Code, @FC_Subsidize_Code, @FC_Subsidize_Item_Column_Name, @FC_Subsidize_Fee_Column_Name

	END

	CLOSE FeeCursor 
	DEALLOCATE FeeCursor


-- ---------------------------------------------
-- 7. Process the joined_scheme
-- ---------------------------------------------

	UPDATE
		#tmpPractice 
	SET
		joined_scheme = C.Joined_Scheme
	FROM (
		SELECT DISTINCT
			SP_ID,
			Practice_Display_Seq,
			(SELECT
				CONVERT(VARCHAR, RTRIM(T.Scheme_Code)) + '|'
			FROM
				(SELECT DISTINCT
					P.SP_ID,
					P.Practice_Display_Seq,
					P.Scheme_Code,
					S.Display_Seq
				FROM
					#tmpPracticeSchemeInfo P
						INNER JOIN SDScheme S
							ON P.Scheme_Code = S.Scheme_Code
				) T
			WHERE
				T.SP_ID = PSI.SP_ID
					AND T.Practice_Display_Seq = PSI.Practice_Display_Seq
			ORDER BY
				T.Display_Seq
			FOR
				XML PATH('')
			) AS Joined_Scheme
		FROM
			#tmpPracticeSchemeInfo PSI
		) C
	WHERE
		#tmpPractice.SP_ID = C.SP_ID
			AND	#tmpPractice.Practice_Display_Seq = C.Practice_Display_Seq





-- =============================================
-- Insert into Service Directory tables
-- =============================================

	DELETE FROM	SDSPPracticeFee

	--

	INSERT INTO SDSPPracticeFee (
		sp_id
		,practice_display_seq
		,sp_name 
		,sp_chi_name
		,practice_name 
		,practice_name_chi 
		,phone_daytime	
		,service_category_code_SD 
		,district_code	
		,district_board_shortname_SD	
		,area_code
		,address_eng 
		,district_name	
		,district_board 
		,area_name
		,address_chi 
		,district_name_chi
		,district_board_chi
		,area_name_chi
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
		,joined_scheme

		,[Mobile_Clinic]
		,[Non_Clinic]
		,[Remarks_Desc]		
		,[Remarks_Desc_Chi]	
	)
	SELECT
		sp_id
		,practice_display_seq
		,sp_name 
		,sp_chi_name
		,practice_name 
		,practice_name_chi 
		,phone_daytime	
		,service_category_code_SD 
		,district_code	
		,district_board_shortname_SD	
		,area_code
		,address_eng 
		,district_name	
		,district_board 
		,area_name
		,address_chi 
		,district_name_chi
		,district_board_chi
		,area_name_chi	

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
		,joined_scheme

		,[Mobile_Clinic]
		,[Non_Clinic]
		,[Remarks_Desc]		
		,[Remarks_Desc_Chi]	
	
	FROM
		#tmpPractice 

-- =============================================
-- Update the LastUpdateDate of Service Directory
-- =============================================

	UPDATE
		DataCutOff_DACO
	SET
		DACO_Cutoff_Dtm = GETDATE(),
		DACO_Update_Dtm = GETDATE()
	WHERE
		DACO_DataType_ID = 'SDIR_LastUpdateDate'


-- =============================================
-- Finalizer
-- =============================================

	DROP TABLE #tmpPractice
	DROP TABLE #tmpPracticeSchemeInfo
	DROP TABLE #tmp_Subsidies_ServiceFeeNA


END
GO

