IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SDScheme_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SDScheme_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		10 August 2016
-- CR No.:			CRE16-002
-- Description:		Retrieve SDScheme
-- =============================================

CREATE PROCEDURE [dbo].[proc_SDScheme_get] 
AS BEGIN

	SELECT
		Scheme_Code,
		Scheme_Desc,
		Scheme_Desc_Chi,
		Scheme_Short_Form,
		Scheme_Short_Form_Chi,
		Scheme_Url,
		Scheme_Url_Chi,
		Logo_Available,
		Display_Seq,
		Record_Status
	FROM
		SDScheme
	WHERE
		Record_Status = 'A'
	ORDER BY
		Display_Seq


END
GO

GRANT EXECUTE ON [dbo].[proc_SDScheme_get] TO HCPUBLIC
GO
