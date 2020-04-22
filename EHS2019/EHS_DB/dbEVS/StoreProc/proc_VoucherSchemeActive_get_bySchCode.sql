IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherSchemeActive_get_bySchCode]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherSchemeActive_get_bySchCode]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 23 April 2008
-- Description:	Retrieve the active scheme details
--              by using Scheme Code and target 
--				date
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Paul Yip
-- Modified date: 20 May 2009
-- Description:	Add "Scheme_Display_Name", "Scheme_Detail_Display_Name"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Pak Ho LEE
-- Modified date: 22 Jun 2009
-- Description:	Handle Scheme Change for VoucherSchemeDetail
--				Voucher -> Unit
--				Voucher_Value -> Unit_Value
--				Scheme_Display_Name -> Scheme_Display_Code
--				Add "Scheme_Detail_Display_Name_Chi"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Pak Ho LEE
-- Modified date: 14 Aug 2009
-- Description:	Handle Scheme Change for VoucherSchemeDetail
--				Remove "Scheme_Detail_Display_Name_Chi"
-- =============================================

CREATE PROCEDURE [dbo].[proc_VoucherSchemeActive_get_bySchCode]
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
			VS.Scheme_Code, VS.Scheme_Description, VS.Scheme_Description_Chi, VSD.sequence, 
			VS.Scheme_Display_Code,
			VSD.Effective_Date, VSD.Expiry_Date, VSD.Unit, 
            VSD.Unit_Value, VSD.Scheme_Detail_Display_Name
	From dbo.VoucherScheme VS, dbo.VoucherSchemeDetail VSD
	Where VS.Scheme_Code = @Scheme_Code and
			VS.Scheme_Code = VSD.Scheme_Code and
			VSD.Record_Status = 'A' and
            getdate() between VSD.Effective_Date and VSD.Expiry_Date
END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherSchemeActive_get_bySchCode] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_VoucherSchemeActive_get_bySchCode] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_VoucherSchemeActive_get_bySchCode] TO HCVU
GO
