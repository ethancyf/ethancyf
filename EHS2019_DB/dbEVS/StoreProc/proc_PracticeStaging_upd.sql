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
	@enrolment_ref_no char(15), @display_seq smallint, @practice_name nvarchar(100),
	--@practice_type char(1), 
	@mo_display_seq smallint,
	@room nvarchar(5), @floor nvarchar(3), @block nvarchar(3),
	@building varchar(100), @building_chi nvarchar(100), @district char(4), 
	@address_code int, @professional_seq smallint, @record_status char(1), 
	@remark nvarchar(255), @update_by varchar(20), @tsmp timestamp,
	@practice_name_chi nvarchar(100), @phone_daytime varchar(20)
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

	UPDATE	PracticeStaging
	SET		Practice_Name = @practice_name,
			--Practice_Type = @practice_type,
			MO_Display_Seq = @mo_display_seq,
			Room = @room,
			[Floor] = @floor,
			Block = @block,
			Building = @building,
			Building_Chi =@building_chi,
			District = @district,
			Address_Code = @address_code,
			Professional_Seq = @professional_seq,
			Record_Status = @record_status,
			Remark = @remark,
			Update_Dtm = getdate(),
			Update_By = @update_By,
			Practice_name_chi = @practice_name_chi,
			Phone_daytime = @phone_daytime

	WHERE	Enrolment_Ref_No = @enrolment_ref_no and
			Display_Seq = @display_seq

END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeStaging_upd] TO HCVU
GO
