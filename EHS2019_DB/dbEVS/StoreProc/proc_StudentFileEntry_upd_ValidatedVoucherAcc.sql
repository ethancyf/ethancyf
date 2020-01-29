IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntry_upd_ValidatedVoucherAcc]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntry_upd_ValidatedVoucherAcc]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:			
-- Modified date:	
-- CR No.			
-- Description:		
-- =============================================
-- =============================================
-- Modification History
-- Create by:		Chris YIM		
-- Create date:		29 Jul 2019
-- CR No.			CRE19-001
-- Description:		Update ValidatedVoucherAcc in SP platform
-- =============================================

CREATE PROCEDURE [dbo].[proc_StudentFileEntry_upd_ValidatedVoucherAcc]
	@Student_File_ID		VARCHAR(15)	,
	@Student_Seq			INT,
	@Doc_Code				VARCHAR(20),
	@Voucher_Acc_ID			VARCHAR(15),
	@Acc_Type				CHAR(1),
	@Acc_Doc_Code			VARCHAR(20),
	@Acc_Validation_Result	VARCHAR(1000),
	@Validated_Acc_Found	CHAR(1),
	@Update_By				VARCHAR(20),
	@Update_Dtm				Datetime
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

	UPDATE 
		StudentFileEntry 
	SET 
		Doc_Code = @Doc_Code,
		Voucher_Acc_ID = @Voucher_Acc_ID,
		Acc_Type = @Acc_Type,
		Acc_Doc_Code = @Acc_Doc_Code,
		Acc_Validation_Result = @Acc_Validation_Result,
		Validated_Acc_Found = @Validated_Acc_Found,
		Last_Rectify_By = @Update_By,				
		Last_Rectify_Dtm = @Update_Dtm
	WHERE 
		Student_File_ID = @Student_File_ID 
		AND Student_Seq = @Student_Seq

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntry_upd_ValidatedVoucherAcc] TO HCSP

GRANT EXECUTE ON [dbo].[proc_StudentFileEntry_upd_ValidatedVoucherAcc] TO HCVU

GO

