IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PreFillUsage_get_ByPrefillID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PreFillUsage_get_ByPrefillID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	25 August 2016
-- CR No.:			CRE16-002
-- Description:		Stored procedure is not used anymore
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 29 Sep 2009
-- Description:	Retrieve Pre Fill Usage
-- =============================================
-- =============================================
-- Modification History
-- Modified by:
-- Modified date: 
-- Description:	
-- =============================================

/*
CREATE PROCEDURE [dbo].[proc_PreFillUsage_get_ByPrefillID]
	@pre_fill_consent_id char(15)
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
	SET @pre_fill_consent_id = SubString(@pre_fill_consent_id,7,9)	
-- =============================================
-- Return results
-- =============================================

	SELECT 
		[Pre_Fill_Consent_ID], [Voucher_Acc_ID]
	FROM 
		[PreFillUsage]
	WHERE
		SubString([Pre_Fill_Consent_ID],7,9) = @pre_fill_consent_id

END
GO

GRANT EXECUTE ON [dbo].[proc_PreFillUsage_get_ByPrefillID] TO HCSP
GO

*/
