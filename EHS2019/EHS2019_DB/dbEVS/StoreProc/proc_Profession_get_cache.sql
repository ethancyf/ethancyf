IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Profession_get_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Profession_get_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	19 Jun 2019
-- CR No.:			CRE19-006 (DHC)
-- Description:		Add Column [EForm_Avail]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	9 December 2014
-- CR No.:			CRE13-019-01
-- Description:		Support Simplified Chinese
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE12-001
-- Modified by:		Tony FUNG
-- Modified date:	07 Feb 2012
-- Description:		1. Grant permission to WSINT for PCDInterface
-- =============================================
-- =============================================
-- Author:		Tommy TSE
-- Create date: 1 Sep 2011
-- CR No.:		CRE11-024-01 (Enhancement on HCVS Extension Part 1)
-- Description:	Retrieve data from Profession Table
-- =============================================

-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE [dbo].[proc_Profession_get_cache]
	
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

	SELECT Service_Category_Code, Service_Category_Desc, Service_Category_Desc_Chi, Service_Category_Desc_CN, Enrol_Period_From, Enrol_Period_To, Claim_Period_From, Claim_Period_To, Service_Category_Code_SD, Service_Category_Desc_SD, Service_Category_Desc_SD_Chi, SD_Display_Seq, SD_Period_From, SD_Period_To, EForm_Avail
	FROM dbo.Profession
	ORDER BY Service_Category_Desc
END

GO

GRANT EXECUTE ON [dbo].[proc_Profession_get_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_Profession_get_cache] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_Profession_get_cache] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_Profession_get_cache] TO WSEXT
GO

GRANT EXECUTE ON [dbo].[proc_Profession_get_cache] TO WSINT
GO