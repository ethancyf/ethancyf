IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_BankAccountVerification_get_byErnDisplaySeq]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_BankAccountVerification_get_byErnDisplaySeq]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Clark Yip
-- Create date: 20 Jun 2008
-- Description:	Retrieve the Bank Account TSMP by ERN, display_seq
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_BankAccountVerification_get_byErnDisplaySeq] 	
	@enrolment_ref_no	Char(15)
	,@display_seq	smallint
	,@sp_practice_Display_Seq smallint
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
	SELECT	tsmp
FROM	BankAccVerification
WHERE	Enrolment_ref_no = @Enrolment_ref_no
AND	Display_Seq = @display_seq
AND sp_practice_Display_Seq = @sp_practice_Display_Seq

END
GO

GRANT EXECUTE ON [dbo].[proc_BankAccountVerification_get_byErnDisplaySeq] TO HCVU
GO
