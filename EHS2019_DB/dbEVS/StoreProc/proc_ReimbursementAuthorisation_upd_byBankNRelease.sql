IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementAuthorisation_upd_byBankNRelease]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementAuthorisation_upd_byBankNRelease]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:			Clark Yip
-- Create date:		08 OCT 2008
-- Description:		Release Reimbursement Authorisation
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE 	[dbo].[proc_ReimbursementAuthorisation_upd_byBankNRelease]
							@reimburse_id			char(15)	
							,@current_user			varchar(20)							

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

UPDATE ReimbursementAuthorisation
SET [Update_By] = @current_user
       ,[Update_Dtm] = {fn now()}
	   ,[Record_status] = 'V'
WHERE reimburse_id = @reimburse_id and Record_status='A'

END
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementAuthorisation_upd_byBankNRelease] TO HCVU
GO
