IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeStaging_upd]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeStaging_upd]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kathy LEE
-- Create date: 24 May 2008
-- Description:	Update Practice Infomation in
--				Table "PracticeStaging" 
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   19 May 2009
-- Description:	    Not update the practice type, but the MO_Display_Seq
--				    Get the practice chi name and contact no
-- =============================================
CREATE PROCEDURE [dbo].[proc_PracticeStaging_upd]
	@enrolment_ref_no	CHAR(15), 
	@display_seq		SMALLINT, 
	@practice_name		NVARCHAR(100),
	@mo_display_seq		SMALLINT,
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
	@update_by			VARCHAR(20),
	@tsmp				TIMESTAMP,
	@practice_name_chi	NVARCHAR(100),
	@phone_daytime		VARCHAR(20), 
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
	IF (SELECT TSMP FROM PracticeStaging
		WHERE Enrolment_Ref_No = @enrolment_ref_no and
				Display_Seq = @display_seq) != @tsmp
	BEGIN
		RAISERROR('00011', 16, 1)
		return @@error
	END
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	UPDATE	
		PracticeStaging
	SET		
		[Practice_Name] =		@practice_name
		,[MO_Display_Seq] =		@mo_display_seq
		,[Room] =				@room
		,[Floor] =				@floor
		,[Block] =				@block
		,[Building] =			@building
		,[Building_Chi] =		@building_chi
		,[District] =			@district
		,[Address_Code] =		@address_code
		,[Professional_Seq] =	@professional_seq
		,[Record_Status] =		@record_status
		,[Remark] =				@remark
		,[Update_Dtm] =			GETDATE()
		,[Update_By] =			@update_By
		,[Practice_name_chi] =	@practice_name_chi
		,[Phone_daytime] =		@phone_daytime
		,[Mobile_Clinic] =		@Mobile_clinic
		,[Remarks_Desc] =		@remarks_desc
		,[Remarks_Desc_Chi] =	@remarks_desc_chi

	WHERE	
		[Enrolment_Ref_No] = @enrolment_ref_no 
		AND	[Display_Seq] = @display_seq

END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeStaging_upd] TO HCVU
GO

