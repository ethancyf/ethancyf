IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ICWStaffAccount_Update_ByStaffID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ICWStaffAccount_Update_ByStaffID]
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
-- Description:		Update ICWStaffAccount
-- =============================================

CREATE PROCEDURE [dbo].[proc_ICWStaffAccount_Update_ByStaffID]
	@Staff_ID			char(8),
	@Staff_Password		varchar(255),
	@Staff_Password_Level	INT
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Return
-- =============================================

	IF @Staff_Password IS NULL BEGIN
		UPDATE
			ICWStaffAccount
		SET
			Last_Login_Dtm = GETDATE()
		WHERE
			Staff_ID = @Staff_ID

	END ELSE BEGIN
		UPDATE
			ICWStaffAccount
		SET
			Staff_Password = @Staff_Password,
			Staff_Password_Level = @Staff_Password_Level,
			Record_Status = 'A',
			Last_Login_Dtm = GETDATE()
		WHERE
			Staff_ID = @Staff_ID

	END

END
GO

GRANT EXECUTE ON [dbo].[proc_ICWStaffAccount_Update_ByStaffID] TO HCVU
GO
