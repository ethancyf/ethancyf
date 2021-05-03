IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SpecialPersonalInformation_getByVoucherAccID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SpecialPersonalInformation_getByVoucherAccID]
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
-- CR# :			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- ============================================
-- =============================================
-- Author:		Dedrick Ng
-- Create date: 27 Sep 2009
-- Description:	Retrieve SpecialPersonalInformation By Special Account ID 
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_SpecialPersonalInformation_getByVoucherAccID]
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
EXEC [proc_SymmetricKey_open]
-- =============================================
-- Return results
-- =============================================

	SELECT
		Special_Acc_ID,
		DOB,
		Exact_DOB,
		Sex,
		Date_of_Issue,
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
		EC_Age,
		EC_Date_of_Registration,
		
		Encrypt_Field10,

		Doc_Code,
		Foreign_Passport_No,
		Permit_To_Remain_Until,
		Encrypt_Field11,
		Other_Info
		
	FROM [dbo].[SpecialPersonalInformation]
	WHERE 
		Special_Acc_ID = @Special_Acc_ID
	
EXEC [proc_SymmetricKey_close]

END

GO

GRANT EXECUTE ON [dbo].[proc_SpecialPersonalInformation_getByVoucherAccID] TO HCVU
GO
