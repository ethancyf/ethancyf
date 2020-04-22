IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntryStaging_AccountInfo_upd]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntryStaging_AccountInfo_upd]
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
-- Author:			Winnie SUEN
-- Create date:		20 Aug 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Update StudentFileEntryStaging Account matching result
-- =============================================

CREATE PROCEDURE [dbo].[proc_StudentFileEntryStaging_AccountInfo_upd]
	@Student_File_ID				varchar(15),
	@Student_Seq					int,
	@Acc_Process_Stage				varchar(20),
	@Voucher_Acc_ID					char(15),
	@Temp_Voucher_Acc_ID			char(15),
	@Acc_Type						char(1),
	@Acc_Doc_Code					char(20),
	@Temp_Acc_Record_Status			char(1),
	@Temp_Acc_Validate_Dtm			datetime,
	@Acc_Validation_Result			varchar(1000),
	@Validated_Acc_Found			char(1),
	@Validated_Acc_Unmatch_Result	varchar(1000),
	@TSMP							TimeStamp

AS BEGIN

	SET NOCOUNT ON;
	
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF (
		SELECT TSMP FROM StudentFileEntryStaging WHERE Student_File_ID = @Student_File_ID AND Student_Seq = @Student_Seq
	) != @TSMP
	BEGIN
		RAISERROR('00011', 16, 1)
		RETURN @@error
	END

-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	UPDATE
		StudentFileEntryStaging
	SET	
		Acc_Process_Stage = @Acc_Process_Stage,
		Acc_Process_Stage_Dtm = FORMAT( GETDATE() ,'yyyy-MM-dd'),
		Voucher_Acc_ID = @Voucher_Acc_ID,
		Temp_Voucher_Acc_ID = @Temp_Voucher_Acc_ID,
		Acc_Type = @Acc_Type,
		Acc_Doc_Code = @Acc_Doc_Code,
		Temp_Acc_Record_Status = @Temp_Acc_Record_Status,
		Temp_Acc_Validate_Dtm = @Temp_Acc_Validate_Dtm,
		Acc_Validation_Result = @Acc_Validation_Result,
		Validated_Acc_Found = @Validated_Acc_Found,
		Validated_Acc_Unmatch_Result = @Validated_Acc_Unmatch_Result,
		Update_Dtm = GETDATE()
	WHERE
		Student_File_ID = @Student_File_ID
		AND Student_Seq = @Student_Seq
		


END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntryStaging_AccountInfo_upd] TO HCVU
GO
