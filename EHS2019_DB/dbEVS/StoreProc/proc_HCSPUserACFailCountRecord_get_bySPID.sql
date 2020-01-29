IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCSPUserACFailCountRecord_get_bySPID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCSPUserACFailCountRecord_get_bySPID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Kathy LEE
-- Create date:		28 July 2008
-- Description:		Get Record_Status of HCSPUserAC
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE procedure [dbo].[proc_HCSPUserACFailCountRecord_get_bySPID]
	@sp_id char(8)
as
BEGIN
	
	SET NOCOUNT ON;

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

	Select SP_ID, isNULL(Password_Fail_Count, 0) as Password_Fail_Count, Record_Status
	FROM HCSPUserAC
	WHERE SP_ID = @sp_id
END

GO

GRANT EXECUTE ON [dbo].[proc_HCSPUserACFailCountRecord_get_bySPID] TO HCVU
GO
