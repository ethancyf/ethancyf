
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[proc_DataEntryUserAC_get_BySPIDDEID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [proc_DataEntryUserAC_get_BySPIDDEID]
GO

-- =============================================
-- Modification History
-- Modified by:		Dickson Law
-- Modified date:	22 Jun 2017
-- CR No.			I-CRE16-007-02 (Refine system from CheckMarx findings)
-- Description:		Add [Data_Entry_Password_Level] field
-- =============================================

-- =============================================
-- Modification History
-- Modified by:	Stanley CHAN
-- Modified date: 	6 Feb 2009
-- Description:	Add Print Option
-- =============================================

-- =============================================
-- Author:		Timothy LEUNG
-- Create date:		23 Jun 2008
-- Description:	Retrieve all Data Entry Account by SPID and DataEntryAccount
-- =============================================

CREATE PROCEDURE [dbo].[proc_DataEntryUserAC_get_BySPIDDEID]
	@SP_ID char(8),
	@Data_Entry_Account varchar(20)
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

    Select DE.SP_ID, DE.Data_Entry_Account, DE.ConsentPrintOption,
			DE.Record_Status, isNull(DEPM.SP_Practice_Display_Seq, 0) SP_Practice_Display_Seq, isNull(DEPM.SP_Bank_Acc_Display_Seq,0) SP_Bank_Acc_Display_Seq, DE.Data_Entry_Password,
			P.Record_Status as Practice_Status, DE.Account_Locked,DE.Data_Entry_Password_Level Password_Level
	from DataEntryUserAC DE left outer join DataEntryPracticeMapping DEPM on
	DE.SP_ID = DEPM.SP_ID and 
	DE.Data_Entry_Account = DEPM.Data_Entry_Account inner join Practice P on 
	P.SP_ID = DE.SP_ID and 
	P.Display_Seq = DEPM.SP_Practice_Display_Seq
	where 
	DE.SP_ID = @SP_ID 
	and	DE.Data_Entry_Account = @Data_Entry_Account 
	--and P.Record_Status = 'A'
	order by SP_Practice_Display_Seq, SP_Bank_Acc_Display_Seq

END
GO

GRANT EXECUTE ON [proc_DataEntryUserAC_get_BySPIDDEID] TO HCSP
GO