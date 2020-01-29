IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SpecialAccount_getBySpecialAccIDStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SpecialAccount_getBySpecialAccIDStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Dedrick Ng
-- Create date: 27 Sep 2009
-- Description:	Retrieve SpecialAccount By Special Account ID & Status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_SpecialAccount_getBySpecialAccIDStatus]
	@Special_Acc_ID	char(15),
	@Record_Status char(1)
AS
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

	SELECT
		Special_Acc_ID,
		Scheme_Code,
		Validated_Acc_ID,
		Record_Status,
		Account_Purpose,
		Confirmed_Dtm,
		Last_Fail_Validate_Dtm,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By,
		DataEntry_By,
		TSMP
		
	FROM [dbo].[SpecialAccount]
	WHERE 
		Special_Acc_ID = @Special_Acc_ID AND Record_Status = @Record_Status
	
END

GO

GRANT EXECUTE ON [dbo].[proc_SpecialAccount_getBySpecialAccIDStatus] TO HCVU
GO
