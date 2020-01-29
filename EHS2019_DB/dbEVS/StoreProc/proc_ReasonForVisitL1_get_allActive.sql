 IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReasonForVisitL1_get_allActive]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReasonForVisitL1_get_allActive]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	9 December 2014
-- CR No.:			CRE13-019-01
-- Description:		Support Simplified Chinese
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================
-- Author:		Paul Yip
-- Create date: 6 September 2010
-- Description:	Retrieve the Level One of 
--				Reason for Visit 
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_ReasonForVisitL1_get_allActive]
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
    select Professional_Code, Reason_L1_Code, Reason_L1, Reason_L1_Chi, Reason_L1_CN
	from dbo.ReasonForVisitL1
	where Record_Status = 'A'

END
GO

GRANT EXECUTE ON [dbo].[proc_ReasonForVisitL1_get_allActive] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_ReasonForVisitL1_get_allActive] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_ReasonForVisitL1_get_allActive] TO WSEXT
Go