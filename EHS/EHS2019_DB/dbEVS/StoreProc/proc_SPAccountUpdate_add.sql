IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SPAccountUpdate_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SPAccountUpdate_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kathy LEE
-- Create date: 30 May 2008
-- Description:	Insert the SP Account Update to Table
--				SPAccountUpdate
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Kathy LEE
-- Modified date: 18 June 2009
-- Description:	Add "scheme_confirm"
-- =============================================
-- =============================================
-- Modification History
-- CR No:			INT13-0028 - SP Amendment Report
-- Modified by:		Tommy LAM
-- Modified date:	18 Nov 2013
-- Description:		Add Column -	[SPAccountUpdate].[Data_Input_By]
-- =============================================
CREATE PROCEDURE [dbo].[proc_SPAccountUpdate_add]
	@enrolment_ref_no char(15), @sp_id char(8), @upd_sp_info char(1),
	@upd_bank_account char(1), @upd_professional char(1), @issue_token char(1), @scheme_confirm char(1),
	@progress_status char(1), @update_by varchar(20), @data_input_by varchar(20)
AS
BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================

declare @current_dtm datetime
declare @rowcount int

SELECT	@rowcount = count(1)       
FROM	SPAccountUpdate
WHERE	Enrolment_Ref_No = @enrolment_ref_no
		
	IF @rowcount > 0
	BEGIN
		RAISERROR('00011', 16, 1)
		return @@ERROR
	END
	
-- =============================================
-- Initialization
-- =============================================

set @current_dtm = getdate()

-- =============================================
-- Return results
-- =============================================
	INSERT INTO SPAccountUpdate
				(Enrolment_Ref_No,
				 SP_ID,
				 Upd_SP_Info,
				 Upd_Bank_Account,
				 Upd_Professional,
				 Issue_Token,
				 Scheme_Confirm,
				 Progress_Status,
				 Update_By,
				 Update_Dtm,
				 Data_Input_By)

	VALUES		 (@enrolment_ref_no,
				  @sp_id,
				  @upd_sp_info,
			 	  @upd_bank_account,
				  @upd_professional,
				  @issue_token,
				  @scheme_confirm,
				  @progress_status,
				  @update_by,
				  @current_dtm,
				  @data_input_by)
END
GO

GRANT EXECUTE ON [dbo].[proc_SPAccountUpdate_add] TO HCVU
GO
