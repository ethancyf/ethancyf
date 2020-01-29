IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccountConfirm_get_cnt]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccountConfirm_get_cnt]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================  
-- Modification History  
-- CR No.:			I-CRE17-007
-- Modified by:		Chris YIM
-- Modified date:	15 February 2018
-- Description:		Tune Performance
-- =============================================  
-- =============================================  
-- Modification History  
-- CR No.:			CRE13-019-02 Extend HCVS to China
-- Modified by:		Chris YIM
-- Modified date:	03 March 2015
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
-- CR No.:   CRP12-007
-- Modified by:  Timothy LEUNG  
-- Modified date: 17 Dec 2012
-- Description:  Performance Tunning  
-- =============================================    
-- =============================================    
-- Modification History    
-- CR No.:   CRP12-001  
-- Modified by:  Helen  
-- Modified date: 11 Jan 2012  
-- Description:  Performance Tunning  
-- =============================================    
-- =============================================    
-- Modification History    
-- CR No.:   CRP12-001  
-- Modified by:  Helen  
-- Modified date: 10 Jan 2012  
-- Description:  Performance Tunning  
-- =============================================    
-- =============================================    
-- Modification History    
-- Modified by:  Kathy LEE   
-- Modified date: 24 Feb 2010  
-- Description:  Also include the account purpose is 'A'  
--     and create_by_bo is 'N'  
-- =============================================    
-- =============================================    
-- Author:   Billy Lam    
-- Create date:  01-06-2008    
-- Description:      
-- =============================================    
-- =============================================    
-- Modification History    
-- Modified by:   Kathy LEE   
-- Modified date: 29 Sep 2009  
-- Description:   Remove Voucher_Used  
-- =============================================    
CREATE Procedure proc_TempVoucherAccountConfirm_get_cnt
@SP_ID      char(8)  
, @Available_HCSP_SubPlatform	char(2)  
as begin    
set nocount on    
-- =============================================    
-- Declaration    
-- =============================================    
DECLARE @Performance_Start_Dtm datetime
SET @Performance_Start_Dtm = GETDATE()

DECLARE @In_SP_ID      char(8)
DECLARE @In_Available_HCSP_SubPlatform	char(2)
SET @In_SP_ID = @SP_ID
SET @In_Available_HCSP_SubPlatform = @Available_HCSP_SubPlatform

declare @Create_Dtm datetime    
-- =============================================    
-- Initization    
-- =============================================    
select @Create_Dtm = getdate()    
select @Create_Dtm = dateadd(day, 1, @Create_Dtm)  

	SELECT 
		COUNT(1) CNT 
	FROM 
		TempVoucherAccount TVA WITH (NOLOCK, INDEX(IX_TempVoucherAccount3)) 
			LEFT JOIN SchemeClaim SC WITH (NOLOCK)   
				ON TVA.Scheme_code = SC.Scheme_Code
	WHERE 
		TVA.Account_Purpose IN ('V', 'C', 'A')  
		AND TVA.Create_By_BO = 'N'    
		AND TVA.Record_Status = 'C'
		AND EXISTS 
			(SELECT 1 FROM VoucherAccountCreationLog WITH (NOLOCK, INDEX(IX_VoucherAccountCreationLOG2)) 
				WHERE Voucher_Acc_ID = TVA.Voucher_Acc_ID AND Voucher_Acc_Type = 'T' AND Transaction_Dtm < @Create_Dtm AND SP_ID = @In_SP_ID)
		AND NOT EXISTS 
			(SELECT 1 FROM VoucherTransaction WITH (NOLOCK, INDEX(IX_VoucherTransaction10)) 
				WHERE Temp_Voucher_Acc_ID = TVA.Voucher_Acc_ID)
		AND (@In_Available_HCSP_SubPlatform IS NULL OR SC.Available_HCSP_SubPlatform = @In_Available_HCSP_SubPlatform)

IF (SELECT Parm_Value1 FROM SystemParameters WHERE Parameter_Name = 'EnableSProcPerformCapture' AND Scheme_Code = 'ALL') = 'Y' BEGIN
	DECLARE @Performance_End_Dtm datetime
	SET @Performance_End_Dtm = GETDATE()
	
	EXEC proc_SProcPerformance_add 'proc_TempVoucherAccountConfirm_get_cnt',
								   @In_SP_ID,
								   @Performance_Start_Dtm,
								   @Performance_End_Dtm
END

end  
go

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccountConfirm_get_cnt] TO HCSP
GO
