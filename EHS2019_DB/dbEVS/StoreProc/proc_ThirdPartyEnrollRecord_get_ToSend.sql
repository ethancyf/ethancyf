IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ThirdPartyEnrollRecord_get_ToSend]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ThirdPartyEnrollRecord_get_ToSend]
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
-- Author:		Tony FUNG
-- Create date: 7 Feb 2012
-- Description:	Retrieve the list of pending Third Party 
--				enrolment records in Table
--				ThridPartyEnrollRecord with any of the
--				following conditions: 
--					- Record_Status = P (Pending)
--				
-- =============================================
CREATE PROCEDURE [dbo].[proc_ThirdPartyEnrollRecord_get_ToSend] 
	@sys_code varchar(50)	
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

SELECT
	Sys_Code,
	Enrolment_Ref_No,
	Data,
	Record_Status,
	Enrolment_Dtm,
	Error_Code,
	Create_dtm,
	Create_by,
	Update_dtm,
	Update_by,
	TSMP
FROM ThirdPartyEnrollRecord
WHERE Sys_Code = @sys_code and Record_Status = 'P' 

END
GO

GRANT EXECUTE ON [dbo].[proc_ThirdPartyEnrollRecord_get_ToSend] TO HCVU
GO
