IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ICWStaffAccount_Get_ByStaffID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ICWStaffAccount_Get_ByStaffID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			I-CRE16-007-02 (Refine system from CheckMarx findings)
-- Modified by:		Winnie SUEN
-- Modified date:	20 Jun 2017
-- Description:		Add field "Staff_Password_Level"
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		1 December 2010
-- Description:		Get ICWStaffAccount
-- =============================================


CREATE PROCEDURE [dbo].[proc_ICWStaffAccount_Get_ByStaffID]
	@Staff_ID	char(8)
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Return
-- =============================================

	SELECT
		Staff_ID,
		Staff_Password,
		Staff_Password_Level,
		Last_Login_Dtm,
		Record_Status,
		Staff_Role
	FROM
		ICWStaffAccount
	WHERE
		Staff_ID = @Staff_ID


END
GO

GRANT EXECUTE ON [dbo].[proc_ICWStaffAccount_Get_ByStaffID] TO HCVU
GO
