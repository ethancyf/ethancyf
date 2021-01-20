IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccountPendingVUSubmit_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccountPendingVUSubmit_get]
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
-- CR No.:		   CRE12-014  
-- Modified by:    Karl LAM   
-- Modified date:  03 Jan 2013  
-- Description:    Add parameters: @result_limit_1st_enable,@result_limit_override_enable, @override_result_limit  for relax 500 rows limitation  
-- =============================================      
-- =============================================   
-- Modification History
-- Modified by:	    Paul Yip
-- Modified date:	20 July 2010
-- Description:		Add Create_By_BO 
-- =============================================
-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 23 October 2008
-- Description:	Retrieve EC TempVoucherAccount which 
--				issue date is before SMARTIC (23 06 2003)
--				pending VU manual processing
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Timothy LEUNG
-- Modified date:	5 November 2008
-- Description:		Retrieve the travel doc DOB 

-- Modified by:	    Clark YIP
-- Modified date:	01 Jan 2009
-- Description:		Add Create_by col
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Kathy LEE
-- Modified date:	14 Sep 2009
-- Description:		Remove Scheme_Code in the search route
-- =============================================


--exec proc_TempVoucherAccountPendingVUSubmit_get null,null,null, 'T',1,0,0 --112

CREATE PROCEDURE [dbo].[proc_TempVoucherAccountPendingVUSubmit_get]
	-- Add the parameters for the stored procedure here	
	@Eng_Name varchar(40),
	@From_Date datetime,
	@To_Date datetime,
	@source char(1),
	@result_limit_1st_enable BIT, 
	@result_limit_override_enable BIT,
	@override_result_limit BIT
AS
BEGIN


-- =============================================
-- Declaration
-- =============================================
DECLARE @rowcount INT
DECLARE @row_cnt_error varchar(max)
	
create table #tempAccount
(
	voucher_acc_id char(15),
	scheme_code char(10),
	Encrypt_Field1 varbinary(100),
	Encrypt_Field2 varbinary(100),
	Encrypt_Field3 varbinary(100),
	Encrypt_Field4 varbinary(50),
	Encrypt_Field5 varbinary(50),
	Encrypt_Field6 varbinary(50),
	Encrypt_Field7 varbinary(50),
	Encrypt_Field8 varbinary(50),
	Encrypt_Field9 varbinary(50),
	Encrypt_Field11 varbinary(50),
	account_purpose char(1),
	dob datetime,
	exact_dob char(1),
	sex char(1),
	date_of_issue datetime,
	account_status char(1),
	personalinformation_status char(1),
	create_dtm datetime,
	ec_age smallint,
	ec_Date_of_registration datetime,
	sp_id char(8),
	sp_practice_display_seq smallint,
	doc_code char(10),
	source char(1),
	other_info char(10),
	create_by varchar(20),
	create_by_bo char(1)
)
CREATE INDEX IX_VAT on #tempAccount (Voucher_Acc_ID, Doc_Code)

-- =============================================
-- Initialization
-- =============================================

	SET NOCOUNT ON;

