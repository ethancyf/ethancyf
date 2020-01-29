IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeStaging_upd_MoDisplaySeq]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeStaging_upd_MoDisplaySeq]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kathy LEE
-- Create date: 18 Jun 2009
-- Description:	Update the MO Display Seq in
--				Table "PracticeStaging"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	  
-- =============================================
CREATE PROCEDURE [dbo].[proc_PracticeStaging_upd_MoDisplaySeq]
	@enrolment_ref_no char(15), 
	@display_seq smallint,
	@mo_display_seq smallint,
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
	IF (SELECT TSMP FROM PracticeStaging
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

	UPDATE	PracticeStaging
	Set		MO_Display_Seq = @mo_display_seq
	WHERE	Enrolment_Ref_No = @enrolment_ref_no and 
			Display_Seq = @display_seq
END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeStaging_upd_MoDisplaySeq] TO HCVU
GO
