IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SPDataMigration_get_IVSS]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SPDataMigration_get_IVSS]
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
-- Description:	Search the SP Migration record from IVSS
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Clark YIP
-- Modified date: 03 Sep 2009
-- Description:	  Filter out the case that is not in data entry
-- =============================================
CREATE PROCEDURE [dbo].[proc_SPDataMigration_get_IVSS]
	@hk_id as char(9)
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
declare @reccount as smallint
--declare @reccount2 as smallint
--declare @reccount3 as smallint

DECLARE @tmp_data table ( Enrolment_Ref_No char(15),						
						SP_ID char(8),						
						Encrypt_Field1 varbinary(100),
						Encrypt_Field2 varbinary(100),
						Encrypt_Field3 varbinary(100))
						--Migration_Case	char(1),
						--EVS_Enrolment_Ref_No char(15))
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
		
	Select @reccount = count(1) from SPMigration_IVSS
	where Encrypt_field1 =EncryptByKey(KEY_GUID('sym_Key'), @HK_ID) and record_status='P'
	--Check for IVSS user
	if @reccount = 0	--The IVSS record has NOT been migrated or HKID does not exists	
	Begin	
		SELECT	@reccount = COUNT(1)
		FROM	ServiceProviderStaging SP
					INNER JOIN SPAccountUpdate AU
						ON SP.Enrolment_Ref_No = AU.Enrolment_Ref_No
		WHERE	SP.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @hk_id)
		
		IF @reccount = 0 BEGIN
			--HKIC exists in dbIVSS staging
			insert into @tmp_data (Enrolment_ref_no, sp_id, Encrypt_field1, encrypt_field2, encrypt_field3)
			Select sp.Enrolment_Ref_No, sp.SP_ID, 
			m.Encrypt_Field1,
			sp.Encrypt_Field2,
			sp.Encrypt_Field3
			--'3' as Migration_Case			
			from [dbIVSS].[dbo].[ServiceProviderStaging] sp, [dbIVSS].[dbo].[SPMigration] m
			Where sp.encrypt_field1 = m.encrypt_field1 and m.record_status='R' and sp.encrypt_field1=EncryptByKey(KEY_GUID('sym_Key'), @HK_ID)
		END
	End	
	
	SELECT	
		Enrolment_Ref_No, 
		isNULL(SP_ID, '') as SP_ID, 
		convert(varchar, DecryptByKey(Encrypt_Field1)) as SP_HKID,
		convert(varchar(40), DecryptByKey(Encrypt_Field2)) as SP_Eng_Name,
		isNULL(convert(nvarchar, DecryptByKey(Encrypt_Field3)), '') as SP_Chi_Name
		--Migration_Case,
		--isNULL(EVS_Enrolment_Ref_No, '') as EVS_Enrolment_Ref_No
	From @tmp_data
		

	EXEC [proc_SymmetricKey_close]

END

GO

GRANT EXECUTE ON [dbo].[proc_SPDataMigration_get_IVSS] TO HCVU
GO
