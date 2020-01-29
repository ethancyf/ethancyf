IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderTokenAll_get_bySPInfo]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderTokenAll_get_bySPInfo]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	12 Feb 2016
-- CR No.			CRE16-019 (To implement token sharing between eHS(S) and eHRSS)
-- Description:		Rename PPI-ePR to eHRSS
-- =============================================
-- =============================================  
-- Modification History      
-- CR No.:		   CRE16-002  
-- Modified by:    Chris YIM 
-- Modified date:  29 Aug 2016  
-- Description:    Use "Inner Join" in WHERE CLAUSE instead of "Like" to retrieve result set
-- =============================================      
-- =============================================
-- Modification History
-- CR No:			CRE14-002 - PPI-ePR Migration
-- Modified by:		Tommy LAM
-- Modified date:	20 Mar 2014
-- Description:		Add Column -	[Token].[Project_Replacement]
--									[Token].[Is_Share_Token_Replacement]
--									[Token].[Is_Share_Token]
-- =============================================
-- =============================================  
-- Modification History      
-- CR No.:		   CRE13-003  
-- Modified by:    Karl LAM   
-- Modified date:  20 May 2013  
-- Description:    Allow Token.Token_Serial_No_Replacement can be used to search out records
--					Add  Token.Last_Replacement_Reason to result set
-- =============================================      
-- =============================================  
-- Modification History      
-- CR No.:		   CRE12-014  
-- Modified by:    Karl LAM   
-- Modified date:  03 Jan 2013  
-- Description:    Add parameters: @result_limit_1st_enable,@result_limit_override_enable, @override_result_limit  for relax 500 rows limitation  
-- =============================================      
-- =============================================   
-- Modification History
-- CR No.:			INT12-0015
-- Modified by:		Timothy LEUNG
-- Modified date:	28 Dec 2012
-- Description:		Remove cursor for performance tuning
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Clark YIP
-- Modified date:	17 August 2009
-- Description:		Allow Active and Scheme Enrolment display together in Any status search
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Clark YIP
-- Modified date:	6 August 2009
-- Description:		1. 'SchemeBackOffice' filter by effective_dtm and expiry_dtm
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Clark YIP
-- Modified date:	5 August 2009
-- Description:		Use SchemeBackOffice and don't use MasterScheme
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Clark YIP
-- Modified date:	21 July 2009
-- Description:		Handle scheme enrolment with token deactivated 
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Clark YIP
-- Modified date:	20 July 2009
-- Description:		Fix the typo about for getting the enrolled sp scheme code from staging
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Clark YIP
-- Modified date:	19 July 2009
-- Description:		Fix the problem about project code is empty for enrolled sp when search any case
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
-- Modified by:		Clark YIP
-- Modified date:	18 July 2009
-- Description:		Fix the problem about getting the scheme code for enrolled sp
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Clark YIP
-- Modified date:	16 July 2009
-- Description:		Retrieve the Project for Scheme Enrolment case
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Clark YIP
-- Modified date:	13 July 2009
-- Description:		Using ExternalCode instead of using MScheme_Code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	13 July 2009
-- Description:		Extend the size of the variable Scheme_Code from char(20) to char(200)
--					in the 4 temp tables:
--						@tmp_sp_processing
--						@tmp_sp_scheme_enrolment
--						@tmp_sp_enrolled_token
--						@tmp_sp_enrolled_detoken
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
-- Modified date:	03 July 2009
-- Description:		Add ERN_Remark from table ernprocessed / ernprocessedstaging
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	01 Jun 2009
-- Description:		System Parameter Add Scheme Code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	 Clark YIP
-- Modified date: 14 Apr 2009
-- Description:	  Add 1 more @Status / Record Status
--						E - Scheme Enrolment
--				  Add the Scheme_Code criteria
-- =============================================
-- =============================================
-- Author:		Tommy Cheung
-- Create date: 18 June 2008
-- Description:	Retrieve Service Provider Information from Table 
--				"SPAccountUpdate", "Token", "ServiceProvider", "ServiceProviderStaging"
--				Token Status: N - NoToken
--							  A - Active
--							  D - Deactivated
--							  S - Suspended
--				@Status / Record Status:
--								N - New Enrolment
--								S - Suspended (SP)
--								D - Delisted (SP)
--								A - Active (SP)
-- =============================================
--exec proc_ServiceProviderTokenAll_get_bySPInfo null,null,null,null,null,null,null,null,null,0,1,1 --20074


CREATE PROCEDURE [dbo].[proc_ServiceProviderTokenAll_get_bySPInfo]
	@enrolment_ref_no char(15), 
	@sp_id char (8), 
	@sp_hkid char(9), 
	@sp_eng_name varchar(40),
	@phone_daytime varchar(20), 
	@service_category_code char(5), 
	@status char(1), 
	@token_serial_no varchar(20), 
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
OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key

DECLARE @schemeList VARCHAR(100)
DECLARE @row_cnt_error varchar(max)	

DECLARE @tmp_sp_processing table ( 
	Enrolment_Ref_No char(15),
	SP_ID char(8),
	SP_HKID char(9),
	SP_Eng_Name varchar(40),
	SP_Chi_Name nvarchar(6),
	Phone_Daytime varchar(20),
	--Service_category_code char(5),
	Already_Joined_EHR char(1),
	Record_Status char(1),
	Token_Serial_No varchar(20),
	Token_Serial_No_Replacement varchar(20),
	Project_Replacement char(10),
	Is_Share_Token_Replacement char(1),
	Last_Replacement_Reason varchar(10),
	Project char(10),
	Is_Share_Token char(1),
	Token_Status char(1),
	Scheme_Code char(200))
						
DECLARE @tmp_sp_scheme_enrolment table ( 
	Enrolment_Ref_No char(15),
	SP_ID char(8),
	SP_HKID char(9),
	SP_Eng_Name varchar(40),
	SP_Chi_Name nvarchar(6),
	Phone_Daytime varchar(20),
	--Service_category_code char(5),
	Already_Joined_EHR char(1),
	Record_Status char(1),
	Token_Serial_No varchar(20),
	Token_Serial_No_Replacement varchar(20),
	Project_Replacement char(10),
	Is_Share_Token_Replacement char(1),
	Last_Replacement_Reason varchar(10),
	Project char(10),
	Is_Share_Token char(1),
	Token_Status char(1),
	Scheme_Code char(200))
						
DECLARE @tmp_sp_enrolled_token table ( 
	Enrolment_Ref_No char(15),
	SP_ID char(8),
	SP_HKID char(9),
	SP_Eng_Name varchar(40),
	SP_Chi_Name nvarchar(6),
	Phone_Daytime varchar(20),
	--Service_category_code char(5),
	Already_Joined_EHR char(1),
	Record_Status char(1),
	Token_Serial_No varchar(20),
	Token_Serial_No_Replacement varchar(20),
	Project_Replacement char(10),
	Is_Share_Token_Replacement char(1),
	Last_Replacement_Reason varchar(10),
	Project char(10),
	Is_Share_Token char(1),
	Token_Status char(1),
	Scheme_Code char(200))

