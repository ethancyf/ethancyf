IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_RR2_ReturnTokenLogo]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_RR2_ReturnTokenLogo]
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
-- Author:		Pak Ho LEE
-- Create date: 14 Aug 2008
-- Description:	Retrieve RR2-Weekly Log of Return of the Tokens and Logos
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_RR2_ReturnTokenLogo]
	@PeriodFrom datetime,
	@PeriodTo datetime
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================

EXEC [proc_SymmetricKey_open]

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
	SP.SP_ID as [SPID],
	convert(varchar(40), DecryptByKey(Encrypt_Field2)) as [HCSP Name],
	ISNULL(SD.Data_Value,'') as [HCSP Profession],
	SP.Delist_Dtm as [Date of Delisting],
	ISNULL(SD2.Column_Name,'') as [Type of Delisting],
	SPD.Token_Return_Dtm as [Date of return of token],
	SPD.Logo_Return_Dtm as [Date of return of logo]

FROM
	[dbo].[SPDelisted] SPD
		INNER JOIN [dbo].[ServiceProvider] SP
			ON SPD.SP_ID = SP.SP_ID
		LEFT OUTER JOIN [dbo].[Practice] P ON
			SP.SP_ID = P.SP_ID
		LEFT OUTER JOIN [dbo].[Professional] Prof ON
			P.SP_ID = Prof.SP_ID AND P.Professional_Seq = Prof.Professional_Seq
		LEFT OUTER JOIN [dbo].[StaticData] SD
			ON SD.Column_Name = 'PROFESSION' AND SD.Item_No = Prof.Service_Category_Code
		LEFT OUTER JOIN [dbo].[StatusData] SD2
			ON SD2.Enum_Class = 'DelistStatus' AND Status_Value = SP.Delist_Status

WHERE

	(SPD.Token_Return_Dtm < DateAdd(Day,1,@PeriodTo) AND SPD.Token_Return_Dtm >= @PeriodFrom) 
	OR
	(SPD.Logo_Return_Dtm < DateAdd(Day,1,@PeriodTo) AND SPD.Logo_Return_Dtm >= @PeriodFrom) 

ORDER BY SP.SP_ID ASC


SELECT @PeriodFrom as [Period From], @PeriodTo as [Period To], GetDate() as [Generate Date]

EXEC [proc_SymmetricKey_close]

END

GO

GRANT EXECUTE ON [dbo].[proc_RR2_ReturnTokenLogo] TO HCVU
GO
