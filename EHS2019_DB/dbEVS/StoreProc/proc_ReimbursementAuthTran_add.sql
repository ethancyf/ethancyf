IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementAuthTran_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementAuthTran_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO





-- =============================================
-- Author:			Clark Yip
-- Create date:		15 May 2008
-- Description:		Insert record in ReimbursementAuthTran table for 1st authorisation
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE 	[dbo].[proc_ReimbursementAuthTran_add]	@tran_id	char(20)
					,@authorised_dtm datetime
					,@authorised_by varchar(20)
					,@reimburse_id char(15)
					,@authorised_status char(1)
as
BEGIN
-- =============================================
-- Declaration
-- ============================================= 

-- =============================================
-- Validation 
-- =============================================

--if (select count(1) from ReimbursementAuthTran 
--	where Transaction_ID = @tran_id) > 0
--begin
--	Raiserror('600002', 16, 1)
--end

-- =============================================
-- Initialization
-- =============================================

-- =============================================
-- Return results
-- =============================================

	-- Insert into ReimbursementAuthTran
	INSERT INTO ReimbursementAuthTran
           ([Transaction_ID]
           ,[First_Authorised_Dtm]
           ,[First_Authorised_By]           
           ,[Authorised_Status]
		   ,[Reimburse_id])
     VALUES
           (@tran_id
           ,@authorised_dtm
		   ,@authorised_by
           ,@authorised_status
		   ,@reimburse_id)

END
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementAuthTran_add] TO HCVU
GO
