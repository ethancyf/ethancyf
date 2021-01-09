IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_eHASubsidizeWriteOff_del]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[proc_eHASubsidizeWriteOff_del]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR# :			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Modification History
-- CR No:			INT18-0007
-- Modified by:		Chris YIM
-- Modified date:	21 Jun 2018
-- Description:		Fix logic on checking TSMP
-- =============================================
-- =============================================
-- Modification History
-- CR No:			INT14-0007
-- Modified by:		Koala CHENG
-- Modified date:	28 Apr 2014
-- Description:		Grant PUBLIC right to allow HCVR to regenerate write off record
-- =============================================
-- =============================================
-- Author:		Karl LAM
-- Create date: 23 Jul 2013
-- Description:	Delete write off record
-- =============================================

CREATE PROCEDURE [dbo].[proc_eHASubsidizeWriteOff_del] 
@Doc_Code		char(20),
@Doc_ID			varchar(20),
@DOB			datetime,
@Exact_DOB		char(1),
@Scheme_Code	char(10),
@Subsidize_Code	char(10),
@Scheme_Seq		smallint,
@TSMP			timestamp

AS
BEGIN

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
-- =============================================
EXEC [proc_SymmetricKey_open]

IF (SELECT COUNT(1) FROM eHASubsidizeWriteOff 
	WHERE	Doc_Code = @Doc_Code
	AND		Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @Doc_ID)
	AND		 [DOB] = @DOB AND  
			(Exact_DOB = @Exact_DOB OR   
			(@Exact_DOB = 'Y' AND (Exact_DOB = 'R' OR Exact_DOB = 'V')) OR  
			(@Exact_DOB = 'M' AND  Exact_DOB = 'U') OR  
			(@Exact_DOB = 'D' AND  Exact_DOB = 'T') )
	AND		Scheme_Code = @Scheme_Code
	AND		Subsidize_Code = @Subsidize_Code
	AND		Scheme_Seq = @Scheme_Seq
	AND		TSMP = @tsmp) = 0
BEGIN  
 RAISERROR('00011',16,1)  
 RETURN @@ERROR  
END  
-- =============================================
-- Process
-- =============================================
	DELETE
	FROM	eHASubsidizeWriteOff
	WHERE	Doc_Code = @Doc_Code
	AND		Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @Doc_ID)
	AND		 [DOB] = @DOB AND  
			(Exact_DOB = @Exact_DOB OR   
			(@Exact_DOB = 'Y' AND (Exact_DOB = 'R' OR Exact_DOB = 'V')) OR  
			(@Exact_DOB = 'M' AND  Exact_DOB = 'U') OR  
			(@Exact_DOB = 'D' AND  Exact_DOB = 'T') )
	AND		Scheme_Code = @Scheme_Code
	AND		Subsidize_Code = @Subsidize_Code
	AND		Scheme_Seq = @Scheme_Seq
	AND		TSMP = @tsmp
	
EXEC [proc_SymmetricKey_close]

END
GO

GRANT EXECUTE ON [dbo].[proc_eHASubsidizeWriteOff_del] TO HCPUBLIC
GO
GRANT EXECUTE ON [dbo].[proc_eHASubsidizeWriteOff_del] TO HCSP
GO
GRANT EXECUTE ON [dbo].[proc_eHASubsidizeWriteOff_del] TO HCVU
GO
GRANT EXECUTE ON [dbo].[proc_eHASubsidizeWriteOff_del] TO WSEXT
GO


