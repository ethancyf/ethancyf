IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccount_get_BySchCodeSPInfoStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccount_get_BySchCodeSPInfoStatus]
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
-- CR No.:			CRE13-019-02 Extend HCVS to China
-- Modified by:		Chris YIM
-- Modified date:	24 March 2015
-- Description:		Add Input Parameter "Available_HCSP_SubPlatform"
-- =============================================  
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	5 May 2010
-- Description:		[EC_Reference_No]: varchar(15) -> varchar(40)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	01 Feb 2010
-- Description:		1. Also retieve the temporary account which account_purpose = 'A'
--					   when those temp account(s) is(are) created in HCSP platfrom (Change
--					   personal particulars by Smart IC)
--					2. DO NOT retrieve the special accounts even through accounts are validation
--					   fail since those accounts will be rectified by back office user
-- =============================================
-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 26 May 2008
-- Description:	Retrieve Temporary Account according to the validating status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	 Timothy LEUNG
-- Modified date: 20 Oct 2008
-- Description:	Add Certification of Exemption information
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	 Clark YIP
-- Modified date: 18 Dec 2008
-- Description:	1. Get 1 more field from TempVoucherAccount table: Original_Acc_ID
--				2. Select one more field: Display_Acc_ID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	4 Aug 2009
-- Description:		Retrieve Practice Chi Name
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	24 Aug 2009
-- Description:		Remove @Scheme_code in search criteria
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	8 Sep 2009
-- Description:		Transaction field: char(20)
-- =============================================

