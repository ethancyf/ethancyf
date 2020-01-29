IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PersonalInfomation_upd_RecordStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PersonalInfomation_upd_RecordStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 20 May 2008
-- Description:	Change Record Status of PersonalInformation
--				while Voucher Account Amendment
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	05 Oct 2009
-- Description:		Add Doc_Code
-- =============================================
CREATE PROCEDURE [dbo].[proc_PersonalInfomation_upd_RecordStatus]
	@Voucher_Acc_ID char(15),
	@Update_by varchar(20),
	@Record_Status char(1),
	@doc_code char(20),
	@TSMP timestamp
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	if (select TSMP from PersonalInformation 
		where Voucher_Acc_ID = @Voucher_Acc_ID
		and doc_code = @doc_code) != @TSMP
	begin
		Raiserror('00011', 16, 1)
		return @@error
	end
-- =============================================
-- Initialization
-- =============================================
    Update PersonalInformation
	set Record_Status = @Record_Status,
		Update_by = @Update_by,
		Update_Dtm = getdate()
	where Voucher_Acc_ID = @Voucher_Acc_ID
	and doc_code = @doc_code
		
END
GO

GRANT EXECUTE ON [dbo].[proc_PersonalInfomation_upd_RecordStatus] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_PersonalInfomation_upd_RecordStatus] TO HCVU
GO
