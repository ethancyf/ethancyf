IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSA0027]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSA0027]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- Modified by:		Helen Lam 	
-- Modified date:	27 Jan 2012
-- Description:		eHSA0027 - FHB statistics for 2011 (CRD12-002)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	1 February 2011
-- Description:		eHSA0018 - FHB statistics for 2010
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_eHSA0027]
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Report setting
-- =============================================
	DECLARE	@Year	int
	SET @Year = 2011


-- =============================================
-- Retrieve data
-- =============================================

-- Content

	EXEC [proc_EHS_eHSA0027-Content] @Year

-- 01-02 for validated HCVS-eligible eHealth Accounts 

	EXEC [proc_EHS_eHSA0027-01-02] @Year
	
-- 03-04 for transactions andn vouchers claim broken down by profession

	EXEC [proc_EHS_eHSA0027-03-04] @Year

-- 05-06 for transactioins and vouchers claim broken down by 18 districts

	EXEC [proc_EHS_eHSA0027-05-06] @Year
	
-- 07 for transactions and vouchers claim broken down by profession and reasons of visit

	EXEC [proc_EHS_eHSA0027-07] @Year

-- 08-09-10 Culmulative transactions submitted through 'Full', 'Text-Only' or IVRS,PCS version

	EXEC [proc_EHS_eHSA0027-08-09-10-11] @Year

-- 12 Voucher claim by EHCP broken down by profession

	EXEC [proc_EHS_eHSA0027-12] @Year

--13  Voucher claim by EHCP have ever used Smart ICs broken down by profession

	EXEC [proc_EHS_eHSA0027-13] @Year

-- 14 Transactions and vouchers of each EHCP (Remark: no need this sheet comment by christine)

--	EXEC [proc_EHS_eHSA0027-14] @Year

-- 15 Transactions and vouchers of each EHCP broken down by means of input

	EXEC [proc_EHS_eHSA0027-15] @Year

-- Legend

	EXEC [proc_EHS_eHSA0027-Legend] @Year

set nocount off
END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSA0027] TO HCVU
GO

