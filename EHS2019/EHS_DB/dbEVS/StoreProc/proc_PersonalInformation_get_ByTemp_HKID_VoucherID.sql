IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PersonalInformation_get_ByTemp_HKID_VoucherID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PersonalInformation_get_ByTemp_HKID_VoucherID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	19 May 2010
-- Description:		Retrieve [EC_Reference_No_Other_Format]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Pak Ho LEE
-- Modified date: 23 Mar 2010
-- Description:	Add Column Create_By_SmartID
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 26 Aug 2008
-- Description:	Retrieve the PersonalInformation
--				Where HKID is same with TempPersonalInformation HKID
--				By TempVoucherAccountID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Dedrick Ng
-- Modified date: 17 Sep 2009
-- Description:	Add new fields and remove obsolete fields
-- =============================================
CREATE PROCEDURE [dbo].[proc_PersonalInformation_get_ByTemp_HKID_VoucherID]
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
OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key
-- =============================================
-- Return results
-- =============================================

	SELECT 
		PInfo.Voucher_Acc_ID,
		PInfo.DOB,
		PInfo.Exact_DOB,
		PInfo.Sex,
		PInfo.Date_of_Issue,
		--PInfo.HKID_Card,
		PInfo.Create_By_SmartID,
		PInfo.Record_Status,
		PInfo.Create_Dtm,
		PInfo.Create_By,
		PInfo.Update_Dtm,		
		PInfo.Update_By,
		PInfo.DataEntry_By,
		--P.HKID, 
		--P.Eng_Name, 
		--P.Chi_Name, 
		--P.CCcode1, P.CCcode2, P.CCcode3, 
		--P.CCcode4, P.CCcode5, P.CCcode6, 
		convert(varchar, DecryptByKey(PInfo.[Encrypt_Field1])) as HKID,
		convert(varchar(40), DecryptByKey(PInfo.[Encrypt_Field2])) as Eng_Name,
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
		--PInfo.EC_Date,
		PInfo.EC_Age,
		PInfo.EC_Date_of_Registration,
		PInfo.Doc_Code,
		PInfo.Create_By_SmartID,
		PInfo.EC_Reference_No_Other_Format
		
	FROM [dbo].[PersonalInformation] PInfo
		INNER JOIN [dbo].[TempPersonalInformation] TPInfo ON
		PInfo.Encrypt_Field1 = TPInfo.Encrypt_Field1 AND  TPInfo.Voucher_Acc_ID = @Voucher_Acc_ID


CLOSE SYMMETRIC KEY sym_Key

	
END

GO

GRANT EXECUTE ON [dbo].[proc_PersonalInformation_get_ByTemp_HKID_VoucherID] TO HCVU
GO
