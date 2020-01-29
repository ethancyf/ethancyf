IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ERNProcessedPermanent_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ERNProcessedPermanent_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Clark YIP
-- Create date:		04-07-2009
-- Description:		Insert ERN processed enrolment to permanent
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE Procedure [dbo].[proc_ERNProcessedPermanent_add]
	@sp_id char(8),
	@enrolment_ref_no char(15),
	@create_by varchar(20),
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

	Insert into ERNProcessed
	(
		SP_ID,
		Enrolment_Ref_No,
		Create_Dtm,
		Create_By,
		Sub_Enrolment_Ref_No
	)
	values
	(
		@sp_id,
		@enrolment_ref_no,		
		getdate(),
		@create_by,
		@sub_enrolment_ref_no
	)
END
GO

GRANT EXECUTE ON [dbo].[proc_ERNProcessedPermanent_add] TO HCVU
GO
