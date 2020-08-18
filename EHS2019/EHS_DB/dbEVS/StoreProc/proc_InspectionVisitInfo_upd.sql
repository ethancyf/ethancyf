IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_InspectionVisitInfo_upd]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_InspectionVisitInfo_upd]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================  
-- Modified by:   James CAI
-- Modified date: 10 Jul 2020  
-- Description:  New Schema  
-- =============================================  
-- =============================================  
-- Modified by:   Golden YANG
-- Modified date: 24 Apr 2020  
-- Description:  New Schema  
-- =============================================  
  
CREATE PROCEDURE [dbo].[proc_InspectionVisitInfo_upd]  
	@Inspection_ID  VARCHAR(30),
	@File_Reference_No  VARCHAR(30),	
	@Referred_Reference_No_1 varchar(30),
	@Referred_Reference_No_2 varchar(30),
	@Referred_Reference_No_3 varchar(30),
	@Type_Of_Inspection	VARCHAR(1000),
	@Visit_Date	datetime,
	@Visit_Begin_Dtm datetime,
	@Visit_End_Dtm datetime ,
	@Confirmation_with VARCHAR(100),
	@Confirmation_Dtm datetime,
	@Form_Condition VARCHAR(10),
	@Form_Condition_Remark NVARCHAR(255),
	@Means_Of_Communication VARCHAR(10),
	@Means_Of_Communication_Fax VARCHAR(30),
	@Means_Of_Communication_Email VARCHAR(255),
	@Low_Risk_Claim VARCHAR(1),
	@Remarks NVARCHAR(255),
	@Case_Officer VARCHAR(100),
	@Subject_Officer VARCHAR(100),	
	@Subject_Contact_No VARCHAR(20),
	@Case_Contact_No VARCHAR(20),
	@No_Of_InOrder	int,
	@No_Of_MissingForm	int,
	@No_Of_Inconsistent	int,
	@No_Of_TotalCheck	int,
	@Checking_Date datetime,
	@Anomalous_Claims VARCHAR(1),
	@No_Of_Anomalous_Claims	int,
	@Is_OverMajor	VARCHAR(1),
	@No_Of_Is_OverMajor	int,
	@Advisory_Letter_Date date,
	@Warning_Letter_Date date,
	@Delist_Letter_Date date,
	@Suspend_Payment_Letter_Date date,
	@Suspend_EHCP_Account_Letter_Date date,
	@Other_Letter_Date date,
	@Other_Letter_Remark	varchar(200),
	@BoardAndCouncil_Date date,
	@Police_Date date,
	@Social_Welfare_Department_Date date,
	@HK_Customs_And_Excise_Department_Date date,
	@Immigration_Department_Date date,
	@Labour_Department_Date date,
	@Other_Party_Date date,
	@Other_Party_Remark	varchar(200),
	@Suspend_EHCP_Date date,
	@Delist_EHCP_Date date,
	@Payment_RecoverySuspension_Date date,
	@Followup_Action varchar(8000),
	@Record_Status varchar(10),
	@Original_Status varchar(10),
	@Userid VARCHAR(20),
	@TSMP	BINARY(8),
	@SP_ID					CHAR(8),
	@Practice_Display_Seq	SMALLINT,
	@Service_Category_Code VARCHAR(5),
	@Last_Visit_Date		DATETIME,
	@UpdateType varchar(30),
	@Request_Reopen_Reason VARCHAR(100)
AS  
BEGIN  
	SET NOCOUNT ON;  
-- =============================================  
-- Declaration  
-- =============================================  
-- =============================================  
-- Validation   
-- =============================================  

	IF (SELECT TSMP FROM InspectionVisitInfo WITH(NOLOCK)
		Where  Inspection_ID = @Inspection_ID
		) != @TSMP
	BEGIN
		RAISERROR('00011', 16, 1)
		RETURN @@error
	END

