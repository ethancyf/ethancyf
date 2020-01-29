IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFile_Claim_Reactivate]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFile_Claim_Reactivate]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	29 Aug 2019
-- CR No.			CRE19-001
-- Description:		Reactivate all suspended claim transaction under the Student File
-- =============================================

CREATE PROCEDURE [dbo].[proc_StudentFile_Claim_Reactivate]
	@Student_File_ID			varchar(15),
	@Update_By					varchar(20),
	@Update_Dtm					datetime

AS BEGIN
-- =============================================
-- Declaration
-- ============================================= 
	DECLARE @TransSuspended TABLE (
		Transaction_ID				CHAR(20),
		Original_Record_Status		CHAR(1),
		Record_Status				char(1)
	)
	
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================

	-- Retrieve the student file transaction list
	INSERT INTO @TransSuspended (Transaction_ID, Original_Record_Status, Record_Status)

	SELECT
		VT.Transaction_ID,
		ST.Original_Record_Status,
		VT.Record_Status
	FROM
		StudentFileEntry SE
	INNER JOIN VoucherTransaction VT
			ON SE.Transaction_ID = VT.Transaction_ID
	
	CROSS APPLY 
			(	SELECT  
					TOP 1 Transaction_ID, Original_Record_Status
				FROM    
					VoucherTranSuspendLog VS
				WHERE   
					VS.Transaction_ID = VT.Transaction_ID AND VS.Record_Status = 'S'
				ORDER BY
					System_Dtm DESC
			) ST
	WHERE
		SE.Student_File_ID = @Student_File_ID
		AND VT.Record_Status = 'S'

-- =============================================
-- Return results
-- =============================================

	-- Update VoucherTrans Record Status
	UPDATE VT		
	SET 				
		Update_By = @Update_By,
		Update_Dtm = @Update_Dtm,
		Record_Status = TS.Original_Record_Status
	FROM 
		VoucherTransaction VT
	INNER JOIN
		@TransSuspended TS
			ON VT.Transaction_ID = TS.Transaction_ID


	-- Add VoucherTranSuspendLOG
	INSERT INTO VoucherTranSuspendLOG (
		[Transaction_ID],
		[System_Dtm],
		[Update_By],
		[Original_Record_Status],
		[Record_Status],
		[Remark]
	)
	SELECT
		Transaction_ID,
		@Update_Dtm,
		@Update_By,
		'S',
		Original_Record_Status,
		''
	FROM
		@TransSuspended

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFile_Claim_Reactivate] TO HCVU
GO
