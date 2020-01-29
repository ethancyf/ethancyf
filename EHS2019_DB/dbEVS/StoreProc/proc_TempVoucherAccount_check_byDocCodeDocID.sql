IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccount_check_byDocCodeDocID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccount_check_byDocCodeDocID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================  
-- Modification History  
-- CR No.:			CRE14-019
-- Modified by:		Lawrence TSANG
-- Modified date:	21 January 2015
-- Description:		Insert into [SProcPerformance] to record sproc performance
-- =============================================  
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 21 Aug 2009
-- Description:	Check if Temporary EHS Account Exist Document & Document Identity
--				For HKIC & EC Case Only
-- =============================================

CREATE PROCEDURE [dbo].[proc_TempVoucherAccount_check_byDocCodeDocID]
	@Doc_Code char(20),
	@identity varchar(20)
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
	DECLARE @Performance_Start_Dtm datetime
	SET @Performance_Start_Dtm = GETDATE()

	DECLARE @In_Doc_Code  char(20)
	DECLARE @In_Identity	varchar(20)
	SET @In_Doc_Code = @Doc_Code
	SET @In_Identity = @identity
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key
-- =============================================
-- Return results
-- =============================================

	SELECT
		TVA.[Voucher_Acc_ID]
	FROM
		[TempPersonalInformation] TPI
			INNER JOIN [TempVoucherAccount] TVA
				ON TPI.[Voucher_Acc_ID] = TVA.[Voucher_Acc_ID]			
	WHERE
		[Doc_Code] = @In_Doc_Code AND
		[Encrypt_Field1] = EncryptByKey(KEY_GUID('sym_Key'), @In_Identity) AND 
		TVA.[Record_Status] <> 'D' AND TVA.[Record_Status] <> 'V'

CLOSE SYMMETRIC KEY sym_Key

	IF (SELECT Parm_Value1 FROM SystemParameters WHERE Parameter_Name = 'EnableSProcPerformCapture' AND Scheme_Code = 'ALL') = 'Y' BEGIN
		DECLARE @Performance_End_Dtm datetime
		SET @Performance_End_Dtm = GETDATE()
		DECLARE @Parameter varchar(255)
		SET @Parameter = @In_Doc_Code + ',' + @In_Identity
		
		EXEC proc_SProcPerformance_add 'proc_TempVoucherAccount_check_byDocCodeDocID',
									   @Parameter,
									   @Performance_Start_Dtm,
									   @Performance_End_Dtm
	END
	

END
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_check_byDocCodeDocID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_check_byDocCodeDocID] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_check_byDocCodeDocID] TO WSEXT
GO