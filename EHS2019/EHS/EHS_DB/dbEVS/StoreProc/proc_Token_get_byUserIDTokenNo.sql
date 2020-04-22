IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Token_get_byUserIDTokenNo]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Token_get_byUserIDTokenNo]
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
-- Modified date:	13 Mar 2014
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
--					Grant execute to WSINT
-- =============================================
-- =============================================
-- Author:			Tommy Cheung
-- Create date:		24-Jun-2008
-- Description:		Get Information from Table Token
-- =============================================

CREATE PROCEDURE [dbo].[proc_Token_get_byUserIDTokenNo]
	@User_ID char(20),
	@Token_Serial_No varchar(20)
AS
BEGIN
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================
SELECT User_ID, 
		Token_Serial_No,
		Project,
		Issue_By,
		Issue_Dtm,
		isNull(Token_Serial_No_Replacement,'') Token_Serial_No_Replacement,
		Record_Status,
		isNull(Update_By,'') Update_By,
		Update_Dtm,
		TSMP,
		Last_Replacement_Dtm,
		Last_Replacement_Activate_Dtm,
		Last_Replacement_Reason,
		Last_Replacement_By,
		Project_Replacement,
		Is_Share_Token_Replacement,
		Is_Share_Token
From Token
Where (User_ID = @User_ID or @User_ID is null) and
(Token_Serial_No = @Token_Serial_No or @Token_Serial_No is null)

END 
GO

GRANT EXECUTE ON [dbo].[proc_Token_get_byUserIDTokenNo] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_Token_get_byUserIDTokenNo] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_Token_get_byUserIDTokenNo] TO WSINT
GO

GRANT EXECUTE ON [dbo].[proc_Token_get_byUserIDTokenNo] TO WSEXT
GO
