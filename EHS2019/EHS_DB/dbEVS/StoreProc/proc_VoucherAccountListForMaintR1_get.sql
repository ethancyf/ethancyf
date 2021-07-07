IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherAccountListForMaintR1_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherAccountListForMaintR1_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- CR No.:			CRE20-023
-- Modified by:		Martin Tang
-- Modified date:	16 June 2021
-- Description:		Extend patient name's maximum length (varbinary 100->200)
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-023
-- Modified by:		Martin Tang
-- Modified date:	20 Apr 2021
-- Description:		Extend patient name's maximum length
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
-- Modified by:		Lawrence TSANG
-- Modified date:	31 October 2014
-- CR No.:			INT14-0027
-- Description:		Fix HCVU eHealth Account Maintenance search amended account
-- =============================================
-- =============================================
-- Author:		Tommy Cheung
-- Create date: 18 August 2008
-- Description:	Retrieve the Voucher Account by using Name, Creation Date and Account Type
--				for Voucher Account Maintenance
-- =============================================
-- =============================================
-- Modification History
    
-- CR No.:		   CRE12-014  
-- Modified by:    Karl LAM   
-- Modified date:  03 Jan 2013  
-- Description:    Add parameters: @result_limit_1st_enable,@result_limit_override_enable, @override_result_limit  for relax 500 rows limitation  

-- Modified by:	    Paul Yip
-- Modified date:	20 July 2010
-- Description:		retrieve Create_By_BO

-- Modified by:	    Paul Yip
-- Modified date:	9 April 2010
-- Description:		not to retrieve invalid account

-- Modified by:	    Timothy LEUNG
-- Modified date:	20 Oct 2008
-- Description:		Add Certification of Exemption information

-- Modified by:	    Clark YIP
-- Modified date:	29 Oct 2008
-- Description:		Add Temp VR Acct get list with status 'D'

-- Modified by:	    Clark YIP
-- Modified date:	01 Jan 2009
-- Description:		Add Create_by col

-- Modified by:	    Kathy LEE
-- Modified date:	11 Sep 2009
-- Description:		Add Acct type 'N' - Invalid Account
--								  'P' - Special Account

-- =============================================

--exec proc_VoucherAccountListForMaintR1_get null,null,null,'T',1,1,0 

CREATE PROCEDURE [dbo].[proc_VoucherAccountListForMaintR1_get]
	-- Add the parameters for the stored procedure here
	--@Scheme_Code char(10),
	@Eng_Name varchar(100),
	@From_Date datetime,
	@To_Date datetime,
	@Acct_Type varchar(2),
	@result_limit_1st_enable BIT, 
	@result_limit_override_enable BIT,
	@override_result_limit BIT
AS
BEGIN

SET NOCOUNT ON;

EXEC [proc_SymmetricKey_open]
-- =============================================
-- Initialization
-- =============================================

	create table #temptable
	(
		Voucher_Acc_ID char(15),
		Scheme_Code char(10), 
		Encrypt_Field1 varbinary(100),
		Encrypt_Field2 varbinary(200),
		Encrypt_Field3 varbinary(100),
		DOB datetime,
		Exact_DOB char(1),
		Sex char(1),
		Create_Dtm datetime,
		Account_Status char(1),
		PersonalInformation_Status char(1),
		SP_ID char(8),
		SP_Practice_Display_Seq smallint,
		EC_Age smallint,
		EC_Date_of_Registration datetime,
		Encrypt_Field11 varbinary(50),
		Create_by varchar(20),
		Doc_Code char(20),
		Source char(1),
		Other_Info varchar(10),
		Create_By_BO char(1)
	)
	
	CREATE INDEX IX_VAT on #temptable (Voucher_Acc_ID)
	
DECLARE @row_cnt_error varchar(max)
DECLARE @rowcount int

