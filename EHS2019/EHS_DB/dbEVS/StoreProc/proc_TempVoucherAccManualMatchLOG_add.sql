IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccManualMatchLOG_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccManualMatchLOG_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Vincent
-- Create date: 23 Feb 2010
-- Description:	Add TempVoucherAccManualMatchLOG
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE [dbo].[proc_TempVoucherAccManualMatchLOG_add]
	@Voucher_Acc_ID CHAR(15),
	@Valid CHAR(1),
	@Return_dtm DATETIME
AS
BEGIN
	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
	DECLARE @system_dtm DATETIME

-- =============================================
-- Initialization
-- =============================================
	SET @System_dtm = NULL

	SELECT TOP 1	@System_dtm = System_dtm 
	FROM				[TempVoucherAccManualSubLOG]
	WHERE			Voucher_Acc_ID = @Voucher_Acc_ID
	ORDER BY		System_dtm DESC


-- =============================================
-- Validation 
-- =============================================
	IF @System_dtm IS NULL
	BEGIN
			RETURN
	END

-- =============================================
-- Return results
-- =============================================
	IF NOT EXISTS(SELECT 1 FROM [TempVoucherAccManualMatchLOG] WHERE System_Dtm = @System_dtm AND Voucher_Acc_ID = @Voucher_Acc_ID)
	BEGIN
				INSERT INTO [TempVoucherAccManualMatchLOG] ([System_Dtm], [Voucher_Acc_ID], [Return_Dtm], [Valid])
					VALUES (@System_dtm, @Voucher_Acc_ID, @Return_dtm, @Valid)
	END
	-- ToDo: Override value?
	--	ELSE
	--	BEGIN	
	--				UPDATE	[TempVoucherAccManualMatchLOG]
	--				SET			[Return_Dtm] = @Return_dtm,
	--								[Valid] = @Valid
	--				WHERE	System_Dtm = @System_dtm 
	--								AND Voucher_Acc_ID = @Voucher_Acc_ID
	--	END
	
END

GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccManualMatchLOG_add] TO HCVU
GO
