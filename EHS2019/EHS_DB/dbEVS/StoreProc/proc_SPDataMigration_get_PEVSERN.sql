IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SPDataMigration_get_PEVSERN]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SPDataMigration_get_PEVSERN]
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
-- Create date: 07 Jul 2009
-- Description:	Search the ERN of dbEVS record which is migrated
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_SPDataMigration_get_PEVSERN]
	@hk_id as char(9)
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
declare @reccount as smallint
declare @reccount2 as smallint

DECLARE @tmp_data table ( Enrolment_Ref_No char(15))
						
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
	
	--Check for HCVS
	
	insert into @tmp_data (Enrolment_ref_no)
	Select enrolment_ref_no from SPMigration
	where Encrypt_field1 =EncryptByKey(KEY_GUID('sym_Key'), @HK_ID) and record_status <>'R'
	
	SELECT	
		Enrolment_Ref_No		
	From @tmp_data
		

	EXEC [proc_SymmetricKey_close]
END

GO

GRANT EXECUTE ON [dbo].[proc_SPDataMigration_get_PEVSERN] TO HCVU
GO
