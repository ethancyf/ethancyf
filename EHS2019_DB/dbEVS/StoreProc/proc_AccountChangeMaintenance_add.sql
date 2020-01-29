IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_AccountChangeMaintenance_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_AccountChangeMaintenance_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No:			CRE14-002 - PPI-ePR Migration
-- Modified by:		Chris YIM
-- Modified date:	11 Jun 2014
-- Description:		Add Column -	[SPAccountMaintenance].[Project]
--									[SPAccountMaintenance].[Is_Share_Token]
--									[SPAccountMaintenance].[Token_Serial_No_Replacement]
--									[SPAccountMaintenance].[Project_Replacement]
--									[SPAccountMaintenance].[Is_Share_Token_Replacement]
-- =============================================
-- =============================================
-- Modification History
-- CR No:			INT13-0028 - SP Amendment Report
-- Modified by:		Tommy LAM
-- Modified date:	19 Nov 2013
-- Description:		Add Column -	[SPAccountMaintenance].[Data_Input_By]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	  Lawrence TSANG
-- Modified date: 15-May-2009
-- Description:	  Add field "Scheme_Code"
-- =============================================
-- =============================================
-- Author:			Tommy Cheung
-- Create date:		26-Jun-2008
-- Description:		Add Record into SPAccountMaintenance Table
-- =============================================

CREATE PROCEDURE [dbo].[proc_AccountChangeMaintenance_add]
@SP_ID char(8),
@Upd_Type varchar(2),
@Remark nvarchar(255),
@Token_Serial_No varchar(20),
@Token_Remark char(5),
@SP_Practice_Display_Seq smallint,
@Delist_Status char(1),
@Update_By varchar(20),
@Record_Status char(1),
@Scheme_Code char(10),
@Data_Input_By varchar(20),
@Project char(10),
@Is_Share_Token char(1),
@Token_Serial_No_Replacement varchar(20),
@Project_Replacement char(10),
@Is_Share_Token_Replacement char(1)
AS
BEGIN
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================

declare @current_dtm datetime
declare @rowcount int

SELECT	@rowcount = COUNT(1)       
FROM	SPAccountMaintenance
WHERE	SP_ID = @SP_ID 
		AND Upd_Type = @Upd_Type
		AND (@SP_Practice_Display_Seq IS NULL 
			OR SP_Practice_Display_Seq = @SP_Practice_Display_Seq) 
		AND Record_Status = @Record_Status
		AND Scheme_Code = @Scheme_Code
		
	IF @rowcount > 0
	BEGIN
		RAISERROR('00011', 16, 1)
		return @@ERROR
	END
	
declare @NewSeqNo int
	
select @NewSeqNo = max(Seq_No) + 1
from SPaccountmaintenance

if @NewSeqNo is null 
begin
	select @NewSeqNo = 1
end 

-- =============================================
-- Initialization
-- =============================================

set @current_dtm = getdate()

-- =============================================
-- Return results
-- =============================================
INSERT INTO SPAccountMaintenance (
		SP_ID, 
		Upd_Type,
		System_Dtm,
		Remark,
		Token_Serial_No,
		Token_Remark,
		SP_Practice_Display_Seq,
		Delist_Status,
		Update_By,
		Record_Status,
		Seq_No,
		Scheme_Code,
		Data_Input_By,
		Project,
		Is_Share_Token,
		Token_Serial_No_Replacement,
		Project_Replacement,
		Is_Share_Token_Replacement
		)
VALUES (@SP_ID,
		@Upd_Type,
		@current_dtm,
		@Remark,		
		@Token_Serial_No,
		@Token_Remark,
		@SP_Practice_Display_Seq,
		@Delist_Status,
		@Update_By,		
		@Record_Status,
		@NewSeqNo,
		@Scheme_Code,
		@Data_Input_By,
		@Project,
		@Is_Share_Token,
		@Token_Serial_No_Replacement,
		@Project_Replacement,
		@Is_Share_Token_Replacement)
END
GO

GRANT EXECUTE ON [dbo].[proc_AccountChangeMaintenance_add] TO HCVU
GO
