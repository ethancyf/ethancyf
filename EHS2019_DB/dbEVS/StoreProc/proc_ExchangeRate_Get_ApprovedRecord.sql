IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ExchangeRate_Get_ApprovedRecord]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ExchangeRate_Get_ApprovedRecord]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- ==========================================================================================
-- Author:	Chris YIM
-- CR No.:  CRE13-019-02
-- Create Date:	05 Jan 2015
-- Description:	Get record from table - [ExchangeRate] by [Record_Status]
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_ExchangeRate_Get_ApprovedRecord]
	@ExchangeRate_ID varchar(8),
	@Info_Type AS CHAR(1)
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

	IF @Info_Type = 'T'		-- Today Exchange Rate
	BEGIN
		SELECT
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
			AND ER.Effective_Date <= CONVERT(DATETIME,CONVERT(VARCHAR(10),GETDATE(),121))
		ORDER BY
			ER.Effective_Date DESC
	END
	ELSE IF @Info_Type = 'N'	-- Next Exchange Rate
	BEGIN
		SELECT
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
			AND ER.Effective_Date > CONVERT(DATETIME,CONVERT(VARCHAR(10),GETDATE(),121))
			AND (ER.ExchangeRate_ID = @ExchangeRate_ID OR @ExchangeRate_ID IS NULL)
		ORDER BY
			ER.Effective_Date
	END
END
GO

GRANT EXECUTE ON [dbo].[proc_ExchangeRate_Get_ApprovedRecord] TO HCVU
GO
