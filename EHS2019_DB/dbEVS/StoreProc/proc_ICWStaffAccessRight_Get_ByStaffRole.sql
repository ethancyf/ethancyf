IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ICWStaffAccessRight_Get_ByStaffRole]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ICWStaffAccessRight_Get_ByStaffRole]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		1 December 2010
-- Description:		Get ICWStaffAccessRight
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_ICWStaffAccessRight_Get_ByStaffRole]
	@Staff_Role		char(3)
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Return
-- =============================================

	SELECT
		Staff_Role,
		Function_Code,
		Record_Status
	FROM
		ICWStaffAccessRight
	WHERE
		Staff_Role = @Staff_Role
			AND Record_Status = 'A'


END
GO

GRANT EXECUTE ON [dbo].[proc_ICWStaffAccessRight_Get_ByStaffRole] TO HCVU
GO
