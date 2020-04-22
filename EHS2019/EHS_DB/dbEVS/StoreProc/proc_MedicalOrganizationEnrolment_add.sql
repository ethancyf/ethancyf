IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_MedicalOrganizationEnrolment_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_MedicalOrganizationEnrolment_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Tommy Cheung
-- Create date: 14 August 2008
-- Description:	Insert the Medical Organization to Table
--				MedicalOrganizationEnrolment
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Kathy LEE
-- Modified date: 04 April 2009
-- Description:	Insert 2 information in Medical Organization
--				'MO Relation' and 'MO Relation Remark'
--				Remove inserting 'SP_Practice_Display_Seq'
-- =============================================

CREATE PROCEDURE [dbo].[proc_MedicalOrganizationEnrolment_add]
	@enrolment_ref_no char(15),	
	@display_seq smallint, 
	--@sp_practice_display_seq smallint,
	@mo_eng_name varchar(100),
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
	@relationship char(1),
	@relationship_remark nvarchar(255)

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

	INSERT INTO MedicalOrganizationEnrolment
				(Enrolment_Ref_No,				
				Display_Seq,
				--SP_Practice_Display_Seq,
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
				Relationship_Remark)
	VALUES		(@enrolment_ref_no,				
				@display_seq, 
				--@sp_practice_display_seq,
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
				@relationship_remark)
END
GO

GRANT EXECUTE ON [dbo].[proc_MedicalOrganizationEnrolment_add] TO HCPUBLIC
GO
