IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SubsidizeWriteOffGeneratorQueue_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[proc_SubsidizeWriteOffGeneratorQueue_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE14-016 (To introduce 'Deceased' status into eHS)
-- Modified by:	    Winnie SUEN
-- Modified date:   24 Nov 2017
-- Description:		Add [DOD], [Exact_DOD]
-- =============================================
-- ==========================================================================================
-- Author:	Tommy LAM
-- CR No.:	CRE13-006
-- Create Date:	05 Nov 2013
-- Description:	Get record from table - [SubsidizeWriteOffGeneratorQueue]
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_SubsidizeWriteOffGeneratorQueue_get]
	@record_status	char(1),
	@page			int,
	@total_page		int
AS BEGIN
-- ============================================================
-- Declaration
-- ============================================================
-- ============================================================
-- Validation
-- ============================================================

	IF @page IS NULL OR @total_page IS NULL
		RETURN

	IF @page <= 0 OR @total_page <= 0
		RETURN

	IF @page > @total_page
		RETURN

-- ============================================================
-- Initialization
-- ============================================================
-- ============================================================
-- Return results
-- ============================================================

	EXEC [proc_SymmetricKey_open]

		SELECT
			[Row_ID],
			[Doc_Code],
			CONVERT(varchar(100), DECRYPTBYKEY([Encrypt_Field1])) AS [Doc_No],
			[DOB],
			[Exact_DOB],
			[Scheme_Code],
			[Subsidize_Code],
			[Record_Status],
			[TSMP],
			[DOD],
			[Exact_DOD]
		FROM
			(
			SELECT
				ROW_NUMBER() OVER(ORDER BY [Row_ID]) AS [Row_No],
				[Row_ID],
				[Doc_Code],
				[Encrypt_Field1],
				[DOB],
				[Exact_DOB],
				[Scheme_Code],
				[Subsidize_Code],
				[Record_Status],
				[TSMP],
				[DOD],
				[Exact_DOD]
			FROM
				[SubsidizeWriteOffGeneratorQueue]
			) AS TEMP_1
		WHERE
			[Row_No] % @total_page = (@page - 1)
			AND [Record_Status] = @record_status

	EXEC [proc_SymmetricKey_close]

END
GO

GRANT EXECUTE ON [dbo].[proc_SubsidizeWriteOffGeneratorQueue_get] TO HCVU
GO
