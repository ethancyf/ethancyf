IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DataEntryUserAC_upd_Password_Transit]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DataEntryUserAC_upd_Password_Transit]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:			Marco CHOI
-- Create date:		15 Jun 2017
-- CR No.:			I-CRE16-007-02 (Refine system from CheckMarx findings)
-- Description:		Update Password of HCSPUserAC with Password Level
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE Procedure [dbo].[proc_DataEntryUserAC_upd_Password_Transit]
@SP_ID	CHAR(8)
, @Data_Entry_Account	VARCHAR(20)
, @Password		VARCHAR(100)
, @data_entry_password_level	INT
, @tsmp		TIMESTAMP
AS

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
IF (SELECT tsmp FROM DataEntryUserAC
	WHERE SP_ID = @SP_ID AND Data_Entry_Account = @Data_Entry_Account) != @tsmp
BEGIN
	Raiserror('00011', 16, 1)
END

-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================
UPDATE DataEntryUserAC
SET Data_Entry_Password = @Password
	,Data_Entry_Password_Level = @data_entry_password_level
WHERE SP_ID = @SP_ID
AND Data_Entry_Account = @Data_Entry_Account 

GO

GRANT EXECUTE ON [dbo].[proc_DataEntryUserAC_upd_Password_Transit] TO HCSP
GO

