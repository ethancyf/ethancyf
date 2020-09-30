IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementAuthorisation_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementAuthorisation_add]
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
-- Description:		Reimbursement Authorisation
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark YIP
-- Modified date:   12 Aug 2009
-- Description:	    Add scheme code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Lawrence TSANG
-- Modified date:   18 August 2009
-- Description:	    Add logic: Get the previous Cutoff_Date if not inserting a record with Authorised_Status 'S'
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Lawrence TSANG
-- Modified date:   21 August 2009
-- Description:	    Remove @authorised_dtm and replace it by GETDATE()
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Lawrence TSANG
-- Modified date:   21 August 2009
-- Description:	    Insert [Void_By], [Void_Dtm], [Void_Remark] to be NULL
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    
-- Modified date:   
-- Description:	    
-- =============================================

CREATE PROCEDURE [dbo].[proc_ReimbursementAuthorisation_add] 
	@tran_status					char(1),
	@authorised_status				char(1),
	@authorised_by    				varchar(20),
	@reimburse_id					char(15),
	@current_user					varchar(20),
	@cutoff_dtm						datetime,
	@scheme_code					char(10),
	@Verification_Case_Available	char(1)
WITH RECOMPILE
AS BEGIN
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
-- Return results
-- =============================================

-- If the Authorised_Status going to be added is not 'S', get the cutoff date from the Authorised_Status = 'S'
IF @authorised_status <> 'S' BEGIN
	SELECT @cutoff_dtm = Cutoff_Date FROM ReimbursementAuthorisation WHERE Reimburse_ID = @reimburse_id AND Authorised_Status = 'S'	
END

INSERT INTO ReimbursementAuthorisation
				([Authorised_Dtm]
				,[Authorised_Status]
				,[Authorised_By]   
				,[Void_By]
				,[Void_Dtm]
				,[Void_Remark]    
				,[Reimburse_ID]
				,[Cutoff_Date]   
				,[Record_Status]
				,[Create_By]
				,[Create_Dtm]
				,[Update_By]
				,[Update_Dtm]
				,[Scheme_Code]
				,[Verification_Case_Available])
		 VALUES
				(GETDATE()
				,@authorised_status
				,@authorised_by       
				,NULL
				,NULL
				,NULL
				,@reimburse_id
				,@cutoff_dtm
				,@tran_status
				,@current_user
				,GETDATE()
				,@current_user
				,GETDATE()
				,@scheme_code
				,@Verification_Case_Available
				)

END
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementAuthorisation_add] TO HCVU
GO
