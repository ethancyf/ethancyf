   if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[proc_VoucherAccount_add_BySpecial]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[proc_VoucherAccount_add_BySpecial]
GO

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

-- =============================================
-- Modification History
-- CR No.:			CRE14-016 (To introduce 'Deceased' status into eHS)
-- Modified by:	    Winnie SUEN
-- Modified date:   15 Nov 2017
-- Description:		Add [Deceased]
-- =============================================
-- =============================================
-- Author:		Dedrick Ng
-- Create date: 1 Feb 2010
-- Description:	Insert Voucher Account By Special Account
-- =============================================
CREATE PROCEDURE [dbo].[proc_VoucherAccount_add_BySpecial]
	@Voucher_Acc_ID char(15),
	@Special_Acc_ID char(15),
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
	INSERT INTO [dbo].[VoucherAccount]
	(
		Voucher_Acc_ID,
		Scheme_Code,
		--Voucher_Used,
		--Total_Voucher_Amt_Used,
		Record_Status,
		Remark,
		Public_Enquiry_Status,
		Public_Enq_Status_Remark,
		Effective_Dtm,
		Terminate_Dtm,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By,
		DataEntry_By,
		Deceased
	)	
	SELECT
		@Voucher_Acc_ID,
		Scheme_Code,
		'A',
		NULL,
		'A',
		NULL,
		GetDate(),
		NULL,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By,
		DataEntry_By,
		Deceased
	FROM [dbo].[SpecialAccount]
	WHERE Special_Acc_ID = @Special_Acc_ID	AND Scheme_Code = @Scheme_Code

END
GO

--Grant Access Right to user
--HCVU (for voucher unit platform)
--HCSP (for service provider platform)
--HCPUBLIC (for public access platform)
Grant execute on [dbo].[proc_VoucherAccount_add_BySpecial] to HCVU
GO       