IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_InspectionVisitInfo_get_byID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_InspectionVisitInfo_get_byID]
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
CREATE PROCEDURE 	[dbo].[proc_InspectionVisitInfo_get_byID] 
	@Inspection_ID	varchar(30)
AS BEGIN
 

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

	declare @Type_Of_Inspection_Desc varchar(1000)
	declare @Type_Of_Inspection varchar(1000)
	
	set @Type_Of_Inspection_Desc=''
	set @Type_Of_Inspection=''

	select 
		@Type_Of_Inspection=@Type_Of_Inspection+RTRIM(sta.Item_No)+', '
	from InspectionTypeSelections sel WITH(NOLOCK)
	inner join StaticData sta WITH(NOLOCK) on sta.Column_Name='TypeOfInspection' and sel.Type_Of_Inspection = sta.Item_No
	where 
		Inspection_ID=@Inspection_ID
	order by Display_Order

	 select @Type_Of_Inspection_Desc =
			(select RTRIM(Data_Value)+', ' from InspectionTypeSelections sel WITH(NOLOCK)
			inner join StaticData sta WITH(NOLOCK) on sta.Column_Name='TypeOfInspection' and sel.Type_Of_Inspection = sta.Item_No
			where sel.Inspection_ID = @Inspection_ID
			order by Display_Order
			for xml path(''))

	if Len(@Type_Of_Inspection_Desc) > 0 
	Begin
		set @Type_Of_Inspection_Desc = SUBSTRING(@Type_Of_Inspection_Desc,1,LEN(@Type_Of_Inspection_Desc)-1)
		set @Type_Of_Inspection = SUBSTRING(@Type_Of_Inspection,1,LEN(@Type_Of_Inspection)-1)
	End
	
	declare @Visit_Date Date,@File_Reference_No Varchar(30)

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

    declare @SP_ID varchar(10)
    set @SP_ID = (select top 1 SP_ID from InspectionVisitInfo With(NoLock) where Inspection_ID = @Inspection_ID)

    Insert into #LatestRecord
    EXEC proc_InspectionVisitInfoLatest_get_bySPID @SP_ID=@SP_ID,@Filter_Date =null,@Inspection_ID = @Inspection_ID

    OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key

	select top 1 @Visit_Date = Visit_Date,@File_Reference_No=File_Reference_No from #LatestRecord WITH(NOLOCK)

	SELECT 
	-- Inspection Record
		ins.Inspection_ID,
		ins.Create_Dtm,
		ins.Create_By,
		ins.Update_Dtm,
		ins.Update_By,
		ins.Request_Remove_Dtm,
		ins.Request_Remove_By,
		ins.Request_Close_Dtm,
		ins.Request_Close_By,
		ins.Request_Reopen_By,
		ins.Request_Reopen_Dtm,
		ins.Request_Reopen_Reason,
		ins.Approve_Remove_Dtm,
		ins.Approve_Remove_By,
		ins.Approve_Reopen_By,
		ins.Approve_Reopen_Dtm,
		ins.Approve_Close_Dtm,
		ins.Approve_Close_By,
		ins.Record_Status,
		ins.Original_Status,
		sta.Status_Description As Record_Status_Value,

		@Type_Of_Inspection as Type_Of_Inspection_ID,
		@Type_Of_Inspection_Desc as Type_Of_Inspection_Value,
		ins.Main_Type_Of_Inspection as Main_Type_Of_Inspection_ID,
		s1.Data_Value as Main_Type_Of_Inspection_Value,
		ins.File_Reference_No,
		ins.File_Reference_Type,
		ins.Referred_Reference_No_1,
		refer1.Inspection_ID As Referred_Inspection_ID_1,
		ins.Referred_Reference_No_2,
		refer2.Inspection_ID As Referred_Inspection_ID_2,
		ins.Referred_Reference_No_3,
		refer3.Inspection_ID As Referred_Inspection_ID_3,
		ins.Case_Officer as Case_Officer_ID,
		ins.Subject_Officer as Subject_Officer_ID,
		(select top 1 
		  ur.[User_ID] + ' - ' + 
		  case when Sex is null then 'Mr.' else 
		  case when Sex='M' then 'Mr.' 
		  when Sex='F' then 'Ms.' 
		  else 'Mr.' end end + ' '+ convert(varchar(40), DecryptByKey(Encrypt_Field2))  
		   FROM [UserRole] ur WITH(NOLOCK)
		 inner join HCVUUserAC usr WITH(NOLOCK) on ur.[User_ID] = usr.[User_ID]
		 where  ur.[User_ID] =ins.Case_Officer) as Case_Officer_Value,
		(select top 1 
		  ur.[User_ID] + ' - ' + 
		  case when Sex is null then 'Mr.' else 
		  case when Sex='M' then 'Mr.' 
		  when Sex='F' then 'Ms.' 
		  else 'Mr.' end end + ' '+ convert(varchar(40), DecryptByKey(Encrypt_Field2))  
		   FROM [UserRole] ur WITH(NOLOCK)
		 inner join HCVUUserAC usr WITH(NOLOCK) on ur.[User_ID] = usr.[User_ID]
		 where  ur.[User_ID] =ins.Subject_Officer) as Subject_Officer_Value,

		ins.Case_Officer_Contact_No,
		ins.Subject_Officer_Contact_No,

		-- Visit Target
		ins.SP_ID,
		case when ins.Freeze_Date is null and RTRIM(a.Record_Status) <> 'A' then ssp.Status_Description else '' end as SP_Status,
		case when ins.Freeze_Date is null then convert(varchar(40), DecryptByKey(a.Encrypt_Field2)) 
		else convert(varchar(40), DecryptByKey(ins.SP_Eng_Name))  end as SP_Eng_Name,
		case when ins.Freeze_Date is null then convert(nvarchar(40), DecryptByKey(a.Encrypt_Field3)) 
		else convert(nvarchar(40), DecryptByKey(ins.SP_Chi_Name))  end as SP_Chi_Name,
		case when ins.Freeze_Date is null then  a.Phone_Daytime else ins.SP_Contact_No end as SP_Contact_No,
		case when ins.Freeze_Date is null then a.Fax else ins.SP_Fax end as SP_Fax,
		case when ins.Freeze_Date is null then a.Email else ins.SP_Email end as SP_Email,
		case when ins.Freeze_Date is null then hcvs.Effective_Dtm else ins.SP_HCVS_Effective_Dtm end as SP_HCVS_Effective_Dtm,		
		case when ins.Freeze_Date is null then hcvsdhc.Effective_Dtm else ins.SP_HCVSDHC_Effective_Dtm end as SP_HCVSDHC_Effective_Dtm,
		case when ins.Freeze_Date is null then hcvschn.Effective_Dtm else ins.SP_HCVSCHN_Effective_Dtm end as SP_HCVSCHN_Effective_Dtm,
	    case when ins.Freeze_Date is null then hcvs.Delist_Dtm else ins.SP_HCVS_Delist_Dtm end as SP_HCVS_Delist_Dtm,		
		case when ins.Freeze_Date is null then hcvsdhc.Delist_Dtm else ins.SP_HCVSDHC_Delist_Dtm end as SP_HCVSDHC_Delist_Dtm,
		case when ins.Freeze_Date is null then hcvschn.Delist_Dtm else ins.SP_HCVSCHN_Delist_Dtm end as SP_HCVSCHN_Delist_Dtm,
		case when ins.Freeze_Date is null then @Visit_Date
		else ins.SP_Last_Visit_Date end as SP_Last_Visit_Date,
		case when ins.Freeze_Date is null then @File_Reference_No
		else ins.SP_Last_Visit_File_Ref_No end as SP_Last_Visit_File_Ref_No,
		
		ins.Practice_Display_Seq,
		case when ins.Freeze_Date is null then p.Practice_Name else ins.Practice_Name end as Practice_Name,
		case when ins.Freeze_Date is null then p.Practice_Name_Chi else ins.Practice_Name_Chi end as Practice_Name_Chi,
		case when ins.Freeze_Date is null and RTRIM(p.Record_Status) <> 'A' then sp.Status_Description else '' end as Practice_Status,
		case when ins.Freeze_Date is null then dbo.func_formatEngAddress(p.Room,p.Floor,p.Block,p.Building,p.District) else ins.Practice_Address end as Practice_Address,
		case when ins.Freeze_Date is null then dbo.func_format_Address_Chi( p.Room,p.Floor,p.Block, p.Building_Chi,p.District) else ins.Practice_Address_Chi end as Practice_Address_Chi,
	    ins.Service_Category_Code,
		pf.Service_Category_Desc,
		case when ins.Freeze_Date is null then prof.Registration_Code else ins.Practice_Reg_Code end As Practice_Reg_Code,
		pf.Service_Category_Desc_Chi,
	    case when ins.Freeze_Date is null then p.Phone_Daytime else	ins.Practice_Contact_No end as Practice_Contact_No,
		ins.Freeze_Date,

		-- Visit Detail
		ins.Visit_Date,
		ins.Visit_Begin_Dtm,
		ins.Visit_End_Dtm,
		ins.Confirmation_with,
		ins.Confirmation_Dtm,
		ins.Form_Condition As Form_Condition_ID,
		s2.Data_Value as Form_Condition_Value,
		ins.Form_Condition_Remark,
		ins.Means_Of_Communication As Means_Of_Communication_ID,	
		s3.Data_Value as Means_Of_Communication_Value,
		ins.Means_Of_Communication_Fax,
		ins.Means_Of_Communication_Email,
		ins.Low_Risk_Claim,
		ins.Remarks,

		--INspection Result
	    ins.No_Of_InOrder,
		ins.No_Of_MissingForm,
		ins.No_Of_Inconsistent,
		ins.No_Of_TotalCheck,
		ins.Anomalous_Claims,
		ins.No_Of_Anomalous_Claims,
		ins.Is_OverMajor,
		ins.No_Of_Is_OverMajor,
		ins.Checking_Date,

		--Action - Issue Letter
		ins.Advisory_Letter_Date,
		ins.Warning_Letter_Date,
		ins.Delist_Letter_Date,
		ins.Suspend_Payment_Letter_Date,
		ins.Suspend_EHCP_Account_Letter_Date,
		ins.Other_Letter_Date,
		ins.Other_Letter_Remark,

		--Action - Refer Parties
		ins.BoardAndCouncil_Date,
		ins.Police_Date,
		ins.Social_Welfare_Department_Date,
		ins.HK_Customs_And_Excise_Department_Date,
		ins.Immigration_Department_Date,
		ins.Labour_Department_Date,
		ins.Other_Party_Date,
		ins.Other_Party_Remark,

		--Action - Actions to EHCP
		ins.Suspend_EHCP_Date,
		ins.Delist_EHCP_Date,
		ins.Payment_RecoverySuspension_Date,
		ins.Require_Followup,

		(SELECT 
		Inspection_ID,
		Followup_Action_Seq,
        Action_Date, 
		Action_Desc
		FROM InspectionFollowUpAction WITH(NOLOCK) where Inspection_ID=@Inspection_ID order by Action_date FOR XML AUTO,  ROOT('root')) as Followup_Action,
		ins.TSMP
	FROM 
		InspectionVisitInfo ins WITH(NOLOCK)
			left outer join ServiceProvider a WITH(NOLOCK) on ins.SP_ID = a.SP_ID
			left outer join Practice p WITH(NOLOCK) on p.SP_ID = a.SP_ID and ins.Practice_Display_Seq = p.Display_Seq
			left outer join Professional prof WITH(NOLOCK) on p.SP_ID = prof.SP_ID and p.Professional_Seq = prof.Professional_Seq
			left outer join Profession pf WITH(NOLOCK) on ins.Service_Category_Code=pf.Service_Category_Code
			left outer join StaticData s1 WITH(NOLOCK) on s1.Column_Name='TypeOfInspection' and s1.Item_No = ins.Main_Type_Of_Inspection
			left outer join StaticData s2 WITH(NOLOCK) on s2.Column_Name='FormCondition' and s2.Item_No = ins.Form_Condition
			left outer join StaticData s3 WITH(NOLOCK) on s3.Column_Name='MeansOfCommunication' and s3.Item_No = ins.Means_Of_Communication
			left outer join StatusData sta WITH(NOLOCK) on sta.Enum_Class='InspectionStatus' and sta.Status_Value = ins.Record_Status
			left outer join StatusData sp WITH(NOLOCK) on sp.Enum_Class = 'PracticeStatus' and sp.Status_Value = p.Record_Status
			left outer join StatusData ssp WITH(NOLOCK) on ssp.Enum_Class = 'ServiceProviderStatus' and ssp.Status_Value = a.Record_Status
			left join district d WITH(NOLOCK) on p.District = d.district_code
			left join DistrictBoard b WITH(NOLOCK) on d.district_board = b.district_board
			left join District_Area da WITH(NOLOCK) ON b.area_code = da.area_code
			left join HCVUUserAC usr_c WITH(NOLOCK) on usr_c.User_ID=ins.Case_Officer
			left join HCVUUserAC usr_s WITH(NOLOCK) on usr_s.User_ID=ins.Subject_Officer			
			left join SchemeInformation hcvs WITH(NOLOCK) on ins.SP_ID = hcvs.SP_ID and hcvs.Scheme_Code='HCVS'
			left join SchemeInformation hcvsdhc WITH(NOLOCK) on ins.SP_ID = hcvsdhc.SP_ID and hcvsdhc.Scheme_Code='HCVDHC'
			left join SchemeInformation hcvschn WITH(NOLOCK) on ins.SP_ID = hcvschn.SP_ID and hcvschn.Scheme_Code='HCVSCHN'
			left join InspectionVisitInfo refer1 WITH(NOLOCK) on ins.Referred_Reference_No_1 = refer1.File_Reference_No
			left join InspectionVisitInfo refer2 WITH(NOLOCK) on ins.Referred_Reference_No_2 = refer2.File_Reference_No
			left join InspectionVisitInfo refer3 WITH(NOLOCK) on ins.Referred_Reference_No_3 = refer3.File_Reference_No
	WHERE 
		ins.Inspection_ID = @Inspection_ID

	CLOSE SYMMETRIC KEY sym_Key

	DROP TABLE #LatestRecord

END
GO

GRANT EXECUTE ON [dbo].[proc_InspectionVisitInfo_get_byID] TO HCVU
GO

