IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ERNProcessedStaging_get_byERN]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ERNProcessedStaging_get_byERN]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Kathy LEE
-- Create date:		02 Jun 2009
-- Description:		Get the enrolment list by ERN in Staging
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	03 July 2009
-- Description:		1. Remove suffix, mo_dispaly_seq, scheme_code, update_by, update_dtm
--					2. Add Sub_Enrolment_Ref_No
-- =============================================
CREATE Procedure [dbo].[proc_ERNProcessedStaging_get_byERN]
	@enrolment_ref_no char(15)
as
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

	Select  Enrolment_Ref_No,
			isnull(SP_ID, '') as SP_ID,
			create_by,
			create_dtm,
			Sub_Enrolment_Ref_No
	from	ERNProcessedStaging
	where	Enrolment_Ref_No = @enrolment_ref_no
END
GO

GRANT EXECUTE ON [dbo].[proc_ERNProcessedStaging_get_byERN] TO HCVU
GO
