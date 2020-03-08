IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticePermanent_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticePermanent_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Tommy Cheung
-- Create date: 23 June 2008
-- Description:	Insert the Practice to Table
--				Practice
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Clark YIP
-- Modified date:  12 Apr 2009
-- Description:	   Not get the practice_type, but the MO_Display_Seq
--				   Add the practice_name_chi and phone_daytime
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE [dbo].[proc_PracticePermanent_add]
	@display_seq smallint, 
	@sp_id char(8), 
	@practice_name nvarchar(100),
	@practice_name_chi nvarchar(100),
	@room nvarchar(5), 
	@floor nvarchar(3), 
	@block nvarchar(3),
	@building varchar(100), 
	@building_chi nvarchar(100), 
	@district char(4), 
	@address_code int, 
	@professional_seq smallint, 
	@record_status char(1), 
	@remark nvarchar(255), 
	@phone_daytime varchar(20), 
	@mo_display_seq smallint,
	@submission_method char(1), 
	@create_by varchar(20),
	@update_by varchar(20)
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

	INSERT INTO Practice
				(Display_Seq,
				 SP_ID,
				 Practice_Name,
				 Practice_name_chi,				 
				 Room,
				 [Floor],
				 Block,
				 Building,
				 Building_Chi,
				 District,
				 Address_Code,
				 Professional_Seq,
				 Record_Status,
				 Remark,
				 Phone_daytime,				 
				 MO_Display_Seq,
				 Submission_Method,
				 Create_Dtm,
				 Create_By,
				 Update_Dtm,
				 Update_By)
	VALUES		(@display_seq,
				 @sp_id,
				 @practice_name,
				 @practice_name_chi,				 
				 @room,
				 @floor,
				 @block,
				 @building,
				 @building_chi,
				 @district,
				 @address_code,
				 @professional_seq,
				 @record_status,
				 @remark,
				 @phone_daytime,
				 @mo_display_seq,
				 @submission_method,
				 getdate(),
				 @create_by,
				 getdate(),
				 @update_by 
                 )
END
GO

GRANT EXECUTE ON [dbo].[proc_PracticePermanent_add] TO HCVU
GO