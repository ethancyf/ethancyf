IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_MedicalOrganizationStaging_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_MedicalOrganizationStaging_add]
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
-- Author:		Clark YIP
-- Create date: 07 May 2009
-- Description:	Insert the Medical Organization Staging Table
-- =============================================

CREATE PROCEDURE [dbo].[proc_MedicalOrganizationStaging_add]
	@enrolment_ref_no char(15), 
	@sp_id char(8),
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
	@record_status char(1),
	@create_by varchar(20),
	@update_by varchar(20)

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

	INSERT INTO MedicalOrganizationStaging
				(Enrolment_Ref_No,
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
				Relationship_remark,
				record_status,
				create_by,
				create_dtm,
				update_by,
				update_dtm)
	VALUES		(@enrolment_ref_no, 
				@sp_id,
				@display_seq, 
				@mo_eng_name,
				@mo_chi_name,
				@room,
				@floor, 
				@block,
				@building,
				@building_chi, 
				@district,
				@address_code, 
				@br_code,
				@phone_daytime, 
				@email,
				@fax,
				@relationship,
				@relationship_remark,
				@record_status,
				@create_by,
				getdate(),
				@update_by,
				getdate())
END
GO

GRANT EXECUTE ON [dbo].[proc_MedicalOrganizationStaging_add] TO HCVU
GO
