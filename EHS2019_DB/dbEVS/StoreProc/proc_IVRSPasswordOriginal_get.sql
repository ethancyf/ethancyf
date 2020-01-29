IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[proc_IVRSPasswordOriginal_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [proc_IVRSPasswordOriginal_get]
GO

-- =============================================
-- Author:			Dickson Law
-- Create date:		12 June 2017
-- CR No.			I-CRE16-007-02 (Refine system from CheckMarx findings)
-- Description:		Get Old IVRS Password
-- =============================================

CREATE PROCEDURE [proc_IVRSPasswordOriginal_get]

@SP_ID			VARCHAR(20)

AS
BEGIN
	SET NOCOUNT ON;	
-- =============================================
-- Declaration
-- =============================================


-- =============================================
-- Initialization
-- =============================================


-- =============================================
-- Return results
-- =============================================

SELECT 
	SP_ID,
	SP_IVRS_Password,
	SP_IVRS_Password_Level,
	Record_Status
  FROM IVRSPasswordOriginal
WHERE SP_ID = @SP_ID 
AND Record_Status='A'

END
GO

GRANT EXECUTE ON [proc_IVRSPasswordOriginal_get] TO HCSP
GO
