IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderSPAccMaintenance_get_bySPInfo]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderSPAccMaintenance_get_bySPInfo]
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
-- Modified by:		Marco CHOI
-- Modified date:	01 Feb 2018
-- CR No.:			CRE17-012
-- Description:		Add Chinese name search for SP and EHA
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
-- Create date: 05 July 2008
-- Description:	Retrieve Service Provider Information from Table 
--				"ServiceProvider" and "SPAccountMaintenance"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Kathy LEE
-- Modified date: 06 Apirl 2009
-- Description:	Add 1 search criteria 'Scheme Code'
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	11 May 2009
-- Description:		Change 'HCVS' to 'EHCVS' on the WHERE clause
--					in updating the temp table
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	20 May 2009
-- Description:		Revise the algorithm on constructing the "Scheme_Code" string
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	01 Jun 2009
-- Description:		System Parameter Add Scheme Code
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
-- Modified by:		Lawrence TSANG
-- Modified date:	3 July 2009
-- Description:		Generalize the function on Scheme_Code construction
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	05 July 2009
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
-- Modified by:		Lawrence TSANG
-- Modified date:	30 July 2009
-- Description:		Remove the logic on getting the Record_Status on Scheme
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Clark YIP
-- Modified date:	05 Aug 2009
-- Description:		Inner join the SchemeBackOffice
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	6 August 2009
-- Description:		Check the [SchemeInformation].[Effective_Dtm] is fallen between [SchemeBackOffice].[Effective_Dtm] and [SchemeBackOffice].[Expiry_Dtm]
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE12-001
-- Modified by:		Koala CHENG
-- Modified date:	28 Feb 2012
-- Description:		1. Grant permission to WSINT for PCDInterface
-- =============================================

--exec proc_ServiceProviderSPAccMaintenance_get_bySPInfo null,null,null,null,'23456789',null,null,1,1,1 --20074

CREATE PROCEDURE [dbo].[proc_ServiceProviderSPAccMaintenance_get_bySPInfo]
	@enrolment_ref_no				char(15), 
	@sp_id							char(8), 
	@sp_hkid						char(9), 
	@sp_eng_name					varchar(40),
	@sp_chi_name					nvarchar(6),
	@phone_daytime					varchar(20), 
	@service_category_code			char(5), 
	@scheme_code					char(10),
	@result_limit_1st_enable		BIT, 
	@result_limit_override_enable	BIT,
	@override_result_limit			BIT

