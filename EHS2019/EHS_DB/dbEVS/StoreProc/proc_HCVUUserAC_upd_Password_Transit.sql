IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCVUUserAC_upd_Password_Transit]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[proc_HCVUUserAC_upd_Password_Transit]
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
CREATE procedure [dbo].[proc_HCVUUserAC_upd_Password_Transit]
@User_ID	char(20)
, @User_Password	varchar(100)
, @User_password_level	INT
, @tsmp				timestamp
as

-- =============================================
-- Validation 
-- =============================================
if (select tsmp from HCVUUserAC
		where User_ID = @User_ID) != @tsmp
begin
	Raiserror('00011', 16, 1)
	return @@error
end

-- =============================================
-- Update Transcation
-- =============================================
update HCVUUserAC
set User_Password = @User_Password
, User_Password_Level = @User_password_level
where User_ID = @User_ID
GO

GRANT  EXECUTE  ON [dbo].[proc_HCVUUserAC_upd_Password_Transit]  TO [HCVU]
GO

