IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SPDelisted_upd]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SPDelisted_upd]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kathy LEE
-- Create date: 11 July 2008
-- Description:	Update the Return Info in Table
--				"SPDelisted"
-- =============================================

-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_SPDelisted_upd]
	@sp_id char(8), @logo_return_dtm datetime, @token_return_dtm datetime,
	@update_by varchar(20), @tsmp timestamp
AS
BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF (SELECT TSMP FROM SPDelisted
		WHERE SP_ID = @sp_id ) != @tsmp
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

	UPDATE	SPDelisted
	SET		Logo_Return_Dtm = @logo_return_dtm,
			Token_Return_Dtm = @token_return_dtm,
			Update_By = @update_by,
			Update_Dtm = getdate()
	WHERE	SP_ID = @sp_id
END
GO

GRANT EXECUTE ON [dbo].[proc_SPDelisted_upd] TO HCVU
GO