-- =============================================  
-- Initialization  
-- =============================================  
-- =============================================  
-- Return results  
-- =============================================  
  

	BEGIN  
		if( @UpdateType='UpdateVisitDetail')
		begin
			UPDATE [InspectionVisitInfo]
			SET
				File_Reference_No = @File_Reference_No
				,Referred_Reference_No_1 = @Referred_Reference_No_1
				,Referred_Reference_No_2 = @Referred_Reference_No_2
				,Referred_Reference_No_3 = @Referred_Reference_No_3
				,[Visit_Date]=@Visit_Date
				,[Visit_Begin_Dtm] = @Visit_Begin_Dtm
				,[Visit_End_Dtm] = @Visit_End_Dtm
				,[Confirmation_with] = @Confirmation_with
				,[Confirmation_Dtm] = @Confirmation_Dtm
				,[Form_Condition] = @Form_Condition
				,[Form_Condition_Remark] = @Form_Condition_Remark
				,[Means_Of_Communication] = @Means_Of_Communication
				,[Means_Of_Communication_Fax] = @Means_Of_Communication_Fax
				,[Means_Of_Communication_Email] = @Means_Of_Communication_Email
				,[Low_Risk_Claim] = @Low_Risk_Claim
				,[Remarks] = @Remarks
				,[Case_Officer] = @Case_Officer
				,[Subject_Officer] = @Subject_Officer
				,[Case_Officer_Contact_No] = @Case_Contact_No
				,[Subject_Officer_Contact_No] = @Subject_Contact_No
				,[Record_Status] = @Record_Status
				,[Update_Dtm] = getdate()
				,[Update_by] = @Userid
				,[SP_ID]=isnull(@SP_ID,SP_ID)
				,[Practice_Display_Seq] = case when @Practice_Display_Seq=0 then Practice_Display_Seq else @Practice_Display_Seq end
				,[Service_Category_Code] = isnull(@Service_Category_Code,Service_Category_Code)
				,[SP_Last_Visit_Date] = @Last_Visit_Date
			WHERE 
				Inspection_ID = @Inspection_ID

			delete InspectionTypeSelections where Inspection_ID=@Inspection_ID
			DECLARE @x XML
			SELECT @x = @Type_Of_Inspection
			insert into InspectionTypeSelections
			SELECT 
			@Inspection_ID,
			c.value('*[1]','VARCHAR(10)') AS Type_Of_Inspection
			FROM @x.nodes('/DocumentElement/TypeOfInspection') T(C)
		end
		
		if( @UpdateType='UpdateInspectionResult')
		begin
			--Check if Checking_Date is null, if yes, it is the first time to input inspection result
			declare @Val_Freeze_date datetime
			set @Val_Freeze_date = null
			select @Val_Freeze_date = Freeze_Date from InspectionVisitInfo WITH(NOLOCK) where Inspection_ID=@Inspection_ID
			if (@Val_Freeze_date is not null)
			begin
				UPDATE [InspectionVisitInfo]
				SET
					No_Of_InOrder = @No_Of_InOrder,
					No_Of_MissingForm = @No_Of_MissingForm,
					No_Of_Inconsistent= @No_Of_Inconsistent,
					No_Of_TotalCheck= @No_Of_TotalCheck,
					Checking_Date = @Checking_Date,
					Anomalous_Claims = @Anomalous_Claims,
					No_Of_Anomalous_Claims = @No_Of_Anomalous_Claims,
					Is_OverMajor= @Is_OverMajor,
					No_Of_Is_OverMajor = @No_Of_Is_OverMajor,
					Record_Status = @Record_Status,
					Original_Status = @Original_Status,
					[Update_Dtm] = getdate(),
					[Update_by] = @Userid,
					Advisory_Letter_Date = @Advisory_Letter_Date,
					Warning_Letter_Date = @Warning_Letter_Date,
					Delist_Letter_Date = @Delist_Letter_Date,
					Suspend_Payment_Letter_Date = @Suspend_Payment_Letter_Date,
					Suspend_EHCP_Account_Letter_Date = @Suspend_EHCP_Account_Letter_Date,
					Other_Letter_Date = @Other_Letter_Date,
					Other_Letter_Remark= @Other_Letter_Remark,
					BoardAndCouncil_Date = @BoardAndCouncil_Date,
					Police_Date = @Police_Date,
					Social_Welfare_Department_Date = @Social_Welfare_Department_Date,
					HK_Customs_And_Excise_Department_Date = @HK_Customs_And_Excise_Department_Date,
					Immigration_Department_Date = @Immigration_Department_Date,
					Labour_Department_Date = @Labour_Department_Date,
					Other_Party_Date = @Other_Party_Date,
					Other_Party_Remark= @Other_Party_Remark,
					Suspend_EHCP_Date = @Suspend_EHCP_Date,
					Delist_EHCP_Date = @Delist_EHCP_Date,
					Payment_RecoverySuspension_Date = @Payment_RecoverySuspension_Date,
					Require_Followup = Case When @Followup_Action is null then 'N' else 'Y' end
				WHERE 
					Inspection_ID = @Inspection_ID
			end
			else
			begin
			    -- Get Last Visit Record
			    declare @SP_Last_Visit_Date Date,@SP_Last_File_Reference_No Varchar(30)
				Create Table #LatestRecord 
				(
					Inspection_ID				varchar(30),
					File_Reference_No			varchar(30),
					Referred_Reference_No_1		varchar(30),
					Referred_Reference_No_2		varchar(30),
					Referred_Reference_No_3		varchar(30),
					SP_ID						varchar(10),
					Practice_Display_Seq		smallint,
					Type_Of_Inspection			varchar(100),
					Visit_Date					varchar(100),
					Visit_Begin_Dtm				Datetime,
					Visit_End_Dtm				Datetime,
					Confirmation_with			varchar(100),
					Confirmation_Dtm			Datetime,
					Form_Condition_ID			varchar(10),
					Form_Condition_Remark		nvarchar(255),
					Means_Of_Communication_ID	varchar(10),
					Means_Of_Communication_Email	varchar(255),
					Means_Of_Communication_Fax	varchar(20),
					Remarks						nvarchar(255),
					Case_Officer				varchar(20),
					Subject_Officer				varchar(20),
					Case_Officer_Contact_No		varchar(20),
					Subject_Officer_Contact_No  varchar(20),
					Record_Status				varchar(3),
					No_Of_InOrder				int,
					No_Of_MissingForm				int,
					No_Of_Inconsistent			int,
					No_Of_TotalCheck			int,
					Is_OverMajor				varchar(1),
					Other_Letter_Remark			varchar(200),
					Other_Party_Remark			varchar(200),
					Follow_Up_Action			varchar(100),
					Anomalous_Claims			varchar(1),
					Checking_Date				Datetime,
					Create_Dtm					Datetime,
					Create_By					varchar(20),
					SP_HKID						varchar(100), 
					SP_Eng_Name					varchar(100), 
					SP_Chi_Name					varchar(100),
					Phone_Daytime				varchar(20),
					Fax							varchar(20),
					Email						varchar(255),
					Practice_Name				nvarchar(200),
					Practice_Name_Chi			nvarchar(200),
					Service_Category_Code		varchar(30),
					Service_Category_Desc		varchar(100),
					SP_Last_Visit_date			Datetime,		
					Type_Of_Inspection_desc		varchar(MAX),
					Form_Condition_Value		varchar(100),
					Means_Of_Communication_Value	varchar(100),
					Status_Description			varchar(100),
					Row_Index					bigint
				)

                set @SP_ID = (select top 1 SP_ID from InspectionVisitInfo WITH(NOLOCK) where Inspection_ID = @Inspection_ID)
				
                Insert into #LatestRecord
                EXEC proc_InspectionVisitInfoLatest_get_bySPID @SP_ID=@SP_ID,@Filter_Date =null,@Inspection_ID = @Inspection_ID

                select top 1 @SP_Last_Visit_Date = Visit_Date,@SP_Last_File_Reference_No=File_Reference_No from #LatestRecord WITH(NOLOCK)

				UPDATE ins 
				SET
					ins.No_Of_InOrder = @No_Of_InOrder,
					ins.No_Of_MissingForm = @No_Of_MissingForm,
					ins.No_Of_Inconsistent= @No_Of_Inconsistent,
					ins.No_Of_TotalCheck= @No_Of_TotalCheck,
					ins.Checking_Date = @Checking_Date,
					ins.Anomalous_Claims = @Anomalous_Claims,
					ins.No_Of_Anomalous_Claims = @No_Of_Anomalous_Claims,
					ins.Is_OverMajor= @Is_OverMajor,
					ins.No_Of_Is_OverMajor = @No_Of_Is_OverMajor,				
					ins.Record_Status = @Record_Status,
					ins.[Update_Dtm] = getdate(),
					ins.[Update_by] = @Userid,
					ins.Advisory_Letter_Date = @Advisory_Letter_Date,
					ins.Warning_Letter_Date = @Warning_Letter_Date,
					ins.Delist_Letter_Date = @Delist_Letter_Date,
					ins.Suspend_Payment_Letter_Date = @Suspend_Payment_Letter_Date,
					ins.Suspend_EHCP_Account_Letter_Date = @Suspend_EHCP_Account_Letter_Date,
					ins.Other_Letter_Date = @Other_Letter_Date,
					ins.Other_Letter_Remark= @Other_Letter_Remark,
					ins.BoardAndCouncil_Date = @BoardAndCouncil_Date,
					ins.Police_Date = @Police_Date,
					ins.Social_Welfare_Department_Date = @Social_Welfare_Department_Date,
					ins.HK_Customs_And_Excise_Department_Date = @HK_Customs_And_Excise_Department_Date,
					ins.Immigration_Department_Date = @Immigration_Department_Date,
					ins.Labour_Department_Date = @Labour_Department_Date,
					ins.Other_Party_Date = @Other_Party_Date,
					ins.Other_Party_Remark= @Other_Party_Remark,
					ins.Suspend_EHCP_Date = @Suspend_EHCP_Date,
					ins.Delist_EHCP_Date = @Delist_EHCP_Date,
					ins.Payment_RecoverySuspension_Date = @Payment_RecoverySuspension_Date,
					ins.Require_Followup = Case When @Followup_Action is null then 'N' else 'Y' end,
					ins.Practice_Name =p.Practice_Name,
					ins.Practice_Name_Chi =p.Practice_Name_Chi,
					ins.Practice_Address  =dbo.func_formatEngAddress(p.Room,p.Floor,p.Block,p.Building,p.District),
					ins.Practice_Address_Chi  = dbo.func_format_Address_Chi(p.Room,p.Floor,p.Block,p.Building_Chi,p.District),
					ins.Practice_Contact_No =p.Phone_Daytime ,
					ins.Practice_Reg_Code = prof.Registration_Code,
					ins.SP_Eng_Name =sp.Encrypt_Field2,
					ins.SP_Chi_Name =sp.Encrypt_Field3,
					ins.SP_Contact_No  =sp.Phone_Daytime,
					ins.SP_Fax  =sp.Fax,
					ins.SP_Email  =sp.Email,
					ins.SP_HCVS_Effective_Dtm  =psHCVS.Effective_Dtm,
					ins.SP_HCVSDHC_Effective_Dtm  =psHCVSDHC.Effective_Dtm,
					ins.SP_HCVSCHN_Effective_Dtm  =psHCVSCHN.Effective_Dtm,
					ins.SP_HCVS_Delist_Dtm  =psHCVS.Delist_Dtm,
					ins.SP_HCVSDHC_Delist_Dtm  =psHCVSDHC.Delist_Dtm,
					ins.SP_HCVSCHN_Delist_Dtm  =psHCVSCHN.Delist_Dtm,
					ins.SP_Last_Visit_Date = @SP_Last_Visit_Date,
					ins.SP_Last_Visit_File_Ref_No = @SP_Last_File_Reference_No,
					ins.Service_Category_Code = isnull(@Service_Category_Code,ins.Service_Category_Code),
					ins.Freeze_Date = getdate()
					From InspectionVisitInfo ins WITH(NOLOCK)
					inner join ServiceProvider sp WITH(NOLOCK) on sp.SP_ID = @SP_ID
					inner join Practice p WITH(NOLOCK) on p.SP_ID = @SP_ID and p.Display_Seq=@Practice_Display_Seq
					left outer join PracticeSchemeInfo psHCVS WITH(NOLOCK) on psHCVS.SP_ID = @SP_ID And psHCVS.Scheme_Code = 'HCVS' And psHCVS.Record_Status = 'A'
					left outer join PracticeSchemeInfo psHCVSDHC WITH(NOLOCK) on psHCVSDHC.SP_ID = @SP_ID And psHCVSDHC.Scheme_Code = 'HCVSDHC'And psHCVSDHC.Record_Status = 'A'
					left outer join PracticeSchemeInfo psHCVSCHN WITH(NOLOCK) on psHCVSCHN.SP_ID = @SP_ID And psHCVSCHN.Scheme_Code = 'HCVSCHN'And psHCVSCHN.Record_Status = 'A'
					left outer join Professional prof WITH(NOLOCK) on p.SP_ID = prof.SP_ID and p.Professional_Seq = prof.Professional_Seq
					left outer join Profession pf WITH(NOLOCK) on prof.Service_Category_Code=pf.Service_Category_Code
				WHERE 
					Inspection_ID = @Inspection_ID

				DROP TABLE #LatestRecord
			end

			delete InspectionFollowUpAction where Inspection_ID=@Inspection_ID
			DECLARE @xml XML
			SELECT @xml = @Followup_Action
			insert into InspectionFollowUpAction
			SELECT 
			@Inspection_ID,
			c.value('*[1]','VARCHAR(20)') AS Followup_Action_Seq,
			convert(datetime, c.value('*[2]','VARCHAR(20)'),105) AS Action_Date,
			c.value('*[3]','VARCHAR(200)') AS Action_Desc
			FROM @xml.nodes('/DocumentElement/FollowupAction') T(C)
						
		end
		
		if( @UpdateType='UpdateStatus')
		begin
			declare @ClosePendingApproval varchar(3) ='CC'
			declare @Closed varchar(3) ='C'
			declare @RemovePendingApproval varchar(3) ='CD'
			declare @Removed varchar(3) = 'D'
			declare @ReopenPendingApproval varchar(3) ='CO'
			declare @InspectionResultInputted varchar(3) ='RI'

			--Close Pending Approval
			if @Record_Status = @ClosePendingApproval 
			begin
			UPDATE [InspectionVisitInfo]
			SET				
			    Record_Status = @Record_Status
				,[Update_Dtm] = getdate()
				,[Update_by] = @Userid
				,Request_Close_Dtm =  getdate() 
				,Request_Close_By =  @Userid 
				WHERE Inspection_ID = @Inspection_ID
			end
			--Closed
			else if @Record_Status = @Closed 
			begin
			UPDATE [InspectionVisitInfo]
			SET				
				Record_Status = @Record_Status
				,[Update_Dtm] = getdate()
				,[Update_by] = @Userid
				,Approve_Close_By = @Userid
				,Approve_Close_Dtm = GETDATE()
			WHERE Inspection_ID = @Inspection_ID
			end
			--Remove Pending Approval (store original_status)
			else if @Record_Status = @RemovePendingApproval
			begin
			UPDATE [InspectionVisitInfo]
			SET				
				Original_Status = @Original_Status
				,Record_Status = @Record_Status
				,[Update_Dtm] = getdate()
				,[Update_by] = @Userid
				,Request_Remove_By = @Userid
				,Request_Remove_Dtm = GETDATE()
			WHERE Inspection_ID = @Inspection_ID
			end
			--Removed
			else if @Record_Status = @Removed
			begin
			UPDATE [InspectionVisitInfo]
			SET		
				Record_Status = @Record_Status
				,[Update_Dtm] = getdate()
				,[Update_by] = @Userid
				,Approve_Remove_By = @Userid
				,Approve_Remove_Dtm = GETDATE()
			WHERE Inspection_ID = @Inspection_ID
			end
			--Reopen Pending Approval
			else if @Record_Status = @ReopenPendingApproval
			begin
			UPDATE [InspectionVisitInfo]
			SET				
                Record_Status = @Record_Status
				,[Update_Dtm] = getdate()
				,[Update_by] = @Userid
				,Request_Reopen_By = @Userid
				,Request_Reopen_Dtm = GETDATE()
				,Request_Reopen_Reason = @Request_Reopen_Reason
			WHERE Inspection_ID = @Inspection_ID
			end
			--Inspection Result Inputted (Reopened)
			else if @Record_Status = @InspectionResultInputted AND @Original_Status = @ReopenPendingApproval
			begin
			UPDATE [InspectionVisitInfo]
			SET				
                Record_Status = @Record_Status
				,[Update_Dtm] = getdate()
				,[Update_by] = @Userid
				,Approve_Reopen_By = @Userid
				,Approve_Reopen_Dtm = GETDATE()
			WHERE Inspection_ID = @Inspection_ID
			end
			else
			--Others
			begin
			UPDATE [InspectionVisitInfo]
			SET				
				Record_Status = @Record_Status
				,Original_Status = @Original_Status
				,[Update_Dtm] = getdate()
				,[Update_by] = @Userid
				WHERE 
				Inspection_ID = @Inspection_ID
			end
		END
	END   
END    
GO

GRANT EXECUTE ON [dbo].[proc_InspectionVisitInfo_upd] TO HCVU
GO


