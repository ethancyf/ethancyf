IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeInformationStaging_get_SchemeName]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchemeInformationStaging_get_SchemeName]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Clark YIP
-- Create date: 12 Apr 2009
-- Description:	Retrieve the Scheme_Name of SchemeInformationStaging
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Clark YIP
-- Modified date: 11 Aug 2009
-- Description:	 Update the typo for SPSchemeInformationStaging
-- =============================================
CREATE PROCEDURE [dbo].[proc_SchemeInformationStaging_get_SchemeName]
	@enrolment_ref_no	char(15)
AS
BEGIN
-- =============================================
-- Declaration
-- =============================================
declare @var varchar(100)
declare @rowcount int
set @var = ''

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
SELECT @rowcount = count(1)
FROM	SchemeInformationStaging
WHERE Enrolment_Ref_No = @enrolment_ref_no

-- =============================================
-- Return results
-- =============================================

if @rowcount > 0
BEGIN
	SELECT @Var = @Var + COALESCE (Rtrim(spsi.scheme_code), '') + ', ' from SchemeInformationStaging SPSI where SPSI.Enrolment_Ref_No=@enrolment_ref_no
	select rtrim(substring(@var, 0, len(@var)))
END
ELSE
BEGIN
	SELECT @Var = @Var + COALESCE (Rtrim(sps.scheme_code), '') + ', ' from SchemeInformation SPS where SPS.SP_ID=
		(select sp.sp_id from serviceprovider sp where sp.enrolment_ref_no=@enrolment_ref_no)
	select rtrim(substring(@var, 0, len(@var)))
END

END
GO

GRANT EXECUTE ON [dbo].[proc_SchemeInformationStaging_get_SchemeName] TO HCVU
GO
