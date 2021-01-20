IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderStaging_upd]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderStaging_upd]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 24 May 2008
-- Description:	Update Service Provider Infomation in
--				Table "ServiceProviderStaging" 
-- =============================================
CREATE PROCEDURE [dbo].[proc_ServiceProviderStaging_upd]
	@enrolment_ref_no char(15), @sp_hkid char(9),
	@sp_eng_name varchar(40), @sp_chi_name nvarchar(6), @room nvarchar(5), 
	@floor nvarchar(3), @block nvarchar(3), @building varchar(100), @building_chi nvarchar(100), @district char(4),
	@address_code int, @phone_daytime varchar(20), @fax varchar(20), @email varchar(255), @email_changed char(1),
    @record_status char(1), @remark nvarchar(255), @update_By varchar(20), @tsmp timestamp
AS
BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF (SELECT TSMP FROM ServiceProviderStaging
		WHERE Enrolment_Ref_No = @enrolment_ref_no) != @tsmp
	BEGIN
		RAISERROR('00011', 16, 1)
		return @@error
	END
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

EXEC [proc_SymmetricKey_open]

	UPDATE	ServiceProviderStaging
	SET		-- SP_HKID = @sp_hkid,
			-- SP_Eng_Name = @sp_eng_name,
			-- SP_Chi_Name = @sp_chi_name,
			Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @sp_hkid),
			Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @sp_eng_name),
			Encrypt_Field3 = EncryptByKey(KEY_GUID('sym_Key'), @sp_chi_name),
			Room = @room,
			[Floor] = @floor,
			Block = @block,
			Building = @building,
			Building_Chi =@building_chi,
			District = @district,
			Address_Code = @address_code,
			Phone_Daytime = @phone_daytime,
			Fax = @fax,
			Email = @email,
			Email_Changed = @email_changed,
			Record_Status = @record_status,
			Remark = @remark,
			Update_Dtm = getdate(),
			Update_By = @update_By

	 WHERE Enrolment_Ref_No = @enrolment_ref_no

EXEC [proc_SymmetricKey_close]
END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderStaging_upd] TO HCVU
GO
