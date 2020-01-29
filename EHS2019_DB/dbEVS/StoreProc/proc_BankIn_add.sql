IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_BankIn_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_BankIn_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:			Clark Yip
-- Create date:		15 May 2008
-- Description:		Insert record in BankIn table
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE 	[dbo].[proc_BankIn_add]	@reimbursement_id	char(15)
					,@submission_dtm datetime
					,@submitted_by	varchar(20)
					,@trans_count int
                    ,@vouchers_count Int
                    ,@total_amt Money
                    ,@bank_payment_dtm datetime
as
BEGIN
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

	--Insert record in BankIn table
	INSERT INTO BankIn
           ([Reimburse_ID]
           ,[Submission_Dtm]
           ,[Submitted_By]
			,[Transaction_Count]
			,[Vouchers_Count]
			,[Total_Amount]
			,[Value_Date])
          
     VALUES
           (@reimbursement_id
           ,@submission_dtm
           ,@submitted_by
			,@trans_count
			,@vouchers_count
			,@total_amt
			,@bank_payment_dtm)
END
GO

GRANT EXECUTE ON [dbo].[proc_BankIn_add] TO HCVU
GO
