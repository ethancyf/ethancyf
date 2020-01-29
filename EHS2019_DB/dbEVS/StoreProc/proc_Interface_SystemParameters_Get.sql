IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Interface_SystemParameters_Get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Interface_SystemParameters_Get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	18 November 2011
-- Description:		After FGS reference by HCSP, 
--					common function in dbEVS_Interfacelog require to grant right to HCSP
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		5 October 2010
-- Description:		Get SystemParameters
-- =============================================
CREATE PROCEDURE [dbo].[proc_Interface_SystemParameters_Get]
AS BEGIN
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

	OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key

	SELECT
		Parameter_Name,
		Scheme_Code,
		Parm_Value,
		CONVERT(varchar, DecryptByKey(Parm_Value_Encrypt)) AS [Parm_Value_Encrypt],
		Description,
		Category,
		Record_Status,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By
	FROM
		SystemParameters
	WHERE
		Record_Status = 'A'

	CLOSE SYMMETRIC KEY sym_Key

END
GO

GRANT EXECUTE ON [dbo].[proc_Interface_SystemParameters_Get] TO WSEXT
GO

GRANT EXECUTE ON [dbo].[proc_Interface_SystemParameters_Get] TO HCSP
GO
