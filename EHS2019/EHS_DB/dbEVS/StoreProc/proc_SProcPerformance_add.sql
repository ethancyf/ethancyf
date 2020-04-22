IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SProcPerformance_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SProcPerformance_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		21 January 2015
-- CR No.:			CRE14-019
-- Description:		Insert into [SProcPerformance] to record sproc performance
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.:			
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_SProcPerformance_add]
	@SProc_Name		varchar(255),
	@Parameter		varchar(255),
	@Start_Dtm		datetime,
	@End_Dtm		datetime
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

	INSERT INTO SProcPerformance (
		System_Dtm,
		SProc_Name,
		Parameter,
		Start_Dtm,
		End_Dtm		
	) VALUES (
		GETDATE(),
		@SProc_Name,
		EncryptByKey(KEY_GUID('sym_Key'), @Parameter),
		@Start_Dtm,
		@End_Dtm
	)

	CLOSE SYMMETRIC KEY sym_Key


END
GO

