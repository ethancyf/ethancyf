IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccountConfirm_get_bySPID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccountConfirm_get_bySPID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- CR No.:			CRE20-023
-- Modified by:		Martin Tang
-- Modified date:	20 Apr 2021
-- Description:		Extend patient name's maximum length
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			INT20-0086
-- Modified by:		Winnie SUEN
-- Modified date:	20 Mar 2021
-- Description:		Fix Data type of [Practice_Name] VARCHAR(100) -> NVARCHAR(100)
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
-- CR No.:			CRE14-016 (To introduce 'Deceased' status into eHS)
-- Modified by:	    Winnie SUEN
-- Modified date:   15 Nov 2017
-- Description:		Add [Deceased], [DOD], [Exact_DOD]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- CR No.			CRE15-014
-- Modified date:	4 Jan 2016
-- Description:		(1) Remove [CCValue]
--					(2) Remove Compare CCCode logic
--					(3) Change [Chi_Name] nvarchar(6) -> nvarchar(12)
-- =============================================
-- =============================================  
-- Modification History  
-- CR No.:			CRE13-019-02 Extend HCVS to China
-- Modified by:		Chris YIM
-- Modified date:	03 March 2015
-- Description:		Add Input Parameter "Available_HCSP_SubPlatform"
-- =============================================  
-- =============================================  
-- Modification History  
-- CR No.:			INT14-0039
-- Modified by:		Lawrence TSANG
-- Modified date:	18 February 2015
-- Description:		Fix the missing parameter @Create_Dtm
-- =============================================  
-- =============================================  
-- Modification History  
-- CR No.:			CRE14-019
-- Modified by:		Lawrence TSANG
-- Modified date:	21 January 2015
-- Description:		Insert into [SProcPerformance] to record sproc performance
-- =============================================  
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	5 May 2010
-- Description:		(1) [EC_Reference_No]: varchar(15) -> varchar(40)
--					(2) Retrieve [EC_Reference_No_Other_Format]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy
-- Modified date:	23 Feb 2010
-- Description:		1. Retrieve the Original Amend Acc ID
--					2. Retrieve the Date of issue of the Original account
--					   which is amendment case
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy
-- Modified date:	03 December 2009
-- Description:		Close SYMMETRIC key after decryption
-- =============================================
-- =============================================
-- Author:			Billy Lam
-- Create date:		31 July 2008
-- Description:		Get the list of TempVoucherAccount to confirm
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Timothy LEUNG
-- Modified date:	20 Oct 2008
-- Description:		Add Certification of Exemption information
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	31 July 2009
-- Description:		Retrieve Practice Chi Name
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	13 August 2009
-- Description:		Add the following information in result set
--					scheme, doc type, doc id
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Paul Yip
-- Modified date:	17 August 2009
-- Description:		Filter by Scheme Code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Paul Yip
-- Modified date:	19 August 2009
-- Description:		Add the following information in result set
--					Doc_code, foreign_passport_no, permit_to_remain_until
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	31 August 2009
-- Description:		1. Change Encrypt_Field1 to varchar
--					2. Assign updated chineses name
-- =============================================

CREATE Procedure proc_TempVoucherAccountConfirm_get_bySPID
@SP_ID						char(8)
, @Practice_Display_Seq		smallint
, @DataEntry_By				varchar(20)
, @Create_Dtm				datetime
, @SchemeCode				char(10)
, @Available_HCSP_SubPlatform	char(2)
as

-- =============================================
-- Initialization
-- =============================================
DECLARE @Performance_Start_Dtm datetime
SET @Performance_Start_Dtm = GETDATE()

DECLARE @In_SP_ID						char(8)
DECLARE @In_Practice_Display_Seq		smallint
DECLARE @In_DataEntry_By				varchar(20)
DECLARE @In_Create_Dtm				datetime
DECLARE @In_SchemeCode				char(10)
DECLARE @In_Available_HCSP_SubPlatform	char(2)
SET @In_SP_ID = @SP_ID
SET @In_Practice_Display_Seq = @Practice_Display_Seq
SET @In_DataEntry_By = @DataEntry_By
SET @In_Create_Dtm = @Create_Dtm
SET @In_SchemeCode = @SchemeCode
SET @In_Available_HCSP_SubPlatform = @Available_HCSP_SubPlatform


select @In_Create_Dtm = dateadd(day, 1, @In_Create_Dtm)

