IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ManualReimbursement_update]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ManualReimbursement_update]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date: 3 November 2010
-- Description:	The record status on Manual Reimbursement will not be used
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 23 July 2010
-- Description:	Update the Manual Reimbursement Status
--				' B: Pending Approval (initial status)
--				' D: Removed
--				' R: Reimbursed
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date: 3 November 2010
-- Description:	The record status on Manual Reimbursement will not be used
-- =============================================
CREATE PROCEDURE [dbo].[proc_ManualReimbursement_update]
	@Transaction_ID		char(20),
	@Record_Status		char(1),	
	@Approval_By		varchar(20),
	@Approval_Dtm		datetime,
	@Reject_By			varchar(20),
	@Reject_Dtm			datetime,
	@ManualReimbursement_TSMP				timestamp

AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF (
		SELECT TSMP FROM [dbo].[ManualReimbursement]
		WHERE Transaction_ID = @Transaction_ID
	) != @ManualReimbursement_TSMP
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

Update [ManualReimbursement] Set 

	[Approval_By] = @Approval_By,
	[Approval_Dtm] = @Approval_Dtm,
	[Reject_By] = @Reject_By,
	[Reject_Dtm] = @Reject_Dtm	

WHERE
	[Transaction_ID] = @Transaction_ID

END


GO

GRANT EXECUTE ON [dbo].[proc_ManualReimbursement_update] TO HCVU
GO
