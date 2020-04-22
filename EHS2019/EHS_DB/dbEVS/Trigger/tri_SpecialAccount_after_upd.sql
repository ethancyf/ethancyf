IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_SpecialAccount_after_upd')
	DROP TRIGGER [dbo].[tri_SpecialAccount_after_upd]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE14-016 (To introduce 'Deceased' status into eHS)
-- Modified by:	    Winnie SUEN
-- Modified date:   15 Nov 2017
-- Description:		Add [Deceased]
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		5 October 2009
-- Description:		Trigger an insert statement into SpecialAccountLOG when a row is inserted / updated into SpecialAccount
-- =============================================

CREATE TRIGGER [dbo].[tri_SpecialAccount_after_upd]
   ON		[dbo].[SpecialAccount]
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
	INSERT INTO [dbo].[SpecialAccountLOG]
		([System_Dtm]
		,[Special_Acc_ID]
		,[Scheme_Code]
		,[Validated_Acc_ID]
		,[Record_Status]
		,[Account_Purpose]
		,[Confirm_Dtm]
		,[Last_Fail_Validate_Dtm]
		,[Original_Acc_ID]
		,[Create_Dtm]
		,[Create_By]
		,[Update_Dtm]
		,[Update_By]
		,[DataEntry_By]
		,[Temp_Voucher_Acc_ID]
		,[Deceased]
		)
	SELECT 
		GETDATE(),
		[Special_Acc_ID],
		[Scheme_Code],
		[Validated_Acc_ID],
		[Record_Status],
		[Account_Purpose],
		[Confirmed_Dtm],
		[Last_Fail_Validate_Dtm],
		[Original_Acc_ID],
		[Create_Dtm],
		[Create_By],
		[Update_Dtm],
		[Update_By],
		[DataEntry_By],
		[Temp_Voucher_Acc_ID],
		[Deceased]
	FROM Inserted

END
GO
