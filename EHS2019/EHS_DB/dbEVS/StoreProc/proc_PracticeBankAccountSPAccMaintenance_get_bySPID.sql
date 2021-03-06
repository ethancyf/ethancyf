IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeBankAccountSPAccMaintenance_get_bySPID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeBankAccountSPAccMaintenance_get_bySPID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.			CRE16-022 (SDIR Remark)
-- Modified by:		CHRIS YIM
-- Modified date:	17 Feb 2020
-- Description:		Add columns [Mobile_Clinic],[Remarks_Desc] & [Remarks_Desc_Chi]
-- =============================================
-- =============================================
-- Modification History
-- CR No.:		  CRE13-019-02
-- Modified by:	  Winnie SUEN
-- Modified date: 19 Dec 2014
-- Description:	  Add "IsFreeTextFormat"
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
-- Modified by:	   Kathy LEE
-- Modified date:  30 July 2009
-- Description:	   Retrieve results without joining SPAccoutMaintenance
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	   Kathy LEE
-- Modified date:  09 July 2009
-- Description:	   Change Practice Record Status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Clark YIP
-- Modified date:  05 Apr 2009
-- Description:	   Not get the practice_type, but the MO_Display_Seq
--				   Get the practice chi name and contact no
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 10 July 2008
-- Description:	Retrieve Bank Account, Practice Information,
--				and professional and Maintenance information by SPID in permanent
-- =============================================

CREATE PROCEDURE [dbo].[proc_PracticeBankAccountSPAccMaintenance_get_bySPID]
	@SP_ID CHAR(8)
AS
BEGIN

	SET NOCOUNT ON;

   -- =============================================
-- Declaration
-- =============================================
	DECLARE	@record_id		INT,
			@address_eng	VARCHAR(255),
			@address_chi	NVARCHAR(255),
			@district_code	CHAR(5),
			@eh_eng			VARCHAR(255),
			@eh_chi			VARCHAR(255),
			@display_seq	SMALLINT

	DECLARE @tmp_practice	TABLE( 
			[Display_Seq]			SMALLINT
			,[SP_ID]				CHAR(8)
			,[Practice_Name]		NVARCHAR(100)
			,[MO_Display_Seq]		SMALLINT
			,[Room]					NVARCHAR(5)
			,[Floor]				NVARCHAR(3)
			,[Block]				NVARCHAR(3)
			,[Building]				VARCHAR(100)
			,[Building_Chi]			NVARCHAR(100)
			,[District]				CHAR(4)
			,[Address_Code]			INT
			,[Professional_Seq]		SMALLINT
			,[Record_Status]		CHAR(1)
			,[Remark]				NVARCHAR(255)
			,[Submission_Method]	CHAR(1)
			,[Create_Dtm]			DATETIME
			,[Create_By]			VARCHAR(20)
			,[Update_Dtm]			DATETIME
			,[Update_By]			VARCHAR(20)
			,[Practice_Name_Chi]	NVARCHAR(100)
			,[Phone_Daytime]		VARCHAR(20)
			,[Mobile_Clinic]		CHAR(1)
			,[Remarks_Desc]			NVARCHAR(200)
			,[Remarks_Desc_Chi]		NVARCHAR(200)	
			)

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	INSERT INTO @tmp_practice(
		[Display_Seq]		
		,[SP_ID]				
		,[Practice_Name]		
		,[MO_Display_Seq]	
		,[Room]				
		,[Floor]				
		,[Block]				
		,[Building]			
		,[Building_Chi]		
		,[District]			
		,[Address_Code]		
		,[Professional_Seq]	
		,[Record_Status]		
		,[Remark]			
		,[Submission_Method]	
		,[Create_Dtm]		
		,[Create_By]			
		,[Update_Dtm]		
		,[Update_By]			
		,[Practice_Name_Chi]
		,[Phone_Daytime]
		,[Mobile_Clinic]		
		,[Remarks_Desc]			
		,[Remarks_Desc_Chi]
		)
	SELECT					 
		[Display_Seq]		
		,[SP_ID]				
		,[Practice_Name]		
		,[MO_Display_Seq]	
		,[Room]				
		,[Floor]				
		,[Block]				
		,[Building]			
		,[Building_Chi]		
		,[District]			
		,[Address_Code]		
		,[Professional_Seq]	
		,[Record_Status]		
		,[Remark]			
		,[Submission_Method]	
		,[Create_Dtm]		
		,[Create_By]			
		,[Update_Dtm]		
		,[Update_By]			
		,[Practice_Name_Chi]
		,[Phone_Daytime]
		,[Mobile_Clinic]		
		,[Remarks_Desc]			
		,[Remarks_Desc_Chi]			 
	FROM 
		Practice
	WHERE 
		SP_ID = @SP_ID

	-- Update address, district
	DECLARE avail_cursor CURSOR  
	FOR	SELECT Address_Code, Display_Seq
	FROM @tmp_practice

	OPEN avail_cursor 
	FETCH next FROM avail_cursor INTO @record_id, @display_seq
	WHILE @@Fetch_status = 0
	BEGIN
		IF @record_id IS NOT null
		BEGIN
			SELECT	@address_eng = '',
					@address_chi = '',
					@district_code = '',
					@eh_eng = '',
					@eh_chi = ''

			EXEC cpi_get_address_detail   @record_id 
									, @address_eng = @address_eng  OUTPUT 
    								, @address_chi = @address_chi    OUTPUT 
									, @district_code = @district_code    OUTPUT 
									, @eh_eng = @eh_eng	OUTPUT
									, @eh_chi = @eh_chi	OUTPUT

		UPDATE @tmp_practice
		SET	Building = @address_eng, 
			Building_Chi = @address_chi,
			District = @district_code
		WHERE Display_Seq = @display_seq
		END

		FETCH NEXT FROM avail_cursor INTO @record_id, @display_seq
	END
	CLOSE avail_cursor 
	DEALLOCATE avail_cursor

