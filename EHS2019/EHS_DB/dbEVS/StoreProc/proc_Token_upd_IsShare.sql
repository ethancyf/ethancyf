IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Token_upd_IsShare]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Token_upd_IsShare]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- CR No:			CRE16-019 (To implement token sharing between eHS(S) and eHRSS)
-- Author:			Lawrence TSANG
-- Create date:		13 February 2017
-- Description:		Update the Is_Share_Token and Is_Share_Token_Replacement
-- =============================================

CREATE PROCEDURE [dbo].[proc_Token_upd_IsShare]
	@User_ID						char(20),
	@Token_Serial_No				varchar(20),
	@Is_Share_Token					char(1),
	@Is_Share_Token_Replacement		char(1),
	@Update_By						varchar(20),
	@TSMP							binary(8)
AS BEGIN

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF (SELECT TSMP FROM Token WHERE [User_ID] = @User_ID AND [Token_Serial_No] = @Token_Serial_No) <> @TSMP
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

	UPDATE
		Token
	SET
		Is_Share_Token = @Is_Share_Token,
		Is_Share_Token_Replacement = @Is_Share_Token_Replacement,
		Update_By = @Update_By,
		Update_Dtm = GETDATE()
	WHERE
		[User_ID] = @User_ID
			AND [Token_Serial_No] = @Token_Serial_No

END
GO


GRANT EXECUTE ON [dbo].[proc_Token_upd_IsShare] TO WSEXT
GO
