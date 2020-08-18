IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_InspectionVisitInfo_get_byAny]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_InspectionVisitInfo_get_byAny]
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
   
CREATE PROCEDURE 	[dbo].[proc_InspectionVisitInfo_get_byAny]
	@Inspection_ID	varchar(30),
	@File_Reference_No	varchar(30),
	@Referred_Reference_No	varchar(30),
	@Subject_Officer_ID varchar(20),
	@SP_ID varchar(8),
	@Visit_Begin_Dtm datetime,
	@Visit_End_Dtm datetime,
	@Main_Type_Of_Inspection varchar(10),
	@Record_Status varchar(10),
	@Practice_Display_Seq smallint,
	@User_ID varchar(20),
	@OnlyOwner int,
	@result_limit_1st_enable	BIT = False, 
	@result_limit_override_enable BIT = False,
	@override_result_limit		BIT = False
AS BEGIN
-- =============================================  
-- Declaration  
-- =============================================  

CREATE TABLE #TempInspectionRecord(  
	Inspection_ID   varchar(30),  
	File_Reference_No varchar(30),
	SP_ID char(8),
    Practice_Display_Seq smallint,
	SP_Eng_Name varchar(40)null,
	SP_Chi_Name nvarchar(40)null,
	Practice_Name nvarchar(100) null,
	Practice_Name_Chi nvarchar(100) null,
	Main_Type_Of_Inspection_Value varchar(2000) null,
	Type_Of_Inspection_Value varchar(2000)null,
	Visit_Date_Format varchar(20)null,
	Visit_Date datetime NULL,
    Case_Officer_Value varchar(100)null,
	Subject_Officer_Value varchar(100)null,
	Subject_Officer_ID varchar(20)null,
	Follow_Up_Action varchar(3)null,
	Status_Description varchar(40)null,
	Record_Status  varchar(3)null
	)  

CREATE CLUSTERED INDEX IDX_C_#TempInspectionRecord ON #TempInspectionRecord(Inspection_ID)

declare @Query_File_Reference_No varchar(30)
set @Query_File_Reference_No = replace(@File_Reference_No,'-','')
set @Query_File_Reference_No = replace(@Query_File_Reference_No,'(','')
set @Query_File_Reference_No = replace(@Query_File_Reference_No,')','')
set @Query_File_Reference_No = replace(@Query_File_Reference_No,'/','')
	

-- =============================================  
-- Validation   
-- =============================================  
-- =============================================  
-- Initialization  
-- =============================================  
DECLARE @rowcount int  
DECLARE @row_cnt_error varchar(max)
DECLARE @errCode_lower char(5) 
DECLARE @errCode_upper char(5) 
SET @errCode_lower = '00009'
SET @errCode_upper = '00017'
   
-- =============================================  
-- Retrieve data  
-- =============================================   

OPEN SYMMETRIC KEY sym_Key 
DECRYPTION BY ASYMMETRIC KEY asym_Key

