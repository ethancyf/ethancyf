IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSM0007_02_VSSPCV13_Reimbursement_HCVU]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSM0007_02_VSSPCV13_Reimbursement_HCVU]
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

CREATE Procedure [proc_EHS_eHSM0007_02_VSSPCV13_Reimbursement_HCVU]  
	@reimburse_id						VARCHAR(15) = NULL
AS    
BEGIN    
	SET NOCOUNT ON;

	--Declaration
	DECLARE @scheme						VARCHAR(10)
	DECLARE @subsidize_code				VARCHAR(10)
	DECLARE @Latest_Cutoff_Date			DATETIME
	DECLARE @Reimburse_Cutoff_Date		DATETIME
	DECLARE @ReimburseLastDayofMonth 	BIT
	DECLARE @Report_StartDtm			DATETIME
	DECLARE @Report_EndDtm				DATETIME
	DECLARE @Approval_StartDtm			DATETIME
	DECLARE @Approval_EndDtm			DATETIME

	SET @scheme = 'VSS'
	SET @subsidize_code = 'PV13'

	DECLARE @DataTable TABLE (
		R			INT,
		C			INT,
		Txt			NVARCHAR(300)
	)

	CREATE TABLE  #SP (
		SP_ID					VARCHAR(8)
		,Practice_No			SMALLINT	
		,SP_Name				VARBINARY(100)
		,SP_Name_Chi			VARBINARY(100)
		,MO_Eng_Name			NVARCHAR(200)
		,MO_Chi_Name			NVARCHAR(200)
		,Practice_Name			NVARCHAR(200)
		,Practice_Name_Chi		NVARCHAR(200)
		,Practice_Address		VARCHAR(300)
		,Practice_Address_Chi	NVARCHAR(300)
		,Phone_No				VARCHAR(20)
	)
	--
	DECLARE @R	INT
	DECLARE @C	INT
	DECLARE @i	INT
		
	DECLARE @FrameTable TABLE (
		C			INT,
		ColName		VARCHAR(5)
	)

	CREATE TABLE #DateRangeTable (
		Seq		INT IDENTITY(1, 1)
		,Dtm	DATETIME	
	)


	--Initiation
	DELETE @DataTable
	DELETE @FrameTable

	SET @i = 1

	WHILE @i <= 24 BEGIN
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
		SELECT TOP 1 @Reimburse_Cutoff_Date = CutOff_Date 
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
	
	--Insert Report Period		
	INSERT INTO @DataTable (R, C, Txt) SELECT 1, 1, 'Approval period: ' + FORMAT(@Report_StartDtm, 'yyyy/MM/dd') +' to ' + FORMAT(@Latest_Cutoff_Date, 'yyyy/MM/dd')	
	INSERT INTO @DataTable (R, C, Txt) SELECT 2, 1, ''

	--Check Reimburse is last day of month
	SELECT @ReimburseLastDayofMonth = CASE WHEN DATEDIFF(d, @Latest_Cutoff_Date, DATEADD(d, -1, DATEADD(m, DATEDIFF(m, 0, @Latest_Cutoff_Date) + 1, 0))) = 0 THEN 1 ELSE 0 END

	--Set Report Date Range	
	DECLARE @Temp_Dtm		DATETIME
	SET @Temp_Dtm = @Latest_Cutoff_Date
	
	WHILE @Report_StartDtm <= @Temp_Dtm
	BEGIN
		INSERT INTO #DateRangeTable (Dtm) SELECT @Temp_Dtm
		SET @Temp_Dtm = DATEADD(s, -1, DATEADD(m, DATEDIFF(m, 0, @Temp_Dtm) , 0))
	END

	DELETE #DateRangeTable
	WHERE Seq > 12
	
	--Set Approval period
	SET @Approval_StartDtm = @Report_StartDtm
	SET @Approval_EndDtm = DATEADD(DAY, 1, CONVERT(DATE, @Latest_Cutoff_Date))
	
	--Get data set
	SELECT 
		VT.Transaction_ID
		,VT.SP_ID
		,VT.Practice_Display_Seq
		,MR.Approval_Dtm
	INTO #ReimburseClaim
	FROM VoucherTransaction VT
	INNER JOIN TransactionDetail T
	ON VT.Transaction_ID = T.Transaction_ID
	AND VT.Scheme_Code = T.Scheme_Code
	INNER JOIN ManualReimbursement MR
	ON MR.Transaction_ID = VT.Transaction_ID
	WHERE MR.Approval_Dtm >= @Approval_StartDtm
		AND MR.Approval_Dtm < @Approval_EndDtm
		AND VT.Scheme_Code = @scheme
		AND T.Subsidize_Item_Code = @subsidize_code
		AND VT.Record_Status ='R'
		AND VT.Manual_Reimburse='Y'		   
		AND (VT.Invalidation IS NULL OR VT.Invalidation NOT IN   
				(	SELECT Status_Value 
					FROM StatStatusFilterMapping 
					WHERE (report_id = 'ALL' OR report_id = 'eHSM0007')   
					AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'  
					AND ((Effective_Date is null or Effective_Date < @Approval_EndDtm) AND (Expiry_Date is null or Expiry_Date >= @Approval_EndDtm))
				)
			) 

	DECLARE @HvRecord INT
	SELECT @HvRecord = COUNT(1) 
	FROM #ReimburseClaim
	--

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
			FROM #ReimburseClaim
		) RC
		ON RC.SP_ID = P.SP_ID
		AND RC.Practice_Display_Seq = P.Display_Seq


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
		SELECT Seq, Dtm
		FROM #DateRangeTable
		ORDER BY Seq DESC

		DECLARE @CursorSeq1 DATETIME
		DECLARE @CursorDtm1 DATETIME
		OPEN Cursor1
		FETCH NEXT FROM Cursor1 INTO @CursorSeq1, @CursorDtm1
		WHILE @@FETCH_STATUS = 0 
		BEGIN

			IF @CursorSeq1 = 1 AND @ReimburseLastDayofMonth = 0 
			BEGIN
				INSERT INTO @DataTable (R, C, Txt) VALUES (@R, @C, 'Back Office claim approved in ' + FORMAT(@CursorDtm1, 'MMM yyyy') + ' (as at ' + FORMAT(@CursorDtm1, 'yyyy/MM/dd') + ')')			
			END
			ELSE
			BEGIN
				INSERT INTO @DataTable (R, C, Txt) VALUES (@R, @C, 'Back Office claim approved in ' + FORMAT(@CursorDtm1, 'MMM yyyy'))				 
			END
			--
		
			SET @C = @C +1
			FETCH NEXT FROM Cursor1 INTO @CursorSeq1, @CursorDtm1
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
			SELECT Seq, Dtm
			FROM #DateRangeTable
			ORDER BY Seq DESC
				
			DECLARE @CursorSeq2 DATETIME
			DECLARE @CursorDtm2 DATETIME
			OPEN Cursor2
			FETCH NEXT FROM Cursor2 INTO @CursorSeq2, @CursorDtm2
			WHILE @@FETCH_STATUS = 0 
			BEGIN
				DECLARE @COUNT INT
				SET @COUNT = 0
			 
				SELECT @COUNT = COUNT(Transaction_ID)
				FROM #ReimburseClaim
				WHERE SP_ID = @CursorSPID 
					AND Practice_Display_Seq = @CursorPracticeNo
					AND YEAR(Approval_Dtm) = YEAR(@CursorDtm2)
					AND MONTH(Approval_Dtm) = MONTH(@CursorDtm2)
				GROUP BY SP_ID, 
					Practice_Display_Seq	
				
				IF @CursorSeq2 = 1 AND @ReimburseLastDayofMonth = 0 
				BEGIN		
					SELECT @COUNT = COUNT(Transaction_ID)
					FROM #ReimburseClaim
					WHERE SP_ID = @CursorSPID 
						AND Practice_Display_Seq = @CursorPracticeNo
						AND Approval_Dtm >= DATEADD(month, DATEDIFF(month, 0, @CursorDtm2), 0)
						AND Approval_Dtm <= @CursorDtm2
					GROUP BY SP_ID, 
						Practice_Display_Seq		
				END

				INSERT INTO @DataTable (R, C, Txt) SELECT @R, @C, @COUNT
			
				SET @C = @C +1
				FETCH NEXT FROM Cursor2 INTO @CursorSeq2, @CursorDtm2
			END
			CLOSE Cursor2
			DEALLOCATE Cursor2
		
			--Cummulative Total		
			DECLARE @CUMCOUNT INT
			SET @CUMCOUNT = 0 

			SELECT @CUMCOUNT = COUNT(Transaction_ID)
			FROM #ReimburseClaim
			WHERE SP_ID = @CursorSPID 
				AND Practice_Display_Seq = @CursorPracticeNo
			GROUP BY SP_ID, 
				Practice_Display_Seq 	
		
			INSERT INTO @DataTable (R, C, Txt) SELECT @R, @C, @CUMCOUNT

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
	SELECT
		ISNULL(C1, ''),  ISNULL(C2, ''),  ISNULL(C3, ''),  ISNULL(C4, ''),  ISNULL(C5, ''),  ISNULL(C6, ''),  
		ISNULL(C7, ''),  ISNULL(C8, ''),  ISNULL(C9, ''),  ISNULL(C10, ''), ISNULL(C11, ''), ISNULL(C12, ''), 
		ISNULL(C13, ''), ISNULL(C14, ''), ISNULL(C15, ''), ISNULL(C16, ''), ISNULL(C17, ''), ISNULL(C18, ''), 
		ISNULL(C19, ''), ISNULL(C20, ''), ISNULL(C21, ''), ISNULL(C22, ''), ISNULL(C23, ''), ISNULL(C24, '')
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
						C19, C20, C21, C22, C23, C24)
	) AS PVT
	ORDER BY R ASC

	DROP TABLE #ReimburseClaim
	DROP TABLE #DateRangeTable
	DROP TABLE #SP

END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSM0007_02_VSSPCV13_Reimbursement_HCVU] TO HCVU
GO