-- =============================================
-- Tempprary Voucher Account Search
-- =============================================
If @Acct_Type = 'T'
BEGIN
	SELECT	@rowcount = count(1) FROM
		TempVoucherAccount VA, TempPersonalInformation P, VoucherAccountCreationLOG C
	WHERE
		VA.Voucher_Acc_ID = P.Voucher_Acc_ID collate database_default
		and C.Voucher_Acc_Type = 'T'
		--and VA.Scheme_Code = @Scheme_Code
		and (@Eng_Name is null or P.Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @Eng_Name))
		and (VA.Create_Dtm >= @From_Date or @From_Date is null)
		and (VA.Create_Dtm < DateAdd(Day,1,@To_Date) or @To_Date is null)
		and VA.Record_Status in ('P','I','C','D')
		and VA.Account_Purpose in ('C','V')
		and VA.Voucher_Acc_ID = C.Voucher_Acc_ID collate database_default
		
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

	
	insert into #temptable
	(
		Voucher_Acc_ID,
		Scheme_Code, 
		Encrypt_Field1,
		Encrypt_Field2,
		Encrypt_Field3,
		DOB,
		Exact_DOB,
		Sex,
		Create_Dtm,
		Account_Status,
		PersonalInformation_Status,
		SP_ID,
		SP_Practice_Display_Seq,
		EC_Age,
		EC_Date_of_Registration,
		Encrypt_Field11,
		Create_by,
		Doc_Code,
		Source,
		Other_Info,
		Create_By_BO
	)
	SELECT
		VA.Voucher_Acc_ID,
		VA.Scheme_Code,
		P.Encrypt_Field1,
		P.Encrypt_Field2,
		P.Encrypt_Field3,
		P.DOB,
		P.Exact_DOB,
		P.Sex,
		VA.Create_Dtm,
		VA.Record_Status,
		P.Record_Status,
		C.SP_ID,
		C.SP_Practice_Display_Seq,
		P.EC_Age,
		P.EC_Date_of_Registration,
		P.Encrypt_Field11,
		--c.sp_id + '('+ convert(varchar,C.SP_Practice_Display_Seq) +')',
		C.Create_By,
		p.doc_code,
		'T',
		P.other_info,
		C.Create_By_BO
	FROM
		TempVoucherAccount VA, TempPersonalInformation P, VoucherAccountCreationLOG C
	WHERE
		VA.Voucher_Acc_ID = P.Voucher_Acc_ID collate database_default
		and C.Voucher_Acc_Type = 'T'
		--and VA.Scheme_Code = @Scheme_Code
		and (@Eng_Name is null or P.Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @Eng_Name))
		and (VA.Create_Dtm >= @From_Date or @From_Date is null)
		and (VA.Create_Dtm < DateAdd(Day,1,@To_Date) or @To_Date is null)
		and VA.Record_Status in ('P','I','C','D')
		and VA.Account_Purpose in ('C','V')
		and VA.Voucher_Acc_ID = C.Voucher_Acc_ID collate database_default
		
	select t.Voucher_Acc_ID,
		t.Scheme_Code, 
		convert(varchar, DecryptByKey(t.Encrypt_Field1)) as IdentityNum,
		convert(varchar(100), DecryptByKey(t.Encrypt_Field2)) as EName,
		isNull(convert(nvarchar, DecryptByKey(t.Encrypt_Field3)),'') as CName,
		t.DOB,
		t.Exact_DOB,
		t.Sex,
		t.Create_Dtm,
		t.Account_Status,
		t.PersonalInformation_Status,
		t.SP_ID,
		t.SP_Practice_Display_Seq,
		t.EC_Age,
		t.EC_Date_of_Registration,
		isnull(convert(char, DecryptByKey(t.Encrypt_Field11)),'') as Adoption_Prefix_Num,
		t.Create_by,
		t.Doc_Code,
		dt.Doc_Display_Code,
		t.Source,
		t.Other_Info,
		t.Create_By_BO
	from #temptable t, doctype dt
	where t.doc_code = dt.doc_code collate database_default
	Order by convert(varchar, DecryptByKey(t.Encrypt_Field1)) ASC


