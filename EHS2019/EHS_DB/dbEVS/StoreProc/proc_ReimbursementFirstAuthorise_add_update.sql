IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementFirstAuthorise_add_update]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementFirstAuthorise_add_update]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Dickson LAW
-- Modified date:	07 March 2018
-- CR No.:			CRE17-004
-- Description:		Generate a new DPAR on EHCP basis
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
-- Create date:		22 Apr 2008
-- Description:		Reimbursement First Authorisation
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark YIP
-- Modified date:   19 Dec 2008
-- Description:	    Combine the First Authorization process
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark YIP
-- Modified date:   14 Aug 2009
-- Description:	    Adopt to new reimbursement schema
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Lawrence TSANG
-- Modified date:   18 August 2009
-- Description:	    Remove parameter @cutoff_dtm and get the previous Cutoff_Date from record with Authorised_Status 'S'
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Lawrence TSANG
-- Modified date:   9 October 2009
-- Description:	    (1) Performance refinement: Retrieve [Reimburse_ID] from [ReimbursementAuthorisation] rather than [ReimbursementAuthTran]
--					(2) Block concurrent update
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    
-- Modified date:   
-- Description:	    
-- =============================================

CREATE PROCEDURE [dbo].[proc_ReimbursementFirstAuthorise_add_update] 
	@current_user	varchar(20),
	@scheme_code	char(10)
WITH RECOMPILE
AS BEGIN
-- =============================================
-- Declaration
-- =============================================
DECLARE @current_dtm	datetime
DECLARE @reimburse_id	char(15)
DECLARE @cutoff_dtm		datetime
DECLARE @VerifiyCaseAvailable CHAR(1)
-- =============================================
-- Initialization
-- =============================================
	SELECT @current_dtm = GETDATE()

	SELECT TOP 1 
		@reimburse_id = Reimburse_ID 
	FROM
		ReimbursementAuthorisation
	WHERE
		Scheme_Code = @scheme_code
			AND Authorised_Status = 'P' 
			AND Record_Status = 'A'
	ORDER BY
		Authorised_Dtm DESC

	SELECT @cutoff_dtm = Cutoff_Date FROM ReimbursementAuthorisation WHERE Reimburse_ID = @reimburse_id AND Authorised_Status = 'S'	

	--	GET Verification Case Available or not
	SELECT 
		@VerifiyCaseAvailable = Verification_Case_Available 
	FROM 
		ReimbursementAuthorisation 
	WHERE 
		Reimburse_ID = @reimburse_id 
			AND Authorised_Status = 'S' 
				AND Record_Status = 'A'
-- =============================================
-- Validation 
-- =============================================
	-- If the "Hold" has been released
	IF (
			SELECT	COUNT(1)
			FROM	ReimbursementAuthorisation
			WHERE	Reimburse_ID = @reimburse_id
						AND Authorised_Status = 'P'
						AND Record_Status = 'A'
						AND Scheme_Code = @scheme_code
		) = 0 BEGIN
		RAISERROR('00011', 16, 1)
		RETURN @@error
	END
	
	-- If the "Hold" has been first authorized
	IF (
			SELECT	COUNT(1)
			FROM	ReimbursementAuthorisation
			WHERE	Reimburse_ID = @reimburse_id
						AND Authorised_Status = '1'
						AND Record_Status = 'A'
						AND Scheme_Code = @scheme_code
		) <> 0 BEGIN
		RAISERROR('00011', 16, 1)
		RETURN @@error
	END
-- =============================================
-- Return results
-- =============================================

-- (Step 1) Update [ReimbursementAuthTran]
	UPDATE
		ReimbursementAuthTran
	SET
		[Authorised_status] = '1'
	   ,[First_Authorised_Dtm] = GETDATE()
	   ,[First_Authorised_By] = @current_user
	WHERE
		Authorised_Status = 'P' 
			AND Scheme_Code = @scheme_code

-- (Step 2) Insert into [ReimbursementAuthorization]
INSERT INTO ReimbursementAuthorisation
			   ([Authorised_Dtm]
			   ,[Authorised_Status]
			   ,[Authorised_By]           
			   ,[Record_Status]
			   ,[Create_By]
			   ,[Create_Dtm]
			   ,[Update_By]
			   ,[Update_Dtm]
			   ,[Reimburse_ID]
			   ,[Cutoff_Date]
			   ,[Scheme_Code]
			   ,[Verification_Case_Available])
		 VALUES
			   (@current_dtm
			   ,'1'
			   ,@current_user           
			   ,'A'
			   ,@current_user
			   ,@current_dtm
			   ,@current_user
			   ,@current_dtm
			   ,@reimburse_id
			   ,@cutoff_dtm
			   ,@scheme_code
			   ,@VerifiyCaseAvailable)

END
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementFirstAuthorise_add_update] TO HCVU
GO
