IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSM0007_03_VSSPCV13_Reimbursement_Invalid_Raw]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSM0007_03_VSSPCV13_Reimbursement_Invalid_Raw]
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
-- Modification History
-- CR# :			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Author:			Marco CHOI
-- Create date:		24 Aug 2017
-- Description:		VSS PCV13 Reimbursement Report 
-- =============================================

CREATE Procedure [proc_EHS_eHSM0007_03_VSSPCV13_Reimbursement_Invalid_Raw]  
	@reimburse_id			VARCHAR(15) = NULL       
AS    
BEGIN    
	SET NOCOUNT ON;    

	--Declaration
	DECLARE @scheme						VARCHAR(10)
	DECLARE @subsidize_code 			VARCHAR(10)
	DECLARE @Reimburse_Cutoff_Date		DATETIME
	DECLARE @Latest_Cutoff_Date			DATETIME
	DECLARE @Report_StartDtm			DATETIME
	DECLARE @Report_EndDtm				DATETIME
	DECLARE @Invalidation_StartDtm		DATETIME
	DECLARE @Invalidation_EndDtm		DATETIME

	SET @scheme = 'VSS'
	SET @subsidize_code = 'PV13'
	
	DECLARE  @ResultTable TABLE (
		Display_Seq				INT IDENTITY(1,1),
		Value1					NVARCHAR(300) DEFAULT NULL,
		Value2					NVARCHAR(300) DEFAULT NULL,
		Value3					NVARCHAR(300) DEFAULT NULL,
		Value4					NVARCHAR(300) DEFAULT NULL,
		Value5					NVARCHAR(300) DEFAULT NULL,
		Value6					NVARCHAR(300) DEFAULT NULL,
		Value7					NVARCHAR(300) DEFAULT NULL,
		Value8					NVARCHAR(300) DEFAULT NULL,
		Value9					NVARCHAR(300) DEFAULT NULL,
		Value10					NVARCHAR(300) DEFAULT NULL,
		Value11					NVARCHAR(300) DEFAULT NULL,
		Value12					NVARCHAR(300) DEFAULT NULL,
		Value13					NVARCHAR(300) DEFAULT NULL,
		Value14					NVARCHAR(300) DEFAULT NULL,
		Value15					NVARCHAR(300) DEFAULT NULL
	)
	
	CREATE TABLE  #InvalidReimburseClaim (
		Reimburse_ID				VARCHAR(15)
		,CutOff_Date				DATETIME
		,Transaction_ID				VARCHAR(20)
		,SP_ID						VARCHAR(8)
		,Practice_Display_Seq		SMALLINT
		,Transaction_Dtm			DATETIME
		,Invalid_Confirm_Time		DATETIME
	)

	CREATE TABLE  #SP (
		SP_ID						VARCHAR(8)
		,Practice_No				SMALLINT	
		,SP_Name					VARBINARY(100)
		,SP_Name_Chi				VARBINARY(100)
		,MO_Eng_Name				NVARCHAR(200)
		,MO_Chi_Name				NVARCHAR(200)
		,Practice_Name				NVARCHAR(200)
		,Practice_Name_Chi			NVARCHAR(200)
		,Practice_Address			VARCHAR(300)
		,Practice_Address_Chi		NVARCHAR(300)
		,Phone_No					VARCHAR(20)
		,Transaction_ID				VARCHAR(20)
		,Transaction_Dtm			DATETIME
		,Reimbursement_Cutoff_Date	DATETIME
		,Invalid_Confirm_Time		DATETIME
	)

	--Set Report Date	
	IF @reimburse_id IS NOT NULL 
	BEGIN		
		SELECT @Reimburse_Cutoff_Date =  CutOff_Date 
		FROM ReimbursementAuthorisation 
		WHERE Authorised_Status ='R'
		AND Record_Status ='A'
		AND Scheme_Code = @scheme
		AND Reimburse_ID = @reimburse_id
	END
	ELSE
	BEGIN
		SELECT TOP 1 @Reimburse_Cutoff_Date =  CutOff_Date 
		FROM ReimbursementAuthorisation 
		WHERE Authorised_Status ='R'
		AND Record_Status ='A'
		AND Scheme_Code = @scheme
		ORDER BY CutOff_Date DESC
	END

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
	
	--Get latest Cutoff date
	SELECT TOP 1 @Latest_Cutoff_Date = CutOff_Date
	FROM ReimbursementAuthorisation 
	WHERE Authorised_Status ='R'
		AND Record_Status ='A'
		AND Scheme_Code = @scheme
		AND CutOff_Date >= @Report_StartDtm
		AND CutOff_Date < @Report_EndDtm
	ORDER BY CutOff_Date DESC	
	
	--Prepare table header	
	--INSERT INTO @ResultTable (Value1) SELECT 'eHS(S)M0007-03: Raw data of invalidated PCV13 claim under VSS with reimbursement cutoff date before ' + FORMAT(@Report_StartDtm, 'd MMM yyyy') 	
	--INSERT INTO @ResultTable (Value1) SELECT ''
	INSERT INTO @ResultTable (Value1) SELECT 'Invalidation confirmation date period: ' + FORMAT(@Report_StartDtm, 'yyyy/MM/dd') +' to ' + FORMAT(@Latest_Cutoff_Date, 'yyyy/MM/dd')	
	INSERT INTO @ResultTable (Value1) SELECT ''
	
	--Set Invalidation period
	SET @Invalidation_StartDtm = @Report_StartDtm
	SET @Invalidation_EndDtm = DATEADD(DAY, 1, CONVERT(DATE, @Latest_Cutoff_Date))

	--Get reimbursed transaction invalidation data set
	--HCSP Reimburse Claim
	INSERT INTO #InvalidReimburseClaim (
		Reimburse_ID				
		,CutOff_Date				
		,Transaction_ID				
		,SP_ID						
		,Practice_Display_Seq		
		,Transaction_Dtm			
		,Invalid_Confirm_Time		
	)
	SELECT 
		R.Reimburse_ID
		,R.CutOff_Date
		,RT.Transaction_ID
		,VT.SP_ID
		,VT.Practice_Display_Seq
		,VT.Transaction_Dtm
		,TI.Update_Dtm AS Invalid_Confirm_Time
	FROM ReimbursementAuthorisation R
	INNER JOIN ReimbursementAuthTran RT
	ON R.Reimburse_ID = RT.Reimburse_ID
	AND R.Scheme_Code = RT.Scheme_Code
	INNER JOIN VoucherTransaction VT
	ON VT.Transaction_ID = RT.Transaction_ID
	AND VT.Scheme_Code = RT.Scheme_Code
	INNER JOIN TransactionDetail T
	ON VT.Transaction_ID = T.Transaction_ID
	AND VT.Scheme_Code = T.Scheme_Code
	INNER JOIN TransactionInvalidation TI
	ON VT.Transaction_ID = TI.Transaction_ID
	WHERE R.CutOff_Date < @Invalidation_StartDtm
		AND R.Scheme_Code = @scheme
		AND T.Subsidize_Item_Code = @subsidize_code
		AND R.Authorised_Status ='R'
		AND RT.Authorised_Status ='R'
		--AND VT.Record_Status ='R'
		AND R.Record_Status= 'A'
		AND VT.Invalidation = 'I'
		AND TI.Record_Status = 'C'
		AND TI.Update_Dtm >= @Invalidation_StartDtm
		AND TI.Update_Dtm < @Invalidation_EndDtm
	--	
	--HCVU Reimburse Claim
	INSERT INTO #InvalidReimburseClaim (
		Reimburse_ID				
		,CutOff_Date				
		,Transaction_ID				
		,SP_ID						
		,Practice_Display_Seq		
		,Transaction_Dtm			
		,Invalid_Confirm_Time		
	)
	SELECT 
		NULL
		,MR.Approval_Dtm
		,VT.Transaction_ID
		,VT.SP_ID
		,VT.Practice_Display_Seq
		,VT.Transaction_Dtm
		,TI.Update_Dtm AS Invalid_Confirm_Time
	FROM VoucherTransaction VT
	INNER JOIN TransactionDetail T
	ON VT.Transaction_ID = T.Transaction_ID
	AND VT.Scheme_Code = T.Scheme_Code
	INNER JOIN ManualReimbursement MR
	ON MR.Transaction_ID = VT.Transaction_ID
	INNER JOIN TransactionInvalidation TI
	ON VT.Transaction_ID = TI.Transaction_ID
	WHERE MR.Approval_Dtm < @Invalidation_StartDtm
		AND VT.Scheme_Code = @scheme
		AND T.Subsidize_Item_Code = @subsidize_code
		--AND VT.Record_Status ='R'
		AND VT.Manual_Reimburse='Y'
		AND VT.Invalidation = 'I'
		AND TI.Record_Status = 'C'
		AND TI.Update_Dtm >= @Invalidation_StartDtm
		AND TI.Update_Dtm < @Invalidation_EndDtm
	--
	DECLARE @HvRecord INT

	SELECT @HvRecord = COUNT(1) 
	FROM #InvalidReimburseClaim 


	IF @HvRecord >0 
	BEGIN
		INSERT INTO @ResultTable (Value1, Value2, Value3, Value4, Value5, Value6, Value7, 	
								  Value8, Value9, Value10, Value11, Value12, Value13, Value14,
								  Value15)
		SELECT 
			'SPID (for internal use only)'
			,'Practice No. (for internal use only)'
			,'Invalidation ConfirmationTime'
			,'Transaction ID'	
			,'Transaction Time'	
			,'Reimbursement Cutoff Date'	
			,'SP Name (In English)'
			,'SP Name (In Chinese)'
			,'MO Name (In English)'
			,'MO Name (In Chinese)'
			,'Practice Name (In English)'
			,'Practice Name (In Chinese)'
			,'Practice Address (In English)'
			,'Practice Address (In Chinese)'
			,'Phone No. of Practice'
		--

		INSERT INTO #SP (	
				SP_ID					
				,Practice_No			
				,SP_Name				
				,SP_Name_Chi			
				,MO_Eng_Name			
				,MO_Chi_Name			
				,Practice_Name			
				,Practice_Name_Chi		
				,Practice_Address		
				,Practice_Address_Chi	
				,Phone_No	
				,Transaction_ID			
				,Transaction_Dtm	
				,Reimbursement_Cutoff_Date	
				,Invalid_Confirm_Time			
			)
		SELECT
			SP.SP_ID
			,P.Display_Seq 
			,Encrypt_Field2
			,Encrypt_Field3
			,MO.MO_Eng_Name
			,MO.MO_Chi_Name
			,P.Practice_Name
			,P.Practice_Name_Chi
			,(SELECT [dbo].[func_formatEngAddress](P.Room, P.[Floor], P.[Block], P.Building, P.District))
			,(SELECT [dbo].[func_format_Address_Chi](P.Room, P.[Floor], P.[Block], P.Building_Chi, P.District))
			,P.Phone_Daytime 
			,IR.Transaction_ID
			,IR.Transaction_Dtm
			,IR.CutOff_Date
			,IR.Invalid_Confirm_Time
		FROM ServiceProvider SP
		INNER JOIN Practice P
		ON SP.SP_ID = P.SP_ID
		INNER JOIN MedicalOrganization MO
		ON MO.SP_ID = P.SP_ID
		AND MO.Display_Seq = P.MO_Display_Seq
		INNER JOIN #InvalidReimburseClaim IR
		ON IR.SP_ID = P.SP_ID
		AND IR.Practice_Display_Seq = P.Display_Seq
		--
	
		EXEC [proc_SymmetricKey_open]

		INSERT INTO @ResultTable (Value1, Value2, Value3, Value4, Value5, Value6, Value7, 	
								  Value8, Value9, Value10, Value11, Value12, Value13, Value14,
								  Value15)

		SELECT	SP_ID					
				,Practice_No				
				,FORMAT(Invalid_Confirm_Time, 'yyyy/MM/dd hh:mm' )	
				,dbo.func_format_system_number(Transaction_ID)		
				,FORMAT(Transaction_Dtm, 'yyyy/MM/dd hh:mm' )	
				,FORMAT(Reimbursement_Cutoff_Date, 'yyyy/MM/dd' )
				,CONVERT(VARCHAR(200), DecryptByKey(SP_Name))					
				,CONVERT(NVARCHAR(200), DecryptByKey(SP_Name_Chi))			
				,MO_Eng_Name			
				,MO_Chi_Name			
				,Practice_Name			
				,Practice_Name_Chi		
				,Practice_Address		
				,Practice_Address_Chi	
				,Phone_No
		FROM #SP
		ORDER BY Invalid_Confirm_Time DESC, Transaction_Dtm DESC, SP_ID ASC, Practice_No ASC
	
		EXEC [proc_SymmetricKey_close]
	END
	ELSE
	BEGIN
		INSERT INTO @ResultTable (Value1) SELECT ''
		INSERT INTO @ResultTable (Value1) SELECT 'There is no record in the reporting period'
	END

	SELECT 
		ISNULL(Value1, '')					
		,ISNULL(Value2, '')					
		,ISNULL(Value3, '')					
		,ISNULL(Value4, '')					
		,ISNULL(Value5, '')					
		,ISNULL(Value6, '')					
		,ISNULL(Value7, '')					
		,ISNULL(Value8, '')					
		,ISNULL(Value9, '')					
		,ISNULL(Value10, '')				
		,ISNULL(Value11, '')					
		,ISNULL(Value12, '')					
		,ISNULL(Value13, '')					
		,ISNULL(Value14, '')				
		,ISNULL(Value15, '')					
	FROM @ResultTable
	ORDER BY Display_Seq

	DROP TABLE #InvalidReimburseClaim
	DROP TABLE #SP

END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSM0007_03_VSSPCV13_Reimbursement_Invalid_Raw] TO HCVU
GO