CREATE PROCEDURE [dbo].[proc_TempVoucherAccount_get_BySchCodeSPInfoStatus]
	@SP_ID	char(8),
	@DataEntry_By varchar(20),
	@Status	char(1),
	@Available_HCSP_SubPlatform	char(2)
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
	DECLARE @In_SP_ID			char(8)	
	DECLARE @In_DataEntry_By	varchar(20)
	DECLARE @In_Status			char(1)
	DECLARE @In_Available_HCSP_SubPlatform	char(2)
	SET @In_SP_ID = @SP_ID
	SET @In_DataEntry_By = @DataEntry_By
	SET @In_Status = @Status
	SET @In_Available_HCSP_SubPlatform = @Available_HCSP_SubPlatform
	
	create table #temp_account
	(
		voucher_acc_id char(15)		COLLATE DATABASE_DEFAULT,
		scheme_code char(10),
		Encrypt_Field1 varbinary(100),
		Encrypt_Field2 varbinary(100),
		Encrypt_Field3 varbinary(100),
		dob datetime,		
		exact_dob char(1),
		sex char(1),
		date_of_issue datetime,
		EC_Serial_No varchar(10),
		EC_Reference_No varchar(40),
		EC_Age smallint,
		EC_Date_of_Registration datetime,
		Foreign_Passport_No char(20),
		Permit_To_Remain_Until datetime,
		Encrypt_Field11 varbinary(100),
		doc_code char(20),
		--Doc_Display_code varchar(20),
		personal_info_record_status char(1),
		account_status char(1),
		display_status char(1),
		source char(1),
		create_dtm datetime,
		dataentry_by varchar(20),
		practice_name nvarchar(300),
		practice_name_chi nvarchar(300),
		original_acc_id char(20),
		Display_Acc_ID	char(20),
		transaction_id char(20),
		other_info varchar(10),
		Deceased	char(1),
		DOD			datetime,
		Exact_DOD	char(1)
	)

	CREATE INDEX IX_VAT on #temp_account (voucher_acc_id)

	create table #transaction
	(
		transaction_id char(20) COLLATE DATABASE_DEFAULT,
		temp_voucher_acc_id char(15),
		doc_code char(20)
	)

	CREATE INDEX IX_VAT on #transaction (transaction_id)

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================

	-- Temp Account (Pending Confirmation / Pending Validation / Pending Validation (Special Account) / Validation Failed)
	insert into #temp_account
	(
		voucher_acc_id,
		scheme_code,
		Encrypt_Field1,
		Encrypt_Field2,
		Encrypt_Field3,
		dob,	
		exact_dob,
		sex,
		date_of_issue,
		EC_Serial_No,
		EC_Reference_No,
		EC_Age,
		EC_Date_of_Registration,
		Foreign_Passport_No,
		Permit_To_Remain_Until,
		Encrypt_Field11,
		doc_code,
		--Doc_Display_code,
		personal_info_record_status,
		account_status,
		display_status,
		source,
		create_dtm,
		dataentry_by,
		practice_name,
		practice_name_chi,
		original_acc_id,
		Display_Acc_ID,
		other_info,
		Deceased,
		DOD,
		Exact_DOD
	)
	select	tva.voucher_acc_id,
			tva.scheme_code,
			tp.Encrypt_Field1,
			tp.Encrypt_Field2,
			tp.Encrypt_Field3,			
			tp.dob,
			tp.exact_dob,
			tp.sex,
			tp.date_of_issue,
			tp.EC_Serial_No,
			tp.EC_Reference_No,
			tp.EC_Age,
			tp.EC_Date_of_Registration,
			tp.Foreign_Passport_No,
			tp.Permit_To_Remain_Until,
			tp.Encrypt_Field11,
			tp.doc_code,
			--d.Doc_Display_Code,
			tp.record_status,
			tva.record_status,
			tva.record_status,
			'T',			
			tva.create_dtm,
			vac.dataentry_by,
			p.practice_name,
			p.practice_name_chi,
			tva.original_acc_id,
			isNull(TVA.Original_Acc_ID, TVA.Voucher_Acc_ID),
			tp.other_info,
			tp.Deceased,
			tp.DOD,
			tp.Exact_DOD
	from tempvoucheraccount tva 	
			LEFT JOIN SchemeClaim SC
				ON tva.Scheme_Code = SC.Scheme_Code,
		TempPersonalInformation tp, 
		VoucherAccountCreationLOG vac, 
		practice p --,doctype d
	where tva.voucher_acc_id = tp.voucher_acc_id
	--and tp.record_status = 'N'
	and tva.account_purpose <> 'O'
	and tva.create_by_BO = 'N'
	and tva.record_status not in ('D', 'V')
	and (@In_Status is null or tva.record_status = @In_Status )
	and tva.voucher_acc_id = vac.voucher_acc_id
	and vac.voucher_acc_type = 'T'
	and vac.sp_id = p.sp_id
	and vac.SP_Practice_Display_seq = p.display_seq	
	and vac.sp_id = @In_SP_ID
	and (@In_DataEntry_By is null or vac.dataentry_by = @In_DataEntry_By)
	--and tp.doc_code = d.doc_code
	and (@In_Available_HCSP_SubPlatform is null or SC.Available_HCSP_SubPlatform = @In_Available_HCSP_SubPlatform)

	
