IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderSPAccMaintenance_get_bySPID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderSPAccMaintenance_get_bySPID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Marco CHOI
-- Modified date:	19 Jul 2018
-- CR No.			CRE17-016
-- Description:		Checking of PCD status during VSS enrolment
--					Get [PCD_Account_Status], [PCD_Enrolment_Status],[PCD_Professional],[PCD_Status_Last_Check_Dtm] columns
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
-- Create date: 10 July 2008
-- Description:	Retrieve the Service Provider Information
--				with SP Account Maintenance from Table "ServiceProvider"
--				and "SPAccountMaintenance"
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
-- Modified by:		Lawrence TSANG
-- Modified date:	1 Jun 2009
-- Description:		Change Record_Status to HCSPUserAC.Record_Status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	15 July 2009
-- Description:		Amended the checking for account locked
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	30 July 2009
-- Description:		Retrieve result without joining table 'SPAccountMaintenance'
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE12-001
-- Modified by:		Tony FUNG
-- Modified date:	07 Feb 2012
-- Description:		1. Grant permission to WSINT for PCDInterface
-- =============================================
-- =============================================
-- Modification History
-- CR No:			INT13-0028 - SP Amendment Report
-- Modified by:		Tommy LAM
-- Modified date:	20 Nov 2013
-- Description:		Add Column -	[ServiceProvider].[Tentative_Email_Input_By]
--									[ServiceProvider].[Data_Input_By]
--									[ServiceProvider].[Data_Input_Effective_Dtm]
-- =============================================
CREATE PROCEDURE [dbo].[proc_ServiceProviderSPAccMaintenance_get_bySPID]
	@sp_id	char(8)
AS
BEGIN
	OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
	DECLARE	@record_id		int,
			@address_eng	varchar(255),
			@address_chi	nvarchar(255),
			@district_code	char(5),
			@eh_eng			varchar(255),
			@eh_chi			varchar(255)

	DECLARE @TempSP TABLE	( 
							Enrolment_Ref_No		char(15),
							Enrolment_Dtm			datetime,
							SP_ID					char(8),
							Encrypt_Field1				varbinary(100),
							Encrypt_Field2				varbinary(100),
							Encrypt_Field3				varbinary(100),
							Room						nvarchar(5),
							[Floor]						nvarchar(3),
							Block						nvarchar(3),
							Building					varchar(255),
							Building_Chi				nvarchar(255) collate database_default,
							District					char(4),
							Address_Code				int,
							Phone_Daytime				varchar(20),
							Fax							varchar(20),
							Email						varchar(255),
							Tentative_Email				varchar(255),
							Record_Status				char(1),						
							Remark						nvarchar(255),
							Submission_Method			char(1),
							Already_Joined_EHR		char(1),
							UnderModification			char(1),
							Effective_Dtm				datetime,
							Token_Return_Dtm			datetime,
							Create_Dtm					datetime,
							Create_By					varchar(20),
							Update_Dtm					datetime,
							Update_By					varchar(20),
							Tentative_Email_Input_By	varchar(20),
							Data_Input_By				varchar(20),
							Data_Input_Effective_Dtm	datetime,
							PCD_Account_Status 			char(1),
							PCD_Enrolment_Status 		char(1),
							PCD_Professional 			varchar(20),
							PCD_Status_Last_Check_Dtm 	datetime
							)

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	INSERT INTO @TempSP	(
						Enrolment_Ref_No,
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
						Tentative_Email,
						Record_Status,
						Remark,
						Submission_Method,
						Already_Joined_EHR,
						UnderModification,
						Effective_Dtm,
						Token_Return_Dtm,
						Create_Dtm,
						Create_By,
						Update_Dtm,
						Update_By,
						Tentative_Email_Input_By,
						Data_Input_By,
						Data_Input_Effective_Dtm,
						PCD_Account_Status,
						PCD_Enrolment_Status,
						PCD_Professional,
						PCD_Status_Last_Check_Dtm
						)

	SELECT	Enrolment_Ref_No, Enrolment_Dtm, SP_ID, Encrypt_Field1, Encrypt_Field2, Encrypt_Field3, Room, [Floor],
			Block, Building, Building_Chi, District, Address_Code, Phone_Daytime,
			Fax, Email, Tentative_Email, Record_Status, Remark, Submission_Method, Already_Joined_EHR,
			UnderModification, Effective_Dtm, Token_Return_Dtm, Create_Dtm, Create_By, Update_Dtm, Update_By,
			Tentative_Email_Input_By, Data_Input_By, Data_Input_Effective_Dtm,
			PCD_Account_Status,PCD_Enrolment_Status,PCD_Professional, PCD_Status_Last_Check_Dtm
	FROM	ServiceProvider
	WHERE	SP_ID = @sp_id

	SELECT	@record_id = Address_Code
	FROM	@TempSP
	WHERE	SP_ID = @sp_id

	IF @record_id IS NOT NULL
	BEGIN
		EXEC cpi_get_address_detail	@record_id,
									@address_eng = @address_eng		OUTPUT,
    								@address_chi = @address_chi		OUTPUT,
									@district_code = @district_code	OUTPUT,
									@eh_eng = @eh_eng				OUTPUT,
									@eh_chi = @eh_chi				OUTPUT

		UPDATE	@TempSP
		SET		Building = @address_eng, 
				Building_Chi = @address_chi,
				District = @district_code
	END
-- =============================================
-- Return results
-- =============================================

	SELECT	T.Enrolment_Ref_No, 
			T.Enrolment_Dtm, 
			T.SP_ID, --T.SP_HKID, T.SP_Eng_Name, T.SP_Chi_Name, 
			CONVERT(varchar, DecryptByKey(T.Encrypt_Field1)) AS SP_HKID,
			CONVERT(varchar(40), DecryptByKey(T.Encrypt_Field2)) AS SP_Eng_Name,
			CONVERT(nvarchar, DecryptByKey(T.Encrypt_Field3)) AS SP_Chi_Name,
			T.Room, T.[Floor], T.Block, T.Building, T.Building_Chi, T.District, T.Address_Code, 
			T.Phone_Daytime, T.Fax, T.Email, T.Tentative_Email, 
			T.Submission_Method, T.UnderModification, T.Already_Joined_EHR,
			T.Effective_Dtm, T.Token_Return_Dtm, T.Create_Dtm, T.Create_By, T.Update_Dtm, T.Update_By, S.TSMP,
			T.Record_Status,
			case isnull(HCSPAC.IVRS_Locked,'')
				when 'Y' then 'S'
				else HCSPAC.Record_Status				
			end [Account_Status],
			CASE ISNULL(T.Tentative_Email, '')
				WHEN '' THEN 'N'
				ELSE 'Y'
			END	[Email_Change],
			T.Remark,
			T.Tentative_Email_Input_By,
			T.Data_Input_By,
			T.Data_Input_Effective_Dtm,
			T.PCD_Account_Status,
			T.PCD_Enrolment_Status,
			T.PCD_Professional,
			T.PCD_Status_Last_Check_Dtm

	FROM	@TempSP T
				INNER JOIN ServicePRovider S
					ON T.SP_ID = S.SP_ID
				INNER JOIN HCSPUserAC HCSPAC
					ON HCSPAC.SP_ID = S.SP_ID

CLOSE SYMMETRIC KEY sym_Key

END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderSPAccMaintenance_get_bySPID] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderSPAccMaintenance_get_bySPID] TO WSINT
GO
