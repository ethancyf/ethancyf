IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SPAccountUpdate_del_byERN]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SPAccountUpdate_del_byERN]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Clark YIP
-- Create date: 19 Jun 2008
-- Description:	Delete the SP Account Update details
--				from Table SPAccountUpdate
-- =============================================
-- =============================================
-- Modification History: 
-- Modified by:	Pak Ho LEE
-- Modified date: 
-- Description:	Add Optional Param for Check TSMP
-- =============================================
CREATE PROCEDURE [dbo].[proc_SPAccountUpdate_del_byERN]
	@enrolment_ref_no	char(15),
	@tsmp timestamp,
	@checkTSMP tinyint
AS
BEGIN
	
	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
if @checkTSMP = 1 AND (select tsmp from SPAccountUpdate
		where Enrolment_Ref_No = @enrolment_ref_no) != @tsmp
begin
	Raiserror('00011', 16, 1)
	return @@error
end
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================
	DELETE	SPAccountUpdate
	WHERE	Enrolment_Ref_No = @enrolment_ref_no
	
END
GO

GRANT EXECUTE ON [dbo].[proc_SPAccountUpdate_del_byERN] TO HCVU
GO
