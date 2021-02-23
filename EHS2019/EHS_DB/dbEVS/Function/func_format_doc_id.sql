
IF EXISTS
         (
             SELECT 1
             FROM sys.objects
             WHERE object_id = OBJECT_ID(N'[dbo].[func_format_doc_id]')
                   AND type IN(N'FN', N'IF', N'TF', N'FS', N'FT')
          )
BEGIN
    DROP FUNCTION [dbo].[func_format_doc_id];
END;
GO

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================  
-- Author:   Martin Tang 
-- Create date: 13 JAN 2021
-- Description: Format Display Document Number  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:     
-- Modified date:     
-- Description:     
-- =============================================  
--select [dbo].[func_format_doc_id]('HKIC',' YD256454Q','')

CREATE FUNCTION [dbo].[func_format_doc_id]
(@doc_code VARCHAR(10), 
 @doc_id1  VARCHAR(20), 
 @doc_id2  VARCHAR(20)
)
RETURNS VARCHAR(20)
AS
     BEGIN  
         -- =============================================  
         -- Declaration  
         -- =============================================  
         DECLARE @temp_trim_id VARCHAR(20);
         DECLARE @result VARCHAR(20);

         -- =============================================  
         -- Validation   
         -- =============================================  
         -- =============================================  
         -- Initialization  
         -- =============================================  
         SET @result = '';

         IF @doc_code = 'ADOPC'
         BEGIN
             SET @result = LTRIM(RTRIM(@doc_id1)) + '/' + LTRIM(RTRIM(@doc_id2));
         END;
         ELSE IF @doc_code = 'Doc/I'
                OR @doc_code = 'ID235B'
                OR @doc_code = 'REPMT'
         BEGIN
			 SET @result = LTRIM(RTRIM(@doc_id1));
         END;
         ELSE IF @doc_code = 'EC'
                OR @doc_code = 'HKBC'
                OR @doc_code = 'HKIC'
         BEGIN
              SET @temp_trim_id = LTRIM(RTRIM(@doc_id1));
              SET @result = LEFT(@temp_trim_id, LEN(@temp_trim_id) - 1) + +'(' + RIGHT(@temp_trim_id, 1) + ')';
         END;
         ELSE IF @doc_code = 'VISA'
         BEGIN
              SET @temp_trim_id = LTRIM(RTRIM(@doc_id1));
			  SET @result = SUBSTRING(@temp_trim_id, 1, 4) + '-' + SUBSTRING(@temp_trim_id, 5, 7) + '-' + SUBSTRING(@temp_trim_id, 12, 2)
                  + '(' + SUBSTRING(@temp_trim_id, 14, 1) + ')';
         END;
         ELSE --other doc type
         BEGIN
              SET @result = LTRIM(RTRIM(@doc_id1));
         END;
         -- =============================================  
         -- Return results  
         -- =============================================  
         RETURN @result;
     END;