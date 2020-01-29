IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_MedicalOrganizationEnrolment_get_byERN]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_MedicalOrganizationEnrolment_get_byERN]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Tommy Cheung
-- Create date: 14 August 2008
-- Description:	Retrieve the Medical Organization from Table
--				MedicalOrganizationEnrolment
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Kathy LEE
-- Modified date: 30 June 2009
-- Description:	 1. Remove "SP_Practice_Display_Seq"
--				 2. Add "Relationship", "Relationship_Remark"
-- =============================================

CREATE PROCEDURE [dbo].[proc_MedicalOrganizationEnrolment_get_byERN] 
	@enrolment_ref_no	char(15)
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
		
DECLARE @tmp_MO table ( Enrolment_Ref_No	char(15),
						Display_Seq	smallint,
						--SP_Practice_Display_Seq	smallint,
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
						Relationship_Remark nvarchar(255))
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

	SELECT  Enrolment_Ref_No,
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
			Relationship_Remark
	FROM    MedicalOrganizationEnrolment
	WHERE	Enrolment_Ref_No = @enrolment_ref_no
	
	SELECT @record_id =  Address_Code
FROM	@tmp_MO
WHERE Enrolment_Ref_No = @enrolment_ref_no

if @record_id IS NOT NULL
BEGIN
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
END

	SELECT  Enrolment_Ref_No,
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
			isNull(Email,'') as Email, 
			Fax,
			Relationship,
			isnull(Relationship_Remark,'') as Relationship_Remark
	FROM    @tmp_MO
	WHERE	Enrolment_Ref_No = @enrolment_ref_no
	
END
GO

GRANT EXECUTE ON [dbo].[proc_MedicalOrganizationEnrolment_get_byERN] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_MedicalOrganizationEnrolment_get_byERN] TO HCVU
GO
