IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ccc_big5_get_byIntenralCCCode]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ccc_big5_get_byIntenralCCCode]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- CR No.			CRE15-014
-- Modified date:	31 Dec 2015
-- Description:		Return [UniCode_Int] instead of [big5]
-- =============================================
-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 07 May 2008
-- Description:	Retrieve the Chinese Char by
--				CCCode
-- =============================================
CREATE PROCEDURE [dbo].[proc_ccc_big5_get_byIntenralCCCode] 
	@ccc            		char(5),
	--@big5           		nvarchar(2)		output,
	@UniCode_Int       		int				output,
	@return_code   			int				output,
	@return_msg     		varchar(255)	output
AS
BEGIN
DECLARE @ccc_head	char(4)
DECLARE @ccc_tail	char(1)
 

SELECT @return_code = 0
IF @ccc IS NULL 
	SELECT @ccc = ''
ELSE
	SELECT @ccc = rtrim(@ccc)

IF @ccc = ''
	SELECT @UniCode_Int = NULL
	
ELSE
IF (@ccc NOT LIKE '[0-9][0-9][0-9][0-9][0-9]')
	SELECT @return_code = 99999, @return_msg = 'Invalid CCCode: '+ @ccc 
	
ELSE
BEGIN
 
	SELECT @ccc_head = substring(@ccc,1,4)
	
	SELECT @ccc_tail = substring(@ccc,5,1)
	
	SELECT @UniCode_Int = UniCode_Int	--@big5 = nchar(UniCode_Int)			
	FROM CCC_BIG5
	WHERE ccc_head = @ccc_head
		  AND ccc_tail = @ccc_tail 

	IF (@@rowcount = 0) OR (@UniCode_Int IS NULL) OR (len(@UniCode_Int) = 0) 
		SELECT @UniCode_Int = NULL

END
END
GO

GRANT EXECUTE ON [dbo].[proc_ccc_big5_get_byIntenralCCCode] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_ccc_big5_get_byIntenralCCCode] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_ccc_big5_get_byIntenralCCCode] TO HCVU
GO
