IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_MedicalOrganizationStaging_get_byERN_FromIVSS]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_MedicalOrganizationStaging_get_byERN_FromIVSS]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Clark YIP
-- Create date: 09 Jul 2009
-- Description:	Retrieve the Medical Organization from Table
--				MOTransition from IVSS
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_MedicalOrganizationStaging_get_byERN_FromIVSS] 
	@enrolment_ref_no	char(15),
	@user_id	varchar(20)
AS
BEGIN

	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
DECLARE	@record_id int,
		@address_eng varchar(255),
		@address_chi nvarchar(255),
		@district_code char(5),
		@eh_eng varchar(255),
		@eh_chi varchar(255)

declare @display_seq smallint
		
DECLARE @tmp_MO table ( Enrolment_Ref_No char(15),
						SP_ID	char(8),
						Display_Seq	smallint,
						MO_Eng_Name	varchar(100),
						MO_Chi_Name	nvarchar(100),
						Room	nvarchar(5),
						[Floor]	nvarchar(3),
						Block	nvarchar(3),
						Building	varchar(100),
						Building_Chi	nvarchar(100),
						District	char(4),
						Address_Code	int,
						BR_Code	varchar(50),
						Phone_Daytime	varchar(20),
						Email	varchar(255),
						Fax	varchar(20),
						Relationship char(5),
						Relationship_Remark nvarchar(255),
						create_by varchar(20),
						create_dtm datetime,
						update_by varchar(20),
						update_dtm datetime,
						Record_Status char(1),
						tsmp timestamp)
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	INSERT INTO @tmp_MO (Enrolment_Ref_No,
						SP_ID,
						Display_Seq,
						MO_Eng_Name,
						MO_Chi_Name,
						Room,
						[Floor],
						Block,
						Building,
						Building_Chi,
						District,
						Address_Code,
						BR_Code,
						Phone_Daytime,
						Email,
						Fax,
						Relationship,
						Relationship_Remark,
						create_by,
						create_dtm,
						update_by,						
						update_dtm,
						Record_Status)

	Select		@enrolment_ref_no as Enrolment_Ref_No,
				null as SP_ID,			
				Display_Seq,				
				MO_Eng_Name,
				MO_Chi_Name,
				Room,
				[Floor],
				Block,
				Building,
				Building_Chi,
				District,
				Address_Code,
				BR_Code,
				Phone_Daytime,
				Email,
				Fax,
				Relationship,
				Relationship_Remark,
				@user_id,
				getdate(),
				@user_id,
				getdate(),
				Record_Status
	From [dbIVSS].[dbo].[MOTransition]
	Where encrypt_field1 = (Select Encrypt_field1 from [dbIVSS].[dbo].[ServiceProviderStaging] where Enrolment_ref_no= @enrolment_ref_no)
	
DECLARE avail_cursor cursor 
FOR	SELECT Address_Code, Display_Seq
FROM @tmp_MO

OPEN avail_cursor 
FETCH next FROM avail_cursor INTO @record_id, @display_seq
WHILE @@Fetch_status = 0
BEGIN
	if @record_id IS NOT null
	BEGIN
		SELECT	@address_eng = '',
				@address_chi = '',
				@district_code = '',
				@eh_eng = '',
				@eh_chi = ''

		exec cpi_get_address_detail   @record_id 
								, @address_eng = @address_eng  OUTPUT 
    							, @address_chi = @address_chi    OUTPUT 
								, @district_code = @district_code    OUTPUT 
								, @eh_eng = @eh_eng	OUTPUT
								, @eh_chi = @eh_chi	OUTPUT

	UPDATE @tmp_MO
	SET	Building = @address_eng, 
		Building_Chi = @address_chi,
		District = @district_code
	WHERE Display_Seq = @display_seq
	END

	FETCH next FROM avail_cursor INTO @record_id, @display_seq
END
CLOSE avail_cursor 
DEALLOCATE avail_cursor


	SELECT  t.enrolment_ref_no,
			t.SP_ID,
			t.Display_Seq,  
			t.MO_Eng_Name, 
			t.MO_Chi_Name, 
			t.Room, 
			t.[Floor],
			t.Block, 
			t.Building, 
			t.Building_Chi, 
			t.District, 
			t.Address_Code, 
			t.BR_Code, 
			t.Phone_Daytime, 
			isNull(t.Email,'') as Email, 
			t.Fax,
			t.Relationship,
			t.Relationship_Remark,
			t.create_by,
			t.create_dtm,
			t.update_by,
			t.update_dtm,
			t.Record_Status,
			m.tsmp
	FROM    @tmp_MO t, [dbIVSS].[dbo].[MOTransition] m
	WHERE	m.encrypt_field1 = (Select Encrypt_field1 from [dbIVSS].[dbo].[ServiceProviderStaging] where Enrolment_ref_no= @enrolment_ref_no)
	and		t.enrolment_ref_no = @enrolment_ref_no
	and		t.display_seq = m.display_seq
	
END
GO

GRANT EXECUTE ON [dbo].[proc_MedicalOrganizationStaging_get_byERN_FromIVSS] TO HCVU
GO
