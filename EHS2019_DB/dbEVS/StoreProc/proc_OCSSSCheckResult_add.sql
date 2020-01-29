IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_OCSSSCheckResult_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_OCSSSCheckResult_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	27 Sep 2018
-- CR No.:			CRE17-010-02 (OCSSS integration - IVRS)
-- Description:		Add IVRS Unique ID
-- =============================================
-- =============================================
-- Author:			Chris YIM
-- Create date:		14 May 2018
-- CR No.:			CRE17-010 (OCSSS integration)
-- Description:		Insert OCSSS Checking Result
-- =============================================

CREATE PROCEDURE [dbo].[proc_OCSSSCheckResult_add]
	@IdentityNum CHAR(9),
	@Scheme_Code CHAR(10),
	@SP_ID CHAR(8),
	@HKIC_Symbol CHAR(1),
	@OCSSS_Ref_Status CHAR(1),
	@IVRS_Unique_ID VARCHAR(40)
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
	-- =============================================
	-- Return results
	-- =============================================
	OPEN SYMMETRIC KEY sym_Key
	DECRYPTION BY ASYMMETRIC KEY asym_Key

	INSERT INTO [OCSSSCheckResult](
		System_Dtm,
		Encrypt_Field1,
		Scheme_Code,
		SP_ID,
		HKIC_Symbol,
		OCSSS_Ref_Status,
		IVRS_Unique_ID
	)
	VALUES(
		GETDATE(),
		EncryptByKey(KEY_GUID('sym_Key'), @IdentityNum),
		@Scheme_Code,
		@SP_ID,
		@HKIC_Symbol,
		@OCSSS_Ref_Status,
		EncryptByKey(KEY_GUID('sym_Key'), @IVRS_Unique_ID)
	)

	CLOSE SYMMETRIC KEY sym_Key
	
END

GO

GRANT EXECUTE ON [proc_OCSSSCheckResult_add] TO HCSP
GO