DECLARE @tmp_sp_enrolled_detoken table ( 
	Enrolment_Ref_No char(15),
	SP_ID char(8),
	SP_HKID char(9),
	SP_Eng_Name varchar(40),
	SP_Chi_Name nvarchar(6),
	Phone_Daytime varchar(20),
	--Service_category_code char(5),
	Already_Joined_EHR char(1),
	Record_Status char(1),
	Token_Serial_No varchar(20),
	Token_Serial_No_Replacement varchar(20),
	Project_Replacement char(10),
	Is_Share_Token_Replacement char(1),
	Last_Replacement_Reason varchar(10),
	Project char(10),
	Is_Share_Token char(1),
	Token_Status char(1),
	Scheme_Code char(200))

declare @schemecount int
declare @single_scheme char(10)
declare @spid char(8)
declare @ern char(15)


DECLARE @rowcount int
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================

--select @scheme_code=(select Display_Code from SchemeBackOffice where Scheme_Code=@scheme_code and Effective_Dtm <= getdate() and Expiry_Dtm >= getdate())
-- =============================================
-- Return results
-- =============================================

-- -----------------------------------------------
-- Service Provider Status: New Enrolment
-- -----------------------------------------------
IF @status = 'N'
BEGIN
	INSERT INTO @tmp_sp_processing (
		Enrolment_Ref_No,
		SP_ID,
		SP_HKID,
		SP_Eng_Name,
		SP_Chi_Name,
		Phone_Daytime,
		--Service_category_code,
		Already_Joined_EHR,
		Record_Status,
		Token_Serial_No,
		Token_Serial_No_Replacement,
		Project_Replacement,
		Is_Share_Token_Replacement,
		Last_Replacement_Reason,
		Project,
		Is_Share_Token,
		Token_Status,
		Scheme_Code)
	SELECT	DISTINCT TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable)) 
		SPS.Enrolment_Ref_No,
		SPS.SP_ID,
		convert(varchar, DecryptByKey(SPS.Encrypt_Field1)),
		convert(varchar(40), DecryptByKey(SPS.Encrypt_Field2)),
		convert(nvarchar, DecryptByKey(SPS.Encrypt_Field3)),
		SPS.Phone_Daytime,
		--PS.Service_category_code,
		SPS.Already_Joined_EHR,
		'N',
		'',
		'',
		'',
		'',
		'',
		'',
		'',
		'N',
		dbo.func_formatSchemeList(SPS.Enrolment_Ref_No,NULL,NULL,'STAGING')--''
	FROM	ServiceProviderStaging SPS
	LEFT JOIN ProfessionalStaging PS
	ON	SPS.Enrolment_Ref_No = PS.Enrolment_Ref_No
	WHERE	(@enrolment_ref_no is null or @enrolment_ref_no = SPS.Enrolment_Ref_No) and
			(@sp_hkid is null or SPS.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @sp_hkid) ) and
			(@sp_eng_name is null or SPS.Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @sp_eng_name)) and
			(@phone_daytime is null or @phone_daytime= SPS.Phone_Daytime) and
			(@service_category_code is null or @service_category_code = PS.Service_Category_Code) and
			(@sp_id is null or @sp_id = SPS.SP_ID) and
			(@token_serial_no is null) and 
			SPS.Enrolment_Ref_No in (select AU.Enrolment_Ref_No from SPAccountUpdate AU 
										where AU.Issue_Token = 'Y' and AU.Progress_Status='T')

	-- =============================================    
	-- Max Row Checking  
	-- =============================================  
	IF ISNULL(@scheme_code, '') = ''
	BEGIN
		BEGIN TRY       
			SELECT	
				@rowcount = count(1)
			FROM	
				@tmp_sp_processing t_sp

			EXEC proc_CheckFeatureResultRowLimit @row_count=@rowcount, @result_limit_1st_enable=@result_limit_1st_enable, @result_limit_override_enable=@result_limit_override_enable,@override_result_limit=@override_result_limit     
		END TRY

		BEGIN CATCH    	    
			SET @row_cnt_error = ERROR_MESSAGE()    

			RAISERROR (@row_cnt_error,16,1)    
			CLOSE SYMMETRIC KEY sym_Key  
			RETURN
		END CATCH  

		SELECT	
			t_sp.Enrolment_Ref_No, 
			isNull(t_sp.SP_ID, '') as SP_ID, 
			SP_HKID, SP_Eng_Name, 
			isNull(SP_Chi_Name, '') as SP_Chi_Name, 
			Phone_Daytime,
			Already_Joined_EHR, 
			t_sp.Record_Status, 
			Token_Serial_No, 
			isNull(Token_Serial_No_Replacement,'') as Token_Serial_No_Replacement,
			isnull(Project_Replacement, '') AS Project_Replacement, 
			isnull(Is_Share_Token_Replacement, '') AS Is_Share_Token_Replacement,
			isnull(Last_Replacement_Reason,'') as Last_Replacement_Reason,
			Project, 
			Is_Share_Token, 
			Token_Status, 
			t_sp.Scheme_Code
		FROM	
			@tmp_sp_processing t_sp
		ORDER By Enrolment_Ref_No ASC
	END
	ELSE
	BEGIN
		BEGIN TRY       
			SELECT	
				@rowcount = count(1)
			FROM	
				@tmp_sp_processing t_sp
				INNER JOIN SchemeInformationStaging s
				ON t_sp.Enrolment_Ref_No = s.Enrolment_Ref_No 
				AND ((@scheme_code is not null and s.scheme_code = @scheme_code) or @scheme_code is null)		
				INNER JOIN SchemeBackOffice m 
				ON s.Scheme_Code = m.Scheme_Code
				and m.Effective_Dtm <= getdate() and m.Expiry_Dtm >= getdate()

			EXEC proc_CheckFeatureResultRowLimit @row_count=@rowcount, @result_limit_1st_enable=@result_limit_1st_enable, @result_limit_override_enable=@result_limit_override_enable,@override_result_limit=@override_result_limit     
		END TRY

		BEGIN CATCH    	    
			SET @row_cnt_error = ERROR_MESSAGE()    

			RAISERROR (@row_cnt_error,16,1)    
			CLOSE SYMMETRIC KEY sym_Key  
			RETURN
		END CATCH  

		SELECT	
			t_new.Enrolment_Ref_No, 
			isNull(t_new.SP_ID, '') as SP_ID, 
			SP_HKID, SP_Eng_Name, 
			isNull(SP_Chi_Name, '') as SP_Chi_Name, 
			Phone_Daytime,
			Already_Joined_EHR, 
			t_new.Record_Status, 
			Token_Serial_No, 
			isNull(Token_Serial_No_Replacement,'') as Token_Serial_No_Replacement,
			isnull(Project_Replacement, '') AS Project_Replacement, 
			isnull(Is_Share_Token_Replacement, '') AS Is_Share_Token_Replacement,
			isnull(Last_Replacement_Reason,'') as Last_Replacement_Reason,
			Project, 
			Is_Share_Token, 
			Token_Status, 
			t_new.Scheme_Code
		FROM	
			@tmp_sp_processing t_new
			INNER JOIN SchemeInformationStaging s
			ON t_new.Enrolment_Ref_No = s.Enrolment_Ref_No 
			AND ((@scheme_code is not null and s.scheme_code = @scheme_code) or @scheme_code is null)	
			INNER JOIN SchemeBackOffice m 
			ON ltrim(rtrim(s.Scheme_Code)) = ltrim(rtrim(m.Scheme_Code))
			and m.Effective_Dtm <= getdate() and m.Expiry_Dtm >= getdate()
		ORDER By t_new.Enrolment_Ref_No ASC

	END
