   if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[proc_SpecialPersonalInformation_upd_Validated]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[proc_SpecialPersonalInformation_upd_Validated]
GO

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

-- =============================================
-- Author:		Dedrick Ng
-- Create date: 29 Jan 2010
-- Description:	Update TempPersonalInformation, Mark Validating = 'N'
-- =============================================
CREATE PROCEDURE [dbo].[proc_SpecialPersonalInformation_upd_Validated]
	@Special_Acc_ID	char(15)
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

	UPDATE [dbo].[SpecialPersonalInformation]
	SET
		Validating = 'N',
		Check_Dtm = GetDate()
	WHERE 
		Special_Acc_ID = @Special_Acc_ID

END
GO

--Grant Access Right to user
--HCVU (for voucher unit platform)
--HCSP (for service provider platform)
--HCPUBLIC (for public access platform)
Grant execute on [dbo].[proc_SpecialPersonalInformation_upd_Validated] to HCVU
GO      