END
-- =============================================
-- Active Validated Account / Suspended Account / Terminated Account Search
-- =============================================
ELSE IF @Acct_Type = 'A' or @Acct_Type = 'S' or @Acct_Type = 'D'
BEGIN
	-- Check the no of rows of the result
	SELECT	@rowcount = count(1)
	FROM
		PersonalInformation P
		inner join VoucherAccount VA
			on P.Voucher_Acc_id = VA.voucher_acc_id	collate database_default
		inner join VoucherAccountCreationLOG C
			on VA.Voucher_Acc_ID = C.Voucher_Acc_ID collate database_default and C.Voucher_Acc_Type = 'V'
	WHERE
		--VA.Voucher_Acc_ID = P.Voucher_Acc_ID
		--and VA.Scheme_Code = @Scheme_Code
		(@Eng_Name is null or P.Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @Eng_Name))
		and (VA.Create_Dtm >= @From_Date or @From_Date is null)
		and (VA.Create_Dtm < DateAdd(Day,1,@To_Date) or @To_Date is null)
		and VA.Record_Status = @Acct_Type
		--and VA.Voucher_Acc_ID = C.Voucher_Acc_ID;

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

	insert into #temptable
	(
		Voucher_Acc_ID,
		Scheme_Code, 
		Encrypt_Field1,
		Encrypt_Field2,
		Encrypt_Field3,
		DOB,
		Exact_DOB,
		Sex,
		Create_Dtm,
		Account_Status,
		PersonalInformation_Status,
		SP_ID,
		SP_Practice_Display_Seq,
		EC_Age,
		EC_Date_of_Registration,
		Encrypt_Field11,
		Create_by,
		Doc_Code,		
		Source,
		Other_Info,
		Create_By_BO
	)
	SELECT
		VA.Voucher_Acc_ID as Voucher_Acc_ID,
		VA.Scheme_Code as Scheme_Code,
		P.Encrypt_Field1,
		P.Encrypt_Field2,
		P.Encrypt_Field3,
		P.DOB as DOB,
		P.Exact_DOB as Exact_DOB,
		P.Sex as Sex,
		VA.Create_Dtm as Create_Dtm,
		VA.Record_Status as Account_Status,
		P.Record_Status as PersonalInformation_Status,
		C.SP_ID as SP_ID,
		C.SP_Practice_Display_Seq as SP_Practice_Display_Seq,
		P.EC_Age as EC_Age,
		P.EC_Date_of_Registration as EC_Date_of_Registration,
		P.Encrypt_Field11,
		--c.sp_id + '('+ convert(varchar,C.SP_Practice_Display_Seq) +')' as Create_By,
		C.Create_By,
		p.doc_code,
		'V' as Source,
		P.other_info,
		C.Create_By_BO
	FROM
		PersonalInformation P
		inner join VoucherAccount VA
			on P.Voucher_Acc_id = VA.voucher_acc_id	collate database_default
		inner join VoucherAccountCreationLOG C 
			on VA.Voucher_Acc_ID = C.Voucher_Acc_ID collate database_default and C.Voucher_Acc_Type = 'V'		
	WHERE
		(@Eng_Name is null or P.Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @Eng_Name))
		and (VA.Create_Dtm >= @From_Date or @From_Date is null)
		and (VA.Create_Dtm < DateAdd(Day,1,@To_Date) or @To_Date is null)
		and VA.Record_Status = @Acct_Type
	
		
	select t.Voucher_Acc_ID,
		t.Scheme_Code, 
		convert(varchar, DecryptByKey(t.Encrypt_Field1)) as IdentityNum,
		convert(varchar(100), DecryptByKey(t.Encrypt_Field2)) as EName,
		isNull(convert(nvarchar, DecryptByKey(t.Encrypt_Field3)),'') as CName,
		t.DOB,
		t.Exact_DOB,
		t.Sex,
		t.Create_Dtm,
		t.Account_Status,
		t.PersonalInformation_Status,
		t.SP_ID,
		t.SP_Practice_Display_Seq,
		t.EC_Age,
		t.EC_Date_of_Registration,
		isnull(convert(char, DecryptByKey(t.Encrypt_Field11)),'') as Adoption_Prefix_Num,
		t.Create_by,
		t.Doc_Code,
		dt.Doc_Display_Code,
		t.Source,
		t.Other_Info,
		t.Create_By_BO
	from #temptable t, doctype dt
	where t.doc_code = dt.doc_code collate database_default
	Order by convert(varchar, DecryptByKey(t.Encrypt_Field1)) ASC

