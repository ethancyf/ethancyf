IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TSWPatientMapping_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TSWPatientMapping_add]
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
-- Author:			Clark Yip
-- Create date:		3 Oct 2008
-- Description:		Add the content in TSWPatientMapping
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   16 Jan 2009
-- Description:	    Filter out the delisted SP
-- =============================================
CREATE PROCEDURE [dbo].[proc_TSWPatientMapping_add]
	@GP_Reg_Code varchar(15),
	@HKIC char(9)
	
AS
BEGIN

-- =============================================
-- Declaration
-- =============================================
declare @SPID char(8)
--select @SPID=SP_ID from Professional where
--Registration_Code=@GP_Reg_Code
select @SPID=p.sp_id from Professional p, serviceprovider sp where
p.sp_id=sp.sp_id and p.Registration_Code=@GP_Reg_Code
and sp.Record_Status<>'D'
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
EXEC [proc_SymmetricKey_open]
-- =============================================
-- Return results
-- =============================================
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO [TSWPatientMapping] 
		([GP_Registration_Code],[GP_SPID],[Encrypt_Field1])
		VALUES(@GP_Reg_Code,isnull(@SPID,''), EncryptByKey(KEY_GUID('sym_Key'), @HKIC))
	EXEC [proc_SymmetricKey_close]
END
GO

GRANT EXECUTE ON [dbo].[proc_TSWPatientMapping_add] TO HCVU
GO
