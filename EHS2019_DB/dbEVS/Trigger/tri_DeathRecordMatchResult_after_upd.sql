IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_DeathRecordMatchResult_after_upd')
	DROP TRIGGER [dbo].[tri_DeathRecordMatchResult_after_upd] 
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	28 November 2016
-- CR No.:			INT16-0025 (Create the trigger for DeathRecordMatchResult)
-- Description:		Add back the trigger. The trigger is missed in the initial promotion materials
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		24 May 2011
-- CR No.:			CRE11-007
-- Description:		Trigger an insert statement into DeathRecordMatchResultLOG when a row is updated into DeathRecordMatchResult
-- =============================================

CREATE TRIGGER [dbo].[tri_DeathRecordMatchResult_after_upd]
   ON		[dbo].[DeathRecordMatchResult]
   AFTER	UPDATE
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Insert
-- =============================================

	INSERT INTO DeathRecordMatchResultLOG (
		System_Dtm,
		EHA_Acc_ID,
		EHA_Doc_Code,
		EHA_Acc_Type,
		Encrypt_Field1,
		With_Claim,
		With_Suspicious_Claim,
		Match_Dtm,
		Match_By,
		Remove_Dtm,
		Remove_By,
		Death_Record_File_ID
	)
    SELECT 
		GETDATE(),
		EHA_Acc_ID,
		EHA_Doc_Code,
		EHA_Acc_Type,
		Encrypt_Field1,
		With_Claim,
		With_Suspicious_Claim,
		Match_Dtm,
		Match_By,
		Remove_Dtm,
		Remove_By,
		Death_Record_File_ID
	FROM
		Deleted

	INSERT INTO DeathRecordMatchResultLOG (
		System_Dtm,
		EHA_Acc_ID,
		EHA_Doc_Code,
		EHA_Acc_Type,
		Encrypt_Field1,
		With_Claim,
		With_Suspicious_Claim,
		Match_Dtm,
		Match_By,
		Remove_Dtm,
		Remove_By,
		Death_Record_File_ID
	)
    SELECT 
		GETDATE(),
		EHA_Acc_ID,
		EHA_Doc_Code,
		EHA_Acc_Type,
		Encrypt_Field1,
		With_Claim,
		With_Suspicious_Claim,
		Match_Dtm,
		Match_By,
		Remove_Dtm,
		Remove_By,
		Death_Record_File_ID
	FROM
		Inserted


END
GO