END
-- =============================================
-- Erased Account Search
-- =============================================
ELSE IF @Acct_Type = 'E'
BEGIN

	-- Check the no of rows of the result
	SELECT	@rowcount = count(1)
	FROM
		TempVoucherAccount VA, TempPersonalInformation P, VoucherAccountCreationLOG C
	WHERE
		VA.Voucher_Acc_ID = P.Voucher_Acc_ID collate database_default
		and C.Voucher_Acc_Type = 'T' 
		--and VA.Scheme_Code = @Scheme_Code
		and (@Eng_Name is null or P.Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @Eng_Name))
		and (VA.Create_Dtm >= @From_Date or @From_Date is null)
		and (VA.Create_Dtm < DateAdd(Day,1,@To_Date) or @To_Date is null)
		and VA.Record_Status = 'I'
		and VA.Account_Purpose = 'O'
		and VA.Voucher_Acc_ID = C.Voucher_Acc_ID collate database_default;	

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

	insert into #temptable
	(
		Voucher_Acc_ID,
		Scheme_Code, 
		Encrypt_Field1,
		Encrypt_Field2,
		Encrypt_Field3,
		DOB,
		Exact_DOB,
		Sex,
		Create_Dtm,
		Account_Status,
		PersonalInformation_Status,
		SP_ID,
		SP_Practice_Display_Seq,
		EC_Age,
		EC_Date_of_Registration,
		Encrypt_Field11,
		Create_by,
		Doc_Code,
		Source,
		Other_Info,
		Create_By_BO
	)
	SELECT
		VA.Voucher_Acc_ID as Voucher_Acc_ID,
		VA.Scheme_Code as Scheme_Code,
		P.Encrypt_Field1,
		P.Encrypt_Field2,
		P.Encrypt_Field3,
		P.DOB as DOB,
		P.Exact_DOB as Exact_DOB,
		P.Sex as Sex,
		VA.Create_Dtm as Create_Dtm,
		VA.Record_Status as Account_Status,
		P.Record_Status as PersonalInformation_Status,
		C.SP_ID as SP_ID,
		C.SP_Practice_Display_Seq as SP_Practice_Display_Seq,
		P.EC_Age as EC_Age,
		P.EC_Date_of_Registration as EC_Date_of_Registration,
		P.Encrypt_Field11,
		--c.sp_id + '('+ convert(varchar,C.SP_Practice_Display_Seq) +')' as Create_By,
		C.Create_By,
		p.doc_code,
		'T' as Source,
		P.other_info,
		C.Create_By_BO
	FROM
		TempVoucherAccount VA, TempPersonalInformation P, VoucherAccountCreationLOG C
	WHERE
		VA.Voucher_Acc_ID = P.Voucher_Acc_ID collate database_default
		and C.Voucher_Acc_Type = 'T'
		--and VA.Scheme_Code = @Scheme_Code
		and (@Eng_Name is null or P.Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @Eng_Name))
		and (VA.Create_Dtm >= @From_Date or @From_Date is null)
		and (VA.Create_Dtm < DateAdd(Day,1,@To_Date) or @To_Date is null)
		and VA.Record_Status = 'I'
		and VA.Account_Purpose = 'O'
		and VA.Voucher_Acc_ID = C.Voucher_Acc_ID collate database_default

	select t.Voucher_Acc_ID,
		t.Scheme_Code, 
		convert(varchar, DecryptByKey(t.Encrypt_Field1)) as IdentityNum,
		convert(varchar(100), DecryptByKey(t.Encrypt_Field2)) as EName,
		isNull(convert(nvarchar, DecryptByKey(t.Encrypt_Field3)),'') as CName,
		t.DOB,
		t.Exact_DOB,
		t.Sex,
		t.Create_Dtm,
		t.Account_Status,
		t.PersonalInformation_Status,
		t.SP_ID,
		t.SP_Practice_Display_Seq,
		t.EC_Age,
		t.EC_Date_of_Registration,
		isnull(convert(char, DecryptByKey(t.Encrypt_Field11)),'') as Adoption_Prefix_Num,
		t.Create_by,
		t.Doc_Code,
		dt.Doc_Display_Code,
		t.Source,
		t.Other_Info,
		t.Create_By_BO
	from #temptable t, doctype dt
	where t.doc_code = dt.doc_code collate database_default
	Order by convert(varchar, DecryptByKey(t.Encrypt_Field1)) ASC
	