-- =============================================
-- Return results
-- =============================================
	SELECT	
		P.[Display_Seq]
		,P.[SP_ID]
		,P.[Practice_Name]
		,P.[Practice_Name_Chi]
		,P.[MO_Display_Seq]
		,P.[Room]
		,P.[Floor]
		,P.[Block]
		,P.[Building]
		,P.[Building_Chi]
		,P.[District]
		,P.[Address_Code]
		,'' AS [Practice_Remark]
		,P.[Phone_Daytime]
		,P.[Professional_Seq]
		,P.[Submission_Method] AS [Practice_Submission_Method]
		,P.[Create_Dtm] AS [Practice_Create_Dtm]
		,P.[Create_By] AS [Practice_Create_By]
		,P.[Update_Dtm] AS [Practice_Update_Dtm]
		,P.[Update_By] AS [Practice_Update_By]
		,P.[Record_Status] AS [Practice_Record_Status]
		,P.[Mobile_Clinic]		
		,P.[Remarks_Desc]			
		,P.[Remarks_Desc_Chi]
	
		,PS.[TSMP] AS [Practice_TSMP]

		,PFS.[Service_Category_Code]
		,PFS.[Registration_Code]
		,PFS.[Record_Status] AS [Professional_Record_Status]
		,PFS.[Create_Dtm] AS [Professional_Create_Dtm]
		,PFS.[Create_By] AS [Professional_Create_By]

		,B.[Display_Seq] AS [Bank_Display_Seq]
		,B.[Bank_Name]
		,B.[Branch_Name]
		,B.[Bank_Account_No]
		,B.[Bank_Acc_Holder]
		,B.[Record_Status] AS [Bank_Record_Status]
		,B.[Remark] AS [Bank_Remark]
		,B.[Submission_Method] AS [Bank_Submission_Method]
		
		,B.[Create_Dtm] AS [Bank_Create_Dtm]
		,B.[Create_By] AS [Bank_Create_By]
		,B.[Update_Dtm] AS [Bank_Update_Dtm]
		,B.[Update_By] AS [Bank_Update_By]
		,B.[TSMP] AS [Bank_TSMP]
		,B.[IsFreeTextFormat] AS [Bank_IsFreeTextFormat]

	FROM 
		@tmp_practice P

			INNER JOIN Practice PS
				ON P.[SP_ID] = PS.[SP_ID] and P.[Display_Seq] = PS.[Display_Seq]

			LEFT JOIN Professional PFS
				ON P.[Professional_Seq] = PFS.[Professional_Seq] and P.[SP_ID] = PFS.[SP_ID]

			LEFT JOIN BankAccount B
				ON P.[Display_Seq] = B.[SP_Practice_Display_Seq] and P.[SP_ID] = B.[SP_ID]
	
	WHERE 
		P.[SP_ID] = @SP_ID

	ORDER BY P.[Display_Seq]

END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeBankAccountSPAccMaintenance_get_bySPID] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_PracticeBankAccountSPAccMaintenance_get_bySPID] TO WSINT
GO

