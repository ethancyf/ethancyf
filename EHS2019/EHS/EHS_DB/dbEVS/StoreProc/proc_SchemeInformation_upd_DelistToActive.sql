IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeInformation_upd_DelistToActive]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchemeInformation_upd_DelistToActive]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Clark YIP
-- Create date:	26 June 2009
-- Description:	Update SchemeInformation record status, from Delisted to Active
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	13 July 2009
-- Description:		Change Effective_Dtm to GETDATE() and Remark to NULL
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark YIP
-- Modified date:   27 July 2009
-- Description:	    Add TSMP checking before update
-- =============================================
CREATE PROCEDURE [dbo].[proc_SchemeInformation_upd_DelistToActive]
	@sp_id			char(8),
	@Scheme_Code	char(10),
	@Update_By		varchar(20),
	@tsmp			timestamp
AS
BEGIN
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
IF (
		SELECT TSMP FROM [dbo].[SchemeInformation]
		WHERE SP_ID = @sp_id AND scheme_code=@scheme_code
	) != @TSMP
	BEGIN
		RAISERROR('00011', 16, 1)
		return @@error
	END
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================
	UPDATE	SchemeInformation
	
	SET		Record_Status = 'A',
			Update_By = @Update_By,
			Update_Dtm = GETDATE(),
			Delist_Status = NULL,
			Delist_Dtm = NULL,
			Effective_Dtm = GETDATE(),
			Logo_Return_Dtm = NULL,
			Remark = NULL
			
	WHERE	SP_ID = @sp_id
				AND Scheme_Code = @Scheme_Code

END
GO

GRANT EXECUTE ON [dbo].[proc_SchemeInformation_upd_DelistToActive] TO HCVU
GO
