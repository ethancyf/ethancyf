IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SpecialAccount_check_byECField]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SpecialAccount_check_byECField]
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
-- Author:		Pak Ho LEE
-- Create date: 08 Sep 2009
-- Description:	Check If the EC Reference Number and EC serial num in used by other account
-- =============================================

CREATE PROCEDURE [dbo].[proc_SpecialAccount_check_byECField]
	@Doc_Code char(20),
	@identity varchar(20),
	@EC_Serial_No varchar(10),
	@EC_Reference_No varchar(15),
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
		TVA.[Special_Acc_ID] as [Voucher_Acc_ID]
	FROM
		[SpecialPersonalInformation] TPI
			INNER JOIN [SpecialAccount] TVA
				ON TPI.[Special_Acc_ID] = TVA.[Special_Acc_ID]			
	WHERE
		[Doc_Code] = @Doc_Code AND 
		TVA.[Record_Status] <> 'D' AND TVA.[Record_Status] <> 'V' AND
		TVA.[Special_Acc_ID] <> @ExcludeVoucher_Acc_ID AND		
		(
			( [EC_Serial_No] = @EC_Serial_No AND [EC_Reference_No] = @EC_Reference_No AND [Encrypt_Field1] <> EncryptByKey(KEY_GUID('sym_Key'), @identity))
			OR
			( [Encrypt_Field1] = EncryptByKey(KEY_GUID('sym_Key'), @identity) AND NOT ([EC_Serial_No] = @EC_Serial_No AND [EC_Reference_No] = @EC_Reference_No))
		)
				
EXEC [proc_SymmetricKey_close]

END
GO

GRANT EXECUTE ON [dbo].[proc_SpecialAccount_check_byECField] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SpecialAccount_check_byECField] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_SpecialAccount_check_byECField] TO WSEXT
Go

