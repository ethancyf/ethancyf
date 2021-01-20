IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherAccountCreationLOG_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherAccountCreationLOG_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE18-019 (To read new Smart HKIC in eHS(S))
-- Modified by:		Winnie SUEN
-- Modified date:	5 Dec 2018
-- Description:		Add [SmartID_Ver]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Paul Yip
-- Modified date:	20 July 2010
-- Description:		Add Create_by_BO field
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	23 Dec 2009
-- Description:		Add Create_by_SmartID field
-- =============================================
-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 03 May 2008
-- Description:	Insert Voucher Account information 
--				into creation log
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	21 Aug 2009
-- Description:		Remove Dummy Column
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_VoucherAccountCreationLOG_add]
	@Voucher_Acc_ID char(15),
	@Voucher_Acc_Type char(1), 
	@Consent_Form_Printed char(1),
	@SP_ID char(8),
	@SP_Practice_Display_Seq integer,
	@Create_By varchar(20),
	--@Update_By varchar(20),
	@DataEntry_By varchar(20),
	@Create_by_SmartID char(1),
	@Create_by_BO char(1),
	@SmartID_Ver varchar(5)
WITH RECOMPILE
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

	INSERT INTO [VoucherAccountCreationLOG]
	(
		Transaction_Dtm,
		Voucher_Acc_ID, 
		Voucher_Acc_Type, 
		Consent_Form_Printed,
		SP_ID,
		SP_Practice_Display_Seq,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By,
		DataEntry_By,
		Create_By_SmartID,
		Create_by_BO,
		SmartID_Ver
	)
	VALUES
	(
		getdate(),
		@Voucher_Acc_ID, 
		@Voucher_Acc_Type, 
		'N',
		@SP_ID,
		@SP_Practice_Display_Seq,
		getdate(),
		@Create_By,
		getdate(),
		@Create_By,
		@DataEntry_By,
		@Create_by_SmartID,
		@Create_by_BO,
		@SmartID_Ver
	)
END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccountCreationLOG_add] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccountCreationLOG_add] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccountCreationLOG_add] TO WSEXT
Go

