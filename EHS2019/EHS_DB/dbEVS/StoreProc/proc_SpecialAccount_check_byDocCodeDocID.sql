IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SpecialAccount_check_byDocCodeDocID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SpecialAccount_check_byDocCodeDocID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 08 Sep 2009
-- Description:	Check if Special EHS Account Exist Document & Document Identity
--				For HKIC & EC Case Only
-- =============================================


CREATE PROCEDURE [dbo].[proc_SpecialAccount_check_byDocCodeDocID]
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
OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key
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
		[Encrypt_Field1] = EncryptByKey(KEY_GUID('sym_Key'), @identity) AND 
		TVA.[Record_Status] <> 'D' AND TVA.[Record_Status] <> 'V'

CLOSE SYMMETRIC KEY sym_Key
END
GO

GRANT EXECUTE ON [dbo].[proc_SpecialAccount_check_byDocCodeDocID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SpecialAccount_check_byDocCodeDocID] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_SpecialAccount_check_byDocCodeDocID] TO WSEXT
Go