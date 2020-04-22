IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Statistics_STAT00005_GetReport]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Statistics_STAT00005_GetReport]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE13-018
-- Modified by:		Tommy LAM
-- Modified date:	21 Jan 2014
-- Description:		Change Column from "No. of vouchers" to "Total redeem amount"
-- =============================================
-- =============================================  
-- Modification History  
-- CR No.:			INT13-0011
-- Modified by:		Koala CHENG  
-- Modified date:	14 May 2013  
-- Description:		Fix database collation problem
-- =============================================  
-- =============================================
-- Author:			Tommy Tse
-- Create date:		22 Nov 2012
-- Description:		Get Report for Statistics - STAT00005
-- =============================================

CREATE PROCEDURE [dbo].[proc_Statistics_STAT00005_GetReport] 
	@statistic_row_type	VARCHAR(10) , --Year(Y), Month(M), Day(D)
	@ProfessionList		VARCHAR(100), --ENU,RCM,RCP,RDT,RMP,RMT,RNU,ROP,ROT,RPT,RRD
	@period_from		DATETIME, --YYYY-MM-DD
	@period_to			DATETIME, --YYYY-MM-DD
	@period_type		VARCHAR(1) --Transaction Date(T), Service Date(S)
AS BEGIN

-- =============================================
-- Validation Of Input Parameters
-- =============================================

	-- Empty Check

	IF @statistic_row_type = ''
		SET @statistic_row_type = NULL

	IF @ProfessionList = ''
		SET @ProfessionList = NULL

	IF @period_type = ''
		SET @period_type = NULL

	-- Range Check

	IF @period_from = ''
		SET @period_from = NULL

	IF @period_to = ''
		SET @period_to = NULL
		
	IF @period_from > @period_to
		RETURN

-- =============================================
-- Return Result Table
-- =============================================

	--Step 3. get name of reason for visit and sort the result 
	SELECT 
		Temp2.Period, 
		Temp2.Profession, 
		L1.Reason_L1 AS 'Level 1 reason of visit', 
		L2.Reason_L2 AS 'Level 2 reason of visit',
		Temp2.[No. of transactions], 
		CONVERT(INT, Temp2.[GrandTotalAmount]) AS 'Total redeem amount'
	FROM (
		--Step 2. count transaction and sum vouchers (Temp2)
		SELECT 
			Period, 
			Profession, 
			L1ROV, 
			L2ROV, 
			COUNT(1) AS 'No. of transactions', 
			SUM(Total_Amount) AS 'GrandTotalAmount' 
		FROM (
			--Step 1. list transaction inner join reason for visit (Temp)
			SELECT
				CASE
					WHEN @statistic_row_type = 'Y' THEN SUBSTRING(CONVERT(VARCHAR(10), VT.Transaction_Dtm, 20), 1, 4)
					WHEN @statistic_row_type = 'M' THEN SUBSTRING(CONVERT(VARCHAR(10), VT.Transaction_Dtm, 20), 1, 7)
					WHEN @statistic_row_type = 'D' THEN CONVERT(VARCHAR(10), VT.Transaction_Dtm, 20)
				END AS [Period],
				VT.Service_Type AS 'Profession', 
				TD.Total_Amount AS 'Total_Amount',
				TAFl1.AdditionalFieldValueCode AS 'L1ROV',
				TAFl2.AdditionalFieldValueCode AS 'L2ROV'
			FROM 
				VoucherTransaction VT
			LEFT JOIN TransactionDetail TD
				ON VT.Transaction_ID = TD.Transaction_ID
			INNER JOIN TransactionAdditionalField TAFl1
				ON VT.Transaction_ID = TAFl1.Transaction_ID
				AND TAFl1.AdditionalFieldID = 'Reason_for_Visit_L1'
			INNER JOIN TransactionAdditionalField TAFl2
				ON VT.Transaction_ID = TAFl2.Transaction_ID
				AND TAFl2.AdditionalFieldID = 'Reason_for_Visit_L2'			
			WHERE
				VT.Scheme_Code = 'HCVS' 
				AND VT.record_status IN ('U', 'A', 'S', 'P', 'V', 'R', 'B')
				AND ISNULL(invalidation, '') <> 'I'
				AND (@ProfessionList IS NULL OR EXISTS (SELECT * FROM func_split_string(@ProfessionList, ',') WHERE Item COLLATE Chinese_Taiwan_Stroke_CI_AS = VT.Service_Type))
				AND (@period_from IS NULL OR ((@period_type = 'T' AND 0<=DATEDIFF(day, @period_from, VT.Transaction_Dtm)) OR (@period_type = 'S' AND 0<=DATEDIFF(day, @period_from, VT.Service_Receive_Dtm))) ) 
				AND (@period_to IS NULL OR ((@period_type = 'T' AND 0<=DATEDIFF(day, VT.Transaction_Dtm, @period_to)) OR (@period_type = 'S' AND 0<=DATEDIFF(day, VT.Service_Receive_Dtm, @period_to))) )
			) Temp
		GROUP BY 
			Period, Profession, L1ROV, L2ROV 
		) Temp2
	LEFT JOIN ReasonForVisitL1 L1
		ON Temp2.L1ROV = L1.Reason_L1_Code 
		AND Temp2.Profession = L1.Professional_Code
	LEFT JOIN ReasonForVisitL2 L2
		ON Temp2.L2ROV = L2.Reason_L2_Code 
		AND Temp2.Profession = L2.Professional_Code
		AND L1.Reason_L1_Code = L2.Reason_L1_Code
	ORDER BY 
		Temp2.Period, Temp2.Profession, L1.Reason_L1, L2.Reason_L2

End

GO

GRANT EXECUTE ON [dbo].[proc_Statistics_STAT00005_GetReport] TO HCVU
GO
