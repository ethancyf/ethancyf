IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeStagingRowCount_byERNProfSeq]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeStagingRowCount_byERNProfSeq]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Tommy Cheung
-- Create date: 18 Sept 2008
-- Description:	Get the no of record in "PracticeStaging"
--				which is in Active Status with ERN and ProfessionalSeq
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_PracticeStagingRowCount_byERNProfSeq]
	@ERN char(15),
	@Professional_Seq smallint
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

	SELECT	COUNT(1)
	FROM	PracticeStaging
	WHERE	Enrolment_Ref_No=@ERN
	and Professional_Seq = @Professional_Seq
	and Record_Status = 'A'
END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeStagingRowCount_byERNProfSeq] TO HCVU
GO
