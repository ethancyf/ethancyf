IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Token_upd]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Token_upd]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	14 October 2016
-- CR No.			CRE16-019 (To implement token sharing between eHS(S) and eHRSS)
-- Description:		Grant to WSEXT
-- =============================================
-- =============================================
-- CR No:			CRE14-002 - PPI-ePR Migration
-- Author:			Tommy LAM
-- Create date:		27 Mar 2014
-- Description:		Update Table - [Token]
-- =============================================

CREATE PROCEDURE [dbo].[proc_Token_upd]
	@User_ID char(20),
	@Token_Serial_No varchar(20),
	@Project char(10),
	@Is_Share_Token char(1),
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

	UPDATE Token SET
		Project = @Project,
		Is_Share_Token = @Is_Share_Token,
		Update_By = @Update_By,
		Update_Dtm = GETDATE()
	WHERE
		[User_ID] = @User_ID
		AND [Token_Serial_No] = @Token_Serial_No

END
GO

GRANT EXECUTE ON [dbo].[proc_Token_upd] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_Token_upd] TO WSEXT
GO
