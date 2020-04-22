  if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[proc_TempVoucherAccPendingVerify_add]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[proc_TempVoucherAccPendingVerify_add]
GO

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


-- =============================================
-- Modification History
-- Modified by:		Paul Yip
-- Modified date:	27 May 2010
-- Description:		Add Doc Type
-- =============================================
-- =============================================
-- Author:			Clark Yip
-- Create date:		23 Oct 2008
-- Description:		Add record into TempVoucherAccPendingVerify table
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE 	[dbo].[proc_TempVoucherAccPendingVerify_add] 
@temp_VR_Acct_ID		 char(15),
@scheme					 char(10),
@acc_type				 char(1)

as
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
-- =============================================
-- Return results
-- =============================================

insert into TempVoucherAccPendingVerify ([Voucher_Acc_ID],[Scheme_Code],[First_Validate_Dtm],[Acc_Type])
Values (@temp_VR_Acct_ID, @scheme, getdate(), @acc_type)

END
GO

--Grant Access Right to user
--HCVU (for voucher unit platform)
--HCSP (for service provider platform)
--HCPUBLIC (for public access platform)
Grant execute on [dbo].[proc_TempVoucherAccPendingVerify_add] to HCVU
GO
      