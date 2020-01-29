IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ExchangeRateStaging_Get_TaskListCount]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ExchangeRateStaging_Get_TaskListCount]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- ==========================================================================================
-- Author:	Chris YIM
-- CR No.:  CRE13-019-02
-- Create Date:	13 Jan 2015
-- Description:	Get record from table - [ExchangeRateStaging] by [Record_Status]
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_ExchangeRateStaging_Get_TaskListCount]
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
	
	SELECT
		COUNT(1) AS [PendingApproval_Count]
	FROM
		ExchangeRateStaging
	WHERE
		Record_Status = @Record_Status

END
GO

GRANT EXECUTE ON [dbo].[proc_ExchangeRateStaging_Get_TaskListCount] TO HCVU
GO
