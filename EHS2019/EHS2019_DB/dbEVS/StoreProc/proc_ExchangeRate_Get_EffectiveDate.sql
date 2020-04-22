IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ExchangeRate_Get_EffectiveDate]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ExchangeRate_Get_EffectiveDate]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- ==========================================================================================
-- Author:	Chris YIM
-- CR No.:  CRE13-019-02
-- Create Date:	14 Jan 2015
-- Description:	Get exchange rate value from table - [ExchangeRate] by [Service_Date]
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_ExchangeRate_Get_EffectiveDate]
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
	
	SELECT TOP 1
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
	ORDER BY Effective_Date
				
END
GO

GRANT EXECUTE ON [dbo].[proc_ExchangeRate_Get_EffectiveDate] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_ExchangeRate_Get_EffectiveDate] TO HCSP
GO
