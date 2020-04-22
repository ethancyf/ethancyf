IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SPDelisted_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SPDelisted_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kathy LEE
-- Create date: 10 July 2008
-- Description:	Insert the SP Delist to Table
--				SPDelisted
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_SPDelisted_add]
	@sp_id char(8), @logo_return_dtm datetime, @token_return_dtm datetime,
	@create_by varchar(20), @update_by varchar(20)
AS
BEGIN

	SET NOCOUNT ON;

     -- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
declare @rowcount int

SELECT	@rowcount = count(1)       
FROM	SPDelisted
WHERE	SP_ID = @SP_ID
		
	IF @rowcount > 0
	BEGIN
		RAISERROR('00011', 16, 1)
		return @@ERROR
	END
	
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================
	INSERT INTO SPDelisted
				(SP_ID,
				 Logo_Return_Dtm,
				 Token_Return_Dtm,
				 Create_Dtm,
				 Create_By,
				 Update_Dtm,
				 Update_By)

	VALUES		 (@sp_id,
				  @logo_return_dtm,
			 	  @token_return_dtm,
				  getdate(),
				  @create_by,
				  getdate(),
				  @update_by)
END
GO

GRANT EXECUTE ON [dbo].[proc_SPDelisted_add] TO HCVU
GO
