IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherAccountRectificationList_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherAccountRectificationList_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			INT21-0022 (HCVU Claim Transaction Performance Tuning)
-- Modified by:		Winnie SUEN
-- Modified date:	02 Sep 2021
-- Description:		(1) Search with Raw Doc No. to handle "Search by any doc type issue"
--					(2) Fine Tune performance with adding "OPTION (RECOMPILE)"
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			INT21-0016
-- Modified by:		Martin Tang
-- Modified date:	06 Aug 2021
-- Description:		Roll back search raw doc no. to handle search account timeout issue
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-023
-- Modified by:		Martin Tang
-- Modified date:	19 July 2021
-- Description:		Fix "Search by any doc type issue"
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-023
-- Modified by:		Martin Tang
-- Modified date:	16 June 2021
-- Description:		Extend patient name's maximum length (varbinary 100->200)
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-023-35
-- Modified by:		Winnie SUEN
-- Modified date:	27 Apr 2021
-- Description:		Add with nolock
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	22 Jan 2019
-- CR No.:			INT18-0030
-- Description:		Fix the search on validated account's amendment record
-- =============================================
-- =============================================  
-- Modification History
-- Modified by:		Koala CHENG
-- CR No.			CRE17-018 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19
-- Modified date:	15 Aug 2018 
-- Description:		1. Add input param for filtering account status
--					2. Add input param for filtering document type
--					3. Retrieve temp account with status Restricted "R" (Created by student file upload and cannot be validated)
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
-- Modified by:  Paul Yip  
-- Modified date: 22 Sep 2010    
-- Description:  Fix temp table length   
-- =============================================    
-- =============================================  
-- Modification History  
-- Modified by:  Derek LEUNG  
-- Modified date: 13 August 2010  
-- Description:  Add IdentityNum, AdoptionPrefixNum as search criteria  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:  Paul Yip  
-- Modified date: 9 August 2010  
-- Description:  Retrieve both temporary account and special account  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:  Kathy LEE  
-- Modified date: 01 Feb 2010  
-- Description:  Only retieve the temporary account which account_purpose = 'A'  
--     and is(are) created in HCVU platfrom  
-- =============================================  
-- =============================================  
-- Author:  Timothy LEUNG  
-- Create date: 29 September 2008  
-- Description: Retrieve the fail validated Temp Voucher Account   
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:     Timothy LEUNG  
-- Modified date: 20 Oct 2008  
-- Description:  Add Certification of Exemption information  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:     Kathy LEE  
-- Modified date: 17 Sep 2009  
-- Description:  1. Remove @Scheme_code in the search route  
--     2. Retrieve Encrypt_Field11  
-- =============================================  
  
--exec proc_VoucherAccountRectificationList_get 'N','','',1,1,0

CREATE PROCEDURE [dbo].[proc_VoucherAccountRectificationList_get]  
 -- Add the parameters for the stored procedure here  
	@strRetrieveSpecialAcc      char(1),  
	@strDocCode	char(20),
	@strIdentityNum    varchar(20),   
	@strAdoptionPrefixNum  char(7) ,
	@strAccountStatus  char(1) ,
	@result_limit_1st_enable BIT, 
	@result_limit_override_enable BIT,
	@override_result_limit BIT ,
	@strRawIdentityNum varchar(20)
AS  
BEGIN  
  
-- =============================================  
-- Declaration  
-- =============================================  
declare @strIdentityNum2 varchar(20)    
declare @strIdentityNum3 varchar(20)
  
declare @tempVR as table  
(  
 Vaildated_Acc_ID char(15),  
 Doc_Code char(10),  
 Last_Fail_Validate_Dtm datetime,  
 TempAcc_Status char(1)  
)  
  
declare @result as table  
(  
 voucher_acc_id char(15),  
 Encrypt_Field1 varbinary(100),  
 Encrypt_Field2 varbinary(200),  
 Encrypt_Field3 varbinary(100),  
 Encrypt_Field11 varbinary(100),  
 dob datetime,  
 exact_dob char(1),  
 Sex char(1),  
 EC_Age smallint,  
 EC_Date_of_Registration datetime,  
 date_of_issue datetime,  
 source char(1),  
 Doc_Code char(20),  
 Doc_Display_Code varchar(20),  
 Other_Info varchar(10),  
 Account_Status char(1),  
 Last_Fail_Validate_Dtm datetime,  
 TempAcc_Status char(1)  
)  

 DECLARE @rowcount int  
 DECLARE @row_cnt_error varchar(max)
 DECLARE @errCode_lower char(5) 
 DECLARE @errCode_upper char(5) 
 SET @errCode_lower = '00009'
 SET @errCode_upper = '00017'
  
