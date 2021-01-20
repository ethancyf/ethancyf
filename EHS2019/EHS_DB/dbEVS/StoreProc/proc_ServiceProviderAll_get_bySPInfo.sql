IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderAll_get_bySPInfo]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderAll_get_bySPInfo]
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
-- =============================================  
-- Modification History      
-- CR No.:		   CRE12-014  
-- Modified by:    Karl LAM   
-- Modified date:  03 Jan 2013  
-- Description:    Add parameters: @result_limit_1st_enable,@result_limit_override_enable, @override_result_limit  for relax 500 rows limitation  
-- =============================================      
-- =============================================   
-- Author:		Kathy LEE
-- Create date: 27 May 2008
-- Description:	Retrieve Service Provider Information from Table 
--				"ServiceProviderEnrolment", "ServiceProviderStaging" and 
--				"ServiceProvider"
--				Status: U - Unprocessed (Data in table "ServiceProviderEnrolment")
--						P - Processing	(Data in table "ServiceProviderStaging")
--						E - Enrolled	(Data in table "ServiceProvider")
--				Table Location: E - Enrolment	(Data in table "ServiceProviderEnrolment")
--								S - Staging		(Data in table "ServiceProviderStaging")
--								P - Permanent	(Data in table "ServiceProvider")
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Kathy LEE
-- Modified date: 20 May 2009
-- Description:	 1. Retrieve Enrolment Dtm and Data Entry start processing (Create Dtm)
--				 2. Add "scheme_code" in search criteria
--				 3. Retrieve Suffix of Enrolment Reference No 
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	01 Jun 2009
-- Description:		System Parameter Add Scheme Code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	16 Jun 2009
-- Description:		Remove Suffix
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	03 July 2009
-- Description:		Add ERN_Remark from table ernprocessed / ernprocessedstaging
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	12 July 2009
-- Description:		Change the serach logic to handle user input the mearged
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
-- Modified by:		Kathy LEE
-- Modified date:	5 August 2009
-- Description:		1. Retrieve Scheme information by using 'SchemeBackOffice'
-- =============================================

--exec proc_ServiceProviderAll_get_bySPInfo null,null,null,null,null,null,null,null,1,1,0 --20128

CREATE PROCEDURE [dbo].[proc_ServiceProviderAll_get_bySPInfo]
	@enrolment_ref_no char(15), 
	@sp_id char (8), 
	@sp_hkid char(9), 
	@sp_eng_name varchar(40),
	@phone_daytime varchar(20), 
	@service_category_code char(5), 
	@status char(1), 
	@scheme_code char(10),
	@result_limit_1st_enable BIT, 
	@result_limit_override_enable BIT,
	@override_result_limit BIT

AS
BEGIN
	
	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
DECLARE @tmp_sp_enrolment table ( Enrolment_Ref_No char(15),						
						Enrolment_Dtm datetime,
						Processing_Dtm datetime,
						SP_ID char(8),						
						Encrypt_Field1 varbinary(100),
						Encrypt_Field2 varbinary(100),
						Encrypt_Field3 varbinary(100),
						Phone_Daytime varchar(20),						
						Scheme_code varchar(255),
						Table_Location char(1))
DECLARE @tmp_sp_staging table ( Enrolment_Ref_No char(15),						
						Enrolment_Dtm datetime,
						Processing_Dtm datetime,
						SP_ID char(8),						
						Encrypt_Field1 varbinary(100),
						Encrypt_Field2 varbinary(100),
						Encrypt_Field3 varbinary(100),
						Phone_Daytime varchar(20),						
						Scheme_code varchar(255),
						Table_Location char(1))

DECLARE @tmp_sp_permanent table ( Enrolment_Ref_No char(15),						
						Enrolment_Dtm datetime,
						Processing_Dtm datetime,
						SP_ID char(8),
						Encrypt_Field1 varbinary(100),
						Encrypt_Field2 varbinary(100),
						Encrypt_Field3 varbinary(100),
						Phone_Daytime varchar(20),
						Scheme_code varchar(255),
						Table_Location char(1))
						
