IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccount_check_byECField]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccount_check_byECField]
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
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	4 May 2010
-- Description:		(1) [EC_Reference_No]: varchar(15) -> varchar(40)
--					(2) [EC_Serial_No] can be null, ISNULL() is required
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 26 Aug 2008
-- Description:	Check If the EC Reference Number and EC serial num in used by other account
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_TempVoucherAccount_check_byECField]
	@Doc_Code char(20),
	@identity varchar(20),
	@EC_Serial_No varchar(10),
	@EC_Reference_No varchar(40),
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
		[TempPersonalInformation] TPI
			INNER JOIN [TempVoucherAccount] TVA
				ON TPI.[Voucher_Acc_ID] = TVA.[Voucher_Acc_ID]			
	WHERE
		[Doc_Code] = @Doc_Code AND 
		TVA.[Record_Status] <> 'D' AND TVA.[Record_Status] <> 'V' AND
		TVA.[Account_Purpose] <> 'O' AND
		TVA.[Voucher_Acc_ID] <> @ExcludeVoucher_Acc_ID AND		
		(
			( ISNULL([EC_Serial_No], '') = @EC_Serial_No AND [EC_Reference_No] = @EC_Reference_No AND [Encrypt_Field1] <> EncryptByKey(KEY_GUID('sym_Key'), @identity))
			OR
			( [Encrypt_Field1] = EncryptByKey(KEY_GUID('sym_Key'), @identity) AND NOT (ISNULL([EC_Serial_No], '') = @EC_Serial_No AND [EC_Reference_No] = @EC_Reference_No))
		)
				
EXEC [proc_SymmetricKey_close]

END
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_check_byECField] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_check_byECField] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_check_byECField] TO WSEXT
Go
