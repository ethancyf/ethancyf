IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_BankInFile_getByReimbuserID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_BankInFile_getByReimbuserID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:			Clark Yip
-- Create date:		1 Aug 2008
-- Description:		Get record for BankIn file
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   19 Aug 2009
-- Description:	    Add Scheme Code
-- =============================================

CREATE PROCEDURE 	[dbo].[proc_BankInFile_getByReimbuserID]	
						@reimburse_id	char(15),
						@scheme_code	char(10)
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

	--Select record from BankIn tables
	select h.Auto_Plan_Code, h.First_Party_Acc_No, h.Payment_Code, h.First_Party_Reference, h.value_date, h.Input_Medium,
		h.[File_Name], h.Total_No_Instruction, h.Total_Amt_Instruction, h.Overflow_Total_No_Instruction,
		h.Overflow_Total_Amt_Instruction, h.Filler, h.Instruction_Source				
		from bankinheaderfile h 
		where h.reimburse_id = @reimburse_id AND h.Scheme_Code=@scheme_Code

select  d.filler, d.Second_Party_Identifier, d.Second_Party_Bank_Acc_Name, d.Second_Party_Bank_No, d.Second_Party_Branch_No,
		d.Second_Party_Account, d.Amount, d.Filler_2, d.Second_Party_ID_Continuation, d.Second_Party_Reference	
		from bankinDatafile d
		where d.reimburse_id = @reimburse_id AND d.Scheme_Code=@scheme_Code
		Order by d.Second_Party_Identifier

END
GO

GRANT EXECUTE ON [dbo].[proc_BankInFile_getByReimbuserID] TO HCVU
GO