DECLARE	@tmp_schemebackoffice table (scheme_code char(10),
						display_code varchar(25),
						display_seq smallint)


DECLARE @rowcount int
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
 DECLARE @row_cnt_error varchar(max)

insert into @tmp_schemebackoffice
(
	scheme_code,
	display_code,
	display_seq
)
SELECT Scheme_Code, Display_Code, Display_Seq FROM SchemeBackOffice WHERE GETDATE() BETWEEN Effective_Dtm AND Expiry_Dtm

-- =============================================
-- Return results
-- =============================================
EXEC [proc_SymmetricKey_open]


IF @status = 'U'
BEGIN
	INSERT INTO @tmp_sp_enrolment (Enrolment_Ref_No,						
						Enrolment_Dtm,
						Processing_Dtm,
						SP_ID,						
						Encrypt_Field1,
						Encrypt_Field2,
						Encrypt_Field3,
						Phone_Daytime,	
						Table_Location)
	SELECT	DISTINCT  TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable)) 
			SPE.Enrolment_Ref_No,			
			SPE.Enrolment_Dtm,
			NULL,
			NULL,			
			SPE.Encrypt_Field1,
			SPE.Encrypt_Field2,
			SPE.Encrypt_Field3,
			SPE.Phone_Daytime,			
			'E'
	FROM	ServiceProviderEnrolment SPE
	INNER JOIN SchemeInformationEnrolment SIE
	ON SPE.Enrolment_ref_no = SIE.Enrolment_ref_no	
	LEFT JOIN ProfessionalEnrolment PE
	ON	SPE.Enrolment_Ref_No = PE.Enrolment_Ref_No	
	WHERE	(@enrolment_ref_no is null or @enrolment_ref_no = SPE.Enrolment_Ref_No) and
			(@sp_hkid is null or EncryptByKey(KEY_GUID('sym_Key'), @sp_hkid) = SPE.Encrypt_Field1) and 
			(@sp_eng_name is null or EncryptByKey(KEY_GUID('sym_Key'), @sp_eng_name) = SPE.Encrypt_Field2) and 
			(@phone_daytime is null or @phone_daytime= SPE.Phone_Daytime) and
			(@service_category_code is null or @service_category_code = PE.Service_Category_Code) and
			(@scheme_code is null or @scheme_code = SIE.scheme_code) and
			@sp_id is null

	-- =============================================    
	-- Max Row Checking  
	-- =============================================  
	BEGIN TRY       
		SELECT	@rowcount = count(1) FROM	@tmp_sp_enrolment
		EXEC proc_CheckFeatureResultRowLimit @row_count=@rowcount, @result_limit_1st_enable=@result_limit_1st_enable, @result_limit_override_enable=@result_limit_override_enable,@override_result_limit=@override_result_limit     
	END TRY

	BEGIN CATCH    	    
		SET @row_cnt_error = ERROR_MESSAGE()    

		RAISERROR (@row_cnt_error,16,1)    
		EXEC [proc_SymmetricKey_close] 
		RETURN
	END CATCH  
	
	update @tmp_sp_enrolment
	set scheme_code = Substring(t.scheme_code, 3, LEN(t.scheme_code))
	from (SELECT [Scheme].enrolment_ref_no, 
			(SELECT  ', ' + ltrim(rtrim(m.display_code))
			 FROM SchemeInformationEnrolment sie, @tmp_schemebackoffice m
			 WHERE sie.scheme_code = m.scheme_code
			 AND sie.enrolment_ref_no=[Scheme].enrolment_ref_no 
			 ORDER BY m.display_seq
			 FOR XML PATH('')) 
			 AS scheme_code
	 FROM(SELECT DISTINCT enrolment_ref_no FROM SchemeInformationEnrolment) AS [Scheme]
	)AS t, @tmp_sp_enrolment s
	where t.enrolment_ref_no =  s.enrolment_ref_no collate database_default
	and isnull(s.scheme_code,'') = ''
	
	SELECT	Enrolment_Ref_No, Enrolment_Dtm, Processing_Dtm, isNull(SP_ID, '') as SP_ID,
			convert(varchar, DecryptByKey(Encrypt_Field1)) as SP_HKID,
			convert(varchar(40), DecryptByKey(Encrypt_Field2)) as SP_Eng_Name,
			isNULL(convert(nvarchar, DecryptByKey(Encrypt_Field3)), '') as SP_Chi_Name,
			Phone_Daytime,
			isnull(scheme_code,'') as scheme_code,
			Table_Location
	FROM	@tmp_sp_enrolment
	ORDER By Enrolment_Dtm ASC
