IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSM0007_01_VSSPCV13_Reimbursement_HCSP]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSM0007_01_VSSPCV13_Reimbursement_HCSP]
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
-- Author:			Marco CHOI
-- Create date:		24 Aug 2017
-- Description:		VSS PCV13 Reimbursement Report 
-- =============================================

CREATE Procedure [proc_EHS_eHSM0007_01_VSSPCV13_Reimbursement_HCSP]  
	@reimburse_id						VARCHAR(15) = NULL
AS    
BEGIN    
	SET NOCOUNT ON;    
	
	--Declaration
	DECLARE @scheme					VARCHAR(10)
	DECLARE @subsidize_code			VARCHAR(10)
	DECLARE @Reimburse_Cutoff_Date	DATETIME
	DECLARE @Latest_Cutoff_Date		DATETIME
	DECLARE @Report_StartDtm		DATETIME
	DECLARE @Report_EndDtm			DATETIME

	SET @scheme = 'VSS'
	SET @subsidize_code = 'PV13'

	DECLARE @DataTable TABLE (
		R			INT,
		C			INT,
		Txt			NVARCHAR(300)
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
	)
	--
	DECLARE @R	INT
	DECLARE @C	INT
	DECLARE @i	INT
		
	DECLARE @FrameTable TABLE (
		C			INT,
		ColName		VARCHAR(5)
	)


	--Initiation
	DELETE @DataTable
	DELETE @FrameTable

	SET @i = 1

	WHILE @i <= 40 BEGIN
		INSERT INTO @FrameTable (C, ColName) VALUES (@i, 'C' + CONVERT(VARCHAR, @i))
		SET @i = @i + 1
	END

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
	
	--GET reimbursement date range
	SELECT TOP 24 Reimburse_ID, CutOff_Date
	INTO #ReimburseTranDateRange
	FROM ReimbursementAuthorisation 
	WHERE Authorised_Status ='R'
		AND Record_Status ='A'
		AND Scheme_Code = @scheme
		AND CutOff_Date >= @Report_StartDtm
		AND CutOff_Date < @Report_EndDtm
	ORDER BY CutOff_Date DESC
		
	--Get latest cutoff date	
	SELECT TOP 1 @Latest_Cutoff_Date = CutOff_Date
	FROM #ReimburseTranDateRange
	ORDER BY CutOff_Date DESC
					
	--Insert Report Period
	INSERT INTO @DataTable (R, C, Txt) 
	SELECT 1, 1, 'Reimbursement Cutoff date: ' + FORMAT(@Report_StartDtm, 'yyyy/MM/dd') +' to ' + FORMAT(@Latest_Cutoff_Date, 'yyyy/MM/dd')
	INSERT INTO @DataTable (R, C, Txt) SELECT 2, 1, ''

	--GET reimbursement data set
	SELECT 
		R.Reimburse_ID
		,R.CutOff_Date
		,VT.SP_ID
		,VT.Practice_Display_Seq
		,COUNT(RT.Transaction_ID) AS TxCount
	INTO #ReimburseTran
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
	WHERE R.CutOff_Date >= @Report_StartDtm
		AND R.CutOff_Date < @Report_EndDtm
		AND R.Scheme_Code = @scheme
		AND T.Subsidize_Item_Code = @subsidize_code
		AND R.Authorised_Status ='R'
		AND RT.Authorised_Status ='R'
		--AND VT.Record_Status ='R'
		AND R.Record_Status= 'A'		   
		AND (VT.Invalidation IS NULL OR VT.Invalidation NOT IN   
				(	SELECT Status_Value 
					FROM StatStatusFilterMapping 
					WHERE (report_id = 'ALL' OR report_id = 'eHSM0007')   
					AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'  
					AND ((Effective_Date is null or Effective_Date < @Report_EndDtm) AND (Expiry_Date is null or Expiry_Date >= @Report_EndDtm))
				)
			) 
	GROUP BY R.Reimburse_ID
		,R.CutOff_Date
		,VT.SP_ID
		,VT.Practice_Display_Seq


	DECLARE @HvRecord INT
	SELECT @HvRecord = COUNT(1) 
	FROM #ReimburseTran

	IF @HvRecord >0 
	BEGIN
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
		FROM ServiceProvider SP
		INNER JOIN Practice P
		ON SP.SP_ID = P.SP_ID
		INNER JOIN MedicalOrganization MO
		ON MO.SP_ID = P.SP_ID
		AND MO.Display_Seq = P.MO_Display_Seq
		INNER JOIN (	
			SELECT DISTINCT SP_ID, Practice_Display_Seq
			FROM #ReimburseTran
		) RT
		ON RT.SP_ID = P.SP_ID
		AND RT.Practice_Display_Seq = P.Display_Seq


		--Data Insert
		--Heading
		SET @R = 3	
		SET @C = 12

		INSERT INTO @DataTable (R, C, Txt) VALUES (@R, 1, 'SPID (for internal use only)')
		INSERT INTO @DataTable (R, C, Txt) VALUES (@R, 2, 'Practice No. (for internal use only)')
		INSERT INTO @DataTable (R, C, Txt) VALUES (@R, 3, 'SP Name (In English)')
		INSERT INTO @DataTable (R, C, Txt) VALUES (@R, 4, 'SP Name (In Chinese)')
		INSERT INTO @DataTable (R, C, Txt) VALUES (@R, 5, 'MO Name (In English)')
		INSERT INTO @DataTable (R, C, Txt) VALUES (@R, 6, 'MO Name (In Chinese)')
		INSERT INTO @DataTable (R, C, Txt) VALUES (@R, 7, 'Practice Name (In English)')
		INSERT INTO @DataTable (R, C, Txt) VALUES (@R, 8, 'Practice Name (In Chinese)')
		INSERT INTO @DataTable (R, C, Txt) VALUES (@R, 9, 'Practice Address (In English)')
		INSERT INTO @DataTable (R, C, Txt) VALUES (@R, 10, 'Practice Address (In Chinese)')
		INSERT INTO @DataTable (R, C, Txt) VALUES (@R, 11, 'Phone No. of Practice')

		DECLARE Cursor1 CURSOR FOR 
		SELECT CutOff_Date
		FROM #ReimburseTranDateRange
		ORDER BY CutOff_Date ASC

		DECLARE @CursorCutOffDate1 DATETIME
		OPEN Cursor1
		FETCH NEXT FROM Cursor1 INTO @CursorCutOffDate1
		WHILE @@FETCH_STATUS = 0 
		BEGIN	
			INSERT INTO @DataTable (R, C, Txt) VALUES (@R, @C, 'Reimbursement cutoff date on ' + FORMAT(@CursorCutOffDate1, 'yyyy/MM/dd'))
			--
		
			SET @C = @C +1
			FETCH NEXT FROM Cursor1 INTO @CursorCutOffDate1
		END

		CLOSE Cursor1
		DEALLOCATE Cursor1

		INSERT INTO @DataTable (R, C, Txt) VALUES (@R, @C, 'Total')
		--======================================================================

		-- Content
		SET @R = @R + 1	

		OPEN SYMMETRIC KEY sym_Key DECRYPTION BY ASYMMETRIC KEY asym_Key

		DECLARE Cursor3 CURSOR FOR 
		SELECT SP_ID, Practice_No
		FROM #SP
		ORDER BY SP_ID ASC, Practice_No ASC

		DECLARE @CursorSPID VARCHAR(8)
		DECLARE @CursorPracticeNo SMALLINT

		OPEN Cursor3
		FETCH NEXT FROM Cursor3 INTO @CursorSPID, @CursorPracticeNo
		WHILE @@FETCH_STATUS = 0 
		BEGIN		
			INSERT INTO @DataTable (R, C, Txt) SELECT @R, 1,  SP_ID												FROM #SP WHERE SP_ID = @CursorSPID AND Practice_No = @CursorPracticeNo
			INSERT INTO @DataTable (R, C, Txt) SELECT @R, 2,  Practice_No										FROM #SP WHERE SP_ID = @CursorSPID AND Practice_No = @CursorPracticeNo
			INSERT INTO @DataTable (R, C, Txt) SELECT @R, 3,  CONVERT(VARCHAR(200), DecryptByKey(SP_Name))		FROM #SP WHERE SP_ID = @CursorSPID AND Practice_No = @CursorPracticeNo
			INSERT INTO @DataTable (R, C, Txt) SELECT @R, 4,  CONVERT(NVARCHAR(200), DecryptByKey(SP_Name_Chi))	FROM #SP WHERE SP_ID = @CursorSPID AND Practice_No = @CursorPracticeNo
			INSERT INTO @DataTable (R, C, Txt) SELECT @R, 5,  MO_Eng_Name										FROM #SP WHERE SP_ID = @CursorSPID AND Practice_No = @CursorPracticeNo
			INSERT INTO @DataTable (R, C, Txt) SELECT @R, 6,  MO_Chi_Name										FROM #SP WHERE SP_ID = @CursorSPID AND Practice_No = @CursorPracticeNo
			INSERT INTO @DataTable (R, C, Txt) SELECT @R, 7,  Practice_Name										FROM #SP WHERE SP_ID = @CursorSPID AND Practice_No = @CursorPracticeNo
			INSERT INTO @DataTable (R, C, Txt) SELECT @R, 8,  Practice_Name_Chi									FROM #SP WHERE SP_ID = @CursorSPID AND Practice_No = @CursorPracticeNo
			INSERT INTO @DataTable (R, C, Txt) SELECT @R, 9,  Practice_Address									FROM #SP WHERE SP_ID = @CursorSPID AND Practice_No = @CursorPracticeNo
			INSERT INTO @DataTable (R, C, Txt) SELECT @R, 10, Practice_Address_Chi								FROM #SP WHERE SP_ID = @CursorSPID AND Practice_No = @CursorPracticeNo
			INSERT INTO @DataTable (R, C, Txt) SELECT @R, 11, Phone_No											FROM #SP WHERE SP_ID = @CursorSPID AND Practice_No = @CursorPracticeNo
		
			SET @C = 12	
		
			DECLARE Cursor2 CURSOR FOR 
			SELECT Reimburse_ID, CutOff_Date
			FROM #ReimburseTranDateRange
			ORDER BY CutOff_Date ASC
		
			DECLARE @CursorReimburseID VARCHAR(15)
			DECLARE @CursorCutOffDate2 DATETIME
			OPEN Cursor2
			FETCH NEXT FROM Cursor2 INTO @CursorReimburseID, @CursorCutOffDate2
			WHILE @@FETCH_STATUS = 0 
			BEGIN
				DECLARE @COUNT INT
				SET @COUNT = 0
			 
				SELECT @COUNT = TxCount
				FROM #ReimburseTran
				WHERE Reimburse_ID = @CursorReimburseID 
					AND CutOff_Date = @CursorCutOffDate2
					AND SP_ID = @CursorSPID 
					AND Practice_Display_Seq = @CursorPracticeNo
			
				INSERT INTO @DataTable (R, C, Txt) 
				SELECT @R, @C, @COUNT
			
				SET @C = @C +1
				FETCH NEXT FROM Cursor2 INTO @CursorReimburseID, @CursorCutOffDate2
			END
			CLOSE Cursor2
			DEALLOCATE Cursor2
		
			--Cummulative Total		
			DECLARE @CUMCOUNT INT
			SET @CUMCOUNT = 0 

			SELECT @CUMCOUNT = ISNULL(SUM(TxCount),0)
			FROM #ReimburseTran
			WHERE SP_ID = @CursorSPID 
				AND Practice_Display_Seq = @CursorPracticeNo
			GROUP BY SP_ID, Practice_Display_Seq 	
		
			INSERT INTO @DataTable (R, C, Txt)
			SELECT @R, @C, @CUMCOUNT

			SET @R = @R + 1
			FETCH NEXT FROM Cursor3 INTO @CursorSPID, @CursorPracticeNo
		END
		CLOSE Cursor3
		DEALLOCATE Cursor3

		CLOSE SYMMETRIC KEY sym_Key

	END
	ELSE
	BEGIN
		INSERT INTO @DataTable (R, C, Txt) SELECT 3, 1, ''
		INSERT INTO @DataTable (R, C, Txt) SELECT 4, 1, 'There is no record in the reporting period'
	END
	--======================================================================


	--Result
	--SELECT 'Reimbursement Cutoff date: ' + FORMAT(@ReportStartDtm, 'yyyy/MM/dd') +' to ' + FORMAT(@Reimburse_Cutoff_Date, 'yyyy/MM/dd')

	SELECT
		ISNULL(C1, ''),  ISNULL(C2, ''),  ISNULL(C3, ''),  ISNULL(C4, ''),  ISNULL(C5, ''),  ISNULL(C6, ''),  
		ISNULL(C7, ''),  ISNULL(C8, ''),  ISNULL(C9, ''),  ISNULL(C10, ''), ISNULL(C11, ''), ISNULL(C12, ''), 
		ISNULL(C13, ''), ISNULL(C14, ''), ISNULL(C15, ''), ISNULL(C16, ''), ISNULL(C17, ''), ISNULL(C18, ''), 
		ISNULL(C19, ''), ISNULL(C20, ''), ISNULL(C21, ''), ISNULL(C22, ''), ISNULL(C23, ''), ISNULL(C24, ''), 
		ISNULL(C25, ''), ISNULL(C26, ''), ISNULL(C27, ''), ISNULL(C28, ''), ISNULL(C29, ''), ISNULL(C30, ''), 
		ISNULL(C31, ''), ISNULL(C32, ''), ISNULL(C33, ''), ISNULL(C34, ''), ISNULL(C35, ''), ISNULL(C36, '')
	FROM (
		SELECT
			D.R,
			F.ColName,
			D.Txt
		FROM
			@FrameTable F
		INNER JOIN @DataTable D
			ON F.C = D.C
		) P 
	PIVOT (
		MAX(Txt)
		FOR ColName IN (C1,  C2,  C3,  C4,  C5,  C6,
						C7,  C8,  C9,  C10, C11, C12, 
						C13, C14, C15, C16, C17, C18, 
						C19, C20, C21, C22, C23, C24, 
						C25, C26, C27, C28, C29, C30, 
						C31, C32, C33, C34, C35, C36)
	) AS PVT
	ORDER BY R ASC

	DROP TABLE #ReimburseTran
	DROP TABLE #ReimburseTranDateRange
	DROP TABLE #SP

END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSM0007_01_VSSPCV13_Reimbursement_HCSP] TO HCVU
GO

