IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SPMigration_get_byKey]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SPMigration_get_byKey]
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
-- Description:	Get the Record_Status in
--				Table SPMigration
--				'N': Not Yet Migrated
--				'R': Ready to Migrate
--				'P': Processing (Migrating)
--				'D': Migrated
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Paul Yip
-- Modified date: 24 July 2009
-- Description:	  Add TSMP
-- =============================================
CREATE PROCEDURE [dbo].[proc_SPMigration_get_byKey]
	@SP_ID  char(8),
	@HK_ID	char(9),
	@ERN	char(15)	
AS
BEGIN
	
	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
declare @rowcount smallint
declare @encrypt_field1 varbinary(100)
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================
EXEC [proc_SymmetricKey_open]

IF (@SP_ID <> '') OR (@ERN <> '')
BEGIN

	Select @rowcount = count(1) from ServiceProviderStaging where sp_id=@SP_ID or Enrolment_Ref_No=@ERN
	Select @encrypt_field1 = Encrypt_Field1 from ServiceProviderStaging where sp_id=@SP_ID or Enrolment_Ref_No=@ERN
	if @rowcount = 0
	Begin
		Select @encrypt_field1 = Encrypt_Field1 from ServiceProvider where sp_id=@SP_ID or Enrolment_Ref_No=@ERN
	End
	
	Select	Record_Status
			,TSMP
	From	SPMigration	
	WHERE	Encrypt_Field1=@encrypt_field1
END

ELSE
BEGIN
	Select	Record_Status
			,TSMP
	From	SPMigration	
	WHERE	EncryptByKey(KEY_GUID('sym_Key'), @HK_ID) = Encrypt_Field1	
END
END
EXEC [proc_SymmetricKey_close]
GO

GRANT EXECUTE ON [dbo].[proc_SPMigration_get_byKey] TO HCVU
GO
