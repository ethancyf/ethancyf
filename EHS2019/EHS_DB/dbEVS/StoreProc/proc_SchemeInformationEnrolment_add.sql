IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeInformationEnrolment_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchemeInformationEnrolment_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kathy LEE
-- Create date: 21 July 2008
-- Description:	Insert the Scheme Information to Table
--				SchemeInformationEnrolment
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Kathy LEE
-- Modified date: 8 May 2009
-- Description:	Remove "Service Fee From", "Service Fee To"
-- =============================================

CREATE PROCEDURE [dbo].[proc_SchemeInformationEnrolment_add]
	@enrolment_ref_no char(15), @scheme_code char(10)
AS
BEGIN

	SET NOCOUNT ON;

  INSERT INTO SchemeInformationEnrolment
				(Enrolment_Ref_No,				
				Scheme_Code)
	VALUES		(@enrolment_ref_no,				
				@scheme_code)
END
GO

GRANT EXECUTE ON [dbo].[proc_SchemeInformationEnrolment_add] TO HCPUBLIC
GO
