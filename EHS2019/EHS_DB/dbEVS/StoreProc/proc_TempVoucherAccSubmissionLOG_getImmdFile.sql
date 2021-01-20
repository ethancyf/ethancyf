IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccSubmissionLOG_getImmdFile]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccSubmissionLOG_getImmdFile]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- CR# :			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	30 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 26 Aug 2008
-- Description:	Get Prepared Record to generate ImmD File 
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Dedrick Ng
-- Modified date: 30 Sep 2009
-- Description:	Remove obsolete fields
-- =============================================
CREATE PROCEDURE [dbo].[proc_TempVoucherAccSubmissionLOG_getImmdFile]
	@File_Name varchar(100)
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================


--DECLARE @curDate as datetime
--Set @curDate = GetDate()

--DECLARE @blnPrepareRecord as tinyint
--Set @blnPrepareRecord = 1

--DECLARE @tempStatus as char(1)

EXEC [proc_SymmetricKey_open]
	
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
			TVASL.System_Dtm,
			TVASL.Voucher_Acc_ID as [seqNo],
			(CASE WHEN LEN(LTRIM(RTRIM(CONVERT(char,DecryptByKey(Encrypt_Field1))))) = 8 THEN ' '
				WHEN LEN(LTRIM(RTRIM(CONVERT(char,DecryptByKey(Encrypt_Field1))))) = 9 THEN LEFT(LTRIM(RTRIM(CONVERT(char,DecryptByKey(Encrypt_Field1)))),1) END) as [icPrf],
			(CASE WHEN LEN(LTRIM(RTRIM(CONVERT(char,DecryptByKey(Encrypt_Field1))))) = 8 THEN LEFT(LTRIM(RTRIM(CONVERT(char,DecryptByKey(Encrypt_Field1)))),7)
				WHEN LEN(LTRIM(RTRIM(CONVERT(char,DecryptByKey(Encrypt_Field1))))) = 9 THEN RIGHT(LEFT(LTRIM(RTRIM(CONVERT(char,DecryptByKey(Encrypt_Field1)))),8),7) END) as [icNo],
			Date_of_Issue as [dtIcReg],
			DOB as [dobOnCard],
			--IsHKID,	
			TVASL.Doc_Code,
			CASE WHEN EC_Reference_No IS NULL THEN NULL
			ELSE LEFT(LTRIM(RTRIM(EC_Reference_No)),13) END as [applnRefNo], 
			EC_Serial_No as [audNo],
			
			--CASE WHEN IsHKID = 'N' THEN
			CASE WHEN TVASL.Doc_Code = 'EC' THEN
				CASE WHEN EC_DOB_Type = 'Y' THEN '1'
					WHEN EC_DOB_Type = 'M' THEN '1'
					WHEN EC_DOB_Type = 'D' THEN '1'
					WHEN EC_DOB_Type = 'R' THEN '2'
					WHEN EC_DOB_Type = 'A' THEN '3' END
			ELSE '' END as [dobFlag],
			
			--CASE WHEN IsHKID = 'N' AND (EC_DOB_Type = 'Y' OR EC_DOB_Type = 'M' OR EC_DOB_Type = 'D') THEN DOB
			CASE WHEN TVASL.Doc_Code = 'EC' AND (EC_DOB_Type = 'Y' OR EC_DOB_Type = 'M' OR EC_DOB_Type = 'D') THEN DOB
			ELSE '' END as [appltDOB],
			
			--CASE WHEN IsHKID = 'N' AND EC_DOB_Type = 'R' THEN DOB
			CASE WHEN TVASL.Doc_Code = 'EC' AND EC_DOB_Type = 'R' THEN DOB
			ELSE '' END as [appltYOB],

			--CASE WHEN IsHKID = 'N' AND EC_DOB_Type = 'A' THEN CONVERT(varchar(4),DATEPART(YEAR,EC_Date_of_Registration)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,EC_Date_of_Registration)))) + CONVERT(varchar(2),DATEPART(MONTH,EC_Date_of_Registration)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(DAY,EC_Date_of_Registration)))) + CONVERT(varchar(2),DATEPART(DAY,EC_Date_of_Registration))
			CASE WHEN TVASL.Doc_Code = 'EC' AND EC_DOB_Type = 'A' THEN CONVERT(varchar(4),DATEPART(YEAR,EC_Date_of_Registration)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,EC_Date_of_Registration)))) + CONVERT(varchar(2),DATEPART(MONTH,EC_Date_of_Registration)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(DAY,EC_Date_of_Registration)))) + CONVERT(varchar(2),DATEPART(DAY,EC_Date_of_Registration))
			ELSE '' END as [appltDtIcReg],
			
			--Case WHEN IsHKID = 'N' AND EC_DOB_Type = 'A' THEN CONVERT(varchar(3), EC_Age)				
			Case WHEN TVASL.Doc_Code = 'EC' AND EC_DOB_Type = 'A' THEN CONVERT(varchar(3), EC_Age)				
			ELSE '' END AS [ageOnDt]
		
						
		FROM [dbo].[TempVoucherAccSubmissionLOG] TVASL, [dbo].[TempVoucherAccSubHeader] TVASH
		WHERE 
			
			TVASH.[File_Name] = @File_Name 
			AND TVASL.System_Dtm = TVASH.System_Dtm
			--AND DATEPART(YEAR,TVASL.System_Dtm) = DATEPART(YEAR,TVASH.System_Dtm) AND DATEPART(MONTH,TVASL.System_Dtm) = DATEPART(MONTH,TVASH.System_Dtm) AND DATEPART(DAY,TVASL.System_Dtm) = DATEPART(DAY,TVASH.System_Dtm)
		ORDER BY Voucher_Acc_ID ASC			

EXEC [proc_SymmetricKey_close]

END

GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccSubmissionLOG_getImmdFile] TO HCVU
GO
