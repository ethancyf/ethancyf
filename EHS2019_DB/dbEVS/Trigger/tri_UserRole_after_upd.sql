IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_UserRole_after_upd')
	DROP TRIGGER [dbo].[tri_UserRole_after_upd] + Environment.NewLineGO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Clark YIP
-- Create date: 14 Aug 2009
-- Description:	Trigger an insert statment into UserRoleLOG
--				when a row is updated / inserted / deleted into UserRole
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Clark YIP	
-- Modified date:	14 Aug 2009
-- Description:		Add Scheme Code
-- =============================================
CREATE TRIGGER [dbo].[tri_UserRole_after_upd] 
   ON  [dbo].[UserRole]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	
	SET NOCOUNT ON;

    INSERT INTO UserRoleLOG
	(
		System_Dtm,		
		[User_ID],
		Role_Type,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By,
		Scheme_Code
	)
	SELECT
		getdate(),
		[User_ID],
		Role_Type,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By,
		Scheme_Code
	FROM inserted

END
GO
