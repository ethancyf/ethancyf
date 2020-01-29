IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Practice_upd_Address]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Practice_upd_Address]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kathy LEE
-- Create date: 26 June 2008
-- Description:	Update Practice Address in
--				Table "Practice" 
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Clark YIP
-- Modified date: 6 Apr 2009
-- Description:	  Update also the phone_daytime of the practice in permanent
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Clark YIP
-- Modified date: 8 Jul 2009
-- Description:	  Update also the mo_display_seq, practice chi name
-- =============================================
CREATE PROCEDURE [dbo].[proc_Practice_upd_Address]
	@sp_id	char(8), @display_seq smallint, @room nvarchar(5), @floor nvarchar(3),
	@block nvarchar(3), @building varchar(100), @building_chi nvarchar(100), @district	char(4),
	@address_code int, @update_by varchar(20), @tsmp timestamp, @phone_daytime varchar(20), @mo_display_seq smallint, @practice_name_chi nvarchar(100)
AS
BEGIN
	
	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF (SELECT TSMP FROM Practice
		WHERE SP_ID = @sp_id and Display_Seq = @display_seq) != @tsmp
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
	UPDATE	Practice
	SET		Room = @room,
			[Floor] = @floor,
			Block = @block,
			Building = @building,
			Building_Chi = @building_chi,
			District = @district,
			Address_Code = @address_code,
			Update_Dtm = getDate(),
			Update_By = @update_by,
			Phone_Daytime=@phone_daytime,
			MO_Display_Seq=@mo_display_seq,
			Practice_Name_Chi=@practice_name_chi
	WHERE	SP_ID = @sp_id and
			Display_Seq = @display_seq

END
GO

GRANT EXECUTE ON [dbo].[proc_Practice_upd_Address] TO HCVU
GO
