IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TokenAction_getOutSync]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TokenAction_getOutSync]
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
-- Author:			Chris YIM
-- Create date:		20 Feb 2017
-- CR No.:			CRE16-019
-- Description:		Determine Out-Sync Case in TokenAction
-- =============================================

CREATE PROCEDURE [dbo].[proc_TokenAction_getOutSync]
	@Period_Dtm DATETIME
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
	DECLARE @IN_Period_Dtm DATETIME

	DECLARE @Start_Dtm DATETIME
	DECLARE @END_Dtm DATETIME

-- =============================================
-- Initialization
-- =============================================
	SET @IN_Period_Dtm = @Period_Dtm

	IF @IN_Period_Dtm IS NOT NULL
		BEGIN
			SET @Start_Dtm = DATEFROMPARTS(DATEPART(YEAR,DATEADD(DAY, -1, @IN_Period_Dtm)), DATEPART(MONTH,DATEADD(DAY, -1, @IN_Period_Dtm)), DATEPART(DAY,DATEADD(DAY, -1, @IN_Period_Dtm)))
			SET @END_Dtm = DATEADD(DAY, 1, @Start_Dtm)
		END
	ELSE
		BEGIN
			SET @Start_Dtm = DATEFROMPARTS(DATEPART(YEAR,DATEADD(DAY, -1, GETDATE())), DATEPART(MONTH,DATEADD(DAY, -1, GETDATE())), DATEPART(DAY,DATEADD(DAY, -1, GETDATE())))
			SET @END_Dtm = DATEADD(DAY, 1, @Start_Dtm)
		END

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Process
-- =============================================

	SELECT 
		COUNT(DISTINCT TBL_OUT_SYNC.[User_ID]) AS [No_Of_OutSync_Case]
	FROM
		(SELECT
			[User_ID],
			[Action_Dtm],
			[Notification_Dtm]
		FROM 
			[TokenAction]
		WHERE
			[User_ID] IS NOT NULL
			AND [Source_Party] = 'EHR'
			AND [Destination_Party] = 'EHS'
			AND [Action_Type] = 'NOTIFYSETSHARE'
			AND [Action_Remark] = 'Y'
			AND ([Action_Result] = 'R' OR [Action_Result] = 'C')
			AND [Notification_Dtm] >= @Start_Dtm AND [Notification_Dtm] < @END_Dtm
		) AS TBL_OUT_SYNC

        LEFT OUTER JOIN	

		(SELECT
			[User_ID],
			[Action_Dtm]
		FROM 
			[TokenAction]
		WHERE
			[User_ID] IS NOT NULL
			AND [Source_Party] = 'EHS'
			AND ([Action_Type] = 'REPLACETOKEN' OR [Action_Type] = 'DELETETOKEN')
			AND [Action_Result] = 'C'
		) AS TBL_REPLACE_DEACTIVATE
		
		ON TBL_OUT_SYNC.[User_ID] = TBL_REPLACE_DEACTIVATE.[User_ID]

	WHERE
		TBL_REPLACE_DEACTIVATE.[User_ID] IS NOT NULL		
		AND TBL_OUT_SYNC.[Notification_Dtm] >= TBL_REPLACE_DEACTIVATE.[Action_Dtm]
		AND TBL_OUT_SYNC.[Action_Dtm] < TBL_REPLACE_DEACTIVATE.[Action_Dtm]
 
END
GO

GRANT EXECUTE ON [dbo].[proc_TokenAction_getOutSync] TO HCVU
GO
