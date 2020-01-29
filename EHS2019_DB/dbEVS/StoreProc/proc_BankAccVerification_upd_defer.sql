IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_BankAccVerification_upd_defer]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_BankAccVerification_upd_defer]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Clark YIP
-- Create date: 16 June 2008
-- Description:	Update BankingVerification for Defer
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_BankAccVerification_upd_defer]
	@Enrolment_Ref_No char(15),
	@Display_Seq smallint,
	@sp_practice_Display_Seq smallint,
	@Defer_By varchar(20),
	@Defer_Dtm datetime,
	@tsmp timestamp

AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================

	IF (
		SELECT TSMP FROM [dbo].[BankAccVerification]
		WHERE Enrolment_Ref_No = @Enrolment_Ref_No AND Display_Seq = @Display_Seq AND sp_practice_Display_Seq = @sp_practice_Display_Seq
	) != @tsmp
	BEGIN
		RAISERROR('00011', 16, 1)
		return @@error
	END

-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	UPDATE BankAccVerification
		Set
			Defer_By = @Defer_By,
			Defer_Dtm = @Defer_Dtm,
			Record_Status = 'D'
		WHERE
			Enrolment_Ref_No = @Enrolment_Ref_No AND Display_Seq = @Display_Seq AND sp_practice_Display_Seq = @sp_practice_Display_Seq

END
GO

GRANT EXECUTE ON [dbo].[proc_BankAccVerification_upd_defer] TO HCVU
GO
