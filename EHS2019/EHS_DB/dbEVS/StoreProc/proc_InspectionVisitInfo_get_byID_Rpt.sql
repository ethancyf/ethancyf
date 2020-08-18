IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_InspectionVisitInfo_get_byID_Rpt]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_InspectionVisitInfo_get_byID_Rpt]
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
   
CREATE PROCEDURE 	[dbo].[proc_InspectionVisitInfo_get_byID_Rpt] 
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
	--declare @Other_Type_Of_Inspection_Desc varchar(4000)
	--declare @Other_Type_Of_Inspection varchar(1000)

	--set @Other_Type_Of_Inspection_Desc=''
	--set @Other_Type_Of_Inspection=''

	--select @Other_Type_Of_Inspection=@Other_Type_Of_Inspection+RTRIM(sta.Item_No)+', '
	--from InspectionTypeSelections sel WITH(NOLOCK)
	--inner join StaticData sta WITH(NOLOCK) on sta.Column_Name='TypeOfInspection' and sel.Type_Of_Inspection = sta.Item_No
	--where Inspection_ID=@Inspection_ID
	--order by Display_Order

	-- select @Other_Type_Of_Inspection_Desc =
	--(select RTRIM(Data_Value)+', ' from InspectionTypeSelections sel  WITH(NOLOCK)
	--inner join StaticData sta WITH(NOLOCK) on sta.Column_Name='TypeOfInspection' and sel.Type_Of_Inspection = sta.Item_No
	--where sel.Inspection_ID = @Inspection_ID
	--order by Display_Order
	--for xml path(''))

	--set @Other_Type_Of_Inspection_Desc = SUBSTRING(@Other_Type_Of_Inspection_Desc,1,LEN(@Other_Type_Of_Inspection_Desc)-1)
	--set @Other_Type_Of_Inspection = SUBSTRING(@Other_Type_Of_Inspection,1,LEN(@Other_Type_Of_Inspection)-1)

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
    set @SP_ID = (select top 1 SP_ID from InspectionVisitInfo WITH(NOLOCK) where Inspection_ID = @Inspection_ID)

    Insert into #LatestRecord
    EXEC proc_InspectionVisitInfoLatest_get_bySPID @SP_ID=@SP_ID,@Filter_Date =null,@Inspection_ID =@Inspection_ID

    OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key

	select top 1 @Visit_Date = Visit_Date,@File_Reference_No=File_Reference_No from #LatestRecord WITH(NOLOCK)

	SELECT 
		ins.Inspection_ID,

		-- Internal Reference Report
		CONVERT(varchar(100), ins.Visit_Date, 106) as Visit_Date_Print,
		datename(weekday, ins.Visit_Date) as Visit_Date_WeekDay,
		case when datename(weekday, ins.Visit_Date)='Monday' then N'星期一' when datename(weekday, ins.Visit_Date)='Tuesday' then N'星期二'
		when datename(weekday, ins.Visit_Date)='Wednesday' then N'星期三' when datename(weekday, ins.Visit_Date)='Thursday' then N'星期四'
		when datename(weekday, ins.Visit_Date)='Friday' then N'星期五' when datename(weekday, ins.Visit_Date)='Saturday' then N'星期六'
		when datename(weekday, ins.Visit_Date)='Sunday' then N'星期日' else N'' end 
		as Visit_Date_ChiWeekDay,
		ins.File_Reference_No,
		ins.SP_ID,
		ins.Service_Category_Code,
		case when ins.Freeze_Date is null then convert(varchar(40), DecryptByKey(a.Encrypt_Field2)) else convert(varchar(40), DecryptByKey(ins.SP_Eng_Name))  end 
		as SP_Eng_Name,
		case when ins.Freeze_Date is null then convert(nvarchar(40), DecryptByKey(a.Encrypt_Field3)) else convert(nvarchar(40), DecryptByKey(ins.SP_Chi_Name))  end 
		as SP_Chi_Name,
		case when ins.Freeze_Date is null then p.Practice_Name else ins.Practice_Name end 
		as Practice_Name,
		case when ins.Freeze_Date is null then p.Practice_Name_Chi else ins.Practice_Name_Chi end 
		as Practice_Name_Chi,
		case when ins.Freeze_Date is null then dbo.func_formatEngAddress(p.Room,p.Floor,p.Block,p.Building,p.District) else ins.Practice_Address end 
		as Practice_Address,
		case when ins.Freeze_Date is null then dbo.func_format_Address_Chi( p.Room,p.Floor,p.Block, p.Building_Chi,p.District) else ins.Practice_Address_Chi end 
		as Practice_Address_Chi,
		d.district_name,
		b.district_board,
		d.district_chi,
		b.district_board_chi,
		da.area_abbreviation as area_name,
		case when ins.Freeze_Date is null then p.Phone_Daytime else	ins.Practice_Contact_No end 
		as Practice_Contact_No,
		ISNULL(ins.Means_Of_Communication_Fax, '') AS [Means_Of_Communication_Fax],
		IIF(ISNULL(ins.Means_Of_Communication_Fax, '') = '', 'N/A', ins.Means_Of_Communication_Fax) 
		AS [Means_Of_Communication_Fax_Print],
		IIF(ISNULL(ins.Means_Of_Communication_Email, '') = '', 'N/A', ins.Means_Of_Communication_Email) 
		AS [Means_Of_Communication_Email_Print],
		case when ins.Freeze_Date is null then ISNULL(CONVERT(varchar(100), @Visit_Date, 106),'N/A')
		else ISNULL(CONVERT(varchar(100), ins.SP_Last_Visit_Date, 106),'N/A')  end 
		as Last_Visit_Date_Print,
		case when ins.Freeze_Date is null then ISNULL(@File_Reference_No,'N/A') 
		else ISNULL(ins.SP_Last_Visit_File_Ref_No,'N/A') end 
		as Last_Visit_File_Ref_No_Print,
		ins.Practice_Display_Seq,
		ins.Confirmation_with,
		CONVERT(varchar(100), ins.Confirmation_Dtm, 106) as Confirmation_Dtm_Print,
		left(CONVERT(varchar(100), ins.Visit_Begin_Dtm, 108),5) as Visit_Begin_Dtm_Print,		
		left(CONVERT(varchar(100), ins.Visit_End_Dtm, 108),5) as Visit_End_Dtm_Print,
		CASE WHEN ISNULL(ins.Form_Condition_Remark,'') = '' THEN s2.Data_Value ELSE s2.Data_Value + ' (' + ins.Form_Condition_Remark + ')' END
		as Form_Condition_Print,
		ins.Remarks,
		case when usr_c.Sex='F' then 'Ms. ' ELSE 'Mr. ' END + convert(varchar(40), DecryptByKey(usr_c.Encrypt_Field2)) 
		as Case_Officer_Name,
		--convert(nvarchar(40), DecryptByKey(usr_c.Encrypt_Field3)) as Case_Officer_ChiName,
		ISNULL(CONVERT(varchar(100), (select top 1 Effective_Dtm from SchemeInformation WITH(NOLOCK) where ins.SP_ID = SP_ID and (Scheme_Code='HCVS' or Scheme_Code='HCVSCHN' or Scheme_Code='HCVSDHC') order by Effective_Dtm), 106),'N/A') 
		as Joining_Date,
		ISNULL(CONVERT(varchar(100), (select top 1 Delist_Dtm from SchemeInformation WITH(NOLOCK) where ins.SP_ID = SP_ID and (Scheme_Code='HCVS' or Scheme_Code='HCVSCHN' or Scheme_Code='HCVSDHC') order by Effective_Dtm), 106),'N/A') 
		as Delisting_Date,

		-- Summary Of Inspection Visit Report
		ins.No_Of_TotalCheck,
		CONVERT(varchar(100), ins.Checking_Date, 106) as Checking_Date_Print,
		ins.No_Of_InOrder,
		ins.No_Of_MissingForm,
		ins.No_Of_Inconsistent,

		-- Confirmation Letter Report
		case when ins.Freeze_Date is null then a.Fax else ins.SP_Fax end as SP_Fax,
		ins.Subject_Officer_Contact_No,
		case when usr_s.Sex='F' then 'Ms. ' ELSE 'Mr. ' END + convert(varchar(40), DecryptByKey(usr_s.Encrypt_Field2)) as Subject_Officer_Name,
		convert(nvarchar(40), DecryptByKey(usr_s.Encrypt_Field3)) + case when usr_s.Sex='F' then N'小姐' when usr_s.Sex='M' then N'先生' else '' end		
		as Subject_Officer_ChiName,
		cast(year(ins.Visit_Date) as varchar) + N'年' +cast(month(ins.Visit_Date) as varchar) + N'月' +cast(day(ins.Visit_Date) as varchar) + N'日' 
		as Visit_Date_ChiPrint,
		cast(year(ins.Confirmation_Dtm) as varchar) + N'年' +cast(month(ins.Confirmation_Dtm) as varchar) + N'月' +cast(day(ins.Confirmation_Dtm) as varchar) + N'日' 
		as Confirmation_Dtm_ChiPrint,
		convert(nvarchar(40), DecryptByKey(usr_c.Encrypt_Field3)) AS Case_Officer_ChiName_Value,
		convert(nvarchar(40), DecryptByKey(usr_c.Encrypt_Field3)) + case when usr_c.Sex='F' then N'小姐' when usr_c.Sex='M' then N'先生' else '' end
		AS Case_Officer_ChiName,
		ins.Case_Officer_Contact_No,
		ins.Record_Status,
		ins.Original_Status,
		sta.Status_Description As Record_Status_Value,
		CONVERT(varchar(100), getdate(), 106) as Current_Date_Print,
		cast(year(getdate()) as varchar) + N'年' +cast(month(getdate()) as varchar) + N'月' +cast(day(getdate()) as varchar) + N'日' AS Current_Date_Chi_Print,
		d.district_chi,
		b.district_board_chi,
		case when ins.Freeze_Date is null then  a.Phone_Daytime else ins.SP_Contact_No end as SP_Contact_No

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
			left join district d WITH(NOLOCK) on p.District = d.district_code
			left join DistrictBoard b WITH(NOLOCK) on d.district_board = b.district_board
			left join District_Area da WITH(NOLOCK) ON b.area_code = da.area_code
			left join HCVUUserAC usr_c WITH(NOLOCK) on usr_c.User_ID=ins.Case_Officer
			left join HCVUUserAC usr_s WITH(NOLOCK) on usr_s.User_ID=ins.Subject_Officer			
			WHERE 
		ins.Inspection_ID = @Inspection_ID

	CLOSE SYMMETRIC KEY sym_Key

	Drop Table #LatestRecord
END
GO


GRANT EXECUTE ON [dbo].[proc_InspectionVisitInfo_get_byID_Rpt] TO HCVU
GO
