IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SPAccountUpdate_get_byERN]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SPAccountUpdate_get_byERN]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kathy LEE
-- Create date: 30 May 2008
-- Description:	Retrieve the SP Account Update details
--				from Table SPAccountUpdate
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Clark YIP
-- Modified date: 07 May 2009
-- Description:	  Get the Scheme_Confirm
-- =============================================
-- =============================================
-- Modification History
-- CR No:			INT13-0028 - SP Amendment Report
-- Modified by:		Tommy LAM
-- Modified date:	18 Nov 2013
-- Description:		Add Column -	[SPAccountUpdate].[Data_Input_By]
-- =============================================
CREATE PROCEDURE [dbo].[proc_SPAccountUpdate_get_byERN]
	@enrolment_ref_no	char(15)
AS
BEGIN
	
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
	SELECT	Enrolment_Ref_No, SP_ID, Upd_SP_Info, Upd_Bank_Account,
			Upd_Professional, Issue_Token, Progress_Status, Update_Dtm,
			Update_By, TSMP, scheme_confirm,
			Data_Input_By

	FROM	SPAccountUpdate
	WHERE	Enrolment_Ref_No = @enrolment_ref_no
	
END
GO

GRANT EXECUTE ON [dbo].[proc_SPAccountUpdate_get_byERN] TO HCVU
GO
