IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransaction_check_duplicate]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransaction_check_duplicate]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE19-006 (DHC)
-- Modified by:		Winnie SUEN
-- Modified date:	24 Jun 2019
-- Description:		Handle check Total Amount with HKD or RMB
-- =============================================
-- =============================================
-- Author:		Winnie SUEN
-- CR No.:		CRE16-007 (Pop-up message to avoid duplicate voucher claim)
-- Create date: 17 Nov 2016
-- Description:	Check If duplicate claim transactions are made
-- =============================================
CREATE PROCEDURE [dbo].[proc_VoucherTransaction_check_duplicate]
	@SP_ID					CHAR(8),
	@Practice_Display_Seq	SMALLINT,
	@Doc_Code				CHAR(20),
	@Identity				VARCHAR(20),
	@Scheme_Code			CHAR(10),
	@Service_Receive_Dtm	DATETIME,
	@Total_Amount			MONEY,
	@Total_Amount_RMB		MONEY,
	@Check_Minute			INT
AS
BEGIN
	SET NOCOUNT ON;

-- =============================================    
-- Declaration    
-- =============================================    
	DECLARE @In_SP_ID					CHAR(8)
	DECLARE @In_Practice_Display_Seq	SMALLINT
	DECLARE @In_Doc_Code				CHAR(20)
	DECLARE @In_Identity				VARCHAR(20)	
	DECLARE @In_Scheme_Code				CHAR(10)
	DECLARE @In_Service_Receive_Dtm		DATETIME
	DECLARE @In_Total_Amount			MONEY
	DECLARE @In_Total_Amount_RMB		MONEY
	
	DECLARE @Check_Second				INT
	   
-- =============================================    
-- Validation     
-- =============================================    
-- =============================================    
-- Initialization    
-- =============================================    
	SET @In_SP_ID = @SP_ID
	SET @In_Practice_Display_Seq = @Practice_Display_Seq
	SET @In_Doc_Code = @Doc_Code  
	SET @In_Identity = @Identity  
	SET @In_Scheme_Code	= @Scheme_Code
	SET @In_Service_Receive_Dtm = @Service_Receive_Dtm
	SET @In_Total_Amount = @Total_Amount
	SET @In_Total_Amount_RMB = @Total_Amount_RMB
 
	
    SET @Check_Second = @Check_Minute * 60 -- convert to second
    
    -- Consider HKBC as HKIC
    IF @In_Doc_Code = 'HKBC'
    BEGIN
		SET @In_Doc_Code = 'HKIC'
    END
-- =============================================    
-- Return results    
-- =============================================    

	EXEC [proc_SymmetricKey_open]  

	-- Retrieve Transaction Related to the [Voucher_Acc_ID](s)    
	SELECT TOP 1 VT.Transaction_ID
	FROM  VoucherTransaction VT WITH(NOLOCK)
	INNER JOIN TransactionDetail TD ON VT.Transaction_ID = TD.Transaction_ID
	WHERE 
		VT.SP_ID = @In_SP_ID
		AND (@In_Practice_Display_Seq IS NULL OR VT.Practice_Display_Seq = @In_Practice_Display_Seq)
		AND VT.Scheme_Code = @In_Scheme_Code
		AND VT.Service_Receive_Dtm = @In_Service_Receive_Dtm		
		AND (( @In_Total_Amount_RMB IS NULL AND  TD.Total_Amount = @In_Total_Amount)
			OR (@In_Total_Amount_RMB IS NOT NULL AND  TD.Total_Amount_RMB = @In_Total_Amount_RMB))
		AND DATEDIFF(SECOND, VT.Transaction_Dtm, GETDATE()) < @Check_Second
		AND VT.Record_Status NOT IN ('I','D','W')
		AND VT.Invalidation IS NULL
		AND EXISTS (
					SELECT PI.Voucher_Acc_ID
					FROM PersonalInformation PI 
					WHERE 
						PI.Voucher_Acc_ID = VT.Voucher_Acc_ID AND PI.Doc_Code = VT.Doc_Code
						AND PI.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @In_Identity)
						AND CASE PI.Doc_Code WHEN 'HKBC' THEN 'HKIC' ELSE PI.Doc_Code END = @In_Doc_Code
					
					UNION
					SELECT TPI.Voucher_Acc_ID
					FROM TempPersonalInformation TPI 
					WHERE 
						TPI.Voucher_Acc_ID = VT.Temp_Voucher_Acc_ID
						AND TPI.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @In_Identity)
						AND CASE TPI.Doc_Code WHEN 'HKBC' THEN 'HKIC' ELSE TPI.Doc_Code END = @In_Doc_Code
					)
			 
		
	EXEC [proc_SymmetricKey_close]		 
		
END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransaction_check_duplicate] TO HCSP
GO
