IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherAccount_check_byAdoptionField]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherAccount_check_byAdoptionField]
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
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 26 Aug 2008
-- Description:	Check If the Adoption Number is different with any other same Identity num account
-- =============================================

CREATE PROCEDURE [dbo].[proc_VoucherAccount_check_byAdoptionField]
	@Doc_Code char(20),
	@identity varchar(20),
	@Adoption_PrefixNum char(7),
	@ExcludeVoucher_Acc_ID char(15)
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
EXEC [proc_SymmetricKey_open]
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================


	SELECT
		TVA.[Voucher_Acc_ID]
	FROM
		[PersonalInformation] TPI
			INNER JOIN [VoucherAccount] TVA
				ON TPI.[Voucher_Acc_ID] = TVA.[Voucher_Acc_ID]			
	WHERE
		[Doc_Code] = @Doc_Code AND 
		TVA.[Record_Status] <> 'D' AND 
		TVA.[Voucher_Acc_ID] <> @ExcludeVoucher_Acc_ID AND 
		(
			[Encrypt_Field1] = EncryptByKey(KEY_GUID('sym_Key'), @identity) AND [Encrypt_Field11] <> EncryptByKey(KEY_GUID('sym_Key'), @Adoption_PrefixNum)
		)

EXEC [proc_SymmetricKey_close]

END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccount_check_byAdoptionField] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccount_check_byAdoptionField] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccount_check_byAdoptionField] TO WSEXT
Go