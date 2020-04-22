IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_BankIn_get_byKey]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_BankIn_get_byKey]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		16 January 2015
-- CR No.:			CRE13-019-02
-- Description:		Get record from BankIn table
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    
-- Modified date:   
-- CR No.:			
-- Description:	    
-- =============================================

CREATE PROCEDURE [dbo].[proc_BankIn_get_byKey]
	@Reimburse_ID	char(15),
	@Scheme_Code	char(10)
AS BEGIN
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
		Reimburse_ID,
		Submission_Dtm,
		Submitted_By,
		Completion_Dtm,
		Value_Date,
		Transaction_File_Link,
		Transaction_Count,
		Vouchers_Count,
		Total_Amount,
		Scheme_Code
	FROM
		BankIn
	WHERE
		Reimburse_ID = @Reimburse_ID
			AND Scheme_Code = @Scheme_Code
	

END
GO

GRANT EXECUTE ON [dbo].[proc_BankIn_get_byKey] TO HCVU
GO
