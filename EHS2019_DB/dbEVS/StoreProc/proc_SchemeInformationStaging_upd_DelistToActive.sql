IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeInformationStaging_upd_DelistToActive]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchemeInformationStaging_upd_DelistToActive]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Lawrence TSANG
-- Create date:	25 June 2009
-- Description:	Update SchemeInformationStaging record status, from Delisted to Active
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	13 July 2009
-- Description:		Set Remark to NULL, Record_Status to 'A'
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	27 July 2009
-- Description:		Add timestamp checking
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_SchemeInformationStaging_upd_DelistToActive]
	@Enrolment_Ref_No	char(15),
	@Scheme_Code		char(10),
	@Update_By			varchar(20),
	@TSMP				timestamp
AS
BEGIN
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF (
		SELECT	TSMP
		FROM	SchemeInformationStaging
		WHERE	Enrolment_Ref_No = @Enrolment_Ref_No 
					AND Scheme_Code = @Scheme_Code
	) != @TSMP
	BEGIN
		RAISERROR('00011', 16, 1)
		RETURN @@error
	END
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================
	UPDATE	SchemeInformationStaging
	
	SET		Record_Status = 'A',
			Update_By = @Update_By,
			Update_Dtm = GETDATE(),
			Delist_Status = NULL,
			Delist_Dtm = NULL,
			Effective_Dtm = NULL,
			Logo_Return_Dtm = NULL,
			Remark = NULL
			
	WHERE	Enrolment_Ref_No = @Enrolment_Ref_No 
				AND Scheme_Code = @Scheme_Code

END
GO

GRANT EXECUTE ON [dbo].[proc_SchemeInformationStaging_upd_DelistToActive] TO HCVU
GO
