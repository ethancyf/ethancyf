IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_AccountChangeMaintenance_get_byKeyValue]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_AccountChangeMaintenance_get_byKeyValue]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No:			CRE14-002 - PPI-ePR Migration
-- Modified by:		Chris YIM
-- Modified date:	11 Jun 2014
-- Description:		Add Columns - [SPAccountMaintenance].[Project]
--								- [SPAccountMaintenance].[Is_Share_Token]
--								- [SPAccountMaintenance].[Token_Serial_No_Replacement]
--								- [SPAccountMaintenance].[Project_Replacement]
--								- [SPAccountMaintenance].[Is_Share_Token_Replacement]
-- =============================================
-- =============================================
-- Modification History
-- CR No:			INT13-0028 - SP Amendment Report
-- Modified by:		Tommy LAM
-- Modified date:	19 Nov 2013
-- Description:		Add Column - [SPAccountMaintenance].[Data_Input_By]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence Tsang
-- Modified date:	22-06-2009
-- Description:		Add Scheme Code
-- =============================================
-- =============================================
-- Author:			Tommy Cheung
-- Create date:		25-06-2008
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_AccountChangeMaintenance_get_byKeyValue]
	@SPID		char(8),
	@UpdType	varchar(2),
	@SystemDtm	datetime
AS

-- =============================================
-- Declaration
-- =============================================
SELECT	SP_ID,
		Upd_Type,
		System_Dtm,
		ISNULL(Remark,'') AS Remark,
		ISNULL(Token_Serial_No,'') AS Token_Serial_No,
		ISNULL(Token_Remark,'') AS Token_Remark,
		ISNULL(SP_Practice_Display_Seq,0) AS SP_Practice_Display_Seq,
		ISNULL(Delist_Status,'') AS Delist_Status,
		ISNULL(Update_by,'') AS Update_by,
		ISNULL(Confirmed_by,'') AS Confirmed_by,
		Confirm_Dtm,
		ISNULL(Record_Status,'') AS Record_Status,
		Scheme_Code,
		TSMP,
		Data_Input_By,
		ISNULL(Project,'') AS Project,
		ISNULL(Is_Share_Token,'') AS Is_Share_Token,
		ISNULL(Token_Serial_No_Replacement,'') AS Token_Serial_No_Replacement,
		ISNULL(Project_Replacement,'') AS Project_Replacement,
		ISNULL(Is_Share_Token_Replacement,'') AS Is_Share_Token_Replacement
		
FROM	SPAccountMaintenance

WHERE	(@SPID IS NULL OR SP_ID = @SPID) 
			AND (@UpdType IS NULL OR Upd_Type = @UpdType) 
			AND (@SystemDtm IS NULL OR System_Dtm = @SystemDtm) 
			AND Record_Status = 'A'

GO

GRANT EXECUTE ON [dbo].[proc_AccountChangeMaintenance_get_byKeyValue] TO HCVU
GO
