IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ExchangeRateStaging_Get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ExchangeRateStaging_Get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- ==========================================================================================
-- Author:	Chris YIM
-- CR No.:  CRE13-019-02
-- Create Date:	02 Jan 2015
-- Description:	Get record from table - [ExchangeRateStaging] by [Record_Status]
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_ExchangeRateStaging_Get]
	@ExchangeRate_ID varchar(8),
	@Record_Status AS CHAR(1)
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
	DECLARE @ExchangeRate_ID_ERS as varchar(8)
	DECLARE @Record_Type as char(1)
	
    SELECT
		@Record_Type = Record_Type,
		@ExchangeRate_ID_ERS = ExchangeRate_ID
    FROM
        ExchangeRateStaging
    WHERE
        Record_Status = @Record_Status
		AND (ExchangeRate_ID = @ExchangeRate_ID OR @ExchangeRate_ID IS NULL)

	IF @Record_Type = 'I'
	BEGIN
		SELECT
			ExchangeRateStaging_ID,
			Effective_Date,
			ExchangeRate_Value,
			Record_Status,
			Record_Type,
			ExchangeRate_ID,
			Create_Dtm,
			Create_By,
			Approve_Dtm,
			Approve_By,
			Reject_Dtm,
			Reject_By,
			Delete_Dtm,
			Delete_By,
			TSMP
		FROM
			ExchangeRateStaging
		WHERE
			Record_Status = @Record_Status
			AND (ExchangeRate_ID = @ExchangeRate_ID OR @ExchangeRate_ID IS NULL)
		ORDER BY
			ExchangeRateStaging_ID DESC
	END

	IF @Record_Type = 'D'
	BEGIN
		SELECT
			ERS_D.ExchangeRateStaging_ID,
			ERS_D.Effective_Date,
			ERS_D.ExchangeRate_Value,
			ERS_D.Record_Status,
			ERS_D.Record_Type,
			ERS_D.ExchangeRate_ID,
			ER_I.Create_Dtm,
			ER_I.Create_By,
			ER_I.Creation_Approve_Dtm AS [Approve_Dtm],
			ER_I.Creation_Approve_By AS [Approve_By],
			ERS_D.Reject_Dtm,
			ERS_D.Reject_By,
			ERS_D.Create_Dtm AS [Delete_Dtm],
			ERS_D.Create_By AS [Delete_By],
			ERS_D.TSMP
		FROM
			ExchangeRateStaging ERS_D
				LEFT JOIN ExchangeRate ER_I
					ON ERS_D.ExchangeRate_ID = ER_I.ExchangeRate_ID
		WHERE
			ERS_D.Record_Type = 'D'
			AND ERS_D.ExchangeRate_ID = @ExchangeRate_ID_ERS 
		ORDER BY
			ExchangeRateStaging_ID DESC
	END
END
GO

GRANT EXECUTE ON [dbo].[proc_ExchangeRateStaging_Get] TO HCVU
GO
