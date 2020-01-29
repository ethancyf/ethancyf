IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Token_upd_ReplacementNo]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Token_upd_ReplacementNo]
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
-- Modification History
-- CR No:			CRE14-002 - PPI-ePR Migration
-- Modified by:		Tommy LAM
-- Modified date:	14 Mar 2014
-- Description:		Add Column -	[Token].[Project_Replacement]
--									[Token].[Is_Share_Token_Replacement]
--									[Token].[Is_Share_Token]
-- =============================================
-- =============================================
-- Modification History
-- CRE13-003 
-- Modified by:		Karl Lam
-- Modified date:	15-May-2013
-- Description:		Add Column Last_Replacement_Dtm, Last_Replacement_Activate_Dtm, Last_Replacement_Reason, Last_Replacement_By
--								Issue_dtm, Issue_By
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	4 April 2011
-- Description:		Grant EXECUTE to WSINT
-- =============================================
-- =============================================
-- Author:			Tommy Cheung
-- Create date:		04-Jul-2008
-- Description:		Update Table Token - Replacement No
-- =============================================
CREATE PROCEDURE [dbo].[proc_Token_upd_ReplacementNo]
	@User_ID char(20),
	@Token_Serial_No varchar(20),
	@Project	char(10),
	@Token_Serial_No_Replacement varchar(20),
	@Update_By varchar(20),
	@TSMP binary(8),
	@Last_Replacement_Dtm datetime,
	@Last_Replacement_Activate_Dtm datetime,
	@Last_Replacement_Reason Varchar(10),
	@Last_Replacement_By varchar(20),
	@Issue_By	varchar(20),
	@Issue_Dtm	datetime,
	@Project_Replacement	char(10),
	@Is_Share_Token_Replacement	char(1),
	@Is_Share_Token	char(1)
	
AS
BEGIN
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF (SELECT TSMP FROM Token
		Where  User_ID=@User_ID) != @TSMP
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
		Token_Serial_No=@Token_Serial_No,
		Token_Serial_No_Replacement=@Token_Serial_No_Replacement,
		Project = @Project,
		Update_By=@Update_By,
		Update_Dtm=getdate(),
		Last_Replacement_Dtm = @Last_Replacement_Dtm ,
		Last_Replacement_Activate_Dtm = @Last_Replacement_Activate_Dtm ,
		Last_Replacement_Reason	= @Last_Replacement_Reason,
		Last_Replacement_By = @Last_Replacement_By,
		Issue_By = @Issue_By,
		Issue_Dtm = @Issue_Dtm,
		Project_Replacement = @Project_Replacement,
		Is_Share_Token_Replacement = @Is_Share_Token_Replacement,
		Is_Share_Token = @Is_Share_Token
Where  User_ID=@User_ID
END
GO

GRANT EXECUTE ON [dbo].[proc_Token_upd_ReplacementNo] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_Token_upd_ReplacementNo] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_Token_upd_ReplacementNo] TO WSINT
GO

GRANT EXECUTE ON [dbo].[proc_Token_upd_ReplacementNo] TO WSEXT
GO
