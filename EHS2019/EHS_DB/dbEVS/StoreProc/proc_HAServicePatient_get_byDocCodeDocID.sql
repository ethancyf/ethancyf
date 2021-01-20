IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HAServicePatient_get_byDocCodeDocID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HAServicePatient_get_byDocCodeDocID]
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
-- CR No.:			CRE20-015-08 (HA Scheme)	
-- Modified by:		Chris YIM
-- Modified date:	01 Dec 2020
-- Description:		Only check doc. num.
-- =============================================
-- =============================================
-- CR No.:		CRE20-015 (HA Scheme)
-- Author:		Winnie SUEN
-- Create date: 12 Oct 2020
-- Description:	get HAServicePatient by Doc Code, Doc ID
-- =============================================

CREATE PROCEDURE proc_HAServicePatient_get_byDocCodeDocID
	@Doc_Code VARCHAR(20),
	@Identity VARCHAR(20)
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

	DECLARE @In_Doc_Code VARCHAR(20)
	DECLARE @In_Identity VARCHAR(20)
	SET @In_Doc_Code = @Doc_Code
	SET @In_Identity = @identity

-- =============================================
-- Return results
-- =============================================
EXEC [proc_SymmetricKey_open]
	
	SELECT
		[Serial_No],
		[Doc_Code],
		CONVERT(VARCHAR, DecryptByKey([Encrypt_Field1])) as [IdentityNum],
		[HKIC_Symbol],
		[Claimed_Payment_Type_Code],
		[Claimed_Payment_Type],
		[Eligibility],
		[Payment_Type_Result],
		[Patient_Type],
		[Create_Dtm]
	FROM 
		[HAServicePatient] 
	WHERE
		--[Doc_Code] = @In_Doc_Code AND 
		[Encrypt_Field1] = EncryptByKey(KEY_GUID('sym_Key'), @In_Identity)
	ORDER BY
		[Serial_No]

EXEC [proc_SymmetricKey_close]

END
GO

GRANT EXECUTE ON [dbo].[proc_HAServicePatient_get_byDocCodeDocID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_HAServicePatient_get_byDocCodeDocID] TO HCVU
GO
