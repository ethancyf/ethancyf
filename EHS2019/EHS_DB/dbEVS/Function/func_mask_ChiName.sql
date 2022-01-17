IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[func_mask_ChiName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[func_mask_ChiName]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE20-023-68 (Add Chinese Name)
-- Modified by:		Winnie SUEN
-- Modified date:	20 Dec 2021
-- Description:		1. Change @chi_name nvarchar(6) -> nvarchar(12)
--					2. Add COLLATE Chinese_Taiwan_Stroke_90_CS_AS_SC to handle 32 bit characters
--					Input:	𤦬𤦬𤦬𤦬𤦬𤦬
--					Output: 𤦬*𤦬*𤦬*
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	17 Sep 2019
-- CR No.:			CRE19-001 (VSS 2019/20)
-- Description:		Mask Chinese name 
--					Input:	愛新覺羅溥儀
--					Output: 愛*覺*溥*
-- ============================================= 
CREATE FUNCTION [dbo].[func_mask_ChiName]
(
	@chi_name NVARCHAR(12)
)
RETURNS nvarchar(40)
AS
BEGIN
	-- =============================================
	-- Declaration
	-- =============================================
	DECLARE @index int
	DECLARE @temp_name nvarchar(40)
	
	DECLARE @result nvarchar(20)

	-- =============================================
	-- Validation 
	-- =============================================
	-- =============================================
	-- Initialization
	-- =============================================
	SET @result = @chi_name 
	SET @index = 2

	-- Extract Initial one by one
	WHILE(@index <= LEN(@chi_name COLLATE Chinese_Taiwan_Stroke_90_CS_AS_SC))
	BEGIN
		SET @result = STUFF(@result COLLATE Chinese_Taiwan_Stroke_90_CS_AS_SC, @index, 1, '*') 

		SET @index = @index + 2
	END

	-- =============================================
	-- Return results
	-- =============================================
	RETURN @result 

END
GO


Grant execute on [dbo].[func_mask_ChiName] to HCVU
GO

