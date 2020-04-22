IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SQLJobQueue_Execute]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SQLJobQueue_Execute]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		3 November 2010
-- Description:		Execute SQLJobQueue
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_SQLJobQueue_Execute]
AS BEGIN
-- =============================================
-- Declaration
-- =============================================
	DECLARE @Queue table (
		Queue_ID		varchar(10),
		Request_Dtm		datetime
	)

-- =============================================
-- Initialization
-- =============================================

-- Retrieve the Queue_ID to be run (Record_Status = 'A' - Active or 'F' - Fail)
	
	INSERT INTO @Queue (
		Queue_ID,
		Request_Dtm	
	)
	SELECT
		Queue_ID,
		Request_Dtm	
	FROM
		SQLJobQueue
	WHERE
		Record_Status IN ('A', 'F')
	ORDER BY
		Request_Dtm


-- =============================================
-- Return results
-- =============================================

	DECLARE My_Cursor CURSOR FOR
		SELECT
			Queue_ID
		FROM
			@Queue
		ORDER BY
			Request_Dtm
			
	DECLARE @Queue_ID	varchar(10)
	DECLARE @SP			varchar(100)
	DECLARE @SP_Parm	varchar(500)

	OPEN My_Cursor
	FETCH NEXT FROM My_Cursor INTO @Queue_ID
	
	WHILE @@Fetch_Status = 0 BEGIN
		
		-- ------------------------
		-- Step 1: Update SQLJobQueue (Set Record_Status to 'P' - Processing)
		-- ------------------------
		
		UPDATE
			SQLJobQueue
		SET
			Record_Status = 'P',
			Start_Dtm = GETDATE()
		WHERE
			Queue_ID = @Queue_ID
				AND Record_Status IN ('A', 'F')
				
		IF @@ROWCOUNT <> 1 GOTO FETCHNEXT

		-- ------------------------
		-- Step 2: Execute SQLJob
		-- ------------------------
		
		SELECT
			@SP = SP,
			@SP_Parm = SP_Parm
		FROM
			SQLJobQueue
		WHERE
			Queue_ID = @Queue_ID
			
		BEGIN TRANSACTION
			
		EXEC (@SP + ' ' + @SP_Parm)
		
		IF @@ERROR = 0 BEGIN
			
			-- ------------------------
			-- Step 3: Update SQLJobQueue (Set Record_Status to 'C' - Complete)
			-- ------------------------
		
			UPDATE
				SQLJobQueue
			SET
				Record_Status = 'C',
				Complete_Dtm = GETDATE()
			WHERE
				Queue_ID = @Queue_ID
					AND Record_Status = 'P'
				
			IF @@ROWCOUNT = 1 BEGIN
				COMMIT TRANSACTION
				
			END ELSE BEGIN
				ROLLBACK TRANSACTION
				
				-- ------------------------
				-- Step 4: Update SQLJobQueue (Rollback Record_Status to 'F' - Fail. Start_Dtm will be retained)
				-- ------------------------
		
				UPDATE
					SQLJobQueue
				SET
					Record_Status = 'F'
				WHERE
					Queue_ID = @Queue_ID
					
				GOTO FETCHNEXT
				
			END
				
		END ELSE BEGIN
			ROLLBACK TRANSACTION
			
			-- ------------------------
			-- Step 3: Update SQLJobQueue (Rollback Record_Status to 'F' - Fail. Start_Dtm will be retained)
			-- ------------------------
		
			UPDATE
				SQLJobQueue
			SET
				Record_Status = 'F'
			WHERE
				Queue_ID = @Queue_ID
				
			GOTO FETCHNEXT
				
		END

		FETCHNEXT:
		FETCH NEXT FROM My_Cursor INTO @Queue_ID

	END

	CLOSE My_Cursor
	DEALLOCATE My_Cursor


END
GO

GRANT EXECUTE ON [dbo].[proc_SQLJobQueue_Execute] TO HCVU
GO
