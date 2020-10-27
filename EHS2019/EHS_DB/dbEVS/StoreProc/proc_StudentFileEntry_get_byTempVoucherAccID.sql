IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntry_get_byTempVoucherAccID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntry_get_byTempVoucherAccID]
GO

SET ANSI_NULLS ON
GO
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
-- Modified by:		Winnie SUEN
-- Modified date:	23 Sep 2020
-- CR No.:			CRE20-003-03 (Enhancement on Programme or Scheme using batch upload)
-- Description:		Get Student Entry by Temp Voucher Account ID
-- =============================================     

CREATE PROCEDURE [dbo].[proc_StudentFileEntry_get_byTempVoucherAccID] (
	@Temp_Voucher_Acc_ID	VARCHAR(15)
)
AS
BEGIN

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

	SELECT 
		SH.Student_File_ID,
		SH.Record_Status,
		SE.Student_Seq
	FROM 
		StudentFileHeader SH
		INNER JOIN StudentFileEntry SE ON SE.Student_File_ID = SH.Student_File_ID
	WHERE
		SE.Temp_Voucher_Acc_ID = @Temp_Voucher_Acc_ID
		AND SE.Acc_Type = 'T'

	UNION

	SELECT 
		SHS.Student_File_ID,
		SHS.Record_Status,
		SES.Student_Seq
	FROM 
		StudentFileHeaderStaging SHS
		INNER JOIN StudentFileEntryStaging SES ON SES.Student_File_ID = SHS.Student_File_ID
	WHERE
		SES.Temp_Voucher_Acc_ID = @Temp_Voucher_Acc_ID
		AND SES.Acc_Type = 'T'

	ORDER BY 
		Student_File_ID, Record_Status, Student_Seq

END;
GO


GRANT EXECUTE ON [dbo].[proc_StudentFileEntry_get_byTempVoucherAccID] TO HCVU;
GO