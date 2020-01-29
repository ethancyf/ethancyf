IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_BankAccountStaging_del]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_BankAccountStaging_del]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kathy LEE
-- Create date: 31 May 2008
-- Description:	Delete Bank Account Information in table
--				"BankAccountStaging"
-- =============================================

-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_BankAccountStaging_del]
	@enrolment_ref_no char(15), @display_seq smallint, 
	@sp_practice_display_seq smallint, @update_by varchar(20),
	@record_status char(1), @tsmp timestamp
AS
BEGIN
	
	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================

DECLARE @rowcount as smallint
-- =============================================
-- Validation 
-- =============================================
	IF (SELECT TSMP FROM BankAccountStaging
		WHERE Enrolment_Ref_No = @enrolment_ref_no and
				Display_Seq = @display_seq and
				SP_Practice_Display_Seq = @sp_practice_display_seq) != @tsmp
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

	UPDATE	BankAccountStaging
	SET		Record_Status = @record_status,
			Update_by = @update_by,
			Update_Dtm = getdate()
	WHERE	Enrolment_Ref_No = @enrolment_ref_no and
			Display_Seq = @display_seq and
			SP_Practice_Display_Seq = @sp_practice_display_seq

	DELETE	FROM BankAccountStaging
	WHERE	Enrolment_Ref_No = @enrolment_ref_no and
			Display_Seq = @display_seq and
			SP_Practice_Display_Seq = @sp_practice_display_seq

--	Deleted the target bankaccount record already
	UPDATE	BankAccountStaging
	SET		Display_Seq = Display_Seq - 1
	WHERE	Display_Seq > @display_seq and
			Enrolment_Ref_No = @enrolment_ref_no and
			SP_Practice_Display_Seq = @sp_practice_display_seq

	SELECT	@rowcount = count(1)
	FROM	BankAccountStaging
	WHERE	Enrolment_Ref_No = @enrolment_ref_no and
			SP_Practice_Display_Seq = @sp_practice_display_seq

	IF @rowcount = 0
	BEGIN
		UPDATE	BankAccountStaging
		SET		SP_Practice_Display_Seq = SP_Practice_Display_Seq - 1
		WHERE	SP_Practice_Display_Seq > @sp_practice_display_seq and
				Enrolment_Ref_No = @enrolment_ref_no
	END
	
END
GO

GRANT EXECUTE ON [dbo].[proc_BankAccountStaging_del] TO HCVU
GO
