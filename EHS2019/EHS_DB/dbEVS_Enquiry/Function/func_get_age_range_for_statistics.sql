IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[func_get_age_range_for_statistics]') AND type IN (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[func_get_age_range_for_statistics]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Koala Cheng
-- Create date:		03 Dec 2012
-- Description:		Get the age range for statistics
-- =============================================

CREATE FUNCTION [dbo].[func_get_age_range_for_statistics] (
		@age_from			int,
		@age_to				int,
		@separator_month	int
	)
RETURNS @TempTable TABLE (Age varchar(15), Years INT, Seq INT)  
AS  
BEGIN  
  
-- =============================================  
-- Declaration  
-- =============================================  
  
 DECLARE @month_index int  
 DECLARE @unit   varchar(8)  
  
-- =============================================  
-- Validation  
-- =============================================  
  
 IF @age_from IS NULL OR @age_to IS NULL  
  RETURN  
  
 IF @age_from < 0 OR @age_to < 0  
  RETURN  
  
 IF @age_from > @age_to  
  RETURN  
  
 IF @separator_month IS NULL  
  SET @separator_month = 0  
  
 IF @separator_month < 0  
  RETURN  
  
-- =============================================  
-- Initialization  
-- =============================================  
-- =============================================  
-- Return results  
-- =============================================  
  
 IF @age_from = 0  
  BEGIN  
   SET @unit = ' month'  
  
   IF @separator_month = 0  
    BEGIN  
     INSERT INTO @TempTable (Age, Years, Seq) VALUES ('0' + @unit, 0, 0)  
     INSERT INTO @TempTable (Age, Years, Seq) VALUES ('1' + @unit, 0, 1)  
     SET @month_index = 2  
     SET @unit = @unit + 's'  
    END  
   ELSE  
    BEGIN  
     IF @separator_month > 1  
      SET @unit = @unit + 's'  
  
     INSERT INTO @TempTable (Age, Years, Seq) VALUES ('0 to ' + CONVERT(varchar(2), @separator_month) + @unit, 0, 0)  
     SET @month_index = @separator_month + 1  
     IF @separator_month = 1  
      SET @unit = @unit + 's'  
    END  
  
   WHILE (@month_index < 12)  
    BEGIN  
     INSERT INTO @TempTable (Age, Years, Seq) VALUES (CONVERT(varchar(2), @month_index) + @unit, 0, @month_Index)  
     SET @month_index = @month_index + 1  
    END  
  
   SET @age_from = 1  
  END  
  
 IF @age_to = 0  
  RETURN  
  
 SET @unit = ' year'  
 IF @age_from = 1  
  BEGIN  
   INSERT INTO @TempTable (Age, Years, Seq) VALUES (CONVERT(varchar(3), @age_from) + @unit, 1, 1 * 100)  
   SET @age_from = 2  
  END  
  
 SET @unit = @unit + 's'  
 WHILE (@age_from <= @age_to)  
  BEGIN  
   INSERT INTO @TempTable (Age, Years, Seq) VALUES (CONVERT(varchar(3), @age_from) + @unit, @age_from, @age_from * 100)  
   SET @age_from = @age_from + 1  
  END  
  
 RETURN  
  
END  
GO

GRANT SELECT ON [dbo].[func_get_age_range_for_statistics] TO HCVU
GO