END
-- =============================================
-- Amended Account Search
-- =============================================
ELSE IF @Acct_Type = 'U'
BEGIN
	-- Check the no of rows of the result
	SELECT	@rowcount = count(DISTINCT VA.Voucher_Acc_ID)
	FROM
		PersonalInformation P
		inner join VoucherAccount VA 
			on P.Voucher_Acc_id = VA.voucher_acc_id	 collate database_default
		inner join VoucherAccountCreationLOG C
			on VA.Voucher_Acc_ID = C.Voucher_Acc_ID collate database_default and C.Voucher_Acc_Type = 'V'
	WHERE
		(@Eng_Name is null or P.Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @Eng_Name))
		and (VA.Create_Dtm >= @From_Date or @From_Date is null)
		and (VA.Create_Dtm < DateAdd(Day,1,@To_Date) or @To_Date is null)		
		AND EXISTS (
			SELECT
				1
			FROM
				PersonalInfoAmendHistory
			WHERE
				Voucher_Acc_ID = P.Voucher_Acc_ID
					AND Doc_Code = P.Doc_Code
					AND Record_Status = 'A'
					AND Action_Type = 'A'
		)
	
		
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
	
	insert into #temptable
	(
		Voucher_Acc_ID,
		Scheme_Code, 
		Encrypt_Field1,
		Encrypt_Field2,
		Encrypt_Field3,
		DOB,
		Exact_DOB,
		Sex,
		Create_Dtm,
		Account_Status,
		PersonalInformation_Status,
		SP_ID,
		SP_Practice_Display_Seq,
		EC_Age,
		EC_Date_of_Registration,
		Encrypt_Field11,
		Create_by,
		Doc_Code,
		Source,
		Other_Info,
		Create_By_BO
	)
	SELECT --DISTINCT
		VA.Voucher_Acc_ID as Voucher_Acc_ID,
		VA.Scheme_Code as Scheme_Code,
		P.Encrypt_Field1,
		P.Encrypt_Field2,
		P.Encrypt_Field3,
		P.DOB as DOB,
		P.Exact_DOB as Exact_DOB,
		P.Sex as Sex,
		VA.Create_Dtm as Create_Dtm,
		VA.Record_Status as Account_Status,
		P.Record_Status as PersonalInformation_Status,
		C.SP_ID as SP_ID,
		C.SP_Practice_Display_Seq as SP_Practice_Display_Seq,
		P.EC_Age as EC_Age,
		P.EC_Date_of_Registration as EC_Date_of_Registration,
		P.Encrypt_Field11,
		--c.sp_id + '('+ convert(varchar,C.SP_Practice_Display_Seq) +')' as Create_By,
		C.Create_By,
		p.doc_code,
		'V' as Source,
		P.other_info,
		C.Create_By_BO
	FROM
		PersonalInformation P
		inner join VoucherAccount VA
			on P.Voucher_Acc_id = VA.voucher_acc_id	 collate database_default
		inner join VoucherAccountCreationLOG C
			on VA.Voucher_Acc_ID = C.Voucher_Acc_ID collate database_default and C.Voucher_Acc_Type = 'V'		
	WHERE
		--VA.Voucher_Acc_ID = P.Voucher_Acc_ID
		--and VA.Scheme_Code = @Scheme_Code
		 (@Eng_Name is null or P.Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @Eng_Name))
		and (VA.Create_Dtm >= @From_Date or @From_Date is null)
		and (VA.Create_Dtm < DateAdd(Day,1,@To_Date) or @To_Date is null)
		--and VA.Voucher_Acc_ID = AH.Voucher_Acc_ID
		AND EXISTS (
			SELECT
				1
			FROM
				PersonalInfoAmendHistory
			WHERE
				Voucher_Acc_ID = P.Voucher_Acc_ID
					AND Doc_Code = P.Doc_Code
					AND Record_Status = 'A'
					AND Action_Type = 'A'
		)
		--and VA.Voucher_Acc_ID = C.Voucher_Acc_ID
		--and p.doc_code = dt.doc_code

	select t.Voucher_Acc_ID,
		t.Scheme_Code, 
		convert(varchar, DecryptByKey(t.Encrypt_Field1)) as IdentityNum,
		convert(varchar(100), DecryptByKey(t.Encrypt_Field2)) as EName,
		isNull(convert(nvarchar, DecryptByKey(t.Encrypt_Field3)),'') as CName,
		t.DOB,
		t.Exact_DOB,
		t.Sex,
		t.Create_Dtm,
		t.Account_Status,
		t.PersonalInformation_Status,
		t.SP_ID,
		t.SP_Practice_Display_Seq,
		t.EC_Age,
		t.EC_Date_of_Registration,
		isnull(convert(char, DecryptByKey(t.Encrypt_Field11)),'') as Adoption_Prefix_Num,
		t.Create_by,
		t.Doc_Code,
		dt.Doc_Display_Code,
		t.Source,
		t.Other_Info,
		t.Create_By_BO
	from #temptable t, doctype dt
	where t.doc_code = dt.doc_code collate database_default
	Order by convert(varchar, DecryptByKey(t.Encrypt_Field1)) ASC
	
