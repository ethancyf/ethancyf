IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_BankAccVerification_upd_reject]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_BankAccVerification_upd_reject]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 19 June 2008
-- Description:	Update BankingVerification for Reject
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_BankAccVerification_upd_reject]
	@Enrolment_Ref_No char(15),
	@Display_Seq smallint,
	@sp_practice_Display_Seq smallint,
	@Void_By varchar(20),
	@Void_Dtm datetime,
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
			Void_By= @Void_By,
			Void_Dtm = @Void_Dtm,
			Record_Status = 'R'
		WHERE
			Enrolment_Ref_No = @Enrolment_Ref_No AND Display_Seq = @Display_Seq AND sp_practice_Display_Seq = @sp_practice_Display_Seq

END



GO

GRANT EXECUTE ON [dbo].[proc_BankAccVerification_upd_reject] TO HCVU
GO