--insert into #temp_account
--	(
--		voucher_acc_id,
--		scheme_code,
--		Encrypt_Field1,
--		Encrypt_Field2,
--		Encrypt_Field3,
--		dob,	
--		exact_dob,
--		sex,
--		date_of_issue,
--		EC_Serial_No,
--		EC_Reference_No,
--		EC_Age,
--		EC_Date_of_Registration,
--		Foreign_Passport_No,
--		Permit_To_Remain_Until,
--		Encrypt_Field11,
--		doc_code,
--		--Doc_Display_code,
--		personal_info_record_status,
--		account_status,
--		display_status,
--		source,
--		create_dtm,
--		dataentry_by,
--		practice_name,
--		practice_name_chi,
--		original_acc_id,
--		Display_Acc_ID,
--		other_info
--	)
--	select	tva.Special_Acc_ID,
--			tva.scheme_code,
--			tp.Encrypt_Field1,
--			tp.Encrypt_Field2,
--			tp.Encrypt_Field3,			
--			tp.dob,
--			tp.exact_dob,
--			tp.sex,
--			tp.date_of_issue,
--			tp.EC_Serial_No,
--			tp.EC_Reference_No,
--			tp.EC_Age,
--			tp.EC_Date_of_Registration,
--			tp.Foreign_Passport_No,
--			tp.Permit_To_Remain_Until,
--			tp.Encrypt_Field11,
--			tp.doc_code,
--			--d.Doc_Display_Code,
--			tp.record_status,
--			tva.record_status,
--			tva.record_status,
--			'S',			
--			tva.create_dtm,
--			vac.dataentry_by,
--			p.practice_name,
--			p.practice_name_chi,
--			tva.original_acc_id,
--			isNull(TVA.Original_Acc_ID, TVA.Special_Acc_ID),
--			tp.other_info
--	from specialaccount tva, specialPersonalInformation tp, VoucherAccountCreationLOG vac, practice p --,doctype d
--	where tva.Special_Acc_ID = tp.Special_Acc_ID
--	and tva.Special_Acc_ID = vac.voucher_acc_id
--	and vac.sp_id = p.sp_id
--	and vac.SP_Practice_Display_seq = p.display_seq
--	and vac.voucher_acc_type = 'S'
--	and tva.special_acc_id in (select voucher_acc_id from TempVoucherAccPendingVerify)
--	--and tp.record_status = 'N'
--	and tva.record_status not in ('D', 'V')
--	and (tva.record_status = @Status or @Status = '')
--	and vac.sp_id = @sp_id
--	and (vac.dataentry_by = @dataentry_by or @dataentry_by = '')
	
	insert into #transaction
	select Transaction_ID, Temp_Voucher_Acc_ID, doc_code
	from VoucherTransaction 
	where Temp_Voucher_Acc_ID collate database_default in
	(select Voucher_Acc_ID 
	 from #temp_account
	)
	
	update	#temp_account
	set		transaction_id = t.transaction_id
	from	#transaction t, #temp_account ta
	where	ta.Voucher_Acc_ID = t.temp_voucher_acc_id collate database_default
			and ta.Doc_code = t.Doc_code collate database_default

-- =============================================
-- Return results
-- =============================================

-- Final Result
EXEC [proc_SymmetricKey_open]
	
	select
		t.voucher_acc_id,
		t.scheme_code,
		convert(varchar, DecryptByKey(t.Encrypt_Field1)) as IdentityNum,
		convert(varchar(100), DecryptByKey(t.Encrypt_Field2)) as Eng_Name,
		convert(nvarchar, DecryptByKey(t.Encrypt_Field3)) as Chi_Name,
		t.dob,
		t.exact_dob,
		t.sex,
		t.date_of_issue,
		t.EC_Serial_No,
		t.EC_Reference_No,
		t.EC_Age,
		t.EC_Date_of_Registration,
		t.Foreign_Passport_No,
		t.Permit_To_Remain_Until,
		convert(char, DecryptByKey(t.Encrypt_Field11)) as Adoption_Prefix_Num,
		t.doc_code,
		dt.Doc_Display_code,
		t.personal_info_record_status as record_status,
		t.account_status as VAStatus,
		t.display_status,
		t.source,
		t.create_dtm,
		t.dataentry_by,
		t.practice_name,
		t.practice_name_chi,
		t.original_acc_id,
		t.Display_Acc_ID,
		isnull(t.transaction_id,'') as Transaction_ID,
		t.other_info,
		t.Deceased,
		t.DOD,
		t.Exact_DOD
	from #temp_account t, doctype dt
	where t.doc_code = dt.doc_code COLLATE DATABASE_DEFAULT
	--where  (account_status = @Status or @Status = '')
	order by t.create_dtm asc
	

drop table #temp_account
drop table #transaction

		
EXEC [proc_SymmetricKey_close]
END
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_get_BySchCodeSPInfoStatus] TO HCSP
GO
