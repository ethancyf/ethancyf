IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_MedicalOrganizationEnrolment_get_byERNList]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_MedicalOrganizationEnrolment_get_byERNList]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Kathy LEE
-- Create date: 26 June 2009
-- Description:	Retrieve the Medical Organization from Table
--				MedicalOrganizationEnrolment for e-Form Print form screen
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_MedicalOrganizationEnrolment_get_byERNList]
	@enrolment_ref_no	char(8000)
AS
BEGIN

	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
DECLARE @TempStr VARCHAR(15)
DECLARE @TempTable table (temp_enrolment_ref_no char(15))
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================

WHILE LEN(@enrolment_ref_no) > 0
 BEGIN
  SET @TempStr = LEFT(@enrolment_ref_no, ISNULL(NULLIF(CHARINDEX(',', @enrolment_ref_no) - 1, -1),
                    LEN(@enrolment_ref_no)))
  SET @enrolment_ref_no = SUBSTRING(@enrolment_ref_no,ISNULL(NULLIF(CHARINDEX(',', @enrolment_ref_no), 0),
                               LEN(@enrolment_ref_no)) + 1, LEN(@enrolment_ref_no))
  INSERT INTO @TempTable VALUES (@TempStr)
 END
 
-- =============================================
-- Return results
-- =============================================

select s.Enrolment_Ref_No,
		s.enrolment_dtm,
		m.MO_Eng_Name,
		isnull(m.MO_Chi_Name,'') as MO_Chi_Name,
		p.service_category_code
From  serviceproviderenrolment s, MedicalOrganizationEnrolment m, professionalenrolment p
where s.enrolment_ref_no = m.enrolment_ref_no 
and m.enrolment_ref_no = p.enrolment_ref_no
and m.Enrolment_Ref_No in (select temp_enrolment_ref_no from @TempTable)
	
END
GO

GRANT EXECUTE ON [dbo].[proc_MedicalOrganizationEnrolment_get_byERNList] TO HCPUBLIC
GO
