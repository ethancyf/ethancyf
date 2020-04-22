IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Practice_upd_Address]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Practice_upd_Address]
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
-- Modified date: 8 Jul 2009
-- Description:	  Update also the mo_display_seq, practice chi name
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Clark YIP
-- Modified date: 6 Apr 2009
-- Description:	  Update also the phone_daytime of the practice in permanent
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 26 June 2008
-- Description:	Update Practice Address in
--				Table "Practice" 
-- =============================================

CREATE PROCEDURE [dbo].[proc_Practice_upd_Address]
	@sp_id				CHAR(8), 
	@display_seq		SMALLINT, 
	@room				NVARCHAR(5), 
	@floor				NVARCHAR(3),
	@block				NVARCHAR(3), 
	@building			VARCHAR(100), 
	@building_chi		NVARCHAR(100), 
	@district			CHAR(4),
	@address_code		INT, 
	@update_by			VARCHAR(20), 
	@tsmp				TIMESTAMP, 
	@phone_daytime		VARCHAR(20), 
	@mo_display_seq		SMALLINT, 
	@practice_name_chi	NVARCHAR(100),
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
	UPDATE	
		Practice
	SET		
		[Room] =				@room,
		[Floor] =				@floor,
		[Block] =				@block,
		[Building] =			@building,
		[Building_Chi] =		@building_chi,	
		[District] =			@district,
		[Address_Code] =		@address_code,
		[Update_Dtm] =			GETDATE(),
		[Update_By] =			@update_by,
		[Phone_Daytime]	=		@phone_daytime,
		[MO_Display_Seq] =		@mo_display_seq,
		[Practice_Name_Chi]	=	@practice_name_chi,
		[Mobile_Clinic]	=		@Mobile_clinic,
		[Remarks_Desc]	=		@remarks_desc,
		[Remarks_Desc_Chi]	=	@remarks_desc_chi
	WHERE	
		[SP_ID] = @sp_id 
		AND	[Display_Seq] = @display_seq

END
GO

GRANT EXECUTE ON [dbo].[proc_Practice_upd_Address] TO HCVU
GO