END

-- -----------------------------------------------
-- Service Provider Status: Scheme Enrolment
-- -----------------------------------------------
ELSE IF @status = 'E'
BEGIN

	INSERT INTO @tmp_sp_scheme_enrolment (
		Enrolment_Ref_No,
		SP_ID,
		SP_HKID,
		SP_Eng_Name,
		SP_Chi_Name,
		Phone_Daytime,
		--Service_category_code,
		Already_Joined_EHR,
		Record_Status,
		Token_Serial_No,
		Token_Serial_No_Replacement,
		Project_Replacement,
		Is_Share_Token_Replacement,
		Last_Replacement_Reason,
		Project,
		Is_Share_Token,
		Token_Status,
		Scheme_Code)
	SELECT	DISTINCT TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable)) 
		SPS.Enrolment_Ref_No,
		SPS.SP_ID,
		convert(varchar, DecryptByKey(SPS.Encrypt_Field1)),
		convert(varchar(40), DecryptByKey(SPS.Encrypt_Field2)),
		convert(nvarchar, DecryptByKey(SPS.Encrypt_Field3)),
		SPS.Phone_Daytime,
		--PS.Service_category_code,
		SPS.Already_Joined_EHR,
		'E',
		isnull(T.Token_Serial_No,'') as Token_Serial_No,
		'',
		'',
		'',
		'',
		isnull(T.Project,'') as Project,
		isnull(T.Is_Share_Token,'') as Is_Share_Token,
		'N',
		dbo.func_formatSchemeList(SPS.Enrolment_Ref_No,'A',NULL,'STAGING')--''
	FROM	ServiceProviderStaging SPS
	Left Join Token T
	ON T.User_ID = SPS.SP_ID
	LEFT JOIN ProfessionalStaging PS
	ON	SPS.Enrolment_Ref_No = PS.Enrolment_Ref_No
	WHERE	
			(@enrolment_ref_no is null or @enrolment_ref_no = SPS.Enrolment_Ref_No) and
			(@sp_hkid is null or SPS.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @sp_hkid) ) and
			(@sp_eng_name is null or SPS.Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @sp_eng_name)) and
			(@phone_daytime is null or @phone_daytime= SPS.Phone_Daytime) and
			(@service_category_code is null or @service_category_code = PS.Service_Category_Code) and
			(@sp_id is null or @sp_id = SPS.SP_ID) and
			(@token_serial_no is null) and 
			SPS.Enrolment_Ref_No in (select AU.Enrolment_Ref_No from SPAccountUpdate AU 
										where AU.Scheme_Confirm = 'Y' and AU.Progress_Status='S')
				
	-- =============================================    
	-- Max Row Checking  
	-- =============================================  
	IF ISNULL(@scheme_code, '') = ''
	BEGIN
		BEGIN TRY       
			SELECT	
				@rowcount = count(1)
			FROM	
				@tmp_sp_scheme_enrolment
			
			EXEC proc_CheckFeatureResultRowLimit @row_count=@rowcount, @result_limit_1st_enable=@result_limit_1st_enable, @result_limit_override_enable=@result_limit_override_enable,@override_result_limit=@override_result_limit     
		END TRY

		BEGIN CATCH    	    
			SET @row_cnt_error = ERROR_MESSAGE()    

			RAISERROR (@row_cnt_error,16,1)    
			CLOSE SYMMETRIC KEY sym_Key  
			RETURN
		END CATCH  

		SELECT	
			Enrolment_Ref_No, 
			isNull(SP_ID, '') as SP_ID, 
			SP_HKID, SP_Eng_Name, 
			isNull(SP_Chi_Name, '') as SP_Chi_Name, 
			Phone_Daytime,
			Already_Joined_EHR, 
			Record_Status, 
			Token_Serial_No, 
			isNull(Token_Serial_No_Replacement,'') as Token_Serial_No_Replacement,
			isnull(Project_Replacement, '') AS Project_Replacement, 
			isnull(Is_Share_Token_Replacement, '') AS Is_Share_Token_Replacement,
			isnull(Last_Replacement_Reason,'') as Last_Replacement_Reason,
			Project, 
			Is_Share_Token, 
			Token_Status, 
			Scheme_Code
		FROM	
			@tmp_sp_scheme_enrolment
		ORDER By Enrolment_Ref_No ASC
	END
	ELSE
	BEGIN
		BEGIN TRY       
			SELECT	
				@rowcount = count(1)
			FROM	
				@tmp_sp_processing t_sp
				INNER JOIN SchemeInformationStaging s
				ON t_sp.Enrolment_Ref_No = s.Enrolment_Ref_No 
				AND s.Record_Status = 'A'
				AND ((@scheme_code is not null and s.scheme_code = @scheme_code) or @scheme_code is null)		
				INNER JOIN SchemeBackOffice m 
				ON s.Scheme_Code = m.Scheme_Code
				and m.Effective_Dtm <= getdate() and m.Expiry_Dtm >= getdate()

			EXEC proc_CheckFeatureResultRowLimit @row_count=@rowcount, @result_limit_1st_enable=@result_limit_1st_enable, @result_limit_override_enable=@result_limit_override_enable,@override_result_limit=@override_result_limit     
		END TRY

		BEGIN CATCH    	    
			SET @row_cnt_error = ERROR_MESSAGE()    

			RAISERROR (@row_cnt_error,16,1)    
			CLOSE SYMMETRIC KEY sym_Key  
			RETURN
		END CATCH  

		SELECT	
			t_se.Enrolment_Ref_No, 
			isNull(t_se.SP_ID, '') as SP_ID, 
			SP_HKID, SP_Eng_Name, 
			isNull(SP_Chi_Name, '') as SP_Chi_Name, 
			Phone_Daytime,
			Already_Joined_EHR, 
			t_se.Record_Status, 
			Token_Serial_No, 
			isNull(Token_Serial_No_Replacement,'') as Token_Serial_No_Replacement,
			isnull(Project_Replacement, '') AS Project_Replacement, 
			isnull(Is_Share_Token_Replacement, '') AS Is_Share_Token_Replacement,
			isnull(Last_Replacement_Reason,'') as Last_Replacement_Reason,
			Project, 
			Is_Share_Token, 
			Token_Status, 
			t_se.Scheme_Code
		FROM	
			@tmp_sp_scheme_enrolment t_se
			INNER JOIN SchemeInformationStaging s
			ON t_se.Enrolment_Ref_No = s.Enrolment_Ref_No 
			AND s.Record_Status = 'A'
			AND ((@scheme_code is not null and s.scheme_code = @scheme_code) or @scheme_code is null)	
			INNER JOIN SchemeBackOffice m 
			ON ltrim(rtrim(s.Scheme_Code)) = ltrim(rtrim(m.Scheme_Code))
			and m.Effective_Dtm <= getdate() and m.Expiry_Dtm >= getdate()
		ORDER By t_se.Enrolment_Ref_No ASC

	END

