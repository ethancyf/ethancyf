IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderStaging_add_forReject]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderStaging_add_forReject]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	03 Jul 2018
-- CR No.			CRE17-016
-- Description:		Checking of PCD status during VSS enrolment
--					Add [Join_PCD] column
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
-- Author:		Kathy LEE
-- Create date: 12 August 2009
-- Description:	Insert the Service Provider to Table
--				ServiceProviderStaging for Reject or Remove case
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================
CREATE PROCEDURE [dbo].[proc_ServiceProviderStaging_add_forReject]
	@enrolment_ref_no char(15), @enrolment_dtm datetime, @sp_id char(8), @sp_hkid char(9),
	@sp_eng_name varchar(40), @sp_chi_name nvarchar(6), @room nvarchar(5), 
	@floor nvarchar(3), @block nvarchar(3), @building varchar(100), @building_chi nvarchar(100), @district char(4),
	@address_code int, @phone_daytime varchar(20), @fax varchar(20), @email varchar(100), @email_changed char(1),
    @record_status char(1), @remark nvarchar(255), @submission_method char(1), @already_joined_EHR char(1), @join_PCD char(1),
    @application_printed char(1), @create_by varchar(20), @update_By varchar(20)
AS
BEGIN

	SET NOCOUNT ON;

OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
if @enrolment_dtm is NULL
		SELECT @enrolment_dtm = getdate()

-- =============================================
-- Return results
-- =============================================

	INSERT INTO ServiceProviderStaging
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
				 Email_Changed,
				 Record_Status,
				 Remark,
				 Submission_Method,
				 Already_Joined_EHR,
				 Join_EHR,
				 Join_PCD,
				 Application_Printed,
				 Create_Dtm,
				 Create_By,
				 Update_Dtm,
				 Update_By
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
				 @email_changed,
				 @record_status,
				 @remark,
				 @submission_method,
				 @already_joined_EHR,
				 NULL,
				 @join_PCD,
				 @application_printed,
				 getdate(),
				 @create_by,
				 getdate(),
				 @update_By)

CLOSE SYMMETRIC KEY sym_Key

END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderStaging_add_forReject] TO HCVU
GO