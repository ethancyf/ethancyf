IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_InspectionVisitInfo_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_InspectionVisitInfo_add]
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
   
CREATE PROCEDURE [dbo].[proc_InspectionVisitInfo_add]  
	@File_Reference_No			VARCHAR(30),
	@File_Reference_Type		VARCHAR(30),
	@Inspection_ID			VARCHAR(30),
	@Referred_Reference_No_1	VARCHAR(30),
	@Referred_Reference_No_2	VARCHAR(30),
	@Referred_Reference_No_3	VARCHAR(30),
	@SP_ID					CHAR(8),
	@Practice_Display_Seq	SMALLINT,
	@Main_Type_Of_Inspection VARCHAR(10),
	@Type_Of_Inspection		VARCHAR(2000),
	@Visit_Date				DATETIME,
	@Visit_Begin_Dtm		DATETIME,
	@Visit_End_Dtm			DATETIME,
	@Confirmation_with		VARCHAR(100),
	@Confirmation_Dtm		DATETIME,
	@Form_Condition			VARCHAR(10),
	@Form_Condition_Remark	NVARCHAR(255),
	@Means_Of_Communication	VARCHAR(10),
	@Means_Of_Communication_Fax	VARCHAR(20),
	@Means_Of_Communication_Email	VARCHAR(255),
	@Low_Risk_Claim         VARCHAR(1),
	@Remarks				NVARCHAR(255),
	@Case_Officer			VARCHAR(100),
	@Case_Contact_No		VARCHAR(20),
	@Subject_Officer		VARCHAR(100),
	@Subject_Contact_No		VARCHAR(20),
	@Record_Status			VARCHAR(10),
	@User_ID				VARCHAR(20),
	@Service_Category_Code  VARCHAR(5)
AS  
BEGIN  
	SET NOCOUNT ON;  
	BEGIN  
	IF EXISTS(Select * From InspectionVisitInfo WITH(NOLOCK) WHERE File_Reference_No = @File_Reference_No)
	BEGIN
		RAISERROR('00007', 16, 1) --Duplicated
		RETURN @@error
	END
	ELSE
		INSERT INTO [InspectionVisitInfo](  
			[File_Reference_No],
			[File_Reference_Type],
			[Inspection_ID],
			[Referred_Reference_No_1],
			[Referred_Reference_No_2],
			[Referred_Reference_No_3],
			[SP_ID] ,
			[Practice_Display_Seq] ,
			[Service_Category_Code],
			[Main_Type_Of_Inspection],
			[Visit_Date] ,
			[Visit_Begin_Dtm],
			[Visit_End_Dtm] ,
			[Confirmation_with] ,
			[Confirmation_Dtm] ,
			[Form_Condition] ,
			[Form_Condition_Remark] ,
			[Means_Of_Communication],
			[Means_Of_Communication_Fax],
			[Means_Of_Communication_Email],
			[Low_Risk_Claim],
			[Remarks] ,
			[Case_Officer] ,
			[Case_Officer_Contact_No],
			[Subject_Officer] ,
			[Subject_Officer_Contact_No],
			[Record_Status] ,
			[Create_Dtm],
			[Create_By],
			[Update_Dtm],
			[Update_By]
			)
		VALUES(  
			@File_Reference_No,
			@File_Reference_Type,
			@Inspection_ID,
			@Referred_Reference_No_1,
			@Referred_Reference_No_2,
			@Referred_Reference_No_3,
			@SP_ID,
			@Practice_Display_Seq,
			@Service_Category_Code,
			@Main_Type_Of_Inspection,
			@Visit_Date,
			@Visit_Begin_Dtm,
			@Visit_End_Dtm,
			@Confirmation_with,
			@Confirmation_Dtm,
			@Form_Condition,
			@Form_Condition_Remark,
			@Means_Of_Communication,
			@Means_Of_Communication_Fax,
			@Means_Of_Communication_Email,
			@Low_Risk_Claim,
			@Remarks,
			@Case_Officer,
			@Case_Contact_No,
			@Subject_Officer,
			@Subject_Contact_No,
			@Record_Status,
			getdate(),
			@User_ID,
			getdate(),
			@User_ID
			)

			delete InspectionTypeSelections where Inspection_ID=@Inspection_ID
			DECLARE @x XML
			SELECT @x = @Type_Of_Inspection
			insert into InspectionTypeSelections
			SELECT 
			@Inspection_ID,
			c.value('*[1]','VARCHAR(10)') AS Type_Of_Inspection
			FROM @x.nodes('/DocumentElement/TypeOfInspection') T(C)

	END   
END    
GO

GRANT EXECUTE ON [dbo].[proc_InspectionVisitInfo_add] TO HCVU
GO
