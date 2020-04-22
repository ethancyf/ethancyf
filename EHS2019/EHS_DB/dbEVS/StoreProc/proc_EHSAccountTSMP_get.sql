IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHSAccountTSMP_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHSAccountTSMP_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:	Karl LAM
-- Modified date:	15 Aug 2013
-- Description:	Grant execute permission to HCVU
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 09 Oct 2009
-- Description:	Get EHSAccountTSMP while Adding Transaction
-- =============================================


CREATE PROCEDURE [proc_EHSAccountTSMP_get]
	@Doc_Code char(20),
	@identity varchar(20)
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key
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
		@Doc_Code, [Doc_Code], [TSMP], [Update_Dtm], [Update_By]
	FROM
		[EHSAccountTSMP]
	WHERE
		[Doc_Code] = @Doc_Code AND Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @identity) 	
	
CLOSE SYMMETRIC KEY sym_Key
		
END
GO

GRANT EXECUTE ON [dbo].[proc_EHSAccountTSMP_get] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_EHSAccountTSMP_get] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_EHSAccountTSMP_get] TO WSEXT
GO