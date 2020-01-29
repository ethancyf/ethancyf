IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccountCount_get_ECNewCreate]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccountCount_get_ECNewCreate]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 30 Oct 2008
-- Description:	Retrieve Count of EC TempVoucherAccount newlly created
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	 Clark YIP
-- Modified date: 30 Sep 2009
-- Description:	  Update base on new schema
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	 Clark YIP
-- Modified date: 15 Oct 2009
-- Description:	  Get all doc type with manual checking needed case
-- =============================================
CREATE PROCEDURE [dbo].[proc_TempVoucherAccountCount_get_ECNewCreate]
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

	SELECT Count(1) 
	FROM
		[TempVoucherAccount] TVA
			INNER JOIN [dbo].[TempPersonalInformation] TPI
				ON TVA.Voucher_Acc_ID = TPI.Voucher_Acc_ID
			INNER JOIN doctype dt
				ON TPI.doc_code = dt.doc_code collate database_default
	WHERE		
		TVA.Record_Status = 'P' AND
		(TPI.Validating ='N' OR TPI.Validating IS NULL) AND
		(TVA.Account_Purpose = 'C' OR TVA.Account_Purpose = 'V') AND
		(
			 (TPI.Doc_Code = 'ADOPC' and (TPI.Exact_DOB in ('T', 'U', 'V') or DT.Force_Manual_Validate = 'Y')) or
			 (TPI.Doc_Code = 'Doc/I' and (TPI.Date_of_Issue < '1 Sep 2003' or DT.Force_Manual_Validate = 'Y')) or
			 (TPI.Doc_Code = 'EC' and (TPI.Date_of_issue < '23 Jun 2003' or TPI.Exact_DOB in ('T', 'U', 'V') or DT.Force_Manual_Validate = 'Y') ) or
			 (TPI.Doc_Code = 'HKBC' and (TPI.Exact_DOB in ('T', 'U', 'V') or DT.Force_Manual_Validate = 'Y' )) or
			 (TPI.doc_code = 'HKIC' and DT.Force_Manual_Validate = 'Y') or
			 (TPI.doc_code = 'ID235B' and DT.Force_Manual_Validate = 'Y') or
			 (TPI.doc_code = 'REPMT' and (TPI.Date_of_Issue < '4 Jun 2007' or DT.Force_Manual_Validate = 'Y')) or
			 (TPI.doc_code = 'VISA' and DT.Force_Manual_Validate = 'Y')
		)
END
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccountCount_get_ECNewCreate] TO HCVU
GO
