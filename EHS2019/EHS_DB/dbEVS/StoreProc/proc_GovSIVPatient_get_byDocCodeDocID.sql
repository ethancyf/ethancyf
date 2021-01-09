IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_GovSIVPatient_get_byDocCodeDocID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_GovSIVPatient_get_byDocCodeDocID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO 

-- =============================================
-- Modification History
-- CR No.:			
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- CR No.:		CRE20-0XX (Gov SIV 2020/21)
-- Author:		Chris YIM
-- Create date: 28 Oct 2020
-- Description:	get GovSIVPatient by Doc Code, Doc ID
-- =============================================

CREATE PROCEDURE proc_GovSIVPatient_get_byDocCodeDocID
	@SP_ID		CHAR(8),
	@Doc_Code	VARCHAR(20),
	@Identity	VARCHAR(20)
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

	DECLARE @In_SP_ID		CHAR(8)
	DECLARE @In_Doc_Code	VARCHAR(20)
	DECLARE @In_Identity	VARCHAR(20)

	SET @In_SP_ID = @SP_ID
	SET @In_Doc_Code = @Doc_Code
	SET @In_Identity = @identity

-- =============================================
-- Return results
-- =============================================
EXEC [proc_SymmetricKey_open]
	
	SELECT
		[SP_ID],
		[Doc_Code],
		CONVERT(VARCHAR(MAX), DecryptByKey([Encrypt_Field1])) as [IdentityNum],
		[Create_Dtm]
	FROM 
		[GovSIVPatient] 
	WHERE
		[Doc_Code] = @In_Doc_Code 
		AND [Encrypt_Field1] = EncryptByKey(KEY_GUID('sym_Key'), @In_Identity)
		AND [SP_ID] = @SP_ID
		
EXEC [proc_SymmetricKey_close]

END
GO

GRANT EXECUTE ON [dbo].[proc_GovSIVPatient_get_byDocCodeDocID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_GovSIVPatient_get_byDocCodeDocID] TO HCVU
GO