END
ELSE IF @status = 'P'
BEGIN
	INSERT INTO @tmp_sp_staging (Enrolment_Ref_No,						
						Enrolment_Dtm,
						Processing_Dtm,
						SP_ID,						
						Encrypt_Field1,
						Encrypt_Field2,
						Encrypt_Field3,
						Phone_Daytime,						
						Table_Location)
	SELECT	DISTINCT  TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable))
			SPS.Enrolment_Ref_No,				
			SPS.Enrolment_Dtm,
			SPS.Create_Dtm,
			SPS.SP_ID,			
			SPS.Encrypt_Field1,
			SPS.Encrypt_Field2,
			SPS.Encrypt_Field3,
			SPS.Phone_Daytime,			
			'S'
	FROM	ServiceProviderStaging SPS
	LEFT JOIN SchemeInformationStaging SIS
	ON SPS.Enrolment_ref_no = SIS.Enrolment_ref_no
	LEFT JOIN ProfessionalStaging PS
	ON		SPS.Enrolment_Ref_No = PS.Enrolment_Ref_No	
	WHERE	(@enrolment_ref_no is null or @enrolment_ref_no = SPS.Enrolment_Ref_No) and
			(@sp_id is null or @sp_id = SPS.SP_ID) and			
			(@sp_hkid is null or EncryptByKey(KEY_GUID('sym_Key'), @sp_hkid) = SPS.Encrypt_Field1) and 
			(@sp_eng_name is null or EncryptByKey(KEY_GUID('sym_Key'), @sp_eng_name) = SPS.Encrypt_Field2) and 
			(@phone_daytime is null or @phone_daytime= SPS.Phone_Daytime) and
			(@service_category_code is null or @service_category_code = PS.Service_Category_Code) and
			(@scheme_code is null or @scheme_code = SIS.scheme_code) and
			SPS.Enrolment_Ref_No not in (SELECT Enrolment_Ref_No FROM SPAccountUpdate)

	-- =============================================    
	-- Max Row Checking  
	-- =============================================  
	BEGIN TRY       
			SELECT	@rowcount = count(1) FROM	@tmp_sp_staging
		EXEC proc_CheckFeatureResultRowLimit @row_count=@rowcount, @result_limit_1st_enable=@result_limit_1st_enable, @result_limit_override_enable=@result_limit_override_enable,@override_result_limit=@override_result_limit     
	END TRY

	BEGIN CATCH    	    
		SET @row_cnt_error = ERROR_MESSAGE()    

		RAISERROR (@row_cnt_error,16,1)    
		EXEC [proc_SymmetricKey_close]
		RETURN
	END CATCH  
	
	update @tmp_sp_staging
	set scheme_code = Substring(t.scheme_code, 3, LEN(t.scheme_code))	
	from (SELECT [Scheme].enrolment_ref_no, 
			(SELECT  ', ' + ltrim(rtrim(m.display_code))
			 FROM SchemeInformationstaging sis, @tmp_schemebackoffice m
			 WHERE sis.scheme_code = m.scheme_code
			 AND sis.enrolment_ref_no=[Scheme].enrolment_ref_no 
			 ORDER BY m.display_seq
			 FOR XML PATH('')) 
			 AS scheme_code
	 FROM(SELECT DISTINCT enrolment_ref_no FROM SchemeInformationStaging) AS [Scheme]
	)AS t, @tmp_sp_staging s
	where t.enrolment_ref_no =  s.enrolment_ref_no collate database_default
	and isnull(s.scheme_code,'') = ''
	
	SELECT	Enrolment_Ref_No, Enrolment_Dtm, Processing_Dtm, isNull(SP_ID,'') as SP_ID,
			convert(varchar, DecryptByKey(Encrypt_Field1)) as SP_HKID,
			convert(varchar(40), DecryptByKey(Encrypt_Field2)) as SP_Eng_Name,
			isNULL(convert(nvarchar, DecryptByKey(Encrypt_Field3)), '') as SP_Chi_Name,
			Phone_Daytime,
			isnull(scheme_code,'') as scheme_code,
			Table_Location
	FROM	@tmp_sp_staging
	ORDER By Enrolment_Dtm ASC
