IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSW0003_02_Report]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSW0003_02_Report]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:	    Winnie SUEN
-- Modified date:	12 Jun 2018
-- CR No.:			CRE18-004 (CIMS Vaccination Sharing)
-- Description:	  	Add display field [Failed to connect HA] and [Failed to connect DH]
-- =============================================
---- =============================================
---- Author:			Marco CHOI
---- Create date:		14 Sep 2017
---- Description:		PCV13 Weekly Statistic Report
---- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_eHSW0003_02_Report] 
	@Report_Dtm			DATETIME = NULL

--DECLARE @Report_Dtm			DATETIME = NULL
--SET @Report_Dtm = '2017-09-18'

AS BEGIN
-- ===================================
-- Declaration
-- ===================================

	IF @Report_Dtm IS NOT NULL BEGIN
		SELECT @Report_Dtm = CONVERT(varchar, @Report_Dtm, 106)
	END ELSE BEGIN
		SELECT @Report_Dtm = CONVERT(varchar, GETDATE(), 106) -- "106" gives "dd MMM yyyy"  
	END
	
	DECLARE @SchemeDate Datetime
	SET @SchemeDate = DATEADD(dd, -1, @Report_Dtm)

	CREATE TABLE #Result (
		Display_Seq				INT IDENTITY(1,1),
		Value1					varchar(200) DEFAULT NULL,
		Value2					varchar(200) DEFAULT NULL,
		Value3					varchar(200) DEFAULT NULL,
		Value4					varchar(200) DEFAULT NULL,
		Value5					varchar(200) DEFAULT NULL,
		Value6					varchar(200) DEFAULT NULL
	)

-- ===================================
-- Data Preparation
-- ===================================
	-- Patch the Reimbursement_Status         
	UPDATE #VT
	SET Reimbursement_Status = CASE RAT.Authorised_Status        
								WHEN 'R' THEN 'G'        
								ELSE RAT.Authorised_Status        
							   END        
	FROM #VT        
	INNER JOIN ReimbursementAuthTran RAT        
	ON #VT.Transaction_ID = RAT.Transaction_ID 
	      
	-- Patch the Transaction_Status
	UPDATE #VT        
	SET Transaction_Status = 'R'        
	WHERE Reimbursement_Status = 'G'  	
	
-- ===================================
-- Gather result
-- ===================================
	INSERT INTO #Result (Value1)
	SELECT 'Reporting period: '+ FORMAT(DATEADD(dd, -7, @Report_Dtm), 'yyyy/MM/dd') + ' to ' + FORMAT(@SchemeDate, 'yyyy/MM/dd')
		
	INSERT INTO #Result (Value1) SELECT ''
	
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5, Value6)
	SELECT	'Transaction ID', 
			'Transaction Time',	
			'Transaction Status',	
			'Reimbursement Status',
			'Failed to connect HA',
			'Failed to connect DH'
	
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5, Value6)
	SELECT 
		dbo.func_format_system_number(Transaction_ID), 
		FORMAT(Transaction_Dtm, 'yyyy-MM-dd HH:mm:ss'),
		SD1.Status_Description AS Transaction_Status,        
		ISNULL(SD2.Status_Description, '') AS Reimbursement_Status,
		IIF(ISNULL(HA_Vaccine_Ref,'') = 'CC', 'Y', 'N') [HA_Connect_Fail],
		IIF(ISNULL(DH_Vaccine_Ref,'') = 'CC', 'Y', 'N') [DH_Connect_Fail]
	FROM #VT VT
	INNER JOIN StatusData SD1
		ON VT.Transaction_Status = SD1.Status_Value      
		AND SD1.Enum_Class = 'ClaimTransStatus'      
	LEFT JOIN StatusData SD2        
		ON VT.Reimbursement_Status = SD2.Status_Value        
		AND SD2.Enum_Class = 'ReimbursementStatus' 
	WHERE 
		Transaction_Dtm >= DATEADD(dd, -7, @Report_Dtm)
		AND Transaction_Dtm < @Report_Dtm 
		AND (Subsidize_Item_Code='PV' OR Subsidize_Item_Code='PV13')
		AND (HA_Vaccine_Ref='CC' OR DH_Vaccine_Ref = 'CC')
	ORDER BY Transaction_Dtm ASC
	
-- ===================================
-- Output result
-- ===================================
	SELECT	
		Value1, 
		Value2, 
		Value3, 
		Value4,
		Value5,
		Value6
	FROM #Result 
	ORDER By Display_Seq ASC
	
	--
	DROP TABLE #Result

END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSW0003_02_Report] TO HCVU
GO