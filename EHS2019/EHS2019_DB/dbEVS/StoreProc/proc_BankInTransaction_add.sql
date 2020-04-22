IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_BankInTransaction_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_BankInTransaction_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:			Clark Yip
-- Create date:		15 May 2008
-- Description:		Insert record into BankInTransaction table
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE 	[dbo].[proc_BankInTransaction_insert]	@tran_id	char(20)
							,@reimburse_id		 char(15)							
as

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


	--BankInTransaction Table
	INSERT INTO [dbEVS].[dbo].[BankInTransaction]
           ([Transaction_ID]
		   ,[Reimburse_ID])
     VALUES
           (@tran_id           
           ,@reimburse_id)
GO
