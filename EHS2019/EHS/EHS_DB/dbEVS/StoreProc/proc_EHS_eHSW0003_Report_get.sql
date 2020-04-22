IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSW0003_Report_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSW0003_Report_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	15 Mar 2019
-- CR No.:			CRE18-015 (Enable PCV13 weekly report eHS(S)W003 upon request)
-- Description:		This sproc will be reuse for eHS(S)W0003 and eHS(S)U009
--					(1) Add input Param [@request_time], [@CutOff_Dtm] and [@Show_02_Report]
--					(2) Remove input Param [@Report_Dtm]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Winnie SUEN
-- Modified date:	12 Jun 2018
-- CR No.:			CRE18-004 (CIMS Vaccination Sharing)
-- Description:	  	(1) Add Column - [VoucherTransaction].[DH_Vaccine_Ref] an [DH_Vaccine_Ref_Status]
--					(2) Performance Tuning (Parameter Sniffing)
-- =============================================
-- =============================================
-- Author:			Marco CHOI
-- Create date:		04 Aug 2017
-- Description:		PCV13 Weekly Statistic Report
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_eHSW0003_Report_get] 
	--@Report_Dtm			DATETIME = NULL,
	@request_time			DATETIME = NULL,	-- The reference date to get @CutOff_Dtm. It's [Request_Dtm] from [FileGenerationQueue] Table (* Passed in from Excel Generator. When changing this field, plz also update the corresponding Excel Generator)
	@CutOff_Dtm				DATETIME = NULL,	-- Inclusive, The Cut Off Date. If defined, it will override the value from the @request_dtm
	@Show_02_Report			CHAR(1) = 'N'		-- (Y/N) Default 'N' to hide sub 02 Fail to Connect CMS/CIMS report
												-- eHSU0009 -> use default value; eHSW0003 -> input 'Y'

AS BEGIN
-- ===================================
-- Declaration
-- =================================== 
	DECLARE	@In_CutOff_Dtm DATETIME	
	SET @In_CutOff_Dtm = @CutOff_Dtm

	DECLARE @In_Show_02_Report CHAR(1)
	SET @In_Show_02_Report = @Show_02_Report

	------------------------------------------------------------------------------------------
	IF @request_time IS NULL
		SET @request_time = GETDATE()

	-- Ensure the time start from 00:00 (datetime compare logic use "<")
	DECLARE	@Report_Dtm DATETIME	

	IF @In_CutOff_Dtm IS NULL
		SET @Report_Dtm = CONVERT(varchar, @request_time, 106) 		
	ELSE
		SET @Report_Dtm = CONVERT(varchar, DATEADD(d, 1, @In_CutOff_Dtm), 106)-- "106" gives "dd MMM yyyy"  


	------------------------------------------------------------------------------------------

	Declare @strGenDtm varchar(50)    
	SET @strGenDtm = CONVERT(VARCHAR(11), GETDATE(), 111) + ' ' + CONVERT(VARCHAR(8), GETDATE(), 108)    
	SET @strGenDtm = LEFT(@strGenDtm, LEN(@strGenDtm)-3)    
	SELECT 'Report Generation Time: ' + @strGenDtm  


	
	SELECT 
		VT.Transaction_ID,
		VT.Transaction_Dtm,
		VT.Voucher_Acc_ID,
		VT.Temp_Voucher_Acc_ID,
		VT.Scheme_Code,
		VT.Service_Receive_Dtm,
		VT.High_Risk,
		VT.EHS_Vaccine_Ref,
		VT.HA_Vaccine_Ref,
		VT.Ext_Ref_Status,
		VT.DH_Vaccine_Ref,
		VT.DH_Vaccine_Ref_Status,
		D.Subsidize_Item_Code,
		VT.Record_Status AS Transaction_Status,
		'' AS Reimbursement_Status
	INTO #VT
	FROM      
		VoucherTransaction VT WITH (NOLOCK)
			INNER JOIN TransactionDetail D WITH (NOLOCK)
				ON VT.Transaction_ID = D.Transaction_ID   
				AND VT.Scheme_Code = D.Scheme_Code
	WHERE VT.Transaction_Dtm < @Report_Dtm
	AND VT.Record_Status NOT IN  
		(	SELECT Status_Value 
			FROM StatStatusFilterMapping 
			WHERE (report_id = 'ALL' OR report_id = 'eHSW0003')   
			AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Record_Status'   
			AND ((Effective_Date is null or Effective_Date <= @Report_Dtm) AND (Expiry_Date is null or Expiry_Date >= @Report_Dtm))
		)     
	AND (VT.Invalidation IS NULL OR VT.Invalidation NOT In   
			(	SELECT Status_Value 
				FROM StatStatusFilterMapping 
				WHERE (report_id = 'ALL' OR report_id = 'eHSW0003')   
				AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'  
				AND ((Effective_Date is null or Effective_Date <= @Report_Dtm) AND (Expiry_Date is null or Expiry_Date >= @Report_Dtm))
			)
		)  
	AND (D.Subsidize_Item_Code = 'PV' OR D.Subsidize_Item_Code = 'PV13')   
	AND (VT.Scheme_Code='VSS' OR VT.Scheme_Code='RVP')
	
	--
	
	--01 sub report
	EXEC proc_EHS_eHSW0003_01_Report @Report_Dtm	
	
	--02 sub report
	IF @In_Show_02_Report = 'Y'
	BEGIN
		EXEC proc_EHS_eHSW0003_02_Report @Report_Dtm
	END	
	
	--Remark
	SELECT '(A) Common Note(s) for the report' 
	UNION ALL
	SELECT '1. Transactions:'
	UNION ALL
	SELECT'   a. All claim transactions created under service providers (either created by back office users or service providers (or the delegated users))'
	UNION ALL
	SELECT '   b. Exclude those reimbursed transactions with invalidation status marked as Invalidated' 
	UNION ALL
	SELECT '   c. Exclude voided/deleted transactions' 

	DROP TABLE #VT
END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSW0003_Report_get] TO HCVU
GO

