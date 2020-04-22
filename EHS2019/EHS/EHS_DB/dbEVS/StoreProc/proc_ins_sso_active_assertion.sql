 IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ins_sso_active_assertion]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ins_sso_active_assertion]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			    
-- Modified by:		
-- Modified date:	    
-- Description:		
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			INT12-0008
-- Modified by:	Tommy TSE
-- Modified date:	29 Aug 2012
-- Description:	Rectify stored procedure permission
-- =============================================
-- =============================================
-- Author:		    Paul Yip
-- Create date:   29 Dec 2009
-- Description:	Insert Single Logon Active Assertion 
-- =============================================

CREATE PROCEDURE [dbo].[proc_ins_sso_active_assertion] 
	@v_in_txn_id VARCHAR(255), 
	@v_in_artifact VARCHAR(255), 
	@v_in_assertion TEXT,
	@v_in_read_count INT,
	@v_in_creation_datetime DATETIME 
AS
BEGIN

SET NOCOUNT ON;

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

INSERT INTO sso_active_assertion (
	txn_id,
	artifact, 
	assertion,
	read_count,
	creation_datetime
) VALUES (
	@v_in_txn_id,
	@v_in_artifact, 
	@v_in_assertion,
	@v_in_read_count,
	@v_in_creation_datetime

);
 

END 

GO

GRANT EXECUTE ON [dbo].[proc_ins_sso_active_assertion] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_ins_sso_active_assertion] TO HCVU
GO
