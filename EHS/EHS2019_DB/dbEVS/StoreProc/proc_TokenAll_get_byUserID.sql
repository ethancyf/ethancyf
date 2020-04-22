IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TokenAll_get_byUserID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TokenAll_get_byUserID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		19 October 2016
-- CR No.:			CRE16-019 (To implement token sharing between eHS(S) and eHRSS)
-- Description:		Add Deactivate_By
-- =============================================
-- =============================================
-- Modification History
-- CR No:			CRE14-002 - PPI-ePR Migration
-- Modified by:		Tommy LAM
-- Modified date:	21 Mar 2014 (Checked in as 23 Oct 2014)
-- Description:		Add Column -	[Token].[Is_Share_Token]
--									[TokenDeactivated].[Project]
--									[TokenDeactivated].[Is_Share_Token]
-- =============================================
-- =============================================
-- CR No. INT14-0015
-- Modified by:	Karl LAM
-- Modified date: 04 AUG 2014
-- Description:	Retrieve the [Project] information for fixing Token Management for Delisted SP (without a token)


-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 07 July 2008
-- Description:	Retrieve the latest Token Information from Table 
--				"TokenDeactivated" or "Token"
-- =============================================
CREATE PROCEDURE [dbo].[proc_TokenAll_get_byUserID]
	@user_id char(20)
AS
BEGIN

	SET NOCOUNT ON;

    -- Insert statements for procedure here
--	SELECT	[User_ID], Token_Serial_No, Deactivate_Dtm, Deactivate_By, isNull(Remark,'') as Remark
--	FROM	TokenDeactivated
--	WHERE	[User_ID] = @user_id and
--			Deactivate_Dtm IN
--			(SELECT max(Deactivate_Dtm)
--			FROM TokenDeactivated 
--			WHERE	[User_ID] = @user_id
--			GROUP BY [User_ID])


	SELECT	[User_ID], Token_Serial_No, Project, Is_Share_Token, Dtm, isNull(Remark,'') as Remark, Deactivate_By
	FROM 
	(SELECT [user_id], token_serial_no, Project, Is_Share_Token, update_dtm as Dtm, '' as Remark, '' AS [Deactivate_By]
	FROM	Token
	WHERE	[User_ID] = @user_id
	UNION
	SELECT [user_id], token_serial_no, Project, Is_Share_Token, deactivate_dtm as Dtm, isNull(Remark,'') as Remark, Deactivate_By
	FROM	TokenDeactivated
	WHERE	[User_ID] = @user_id
	) TempTokenTable
	WHERE TempTokenTable.Dtm in
	(
	SELECT max(Dtm)
	FROM
	(
	SELECT  update_dtm as Dtm
	FROM	Token
	WHERE	[User_ID] = @user_id
	UNION
	SELECT   deactivate_dtm as Dtm			
	FROM	TokenDeactivated
	WHERE	[User_ID] = @user_id
	)TempTokenTable1	
	)
END
GO

GRANT EXECUTE ON [dbo].[proc_TokenAll_get_byUserID] TO HCVU
GO
