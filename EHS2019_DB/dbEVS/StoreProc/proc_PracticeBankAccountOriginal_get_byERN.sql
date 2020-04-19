IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeBankAccountOriginal_get_byERN]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeBankAccountOriginal_get_byERN]
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
-- CR No.:		CRE12-001
-- Author:		Koala CHENG
-- Create date: 16 Jan 2012
-- Description:	Retrieve the Bank Account with 
--				Practice Information Original record by enrollment
--				reference number
-- =============================================
CREATE PROCEDURE [dbo].[proc_PracticeBankAccountOriginal_get_byERN]
	@enrolment_ref_no	CHAR(15)
AS
BEGIN

	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
DECLARE	@record_id			INT
		,@address_eng		VARCHAR(255)
		,@address_chi		NVARCHAR(255)
		,@district_code		CHAR(5)
		,@eh_eng			VARCHAR(255)
		,@eh_chi			VARCHAR(255)
		,@display_seq		SMALLINT

DECLARE @tmp_practice TABLE( 
		[Enrolment_Ref_No]		CHAR(15)
		,[Display_Seq]			SMALLINT
		,[Practice_Name]		NVARCHAR(100)
		,[Practice_Name_Chi]	NVARCHAR(100)
		,[Room]					NVARCHAR(5)
		,[Floor]				NVARCHAR(3)
		,[Block]				NVARCHAR(3)
		,[Building]				VARCHAR(100)
		,[Building_Chi]			NVARCHAR(100)
		,[District]				CHAR(4)
		,[Address_Code]			INT
		,[Professional_Seq]		SMALLINT
		,[Phone_Daytime]		VARCHAR(20)
		,[MO_Display_Seq]		SMALLINT
		--,[Mobile_Clinic]		CHAR(1)
		--,[Remarks_Desc]			VARCHAR(200)
		--,[Remarks_Desc_Chi]		NVARCHAR(100)	
		)

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	INSERT INTO @tmp_practice( 
		[Enrolment_Ref_No]
		,[Display_Seq]			
		,[Practice_Name]		
		,[Practice_Name_Chi]	
		,[Room]					
		,[Floor]				
		,[Block]				
		,[Building]				
		,[Building_Chi]			
		,[District]				
		,[Address_Code]			
		,[Professional_Seq]		
		,[Phone_Daytime]		
		,[MO_Display_Seq]		
		--,[Mobile_Clinic]		
		--,[Remarks_Desc]			
		--,[Remarks_Desc_Chi]		
		)
	SELECT 
		[Enrolment_Ref_No]
		,[Display_Seq]			
		,[Practice_Name]		
		,[Practice_Name_Chi]	
		,[Room]					
		,[Floor]				
		,[Block]				
		,[Building]				
		,[Building_Chi]			
		,[District]				
		,[Address_Code]			
		,[Professional_Seq]		
		,[Phone_Daytime]		
		,[MO_Display_Seq]		
		--,[Mobile_Clinic]		
		--,[Remarks_Desc]			
		--,[Remarks_Desc_Chi]		
	FROM 
		PracticeOriginal
	WHERE 
		Enrolment_Ref_No = @enrolment_ref_no

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

			UPDATE 
				@tmp_practice
			SET	
				Building = @address_eng, 
				Building_Chi = @address_chi,
				District = @district_code
			WHERE 
				Display_Seq = @display_seq
		END

		FETCH NEXT FROM avail_cursor INTO @record_id, @display_seq
	END

	CLOSE avail_cursor 
	DEALLOCATE avail_cursor
-- =============================================
-- Return results
-- =============================================

    SELECT 
		P.[Enrolment_Ref_No]
		,P.[Display_Seq]			
		,P.[Practice_Name]		
		,P.[Practice_Name_Chi]	
		,P.[Room]					
		,P.[Floor]				
		,P.[Block]				
		,P.[Building]				
		,P.[Building_Chi]			
		,P.[District]				
		,P.[Address_Code]			
		,P.[Professional_Seq]		
		,P.[Phone_Daytime]		
		,P.[MO_Display_Seq]		
		--,P.[Mobile_Clinic]		
		--,P.[Remarks_Desc]			
		--,P.[Remarks_Desc_Chi]		
		,PE.[Service_Category_Code]
		,PE.[Registration_Code]
		,B.[Display_Seq] AS Bank_Display_Seq
		,B.[Bank_Name]
		,B.[Branch_Name]
		,B.[Bank_Account_No]
		,B.[Bank_Acc_Holder]
	FROM 
		@tmp_practice P
			INNER JOIN ProfessionalOriginal PE
				ON P.Professional_Seq = PE.Professional_Seq 
					AND P.Enrolment_Ref_No = PE.Enrolment_Ref_No
			LEFT JOIN BankAccountOriginal B
				ON P.Display_Seq = B.SP_Practice_Display_Seq 
					AND P.Enrolment_Ref_No = B.Enrolment_Ref_No
	WHERE 
		P.Enrolment_Ref_No = @enrolment_ref_no
	ORDER BY
		P.Display_Seq

END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeBankAccountOriginal_get_byERN] TO HCVU
GO

