IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherAccountListForMaintR2_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherAccountListForMaintR2_get]
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

-- Modified by:    Karl LAM   
-- Modified date:  27 May 2013
-- Description:    Check Doc_Code in every temp table insert instead of using in the where clause while selecting final result set

-- CR No.:		   CRE12-014  
-- Modified by:    Karl LAM   
-- Modified date:  03 Jan 2013  
-- Description:    Add parameters: @result_limit_1st_enable,@result_limit_override_enable, @override_result_limit  for relax 500 rows limitation  

-- Modified by:	    Paul Yip
-- Modified date:	20 July 2010
-- Description:		retrieve Create_By_BO 

-- Modified by:	    Paul Yip
-- Modified date:	9 April 2010
-- Description:		not to retrieve invalid account.

-- Modified by:	    Vincent YUEN 
-- Modified date:	24 Feb 2010
-- Description:		Special and Invalid Account Search by Reference No.

-- Modified by:	    Kathy LEE
-- Modified date:	12 Sep 2009
-- Description:		1. Add Adoption_Prefix_Num in the search route

-- Modified by:	    Clark YIP
-- Modified date:	01 Jan 2009
-- Description:		Add Create_by col

-- Modified by:		Timothy LEUNG
-- Modified date:	28 Oct 2008
-- Description:		Add Reference No for search criteria

-- Modified by:	    Clark YIP
-- Modified date:	28 Oct 2008
-- Description:		Add Temp VR Acct get list with status 'D'

-- Modified by:	    Timothy LEUNG
-- Modified date:	20 Oct 2008
-- Description:		Add Certification of Exemption information
-- =============================================
-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 1 June 2008
-- Description:	Retrieve the Voucher Account by using HKID, Name and DOB
--				for Voucher Account Maintenance
-- =============================================

--exec proc_VoucherAccountListForMaintR2_get '','','','','','','',1,0,0

CREATE PROCEDURE [dbo].[proc_VoucherAccountListForMaintR2_get]
	-- Add the parameters for the stored procedure here
	@Doc_Code char(20),
	@IdentityNum varchar(20),
	@Adoption_Prefix_Num char(7),
	@Eng_Name varchar(40),
	@DOB datetime,	
	@Voucher_Acc_ID char(15),
	@ReferenceNo char(15),
	@result_limit_1st_enable BIT, 
	@result_limit_override_enable BIT,
	@override_result_limit BIT
AS
BEGIN

-- =============================================
-- Declaration
-- =============================================
	 DECLARE @rowcount int  
	 DECLARE @row_cnt_error varchar(max)
	 DECLARE @errCode_lower char(5) 
	 DECLARE @errCode_upper char(5) 
	 SET @errCode_lower = '00009'
	 SET @errCode_upper = '00017'
	
	declare @IdentityNum2 varchar(20)
	
-- =============================================
-- Initialization
-- =============================================

	set @IdentityNum2 = ' ' + @IdentityNum

	SET NOCOUNT ON;

	EXEC [proc_SymmetricKey_open]

	create table #temptable
	(
	Voucher_Acc_ID char(15),
	Scheme_Code char(10), 
	Remark nvarchar(255),
	Public_Enquiry_Status char(1),
	Public_Enq_Status_Remark nvarchar(255),
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
	DOB datetime,
	Exact_DOB char(1),
	Sex char(1),
	Date_of_Issue datetime,
	Account_Status char(1),
	PersonalInformation_Status char(1),		
	Source char(1),
	Create_Dtm datetime,
	SP_ID char(8),
	SP_Practice_Display_Seq smallint,
	EC_Age smallint,
	EC_Date_of_Registration datetime,
	Create_by varchar(20),
	Doc_Code char(20),
	Account_Purpose char(1),
	Other_Info varchar(10),
	Create_By_BO char(1)
	)
	
	CREATE INDEX IX_VAT on #temptable (Voucher_Acc_ID, Doc_Code)

