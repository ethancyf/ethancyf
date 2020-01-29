IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderOriginal_check_byERN]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderOriginal_check_byERN]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================
-- =============================================
-- CR No.:		CRE12-001
-- Author:		Koala CHENG
-- Create date: 16 Jan 2012
-- Description:	Retrieve the Service Provider from Table
--				ServiceProviderOriginal
-- =============================================
CREATE PROCEDURE [dbo].[proc_ServiceProviderOriginal_check_byERN]
	@enrolment_ref_no	char(15)
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
SELECT Enrolment_Ref_No
FROM	ServiceProviderOriginal WITH (NOLOCK)
WHERE	Enrolment_Ref_No = @enrolment_ref_no

END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderOriginal_check_byERN] TO HCVU
GO