-- =============================================  
-- Initialization  
-- =============================================  
 SET NOCOUNT ON;  
   
 set @strIdentityNum2 = ' ' + @strIdentityNum    
 set @strIdentityNum3 = @strRawIdentityNum
  
EXEC [proc_SymmetricKey_open]
    
 --Raiserror('00016', 16, 1)   
-- =============================================  
-- Validated Account (Amendment)  
-- =============================================   
insert into @tempVR  
(  
 Vaildated_Acc_ID,  
 Doc_Code,  
 Last_Fail_Validate_Dtm,  
 TempAcc_Status  
)  
select TVA.Validated_Acc_ID, P.Doc_Code, TVA.Last_Fail_Validate_Dtm, TVA.Record_status  
 from TempVoucherAccount TVA WITH(NOLOCK), TempPersonalInformation TP WITH(NOLOCK), PersonalInformation P  WITH(NOLOCK)
 where   
 P.Voucher_Acc_ID = TVA.Validated_Acc_ID   
 and P.Record_Status = 'U'  
 and TVA.Record_Status in ('I','P')  
 and TVA.Voucher_Acc_ID = TP.Voucher_Acc_ID  
 and TVA.Account_Purpose = 'A'   
 --and (TP.Validating is null or TP.Validating = 'N')  
 and TVA.Create_By_BO = 'Y'  
 and ((	(@strIdentityNum = ''
			OR TP.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @strIdentityNum)
			OR TP.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @strIdentityNum2))
		AND (@strAdoptionPrefixNum = '' 
			OR TP.Encrypt_Field11 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @strAdoptionPrefixNum)))
	OR (@strIdentityNum3 = ''
		OR TP.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @strIdentityNum3)))
 and (@strAccountStatus = '' or TVA.Record_Status = @strAccountStatus)
 and (@strDocCode = '' or TP.Doc_Code = @strDocCode)
 OPTION (RECOMPILE);
   
 insert into @result  
 (  
  voucher_acc_id,  
  Encrypt_Field1,  
  Encrypt_Field2,  
  Encrypt_Field3,  
  Encrypt_Field11,  
  dob,  
  exact_dob,  
  Sex,  
  EC_Age,  
  EC_Date_of_Registration,  
  date_of_issue,  
  source,  
  Doc_Code,  
  Other_Info,  
  Account_Status,  
  Last_Fail_Validate_Dtm,  
  TempAcc_Status  
 )  
 select   TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable)) 
  VA.Voucher_Acc_ID,   
  P.Encrypt_Field1,  
  P.Encrypt_Field2,  
  P.Encrypt_Field3,  
  P.Encrypt_Field11,  
  P.DOB,  
  P.Exact_DOB,  
  P.Sex,  
  p.EC_Age,  
  p.EC_Date_of_Registration,  
  p.date_of_issue,  
  'V',  
  P.Doc_Code,  
  P.Other_Info,  
  VA.Record_status,  
  T.Last_Fail_Validate_Dtm,  
  T.TempAcc_Status  
 from VoucherAccount VA WITH(NOLOCK), PersonalInformation P WITH(NOLOCK), @tempVR T  
 where   
 VA.Voucher_Acc_ID = P.Voucher_Acc_ID   
 and VA.Voucher_Acc_ID = T.Vaildated_Acc_ID  
 and P.Doc_Code = T.Doc_Code  
 and (((@strIdentityNum = ''
			OR P.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @strIdentityNum)
			OR P.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @strIdentityNum2))
		AND (@strAdoptionPrefixNum = ''
			OR P.Encrypt_Field11 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @strAdoptionPrefixNum)))
	OR (@strIdentityNum3 = ''
		OR P.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @strIdentityNum3)))	 
 OPTION (RECOMPILE);
 
	-- =============================================    
	-- Max Row Checking  
	-- =============================================  
	BEGIN TRY       
		SELECT @rowcount = count(voucher_acc_id) FROM @result  
		-- SQL exception is caught by [catch] block if row limit is reached. No result will be selected out
		EXEC proc_CheckFeatureResultRowLimit @row_count=@rowcount, @result_limit_1st_enable=@result_limit_1st_enable, @result_limit_override_enable=@result_limit_override_enable,@override_result_limit=@override_result_limit     
	END TRY

	BEGIN CATCH    	    
		SET @row_cnt_error = ERROR_MESSAGE()    
		
		IF (isnull(@row_cnt_error,'') <> '' AND @row_cnt_error <> @errCode_lower) or
			(@result_limit_override_enable = 0 AND @row_cnt_error = @errCode_lower )
			BEGIN
				--throw error if upper limit reached (error = @errCode_upper)
				--if upper limit is not enabled, throw error if lower limit is reached
				--if the error is not related to upper / lower limit, there must be sth wrong in the try block, throw the error immediately
				RAISERROR (@row_cnt_error,16,1)    
				EXEC [proc_SymmetricKey_close]  
				RETURN
			END
	END CATCH   

    
