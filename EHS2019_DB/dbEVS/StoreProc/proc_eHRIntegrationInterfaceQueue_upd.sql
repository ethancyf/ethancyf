IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_eHRIntegrationInterfaceQueue_upd]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_eHRIntegrationInterfaceQueue_upd]
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
-- Description:		Update Status of Token Notification
-- =============================================

CREATE PROCEDURE [dbo].[proc_eHRIntegrationInterfaceQueue_upd]
	@Queue_ID		VARCHAR(14),
	@Status			CHAR(1),
	@Start_Dtm		DATETIME,
	@Complete_Dtm	DATETIME,
	@Last_Fail_Dtm	DATETIME
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
	DECLARE @IN_Queue_ID		VARCHAR(14)
	DECLARE @IN_Status			CHAR(1)
	DECLARE @IN_Start_Dtm		DATETIME
	DECLARE @IN_Complete_Dtm	DATETIME
	DECLARE @IN_Last_Fail_Dtm	DATETIME

-- =============================================
-- Initialization
-- =============================================
	SET @IN_Queue_ID = @Queue_ID
	SET @IN_Status = @Status
	SET @IN_Start_Dtm = @Start_Dtm
	SET @IN_Complete_Dtm = @Complete_Dtm
	SET @IN_Last_Fail_Dtm = @Last_Fail_Dtm

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Process
-- =============================================

	IF @IN_Complete_Dtm IS NULL
		BEGIN
			UPDATE
				[eHRIntegrationInterfaceQueue]
			SET
				[Record_Status] = @IN_Status,
				[Start_Process_Dtm] = @IN_Start_Dtm,
				[Last_Fail_Process_Dtm] = @IN_Last_Fail_Dtm

			WHERE
				Queue_ID = @Queue_ID
		END
	ELSE
		BEGIN
			UPDATE
				[eHRIntegrationInterfaceQueue]
			SET
				[Record_Status] = @IN_Status,
				[Start_Process_Dtm] = @IN_Start_Dtm,
				[Complete_Process_Dtm] = @IN_Complete_Dtm
			WHERE
				Queue_ID = @Queue_ID
		END

END
GO

GRANT EXECUTE ON [dbo].[proc_eHRIntegrationInterfaceQueue_upd] TO HCVU
GO
