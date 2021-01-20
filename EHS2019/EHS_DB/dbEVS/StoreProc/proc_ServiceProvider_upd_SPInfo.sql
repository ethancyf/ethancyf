IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProvider_upd_SPInfo]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProvider_upd_SPInfo]
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
-- Create date: 26 June 2008
-- Description:	Update the Service Provider Info in
--				Table "ServiceProvider"
-- =============================================
-- =============================================
-- Modification History
-- CR No:			INT13-0028 - SP Amendment Report
-- Modified by:		Tommy LAM
-- Modified date:	21 Nov 2013
-- Description:		Add Column -	[ServiceProvider].[Tentative_Email_Input_By]
--									[ServiceProvider].[Data_Input_By]
--									[ServiceProvider].[Data_Input_Effective_Dtm]
-- =============================================
CREATE PROCEDURE [dbo].[proc_ServiceProvider_upd_SPInfo]
	@sp_id char(8), @sp_eng_name varchar(40), @sp_chi_name nvarchar(6), @room nvarchar(5),
	@floor nvarchar(3), @block nvarchar(3), @building varchar(100), @building_chi nvarchar(100),
	@district char(4), @address_Code int, @phone_daytime varchar(20),
	@fax varchar(20), @tentative_email varchar(255), @update_by	varchar(20),
	@data_input_by varchar(20), @tsmp timestamp
AS
BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================

	declare @current_dtm datetime

-- =============================================
-- Validation 
-- =============================================
	IF (SELECT TSMP FROM ServiceProvider
		WHERE SP_ID = @sp_id) != @tsmp
	BEGIN
		RAISERROR('00011', 16, 1)
		return @@error
	END
-- =============================================
-- Initialization
-- =============================================

	set @current_dtm = getdate()

-- =============================================
-- Return results
-- =============================================
EXEC [proc_SymmetricKey_open]

-- If @tentative_email is DBNull, Do not copy the tentative email to Permanent

	IF @tentative_email IS NULL
	BEGIN
		UPDATE	ServiceProvider
		Set		--SP_Eng_Name = @sp_eng_name,
				--SP_Chi_Name = @sp_chi_name,
				Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @sp_eng_name),
				Encrypt_Field3 = EncryptByKey(KEY_GUID('sym_Key'), @sp_chi_name),
				Room = @room,
				[Floor] = @floor,
				Block = @block,
				Building = @building,
				District = @district,
				Address_Code = @address_code,
				Phone_Daytime = @phone_daytime,
				Fax = @fax,
				--Tentative_Email = @tentative_email,
				UnderModification = Null,
				Update_Dtm = @current_dtm,
				Update_By = @update_by,
				Data_Input_By = @data_input_by,
				Data_Input_Effective_Dtm = @current_dtm
		WHERE	SP_ID = @sp_id
	END
	ELSE
	BEGIN
		UPDATE	ServiceProvider
		Set		--SP_Eng_Name = @sp_eng_name,
				--SP_Chi_Name = @sp_chi_name,
				Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @sp_eng_name),
				Encrypt_Field3 = EncryptByKey(KEY_GUID('sym_Key'), @sp_chi_name),
				Room = @room,
				[Floor] = @floor,
				Block = @block,
				Building = @building,
				District = @district,
				Address_Code = @address_code,
				Phone_Daytime = @phone_daytime,
				Fax = @fax,
				Tentative_Email = @tentative_email,
				UnderModification = Null,
				Update_Dtm = @current_dtm,
				Update_By = @update_by,
				Tentative_Email_Input_By = @data_input_by,
				Data_Input_By = @data_input_by,
				Data_Input_Effective_Dtm = @current_dtm
		WHERE	SP_ID = @sp_id
	END


	

EXEC [proc_SymmetricKey_close]

END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProvider_upd_SPInfo] TO HCVU
GO
