IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeRowCount_bySPIDProfSeq]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeRowCount_bySPIDProfSeq]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Tommy Cheung
-- Create date: 04 July 2008
-- Description:	Get the no of record in "Practice"
--				which is in Active Status with SPID and ProfessionalSeq
-- =============================================
-- =============================================
-- Modification History
-- CR No:			INT13-0028 - SP Amendment Report
-- Modified by:		Tommy LAM
-- Modified date:	17 Dec 2013
-- Description:		Include "Suspended" record for the result
-- =============================================
CREATE PROCEDURE [dbo].[proc_PracticeRowCount_bySPIDProfSeq]
	@SP_ID varchar(20),
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
	FROM	Practice
	WHERE	SP_ID=@SP_ID
	and Professional_Seq = @Professional_Seq
	and (Record_Status = 'A' or Record_Status = 'S')
END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeRowCount_bySPIDProfSeq] TO HCVU
GO
