IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderStagingSPAccUpd_get_bySPInfo]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderStagingSPAccUpd_get_bySPInfo]
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
-- Create date: 02 June 2008
-- Description:	Retrieve Service Provider Information from Table 
--				"ServiceProviderStaging" and "SPAccountUpdate"
--				Progress_Status is 'V - VettingStage'
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	  Kathy LEE
-- Modified date: 08 June 2009
-- Description:	  Add Scheme Code
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
-- Modified by:		Kathy LEE
-- Modified date:	5 August 2009
-- Description:		1. Retrieve Scheme information by using 'SchemeBackOffice'
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Clark YIP
-- Modified date:	6 August 2009
-- Description:		1. 'SchemeBackOffice' filter by effective_dtm and expiry_dtm
-- =============================================

--exec proc_ServiceProviderStagingSPAccUpd_get_bySPInfo null, null, null, null, null, null, null, null, 'V', 1 ,0,0

CREATE PROCEDURE [dbo].[proc_ServiceProviderStagingSPAccUpd_get_bySPInfo]
	@enrolment_ref_no		char(15), 
	@sp_id					char(8), 
	@sp_hkid				char(9), 
	@sp_eng_name			varchar(40),
	@phone_daytime			varchar(20), 
	@service_category_code	char(5), 
	@record_status			char(1), 
	@scheme_code			char(10),
	@progress_status		char(1),
	@result_limit_1st_enable BIT, 
	@result_limit_override_enable BIT,
	@override_result_limit BIT

