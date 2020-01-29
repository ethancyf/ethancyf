IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_SDIR_Daily_stat_read]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_SDIR_Daily_stat_read]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Mattie LO
-- Create date: 21 October 2009
-- Description:	Retriveved Daily statistics on SDIR, # of Search in total, # of Search by distinct session_ID and # of distinct session_ID from temp table
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	29 October 2009
-- Description:		Change the date format to YYYY/MM/DD (code 111)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE Procedure [proc_EHS_SDIR_Daily_stat_read]
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Final Result
-- =============================================
CREATE TABLE #tmpTable
(
	[report_dtm]				[datetime] NULL,
	[total_search]				[int] NULL,
	[total_search_bySessionID]	[int] NULL,
	[total_sessionID]			[int] NULL
)
	
INSERT INTO #tmpTable	(
	[report_dtm],
	[total_search],
	[total_search_bySessionID],
	[total_sessionID]
)
SELECT TOP 14	convert(varchar, report_Dtm, 111) as report_Dtm,
				total_search,
				total_search_bySessionID,
				total_sessionID
FROM [_EHS_SDIR_Daily_stat] ORDER BY system_Dtm DESC

SELECT 
		CONVERT(varchar, report_Dtm, 111) as report_Dtm,
		total_search,
		total_search_bySessionID,
		total_sessionID		
FROM #tmpTable 
ORDER BY report_Dtm

DROP TABLE #tmpTable

END

GO

GRANT EXECUTE ON [dbo].[proc_EHS_SDIR_Daily_stat_read] TO HCVU
GO
