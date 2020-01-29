IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TokenDeactivate_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TokenDeactivate_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		2 February 2017
-- CR No.:			CRE16-019 (To implement token sharing between eHS(S) and eHRSS)
-- Description:		Grant EXECUTE to WSEXT
-- =============================================
-- =============================================
-- Modification History
-- CR No:			CRE14-002 - PPI-ePR Migration
-- Modified by:		Tommy LAM
-- Modified date:	13 Mar 2014
-- Description:		Add Column -	[TokenDeactivated].[Project]
--									[TokenDeactivated].[Is_Share_Token]
-- =============================================
-- =============================================  
-- Modification History      
-- CR No.:		   CRE13-003  
-- Modified by:    Karl LAM   
-- Modified date:  17 Jun 2013  
-- Description:   Add grant execute to hscp user
-- =============================================    
--* Created Date/Time: 04-Jul-2008                           	   */
--* Created by: Tommy Cheung                                       */
--* Modified by:                                                   */
--* Modified Date/Time:                                            */
--* Purpose: Add Record To TokenDeactivate

CREATE PROCEDURE [dbo].[proc_TokenDeactivate_add]
	@User_ID char(20),
	@Token_Serial_No varchar(20),
	@Deactivate_By varchar(20),
	@Remark char(5),
	@Project char(10),
	@Is_Share_Token char(1)
AS
BEGIN
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
INSERT INTO TokenDeactivated (
		User_ID, 
		Token_Serial_No,
		Deactivate_By,
		Remark,
		Deactivate_Dtm,
		Project,
		Is_Share_Token)
VALUES (@User_ID,
		@Token_Serial_No,
		@Deactivate_By,
		@Remark,
		getdate(),
		@Project,
		@Is_Share_Token)
END
GO

GRANT EXECUTE ON [dbo].[proc_TokenDeactivate_add] TO HCVU, HCSP, WSEXT
GO