END

-- =============================================
-- Special Account Search
-- =============================================
ELSE IF @Acct_Type = 'P'
BEGIN
	-- Check the no of rows of the result
	SELECT	@rowcount = count(1)
	FROM
		SpecialAccount SA, SpecialPersonalInformation P, VoucherAccountCreationLOG C
	WHERE
		SA.Special_Acc_ID = P.Special_Acc_ID collate database_default
		and C.Voucher_Acc_Type = 'S'
		--and SA.Scheme_Code = @Scheme_Code
		and (@Eng_Name is null or P.Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @Eng_Name))
		and (SA.Create_Dtm >= @From_Date or @From_Date is null)
		and (SA.Create_Dtm < DateAdd(Day,1,@To_Date) or @To_Date is null)
		--and SA.Record_Status = @Acct_Type
		and SA.Special_Acc_ID = C.Voucher_Acc_ID collate database_default;

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

	insert into #temptable
	(
		Voucher_Acc_ID,
		Scheme_Code, 
		Encrypt_Field1,
		Encrypt_Field2,
		Encrypt_Field3,
		DOB,
		Exact_DOB,
		Sex,
		Create_Dtm,
		Account_Status,
		PersonalInformation_Status,
		SP_ID,
		SP_Practice_Display_Seq,
		EC_Age,
		EC_Date_of_Registration,
		Encrypt_Field11,
		Create_by,
		Doc_Code,
		Source,
		Other_Info,
		Create_By_BO
	)
	SELECT
		SA.Special_Acc_ID as Voucher_Acc_ID,
		SA.Scheme_Code as Scheme_Code,
		P.Encrypt_Field1,
		P.Encrypt_Field2,
		P.Encrypt_Field3,
		P.DOB as DOB,
		P.Exact_DOB as Exact_DOB,
		P.Sex as Sex,
		SA.Create_Dtm as Create_Dtm,
		SA.Record_Status as Account_Status,
		P.Record_Status as PersonalInformation_Status,
		C.SP_ID as SP_ID,
		C.SP_Practice_Display_Seq as SP_Practice_Display_Seq,
		P.EC_Age as EC_Age,
		P.EC_Date_of_Registration as EC_Date_of_Registration,
		P.Encrypt_Field11,
		--c.sp_id + '('+ convert(varchar,C.SP_Practice_Display_Seq) +')' as Create_By,
		C.Create_By,
		p.doc_code,
		'S' as Source,
		P.other_info,
		C.Create_By_BO
	FROM
		SpecialAccount SA, SpecialPersonalInformation P, VoucherAccountCreationLOG C
	WHERE
		SA.Special_Acc_ID = P.Special_Acc_ID collate database_default
		and C.Voucher_Acc_Type = 'S'
		--and SA.Scheme_Code = @Scheme_Code
		and (@Eng_Name is null or P.Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @Eng_Name))
		and (SA.Create_Dtm >= @From_Date or @From_Date is null)
		and (SA.Create_Dtm < DateAdd(Day,1,@To_Date) or @To_Date is null)
		--and SA.Record_Status = @Acct_Type
		and SA.Special_Acc_ID = C.Voucher_Acc_ID collate database_default
		

	select t.Voucher_Acc_ID,
		t.Scheme_Code, 
		convert(varchar, DecryptByKey(t.Encrypt_Field1)) as IdentityNum,
		convert(varchar(100), DecryptByKey(t.Encrypt_Field2)) as EName,
		isNull(convert(nvarchar, DecryptByKey(t.Encrypt_Field3)),'') as CName,
		t.DOB,
		t.Exact_DOB,
		t.Sex,
		t.Create_Dtm,
		t.Account_Status,
		t.PersonalInformation_Status,
		t.SP_ID,
		t.SP_Practice_Display_Seq,
		t.EC_Age,
		t.EC_Date_of_Registration,
		isnull(convert(char, DecryptByKey(t.Encrypt_Field11)),'') as Adoption_Prefix_Num,
		t.Create_by,
		t.Doc_Code,
		dt.Doc_Display_Code,
		t.Source,
		t.Other_Info,
		t.Create_By_BO
	from #temptable t, doctype dt
	where t.doc_code = dt.doc_code collate database_default
	Order by convert(varchar, DecryptByKey(t.Encrypt_Field1)) ASC
	
	
	
