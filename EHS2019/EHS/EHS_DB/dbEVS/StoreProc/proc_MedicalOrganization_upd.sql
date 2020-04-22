IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_MedicalOrganization_upd]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_MedicalOrganization_upd]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- CR No.:			CRE16-009 (To enable the eHS(S) to display Chinese Unicode in the field of "English Name of M.O.")
-- Modified by:		Koala CHENG
-- Modified date:	07 Sep 2016
-- Description:		Change MO_Eng_Name from VARCHAR(100) to NVARCHAR(100)
-- =============================================
-- =============================================
-- Author:		Clark YIP
-- Create date: 15 May 2009
-- Description:	Update MedicalOrganization Record Status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Clark YIP
-- Modified date: 6 Aug 2009
-- Description:	 Allow update Email
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	13 Aug 2009
-- Description:		Allow update Fax
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date: 
-- Description:	 
-- =============================================
CREATE PROCEDURE [dbo].[proc_MedicalOrganization_upd]
	@sp_id	char(8), @display_seq smallint, @mo_eng_name nvarchar(100), @mo_chi_name nvarchar(100), @room nvarchar(5), @floor nvarchar(3),
	@block nvarchar(3), @building varchar(100), @building_chi nvarchar(100), @district	char(4),
	@address_code int, @phone_daytime varchar(20), @update_by varchar(20), @tsmp timestamp, @email varchar(255), @fax varchar(20)
	
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================

	IF (SELECT TSMP FROM [MedicalOrganization]
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
	UPDATE	[MedicalOrganization]
	SET		MO_Eng_Name = @mo_eng_name,
			MO_Chi_Name = @mo_chi_name,
			Room = @room,
			[Floor] = @floor,
			Block = @block,
			Building = @building,
			Building_Chi = @building_chi,
			District = @district,
			Address_Code = @address_code,
			Update_Dtm = getDate(),
			Update_By = @update_by,
			Phone_Daytime=@phone_daytime,
			Email=@email,
			Fax = @fax
	WHERE	SP_ID = @sp_id and
			Display_Seq = @display_seq
END
GO

GRANT EXECUTE ON [dbo].[proc_MedicalOrganization_upd] TO HCVU
GO
