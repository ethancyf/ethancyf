IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_VaccinationClaimReportRVP_Stat_Read]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_VaccinationClaimReportRVP_Stat_Read]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Derek LEUNG
-- Modified date:	15 September 2010
-- Description:		Return date on 1st sheet. Update Template to meet new report standard
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Derek LEUNG
-- Modified date:		16 August 2010
-- Description:		Read RVP-PV / RVPSIV raw data
--                  		and remove reading RVP-HSIV raw data
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Derek LEUNG
-- Modified date:		28 June 2010
-- Description:		Read one more column for RVP-HSIV raw data (RCH Type)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:		15 March 2010
-- Description:		Read one more column for RVP-HSIV raw data
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:		4 March 2010
-- Description:		Read one more column for RVP-HSIV raw data
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:		29 January 2010
-- Description:		Modify HSIVSS section
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:		11 January 2010
-- Description:		Read more columns for HSIVSS report
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		28 October 2009
-- Description:		Generate report for the Vaccination (RVP only)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	6 November 2009
-- Description:		Read more columns for Txn by Age (RVP)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

Create PROCEDURE [dbo].[proc_EHS_VaccinationClaimReportRVP_Stat_Read]   
AS BEGIN
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
Declare @strGenDtm varchar(50)  
SET @strGenDtm = CONVERT(VARCHAR(11), GETDATE(), 111) + ' ' + CONVERT(VARCHAR(8), GETDATE(), 108)  
SET @strGenDtm = LEFT(@strGenDtm, LEN(@strGenDtm)-3)  
SELECT 'Report Generation Time: ' + @strGenDtm
-- --------------------------------------------------
-- From stored procedure:	proc_EHS_eHealthAccountClaimByDocumentType_RVP_Stat
-- To Excel sheet:			eHA (RVP)
-- --------------------------------------------------
	SELECT 
		Result_Value1,
		Result_Value2,
		Result_Value3,
		Result_Value4,
		Result_Value5,
		Result_Value6,
		Result_Value7,
		Result_Value8,
		Result_Value9,
		Result_Value10,
		Result_Value11
	FROM
		_eHealthAccountByDocumentType_RVP_Stat
	ORDER BY
		Display_Seq	

-- --------------------------------------------------
-- From stored procedure:	proc_EHS_RVPAgeReport_Stat
-- To Excel sheet:			Txn (RVP)
-- --------------------------------------------------
	SELECT
		Result_Value1,
		Result_Value2,
		Result_Value3,
		Result_Value4,
		Result_Value5,
		Result_Value6,
		Result_Value7,
		Result_Value8,
		Result_Value9,
		Result_Value10,
		Result_Value11,
		Result_Value12,
		Result_Value13,
		Result_Value14,
		Result_Value15,
		Result_Value16
	FROM  
		_EHS_RVPAgeReport_Stat
	ORDER BY
		Display_Seq

-- --------------------------------------------------
-- From stored procedure:	proc_EHS_RVPHSIVTransaction_Stat
-- To Excel sheet:			Txn raw (RVP)
-- --------------------------------------------------
/**
	SELECT
		Result_Value1,
		Result_Value2,
		Result_Value3,
		Result_Value4,
		Result_Value5,
		Result_Value6,
		Result_Value7,
		Result_Value8,
		Result_Value9,
		Result_Value10,
		Result_Value11,
		Result_Value12,
		Result_Value13, 
		Result_Value14
	FROM  
		_RVPHSIVTransaction_Stat
	ORDER BY
		Result_Seq
**/

-- --------------------------------------------------
-- From stored procedure:	proc_EHS_RVPSIVTransaction_Stat
-- To Excel sheet:			Txn raw (RVP)
-- --------------------------------------------------
	SELECT
		Result_Value1,
		Result_Value2,
		Result_Value3,
		Result_Value4,
		Result_Value5,
		Result_Value6,
		Result_Value7,
		Result_Value8,
		Result_Value9,
		Result_Value10,
		Result_Value11,
		Result_Value12,
		Result_Value13, 
		Result_Value14
	FROM  
		_RVPSIVTransaction_Stat
	ORDER BY
		Result_Seq

-- --------------------------------------------------
-- From stored procedure:	proc_EHS_RVPPVTransaction_Stat
-- To Excel sheet:			Txn raw (RVP)
-- --------------------------------------------------
	SELECT
		Result_Value1,
		Result_Value2,
		Result_Value3,
		Result_Value4,
		Result_Value5,
		Result_Value6,
		Result_Value7,
		Result_Value8,
		Result_Value9,
		Result_Value10,
		Result_Value11,
		Result_Value12,
		Result_Value13, 
		Result_Value14
	FROM  
		_RVPPVTransaction_Stat
	ORDER BY
		Result_Seq

END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_VaccinationClaimReportRVP_Stat_Read] TO HCVU
GO

