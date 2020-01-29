IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeRule_get_bySchemeCodeRuleName]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchemeRule_get_bySchemeCodeRuleName]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 28 April 2008
-- Description:	Retrieve the Voucher Scheme Rule
--				By Scheme Name and Rule
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_SchemeRule_get_bySchemeCodeRuleName]
	-- Add the parameters for the stored procedure here
	@Scheme_Code char(10),
	@Rule_Name varchar(20)
AS
BEGIN
	SET NOCOUNT ON;
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
	select Scheme_code, Rule_Name, Operator, [Value]
	from dbo.VoucherSchemeRule
	where Scheme_Code = @Scheme_Code and
	Rule_Name = @Rule_Name
	
END
GO

GRANT EXECUTE ON [dbo].[proc_SchemeRule_get_bySchemeCodeRuleName] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_SchemeRule_get_bySchemeCodeRuleName] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SchemeRule_get_bySchemeCodeRuleName] TO HCVU
GO
