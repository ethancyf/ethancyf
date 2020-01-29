IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_BankAccountVerification_get_byERN]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_BankAccountVerification_get_byERN]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Clark Yip
-- Create date: 28 April 2008
-- Description:	Retrieve the Bank Account display_seq by ERN
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_BankAccountVerification_get_byERN] 	
	@enrolment_ref_no	Char(15)		
AS
BEGIN
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
-- =============================================
-- Return results
-- =============================================
	SELECT	display_Seq, SP_Practice_Display_Seq, tsmp		
FROM	BankAccVerification
WHERE	Enrolment_ref_no = @Enrolment_ref_no

END
GO

GRANT EXECUTE ON [dbo].[proc_BankAccountVerification_get_byERN] TO HCVU
GO