END
ELSE IF @status = 'E'
BEGIN
	INSERT INTO @tmp_sp_permanent (Enrolment_Ref_No,						
						Enrolment_Dtm,
						Processing_Dtm,
						SP_ID,
						Encrypt_Field1,
						Encrypt_Field2,
						Encrypt_Field3,
						Phone_Daytime,
						Table_Location)
	SELECT	DISTINCT  TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable))
			SP.Enrolment_Ref_No,			
			SP.Enrolment_Dtm,
			NULL,
			SP.SP_ID,			
			SP.Encrypt_Field1,
			SP.Encrypt_Field2,
			SP.Encrypt_Field3,
			SP.Phone_Daytime,			
			'P'
	FROM	ServiceProvider SP, Professional P, SchemeInformation SI
	WHERE	SP.SP_ID = P.SP_ID and
			SP.SP_ID = SI.SP_ID and
			(@enrolment_ref_no is null or @enrolment_ref_no = SP.Enrolment_Ref_No) and
			(@sp_id is null or @sp_id = SP.SP_ID) and			
			(@sp_hkid is null or EncryptByKey(KEY_GUID('sym_Key'), @sp_hkid) = SP.Encrypt_Field1) and 
			(@sp_eng_name is null or EncryptByKey(KEY_GUID('sym_Key'), @sp_eng_name) = SP.Encrypt_Field2) and 
			(@phone_daytime is null or @phone_daytime= SP.Phone_Daytime) and
			(@service_category_code is null or @service_category_code = P.Service_Category_Code) and
			(@scheme_code is null or @scheme_code = SI.scheme_code) and
			SP.UnderModification is null and
			SP.Record_Status <> 'D'

	-- =============================================    
	-- Max Row Checking  
	-- =============================================  
	BEGIN TRY       
		SELECT	@rowcount = count(1) FROM	@tmp_sp_permanent
		EXEC proc_CheckFeatureResultRowLimit @row_count=@rowcount, @result_limit_1st_enable=@result_limit_1st_enable, @result_limit_override_enable=@result_limit_override_enable,@override_result_limit=@override_result_limit     
	END TRY

	BEGIN CATCH    	    
		SET @row_cnt_error = ERROR_MESSAGE()    

		RAISERROR (@row_cnt_error,16,1)    
		EXEC [proc_SymmetricKey_close]
		RETURN
	END CATCH  
	
	update @tmp_sp_permanent
	set scheme_code = Substring(t.scheme_code, 3, LEN(t.scheme_code))
	from (SELECT [Scheme].sp_id, 
			(SELECT  ', ' + ltrim(rtrim(m.display_code))
			 FROM SchemeInformation si, @tmp_schemebackoffice m
			 WHERE si.scheme_code = m.scheme_code
			 AND si.sp_id=[Scheme].sp_id 
			 ORDER BY m.display_seq
			 FOR XML PATH('')) 
			 AS scheme_code
	 FROM(SELECT DISTINCT sp_id FROM SchemeInformation) AS [Scheme]
	)AS t, @tmp_sp_permanent s
	where t.sp_id =  s.sp_id collate database_default
	and isnull(s.scheme_code,'') = ''
	
	SELECT	Enrolment_Ref_No, Enrolment_Dtm, Processing_Dtm, isNULL(SP_ID, '') as SP_ID, --SP_HKID, SP_Eng_Name, isNULL(SP_Chi_Name,'') as SP_Chi_Name , 
			convert(varchar, DecryptByKey(Encrypt_Field1)) as SP_HKID,
			convert(varchar(40), DecryptByKey(Encrypt_Field2)) as SP_Eng_Name,
			isNULL(convert(nvarchar, DecryptByKey(Encrypt_Field3)), '') as SP_Chi_Name,
			Phone_Daytime,
			isnull(scheme_code,'') as scheme_code,
			Table_Location
	FROM	@tmp_sp_permanent
	ORDER By Enrolment_Dtm ASC