END

---- =============================================
---- Invalid Account Search
---- =============================================
--ELSE IF @Acct_Type = 'N'
--BEGIN
--	-- Check the no of rows of the result
--	SELECT	@rowcount = count(1)
--	FROM
--		InvalidAccount IA, InvalidPersonalInformation P, VoucherAccountCreationLOG C
--	WHERE
--		IA.Invalid_Acc_ID = P.Invalid_Acc_ID collate database_default
--		and C.Voucher_Acc_Type = 'I'
--		--and IA.Scheme_Code = @Scheme_Code
--		and (@Eng_Name is null or P.Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @Eng_Name))
--		and (IA.Create_Dtm >= @From_Date or @From_Date is null)
--		and (IA.Create_Dtm < DateAdd(Day,1,@To_Date) or @To_Date is null)
--		and IA.Record_Status = @Acct_Type
--		and IA.invalid_Acc_ID = C.Voucher_Acc_ID collate database_default;
--
--	IF @rowcount > @maxrow
--	BEGIN
--		Raiserror('00009', 16, 1)
--		return @@error
--	END
--
--	insert into #temptable
--	(
--		Voucher_Acc_ID,
--		Scheme_Code, 
--		Encrypt_Field1,
--		Encrypt_Field2,
--		Encrypt_Field3,
--		DOB,
--		Exact_DOB,
--		Sex,
--		Create_Dtm,
--		Account_Status,
--		PersonalInformation_Status,
--		SP_ID,
--		SP_Practice_Display_Seq,
--		EC_Age,
--		EC_Date_of_Registration,
--		Encrypt_Field11,
--		Create_by,
--		Doc_Code,
--		Source,
--		Other_Info
--	)
--	SELECT
--		IA.Invalid_Acc_ID as Voucher_Acc_ID,
--		IA.Scheme_Code as Scheme_Code,
--		P.Encrypt_Field1,
--		P.Encrypt_Field2,
--		P.Encrypt_Field3,
--		P.DOB as DOB,
--		P.Exact_DOB as Exact_DOB,
--		P.Sex as Sex,
--		IA.Create_Dtm as Create_Dtm,
--		IA.Record_Status as Account_Status,
--		P.Record_Status as PersonalInformation_Status,
--		C.SP_ID as SP_ID,
--		C.SP_Practice_Display_Seq as SP_Practice_Display_Seq,
--		P.EC_Age as EC_Age,
--		P.EC_Date_of_Registration as EC_Date_of_Registration,
--		P.Encrypt_Field11,
--		c.sp_id + '('+ convert(varchar,C.SP_Practice_Display_Seq) +')' as Create_By,
--		p.doc_code,
--		'I' as Source,
--		P.other_info
--	FROM
--		InvalidAccount IA, InvalidPersonalInformation P, VoucherAccountCreationLOG C
--	WHERE
--		IA.Invalid_Acc_ID = P.Invalid_Acc_ID collate database_default
--		and C.Voucher_Acc_Type = 'I'
--		--and IA.Scheme_Code = @Scheme_Code
--		and (@Eng_Name is null or P.Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @Eng_Name))
--		and (IA.Create_Dtm >= @From_Date or @From_Date is null)
--		and (IA.Create_Dtm < DateAdd(Day,1,@To_Date) or @To_Date is null)
--		--and SA.Record_Status = @Acct_Type
--		and IA.Invalid_Acc_ID = C.Voucher_Acc_ID collate database_default
--
--	select t.Voucher_Acc_ID,
--		t.Scheme_Code, 
--		convert(varchar, DecryptByKey(t.Encrypt_Field1)) as IdentityNum,
--		convert(varchar(40), DecryptByKey(t.Encrypt_Field2)) as EName,
--		isNull(convert(nvarchar, DecryptByKey(t.Encrypt_Field3)),'') as CName,
--		t.DOB,
--		t.Exact_DOB,
--		t.Sex,
--		t.Create_Dtm,
--		t.Account_Status,
--		t.PersonalInformation_Status,
--		t.SP_ID,
--		t.SP_Practice_Display_Seq,
--		t.EC_Age,
--		t.EC_Date_of_Registration,
--		isnull(convert(char, DecryptByKey(t.Encrypt_Field11)),'') as Adoption_Prefix_Num,
--		t.Create_by,
--		t.Doc_Code,
--		dt.Doc_Display_Code,
--		t.Source,
--		t.Other_Info
--	from #temptable t, doctype dt
--	where t.doc_code = dt.doc_code collate database_default
--	Order by convert(varchar, DecryptByKey(t.Encrypt_Field1)) ASC
--	
--END

	EXEC [proc_SymmetricKey_close]
END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccountListForMaintR1_get] TO HCVU
GO
