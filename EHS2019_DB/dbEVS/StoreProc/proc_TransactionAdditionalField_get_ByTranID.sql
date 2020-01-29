IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TransactionAdditionalField_get_ByTranID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TransactionAdditionalField_get_ByTranID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 21 Aug 2009
-- Description:	Retrieve Transaction Additional Fields by Tran ID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_TransactionAdditionalField_get_ByTranID] 
	@tran_id		 varchar(20)
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

	SELECT
		TAF.[Transaction_ID],
		TAF.[Scheme_Code],
		TAF.[Scheme_Seq],
		TAF.[Subsidize_Code],
		TAF.[AdditionalFieldID],
		TAF.[AdditionalFieldValueCode],
		TAF.[AdditionalFieldValueDesc]
	FROM
		[TransactionAdditionalField] TAF		
	WHERE
		[Transaction_ID] = @tran_id

END
GO

GRANT EXECUTE ON [dbo].[proc_TransactionAdditionalField_get_ByTranID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_TransactionAdditionalField_get_ByTranID] TO HCVU
GO
