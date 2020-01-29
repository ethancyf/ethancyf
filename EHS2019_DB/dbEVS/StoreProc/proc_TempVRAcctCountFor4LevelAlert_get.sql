IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVRAcctCountFor4LevelAlert_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVRAcctCountFor4LevelAlert_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================  
-- Modification History  
-- CR No.:			I-CRE17-007
-- Modified by:		Chris YIM
-- Modified date:	14 February 2018
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
-- CR No.:			CRP12-001
-- Modified by:		Helen
-- Modified date:	10 Jan 2012
-- Description:		Performance Tunning
-- =============================================  
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	28 September 2009
-- Description:		Remove @scheme
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	27 May 2009
-- Description:		System Parameter Add Scheme Code
-- =============================================
-- =============================================
-- Author:			Clark Yip
-- Create date:		20 Oct 2008
-- Description:		Get Temp VRAcct Count by different alert level
-- =============================================
CREATE PROCEDURE [dbo].[proc_TempVRAcctCountFor4LevelAlert_get] 
	@sp_id	char(8),
	@level	smallint,
	@Available_HCSP_SubPlatform	char(2)
AS BEGIN
-- =============================================
-- Declaration
-- =============================================
	DECLARE @Performance_Start_Dtm datetime
	SET @Performance_Start_Dtm = GETDATE()
	
	DECLARE @In_SP_ID	char(8)
	DECLARE @In_Level	smallint
	DECLARE @In_Available_HCSP_SubPlatform	char(2)
	SET @In_SP_ID = @sp_id
	SET @In_Level = @level
	SET @In_Available_HCSP_SubPlatform = @Available_HCSP_SubPlatform

	DECLARE @day_level int
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================

	IF @In_Level=1
	BEGIN
		SELECT	@day_level = CONVERT(int, Parm_Value1) 
		FROM	SystemParameters 
		WHERE	Parameter_Name = 'Alert_L1_OutstandingDay' 
					AND Scheme_Code = 'ALL'	--0day
	END

	IF @In_Level=2
	BEGIN
		SELECT	@day_level = CONVERT(int, Parm_Value1)
		FROM	SystemParameters 
		WHERE	Parameter_Name = 'Alert_L2_OutstandingDay' 
					AND Scheme_Code = 'ALL'	--7day
	END

	IF @In_Level=3
	BEGIN
		SELECT	@day_level = CONVERT(int, Parm_Value1) 
		FROM	SystemParameters 
		WHERE	Parameter_Name = 'Alert_L3_OutstandingDay' 
					AND Scheme_Code = 'ALL'	--14day
	END

	IF @In_Level=4
	BEGIN
		SELECT	@day_level = CONVERT(int, Parm_Value1) 
		FROM	SystemParameters 
		WHERE	Parameter_Name = 'Alert_L4_OutstandingDay' 
					AND Scheme_Code = 'ALL'	--21day
	END
-- =============================================
-- Return results
-- =============================================

	SELECT
		TPV.First_Validate_Dtm	
	FROM
		(SELECT TVAPV.Voucher_Acc_ID, First_Validate_Dtm FROM TempVoucherAccPendingVerify TVAPV WITH (NOLOCK)
			INNER JOIN
				(SELECT Voucher_Acc_ID FROM VoucherAccountCreationLOG WITH (NOLOCK) WHERE SP_ID = @In_SP_ID) VACL
				ON TVAPV.Voucher_Acc_ID = VACL.Voucher_Acc_ID) TPV
			INNER JOIN TempVoucherAccount TVA  WITH (NOLOCK)
				ON TVA.Voucher_Acc_ID = TPV.Voucher_Acc_ID
					AND TVA.Record_Status = 'I' 
					AND (TVA.Account_Purpose = 'C' OR TVA.Account_Purpose = 'V')
			LEFT JOIN SchemeClaim SC WITH (NOLOCK)
				ON TVA.Scheme_Code = SC.Scheme_Code		
	WHERE   
		DATEDIFF(Day, TPV.First_Validate_Dtm, GETDATE()) >= @day_level
		AND (@In_Available_HCSP_SubPlatform IS NULL OR SC.Available_HCSP_SubPlatform = @In_Available_HCSP_SubPlatform)
	ORDER BY
		TPV.First_Validate_Dtm

	IF (SELECT Parm_Value1 FROM SystemParameters WHERE Parameter_Name = 'EnableSProcPerformCapture' AND Scheme_Code = 'ALL') = 'Y' BEGIN
		DECLARE @Performance_End_Dtm datetime
		SET @Performance_End_Dtm = GETDATE()
		DECLARE @Parameter varchar(255)
		SET @Parameter = @In_SP_ID + ',' + CONVERT(varchar, @In_Level)
		
		EXEC proc_SProcPerformance_add 'proc_TempVRAcctCountFor4LevelAlert_get',
									   @Parameter,
									   @Performance_Start_Dtm,
									   @Performance_End_Dtm
		
	END

--drop table #VoucherAccountCreationLOG
END
GO


GRANT EXECUTE ON [dbo].[proc_TempVRAcctCountFor4LevelAlert_get] TO HCSP
GO
