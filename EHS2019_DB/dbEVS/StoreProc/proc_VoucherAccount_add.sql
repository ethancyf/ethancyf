IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherAccount_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherAccount_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 26 Aug 2008
-- Description:	Insert Voucher Account
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Dedrick Ng
-- Modified date: 30 Sep 2009
-- Description:	Remove obsolete fields
-- =============================================
CREATE PROCEDURE [dbo].[proc_VoucherAccount_add]
	@Voucher_Acc_ID char(15),
	@Scheme_Code char(10),
	--@Voucher_Used money,
	--@Total_Voucher_Amt_Used money,
	@Record_Status char(1),
	@Remark nvarchar(255),
	@Public_Enquiry_Status char(1),
	@Public_Enq_Status_Remark nvarchar(255),
	@Effective_Dtm datetime,
	@Terminate_Dtm datetime,
	@Create_Dtm datetime,
	@Create_By varchar(20),
	@Update_Dtm datetime,
	@Update_By varchar(20),
	@DataEntry_By varchar(20)
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
		DataEntry_By
	)
	VALUES
	(
		@Voucher_Acc_ID,
		@Scheme_Code,
		--@Voucher_Used,
		--@Total_Voucher_Amt_Used,
		@Record_Status,
		@Remark,
		@Public_Enquiry_Status,
		@Public_Enq_Status_Remark,
		@Effective_Dtm,
		@Terminate_Dtm,
		@Create_Dtm,
		@Create_By,
		@Update_Dtm,
		@Update_By,
		@DataEntry_By
	)
	
END

GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccount_add] TO HCVU
GO
