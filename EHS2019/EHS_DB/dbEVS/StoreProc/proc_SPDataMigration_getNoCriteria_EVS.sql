IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SPDataMigration_getNoCriteria_EVS]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SPDataMigration_getNoCriteria_EVS]
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
-- Author:		Lawrence TSANG
-- Create date: 17 July 2009
-- Description:	Search the SP Migration record in dbEVS
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	 Clark YIP
-- Modified date: 19 July 2009
-- Description:	  Handle the case with dbIVSS migration before
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	  Lawrence TSANG
-- Modified date: 30 July 2009
-- Description:	  Create a temp table to handle the select ordering: 01HCVS -> 02IVSS
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	  
-- Modified date: 
-- Description:	  
-- =============================================
CREATE PROCEDURE [dbo].[proc_SPDataMigration_getNoCriteria_EVS]
	@hk_id	char(9)
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
	DECLARE @TempMigration AS TABLE (
		SP_HKID				char(9),
		Record_Status		char(1),
		Print_Status		char(1),
		Enrolment_Ref_No	char(15),
		Source				char(10)
	)
-- =============================================
-- Return results
-- =============================================
	EXEC [proc_SymmetricKey_open]
	
	INSERT INTO @TempMigration 
	SELECT	CONVERT(varchar, DecryptByKey(Encrypt_Field1)) AS SP_HKID,
			Record_Status,
			Print_Status,
			Enrolment_Ref_No,
			'01HCVS'
	FROM	SPMigration
	WHERE	Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @hk_id)

	INSERT INTO @TempMigration 
	SELECT	CONVERT(varchar, DecryptByKey(Encrypt_Field1)) AS SP_HKID,
			Record_Status,
			Print_Status,
			Enrolment_Ref_No,
			'02IVSS'
	FROM	SPMigration_IVSS
	WHERE	Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @hk_id)
				AND Encrypt_Field1 IN (SELECT Encrypt_Field1 FROM ServiceProviderStaging UNION SELECT Encrypt_Field1 FROM ServiceProvider)

	SELECT		SP_HKID,
				Record_Status,
				Print_Status,
				Enrolment_Ref_No,
				Source	 
	FROM		@TempMigration
	ORDER BY	Source

	EXEC [proc_SymmetricKey_close]

END

GO

GRANT EXECUTE ON [dbo].[proc_SPDataMigration_getNoCriteria_EVS] TO HCVU
GO
