IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SubsidizeFee_get_all]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[proc_SubsidizeFee_get_all]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No:			
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================
-- =============================================
-- Author:		Twinsen CHAN
-- Create date: 22 Nov 2012
-- Description:	Retrieve the SubsidizeFee information
-- =============================================
CREATE PROCEDURE [dbo].[proc_SubsidizeFee_get_all] 
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
		[SBFE_Scheme_Code],
		[SBFE_Subsidize_Seq],
		[SBFE_Subsidize_Code],
		[SBFE_Effective_Dtm],
		[SBFE_Expiry_Dtm],
		[SBFE_Subsidize_Fee],
		[SBFE_Subsidize_Fee_Type],
		[SBFE_Subsidize_Fee_Type_Display_Seq],
		[SBFE_Subsidize_Fee_Type_Display_Resource],
		[SBFE_Subsidize_Fee_Visible],
		[SBFE_Create_By],
		[SBFE_Create_Dtm],
		[SBFE_Update_By],
		[SBFE_Update_Dtm]
	FROM [SubsidizeFee_SBFE]

END
GO

GRANT EXECUTE ON [dbo].[proc_SubsidizeFee_get_all] TO HCPUBLIC
GO
GRANT EXECUTE ON [dbo].[proc_SubsidizeFee_get_all] TO HCSP
GO
GRANT EXECUTE ON [dbo].[proc_SubsidizeFee_get_all] TO HCVU
GO
GRANT EXECUTE ON [dbo].[proc_SubsidizeFee_get_all] TO WSEXT
GO
