IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PersonalInformation_upd_Erase]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PersonalInformation_upd_Erase]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	11 Mar 2010
-- Description:		Grant right to [HCSP]
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 26 Aug 2008
-- Description:	Update PersonalInformation Status to Erase For Logging
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Dedrick Ng
-- Modified date: 12 Oct 2009
-- Description:	Add new fields and remove obsolete fields
-- =============================================
CREATE PROCEDURE [dbo].[proc_PersonalInformation_upd_Erase]
	@Voucher_Acc_ID	char(15),
	@Doc_Code	char(20)	
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

	UPDATE [dbo].[PersonalInformation]
	SET
		Record_Status = 'E'
	WHERE
		Voucher_Acc_ID = @Voucher_Acc_ID AND Doc_Code = @Doc_Code
END

GO

GRANT EXECUTE ON [dbo].[proc_PersonalInformation_upd_Erase] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_PersonalInformation_upd_Erase] TO HCVU
GO