--- Validate
	insert into #temptable
	(
	Voucher_Acc_ID,
	Scheme_Code, 
	Remark,
	Public_Enquiry_Status,
	Public_Enq_Status_Remark,
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
	DOB,
	Exact_DOB,
	Sex,
	Date_of_Issue,
	Account_Status,
	PersonalInformation_Status,
	Source,
	Create_Dtm,
	SP_ID,
	SP_Practice_Display_Seq,
	EC_Age,
	EC_Date_of_Registration,
	Create_by,
	Doc_Code,
	Account_Purpose,
	Other_Info,
	Create_By_BO
	)
	select TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable))
	VA.Voucher_Acc_ID, 
	VA.Scheme_Code, 
	VA.Remark,
	VA.Public_Enquiry_Status,
	VA.Public_Enq_Status_Remark,
	P.Encrypt_Field1,
	P.Encrypt_Field2,
	P.Encrypt_Field3,
	P.Encrypt_Field4,
	P.Encrypt_Field5,
	P.Encrypt_Field6,
	P.Encrypt_Field7,
	P.Encrypt_Field8,
	P.Encrypt_Field9,
	P.Encrypt_Field11,
	P.DOB,
	P.Exact_DOB,
	P.Sex,
	P.Date_of_Issue,
	VA.Record_Status,
	P.Record_status,
	'V',
	VA.Create_Dtm,
	C.SP_ID,
	C.SP_Practice_Display_Seq,
	P.EC_Age,
	P.EC_Date_of_Registration,
	--c.sp_id + '('+ convert(varchar,C.SP_Practice_Display_Seq) +')' as Create_By,
	C.Create_By,
	P.Doc_Code,
	'',
	P.Other_Info,
	C.Create_By_BO
	FROM
		PersonalInformation P
		inner join VoucherAccount VA
			on P.Voucher_Acc_id = VA.voucher_acc_id	
		inner join VoucherAccountCreationLOG C
			on VA.Voucher_Acc_ID = C.Voucher_Acc_ID and C.Voucher_Acc_Type = 'V'

	where 
	(@IdentityNum = '' or P.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @IdentityNum)
		or P.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @IdentityNum2))
	and (@Adoption_Prefix_Num = '' or P.Encrypt_Field11 = EncryptByKey(KEY_GUID('sym_Key'), @Adoption_Prefix_Num))
	and (@Eng_Name = '' or P.Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @Eng_Name))
	and (@DOB is NULL or P.DOB = @DOB)
	and (@Voucher_Acc_ID = '' or VA.Voucher_Acc_ID=@Voucher_Acc_ID)
	and (@ReferenceNo = '')
	and (@Doc_Code = '' or P.Doc_Code = @Doc_Code)


	-- =============================================    
	-- Max Row Checking  
	-- =============================================  
	BEGIN TRY       
		SELECT	@rowcount = count(Voucher_Acc_ID)
		FROM #temptable

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

