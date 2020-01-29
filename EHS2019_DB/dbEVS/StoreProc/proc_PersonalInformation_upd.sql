IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PersonalInformation_upd]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PersonalInformation_upd]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE18-019 (To read new Smart HKIC in eHS(S))
-- Modified by:		Winnie SUEN
-- Modified date:	5 Dec 2018
-- Description:		Add [Create_By_SmartID], [SmartID_Ver] as input arg instead of hard code
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
-- Modified by:	    Kathy LEE
-- Modified date:	07 Apr 2010
-- Description:  Mark the record is NOT created by SmartIC  
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Kathy LEE
-- Modified date:	04 Feb 2010
-- Description:		Set Update_Dtm = Getdate()
-- =============================================
-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 10 June 2008
-- Description:	update the personal information 
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Kathy LEE
-- Modified date:	15 Sep 2009
-- Description:		1. Remove EC_Date
--					2. Add Foreign_Passport_No, Doc_code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Paul Yip
-- Modified date:	6 Nov 2009
-- Description:		1. change Foreign_Passport_No to char(20)
-- =============================================
CREATE PROCEDURE [dbo].[proc_PersonalInformation_upd]
	-- Add the parameters for the stored procedure here
	@Voucher_Acc_ID char(15),
    @Eng_Name varchar(40),
    @Chi_Name nvarchar(12),
    @CCcode1 char(5),
	@CCcode2 char(5),
	@CCcode3 char(5),
	@CCcode4 char(5),
	@CCcode5 char(5),
	@CCcode6 char(5),
    @Sex char(1),
	@Update_By varchar(20),
	@Foreign_Passport_No char(20),
	@Doc_code char(10),
	@TSMP timestamp,
	@Create_By_SmartID char(1),
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
	
	--if @doc_code = 'HKIC'
	--	begin
	--		set @create_by_smartID = 'N'
	--	end
	--else
	--	begin
	--		set @create_by_smartID = NULL
	--	end
-- =============================================
-- Initialization
-- =============================================
OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key

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
    Sex = @Sex,
	Update_By = @Update_By,
	Update_Dtm = getdate(),
	Foreign_Passport_No = @Foreign_Passport_No,	
	Create_by_SmartID = @Create_By_SmartID, 
	SmartID_Ver = @SmartID_Ver

	where Voucher_Acc_ID = @Voucher_Acc_ID
	and Doc_code = @Doc_code

CLOSE SYMMETRIC KEY sym_Key
END
GO

GRANT EXECUTE ON [dbo].[proc_PersonalInformation_upd] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_PersonalInformation_upd] TO HCVU
GO
