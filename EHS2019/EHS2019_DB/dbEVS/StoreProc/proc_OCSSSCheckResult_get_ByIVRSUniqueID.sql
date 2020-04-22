IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_OCSSSCheckResult_get_ByIVRSUniqueID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_OCSSSCheckResult_get_ByIVRSUniqueID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.:			
-- Description:		
-- =============================================
-- =============================================
-- Author:			Chris YIM
-- Create date:		27 Sep 2018
-- CR No.:			CRE17-010-02 (OCSSS integration - IVRS)
-- Description:		Get OCSSS Checking Result By IVRS Unique ID
-- =============================================

CREATE PROCEDURE [dbo].[proc_OCSSSCheckResult_get_ByIVRSUniqueID]
	@IdentityNum CHAR(9),
	@HKIC_Symbol CHAR(1),
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

	SELECT TOP 1
		[System_Dtm],
		[OCSSS_Ref_Status]
	FROM 
		[OCSSSCheckResult] WITH (NOLOCK)
	WHERE
		[Encrypt_Field1] = EncryptByKey(KEY_GUID('sym_Key'), @IdentityNum)
		AND [HKIC_Symbol] = @HKIC_Symbol
		AND [IVRS_Unique_ID] = EncryptByKey(KEY_GUID('sym_Key'), @IVRS_Unique_ID)
	ORDER BY
		[System_Dtm] DESC

	CLOSE SYMMETRIC KEY sym_Key
	
END

GO

GRANT EXECUTE ON [proc_OCSSSCheckResult_get_ByIVRSUniqueID] TO HCSP
GO

