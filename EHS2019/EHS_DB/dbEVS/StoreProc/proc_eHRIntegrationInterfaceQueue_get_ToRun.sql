IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_eHRIntegrationInterfaceQueue_get_ToRun]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_eHRIntegrationInterfaceQueue_get_ToRun]
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
-- Create date:		08 Feb 2017
-- CR No.:			CRE16-019
-- Description:		Get Token Notification
-- =============================================

CREATE PROCEDURE [dbo].[proc_eHRIntegrationInterfaceQueue_get_ToRun]
AS BEGIN

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
-- Process
-- =============================================

	OPEN SYMMETRIC KEY sym_Key
	DECRYPTION BY ASYMMETRIC KEY asym_Key

	SELECT
		Queue_ID,
		Queue_Type,
		CONVERT(NVARCHAR(MAX),DecryptByKey(Queue_Content)) AS Queue_Content,
		TA.[User_ID],
		TA.[Token_Serial_No],
		TA.[Token_Serial_No_Replacement],
		TA.[Action_Remark],
		CONVERT(VARCHAR(MAX),TA.[Action_Dtm],121) AS [Action_Dtm],
		TA.[Message_Timestamp]
	FROM
		eHRIntegrationInterfaceQueue EHRIIQ
			INNER JOIN 
			(SELECT
				[User_ID],
				[Token_Serial_No],
				[Token_Serial_No_Replacement],
				[Action_Remark],
				[Action_Dtm],
				[Message_Timestamp],				
				[Reference_Queue_ID]
			FROM			
				TokenAction
			WHERE
				Reference_Queue_ID IS NOT NULL
				AND Action_By_Schedule_Job = 'N'
			) TA
				ON EHRIIQ.Queue_ID = TA.Reference_Queue_ID
	WHERE
		(
			( EHRIIQ.[Record_Status] = 'P' AND EHRIIQ.[Complete_Process_Dtm] IS NULL ) OR 
			( EHRIIQ.[Record_Status] = 'E' AND EHRIIQ.[Complete_Process_Dtm] IS NULL)
		)

	CLOSE SYMMETRIC KEY sym_Key

END
GO

GRANT EXECUTE ON [dbo].[proc_eHRIntegrationInterfaceQueue_get_ToRun] TO HCVU
GO
