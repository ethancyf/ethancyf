IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransaction_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransaction_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE19-006 (DHC)
-- Modified by:		Winnie SUEN
-- Modified date:	24 Jun 2019
-- Description:		Add Column - [VoucherTransaction].[DHC_Service]
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE18-0XX (IDEAS2)
-- Modified by:		Winnie SUEN
-- Modified date:	3 Jan 2019
-- Description:		Add Column - [VoucherTransaction].[SmartID_Ver]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	10 Sep 2018
-- CR No.:			CRE17-010 (OCSSS integration)
-- Description:		Add Column - [VoucherTransaction].[HKIC_Symbol]
--								 [VoucherTransaction].[OCSSS_Ref_Status]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Chris YIM
-- Modified date:	04 Jun 2018
-- CR No.:			CRE18-004 (CIMS Vaccination Sharing)
-- Description:	  	Add Column    - [VoucherTransaction].[DH_Vaccine_Ref]
--                                  [VoucherTransaction].[DH_Vaccine_Ref_Status]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	27 Feb 2018
-- CR No.:			CRE17-013
-- Description:		Extend bank account name to 300 chars
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Chris YIM
-- Modified date:	20 June 2017
-- CR No.:			CRE16-026-03 (Add PCV13)
-- Description:	  	Add Column - [VoucherTransaction].[High_Risk]
--								 [VoucherTransaction].[EHS_Vaccine_Ref]
--								 [VoucherTransaction].[HA_Vaccine_Ref]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	29 Aug 2016
-- CR No.			CRE16-002
-- Description:		Revamp VSS - add column Category_Code
-- =============================================
-- =============================================
-- Modification History
-- CR No.:		CRE11-024-02
-- Modified by:	Koala CHENG
-- Modified date:	30 Oct 2011
-- Description:	SP create claim can be incomplete, so confirm time can be null if status is Incomplete
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================  
-- Modified by:  Derek LEUNG  
-- Modified date: 20 September 2010  
-- Description:  Add Column IsUpload  
-- =============================================  
-- =============================================  
-- Modified by:  Koala Cheng  
-- Modified date: 26 Jul 2010  
-- Description:  1. Add Ext_Ref_Status for eVaccination  
-- =============================================  
-- =============================================  
-- Modified by:  Kathy LEE  
-- Modified date: 8 Jul 2010  
-- Description:  1. Grant Right to HCVU  
--     2. Add Manual_Reimburse  
-- =============================================  
-- =============================================  
-- Modified by:  Kathy LEE  
-- Modified date: 29 Dec 2009  
-- Description:  Add 'Create_By_SmartID'  
-- =============================================  
-- =============================================  
-- Modified by:  Pak Ho LEE  
-- Modified date: 10 Dec 2009  
-- Description:  Pre School  
-- =============================================  
-- =============================================  
-- Author:  Timothy LEUNG  
-- Create date: 03 May 2008  
-- Description: Insert a new Voucher Claim Transaction  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:  Timothy LEUNG  
-- Modified date: 01 December 2008  
-- Description:  Handle Claim_Amount  
--  
-- Modified by:  Timothy LEUNG  
-- Modified date: 30 December 2008  
-- Description:  Add field SourceApp to source the source application make the claim  
--  
-- Modified by:  Pak Ho LEE  
-- Modified date: 11 June 2009  
-- Description:  Add Available Voucher Checking  
-- =============================================  
-- =============================================  
-- Modified by:  Pak Ho LEE  
-- Modified date: 31 Aug 2009  
-- Description:  New Schema  
-- =============================================  
  
