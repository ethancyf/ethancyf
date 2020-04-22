IF EXISTS (
SELECT 1 FROM Sys.Objects
WHERE [name] = 'proc_FunctionFeature_FNFT_get_ByFunctionCodeAndFeatureCode' AND [type] = 'P')
BEGIN
	DROP PROCEDURE proc_FunctionFeature_FNFT_get_ByFunctionCodeAndFeatureCode
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ==========================================================================================
-- Author:	Nick POON
-- Create Date:	18 Dec 2012
-- Description:	Get Feature By Function Code and Feature Code - [FunctionFeature_FNFT]
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_FunctionFeature_FNFT_get_ByFunctionCodeAndFeatureCode]
	@function_code char(6),
	@feature_code varchar(50)
AS
BEGIN
-- ============================================================
-- Declaration
-- ============================================================
-- ============================================================
-- Validation
-- ============================================================
-- ============================================================
-- Initialization
-- ============================================================
-- ============================================================
-- Return results
-- ============================================================
SELECT
	FNFT_Function_Code,
	FNFT_Feature_Code
FROM
	FunctionFeature_FNFT
WHERE
	FNFT_Function_Code = @function_code
	AND FNFT_Feature_Code = @feature_code
	AND	FNFT_Record_Status = 'A'
END
GO

GRANT EXECUTE ON [dbo].[proc_FunctionFeature_FNFT_get_ByFunctionCodeAndFeatureCode] TO HCVU
GO