AS
BEGIN
	EXEC [proc_SymmetricKey_open]
	
	SET NOCOUNT ON;
	
	DECLARE @TempSP_Staging TABLE	(
									Enrolment_Ref_No	char(15),
									Enter_Confirm_Dtm	datetime,						
									SP_ID				char(8),						
									Encrypt_Field1		varbinary(100),
									Encrypt_Field2		varbinary(100),
									Encrypt_Field3		varbinary(100),
									Phone_Daytime		varchar(20),
									Record_Status		char(2),
									Scheme_Code			varchar(255)
									)
	
	DECLARE @rowcount int
	DECLARE @row_cnt_error varchar(max)
		
	SELECT	@rowcount = COUNT(DISTINCT SPS.Enrolment_Ref_No)
	FROM	ServiceProviderStaging SPS
				INNER JOIN ProfessionalStaging PS
					ON SPS.Enrolment_Ref_No = PS.Enrolment_Ref_No 
				INNER JOIN SchemeInformationStaging SIS
					ON SPS.enrolment_ref_no = sis.enrolment_ref_no 
				INNER JOIN SPAccountUpdate SPAU
					ON SPS.Enrolment_Ref_No = SPAU.Enrolment_Ref_No 
						AND SPAU.Progress_Status = @progress_status
				INNER JOIN ServiceProviderVerification SPV
					ON SPS.Enrolment_Ref_No = SPV.Enrolment_Ref_No
				
	WHERE	(@enrolment_ref_no IS NULL OR @enrolment_ref_no = SPS.Enrolment_Ref_No)
				AND (@sp_id IS NULL OR @sp_id = SPS.SP_ID) 
				AND (@sp_hkid IS NULL OR EncryptByKey(KEY_GUID('sym_Key'), @sp_hkid) = SPS.Encrypt_Field1) 
				AND (@sp_eng_name IS NULL OR EncryptByKey(KEY_GUID('sym_Key'), @sp_eng_name) = SPS.Encrypt_Field2) 
				AND (@phone_daytime IS NULL OR @phone_daytime= SPS.Phone_Daytime) 
				AND (@service_category_code IS NULL OR @service_category_code = PS.Service_Category_Code) 
				AND (@record_status IS NULL OR @record_status = SPV.Record_Status)
				AND (@scheme_code IS NULL OR @scheme_code = SIS.Scheme_Code)

	
	-- =============================================    
	-- Max Row Checking  
	-- =============================================  
	BEGIN TRY       
		EXEC proc_CheckFeatureResultRowLimit @row_count=@rowcount, @result_limit_1st_enable=@result_limit_1st_enable, @result_limit_override_enable=@result_limit_override_enable,@override_result_limit=@override_result_limit     
	END TRY

	BEGIN CATCH    	    
		SET @row_cnt_error = ERROR_MESSAGE()    

		RAISERROR (@row_cnt_error,16,1)    
		EXEC [proc_SymmetricKey_close]
		RETURN
	END CATCH  

	INSERT INTO @TempSP_Staging
	(
		Enrolment_Ref_No,
		Enter_Confirm_Dtm,	
		SP_ID,						
		Encrypt_Field1,
		Encrypt_Field2,
		Encrypt_Field3,
		Phone_Daytime,
		Record_Status
	)	
	SELECT	DISTINCT SPS.Enrolment_Ref_No,
			SPV.Enter_Confirm_Dtm,
			SPS.SP_ID,
			SPS.Encrypt_Field1,
			SPS.Encrypt_Field2,
			SPS.Encrypt_Field3,
			SPS.Phone_Daytime,			
			SPV.Record_Status
			
	FROM	ServiceProviderStaging SPS
				INNER JOIN ProfessionalStaging PS
					ON SPS.Enrolment_Ref_No = PS.Enrolment_Ref_No
				INNER JOIN SchemeInformationStaging SIS
					ON SPS.enrolment_ref_no = sis.enrolment_ref_no 
				INNER JOIN SPAccountUpdate SPAU
					ON SPS.Enrolment_Ref_No = SPAU.Enrolment_Ref_No
						AND SPAU.Progress_Status = @progress_status
				INNER JOIN ServiceProviderVerification SPV
					ON SPS.Enrolment_Ref_No = SPV.Enrolment_Ref_No
		
	WHERE	(@enrolment_ref_no IS NULL OR @enrolment_ref_no = SPS.Enrolment_Ref_No)
				AND (@sp_id IS NULL OR @sp_id = SPS.SP_ID) 
				AND (@sp_hkid IS NULL OR EncryptByKey(KEY_GUID('sym_Key'), @sp_hkid) = SPS.Encrypt_Field1) 
				AND (@sp_eng_name IS NULL OR EncryptByKey(KEY_GUID('sym_Key'), @sp_eng_name) = SPS.Encrypt_Field2) 
				AND (@phone_daytime IS NULL OR @phone_daytime= SPS.Phone_Daytime) 
				AND (@service_category_code IS NULL OR @service_category_code = PS.Service_Category_Code) 
				AND (@record_status IS NULL OR @record_status = SPV.Record_Status)
				AND (@scheme_code IS NULL OR @scheme_code = SIS.Scheme_Code)
			
	UPDATE	@TempSP_Staging
	SET		Scheme_Code = SUBSTRING( T.scheme_code, 3, LEN(T.scheme_code) )	
	FROM	(
				SELECT [Scheme].enrolment_ref_no, 
				(
					SELECT	', ' + LTRIM(RTRIM(M.display_code))
					FROM	SchemeInformationStaging SIS, SchemeBackOffice M
					WHERE	SIS.Scheme_Code = M.Scheme_Code
					AND	SIS.Enrolment_Ref_No = [Scheme].Enrolment_Ref_No 
					AND M.Effective_Dtm <= getdate() and M.Expiry_Dtm >= getdate()
					ORDER BY M.display_seq
					FOR XML PATH('')
				) AS Scheme_Code
				
				FROM	(
							SELECT	DISTINCT Enrolment_Ref_No 
							FROM	SchemeInformationStaging
						) AS [Scheme]
			) AS T
				INNER JOIN @TempSP_Staging S
					ON T.Enrolment_Ref_No = S.Enrolment_Ref_No COLLATE database_default
						AND ISNULL(S.Scheme_Code, '') = ''
						
		
	SELECT	DISTINCT Enrolment_Ref_No,
			Enter_Confirm_Dtm,
			SP_ID,			
			CONVERT(varchar, DecryptByKey(Encrypt_Field1)) AS SP_HKID,
			CONVERT(varchar(40), DecryptByKey(Encrypt_Field2)) AS SP_Eng_Name,
			ISNULL(CONVERT(nvarchar, DecryptByKey(Encrypt_Field3)), '') AS SP_Chi_Name,
			Phone_Daytime,			
			Record_Status,
			Scheme_Code

	FROM	@TempSP_Staging

	EXEC [proc_SymmetricKey_close]
	
END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderStagingSPAccUpd_get_bySPInfo] TO HCVU
GO
