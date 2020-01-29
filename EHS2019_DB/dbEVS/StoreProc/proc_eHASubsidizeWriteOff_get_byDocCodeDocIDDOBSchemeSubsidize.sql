IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_eHASubsidizeWriteOff_get_byDocCodeDocIDDOBSchemeSubsidize]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[proc_eHASubsidizeWriteOff_get_byDocCodeDocIDDOBSchemeSubsidize]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE14-016 (To introduce 'Deceased' status into eHS)
-- Modified by:	    Winnie SUEN
-- Modified date:   23 Nov 2017
-- Description:		Add [PValue_TotalRefund], [PValue_SeasonRefund]
-- =============================================
-- =============================================  
-- Modification History  
-- CR No.:			CRE14-019
-- Modified by:		Lawrence TSANG
-- Modified date:	21 January 2015
-- Description:		Insert into [SProcPerformance] to record sproc performance
-- =============================================  
-- =============================================
-- Author:		Karl LAM
-- Create date: 23 Jul 2013
-- Description:	Retrieve the Write Off information of the specific eHA (same docCode docID DOB)
-- =============================================

CREATE PROCEDURE [dbo].[proc_eHASubsidizeWriteOff_get_byDocCodeDocIDDOBSchemeSubsidize] 
@Doc_Code	char(20),
@Doc_ID		varchar(20),
@DOB		datetime,
@Exact_DOB	char(1),
@Scheme_Code char(10),
@Subsidize_Code	char(10)

AS
BEGIN

	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
	DECLARE @Performance_Start_Dtm datetime
	SET @Performance_Start_Dtm = GETDATE()

	DECLARE @In_Doc_Code			char(20)
	DECLARE @In_Doc_ID			varchar(20)
	DECLARE @In_DOB				datetime
	DECLARE @In_Exact_DOB			char(1)
	DECLARE @In_Scheme_Code		char(10)
	DECLARE @In_Subsidize_Code	char(10)
	SET @In_Doc_Code = @Doc_Code
	SET @In_Doc_ID = @Doc_ID
	SET @In_DOB = @DOB
	SET @In_Exact_DOB = @Exact_DOB
	SET @In_Scheme_Code = @Scheme_Code
	SET @In_Subsidize_Code = @Subsidize_Code
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================
OPEN SYMMETRIC KEY sym_Key
DECRYPTION BY ASYMMETRIC KEY asym_Key

	SELECT 	Doc_Code,
			Doc_ID = CONVERT(varchar(20), DecryptByKey(Encrypt_Field1)),  
			DOB,
			Exact_DOB,
			Scheme_Code,
			Scheme_Seq,
			Subsidize_Code,
			WriteOff_Unit,
			WriteOff_Per_Unit_Value,
			PValue_Ceiling,
			PValue_TotalEntitlement,
			PValue_SeasonEntitlement,
			PValue_TotalUsed,
			PValue_SeasonUsed,
			PValue_Available,						
			Create_Dtm,
			Create_Reason,			
			TSMP,
			PValue_TotalRefund,
			PValue_SeasonRefund
	FROM	eHASubsidizeWriteOff
	WHERE	Doc_Code = @In_Doc_Code
	AND		Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @In_Doc_ID)
	AND		 [DOB] = @In_DOB 
	AND  	Exact_DOB = @In_Exact_DOB 
	AND		(Scheme_Code = @In_Scheme_Code or isnull(@In_Scheme_Code,'') = '')
	AND		(Subsidize_Code = @In_Subsidize_Code or isnull(@In_Subsidize_Code,'') = '')
	ORDER BY Scheme_Code,Subsidize_Code, Scheme_Seq ASC
	
CLOSE SYMMETRIC KEY sym_Key

	IF (SELECT Parm_Value1 FROM SystemParameters WHERE Parameter_Name = 'EnableSProcPerformCapture' AND Scheme_Code = 'ALL') = 'Y' BEGIN
		DECLARE @Performance_End_Dtm datetime
		SET @Performance_End_Dtm = GETDATE()
		DECLARE @Parameter varchar(255)
		SET @Parameter = @In_Doc_Code + ',' + @In_Doc_ID + ',' + CONVERT(varchar, @In_DOB, 120) + ','
						 + @In_Exact_DOB + ',' + @In_Scheme_Code + ',' + @In_Subsidize_Code
		
		EXEC proc_SProcPerformance_add 'proc_eHASubsidizeWriteOff_get_byDocCodeDocIDDOBSchemeSubsidize',
									   @Parameter,
									   @Performance_Start_Dtm,
									   @Performance_End_Dtm
	END
	
END
GO

GRANT EXECUTE ON [dbo].[proc_eHASubsidizeWriteOff_get_byDocCodeDocIDDOBSchemeSubsidize] TO HCPUBLIC
GO
GRANT EXECUTE ON [dbo].[proc_eHASubsidizeWriteOff_get_byDocCodeDocIDDOBSchemeSubsidize] TO HCSP
GO
GRANT EXECUTE ON [dbo].[proc_eHASubsidizeWriteOff_get_byDocCodeDocIDDOBSchemeSubsidize] TO HCVU
GO
GRANT EXECUTE ON [dbo].[proc_eHASubsidizeWriteOff_get_byDocCodeDocIDDOBSchemeSubsidize] TO WSEXT
GO

