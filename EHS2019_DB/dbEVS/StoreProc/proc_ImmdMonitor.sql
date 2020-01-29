IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ImmdMonitor]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ImmdMonitor]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 30 Dec 2008
-- Description:	Monitor Immd Schedule Job
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_ImmdMonitor]
AS
BEGIN
	SET NOCOUNT ON;
	
-- =============================================
-- Declaration
-- =============================================

DECLARE @monitor_Minute AS INT
SET @monitor_Minute = 32

DECLARE @year AS Varchar(2)
DECLARE @Current_Dtm AS DATETIME
SET @Current_Dtm = Getdate()


DECLARE @ActionTable Table 
(
	Action VARCHAR(30)
)

DECLARE @Program_ID VARCHAR(30)
SET @Program_ID = 'ImmdValidation'

DECLARE @Status VARCHAR(20)
SET @Status = 'Fail'

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================

SELECT @year = CONVERT(VARCHAR(2), @Current_Dtm, 12)

INSERT INTO @ActionTable (Action) VALUES ('Parse XML File')
INSERT INTO @ActionTable (Action) VALUES ('Parse Import XML')
INSERT INTO @ActionTable (Action) VALUES ('Process Immd Result')

-- =============================================
-- Return results
-- =============================================

IF @year = '08'
BEGIN
	IF (
		SELECT COUNT(*) FROM ScheduleJobLOG08
		WHERE
			[Program_ID] = @Program_ID And [Status] = @Status AND
			Action in (Select Action From @ActionTable) AND
			DATEDIFF(minute, System_Dtm, getdate()) <= @monitor_Minute
		) > 0
	BEGIN
		RAISERROR('00099', 16, 1)
	END
END
ELSE IF @year = '09'
BEGIN
	IF (
		SELECT COUNT(*) FROM ScheduleJobLOG09
		WHERE
			[Program_ID] = @Program_ID And [Status] = @Status AND
			Action in (Select Action From @ActionTable) AND
			DATEDIFF(minute, System_Dtm, getdate()) <= @monitor_Minute
		) > 0
	BEGIN
		RAISERROR('00099', 16, 1)
	END
END
ELSE IF @year = '10'
BEGIN
	IF (
		SELECT COUNT(*) FROM ScheduleJobLOG10
		WHERE
			[Program_ID] = @Program_ID And [Status] = @Status AND
			Action in (Select Action From @ActionTable) AND
			DATEDIFF(minute, System_Dtm, getdate()) <= @monitor_Minute
		) > 0
	BEGIN
		RAISERROR('00099', 16, 1)
	END
END
ELSE IF @year = '11'
BEGIN
	IF (
		SELECT COUNT(*) FROM ScheduleJobLOG11
		WHERE
			[Program_ID] = @Program_ID And [Status] = @Status AND
			Action in (Select Action From @ActionTable) AND
			DATEDIFF(minute, System_Dtm, getdate()) <= @monitor_Minute
		) > 0
	BEGIN
		RAISERROR('00099', 16, 1)
	END
END
ELSE IF @year = '12'
BEGIN
	IF (
		SELECT COUNT(*) FROM ScheduleJobLOG12
		WHERE
			[Program_ID] = @Program_ID And [Status] = @Status AND
			Action in (Select Action From @ActionTable) AND
			DATEDIFF(minute, System_Dtm, getdate()) <= @monitor_Minute
		) > 0
	BEGIN
		RAISERROR('00099', 16, 1)
	END
END
ELSE IF @year = '13'
BEGIN
	IF (
		SELECT COUNT(*) FROM ScheduleJobLOG13
		WHERE
			[Program_ID] = @Program_ID And [Status] = @Status AND
			Action in (Select Action From @ActionTable) AND
			DATEDIFF(minute, System_Dtm, getdate()) <= @monitor_Minute
		) > 0
	BEGIN
		RAISERROR('00099', 16, 1)
	END
END
ELSE IF @year = '14'
BEGIN
	IF (
		SELECT COUNT(*) FROM ScheduleJobLOG14
		WHERE
			[Program_ID] = @Program_ID And [Status] = @Status AND
			Action in (Select Action From @ActionTable) AND
			DATEDIFF(minute, System_Dtm, getdate()) <= @monitor_Minute
		) > 0
	BEGIN
		RAISERROR('00099', 16, 1)
	END
END
ELSE IF @year = '15'
BEGIN
	IF (
		SELECT COUNT(*) FROM ScheduleJobLOG15
		WHERE
			[Program_ID] = @Program_ID And [Status] = @Status AND
			Action in (Select Action From @ActionTable) AND
			DATEDIFF(minute, System_Dtm, getdate()) <= @monitor_Minute
		) > 0
	BEGIN
		RAISERROR('00099', 16, 1)
	END
END
ELSE IF @year = '16'
BEGIN
	IF (
		SELECT COUNT(*) FROM ScheduleJobLOG16
		WHERE
			[Program_ID] = @Program_ID And [Status] = @Status AND
			Action in (Select Action From @ActionTable) AND
			DATEDIFF(minute, System_Dtm, getdate()) <= @monitor_Minute
		) > 0
	BEGIN
		RAISERROR('00099', 16, 1)
	END
