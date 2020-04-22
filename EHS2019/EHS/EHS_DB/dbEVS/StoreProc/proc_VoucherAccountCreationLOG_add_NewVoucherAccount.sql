IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherAccountCreationLOG_add_NewVoucherAccount]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherAccountCreationLOG_add_NewVoucherAccount]
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
-- Modified by:		Paul Yip
-- Modified date:	20 July 2010
-- Description:		Add Create_By_BO
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	25 Mar 2010
-- Description:		Add Create_By_SmartID
--					The Create_By_SmartID should be 
--					refer to the Existing Temp Personal Information's Create_By_SmartID
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 26 Aug 2008
-- Description:	Insert Voucher Account Creation LOG By Old Temp Voucher Account LOG
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 29 DEC 2008
-- Description:	Validate Account Creation LOG: Voucher_Acc_Type = 'V'
-- =============================================
CREATE PROCEDURE [dbo].[proc_VoucherAccountCreationLOG_add_NewVoucherAccount]
	@Voucher_Acc_ID char(15),
	@Temp_Voucher_Acc_ID char(15)
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

	DECLARE @Voucher_Acc_Type CHAR(1)
	SELECT [Voucher_Acc_Type]
	FROM [VoucherAccountCreationLOG]
	WHERE Voucher_Acc_ID = @Temp_Voucher_Acc_ID
	
	IF @Voucher_Acc_Type = 'T'
	BEGIN
		INSERT INTO [dbo].[VoucherAccountCreationLOG]
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
			Create_By_BO,
			SmartID_Ver
		)
		SELECT 
			GetDate(),
			@Voucher_Acc_ID,
			'V',
			VACL.Consent_Form_Printed,
			VACL.SP_ID,
			VACL.SP_Practice_Display_Seq,
			VACL.Create_Dtm,
			VACL.Create_By,
			VACL.Update_Dtm,
			VACL.Update_By,
			VACL.DataEntry_By,
			TPI.Create_By_SmartID,
			VACL.Create_By_BO,
			TPI.SmartID_Ver
		FROM [dbo].[VoucherAccountCreationLOG] VACL
			INNER JOIN [TempPersonalInformation] TPI ON TPI.Voucher_Acc_ID = VACL.Voucher_Acc_ID
		
		WHERE VACL.Voucher_Acc_ID = @Temp_Voucher_Acc_ID
	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[VoucherAccountCreationLOG]
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
			Create_By_BO,
			SmartID_Ver
		)
		SELECT 
			GetDate(),
			@Voucher_Acc_ID,
			'V',
			Consent_Form_Printed,
			SP_ID,
			SP_Practice_Display_Seq,
			Create_Dtm,
			Create_By,
			Update_Dtm,
			Update_By,
			DataEntry_By,
			'N',
			Create_By_BO,
			NULL
		FROM [dbo].[VoucherAccountCreationLOG]
		
		WHERE Voucher_Acc_ID = @Temp_Voucher_Acc_ID
	END	

END

GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccountCreationLOG_add_NewVoucherAccount] TO HCVU
GO
