IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderAllEnquiry_get_bySPInfo]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderAllEnquiry_get_bySPInfo]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Marco CHOI
-- Modified date:	01 Feb 2018
-- CR No.:			CRE17-012
-- Description:		Add Chinese name search for SP and EHA
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	31 October 2014
-- CR No.:			INT14-0028
-- Description:		Not joining the [SPAccountMaintenance] table when retrieving permanent records
-- =============================================
-- =============================================  
-- Modification History      
-- CR No.:		   CRE12-014  
-- Modified by:    Karl LAM   
-- Modified date:  03 Jan 2013  
-- Description:    Add parameters: @result_limit_1st_enable,@result_limit_override_enable, @override_result_limit  for relax 500 rows limitation  
-- =============================================      
-- =============================================   
-- Author:		Kathy LEE
-- Create date: 13 July 2008
-- Description:	Retrieve Service Provider Information from Table 
--				"ServiceProviderEnrolment", "ServiceProviderStaging" and 
--				"ServiceProvider" for function "SP-Enquiry"
--				Status: U - Unprocessed (Data in table "ServiceProviderEnrolment")
--						P - Processing	(Data in table "ServiceProviderStaging")
--						E - Enrolled	(Data in table "ServiceProvider")
--				Table Location: E - Enrolment	(Data in table "ServiceProviderEnrolment")
--								S - Staging		(Data in table "ServiceProviderStaging")
--								P - Permanent	(Data in table "ServiceProvider")
-- =============================================

-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	01 Jun 2009
-- Description:		System Parameter Add Scheme Code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	04 Jun 2009
-- Description:		Add Scheme Code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	22 Jun 2009
-- Description:		Handle Scheme Change for VoucherSchemeDetail
--					Scheme_Display_Name -> Scheme_Display_Code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	03 July 2009
-- Description:		1. Add ERN_Remark from table ernprocessed / ernprocessedstaging
--					2. Amended the getting scheme information logic
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	12 July 2009
-- Description:		Change the serach logic to hndle user input the mearged
--					Enrolment_ref_no in Staging or Permanent
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	12 July 2009
-- Description:		Using ExternalCode instead of using MScheme_Code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	18 July 2009
-- Description:		1. Do not retrieve record when using mearged
--					   Enrolment_ref_no in Staging or Permanent as a search key
--					2. Remove ERN_REmark
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark YIP
-- Modified date:   05 Aug 2009
-- Description:	    Join the SchemeBackOffice
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Lawrence TSANG
-- Modified date:   17 August 2009
-- Description:	    Filter the [SchemeBackOffice] records to BETWEEN Effective_Dtm AND Expiry_Dtm
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    
-- Modified date:   
-- Description:	    
-- =============================================

--exec proc_ServiceProviderAllEnquiry_get_bySPInfo null,null,null,null,'23456789',null,null,1,1,0 --20149
				
CREATE PROCEDURE [dbo].[proc_ServiceProviderAllEnquiry_get_bySPInfo]
	@enrolment_ref_no		char(15), 
	@sp_id					char(8),
	@sp_hkid				char(9), 
	@sp_eng_name			varchar(40),
	@sp_chi_name			nvarchar(6),
	@phone_daytime			varchar(20), 
	@service_category_code	char(5),
	@scheme_code			char(10),
	@result_limit_1st_enable BIT, 
	@result_limit_override_enable BIT,
	@override_result_limit BIT
	
AS BEGIN
	
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
	-- 3 identical tables
	DECLARE @TempSP_Enrolment TABLE	(
									Enrolment_Ref_No	char(15),
									Enrolment_Dtm		datetime,
									SP_ID				char(8),
									Encrypt_Field1		varbinary(100),
									Encrypt_Field2		varbinary(100),
									Encrypt_Field3		varbinary(100),
									Phone_Daytime		varchar(20),
									Record_Status		char(2),
									Table_Location		char(1),
									Scheme_Code			varchar(200)
									)
									
	DECLARE @TempSP_Staging TABLE	( 
									Enrolment_Ref_No	char(15),
									Enrolment_Dtm		datetime,
									SP_ID				char(8),
									Encrypt_Field1		varbinary(100),
									Encrypt_Field2		varbinary(100),
									Encrypt_Field3		varbinary(100),
									Phone_Daytime		varchar(20),
									Record_Status		char(2),
									Table_Location		char(1),
									Scheme_Code			varchar(200)
									)

	DECLARE @TempSP_Permanent TABLE ( 
									Enrolment_Ref_No	char(15),
									Enrolment_Dtm		datetime,
									SP_ID				char(8),
									Encrypt_Field1		varbinary(100),
									Encrypt_Field2		varbinary(100),
									Encrypt_Field3		varbinary(100),
									Phone_Daytime		varchar(20),
									Record_Status		char(2),
									Table_Location		char(1),
									Scheme_Code			varchar(200)
									)
									
	DECLARE	@tmp_master_scheme table (MScheme_Code char(10),
									ExternalCode varchar(25),
									Sequence_No smallint)

	DECLARE @rowcount INT
	DECLARE @row_cnt_error varchar(max)
	
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================

	insert into @tmp_master_scheme
	(
		MScheme_Code,
		ExternalCode,
		Sequence_No
	)
	SELECT Scheme_Code, Display_Code, Display_Seq FROM SchemeBackOffice WHERE GETDATE() BETWEEN Effective_Dtm AND Expiry_Dtm
			
