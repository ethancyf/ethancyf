IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SPMigration_IVSS_add_FromIVSS]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SPMigration_IVSS_add_FromIVSS]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Clark YIP
-- Create date: 10 Jul 2009
-- Description:	Insert the SPMigration_IVSS Table
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	 Clark YIP
-- Modified date: 14 Jul 2009
-- Description:	  Change the input field from ERN to HKID, and no ern from IVSS table
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	4 September 2009
-- Description:		Check if the [Encrypt_Field1] exists in [SPMigration_IVSS], reject the insert
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_SPMigration_IVSS_add_FromIVSS]	
	@hk_id char(9),
	@record_status char(1)
	

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

	IF (SELECT COUNT(1) FROM [SPMigration_IVSS] WHERE Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @HK_ID)) = 0 BEGIN

		--SPMigration_IVSS
		Insert into [SPMigration_IVSS]
		(
			[Encrypt_Field1],	
			[Enrolment_Ref_No],		
			[Record_Status],
			[Print_Status]
		)
		Select
			[Encrypt_Field1],	
			null,		
			@record_status,	--'P'
			[Print_Status]		
		From [dbIVSS].[dbo].[SPMigration]
		Where encrypt_field1 = EncryptByKey(KEY_GUID('sym_Key'), @HK_ID)
	
	END
				
CLOSE SYMMETRIC KEY sym_Key
				
END
GO

GRANT EXECUTE ON [dbo].[proc_SPMigration_IVSS_add_FromIVSS] TO HCVU
GO
