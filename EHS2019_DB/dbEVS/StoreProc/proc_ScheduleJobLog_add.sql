IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ScheduleJobLog_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ScheduleJobLog_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			INT13-0001
-- Modified by:		Koala CHENG
-- Modified date:	02 Jan 2013
-- Description:		Fix year table
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE11-029
-- Modified by:		Koala CHENG
-- Modified date:	14 Mar 2012
-- Description:		Add Column Start_Dtm, End_Dtm, Log_ID, Action_Key, Application_Server
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 11 Aug 2008
-- Description:	Insert Schedule Job Log
-- =============================================
CREATE PROCEDURE [dbo].[proc_ScheduleJobLog_add]
	@Action_Dtm	datetime,
	@Client_IP varchar(20),
	@Program_ID varchar(30),
	@Action varchar(30),
	@Status varchar(20),
	@Return_Description ntext,
	@Description ntext,
	@Start_Dtm datetime = NULL,
	@End_Dtm datetime = NULL,
	@Log_ID varchar(10) = NULL,
	@Action_Key varchar(20) = NULL
AS
BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================

DECLARE @curDate as datetime
DECLARE @year as varchar(2)
DECLARE @Application_Server as varchar(20)

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================

Set @curDate = GetDate()
Set @year = SUBSTRING(CAST(YEAR(@curDate) as Char(4)),3,2) 
Set @Application_Server = host_name()  

