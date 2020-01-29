IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_BankAccountStaging_upd_Status]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_BankAccountStaging_upd_Status]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Clark YIP
-- Create date: 23 Jun 2008
-- Description:	Update Bank Accoutn Staging Status
-- =============================================
CREATE PROCEDURE [dbo].[proc_BankAccountStaging_upd_Status]
	@enrolment_ref_no char(15), 
	@display_seq smallint,
	@SP_Practice_Display_Seq smallint,
	@record_status char(1),
	@update_By varchar(20),
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
	IF (SELECT TSMP FROM BankAccountStaging
		WHERE Enrolment_Ref_No = @enrolment_ref_no and
				Display_Seq = @display_seq and SP_Practice_Display_Seq = @SP_Practice_Display_Seq ) != @tsmp
	BEGIN
		RAISERROR('00011', 16, 1)
	END
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	UPDATE	BankAccountStaging
	SET		Record_Status = @record_status,			
			Update_Dtm = getdate(),
			Update_By = @update_By

	WHERE	Enrolment_Ref_No = @enrolment_ref_no and
			Display_Seq = @display_seq and
			SP_Practice_Display_Seq = @SP_Practice_Display_Seq
END
GO

GRANT EXECUTE ON [dbo].[proc_BankAccountStaging_upd_Status] TO HCVU
GO
