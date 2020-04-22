IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SPDataMigration_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SPDataMigration_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Clark Yip
-- Create date: 23 June 2009
-- Description:	Retrieve the SP record for data migration
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Paul Yip
-- Modified date: 
-- Description:	  
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Clark Yip
-- Modified date: 29 Jul 2009
-- Description:	  1. Check practice staging table delist case with status D
--				  2. Filter out those new created sp with complete model (no need to do migration anymore)	
--				  3. Allow those P record without complete model to be searched
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Clark Yip
-- Modified date: 31 Aug 2009
-- Description:	  1. Filter out the "complete model" checking if the practice record_status is D
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Clark Yip
-- Modified date: 02 Sep 2009
-- Description:	  1. Filter out the "complete model" checking in staging & permanent (Only all practice complete will count as complete)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Clark Yip
-- Modified date: 17 Sep 2009
-- Description:	  1. Filter out the "complete model" checking in staging & permanent (Only all practice complete will count as complete and not New Add in staging)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Clark Yip
-- Modified date: 18 Sep 2009
-- Description:	  1. For those SP_ID IS NULL AND MO_Display_Seq = 0 will also count as not complete model
-- =============================================

CREATE PROCEDURE [dbo].[proc_SPDataMigration_get] 
	@record_status	char(1)
	,@enrolment_ref_no	Char(15)	
	,@sp_id	      	char(8)
	,@sp_hkid		Char(9)

AS
BEGIN
-- =============================================
-- Declaration
-- =============================================
Declare @rowcount_staging int
Declare @rowcount int
DECLARE @maxrow int
DECLARE	@ern char(15)

DECLARE @tmp_data_migration table ( enrolment_ref_no char(15),
						SP_Eng_Name varchar(40),
						SP_Chi_Name nvarchar(40),						
						Migration_Status char(1),						
						SP_ID char(8),
						SP_HKIC char(9),						
						Phone_Daytime varchar(20))
						
DECLARE @tmp_data_migration_staging table ( enrolment_ref_no char(15),
						SP_Eng_Name varchar(40),
						SP_Chi_Name nvarchar(40),						
						Migration_Status char(1),						
						SP_ID char(8),
						SP_HKIC char(9),						
						Phone_Daytime varchar(20))

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key
-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT	@maxrow = Parm_Value1
	FROM	[dbo].[SystemParameters]
	WHERE	[Parameter_name] = 'MaxRowRetrieve' and [Record_Status] = 'A'
	
-- =============================================
-- Return results
-- =============================================	

