IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_PracticeSchemeInfo_after_upd')
	DROP TRIGGER [dbo].[tri_PracticeSchemeInfo_after_upd]
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
-- Description:	Trigger an insert statment into PracticeSchemeInfoLOG
--				when a row is updated / inserted into PracticeSchemeInfo
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:
-- =============================================
Create TRIGGER [dbo].[tri_PracticeSchemeInfo_after_upd]
   ON  [dbo].[PracticeSchemeInfo]
   AFTER INSERT,UPDATE
AS 
BEGIN
	
	SET NOCOUNT ON;

	INSERT INTO PracticeSchemeInfoLOG
				(System_Dtm,
				SP_ID,
				Scheme_Code,
				Practice_Display_Seq,
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
				Subsidize_Code,
				ProvideServiceFee,
				Provide_Service,
				Clinic_Type
)
	SELECT		getdate(),
				SP_ID,
				Scheme_Code,
				Practice_Display_Seq,
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
				Subsidize_Code,
				ProvideServiceFee,
				Provide_Service,
				Clinic_Type
	FROM inserted

END
GO
