IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HAServicePatientGroup_get_byDocID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HAServicePatientGroup_get_byDocID]
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
-- CR No.:		CRE21-019 (HA Scheme)
-- Author:		Winnie SUEN
-- Create date: 26 Oct 2021
-- Description:	get HAServicePatientGroup by Doc ID
-- =============================================

CREATE PROCEDURE proc_HAServicePatientGroup_get_byDocID
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

	DECLARE @In_Identity VARCHAR(20)
	SET @In_Identity = @identity

-- =============================================
-- Return results
-- =============================================
EXEC [proc_SymmetricKey_open]
	
	SELECT
		[Doc_Code],
		CONVERT(VARCHAR, DecryptByKey([Encrypt_Field1])) as [IdentityNum],
		[Group_No],
		[Cutoff_Dtm],
		[Create_Dtm]
	FROM 
		[HAServicePatientGroup] WITH (NOLOCK)
	WHERE
		[Encrypt_Field1] = EncryptByKey(KEY_GUID('sym_Key'), @In_Identity)
	ORDER BY
		[Group_No]

EXEC [proc_SymmetricKey_close]

END
GO

GRANT EXECUTE ON [dbo].[proc_HAServicePatientGroup_get_byDocID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_HAServicePatientGroup_get_byDocID] TO HCVU
GO
