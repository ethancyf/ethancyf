IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeSchemeInfoStaging_del_ByKey]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeSchemeInfoStaging_del_ByKey]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Clark YIP
-- Create date: 06 May 2009
-- Description:	Delete PracticeSchemeInfoStaging Record
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	04 August 2009
-- Description:		Add "SubsidizeCode"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_PracticeSchemeInfoStaging_del_ByKey]
	@Enrolment_Ref_No char(15),
	@Practice_Display_Seq smallint,
	@scheme_code char(10),
	@tsmp timestamp,
	@checkTSMP tinyint,
	@subsidize_code char(10)
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF @checkTSMP  = 1 AND (SELECT TSMP FROM PracticeSchemeInfoStaging
		WHERE Enrolment_Ref_No = @enrolment_ref_no and
				Practice_Display_Seq = @Practice_Display_Seq and scheme_code = @scheme_code and subsidize_code = @subsidize_code) != @tsmp
	BEGIN
		RAISERROR('00011', 16, 1)
		return @@error
	END

-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

DELETE FROM [dbo].[PracticeSchemeInfoStaging]

WHERE 
	Enrolment_Ref_No = @Enrolment_Ref_No AND
	Practice_Display_Seq = @Practice_Display_Seq and
	subsidize_code = @subsidize_code
END

GO

GRANT EXECUTE ON [dbo].[proc_PracticeSchemeInfoStaging_del_ByKey] TO HCVU
GO