-- ---------------------------------------------  
-- Query Inspection Record
-- ---------------------------------------------  
	INSERT INTO #TempInspectionRecord 
	SELECT  TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable))
		ins.Inspection_ID,
		ins.File_Reference_No,
		ins.SP_ID,
		ins.Practice_Display_Seq,
		convert(varchar(40), DecryptByKey(a.Encrypt_Field2)) 
		as SP_Eng_Name, 
		convert(nvarchar(40), DecryptByKey(a.Encrypt_Field3)) 
		as SP_Chi_Name,
		p.Practice_Name,
		p.Practice_Name_Chi,
		s1.Data_Value as Main_Type_Of_Inspection_Value,
		instype.Type_Of_Inspection_desc as Type_Of_Inspection_Value,
		case when ins.Visit_Date is null then 'N/A' else
		CONVERT(varchar(100), ins.Visit_Date, 106) end 
		as Visit_Date_Format ,
		ins.Visit_Date,
	    case when caseAc.Encrypt_Field2 is null then ''
		else convert(varchar(40), DecryptByKey( caseAc.Encrypt_Field2))  end 
		as Case_Officer_Value,
		case when subjectAc.Encrypt_Field2 is null then ''
		else convert(varchar(40), DecryptByKey( subjectAc.Encrypt_Field2))  end 
		as Subject_Officer_Value,
		ins.Subject_Officer As Subject_Officer_ID,
		case when ins.Require_Followup is null then 'N/A' when ins.Require_Followup='Y' then 'Yes' else 'No' end 
		as Follow_Up_Action,
		sta.Status_Description,
		ins.Record_Status
	FROM 
		InspectionVisitInfo ins  WITH(NOLOCK)
			left outer join ServiceProvider a WITH(NOLOCK) on ins.SP_ID = a.SP_ID
			left outer join Practice p WITH(NOLOCK) on p.SP_ID = a.SP_ID and ins.Practice_Display_Seq = p.Display_Seq
			left outer join StatusData sta WITH(NOLOCK) on sta.Enum_Class='InspectionStatus' and sta.Status_Value = ins.Record_Status
			left outer join HCVUUserAC subjectAc WITH(NOLOCK) on subjectAc.User_ID = ins.Subject_Officer
			left outer join HCVUUserAC caseAc WITH(NOLOCK) on caseAc.User_ID = ins.Case_Officer
			left outer join StaticData s1 WITH(NOLOCK) on s1.Column_Name = 'TypeOfInspection' and ins.Main_Type_Of_Inspection = s1.Item_No
			left outer join (
				SELECT Inspection_ID,
                      Type_Of_Inspection = stuff
                          ((SELECT     ',' + t.Type_Of_Inspection
                              FROM         InspectionTypeSelections AS t WITH(NOLOCK)
							  inner join StaticData sta WITH(NOLOCK) on sta.Column_Name='TypeOfInspection' and t.Type_Of_Inspection = sta.Item_No
                              WHERE     t.Inspection_ID = InspectionTypeSelections.Inspection_ID 
							  order by sta.Display_Order
							  FOR xml path('')), 1, 1, ''),
						Type_Of_Inspection_desc = stuff
                          ((SELECT     ',' + sta.Data_Value
                              FROM         InspectionTypeSelections AS t WITH(NOLOCK)
							  inner join StaticData sta WITH(NOLOCK) on sta.Column_Name='TypeOfInspection' and t.Type_Of_Inspection = sta.Item_No
                              WHERE     t.Inspection_ID = InspectionTypeSelections.Inspection_ID 
							   order by sta.Display_Order 
							   FOR xml path('')), 1, 1, '')
FROM         InspectionTypeSelections WITH(NOLOCK)
GROUP BY Inspection_ID ) instype on instype.Inspection_ID = ins.Inspection_ID
			
	WHERE 
		((LTRIM(RTRIM(@Inspection_ID)) <> '' AND ins.Inspection_ID = @Inspection_ID) OR (LTRIM(RTRIM(@Inspection_ID)) = ''))
		 AND ((LTRIM(RTRIM(@File_Reference_No)) <> '' AND replace(replace(replace(replace(ins.File_Reference_No,'-',''),'/',''),'(',''),')','') like '%'+@Query_File_Reference_No+'%') OR (LTRIM(RTRIM(@File_Reference_No)) = ''))
		 AND ((LTRIM(RTRIM(@Referred_Reference_No)) <> '' AND (ins.Referred_Reference_No_1 = @Referred_Reference_No Or ins.Referred_Reference_No_2 = @Referred_Reference_No Or ins.Referred_Reference_No_3 = @Referred_Reference_No)) OR (LTRIM(RTRIM(@Referred_Reference_No)) = ''))
		 AND ((LTRIM(RTRIM(@SP_ID)) <> '' AND ins.SP_ID = @SP_ID) OR (LTRIM(RTRIM(@SP_ID)) = ''))
		 AND ((LTRIM(RTRIM(@Subject_Officer_ID)) <> '' AND ins.Subject_Officer = @Subject_Officer_ID) OR (LTRIM(RTRIM(@Subject_Officer_ID)) = ''))
		 AND ((LTRIM(RTRIM(@Main_Type_Of_Inspection)) <> '' AND ins.Main_Type_Of_Inspection = @Main_Type_Of_Inspection) OR (LTRIM(RTRIM(@Main_Type_Of_Inspection)) = ''))		  
		 AND ((LTRIM(RTRIM(@Record_Status)) <> '' AND ins.Record_Status = @Record_Status) OR (LTRIM(RTRIM(@Record_Status)) = ''))
		 AND (@Visit_Begin_Dtm IS NULL OR ins.Visit_Date >= @Visit_Begin_Dtm)
		 AND (@Visit_End_Dtm IS NULL OR ins.Visit_Date < DATEADD(D, 1, @Visit_End_Dtm))
		 AND ((@Practice_Display_Seq>0 AND ins.Practice_Display_Seq = @Practice_Display_Seq) OR (@Practice_Display_Seq=0 and ins.Practice_Display_Seq>=0))
		 AND (@OnlyOwner=0 or (@OnlyOwner=1 and (ins.Case_Officer=@User_ID or ins.Subject_Officer=@User_ID)))
		
