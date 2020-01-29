IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_BankAccountVerification_get_byAny]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_BankAccountVerification_get_byAny]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================  
-- Modification History      
-- CR No.:		   CRE16-002  
-- Modified by:    Chris YIM 
-- Modified date:  29 Aug 2016  
-- Description:    Use "Inner Join" in WHERE CLAUSE instead of "Like" to retrieve result set
-- =============================================      
-- =============================================  
-- Modification History      
-- CR No.:		   CRE12-014  
-- Modified by:    Karl LAM   
-- Modified date:  03 Jan 2013  
-- Description:    Add parameters: @result_limit_1st_enable,@result_limit_override_enable, @override_result_limit  for relax 500 rows limitation  
-- =============================================      
-- =============================================   
-- Author:		Clark Yip
-- Create date: 28 April 2008
-- Description:	Retrieve the Bank Account, SP, Practice information (ych480:Encryption)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Clark YIP
-- Modified date: 07 May 2009
-- Description:	  Get the vetting dtm, scheme_code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	27 May 2009
-- Description:		System Parameter Add Scheme Code
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
-- Modified by:		Clark YIP
-- Modified date:	13 July 2009
-- Description:		Using ExternalCode instead of using MScheme_Code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	13 July 2009
-- Description:		Add ISNULL() at the first if-clause in the final result
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
-- =============================================
-- Modification History
-- Modified by:		Paul YIP
-- Modified date:	12 August 2009
-- Description:		1. trim the scheme code
-- =============================================

--exec proc_BankAccountVerification_get_byAny 'U',null,null,null,null,null, null, null,1,1,1

CREATE PROCEDURE [dbo].[proc_BankAccountVerification_get_byAny] 
	 @record_status	char(1)
	,@enrolment_ref_no	Char(15)	
	,@sp_id	      	char(8)
	,@sp_hkid		Char(9)
	,@sp_name	    varchar(40)
	,@service_type 	char(5)
	,@contact_no	varchar(20)	
	,@scheme_code	varchar(100)
	,@result_limit_1st_enable BIT
	,@result_limit_override_enable BIT
	,@override_result_limit BIT
AS
BEGIN
-- =============================================
-- Declaration
-- =============================================
DECLARE @rowcount int
DECLARE @row_cnt_error varchar(max)
DECLARE	@ern char(15)

DECLARE @tmp_bk_verification table( 
	appNum char(15),
	SPName varchar(40),
	SPChiName nvarchar(6),
	status char(1),
	bankStatus char(1),						
	SPID char(8),
	SPHKID char(9),						
	DaytimeContact varchar(20),
	Vetting_Dtm datetime,
	Scheme_Code varchar(100))

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================

OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key
-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
-- =============================================
-- Return results
-- =============================================
INSERT INTO @tmp_bk_verification
		(appNum,
		SPName,
		SPChiName,
		[status],
		bankStatus,
		SPID,
		SPHKID,
		DaytimeContact,
		Vetting_Dtm,
		Scheme_Code)
	SELECT	DISTINCT  TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable)) 
		b.enrolment_ref_no as appNum, 
		convert(varchar(40), DecryptByKey(sp.[Encrypt_Field2])) as SPName, --sp.sp_eng_name  as SPName,
		convert(nvarchar, DecryptByKey(sp.[Encrypt_Field3])) as SPChiName, --sp.sp_chi_name  as SPChiName,
		b.record_status as [status],
		bv.record_status as bankStatus,
		sp.SP_ID as SPID,
		convert(varchar, DecryptByKey(sp.[Encrypt_Field1])) as SPHKID, --sp.SP_HKID as SPHKID,
		sp.Phone_Daytime as DaytimeContact,
		spv.Vetting_Dtm, 
		dbo.func_formatSchemeList(b.Enrolment_Ref_No,NULL,NULL,'STAGING')
	FROM	
		BankAccountStaging b, 
		ServiceProviderStaging sp, 
		PracticeStaging p, 
		BankAccVerification bv, 
		SPAccountUpdate u, 
		ProfessionalStaging prof, 
		ServiceProviderVerification spv
	WHERE	b.Enrolment_ref_no = sp.Enrolment_ref_no 
		and p.Enrolment_ref_no = prof.Enrolment_ref_no 
		and p.Professional_seq = prof.professional_seq
		and b.Enrolment_ref_no = p.Enrolment_ref_no
		and b.Enrolment_ref_no = bv.Enrolment_ref_no and b.display_seq = bv.display_seq
		and b.Enrolment_ref_no = u.Enrolment_ref_no 		
		and b.sp_practice_display_seq = p.display_seq
		and bv.sp_practice_display_seq = b.sp_practice_display_seq
		and u.progress_status='B'
		and spv.Enrolment_Ref_No=b.enrolment_ref_no
		AND ((@record_status<>'' AND bv.record_status = @record_status) OR (@record_status='' AND bv.record_status in ('U','D')))
		AND ((LTRIM(RTRIM(@enrolment_ref_no)) <> '' AND b.Enrolment_ref_no = @enrolment_ref_no) OR (LTRIM(RTRIM(@enrolment_ref_no)) = ''))
		AND ((LTRIM(RTRIM(@sp_id)) <> '' AND sp.sp_id = @sp_id) OR (LTRIM(RTRIM(@sp_id)) = ''))				
		AND ((LTRIM(RTRIM(@sp_hkid)) <> '' AND convert(varchar, DecryptByKey(sp.[Encrypt_Field1])) = @sp_hkid) OR (LTRIM(RTRIM(@sp_hkid)) = ''))
		AND ((LTRIM(RTRIM(@contact_no)) <> '' AND sp.Phone_Daytime = @contact_no) OR (LTRIM(RTRIM(@contact_no)) = ''))
		AND ((LTRIM(RTRIM(@service_type)) <>'' AND prof.Service_Category_Code = @service_type) OR LTRIM(RTRIM(@service_type)) = '')
		AND ((LTRIM(RTRIM(@sp_name)) <> '' AND convert(varchar(40), DecryptByKey(sp.[Encrypt_Field2])) = @sp_name) OR (LTRIM(RTRIM(@sp_name)) = ''))


	-- Check the no of rows of the result
	IF ISNULL(@scheme_code, '') =''
	BEGIN
		-- =============================================    
		-- Max Row Checking  
		-- =============================================  
		BEGIN TRY       
			SELECT	@rowcount = count(1) FROM	@tmp_bk_verification
			EXEC proc_CheckFeatureResultRowLimit @row_count=@rowcount, @result_limit_1st_enable=@result_limit_1st_enable, @result_limit_override_enable=@result_limit_override_enable,@override_result_limit=@override_result_limit     
		END TRY

		BEGIN CATCH    	    
			SET @row_cnt_error = ERROR_MESSAGE()    

			RAISERROR (@row_cnt_error,16,1)    
			CLOSE SYMMETRIC KEY sym_Key  
			RETURN
		END CATCH  

		
		SELECT	appNum,
				SPName,
				SPChiName,
				status,
				bankStatus,
				SPID,
				SPHKID,
				DaytimeContact,
				Vetting_Dtm,
				Scheme_Code
		FROM	@tmp_bk_verification
	END
	ELSE
	BEGIN
		-- =============================================    
		-- Max Row Checking  
		-- =============================================  
		BEGIN TRY       
			SELECT	
				@rowcount = count(1) 
			FROM 
				@tmp_bk_verification t
				INNER JOIN SchemeInformationStaging s
				ON t.appNum = s.Enrolment_Ref_No AND s.Scheme_Code = @scheme_code
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

		SELECT	appNum,
				SPName,
				SPChiName,
				status,
				bankStatus,
				SPID,
				SPHKID,
				DaytimeContact,
				Vetting_Dtm,
		        t.Scheme_Code
        FROM @tmp_bk_verification t
                INNER JOIN SchemeInformationStaging s
                ON t.appNum = s.Enrolment_Ref_No AND s.Scheme_Code = @scheme_code
                INNER JOIN SchemeBackOffice m 
                ON s.Scheme_Code = m.Scheme_Code
                and m.Effective_Dtm <= getdate() and m.Expiry_Dtm >= getdate()

	END	
	
	CLOSE SYMMETRIC KEY sym_Key
END
GO

GRANT EXECUTE ON [dbo].[proc_BankAccountVerification_get_byAny] TO HCVU
GO
