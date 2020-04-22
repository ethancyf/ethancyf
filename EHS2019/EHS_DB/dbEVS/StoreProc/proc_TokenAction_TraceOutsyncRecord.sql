IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TokenAction_TraceOutsyncRecord]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TokenAction_TraceOutsyncRecord]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		8 February 2017
-- CR No.:			CRE16-019 (To implement token sharing between eHS(S) and eHRSS)
-- Description:		Trace Outsync Record
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.:			
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_TokenAction_TraceOutsyncRecord]
	@From_Dtm		datetime,
	@To_Dtm			datetime
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
	DECLARE @EHRNotifySetShare table (
		Case_ID				int NOT NULL IDENTITY(1, 1),
		User_ID				char(20),
		Action_Dtm			datetime,
		Notification_Dtm	datetime
	)

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Gather data
-- =============================================

	INSERT INTO @EHRNotifySetShare (
		User_ID,
		Action_Dtm,
		Notification_Dtm
	)
	SELECT
		User_ID,
		Action_Dtm,
		Notification_Dtm
	FROM
		TokenAction
	WHERE
		User_ID IS NOT NULL
			AND Source_Party = 'EHR'
			AND Destination_Party = 'EHS'
			AND Action_Type = 'NOTIFYSETSHARE'
			AND Action_Remark = 'Y'
			AND Notification_Dtm >= @From_Dtm AND Notification_Dtm < @To_Dtm


-- =============================================
-- Process
-- =============================================

	SELECT
		DENSE_RANK() OVER (ORDER BY E.Case_ID) AS [Set_Share_Case_ID],
		E.User_ID AS [SP_ID],
		E.Action_Dtm AS [Set_Share_Action_Dtm],
		E.Notification_Dtm AS [Set_Share_Notification_Dtm],
		T.Action_Type AS [Suspicious_EHS_Action],
		T.Action_Dtm AS [Suspicious_EHS_Action_Dtm],
		T.Token_Serial_No,
		T.Token_Serial_No_Replacement,
		T.Action_Remark
	FROM
		@EHRNotifySetShare E
			INNER JOIN TokenAction T
				ON E.User_ID = T.User_ID
					AND T.Source_Party = 'EHS'
					AND T.Action_Type IN ('REPLACETOKEN', 'DELETETOKEN')
					AND E.Action_Dtm < T.Action_Dtm
					AND T.Action_Dtm < E.Notification_Dtm
	ORDER BY
		E.Case_ID,
		E.Action_Dtm


END
GO

GRANT EXECUTE ON [dbo].[proc_TokenAction_TraceOutsyncRecord] TO HCVU
GO