--- Temporary
	insert into #temptable
	(
	Voucher_Acc_ID,
	Scheme_Code, 
	Remark,
	Public_Enquiry_Status,
	Public_Enq_Status_Remark,
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
	DOB,
	Exact_DOB,
	Sex,
	Date_of_Issue,
	Account_Status,
	PersonalInformation_Status,
	Source,
	Create_Dtm,
	SP_ID,
	SP_Practice_Display_Seq,
	EC_Age,
	EC_Date_of_Registration,
	Create_By,
	Doc_Code,
	Account_Purpose,
	Other_Info,
	Create_By_BO
	)
	select  TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable))
	TVA.Voucher_Acc_ID, 
	TVA.Scheme_Code,
	'',
	'',
	'',
	TP.Encrypt_Field1,
	TP.Encrypt_Field2,
	TP.Encrypt_Field3,
	TP.Encrypt_Field4,
	TP.Encrypt_Field5,
	TP.Encrypt_Field6,
	TP.Encrypt_Field7,
	TP.Encrypt_Field8,
	TP.Encrypt_Field9,
	TP.Encrypt_Field11,
	TP.DOB,
	TP.Exact_DOB,
	TP.Sex,
	TP.Date_of_Issue,
	TVA.Record_Status,
	TP.Record_Status,
	'T',
	TVA.Create_Dtm,
	C.SP_ID,
	C.SP_Practice_Display_Seq,
	TP.EC_Age,
	TP.EC_Date_of_Registration,
	--c.sp_id + '('+ convert(varchar,C.SP_Practice_Display_Seq) +')' as Create_By,
	C.Create_By,
	TP.Doc_Code,
	TVA.Account_Purpose,
	TP.Other_Info,
	C.Create_By_BO
	from TempVoucherAccount TVA, TempPersonalInformation TP, VoucherAccountCreationLOG C--, DocType dt
	where 
	TVA.Voucher_Acc_ID = TP.Voucher_Acc_ID 
	and C.Voucher_Acc_Type = 'T'
	and 
	(
	(TVA.Account_Purpose in ('V','C') and TVA.Record_Status in ('P', 'C', 'A', 'V', 'I', 'D')) 
	or 
	(TVA.Account_Purpose = 'O' and TVA.Record_Status = 'I')
	or
	(TVA.Account_Purpose = 'A' and TVA.Record_Status <> 'D' --and (TP.Exact_DOB in ('T', 'U', 'V') or TP.EC_Date < '23 Jun 2003')
	)
	)
	and (@IdentityNum = '' or TP.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @IdentityNum)
		or TP.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @IdentityNum2))
	and (@Adoption_Prefix_Num = '' or TP.Encrypt_Field11 = EncryptByKey(KEY_GUID('sym_Key'), @Adoption_Prefix_Num))
	and (@Eng_Name = '' or TP.Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @Eng_Name))
	and (@DOB is NULL or TP.DOB = @DOB)
	and (@Voucher_Acc_ID = '')
	and (@ReferenceNo = '' or TVA.Voucher_Acc_ID=@ReferenceNo)
	and (TVA.Voucher_Acc_ID = C.Voucher_Acc_ID)
	and (@Doc_Code = '' or TP.Doc_Code = @Doc_Code)



	-- =============================================    
	-- Max Row Checking  
	-- =============================================  
	BEGIN TRY       
		SELECT	@rowcount = count(Voucher_Acc_ID)
		FROM #temptable

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
	 	
	
--- Special
	insert into #temptable
	(
	Voucher_Acc_ID,
	Scheme_Code, 
	Remark,
	Public_Enquiry_Status,
	Public_Enq_Status_Remark,
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
	DOB,
	Exact_DOB,
	Sex,
	Date_of_Issue,
	Account_Status,
	PersonalInformation_Status,
	Source,
	Create_Dtm,
	SP_ID,
	SP_Practice_Display_Seq,
	EC_Age,
	EC_Date_of_Registration,
	Create_By,
	Doc_Code,
	Account_Purpose,
	Other_Info,
	Create_By_BO
	)
	select  TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable))
	TVA.Special_Acc_ID, 
	TVA.Scheme_Code,
	'',
	'',
	'',
	TP.Encrypt_Field1,
	TP.Encrypt_Field2,
	TP.Encrypt_Field3,
	TP.Encrypt_Field4,
	TP.Encrypt_Field5,
	TP.Encrypt_Field6,
	TP.Encrypt_Field7,
	TP.Encrypt_Field8,
	TP.Encrypt_Field9,
	TP.Encrypt_Field11,
	TP.DOB,
	TP.Exact_DOB,
	TP.Sex,
	TP.Date_of_Issue,
	TVA.Record_Status,
	TP.Record_Status,
	'S',
	TVA.Create_Dtm,
	C.SP_ID,
	C.SP_Practice_Display_Seq,
	TP.EC_Age,
	TP.EC_Date_of_Registration,
	--c.sp_id + '('+ convert(varchar,C.SP_Practice_Display_Seq) +')' as Create_By,
	C.Create_By,
	TP.Doc_Code,
	'',
	TP.Other_Info,
	C.Create_By_BO
	from SpecialAccount TVA, SpecialPersonalInformation TP, VoucherAccountCreationLOG C
	where 
	TVA.Special_Acc_ID = TP.Special_Acc_ID
	and C.Voucher_Acc_Type = 'S'
	and (@IdentityNum = '' or TP.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @IdentityNum)
		or TP.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @IdentityNum2))
	and (@Adoption_Prefix_Num = '' or TP.Encrypt_Field11 = EncryptByKey(KEY_GUID('sym_Key'), @Adoption_Prefix_Num))
	and (@Eng_Name = '' or TP.Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @Eng_Name))
	and (@DOB is NULL or TP.DOB = @DOB)
	and (@Voucher_Acc_ID = '')
	and (@ReferenceNo = '' or TVA.Special_Acc_ID=@ReferenceNo)
	and (TVA.Special_Acc_ID = C.Voucher_Acc_ID)
	and (@Doc_Code = '' or TP.Doc_Code = @Doc_Code)



	-- =============================================    
	-- Max Row Checking  
	-- =============================================  
	BEGIN TRY       	
		SELECT	@rowcount = count(Voucher_Acc_ID)
		FROM #temptable
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
 
	
--- Invalid
--	insert into #temptable
--	(
--	Voucher_Acc_ID,
--	Scheme_Code, 
--	Remark,
--	Public_Enquiry_Status,
--	Public_Enq_Status_Remark,
--	Encrypt_Field1,
--	Encrypt_Field2,
--	Encrypt_Field3,
--	Encrypt_Field4,
--	Encrypt_Field5,
--	Encrypt_Field6,
--	Encrypt_Field7,
--	Encrypt_Field8,
--	Encrypt_Field9,
--	Encrypt_Field11,
--	DOB,
--	Exact_DOB,
--	Sex,
--	Date_of_Issue,
--	Account_Status,
--	PersonalInformation_Status,
--	Source,
--	Create_Dtm,
--	SP_ID,
--	SP_Practice_Display_Seq,
--	EC_Age,
--	EC_Date_of_Registration,
--	Create_By,
--	Doc_Code,
--	Account_Purpose,
--	Other_Info
--	)
--	select 
--	TVA.Invalid_Acc_ID, 
--	TVA.Scheme_Code,
--	'',
--	'',
--	'',
--	TP.Encrypt_Field1,
--	TP.Encrypt_Field2,
--	TP.Encrypt_Field3,
--	TP.Encrypt_Field4,
--	TP.Encrypt_Field5,
--	TP.Encrypt_Field6,
--	TP.Encrypt_Field7,
--	TP.Encrypt_Field8,
--	TP.Encrypt_Field9,
--	TP.Encrypt_Field11,
--	TP.DOB,
--	TP.Exact_DOB,
--	TP.Sex,
--	TP.Date_of_Issue,
--	TVA.Record_Status,
--	TP.Record_Status,
--	'I',
--	TVA.Create_Dtm,
--	C.SP_ID,
--	C.SP_Practice_Display_Seq,
--	TP.EC_Age,
--	TP.EC_Date_of_Registration,
--	c.sp_id + '('+ convert(varchar,C.SP_Practice_Display_Seq) +')' as Create_By,
--	TP.Doc_Code,
--	'',
--	TP.Other_Info
--	from InvalidAccount TVA, InvalidPersonalInformation TP, VoucherAccountCreationLOG C
--	where 
--	TVA.Invalid_Acc_ID = TP.Invalid_Acc_ID 
--	and C.Voucher_Acc_Type = 'I'
--	and (@IdentityNum = '' or TP.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @IdentityNum)
--		or TP.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @IdentityNum2))
--	and (@Adoption_Prefix_Num = '' or TP.Encrypt_Field11 = EncryptByKey(KEY_GUID('sym_Key'), @Adoption_Prefix_Num))
--	and (@Eng_Name = '' or TP.Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @Eng_Name))
--	and (@DOB is NULL or TP.DOB = @DOB)
--	and (@Voucher_Acc_ID = '')
--	and (@ReferenceNo = '' or TVA.Invalid_Acc_ID=@ReferenceNo)
--	and (TVA.Invalid_Acc_ID = C.Voucher_Acc_ID)


