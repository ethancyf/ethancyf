IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_InvalidAccount_after_del')
	DROP TRIGGER [dbo].[tri_InvalidAccount_after_del] + Environment.NewLineGO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:	    Lawrence TSANG
-- Modified date:   9 April 2010
-- Description:	    Table [InvalidAccountLOG] schema change: 
--						Drop columns: Validated_Acc_ID, Confirmed_Dtm, Last_Fail_Validate_Dtm, DataEntry_By
--						Add column: Original_Acc_Type
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		5 October 2009
-- Description:		Trigger an insert statement into InvalidAccountLOG when a row is deleted from InvalidAccount
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE TRIGGER [dbo].[tri_InvalidAccount_after_del]
   ON		[dbo].[InvalidAccount]
   AFTER	DELETE
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
	INSERT INTO InvalidAccountLOG (
		System_Dtm,
		Invalid_Acc_ID,
		Scheme_Code,
		Record_Status,
		Account_Purpose,
		Original_Acc_ID,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By,
		Count_Benefit,
		Original_Acc_Type
	)
	SELECT 
		GETDATE(),
		Invalid_Acc_ID,
		Scheme_Code,
		Record_Status,
		Account_Purpose,
		Original_Acc_ID,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By,
		Count_Benefit,
		Original_Acc_Type
	FROM
		Deleted

END
GO
