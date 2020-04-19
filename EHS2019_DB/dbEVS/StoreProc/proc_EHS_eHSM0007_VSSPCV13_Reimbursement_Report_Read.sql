IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSM0007_VSSPCV13_Reimbursement_Report_Read]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSM0007_VSSPCV13_Reimbursement_Report_Read]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
    
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	24 Mar 2020
-- CR No.:			INT20-0005
-- Description:		Fix incorrect display order
-- =============================================
-- =============================================
-- Author:			Marco CHOI
-- Create date:		24 Aug 2017
-- Description:		VSS PCV13 Reimbursement Report 
---- ============================================= 
    
CREATE PROCEDURE [dbo].[proc_EHS_eHSM0007_VSSPCV13_Reimbursement_Report_Read] 
	@request_time 			datetime,
	@Reimburse_ID			VARCHAR(15) = NULL       
AS BEGIN    
-- =============================================    
-- Declaration    

	DECLARE @ContentTable table (   
		Display_Seq		INT IDENTITY(1,1),     
		Value1			VARCHAR(500),      
		Value2			VARCHAR(500) 
	) 

	DECLARE @RemarkTable table (   
		Display_Seq		INT IDENTITY(1,1),     
		Value1			VARCHAR(2000) 
	)
		
	DECLARE @Reimburse_Cutoff_Date		DATETIME
	DECLARE @Report_StartDtm			DATETIME
	DECLARE @Report_EndDtm				DATETIME
-- =============================================    
-- =============================================    
-- Validation     
-- =============================================    
-- =============================================    
-- Initialization    
-- =============================================    
-- =============================================    
-- Return results    
	
	SELECT @Reimburse_Cutoff_Date = CutOff_Date 
	FROM ReimbursementAuthorisation 
	WHERE Reimburse_ID = @Reimburse_ID 
		AND Authorised_Status='R'

	IF @Reimburse_Cutoff_Date >= DATEFROMPARTS(YEAR(@Reimburse_Cutoff_Date),10,01) 
	BEGIN
		SET @Report_StartDtm = DATEFROMPARTS(YEAR(@Reimburse_Cutoff_Date),10,01)
		SET @Report_EndDtm = DATEADD(YEAR, 1, @Report_StartDtm)
	END
	ELSE
	BEGIN
		SET @Report_EndDtm = DATEFROMPARTS(YEAR(@Reimburse_Cutoff_Date),10,01)
		SET @Report_StartDtm = DATEADD(YEAR, -1, @Report_EndDtm)
	END

	SET @Report_EndDtm = DATEADD( DAY, -1, @Report_EndDtm)

	INSERT INTO @ContentTable (Value1, Value2)
	SELECT 'eHS(S)M0007-01', 'Report of reimbursed PCV13 claim under VSS group by service provider and practice (created by service provider)'
	INSERT INTO @ContentTable (Value1, Value2)
	SELECT 'eHS(S)M0007-02', 'Report of reimbursed PCV13 claim under VSS group by service provider and practice (created by back office)'
	INSERT INTO @ContentTable (Value1, Value2)
	SELECT 'eHS(S)M0007-03', 'Raw data of invalidated PCV13 claim under VSS with reimbursement cutoff date before ' + FORMAT(@Report_StartDtm, 'd MMM yyyy')
	
	INSERT INTO @ContentTable (Value1) SELECT ''
	INSERT INTO @ContentTable (Value1) SELECT ''
	INSERT INTO @ContentTable (Value1) SELECT 'Report Generation Time: ' + FORMAT(GETDATE(), 'yyyy/MM/dd HH:mm')    

	SELECT 	ISNULL(Value1, ''),	ISNULL(Value2,'')
	FROM @ContentTable  
	ORDER BY
		Display_Seq

-- --------------------------------------------------    
-- From stored procedure: [proc_EHS_eHSM0007_01_VSSPCV13_Reimbursement_HCSP]    
-- To Excel sheet:   01   
-- --------------------------------------------------    
	EXEC proc_EHS_eHSM0007_01_VSSPCV13_Reimbursement_HCSP  @Reimburse_ID  
 