END


-- -----------------------------------------------
-- Service Provider Status: Any
-- -----------------------------------------------
ELSE IF @status is null
BEGIN
	INSERT INTO @tmp_sp_processing (
		Enrolment_Ref_No,
		SP_ID,
		SP_HKID,
		SP_Eng_Name,
		SP_Chi_Name,
		Phone_Daytime,
		--Service_category_code,
		Already_Joined_EHR,
		Record_Status,
		Token_Serial_No,
		Token_Serial_No_Replacement,
		Project_Replacement,
		Is_Share_Token_Replacement,
		Last_Replacement_Reason,
		Project,
		Is_Share_Token,
		Token_Status,
		Scheme_Code)
	SELECT	DISTINCT TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable)) 
		SPS.Enrolment_Ref_No,
		SPS.SP_ID,
		convert(varchar, DecryptByKey(SPS.Encrypt_Field1)),
		convert(varchar(40), DecryptByKey(SPS.Encrypt_Field2)),
		convert(nvarchar, DecryptByKey(SPS.Encrypt_Field3)),
		SPS.Phone_Daytime,
		--PS.Service_category_code,
		SPS.Already_Joined_EHR,
		'N',
		'',
		'',
		'',
		'',
		'',
		'',
		'',
		'N',
		dbo.func_formatSchemeList(SPS.Enrolment_Ref_No,NULL,NULL,'STAGING')--''
	FROM	ServiceProviderStaging SPS
	LEFT JOIN ProfessionalStaging PS
	ON	SPS.Enrolment_Ref_No = PS.Enrolment_Ref_No
	WHERE	(@enrolment_ref_no is null or @enrolment_ref_no = SPS.Enrolment_Ref_No) and
			(@sp_hkid is null or SPS.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @sp_hkid)) and
			(@sp_eng_name is null or SPS.Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @sp_eng_name)) and
			(@phone_daytime is null or @phone_daytime= SPS.Phone_Daytime) and
			(@service_category_code is null or @service_category_code = PS.Service_Category_Code) and
			(@sp_id is null or @sp_id = SPS.SP_ID) and
			(@token_serial_no is null) and 
			SPS.Enrolment_Ref_No in (select AU.Enrolment_Ref_No from SPAccountUpdate AU 
										where AU.Issue_Token = 'Y' and AU.Progress_Status='T')

	INSERT INTO @tmp_sp_scheme_enrolment (Enrolment_Ref_No,
		SP_ID,
		SP_HKID,
		SP_Eng_Name,
		SP_Chi_Name,
		Phone_Daytime,
		--Service_category_code,
		Already_Joined_EHR,
		Record_Status,
		Token_Serial_No,
		Token_Serial_No_Replacement,
		Project_Replacement,
		Is_Share_Token_Replacement,
		Last_Replacement_Reason,
		Project,
		Is_Share_Token,
		Token_Status,
		Scheme_Code)
	SELECT	DISTINCT TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable)) 
		SPS.Enrolment_Ref_No,
		SPS.SP_ID,
		convert(varchar, DecryptByKey(SPS.Encrypt_Field1)),
		convert(varchar(40), DecryptByKey(SPS.Encrypt_Field2)),
		convert(nvarchar, DecryptByKey(SPS.Encrypt_Field3)),
		SPS.Phone_Daytime,
		--PS.Service_category_code,
		SPS.Already_Joined_EHR,
		'E',
		isnull(T.Token_Serial_No,'') as Token_Serial_No,
		'',
		'',
		'',
		'',
		isnull(T.Project,'') as Project,
		isnull(T.Is_Share_Token,'') AS Is_Share_Token,
		'N',
		dbo.func_formatSchemeList(SPS.Enrolment_Ref_No,'A',NULL,'STAGING')--''
	FROM	ServiceProviderStaging SPS
	Left Join Token T
	ON T.User_ID = SPS.SP_ID
	LEFT JOIN ProfessionalStaging PS
	ON	SPS.Enrolment_Ref_No = PS.Enrolment_Ref_No
	WHERE	
			(@enrolment_ref_no is null or @enrolment_ref_no = SPS.Enrolment_Ref_No) and
			(@sp_hkid is null or SPS.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @sp_hkid) ) and
			(@sp_eng_name is null or SPS.Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @sp_eng_name)) and
			(@phone_daytime is null or @phone_daytime= SPS.Phone_Daytime) and
			(@service_category_code is null or @service_category_code = PS.Service_Category_Code) and
			(@sp_id is null or @sp_id = SPS.SP_ID) and
			(@token_serial_no is null) and 
			SPS.Enrolment_Ref_No in (select AU.Enrolment_Ref_No from SPAccountUpdate AU 
										where AU.Scheme_Confirm = 'Y' and AU.Progress_Status='S')

	INSERT INTO @tmp_sp_enrolled_token (
		Enrolment_Ref_No,
		SP_ID,
		SP_HKID,
		SP_Eng_Name,
		SP_Chi_Name,
		Phone_Daytime,
		--Service_category_code,
		Already_Joined_EHR,
		Record_Status,
		Token_Serial_No,
		Token_Serial_No_Replacement,
		Project_Replacement,
		Is_Share_Token_Replacement,
		Last_Replacement_Reason,
		Project,
		Is_Share_Token,
		Token_Status,
		Scheme_Code)
	SELECT	DISTINCT TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable)) 
		SP.Enrolment_Ref_No,
		SP.SP_ID,
		convert(varchar, DecryptByKey(SP.Encrypt_Field1)),
		convert(varchar(40), DecryptByKey(SP.Encrypt_Field2)),
		convert(nvarchar, DecryptByKey(SP.Encrypt_Field3)),
		SP.Phone_Daytime,
		--P.Service_category_code,
		SP.Already_Joined_EHR,
		SP.Record_Status,
		T.Token_Serial_No,
		isnull(T.Token_Serial_No_Replacement,'') as Token_Serial_No_Replacement,
		isnull(T.Project_Replacement,'') as Project_Replacement,
		isnull(T.Is_Share_Token_Replacement,'') as Is_Share_Token_Replacement,
		isnull(T.Last_Replacement_Reason, '') as Last_Replacement_Reason,
		T.Project,
		T.Is_Share_Token,
		T.Record_Status,
		dbo.func_formatSchemeList(NULL,NULL,SP.SP_ID,'ENROLLED')--''
	FROM	ServiceProvider SP,Professional P,Token T
	WHERE	SP.SP_ID = P.SP_ID and T.User_ID = SP.SP_ID and
			(@enrolment_ref_no is null or @enrolment_ref_no = SP.Enrolment_Ref_No) and
			(@sp_id is null or @sp_id = SP.SP_ID) and
			(@sp_hkid is null or SP.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @sp_hkid)) and
			(@sp_eng_name is null or SP.Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @sp_eng_name)) and
			(@phone_daytime is null or @phone_daytime= SP.Phone_Daytime) and
			(@service_category_code is null or @service_category_code = P.Service_Category_Code) and
			(@token_serial_no is null or @token_serial_no = T.Token_Serial_No or @token_serial_no = T.Token_Serial_No_Replacement) 

	INSERT INTO @tmp_sp_enrolled_detoken (Enrolment_Ref_No,
		SP_ID,
		SP_HKID,
		SP_Eng_Name,
		SP_Chi_Name,
		Phone_Daytime,
		--Service_category_code,
		Already_Joined_EHR,
		Record_Status,
		Token_Serial_No,
		Token_Serial_No_Replacement,
		Project_Replacement,
		Is_Share_Token_Replacement,
		Last_Replacement_Reason,
		Project,
		Is_Share_Token,
		Token_Status,
		Scheme_Code)
	SELECT	DISTINCT TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable)) 
		SP.Enrolment_Ref_No,
		SP.SP_ID,
		convert(varchar, DecryptByKey(SP.Encrypt_Field1)),
		convert(varchar(40), DecryptByKey(SP.Encrypt_Field2)),
		convert(nvarchar, DecryptByKey(SP.Encrypt_Field3)),
		SP.Phone_Daytime,
		--P.Service_category_code,
		SP.Already_Joined_EHR,
		SP.Record_Status,
		'',
		'',
		'',
		'',
		'',
		'',
		'',
		'D',
		dbo.func_formatSchemeList(NULL,NULL,SP.SP_ID,'ENROLLED') --''
	FROM	ServiceProvider SP
	LEFT JOIN Professional P
	ON	SP.SP_ID = P.SP_ID
	WHERE	(@enrolment_ref_no is null or @enrolment_ref_no = SP.Enrolment_Ref_No) and
			(@sp_hkid is null or SP.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @sp_hkid)) and
			(@sp_eng_name is null or SP.Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @sp_eng_name)) and
			(@phone_daytime is null or @phone_daytime= SP.Phone_Daytime) and
			(@service_category_code is null or @service_category_code = P.Service_Category_Code) and
			(@sp_id is null or @sp_id = SP.SP_ID) and
			(@token_serial_no is null) and 
			SP.SP_ID not in (select T.User_ID from Token T)

	-- =============================================    
	-- Max Row Checking  
	-- =============================================  
	IF ISNULL(@scheme_code, '') = ''
	BEGIN
		BEGIN TRY       
			SELECT	@rowcount = count(1)
			FROM	
				(SELECT	Enrolment_Ref_No FROM @tmp_sp_processing 
					UNION ALL	SELECT	Enrolment_Ref_No FROM @tmp_sp_enrolled_token
					UNION ALL	SELECT  Enrolment_Ref_No FROM @tmp_sp_enrolled_detoken
					UNION ALL	SELECT	Enrolment_Ref_No FROM @tmp_sp_scheme_enrolment) SPProfileToken_ERN

			EXEC proc_CheckFeatureResultRowLimit @row_count=@rowcount, @result_limit_1st_enable=@result_limit_1st_enable, @result_limit_override_enable=@result_limit_override_enable,@override_result_limit=@override_result_limit     
		END TRY

		BEGIN CATCH    	    
			SET @row_cnt_error = ERROR_MESSAGE()    

			RAISERROR (@row_cnt_error,16,1)    
			CLOSE SYMMETRIC KEY sym_Key  
			RETURN
		END CATCH  

	
		SELECT	Enrolment_Ref_No, isNull(SP_ID, '') as SP_ID, SP_HKID, SP_Eng_Name, isNull(SP_Chi_Name, '') as SP_Chi_Name, Phone_Daytime,
				Already_Joined_EHR, Record_Status, Token_Serial_No, isNull(Token_Serial_No_Replacement,'') as Token_Serial_No_Replacement,
				isnull(Project_Replacement, '') as Project_Replacement, isnull(Is_Share_Token_Replacement, '') as Is_Share_Token_Replacement,
				isnull(Last_Replacement_Reason, '') as Last_Replacement_Reason,
				Project, Is_Share_Token, Token_Status, Scheme_Code
		FROM   (SELECT	Enrolment_Ref_No, isNull(SP_ID, '') as SP_ID, SP_HKID, SP_Eng_Name, isNull(SP_Chi_Name, '') as SP_Chi_Name, Phone_Daytime,
				Already_Joined_EHR, Record_Status, Token_Serial_No, isNull(Token_Serial_No_Replacement,'') as Token_Serial_No_Replacement,
				isnull(Project_Replacement, '') as Project_Replacement, isnull(Is_Share_Token_Replacement, '') as Is_Share_Token_Replacement,
				isnull(Last_Replacement_Reason, '') as Last_Replacement_Reason, 
				Project, Is_Share_Token, Token_Status, Scheme_Code
				FROM	@tmp_sp_processing
				Union
				SELECT	Enrolment_Ref_No, isNull(SP_ID, '') as SP_ID, SP_HKID, SP_Eng_Name, isNull(SP_Chi_Name, '') as SP_Chi_Name, Phone_Daytime,
				Already_Joined_EHR, Record_Status, Token_Serial_No, isNull(Token_Serial_No_Replacement,'') as Token_Serial_No_Replacement,
				isnull(Project_Replacement, '') as Project_Replacement, isnull(Is_Share_Token_Replacement, '') as Is_Share_Token_Replacement,
				isnull(Last_Replacement_Reason, '') as Last_Replacement_Reason,
				Project, Is_Share_Token, Token_Status, Scheme_Code
				FROM	@tmp_sp_enrolled_token
				Union
				SELECT	Enrolment_Ref_No, isNull(SP_ID, '') as SP_ID, SP_HKID, SP_Eng_Name, isNull(SP_Chi_Name, '') as SP_Chi_Name, Phone_Daytime,
				Already_Joined_EHR, Record_Status, Token_Serial_No, isNull(Token_Serial_No_Replacement,'') as Token_Serial_No_Replacement,
				isnull(Project_Replacement, '') as Project_Replacement, isnull(Is_Share_Token_Replacement, '') as Is_Share_Token_Replacement,
				isnull(Last_Replacement_Reason, '') as Last_Replacement_Reason,
				Project, Is_Share_Token, Token_Status, Scheme_Code
				FROM	@tmp_sp_enrolled_detoken
				Union
				SELECT	Enrolment_Ref_No, isNull(SP_ID, '') as SP_ID, SP_HKID, SP_Eng_Name, isNull(SP_Chi_Name, '') as SP_Chi_Name, Phone_Daytime,
				Already_Joined_EHR, Record_Status, Token_Serial_No, isNull(Token_Serial_No_Replacement,'') as Token_Serial_No_Replacement,
				isnull(Project_Replacement, '') as Project_Replacement, isnull(Is_Share_Token_Replacement, '') as Is_Share_Token_Replacement,
				isnull(Last_Replacement_Reason, '') as Last_Replacement_Reason,
				Project, Is_Share_Token, Token_Status, Scheme_Code
				FROM	@tmp_sp_scheme_enrolment
				) SPProfileToken_ERN
		ORDER By Enrolment_Ref_No ASC
	END
	ELSE
	BEGIN
		BEGIN TRY       
			SELECT	@rowcount = count(1)
			FROM	
				(SELECT	t_new.Enrolment_Ref_No 
					FROM @tmp_sp_processing t_new
					INNER JOIN SchemeInformationStaging s
					ON t_new.Enrolment_Ref_No = s.Enrolment_Ref_No 
					AND ((@scheme_code is not null and s.scheme_code = @scheme_code) or @scheme_code is null)	
					INNER JOIN SchemeBackOffice m 
					ON ltrim(rtrim(s.Scheme_Code)) = ltrim(rtrim(m.Scheme_Code))
					and m.Effective_Dtm <= getdate() and m.Expiry_Dtm >= getdate()

				UNION ALL	
					SELECT t_et.Enrolment_Ref_No 
					FROM @tmp_sp_enrolled_token t_et 					
						INNER JOIN SchemeInformation s
						ON t_et.SP_ID = s.SP_ID 
						AND ((@scheme_code is not null and s.scheme_code = @scheme_code) or @scheme_code is null)	
						INNER JOIN SchemeBackOffice m 
						ON ltrim(rtrim(s.Scheme_Code)) = ltrim(rtrim(m.Scheme_Code))
						and m.Effective_Dtm <= getdate() and m.Expiry_Dtm >= getdate()
				UNION ALL	
					SELECT t_edt.Enrolment_Ref_No 
					FROM @tmp_sp_enrolled_detoken t_edt
						INNER JOIN SchemeInformation s
						ON t_edt.SP_ID = s.SP_ID 
						AND ((@scheme_code is not null and s.scheme_code = @scheme_code) or @scheme_code is null)	
						INNER JOIN SchemeBackOffice m 
						ON ltrim(rtrim(s.Scheme_Code)) = ltrim(rtrim(m.Scheme_Code))
						and m.Effective_Dtm <= getdate() and m.Expiry_Dtm >= getdate()
				
				UNION ALL	
					SELECT t_se.Enrolment_Ref_No 
					FROM @tmp_sp_scheme_enrolment t_se
						INNER JOIN SchemeInformationStaging s
						ON t_se.Enrolment_Ref_No = s.Enrolment_Ref_No 
						AND s.Record_Status = 'A'
						AND ((@scheme_code is not null and s.scheme_code = @scheme_code) or @scheme_code is null)	
						INNER JOIN SchemeBackOffice m 
						ON ltrim(rtrim(s.Scheme_Code)) = ltrim(rtrim(m.Scheme_Code))
						and m.Effective_Dtm <= getdate() and m.Expiry_Dtm >= getdate()
					
				) SPProfileToken_ERN

			EXEC proc_CheckFeatureResultRowLimit @row_count=@rowcount, @result_limit_1st_enable=@result_limit_1st_enable, @result_limit_override_enable=@result_limit_override_enable,@override_result_limit=@override_result_limit     
		END TRY

		BEGIN CATCH    	    
			SET @row_cnt_error = ERROR_MESSAGE()    

			RAISERROR (@row_cnt_error,16,1)    
			CLOSE SYMMETRIC KEY sym_Key  
			RETURN
		END CATCH  

	
		SELECT	Enrolment_Ref_No, isNull(SP_ID, '') as SP_ID, SP_HKID, SP_Eng_Name, isNull(SP_Chi_Name, '') as SP_Chi_Name, Phone_Daytime,
				Already_Joined_EHR, Record_Status, Token_Serial_No, isNull(Token_Serial_No_Replacement,'') as Token_Serial_No_Replacement,
				isnull(Project_Replacement, '') as Project_Replacement, isnull(Is_Share_Token_Replacement, '') as Is_Share_Token_Replacement,
				isnull(Last_Replacement_Reason, '') as Last_Replacement_Reason,
				Project, Is_Share_Token, Token_Status, Scheme_Code
		FROM   (
				SELECT t_new.Enrolment_Ref_No, isNull(t_new.SP_ID, '') as SP_ID, SP_HKID, SP_Eng_Name, isNull(SP_Chi_Name, '') as SP_Chi_Name, Phone_Daytime,
				Already_Joined_EHR, t_new.Record_Status, Token_Serial_No, isNull(Token_Serial_No_Replacement,'') as Token_Serial_No_Replacement,
				isnull(Project_Replacement, '') as Project_Replacement, isnull(Is_Share_Token_Replacement, '') as Is_Share_Token_Replacement,
				isnull(Last_Replacement_Reason, '') as Last_Replacement_Reason, 
				Project, Is_Share_Token, Token_Status, t_new.Scheme_Code
				FROM @tmp_sp_processing t_new
					INNER JOIN SchemeInformationStaging s
					ON t_new.Enrolment_Ref_No = s.Enrolment_Ref_No 
					AND ((@scheme_code is not null and s.scheme_code = @scheme_code) or @scheme_code is null)	
					INNER JOIN SchemeBackOffice m 
					ON ltrim(rtrim(s.Scheme_Code)) = ltrim(rtrim(m.Scheme_Code))
					and m.Effective_Dtm <= getdate() and m.Expiry_Dtm >= getdate()
				
				Union
				SELECT t_et.Enrolment_Ref_No, isNull(t_et.SP_ID, '') as SP_ID, SP_HKID, SP_Eng_Name, isNull(SP_Chi_Name, '') as SP_Chi_Name, Phone_Daytime,
				Already_Joined_EHR, t_et.Record_Status, Token_Serial_No, isNull(Token_Serial_No_Replacement,'') as Token_Serial_No_Replacement,
				isnull(Project_Replacement, '') as Project_Replacement, isnull(Is_Share_Token_Replacement, '') as Is_Share_Token_Replacement,
				isnull(Last_Replacement_Reason, '') as Last_Replacement_Reason,
				Project, Is_Share_Token, Token_Status, t_et.Scheme_Code
				FROM @tmp_sp_enrolled_token t_et 					
					INNER JOIN SchemeInformation s
					ON t_et.SP_ID = s.SP_ID 
					AND ((@scheme_code is not null and s.scheme_code = @scheme_code) or @scheme_code is null)	
					INNER JOIN SchemeBackOffice m 
					ON ltrim(rtrim(s.Scheme_Code)) = ltrim(rtrim(m.Scheme_Code))
					and m.Effective_Dtm <= getdate() and m.Expiry_Dtm >= getdate()

				Union
				SELECT t_edt.Enrolment_Ref_No, isNull(t_edt.SP_ID, '') as SP_ID, SP_HKID, SP_Eng_Name, isNull(SP_Chi_Name, '') as SP_Chi_Name, Phone_Daytime,
				Already_Joined_EHR, t_edt.Record_Status, Token_Serial_No, isNull(Token_Serial_No_Replacement,'') as Token_Serial_No_Replacement,
				isnull(Project_Replacement, '') as Project_Replacement, isnull(Is_Share_Token_Replacement, '') as Is_Share_Token_Replacement,
				isnull(Last_Replacement_Reason, '') as Last_Replacement_Reason,
				Project, Is_Share_Token, Token_Status, t_edt.Scheme_Code
				FROM @tmp_sp_enrolled_detoken t_edt
					INNER JOIN SchemeInformation s
					ON t_edt.SP_ID = s.SP_ID 
					AND ((@scheme_code is not null and s.scheme_code = @scheme_code) or @scheme_code is null)	
					INNER JOIN SchemeBackOffice m 
					ON ltrim(rtrim(s.Scheme_Code)) = ltrim(rtrim(m.Scheme_Code))
					and m.Effective_Dtm <= getdate() and m.Expiry_Dtm >= getdate()
				
				Union
				SELECT t_se.Enrolment_Ref_No, isNull(t_se.SP_ID, '') as SP_ID, SP_HKID, SP_Eng_Name, isNull(SP_Chi_Name, '') as SP_Chi_Name, Phone_Daytime,
				Already_Joined_EHR, t_se.Record_Status, Token_Serial_No, isNull(Token_Serial_No_Replacement,'') as Token_Serial_No_Replacement,
				isnull(Project_Replacement, '') as Project_Replacement, isnull(Is_Share_Token_Replacement, '') as Is_Share_Token_Replacement,
				isnull(Last_Replacement_Reason, '') as Last_Replacement_Reason,
				Project, Is_Share_Token, Token_Status, t_se.Scheme_Code
				FROM @tmp_sp_scheme_enrolment t_se
					INNER JOIN SchemeInformationStaging s
					ON t_se.Enrolment_Ref_No = s.Enrolment_Ref_No 
					AND s.Record_Status = 'A'
					AND ((@scheme_code is not null and s.scheme_code = @scheme_code) or @scheme_code is null)	
					INNER JOIN SchemeBackOffice m 
					ON ltrim(rtrim(s.Scheme_Code)) = ltrim(rtrim(m.Scheme_Code))
					and m.Effective_Dtm <= getdate() and m.Expiry_Dtm >= getdate()
			) SPProfileToken_ERN
		ORDER By Enrolment_Ref_No ASC
	END
