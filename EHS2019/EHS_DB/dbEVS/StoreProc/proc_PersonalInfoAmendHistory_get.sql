IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PersonalInfoAmendHistory_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PersonalInfoAmendHistory_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE20-023 (COVID19)
-- Modified by:		Winnie SUEN
-- Modified date:	28 May 2021
-- Description:		Add [PASS_Issue_Region]
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
-- CR No.:			CRE18-019 (To read new Smart HKIC in eHS(S))
-- Modified by:		Winnie SUEN
-- Modified date:	5 Dec 2018
-- Description:		Add [SmartID_Ver]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	19 May 2010
-- Description:		Retrieve [EC_Reference_No_Other_Format]
-- =============================================
-- =============================================
-- Author:		Timothy Leung
-- Create date: 10 June 2008
-- Description:	Retrieve the amendment history for specific Voucher Account
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Timothy LEUNG
-- Modified date:	20 Oct 2008
-- Description:		Add Certification of Exemption information
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Kathy LEE
-- Modified date:	15 Sep 2009
-- Description:		Add doc_code is the search
-- =============================================

CREATE PROCEDURE [dbo].[proc_PersonalInfoAmendHistory_get]
	@Voucher_Acc_ID char(15),
	@doc_code char(10)
AS
BEGIN

	EXEC [proc_SymmetricKey_open]

	SET NOCOUNT ON;

	select 
  	System_dtm as Amend_Dtm,
	Voucher_Acc_ID,
	convert(varchar, DecryptByKey(Encrypt_Field1)) as IdentityNum, 
	convert(varchar(100), DecryptByKey(Encrypt_Field2)) as Eng_Name,
	convert(nvarchar, DecryptByKey(Encrypt_Field3)) as Chi_Name,
	convert(varchar, DecryptByKey(Encrypt_Field4)) as CCcode1,
	convert(varchar, DecryptByKey(Encrypt_Field5)) as CCcode2,
	convert(varchar, DecryptByKey(Encrypt_Field6)) as CCcode3,
	convert(varchar, DecryptByKey(Encrypt_Field7)) as CCcode4,
	convert(varchar, DecryptByKey(Encrypt_Field8)) as CCcode5,
	convert(varchar, DecryptByKey(Encrypt_Field9)) as CCcode6,
	DOB,
	Exact_DOB,
	Sex,
	Date_of_Issue,
	Create_By_SmartID,
	Update_By,
	Record_Status, 
	isNull(SubmitToVerify,'') as SubmitToVerify,
	EC_Serial_No,
	EC_Reference_No,	
	EC_Age,
	EC_Date_of_Registration,
	Foreign_Passport_No,
	Permit_To_Remain_Until,
	convert(char, DecryptByKey(Encrypt_Field11)) as Adoption_Prefix_Num,
	dt.doc_code,
	dt.doc_display_code,
	P.other_info,
	P.EC_Reference_No_Other_Format,
	--isNull(Cancel_By,'') as Cancel_By,
	--isNull(Cancel_Dtm,'') as Cancel_Dtm,
	--isNull(Action_type,'') as Action_type,
	--isNull(Temp_Voucher_Acc_ID, '') as Temp_Voucher_Acc_ID
	p.SmartID_Ver,
	p.PASS_Issue_Region
	from PersonalInfoAmendHistory P, DocType DT
	where P.Voucher_Acc_ID = @Voucher_Acc_ID and
			P.Record_Status in ('V','A') and
			P.Doc_Code = DT.Doc_Code and
			P.Doc_Code = @doc_code		
	order by P.system_dtm desc

	EXEC [proc_SymmetricKey_close]
END
GO

GRANT EXECUTE ON [dbo].[proc_PersonalInfoAmendHistory_get] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_PersonalInfoAmendHistory_get] TO HCVU
GO
