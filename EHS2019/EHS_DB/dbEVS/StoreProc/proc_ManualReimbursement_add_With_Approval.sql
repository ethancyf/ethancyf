 IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ManualReimbursement_add_With_Approval]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ManualReimbursement_add_With_Approval]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.			
-- Description:		
-- =============================================
-- =============================================
-- Modification History
-- Create by:		Chris YIM
-- Create date:		31 Jul 2020
-- CR No.			CRE19-031 (VSS MMR Upload)
-- Description:		Add Manual Reimbursement with Approval
-- =============================================

CREATE PROCEDURE [dbo].[proc_ManualReimbursement_add_With_Approval]
	@Transaction_ID				CHAR(20),
	@Creation_Reason			CHAR(5),
	@Creation_Remark			NVARCHAR(255),
	@Override_Reason			NVARCHAR(255),
	@Payment_Method				CHAR(5),
	@Payment_Remark				NVARCHAR(255),
    @Approval_By				VARCHAR(20),
    @Approval_Dtm				DATETIME,
    @Reject_By					VARCHAR(20),
    @Reject_Dtm					DATETIME
		
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
-- Process data
-- =============================================
	
	INSERT INTO [ManualReimbursement](
		[Transaction_ID],	
		[Record_Status],
		[Creation_Reason],
		[Creation_Remark],
		[Override_Reason],
		[Payment_Method],
		[Payment_Remark],
		[Approval_By],
		[Approval_Dtm],
		[Reject_By],
		[Reject_Dtm]
	) 
	VALUES(
		@Transaction_ID,	
		'',
		@Creation_Reason,
		@Creation_Remark,
		@Override_Reason,
		@Payment_Method,
		@Payment_Remark,
		@Approval_By,
		@Approval_Dtm,
		@Reject_By,
		@Reject_Dtm
	)
	
END
GO

GRANT EXECUTE ON [dbo].[proc_ManualReimbursement_add_With_Approval] TO HCVU
GO

