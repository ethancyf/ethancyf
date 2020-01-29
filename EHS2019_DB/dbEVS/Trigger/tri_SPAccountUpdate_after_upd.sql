IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_SPAccountUpdate_after_upd')
	DROP TRIGGER [dbo].[tri_SPAccountUpdate_after_upd]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Tommy Cheung
-- Create date: 17 July 2008
-- Description:	Trigger an insert statment into SPAccountUpdateLOG
--				when a row is updated / inserted into SPAccountUpdate
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	14 August 2009
-- Description:		Include information 'Scheme_Confirm' in this tigger
-- =============================================
-- =============================================
-- Modification History
-- CR No:			INT13-0028 - SP Amendment Report
-- Modified by:		Tommy LAM
-- Modified date:	19 Nov 2013
-- Description:		Add Column -	[SPAccountUpdate].[Data_Input_By]
--									[SPAccountUpdateLOG].[Data_Input_By]
-- =============================================
CREATE TRIGGER [dbo].[tri_SPAccountUpdate_after_upd]
   ON  [dbo].[SPAccountUpdate]
   AFTER INSERT, UPDATE
AS 
BEGIN

	SET NOCOUNT ON;

    INSERT INTO SPAccountUpdateLOG
	(System_Dtm,
		Enrolment_Ref_No,
		SP_ID, 
		Upd_SP_Info,
		Upd_Bank_Account, 
		Upd_Professional, 
		Issue_Token, 
		Progress_Status,
		Update_Dtm,
		Update_By,
		Scheme_Confirm,
		Data_Input_By)
	SELECT getdate(),
		Enrolment_Ref_No,
		SP_ID, 
		Upd_SP_Info,
		Upd_Bank_Account, 
		Upd_Professional, 
		Issue_Token, 
		Progress_Status,
		Update_Dtm,
		Update_By,
		Scheme_Confirm,
		Data_Input_By
	FROM inserted
END
GO
