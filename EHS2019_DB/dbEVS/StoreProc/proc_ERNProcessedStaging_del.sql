IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ERNProcessedStaging_del]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ERNProcessedStaging_del]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Clark YIP
-- Create date:		05-07-2009
-- Description:		delete ERNProcessedStaging
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE Procedure [dbo].[proc_ERNProcessedStaging_del]
	@enrolment_ref_no char(15),
	@sub_enrolment_ref_no char(15)	
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


	Delete from ERNProcessedStaging
	Where Enrolment_Ref_No=@enrolment_ref_no and Sub_Enrolment_Ref_No=@sub_enrolment_ref_no

END
GO

GRANT EXECUTE ON [dbo].[proc_ERNProcessedStaging_del] TO HCVU
GO
