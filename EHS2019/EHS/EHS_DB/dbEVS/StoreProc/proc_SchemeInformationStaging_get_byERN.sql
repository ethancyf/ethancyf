IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeInformationStaging_get_byERN]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchemeInformationStaging_get_byERN]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kathy LEE
-- Create date: 26 July 2008
-- Description:	Retrieve the Scheme Information from Table
--				SchemeInformationStaging
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	7 May 2009
-- Description:		1. Remove "Service Fee From", "Service Fee To"
--					2. Add "Delist Status", "Delist Dtm", "Effective_Dtm" and "Logo_Return_Dtm"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Clark YIP
-- Modified date:	13 May 2009
-- Description:		1. Add a comma (,) after the field "Delist_Dtm"
--					2. Change the typing mistake field "Logo_Retunr_Dtm" to "Logo_Return_Dtm"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	30 June 2009
-- Description:		Inner join MasterScheme to get Sequence_No
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Clark YIP
-- Modified date:	5 August 2009
-- Description:		Inner join SchemeBackOffice
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	6 August 2009
-- Description:		Check today is between [SchemeBackOffice].[Effective_Dtm] and [SchemeBackOffice].[Expiry_Dtm]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Clark YIP
-- Modified date:	07 Sep 2009
-- Description:		Grant execution right to HCSP
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================
CREATE PROCEDURE proc_SchemeInformationStaging_get_byERN
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
			S.SP_ID,
			S.Scheme_Code,
			S.Record_Status,
			S.Remark,
			S.Delist_Status,
			S.Effective_Dtm,
			S.Delist_Dtm,
			S.Logo_Return_Dtm,
			S.Create_Dtm,
			S.Create_By,
			S.Update_Dtm,
			S.Update_By,
			S.TSMP,
			B.Display_Seq
				
	FROM	SchemeInformationStaging S
				INNER JOIN SchemeBackOffice B
					ON S.Scheme_Code = B.Scheme_Code
						AND GETDATE() BETWEEN B.Effective_Dtm AND B.Expiry_Dtm
	
	WHERE	S.Enrolment_Ref_No = @enrolment_ref_no
	
END
GO

GRANT EXECUTE ON [dbo].[proc_SchemeInformationStaging_get_byERN] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SchemeInformationStaging_get_byERN] TO HCVU
GO
