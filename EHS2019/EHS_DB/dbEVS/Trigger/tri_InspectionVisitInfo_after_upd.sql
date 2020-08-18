IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_InspectionVisitInfo_after_upd')
	DROP TRIGGER [dbo].[tri_InspectionVisitInfo_after_upd]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	24 July 2020
-- CR No.:			CRE19-022 (Inspection Module)
-- Description:		Trigger for InspectionVisitInfo
-- =============================================   

CREATE TRIGGER [dbo].[tri_InspectionVisitInfo_after_upd]
   ON		[dbo].[InspectionVisitInfo]
   AFTER	INSERT, UPDATE
AS BEGIN

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

    INSERT INTO InspectionVisitInfoLOG (
		System_Dtm,
		Inspection_ID,
		Original_Status,
		Record_Status,
		Main_Type_Of_Inspection,
		File_Reference_Type,
		File_Reference_No,
		Referred_Reference_No_1,
		Referred_Reference_No_2,
		Referred_Reference_No_3,
		Case_Officer,
		Case_Officer_Contact_No,
		Subject_Officer,
		Subject_Officer_Contact_No,
		SP_ID,
		SP_Eng_Name,
		SP_Chi_Name,
		SP_Contact_No,
		SP_Fax,
		SP_Email,
		SP_HCVS_Effective_Dtm,
		SP_HCVS_Delist_Dtm,
		SP_HCVSDHC_Effective_Dtm,
		SP_HCVSDHC_Delist_Dtm,
		SP_HCVSCHN_Effective_Dtm,
		SP_HCVSCHN_Delist_Dtm,
		SP_Last_Visit_Date,
		SP_Last_Visit_File_Ref_No,
		Practice_Display_Seq,
		Practice_Name,
		Practice_Name_Chi,
		Practice_Address,
		Practice_Address_Chi,
		Practice_Reg_Code,
		Practice_Contact_No,
		Visit_Date,
		Visit_Begin_Dtm,
		Visit_End_Dtm,
		Confirmation_with,
		Confirmation_Dtm,
		Form_Condition,
		Form_Condition_Remark,
		Means_Of_Communication,
		Means_Of_Communication_Fax,
		Means_Of_Communication_Email,
		Low_Risk_Claim,
		Remarks,
		No_Of_InOrder,
		No_Of_MissingForm,
		No_Of_Inconsistent,
		No_Of_TotalCheck,
		Anomalous_Claims,
		No_Of_Anomalous_Claims,
		Is_OverMajor,
		No_Of_Is_OverMajor,
		Advisory_Letter_Date,
		Warning_Letter_Date,
		Delist_Letter_Date,
		Suspend_Payment_Letter_Date,
		Suspend_EHCP_Account_Letter_Date,
		Other_Letter_Date,
		Other_Letter_Remark,
		BoardAndCouncil_Date,
		Police_Date,
		Social_Welfare_Department_Date,
		HK_Customs_And_Excise_Department_Date,
		Immigration_Department_Date,
		Labour_Department_Date,
		Other_Party_Date,
		Other_Party_Remark,
		Suspend_EHCP_Date,
		Delist_EHCP_Date,
		Payment_RecoverySuspension_Date,
		Require_Followup,
		Checking_Date,
		Freeze_Date,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By,
		Request_Remove_Dtm,
		Request_Remove_By,
		Request_Close_Dtm,
		Request_Close_By,
		Request_Reopen_Dtm,
		Request_Reopen_By,
		Request_Reopen_Reason,
		Approve_Remove_Dtm,
		Approve_Remove_By,
		Approve_Close_Dtm,
		Approve_Close_By,
		Approve_Reopen_Dtm,
		Approve_Reopen_By,
		Service_Category_Code
	)
	SELECT
		GETDATE(),
		Inspection_ID,
		Original_Status,
		Record_Status,
		Main_Type_Of_Inspection,
		File_Reference_Type,
		File_Reference_No,
		Referred_Reference_No_1,
		Referred_Reference_No_2,
		Referred_Reference_No_3,
		Case_Officer,
		Case_Officer_Contact_No,
		Subject_Officer,
		Subject_Officer_Contact_No,
		SP_ID,
		SP_Eng_Name,
		SP_Chi_Name,
		SP_Contact_No,
		SP_Fax,
		SP_Email,
		SP_HCVS_Effective_Dtm,
		SP_HCVS_Delist_Dtm,
		SP_HCVSDHC_Effective_Dtm,
		SP_HCVSDHC_Delist_Dtm,
		SP_HCVSCHN_Effective_Dtm,
		SP_HCVSCHN_Delist_Dtm,
		SP_Last_Visit_Date,
		SP_Last_Visit_File_Ref_No,
		Practice_Display_Seq,
		Practice_Name,
		Practice_Name_Chi,
		Practice_Address,
		Practice_Address_Chi,
		Practice_Reg_Code,
		Practice_Contact_No,
		Visit_Date,
		Visit_Begin_Dtm,
		Visit_End_Dtm,
		Confirmation_with,
		Confirmation_Dtm,
		Form_Condition,
		Form_Condition_Remark,
		Means_Of_Communication,
		Means_Of_Communication_Fax,
		Means_Of_Communication_Email,
		Low_Risk_Claim,
		Remarks,
		No_Of_InOrder,
		No_Of_MissingForm,
		No_Of_Inconsistent,
		No_Of_TotalCheck,
		Anomalous_Claims,
		No_Of_Anomalous_Claims,
		Is_OverMajor,
		No_Of_Is_OverMajor,
		Advisory_Letter_Date,
		Warning_Letter_Date,
		Delist_Letter_Date,
		Suspend_Payment_Letter_Date,
		Suspend_EHCP_Account_Letter_Date,
		Other_Letter_Date,
		Other_Letter_Remark,
		BoardAndCouncil_Date,
		Police_Date,
		Social_Welfare_Department_Date,
		HK_Customs_And_Excise_Department_Date,
		Immigration_Department_Date,
		Labour_Department_Date,
		Other_Party_Date,
		Other_Party_Remark,
		Suspend_EHCP_Date,
		Delist_EHCP_Date,
		Payment_RecoverySuspension_Date,
		Require_Followup,
		Checking_Date,
		Freeze_Date,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By,
		Request_Remove_Dtm,
		Request_Remove_By,
		Request_Close_Dtm,
		Request_Close_By,
		Request_Reopen_Dtm,
		Request_Reopen_By,
		Request_Reopen_Reason,
		Approve_Remove_Dtm,
		Approve_Remove_By,
		Approve_Close_Dtm,
		Approve_Close_By,
		Approve_Reopen_Dtm,
		Approve_Reopen_By,
		Service_Category_Code
	FROM
		inserted


END
GO
