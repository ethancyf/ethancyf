   if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[proc_TempVoucherAccSubmissionLOG_getImmdFileOtherDoc]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[proc_TempVoucherAccSubmissionLOG_getImmdFileOtherDoc]
GO

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

-- =============================================
-- Modification History
-- CR No.:			CRE20-023
-- Modified by:		Martin Tang
-- Modified date:	20 Apr 2021
-- Description:		Extend patient name's maximum length
-- =============================================
-- =============================================
-- Modification History
-- CR# :			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	30 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- ============================================
-- =============================================
-- Author:		Dedrick Ng
-- Create date: 2 Nov 2009
-- Description:	Get Prepared Record to generate ImmD File 
--				(For HKBC, Doc/I, REPMT, VISA, ADOPT)
-- =============================================
CREATE PROCEDURE [dbo].[proc_TempVoucherAccSubmissionLOG_getImmdFileOtherDoc]
	@File_Name varchar(100)
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================


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
			--LTRIM(RTRIM(CONVERT(char,DecryptByKey(Encrypt_Field1)))) as [identifyNo],
			(CASE 
				WHEN LEN(LTRIM(RTRIM(CONVERT(char,DecryptByKey(Encrypt_Field1))))) = 8 AND Doc_Code = 'HKBC'
					THEN ' ' + LEFT(LTRIM(RTRIM(CONVERT(char,DecryptByKey(Encrypt_Field1)))),7)
				WHEN LEN(LTRIM(RTRIM(CONVERT(char,DecryptByKey(Encrypt_Field1))))) = 9 AND Doc_Code = 'HKBC'
					THEN LEFT(LTRIM(RTRIM(CONVERT(char,DecryptByKey(Encrypt_Field1)))),8)
				ELSE
					LTRIM(RTRIM(CONVERT(char,DecryptByKey(Encrypt_Field1))))
			END) 
			as [identifyNo],	
			Date_of_Issue as [dtIcReg],
			DOB as [dobOnCard],
			LTRIM(RTRIM(CONVERT(varchar(100),DecryptByKey(Encrypt_Field2)))) as [nameOnCard],
			Sex as [sexOnCard],
			--TVASL.App_Seq_No,
			TVASL.Doc_Code
			
		
						
		FROM [dbo].[TempVoucherAccSubmissionLOG] TVASL, [dbo].[TempVoucherAccSubHeader] TVASH
		WHERE 
			
			TVASH.[File_Name] = @File_Name 
			AND TVASL.File_Name = TVASH.File_Name
			--AND TVASL.Doc_Code = TVASH.Doc_Code
		ORDER BY Voucher_Acc_ID ASC			

EXEC [proc_SymmetricKey_close]

END
GO

--Grant Access Right to user
--HCVU (for voucher unit platform)
--HCSP (for service provider platform)
--HCPUBLIC (for public access platform)
Grant execute on [dbo].[proc_TempVoucherAccSubmissionLOG_getImmdFileOtherDoc] to HCVU
GO     