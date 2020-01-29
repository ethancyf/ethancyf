IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ThirdPartyEnrollRecord_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ThirdPartyEnrollRecord_add]
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
-- Description:	Insert the Third Party enrolment recort to Table
--				ThridPartyEnrollRecord
-- =============================================
CREATE PROCEDURE [dbo].[proc_ThirdPartyEnrollRecord_add] 
	@sys_code varchar(50),
	@enrolment_ref_no varchar(20),
	@data nvarchar(max),
	@enrolment_dtm DateTime,
	@record_status char(1),
	@error_code varchar(10),
	@create_by varchar(20),
	@update_by varchar(20)
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

INSERT INTO ThirdPartyEnrollRecord 
		(Sys_Code,
		Enrolment_Ref_No,
		Data,
		Enrolment_dtm,
		Record_Status,
		Error_Code,
		Create_by,
		Create_dtm,
		Update_by,
		Update_dtm)
VALUES 		
		(@sys_Code,
		@enrolment_ref_no,
		@data,
		@enrolment_dtm,
		@record_status,
		@error_code,
		@create_by,
		getDate(),
		@update_by,
		getDate())
END
GO

GRANT EXECUTE ON [dbo].[proc_ThirdPartyEnrollRecord_add] TO HCPUBLIC
GO
