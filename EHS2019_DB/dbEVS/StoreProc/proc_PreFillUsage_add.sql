IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PreFillUsage_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PreFillUsage_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	25 August 2016
-- CR No.:			CRE16-002
-- Description:		Stored procedure is not used anymore
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	10 Dec 2009
-- Description:		Add use Date
-- =============================================
-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 09 Sep 2009
-- Description:	Add PreFillUsage after the PreFilled Consent number is used
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	12 Oct 2009
-- Description:		Handle Concurrent Use of PreFill
-- =============================================

/*
CREATE PROCEDURE [proc_PreFillUsage_add]
	@pre_fill_consent_id char(15), @voucher_acc_id char(15)
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================

	IF (SELECT Count(*) FROM [PreFillUsage] WHERE [Pre_Fill_Consent_ID] = @pre_fill_consent_id) > 0
	BEGIN
		Raiserror('00011', 16, 1)
		RETURN @@error
	END	

-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================
	--delete from PreFillPersonalInformation
	--where Pre_Fill_Consent_ID = @pre_fill_consent_id

	insert into PreFillUsage
	(Pre_Fill_Consent_ID, Voucher_Acc_ID, Used_Date)
	values
	(@pre_fill_consent_id, @voucher_acc_id, GetDate())
		
END
GO

GRANT EXECUTE ON [dbo].[proc_PreFillUsage_add] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_PreFillUsage_add] TO WSEXT
GO

*/
