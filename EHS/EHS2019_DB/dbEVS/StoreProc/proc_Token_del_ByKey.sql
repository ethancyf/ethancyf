IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Token_del_ByKey]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Token_del_ByKey]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		2 February 2017
-- CR No.:			CRE16-019 (To implement token sharing between eHS(S) and eHRSS)
-- Description:		Grant EXECUTE to WSEXT
-- =============================================
-- =============================================
-- Author:		Tommy Cheung
-- Create date:	3 July 2008
-- Description:	Delete Token Record
-- =============================================
CREATE PROCEDURE [dbo].[proc_Token_del_ByKey]
	@User_ID char(20),
	@Token_Serial_No varchar(20),
	@TSMP binary(8)
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF(select TSMP from Token where User_ID = @User_ID and 
	Token_Serial_No = @Token_Serial_No) != @TSMP
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
DELETE FROM Token
WHERE User_ID = @User_ID and 
	Token_Serial_No = @Token_Serial_No
END
GO

GRANT EXECUTE ON [dbo].[proc_Token_del_ByKey] TO HCVU, WSEXT
GO
