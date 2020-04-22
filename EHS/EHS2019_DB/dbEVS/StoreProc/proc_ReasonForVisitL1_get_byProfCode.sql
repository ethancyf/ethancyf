IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReasonForVisitL1_get_byProfCode]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReasonForVisitL1_get_byProfCode]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 29 April 2008
-- Description:	Retrieve the Level One of 
--				Reason for Visit by Healthcare Profession Category
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_ReasonForVisitL1_get_byProfCode]
	-- Add the parameters for the stored procedure here
	@Professional_Code char(5)
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
    select Professional_Code, Reason_L1_Code, Reason_L1, Reason_L1_Chi
	from dbo.ReasonForVisitL1
	where Professional_Code = @Professional_Code and Record_Status = 'A'

END
GO

GRANT EXECUTE ON [dbo].[proc_ReasonForVisitL1_get_byProfCode] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_ReasonForVisitL1_get_byProfCode] TO HCVU
GO
