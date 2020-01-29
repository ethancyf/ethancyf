IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[func_format_voucher_account_number]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[func_format_voucher_account_number]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:			Vincent
-- Create date:	27 JAN 2010
-- Description:	Format Voucher Account ID for Display
-- =============================================
-- =============================================
-- Modification History
-- Modified by:			
-- Modified date:			
-- Description:			
-- =============================================
CREATE FUNCTION [dbo].[func_format_voucher_account_number]
(
	@account_source CHAR(1),
	@voucher_account_id VARCHAR(50)
)
RETURNS VARCHAR(50)
AS
BEGIN
	-- =============================================
	-- Declaration
	-- =============================================
	DECLARE @result VARCHAR(50)

	DECLARE @temp_voucher_account_id VARCHAR(50)
	DECLARE @validate_account_prefix VARCHAR(50)
	DECLARE @validate_account_check_digit CHAR(1)
	-- =============================================
	-- Validation 
	-- =============================================
	-- =============================================
	-- Initialization
	-- =============================================
	SET @temp_voucher_account_id = LTRIM(RTRIM(@voucher_account_id))

	IF @account_source = 'V'
	BEGIN
		-- Validated Account Type
		SELECT	@validate_account_prefix = LTRIM(RTRIM(Parm_Value1))
		FROM		SystemParameters
		WHERE	Parameter_Name = 'eHealthAccountPrefix'

		SET @temp_voucher_account_id = @validate_account_prefix + @temp_voucher_account_id
		SET @validate_account_check_digit = dbo.func_generate_check_digit(@temp_voucher_account_id)
		SET @result = @temp_voucher_account_id + @validate_account_check_digit
	END
	ELSE
	BEGIN
		-- Temp, Special, Invalid Account Type
		SET @result = dbo.func_format_system_number(@temp_voucher_account_id)
	ENd

	-- =============================================
	-- Return results
	-- =============================================
	RETURN @result

END
GO


Grant execute on [dbo].[func_format_voucher_account_number] to HCSP
GO

Grant execute on [dbo].[func_format_voucher_account_number] to HCVU
GO

Grant execute on [dbo].[func_format_voucher_account_number] to HCPUBLIC
GO 