-- =============================================
-- Return results
-- =============================================
	OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key

	-- ---------------------------------------------
	-- SP in Enrolment Table
	-- ---------------------------------------------
	INSERT INTO	@TempSP_Enrolment	(
									Enrolment_Ref_No,
									Enrolment_Dtm,
									SP_ID,
									Encrypt_Field1,
									Encrypt_Field2,
									Encrypt_Field3,
									Phone_Daytime,
									Record_Status,
									Table_Location
									)
    SELECT	DISTINCT  TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable)) 
			SPE.Enrolment_Ref_No,
			SPE.Enrolment_Dtm,
			NULL,
			SPE.Encrypt_Field1,
			SPE.Encrypt_Field2,
			SPE.Encrypt_Field3,
			SPE.Phone_Daytime,
			'U',	-- 'U' for Unprocessed
			'E'	-- 'E' for Enrolment table
					
	FROM	ServiceProviderEnrolment SPE
				LEFT JOIN ProfessionalEnrolment PE
					ON SPE.Enrolment_Ref_No = PE.Enrolment_Ref_No
				INNER JOIN SchemeInformationEnrolment SIE
					ON SPE.Enrolment_Ref_No = SIE.Enrolment_Ref_No
					
	WHERE	(@enrolment_ref_no IS NULL OR @enrolment_ref_no = SPE.Enrolment_Ref_No) 
				AND (@sp_hkid IS NULL OR EncryptByKey(KEY_GUID('sym_Key'), @sp_hkid) = SPE.Encrypt_Field1) 
				AND (@sp_eng_name IS NULL OR EncryptByKey(KEY_GUID('sym_Key'), @sp_eng_name) = SPE.Encrypt_Field2)
				AND (@sp_chi_name IS NULL OR EncryptByKey(KEY_GUID('sym_Key'), @sp_chi_name) = SPE.Encrypt_Field3)
				AND (@phone_daytime IS NULL OR @phone_daytime= SPE.Phone_Daytime) 
				AND (@service_category_code IS NULL OR @service_category_code = PE.Service_Category_Code) 
				AND (@scheme_code IS NULL OR @scheme_code = SIE.Scheme_Code)
				AND @sp_id IS NULL

	-- ---------------------------------------------
	-- SP in Staging Table
	-- ---------------------------------------------
	INSERT INTO @TempSP_Staging		(
									Enrolment_Ref_No,
									Enrolment_Dtm,
									SP_ID,
									Encrypt_Field1,
									Encrypt_Field2,
									Encrypt_Field3,
									Phone_Daytime,
									Record_Status,
									Table_Location									
									)
	SELECT	DISTINCT  TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable)) 
			SPS.Enrolment_Ref_No,
			SPS.Enrolment_Dtm,
			SPS.SP_ID,
			SPS.Encrypt_Field1,
			SPS.Encrypt_Field2,
			SPS.Encrypt_Field3,
			SPS.Phone_Daytime,
			CASE ISNULL(SPAU.Progress_Status, '')
				WHEN '' THEN 'E'
				WHEN 'V' THEN 'N'				
				ELSE ISNULL(SPAU.Progress_Status,'')
			END,
			'S'		-- 'S' for Staging table
			
	FROM	ServiceProviderStaging SPS
				LEFT JOIN ProfessionalStaging PS
					ON SPS.Enrolment_Ref_No = PS.Enrolment_Ref_No
				LEFT JOIN SPAccountUpdate SPAU
					ON SPS.Enrolment_Ref_No = SPAU.Enrolment_Ref_no
				LEFT JOIN SchemeInformationStaging SIS
					ON SPS.Enrolment_Ref_No = SIS.Enrolment_Ref_No
					
	WHERE	(@enrolment_ref_no IS NULL OR @enrolment_ref_no = SPS.Enrolment_Ref_No) 
				AND (@sp_id IS NULL OR @sp_id = SPS.SP_ID) 
				AND (@sp_hkid IS NULL OR EncryptByKey(KEY_GUID('sym_Key'), @sp_hkid) = SPS.Encrypt_Field1) 
				AND (@sp_eng_name IS NULL OR EncryptByKey(KEY_GUID('sym_Key'), @sp_eng_name) = SPS.Encrypt_Field2)
				AND (@sp_chi_name IS NULL OR EncryptByKey(KEY_GUID('sym_Key'), @sp_chi_name) = SPS.Encrypt_Field3)
				AND (@phone_daytime IS NULL OR @phone_daytime= SPS.Phone_Daytime) 
				AND (@service_category_code IS NULL OR @service_category_code = PS.Service_Category_Code) 
				AND (@scheme_code IS NULL OR @scheme_code = SIS.Scheme_Code)
				AND SPS.SP_ID IS NULL
				
	-- ---------------------------------------------
	-- SP in Permanent Table
	-- ---------------------------------------------
	INSERT INTO @TempSP_Permanent	(
									Enrolment_Ref_No,
									Enrolment_Dtm,
									SP_ID,
									Encrypt_Field1,
									Encrypt_Field2,
									Encrypt_Field3,
									Phone_Daytime,
									Record_Status,
									Table_Location
									)
	SELECT	DISTINCT  TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable)) 
			SP.Enrolment_Ref_No,
			Enrolment_Dtm,
			SP.SP_ID,
			SP.Encrypt_Field1,
			SP.Encrypt_Field2,
			SP.Encrypt_Field3,
			SP.Phone_Daytime,
			CASE ISNULL(SP.Record_Status, '')  -- If 'A' (Active), check whether the account is locked, if so output 'LA' (Active (Account Locked))
				WHEN 'A' THEN CASE HCSPAC.Record_Status 
								WHEN 'S' THEN 'LA'
								ELSE SP.Record_Status
							  END 
				ELSE SP.Record_Status
			END,
			'P'		-- 'P' for Permanent table
						
	FROM	ServiceProvider SP
				INNER JOIN Professional P
					ON SP.SP_ID = P.SP_ID
				INNER JOIN HCSPUserAC HCSPAC
					ON HCSPAC.SP_ID = SP.SP_ID
				INNER JOIN SchemeInformation I
					ON SP.SP_ID = I.SP_ID
						
	WHERE	(@enrolment_ref_no IS NULL OR @enrolment_ref_no = SP.Enrolment_Ref_No) 
				AND (@sp_id IS NULL OR @sp_id = SP.SP_ID) 
				AND (@sp_hkid IS NULL OR EncryptByKey(KEY_GUID('sym_Key'), @sp_hkid) = SP.Encrypt_Field1) 
				AND (@sp_eng_name IS NULL OR EncryptByKey(KEY_GUID('sym_Key'), @sp_eng_name) = SP.Encrypt_Field2)
				AND (@sp_chi_name IS NULL OR EncryptByKey(KEY_GUID('sym_Key'), @sp_chi_name) = SP.Encrypt_Field3)
				AND (@phone_daytime IS NULL OR @phone_daytime= SP.Phone_Daytime) 
				AND (@service_category_code IS NULL OR @service_category_code = P.Service_Category_Code)
				AND (@scheme_code IS NULL OR @scheme_code = I.Scheme_Code)

	-- =============================================    
	-- Max Row Checking  
	-- =============================================  
	BEGIN TRY       
			SELECT	@rowcount = COUNT(1)
			FROM	(
			SELECT Enrolment_Ref_No FROM @TempSP_Enrolment
			UNION ALL
			SELECT Enrolment_Ref_No FROM @TempSP_Staging
			UNION ALL
			SELECT Enrolment_Ref_No FROM @TempSP_Permanent
			) SPPRofile_ERN

		EXEC proc_CheckFeatureResultRowLimit @row_count=@rowcount, @result_limit_1st_enable=@result_limit_1st_enable, @result_limit_override_enable=@result_limit_override_enable,@override_result_limit=@override_result_limit     
	END TRY

	BEGIN CATCH    	    
		SET @row_cnt_error = ERROR_MESSAGE()    

		RAISERROR (@row_cnt_error,16,1)    
		CLOSE SYMMETRIC KEY sym_Key  
		RETURN
	END CATCH 
	
	update @TempSP_Enrolment
	set scheme_code = Substring(t.scheme_code, 3, LEN(t.scheme_code))
	from (SELECT [Scheme].enrolment_ref_no, 
			(SELECT  ', ' + ltrim(rtrim(m.externalcode))
			 FROM SchemeInformationEnrolment sie, @tmp_master_scheme m
			 WHERE sie.scheme_code = m.mscheme_code
			 AND sie.enrolment_ref_no=[Scheme].enrolment_ref_no 
			 ORDER BY m.sequence_no
			 FOR XML PATH('')) 
			 AS scheme_code
	 FROM(SELECT DISTINCT enrolment_ref_no FROM SchemeInformationEnrolment) AS [Scheme]
	)AS t, @TempSP_Enrolment s
	where t.enrolment_ref_no =  s.enrolment_ref_no collate database_default
	and isnull(s.scheme_code,'') = ''
	
	update @TempSP_Staging
	set scheme_code = Substring(t.scheme_code, 3, LEN(t.scheme_code))	
	from (SELECT [Scheme].enrolment_ref_no, 
			(SELECT  ', ' + ltrim(rtrim(m.externalcode))
			 FROM SchemeInformationstaging sis, @tmp_master_scheme m
			 WHERE sis.scheme_code = m.mscheme_code
			 AND sis.enrolment_ref_no=[Scheme].enrolment_ref_no 
			 ORDER BY m.sequence_no
			 FOR XML PATH('')) 
			 AS scheme_code
	 FROM(SELECT DISTINCT enrolment_ref_no FROM SchemeInformationStaging) AS [Scheme]
	)AS t, @TempSP_Staging s
	where t.enrolment_ref_no =  s.enrolment_ref_no collate database_default
	and isnull(s.scheme_code,'') = ''
	
	update @TempSP_Permanent
	set scheme_code = Substring(t.scheme_code, 3, LEN(t.scheme_code))
	from (SELECT [Scheme].sp_id, 
			(SELECT  ', ' + ltrim(rtrim(m.externalcode))
			 FROM SchemeInformation si, @tmp_master_scheme m
			 WHERE si.scheme_code = m.mscheme_code
			 AND si.sp_id=[Scheme].sp_id 
			 ORDER BY m.sequence_no
			 FOR XML PATH('')) 
			 AS scheme_code
	 FROM(SELECT DISTINCT sp_id FROM SchemeInformation) AS [Scheme]
	)AS t, @TempSP_Permanent s
	where t.sp_id =  s.sp_id collate database_default
	and isnull(s.scheme_code,'') = ''
	
	SELECT		Enrolment_Ref_No, 
				Enrolment_Dtm, 
				ISNULL(SP_ID, '') AS SP_ID, 
				CONVERT(varchar, DecryptByKey(Encrypt_Field1)) AS SP_HKID,
				CONVERT(varchar(40), DecryptByKey(Encrypt_Field2)) AS SP_Eng_Name,
				ISNULL(CONVERT(nvarchar, DecryptByKey(Encrypt_Field3)), '') AS SP_Chi_Name,
				Phone_Daytime,
				Record_Status, 
				Table_Location, 
				Scheme_Code
				
	FROM		(
				SELECT	Enrolment_Ref_No, Enrolment_Dtm, SP_ID, Encrypt_Field1, Encrypt_Field2, Encrypt_Field3, Phone_Daytime,
						Record_Status, Table_Location, Scheme_Code
				FROM	@TempSP_Enrolment 
				UNION  
				SELECT	Enrolment_Ref_No, Enrolment_Dtm, SP_ID, Encrypt_Field1, Encrypt_Field2, Encrypt_Field3, Phone_Daytime,
						Record_Status, Table_Location, Scheme_Code
				FROM	@TempSP_Staging 
				UNION	
				SELECT	Enrolment_Ref_No, Enrolment_Dtm, SP_ID, Encrypt_Field1, Encrypt_Field2, Encrypt_Field3, Phone_Daytime,
						Record_Status, Table_Location, Scheme_Code
				FROM	@TempSP_Permanent
				) SPProfile
				
	ORDER By	Enrolment_Ref_No ASC 

CLOSE SYMMETRIC KEY sym_Key

END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderAllEnquiry_get_bySPInfo] TO HCVU
GO
