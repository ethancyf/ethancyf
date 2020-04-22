IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TransactionAdditionalField_check_byRCHCode]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TransactionAdditionalField_check_byRCHCode]
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
-- Author:		Twinsen CHAN
-- CR No.:		CRE11-013
-- Create date: 25 Feb 2013
-- Description:	Check If the RCH Code is being used in any TransactionAdditionalField
-- =============================================
CREATE PROCEDURE [dbo].[proc_TransactionAdditionalField_check_byRCHCode]
	@RCHCode	varchar(10)
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
DECLARE @Transaction_ID		char(20)
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	SELECT TOP 1 @Transaction_ID = Transaction_ID
	FROM TransactionAdditionalField WITH(NOLOCK)
	WHERE 
		AdditionalFieldID = 'RHCCode' 
		AND AdditionalFieldValueCode = @RCHCode

	IF @Transaction_ID IS NULL
		SELECT 0
	ELSE
		SELECT 1
END
GO

GRANT EXECUTE ON [dbo].[proc_TransactionAdditionalField_check_byRCHCode] TO HCVU
GO
