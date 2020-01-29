IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransactionConfirm_get_cnt]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransactionConfirm_get_cnt]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================  
-- Modification History  
-- CR No.:			I-CRE17-007
-- Modified by:		Chris YIM
-- Modified date:	21 February 2018
-- Description:		Tune Performance
-- ============================================= 
-- =============================================  
-- Modification History  
-- CR No.:			CRE13-019-02 Extend HCVS to China
-- Modified by:		Chris YIM
-- Modified date:	26 February 2015
-- Description:		Add Input Parameter "Available_HCSP_SubPlatform"
-- =============================================  
-- =============================================  
-- Modification History  
-- CR No.:			CRE14-019
-- Modified by:		Lawrence TSANG
-- Modified date:	21 January 2015
-- Description:		Insert into [SProcPerformance] to record sproc performance
-- =============================================  
-- =============================================    
-- Modification History    
-- CR No.:   CRP12-XXX  
-- Modified by:  Timothy LEUNG  
-- Modified date: 18 Dec 2012  
-- Description:  Performance Tunning  
-- =============================================    
-- =============================================    
-- Modification History    
-- CR No.:   CRP12-001  
-- Modified by:  Koala CHENG  
-- Modified date: 10 Jan 2012  
-- Description:  Performance Tunning  
-- =============================================    
-- =============================================  
-- Modification History  
-- Modified by:  Koala CHENG  
-- CR No:   CRE11-024-02 HCVS Pilot Extension Part 2  
-- Modified date:  08-Oct-2011  
-- Description:  Modify to allow get count by VoucherTransaction.Record_Status  
-- =============================================  
-- =============================================  
-- Author:   Billy Lam  
-- Create date:  01-06-2008  
-- Description:  Get the count of VoucherTansaction to confirm  
-- =============================================  
  
Create Procedure dbo.proc_VoucherTransactionConfirm_get_cnt  
@SP_ID						CHAR(8),  
@DataEntry					VARCHAR(20)=NULL,  
@Record_Status				CHAR(1)=NULL,  
@Available_HCSP_SubPlatform CHAR(2),  
@Record_From_Dtm			DATETIME OUTPUT  
AS  
  
-- =============================================  
-- Declaration  
-- =============================================  
	DECLARE @Performance_Start_Dtm DATETIME
	SET @Performance_Start_Dtm = GETDATE()
	
	DECLARE @In_SP_ID						CHAR(8)
	DECLARE @In_DataEntry					VARCHAR(20)
	DECLARE @In_Record_Status				CHAR(1)
	DECLARE @In_Available_HCSP_SubPlatform	CHAR(2)
	SET @In_SP_ID = @SP_ID
	SET @In_DataEntry = @DataEntry
	SET @In_Record_Status = @Record_Status
	SET @In_Available_HCSP_SubPlatform = @Available_HCSP_SubPlatform

	DECLARE @Transaction_Dtm DATETIME  
	DECLARE @cnt1 INT  
  
-- =============================================  
-- Initization  
-- =============================================  
	SELECT @Transaction_Dtm = dateadd(day, 1, getdate())  
  
-- =============================================  
-- Get Result  
-- =============================================  
  
	SELECT 
		@cnt1 = COUNT(1), 
		@Record_From_Dtm= MIN(VT.Transaction_Dtm)  
	FROM 
		VoucherTransaction VT WITH (NOLOCK)
			LEFT JOIN SchemeClaim SC WITH (NOLOCK)
				ON VT.Scheme_Code = SC.Scheme_Code
	WHERE VT.SP_ID = @In_SP_ID  
		AND (@In_DataEntry IS NULL OR VT.DataEntry_By = @In_DataEntry)  
		AND VT.Transaction_Dtm < @Transaction_Dtm  
		AND VT.Record_Status = @In_Record_Status  
		AND (@In_Available_HCSP_SubPlatform IS NULL OR SC.Available_HCSP_SubPlatform = @In_Available_HCSP_SubPlatform)
  
-- =============================================  
-- Return Result  
-- =============================================  
  
	SELECT @cnt1 cnt  
  
 
	IF (SELECT Parm_Value1 FROM SystemParameters WHERE Parameter_Name = 'EnableSProcPerformCapture' AND Scheme_Code = 'ALL') = 'Y' BEGIN
		DECLARE @Performance_End_Dtm datetime
		SET @Performance_End_Dtm = GETDATE()
		DECLARE @Parameter varchar(255)
		SET @Parameter = @In_SP_ID + ',' + ISNULL(@In_DataEntry, '') + ',' + ISNULL(@In_Record_Status, '')
		
		EXEC proc_SProcPerformance_add 'proc_VoucherTransactionConfirm_get_cnt',
									   @Parameter,
									   @Performance_Start_Dtm,
									   @Performance_End_Dtm
		
	END


GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransactionConfirm_get_cnt] TO HCSP
GO
