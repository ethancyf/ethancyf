IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCSPUserAC_get_byCodeStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCSPUserAC_get_byCodeStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:			Clark YIP
-- Create date:		15-07-2008
-- Description:		Get HCSPUserAC 
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   05-03-2009
-- Description:	    Not to check the Activation Link with the HCSPUserAC record_status
-- =============================================
CREATE procedure [dbo].[proc_HCSPUserAC_get_byCodeStatus]
@code			varchar(100),
@status			char(1)
as
BEGIN
-- =============================================
-- Return results
-- =============================================
SELECT count(1)
  FROM HCSPUserAC
where --record_status=@status AND
 Activation_code = @code
END
GO

GRANT EXECUTE ON [dbo].[proc_HCSPUserAC_get_byCodeStatus] TO HCSP
GO
