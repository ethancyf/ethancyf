 IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemDateTime_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemDateTime_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	10 July 2017
-- CR No.			I-CRE16-007-02 (Refine system from CheckMarx findings)
-- Description:		Grant Execute to HCVU, HCSP
-- =============================================
-- =============================================
-- Author:		Tommy TSE
-- Create date: 22 Sep 2011
-- CR No.:		CRE11-024-02 (Enhancement on HCVS Extension Part 2)
-- Description:	Retrieve DB System DateTime
-- =============================================


CREATE procedure proc_SystemDateTime_get
as

select getdate() DateTime

GO

GRANT EXECUTE ON [dbo].[proc_SystemDateTime_get] TO HCVU, HCSP, WSEXT, WSINT
Go