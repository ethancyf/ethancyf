IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReasonForVisitL2_get_byProfCodeReaL1CodeReaL2Code]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReasonForVisitL2_get_byProfCodeReaL1CodeReaL2Code]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Tommy TSE
-- Create date: 28 Oct 2011
-- Description:	Grant Execution Right to WSEXT
-- =============================================
-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 3 May 2008
-- Description:	Retrieve the Level Two of 
--				Reason for Visit by Healthcare Profession 
--				Category and Level One of Reason for Visit
--              and Level Two of Reason for Visit
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_ReasonForVisitL2_get_byProfCodeReaL1CodeReaL2Code]
	@Professional_Code char(5),
	@Reason_L1_Code smallint,
	@Reason_L2_Code smallint
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
    select Professional_Code, Reason_L1_Code, Reason_L2_Code, Reason_L2, Reason_L2_Chi
	from dbo.ReasonForVisitL2
	where Professional_Code = @Professional_Code and 
			Reason_L1_Code = @Reason_L1_Code and 
			Reason_L2_Code = @Reason_L2_Code and 
			Record_Status = 'A'

END
GO

GRANT EXECUTE ON [dbo].[proc_ReasonForVisitL2_get_byProfCodeReaL1CodeReaL2Code] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_ReasonForVisitL2_get_byProfCodeReaL1CodeReaL2Code] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_ReasonForVisitL2_get_byProfCodeReaL1CodeReaL2Code] TO WSEXT
GO