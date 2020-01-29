IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_TempVoucherAccount_after_del')
	DROP TRIGGER [dbo].[tri_TempVoucherAccount_after_del] 
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================  
-- Modification History
-- Modified by:		Koala CHENG
-- CR No.			CRE17-018 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19
-- Modified date:	16 Aug 2018 
-- Description:		Add new column [SourceApp]
-- ============================================= 
-- =============================================
-- Modification History
-- CR No.:			CRE14-016 (To introduce 'Deceased' status into eHS)
-- Modified by:	    Winnie SUEN
-- Modified date:   15 Nov 2017
-- Description:		Add [Deceased]
-- =============================================
-- =============================================
-- Author:		Tommy Cheung
-- Create date: 21 August 2008
-- Description:	Trigger an insert statment into TempVoucherAccountLOG
--				when a row is deleted from TempVoucherAccount
-- =============================================
CREATE TRIGGER [dbo].[tri_TempVoucherAccount_after_del]
   ON  [dbo].[TempVoucherAccount]
   AFTER DELETE
AS 
BEGIN

	SET NOCOUNT ON;

    INSERT INTO TempVoucherAccountLOG
		(System_Dtm,
		Voucher_Acc_ID,
		Scheme_Code,
		Validated_Acc_ID,
		Record_Status,
		Account_Purpose,
		Confirm_Dtm,
		Last_Fail_Validate_Dtm,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By,
		DataEntry_By,
		Original_Acc_ID,
		Deceased,
		SourceApp
	)
	SELECT getdate(),
		Voucher_Acc_ID,
		Scheme_Code,
		Validated_Acc_ID,
		Record_Status,
		Account_Purpose,
		Confirm_Dtm,
		Last_Fail_Validate_Dtm,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By,
		DataEntry_By,
		Original_Acc_ID,
		Deceased,
		SourceApp
	FROM deleted

END
GO
