IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccountRectifyCode_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccountRectifyCode_get]
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
-- Modified date:	06 March 2015
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
-- Modified by:	    Kathy LEE
-- Modified date:	18 May 2010
-- Description:		1. Performance tuning
--					2. Remove counting for special accont
-- =============================================
-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 02 June 2008
-- Description:	Retrieve Temporary Account according to the validating status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Kathy LEE
-- Modified date:	21 Sep 2009
-- Description:		1. Remove @Scheme_code in the search route
-- =============================================
Create PROCEDURE [dbo].[proc_TempVoucherAccountRectifyCode_get]
	@SP_ID	char(15),
	@Available_HCSP_SubPlatform	char(2)
	
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
	DECLARE @Performance_Start_Dtm datetime
	SET @Performance_Start_Dtm = GETDATE()

	DECLARE @In_SP_ID	char(15)
	DECLARE @In_Available_HCSP_SubPlatform	char(2)
	SET @In_SP_ID = @SP_ID
	SET @In_Available_HCSP_SubPlatform = @Available_HCSP_SubPlatform

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
		COUNT(1) AS Record_Count
	FROM 
		(SELECT Voucher_Acc_ID, Scheme_Code FROM TempVoucherAccount WITH (NOLOCK) WHERE Create_By_BO = 'N' AND Account_Purpose <> 'O' AND Record_Status = 'I') TVA
			INNER JOIN
				(SELECT Voucher_Acc_ID FROM TempPersonalInformation WITH (NOLOCK) WHERE Validating IS NULL OR Validating <> 'Y') TP
					ON TVA.Voucher_Acc_ID = TP.Voucher_Acc_ID 
			INNER JOIN
				(SELECT Voucher_Acc_ID, SP_ID, SP_Practice_Display_Seq,Voucher_Acc_Type 
							FROM VoucherAccountCreationLOG WITH (NOLOCK) 
								WHERE SP_ID = @In_SP_ID 
				) VACL
					ON TVA.Voucher_Acc_ID = VACL.Voucher_Acc_ID
						AND VACL.Voucher_Acc_Type = 'T'
			LEFT JOIN 
				SchemeClaim SC WITH (NOLOCK)
					ON TVA.Scheme_Code = SC.Scheme_Code
	WHERE 
		EXISTS 
			(SELECT 1 FROM Practice WITH (NOLOCK) WHERE SP_ID = VACL.SP_ID AND Display_Seq = VACL.SP_Practice_Display_Seq)
		AND 
			(@In_Available_HCSP_SubPlatform IS NULL OR SC.Available_HCSP_SubPlatform = @In_Available_HCSP_SubPlatform)


	IF (SELECT Parm_Value1 FROM SystemParameters WHERE Parameter_Name = 'EnableSProcPerformCapture' AND Scheme_Code = 'ALL') = 'Y' BEGIN
		DECLARE @Performance_End_Dtm datetime
		SET @Performance_End_Dtm = GETDATE()
		
		EXEC proc_SProcPerformance_add 'proc_TempVoucherAccountRectifyCode_get',
									   @In_SP_ID,
									   @Performance_Start_Dtm,
									   @Performance_End_Dtm
	END


END
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccountRectifyCode_get] TO HCSP
GO
