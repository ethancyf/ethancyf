IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TransactionAdditionalField_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TransactionAdditionalField_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:		CRE11-024-02
-- Modified by:	Koala CHENG
-- Modified date:	08 Oct 2011
-- Description:	After insert, update VoucherTransaction.Update_by/Update_Dtm
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================
-- Modified by:		Kathy LEE
-- Modified date:	8 Jul 2010
-- Description:		Grant Right to HCVU
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 31 Aug 2009
-- Description:	Insert Transaction Detail
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_TransactionAdditionalField_add]
	@Transaction_ID			char(20),
	@Scheme_Code			char(10),
	@Scheme_Seq				smallint,
	@Subsidize_Code			char(10),
	@AdditionalFieldID		varchar(20),
	@AdditionalFieldValueCode	varchar(50),
	@AdditionalFieldValueDesc	nvarchar(255),
	@update_by				varchar(20)
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

	INSERT INTO [TransactionAdditionalField]
	(
		[Transaction_ID],
		[Scheme_Code],
		[Scheme_Seq],
		[Subsidize_Code],
		[AdditionalFieldID],
		[AdditionalFieldValueCode],
		[AdditionalFieldValueDesc]
	)
	VALUES
	(
		@Transaction_ID,
		@Scheme_Code,
		@Scheme_Seq,
		@Subsidize_Code,
		@AdditionalFieldID,
		@AdditionalFieldValueCode,
		@AdditionalFieldValueDesc
	)

	UPDATE [VoucherTransaction]
	SET Update_By = @update_by,
		Update_Dtm = GETDATE()
	WHERE [Transaction_ID]=@Transaction_ID
END
GO

GRANT EXECUTE ON [dbo].[proc_TransactionAdditionalField_add] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_TransactionAdditionalField_add] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_TransactionAdditionalField_add] TO WSEXT
Go