create table #tempVoucherAcc 
(
	Transaction_Dtm datetime,
	IdentityNum		varchar(20),
	Date_Of_Issue	datetime,
	Eng_Name		varchar(100),
	Chi_Name		nvarchar(12),
	DOB				datetime,
	Exact_DOB		char(1),
	Sex				char(1),
	DataEntry_By	varchar(20),
	Practice_Name	nvarchar(100),
	Practice_Name_Chi nvarchar(100),
	Voucher_Acc_ID	char(15) COLLATE DATABASE_DEFAULT,
	Scheme_Code		char(10),
	TSMP			binary(8),
	EC_Serial_No	varchar(15),
	EC_Reference_No	varchar(40),
	EC_Reference_No_Other_Format	char(1),
	EC_Age			smallint,
	EC_Date_of_Registration datetime,
	Foreign_Passport_No	char(20),
	Permit_To_Remain_Until datetime,
	Adoption_Prefix_Num char(7),
	Scheme_Display_Code char(25),
	--Doc_display_code	varchar(20),
	Doc_Code			char(20),
	
	CCcode1				char(5) COLLATE DATABASE_DEFAULT,
	CCcode2				char(5) COLLATE DATABASE_DEFAULT,
	CCcode3				char(5) COLLATE DATABASE_DEFAULT,
	CCcode4				char(5) COLLATE DATABASE_DEFAULT,
	CCcode5				char(5) COLLATE DATABASE_DEFAULT,
	CCcode6				char(5) COLLATE DATABASE_DEFAULT,
	
	--CCValue1			int,
	--CCValue2			int,
	--CCValue3			int,
	--CCValue4			int,
	--CCValue5			int,
	--CCValue6			int,
	
	Other_Info			varchar(10),
	
	Original_Amend_Acc_ID	char(15),
	Original_TSMP			binary(8),
	Validated_Acc_ID		char(15),
	Original_DOI			datetime,
	Original_DOB			datetime,
	Original_Exact_DOB		char(1),
	Send_To_ImmD			char(1),

	Deceased	char(1),
	DOD			datetime,
	Exact_DOD	char(1)
)

CREATE INDEX IX_VAT on #tempVoucherAcc (IdentityNum, Doc_Code)

create table #tempTran
(
	voucher_acc_id char(15)	 COLLATE DATABASE_DEFAULT
)

CREATE INDEX IX_VAT on #tempTran (voucher_acc_id)

-- =============================================
-- Return results
-- =============================================
insert into #tempTran
(
	voucher_acc_id
)
select temp_voucher_acc_id
from vouchertransaction
where record_status <> 'I'
and sp_id = @In_SP_ID
and isnull(temp_voucher_acc_id,'') <> ''

EXEC [proc_SymmetricKey_open]

