IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_InspectionVisitInfoLatest_get_bySPID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_InspectionVisitInfoLatest_get_bySPID]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Modified by: 
-- Modified date:
-- Description:	
-- =============================================
-- =============================================
-- Author:		James
-- Create date: 10 Jul 2020
-- CR No.:		CRE19-022 (Inspection Module)
-- Description:	Retrieve the latest inspectionvisitinfo record
-- =============================================

CREATE PROCEDURE 	[dbo].[proc_InspectionVisitInfoLatest_get_bySPID]
	@SP_ID varchar(8),
	@Filter_Date Date,
	@Inspection_ID varchar(30) = ''
AS BEGIN

	OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key

	SELECT Top 1
		ins.Inspection_ID,
		ins.File_Reference_No,
		ins.Referred_Reference_No_1,
		ins.Referred_Reference_No_2,
		ins.Referred_Reference_No_3,
		ins.SP_ID,
		ins.Practice_Display_Seq,
		instype.Type_Of_Inspection,
		CONVERT(varchar(100), ins.Visit_Date, 106) As Visit_Date ,
		ins.Visit_Begin_Dtm,
		ins.Visit_End_Dtm,
		ins.Confirmation_with,
		ins.Confirmation_Dtm,
		ins.Form_Condition AS Form_Condition_ID,
		ins.Form_Condition_Remark,
		ins.Means_Of_Communication As Means_Of_Communication_ID,
		ins.Means_Of_Communication_Email,
		ins.Means_Of_Communication_Fax,
		ins.Remarks,
		ins.Case_Officer,
		ins.Subject_Officer,
		ins.Case_Officer_Contact_No,
		ins.Subject_Officer_Contact_No,
		ins.Record_Status,
		ins.No_Of_InOrder,
		ins.No_Of_MissingForm,
		ins.No_Of_Inconsistent,
		ins.No_Of_TotalCheck,
		ins.Is_OverMajor,
		ins.Other_Letter_Remark,
		ins.Other_Party_Remark,
		case when ins.Require_Followup is null then 'N/A' when ins.Require_Followup='Y' then 'Yes' else 'No' end as Follow_Up_Action,
		ins.Anomalous_Claims,
		ins.Checking_Date,
		ins.Create_Dtm,
		ins.Create_By,
		convert(varchar, DecryptByKey(a.Encrypt_Field1)) as SP_HKID, 
		convert(varchar(40), DecryptByKey(a.Encrypt_Field2)) as SP_Eng_Name, 
		convert(nvarchar, DecryptByKey(a.Encrypt_Field3)) as SP_Chi_Name,
		a.Phone_Daytime,
		a.Fax,
		a.Email,
		p.Practice_Name,
		p.Practice_Name_Chi,
		ins.Service_Category_Code,
		pro.Service_Category_Desc,
		ins.SP_Last_Visit_date,		
		instype.Type_Of_Inspection_desc,
		s2.Data_Value As Form_Condition_Value,
		s3.Data_Value As Means_Of_Communication_Value,
		sta.Status_Description,
		ROW_NUMBER() OVER (ORDER BY ins.Visit_Date DESC) as Row_Index 
	FROM 
		InspectionVisitInfo ins WITH(NOLOCK)
			left outer join ServiceProvider a WITH(NOLOCK) on ins.SP_ID = a.SP_ID
			left outer join Practice p WITH(NOLOCK) on p.SP_ID = a.SP_ID and ins.Practice_Display_Seq = p.Display_Seq
			left outer join Professional prof WITH(NOLOCK) on p.SP_ID = prof.SP_ID and p.Professional_Seq = prof.Professional_Seq
			left outer join Profession pro WITH(NOLOCK) on pro.Service_Category_Code = ins.Service_Category_Code
			left outer join StaticData s1 WITH(NOLOCK) on s1.Column_Name='TypeOfInspection' and s1.Item_No = ins.Main_Type_Of_Inspection
			left outer join StaticData s2 WITH(NOLOCK) on s2.Column_Name='FormCondition' and s2.Item_No = ins.Form_Condition
			left outer join StaticData s3 WITH(NOLOCK) on s3.Column_Name='MeansOfCommunication' and s3.Item_No = ins.Means_Of_Communication
			left outer join StatusData sta WITH(NOLOCK) on sta.Enum_Class='InspectionStatus' and sta.Status_Value = ins.Record_Status
			left outer join (
				SELECT Inspection_ID,
                      Type_Of_Inspection = stuff
                          ((SELECT     ',' + t.Type_Of_Inspection
                              FROM         InspectionTypeSelections AS t WITH(NOLOCK)
							  inner join StaticData sta WITH(NOLOCK) on sta.Column_Name='TypeOfInspection' and t.Type_Of_Inspection = sta.Item_No
                              WHERE     t.Inspection_ID = InspectionTypeSelections.Inspection_ID FOR xml path('')), 1, 1, ''),
						Type_Of_Inspection_desc = stuff
                          ((SELECT     ',' + sta.Data_Value
                              FROM         InspectionTypeSelections AS t WITH(NOLOCK)
							  inner join StaticData sta WITH(NOLOCK) on sta.Column_Name='TypeOfInspection' and t.Type_Of_Inspection = sta.Item_No
                              WHERE     t.Inspection_ID = InspectionTypeSelections.Inspection_ID FOR xml path('')), 1, 1, '')
FROM         InspectionTypeSelections WITH(NOLOCK)
GROUP BY Inspection_ID ) instype on instype.Inspection_ID = ins.Inspection_ID


			
	WHERE 

		 (LTRIM(RTRIM(@SP_ID)) <> '' AND ins.SP_ID = @SP_ID) And 
		 ((ins.Visit_Date is not null )And 
		 ((@Filter_Date is null And ins.Visit_Date<= GETDATE()) or (@Filter_Date is not null And ins.Visit_Date<= @Filter_Date))) And
		  (LTRIM(RTRIM(@Inspection_ID)) = '' or (LTRIM(RTRIM(@Inspection_ID)) <> '' And ins.Inspection_ID <> @Inspection_ID)) -- Exclude Current Record
		AND ins.Record_Status <> 'D' -- Exclude Removed Record

ORDER BY 
		ins.Visit_Date DESC,ins.Visit_End_Dtm Desc, ins.Create_Dtm DESC
	CLOSE SYMMETRIC KEY sym_Key

END
GO

GRANT EXECUTE ON [dbo].[proc_InspectionVisitInfoLatest_get_bySPID] TO HCVU
GO
