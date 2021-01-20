IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccountDeletedList_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccountDeletedList_get]
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
-- Author:		Clark YIP
-- Create date: 2008 10 27
-- Description:	Retrieve the deleted validated Temp Voucher Account list
-- =============================================

-- =============================================
-- Modified by:	    Kathy LEE
-- Modified date:	14 Sep 2009
-- Description:		Remove Scheme_Code in the search route
-- =============================================

CREATE PROCEDURE [dbo].[proc_TempVoucherAccountDeletedList_get]
	-- Add the parameters for the stored procedure here	
	
AS
BEGIN

-- =============================================
-- Declaration
-- =============================================
DECLARE @tmp_VR_Delete table ( Voucher_Acc_ID char(15),
						Scheme_Code char(10),
						VAStatus char(1),
						Remark nvarchar(255),
						Public_Enquiry_Status char(1),
						Public_Enq_Status_Remark nvarchar(255),
						IdentityNum varchar(20),
						EName varchar(50),
						CName nvarchar(50),
						CCcode1 char(5),
						CCcode2 char(5),
						CCcode3 char(5),
						CCcode4 char(5),
						CCcode5 char(5),
						CCcode6 char(5),
						DOB datetime,
						Exact_DOB char(1),
						Sex char(1),
						Date_of_Issue datetime,
						PIStatus char(1),
						Create_Dtm datetime,
						Source char(1),
						SP_ID varchar(50),
						SP_Practice_Display_Seq smallint,
						EC_Age smallint,
						EC_Date_of_Registration datetime,
						Adoption_Prefix_Num char(7),
						Doc_Code char(20),
						Doc_Display_Code varchar(20))

-- =============================================
-- Initialization
-- =============================================
	SET NOCOUNT ON;

	EXEC [proc_SymmetricKey_open]

insert into @tmp_VR_Delete ( Voucher_Acc_ID ,
						Scheme_Code,
						VAStatus,
						Remark,
						Public_Enquiry_Status,
						Public_Enq_Status_Remark,
						IdentityNum,
						EName,
						CName,
						CCcode1,
						CCcode2,
						CCcode3,
						CCcode4,
						CCcode5,
						CCcode6,
						DOB,
						Exact_DOB,
						Sex,
						Date_of_Issue,
						PIStatus,
						Create_Dtm,
						Source,
						SP_ID,
						SP_Practice_Display_Seq,
						EC_Age,
						EC_Date_of_Registration,
						Adoption_Prefix_Num,
						Doc_Code,
						Doc_Display_Code)
select 
	VA.Voucher_Acc_ID, 
	VA.Scheme_Code, 
	VA.Record_Status as VAStatus,
	'' as Remark,
	'A' as Public_Enquiry_Status,
	'' as Public_Enq_Status_Remark,
	convert(varchar, DecryptByKey(P.Encrypt_Field1)),
	convert(varchar, DecryptByKey(P.Encrypt_Field2)),
	convert(nvarchar, DecryptByKey(P.Encrypt_Field3)),
	convert(varchar, DecryptByKey(P.Encrypt_Field4)),
	convert(varchar, DecryptByKey(P.Encrypt_Field5)),
	convert(varchar, DecryptByKey(P.Encrypt_Field6)),
	convert(varchar, DecryptByKey(P.Encrypt_Field7)),
	convert(varchar, DecryptByKey(P.Encrypt_Field8)),
	convert(varchar, DecryptByKey(P.Encrypt_Field9)),
	P.DOB,
	P.Exact_DOB,
	P.Sex,
	P.Date_of_Issue,
	P.Record_Status as PIStatus,
	VA.Create_Dtm,
	'T' as AccountType,
	C.SP_ID,
	C.SP_practice_display_seq,
	P.EC_Age,
	P.EC_Date_of_Registration,
	isnull(convert(char, DecryptByKey(P.Encrypt_Field11)),''),
	P.Doc_Code,
	DT.Doc_Display_Code
	from TempVoucherAccount VA, TempPersonalInformation P, VoucherAccountCreationLOG C, DocType DT
	where 
	VA.Voucher_Acc_ID = P.Voucher_Acc_ID 
	and P.Voucher_Acc_ID = C.Voucher_Acc_ID and VA.Voucher_Acc_ID = C.Voucher_Acc_ID
	and VA.Record_Status='D' and (VA.Account_Purpose='C' or VA.Account_Purpose='V')
	and P.Doc_Code = DT.Doc_Code
	order by convert(varchar, DecryptByKey(P.Encrypt_Field1)) 
	EXEC [proc_SymmetricKey_close]

	select	tvr.Voucher_Acc_ID ,
			tvr.Scheme_Code,
			tvr.VAStatus,
			tvr.Remark,
			tvr.Public_Enquiry_Status,
			tvr.Public_Enq_Status_Remark,
			tvr.IdentityNum,
			tvr.EName,
			tvr.CName,
			tvr.CCcode1,
			tvr.CCcode2,
			tvr.CCcode3,
			tvr.CCcode4,
			tvr.CCcode5,
			tvr.CCcode6,
			tvr.DOB,
			tvr.Exact_DOB,
			tvr.Sex,
			tvr.Date_of_Issue,
			tvr.PIStatus,
			tvr.Create_Dtm,
			tvr.Source,
			tvr.sp_id,
			tvr.SP_Practice_Display_Seq,
			tvr.sp_id + '('+ convert(varchar,tvr.SP_Practice_Display_Seq) +')' as Create_By,
			tvr.EC_Age,
			tvr.EC_Date_of_Registration,
			tvr.Adoption_Prefix_Num,
			tvr.Doc_Code,
			tvr.Doc_Display_Code,
			t.transaction_id as trans_id
	from @tmp_VR_Delete tvr
		left join vouchertransaction t
			on tvr.voucher_acc_id=t.temp_voucher_acc_id	
END
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccountDeletedList_get] TO HCVU
GO
