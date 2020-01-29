IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Proc_PersonalInformation_get_BySpecial_HKID_VoucherID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[Proc_PersonalInformation_get_BySpecial_HKID_VoucherID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

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
-- Description:		Retrieve [EC_Reference_Other_Format]
-- =============================================
-- =============================================
-- Author:		Dedrick Ng
-- Create date: 27 Sep 2009
-- Description:	Retrieve the PersonalInformation
--				Where HKID is same with SpecialPersonalInformation HKID
--				By SpecialAccountID
-- =============================================

CREATE PROCEDURE [dbo].[Proc_PersonalInformation_get_BySpecial_HKID_VoucherID]
	@Special_Acc_ID	char(15)
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
		PInfo.EC_Reference_No_Other_Format,
		--PInfo.EC_Date,
		PInfo.EC_Age,
		PInfo.EC_Date_of_Registration,
		PInfo.Doc_Code,
		PInfo.Deceased, 
		PInfo.DOD, 
		PInfo.Exact_DOD				
	FROM [dbo].[PersonalInformation] PInfo
		INNER JOIN [dbo].[SpecialPersonalInformation] TPInfo ON
		PInfo.Encrypt_Field1 = TPInfo.Encrypt_Field1 AND  TPInfo.Special_Acc_ID = @Special_Acc_ID


CLOSE SYMMETRIC KEY sym_Key

	
END

GO

GRANT EXECUTE ON [dbo].[Proc_PersonalInformation_get_BySpecial_HKID_VoucherID] TO HCVU
GO
