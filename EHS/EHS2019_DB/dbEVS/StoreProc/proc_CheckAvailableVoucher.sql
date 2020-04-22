IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_CheckAvailableVoucher]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_CheckAvailableVoucher]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.			CRE16-026-03 (Add PCV13)
-- Modified by:		Lawrence TSANG
-- Modified date:	17 October 2017
-- Description:		Stored procedure not used anymore
-- =============================================
-- =============================================
-- Author:			Pak Ho LEE
-- Create date:		11-June-2009
-- Description:		Check Available Voucher
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	18-Aug-2009
-- Description:		Vouchers -> Unit
-- =============================================

/*
CREATE procedure [dbo].[proc_CheckAvailableVoucher]
	@Scheme_Code varchar(10),
	@Voucher_Acc_ID char(15), 
	@Temp_Voucher_Acc_ID char(15),
	@AvailableVoucher integer output
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Description
-- =============================================

-- Step 1: Retrieve the HKID of the Voucher Account / Temporary Voucher Account

-- Step 2: Count No. of Voucher Used in Voucher Account & Temporary Voucher Account [By HKID]

-- Step 3: Count Scheme Available Voucher (By DOB & Voucher Rules)

-- =============================================
-- Declaration
-- =============================================

OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key
	
DECLARE @check_HKID AS Char(9)
DECLARE @check_Encrypt_Field1 AS Varbinary(100)

DECLARE @totalVoucherUsed as Integer
DECLARE @tmpVoucherUsed as Integer

DECLARE @scheme_available_voucher as Integer

Set @totalVoucherUsed = 0
Set @tmpVoucherUsed = 0
Set @scheme_available_voucher = 0

DECLARE @intAge as Integer

SET @intAge = 0

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================

-- Step 1
	SELECT 
		@check_HKID = CONVERT(varchar, DecryptByKey(P.Encrypt_Field1)),
		@check_Encrypt_Field1 = P.Encrypt_Field1,
		@intAge = DateDiff(YEAR, DOB, GetDate())
	FROM 
		[dbo].[PersonalInformation] P --, [dbo].[VoucherAccount] VA
	WHERE 
		P.Voucher_Acc_ID = @Voucher_Acc_ID
				
	IF @check_HKID IS NULL 
	BEGIN
		SELECT 
			@check_HKID = CONVERT(varchar, DecryptByKey(P.Encrypt_Field1)),
			@check_Encrypt_Field1 = P.Encrypt_Field1,
			@intAge = DateDiff(YEAR, DOB, GetDate())
		FROM 
			[dbo].[TempPersonalInformation] P --, [dbo].[TempVoucherAccount] VA
		WHERE 
			P.Voucher_Acc_ID = @Temp_Voucher_Acc_ID
	END
		
	IF @check_HKID IS NULL
	BEGIN
		RAISERROR('00011', 16, 1)
		RETURN @@error
	END
	ELSE
	BEGIN	
-- Step2
	-- Claim By Validated Account Voucher Used
		SELECT @tmpVoucherUsed = ISNULL(SUM(ISNULL([Voucher_Claim],0)),0)
		FROM
			[dbo].[VoucherTransaction] VT
		WHERE
			([Temp_Voucher_Acc_Id] IS NULL OR [Temp_Voucher_Acc_Id] = '') AND		
			VT.[Record_Status]  <> 'I' AND VT.[Scheme_Code] = @Scheme_Code AND
			VT.[Voucher_Acc_ID] IN
			(
				SELECT
					VA.[Voucher_Acc_ID]
				FROM 
					[dbo].[PersonalInformation] P, [dbo].[VoucherAccount] VA
				WHERE 
					P.[Voucher_Acc_ID] = VA.[Voucher_Acc_ID] AND VA.[Scheme_Code] = @Scheme_Code AND
					P.[Encrypt_Field1] = @check_Encrypt_Field1
			)
		SET @totalVoucherUsed = @totalVoucherUsed + @tmpVoucherUsed
		
		
		-- Temporary Account Voucher Used
		SELECT @tmpVoucherUsed = ISNULL(SUM(ISNULL([Voucher_Claim],0)),0)
		FROM
			[dbo].[VoucherTransaction]
		WHERE 
			[Record_Status] <> 'I' AND [Scheme_Code] = @Scheme_Code AND
			[Temp_Voucher_Acc_ID] IN
			(
				SELECT
					VA.[Voucher_Acc_ID]
				FROM 
					[dbo].[TempPersonalInformation] P, [dbo].[TempVoucherAccount] VA
				WHERE 
					P.[Voucher_Acc_ID] = VA.[Voucher_Acc_ID] AND VA.[Scheme_Code] = @Scheme_Code AND
					P.[Encrypt_Field1] = @check_Encrypt_Field1
			)
		SET @totalVoucherUsed = @totalVoucherUsed + @tmpVoucherUsed	
	END

-- Step 3

	SELECT @scheme_available_voucher = ISNULL(SUM(ISNULL([Unit],0)),0) 
	FROM
		[dbo].[VoucherScheme] VS, [dbo].[VoucherSchemeDetail] VSD
	WHERE
		VS.[Scheme_Code] = VSD.[Scheme_Code] AND VS.[Scheme_Code] = @Scheme_Code AND
		VSD.[Record_Status] = 'A' AND VSD.[Effective_Date] <= GETDATE() AND
		(
			-- Match Rules
			SELECT COUNT(1) 
			FROM 
				[dbo].[VoucherSchemeRule]
			WHERE
				[Scheme_Code] = @Scheme_Code AND [Rule_Name] = 'AGE' AND
				(
					([Operator] = '>' AND @intAge > [Value]) OR
					([Operator] = '<' AND @intAge < [Value]) OR
					([Operator] = '=' AND @intAge = [Value]) 
				)
				
				
		) > 0 AND
		(
			-- No Not Match Rules
			SELECT COUNT(1) 
			FROM 
				[dbo].[VoucherSchemeRule]
			WHERE
				[Scheme_Code] = @Scheme_Code AND [Rule_Name] = 'AGE' AND
				(
					([Operator] = '>' AND @intAge <= [Value]) OR
					([Operator] = '<' AND @intAge >= [Value]) OR
					([Operator] = '=' AND @intAge <> [Value]) 
				)
		) <= 0

-- =============================================
-- Return results
-- =============================================

	SELECT @AvailableVoucher = (@scheme_available_voucher - @totalVoucherUsed) 	
	
CLOSE SYMMETRIC KEY sym_Key

END
GO

GRANT EXECUTE ON [dbo].[proc_CheckAvailableVoucher] TO HCSP
GO
*/