CREATE PROCEDURE [dbo].[proc_VoucherTransaction_add]  
	@Transaction_ID			CHAR(20),  
	@Voucher_Acc_ID			CHAR(15),  
	@Temp_Voucher_Acc_ID	CHAR(15),  
	@Scheme_Code			CHAR(10),  
	@Service_Receive_Dtm	DATETIME,  
	@Service_Type			CHAR(5),  
	@Voucher_Before_Claim	SMALLINT,  
	@Voucher_After_Claim	SMALLINT,  
	@SP_ID					CHAR(8),  
	@Practice_Display_Seq	SMALLINT,  
	@Bank_Acc_Display_Seq	SMALLINT,  
	@Bank_Account_No		VARCHAR(30),  
	@Bank_Acc_Holder		NVARCHAR(300),  
	@DataEntry_By			VARCHAR(20),  
	@Consent_Form_Printed	CHAR(1),  
	@Record_Status			CHAR(1),  
	@TSWProgram				CHAR(1),  
	@Create_By				VARCHAR(20),  
	@Claim_Amount			MONEY,  
	@SourceApp				VARCHAR(10),  
	@Doc_Code				CHAR(20),  
	@PreSchool				CHAR(1),  
	@Create_By_SmartID		CHAR(1),  
	@Manual_Reimburse		CHAR(1),  
	@HA_Vaccine_Ref_Status	VARCHAR(10),   
	@IsUpload				VARCHAR(1) = 'N',  
	@Category_Code			VARCHAR(10),
	@High_Risk				CHAR(1),
	@EHS_Vaccine_Ref		VARCHAR(2),
	@HA_Vaccine_Ref			VARCHAR(2),
	@DH_Vaccine_Ref			VARCHAR(2),
	@DH_Vaccine_Ref_Status	VARCHAR(10),
	@HKIC_Symbol			CHAR(1),
	@OCSSS_Ref_Status		CHAR(1),
	@SmartID_Ver			VARCHAR(5),
	@DHC_Service			CHAR(1)
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
  
	IF LTRIM(RTRIM(@DataEntry_By)) = ''  
	BEGIN  
		INSERT INTO [VoucherTransaction](  
			[Transaction_ID],  
			[Transaction_Dtm],  
			[Voucher_Acc_ID],  
			[Temp_Voucher_Acc_ID],  
			[Scheme_Code],  
			[Service_Receive_Dtm],  
			[Service_Type],  
			[Voucher_Before_Claim],  
			[Voucher_After_Claim],  
			[SP_ID],  
			[Practice_Display_Seq],  
			[Bank_Acc_Display_Seq],  
			[Bank_Account_No],  
			[Bank_Acc_Holder],  
			[DataEntry_By],  
			[Confirmed_Dtm],  
			[Consent_Form_Printed],  
			[Record_Status],  
			[Void_Transaction_ID],  
			[Void_Dtm],  
			[Void_Remark],  
			[Void_By],  
			[Void_By_DataEntry],  
			[TSWProgram],  
			[Create_Dtm],  
			[Create_By],  
			[Update_Dtm],  
			[Update_By],  
			[Void_By_HCVU],  
			[Claim_Amount],  
			[SourceApp],  
			[Doc_Code],  
			[Special_Acc_ID],  
			[Invalid_Acc_ID],  
			[PreSchool],  
			[Create_By_SmartID],  
			[Manual_Reimburse],  
			[Ext_Ref_Status],   
			[IsUpload],
			[Category_Code],
			[High_Risk],
			[EHS_Vaccine_Ref], 
			[HA_Vaccine_Ref],
			[DH_Vaccine_Ref],
			[DH_Vaccine_Ref_Status],
			[HKIC_Symbol],
			[OCSSS_Ref_Status],
			[SmartID_Ver],
			[DHC_Service]
			)
		VALUES(  
			@Transaction_ID,  
			GetDate(),		--@Transaction_Dtm,  
			@Voucher_Acc_ID,  
			@Temp_Voucher_Acc_ID,  
			@Scheme_Code,  
			@Service_Receive_Dtm,  
			@Service_Type,   
			@Voucher_Before_Claim,  
			@Voucher_After_Claim,  
			@SP_ID,  
			@Practice_Display_Seq,  
			@Bank_Acc_Display_Seq,  
			@Bank_Account_No,  
			@Bank_Acc_Holder,  
			@DataEntry_By,  
			CASE @Record_Status WHEN 'U' THEN NULL ELSE GetDate() END,  --@Confirmed_Dtm,  
			@Consent_Form_Printed,  
			@Record_Status,   
			NULL,			--@Void_Transaction_ID,  
			NULL,			--@Void_Dtm,  
			NULL,			--@Void_Remark,  
			NULL,			--@Void_By,  
			NULL,			--@Void_By_DataEntry,  
			@TSWProgram,  
			GetDate(),		--@Create_Dtm,  
			@Create_By,  
			GetDate(),		--@Update_Dtm,  
			@Create_By,		--@Update_By,  
			NULL,			--@Void_By_HCVU,  
			@Claim_Amount,	--@Claim_Amount,  
			@SourceApp,  
			@Doc_Code,  
			NULL,			--@Special_Acc_ID,  
			NULL,			--@Invalid_Acc_ID  
			@PreSchool,  
			@Create_By_SmartID,  
			@Manual_Reimburse,  
			@HA_Vaccine_Ref_Status,   
			@IsUpload,
			@Category_Code,
			@High_Risk,
			@EHS_Vaccine_Ref,
			@HA_Vaccine_Ref,
			@DH_Vaccine_Ref,
			@DH_Vaccine_Ref_Status,
			@HKIC_Symbol,
			@OCSSS_Ref_Status,
			@SmartID_Ver,
			@DHC_Service
			)
	END  
	ELSE  
	BEGIN  
		INSERT INTO [VoucherTransaction](  
			[Transaction_ID],  
			[Transaction_Dtm],  
			[Voucher_Acc_ID],  
			[Temp_Voucher_Acc_ID],  
			[Scheme_Code],  
			[Service_Receive_Dtm],  
			[Service_Type],  
			[Voucher_Before_Claim],  
			[Voucher_After_Claim],  
			[SP_ID],  
			[Practice_Display_Seq],  
			[Bank_Acc_Display_Seq],  
			[Bank_Account_No],  
			[Bank_Acc_Holder],  
			[DataEntry_By],  
			[Confirmed_Dtm],  
			[Consent_Form_Printed],  
			[Record_Status],  
			[Void_Transaction_ID],  
			[Void_Dtm],  
			[Void_Remark],  
			[Void_By],  
			[Void_By_DataEntry],  
			[TSWProgram],  
			[Create_Dtm],  
			[Create_By],  
			[Update_Dtm],  
			[Update_By],  
			[Void_By_HCVU],  
			[Claim_Amount],  
			[SourceApp],  
			[Doc_Code],  
			[Special_Acc_ID],  
			[Invalid_Acc_ID],  
			[PreSchool],  
			[Create_By_SmartID],  
			[Manual_Reimburse],  
			[Ext_Ref_Status],   
			[IsUpload],
			[Category_Code],
			[High_Risk],
			[EHS_Vaccine_Ref], 
			[HA_Vaccine_Ref],
			[DH_Vaccine_Ref],
			[DH_Vaccine_Ref_Status],
			[HKIC_Symbol],
			[OCSSS_Ref_Status],
			[SmartID_Ver],
			[DHC_Service]
			)
		VALUES(  
			@Transaction_ID,  
			GetDate(),		--@Transaction_Dtm,  
			@Voucher_Acc_ID,  
			@Temp_Voucher_Acc_ID,  
			@Scheme_Code,  
			@Service_Receive_Dtm,  
			@Service_Type,  
			@Voucher_Before_Claim,  
			@Voucher_After_Claim,  
			@SP_ID,  
			@Practice_Display_Seq,  
			@Bank_Acc_Display_Seq,  
			@Bank_Account_No,  
			@Bank_Acc_Holder,  
			@DataEntry_By,  
			NULL,			--@Confirmed_Dtm,  
			@Consent_Form_Printed,  
			@Record_Status,  
			NULL,			--@Void_Transaction_ID,  
			NULL,			--@Void_Dtm,  
			NULL,			--@Void_Remark,  
			NULL,			--@Void_By,  
			NULL,			--@Void_By_DataEntry,  
			@TSWProgram,  
			GetDate(),		--@Create_Dtm,  
			@Create_By,  
			GetDate(),		--@Update_Dtm,  
			@Create_By,		--@Update_By,  
			NULL,			--@Void_By_HCVU,  
			@Claim_Amount,	--@Claim_Amount,  
			@SourceApp,  
			@Doc_Code,  
			NULL,			--@Special_Acc_ID,  
			NULL,			--@Invalid_Acc_ID,  
			@PreSchool,  
			@Create_By_SmartID,  
			@Manual_Reimburse,  
			@HA_Vaccine_Ref_Status,   
			@IsUpload,
			@Category_Code,
			@High_Risk,
			@EHS_Vaccine_Ref,
			@HA_Vaccine_Ref,
			@DH_Vaccine_Ref,
			@DH_Vaccine_Ref_Status,
			@HKIC_Symbol,
			@OCSSS_Ref_Status,
			@SmartID_Ver,
			@DHC_Service
			)
	END   
END  
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransaction_add] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransaction_add] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransaction_add] TO WSEXT
GO

