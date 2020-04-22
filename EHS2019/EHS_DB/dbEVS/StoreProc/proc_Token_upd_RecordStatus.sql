IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Token_upd_RecordStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Token_upd_RecordStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:			Tommy Cheung
-- Create date:		04-Jul-2008
-- Description:		Update Table Token - Record Status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_Token_upd_RecordStatus]
	@User_ID char(20),
	@Token_Serial_No varchar(20),
	@Record_Status char(1),
	@Update_By varchar(20),
	@TSMP binary(8)
AS
BEGIN
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF (SELECT TSMP FROM Token
		Where  User_ID=@User_ID
		and Token_Serial_No=@Token_Serial_No) != @TSMP
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
Update Token Set
		Record_Status=@Record_Status,
		Update_By=@Update_By,
		Update_Dtm=getdate()
Where  User_ID=@User_ID
and Token_Serial_No=@Token_Serial_No

END
GO

GRANT EXECUTE ON [dbo].[proc_Token_upd_RecordStatus] TO HCVU
GO
