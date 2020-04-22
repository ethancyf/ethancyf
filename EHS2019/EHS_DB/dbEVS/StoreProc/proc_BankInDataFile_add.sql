IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_BankInDataFile_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_BankInDataFile_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:			Clark Yip
-- Create date:		15 May 2008
-- Description:		Insert record in BankInDataFile table
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE 	[dbo].[proc_BankInDataFile_add]	@reimburse_id	char(15)
							,@seq_num		 int
							,@Second_Party_Identifier	char(12)
							,@Second_Party_Bank_Acc_Name	char(20)
							,@Second_Party_Bank_No	char(3)
							,@Second_Party_Branch_No	char(3)
							,@Second_Party_Account	char(9)
							,@Amount	char(10)
							,@Second_Party_ID_Continuation	char(6)
							,@Second_Party_Reference	char(12)
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


	--Insert record in BankInDataFile table
	INSERT INTO BankInDataFile
           ([Reimburse_ID]
           ,[Record_Seq]
           ,[FILLER]
           ,[Second_Party_Identifier]
           ,[Second_Party_Bank_Acc_Name]
           ,[Second_Party_Bank_No]
           ,[Second_Party_Branch_No]
           ,[Second_Party_Account]
           ,[Amount]
           ,[Filler_2]
           ,[Second_Party_ID_Continuation]
           ,[Second_Party_Reference])
     VALUES
           (@reimburse_id
			,@seq_num
			,' '
			,@Second_Party_Identifier
			,@Second_Party_Bank_Acc_Name
			,@Second_Party_Bank_No
			,@Second_Party_Branch_No
			,@Second_Party_Account
			,@Amount
			,'    '
			,@Second_Party_ID_Continuation
			,@Second_Party_Reference)

END
GO

GRANT EXECUTE ON [dbo].[proc_BankInDataFile_add] TO HCVU
GO