if @source = 'T'
begin
	--- Temporary Accont
	insert into #tempAccount
	(
		voucher_acc_id,
		scheme_code,
		Encrypt_Field1,
		Encrypt_Field2,
		Encrypt_Field3,
		Encrypt_Field4,
		Encrypt_Field5,
		Encrypt_Field6,
		Encrypt_Field7,
		Encrypt_Field8,
		Encrypt_Field9,
		Encrypt_Field11,
		account_purpose,
		dob,
		exact_dob,
		sex,
		date_of_issue,
		account_status,
		personalinformation_status,
		create_dtm,
		ec_age,
		ec_Date_of_registration,
		sp_id,
		sp_practice_display_seq,
		doc_code,
		source,
		other_info,
		create_by,
		create_by_bo	
	)
	select 
		VA.Voucher_Acc_ID, 
		VA.Scheme_Code, 
		Encrypt_Field1,
		Encrypt_Field2,
		Encrypt_Field3,
		Encrypt_Field4,
		Encrypt_Field5,
		Encrypt_Field6,
		Encrypt_Field7,
		Encrypt_Field8,
		Encrypt_Field9,
		Encrypt_Field11,
		VA.Account_Purpose, 
		P.DOB,
		P.Exact_DOB,
		P.Sex,
		P.Date_of_Issue,
		VA.Record_Status,
		P.Record_Status,
		P.Create_Dtm,
		P.EC_Age,
		P.EC_Date_of_Registration,	
		C.SP_ID,
		C.SP_Practice_Display_Seq,
		p.doc_code,
		'T' as Source,
		P.Other_Info,
		C.Create_By,
		C.Create_By_BO
		from TempVoucherAccount VA, TempPersonalInformation P, VoucherAccountCreationLOG C
		where 
		VA.Voucher_Acc_ID = P.Voucher_Acc_ID 
		and VA.Record_Status = 'P'
		and VA.Account_Purpose in ('V', 'A', 'C')
		and (P.Validating is null or P.Validating <> 'Y')
		and VA.Voucher_Acc_ID = C.Voucher_Acc_ID
		and C.voucher_acc_type = 'T'
		and (@Eng_Name is null or P.Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @Eng_Name))
		and (VA.Create_Dtm >= @From_Date or @From_Date is null)
		and (VA.Create_Dtm < DateAdd(Day,1,@To_Date) or @To_Date is null)
end

else if @source = 'S'
begin	
	--- Special Accont
		insert into #tempAccount
		(
		voucher_acc_id,
		scheme_code,
		Encrypt_Field1,
		Encrypt_Field2,
		Encrypt_Field3,
		Encrypt_Field4,
		Encrypt_Field5,
		Encrypt_Field6,
		Encrypt_Field7,
		Encrypt_Field8,
		Encrypt_Field9,
		Encrypt_Field11,
		account_purpose,
		dob,
		exact_dob,
		sex,
		date_of_issue,
		account_status,
		personalinformation_status,
		create_dtm,
		ec_age,
		ec_Date_of_registration,
		sp_id,
		sp_practice_display_seq,
		doc_code,
		source,
		other_info,
		create_by,
		create_by_bo	
	)
	select 
		VA.Special_acc_id, 
		VA.Scheme_Code, 
		Encrypt_Field1,
		Encrypt_Field2,
		Encrypt_Field3,
		Encrypt_Field4,
		Encrypt_Field5,
		Encrypt_Field6,
		Encrypt_Field7,
		Encrypt_Field8,
		Encrypt_Field9,
		Encrypt_Field11,
		VA.Account_Purpose, 
		P.DOB,
		P.Exact_DOB,
		P.Sex,
		P.Date_of_Issue,
		VA.Record_Status,
		P.Record_Status,
		P.Create_Dtm,
		P.EC_Age,
		P.EC_Date_of_Registration,	
		C.SP_ID,
		C.SP_Practice_Display_Seq,
		p.doc_code,
		'S' as Source,
		P.Other_Info,
		C.Create_By,
		C.Create_By_BO
		from SpecialAccount VA, SpecialPersonalInformation P, VoucherAccountCreationLOG C
		where 
		VA.Special_acc_id = P.Special_acc_id 
		and VA.Record_Status = 'P'
		and VA.Account_Purpose in ('V', 'C')
		and (P.Validating is null or P.Validating <> 'Y')
		and VA.Special_acc_id = C.Voucher_Acc_ID
		and C.voucher_acc_type = 'S'
		and (@Eng_Name is null or P.Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @Eng_Name))
		and (VA.Create_Dtm >= @From_Date or @From_Date is null)
		and (VA.Create_Dtm < DateAdd(Day,1,@To_Date) or @To_Date is null)
end


-- =============================================
-- Return results
-- =============================================
	EXEC [proc_SymmetricKey_open]
		
		
select TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable)) 
	t.voucher_acc_id,
	t.scheme_code,
	convert(varchar, DecryptByKey(t.Encrypt_Field1)) as IdentityNum,
	convert(varchar, DecryptByKey(t.Encrypt_Field2)) as EName,
	convert(nvarchar, DecryptByKey(t.Encrypt_Field3)) as CName,
	convert(varchar, DecryptByKey(t.Encrypt_Field4)) as CCcode1,
	convert(varchar, DecryptByKey(t.Encrypt_Field5)) as CCcode2,
	convert(varchar, DecryptByKey(t.Encrypt_Field6)) as CCcode3,
	convert(varchar, DecryptByKey(t.Encrypt_Field7)) as CCcode4,
	convert(varchar, DecryptByKey(t.Encrypt_Field8)) as CCcode5,
	convert(varchar, DecryptByKey(t.Encrypt_Field9)) as CCcode6,
	isnull(convert(char, DecryptByKey(t.Encrypt_Field11)),'') as Adoption_Prefix_Num,
	t.account_purpose,
	t.dob,
	t.exact_dob,
	t.sex,
	t.date_of_issue,
	t.account_status,
	t.personalinformation_status,
	t.create_dtm,
	t.ec_age,
	t.ec_Date_of_registration,
	t.sp_id,
	t.sp_practice_display_seq,
	--t.sp_id + '('+ convert(varchar, t.SP_Practice_Display_Seq) +')' as Create_By,
	t.doc_code,
	dt.doc_display_code,
	t.source,
	t.other_info,
	t.create_by,
	t.create_by_bo	
INTO #tempResult
from #tempAccount t, doctype dt
where
t.doc_code = dt.doc_code  collate database_default
and (
	 (t.Doc_Code = 'ADOPC' and (t.Exact_DOB in ('T', 'U', 'V') or DT.Force_Manual_Validate = 'Y')) or
	 (t.Doc_Code = 'Doc/I' and (t.Date_of_Issue < '1 Sep 2003' or DT.Force_Manual_Validate = 'Y')) or
	 (t.Doc_Code = 'EC' and (t.Date_of_issue < '23 Jun 2003' or t.Exact_DOB in ('T', 'U', 'V') or DT.Force_Manual_Validate = 'Y') ) or
	 (t.Doc_Code = 'HKBC' and (t.Exact_DOB in ('T', 'U', 'V') or DT.Force_Manual_Validate = 'Y' )) or
	 (t.doc_code = 'HKIC' and DT.Force_Manual_Validate = 'Y') or
	 (t.doc_code = 'ID235B' and DT.Force_Manual_Validate = 'Y') or
	 (t.doc_code = 'REPMT' and (t.Date_of_Issue < '4 Jun 2007' or DT.Force_Manual_Validate = 'Y')) or
	 (t.doc_code = 'VISA' and DT.Force_Manual_Validate = 'Y')
	)
order by convert(varchar, DecryptByKey(t.Encrypt_Field1))


-- =============================================    
-- Max Row Checking  
-- =============================================  
BEGIN TRY       
		SELECT	@rowcount = COUNT(1)
		FROM	#tempResult

	EXEC proc_CheckFeatureResultRowLimit @row_count=@rowcount, @result_limit_1st_enable=@result_limit_1st_enable, @result_limit_override_enable=@result_limit_override_enable,@override_result_limit=@override_result_limit     
END TRY

BEGIN CATCH    	    
	SET @row_cnt_error = ERROR_MESSAGE()    
	EXEC [proc_SymmetricKey_close]
	RAISERROR (@row_cnt_error,16,1)    
	RETURN
END CATCH 


SELECT
	voucher_acc_id,
	scheme_code,
	IdentityNum,
	EName,
	CName,
	CCcode1,
	CCcode2,
	CCcode3,
	CCcode4,
	CCcode5,
	CCcode6,
	Adoption_Prefix_Num,
	account_purpose,
	dob,
	exact_dob,
	sex,
	date_of_issue,
	account_status,
	personalinformation_status,
	create_dtm,
	ec_age,
	ec_Date_of_registration,
	sp_id,
	sp_practice_display_seq,
	doc_code,
	doc_display_code,
	source,
	other_info,
	create_by,
	create_by_bo
From #tempResult
	
EXEC [proc_SymmetricKey_close]

drop table #tempAccount
drop table #tempResult

END
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccountPendingVUSubmit_get] TO HCVU
GO
