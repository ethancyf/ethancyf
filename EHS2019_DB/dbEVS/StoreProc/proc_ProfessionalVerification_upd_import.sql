IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionalVerification_upd_import]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionalVerification_upd_import]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 30 May 2008
-- Description:	Update ProfessionalVerification for Import Information
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_ProfessionalVerification_upd_import]
	@Enrolment_Ref_No char(15),
	@Professional_Seq smallint,
	@Import_By varchar(20),
	@Import_Dtm datetime,
	@Result char(1),
	@Remark nvarchar(70),
	@Record_Status char(1),
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
		SELECT TSMP FROM [dbo].[ProfessionalVerification]
		WHERE Enrolment_Ref_No = @Enrolment_Ref_No AND Professional_Seq = @Professional_Seq
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

-- Three Case:
-- 1. Newly Import
--	Update [ProfessionalVerification] Import Result & Set Record_Status = I 
	-- Not Yet Import (Import_By & Import_Dtm IS NULL)

-- 2. Re-Import
--	Update [ProfessionalVerification] Import Result
	-- Imported But Not Yet Confirmed / Reject

-- 3. Record Not Exist or Application Reject
--	This Case Validate In UI, will not call this StoreProc


	If @Record_Status Is NOT NULL
	BEGIN
		UPDATE ProfessionalVerification
		Set
			Import_By = @Import_By,
			Import_Dtm = @Import_Dtm,
			Verification_Result = @Result,
			Verification_Remark = @Remark,
			Record_Status = 'I'
		WHERE
			Enrolment_Ref_No = @Enrolment_Ref_No AND Professional_Seq = @Professional_Seq
	END
	ELSE
	BEGIN
		UPDATE ProfessionalVerification
		Set
			Import_By = @Import_By,
			Import_Dtm = @Import_Dtm,
			Verification_Result = @Result,
			Verification_Remark = @Remark
		WHERE
			Enrolment_Ref_No = @Enrolment_Ref_No AND Professional_Seq = @Professional_Seq	
	END

END

GO

GRANT EXECUTE ON [dbo].[proc_ProfessionalVerification_upd_import] TO HCVU
GO
