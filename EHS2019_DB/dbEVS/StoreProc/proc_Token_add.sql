IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Token_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Token_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		19 October 2016
-- CR No.:			CRE16-019 (To implement token sharing between eHS(S) and eHRSS)
-- Description:		- Grant to WSEXT
--					- Add more fields
-- =============================================
-- =============================================
-- Modification History
-- CR No:			CRE14-002 - PPI-ePR Migration
-- Modified by:		Tommy LAM
-- Modified date:	25 Mar 2014
-- Description:		Add Column -	[Token].[Is_Share_Token]
-- =============================================
-- =============================================
-- Author:		Tommy Cheung
-- Create date: 24 Jun 2008
-- Description:	Get Information from Table Token
-- =============================================

CREATE PROCEDURE [dbo].[proc_Token_add]
	@User_ID char(20),
	@Token_Serial_No varchar(20),
	@Project char(10),
	@Is_Share_Token char(1),
	@Token_Serial_No_Replacement varchar(20),
	@Project_Replacement char(10),
	@Is_Share_Token_Replacement char(1),
	@Issue_By varchar(20),
	@Last_Replacement_Dtm datetime,
	@Last_Replacement_Activate_Dtm datetime,
	@Last_Replacement_Reason varchar(10),
	@Last_Replacement_By varchar(20),
	@Record_Status char(1),
	@Update_By varchar(20)
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
INSERT INTO Token (
	User_ID,
	Token_Serial_No,
	Project,
	Issue_By,
	Issue_Dtm,
	Token_Serial_No_Replacement,
	Record_Status,
	Update_By,
	Update_Dtm,
	Last_Replacement_Dtm,
	Last_Replacement_Activate_Dtm,
	Last_Replacement_Reason,
	Last_Replacement_By,
	Project_Replacement,
	Is_Share_Token_Replacement,
	Is_Share_Token
) VALUES (
	@User_ID,
	@Token_Serial_No,
	@Project,
	@Issue_By,
	GETDATE(),
	@Token_Serial_No_Replacement,
	@Record_Status,
	@Update_By,
	GETDATE(),
	@Last_Replacement_Dtm,
	@Last_Replacement_Activate_Dtm,
	@Last_Replacement_Reason,
	@Last_Replacement_By,
	@Project_Replacement,
	@Is_Share_Token_Replacement,
	@Is_Share_Token
)

END
GO

GRANT EXECUTE ON [dbo].[proc_Token_add] TO HCVU, WSEXT
GO
