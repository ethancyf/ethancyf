IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SpecialAccount_upd_RecordStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SpecialAccount_upd_RecordStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Pak Ho LEE
-- Create date:		18 Sep 2009
-- Description:		Update Special Account Record Status from HCVU
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE Procedure [proc_SpecialAccount_upd_RecordStatus]
	@Special_Acc_ID	char(15),
	@Update_By		varchar(20),
	@Update_Dtm		datetime,
	@Record_Status	char(1),
	@tsmp			timestamp
AS
BEGIN
	SET NOCOUNT ON;
	
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF (SELECT [TSMP] from [SpecialAccount] where [Special_Acc_ID] = @Special_Acc_ID) != @tsmp  
	BEGIN  
		Raiserror('00011', 16, 1)  
		return @@error  
	END
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================
	
	Update [SpecialAccount]
	SET	[Record_Status] = @Record_Status,
		[Update_By] = @Update_By,
		[Update_Dtm] = @Update_Dtm
	WHERE [Special_Acc_ID] = @Special_Acc_ID
		  
END
GO

GRANT EXECUTE ON [dbo].[proc_SpecialAccount_upd_RecordStatus] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SpecialAccount_upd_RecordStatus] TO HCVU
GO
