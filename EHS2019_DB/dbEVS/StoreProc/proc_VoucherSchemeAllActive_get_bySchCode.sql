IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherSchemeAllActive_get_bySchCode]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherSchemeAllActive_get_bySchCode]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 08 May 2008
-- Description:	Retrieve all active scheme details
--              by using Scheme Code and target 
--				date
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	22 Jun 2009
-- Description:		Handle Scheme Change for VoucherSchemeDetail
--					Voucher -> Unit
--					Voucher_Value -> Unit_Value
-- =============================================

CREATE PROCEDURE [dbo].[proc_VoucherSchemeAllActive_get_bySchCode]
	@Scheme_Code as varchar(10)
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
	SELECT
		VS.Scheme_Code, VS.Scheme_Description, 
		VSD.sequence, VSD.Effective_Date, VSD.Expiry_Date, 
		VSD.Unit, VSD.Unit_Value
        
	From dbo.VoucherScheme VS, dbo.VoucherSchemeDetail VSD
	Where VS.Scheme_Code = @Scheme_Code and
			VS.Scheme_Code = VSD.Scheme_Code and
			VSD.Record_Status = 'A' and
            VSD.Effective_Date <= getdate()  
END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherSchemeAllActive_get_bySchCode] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_VoucherSchemeAllActive_get_bySchCode] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_VoucherSchemeAllActive_get_bySchCode] TO HCVU
GO
