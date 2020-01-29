IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_SchemeInformation_after_upd')
	DROP TRIGGER [dbo].[tri_SchemeInformation_after_upd] + Environment.NewLineGO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kathy LEE
-- Create date: 12 Sept 2008
-- Description:	Trigger an insert statment into SchemeInformationLOG
--				when a row is updated / inserted into SchemeInformation
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Kathy LEE
-- Modified date: 8 May 2009
-- Description:	1. Remove "Service Fee From", "Service Fee To"
--				2. Add "Delist Status", "Delist Dtm", "Effective Dtm", "Logo Return Dtm",
--					"Record Status", "Remark"
-- =============================================
Create TRIGGER [dbo].[tri_SchemeInformation_after_upd]
   ON  [dbo].[SchemeInformation]
   AFTER INSERT,UPDATE
AS 
BEGIN
	
	SET NOCOUNT ON;

	INSERT INTO SchemeInformationLOG
				(System_Dtm,
				SP_ID,
				Scheme_Code,
				Record_Status,
				Remark,
				Delist_Status,
				Delist_Dtm,
				Effective_Dtm,
				Logo_Return_Dtm,				
				Create_Dtm,
				Create_By,
				Update_Dtm,
				Update_By)
	SELECT		getdate(),
				SP_ID,
				Scheme_Code,
				Record_Status,
				Remark,
				Delist_Status,
				Delist_Dtm,
				Effective_Dtm,
				Logo_Return_Dtm,				
				Create_Dtm,
				Create_By,
				Update_Dtm,
				Update_By
	FROM inserted

END
GO
