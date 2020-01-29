IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_BankIn_update]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_BankIn_update]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	2 January 2015
-- CR No.:			CRE13-019-02
-- Description:		Extend HCVS to China
-- =============================================
-- =============================================
-- Author:			Clark Yip
-- Create date:		15 May 2008
-- Description:		Update record in BankIn table
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   19 Aug 2009
-- Description:	    Add Scheme Code
-- =============================================

CREATE PROCEDURE 	[dbo].[proc_BankIn_update]	
							@reimbursement_id	char(15),
							@file_link VarChar(255),
							@scheme_code	char(10)
as
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

	--Update record in BankIn table
	UPDATE BankIn
	SET [Completion_dtm] = {fn now()}, [Transaction_file_link] = @file_link, Record_Status = 'C'
    WHERE [Reimburse_id] = @reimbursement_id AND [Scheme_Code] = @scheme_code


END
GO

GRANT EXECUTE ON [dbo].[proc_BankIn_update] TO HCVU
GO
