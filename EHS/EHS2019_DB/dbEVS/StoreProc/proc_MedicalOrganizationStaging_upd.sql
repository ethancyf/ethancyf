IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_MedicalOrganizationStaging_upd]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_MedicalOrganizationStaging_upd]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- CR No.:			CRE16-009 (To enable the eHS(S) to display Chinese Unicode in the field of "English Name of M.O.")
-- Modified by:		Koala CHENG
-- Modified date:	07 Sep 2016
-- Description:		Change MO_Eng_Name from VARCHAR(100) to NVARCHAR(100)
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 31 May 2009
-- Description:	Update the Medical Organization info in Staging
-- =============================================
CREATE PROCEDURE [dbo].[proc_MedicalOrganizationStaging_upd]
	@enrolment_ref_no char(15),
	@display_seq smallint, 
	@mo_eng_name nvarchar(100),
	@mo_chi_name nvarchar(100),
	@room nvarchar(5),
	@floor nvarchar(3), 
	@block nvarchar(3),
	@building varchar(100),
	@building_chi nvarchar(100), 
	@district char(4),
	@address_code int, 
	@br_code varchar(50),
	@phone_daytime varchar(20), 
	@email varchar(255),
	@fax varchar(20),
	@relationship char(5),
	@relationship_remark nvarchar(255),
	@update_by varchar(20),
	@record_status char(1),
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

	IF (
		SELECT TSMP FROM [dbo].[MedicalOrganizationStaging]
		WHERE Enrolment_Ref_No = @enrolment_ref_no AND Display_Seq = @Display_Seq
	) != @TSMP
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

	update medicalorganizationstaging
	set MO_Eng_Name = @mo_eng_name,
		MO_Chi_Name = @mo_chi_name,
		Room = @room,
		[Floor] = @floor,
		Block = @block,
		Building = @building,
		Building_Chi = @building_chi,
		District = @district,
		Address_Code = @address_code,
		BR_Code = @br_code,
		Phone_Daytime = @phone_daytime,
		Email = @email,
		Fax = @fax,
		Relationship = @relationship,
		Relationship_remark =@relationship_remark,
		record_status = @record_status,
		update_by =@update_by,		
		update_dtm = getdate()
	where enrolment_ref_no = @enrolment_ref_no
		and display_seq = @display_seq
END
GO

GRANT EXECUTE ON [dbo].[proc_MedicalOrganizationStaging_upd] TO HCVU
GO
