IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderStaging_get_byERN]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderStaging_get_byERN]
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
-- Create date: 18 May 2008
-- Description:	Retrieve the Service Provider from Table
--				ServiceProviderStaging
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_ServiceProviderStaging_get_byERN]
	@enrolment_ref_no	char(15)
AS
BEGIN
-- =============================================
-- Declaration
-- =============================================
DECLARE	@record_id int,
		@address_eng varchar(255),
		@address_chi nvarchar(255),
		@district_code char(5),
		@eh_eng varchar(255),
		@eh_chi varchar(255)

DECLARE @tmp_SP table ( Enrolment_Ref_No char(15),
						Enrolment_Dtm datetime,
						SP_ID char(8),
						Encrypt_Field1	varbinary(100),
						Encrypt_Field2	varbinary(100),
						Encrypt_Field3	varbinary(100),
						Room nvarchar(5),
						[Floor] nvarchar(3),
						Block nvarchar(3),
						Building varchar(255),
						Building_Chi nvarchar(255) collate database_default,
						District char(4),
						Address_Code int,
						Phone_Daytime varchar(20),
						Fax	varchar(20),
						Email varchar(255),
						Email_Changed char(1),
						Record_Status char(1),
						Remark nvarchar(255),
						Submission_Method char(1),
						Already_Joined_EHR char(1),
						Join_PCD char(1),
						Create_Dtm datetime,
						Create_By varchar(20),
						Update_Dtm datetime,
						Update_By varchar(20))

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================

INSERT INTO @tmp_SP (Enrolment_Ref_No,
					Enrolment_Dtm,
					SP_ID,
					--SP_HKID,
					--SP_Eng_Name,
					--SP_Chi_Name,
					Encrypt_Field1,
					Encrypt_Field2,
					Encrypt_Field3,
					Room,
					Floor,
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
					Join_PCD,
					Create_Dtm,
					Create_By,
					Update_Dtm,
					Update_By)

SELECT Enrolment_Ref_No, Enrolment_Dtm, SP_ID, Encrypt_Field1, Encrypt_Field2, Encrypt_Field3, Room, [Floor],
		Block, Building, Building_Chi, District, Address_Code, Phone_Daytime,
		Fax, Email, Email_Changed, Record_Status, Remark, Submission_Method, Already_Joined_EHR, Join_PCD,
		Create_Dtm, Create_By, Update_Dtm, Update_By
FROM	ServiceProviderStaging
WHERE	Enrolment_Ref_No = @enrolment_ref_no

SELECT @record_id =  Address_Code
FROM	@tmp_SP
WHERE Enrolment_Ref_No = @enrolment_ref_no

if @record_id IS NOT NULL
BEGIN
	exec cpi_get_address_detail   @record_id 
								, @address_eng = @address_eng  OUTPUT 
    							, @address_chi = @address_chi    OUTPUT 
								, @district_code = @district_code    OUTPUT 
								, @eh_eng = @eh_eng	OUTPUT
								, @eh_chi = @eh_chi	OUTPUT

	UPDATE @tmp_sp
	SET	Building = @address_eng, 
		Building_Chi = @address_chi,
		District = @district_code
END
-- =============================================
-- Return results
-- =============================================

EXEC [proc_SymmetricKey_open]
	
	SELECT T.Enrolment_Ref_No, T.Enrolment_Dtm, T.SP_ID, convert(varchar, DecryptByKey(T.Encrypt_Field1)) as SP_HKID, 
		convert(varchar(40), DecryptByKey(T.Encrypt_Field2)) as SP_Eng_Name, convert(nvarchar, DecryptByKey(T.Encrypt_Field3)) as SP_Chi_Name, T.Room, T.[Floor],
			T.Block, T.Building, T.Building_Chi, T.District, T.Address_Code, T.Phone_Daytime,
			T.Fax, T.Email, T.Email_Changed, T.Record_Status, T.Remark, T.Submission_Method, T.Already_Joined_EHR, T.Join_PCD,
			T.Create_Dtm, T.Create_By, T.Update_Dtm, T.Update_By, S.TSMP
	FROM	@tmp_sp T, ServicePRoviderStaging S
	WHERE	T.Enrolment_Ref_No = S.Enrolment_Ref_No

EXEC [proc_SymmetricKey_close]

END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderStaging_get_byERN] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderStaging_get_byERN] TO HCVU
GO
