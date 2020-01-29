IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TransactionAdditionalField_get_SchoolCode_ByTranID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TransactionAdditionalField_get_SchoolCode_ByTranID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- CR No.:
-- Description:	
-- =============================================
-- =============================================
-- Author:			Chris YIM
-- Create date:		26 Aug 2018
-- CR No.:			CRE17-018
-- Description:		Get TransactionAdditionalField by School Code
-- =============================================

CREATE PROCEDURE [dbo].[proc_TransactionAdditionalField_get_SchoolCode_ByTranID]
	@TransactionID		CHAR(20)
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
	DECLARE @IN_TransactionID			char(20)
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	SET @IN_TransactionID = @TransactionID
-- =============================================
-- Return results
-- =============================================

	SELECT 
		TAF.[Transaction_ID],
		TAF.[Scheme_Code],
		TAF.[Scheme_Seq],
		TAF.[Subsidize_Code],
		TAF.[AdditionalFieldID],
		TAF.[AdditionalFieldValueCode],
		TAF.[AdditionalFieldValueDesc]
	FROM 
		TransactionAdditionalField TAF WITH(NOLOCK)
	WHERE 
		AdditionalFieldID = 'SchoolCode' 
		AND [Transaction_ID] = @IN_TransactionID

END
GO

GRANT EXECUTE ON [dbo].[proc_TransactionAdditionalField_get_SchoolCode_ByTranID] TO HCVU
GO

