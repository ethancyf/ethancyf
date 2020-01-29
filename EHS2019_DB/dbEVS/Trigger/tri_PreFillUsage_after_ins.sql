IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_PreFillUsage_after_ins')
	DROP TRIGGER [dbo].[tri_PreFillUsage_after_ins] + Environment.NewLineGO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [tri_PreFillUsage_after_ins]
   ON		[dbo].[PreFillUsage]
   AFTER	INSERT
AS BEGIN

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

    INSERT INTO _tmpPreFillUsage
		( 
			Pre_fill_consent_id,
			Voucher_Acc_ID,
			Used_Date
		)
	SELECT	Pre_fill_consent_id,
			voucher_acc_id,
			getdate()
		
	FROM inserted

END
GO
