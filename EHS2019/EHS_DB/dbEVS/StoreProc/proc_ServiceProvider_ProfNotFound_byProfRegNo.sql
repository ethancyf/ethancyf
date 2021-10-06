 IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProvider_ProfNotFound_byProfRegNo]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_ServiceProvider_ProfNotFound_byProfRegNo];
    END;
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- CR No.:			CRE20-006-2 
-- Modified by:		Nichole Ip
-- Modified date:	25 Aug 2021
-- Description:		1. Add with(nolock) on table
-- =============================================
-- =============================================
-- CR No.:		CRE20-006 
-- Created by:	Nichole Ip
-- Created date: 23 Oct 2020
-- Description:	Professional registration no. is not found in eHS(S)
-- =============================================

CREATE PROCEDURE [dbo].[proc_ServiceProvider_ProfNotFound_byProfRegNo] 
	-- Add the parameters for the stored procedure here
	 @ProfCode varchar(20),
	 @RegNo varchar(20)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	-- ============================================================
    -- Declaration
    -- ============================================================
	-- ============================================================
    -- Validation
    -- ============================================================
    -- ============================================================
    -- Initialization
    -- ============================================================ 
	-- ============================================================
    -- Return results
    -- ============================================================
	SELECT CASE
			   WHEN (ISNULL(PF.SP_ID, '') = '' or PF.Record_Status = 'D' )
			   THEN 'Y'
			   ELSE PF.SP_ID
		   END AS NotExisted
	FROM ServiceProvider SP WITH(NOLOCK) 
		 LEFT JOIN Professional PF  WITH(NOLOCK) ON  SP.SP_ID = PF.SP_ID 
	WHERE PF.Registration_Code = @RegNo
		  AND PF.Service_Category_Code = @ProfCode;
END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProvider_ProfNotFound_byProfRegNo] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProvider_ProfNotFound_byProfRegNo] TO WSEXT 
GO
