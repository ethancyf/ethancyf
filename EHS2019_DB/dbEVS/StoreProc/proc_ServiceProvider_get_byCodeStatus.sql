IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProvider_get_byCodeStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProvider_get_byCodeStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Clark YIP
-- Create date: 15 July 2008
-- Description:	Retrieve the Service Provider Information
--				from Table "ServiceProvider"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	  Clark YIP 
-- Modified date: 23 Jan 2009
-- Description:	  Remove to check SP status in the link validation stage
-- =============================================
CREATE PROCEDURE [dbo].[proc_ServiceProvider_get_byCodeStatus]
	@code	varchar(100)
	,@status	char(1)
AS
BEGIN
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
	SELECT count(1)
	FROM ServiceProvider
	where 
	--record_status=@status AND
	Activation_code = @code
	AND Activation_code is not null
	AND Tentative_Email is not null
END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProvider_get_byCodeStatus] TO HCSP
GO
