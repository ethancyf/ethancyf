IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_BankIn_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_BankIn_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	24 March 2015
-- CR No.:			INT15-0002
-- Description:		Set the stored procedure to recompile each time
-- =============================================
-- =============================================
-- Author:			Clark Yip
-- Create date:		15 May 2008
-- Description:		Get record from BankIn table
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   15 Apr 2009
-- Description:	    Get the Scheme_Code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Lawrence TSANG
-- Modified date:   19 August 2009
-- Description:	    (1)	Remove @scheme_code
--					(2) Inner join ReimbursementAuthorisation to get the record Second Authorised by the user
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    
-- Modified date:   
-- Description:	    
-- =============================================

CREATE PROCEDURE [dbo].[proc_BankIn_get]
	@submitted_by	varchar(20)
WITH RECOMPILE
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
		B.Reimburse_ID,
		B.Submission_Dtm,
		B.Submitted_By,
		B.Completion_Dtm,
		B.Value_Date,
		B.Transaction_File_Link,
		B.Transaction_Count,
		B.Vouchers_Count,
		B.Total_Amount,
		B.Scheme_Code
	FROM
		BankIn B
			INNER JOIN ReimbursementAuthorisation R
				ON B.Reimburse_ID = R.Reimburse_ID
					AND B.Scheme_Code = R.Scheme_Code
					AND R.Authorised_Status = '2'
	WHERE
		R.Authorised_By = @submitted_by
	ORDER BY
		B.Submission_Dtm DESC

END
GO

GRANT EXECUTE ON [dbo].[proc_BankIn_get] TO HCVU
GO
