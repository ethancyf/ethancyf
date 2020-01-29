 IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ManualReimbursement_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ManualReimbursement_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	27 Jun 2016
-- CR No.			CRE16-003
-- Description:		Disallow input Chinese Chars
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Derek LEUNG
-- Modified date:	3 Nov 2010
-- Description:		Insert empty string as record status
-- =============================================	
-- =============================================
-- Author:			Pak Ho LEE
-- Create date:		23 July 2010
-- Description:		Add ManualReimbursement Record when create pending approval Transaction
-- =============================================

/*
@Transaction_ID		char		no	20
@Record_Status		char		no	1
@Creation_Reason	char		no	5
@Creation_Remark	nvarchar	no	255
@Override_Reason	nvarchar	no	255
@Payment_Method		char		no	5
@Payment_Remark		nvarchar	no	255
@Approval_By		varchar		no	20
@Approval_Dtm		datetime	no	8
@Reject_By			varchar		no	20
@Reject_Dtm			datetime	no	8
*/

CREATE PROCEDURE [dbo].[proc_ManualReimbursement_add]
	@Transaction_ID				char(20),
	@Creation_Reason			char(5),
	@Creation_Remark			nvarchar(255),
	@Override_Reason			nvarchar(255),
	@Payment_Method				char(5),
	@Payment_Remark				nvarchar(255)
		
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
	
	INSERT INTO [ManualReimbursement]
	(
		[Transaction_ID],	
		[Creation_Reason],
		[Creation_Remark],
		[Override_Reason],
		[Payment_Method],
		[Payment_Remark],
		[Approval_By],
		[Approval_Dtm],
		[Reject_By],
		[Reject_Dtm], 
		[Record_Status]
	
	) VALUES
	(
		@Transaction_ID,		
		@Creation_Reason,
		@Creation_Remark,
		@Override_Reason,
		@Payment_Method,
		@Payment_Remark,
		NULL,	
		NULL,
		NULL,
		NULL, 
		''
	)
	
END
GO

GRANT EXECUTE ON [dbo].[proc_ManualReimbursement_add] TO HCVU
GO
