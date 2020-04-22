IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SpecialAccount_Add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SpecialAccount_Add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	26 Jan 2010
-- Description:		Refer proc_SpecialAccount_Add_ByDate
-- =============================================
-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 30 Sep 2009
-- Description:	convert the temporary account special account
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	08 Oct 2009
-- Description:		Filter out the temporary account which account purpose
--					are in 'A' (For Amendment) and 'O' (for original record)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	14 Oct 2009
-- Description:		Handle DocType.IMMD_Validate_Avail and Force_Manual_Validate
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	30 Nov 2009
-- Description:		1. Only those temporary account with transaction will be
--					   convert to special account
--					2. Set Original_record_status in VoucherTranSuspendLOG
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	30 Nov 2009
-- Description:		1. Do not count the following case: 
--					Account: Confirmed, Transaction: Pending Confirmation
-- =============================================
   
CREATE proc [dbo].[proc_SpecialAccount_Add]   
as  
begin  
	Declare @currentDate as Datetime
	Set @currentDate = GetDate()
	exec proc_SpecialAccount_Add_ByDate @currentDate
end 
GO
