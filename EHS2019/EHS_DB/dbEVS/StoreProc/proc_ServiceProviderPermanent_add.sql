IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderPermanent_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderPermanent_add]
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
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	07 Aug 2018
-- CR No.			CRE17-016 - Checking of PCD status during VSS enrolment
-- Description:		Add Column -	[ServiceProvider].[PCD_Account_Status]
--									[ServiceProvider].[PCD_Enrolment_Status]
--									[ServiceProvider].[PCD_Professional]
--									[ServiceProvider].[PCD_Status_Last_Check_Dtm]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	12 Feb 2016
-- CR No.			CRE15-019
-- Description:		Rename PPI-ePR to eHRSS
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	28 January 2016
-- CR No.			CRE15-008
-- Description:		Remove PPI-ePR enrolment
-- =============================================
-- =============================================
-- Author:		Tommy Cheung
-- Create date: 23 June 2008
-- Description:	Insert the Service Provider to Table
--				ServiceProvider
-- =============================================
-- =============================================
-- Modification History
-- CR No:			INT13-0028 - SP Amendment Report
-- Modified by:		Tommy LAM
-- Modified date:	21 Nov 2013
-- Description:		Add Column -	[ServiceProvider].[Data_Input_By]
--									[ServiceProvider].[Data_Input_Effective_Dtm]
-- =============================================
CREATE PROCEDURE [dbo].[proc_ServiceProviderPermanent_add]
	@enrolment_ref_no char(15), @enrolment_dtm datetime, @sp_id char(8), @sp_hkid char(9),
	@sp_eng_name varchar(40), @sp_chi_name nvarchar(6), @room nvarchar(5), 
	@floor nvarchar(3), @block nvarchar(3), @building varchar(100), @building_chi nvarchar(100), @district char(4),
	@address_code int, @phone_daytime varchar(20), @fax varchar(20), @email varchar(100),
    @record_status char(1), @remark nvarchar(255), @submission_method char(1), @already_joined_EHR char(1),
    @application_printed char(1), @create_by varchar(20), @update_By varchar(20),
	@data_input_by varchar(20), @PCD_Account_Status char(1), @PCD_Enrolment_Status char(1), @PCD_Professional varchar(20), @PCD_Status_Last_Check_Dtm datetime
AS
BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================

	declare @current_dtm datetime

	EXEC [proc_SymmetricKey_open]
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================

	set @current_dtm = getdate()

-- =============================================
-- Return results
-- =============================================

	INSERT INTO ServiceProvider
				(Enrolment_Ref_No,
				 Enrolment_Dtm,
				 SP_ID,
				 Encrypt_Field1,
				 Encrypt_Field2,
				 Encrypt_Field3,
				 Room,
				 [Floor],
				 Block,
				 Building,
				 Building_Chi,
				 District,
				 Address_Code,
				 Phone_Daytime,
				 Fax,
				 Email,
				 Record_Status,
				 Remark,
				 Submission_Method,
				 Already_Joined_EHR,
				 Join_EHR,
				 Application_Printed,
				 Create_Dtm,
				 Create_By,
				 Update_Dtm,
				 Update_By,
				 Effective_Dtm,
				 Data_Input_By,
				 Data_Input_Effective_Dtm,
				 PCD_Account_Status,
				 PCD_Enrolment_Status,
				 PCD_Professional,
				 PCD_Status_Last_Check_Dtm
				)
	VALUES		(@enrolment_ref_no,
				 @enrolment_dtm,
				 @sp_id,
				 EncryptByKey(KEY_GUID('sym_Key'), @sp_hkid),
				 EncryptByKey(KEY_GUID('sym_Key'), @sp_eng_name),
				 EncryptByKey(KEY_GUID('sym_Key'), @sp_chi_name),
				 @room,
				 @floor,
				 @block,
				 @building,
				 @building_chi,
				 @district,
				 @address_code,
				 @phone_daytime,
				 @fax,
				 @email,
				 @record_status,
				 @remark,
				 @submission_method,
				 @already_joined_EHR,
				 NULL,
				 @application_printed,
				 @current_dtm,
				 @create_by,
				 @current_dtm,
				 @update_By,
				 @current_dtm,
				 @data_input_by,
				 @current_dtm,
				 @PCD_Account_Status,
				 @PCD_Enrolment_Status,
				 @PCD_Professional,
				 @PCD_Status_Last_Check_Dtm
				)

EXEC [proc_SymmetricKey_close]
END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderPermanent_add] TO HCVU
GO