-- =============================================  
-- Temporary Account (Created by back office)  
-- =============================================  
  
 insert into @result      
 (      
  voucher_acc_id,  
  Encrypt_Field1,  
  Encrypt_Field2,  
  Encrypt_Field3,  
  Encrypt_Field11,  
  dob,  
  exact_dob,  
  Sex,  
  EC_Age,  
  EC_Date_of_Registration,  
  date_of_issue,  
  source,  
  Doc_Code,  
  Other_Info,  
  Account_Status,  
  Last_Fail_Validate_Dtm,  
  TempAcc_Status  
)      
select  TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable))     
   TA.Voucher_acc_id,          
   Encrypt_Field1,      
   Encrypt_Field2,      
   Encrypt_Field3,        
   Encrypt_Field11,           
   TP.DOB,      
   TP.Exact_DOB,      
   TP.Sex,         
   TP.EC_Age,      
   TP.EC_Date_of_Registration,  
   TP.Date_of_Issue,  
   'T' as Source,           
   TP.doc_code,      
   TP.Other_Info,  
   TA.Record_Status,   
   TA.Last_Fail_Validate_Dtm,  
   ''   
 from TempVoucherAccount TA WITH(NOLOCK), TempPersonalInformation TP WITH(NOLOCK), VoucherAccountCreationLOG C WITH(NOLOCK)
 where       
 TA.Voucher_acc_id = TP.Voucher_acc_id       
 and TA.Record_Status in ('P','I','R')      
 and TA.Account_Purpose in ('V', 'C')      
 and TA.Create_By_BO = 'Y'  
 --and P.Validating = 'N'      
 and TA.Voucher_acc_id = C.Voucher_Acc_ID      
 and C.voucher_acc_type = 'T'   
 and (((@strIdentityNum = ''
			OR TP.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @strIdentityNum)
			OR TP.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @strIdentityNum2))
		AND (@strAdoptionPrefixNum = ''
			OR TP.Encrypt_Field11 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @strAdoptionPrefixNum)))
	OR (@strIdentityNum3 = ''
		OR TP.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @strIdentityNum3)))
 and (@strAccountStatus = '' or TA.Record_Status = @strAccountStatus)    
 and (@strDocCode = '' or TP.Doc_Code = @strDocCode)
 OPTION (RECOMPILE)

	-- =============================================    
	-- Max Row Checking  
	-- =============================================  
	BEGIN TRY       
		 SELECT @rowcount = count(voucher_acc_id) FROM @result  
		-- SQL exception is caught by [catch] block if row limit is reached. No result will be selected out
		EXEC proc_CheckFeatureResultRowLimit @row_count=@rowcount, @result_limit_1st_enable=@result_limit_1st_enable, @result_limit_override_enable=@result_limit_override_enable,@override_result_limit=@override_result_limit     
	END TRY

	BEGIN CATCH    	    
		SET @row_cnt_error = ERROR_MESSAGE()    
		
		IF (isnull(@row_cnt_error,'') <> '' AND @row_cnt_error <> @errCode_lower) or
			(@result_limit_override_enable = 0 AND @row_cnt_error = @errCode_lower )
			BEGIN
				--throw error if upper limit reached (error = @errCode_upper)
				--if upper limit is not enabled, throw error if lower limit is reached
				--if the error is not related to upper / lower limit, there must be sth wrong in the try block, throw the error immediately
				RAISERROR (@row_cnt_error,16,1)    
				EXEC [proc_SymmetricKey_close]
				RETURN
			END
	END CATCH   

-- =============================================  
-- Special Account   
-- =============================================  
   
