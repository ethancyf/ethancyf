IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntryStaging_upd_TempVoucherAccID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntryStaging_upd_TempVoucherAccID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.:			
-- Description:		
-- =============================================
-- =============================================
-- Modification History
-- Created by:		Chris YIM
-- Created date:	10 Sep 2020
-- CR No.			CRE20-003 (Batch Upload)
-- Description:		Update temp account ID in StudentFileEntryVaccineStaging
-- =============================================

CREATE PROCEDURE [dbo].[proc_StudentFileEntryStaging_upd_TempVoucherAccID]
	@Student_File_ID				VARCHAR(15)	,
	@Student_Seq					INT,
	@Temp_Voucher_Acc_ID			CHAR(15)
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
	DECLARE @Voucher_Acc_ID		CHAR(15)
	DECLARE @Record_Status		CHAR(1)
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	SELECT 
		@Voucher_Acc_ID = TVA.Voucher_Acc_ID,
		@Record_Status = TVA.Record_Status
	FROM 
		TempVoucherAccount TVA
	WHERE
		TVA.Voucher_Acc_ID = @Temp_Voucher_Acc_ID

-- =============================================
-- Return results
-- =============================================

	UPDATE 
		StudentFileEntryStaging
	SET 
		Voucher_Acc_ID = NULL,
		Temp_Voucher_Acc_ID = @Voucher_Acc_ID,
		Acc_Type = 'T',
		Temp_Acc_Record_Status = @Record_Status
	FROM 
		StudentFileEntryStaging SFES
	WHERE 
		SFES.Student_File_ID = @Student_File_ID 
		AND SFES.Student_Seq = @Student_Seq

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntryStaging_upd_TempVoucherAccID] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntryStaging_upd_TempVoucherAccID] TO HCSP
GO

