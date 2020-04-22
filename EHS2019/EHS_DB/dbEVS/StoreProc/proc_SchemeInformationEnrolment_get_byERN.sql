IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeInformationEnrolment_get_byERN]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchemeInformationEnrolment_get_byERN]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kathy LEE
-- Create date: 21 July 2008
-- Description:	Retrieve the Scheme Information from Table
--				SchemeInformationEnrolment
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Kathy LEE
-- Modified date: 7 May 2009
-- Description:	1. Remove "Service Fee From", "Service Fee To"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	30 June 2009
-- Description:		Inner join MasterScheme to get Sequence_No
-- =============================================
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	4 August 2009
-- Description:		Inner join SchemeEForm
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	5 August 2009
-- Description:		Add WHERE clause: Today is between [Enrol_Period_From] and [Enrol_Period_To]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE [dbo].[proc_SchemeInformationEnrolment_get_byERN]
	@enrolment_ref_no char(15)
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
			S.Scheme_Code,
			E.Display_Seq
			
	FROM	SchemeInformationEnrolment S
				INNER JOIN ServiceProviderEnrolment SPE
					ON S.enrolment_ref_no = spe.enrolment_ref_no
				INNER JOIN SchemeEForm E
					ON S.Scheme_Code = E.Scheme_Code
					
	WHERE	S.Enrolment_Ref_No = @enrolment_ref_no
				AND SPE.enrolment_dtm BETWEEN E.Enrol_Period_From AND E.Enrol_Period_To
	
END
GO

GRANT EXECUTE ON [dbo].[proc_SchemeInformationEnrolment_get_byERN] TO HCPUBLIC
GO
