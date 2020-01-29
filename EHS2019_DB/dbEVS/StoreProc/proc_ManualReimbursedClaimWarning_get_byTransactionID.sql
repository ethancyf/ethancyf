IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ManualReimbursedClaimWarning_get_byTransactionID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ManualReimbursedClaimWarning_get_byTransactionID]
GO

/****** Object:  StoredProcedure [dbo].[proc_ManualReimbursedClaimWarning_get_byTransactionID]    Script Date: 07/13/2010 18:11:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:	Derek Leung	
-- Create date: 21 July 2010
-- Description: Insert warning message of HCVU claim transaction 
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_ManualReimbursedClaimWarning_get_byTransactionID]
	@Transaction_ID char(20)
	
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

	SELECT	Display_Seq, 
			Function_Code, 
			Severity_Code, 
			Message_Code, 
			Message_Variable_Name, 
			Message_Variable_Value, 
			Message_Variable_Name_Chi, 
			Message_Variable_Value_Chi
	FROM	ManualReimbursedClaimWarning
	WHERE	Transaction_ID = @Transaction_ID
	Order By Display_Seq
	
END

GO

GRANT EXECUTE ON [dbo].[proc_ManualReimbursedClaimWarning_get_byTransactionID] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_ManualReimbursedClaimWarning_get_byTransactionID] TO HCSP
GO


