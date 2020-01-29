IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReasonForVisitL1_get_byProfCodeReaL1Code]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReasonForVisitL1_get_byProfCodeReaL1Code]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 29 April 2008
-- Description:	Retrieve the Level Two of 
--				Reason for Visit by Healthcare Profession 
--				Category and Level One of Reason for Visit
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_ReasonForVisitL1_get_byProfCodeReaL1Code]
	@Professional_Code char(5),
	@Reason_L1_Code smallint
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
	where Professional_Code = @Professional_Code and 
			Reason_L1_Code = @Reason_L1_Code and 
			Record_Status = 'A'
	
END
GO

GRANT EXECUTE ON [dbo].[proc_ReasonForVisitL1_get_byProfCodeReaL1Code] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_ReasonForVisitL1_get_byProfCodeReaL1Code] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_ReasonForVisitL1_get_byProfCodeReaL1Code] TO WSEXT
Go
