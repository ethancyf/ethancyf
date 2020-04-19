IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticePermanent_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticePermanent_add]
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
-- Modified by:	Clark YIP
-- Modified date:  12 Apr 2009
-- Description:	   Not get the practice_type, but the MO_Display_Seq
--				   Add the practice_name_chi and phone_daytime
-- =============================================
-- =============================================
-- Author:		Tommy Cheung
-- Create date: 23 June 2008
-- Description:	Insert the Practice to Table
--				Practice
-- =============================================

CREATE PROCEDURE [dbo].[proc_PracticePermanent_add]
	@display_seq		SMALLINT, 
	@sp_id				CHAR(8), 
	@practice_name		NVARCHAR(100),
	@practice_name_chi	NVARCHAR(100),
	@room				NVARCHAR(5), 
	@floor				NVARCHAR(3), 
	@block				NVARCHAR(3),
	@building			VARCHAR(100), 
	@building_chi		NVARCHAR(100), 
	@district			CHAR(4), 
	@address_code		INT, 
	@professional_seq	SMALLINT, 
	@record_status		CHAR(1), 
	@remark				NVARCHAR(255), 
	@phone_daytime		VARCHAR(20), 
	@mo_display_seq		SMALLINT,
	@submission_method	CHAR(1), 
	@create_by			VARCHAR(20),
	@update_by			VARCHAR(20),
	@Mobile_clinic		CHAR(1),
	@remarks_desc		NVARCHAR(200),
	@remarks_desc_chi	NVARCHAR(200)
AS
BEGIN

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

	INSERT INTO Practice(
		[Display_Seq]
		,[SP_ID]
		,[Practice_Name]
		,[Practice_name_chi]				 
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
		,[Phone_daytime]
		,[MO_Display_Seq]
		,[Submission_Method]
		,[Create_Dtm]
		,[Create_By]
		,[Update_Dtm]
		,[Update_By]
		,[Mobile_Clinic]
		,[Remarks_Desc]
		,[Remarks_Desc_Chi]
		)
	VALUES(
		@display_seq
		,@sp_id
		,@practice_name
		,@practice_name_chi				
		,@room
		,@floor
		,@block
		,@building
		,@building_chi
		,@district
		,@address_code
		,@professional_seq
		,@record_status
		,@remark
		,@phone_daytime
		,@mo_display_seq
		,@submission_method
		,GETDATE()
		,@create_by
		,GETDATE()
		,@update_by
		,@Mobile_clinic
		,@remarks_desc
		,@remarks_desc_chi
        )
END
GO

GRANT EXECUTE ON [dbo].[proc_PracticePermanent_add] TO HCVU
GO

