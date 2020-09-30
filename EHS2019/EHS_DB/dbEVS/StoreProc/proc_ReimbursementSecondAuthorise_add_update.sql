IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementSecondAuthorise_add_update]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementSecondAuthorise_add_update]
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
-- Create date:		22 Dec 2008
-- Description:		Reimbursement Second Authorisation
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   22 Jan 2009
-- Description:	    Adjust the Total_No_Of_Instruction field
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   09 Feb 2009
-- Description:	    The field [Second_Party_Bank_Acc_Name] will store the bank_account_holder name instead of SP name
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
-- Modified date:   9 October 2009
-- Description:	    (1) Reformat the code
--					(2) Block concurrent update
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    
-- Modified date:   
-- Description:	    
-- =============================================

CREATE PROCEDURE [dbo].[proc_ReimbursementSecondAuthorise_add_update] 
	@current_user	varchar(20),
	@reimburse_ID	char(15),
	@scheme_code	char(10)
WITH RECOMPILE
AS BEGIN
-- =============================================
-- Declaration
-- =============================================
	DECLARE @current_dtm	datetime
	DECLARE @cutoff_dtm		datetime
	DECLARE @VerifiyCaseAvailable CHAR(1)
-- =============================================
-- Initialization
-- =============================================
	SELECT @current_dtm = GETDATE()

	SELECT 
		@cutoff_dtm = Cutoff_Date 
	FROM
		ReimbursementAuthorisation 
	WHERE
		Reimburse_ID = @reimburse_id 
			AND Authorised_Status = '1' 
			AND Record_Status = 'A'
			AND Scheme_Code = @scheme_code

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
	-- If the "First Authorized" has been cancelled
	IF (
			SELECT	COUNT(1)
			FROM	ReimbursementAuthorisation
			WHERE	Reimburse_ID = @reimburse_id
						AND Authorised_Status = '1'
						AND Record_Status = 'A'
						AND Scheme_Code = @scheme_code
		) = 0 BEGIN
		RAISERROR('00011', 16, 1)
		RETURN @@error
	END
	
	-- If the "First Authorized" has been second authorized
	IF (
			SELECT	COUNT(1)
			FROM	ReimbursementAuthorisation
			WHERE	Reimburse_ID = @reimburse_id
						AND Authorised_Status = '2'
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
		[Authorised_status] = '2'
	   ,[Second_Authorised_dtm] = @current_dtm
	   ,[Second_Authorised_by] = @current_user
	WHERE
		Authorised_Status = '1' 
			AND Scheme_Code = @scheme_code
			AND Reimburse_ID = @reimburse_ID

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
			   ,'2'
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

GRANT EXECUTE ON [dbo].[proc_ReimbursementSecondAuthorise_add_update] TO HCVU
GO
