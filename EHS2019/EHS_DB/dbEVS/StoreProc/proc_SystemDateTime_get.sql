IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemDateTime_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemDateTime_get]
GO
-- =============================================
-- Modification History
-- CR No.:			CRE12-001
-- Modified by:		Tony FUNG
-- Modified date:	07 Feb 2012
-- Description:		1. Grant permission to WSINT for PCDInterface
-- =============================================

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure proc_SystemDateTime_get
as

select getdate() DateTime

GO

GRANT EXECUTE ON [dbo].[proc_SystemDateTime_get] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_SystemDateTime_get] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SystemDateTime_get] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_SystemDateTime_get] TO WSEXT
Go

GRANT EXECUTE ON [dbo].[proc_SystemDateTime_get] TO WSINT
Go