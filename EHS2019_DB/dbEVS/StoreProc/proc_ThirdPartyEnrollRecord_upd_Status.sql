IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ThirdPartyEnrollRecord_upd_Status]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ThirdPartyEnrollRecord_upd_Status]
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
-- Create date: 8 Feb 2012
-- Description:	Update status, error code, and number of fail counts of 
--				a given Third Party enrolment record in Table
--				ThridPartyEnrollRecord
-- =============================================
CREATE PROCEDURE [dbo].[proc_ThirdPartyEnrollRecord_upd_Status] 
	@sys_code varchar(50),
	@enrolment_ref_no varchar(20),
	@record_status char(1),
	@error_code varchar(10),
	@update_by varchar(20),
	@tsmp timestamp
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

	UPDATE 
		ThirdPartyEnrollRecord 
	SET 
		Record_Status=@record_status, 
		Error_Code=@error_code,
		Update_By=@update_by,
		Update_Dtm=GetDate()
	WHERE
		Sys_Code=@sys_code
		AND Enrolment_Ref_No=@enrolment_ref_no
		AND TSMP=@tsmp


END
GO

GRANT EXECUTE ON [dbo].[proc_ThirdPartyEnrollRecord_upd_Status] TO HCVU
GO
