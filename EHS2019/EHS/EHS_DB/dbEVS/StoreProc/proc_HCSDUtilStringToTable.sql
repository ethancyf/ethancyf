IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCSDUtilStringToTable]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCSDUtilStringToTable]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Mattie LO
-- Create date: 30 August 2009
-- Description:	 Convert a semicolon seperated string to a result set
-- =============================================

CREATE PROCEDURE [dbo].[proc_HCSDUtilStringToTable]
   @String varchar(8000)
AS
DECLARE @StartPos as int
DECLARE @Length as int
CREATE TABLE #Converted (value varchar(1500))
SELECT @StartPos = 1 
IF right(@String, 1) <> ';' 
   SELECT @String = @String + ';'
WHILE charindex(';', @String, @StartPos) <> 0
BEGIN
   SELECT @Length = charindex(';', @String, @StartPos) - @StartPos
   INSERT #Converted VALUES (substring(@String, @StartPos, @Length))
   SELECT @StartPos = @StartPos + @Length + 1
END
SELECT	value
FROM	#Converted
GO

GRANT EXECUTE ON [dbo].[proc_HCSDUtilStringToTable] TO HCPUBLIC
GO
