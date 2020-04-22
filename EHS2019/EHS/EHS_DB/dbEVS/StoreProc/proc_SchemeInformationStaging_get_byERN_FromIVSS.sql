IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeInformationStaging_get_byERN_FromIVSS]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchemeInformationStaging_get_byERN_FromIVSS]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Clark YIP
-- Create date: 09 July 2009
-- Description:	Retrieve the Scheme Information from Table
--				SchemeInformationStaging in IVSS
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	 Clark YIP 
-- Modified date: 24 July 2009
-- Description:	  Set Effective date to null as default
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	 Clark YIP 
-- Modified date: 11 Aug 2009
-- Description:	  Change the SequenceNo to Display_Seq
-- =============================================
CREATE PROCEDURE proc_SchemeInformationStaging_get_byERN_FromIVSS
	@enrolment_ref_no char(15),
	@user_id		varchar(20)
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

	SELECT	S.Enrolment_Ref_No,
			S.SP_ID,
			'CIVSS' as Scheme_code,
			'A' as Record_Status,
			'' as Remark,
			null as Delist_Status,
			null as Effective_Dtm,
			null as Delist_Dtm,
			null as Logo_Return_Dtm,
			getdate() as Create_Dtm,
			@user_id as Create_By,
			getdate() as Update_Dtm,
			@user_id as Update_By,
			S.TSMP,
			'2' as Display_Seq
			
	FROM	[dbIVSS].[dbo].SchemeInformationStaging S	
	WHERE	S.Enrolment_Ref_No = @enrolment_ref_no
	
END
GO

GRANT EXECUTE ON [dbo].[proc_SchemeInformationStaging_get_byERN_FromIVSS] TO HCVU
GO
