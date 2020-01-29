IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeStaging_del_ByKey]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeStaging_del_ByKey]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 23 June 2008
-- Description:	Delete Practice Staging Record
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_PracticeStaging_del_ByKey]
	@Enrolment_Ref_No char(15),
	@Display_Seq smallint,
	@tsmp timestamp,
	@checkTSMP tinyint
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF @checkTSMP  = 1 AND (SELECT TSMP FROM PracticeStaging
		WHERE Enrolment_Ref_No = @enrolment_ref_no and
				Display_Seq = @display_seq) != @tsmp
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

DELETE FROM [dbo].[PracticeStaging]

WHERE 
	Enrolment_Ref_No = @Enrolment_Ref_No AND
	Display_Seq = @Display_Seq 
END

GO

GRANT EXECUTE ON [dbo].[proc_PracticeStaging_del_ByKey] TO HCVU
GO
