IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[proc_IVRSPasswordOriginal_upd]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [proc_IVRSPasswordOriginal_upd]
GO

-- =============================================
-- Author:			Dickson Law
-- Create date:		12 June 2017
-- CR No.			I-CRE16-007-02 (Refine system from CheckMarx findings)
-- Description:		update old IVRS password to 'R' if user login success with new has IVRS passowrd
-- =============================================

CREATE PROCEDURE [proc_IVRSPasswordOriginal_upd]

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

UPDATE IVRSPasswordOriginal
SET Record_Status = 'R' 
WHERE SP_ID = @SP_ID

END
GO


GRANT EXECUTE ON [proc_IVRSPasswordOriginal_upd] TO HCSP
GO