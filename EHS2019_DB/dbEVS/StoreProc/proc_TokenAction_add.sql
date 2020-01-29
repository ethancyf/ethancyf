IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TokenAction_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TokenAction_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		19 October 2016
-- CR No.:			CRE16-019 (To implement token sharing between eHS(S) and eHRSS)
-- Description:		Add TokenAction
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.:			
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_TokenAction_add]
	@Source_Party					varchar(10),
	@Destination_Party				varchar(10),
	@Action_Type					varchar(30),
	@User_ID						char(20),
	@Token_Serial_No				varchar(20),
	@Token_Serial_No_Replacement	varchar(20),
	@Action_Remark					varchar(50),
	@Action_By_Schedule_Job			char(1),
	@Action_Result					char(1),
	@Action_Dtm						datetime,
	@Notification_Dtm				datetime,
	@Message_Timestamp				varchar(50),
	@Reference_Queue_ID				varchar(14)
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

	INSERT INTO TokenAction (
		System_Dtm,
		Source_Party,
		Destination_Party,
		Action_Type,
		User_ID,
		Token_Serial_No,
		Token_Serial_No_Replacement,
		Action_Remark,
		Action_By_Schedule_Job,
		Action_Result,
		Action_Dtm,
		Notification_Dtm,
		Message_Timestamp,
		Reference_Queue_ID
	) VALUES (
		GETDATE(),
		@Source_Party,
		@Destination_Party,
		@Action_Type,
		@User_ID,
		@Token_Serial_No,
		@Token_Serial_No_Replacement,
		@Action_Remark,
		@Action_By_Schedule_Job,
		@Action_Result,
		@Action_Dtm,
		@Notification_Dtm,
		@Message_Timestamp,
		@Reference_Queue_ID
	)


END
GO

GRANT EXECUTE ON [dbo].[proc_TokenAction_add] TO HCVU, WSEXT
GO
