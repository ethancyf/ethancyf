IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SPDataMigration_get_EVS]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SPDataMigration_get_EVS]
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
-- Description:	Search the SP Migration record in dbEVS
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	 Clark YIP
-- Modified date: 14 Jul 2009
-- Description:	  filter out the ern criteria
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	 Paul YIP
-- Modified date: 27 Jul 2009
-- Description:	  Add Time Stamp
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	 Clark YIP
-- Modified date: 01 Sep 2009
-- Description:	  Filter out delistd sp 
-- =============================================
CREATE PROCEDURE [dbo].[proc_SPDataMigration_get_EVS]
	@hk_id as char(9)
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
declare @reccount as smallint
declare @reccount2 as smallint

DECLARE @tmp_data table ( Enrolment_Ref_No char(15),						
						SP_ID char(8),						
						Encrypt_Field1 varbinary(100),
						Encrypt_Field2 varbinary(100),
						Encrypt_Field3 varbinary(100),
						tsmp binary(8))
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
	
	--Check for HCVS
	Select @reccount = count(1) from SPMigration
	where Encrypt_field1 =EncryptByKey(KEY_GUID('sym_Key'), @HK_ID) and record_status='R'
	
	if @reccount > 0
	Begin
		Select @reccount2 = count(1) from ServiceProviderStaging sp, SPMigration m
		Where sp.encrypt_field1 = m.encrypt_field1 and m.record_status='R'  and sp.encrypt_field1=EncryptByKey(KEY_GUID('sym_Key'), @HK_ID)
		--Where sp.encrypt_field1 = m.encrypt_field1 and sp.Enrolment_ref_no = m.enrolment_ref_no and m.record_status='R'  and sp.encrypt_field1=EncryptByKey(KEY_GUID('sym_Key'), @HK_ID)
		
		if @reccount2 > 0
			Begin
				--dbEVS user in staging
				insert into @tmp_data (Enrolment_ref_no, sp_id, Encrypt_field1, encrypt_field2, encrypt_field3, tsmp)
				Select sp.Enrolment_Ref_No, sp.SP_ID, 
				sp.Encrypt_Field1,
				sp.Encrypt_Field2,
				sp.Encrypt_Field3,
				m.tsmp				
				from ServiceProviderStaging sp, SPMigration m
				Where sp.encrypt_field1 = m.encrypt_field1 and m.record_status='R' and sp.encrypt_field1=EncryptByKey(KEY_GUID('sym_Key'), @HK_ID)
					AND sp.record_status not in ('D','I','V')
				--Where sp.encrypt_field1 = m.encrypt_field1 and sp.Enrolment_ref_no = m.enrolment_ref_no and m.record_status='R' and sp.encrypt_field1=EncryptByKey(KEY_GUID('sym_Key'), @HK_ID)
			End
		Else
		Begin
			--dbEVS user in permanent
			insert into @tmp_data (Enrolment_ref_no, sp_id, Encrypt_field1, encrypt_field2, encrypt_field3, tsmp)
			Select sp.Enrolment_Ref_No, sp.SP_ID, 
			sp.Encrypt_Field1,
			sp.Encrypt_Field2,
			sp.Encrypt_Field3,
			m.tsmp			
			from ServiceProvider sp, SPMigration m
			Where sp.encrypt_field1 = m.encrypt_field1 and m.record_status='R' and sp.encrypt_field1=EncryptByKey(KEY_GUID('sym_Key'), @HK_ID)
					AND sp.record_status<>'D'
			--Where sp.encrypt_field1 = m.encrypt_field1 and sp.Enrolment_ref_no = m.enrolment_ref_no and m.record_status='R' and sp.encrypt_field1=EncryptByKey(KEY_GUID('sym_Key'), @HK_ID)
		End	
	End
		
	SELECT	
		Enrolment_Ref_No, 
		isNULL(SP_ID, '') as SP_ID, 
		convert(varchar, DecryptByKey(Encrypt_Field1)) as SP_HKID,
		convert(varchar(40), DecryptByKey(Encrypt_Field2)) as SP_Eng_Name,
		isNULL(convert(nvarchar, DecryptByKey(Encrypt_Field3)), '') as SP_Chi_Name,
		tsmp as TSMP
		--Migration_Case,
		--isNULL(EVS_Enrolment_Ref_No, '') as EVS_Enrolment_Ref_No
	From @tmp_data
		

EXEC [proc_SymmetricKey_close]

END

GO

GRANT EXECUTE ON [dbo].[proc_SPDataMigration_get_EVS] TO HCVU
GO