-- --------------------------------------------------    
-- From stored procedure: [proc_EHS_eHSM0007_02_VSSPCV13_Reimbursement_HCVU]    
-- To Excel sheet:   02    
-- --------------------------------------------------    
	EXEC proc_EHS_eHSM0007_02_VSSPCV13_Reimbursement_HCVU  @Reimburse_ID  
  
-- --------------------------------------------------    
-- From stored procedure: [proc_EHS_eHSM0007_03_VSSPCV13_Reimbursement_Invalid_Raw]    
-- To Excel sheet:   03    
-- --------------------------------------------------    
	EXEC proc_EHS_eHSM0007_03_VSSPCV13_Reimbursement_Invalid_Raw  @Reimburse_ID  
	
-- --------------------------------------------------    
-- From stored procedure: []    
-- To Excel sheet:   Remark    
-- --------------------------------------------------   
	INSERT INTO @RemarkTable (Value1) SELECT '(A) Common Note(s) for the report'
	INSERT INTO @RemarkTable (Value1) SELECT '  1. Report date range:' 
	INSERT INTO @RemarkTable (Value1) SELECT '      a. Period is from 1 Oct up to 30 Sep next year'
	INSERT INTO @RemarkTable (Value1) SELECT '      b. Report is generated after completion of reimbursement and the cut-off end date for all worksheets is the latest reimbursement cut-off date'
	INSERT INTO @RemarkTable (Value1) SELECT ' '
	INSERT INTO @RemarkTable (Value1) SELECT '  2. Include PCV13 claim under VSS only'
	INSERT INTO @RemarkTable (Value1) SELECT '  3. Sub report details:'
	INSERT INTO @RemarkTable (Value1) SELECT '      a. Sub report 01'
	INSERT INTO @RemarkTable (Value1) SELECT '          - Include reimbursed VSS PCV13 claim with'
	INSERT INTO @RemarkTable (Value1) SELECT '              * reimbursement cutoff date between ' + FORMAT(@Report_StartDtm, 'd MMM yyyy') + ' to ' +  + FORMAT(@Report_EndDtm, 'd MMM yyyy') 
	INSERT INTO @RemarkTable (Value1) SELECT '          - Exclude invalidated claims'
	INSERT INTO @RemarkTable (Value1) SELECT '          - Exclude back office claims'
	INSERT INTO @RemarkTable (Value1) SELECT '          - Summary of claim total by reimbursement cut off date for each service provider and practice'
	INSERT INTO @RemarkTable (Value1) SELECT '              (may have more than 12 reimbursement cutoff date if having more than 1 reimbursement per month)'
	INSERT INTO @RemarkTable (Value1) SELECT ' '
	INSERT INTO @RemarkTable (Value1) SELECT '      b. Sub report 02'
	INSERT INTO @RemarkTable (Value1) SELECT '          - Include back office approved VSS PCV13 claim with'
	INSERT INTO @RemarkTable (Value1) SELECT '              * approval date between ' + FORMAT(@Report_StartDtm, 'd MMM yyyy') + ' to ' +  + FORMAT(@Report_EndDtm, 'd MMM yyyy') + ' (exclude pending approval claim)'
	INSERT INTO @RemarkTable (Value1) SELECT '          - Exclude invalidated claims'
	INSERT INTO @RemarkTable (Value1) SELECT '          - Group by claim approval month'
	INSERT INTO @RemarkTable (Value1) SELECT ' '
	INSERT INTO @RemarkTable (Value1) SELECT '      c. Sub report 03'
	INSERT INTO @RemarkTable (Value1) SELECT '          - Include invalidated VSS PCV13 claim with'
	INSERT INTO @RemarkTable (Value1) SELECT '              * invalidation confirmation date between ' + FORMAT(@Report_StartDtm, 'd MMM yyyy') + ' to ' +  + FORMAT(@Report_EndDtm, 'd MMM yyyy') + '; and'
	INSERT INTO @RemarkTable (Value1) SELECT '              * reimbursement cutoff date before ' + FORMAT(@Report_StartDtm, 'd MMM yyyy')

	SELECT Value1 FROM @RemarkTable 
	ORDER BY Display_Seq
    
-- =============================================    
END    
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSM0007_VSSPCV13_Reimbursement_Report_Read] TO HCVU
GO