ORDER BY 
		ins.Visit_Date DESC

-- =============================================    
-- Max Row Checking  
-- =============================================  
BEGIN TRY       
	SELECT @rowcount = COUNT(1) FROM #TempInspectionRecord WITH(NOLOCK)

	EXEC proc_CheckFeatureResultRowLimit @row_count=@rowcount, @result_limit_1st_enable=@result_limit_1st_enable, @result_limit_override_enable=@result_limit_override_enable,@override_result_limit=@override_result_limit     
END TRY

BEGIN CATCH    	    
	SET @row_cnt_error = ERROR_MESSAGE()    
	
	IF (isnull(@row_cnt_error,'') <> '' AND @row_cnt_error <> @errCode_lower) or
		(@result_limit_override_enable = 0 AND @row_cnt_error = @errCode_lower )
		BEGIN
			--throw error if upper limit reached (error = @errCode_upper)
			--if upper limit is not enabled, throw error if lower limit is reached
			--if the error is not related to upper / lower limit, there must be sth wrong in the try block, throw the error immediately
			--DROP TABLE #TempTransaction
			RAISERROR (@row_cnt_error,16,1)    
			CLOSE SYMMETRIC KEY sym_Key  
			RETURN
		END
END CATCH  		

-- =============================================    
-- Throw out error if lower limit is reached 
-- =============================================  
IF isnull(@row_cnt_error,'') = @errCode_lower 
		BEGIN		
			--DROP TABLE #TempTransaction
			RAISERROR (@row_cnt_error,16,1)    
			CLOSE SYMMETRIC KEY sym_Key  
			RETURN
		END
		CLOSE SYMMETRIC KEY sym_Key  
 
-- =============================================  
-- Return results  
-- =============================================  
SELECT
Inspection_ID,  
File_Reference_No,
SP_ID,
Practice_Display_Seq,
SP_Eng_Name,
SP_Chi_Name,
Practice_Name,
Practice_Name_Chi,
Main_Type_Of_Inspection_Value,
Type_Of_Inspection_Value,
Visit_Date,
Visit_Date_Format,
Case_Officer_Value,
Subject_Officer_Value,
Subject_Officer_ID,
Follow_Up_Action,
Record_Status,
Status_Description
from #TempInspectionRecord WITH(NOLOCK)

DROP TABLE #TempInspectionRecord
END
GO


GRANT EXECUTE ON [dbo].[proc_InspectionVisitInfo_get_byAny] TO HCVU
GO
