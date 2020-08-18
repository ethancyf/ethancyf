IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionVoucherQuota_get_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionVoucherQuota_get_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:	    Chris YIM
-- Modified date:   20 Feb 2019
-- CR No.:			CRE20-005
-- Description:		Grant rights of WSEXT
-- =============================================
-- =============================================
-- Author:			Winnie SUEN
-- Create date:		20 Nov 2018
-- CR No.:			CRE19-003 (Opt voucher capping)
-- Description:		Retrieve ProfessionVoucherQuota to cache
-- =============================================

CREATE PROCEDURE [dbo].[proc_ProfessionVoucherQuota_get_cache]
	
AS
BEGIN
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

	SELECT  
		Service_Category_Code
		,Effective_Dtm
		,Expiry_Dtm
		,Quota
		,Cumulative_Year
	FROM	
		ProfessionVoucherQuota
	ORDER BY 
		Service_Category_Code ASC, Effective_Dtm ASC

END
GO

GRANT EXECUTE ON [dbo].[proc_ProfessionVoucherQuota_get_cache] TO HCSP, HCVU, HCPUBLIC, WSEXT
GO

