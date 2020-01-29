IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_PracticeSchemeInfoStaging_after_upd')
	DROP TRIGGER [dbo].[tri_PracticeSchemeInfoStaging_after_upd]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE16-002
-- Modified by:		Lawrence TSANG
-- Modified date:	15 August 2016
-- Description:		Add Clinic_Type
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE15-004
-- Modified by:		Winnie SUEN
-- Modified date:	25 June 2015
-- Description:		Add field Provide_Service
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 29 July 2009
-- Description:	Trigger an insert statment into PracticeSchemeInfoStagingLOG
--				when a row is updated / inserted into PracticeSchemeInfoStaging
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:
-- =============================================
CREATE TRIGGER [dbo].[tri_PracticeSchemeInfoStaging_after_upd]
   ON  [dbo].[PracticeSchemeInfoStaging]
   AFTER INSERT,UPDATE
AS 
BEGIN
	
	SET NOCOUNT ON;

	INSERT INTO PracticeSchemeInfoStagingLOG
				(System_Dtm,
				Enrolment_Ref_No,
				Scheme_Code,
				Practice_Display_Seq,
				SP_ID,
				Service_Fee,
				Delist_Status,
				Delist_Dtm,
				Effective_Dtm,
				Record_Status,
				Remark,
				Create_Dtm,
				Create_By,
				Update_Dtm,
				Update_By,
				Subsidize_code,
				ProvideServiceFee,
				Provide_Service,
				Clinic_Type
)
	SELECT		getdate(),
				Enrolment_Ref_No,
				Scheme_Code,
				Practice_Display_Seq,
				SP_ID,
				Service_Fee,
				Delist_Status,
				Delist_Dtm,
				Effective_Dtm,
				Record_Status,
				Remark,
				Create_Dtm,
				Create_By,
				Update_Dtm,
				Update_By,
				Subsidize_code,
				ProvideServiceFee,
				Provide_Service,
				Clinic_Type
	FROM inserted

END
GO
