IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[func_formatSchemeList]'))
	DROP FUNCTION [dbo].[func_formatSchemeList]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================
-- =============================================
-- Author:		Timothy LEUNG
-- CR No.:		INT12-0015
-- Create date: 28 Dec 2012
-- Description:	Format the enrolled scheme
-- =============================================
CREATE FUNCTION [dbo].[func_formatSchemeList] 
(
	-- Add the parameters for the function here
	@ERN char(15),
	@STAGING_RECORD_STATUS char(1),
	@SPID char(8),
	@Target varchar(10)
)
RETURNS varchar(100)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @SchemeList varchar(100)
	-- Add the T-SQL statements to compute the return value here
	
	IF @Target = 'STAGING' 
	BEGIN
		IF @STAGING_RECORD_STATUS = 'A'
		BEGIN
			SELECT @schemeList = COALESCE(@schemeList + ', ', '') + RTRIM(m.Display_Code) 
			FROM SchemeInformationStaging s INNER JOIN SchemeBackOffice m ON
				 s.Scheme_Code = m.Scheme_Code
			WHERE  s.Enrolment_Ref_No=@ERN 
				AND m.Effective_Dtm <= GETDATE() AND m.Expiry_Dtm >= GETDATE() 
				AND s.Record_Status = 'A'
			ORDER BY m.Display_Seq
		END 
		ELSE
		BEGIN
			SELECT @schemeList = COALESCE(@schemeList + ', ', '') + RTRIM(m.Display_Code) 
			FROM SchemeInformationStaging s INNER JOIN SchemeBackOffice m ON
				 s.Scheme_Code = m.Scheme_Code
			WHERE  s.Enrolment_Ref_No=@ERN 
				AND m.Effective_Dtm <= GETDATE() AND m.Expiry_Dtm >= GETDATE() 
			ORDER BY m.Display_Seq
		END
	END
	ELSE IF @Target = 'ENROLLED'
	BEGIN
		SELECT @schemeList = COALESCE(@schemeList + ', ', '') + RTRIM(m.Display_Code) 
		FROM SchemeInformation s INNER JOIN SchemeBackOffice m  ON
			s.Scheme_Code = m.Scheme_Code
		WHERE  s.SP_ID=@SPID AND m.Effective_Dtm <= GETDATE() AND m.Expiry_Dtm >= GETDATE() 
		ORDER BY m.Display_Seq
	END
	-- Return the result of the function
	RETURN @SchemeList


END
GO

