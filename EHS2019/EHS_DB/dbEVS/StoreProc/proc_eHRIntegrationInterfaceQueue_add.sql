IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_eHRIntegrationInterfaceQueue_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_eHRIntegrationInterfaceQueue_add]
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
-- Author:			Lawrence TSANG
-- Create date:		19 October 2016
-- CR No.:			CRE16-019 (To implement token sharing between eHS(S) and eHRSS)
-- Description:		Add eHRIntegrationInterfaceQueue
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.:			
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_eHRIntegrationInterfaceQueue_add]
	@Queue_ID						varchar(14),
	@Queue_Type						varchar(20),
	@Queue_Content					nvarchar(1000)
AS BEGIN

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
-- Process
-- =============================================

	EXEC [proc_SymmetricKey_open]

	INSERT INTO eHRIntegrationInterfaceQueue (
		Queue_ID,
		Queue_Type,
		Queue_Content,
		Record_Status,
		Create_Dtm
	) VALUES (
		@Queue_ID,
		@Queue_Type,
		EncryptByKey(KEY_GUID('sym_Key'), @Queue_Content),
		'P',
		GETDATE()
	)

	EXEC [proc_SymmetricKey_close]


END
GO

GRANT EXECUTE ON [dbo].[proc_eHRIntegrationInterfaceQueue_add] TO HCVU
GO
