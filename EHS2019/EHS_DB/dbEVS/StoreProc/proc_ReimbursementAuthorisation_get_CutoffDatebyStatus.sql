IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementAuthorisation_get_CutoffDatebyStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementAuthorisation_get_CutoffDatebyStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Clark YIP
-- Create date: 08 Oct 2008
-- Description:	Get Reimbursement Cutoff date
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_ReimbursementAuthorisation_get_CutoffDatebyStatus]
	@reimburse_id char(15),
	@record_status char(1)
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

SELECT CutOff_Date
	
FROM [ReimbursementAuthorisation]
WHERE Reimburse_ID = @reimburse_id AND Record_Status = @record_status

END

GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementAuthorisation_get_CutoffDatebyStatus] TO HCVU
GO