insert into #tempVoucherAcc
(
	Transaction_Dtm,
	IdentityNum,
	Date_Of_Issue,
	Eng_Name,
	Chi_Name,
	DOB,
	Exact_DOB,
	Sex,
	DataEntry_By,
	Practice_Name,
	Practice_Name_Chi,
	Voucher_Acc_ID,
	Scheme_Code,
	TSMP,
	EC_Serial_No,
	EC_Reference_No,
	EC_Reference_No_Other_Format,
	EC_Age,
	EC_Date_of_Registration,
	Foreign_Passport_No,
	Permit_To_Remain_Until,
	Adoption_Prefix_Num,
	Scheme_Display_Code,
	--Doc_display_code,
	Doc_Code,
	CCcode1,
	CCcode2,
	CCcode3,
	CCcode4,
	CCcode5,
	CCcode6,
	Other_Info,
	Original_Amend_Acc_ID,
	Validated_Acc_ID,
	Deceased,
	DOD,
	Exact_DOD
)
select	c.Transaction_Dtm,
		convert(varchar, DecryptByKey(p.Encrypt_Field1)),
		p.Date_of_Issue,
		convert(varchar(100), DecryptByKey(p.Encrypt_Field2)),
		convert(nvarchar, DecryptByKey(p.Encrypt_Field3)),
		p.DOB,
		p.Exact_DOB,
		p.Sex,
		t.DataEntry_By,
		pr.Practice_Name,
		pr.Practice_Name_Chi,
		t.Voucher_Acc_ID,
		t.Scheme_Code,
		t.tsmp,
		p.EC_Serial_No,
		p.EC_Reference_No,
		p.EC_Reference_No_Other_Format,
		p.EC_Age,
		p.EC_Date_of_Registration,
		p.foreign_passport_no,
		p.permit_to_remain_until,
		convert(char, DecryptByKey(p.Encrypt_Field11)),
		t.scheme_code,--sc.display_code,
		--dt.doc_display_code,
		p.Doc_code,
		CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field4])) as [CCcode1],
		CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field5])) as [CCcode2],
		CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field6])) as [CCcode3],
		CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field7])) as [CCcode4],
		CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field8])) as [CCcode5],
		CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field9])) as [CCcode6],
		p.other_info,
		t.Original_Amend_Acc_ID,
		t.Validated_Acc_ID,
		p.Deceased,
		p.DOD,
		p.Exact_DOD
	from TempVoucherAccount t, TempPersonalInformation p, Practice pr, VoucherAccountCreationLog c, SchemeClaim sc--, doctype dt
	where t.Voucher_Acc_ID = p.Voucher_Acc_ID
	and t.Voucher_Acc_ID not in (select voucher_acc_id from #tempTran)
	and t.Scheme_Code = sc.Scheme_Code
	--and (t.Account_Purpose = 'V' or t.Account_Purpose = 'C')
	and t.account_purpose in ('V', 'C', 'A')
	and t.create_by_bo = 'N'
	and t.Record_Status = 'C'
	and t.Voucher_Acc_ID = c.Voucher_Acc_ID
	and c.Voucher_Acc_Type = 'T'	
	and c.SP_ID = pr.SP_ID
	and c.SP_ID = @In_SP_ID
	and c.SP_Practice_Display_Seq = pr.Display_Seq
	and (@In_DataEntry_By is null or t.DataEntry_By = @In_DataEntry_By)
	and (@In_Practice_Display_Seq is null or pr.Display_Seq = @In_Practice_Display_Seq)
	and (@In_SchemeCode is null or t.scheme_code = @In_SchemeCode)
	and c.Transaction_Dtm < @In_Create_Dtm
	and (@In_Available_HCSP_SubPlatform is null or sc.Available_HCSP_SubPlatform = @In_Available_HCSP_SubPlatform)

	
EXEC [proc_SymmetricKey_close]

update #tempVoucherAcc
set original_TSMP = t.TSMP,
	Original_DOI = p.Date_Of_Issue,
	original_DOB = p.DOB,
	original_Exact_DOB = p.exact_dob
from #tempVoucherAcc vt, tempvoucheraccount t, temppersonalinformation p
where vt.original_amend_acc_id = t.voucher_acc_id collate database_default
and t.voucher_acc_id = p.voucher_acc_id collate database_default
and vt.original_amend_acc_id is not null
and t.account_purpose = 'O'

update #tempVoucherAcc
set Send_To_ImmD = 'X'
where original_amend_acc_id is null

update #tempVoucherAcc
set Send_To_ImmD = 'N'
where Date_Of_Issue = Original_DOI
and DOB = Original_DOB
and Exact_DOB = original_Exact_DOB
and Original_DOI is not null
and Original_DOB is not null
and original_Exact_DOB is not null
and original_amend_acc_id is not null

update #tempVoucherAcc
set Send_To_ImmD = 'Y'
where Send_To_ImmD is null

	update #tempVoucherAcc
	set Scheme_Display_Code = sc.display_code
	from #tempVoucherAcc t, schemeclaim sc
	where t.Scheme_Display_Code = sc.scheme_code COLLATE DATABASE_DEFAULT
	

	select 	t.Transaction_Dtm,
			t.IdentityNum,
			t.Date_Of_Issue,
			t.Eng_Name,
			t.Chi_Name,
			t.DOB,
			t.Exact_DOB,
			t.Sex,
			t.DataEntry_By,
			t.Practice_Name,
			t.Practice_Name_Chi,
			t.Voucher_Acc_ID,
			t.Scheme_Code,
			t.TSMP,
			t.EC_Serial_No,
			t.EC_Reference_No,
			t.EC_Reference_No_Other_Format,
			t.EC_Age,
			t.EC_Date_of_Registration,
			t.Foreign_Passport_No,
			t.Permit_To_Remain_Until,
			t.Adoption_Prefix_Num,
			t.Scheme_Display_Code,
			dt.Doc_display_code,
			t.Doc_Code,
			
			--case ltrim(rtrim(t.Doc_Code))
			--	when 'HKIC' then isnull(t.CCValue1,'') + isnull(t.CCValue2,'') + isnull(t.CCValue3,'') + isnull(t.CCValue4,'') + isnull(t.CCValue5,'') +  isnull(t.CCValue6,'') COLLATE DATABASE_DEFAULT
			--	else isnull(t.Chi_Name,'')
			--end as [Chi_Name],
			t.other_info,
			t.Original_Amend_Acc_ID,
			t.Original_TSMP,
			t.Validated_Acc_ID,
			t.Send_To_ImmD,
			t.Deceased,
			t.DOD,
			t.Exact_DOD
	from #tempVoucherAcc t, doctype dt
	where t.doc_code = dt.doc_code COLLATE DATABASE_DEFAULT
	order by Transaction_Dtm asc


drop table #tempTran
drop table #tempVoucherAcc

IF (SELECT Parm_Value1 FROM SystemParameters WHERE Parameter_Name = 'EnableSProcPerformCapture' AND Scheme_Code = 'ALL') = 'Y' BEGIN
	DECLARE @Performance_End_Dtm datetime
	SET @Performance_End_Dtm = GETDATE()
	DECLARE @Parameter varchar(255)
	SET @Parameter = ISNULL(@In_SP_ID, '') + ',' + ISNULL(CONVERT(varchar, @In_Practice_Display_Seq), '') + ',' + ISNULL(@In_DataEntry_By, '') + ','
					  + ISNULL(CONVERT(varchar, @In_Create_Dtm, 120), '') + ',' + ISNULL(@In_SchemeCode, '')
	
	EXEC proc_SProcPerformance_add 'proc_TempVoucherAccountConfirm_get_bySPID',
								   @Parameter,
								   @Performance_Start_Dtm,
								   @Performance_End_Dtm
END
	
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccountConfirm_get_bySPID] TO HCSP
GO
