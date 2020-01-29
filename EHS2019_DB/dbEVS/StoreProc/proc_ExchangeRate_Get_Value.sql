IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ExchangeRate_Get_Value]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ExchangeRate_Get_Value]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- ==========================================================================================
-- Author:	Chris YIM
-- CR No.:  CRE13-019-02
-- Create Date:	08 Jan 2015
-- Description:	Get exchange rate value from table - [ExchangeRate] by [Service_Date]
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_ExchangeRate_Get_Value]
	@ServiceStart_Date datetime,
	@ServiceEnd_Date datetime
AS BEGIN
-- ============================================================
-- Declaration
-- ============================================================
-- ============================================================
-- Validation
-- ============================================================
-- ============================================================
-- Initialization
-- ============================================================
-- ============================================================
-- Return results
-- ============================================================

	IF @ServiceStart_Date IS NOT NULL
	BEGIN
		SELECT 
			ER_RECORD.ExchangeRate_ID,
			ER_RECORD.Effective_Date_From AS [Effective_Date],
			ER_RECORD.ExchangeRate_Value,
			ER_RECORD.Record_Status,
			ER_RECORD.Create_Dtm,
			ER_RECORD.Create_By,
			ER_RECORD.Update_Dtm,
			ER_RECORD.Update_By,
			ER_RECORD.Creation_Approve_Dtm,
			ER_RECORD.Creation_Approve_By,
			ER_RECORD.TSMP
		FROM
			(SELECT 
				ER_A.ExchangeRate_ID,
				ER_A.Effective_Date AS [Effective_Date_From],
				ER_B.Effective_Date AS [Effective_Date_To],
				ER_A.ExchangeRate_Value,
				ER_A.Record_Status,
				ER_A.Create_Dtm,
				ER_A.Create_By,
				ER_A.Update_Dtm,
				ER_A.Update_By,
				ER_A.Creation_Approve_Dtm,
				ER_A.Creation_Approve_By,
				ER_A.TSMP
			FROM		
				(SELECT
					ROW_NUMBER() OVER(ORDER BY ER.Effective_Date) AS 'Row_Index',
					ER.ExchangeRate_ID,
					ER.Effective_Date,
					ER.ExchangeRate_Value,
					ER.Record_Status,
					ER.Create_Dtm,
					ER.Create_By,
					ER.Update_Dtm,
					ER.Update_By,
					ER.Creation_Approve_Dtm,
					ER.Creation_Approve_By,
					ER.TSMP
				FROM
					ExchangeRate ER
				WHERE
					ER.Record_Status = 'A'
					AND ER.Effective_Date <= CONVERT(DATETIME,CONVERT(VARCHAR(10),GETDATE(),121))) AS ER_A
				LEFT JOIN
				(SELECT
					ROW_NUMBER() OVER(ORDER BY ER.Effective_Date) AS 'Row_Index',
					ER.Effective_Date
				FROM
					ExchangeRate ER
				WHERE
					ER.Record_Status = 'A'
					AND ER.Effective_Date <= CONVERT(DATETIME,CONVERT(VARCHAR(10),GETDATE(),121))) AS ER_B
				ON ER_A.Row_Index = ER_B.Row_Index - 1
			WHERE
				ER_B.Row_Index IS NOT NULL
			UNION
			SELECT
				*
			FROM
				(SELECT TOP 1
					ER.ExchangeRate_ID,
					ER.Effective_Date AS [Effective_Date_From],
					CONVERT(DATETIME,CONVERT(VARCHAR(10),DATEADD(yy,100,GETDATE()),121)) AS [Effective_Date_To],
					ER.ExchangeRate_Value,
					ER.Record_Status,
					ER.Create_Dtm,
					ER.Create_By,
					ER.Update_Dtm,
					ER.Update_By,
					ER.Creation_Approve_Dtm,
					ER.Creation_Approve_By,
					ER.TSMP
				FROM
					ExchangeRate ER
				WHERE
					ER.Record_Status = 'A'
					AND ER.Effective_Date <= CONVERT(DATETIME,CONVERT(VARCHAR(10),GETDATE(),121))
				ORDER BY ER.Effective_Date DESC) AS ER_TODAY
			) AS ER_RECORD
		WHERE
			[Effective_Date_From] <= @ServiceEnd_Date
			AND [Effective_Date_To] > @ServiceStart_Date
	END
END
GO

GRANT EXECUTE ON [dbo].[proc_ExchangeRate_Get_Value] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_ExchangeRate_Get_Value] TO HCSP
GO
