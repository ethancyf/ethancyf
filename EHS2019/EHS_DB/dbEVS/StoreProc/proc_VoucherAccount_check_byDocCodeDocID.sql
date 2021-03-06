IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherAccount_check_byDocCodeDocID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherAccount_check_byDocCodeDocID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Koala CHENG
-- Modified date:	06 Jan 2021
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
-- Create date: 21 Aug 2009
-- Description:	Check if EHS Account Exist Document & Document Identity
--				For HKIC & EC Case Only
-- =============================================


CREATE PROCEDURE [dbo].[proc_VoucherAccount_check_byDocCodeDocID]
	@Doc_Code char(20),
	@identity varchar(20)
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
		[Voucher_Acc_ID]
	FROM
		[PersonalInformation] WITH (NOLOCK)
	WHERE
		[Doc_Code] = @Doc_Code AND Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @identity)

	EXEC [proc_SymmetricKey_close]
END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccount_check_byDocCodeDocID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccount_check_byDocCodeDocID] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccount_check_byDocCodeDocID] TO WSEXT
GO