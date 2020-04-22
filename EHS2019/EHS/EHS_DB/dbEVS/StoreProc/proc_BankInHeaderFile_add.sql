IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_BankInHeaderFile_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_BankInHeaderFile_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:			Clark Yip
-- Create date:		15 May 2008
-- Description:		Insert record in BankInHeaderFile table
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE 	[dbo].[proc_BankInHeaderFile_add]	@reimburse_id	char(15)
							,@Auto_Plan_Code	 char(1)
							,@First_Party_Acc_No char(12)
							,@Payment_Code		 char(3)
							,@First_Party_Reference	char(12)
							,@Value_Date			char(6)						
							,@File_Name				char(8)
							,@Total_No_Instruction	char(5)
							,@Total_Amt_Instruction char(10)
							,@Overflow_Total_No_Instruction	char(7)
							,@Overflow_Total_Amt_Instruction	char(12)				
           
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


	--Insert record in BankInHeaderFile table
	INSERT INTO [BankInHeaderFile]
           ([Reimburse_ID]
           ,[Auto_Plan_Code]
           ,[First_Party_Acc_No]
           ,[Payment_Code]
           ,[First_Party_Reference]
           ,[Value_Date]
           ,[Input_Medium]
           ,[File_Name]
           ,[Total_No_Instruction]
           ,[Total_Amt_Instruction]
           ,[Overflow_Total_No_Instruction]
           ,[Overflow_Total_Amt_Instruction]
           ,[FILLER]
           ,[Instruction_Source])
     VALUES
           (@reimburse_id
           ,@Auto_Plan_Code
			,@First_Party_Acc_No
			,@Payment_Code
			,@First_Party_Reference
			,@Value_Date
			,'K'
			,@File_Name
			,@Total_No_Instruction
			,@Total_Amt_Instruction
			,@Overflow_Total_No_Instruction
			,@Overflow_Total_Amt_Instruction
			,'  '
			,'1')
END
GO

GRANT EXECUTE ON [dbo].[proc_BankInHeaderFile_add] TO HCVU
GO
