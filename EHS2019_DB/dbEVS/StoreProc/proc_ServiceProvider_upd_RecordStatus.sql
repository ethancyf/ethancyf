IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProvider_upd_RecordStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProvider_upd_RecordStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- CR No:			CRE17-016 - Checking of PCD status during VSS enrolment
-- Modified by:		Koala CHENG
-- Modified date:	07 Aug 2018
-- Description:		1. If delist SP, clear PCD status columns
--							[PCD_Account_Status], [PCD_Enrolment_Status],[PCD_Professional], [PCD_Status_Last_Check_Dtm]
--					2. Check timestamp in where clause 
-- =============================================
-- =============================================
-- Modification History
-- CR No:			INT13-0028 - SP Amendment Report
-- Modified by:		Tommy LAM
-- Modified date:	20 Nov 2013
-- Description:		Add Column -	[ServiceProvider].[Data_Input_By]
--									[ServiceProvider].[Data_Input_Effective_Dtm]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Clark YIP
-- Modified date:  08 May 2009
-- Description:	   Remove the Delist_Status, delist_dtm
-- =============================================
-- =============================================
-- Author:		Tommy Cheung
-- Create date: 18 June 2008
-- Description:	Update the status of Record_Status in
--				Table "ServiceProvider"
-- =============================================
CREATE PROCEDURE [dbo].[proc_ServiceProvider_upd_RecordStatus]
	@SP_ID	char(8),
	@Record_Status char(1),
	--@Delist_Status char(1),
	@Update_By varchar(20),
	@Data_Input_By varchar(20),
	@TSMP timestamp
AS
BEGIN
	
	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================

	declare @current_dtm datetime

-- =============================================
-- Validation 
-- =============================================
	IF (SELECT TSMP FROM ServiceProvider
		WHERE SP_ID = @SP_ID) != @TSMP
	BEGIN
		RAISERROR('00011', 16, 1)
		return @@error
	END
-- =============================================
-- Initialization
-- =============================================

	set @current_dtm = getdate()

-- =============================================
-- Return results
-- =============================================
IF @Record_Status = 'D'
BEGIN
	UPDATE	ServiceProvider
	Set		Record_Status = @Record_Status,
			Update_By = @Update_By,
			Update_Dtm = @current_dtm,
			Data_Input_By = @Data_Input_By,
			Data_Input_Effective_Dtm = @current_dtm,
			PCD_Account_Status = 'I', -- N/A
			PCD_Enrolment_Status = 'I', -- N/A
			PCD_Professional = NULL,
			PCD_Status_Last_Check_Dtm = NULL
	WHERE	SP_ID = @SP_ID
			AND TSMP = @TSMP
END

ELSE
BEGIN
	UPDATE	ServiceProvider
	Set		Record_Status = @Record_Status,
			Update_By = @Update_By,
			Update_Dtm = @current_dtm,
			Data_Input_By = @Data_Input_By,
			Data_Input_Effective_Dtm = @current_dtm
	WHERE	SP_ID = @SP_ID
			AND TSMP = @TSMP
END
END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProvider_upd_RecordStatus] TO HCVU
GO
