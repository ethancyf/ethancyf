IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_Practice_after_upd')
	DROP TRIGGER [dbo].[tri_Practice_after_upd]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	17 Feb 2020
-- CR No.			CRE16-022 (SDIR Remark)
-- Description:		Add columns [Mobile_Clinic],[Remarks_Desc] & [Remarks_Desc_Chi]
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		5 October 2009
-- Description:		Trigger an insert statement into PracticeLOG when a row is inserted / updated into Practice
-- =============================================


CREATE TRIGGER [dbo].[tri_Practice_after_upd]
   ON		[dbo].[Practice]
   AFTER	INSERT, UPDATE
AS BEGIN

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
	INSERT INTO [dbo].[PracticeLOG]
		([System_Dtm]
		,[SP_ID]
		,[Display_Seq]
		,[Practice_Name]
		,[Practice_Type]
		,[Room]
		,[Floor]
		,[Block]
		,[Building]
		,[Building_Chi]
		,[District]
		,[Address_Code]
		,[Professional_Seq]
		,[Record_Status]
		,[Remark]
		,[Submission_Method]
		,[Create_Dtm]
		,[Create_By]
		,[Update_Dtm]
		,[Update_By]
		,[MO_Display_Seq]
		,[Practice_Name_Chi]
		,[Phone_Daytime]
		,[Mobile_Clinic]
		,[Remarks_Desc]
		,[Remarks_Desc_Chi]
		)
   SELECT 
		GETDATE(),
		[SP_ID],
		[Display_Seq],
		[Practice_Name],
		[Practice_Type],
		[Room],
		[Floor],
		[Block],
		[Building],
		[Building_Chi],
		[District],
		[Address_Code],
		[Professional_Seq],
		[Record_Status],
		[Remark],
		[Submission_Method],
		[Create_Dtm],
		[Create_By],
		[Update_Dtm],
		[Update_By],
		[MO_Display_Seq],
		[Practice_Name_Chi],
		[Phone_Daytime],
		[Mobile_Clinic],
		[Remarks_Desc],
		[Remarks_Desc_Chi]
	FROM
		Inserted

END
GO

