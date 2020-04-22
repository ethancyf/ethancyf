IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SpecialAccount_upd_StatusUser]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SpecialAccount_upd_StatusUser]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Dedrick Ng
-- Create date: 27 Sep 2009
-- Description:	Update SpecialAccount Record Status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_SpecialAccount_upd_StatusUser]
	@Special_Acc_ID char(15),
	@Scheme_Code char(10),
	@Record_Status char(1),
	@User_ID varchar(20)
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
	UPDATE [dbo].[SpecialAccount]
	SET
		Record_Status = @Record_Status,
		Update_Dtm = GetDate(),
		Update_By = @User_ID
	WHERE 
		Special_Acc_ID = @Special_Acc_ID AND Scheme_Code = @Scheme_Code
END

GO

GRANT EXECUTE ON [dbo].[proc_SpecialAccount_upd_StatusUser] TO HCVU
GO
