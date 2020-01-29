IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_RVPHomeList_update]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[proc_RVPHomeList_update]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
-- =============================================
-- Author:		Twinsen CHAN
-- CR No.:		CRE11-013
-- Create date: 19 Oct 2012
-- Description:	Update RVP Home List
-- =============================================
CREATE PROCEDURE [dbo].[proc_RVPHomeList_update]
	@RCH_code		varchar(10),
	@Type			varchar(5),
	@Homename_Eng	varchar(255),
	@Homename_Chi	nvarchar(255),
	@Address_Eng	varchar(1000),
	@Address_Chi	nvarchar(255),
	@Record_Status	char(1),
	@Update_By		varchar(20),
	@TSMP			timestamp

AS
BEGIN
	SET NOCOUNT ON;

	IF (SELECT TSMP
		FROM [dbo].[RVPHomeList] 
		WHERE RCH_code = @RCH_code) <> @TSMP
	BEGIN
			RAISERROR('00006', 16, 1)
			RETURN @@error
	END

	UPDATE [dbo].[RVPHomeList]
	SET 
		[Type] = @Type,
        [Homename_Eng] = @Homename_Eng,
        [Homename_Chi] = @Homename_Chi,
        [Address_Eng] = @Address_Eng,
        [Address_Chi] = @Address_Chi,
        [Record_Status] = @Record_Status,
        [Update_By] = @Update_By,
        [Update_Dtm] = GETDATE()
	WHERE 
		[RCH_code] = @RCH_code
		AND [TSMP] = @TSMP
END
GO

GRANT EXECUTE ON [dbo].[proc_RVPHomeList_update] TO HCVU
GO