AS
BEGIN
	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
	DECLARE @row_cnt_error varchar(max)
	DECLARE @rowcount	int

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	
-- =============================================
-- Return results
-- =============================================
	EXEC [proc_SymmetricKey_open]
	
	SET NOCOUNT ON;

	DECLARE @TempServiceProvider TABLE
	(
		Enrolment_Ref_No	char(15),
		SP_ID				char(8),
		SP_HKID				char(9),
		SP_Eng_Name			varchar(40),
		SP_Chi_Name			nvarchar(6),
		Phone_Daytime		varchar(20),
		Record_Status		char(2),
		Scheme_Code			varchar(250)
	)

	INSERT INTO	@TempServiceProvider
	(
		Enrolment_Ref_No,
		SP_ID,
		SP_HKID,
		SP_Eng_Name,
		SP_Chi_Name,
		Phone_Daytime,
		Record_Status,
		Scheme_Code
	)
	
	SELECT		DISTINCT  TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable)) 
				SP.Enrolment_Ref_No,
				SP.SP_ID,
				CONVERT(varchar, DecryptByKey(SP.Encrypt_Field1)) AS SP_HKID,
				CONVERT(varchar(40), DecryptByKey(SP.Encrypt_Field2)) AS SP_Eng_Name,
				ISNULL(CONVERT(nvarchar, DecryptByKey(SP.Encrypt_Field3)), '') AS SP_Chi_Name,
				SP.Phone_Daytime,
				SP.Record_Status,	
				''	-- Empty String for Scheme_Code

	FROM		ServiceProvider SP
					INNER JOIN SchemeInformation SPSI
						ON SP.SP_ID = SPSI.SP_ID
					INNER JOIN	Professional P
						ON SP.SP_ID = P.SP_ID
					INNER JOIN HCSPUserAC HCSPAC
						ON HCSPAC.SP_ID = SP.SP_ID
					LEFT JOIN	(
									SELECT	SP_ID, Record_Status, Upd_Type 
									FROM	SPAccountMaintenance 
									WHERE	Record_Status = 'A' 
												AND Upd_Type <> 'DT' 
												AND Upd_Type <> 'AT'
												AND SP_Practice_Display_Seq IS NULL
								) M
						ON SP.SP_ID = M.SP_ID 
							AND M.Record_Status = 'A'
		
	WHERE		(@enrolment_ref_no IS NULL OR @enrolment_ref_no = SP.Enrolment_Ref_No) 
					AND (@sp_id IS NULL OR @sp_id = SP.SP_ID) 
					AND (@sp_hkid IS NULL OR EncryptByKey(KEY_GUID('sym_Key'), @sp_hkid) = SP.Encrypt_Field1) 
					AND (@sp_eng_name IS NULL OR EncryptByKey(KEY_GUID('sym_Key'), @sp_eng_name) = SP.Encrypt_Field2)
					AND (@sp_chi_name IS NULL OR EncryptByKey(KEY_GUID('sym_Key'), @sp_chi_name) = SP.Encrypt_Field3) 
					AND (@phone_daytime IS NULL OR @phone_daytime= SP.Phone_Daytime) 
					AND (@service_category_code IS NULL OR @service_category_code = P.Service_Category_Code) 
					AND (@scheme_code IS NULL OR @scheme_code = SPSI.Scheme_Code)
				
	ORDER BY	Enrolment_Ref_No ASC 

	-- =============================================    
	-- Max Row Checking  
	-- ============================================= 
	BEGIN TRY       
			SELECT	@rowcount = COUNT(1)
			FROM	@TempServiceProvider

		EXEC proc_CheckFeatureResultRowLimit @row_count=@rowcount, @result_limit_1st_enable=@result_limit_1st_enable, @result_limit_override_enable=@result_limit_override_enable,@override_result_limit=@override_result_limit     
	END TRY

	BEGIN CATCH    	    
		SET @row_cnt_error = ERROR_MESSAGE()    

		RAISERROR (@row_cnt_error,16,1)    
		CLOSE SYMMETRIC KEY sym_Key  
		RETURN
	END CATCH  

	DECLARE @TempSchemeInfo TABLE
	(
		SP_ID			char(8),
		Scheme_Code		char(10),
		Effective_Dtm	datetime
	)
	
	INSERT INTO @TempSchemeInfo
	(
		SP_ID,
		Scheme_Code,
		Effective_Dtm
	)
	SELECT		SP_ID,
				Scheme_Code,
				Effective_Dtm
	FROM		SchemeInformation

-- Construct the "Scheme_Code" string
	UPDATE	@TempServiceProvider
	SET		P.Scheme_Code = P.Scheme_Code + S.Scheme_Code
	FROM	@TempServiceProvider P 
				INNER JOIN	(	SELECT	T1.SP_ID,
										(	SELECT		LTRIM(RTRIM(B.Display_Code)) + ', '
											FROM		@TempSchemeInfo T
															INNER JOIN SchemeBackOffice B
																ON T.Scheme_Code = B.Scheme_Code
																	AND T.Effective_Dtm BETWEEN B.Effective_Dtm AND B.Expiry_Dtm
											WHERE		T.SP_ID = T1.SP_ID 
											ORDER BY	B.Display_Seq
											FOR			XML PATH('')
										) AS Scheme_Code
								FROM	(SELECT DISTINCT SP_ID FROM @TempSchemeInfo) AS T1
							) AS S
					ON P.SP_ID = S.SP_ID

-- Drop the last ", "
	UPDATE	@TempServiceProvider
	SET		T.Scheme_Code = LEFT(T.Scheme_Code, LEN(T.Scheme_Code) - 1)
	FROM	@TempServiceProvider T
	
-- Final Result
	SELECT	Enrolment_Ref_No,
			SP_ID,
			SP_HKID,
			SP_Eng_Name,
			SP_Chi_Name,
			Phone_Daytime,
			Record_Status,
			Scheme_Code
	FROM	@TempServiceProvider
	
	EXEC [proc_SymmetricKey_close]

END

GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderSPAccMaintenance_get_bySPInfo] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderSPAccMaintenance_get_bySPInfo] TO WSINT
GO
