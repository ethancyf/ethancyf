IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCSPUserAC_upd_RecordStatusPasswordFailCount]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCSPUserAC_upd_RecordStatusPasswordFailCount]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE16-004 (Enable SP to unlock account)
-- Modified by:	    Winnie SUEN
-- Modified date:   14 Dec 2017
-- Description:		Grant premission to HCSP for unlock account in Recover Login function
-- =============================================
-- =============================================
-- Author:			Kathy LEE
-- Create date:		28 July 2008
-- Description:		Update and reset password fail count of HCSPUserAC
-- =============================================

CREATE procedure [dbo].[proc_HCSPUserAC_upd_RecordStatusPasswordFailCount]
	@sp_id char(8),  @update_by varchar(20), @tsmp timestamp
as
BEGIN
	
	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF (SELECT TSMP FROM HCSPUserAC
		WHERE SP_ID = @sp_id) != @tsmp
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

	UPDATE	HCSPUserAC
	Set		Record_Status = 
			case 
				when SP_Password is NULL Then 'P'
				else 'A'
			end,
			Password_Fail_Count = 0,
			IVRS_Password_Fail_Count = 0,
			IVRS_Locked= 'N',
			Update_By = @update_by,
			Update_Dtm = getdate()
	WHERE	SP_ID = @sp_id
end

GO

GRANT EXECUTE ON [dbo].[proc_HCSPUserAC_upd_RecordStatusPasswordFailCount] TO HCVU, HCSP
GO
