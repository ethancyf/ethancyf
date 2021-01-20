IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SPMigration_upd_Status]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SPMigration_upd_Status]
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
-- Author:		Clark YIP
-- Create date: 10 June 2009
-- Description:	Update the Record_Status in
--				Table SPMigration
--				'N': Not Yet Migrated
--				'R': Ready to Migrate
--				'P': Processing (Migrating)
--				'D': Migrated
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Paul	
-- Modified date:	7 July 2009	
-- Description:		Add enrolment ref number
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Paul	
-- Modified date:	24 July 2009	
-- Description:		Add timestamp checking
-- =============================================
CREATE PROCEDURE [dbo].[proc_SPMigration_upd_Status]
	@HK_ID	char(9),
	@Record_Status char(1),
	@enrolment_ref_no	char(15),
	@tsmp timestamp	
AS
BEGIN
	
	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	EXEC [proc_SymmetricKey_open]

	IF (SELECT TSMP FROM SPMigration
		WHERE encrypt_field1 = encryptByKey(KEY_GUID('sym_Key'), @HK_ID)  ) != @tsmp
	BEGIN
		RAISERROR('00011', 16, 1)
		return @@error
	END
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================
	UPDATE	SPMigration
	Set		Record_Status = @Record_Status,
			Enrolment_Ref_No = @enrolment_ref_no			
	WHERE	EncryptByKey(KEY_GUID('sym_Key'), @HK_ID) = Encrypt_Field1

EXEC [proc_SymmetricKey_close]

END
GO

GRANT EXECUTE ON [dbo].[proc_SPMigration_upd_Status] TO HCVU
GO
