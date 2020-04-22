   if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[proc_SpecialAccount_upd_Status]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[proc_SpecialAccount_upd_Status]
GO

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

-- =============================================
-- Author:		Dedrick Ng
-- Create date: 1 Feb 2010
-- Description:	Update SpecialAccount Record Status
-- =============================================
CREATE PROCEDURE [dbo].[proc_SpecialAccount_upd_Status]
	@Special_Acc_ID char(15),
	@Scheme_Code char(10),
	@Record_Status char(1)
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
		Record_Status = @Record_Status
	WHERE 
		Special_Acc_ID = @Special_Acc_ID AND Scheme_Code = @Scheme_Code
END
GO

--Grant Access Right to user
--HCVU (for voucher unit platform)
--HCSP (for service provider platform)
--HCPUBLIC (for public access platform)
Grant execute on [dbo].[proc_SpecialAccount_upd_Status] to HCVU
GO        