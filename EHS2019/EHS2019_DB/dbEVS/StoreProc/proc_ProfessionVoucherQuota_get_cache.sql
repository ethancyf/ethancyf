IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionVoucherQuota_get_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionVoucherQuota_get_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.:			
-- Description:		
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

GRANT EXECUTE ON [dbo].[proc_ProfessionVoucherQuota_get_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_ProfessionVoucherQuota_get_cache] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_ProfessionVoucherQuota_get_cache] TO HCPUBLIC
GO