--	SELECT	@rowcount = count(Voucher_Acc_ID)
--		FROM #temptable
	
--	IF @rowcount > @maxrow
--	BEGIN
--		Raiserror('00009', 16, 1)
--		return @@error
--	END



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

	select 
	t.Voucher_Acc_ID, 
	t.Scheme_Code, 
	t.Remark,
	t.Public_Enquiry_Status,
	t.Public_Enq_Status_Remark,
	convert(varchar, DecryptByKey(t.Encrypt_Field1)) as IdentityNum,
	convert(varchar(40), DecryptByKey(t.Encrypt_Field2)) as Eng_name,
	isnull(convert(nvarchar, DecryptByKey(t.Encrypt_Field3)),'') as Chi_Name,
	convert(varchar, DecryptByKey(t.Encrypt_Field4)) as CCcode1,
	convert(varchar, DecryptByKey(t.Encrypt_Field5)) as CCcode2,
	convert(varchar, DecryptByKey(t.Encrypt_Field6)) as CCcode3,
	convert(varchar, DecryptByKey(t.Encrypt_Field7)) as CCcode4,
	convert(varchar, DecryptByKey(t.Encrypt_Field8)) as CCcode5,
	convert(varchar, DecryptByKey(t.Encrypt_Field9)) as CCcode6,
	isnull(convert(char, DecryptByKey(t.Encrypt_Field11)),'') as Adoption_Prefix_Num,
	t.DOB,
	t.Exact_DOB,
	t.Sex,
	t.Date_of_Issue,
	t.Account_Status,
	t.PersonalInformation_Status,
	t.Source,
	t.Create_Dtm,
	t.SP_ID,
	t.SP_Practice_Display_Seq,
	t.EC_Age,
	t.EC_Date_of_Registration,
	t.Create_By,
	t.Doc_Code,
	dt.Doc_Display_Code,
	t.Account_Purpose,
	t.Other_Info,
	t.Create_By_BO
	from #temptable t, doctype dt
	where t.doc_code = dt.doc_code collate database_default
	--and (@Doc_Code = '' or T.Doc_Code = @Doc_Code)
	Order by convert(varchar, DecryptByKey(t.Encrypt_Field1)) ASC	
	EXEC [proc_SymmetricKey_close]
	
END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccountListForMaintR2_get] TO HCVU
GO
