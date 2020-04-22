IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementHold_upd]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementHold_upd]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	26 February 2018
-- CR No.:			I-CRE17-007
-- Description:		Performance Tuning
--					1. Store Transaction ID in temp table first rather than direct insert to [ReimbursementAuthTran] (Reduce [VoucherTransaction] lock time])
--					2. Then insert Transaction ID from temp table to [ReimbursementAuthTran]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	15 June 2015
-- CR No.:			CRE13-019-02
-- Description:		Extend HCVS to China
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	15 June 2015
-- CR No.:			INT15-0005
-- Description:		Avoid concurrent update in First Authorization. To prevent the following 2 situations:
--					(1) The current Cutoff Date has been reset
--					(2) The current Reimbursement has already been completed
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	24 March 2015
-- CR No.:			INT15-0002
-- Description:		Set the stored procedure to recompile each time
-- =============================================
-- =============================================
-- Author:			Clark Yip
-- Create date:		13 Aug 2009
-- Description:		Reimbursement Hold action
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	3 September 2009
-- Description:		Add criterion while inserting into [ReimbursementAuthTran]: [VoucherTransaction].[Transaction_ID] does not exist in [ReimbursementAuthTran].[Transaction_ID]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	30 September 2009
-- Description:		Remove column [VoucherTransaction].[Authorised_Status]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Clark YIP
-- Modified date:	07 Oct 2009
-- Description:		Refine the sql
-- =============================================

CREATE PROCEDURE [dbo].[proc_ReimbursementHold_upd]
	@cutoff_dtm		datetime,
	@current_user	varchar(20),
	@scheme_code	char(10),
	@reimburse_id	char(15)
WITH RECOMPILE
AS BEGIN
-- =============================================
-- Declaration
-- =============================================
	CREATE TABLE #Transaction(
		Transaction_ID CHAR(20)
	)
-- =============================================
-- Validation 
-- =============================================

--	(1) The current Cutoff Date has been reset

	IF (SELECT COUNT(1) FROM ReimbursementAuthorisation WHERE Reimburse_ID = @reimburse_id AND Authorised_Status = 'S' AND Record_Status = 'A') = 0 BEGIN
		RAISERROR('00011', 16, 1)
		RETURN @@ERROR
	END


--	(2) The current Reimbursement has already been completed

	IF (SELECT	COUNT(1)
		FROM	ReimbursementAuthorisation
		WHERE	Reimburse_ID = @reimburse_id
					AND Authorised_Status = 'R'
					AND Record_Status = 'A'
					AND Scheme_Code IN (
						SELECT Scheme_Code FROM SchemeClaim WHERE Record_Status = 'A' AND Reimbursement_Mode = (
							SELECT Reimbursement_Mode FROM SchemeClaim WHERE Scheme_Code = @scheme_code
						)
					)
		) <> 0 BEGIN
		RAISERROR('00011', 16, 1)
		RETURN @@ERROR
	END


-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

-- Step 1: Insert transaction id into temp table
	INSERT INTO   #Transaction(Transaction_ID)
	SELECT 
		VT.Transaction_ID AS [Transaction_ID]
	FROM
		VoucherTransaction VT 
	LEFT OUTER JOIN
		ReimbursementAuthTran RA ON VT.Transaction_ID = RA.Transaction_ID 
	WHERE 
		VT.Record_Status = 'A' 
		AND VT.Confirmed_Dtm <= @cutoff_dtm
			AND VT.Scheme_Code = @scheme_code	
			AND RA.Transaction_ID IS NULL
			
-- Step 2: Insert into [ReimbursementAuthTran]
	INSERT INTO [ReimbursementAuthTran] (
		Transaction_ID,
		Scheme_Code,
		Reimburse_ID,
		Authorised_Status,
		Authorised_Cutoff_Dtm,
		Authorised_Cutoff_By
	)
	SELECT 
		VT.Transaction_ID,
		@scheme_code,
		@reimburse_id,
		'P',
		@cutoff_dtm,
		@current_user
		
	FROM
		#Transaction VT
	LEFT OUTER JOIN
		ReimbursementAuthTran RA ON VT.Transaction_ID = RA.Transaction_ID
	WHERE 
		RA.Transaction_ID IS NULL	

-- Step 3: Insert into [ReimbursementAuthorisation]
	INSERT INTO [ReimbursementAuthorisation]
	   ([Authorised_Dtm]
	   ,[Authorised_Status]
	   ,[Scheme_Code]
	   ,[Authorised_By]           
	   ,[Record_Status]
	   ,[Reimburse_ID]
	   ,[Cutoff_Date]
	   ,[Create_By]
	   ,[Create_Dtm]
	   ,[Update_By]
	   ,[Update_Dtm]		   
	   )
	VALUES
	   (GETDATE()
	   ,'P'
	   ,@scheme_code
	   ,@current_user           
	   ,'A'
	   ,@reimburse_id
	   ,@cutoff_dtm
	   ,@current_user
	   ,GETDATE()
	   ,@current_user
	   ,GETDATE()
	   )
	   
-- =============================================
-- House Keeping
-- =============================================
DROP TABLE #Transaction

END
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementHold_upd] TO HCVU
GO
