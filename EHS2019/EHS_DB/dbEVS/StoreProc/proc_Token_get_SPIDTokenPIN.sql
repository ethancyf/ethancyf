IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Token_get_SPIDTokenPIN]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Token_get_SPIDTokenPIN]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

--******************************************************************/
--*                                                                */
--* Server:    HOCMISSV02                                          */
--* Database:  dbEVS                                            	  */
--* Date/Time: 10-Jun-2008                                    	  */
--* Created Date/Time: 10-Jun-2008                           	  */
--* Created by: Clark Yip                                          */
--* Modified by:                                                   */
--* Modified Date/Time:                                            */
--******************************************************************/

CREATE PROCEDURE [dbo].[proc_Token_get_SPIDTokenPIN]
	@spid char(20),
	@token_pin varchar(20)
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
	SELECT count(1)
	From Token
	where [User_id] = @spid and
			[Token_serial_no] = @token_pin
END 
GO

GRANT EXECUTE ON [dbo].[proc_Token_get_SPIDTokenPIN] TO HCVU
GO
