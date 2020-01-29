IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TransactionAdditionalField_del]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TransactionAdditionalField_del]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:	
-- Modified date:	
-- Description:	
-- =============================================
-- =============================================
-- Author:		Koala CHENG
-- CR No:		CRE11-024-02 HCVS Pilot Extension Part 2
-- Create date: 16 Aug 2011
-- Description:	Delete all Transaction Additional Field
-- =============================================
CREATE PROCEDURE [dbo].[proc_TransactionAdditionalField_del]
	@Transaction_ID			char(20)
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

	DELETE [TransactionAdditionalField]
	WHERE [Transaction_ID] = @Transaction_ID

END
GO

GRANT EXECUTE ON [dbo].[proc_TransactionAdditionalField_del] TO HCSP
GO