-- =============================================
-- Return results
-- =============================================


	IF @year = '08'
	BEGIN
		Insert Into ScheduleJobLog08
			(System_Dtm, Action_Dtm, Client_IP, Program_ID, Action, Status, Return_Description, Description, Start_Dtm, End_Dtm, Log_ID, Action_Key, Application_Server)
		VALUES
			(GetDate(),@Action_Dtm,@Client_IP,@Program_ID,@Action,@Status,@Return_Description,@Description, @Start_Dtm, @End_Dtm, @Log_ID, @Action_Key, @Application_Server)
	END

	ELSE IF @year = '09'
	BEGIN
		Insert Into ScheduleJobLog09
			(System_Dtm, Action_Dtm, Client_IP, Program_ID, Action, Status, Return_Description, Description, Start_Dtm, End_Dtm, Log_ID, Action_Key, Application_Server)
		VALUES
			(GetDate(),@Action_Dtm,@Client_IP,@Program_ID,@Action,@Status,@Return_Description,@Description, @Start_Dtm, @End_Dtm, @Log_ID, @Action_Key, @Application_Server)
	END

	ELSE IF @year ='10'
	BEGIN
		Insert Into ScheduleJobLog10
			(System_Dtm, Action_Dtm, Client_IP, Program_ID, Action, Status, Return_Description, Description, Start_Dtm, End_Dtm, Log_ID, Action_Key, Application_Server)
		VALUES
			(GetDate(),@Action_Dtm,@Client_IP,@Program_ID,@Action,@Status,@Return_Description,@Description, @Start_Dtm, @End_Dtm, @Log_ID, @Action_Key, @Application_Server)
	END

	ELSE IF @year ='11'
	BEGIN
		Insert Into ScheduleJobLog11
			(System_Dtm, Action_Dtm, Client_IP, Program_ID, Action, Status, Return_Description, Description, Start_Dtm, End_Dtm, Log_ID, Action_Key, Application_Server)
		VALUES
			(GetDate(),@Action_Dtm,@Client_IP,@Program_ID,@Action,@Status,@Return_Description,@Description, @Start_Dtm, @End_Dtm, @Log_ID, @Action_Key, @Application_Server)
	END

	ELSE IF @year ='12'
	BEGIN
		Insert Into ScheduleJobLog12
			(System_Dtm, Action_Dtm, Client_IP, Program_ID, Action, Status, Return_Description, Description, Start_Dtm, End_Dtm, Log_ID, Action_Key, Application_Server)
		VALUES
			(GetDate(),@Action_Dtm,@Client_IP,@Program_ID,@Action,@Status,@Return_Description,@Description, @Start_Dtm, @End_Dtm, @Log_ID, @Action_Key, @Application_Server)
	END

	ELSE IF @year ='13'
	BEGIN
		Insert Into ScheduleJobLog13
			(System_Dtm, Action_Dtm, Client_IP, Program_ID, Action, Status, Return_Description, Description, Start_Dtm, End_Dtm, Log_ID, Action_Key, Application_Server)
		VALUES
			(GetDate(),@Action_Dtm,@Client_IP,@Program_ID,@Action,@Status,@Return_Description,@Description, @Start_Dtm, @End_Dtm, @Log_ID, @Action_Key, @Application_Server)
	END

	ELSE IF @year ='14'
	BEGIN
		Insert Into ScheduleJobLog14
			(System_Dtm, Action_Dtm, Client_IP, Program_ID, Action, Status, Return_Description, Description, Start_Dtm, End_Dtm, Log_ID, Action_Key, Application_Server)
		VALUES
			(GetDate(),@Action_Dtm,@Client_IP,@Program_ID,@Action,@Status,@Return_Description,@Description, @Start_Dtm, @End_Dtm, @Log_ID, @Action_Key, @Application_Server)
	END

	ELSE IF @year ='15'
	BEGIN
		Insert Into ScheduleJobLog15
			(System_Dtm, Action_Dtm, Client_IP, Program_ID, Action, Status, Return_Description, Description, Start_Dtm, End_Dtm, Log_ID, Action_Key, Application_Server)
		VALUES
			(GetDate(),@Action_Dtm,@Client_IP,@Program_ID,@Action,@Status,@Return_Description,@Description, @Start_Dtm, @End_Dtm, @Log_ID, @Action_Key, @Application_Server)
	END

	ELSE IF @year ='16'
	BEGIN
		Insert Into ScheduleJobLog16
			(System_Dtm, Action_Dtm, Client_IP, Program_ID, Action, Status, Return_Description, Description, Start_Dtm, End_Dtm, Log_ID, Action_Key, Application_Server)
		VALUES
			(GetDate(),@Action_Dtm,@Client_IP,@Program_ID,@Action,@Status,@Return_Description,@Description, @Start_Dtm, @End_Dtm, @Log_ID, @Action_Key, @Application_Server)
	END

	ELSE IF @year ='17'
	BEGIN
		Insert Into ScheduleJobLog17
			(System_Dtm, Action_Dtm, Client_IP, Program_ID, Action, Status, Return_Description, Description, Start_Dtm, End_Dtm, Log_ID, Action_Key, Application_Server)
		VALUES
			(GetDate(),@Action_Dtm,@Client_IP,@Program_ID,@Action,@Status,@Return_Description,@Description, @Start_Dtm, @End_Dtm, @Log_ID, @Action_Key, @Application_Server)
	END

	ELSE IF @year ='18'
	BEGIN
		Insert Into ScheduleJobLog18
			(System_Dtm, Action_Dtm, Client_IP, Program_ID, Action, Status, Return_Description, Description, Start_Dtm, End_Dtm, Log_ID, Action_Key, Application_Server)
		VALUES
			(GetDate(),@Action_Dtm,@Client_IP,@Program_ID,@Action,@Status,@Return_Description,@Description, @Start_Dtm, @End_Dtm, @Log_ID, @Action_Key, @Application_Server)
	END

	ELSE IF @year = '19'
	BEGIN
		Insert Into ScheduleJobLog19
			(System_Dtm, Action_Dtm, Client_IP, Program_ID, Action, Status, Return_Description, Description, Start_Dtm, End_Dtm, Log_ID, Action_Key, Application_Server)
		VALUES
			(GetDate(),@Action_Dtm,@Client_IP,@Program_ID,@Action,@Status,@Return_Description,@Description, @Start_Dtm, @End_Dtm, @Log_ID, @Action_Key, @Application_Server)
	END

	ELSE IF @year ='20'
	BEGIN
		Insert Into ScheduleJobLog20
			(System_Dtm, Action_Dtm, Client_IP, Program_ID, Action, Status, Return_Description, Description, Start_Dtm, End_Dtm, Log_ID, Action_Key, Application_Server)
		VALUES
			(GetDate(),@Action_Dtm,@Client_IP,@Program_ID,@Action,@Status,@Return_Description,@Description, @Start_Dtm, @End_Dtm, @Log_ID, @Action_Key, @Application_Server)
	END

	ELSE IF @year ='21'
	BEGIN
		Insert Into ScheduleJobLog21
			(System_Dtm, Action_Dtm, Client_IP, Program_ID, Action, Status, Return_Description, Description, Start_Dtm, End_Dtm, Log_ID, Action_Key, Application_Server)
		VALUES
			(GetDate(),@Action_Dtm,@Client_IP,@Program_ID,@Action,@Status,@Return_Description,@Description, @Start_Dtm, @End_Dtm, @Log_ID, @Action_Key, @Application_Server)
	END

	ELSE IF @year ='22'
	BEGIN
		Insert Into ScheduleJobLog22
			(System_Dtm, Action_Dtm, Client_IP, Program_ID, Action, Status, Return_Description, Description, Start_Dtm, End_Dtm, Log_ID, Action_Key, Application_Server)
		VALUES
			(GetDate(),@Action_Dtm,@Client_IP,@Program_ID,@Action,@Status,@Return_Description,@Description, @Start_Dtm, @End_Dtm, @Log_ID, @Action_Key, @Application_Server)
	END

	ELSE IF @year ='23'
	BEGIN
		Insert Into ScheduleJobLog23
			(System_Dtm, Action_Dtm, Client_IP, Program_ID, Action, Status, Return_Description, Description, Start_Dtm, End_Dtm, Log_ID, Action_Key, Application_Server)
		VALUES
			(GetDate(),@Action_Dtm,@Client_IP,@Program_ID,@Action,@Status,@Return_Description,@Description, @Start_Dtm, @End_Dtm, @Log_ID, @Action_Key, @Application_Server)
	END

	ELSE IF @year ='24'
	BEGIN
		Insert Into ScheduleJobLog24
			(System_Dtm, Action_Dtm, Client_IP, Program_ID, Action, Status, Return_Description, Description, Start_Dtm, End_Dtm, Log_ID, Action_Key, Application_Server)
		VALUES
			(GetDate(),@Action_Dtm,@Client_IP,@Program_ID,@Action,@Status,@Return_Description,@Description, @Start_Dtm, @End_Dtm, @Log_ID, @Action_Key, @Application_Server)
	END

	ELSE IF @year ='25'
	BEGIN
		Insert Into ScheduleJobLog25
			(System_Dtm, Action_Dtm, Client_IP, Program_ID, Action, Status, Return_Description, Description, Start_Dtm, End_Dtm, Log_ID, Action_Key, Application_Server)
		VALUES
			(GetDate(),@Action_Dtm,@Client_IP,@Program_ID,@Action,@Status,@Return_Description,@Description, @Start_Dtm, @End_Dtm, @Log_ID, @Action_Key, @Application_Server)
	END

	ELSE IF @year ='26'
	BEGIN
		Insert Into ScheduleJobLog26
			(System_Dtm, Action_Dtm, Client_IP, Program_ID, Action, Status, Return_Description, Description, Start_Dtm, End_Dtm, Log_ID, Action_Key, Application_Server)
		VALUES
			(GetDate(),@Action_Dtm,@Client_IP,@Program_ID,@Action,@Status,@Return_Description,@Description, @Start_Dtm, @End_Dtm, @Log_ID, @Action_Key, @Application_Server)
	END

	ELSE IF @year ='27'
	BEGIN
		Insert Into ScheduleJobLog27
			(System_Dtm, Action_Dtm, Client_IP, Program_ID, Action, Status, Return_Description, Description, Start_Dtm, End_Dtm, Log_ID, Action_Key, Application_Server)
		VALUES
			(GetDate(),@Action_Dtm,@Client_IP,@Program_ID,@Action,@Status,@Return_Description,@Description, @Start_Dtm, @End_Dtm, @Log_ID, @Action_Key, @Application_Server)
	END

	ELSE IF @year ='28'
	BEGIN
		Insert Into ScheduleJobLog28
			(System_Dtm, Action_Dtm, Client_IP, Program_ID, Action, Status, Return_Description, Description, Start_Dtm, End_Dtm, Log_ID, Action_Key, Application_Server)
		VALUES
			(GetDate(),@Action_Dtm,@Client_IP,@Program_ID,@Action,@Status,@Return_Description,@Description, @Start_Dtm, @End_Dtm, @Log_ID, @Action_Key, @Application_Server)
	END
END
GO

GRANT EXECUTE ON [dbo].[proc_ScheduleJobLog_add] TO HCVU
GO
