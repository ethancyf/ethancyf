IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeInformation_upd_LogoReturnDtm]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchemeInformation_upd_LogoReturnDtm]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kathy LEE
-- Create date: 21 May 2009
-- Description:	Update the logo return datetime by scheme in
--				Table "schemeinforamation"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date:  
-- Description:	   
-- =============================================
CREATE PROCEDURE [dbo].[proc_SchemeInformation_upd_LogoReturnDtm]
	@SP_ID	char(8),
	@Scheme_Code char(10),
	@Logo_Return_Dtm datetime,
	@Update_By varchar(20), 
	@TSMP timestamp
AS
BEGIN
	
	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF (SELECT TSMP FROM SchemeInformation
		WHERE SP_ID = @SP_ID and Scheme_Code = @Scheme_Code) != @TSMP
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
Update SchemeInformation
set Logo_Return_Dtm = @Logo_Return_Dtm,
	update_by = @Update_By,
	update_dtm = getdate()
where sp_id = @SP_ID
and scheme_code = @scheme_code

END
GO

GRANT EXECUTE ON [dbo].[proc_SchemeInformation_upd_LogoReturnDtm] TO HCVU
GO
