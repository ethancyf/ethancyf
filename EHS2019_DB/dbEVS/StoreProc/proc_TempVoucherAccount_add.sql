IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccount_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccount_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Modified by:	    Winnie SUEN
-- Modified date:	26 Aug 2018
-- Description:		Add [SourceApp]
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE14-016 (To introduce 'Deceased' status into eHS)
-- Modified by:	    Winnie SUEN
-- Modified date:   15 Nov 2017
-- Description:		Add [Deceased]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	20 Jan 2010
-- Description:		Add Original_Amend_Acc_ID, Create_By_BO
-- =============================================
-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 05 May 2008
-- Description:	Insert Temporary Voucher Recipient Account Information
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	21 Aug 2009
-- Description:		Remove Dummy Column & Add Orginal_Acc_ID
-- =============================================


-- =============================================
-- Remark
--	Confirm Date is pass (Amendment Case)
-- =============================================

CREATE PROCEDURE [dbo].[proc_TempVoucherAccount_add] 
	@Voucher_Acc_ID char(15),
	@Scheme_Code char(10),
	--@Voucher_Used smallint,
	--@Total_Voucher_Amt_Used money,
	@Validated_Acc_ID char(15),
	@Record_Status char(1),
	@Account_Purpose char(1),
	@Confirm_Dtm datetime,
	--@Last_Fail_Validate_Dtm datetime,
	@Create_By varchar(20),
	--@Update_By varchar(20),
	@DataEntry_By varchar(20),
	@Original_Acc_ID char(15),
	@Original_Amend_Acc_ID char(15),
	@Create_By_BO char(1),
	@Deceased	char(1),
	@SourceApp	VARCHAR(10)
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

	IF @Confirm_Dtm IS NULL 
		Set @Confirm_Dtm = GetDate()

	IF LTRIM(RTRIM(@DataEntry_By)) = ''
	BEGIN
		INSERT INTO [TempVoucherAccount]
		(
			[Voucher_Acc_ID], [Scheme_Code], --[Voucher_Used], [Total_Voucher_Amt_Used], 
			[Validated_Acc_ID], 
			[Record_Status], [Account_Purpose], [Confirm_Dtm], [Last_Fail_Validate_Dtm], [Create_Dtm],
			[Create_By], [Update_Dtm], [Update_By], [DataEntry_By], [Original_Acc_ID], [Original_Amend_Acc_ID],
			[Create_By_BO], [Deceased], [SourceApp]
		)
		VALUES
		(
			@Voucher_Acc_ID, @Scheme_Code, --0, 0, 
			@Validated_Acc_ID,
			@Record_Status, @Account_Purpose, @Confirm_Dtm, NULL, GetDate(),
			@Create_By, GetDate(), @Create_By, '', @Original_Acc_ID, @Original_Amend_Acc_ID,
			@Create_By_BO, @Deceased, @SourceApp
		)
	END
	ELSE
	BEGIN
		INSERT INTO [TempVoucherAccount]
		(
			[Voucher_Acc_ID], [Scheme_Code], --[Voucher_Used], [Total_Voucher_Amt_Used], 
			[Validated_Acc_ID], 
			[Record_Status], [Account_Purpose], [Confirm_Dtm], [Last_Fail_Validate_Dtm], [Create_Dtm],
			[Create_By], [Update_Dtm], [Update_By], [DataEntry_By], [Original_Acc_ID], [Original_Amend_Acc_ID],
			[Create_By_BO], [Deceased], [SourceApp]
		)
		VALUES
		(
			@Voucher_Acc_ID, @Scheme_Code, --0, 0, 
			@Validated_Acc_ID,
			@Record_Status, @Account_Purpose, NULL, NULL, GetDate(),
			@Create_By, GetDate(), @Create_By, @DataEntry_By, @Original_Acc_ID, @Original_Amend_Acc_ID,
			@Create_By_BO, @Deceased, @SourceApp
		)
	END

END
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_add] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_add] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_add] TO WSEXT
GO