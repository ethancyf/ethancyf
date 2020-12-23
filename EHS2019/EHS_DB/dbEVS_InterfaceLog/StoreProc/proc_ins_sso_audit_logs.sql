 IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ins_sso_audit_logs]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ins_sso_audit_logs]
GO

SET ANSI_NULLS ON
GO
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
-- Author:  Paul Yip  
-- Create date: 29 Dec 2009  
-- Description:   
-- =============================================  
  
CREATE PROCEDURE [dbo].[proc_ins_sso_audit_logs]   
 @v_in_txn_id VARCHAR(255),  
 @v_in_msg_type VARCHAR(255),  
 @v_in_source_site VARCHAR(10),  
 @v_in_target_site VARCHAR(10),  
 @v_in_artifact VARCHAR(255),   
 @v_in_plain_assertion VARCHAR(8000),   
 @v_in_secured_assertion TEXT,  
 @v_in_plain_artifact_resolve_req TEXT,   
 @v_in_secured_artifact_resolve_req TEXT,  
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
    
declare @E_plain_assertion varbinary(8000)  
  
EXEC [proc_SymmetricKey_open]
select @E_plain_assertion = EncryptByKey(KEY_GUID('sym_Key'), @v_in_plain_assertion)  
EXEC [proc_SymmetricKey_close]
  
INSERT INTO sso_audit_logs (  
 txn_id,  
 msg_type,  
 source_site,  
 target_site,  
 artifact,   
 plain_assertion,   
 secured_assertion,  
 plain_artifact_resolve_req,   
 secured_artifact_resolve_req,  
 creation_datetime  
) VALUES (   
 @v_in_txn_id,  
 @v_in_msg_type,  
 @v_in_source_site,  
 @v_in_target_site,  
 @v_in_artifact,   
 @E_plain_assertion,   
 @v_in_secured_assertion,  
 @v_in_plain_artifact_resolve_req,   
 @v_in_secured_artifact_resolve_req,  
 @v_in_creation_datetime  
);  
  
END  
GO

GRANT EXECUTE ON [dbo].[proc_ins_sso_audit_logs] TO HCSSO
GO

