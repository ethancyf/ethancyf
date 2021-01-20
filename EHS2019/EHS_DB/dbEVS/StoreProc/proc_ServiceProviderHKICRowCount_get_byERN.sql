IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderHKICRowCount_get_byERN]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderHKICRowCount_get_byERN]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 17 June 2008
-- Description:	Search whether any exisitng HKIC No. in
--				Table "ServiceProvider" and Table "ServiceProviderStaging"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE [dbo].[proc_ServiceProviderHKICRowCount_get_byERN]
	@enrolment_ref_no char(15)
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================

-- Should be amended to data type for encrypted HKID
	DECLARE @hkid char(9)

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================
EXEC [proc_SymmetricKey_open]

	SELECT	@hkid = convert(varchar, DecryptByKey(Encrypt_Field1))
	FROM	ServiceProviderEnrolment
	WHERE	Enrolment_Ref_No = @enrolment_ref_no

	EXEC	proc_ServiceProviderHKICRowCount_get_byHKIC @hkid 

	/*SELECT (SELECT COUNT(1)
	FROM	ServiceProvider SP
	WHERE	SP.SP_HKID = @hkid)
	+
	(SELECT Count(1)
	FROM	ServiceProviderStaging SPS
	WHERE	SPS.SP_HKID = @hkid)*/

EXEC [proc_SymmetricKey_close]

END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderHKICRowCount_get_byERN] TO HCVU
GO