END
ELSE IF @status is null
BEGIN
	INSERT INTO @tmp_sp_enrolment (Enrolment_Ref_No,
						Enrolment_Dtm,
						Processing_Dtm,
						SP_ID,
						Encrypt_Field1,
						Encrypt_Field2,
						Encrypt_Field3,
						Phone_Daytime,						
						Table_Location)
	SELECT	DISTINCT  TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable))
			SPE.Enrolment_Ref_No,
			SPE.Enrolment_Dtm,
			NULL,
			NULL,			
			SPE.Encrypt_Field1,
			SPE.Encrypt_Field2,
			SPE.Encrypt_Field3,
			SPE.Phone_Daytime,
			'E'
	FROM	ServiceProviderEnrolment SPE
	INNER JOIN SchemeInformationEnrolment SIE
	ON SPE.Enrolment_ref_no = SIE.Enrolment_ref_no	
	LEFT JOIN ProfessionalEnrolment PE
	ON	SPE.Enrolment_Ref_No = PE.Enrolment_Ref_No	
	WHERE	(@enrolment_ref_no is null or @enrolment_ref_no = SPE.Enrolment_Ref_No) and			
			(@sp_hkid is null or EncryptByKey(KEY_GUID('sym_Key'), @sp_hkid) = SPE.Encrypt_Field1) and 
			(@sp_eng_name is null or EncryptByKey(KEY_GUID('sym_Key'), @sp_eng_name) = SPE.Encrypt_Field2) and 
			(@phone_daytime is null or @phone_daytime= SPE.Phone_Daytime) and
			(@service_category_code is null or @service_category_code = PE.Service_Category_Code) and
			(@scheme_code is null or @scheme_code = SIE.scheme_code) and
			@sp_id is null


	INSERT INTO @tmp_sp_staging (Enrolment_Ref_No,
						Enrolment_Dtm,
						Processing_Dtm,
						SP_ID,						
						Encrypt_Field1,
						Encrypt_Field2,
						Encrypt_Field3,
						Phone_Daytime,						
						Table_Location)
	SELECT	DISTINCT  TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable))
			SPS.Enrolment_Ref_No,		
			SPS.Enrolment_Dtm,
			SPS.Create_Dtm,
			SPS.SP_ID,		
			SPS.Encrypt_Field1,
			SPS.Encrypt_Field2,
			SPS.Encrypt_Field3,
			SPS.Phone_Daytime,			
			'S'
	FROM	ServiceProviderStaging SPS
	LEFT JOIN SchemeInformationStaging SIS
	ON SPS.Enrolment_ref_no = SIS.Enrolment_ref_no
	LEFT JOIN ProfessionalStaging PS
	ON		SPS.Enrolment_Ref_No = PS.Enrolment_Ref_No	
	WHERE	(@enrolment_ref_no is null or @enrolment_ref_no = SPS.Enrolment_Ref_No) and
			(@sp_id is null or @sp_id = SPS.SP_ID) and
			(@sp_hkid is null or EncryptByKey(KEY_GUID('sym_Key'), @sp_hkid) = SPS.Encrypt_Field1) and 
			(@sp_eng_name is null or EncryptByKey(KEY_GUID('sym_Key'), @sp_eng_name) = SPS.Encrypt_Field2) and 
			(@phone_daytime is null or @phone_daytime= SPS.Phone_Daytime) and
			(@service_category_code is null or @service_category_code = PS.Service_Category_Code) and
			(@scheme_code is null or @scheme_code = SIS.scheme_code) and
			SPS.Enrolment_Ref_No not in (SELECT Enrolment_Ref_No FROM SPAccountUpdate)

	INSERT INTO @tmp_sp_permanent (Enrolment_Ref_No,						
						Enrolment_Dtm,
						Processing_Dtm,
						SP_ID,
						Encrypt_Field1,
						Encrypt_Field2,
						Encrypt_Field3,
						Phone_Daytime,
						Table_Location)
	SELECT	DISTINCT  TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable))
			SP.Enrolment_Ref_No,			
			SP.Enrolment_Dtm,
			NULL,
			SP.SP_ID,
			SP.Encrypt_Field1,
			SP.Encrypt_Field2,
			SP.Encrypt_Field3,
			SP.Phone_Daytime,
			'P'
	FROM	ServiceProvider SP, Professional P, SchemeInformation SI
	WHERE	SP.SP_ID = P.SP_ID and
			SP.SP_ID = SI.SP_ID and
			(@enrolment_ref_no is null or @enrolment_ref_no = SP.Enrolment_Ref_No) and
			(@sp_id is null or @sp_id = SP.SP_ID) and
			(@sp_hkid is null or EncryptByKey(KEY_GUID('sym_Key'), @sp_hkid) = SP.Encrypt_Field1) and 
			(@sp_eng_name is null or EncryptByKey(KEY_GUID('sym_Key'), @sp_eng_name) = SP.Encrypt_Field2) and 
			(@phone_daytime is null or @phone_daytime= SP.Phone_Daytime) and
			(@service_category_code is null or @service_category_code = P.Service_Category_Code) and
			(@scheme_code is null or @scheme_code = SI.scheme_code) and
			SP.UnderModification is null and
			SP.Record_Status <> 'D'

	-- =============================================    
	-- Max Row Checking  
	-- =============================================  
	BEGIN TRY       
			SELECT	@rowcount = count(1)
			FROM	(SELECT	Enrolment_Ref_No FROM @tmp_sp_enrolment
				UNION ALL	SELECT	Enrolment_Ref_No FROM @tmp_sp_staging
				UNION ALL	SELECT	Enrolment_Ref_No FROM @tmp_sp_permanent) SPPRofile_ERN
		EXEC proc_CheckFeatureResultRowLimit @row_count=@rowcount, @result_limit_1st_enable=@result_limit_1st_enable, @result_limit_override_enable=@result_limit_override_enable,@override_result_limit=@override_result_limit     
	END TRY

	BEGIN CATCH    	    
		SET @row_cnt_error = ERROR_MESSAGE()    

		RAISERROR (@row_cnt_error,16,1)    
		EXEC [proc_SymmetricKey_close]  
		RETURN
	END CATCH  
	
	update @tmp_sp_enrolment
	set scheme_code = Substring(t.scheme_code, 3, LEN(t.scheme_code))
	from (SELECT [Scheme].enrolment_ref_no, 
			(SELECT  ', ' + ltrim(rtrim(m.display_code))
			 FROM SchemeInformationEnrolment sie, @tmp_schemebackoffice m
			 WHERE sie.scheme_code = m.scheme_code
			 AND sie.enrolment_ref_no=[Scheme].enrolment_ref_no
			 ORDER BY m.display_seq
			 FOR XML PATH('')) 
			 AS scheme_code
	 FROM(SELECT DISTINCT enrolment_ref_no FROM SchemeInformationEnrolment) AS [Scheme]
	)AS t, @tmp_sp_enrolment s
	where t.enrolment_ref_no =  s.enrolment_ref_no collate database_default
	and isnull(s.scheme_code,'') = ''
	
	update @tmp_sp_staging
	set scheme_code = Substring(t.scheme_code, 3, LEN(t.scheme_code))
	from (SELECT [Scheme].enrolment_ref_no, 
			(SELECT  ', ' + ltrim(rtrim(m.display_code))
			 FROM SchemeInformationstaging sis, @tmp_schemebackoffice m
			 WHERE sis.scheme_code = m.scheme_code
			 AND sis.enrolment_ref_no=[Scheme].enrolment_ref_no 
			 ORDER BY m.display_seq
			 FOR XML PATH('')) 
			 AS scheme_code
	 FROM(SELECT DISTINCT enrolment_ref_no FROM SchemeInformationStaging) AS [Scheme]
	)AS t, @tmp_sp_staging s
	where t.enrolment_ref_no =  s.enrolment_ref_no collate database_default
	and isnull(s.scheme_code,'') = ''
	
	update @tmp_sp_permanent
	set scheme_code = Substring(t.scheme_code, 3, LEN(t.scheme_code))
	from (SELECT [Scheme].sp_id, 
			(SELECT  ', ' + ltrim(rtrim(m.display_code))
			 FROM SchemeInformation si, @tmp_schemebackoffice m
			 WHERE si.scheme_code = m.scheme_code
			 AND si.sp_id=[Scheme].sp_id 
			 ORDER BY m.display_seq
			 FOR XML PATH('')) 
			 AS scheme_code
	 FROM(SELECT DISTINCT sp_id FROM SchemeInformation) AS [Scheme]
	)AS t, @tmp_sp_permanent s
	where t.sp_id =  s.sp_id collate database_default
	and isnull(s.scheme_code,'') = ''
	
	SELECT	Enrolment_Ref_No, Enrolment_Dtm, Processing_Dtm, isNULL(SP_ID, '') as SP_ID, 
			convert(varchar, DecryptByKey(Encrypt_Field1)) as SP_HKID,
			convert(varchar(40), DecryptByKey(Encrypt_Field2)) as SP_Eng_Name,
			isNULL(convert(nvarchar, DecryptByKey(Encrypt_Field3)), '') as SP_Chi_Name,
			Phone_Daytime,
			isnull(scheme_code,'') as scheme_code,
			Table_Location
	FROM	(SELECT	Enrolment_Ref_No, Enrolment_Dtm, Processing_Dtm, SP_ID, Encrypt_Field1, Encrypt_Field2, Encrypt_Field3, Phone_Daytime,
					scheme_code, Table_Location
			 FROM	@tmp_sp_enrolment 
			 UNION  
			 SELECT	Enrolment_Ref_No, Enrolment_Dtm, Processing_Dtm, SP_ID, Encrypt_Field1, Encrypt_Field2, Encrypt_Field3, Phone_Daytime,
					scheme_code, Table_Location
			 FROM	@tmp_sp_staging 
			 UNION	
			 SELECT	Enrolment_Ref_No, Enrolment_Dtm, Processing_Dtm, SP_ID, Encrypt_Field1, Encrypt_Field2, Encrypt_Field3, Phone_Daytime,
					scheme_code, Table_Location
			 FROM	@tmp_sp_permanent) SPProfile
	ORDER By Enrolment_Dtm ASC 
END

EXEC [proc_SymmetricKey_close]
END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderAll_get_bySPInfo] TO HCVU
GO
