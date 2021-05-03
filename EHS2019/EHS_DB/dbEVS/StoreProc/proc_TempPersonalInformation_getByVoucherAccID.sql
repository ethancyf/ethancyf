IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempPersonalInformation_getByVoucherAccID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempPersonalInformation_getByVoucherAccID]
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
-- Modified by:		Lawrence TSANG
-- Modified date:	19 May 2010
-- Description:		Add [EC_Reference_No_Other_Format]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	23 Mar 2010
-- Description:		Add column Create_By_SmartID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	11 Mar 2010
-- Description:		Grant right to [HCSP]
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 26 Aug 2008
-- Description:	Retrieve TempPersonalInformation By Voucher Account ID 
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Dedrick Ng
-- Modified date: 17 Sep 2009
-- Description:	Add new fields and Remove obsolete fields
-- =============================================
CREATE PROCEDURE [dbo].[proc_TempPersonalInformation_getByVoucherAccID]
	@Voucher_Acc_ID	char(15)
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
EXEC [proc_SymmetricKey_open]
-- =============================================
-- Return results
-- =============================================

	SELECT
		Voucher_Acc_ID,
		DOB,
		Exact_DOB,
		Sex,
		Date_of_Issue,
		--HKID_Card,
		Check_Dtm,
		Validating,
		Record_Status,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By,
		DataEntry_By,
		--P.HKID, 
		--P.Eng_Name, 
		--P.Chi_Name, 
		--P.CCcode1, P.CCcode2, P.CCcode3, 
		--P.CCcode4, P.CCcode5, P.CCcode6, 
		convert(varchar, DecryptByKey([Encrypt_Field1])) as HKID,
		convert(varchar(100), DecryptByKey([Encrypt_Field2])) as Eng_Name,
		convert(nvarchar, DecryptByKey([Encrypt_Field3])) as Chi_Name,
		convert(varchar, DecryptByKey([Encrypt_Field4])) as CCcode1,
		convert(varchar, DecryptByKey([Encrypt_Field5])) as CCcode2,
		convert(varchar, DecryptByKey([Encrypt_Field6])) as CCcode3,
		convert(varchar, DecryptByKey([Encrypt_Field7])) as CCcode4,
		convert(varchar, DecryptByKey([Encrypt_Field8])) as CCcode5,
		convert(varchar, DecryptByKey([Encrypt_Field9])) as CCcode6,
		TSMP,
		EC_Serial_No,
		EC_Reference_No,
		--EC_Date,
		EC_Age,
		EC_Date_of_Registration,
		
		Encrypt_Field10,

		Doc_Code,
		Foreign_Passport_No,
		Permit_To_Remain_Until,
		Encrypt_Field11,
		Other_Info,
		Create_By_SmartID,
		EC_Reference_No_Other_Format,
		Deceased, 
		DOD, 
		Exact_DOD
		
	FROM [dbo].[TempPersonalInformation]
	WHERE 
		Voucher_Acc_ID = @Voucher_Acc_ID
	
EXEC [proc_SymmetricKey_close]

END

GO

GRANT EXECUTE ON [dbo].[proc_TempPersonalInformation_getByVoucherAccID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_TempPersonalInformation_getByVoucherAccID] TO HCVU
GO