END
ELSE IF @year = '17'
BEGIN
	IF (
		SELECT COUNT(*) FROM ScheduleJobLOG17
		WHERE
			[Program_ID] = @Program_ID And [Status] = @Status AND
			Action in (Select Action From @ActionTable) AND
			DATEDIFF(minute, System_Dtm, getdate()) <= @monitor_Minute
		) > 0
	BEGIN
		RAISERROR('00099', 16, 1)
	END
END
ELSE IF @year = '18'
BEGIN
	IF (
		SELECT COUNT(*) FROM ScheduleJobLOG18
		WHERE
			[Program_ID] = @Program_ID And [Status] = @Status AND
			Action in (Select Action From @ActionTable) AND
			DATEDIFF(minute, System_Dtm, getdate()) <= @monitor_Minute
		) > 0
	BEGIN
		RAISERROR('00099', 16, 1)
	END
END
ELSE IF @year = '19'
BEGIN
	IF (
		SELECT COUNT(*) FROM ScheduleJobLOG19
		WHERE
			[Program_ID] = @Program_ID And [Status] = @Status AND
			Action in (Select Action From @ActionTable) AND
			DATEDIFF(minute, System_Dtm, getdate()) <= @monitor_Minute
		) > 0
	BEGIN
		RAISERROR('00099', 16, 1)
	END
END
ELSE IF @year = '20'
BEGIN
	IF (
		SELECT COUNT(*) FROM ScheduleJobLOG20
		WHERE
			[Program_ID] = @Program_ID And [Status] = @Status AND
			Action in (Select Action From @ActionTable) AND
			DATEDIFF(minute, System_Dtm, getdate()) <= @monitor_Minute
		) > 0
	BEGIN
		RAISERROR('00099', 16, 1)
	END
END
ELSE IF @year = '21'
BEGIN
	IF (
		SELECT COUNT(*) FROM ScheduleJobLOG21
		WHERE
			[Program_ID] = @Program_ID And [Status] = @Status AND
			Action in (Select Action From @ActionTable) AND
			DATEDIFF(minute, System_Dtm, getdate()) <= @monitor_Minute
		) > 0
	BEGIN
		RAISERROR('00099', 16, 1)
	END
END
ELSE IF @year = '22'
BEGIN
	IF (
		SELECT COUNT(*) FROM ScheduleJobLOG22
		WHERE
			[Program_ID] = @Program_ID And [Status] = @Status AND
			Action in (Select Action From @ActionTable) AND
			DATEDIFF(minute, System_Dtm, getdate()) <= @monitor_Minute
		) > 0
	BEGIN
		RAISERROR('00099', 16, 1)
	END
END
ELSE IF @year = '23'
BEGIN
	IF (
		SELECT COUNT(*) FROM ScheduleJobLOG23
		WHERE
			[Program_ID] = @Program_ID And [Status] = @Status AND
			Action in (Select Action From @ActionTable) AND
			DATEDIFF(minute, System_Dtm, getdate()) <= @monitor_Minute
		) > 0
	BEGIN
		RAISERROR('00099', 16, 1)
	END
END
ELSE IF @year = '24'
BEGIN
	IF (
		SELECT COUNT(*) FROM ScheduleJobLOG24
		WHERE
			[Program_ID] = @Program_ID And [Status] = @Status AND
			Action in (Select Action From @ActionTable) AND
			DATEDIFF(minute, System_Dtm, getdate()) <= @monitor_Minute
		) > 0
	BEGIN
		RAISERROR('00099', 16, 1)
	END
END
ELSE IF @year = '25'
BEGIN
	IF (
		SELECT COUNT(*) FROM ScheduleJobLOG25
		WHERE
			[Program_ID] = @Program_ID And [Status] = @Status AND
			Action in (Select Action From @ActionTable) AND
			DATEDIFF(minute, System_Dtm, getdate()) <= @monitor_Minute
		) > 0
	BEGIN
		RAISERROR('00099', 16, 1)
	END
END
ELSE IF @year = '26'
BEGIN
	IF (
		SELECT COUNT(*) FROM ScheduleJobLOG26
		WHERE
			[Program_ID] = @Program_ID And [Status] = @Status AND
			Action in (Select Action From @ActionTable) AND
			DATEDIFF(minute, System_Dtm, getdate()) <= @monitor_Minute
		) > 0
	BEGIN
		RAISERROR('00099', 16, 1)
	END
END
ELSE IF @year = '27'
BEGIN
	IF (
		SELECT COUNT(*) FROM ScheduleJobLOG27
		WHERE
			[Program_ID] = @Program_ID And [Status] = @Status AND
			Action in (Select Action From @ActionTable) AND
			DATEDIFF(minute, System_Dtm, getdate()) <= @monitor_Minute
		) > 0
	BEGIN
		RAISERROR('00099', 16, 1)
	END
END
ELSE IF @year = '28'
BEGIN
	IF (
		SELECT COUNT(*) FROM ScheduleJobLOG28
		WHERE
			[Program_ID] = @Program_ID And [Status] = @Status AND
			Action in (Select Action From @ActionTable) AND
			DATEDIFF(minute, System_Dtm, getdate()) <= @monitor_Minute
		) > 0
	BEGIN
		RAISERROR('00099', 16, 1)
	END
END

END
GO
