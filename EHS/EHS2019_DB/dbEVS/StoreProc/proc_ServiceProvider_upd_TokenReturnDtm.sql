IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProvider_upd_TokenReturnDtm]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProvider_upd_TokenReturnDtm]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kathy LEE
-- Create date: 21 May 2009
-- Description:	Update the token return datetime in
--				Table "ServiceProvider"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date:  
-- Description:	   
-- =============================================
CREATE PROCEDURE [dbo].[proc_ServiceProvider_upd_TokenReturnDtm]
	@SP_ID	char(8),
	@Token_Return_Dtm datetime,
	@Update_By varchar(20), 
	@TSMP timestamp
AS
BEGIN
	
	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF (SELECT TSMP FROM ServiceProvider
		WHERE SP_ID = @SP_ID) != @TSMP
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
Update ServiceProvider
set token_return_dtm = @token_return_dtm,
	update_by = @Update_By,
	update_dtm = getdate()
where sp_id = @SP_ID

END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProvider_upd_TokenReturnDtm] TO HCVU
GO
