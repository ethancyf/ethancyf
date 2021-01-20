IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeTransition_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeTransition_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Author:		Clark YIP
-- Create date: 11 June 2009
-- Description:	Insert the PracticeTransition to Table
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date:  
-- Description:	   
-- =============================================

CREATE PROCEDURE [dbo].[proc_PracticeTransition_add]
	@hk_id char(9),
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
	@phone_daytime varchar(20), 
	@mo_display_seq smallint,	
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

EXEC [proc_SymmetricKey_open]

	INSERT INTO PracticeTransition
				(Encrypt_Field1,
				 Display_Seq,
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
				 Phone_daytime,				 
				 MO_Display_Seq,				 
				 Create_Dtm,
				 Create_By,
				 Update_Dtm,
				 Update_By)
	VALUES		(
				 EncryptByKey(KEY_GUID('sym_Key'), @HK_ID),
				 @display_seq,
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
				 @phone_daytime,
				 @mo_display_seq,
				 getdate(),
				 @create_by,
				 getdate(),
				 @update_by 
                 )
                 
EXEC [proc_SymmetricKey_close]

END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeTransition_add] TO HCVU
GO
