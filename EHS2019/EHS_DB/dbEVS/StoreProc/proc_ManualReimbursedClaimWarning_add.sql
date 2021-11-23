IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ManualReimbursedClaimWarning_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ManualReimbursedClaimWarning_add]
GO
/****** Object:  StoredProcedure [dbo].[proc_ManualReimbursedClaimWarning_add]    Script Date: 07/13/2010 16:29:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE20-0023-63 (Revise Back Office Claim Message)
-- Modified by:		Winnie SUEN
-- Modified date:	08 Oct 2021
-- Description:		1. Enlarge column [Message_Variable_Name] and [Message_Variable_Name_Chi] data size (80 -> 1000)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================
-- Author:	Derek Leung	
-- Create date: 21 July 2010
-- Description: Retreive Manual Reimbursement Warning Message	
-- =============================================

CREATE PROCEDURE [dbo].[proc_ManualReimbursedClaimWarning_add]
	@Transaction_ID char(20),
	@Display_Seq integer, 
	@Function_Code char(6),
	@Severity_Code char(1),
	@Message_Code char(5), 
	@Message_Variable_Name varchar(1000) = NULL, 
	@Message_Variable_Value nvarchar(255)  = NULL, 
	@Message_Variable_Name_Chi varchar(1000)  = NULL, 
	@Message_Variable_Value_Chi nvarchar(255)  = NULL
	
AS
BEGIN
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
-- Return results
-- =============================================

	INSERT INTO [ManualReimbursedClaimWarning]
	(
		Transaction_ID,
		Display_Seq, 
		Function_Code, 
		Severity_Code,
		Message_Code, 
		Message_Variable_Name, 
		Message_Variable_Value, 
		Message_Variable_Name_Chi, 
		Message_Variable_Value_Chi	
	)
	VALUES
	(
		@Transaction_ID,
		@Display_Seq,
		@Function_Code,
		@Severity_Code,
		@Message_Code, 
		@Message_Variable_Name, 
		@Message_Variable_Value, 
		@Message_Variable_Name_Chi, 
		@Message_Variable_Value_Chi
	)
END

GO

GRANT EXECUTE ON [dbo].[proc_ManualReimbursedClaimWarning_add] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_ManualReimbursedClaimWarning_add] TO WSEXT
GO