IF @strRetrieveSpecialAcc = 'Y'  
BEGIN   
  insert into @result      
  (      
   voucher_acc_id,  
   Encrypt_Field1,  
   Encrypt_Field2,  
   Encrypt_Field3,  
   Encrypt_Field11,  
   dob,  
   exact_dob,  
   Sex,  
   EC_Age,  
   EC_Date_of_Registration,  
   date_of_issue,  
   source,  
   Doc_Code,  
   Other_Info,  
   Account_Status,  
   Last_Fail_Validate_Dtm,  
   TempAcc_Status  
 )      
 select TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable))     
    VA.Special_acc_id,          
    Encrypt_Field1,      
    Encrypt_Field2,      
    Encrypt_Field3,          
    Encrypt_Field11,          
    P.DOB,      
    P.Exact_DOB,      
    P.Sex,        
    P.EC_Age,      
    P.EC_Date_of_Registration,       
    P.Date_of_Issue,      
    'S' as Source,     
    p.doc_code,    
    P.Other_Info,  
    VA.Record_Status,   
    VA.Last_Fail_Validate_Dtm,  
    ''     
  from SpecialAccount VA WITH(NOLOCK), SpecialPersonalInformation P WITH(NOLOCK), VoucherAccountCreationLOG C WITH(NOLOCK)   
  where       
  VA.Special_acc_id = P.Special_acc_id       
  and VA.Record_Status in ('P','I')      
  and VA.Account_Purpose in ('V', 'C')      
  --and P.Validating = 'N'      
  and VA.Special_acc_id = C.Voucher_Acc_ID      
  and C.voucher_acc_type = 'S'      
  and (((@strIdentityNum = ''
			OR P.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @strIdentityNum)
			OR P.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @strIdentityNum2))
		AND (@strAdoptionPrefixNum = ''
			OR P.Encrypt_Field11 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @strAdoptionPrefixNum)))
	OR (@strIdentityNum3 = ''
		OR P.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @strIdentityNum3)))	  
  and (@strAccountStatus = '' or VA.Record_Status = @strAccountStatus) 
  and (@strDocCode = '' or P.Doc_Code = @strDocCode)
  OPTION (RECOMPILE);
     
	-- =============================================    
	-- Max Row Checking  
	-- =============================================  
	BEGIN TRY       
		  SELECT @rowcount = count(voucher_acc_id) FROM @result   
		-- SQL exception is caught by [catch] block if row limit is reached. No result will be selected out
		EXEC proc_CheckFeatureResultRowLimit @row_count=@rowcount, @result_limit_1st_enable=@result_limit_1st_enable, @result_limit_override_enable=@result_limit_override_enable,@override_result_limit=@override_result_limit     
	END TRY

	BEGIN CATCH    	    
		SET @row_cnt_error = ERROR_MESSAGE()    
		
		IF (isnull(@row_cnt_error,'') <> '' AND @row_cnt_error <> @errCode_lower) or
			(@result_limit_override_enable = 0 AND @row_cnt_error = @errCode_lower )
			BEGIN
				--throw error if upper limit reached (error = @errCode_upper)
				--if upper limit is not enabled, throw error if lower limit is reached
				--if the error is not related to upper / lower limit, there must be sth wrong in the try block, throw the error immediately
				RAISERROR (@row_cnt_error,16,1)    
				EXEC [proc_SymmetricKey_close]
				RETURN
			END
	END CATCH   
    
END  



-- =============================================    
-- Throw out error if lower limit is reached 
-- =============================================  
IF isnull(@row_cnt_error,'') = @errCode_lower 
		BEGIN		
			RAISERROR (@row_cnt_error,16,1)    
			EXEC [proc_SymmetricKey_close]
			RETURN
		END

-- =============================================  
-- Return results  
-- =============================================  

select  voucher_acc_id,  
  convert(varchar, DecryptByKey(Encrypt_Field1)) as IdentityNum,  
  convert(varchar(100), DecryptByKey(Encrypt_Field2)) as Eng_name,  
  isnull(convert(nvarchar, DecryptByKey(Encrypt_Field3)),'') as Chi_Name,  
  isnull(convert(char, DecryptByKey(Encrypt_Field11)),'') as Adoption_Prefix_Num,  
  dob,  
  exact_dob,  
  Sex,  
  EC_Age,  
  EC_Date_of_Registration,  
  date_of_issue,  
  source,  
  r.Doc_Code,  
  DT.Doc_Display_Code,  
  Other_Info,  
  Account_Status,  
  Last_Fail_Validate_Dtm,  
  TempAcc_Status  
 from @result r, DocType DT WITH(NOLOCK)  
 where DT.Doc_code = r.Doc_Code  
 order by convert(varchar, DecryptByKey(Encrypt_Field1))  
   
 EXEC [proc_SymmetricKey_close]  
END  
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccountRectificationList_get] TO HCVU
GO
