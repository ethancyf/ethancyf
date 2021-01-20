IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProvider_get_byERN]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProvider_get_byERN]
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
-- Modification History
-- CR No:			INT13-0028 - SP Amendment Report
-- Modified by:		Tommy LAM
-- Modified date:	20 Nov 2013
-- Description:		Add Column -	[ServiceProvider].[Data_Input_By]
--									[ServiceProvider].[Data_Input_Effective_Dtm]
-- =============================================
-- =============================================
-- Modification History
-- CR No:			CRE13-003
-- Modified by:		Koala CHENG
-- Modified date:	23 Jul 2013
-- Description:		Fix retrieve address by Address_Code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Kathy LEE
-- Modified date: 7 May 2009
-- Description:	1. Remove "Delist Status", "Delist Dtm" and "Effective Dtm"
--              2. Add "Token Return Dtm"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Kathy LEE
-- Modified date: 2 Ocy 2009
-- Description:	Add order by in the final resutl
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 12 June 2008
-- Description:	Retrieve the Service Provider Information
--				from Table "ServiceProvider"
-- =============================================
CREATE PROCEDURE [dbo].[proc_ServiceProvider_get_byERN]
	@enrolment_ref_no	char(15)
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
DECLARE	@record_id int,
		@address_eng varchar(255),
		@address_chi nvarchar(255),
		@district_code char(5),
		@eh_eng varchar(255),
		@eh_chi varchar(255)
declare @sp_id char(8)

DECLARE @tmp_SP table ( Enrolment_Ref_No char(15),
						Enrolment_Dtm datetime,
						SP_ID char(8),
						--SP_HKID	char(9),
						--SP_Eng_Name	varchar(40),
						--SP_Chi_Name	nvarchar(6),
						Encrypt_Field1 varbinary(100),
						Encrypt_Field2 varbinary(100),
						Encrypt_Field3 varbinary(100),
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
						Record_Status char(1),
						Remark nvarchar(255),
						Submission_Method char(1),
						Already_Joined_EHR char(1),
						UnderModification char(1),	
						Token_Return_Dtm datetime,		
						Create_Dtm datetime,
						Create_By varchar(20),
						Update_Dtm datetime,
						Update_By varchar(20),
						Data_Input_By varchar(20),
						Data_Input_Effective_Dtm datetime,
						ConsentPrintOption char(1))

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
					Record_Status,
					Remark,
					Submission_Method,
					Already_Joined_EHR,
					UnderModification,
					Token_Return_Dtm,				
					Create_Dtm,
					Create_By,
					Update_Dtm,
					Update_By,
					Data_Input_By,
					Data_Input_Effective_Dtm,
					ConsentPrintOption
					)

SELECT ServiceProvider.Enrolment_Ref_No, ServiceProvider.Enrolment_Dtm, ServiceProvider.SP_ID, ServiceProvider.Encrypt_Field1, ServiceProvider.Encrypt_Field2, ServiceProvider.Encrypt_Field3, ServiceProvider.Room, ServiceProvider.[Floor],
		ServiceProvider.Block, ServiceProvider.Building, ServiceProvider.Building_Chi, ServiceProvider.District, ServiceProvider.Address_Code, ServiceProvider.Phone_Daytime,
		ServiceProvider.Fax, ServiceProvider.Email, ServiceProvider.Record_Status, ServiceProvider.Remark, ServiceProvider.Submission_Method, ServiceProvider.Already_Joined_EHR,
		ServiceProvider.UnderModification, ServiceProvider.Token_Return_Dtm, ServiceProvider.Create_Dtm, ServiceProvider.Create_By, 
		ServiceProvider.Update_Dtm, ServiceProvider.Update_By,
		ServiceProvider.Data_Input_By, ServiceProvider.Data_Input_Effective_Dtm,
		HCSPUserAC.ConsentPrintOption
FROM	ServiceProvider, HCSPUserAC
WHERE	Enrolment_Ref_No = @enrolment_ref_no and ServiceProvider.SP_ID = HCSPUserAC.SP_ID

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

	SELECT T.Enrolment_Ref_No, T.Enrolment_Dtm, T.SP_ID, -- T.SP_HKID, T.SP_Eng_Name, T.SP_Chi_Name, 
		convert(varchar, DecryptByKey(T.Encrypt_Field1)) as SP_HKID,
		convert(varchar(40), DecryptByKey(T.Encrypt_Field2)) as SP_Eng_Name,
		convert(nvarchar, DecryptByKey(T.Encrypt_Field3)) as SP_Chi_Name,
			T.Room, T.[Floor],
			T.Block, T.Building, T.Building_Chi, T.District, T.Address_Code, T.Phone_Daytime,
			T.Fax, T.Email, T.Record_Status, T.Remark, T.Submission_Method, T.UnderModification, T.Already_Joined_EHR,
			T.Token_Return_Dtm, T.Create_Dtm, T.Create_By, T.Update_Dtm, T.Update_By,
			T.Data_Input_By, T.Data_Input_Effective_Dtm, S.TSMP, T.ConsentPrintOption
	FROM	@tmp_sp T, ServicePRovider S
	WHERE	T.SP_ID = S.SP_ID
	order by S.effective_dtm desc

EXEC [proc_SymmetricKey_close]

END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProvider_get_byERN] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProvider_get_byERN] TO HCVU
GO
