IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCSPUserAC_upd_Password_Transit]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCSPUserAC_upd_Password_Transit]
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
-- CR No.:			
-- Description:	
-- =============================================
CREATE procedure [dbo].[proc_HCSPUserAC_upd_Password_Transit]
@sp_ID	char(8)
, @sp_Password	varchar(100)
, @sp_password_level	int
, @tsmp	timestamp
AS
BEGIN
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF (SELECT TSMP FROM HCSPUserAC
		WHERE SP_ID = @sp_ID) != @tsmp
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
UPDATE HCSPUserAC
SET SP_Password = @sp_Password
	,SP_Password_Level = @sp_password_level
WHERE sp_id = @sp_ID

END

GO

GRANT EXECUTE ON [dbo].[proc_HCSPUserAC_upd_Password_Transit] TO HCSP
GO