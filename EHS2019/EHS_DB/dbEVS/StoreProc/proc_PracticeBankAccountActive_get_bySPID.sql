IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeBankAccountActive_get_bySPID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeBankAccountActive_get_bySPID]
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
-- CR No.:			CRE19-006 (DHC)
-- Modified by:		Winnie SUEN
-- Modified date:	24 Jun 2019
-- Description:		Return [Registration_Code]
-- =============================================
-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 26 Apr 2008
-- Description:	Retrieve the active Bank Account information 
--              by using SP_ID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	01 Jun 2009
-- Description:		Adapt the new schema of Table [Practice]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	22 Jul 2009
-- Description:		Remove redundant Inner Join with Practice
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	22 July 2009
-- Description:		Hardcode to receive only EHCVS scheme only
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	29 July 2009
-- Description:		Practice Chi Name nvarchar 100
--					Add Practice_Name_Display_Chi
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	05 Aug 2009
-- Description:		Check Practice Scheme Info of EHCVS = Active
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	06 Aug 2009
-- Description:		SchemeCode=EHCVS ==> SubsidizeCode=>EHCVS
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	14 Aug 2009
-- Description:		Remove SchemeCode=EHCVS 
--					Remove Practice_Name_Display
--					Remove Practice_Name_Display_Chi (Handle in Middle Tier)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	17 Aug 2009
-- Description:		Remove PracticeSchemeInfo Inner Join 
--					(Since Practice has more than 1 Practice Scheme) 
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================

CREATE PROCEDURE [dbo].[proc_PracticeBankAccountActive_get_bySPID]
	@SP_ID CHAR(8)
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
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
		if @record_id IS NOT null
		BEGIN
			SELECT	@address_eng = '',
					@address_chi = '',
					@district_code = '',
					@eh_eng = '',
					@eh_chi = ''

			exec cpi_get_address_detail   @record_id 
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

		FETCH next FROM avail_cursor INTO @record_id, @display_seq
	END
	CLOSE avail_cursor 
	DEALLOCATE avail_cursor

-- =============================================
-- Return results
-- =============================================

	SELECT
		P.[SP_ID]
		,P.[Display_Seq] AS [PracticeID]
		,P.[Practice_Name]
		,P.[Practice_Name_Chi]
		,P.[Record_Status] AS [Practice_Status]
		,P.[SP_ID] + '-' + CONVERT(VARCHAR(3),	P.[Display_Seq]) + '-' + CONVERT(VARCHAR(3), B.[Display_Seq]) as [BankAccountKey]
		,P.[Room]
		,P.[Floor]
		,P.[Block]
		,P.[Building]
		,P.[Building_Chi]
		,P.[District]
		,P.[Address_Code]
		,P.[Phone_Daytime]
		,P.[Mobile_Clinic]		
		,P.[Remarks_Desc]			
		,P.[Remarks_Desc_Chi]

		,PFS.[Service_Category_Code]
		,PFS.[Registration_Code]

		,B.[Display_Seq] as [BankAcctID]
		,B.[Bank_Account_No]
		,B.[Bank_Acc_Holder]
		,B.[Record_Status] AS [BankAcct_Status]
	
	FROM @tmp_practice P

		INNER JOIN [Professional] PFS
			ON P.[Professional_Seq] = PFS.[Professional_Seq]
				AND P.[SP_ID] = PFS.[SP_ID]

		INNER JOIN [BankAccount] B
			ON P.[Display_Seq] = B.[SP_Practice_Display_Seq] 
				AND P.[SP_ID] = B.[SP_ID]

	WHERE 
		P.[SP_ID] = @SP_ID 
		AND B.[Record_status] = 'A' 
		AND P.[Record_status] = 'A' 

	ORDER BY P.Display_Seq

END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeBankAccountActive_get_bySPID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_PracticeBankAccountActive_get_bySPID] TO WSEXT
GO

