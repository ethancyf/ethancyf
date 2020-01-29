IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementRelease_upd]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementRelease_upd]
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
-- Create date:		13 Aug 2009
-- Description:		Reimbursement Release action
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	9 October 2009
-- Description:		(1) Reformat the code
--					(2) Block concurrent update
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_ReimbursementRelease_upd]							
	@current_user	varchar(20),
	@scheme_code	char(10),
	@reimburse_id	char(15)
WITH RECOMPILE
AS BEGIN
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	-- If the "Hold" has been released
	IF (
			SELECT	COUNT(1)
			FROM	ReimbursementAuthorisation
			WHERE	Reimburse_ID = @reimburse_id
						AND Authorised_Status = 'P'
						AND Record_Status = 'A'
						AND Scheme_Code = @scheme_code
		) = 0 BEGIN
		RAISERROR('00011', 16, 1)
		RETURN @@error
	END
	
	-- If the "Hold" has been first authorized
	IF (
			SELECT	COUNT(1)
			FROM	ReimbursementAuthorisation
			WHERE	Reimburse_ID = @reimburse_id
						AND Authorised_Status = '1'
						AND Record_Status = 'A'
						AND Scheme_Code = @scheme_code
		) <> 0 BEGIN
		RAISERROR('00011', 16, 1)
		RETURN @@error
	END
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

-- (Step 1) Update [ReimbursementAuthorisation]
	UPDATE
		ReimbursementAuthorisation
		
	SET 
		[Void_By] = @current_user
	   ,[Void_Dtm] = GETDATE()
       ,[Void_Remark] = NULL
	   ,[Record_Status] = 'V'
	   ,[Update_By] = @current_user
       ,[Update_Dtm] = GETDATE()
	   
	WHERE 
		Authorised_Status = 'P' 
			AND Reimburse_ID = @reimburse_id
			AND Scheme_Code = @scheme_code

-- (Step 2) Delete [ReimbursementAuthTran]
	DELETE FROM
		ReimbursementAuthTran
	WHERE
		Authorised_Status = 'P'
			AND Reimburse_ID = @reimburse_id
			AND Scheme_Code = @scheme_code

END
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementRelease_upd] TO HCVU
GO
