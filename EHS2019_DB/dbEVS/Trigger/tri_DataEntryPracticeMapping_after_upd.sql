IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_DataEntryPracticeMapping_after_upd')
	DROP TRIGGER [dbo].[tri_DataEntryPracticeMapping_after_upd] + Environment.NewLineGO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		5 October 2009
-- Description:		Trigger an insert statement into DataEntryPracticeMappingLOG when a row is inserted / updated into DataEntryPracticeMapping
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE TRIGGER [dbo].[tri_DataEntryPracticeMapping_after_upd]
   ON		[dbo].[DataEntryPracticeMapping]
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
	INSERT INTO [dbo].[DataEntryPracticeMappingLOG]
		([System_Dtm]
		,[SP_ID]
		,[Data_Entry_Account]
		,[SP_Practice_Display_Seq]
		,[SP_Bank_Acc_Display_Seq])
	SELECT 
		GETDATE(),
		[SP_ID],
		[Data_Entry_Account],
		[SP_Practice_Display_Seq],
		[SP_Bank_Acc_Display_Seq]
	FROM
		Inserted

END
GO
