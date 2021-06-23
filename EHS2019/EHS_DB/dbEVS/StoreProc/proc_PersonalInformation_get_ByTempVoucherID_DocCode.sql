 IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PersonalInformation_get_ByTempVoucherID_DocCode]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PersonalInformation_get_ByTempVoucherID_DocCode]
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
-- CR No.:			CRE14-016 (To introduce 'Deceased' status into eHS)
-- Modified by:	    Winnie SUEN
-- Modified date:   15 Nov 2017
-- Description:		Add [Deceased], [DOD], [Exact_DOD]
-- =============================================
-- =============================================
-- Author:		Paul Yip
-- Create date: 20 Dec 2010
-- Description:	Retrieve the PersonalInformation
--				Where identity number is same with TempPersonalInformation identity number
--				By TempVoucherAccountID, and the doc type of personal information must match with that specified
-- =============================================

CREATE PROCEDURE [dbo].[proc_PersonalInformation_get_ByTempVoucherID_DocCode]
	@Voucher_Acc_ID	char(15),
	@Doc_Code char(20)
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
		PInfo.Voucher_Acc_ID,
		PInfo.DOB,
		PInfo.Exact_DOB,
		PInfo.Sex,
		PInfo.Date_of_Issue,
		PInfo.Create_By_SmartID,
		PInfo.Record_Status,
		PInfo.Create_Dtm,
		PInfo.Create_By,
		PInfo.Update_Dtm,		
		PInfo.Update_By,
		PInfo.DataEntry_By, 
		convert(varchar, DecryptByKey(PInfo.[Encrypt_Field1])) as HKID,
		convert(varchar(100), DecryptByKey(PInfo.[Encrypt_Field2])) as Eng_Name,
		convert(nvarchar, DecryptByKey(PInfo.[Encrypt_Field3])) as Chi_Name,
		convert(varchar, DecryptByKey(PInfo.[Encrypt_Field4])) as CCcode1,
		convert(varchar, DecryptByKey(PInfo.[Encrypt_Field5])) as CCcode2,
		convert(varchar, DecryptByKey(PInfo.[Encrypt_Field6])) as CCcode3,
		convert(varchar, DecryptByKey(PInfo.[Encrypt_Field7])) as CCcode4,
		convert(varchar, DecryptByKey(PInfo.[Encrypt_Field8])) as CCcode5,
		convert(varchar, DecryptByKey(PInfo.[Encrypt_Field9])) as CCcode6,
		PInfo.TSMP,
		PInfo.EC_Serial_No,
		PInfo.EC_Reference_No,
		PInfo.EC_Age,
		PInfo.EC_Date_of_Registration,
		PInfo.Doc_Code,
		PInfo.Create_By_SmartID,
		PInfo.EC_Reference_No_Other_Format,
		PInfo.Deceased, 
		PInfo.DOD, 
		PInfo.Exact_DOD,
		PInfo.SmartID_Ver,
		PInfo.PASS_Issue_Region
		
	FROM [dbo].[PersonalInformation] PInfo
		INNER JOIN [dbo].[TempPersonalInformation] TPInfo ON
		PInfo.Encrypt_Field1 = TPInfo.Encrypt_Field1 
		AND  TPInfo.Voucher_Acc_ID = @Voucher_Acc_ID AND PInfo.Doc_Code = @Doc_Code


EXEC [proc_SymmetricKey_close]

	
END

GO

GRANT EXECUTE ON [dbo].[proc_PersonalInformation_get_ByTempVoucherID_DocCode] TO HCVU
GO
