   if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[proc_SpecialAccount_upd_Invalid]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[proc_SpecialAccount_upd_Invalid]
GO

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

-- =============================================
-- Author:		Dedrick Ng
-- Create date: 24 Feb 2010
-- Description:	Update SpecialAccount To Invalid
-- =============================================
CREATE PROCEDURE [dbo].[proc_SpecialAccount_upd_Invalid]
	@Voucher_Acc_ID	char(15),
	@Scheme_Code char(10)
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

	UPDATE [dbo].[SpecialAccount]
	SET
		Record_Status = 'I',
		Last_Fail_Validate_Dtm = GetDate()
	WHERE 
		Special_Acc_ID = @Voucher_Acc_ID AND Scheme_Code = @Scheme_Code AND Record_Status = 'P'

END
GO

--Grant Access Right to user
--HCVU (for voucher unit platform)
--HCSP (for service provider platform)
--HCPUBLIC (for public access platform)
Grant execute on [dbo].[proc_SpecialAccount_upd_Invalid] to HCVU
GO       