insert into @tmp_data_migration_staging
						(enrolment_ref_no,
						SP_ID,
						SP_HKIC,
						SP_Eng_Name,
						SP_Chi_Name,
						Phone_Daytime,
						Migration_Status
						)
				select sp.enrolment_ref_no, sp.sp_id, convert(varchar, DecryptByKey(sp.[Encrypt_Field1])), 
			convert(varchar(40), DecryptByKey(sp.[Encrypt_Field2])), convert(nvarchar(40), DecryptByKey(sp.[Encrypt_Field3])), 
			sp.phone_daytime, isnull(m.record_status,'N') from	practicestaging p, serviceproviderstaging sp 
			left outer join spmigration m on m.encrypt_field1=sp.encrypt_field1
			Where	p.enrolment_ref_no=sp.enrolment_ref_no and
					((@record_status='R' AND (m.record_status='R' OR m.record_status = 'P')) OR (@record_status='N' AND (m.record_status is null OR m.record_status = @record_status)) OR (@record_status='' AND (m.record_status in ('N','R','P') or m.record_status is null)))
					AND ((LTRIM(RTRIM(@enrolment_ref_no)) <> '' AND sp.Enrolment_ref_no = @enrolment_ref_no) OR (LTRIM(RTRIM(@enrolment_ref_no)) = ''))
					AND ((LTRIM(RTRIM(@sp_id)) <> '' AND sp.sp_id = @sp_id) OR (LTRIM(RTRIM(@sp_id)) = ''))		
					AND ((LTRIM(RTRIM(@sp_hkid)) <> '' AND convert(varchar, DecryptByKey(sp.[Encrypt_Field1])) = @sp_hkid) OR (LTRIM(RTRIM(@sp_hkid)) = ''))
					and p.Record_Status not in ('D','I','V')
				and sp.enrolment_ref_no is not null and sp.enrolment_ref_no not in
				(
					--Get those partially completed SP (In new enrolment, SPStaging is created but PracticeStaging is not created)
					Select enrolment_ref_no from ServiceProviderStaging Where enrolment_ref_no not in 
					(
						select Distinct enrolment_ref_no from PracticeStaging --Where Record_Status not in ('D','I','V')
					)
					UNION
					--Get those Practice WITH Complete Model
					(					
						Select Distinct enrolment_ref_no 
							from PracticeStaging 
							where Enrolment_Ref_No not in 
							(
								--Get those Practice WITHOUT Complete Model
								SELECT DISTINCT	Enrolment_Ref_No
									FROM			PracticeStaging
									WHERE			
									(
										SP_ID IS NOT NULL AND MO_Display_Seq = 0 AND Record_Status <>'A'
									)
									OR
									(
										SP_ID IS NULL AND MO_Display_Seq = 0 --AND Record_Status='A'
									)
							)
							
					)		
				)
				Group by sp.enrolment_ref_no, sp.sp_id, sp.encrypt_field1, sp.encrypt_field2, sp.encrypt_field3, sp.phone_daytime, m.record_status
						
		insert into @tmp_data_migration
					(enrolment_ref_no,
					SP_ID,
					SP_HKIC,
					SP_Eng_Name,
					SP_Chi_Name,
					Phone_Daytime,
					Migration_Status
					)
			select sp.enrolment_ref_no, sp.sp_id, convert(varchar, DecryptByKey(sp.[Encrypt_Field1])), convert(varchar(40), DecryptByKey(sp.[Encrypt_Field2])), convert(nvarchar(40), DecryptByKey(sp.[Encrypt_Field3])), sp.phone_daytime, isnull(m.record_status,'N') from
				practice p,serviceprovider sp
				left outer join spmigration m on m.encrypt_field1=sp.encrypt_field1
				where 
					p.sp_id=sp.sp_id and
					--((@record_status<>'' AND m.record_status = @record_status) OR (@record_status='' AND (m.record_status in ('R','N') or m.record_status is null)))
					((@record_status='R' AND (m.record_status='R' OR m.record_status = 'P')) OR (@record_status='N' AND (m.record_status is null OR m.record_status = @record_status)) OR (@record_status='' AND (m.record_status in ('N','R','P') or m.record_status is null)))
				AND ((LTRIM(RTRIM(@enrolment_ref_no)) <> '' AND sp.Enrolment_ref_no = @enrolment_ref_no) OR (LTRIM(RTRIM(@enrolment_ref_no)) = ''))
				AND ((LTRIM(RTRIM(@sp_id)) <> '' AND sp.sp_id = @sp_id) OR (LTRIM(RTRIM(@sp_id)) = ''))		
				AND ((LTRIM(RTRIM(@sp_hkid)) <> '' AND convert(varchar, DecryptByKey(sp.[Encrypt_Field1])) = @sp_hkid) OR (LTRIM(RTRIM(@sp_hkid)) = ''))
				AND sp.enrolment_ref_no not in (Select enrolment_ref_no from @tmp_data_migration_staging)
				AND sp.enrolment_ref_no not in (Select enrolment_ref_no from ServiceProviderStaging)
				and sp.record_status <> 'D' 
				and p.record_status <> 'D'
				and sp.sp_id not in
				(
				Select sp_id from ServiceProvider Where sp_id not in 
				(
					select Distinct sp_id from Practice --Where Record_Status not in ('D')
				)
				UNION
					--Select Distinct sp_id from Practice where mo_display_seq>0	and record_status<>'D'
					Select Distinct sp_id
						from Practice
						where SP_ID not in (
							SELECT DISTINCT	SP_ID
							FROM			Practice
							WHERE			MO_Display_Seq = 0
						)
						AND record_status<>'D'
				)
				Group by sp.enrolment_ref_no, sp.sp_id, sp.encrypt_field1, sp.encrypt_field2, sp.encrypt_field3, sp.phone_daytime, m.record_status
		
		SELECT	@rowcount_staging = count(1)
		FROM	@tmp_data_migration_staging
		
		SELECT	@rowcount = count(1)
		FROM	@tmp_data_migration
					
		IF @rowcount+@rowcount_staging > @maxrow
		BEGIN
			Raiserror('00009', 16, 1)
			return @@error
		END		

	Update @tmp_data_migration set Migration_Status='N' where Migration_Status='P'
	
	Update @tmp_data_migration_staging set Migration_Status='N' where Migration_Status='P'

	SELECT	enrolment_ref_no,
			SP_Eng_Name,
			SP_Chi_Name,
			Migration_Status,						
			SP_ID,
			SP_HKIC,
			Phone_Daytime
	FROM	@tmp_data_migration 
	Where sp_id in (select distinct sp_id from Practice where record_status<>'D')
	UNION
	SELECT	enrolment_ref_no,
			SP_Eng_Name,
			SP_Chi_Name,
			Migration_Status,						
			SP_ID,
			SP_HKIC,
			Phone_Daytime
	FROM	@tmp_data_migration_staging
	Where enrolment_ref_no in (select distinct enrolment_ref_no from PracticeStaging where record_status not in ('D','I','V'))
	
	CLOSE SYMMETRIC KEY sym_Key	
END
GO

GRANT EXECUTE ON [dbo].[proc_SPDataMigration_get] TO HCVU
GO
