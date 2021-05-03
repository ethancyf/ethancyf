IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PersonalInformationName_updBySmartIC]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PersonalInformationName_updBySmartIC]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- CR No.:			CRE20-023
-- Modified by:		Martin Tang
-- Modified date:	20 Apr 2021
-- Description:		Extend patient name's maximum length
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE18-019 (To read new Smart HKIC in eHS(S))
-- Modified by:		Winnie SUEN
-- Modified date:	5 Dec 2018
-- Description:		Add [SmartID_Ver]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- CR No.			CRE15-014
-- Modified date:	30 Dec 2015
-- Description:		Change [Chi_Name] nvarchar(6) -> nvarchar(12)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	12 Mar 2010
-- Description:		Mark the record is created by SmartIC
-- =============================================
-- =============================================
-- Author:		STANLEY CHAN
-- Create date: 23 DEC 2009
-- Description:	update the personal information Name By Smart IC
-- =============================================
CREATE PROCEDURE [dbo].[proc_PersonalInformationName_updBySmartIC]
	-- Add the parameters for the stored procedure here
	@Voucher_Acc_ID char(15),
	@Eng_Name		varchar(100),
    @Chi_Name		nvarchar(12),
    @CCcode1		char(5),
	@CCcode2		char(5),
	@CCcode3		char(5),
	@CCcode4		char(5),
	@CCcode5		char(5),
	@CCcode6		char(5),
	@Update_By		varchar(20),
	@Doc_code		char(10),
	@TSMP			timestamp,
	@SmartID_Ver	varchar(5)
	                         
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	if (select TSMP from PersonalInformation 
		where Voucher_Acc_ID = @Voucher_Acc_ID and
		Doc_code = @Doc_code) != @TSMP
	begin
		Raiserror('00011', 16, 1)
		return @@error
	end
-- =============================================
-- Initialization
-- =============================================
	EXEC [proc_SymmetricKey_open]

    Update PersonalInformation
	set 
	Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @Eng_Name),
    Encrypt_Field3 = EncryptByKey(KEY_GUID('sym_Key'), @Chi_Name),
    Encrypt_Field4 = EncryptByKey(KEY_GUID('sym_Key'), @CCcode1),
	Encrypt_Field5 = EncryptByKey(KEY_GUID('sym_Key'), @CCcode2),
	Encrypt_Field6 = EncryptByKey(KEY_GUID('sym_Key'), @CCcode3),
	Encrypt_Field7 = EncryptByKey(KEY_GUID('sym_Key'), @CCcode4),
	Encrypt_Field8 = EncryptByKey(KEY_GUID('sym_Key'), @CCcode5),
	Encrypt_Field9 = EncryptByKey(KEY_GUID('sym_Key'), @CCcode6),
	Update_By = @Update_By,
	Update_Dtm = getdate(),
	Create_by_SmartID = 'Y',	
	SmartID_Ver = @SmartID_Ver
	--Verify_By_SmartID = 'Y'

	where Voucher_Acc_ID = @Voucher_Acc_ID
	and Doc_code = @Doc_code

	EXEC [proc_SymmetricKey_close]
END
GO

GRANT EXECUTE ON [dbo].[proc_PersonalInformationName_updBySmartIC] TO HCSP
GO
