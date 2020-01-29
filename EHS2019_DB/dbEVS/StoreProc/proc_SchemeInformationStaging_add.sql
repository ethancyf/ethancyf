IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeInformationStaging_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchemeInformationStaging_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kathy LEE
-- Create date: 21 July 2008
-- Description:	Insert the Scheme Information to Table
--				SchemeInformationStaging
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Kathy LEE
-- Modified date: 8 May 2009
-- Description:	1. Remove "Service Fee From", "Service Fee To"
--				2. Add "Delist Status", "Delist Dtm", "Effective Dtm", "Logo Return Dtm",
--					"Record Status", "Remark"
-- =============================================
CREATE PROCEDURE [dbo].[proc_SchemeInformationStaging_add]
	@enrolment_ref_no char(15), @scheme_code char(10), @sp_id char(8), @record_status char(1), 
	@remark nvarchar(255), @Delist_Status char(1), @Delist_Dtm datetime, @effective_dtm datetime, 
	@logo_return_dtm datetime, @create_by varchar(20), @update_by varchar(20)
AS
BEGIN

	SET NOCOUNT ON;

  INSERT INTO SchemeInformationStaging
				(Enrolment_Ref_No,
				Scheme_Code,
				SP_ID,
				Record_Status,
				Remark,
				Delist_Status,
				Delist_Dtm,
				Effective_Dtm,
				Logo_Return_Dtm,				
				Create_Dtm,
				Create_By,
				Update_Dtm,
				Update_By)
	VALUES		(@enrolment_ref_no,
				@scheme_code,
				@sp_id,
				@record_status,
				@remark,
				@Delist_Status,
				@Delist_Dtm,
				@effective_dtm,
				@logo_return_dtm,
				getdate(),
				@create_by,
				getdate(),
				@update_by)
END
GO

GRANT EXECUTE ON [dbo].[proc_SchemeInformationStaging_add] TO HCVU
GO
