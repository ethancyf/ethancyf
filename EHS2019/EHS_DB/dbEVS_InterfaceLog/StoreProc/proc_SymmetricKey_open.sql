IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SymmetricKey_open]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SymmetricKey_open]
GO
-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005 (To Improve database performance in eHS(S))
-- Modified by:		Winnie SUEN
-- Modified date:	04 Dec 2020
-- Description:		Performance Tuning
--					1. Use dynamic sql to open key to reduce compile lock
--					2. Centralize the method to open & close key
-- =============================================

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[proc_SymmetricKey_open]
AS
BEGIN
	EXEC sp_executesql N'OPEN SYMMETRIC KEY sym_Key DECRYPTION BY ASYMMETRIC KEY asym_Key';
END
GO

GRANT EXECUTE ON [dbo].[proc_SymmetricKey_open] TO HCSP, HCVU, WSEXT, WSINT
GO