END
-- ------------------------------------------------------
-- Service Provider Status: Active, Delisted, Suspended
-- ------------------------------------------------------	
ELSE
BEGIN
					
	INSERT INTO @tmp_sp_enrolled_token (
		Enrolment_Ref_No,
		SP_ID,
		SP_HKID,
		SP_Eng_Name,
		SP_Chi_Name,
		Phone_Daytime,
		Already_Joined_EHR,
		Record_Status,
		Token_Serial_No,
		Token_Serial_No_Replacement,
		Project_Replacement,
		Is_Share_Token_Replacement,
		Last_Replacement_Reason,
		Project,
		Is_Share_Token,
		Token_Status,
		Scheme_Code)
	SELECT	DISTINCT  TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable)) 
		SP.Enrolment_Ref_No,
		SP.SP_ID,
		convert(varchar, DecryptByKey(SP.Encrypt_Field1)),
		convert(varchar(40), DecryptByKey(SP.Encrypt_Field2)),
		convert(nvarchar, DecryptByKey(SP.Encrypt_Field3)),
		SP.Phone_Daytime,
		SP.Already_Joined_EHR,
		SP.Record_Status,
		T.Token_Serial_No,
		isnull(T.Token_Serial_No_Replacement,'') as Token_Serial_No_Replacement,
		isnull(T.Project_Replacement,'') as Project_Replacement,
		isnull(T.Is_Share_Token_Replacement,'') as Is_Share_Token_Replacement,
		isnull(T.Last_Replacement_Reason, '') as Last_Replacement_Reason,
		T.Project,
		T.Is_Share_Token,
		T.Record_Status,
		dbo.func_formatSchemeList(NULL,NULL,SP.SP_ID,'ENROLLED')--''
	FROM	ServiceProvider SP,Professional P,Token T
	WHERE	SP.SP_ID = P.SP_ID and T.User_ID = SP.SP_ID and
			(@enrolment_ref_no is null or @enrolment_ref_no = SP.Enrolment_Ref_No) and
			(@sp_id is null or @sp_id = SP.SP_ID) and
			(@sp_hkid is null or SP.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @sp_hkid)) and
			(@sp_eng_name is null or SP.Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @sp_eng_name)) and
			(@phone_daytime is null or @phone_daytime= SP.Phone_Daytime) and
			(@service_category_code is null or @service_category_code = P.Service_Category_Code) and
			(@status = SP.Record_Status) and
			(@token_serial_no is null or @token_serial_no = T.Token_Serial_No or @token_serial_no = T.Token_Serial_No_Replacement) 

	INSERT INTO @tmp_sp_enrolled_detoken (
		Enrolment_Ref_No,
		SP_ID,
		SP_HKID,
		SP_Eng_Name,
		SP_Chi_Name,
		Phone_Daytime,
		Already_Joined_EHR,
		Record_Status,
		Token_Serial_No,
		Token_Serial_No_Replacement,
		Project_Replacement,
		Is_Share_Token_Replacement,
		Last_Replacement_Reason,
		Project,
		Is_Share_Token,
		Token_Status,
		Scheme_Code)
	SELECT	DISTINCT  TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable)) 
		SP.Enrolment_Ref_No,
		SP.SP_ID,
		convert(varchar, DecryptByKey(SP.Encrypt_Field1)),
		convert(varchar(40), DecryptByKey(SP.Encrypt_Field2)),
		convert(nvarchar, DecryptByKey(SP.Encrypt_Field3)),
		SP.Phone_Daytime,
		--P.Service_category_code,
		SP.Already_Joined_EHR,
		SP.Record_Status,
		'',
		'',
		'',
		'',
		'',
		'',
		'',
		'D',
		dbo.func_formatSchemeList(NULL,NULL,SP.SP_ID,'ENROLLED')--''
	FROM	ServiceProvider SP
	LEFT JOIN Professional P
	ON	SP.SP_ID = P.SP_ID
	WHERE	(@enrolment_ref_no is null or @enrolment_ref_no = SP.Enrolment_Ref_No) and
			(@sp_hkid is null or SP.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @sp_hkid)) and
			(@sp_eng_name is null or SP.Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @sp_eng_name)) and
			(@phone_daytime is null or @phone_daytime= SP.Phone_Daytime) and
			(@service_category_code is null or @service_category_code = P.Service_Category_Code) and
			(@sp_id is null or @sp_id = SP.SP_ID) and
			(@status = SP.Record_Status) and
			(@token_serial_no is null) and 
			SP.SP_ID not in (select T.User_ID from Token T) 

	-- =============================================    
	-- Max Row Checking  
	-- =============================================  
	IF ISNULL(@scheme_code, '') = ''
	BEGIN
		BEGIN TRY       
			SELECT	@rowcount = count(1)
			FROM	
				(SELECT	Enrolment_Ref_No FROM @tmp_sp_enrolled_token
				UNION ALL	
				SELECT	Enrolment_Ref_No FROM @tmp_sp_enrolled_detoken
				) SPProfileToken_ERN

			EXEC proc_CheckFeatureResultRowLimit @row_count=@rowcount, @result_limit_1st_enable=@result_limit_1st_enable, @result_limit_override_enable=@result_limit_override_enable,@override_result_limit=@override_result_limit     
		END TRY

		BEGIN CATCH    	    
			SET @row_cnt_error = ERROR_MESSAGE()    

			RAISERROR (@row_cnt_error,16,1)    
			CLOSE SYMMETRIC KEY sym_Key  
			RETURN
		END CATCH  
	
		SELECT	Enrolment_Ref_No, isNull(SP_ID, '') as SP_ID, SP_HKID, SP_Eng_Name, isNull(SP_Chi_Name, '') as SP_Chi_Name, Phone_Daytime,
				Already_Joined_EHR, Record_Status, Token_Serial_No, isNull(Token_Serial_No_Replacement,'') as Token_Serial_No_Replacement,
				isnull(Project_Replacement, '') as Project_Replacement, isnull(Is_Share_Token_Replacement, '') as Is_Share_Token_Replacement,
				isnull(Last_Replacement_Reason, '') as Last_Replacement_Reason,
				Project, Is_Share_Token, Token_Status, Scheme_Code
		FROM	(SELECT	Enrolment_Ref_No, isNull(SP_ID, '') as SP_ID, SP_HKID, SP_Eng_Name, isNull(SP_Chi_Name, '') as SP_Chi_Name, Phone_Daytime,
				Already_Joined_EHR, Record_Status, Token_Serial_No, isNull(Token_Serial_No_Replacement,'') as Token_Serial_No_Replacement,
				isnull(Project_Replacement, '') as Project_Replacement, isnull(Is_Share_Token_Replacement, '') as Is_Share_Token_Replacement,
				isnull(Last_Replacement_Reason, '') as Last_Replacement_Reason,
				Project, Is_Share_Token, Token_Status, Scheme_Code
				FROM	@tmp_sp_enrolled_token

				Union
				SELECT	Enrolment_Ref_No, isNull(SP_ID, '') as SP_ID, SP_HKID, SP_Eng_Name, isNull(SP_Chi_Name, '') as SP_Chi_Name, Phone_Daytime,
				Already_Joined_EHR, Record_Status, Token_Serial_No, isNull(Token_Serial_No_Replacement,'') as Token_Serial_No_Replacement,
				isnull(Project_Replacement, '') as Project_Replacement, isnull(Is_Share_Token_Replacement, '') as Is_Share_Token_Replacement,
				isnull(Last_Replacement_Reason, '') as Last_Replacement_Reason,
				Project, Is_Share_Token, Token_Status, Scheme_Code
				FROM	@tmp_sp_enrolled_detoken
				) SPProfileToken_ERN
			
		ORDER By Enrolment_Ref_No ASC
	END
	ELSE
	BEGIN
		BEGIN TRY       
			SELECT	@rowcount = count(1)
			FROM(
				SELECT t_et.Enrolment_Ref_No
				FROM @tmp_sp_enrolled_token t_et 					
				INNER JOIN SchemeInformation s
				ON t_et.SP_ID = s.SP_ID 
				AND ((@scheme_code is not null and s.scheme_code = @scheme_code) or @scheme_code is null)	
				INNER JOIN SchemeBackOffice m 
				ON ltrim(rtrim(s.Scheme_Code)) = ltrim(rtrim(m.Scheme_Code))
				and m.Effective_Dtm <= getdate() and m.Expiry_Dtm >= getdate()

				UNION ALL	
				SELECT t_edt.Enrolment_Ref_No 
				FROM @tmp_sp_enrolled_detoken t_edt
				INNER JOIN SchemeInformation s
				ON t_edt.SP_ID = s.SP_ID 
				AND ((@scheme_code is not null and s.scheme_code = @scheme_code) or @scheme_code is null)	
				INNER JOIN SchemeBackOffice m 
				ON ltrim(rtrim(s.Scheme_Code)) = ltrim(rtrim(m.Scheme_Code))
				and m.Effective_Dtm <= getdate() and m.Expiry_Dtm >= getdate()

				) SPProfileToken_ERN

			EXEC proc_CheckFeatureResultRowLimit @row_count=@rowcount, @result_limit_1st_enable=@result_limit_1st_enable, @result_limit_override_enable=@result_limit_override_enable,@override_result_limit=@override_result_limit     
		END TRY

		BEGIN CATCH    	    
			SET @row_cnt_error = ERROR_MESSAGE()    

			RAISERROR (@row_cnt_error,16,1)    
			CLOSE SYMMETRIC KEY sym_Key  
			RETURN
		END CATCH  
	
		SELECT	Enrolment_Ref_No, isNull(SP_ID, '') as SP_ID, SP_HKID, SP_Eng_Name, isNull(SP_Chi_Name, '') as SP_Chi_Name, Phone_Daytime,
				Already_Joined_EHR, Record_Status, Token_Serial_No, isNull(Token_Serial_No_Replacement,'') as Token_Serial_No_Replacement,
				isnull(Project_Replacement, '') as Project_Replacement, isnull(Is_Share_Token_Replacement, '') as Is_Share_Token_Replacement,
				isnull(Last_Replacement_Reason, '') as Last_Replacement_Reason,
				Project, Is_Share_Token, Token_Status, Scheme_Code
		FROM	(SELECT	t_et.Enrolment_Ref_No, isNull(t_et.SP_ID, '') as SP_ID, SP_HKID, SP_Eng_Name, isNull(SP_Chi_Name, '') as SP_Chi_Name, Phone_Daytime,
				Already_Joined_EHR, t_et.Record_Status, Token_Serial_No, isNull(Token_Serial_No_Replacement,'') as Token_Serial_No_Replacement,
				isnull(Project_Replacement, '') as Project_Replacement, isnull(Is_Share_Token_Replacement, '') as Is_Share_Token_Replacement,
				isnull(Last_Replacement_Reason, '') as Last_Replacement_Reason,
				Project, Is_Share_Token, Token_Status, t_et.Scheme_Code
				FROM @tmp_sp_enrolled_token t_et 					
					INNER JOIN SchemeInformation s
					ON t_et.SP_ID = s.SP_ID 
					AND ((@scheme_code is not null and s.scheme_code = @scheme_code) or @scheme_code is null)	
					INNER JOIN SchemeBackOffice m 
					ON ltrim(rtrim(s.Scheme_Code)) = ltrim(rtrim(m.Scheme_Code))
					and m.Effective_Dtm <= getdate() and m.Expiry_Dtm >= getdate()

				Union
				SELECT t_edt.Enrolment_Ref_No, isNull(t_edt.SP_ID, '') as SP_ID, SP_HKID, SP_Eng_Name, isNull(SP_Chi_Name, '') as SP_Chi_Name, Phone_Daytime,
				Already_Joined_EHR, t_edt.Record_Status, Token_Serial_No, isNull(Token_Serial_No_Replacement,'') as Token_Serial_No_Replacement,
				isnull(Project_Replacement, '') as Project_Replacement, isnull(Is_Share_Token_Replacement, '') as Is_Share_Token_Replacement,
				isnull(Last_Replacement_Reason, '') as Last_Replacement_Reason,
				Project, Is_Share_Token, Token_Status, t_edt.Scheme_Code
				FROM @tmp_sp_enrolled_detoken t_edt
					INNER JOIN SchemeInformation s
					ON t_edt.SP_ID = s.SP_ID 
					AND ((@scheme_code is not null and s.scheme_code = @scheme_code) or @scheme_code is null)	
					INNER JOIN SchemeBackOffice m 
					ON ltrim(rtrim(s.Scheme_Code)) = ltrim(rtrim(m.Scheme_Code))
					and m.Effective_Dtm <= getdate() and m.Expiry_Dtm >= getdate()
				) SPProfileToken_ERN
			
		ORDER By Enrolment_Ref_No ASC
	END
END

CLOSE SYMMETRIC KEY sym_Key
END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderTokenAll_get_bySPInfo] TO HCVU
GO
